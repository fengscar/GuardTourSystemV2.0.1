using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GuardTourSystem.ViewModel;
using GuardTourSystem.Services.Database.BLL;
using GuardTourSystem.Settings;
using GuardTourSystem.Utils;
using GuardTourSystem.Services;
using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using System.Windows.Threading;
using log4net;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Threading;
using KaiheSerialPortLibrary;

namespace GuardTourSystem
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        [DllImport("user32", CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindow(string cls, string win);
        [DllImport("user32")]
        static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32")]
        static extern bool IsIconic(IntPtr hWnd); //该窗口是否是最小化
        [DllImport("user32")]
        static extern IntPtr OpenIcon(IntPtr hWnd);


        public static Mutex StaticMutex { get; set; }//要作为属性,不然会被回收,导致应用多次打开

        protected override void OnStartup(StartupEventArgs e)
        {
            //判断配置文件是否正常
            VerifyConfigFile();

            // 处理所有的异常
            App.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(UnhandledExceptionEventHandler);

            //载入当前设置的语言
            LanLoader.ChangeLanguage(AppSetting.Default.Language);

            //判断软件是否已经开启
            IsNewApplication();

            //串口初始化
            SerialPortDebug.DEBUG = false;
            DBug.DEBUG = false;
            //SerialPortManager.Instance.ChangeBautRate(460800);
            SerialPortManager.Instance.ChangeBautRate(9600);
            ////Task.Run(() => SerialPortManager.Instance.InitKaiHeDevices()); //初始化放在后台完成

        }

        private void VerifyConfigFile()
        {
            try
            {
                object textCanGet = null;
                textCanGet = AppSetting.Default.Language;
                textCanGet = AppSetting.Default.LoginUser;

                textCanGet = AppSetting.Default.SoftwareName;
                textCanGet = AppSetting.Default.SoftwareVersion;
                textCanGet = AppSetting.Default.CopyrightInfo;

                textCanGet = AppSetting.Default.CompanyName;
                textCanGet = AppSetting.Default.CompanyTel;
                textCanGet = AppSetting.Default.CompanyAddress;
                textCanGet = AppSetting.Default.CompanyWebsite;
            }
            catch
            {
                MessageBox.Show("错误!配置文件被损坏. \nSorry,Config file error.");
                Shutdown();
            }
        }

        // 处理全局异常 的函数 
        // 1. 写入日志
        // 2. 弹窗提示
        static void UnhandledExceptionEventHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //Util.Log(e.ExceptionObject.ToString());//写日志
            Logger.Error(e.Exception.Message, e.Exception);
            //弹窗提示
            AppContentViewModel.Instance.PopupWindow(PopupEnum.Error, e.Exception.ToString());
            e.Handled = true;
        }

        //判断应用是否已经启动
        // 使用一个互斥锁来判断: 当应用启动时,获取名称为GuardTourSystem的互斥锁,判断该锁是否为新.
        // 如果是新的,跳过
        // 如果不是新的,调用USER32.DLL中的FindWindow来查找主窗口,并让其显示到最前端,同时关闭第二次打开的应用
        private void IsNewApplication()
        {
            var ss = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            bool isNew;
            StaticMutex = new Mutex(true, "KaiHeGuardTourSystem", out isNew);
            if (!isNew)
            {
                var windowHandle = GetCurrentWindowHandle();
                if (windowHandle != IntPtr.Zero)
                {
                    //显示已经打开的...
                    SetForegroundWindow(windowHandle);
                    //如果是最小化的窗口,要进行OpenIcon
                    if (IsIconic(windowHandle))
                        OpenIcon(windowHandle);
                }
                //关闭第二个启动的程序
                Shutdown();
            }
        }


        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            //关闭串口
            SerialPortManager.Instance.CloseDevicePort();

            SQLiteHelper.Instance.CloseDatabase();

        }

        /// <summary>
        /// 得到当前软件的窗口的Handle
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetCurrentWindowHandle()
        {
            var windowTitle = MainWindowViewModel.Instance.WindowName; //得到当前的主窗口名称
            var mainWindow = FindWindow(null, windowTitle); //第二个参数是主窗口的标题名称...(不是MainWindow)
            if (mainWindow != IntPtr.Zero)
            {
                return mainWindow;
            }

            var loginTitle = LoginWindowViewModel.Instance.LoginTitle;
            var loginWindow = FindWindow(null, loginTitle);
            if (loginWindow != IntPtr.Zero)
            {
                return loginWindow;
            }

            return IntPtr.Zero;
        }
    }
}
