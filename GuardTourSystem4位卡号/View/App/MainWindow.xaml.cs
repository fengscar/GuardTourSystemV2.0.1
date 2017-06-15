using GuardTourSystem.Model;
using GuardTourSystem.Utils;
using GuardTourSystem.ViewModel;
using GuardTourSystem.Settings;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using log4net;
using GuardTourSystem.Database.BLL;

namespace GuardTourSystem.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow, IInteractionRequestAware
    {
        private static MainWindow instance;
        public static MainWindow Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainWindow();
                }
                return instance;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            instance = this;

            this.DataContext = MainWindowViewModel.Instance;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveWindowSize();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWindowSize();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Closed += MainWindowViewModel.Instance.CloseMainWindow;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //拖动
            try
            {
                this.DragMove();
            }
            catch (Exception)
            {
                DBug.w(e);
            }
        }

        public Action FinishInteraction { get; set; }

        public INotification Notification { get; set; }



        public void SaveWindowSize()
        {
            //窗口关闭时 保存窗口的大小
            if (this.WindowState == WindowState.Maximized)
            {
                return;
            }
            AppSetting.Default.Save();
        }

        //恢复窗口的大小 . 如果窗口大过屏幕... 重新定位
        public void LoadWindowSize()
        {
            //var width = AppSetting.Default.MainWindowWidth;
            //var height = AppSetting.Default.MainWindowHeight;

            //var sh = SystemParameters.PrimaryScreenHeight;
            //if (width > 1 && height > 1)
            //{
            //    this.Width = width;
            //    this.Height = height;
            //}

            //if (this.Width >= SystemParameters.PrimaryScreenWidth)
            //{
            //    this.Width = SystemParameters.PrimaryScreenWidth - 100;
            //}

            //if (this.Height >= SystemParameters.PrimaryScreenHeight)
            //{
            //    this.Height = SystemParameters.PrimaryScreenHeight - 100;
            //}

            //this.Left = SystemParameters.PrimaryScreenWidth / 2 - this.Width / 2;
            //this.Top = SystemParameters.PrimaryScreenHeight / 2 - this.Height / 2;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            throw new Exception("错误");
        }

        public void ShowMetroMessageDialog(string title, string message)
        {
            this.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "好的" });
        }

        public async Task<MessageDialogResult> ShowMetroConfirmDialog(string title, string message)
        {
            return await this.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AffirmativeButtonText = "确定", NegativeButtonText = "取消" });
        }

    }
}
