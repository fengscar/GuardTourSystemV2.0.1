using GuardTourSystem.Model.Settings;
using GuardTourSystem.Utils;
using GuardTourSystem.Properties;
using GuardTourSystem.Settings;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GuardTourSystem.ViewModel
{
    class IgnoreRepeatViewModel : AbstractPopupNotificationViewModel
    {
        private bool isIgnore;
        public bool IsIgnore
        {
            get { return isIgnore; }
            set
            {
                isIgnore = value;
                RaisePropertyChanged("IsIgnore");
                //SetProperty(ref this.isIgnore, value);
            }
        }
        private int ignoreTime;
        public int IgnoreTime
        {
            get { return ignoreTime; }
            set
            {
                ignoreTime = value;
                RaisePropertyChanged("IgnoreTime");
                //SetProperty(ref this.ignoreTime, value);
            }
        }

        /// <summary>
        /// Constractor
        /// </summary>
        public IgnoreRepeatViewModel()
        {
            Title = "忽略重复打卡记录";
            ConfirmButtonText = "确定";
            this.IsIgnore = AppSetting.Default.IsIgnore;
            this.IgnoreTime = AppSetting.Default.IgnoreTime;
            // 初始化语言菜单栏
            this.CConfirm = new DelegateCommand(this.Ignore);
        }

        private void Ignore()
        {
            if (IsIgnore)
            {
                if (IgnoreTime < 0)
                {
                    ErrorInfo = "输入的时间要大于等于0";
                    return;
                }
            }
            AppSetting.Default.IsIgnore = IsIgnore;
            AppSetting.Default.IgnoreTime = IgnoreTime;
            AppSetting.Default.Save();
            this.Finish();
        }

    }
}
