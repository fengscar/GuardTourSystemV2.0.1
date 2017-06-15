using MahApps.Metro.Controls;
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
using MahApps.Metro.Controls.Dialogs;
using GuardTourSystem.Model;
using GuardTourSystem.ViewModel;
using GuardTourSystem.Utils;
using GuardTourSystem.Print;
using Telerik.Windows.Documents.UI;

namespace GuardTourSystem.View
{
    /// <summary>
    /// WorkerView.xaml 的交互逻辑
    /// </summary>
    public partial class WorkerView : ShowContentView
    {
        public WorkerView()
        {
            InitializeComponent();
            this.DataContext = new WorkerViewModel();
        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            ExcelExporter.TelerikControlExport(this.GridView, LanLoader.Load(LanKey.Export_Worker));
        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            var setting = new PrintSettings()
            {
                DocumentName = "Worker Info "
            };
            PrintExtensions.Print(this.GridView, setting);
        }
        //private void usercontrol_Loaded(object sender, RoutedEventArgs e)
        //{
        //    MainWindow mw = Window.GetWindow(this) as MainWindow;
        //    vm.ShowMessageDialog = new Action<string, string>(mw.ShowMessageMetroDialog);
        //    vm.ShowConfirmDialogAction = new Func<string, string, Task<MessageDialogResult>>(mw.ShowMetroConfirmDialog);
        //}

    }
}
