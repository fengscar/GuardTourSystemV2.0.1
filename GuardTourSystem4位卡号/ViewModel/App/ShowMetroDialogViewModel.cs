using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.ViewModel
{
    //所有支持显示 Metro Dialog 的ViewModel
    public class ShowMetroDialogViewModel : NotificationObject
    {
        /// <summary>
        /// 显示Metro对话框 ,必须在主线程调用,因为View中调用了GetWindow();
        /// 第一个参数为标题,第二个参数为具体说明
        /// </summary>
        public Action<string, string> ShowMessageDialog { get; set; }
        //显示确认对话框
        public Func<string, string, Task<MessageDialogResult>> ShowConfirmDialog { get; set; }
    }
}
