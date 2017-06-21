using GuardTourSystem.Database.BLL;
using GuardTourSystem.Utils;
using GuardTourSystem.Model;
using GuardTourSystem.Print;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GuardTourSystem.ViewModel
{
    //班次设置的 ViewModel
    class FrequenceViewModel : BaseFrequenceViewModel
    {
        protected override string ExportTitle
        {
            get
            {
                return "班次信息\t 导出时间: " + DateTime.Now.ToString();
            }
        }
        private ObservableCollection<Frequence> frequences;
        public ObservableCollection<Frequence> Frequences
        {
            get { return frequences; }
            set
            {
                SetProperty(ref this.frequences, value);
            }
        }

        private Frequence frequence;
        public Frequence Frequence
        {
            get { return frequence; }
            set
            {
                this.CUpdateFrequence.RaiseCanExecuteChanged();
                this.CDeleteFrequence.RaiseCanExecuteChanged();
                SetProperty(ref this.frequence, value);
            }
        }

        public List<Route> Routes { get; set; }

        //private string tooltip;

        //public string ToolTipInfo
        //{
        //    get { return tooltip; }
        //    set
        //    {
        //        SetProperty(ref this.tooltip, value);
        //    }
        //}

        public DelegateCommand CAddFrequence { get; set; }
        public DelegateCommand CUpdateFrequence { get; set; }
        public DelegateCommand CDeleteFrequence { get; set; }

        //public DelegateCommand CSetAlarm { get; set; }

        public DelegateCommand<DataGrid> CPrint { get; set; }

        public InteractionRequest<INotification> FrequenceInfoPopupRequest { get; set; }
        public InteractionRequest<INotification> SetAlarmPopupRequest { get; set; }

        public FrequenceViewModel()
        {
            InitData();

            this.CAddFrequence = new DelegateCommand(this.AddFrequence, () => { return Routes.Count != 0; });
            this.CUpdateFrequence = new DelegateCommand(this.UpdateFrequence, () => { return Frequence != null; });
            this.CDeleteFrequence = new DelegateCommand(this.DeleteFrequence, () => { return Frequence != null; });
            //this.CSetAlarm = new DelegateCommand(this.SetAlarm);

            this.CPrint = new DelegateCommand<DataGrid>(this.Print, (grid) => { return Frequences != null; });


            this.FrequenceInfoPopupRequest = new InteractionRequest<INotification>();
            this.SetAlarmPopupRequest = new InteractionRequest<INotification>();

        }

        //private void SetAlarm()
        //{
        //    this.SetAlarmPopupRequest.Raise(new AlarmSettingViewModel(this.Frequences.ToList()));
        //}

        private void Print(DataGrid dataGrid)
        {
            //var printData = new PrintData() { Title = "考核结果", DataCount = Frequences.Count, DataTable = dataGrid.GetDataTable() };
            //var printer = new Printer(new FrequenceDocument(printData));
            //printer.ShowPreviewWindow();
        }

        private void InitData()
        {
            this.Routes = RouteService.GetAllRoute();
            Frequences = new ObservableCollection<Frequence>(GetFrequenceData());
        }


        public void AddFrequence()
        {
            var InfoVM = new FrequenceInfoPopupViewModel(Routes, null);
            this.FrequenceInfoPopupRequest.Raise(InfoVM,
                notification =>
                {
                    var viewmodel = notification as FrequenceInfoPopupViewModel;
                    if (!viewmodel.IsCancel)
                    {
                        Frequences.Add(viewmodel.Frequence);
                        PatrolSettingViewModel.Instance.PublishDataChange(ChangeEvent.FrequenceChange);
                    }
                });
            //this.CSetAlarm.RaiseCanExecuteChanged();

        }
        public void UpdateFrequence()
        {
            var InfoVM = new FrequenceInfoPopupViewModel(Routes, Frequence.Clone() as Frequence);
            this.FrequenceInfoPopupRequest.Raise(InfoVM,
                notification =>
                {
                    var viewmodel = notification as FrequenceInfoPopupViewModel;
                    if (!viewmodel.IsCancel)
                    {
                        var newFrequence = viewmodel.Frequence;
                        var oldFrequenceIndex = Frequences.ToList().FindIndex(f => { return f.ID == newFrequence.ID; });
                        Frequences[oldFrequenceIndex] = newFrequence;
                        Frequence = newFrequence;
                        PatrolSettingViewModel.Instance.PublishDataChange(ChangeEvent.FrequenceChange);
                    }
                });
        }
        public async void DeleteFrequence()
        {
            // ShowConfirmDialogAction() 返回一个 Task<MessageDialogResult> 类型
            var result = ShowConfirmDialog("确定要删除吗?", "删除后数据不可恢复,并且会删除计划和排班表中的相应数据");

            // 使用 await关键字等待用户操作... 
            // 当用户点击后 , result将返回 用户的点击结果
            if (await result == MessageDialogResult.Negative) //用户取消
            {
                return;
            }
            else
            {
                if (FrequenceService.DeleteFrequence(Frequence)) // 删除成功
                {
                    this.Frequences.Remove(Frequence);
                    Frequence = null;
                    PatrolSettingViewModel.Instance.PublishDataChange(ChangeEvent.FrequenceChange);
                }
                else
                {
                    ShowMessageDialog("班次删除失败!", null);
                };
            }
            //this.CSetAlarm.RaiseCanExecuteChanged();
        }
        public override void NotifyChange(ChangeEvent ce)
        {
            // 只关心 线路数量的变化, 选择是否 显示 [新增班次]
            if (ce == ChangeEvent.RoutesChange)
            {
                InitData();
                this.CAddFrequence.RaiseCanExecuteChanged();
            }
        }

    }

    public class FrequenceItemViewModel : BindableBase
    {
        private ObservableCollection<Frequence> frequences;

        public ObservableCollection<Frequence> Frequences
        {
            get { return frequences; }
            set
            {
                SetProperty(ref this.frequences, value);
            }
        }
    }
}
