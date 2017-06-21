using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Print;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using GuardTourSystem.Utils;
using GuardTourSystem.View;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GuardTourSystem.ViewModel
{
    class EventViewModel : ShowContentViewModel
    {
        protected override string ExportTitle
        {
            get
            {
                return "事件信息\t 导出时间: " + DateTime.Now.ToString();
            }
        }

        private ObservableCollection<Event> events;
        public ObservableCollection<Event> Events
        {
            get { return events; }
            set
            {
                SetProperty(ref this.events, value);
            }
        } //当前所有的事件

        private Event curEvent;
        public Event Event //选中的Event
        {
            get { return curEvent; }
            set
            {
                this.CUpdateEvent.RaiseCanExecuteChanged();
                this.CDelEvent.RaiseCanExecuteChanged();
                SetProperty(ref this.curEvent, value);
            }
        }

        public IEventService DataService { get; set; }

        public DelegateCommand CAddEvent { get; set; }
        public DelegateCommand CDelEvent { get; set; }
        public DelegateCommand CUpdateEvent { get; set; }

        public DelegateCommand CPrint { get; set; }

        public InteractionRequest<INotification> EventInfoPopupRequest { get; private set; }

        public EventViewModel()
        {
            PatrolSettingViewModel.Instance.EventViewModel = this;

            InitBatchAdd();

            DataService = new EventBLL();
            this.Events = new ObservableCollection<Event>(DataService.GetAllEvent());

            this.CAddEvent = new DelegateCommand(this.AddEvent);
            this.CUpdateEvent = new DelegateCommand(this.UpdateEvent, () => { return Event != null; });
            this.CDelEvent = new DelegateCommand(this.DelEvent, () => { return Event != null; });

            this.CPrint = new DelegateCommand(this.Print);

            this.EventInfoPopupRequest = new InteractionRequest<INotification>();
        }

        public void AddEvent()
        {
            var infoVM = new EventInfoViewModel(null);
            this.EventInfoPopupRequest.Raise(infoVM,
                notification =>
                {
                    var newEvent = (notification as EventInfoViewModel).Event;
                    if (newEvent.ID != -1)
                    {
                        Events.Add(newEvent);
                    }
                });
        }

        public void UpdateEvent()
        {
            var infoVM = new EventInfoViewModel(Event.Clone() as Event);
            this.EventInfoPopupRequest.Raise(infoVM,
                notification =>
                {
                    var viewmodel = notification as EventInfoViewModel;
                    if (viewmodel.IsCancel)
                    { //用户取消了操作
                        return;
                    }
                    var newEvent = viewmodel.Event;
                    var updatedEventIndex = Events.ToList().FindIndex(e => { return e.ID == newEvent.ID; });
                    Events[updatedEventIndex] = newEvent; //不知为何 必须使用 Events[i]的形式来更新
                    Event = newEvent; //重新选中该事件
                });
        }

        public async void DelEvent()
        {
            // ShowConfirmDialogAction() 返回一个 Task<MessageDialogResult> 类型
            var result = ShowConfirmDialog("确定要删除吗?", "删除后数据不可恢复");

            // 使用 await关键字等待用户操作... 
            // 当用户点击后 , result将返回 用户的点击结果
            if (await result == MessageDialogResult.Negative) //用户取消
            {
                return;
            }
            else
            {
                if (DataService.DelEvent(Event)) // 删除成功 (数据库中 Deleted设置为 1)
                {
                    this.Events.Remove(Event);
                    Event = null;
                }
                else
                {
                    ShowMessageDialog("删除事件失败!", null);
                };
            }
        }

        private void Print()
        {
            var printData = new PrintData() { ContentList = Events.ToList(), Title = "计数事件信息", DataCount = Events.Count };
            var printer = new Printer(new EventDocument(printData));
            printer.ShowPreviewWindow();
        }

        #region 批量添加事件
        public BatchAddViewModel BatchAddVM { get; set; }
        private void InitBatchAdd()
        {
            this.BatchAddVM = new BatchAddViewModel(LanLoader.Load(LanKey.BatchAddReadEvent), OnBatchAdd, OnGetRecords);
        }
        public bool OnBatchAdd(List<AddItem> selectItems)
        {
            bool allCanAdd = true;
            List<Event> events = new List<Event>();
            //验证每个Item能否添加
            foreach (var item in selectItems)
            {
                string error = "";
                var newEvent = new Event() { Card = item.Card, Name = item.Name };
                events.Add(newEvent);
                if (!DataService.CanAdd(newEvent, out error)) //有任意一个 不能添加
                {
                    item.Error = error;
                    allCanAdd = false;//为了一次性验证完成,不使用return
                }
            }
            if (!allCanAdd)
            {
                return false;
            }
            //验证通过, 添加每个选中的Item
            int id;
            string errorInfo;
            events.ForEach((e) => { DataService.AddEvent(e, out id, out errorInfo); });
            //添加完成,刷新界面的Event
            this.Events = new ObservableCollection<Event>(DataService.GetAllEvent());

            return true;
        }
        public void OnGetRecords(List<AddItem> items)
        {

        }
        #endregion
    }
}
