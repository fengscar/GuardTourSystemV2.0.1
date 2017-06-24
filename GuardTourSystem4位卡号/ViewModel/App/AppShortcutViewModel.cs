using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GuardTourSystem.Utils;

namespace GuardTourSystem.ViewModel
{
    class AppShortcutViewModel : NotificationObject
    {
        private static AppShortcutViewModel instance = null;
        public static AppShortcutViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    if (instance == null)
                    {
                        instance = new AppShortcutViewModel();
                    }
                }
                return instance;
            }
        }
        public DelegateCommand CReadData { get; set; }
        public DelegateCommand CQueryRawData { get; set; }
        public DelegateCommand CQueryRawCount { get; set; }
        //public DelegateCommand CQueryResult { get; set; }
        //public DelegateCommand CQueryChart { get; set; }
        public DelegateCommand CTestMachine { get; set; }
        public DelegateCommand CQuit { get; set; }

        private bool isReadDataCheck;
        public bool ReadDataCheck
        {
            get { return isReadDataCheck; }
            set
            {
                isReadDataCheck = value;
                RaisePropertyChanged("ReadDataCheck");
            }
        }

        private bool isQueryRawDataCheck;
        public bool QueryRawDataCheck
        {
            get { return isQueryRawDataCheck; }
            set
            {
                isQueryRawDataCheck = value;
                RaisePropertyChanged("QueryRawDataCheck");
            }
        }
        private bool isQueryRawCountCheck;
        public bool QueryRawCountCheck
        {
            get { return isQueryRawCountCheck; }
            set
            {
                isQueryRawCountCheck = value;
                RaisePropertyChanged("QueryRawCountCheck");
            }
        }

        //private bool queryResultCheck;
        //public bool QueryResultCheck
        //{
        //    get { return queryResultCheck; }
        //    set
        //    {
        //        queryResultCheck = value;
        //        RaisePropertyChanged("QueryResultCheck");
        //    }
        //}

        //private bool queryChartCheck;
        //public bool QueryChartCheck
        //{
        //    get { return queryChartCheck; }
        //    set
        //    {
        //        SetProperty(ref this.queryChartCheck, value);
        //    }
        //}

        private bool testDeviceCheck;
        public bool TestDeviceCheck
        {
            get { return testDeviceCheck; }
            set
            {
                testDeviceCheck = value;
                RaisePropertyChanged("TestDeviceCheck");
            }
        }
        private AppShortcutViewModel()
        {
            CReadData = new DelegateCommand(() => { UnCheckAll(); ReadDataCheck = true; AppContentViewModel.Instance.ShowView(ViewEnum.ReadPatrol); });
            CQueryRawData = new DelegateCommand(() => { UnCheckAll(); QueryRawDataCheck = true; AppContentViewModel.Instance.ShowView(ViewEnum.QueryRawData); });
            CQueryRawCount = new DelegateCommand(() => { UnCheckAll(); QueryRawCountCheck = true; AppContentViewModel.Instance.ShowView(ViewEnum.QueryRawCount); });
            //this.CQueryResult = new DelegateCommand(() => { UnCheckAll(); QueryResultCheck = true; AppContentViewModel.Instance.ShowView(ViewEnum.QueryResult); });
            //this.CQueryChart = new DelegateCommand(() => { UnCheckAll(); QueryChartCheck = true; AppContentViewModel.Instance.ShowView(ViewEnum.QueryChart); });
            //这个是弹窗
            CTestMachine = new DelegateCommand(() => { UnCheckAll(); TestDeviceCheck = true; AppContentViewModel.Instance.PopupWindow(PopupEnum.DeviceTest); });
            //退出系统
            CQuit = new DelegateCommand(() => { Application.Current.Shutdown(); });
        }
        private void UnCheckAll()
        {
            ReadDataCheck = QueryRawDataCheck = QueryRawCountCheck = TestDeviceCheck = false;
        }


        public void ContentChange(string value)
        {
            UnCheckAll();
            if (value == null)
            {
                return;
            }
            else if (value.Equals(ViewEnum.ReadPatrol.GetContentName()))
            {
                ReadDataCheck = true;
            }
            else if (value.Equals(ViewEnum.QueryRawData.GetContentName()))
            {
                QueryRawDataCheck = true;
            }
            else if (value.Equals(ViewEnum.QueryRawCount.GetContentName()))
            {
                QueryRawCountCheck = true;
            }
            //else if (value.Equals(ViewEnum.QueryResult.GetContentName()))
            //{
            //    QueryResultCheck = true;
            //}
            //else if (value.Equals(ViewEnum.QueryChart.GetContentName()))
            //{
            //    QueryChartCheck = true;
            //}
            else if (value.Equals(PopupEnum.DeviceTest.GetContentName()))
            {
                TestDeviceCheck = true;
            }
        }
    }
}
