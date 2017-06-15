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
using GuardTourSystem.Print;
using GuardTourSystem.Utils;

namespace GuardTourSystem.View.Query.ChartControls
{
    /// <summary>
    /// WorkerGridControl.xaml 的交互逻辑
    /// </summary>
    public partial class WorkerGridControl : UserControl, IPrintable
    {
        public WorkerGridControl()
        {
            InitializeComponent();
        }

        public void PrintView()
        {
            this.WorkerGridView.Print();
        }

        public void ExportExcel()
        {
            ExcelExporter.TelerikControlExport(this.WorkerGridView, LanLoader.Load(LanKey.WorkerCountInfo));
        }
    }
}
