using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GuardTourSystem.ViewModel
{
    class AppErrorViewModel : AbstractPopupNotificationViewModel
    {
        public AppErrorViewModel(string errorInfo)
        {
            this.Title = "系统错误...";
            this.ErrorInfo = errorInfo;
            this.CConfirm = new DelegateCommand(this.Cancel);
            this.CClose = new DelegateCommand(this.ShutdownApplication);
        }

        private void ShutdownApplication()// 退出程序
        {
            Application.Current.Shutdown();
        }
    }
}
