using GuardTourSystem.Model;
using GuardTourSystem.ViewModel;
using MahApps.Metro.Controls;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GuardTourSystem.Utils;

namespace GuardTourSystem.View
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow 
    {
        private static LoginWindow instance;
        public static LoginWindow Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoginWindow();
                }
                return instance;
            }
        }

        private LoginWindowViewModel ViewModel;
        public LoginWindow()
        {
            InitializeComponent();
            instance = this;//APP启动该窗口时,是调用构造函数,而不是Instance

            DataContext = ViewModel = LoginWindowViewModel.Instance;
            // 获取焦点
            PasswordBox.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ////将 窗口定位到中心 偏上...
            //this.Left = SystemParameters.PrimaryScreenWidth / 2 - this.Width / 2;
            //this.Top = SystemParameters.PrimaryScreenHeight / 2 - this.Height;

            //登录窗口载入完成后,初始化主窗口
            //MainWindowViewModel.Instance.InitMainWindow();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //如果User为空,表示还未登录,退出APP
            if (MainWindowViewModel.Instance.User == null)
            {
                App.Current.Shutdown();
            }
        }

        private async void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) //点击Enter,进入
            {
                LoginBtn.SetPressed(true);// 使用扩展方法,设置按键被按下
                ViewModel.CLogin.Execute();
                await Task.Factory.StartNew(() => { Thread.Sleep(50); });
                LoginBtn.SetPressed(false);
            }
            else if (e.Key == Key.Escape) //点击Escape,退出
            {
                Close();
            }
        }


        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Focus();
        }
    }
}
