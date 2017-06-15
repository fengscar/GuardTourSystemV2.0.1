using GuardTourSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GuardTourSystem.View
{
    /// <summary>
    /// SetupWizardView.xaml 的交互逻辑
    /// </summary>
    public partial class SetupWizardView : UserControl
    {
        public AppStatusViewModel VM { get; set; }

        public SetupWizardView()
        {
            InitializeComponent();
            VM = AppStatusViewModel.Instance;
        }

        private void ShowCompany(object sender, RoutedEventArgs e)
        {
            VM.ShowCompany();
        }
        private void ShowError(object sender, RoutedEventArgs e)
        {
            VM.ShowError("错误信息 默认五秒");
        }
        private void ShowInfo(object sender, RoutedEventArgs e)
        {
            VM.ShowInfo("提示信息 默认三秒");

        }
        private void ShowProgress(object sender, RoutedEventArgs e)
        {
            VM.ShowProgress(true, "正在测试进度条");
        }

        private void UpdateProgress(object sender, RoutedEventArgs e)
        {
            DispatcherTimer _mainTimer = new DispatcherTimer();
            int i = 0;
            _mainTimer.Interval = TimeSpan.FromMilliseconds(200);
            _mainTimer.Tick += new EventHandler((obj, ev) => { i++; VM.UpdateProgress(i+"/"+100, i); });
            _mainTimer.IsEnabled = true;
        }
    }
}
