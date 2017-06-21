using GuardTourSystem.Model;
using GuardTourSystem.Model.DAL;
using GuardTourSystem.Print;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using GuardTourSystem.Utils;
using GuardTourSystem.View;
using KaiheSerialPortLibrary;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace GuardTourSystem.ViewModel
{
    class WorkerViewModel : ShowContentViewModel, ISerialPortStateChangedListener
    {
        protected override string ExportTitle
        {
            get
            {
                return "管理卡信息\t 导出时间: " + DateTime.Now.ToString();
            }
        }

        private ObservableCollection<Worker> workers;
        public ObservableCollection<Worker> Workers //当前的所有计数员
        {
            get { return workers; }
            set
            {
                SetProperty(ref this.workers, value);
            }
        }

        private Worker worker;
        public Worker Worker  //当前选中的Worker
        {
            get { return worker; }
            set
            {
                this.CDelWorker.RaiseCanExecuteChanged();
                this.CUpdateWorker.RaiseCanExecuteChanged();
                SetProperty(ref this.worker, value);
            }
        }

        private IWorkerService DataService;


        public DelegateCommand CAddWorker { get; set; }
        public DelegateCommand CDelWorker { get; set; }
        public DelegateCommand CUpdateWorker { get; set; }
        //public DelegateCommand CSendWorkers { get; set; }
        public DelegateCommand CPrint { get; set; }

        public InteractionRequest<INotification> WorkerInfoPopupRequest { get; private set; }

        public WorkerViewModel()
        {
            SerialPortManager.Instance.AddListener(this);
            PatrolSettingViewModel.Instance.WorkerViewModel = this;

            InitBatchAdd();

            DataService = new WorkerBLL();
            this.Workers = new ObservableCollection<Worker>(DataService.GetAllWorker());

            this.CAddWorker = new DelegateCommand(this.AddWorker);
            this.CDelWorker = new DelegateCommand(this.DelWorker, () => { return Worker != null; });
            this.CUpdateWorker = new DelegateCommand(this.UpdateWorker, () => { return Worker != null; });
            //this.CSendWorkers = new DelegateCommand(this.SendWorkers, () => { return Workers.Count != 0 && !SerialPortManager.Instance.IsWritting; });
            this.CPrint = new DelegateCommand(this.Print);

            this.WorkerInfoPopupRequest = new InteractionRequest<INotification>();
        }

        public void AddWorker()
        {
            var infoVM = new WorkerInfoViewModel(null);
            this.WorkerInfoPopupRequest.Raise(infoVM,
                notification =>
                {
                    var workerViewModel = notification as WorkerInfoViewModel;
                    if (!workerViewModel.IsCancel && workerViewModel.Worker.ID != -1)
                    {
                        Workers.Add(workerViewModel.Worker);
                    }
                    //PatrolSettingViewModel.Instance.PublishDataChange(ChangeEvent.WorkersChange);
                });
        }

        public void UpdateWorker()
        {
            var infoVM = new WorkerInfoViewModel(Worker.Clone() as Worker);
            this.WorkerInfoPopupRequest.Raise(infoVM, notification =>
            {
                var workerViewModel = notification as WorkerInfoViewModel;
                var newWorker = workerViewModel.Worker;
                if (!workerViewModel.IsCancel && newWorker.ID != -1)
                {
                    var updatedWorkerIndex = Workers.ToList().FindIndex(w => { return w.ID == newWorker.ID; });
                    Workers[updatedWorkerIndex] = newWorker;
                    Worker = newWorker;//将当前选中的Worker重新锁定到该行
                    PatrolSettingViewModel.Instance.PublishDataChange(ChangeEvent.WorkersChange);
                }
            });
        }

        public async void DelWorker()
        {
            // ShowConfirmDialogAction() 返回一个 Task<MessageDialogResult> 类型
            var result = ShowConfirmDialog("确定要删除吗?", "删除后数据不可恢复,并且会删除计划和排班表中的相应数据");

            DBug.w("等待用户回复");
            // 使用 await关键字等待用户操作... 
            // 当用户点击后 , result将返回 用户的点击结果
            if (await result == MessageDialogResult.Negative) //用户取消
            {
                return;
            }
            else
            {
                if (DataService.DelWorker(Worker)) // 删除成功 (数据库中 Deleted设置为 1)
                {
                    this.Workers.Remove(Worker);
                    Worker = null;
                    PatrolSettingViewModel.Instance.PublishDataChange(ChangeEvent.WorkersChange);
                }
                else
                {
                    ShowMessageDialog("计数员删除失败!", null);
                };
            }
        }


        ////发送计数员信息到 计数机
        //public async void SendWorkers()
        //{
        //    var result = await ShowConfirmDialog("将重新设置计数机中的计数员信息", "该操作预计耗时10秒,请保持计数机的正常连接");

        //    if (result == MessageDialogResult.Negative) //用户取消
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        AppStatusViewModel.Instance.ShowProgress(true, "正在设置计数机的计数员信息...", 10000);
        //        string errorInfo = null;
        //        //clear infos
        //        var clearWorkerInfoFlow = await SerialPortUtil.Write(new ClearWorkerInfo());
        //        if (clearWorkerInfoFlow.Check())
        //        {
        //            //set worker count
        //            var setWorkerCountFlow = await SerialPortUtil.Write(new SetWorkerCount(Workers.Count));
        //            if (setWorkerCountFlow.Check())
        //            {
        //                /// 3. 发送每个人员
        //                for (int i = 0; i < Workers.Count; i++)
        //                {
        //                    var worker = Workers[i];
        //                    var setWorkerInfoFlow = await SerialPortUtil.Write(new SetWorkerInfo(i, worker.Card, worker.Name));
        //                    if (!setWorkerInfoFlow.Check())
        //                    {
        //                        errorInfo = setWorkerInfoFlow.ResultInfo;
        //                        break;
        //                    }
        //                }
        //                AppStatusViewModel.Instance.ShowInfo("计数机的计数员信息设置成功!");
        //                return;
        //            }
        //            else
        //            {
        //                errorInfo = setWorkerCountFlow.ResultInfo;
        //            }
        //        }
        //        else
        //        {
        //            errorInfo = clearWorkerInfoFlow.ResultInfo;
        //        }
        //        AppStatusViewModel.Instance.ShowError("计数员信息设置失败: " + errorInfo);
        //        return;
        //    }
        //}
        private void Print()
        {
            //var printData = new PrintData() { ContentList = Workers.ToList(), Title = "计数员信息", DataCount = Workers.Count };
            //var printer = new Printer(new FrequenceDocument(printData));
            //printer.ShowPreviewWindow();
        }



        ///-----------------------------------------------------------------------
        /// 以下是 批量读取的代码
        /// ----------------------------------------------------------------------
        public BatchAddViewModel BatchAddVM { get; set; }
        //计数机批量读取的钮号
        private void InitBatchAdd()
        {
            this.BatchAddVM = new BatchAddViewModel(LanLoader.Load(LanKey.BatchAddReadWorker), OnBatchAdd, OnGetRecords);
        }

        public bool OnBatchAdd(List<AddItem> selectItems)
        {
            //是否所有item都能添加
            bool allCanAdd = true;
            List<Worker> workers = new List<Worker>();
            //验证每个Item能否添加
            foreach (var item in selectItems)
            {
                string error = "";
                var newWorker = new Worker() { Card = item.Card, Name = item.Name};
                workers.Add(newWorker);
                if (!DataService.CanAdd(newWorker, out error)) //有任意一个 不能添加
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
            workers.ForEach((worker) => { DataService.AddWorker(worker, out id, out errorInfo); });
            //添加完成,刷新界面的Event
            this.Workers = new ObservableCollection<Worker>(DataService.GetAllWorker());

            return true;
        }

        public void OnGetRecords(List<AddItem> items)
        {

        }

        public void onPortWrittingStateChanged(bool isWrite)
        {
            Application.Current.Dispatcher.BeginInvoke(
           new Action(
               () =>
               {
                   //在这里操作UI
                   //this.CSendWorkers.RaiseCanExecuteChanged();
               })
                 , null);
        }
    }
}
