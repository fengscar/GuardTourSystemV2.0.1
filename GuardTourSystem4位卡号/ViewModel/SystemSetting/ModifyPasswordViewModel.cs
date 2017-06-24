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
using GuardTourSystem.Services.Database.BLL;
using GuardTourSystem.Model;

namespace GuardTourSystem.ViewModel
{
    class ModifyPasswordViewModel : AbstractPopupNotificationViewModel
    {
        private string oldpwd;

        public string OldPassword
        {
            get { return oldpwd; }
            set
            {
                oldpwd = value;
                RaisePropertyChanged("OldPassword");
                //SetProperty(ref this.oldpwd, value);
            }
        }

        private string newPwd1;

        public string NewPassword1
        {
            get { return newPwd1; }
            set
            {
                newPwd1 = value;
                RaisePropertyChanged("NewPassword1");
                //SetProperty(ref this.newPwd1, value);
            }
        }

        private string newPwd2;

        public string NewPassword2
        {
            get { return newPwd2; }
            set
            {
                newPwd2 = value;
                RaisePropertyChanged("NewPassword2");
                //SetProperty(ref this.newPwd2, value);
            }
        }

        /// <summary>
        /// Constractor
        /// </summary>
        public ModifyPasswordViewModel()
        {
            Title = "修改密码";
            ConfirmButtonText = "确定";
            // 初始化语言菜单栏
            CConfirm = new DelegateCommand(ChangePassword);
        }

        private void ChangePassword()
        {
            // 基本输入验证
            if (string.IsNullOrEmpty(OldPassword))
            {
                ErrorInfo = "请输入原始密码";
                return;
            }
            if (string.IsNullOrEmpty(NewPassword1))
            {
                ErrorInfo = "请输入新密码";
                return;
            }
            if (string.IsNullOrEmpty(NewPassword2) )
            {
                ErrorInfo = "请再次输入新密码";
                return;
            }
            if (!NewPassword1.Equals(NewPassword2))
            {
                ErrorInfo = "输入的新密码不相同";
                return;
            }
            // 验证密码
            var bll=new UserBLL();
            //var CurrentUserName = AppSetting.Default.LoginUser;
            var CurrentUserRole = MainWindowViewModel.Instance.User.UserRole;
            string error;
            if (bll.UpdatePassword(CurrentUserRole, OldPassword, NewPassword1, out error))
            {
                Finish();
            }
            else
            {
                ErrorInfo = error;
            }
        }

        
    }
}
