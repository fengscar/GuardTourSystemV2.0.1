using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GuardTourSystem.Utils;

namespace GuardTourSystem.ViewModel
{
    class AppShortcutViewModel : BindableBase
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
        public DelegateCommand CQueryResult { get; set; }
        public DelegateCommand CQueryChart { get; set; }
        public DelegateCommand CTestMachine { get; set; }
        public DelegateCommand CQuit { get; set; }

        private bool isReadDataCheck;
        public bool ReadDataCheck
        {
            get { return isReadDataCheck; }
            set
            {
                SetProperty(ref this.isReadDataCheck, value);
            }
        }

        private bool isQueryRawDataCheck;
        public bool QueryRawDataCheck
        {
            get { return isQueryRawDataCheck; }
            set
            {
                SetProperty(ref this.isQueryRawDataCheck, value);
            }
        }
        private bool queryResultCheck;
        public bool QueryResultCheck
        {
            get { return queryResultCheck; }
            set
            {
                SetProperty(ref this.queryResultCheck, value);
            }
        }

        private bool queryChartCheck;
        public bool QueryChartCheck
        {
            get { return queryChartCheck; }
            set
            {
                SetProperty(ref this.queryChartCheck, value);
            }
        }

        private bool testDeviceCheck;
        public bool TestDeviceCheck
        {
            get { return testDeviceCheck; }
            set
            {
                SetProperty(ref this.testDeviceCheck, value);
            }
        }
        private AppShortcutViewModel()
        {
            this.CReadData = new DelegateCommand(() => { UnCheckAll(); ReadDataCheck = true; AppContentViewModel.Instance.ShowView(ViewEnum.ReadPatrol); });
            this.CQueryRawData = new DelegateCommand(() => { UnCheckAll(); QueryRawDataCheck = true; AppContentViewModel.Instance.ShowView(ViewEnum.QueryRawData); });
            this.CQueryResult = new DelegateCommand(() => { UnCheckAll(); QueryResultCheck = true; AppContentViewModel.Instance.ShowView(ViewEnum.QueryResult); });
            this.CQueryChart = new DelegateCommand(() => { UnCheckAll(); QueryChartCheck = true; AppContentViewModel.Instance.ShowView(ViewEnum.QueryChart); });
            //这个是弹窗
            this.CTestMachine = new DelegateCommand(() => { UnCheckAll(); TestDeviceCheck = true; AppContentViewModel.Instance.PopupWindow(PopupEnum.DeviceTest); });
            //退出系统
            this.CQuit = new DelegateCommand(() => { Application.Current.Shutdown(); });
        }
        private void UnCheckAll()
        {
            ReadDataCheck = QueryRawDataCheck = QueryResultCheck = QueryChartCheck = TestDeviceCheck = false;
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
            else if (value.Equals(ViewEnum.QueryResult.GetContentName()))
            {
                QueryResultCheck = true;
            }
            else if (value.Equals(ViewEnum.QueryChart.GetContentName()))
            {
                QueryChartCheck = true;
            }
            else if (value.Equals(PopupEnum.DeviceTest.GetContentName()))
            {
                TestDeviceCheck = true;
            }
        }
    }
}
