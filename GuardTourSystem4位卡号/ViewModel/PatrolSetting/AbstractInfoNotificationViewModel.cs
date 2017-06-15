using GuardTourSystem.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GuardTourSystem.ViewModel
{
    //巡检信息设置 弹出窗口的 ViewModel ,新增了2个必须实现的方法
    public abstract class AbstractInfoNotificationViewModel : AbstractPopupNotificationViewModel
    {
        public abstract void AddInfo();
        public abstract void UpdateInfo();
    }
}
