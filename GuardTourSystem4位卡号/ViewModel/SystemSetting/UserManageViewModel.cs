using GuardTourSystem.Model.Settings;
using GuardTourSystem.Utils;
using GuardTourSystem.Properties;
using GuardTourSystem.Settings;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
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
    class UserManageViewModel : AbstractPopupNotificationViewModel
    {
        private string newPwd1;
        public string NewPassword1
        {
            get { return newPwd1; }
            set
            {
                SetProperty(ref this.newPwd1, value);
            }
        }

        private string newPwd2;
        public string NewPassword2
        {
            get { return newPwd2; }
            set
            {
                SetProperty(ref this.newPwd2, value);
            }
        }

        /// <summary>
        /// Constractor
        /// </summary>
        public UserManageViewModel()
        {
            Title = "操作员密码修改";
            ConfirmButtonText = "确定";
            // 初始化语言菜单栏
            this.CConfirm = new DelegateCommand(ChangePassword);
        }

        private void ChangePassword()
        {
            // 基本输入验证
            if (string.IsNullOrEmpty(NewPassword1) )
            {
                ErrorInfo = "请输入新密码";
                return;
            }
            if (string.IsNullOrEmpty(NewPassword2) || !NewPassword2.Equals(NewPassword1))
            {
                ErrorInfo = "两次输入的密码不一致.";
                return;
            }
            // 验证密码
            var bll=new UserBLL();
            //var CurrentUserName = AppSetting.Default.LoginUser;
            string error;
            var oper=bll.GetUser((int)Role.Operator);
            if (oper == null)
            {
                throw new Exception("未找到指定用户");
            }
            oper.Password = NewPassword1;
            if (bll.UpdateUser(oper, out error))
            {
                this.Finish();
            }
            else
            {
                ErrorInfo = error;
            }
        }
    }
}
