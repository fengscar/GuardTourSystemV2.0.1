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
    /// RouteChart.xaml 的交互逻辑
    /// </summary>
    public partial class RouteChart : UserControl,IPrintable
    {
        public RouteChart()
        {
            InitializeComponent();
        }

        public void PrintView()
        {
            var tabItem = this.TabControl.SelectedItem as RadTabItem;
            if (tabItem.Content is IPrintable)
            {
                var printalbe = tabItem.Content as IPrintable;
                printalbe.PrintView();
            }
            else
            {
                throw new Exception("TabItem还未实现IPrintable接口");
            }
        }
    }
}
