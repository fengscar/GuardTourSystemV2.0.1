using GuardTourSystem.Utils;
using GuardTourSystem.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using GuardTourSystem.Print;
using Telerik.Windows.Controls.GridView;

namespace GuardTourSystem.View
{
    /// <summary>
    /// QueryRecordView.xaml 的交互逻辑
    /// </summary>
    public partial class QueryResultView : ShowContentView
    {
        public ScrollViewer ScrollViewer { get; set; }
        public DataTable DataTable { get; set; }
        public QueryResultView()
        {
            InitializeComponent();
            this.DataContext = new QueryResultViewModel();

            var grid = new DataGrid();
        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            ExcelExporter.TelerikControlExport(this.GridView, LanLoader.Load(LanKey.MenuQueryResult));
        }

        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            this.GridView.Print(null, Telerik.Windows.Documents.Model.PageOrientation.Landscape);
        }
        //private void InitScrollViewer()
        //{
        //    if (ScrollViewer == null)
        //    {
        //        ScrollViewer = GetVisualChild<ScrollViewer>(this.DataGrid);
        //        ScrollViewer.CanContentScroll = false;
        //    }
        //}

        //private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    e.Row.Header = e.Row.GetIndex() + 1;
        //}

        //private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    InitScrollViewer();
        //    ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - e.Delta);
        //}
        //private static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        //{
        //    T child = default(T);
        //    int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
        //    for (int i = 0; i < numVisuals; i++)
        //    {
        //        Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
        //        child = v as T;
        //        if (child == null)
        //        {
        //            child = GetVisualChild<T>(v);
        //        }
        //        if (child != null)
        //        {
        //            break;
        //        }
        //    }
        //    return child;
        //}

    }
}
