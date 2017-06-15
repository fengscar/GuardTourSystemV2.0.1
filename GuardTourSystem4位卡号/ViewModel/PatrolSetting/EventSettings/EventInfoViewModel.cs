using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GuardTourSystem.ViewModel
{
    class EventInfoViewModel : AbstractInfoNotificationViewModel
    {

        private Event mEvent;

        public Event Event
        {
            get { return mEvent; }
            set
            {
                SetProperty(ref this.mEvent, value);
            }
        }

        public IEventService DataService { get; set; }

        public EventInfoViewModel(Event e)
            : base()
        {
            this.DataService = new EventBLL();

            Event = e;
            //传NULL,表示 添加人员
            if (Event == null)
            {
                Title = "新增事件";
                Event = new Event() { ID = -1 };
                ConfirmButtonText = LanLoader.Load(LanKey.Add);
                this.CConfirm = new DelegateCommand(new Action(this.AddInfo));
            }
            // 更新人员
            else
            {
                Title = "编辑事件信息";
                ConfirmButtonText = LanLoader.Load(LanKey.Edit);
                this.CConfirm = new DelegateCommand(new Action(this.UpdateInfo));
            }
        }

        public override void AddInfo()
        {
            //操作数据库
            int eventID;
            string errorInfo;
            if (DataService.AddEvent(Event, out eventID,out errorInfo))
            {
                Event.ID = eventID;
                this.Finish(); 
            }
            else
            {
                this.ErrorInfo = errorInfo;
            }
        }
        public override void UpdateInfo()
        {
            //操作数据库
            string errorInfo;
            if (DataService.UpdateEvent(Event, out errorInfo))
            {
                this.Finish(); 
            }
            else
            {
                this.ErrorInfo = errorInfo;
            }
        }
    }
}
