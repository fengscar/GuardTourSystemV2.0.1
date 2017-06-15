using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// RegularSelectControl.xaml 的交互逻辑
    /// </summary>
    public partial class RegularSelectControl : UserControl
    {
        public RegularSelectControl()
        {
            InitializeComponent();
        }


        public event EventHandler CheckChanged;

        void Check_Changed(object sender, RoutedEventArgs e)
        {
            CheckChanged.BeginInvoke(null, null, null, null);
        }


        //private void UserControl_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var grid = VisualTreeHelper.GetChild(this, 0);
        //    var c = VisualTreeHelper.GetChildrenCount(grid);
        //    var uniformgrid = VisualTreeHelper.GetChild(grid, 0) as UniformGrid;
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(uniformgrid); i++)
        //    {
        //        var checkBox = VisualTreeHelper.GetChild(uniformgrid, i) as CheckBox;
        //        checkBox.Checked += Check_Changed;
        //        checkBox.Unchecked += Check_Changed;
        //    }
        //}
    }
}
