using GuardTourSystem.Database.BLL;
using GuardTourSystem.Services;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.ViewModel.Popup
{
    class SystemInitViewModel : AbstractPopupNotificationViewModel
    {
        //删除逻辑如下
        //1.如果删除地点: !必须! 附带删除班次和计划
        //2.如果删除班次: !必须! 附带删除计划

        private bool initPlace;
        public bool InitPlace
        {
            get { return initPlace; }
            set
            {
                if (value)
                {
                    InitFrequence = true;
                    InitPlan = true;
                }
                this.CConfirm.RaiseCanExecuteChanged();
                SetProperty(ref this.initPlace, value);
            }
        }
        private bool initFrequence;
        public bool InitFrequence
        {
            get { return initFrequence; }
            set
            {
                if (!value && InitPlace)
                {
                    return;
                }
                if (value)
                {
                    InitPlan = true;
                }
                this.CConfirm.RaiseCanExecuteChanged();
                SetProperty(ref this.initFrequence, value);
            }
        }

        private bool initPlan;
        public bool InitPlan
        {
            get { return initPlan; }
            set
            {
                if (!value && (InitPlace || InitFrequence))
                {
                    return;
                }
                this.CConfirm.RaiseCanExecuteChanged();
                SetProperty(ref this.initPlan, value);
            }
        }

        private bool initEvent;

        public bool InitEvent
        {
            get { return initEvent; }
            set
            {
                this.CConfirm.RaiseCanExecuteChanged();
                SetProperty(ref this.initEvent, value);
            }
        }
        private bool initWorker;

        public bool InitWorker
        {
            get { return initWorker; }
            set
            {
                this.CConfirm.RaiseCanExecuteChanged();
                SetProperty(ref this.initWorker, value);
            }
        }

        private bool initData;

        public bool InitData
        {
            get { return initData; }
            set
            {
                this.CConfirm.RaiseCanExecuteChanged();
                SetProperty(ref this.initData, value);
            }
        }



        public SystemInitViewModel()
        {
            Title = "系统初始化";
            this.ConfirmButtonText = "开始";
            this.CConfirm = new DelegateCommand(ReInit,
                () => { return InitPlan || InitPlace || InitFrequence || InitWorker || InitEvent || InitData; });
        }

        public async void ReInit()
        {
            this.Finish();
            //弹出确认框,让用户点击确定
            var confirmMessage = new StringBuilder("将初始化以下数据:\n");
            int index = 1;
            if (InitPlace)
            {
                confirmMessage.Append(index + ". 地点信息\n");
                index++;
            }
            if (InitFrequence)
            {
                confirmMessage.Append(index + ". 班次信息\n");
                index++;
            }
            if (InitPlan)
            {
                confirmMessage.Append(index + ". 巡检计划\n");
                index++;
            }
            if (InitWorker)
            {
                confirmMessage.Append(index + ". 巡检员信息\n");
                index++;
            }
            if (InitEvent)
            {
                confirmMessage.Append(index + ". 事件信息\n");
                index++;
            }
            if (InitData)
            {
                confirmMessage.Append(index + ". 巡检记录\n");
                index++;
            }
            confirmMessage.Append("该操作无法撤销!");

            var result = await this.ShowConfirmDialog("确定要进行系统初始化吗?", confirmMessage.ToString());
            if (result == MessageDialogResult.Negative) //用户取消
            {
                return;
            }
            else
            {
                if (InitPlace)
                {
                    new RouteBLL().Init();
                    new PlaceBLL().Init();
                }
                if (InitFrequence)
                {
                    new FrequenceBLL().Init();
                }
                if (InitPlan)
                {
                    new DutyBLL().Init();
                }
                if (InitWorker)
                {
                    new WorkerBLL().Init();
                }
                if (InitEvent)
                {
                    new EventBLL().Init();
                }
                if (InitData)
                {
                    new RawDataBLL().Init();
                }
                AppStatusViewModel.Instance.ShowInfo("系统初始化成功", 5);

                //重新生成当天考核表
                string error = null;
                new DutyBLL().GenerateDuty(out error, null, DateTime.Now);
            }
        }
    }
}
