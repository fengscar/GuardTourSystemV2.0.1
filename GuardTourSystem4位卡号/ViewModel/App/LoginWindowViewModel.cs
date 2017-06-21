using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.BLL;
using GuardTourSystem.Utils;
using GuardTourSystem.View;
using GuardTourSystem.Settings;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GuardTourSystem.ViewModel
{
    class LoginWindowViewModel : NotificationObject
    {
        public static readonly LoginWindowViewModel Instance = new LoginWindowViewModel();

        #region properties
        //登录窗口的 名称
        private string loginTitle;
        public string LoginTitle
        {
            get { return loginTitle; }
            set
            {
                loginTitle = value;
                RaisePropertyChanged("LoginTitle");
            }
        }


        private List<User> userList;
        public List<User> UserList
        {
            get { return userList; }
            set
            {
                userList = value;
                RaisePropertyChanged("UserList");
            }
        }

        // 当前选择的用户
        private User user;
        public User User
        {
            get { return user; }
            set
            {
                ErrorInfo = null;
                user = value;
                RaisePropertyChanged("User");
            }
        }

        //用户输入的密码
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                ErrorInfo = null;
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        //错误提示
        private string errorInfo;
        public string ErrorInfo
        {
            get { return errorInfo; }
            set
            {
                errorInfo = value;
                RaisePropertyChanged("ErrorInfo");
            }
        }

        //使用默认密码的 提示
        public string ToolTip { get; set; }
        #endregion

        // 用户登录...
        public DelegateCommand CLogin { get; private set; }
        // 取消( 退出) 
        public DelegateCommand CQuit { get; private set; }
        // 输入默认密码
        public DelegateCommand CDefaultPassword { get; private set; }


        private LoginWindowViewModel()
        {
            //配置数据库初始化
            SettingManager.Init();

            LoadUsersData();

            LoginTitle = LanLoader.Load(LanKey.LOGIN_WINDOW);
            ToolTip = LanLoader.Load(LanKey.LoginWindowPasswordTootTip, User.DEF_PWD_ADMIN, User.DEF_PWD_OPERATOR);
            //ToolTip = "获取当前用户默认密码." + "\n\n" + "管理员的默认密码为 " + User.DEF_PWD_ADMIN + "\n" + "操作员的默认密码为 " + User.DEF_PWD_OPERATOR;

            // Commands
            this.CLogin = new DelegateCommand(this.Login);
            this.CQuit = new DelegateCommand(this.Quit);
            this.CDefaultPassword = new DelegateCommand(this.DefaultPassword);
        }


        //在密码栏 填入 默认密码
        private void DefaultPassword()
        {
            switch (this.User.UserRole)
            {
                case Role.Manager:
                    this.Password = User.DEF_PWD_ADMIN;
                    break;
                case Role.Operator:
                    this.Password = User.DEF_PWD_OPERATOR;
                    break;
            }
        }


        private void Login()
        {
            if (string.IsNullOrEmpty(this.Password))
            {
                ErrorInfo = LanLoader.Load(LanKey.LoginWindowErrorEmptyPassword);
                return;
            }
            if (!this.User.Password.Equals(this.Password))
            {
                ErrorInfo = LanLoader.Load(LanKey.LoginWindowErrorWrongPassword);
                return;
            }
            //验证通过,保存信息
            AppSetting.Default.LoginUser = this.User.UserRole == Role.Manager ? 0 : 1;
            AppSetting.Default.Save();

            //关闭登录窗口,并打开 主界面
            MainWindowViewModel.Instance.ShowMainWindow(this.User);
            AppMenuViewModel.Instance.InitMenu();
            AppStatusViewModel.Instance.InitUser();
            this.CloseLoginWindow();
        }

        private void Quit()
        {
            Application.Current.Shutdown();
        }

        private void LoadUsersData()
        {
            // 载入 已有用戶的数据
            IUserService ds = new UserBLL();
            this.UserList = ds.GetAllUser();

            // 载入上次登录的用户
            var lastuser = AppSetting.Default.LoginUser;
            foreach (var item in UserList)
            {
                if ((int)item.UserRole == lastuser)
                {
                    User = item;
                }
            }
            // 如果没有找到 上次登录的用户
            if (User == null)
            {
                User = UserList[0];
            }
        }

        public void CloseLoginWindow()
        {
            LoginWindow.Instance.Close();
        }
    }
}
