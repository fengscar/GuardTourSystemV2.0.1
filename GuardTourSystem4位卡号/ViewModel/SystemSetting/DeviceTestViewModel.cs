using GuardTourSystem.Utils;
using KaiheSerialPortLibrary;
using Microsoft.Practices.Prism.Commands;
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
    class DeviceTestViewModel : AbstractPopupNotificationViewModel, ISerialPortStateChangedListener
    {
        /// <summary>
        /// 每个测试项目的ViewModel
        /// </summary>
        public class DeviceTestItemViewModel : BindableBase
        {
            /// <summary>
            /// 该项的测试指令
            /// </summary>
            public WriteFlow TestFlow { get; private set; }

            //是否测试
            private Action OnSelectChange;
            private bool isSelect;
            public bool IsSelect
            {
                get { return isSelect; }
                set
                {
                    SetProperty(ref this.isSelect, value);
                    if (OnSelectChange != null)
                    {
                        OnSelectChange();
                    }
                }
            }
            //测试结果
            private string result;
            public string Result
            {
                get { return result; }
                set
                {
                    SetProperty(ref this.result, value);
                }
            }

            //测试项目 类型名称
            public string Name { get; private set; }

            //测试项目 类型key
            public ETestType TestType { get; set; }

            public DeviceTestItemViewModel(ETestType key, Action onSelectChanged)
            {
                this.OnSelectChange = onSelectChanged;
                this.IsSelect = true;
                this.TestType = key;
                this.Result = null;
                InitTestItem();
            }
            private void InitTestItem()
            {
                switch (this.TestType)
                {
                    case ETestType.TEST_LED:
                        this.Name = LanLoader.Load(LanKey.TEST_LED);
                        this.TestFlow = new TestLed();
                        break;
                    case ETestType.TEST_BUZZER:
                        this.Name = LanLoader.Load(LanKey.TEST_BUZZER);
                        this.TestFlow = new TestBuzzer();

                        break;
                    case ETestType.TEST_DEVICE_TIME:
                        this.Name = LanLoader.Load(LanKey.TEST_DEVICE_TIME);
                        this.TestFlow = new GetDeviceTime();
                        break;
                    case ETestType.TEST_DEVICE_ID:
                        this.Name = LanLoader.Load(LanKey.TEST_DEVICE_ID);
                        this.TestFlow = new GetDeviceID();
                        break;
                    case ETestType.TEST_PATROL_COUNT:
                        this.Name = LanLoader.Load(LanKey.TEST_PATROL_COUNT);
                        this.TestFlow = new GetPatrolCount();
                        break;
                    case ETestType.TEST_HIT_COUNT:
                        this.Name = LanLoader.Load(LanKey.TEST_HIT_COUNT);
                        this.TestFlow = new GetHitCount();
                        break;
                    default:
                        break;
                }
            }
            public enum ETestType
            {
                TEST_LED,
                TEST_BUZZER,
                TEST_DEVICE_TIME,
                TEST_DEVICE_ID,
                TEST_PATROL_COUNT,
                TEST_HIT_COUNT,
            }
        }

        private bool selectAll;
        public bool SelectAll
        {
            get
            {
                return selectAll;
            }
            set
            {
                SetProperty(ref this.selectAll, value);
                TestItems.ForEach(item => item.IsSelect = value);
            }
        }

        //是否将巡检机设置为 静音模式
        private bool muteMode;
        public bool MuteMode
        {
            get { return muteMode; }
            set
            {
                SetProperty(ref this.muteMode, value);
            }
        }

        private string deviceID;
        public string DeviceID
        {
            get { return deviceID; }
            set
            {
                InputChecker.CheckRfidCard(ref value, 2);
                SetProperty(ref this.deviceID, value);
                this.CSetDeviceID.RaiseCanExecuteChanged();
            }
        }


        public InfoViewModel InfoVM { get; set; }
        public List<DeviceTestItemViewModel> TestItems { get; set; }
        public DelegateCommand CStartTest { get; set; }
        public DelegateCommand CVerifyTime { get; set; }
        public DelegateCommand CSetDeviceID { get; set; }
        public DelegateCommand CSetNotifyType { get; set; }
        //public DelegateCommand CClearDeviceWorker { get; set; }
        //public DelegateCommand CClearDeviceRoute { get; set; }

        public DeviceTestViewModel()
        {
            SerialPortManager.Instance.AddListener(this);

            this.InfoVM = new InfoViewModel();
            this.Title = "巡检机管理";
            this.TestItems = new List<DeviceTestItemViewModel>();

            this.CStartTest = new DelegateCommand(StartTest, this.CanStartTest);
            this.CVerifyTime = new DelegateCommand(this.VerifyTime, () => !SerialPortManager.Instance.IsWritting);
            this.CSetDeviceID = new DelegateCommand(this.SetDeviceID, () => { return CanSetDeviceID() && !SerialPortManager.Instance.IsWritting; });
            this.CSetNotifyType = new DelegateCommand(this.SetNotifyType, () => !SerialPortManager.Instance.IsWritting);
            //this.CClearDeviceWorker = new DelegateCommand(this.ClearDeviceWorker, () => !SerialPortManager.Instance.IsWritting);
            //this.CClearDeviceRoute = new DelegateCommand(this.ClearDeviceRoute, () => !SerialPortManager.Instance.IsWritting);

            InitTestItems();
        }



        //切换 蜂鸣器/振动 与静音 
        private void SetNotifyType()
        {
            InfoVM.Clear();
            InfoVM.Append("正在设置机号...");

        }

        private async void SetDeviceID()
        {
            InfoVM.Clear();
            InfoVM.Append("正在设置机号...");
            var setIdFlow = await SerialPortUtil.Write(new SetDeviceID(DeviceID));
            if (!setIdFlow.Check())
            {
                InfoVM.Append("机号设置失败:" + setIdFlow.ResultInfo);
                return;
            }
            else
            {
                InfoVM.Append("机号设置成功:已将巡检机机号设置为 " + DeviceID);
            }
        }
        private bool CanSetDeviceID()
        {
            if (string.IsNullOrEmpty(DeviceID))
            {
                return false;
            }
            if (DeviceID.Length != 2)
            {
                return false;
            }
            return true;
        }

        private bool CanStartTest()
        {
            var isPortWriting = SerialPortManager.Instance.IsWritting;
            var hasItemSelect = TestItems.Count != 0 && TestItems.Any(item => item.IsSelect == true);
            return hasItemSelect && !isPortWriting;
        }

        private void InitTestItems()
        {
            this.TestItems.Add(new DeviceTestItemViewModel(DeviceTestItemViewModel.ETestType.TEST_LED, this.OnItemSelectChanged));
            this.TestItems.Add(new DeviceTestItemViewModel(DeviceTestItemViewModel.ETestType.TEST_BUZZER, this.OnItemSelectChanged));
            this.TestItems.Add(new DeviceTestItemViewModel(DeviceTestItemViewModel.ETestType.TEST_DEVICE_ID, this.OnItemSelectChanged));
            this.TestItems.Add(new DeviceTestItemViewModel(DeviceTestItemViewModel.ETestType.TEST_DEVICE_TIME, this.OnItemSelectChanged));
            this.TestItems.Add(new DeviceTestItemViewModel(DeviceTestItemViewModel.ETestType.TEST_PATROL_COUNT, this.OnItemSelectChanged));
            this.TestItems.Add(new DeviceTestItemViewModel(DeviceTestItemViewModel.ETestType.TEST_HIT_COUNT, this.OnItemSelectChanged));
        }

        private void OnItemSelectChanged()
        {
            selectAll = TestItems.All((item) => item.IsSelect == true);
            this.OnPropertyChanged("SelectAll");

            this.CStartTest.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// 校准巡检机时间
        /// </summary>
        private async void VerifyTime()
        {
            InfoVM.Clear();
            InfoVM.Append("正在校准巡检机时间...");
            var setTimeFlow = await SerialPortUtil.Write(new SetDeviceTime(DateTime.Now));
            if (setTimeFlow.Check())
            {
                InfoVM.Append("校准成功:已将巡检机时间设置为 :\n" + DateTime.Now.ToString());
            }
            else
            {
                InfoVM.Append("校准失败:" + setTimeFlow.ResultInfo);
            }
        }

        /// <summary>
        /// 开始测试选中的测试项
        /// 测试每项过程如下:
        /// 1. 发送测试指令时. 
        ///     a.将测试项结果显示为 测试中
        ///     b.并在通信栏显示正在XXX
        /// 2. 测试结果返回后
        ///     a. 在测试项的结果 显示为ResultInfo
        ///     b. 在通信栏显示具体的结果
        /// </summary>
        public async void StartTest()
        {
            InfoVM.Clear();
            TestItems.ForEach(item => item.Result = null);//清空测试项的结果
            var selectItems = TestItems.FindAll(item => item.IsSelect == true);
            foreach (var item in selectItems)
            {
                switch (item.TestType)
                {
                    case DeviceTestItemViewModel.ETestType.TEST_LED:
                        await StartTestItem(item, flow =>
                        {
                            InfoVM.Append(item.Name + " 测试完成\n");
                        });
                        break;
                    case DeviceTestItemViewModel.ETestType.TEST_BUZZER:
                        await StartTestItem(item, flow =>
                        {
                            InfoVM.Append(item.Name + " 测试完成\n");
                        });
                        break;
                    case DeviceTestItemViewModel.ETestType.TEST_DEVICE_ID:
                        await StartTestItem(item, flow =>
                        {
                            var getDeviceID = (GetDeviceID)flow;
                            if (getDeviceID != null)
                            {
                                InfoVM.Append(item.Name + " 测试完成: \n该巡检机机号为 " + getDeviceID.DeviceID + "\n");
                            }
                        });
                        break;
                    case DeviceTestItemViewModel.ETestType.TEST_DEVICE_TIME:
                        await StartTestItem(item, flow =>
                        {
                            var getDeviceTime = (GetDeviceTime)flow;
                            if (getDeviceTime != null)
                            {
                                InfoVM.Append(item.Name + " 测试完成: \n巡检机时间当前时间为 " + getDeviceTime.DeviceTime + "\n");
                            }
                        });
                        break;

                    case DeviceTestItemViewModel.ETestType.TEST_PATROL_COUNT:
                        await StartTestItem(item, flow =>
                        {
                            var getPatrolCountFlow = (GetPatrolCount)flow;
                            if (getPatrolCountFlow != null)
                            {

                                InfoVM.Append(item.Name + " 测试完成:当前有 " + getPatrolCountFlow.Count + " 条巡检记录\n");
                            }
                        });
                        break;

                    case DeviceTestItemViewModel.ETestType.TEST_HIT_COUNT:
                        await StartTestItem(item, flow =>
                        {
                            var getHitCountFlow = (GetHitCount)flow;
                            if (getHitCountFlow != null)
                            {

                                InfoVM.Append(item.Name + " 测试完成:当前有 " + getHitCountFlow.Count + " 条敲击记录");
                            }
                        });
                        break;
                }
            }
        }
        /// <summary>
        /// 开始一项测试,并完成大部分操作
        /// </summary>
        /// <param name="item"></param>
        /// <param name="OnTestFinish">当测试成功时,在通信栏的操作</param>
        private async Task<Boolean> StartTestItem(DeviceTestItemViewModel item, Action<WriteFlow> OnTestSuccess)
        {
            //test start
            item.Result = LanLoader.Load(LanKey.TEST_CONNECTING);
            InfoVM.Append("正在测试: " + item.Name);
            var result = await SerialPortUtil.Write(item.TestFlow);
            //test finished
            if (result.Check())
            {
                item.Result = LanLoader.Load(LanKey.TEST_SUCCESS);
                OnTestSuccess(result);
            }
            else
            {
                item.Result = result.ResultInfo;
                InfoVM.Append(item.Name + " 测试失败!\n");
            }
            return true;
        }
        //private async void ClearDeviceWorker()
        //{
        //    InfoVM.Clear();
        //    InfoVM.Append("正在清空巡检机的人员信息...");

        //    var clearResult = await SerialPortUtil.Write(new ClearWorkerInfo());
        //    if (clearResult.Check())
        //    {
        //        InfoVM.Append("清空巡检机的人员信息成功.");
        //    }
        //    else
        //    {
        //        InfoVM.Append("清空巡检机的人员信息失败:" + clearResult.ResultInfo);
        //    }
        //}

        //private async void ClearDeviceRoute()
        //{
        //    InfoVM.Clear();
        //    InfoVM.Append("正在清空巡检机的线路信息...");

        //    var clearResult = await SerialPortUtil.Write(new ClearPlaceInfo());
        //    if (clearResult.Check())
        //    {
        //        InfoVM.Append("清空巡检机的线路信息成功.");
        //    }
        //    else
        //    {
        //        InfoVM.Append("清空巡检机的线路信息失败:" + clearResult.ResultInfo);
        //    }
        //}

        public void onPortWrittingStateChanged(bool isWrite)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                //在这里操作UI
                this.CVerifyTime.RaiseCanExecuteChanged();
                this.CStartTest.RaiseCanExecuteChanged();
                this.CSetDeviceID.RaiseCanExecuteChanged();
                //this.CClearDeviceWorker.RaiseCanExecuteChanged();
                //this.CClearDeviceRoute.RaiseCanExecuteChanged();
            }), null);
        }
    }
}
