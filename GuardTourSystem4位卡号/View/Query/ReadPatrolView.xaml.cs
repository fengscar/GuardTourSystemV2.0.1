using GuardTourSystem.Print;
using GuardTourSystem.Utils;
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

namespace GuardTourSystem.View
{
    /// <summary>
    /// ReadDataView.xaml 的交互逻辑
    /// </summary>
    public partial class ReadPatrolView : ShowContentView
    {
        public ReadPatrolView()
        {
            InitializeComponent();
            this.DataContext =new ReadPatrolViewModel();
        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            ExcelExporter.TelerikControlExport(this.RawDataControl.GridView, LanLoader.Load(LanKey.MenuQueryReadPatrol));
        }
        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            this.RawDataControl.GridView.Print(null, Telerik.Windows.Documents.Model.PageOrientation.Landscape);
        }
    }
}
