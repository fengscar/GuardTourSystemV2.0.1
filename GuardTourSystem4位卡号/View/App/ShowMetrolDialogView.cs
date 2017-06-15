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
using System.Windows.Interop;

namespace GuardTourSystem.View
{
    //支持显示 Metro风格的 View
    //在载入时,将初始化VM的ShowMessageDialog方法和ShowConfirmDialogAction方法
    public class ShowMetroDialogView : UserControl
    {
        public ShowMetroDialogView()
        {
            this.Loaded += ShowMetroDialogView_Loaded;
        }

        void ShowMetroDialogView_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as ShowMetroDialogViewModel;
            if (vm != null && (vm.ShowConfirmDialog == null || vm.ShowMessageDialog == null))
            {
                DBug.w("正在Loaded: " + this.DataContext);

                MainWindow mainWindow = MainWindow.Instance;
                vm.ShowMessageDialog = new Action<string, string>(mainWindow.ShowMetroMessageDialog);
                vm.ShowConfirmDialog = new Func<string, string, Task<MessageDialogResult>>(mainWindow.ShowMetroConfirmDialog);
            }
        }
    }
}
