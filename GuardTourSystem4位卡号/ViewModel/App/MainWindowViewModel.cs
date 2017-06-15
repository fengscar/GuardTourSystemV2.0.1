using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.BLL;
using GuardTourSystem.Settings;
using GuardTourSystem.Utils;
using GuardTourSystem.View;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GuardTourSystem.ViewModel
{
    class MainWindowViewModel : ShowContentViewModel
    {
        #region 单例模式
        private static readonly object locker = new object();
        private static MainWindowViewModel instance = null;
        public static MainWindowViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new MainWindowViewModel();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        #region 根据当前内容 动态改变 窗口名称
        /// <summary>
        /// APP名称,固定不变.
        /// </summary>
        public string AppName { get { return AppSetting.Default.SoftwareName; } }
        /// <summary>
        /// 窗口名称  = APP名称 + 当前显示内容名称;
        /// </summary>
        public string WindowName //窗口名称为 AppName + windowName
        {
            get
            {
                if (string.IsNullOrEmpty(ContentName))
                {
                    return AppName;
                }
                return AppName + " ---- " + ContentName;
            }
        }
        /// <summary>
        /// 当前显示内容的名称
        /// </summary>
        private string contentName;
        public string ContentName
        {
            get { return contentName; }
            set
            {
                SetProperty(ref this.contentName, value);
                OnPropertyChanged("WindowName");//提示WindowName改变
                AppShortcutViewModel.Instance.ContentChange(value);
            }
        }

        #endregion

        /// <summary>
        /// 当前登录的用户
        /// </summary>
        public User User { get; set; }


        private MainWindowViewModel()
        {
            InitData();
        }

        private void InitData()
        {
            //配置数据库初始化
            SettingManager.Init();

            //巡检数据库初始化
            PatrolSQLiteManager.Init();
            //将 值班表更新到 当天
            IDutyService DutyService = new DutyBLL();
            string error;
            //DutyService.GenerateDuty(DateTime.Now.AddDays(-7),DateTime.Now);
            DutyService.GenerateDuty(out error);
            if (error != null)
            {
                AppStatusViewModel.Instance.ShowError("生成值班表失败: " + error, 10);
            }
        }

        public void ShowMainWindow(User user)
        {
            this.User = user;
            MainWindow.Instance.Show();
        }
        public void InitMainWindow()
        {
            Task.Run(() =>
            {
                Thread.Sleep(2000);//延迟,等待LoginWindow显示完成后再进行初始化
                Application.Current.Dispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        if (!MainWindow.Instance.IsInitialized)
                        {
                            MainWindow.Instance.InitializeComponent();
                        }
                    })
                    , null);
            });
        }
        public void CloseMainWindow(object sender, EventArgs e)
        {
            //关闭巡检数据库
            SQLiteHelper.Instance.CloseDatabase();
            // 关闭所有窗口 ( 终止应用)
            Application.Current.Shutdown();
        }
    }
}
