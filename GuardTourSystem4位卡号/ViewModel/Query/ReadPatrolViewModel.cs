using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Model.DAL;
using GuardTourSystem.Model.Model;
using GuardTourSystem.Print;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using GuardTourSystem.Utils;
using KaiheSerialPortLibrary;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
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


        public ObservableCollection<RawData> RawDatas { get; set; } // 巡检机数据


        public ReadPatrolViewModel()
        {
            SerialPortManager.Instance.AddListener(this);

            this.RawDatas = new ObservableCollection<RawData>();
            this.InfoVM = new InfoViewModel();
            this.CReadRecord = new DelegateCommand(this.ReadRecord, () => !SerialPortManager.Instance.IsWritting); //,() => { return !MainWindowViewModel.Instance.IsWriting; });
            this.CClearRecord = new DelegateCommand(() => this.ClearRecord(), () => !SerialPortManager.Instance.IsWritting);//, () => { return !MainWindowViewModel.Instance.IsWriting; });
            this.CPrint = new DelegateCommand(this.Print);
        }

        ///<summary>
        ///读取巡检记录 PatrolReocrd -> 原始数据 RawData -> 更新记录 Record
        ///     1. 获取巡检机中的巡检记录
        ///     2. 调用 HandleDeviceData() 处理巡检数据 
        ///     3. 删除巡检机中的巡检记录
        ///     4. 校准巡检机时间
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

            // 将机器记录machineRecord转成 巡检机数据deviceRecords (添加一个接收时间)
            var deviceRecords = new List<DevicePatrolRecord>();
            var readTime = DateTime.Now;
            foreach (var record in patrolRecords)
            {
                deviceRecords.Add(new DevicePatrolRecord(record.Device, record.Time, record.Card, readTime));
            }

            // 处理接收到的巡检机数据 , 并显示到UI
            if (!await HandleDeviceData(deviceRecords))
            {
                //处理数据失败,不删除巡检机数据
                return;
            }

            //3. 处理完再删除巡检机数据
            await this.ClearRecord();

            //校准巡检机时间
            InfoVM.Append(LanLoader.Load(LanKey.DeviceTestVerifingTime));
            var setTimeResult = await SerialPortUtil.Write(new SetDeviceTime(DateTime.Now));
            InfoVM.Append(setTimeResult.Check() ?
                    LanLoader.Load(LanKey.DeviceTestVerifyTimeSuccess) :
                    LanLoader.Load(LanKey.DeviceTestVerifyTimeFail, setTimeResult.Result.ToLanString()));

        }

        /// <summary>
        /// 处理接收到的巡检数据
        /// 1. 保存 巡检数据DeviceData
        /// 2. 生成并保存 原始数据RawData
        /// 3. 更新考核表 (T_Record)
        /// </summary>
        /// <param name="deviceDatas"></param>
        /// <returns></returns>
        public async Task<bool> HandleDeviceData(List<DevicePatrolRecord> deviceDatas)
        {
            //保存 巡检数据 
            InfoVM.Append(LanKey.PatrolDataHandling);

            var saveResult = new DeviceDataDAO().AddDeviceRecord(deviceDatas);
            var saveResultText = saveResult ?
                LanLoader.Load(LanKey.PatrolDataSaveSuccess, deviceDatas.Count) :
                LanLoader.Load(LanKey.PatrolDataSaveFail);
            InfoVM.Append(saveResultText);


            IRawDataService rds = new RawDataBLL();
            IDutyService ids = new DutyBLL();
            // 生成并保存原始数据
            AppStatusViewModel.Instance.ShowProgress(true, "正在处理巡检数据...");
            InfoVM.Append("正在处理巡检数据...");
            var rawDatas = await Task.Run(() => { return rds.GenerateRawData(deviceDatas); });
            //更新UI
            await Task.Run(() =>
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


            // 如果有原始数据,更新考核表
            InfoVM.Append(LanKey.PatrolDataDutyUpdating);
            var updateResult = await Task.Run(() => { return ids.UpdateDuty(rawDatas); });
            InfoVM.Append(updateResult ? LanKey.PatrolDataDutyUpdateSuccess : LanKey.PatrolDataDutyUpdateFail);

         
            AppStatusViewModel.Instance.ShowCompany();

            return saveResult && updateResult;
        }

        /// <summary>
        /// 清空巡检记录
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
