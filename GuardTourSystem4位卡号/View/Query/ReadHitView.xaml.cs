using GuardTourSystem.Utils;
using GuardTourSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// ReadHitView.xaml 的交互逻辑
    /// </summary>
    public partial class ReadHitView : ShowContentView
    {
        public ReadHitView()
        {
            InitializeComponent();
            this.DataContext = new ReadHitViewModel();
        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            ExcelExporter.TelerikControlExport(this.GridView, LanLoader.Load(LanKey.MenuQueryReadHit));
        }
        //private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    e.Row.Header = e.Row.GetIndex() + 1;
        //}

    }
}
