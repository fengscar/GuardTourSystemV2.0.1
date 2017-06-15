using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GuardTourSystem.View.Custom
{
    //扩展方法 , 只要引用了该命名空间的UserControl即可使用notifyPropertyChagned来通知属性变更
    public static class ExtendNotifyChanged
    {
        public static event PropertyChangedEventHandler PropertyChanged;

        public static void ExtendNotify(this UserControl control, string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(control, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
