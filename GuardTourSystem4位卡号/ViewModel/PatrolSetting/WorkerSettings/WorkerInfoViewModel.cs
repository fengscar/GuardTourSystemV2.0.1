using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Model.DAL;
using GuardTourSystem.Model.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Utils;
using KaiheSerialPortLibrary;
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
    class WorkerInfoViewModel : AbstractInfoNotificationViewModel
    {
        #region 依赖属性
        private int selectIndex;

        public int CardTypeSelectIndex
        {
            get { return selectIndex; }
            set
            {
                SetProperty(ref this.selectIndex, value);
            }
        }


        private Worker worker;
        public Worker Worker
        {
            get { return worker; }
            set
            {
                SetProperty(ref this.worker, value);
            }
        }
        public Worker OldWorker { get; set; }

        #endregion

        public IWorkerService DataService { get; set; }

        public DelegateCommand CReadDeviceID { get; set; }

        public WorkerInfoViewModel(Worker worker)
            : base()
        {
            this.DataService = new WorkerBLL();

            Worker = worker;
            //传NULL,表示 添加人员
            if (worker == null)
            {
                Worker = new Worker() { ID = -1 };
                Title = "新增巡检员";
                ConfirmButtonText = LanLoader.Load(LanKey.Add);
                this.CConfirm = new DelegateCommand(new Action(this.AddInfo));
                this.CardTypeSelectIndex = 0;
            }
            // 更新人员
            else
            {
                OldWorker = worker;
                Title = "编辑巡检员";
                ConfirmButtonText = LanLoader.Load(LanKey.Edit);
                this.CConfirm = new DelegateCommand(new Action(this.UpdateInfo));
            }
            this.CReadDeviceID = new DelegateCommand(this.ReadDeviceID);
        }

        private async void ReadDeviceID()
        {
            var deviceFlow = await SerialPortUtil.Write(new GetDeviceID());
            if (deviceFlow.Check(5))
            {
                this.Worker.Card = ((GetDeviceID)deviceFlow).DeviceID;
            }
            else
            {
                ErrorInfo = "读取机号失败:" + deviceFlow.ResultInfo;
            }
        }


        public override void AddInfo()
        {
            //操作数据库
            string error;
            int workerID;
            if (DataService.AddWorker(Worker, out workerID, out error))
            {
                Worker.ID = workerID;
                this.Finish();
            }
            else
            {
                this.ErrorInfo = error;
            }
        }

        public override void UpdateInfo()
        {
            //操作数据库
            string error;
            if (DataService.UpdateWorker(Worker, out error))
            {
                //更新T_Duty的人员信息
                var freqService = new FrequenceBLL();
                var dutyService = new DutyBLL();
                var freqs = freqService.GetAllFrequence();
                foreach (var freq in freqs)
                {
                    //如果该班次的巡检员 == 更新的巡检员
                    if (freq.Worker != null && freq.Worker.Card.Equals(OldWorker.Card))
                    {
                        //更新 该班次值班表
                        dutyService.GenerateDuty(out error, freq, DateTime.Now);
                    }
                }

                this.Finish();
            }
            else
            {
                this.ErrorInfo = error;
                return;
            }
        }
    }
}
