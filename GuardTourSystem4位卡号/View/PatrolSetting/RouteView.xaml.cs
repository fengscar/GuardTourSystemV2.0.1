using GuardTourSystem.Utils;
using GuardTourSystem.ViewModel;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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

namespace GuardTourSystem.View
{
    /// <summary>
    /// RouteView.xaml 的交互逻辑
    /// </summary>
    public partial class RouteView : ShowContentView
    {
        private RoutePlaceViewModel vm;

        public RouteView()
        {
            InitializeComponent();

            vm = new RoutePlaceViewModel();
            DataContext = vm;

        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            ExcelExporter.TelerikControlExport(TreeListView, LanLoader.Load(LanKey.Export_RouteAndPlace));
        }
    }
}
