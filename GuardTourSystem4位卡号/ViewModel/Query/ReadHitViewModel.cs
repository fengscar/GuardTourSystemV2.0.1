using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Print;
using KaiheSerialPortLibrary;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GuardTourSystem.ViewModel
{
    class ReadHitViewModel : ShowContentViewModel, ISerialPortStateChangedListener
    {
        public HitBLL DataService { get; set; }
        private ObservableCollection<DeviceHitRecord> hits;
        public ObservableCollection<DeviceHitRecord> Hits //敲击记录
        {
            get { return hits; }
            set
            {
                SetProperty(ref this.hits, value);
            }
        }
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                SetProperty(ref this.isBusy, value);
            }
        }


        public InfoViewModel InfoVM { get; set; }
        public DelegateCommand CReadHit { get; set; }
        public DelegateCommand CShowHit { get; set; }
        public DelegateCommand CClearHit { get; set; }

        public DelegateCommand CPrint { get; set; }

        private bool showClearHitBtn;

        public bool ShowClearBtn //是否要显示 清空 按键
        {
            get { return showClearHitBtn; }
            set
            {
                SetProperty(ref this.showClearHitBtn, value);
            }
        }

        public ReadHitViewModel()
        {
            SerialPortManager.Instance.AddListener(this);

            DataService = new HitBLL();
            ShowClearBtn = false;
            InfoVM = new InfoViewModel();
            this.Hits = new ObservableCollection<DeviceHitRecord>();

            this.CReadHit = new DelegateCommand(this.ReadHit, () => !SerialPortManager.Instance.IsWritting);
            this.CShowHit = new DelegateCommand(this.ShowHit);
            this.CClearHit = new DelegateCommand(this.ClearHit, () => !SerialPortManager.Instance.IsWritting);

            this.CPrint = new DelegateCommand(this.Print);
        }


        // 读取敲击记录 
        //1. get hit count
        //2. get device id
        //3. get each hit record
        public async void ReadHit()
        {
            InfoVM.Clear();
            Hits.Clear();

            IsBusy = true;

            InfoVM.Append("正在获取敲击记录...");
            var hitBundle = await AppSerialPortUtil.GetAllHitRecords();
            if (!hitBundle.Check())
            {
                InfoVM.Append("获取敲击记录失败: " + hitBundle.ResultInfo);
                IsBusy = false;
                return;
            }
            var hitRecords = (List<HitRecord>)hitBundle.Value;
            if (hitRecords.Count == 0)
            {
                InfoVM.Append("计数机中没有敲击记录!!!");
                IsBusy = false;
                return;
            }
            InfoVM.Append("获取敲击记录成功.");
            //不删除设备上的敲击记录

            //将 计数机敲击记录 转化成 数据库敲击记录 TODO 是否多余???
            var deviceHits = new List<DeviceHitRecord>();
            foreach (var record in hitRecords)
            {
                deviceHits.Add(new DeviceHitRecord(record.Device, record.Time));
            }

            //添加到数据库中,由于敲击记录不删除,添加时要去重.(有数据库的DAO层操作)

            if (DataService.AddHits(deviceHits))
            {
                InfoVM.Append("敲击记录保存成功.");
                //更新UI
                deviceHits.ForEach(h => Hits.Add(h));
            }
            else
            {
                InfoVM.Append("敲击记录保存失败.");
            }
            IsBusy = false;
            return;
        }

        private void ShowHit()
        {
            InfoVM.Clear();
            Hits.Clear();

            IsBusy = true;
            InfoVM.Append("正在读取已存储的敲击记录...");
            Hits = new ObservableCollection<DeviceHitRecord>(DataService.GetAllHits());

            InfoVM.Append("读取敲击记录成功.");
            IsBusy = false;
        }

        public async void ClearHit()
        {
            InfoVM.Append("正在清空敲击记录...");
            var clearHitFlow = await SerialPortUtil.Write(new ClearHitRecord());
            if (!clearHitFlow.Check())
            {
                InfoVM.Append("清除敲击记录失败: " + clearHitFlow.ResultInfo);
                return;
            }
            InfoVM.Append("敲击记录已清空");
        }

        private void Print()
        {
            var printData = new PrintData() { ContentList = Hits.ToList(), Title = "敲击记录", DataCount = Hits.Count };
            var printer = new Printer(new HitDocument(printData));
            printer.ShowPreviewWindow();
        }

        public void onPortWrittingStateChanged(bool isWrite)
        {
            Application.Current.Dispatcher.BeginInvoke(
           new Action(() =>
           {
               //在这里操作UI
               this.CReadHit.RaiseCanExecuteChanged();
               this.CClearHit.RaiseCanExecuteChanged();
           })
           , null);
        }
    }
}
