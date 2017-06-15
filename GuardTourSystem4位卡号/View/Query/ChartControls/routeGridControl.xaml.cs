using GuardTourSystem.Print;
using GuardTourSystem.Utils;
using GuardTourSystem.ViewModel.Query.ChartViewModel;
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
using Telerik.Windows.Controls;

namespace GuardTourSystem.View.Query.ChartControls
{
    /// <summary>
    /// routeGridControl.xaml 的交互逻辑
    /// </summary>
    public partial class RouteGridControl : UserControl, IPrintable
    {
        public RouteGridControl()
        {
            InitializeComponent();
        }

        public void PrintView()
        {
            if (this.DataContext is RouteChartViewModel)
            {
                var viewmodel = this.DataContext as RouteChartViewModel;
                viewmodel.Print();
            }
        }

        public void ExportExcel()
        {
            ExcelExporter.TelerikControlExport(this.RouteTreeList, LanLoader.Load(LanKey.RouteCountInfo));
        }
    }
}
