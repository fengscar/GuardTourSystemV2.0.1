using GuardTourSystem.Utils;
using GuardTourSystem.ViewModel;
using MahApps.Metro.Controls.Dialogs;
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
using Telerik.Windows.Documents.Model;
using GuardTourSystem.Print;

namespace GuardTourSystem.View
{
    /// <summary>
    /// FrequenceView.xaml 的交互逻辑
    /// </summary>
    public partial class FrequenceView : ShowContentView
    {
        public FrequenceView()
        {
            InitializeComponent();
            this.DataContext = new FrequenceViewModel();
        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            ExcelExporter.TelerikControlExport(this.GridView, LanLoader.Load(LanKey.Export_Frequence));
        }
        private void Print_Button_Click(object sender, RoutedEventArgs e)
        {
            this.GridView.Print(null, PageOrientation.Landscape);
        }
    }
}
