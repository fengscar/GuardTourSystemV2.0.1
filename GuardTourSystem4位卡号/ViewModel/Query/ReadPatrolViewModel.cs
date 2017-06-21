using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Print;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using GuardTourSystem.Utils;
using KaiheSerialPortLibrary;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GuardTourSystem.ViewModel
{
    class ReadPatrolViewModel : ShowContentViewModel, ISerialPortStateChangedListener
    {
        public InfoViewModel InfoVM { get; set; }

        public DelegateCommand CReadRecord { get; set; }
        public DelegateCommand CClearRecord { get; set; }
        public DelegateCommand CPrint { get; set; }


        public ObservableCollection<RawData> RawDatas { get; set; } // 计数机数据


        public ReadPatrolViewModel()
        {
            SerialPortManager.Instance.AddListener(this);

            this.RawDatas = new ObservableCollection<RawData>();
            this.InfoVM = new InfoViewModel();
            this.CReadRecord = new DelegateCommand(this.ReadRecord, () => !SerialPortManager.Instance.IsWritting); //,() => { return !MainWindowViewModel.Instance.IsWriting; });
            this.CClearRecord = new DelegateCommand(this.ClearRecordWithAsk, () => !SerialPortManager.Instance.IsWritting);//, () => { return !MainWindowViewModel.Instance.IsWriting; });
            this.CPrint = new DelegateCommand(this.Print);
        }

        ///<summary>
        ///读取计数记录 PatrolReocrd -> 原始数据 RawData -> 更新记录 Record
        ///     1. 获取计数机中的计数记录
        ///     2. 调用 HandleDeviceData() 处理计数数据 
        ///     3. 删除计数机中的计数记录
        ///     4. 校准计数机时间
        ///</summary>
        public async void ReadRecord()
        {
            InfoVM.Clear();
            RawDatas.Clear();

            InfoVM.Append(LanKey.PatrolDataReading);
            var recordBundle = await AppSerialPortUtil.GetAllPatrolRecord();
            if (!recordBundle.Check())
            {
                InfoVM.Append(LanKey.PatrolDataReadFail, recordBundle.Result.ToLanString());
                return;
            }
            var patrolRecords = (List<PatrolRecord>)recordBundle.Value;
            if (patrolRecords.Count == 0)
            {
                InfoVM.Append(LanKey.PatrolDataEmptyData);
                return;
            }
            InfoVM.Append(LanKey.PatrolDataReadSuccess);

            // 将机器记录machineRecord转成 计数机数据deviceRecords (添加一个接收时间)
            var deviceRecords = new List<DevicePatrolRecord>();
            var readTime = DateTime.Now;
            patrolRecords.ForEach(record => deviceRecords.Add(new DevicePatrolRecord(record.Device, record.Time, record.Card, readTime)));

            // 处理接收到的计数机数据 , 并显示到UI
            if (!await HandleDeviceData(deviceRecords))
            {
                //处理数据失败,不删除计数机数据
                return;
            }
            //3. 处理完再删除计数机数据
            await this.ClearRecord();

            //校准计数机时间
            InfoVM.Append(LanLoader.Load(LanKey.DeviceTestVerifingTime));
            var setTimeResult = await SerialPortUtil.Write(new SetDeviceTime(DateTime.Now));
            InfoVM.Append(setTimeResult.Check() ?
                    LanLoader.Load(LanKey.DeviceTestVerifyTimeSuccess) :
                    LanLoader.Load(LanKey.DeviceTestVerifyTimeFail, setTimeResult.Result.ToLanString()));

        }

        /// <summary>
        /// 处理接收到的计数数据
        /// 1. 保存 计数数据DeviceData
        /// 2. 生成并保存 原始数据RawData
        /// </summary>
        /// <param name="deviceDatas"></param>
        /// <returns></returns>
        public async Task<bool> HandleDeviceData(List<DevicePatrolRecord> deviceDatas)
        {
            //保存 计数数据 
            InfoVM.Append(LanKey.PatrolDataHandling);

            var saveResult = new DeviceDataDAO().AddDeviceRecord(deviceDatas);
            var saveResultText = saveResult ?
                LanLoader.Load(LanKey.PatrolDataSaveSuccess, deviceDatas.Count) :
                LanLoader.Load(LanKey.PatrolDataSaveFail);
            InfoVM.Append(saveResultText);


            IRawDataService rds = new RawDataBLL();

            // 生成并保存原始数据
            AppStatusViewModel.Instance.ShowProgress(true, "正在处理计数数据...");
            InfoVM.Append("正在处理计数数据...");

            SQLiteHelper.Instance.BeginTransaction();
            // 存 RawData
            var rawDatas = await Task.Factory.StartNew(() => { return rds.GenerateRawData(deviceDatas); });
            // 开始统计并存入T_Count表
            if (rawDatas.Count != 0)
            {
                CountBLL countBll = new CountBLL();
                countBll.Count(rawDatas);
            }
            SQLiteHelper.Instance.CommitTransaction();
            //更新UI
            await Task.Factory.StartNew(() =>
            {
                foreach (var raw in rawDatas)
                {
                    RawDatas.Add(raw);
                }
            });

            if (!saveResult)
            {
                InfoVM.Append(LanKey.PatrolDataHandleFail);
            }
            else
            {
                InfoVM.Append(LanKey.PatrolDataHandleSuccess);
            }


            //// 如果有原始数据,更新考核表
            //InfoVM.Append(LanKey.PatrolDataDutyUpdating);
            //var updateResult = await Task.Run(() => { return ids.UpdateDuty(rawDatas); });
            //InfoVM.Append(updateResult ? LanKey.PatrolDataDutyUpdateSuccess : LanKey.PatrolDataDutyUpdateFail);


            AppStatusViewModel.Instance.ShowCompany();

            return saveResult;
        }
        public async void ClearRecordWithAsk()
        {
            var result = await this.ShowConfirmDialog("确定要清空设备中的打卡数据吗?", "清空后将无法恢复!");
            if (result == MessageDialogResult.Negative) //用户取消
            {
                return;
            }
            else
            {
                ClearRecord();
            }
        }
        /// <summary>
        /// 清空计数记录
        /// </summary>
        public async Task<bool> ClearRecord()
        {
            var clearingText = LanLoader.Load(LanKey.PatrolDataClearing);
            InfoVM.Append(clearingText);
            AppStatusViewModel.Instance.ShowProgress(true, clearingText);
            var clearResult = await SerialPortUtil.Write(new ClearPatrolRecord());
            if (!clearResult.Check())
            {
                var failText = LanLoader.Load(LanKey.PatrolDataClearFail, clearResult.Result.ToLanString());
                InfoVM.Append(failText);
                AppStatusViewModel.Instance.ShowError(failText);
            }
            else
            {
                var sucText = LanLoader.Load(LanKey.PatrolDataClearSuccess);
                InfoVM.Append(sucText);
                AppStatusViewModel.Instance.ShowInfo(sucText);
            }
            return clearResult.Check();
        }

        private void Print()
        {
            //var printData = new PrintData() { ContentList = RawDatas.ToList(), Title = LanLoader.Load(LanKey.PatrolData), DataCount = RawDatas.Count };
            //var printer = new Printer(new RawDataDocument(printData,null));
            //printer.ShowPreviewWindow();
        }


        public void onPortWrittingStateChanged(bool isWrite)
        {
            Application.Current.Dispatcher.BeginInvoke(
            new Action(() =>
                {
                    //在这里操作UI
                    this.CReadRecord.RaiseCanExecuteChanged();
                    this.CClearRecord.RaiseCanExecuteChanged();
                })
            , null);
        }
    }
}
