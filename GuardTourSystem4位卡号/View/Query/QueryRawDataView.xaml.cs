using GuardTourSystem.Utils;
using GuardTourSystem.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace GuardTourSystem.View
{
    /// <summary>
    /// QueryRawDataView.xaml 的交互逻辑
    /// </summary>
    public partial class QueryRawDataView : ShowContentView
    {
        public QueryRawDataView()
        {
            InitializeComponent();
            this.DataContext = new QueryRawDataViewModel();
        }
        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            ExcelExporter.TelerikControlExport(this.RawDataControl.GridView, LanLoader.Load(LanKey.MenuQueryRawData));
        }
        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            this.RawDataControl.GridView.Print(null,Telerik.Windows.Documents.Model.PageOrientation.Landscape);
        }
    }
}
