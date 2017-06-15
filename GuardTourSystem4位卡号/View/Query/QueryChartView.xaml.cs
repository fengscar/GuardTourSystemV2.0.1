using GuardTourSystem.View.Query.ChartControls;
using GuardTourSystem.ViewModel;
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

namespace GuardTourSystem.View
{
    /// <summary>
    /// QueryChartView.xaml 的交互逻辑
    /// </summary>
    public partial class QueryChartView : ShowContentView
    {
        public QueryChartView()
        {
            InitializeComponent();
            this.DataContext = new QueryChartViewModel();
        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.ContentControl.Content is IPrintable)
            {
                var printalbe = this.ContentControl.Content as IPrintable;
                printalbe.PrintView();
            }
            else
            {
                throw new Exception("还未实现IPrintable接口");
            }
        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            //如果当前的选项卡不是 表格,切换到表格
            if (this.ContentControl.Content is WorkerChart)
            {
                var workerChart = this.ContentControl.Content as WorkerChart;
                if (workerChart.TabControl.SelectedIndex != 0)
                {
                    workerChart.TabControl.SelectedIndex = 0;
                }
                var workerGridChartTabItem = workerChart.TabControl.SelectedItem as RadTabItem;
                var workerGridChart = workerGridChartTabItem.Content as WorkerGridControl;
                workerGridChart.ExportExcel();

            }
            if (this.ContentControl.Content is RouteChart)
            {
                var routeChart = this.ContentControl.Content as RouteChart;
                if (routeChart.TabControl.SelectedIndex != 0)
                {
                    routeChart.TabControl.SelectedIndex = 0;
                }
                var routeGridChartTabItem = routeChart.TabControl.SelectedItem as RadTabItem;
                var routeGridChart = routeGridChartTabItem.Content as RouteGridControl;
                routeGridChart.ExportExcel();
            }
        }
    }
}
