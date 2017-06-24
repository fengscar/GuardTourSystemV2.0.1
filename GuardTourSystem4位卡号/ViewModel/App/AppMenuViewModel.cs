using GuardTourSystem.Model;
using GuardTourSystem.Utils;
using GuardTourSystem.View;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GuardTourSystem.ViewModel
{
    //MainWindow的主界面的ViewModel
    //包含 有:
    //  1. 菜单栏
    //  2. 快捷方式栏
    //  3. 内容控件
    class AppMenuViewModel : NotificationObject
    {
        #region 单例模式
        private static AppMenuViewModel instance { get; set; }
        public static AppMenuViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppMenuViewModel();
                }
                return instance;
            }
            private set { }
        }
        #endregion

        private AppMenuViewModel()
        {
            MainMenuItems = new ObservableCollection<MenuItem>();
            //this.InitMenu();
        }

        private ObservableCollection<MenuItem> mainMenuItems;

        public ObservableCollection<MenuItem> MainMenuItems//主目录
        {
            get { return mainMenuItems; }
            set
            {
                mainMenuItems = value;
                RaisePropertyChanged("MainMenuItems");
            }
        }


        //根据用户权限获取菜单
        public void InitMenu()
        {
            var needPermission = true;
            if (MainWindowViewModel.Instance.User != null)
            {
                needPermission = MainWindowViewModel.Instance.User.UserRole.Equals(Role.Manager);
            }

            MenuItem queryMenu = new MenuItem(LanLoader.Load(LanKey.MenuQuery), null, GetIconPath("Main_Query"));
            MenuItem inputMenu = new MenuItem(LanLoader.Load(LanKey.MenuPatrolSetting), null, GetIconPath("Main_PatrolSetting"));
            MenuItem dataMenu = new MenuItem(LanLoader.Load(LanKey.MenuDataManage), null, GetIconPath("Main_DataManage"));
            MenuItem systemMenu = new MenuItem(LanLoader.Load(LanKey.MenuSystem), null, GetIconPath("Main_System"));
            MenuItem helpMenu = new MenuItem(LanLoader.Load(LanKey.MenuHelp), null, GetIconPath("Main_Help"));
            //MenuItem textMenu = new MenuItem("测试", InitShowViewCommand(ViewEnum.Test), null);

            ObservableCollection<MenuItem> queryMenuItems = new ObservableCollection<MenuItem>();//数据查询
            ObservableCollection<MenuItem> inputMenuItems = new ObservableCollection<MenuItem>();//信息录入
            ObservableCollection<MenuItem> dataMenuItems = new ObservableCollection<MenuItem>();//数据维护
            ObservableCollection<MenuItem> systemMenuItems = new ObservableCollection<MenuItem>();//系统管理
            ObservableCollection<MenuItem> helpMenuItems = new ObservableCollection<MenuItem>();//系统帮助

            //载入数据查询菜单
            queryMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuQueryReadPatrol), InitShowViewCommand(ViewEnum.ReadPatrol), GetIconPath("ReadPatrol")));
            //queryMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuQueryReadHit), InitShowViewCommand(ViewEnum.ReadHit), GetIconPath("ReadHit")));
            queryMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuQueryRawData), InitShowViewCommand(ViewEnum.QueryRawData), GetIconPath("QueryRawData")));
            queryMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuQueryRawCount), InitShowViewCommand(ViewEnum.QueryRawCount), GetIconPath("QueryRawCount")));
            //queryMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuQueryResult), InitShowViewCommand(ViewEnum.QueryResult), GetIconPath("QueryResult")));
            //queryMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuQueryChart), InitShowViewCommand(ViewEnum.QueryChart), GetIconPath("QueryChart")));
            //queryMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuQueryReanalysis), InitPopupWindowCommand(PopupEnum.Reanalysis), GetIconPath("Reanalysis")));
            queryMenu.SubItems = queryMenuItems;
            //载入信息录入菜单
            inputMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuPatrolSettingRoute), InitShowViewCommand(ViewEnum.SetRoute), GetIconPath("SetRoute"), needPermission));
            //inputMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuPatrolSettingWorker), InitShowViewCommand(ViewEnum.SetWorker), GetIconPath("SetWorker"), needPermission));
            //inputMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuPatrolSettingEvent), InitShowViewCommand(ViewEnum.SetEvent), GetIconPath("SetEvent"), needPermission));
            //inputMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuPatrolSettingFrequence), InitShowViewCommand(ViewEnum.SetFrequence), GetIconPath("SetFrequence"), needPermission));
            //inputMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuPatrolSettingRegular), InitShowViewCommand(ViewEnum.SetRegular), GetIconPath("SetRegular"), needPermission));
            //inputMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuPatrolSettingIrregular), InitShowViewCommand(ViewEnum.SetIrregular), GetIconPath("SetIrregular"), needPermission));
            inputMenu.SubItems = inputMenuItems;
            //载入数据维护菜单
            dataMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuDataManageBackupAndRecovery), InitShowViewCommand(ViewEnum.DataManage), GetIconPath("DataManage")));
            dataMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuDataManageClearPatrolData), InitPopupWindowCommand(PopupEnum.ClearPatrolData), GetIconPath("ClearPatrolData"), needPermission));
            //dataMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuDataManageImportPatrolData), InitPopupWindowCommand(PopupEnum.ImportPatrolData), GetIconPath("ImportPatrolData")));
            //dataMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuDataManageExportPatrolData), InitPopupWindowCommand(PopupEnum.ExportPatrolData), GetIconPath("ExportPatrolData")));
            dataMenu.SubItems = dataMenuItems;
            //载入系统管理菜单
            systemMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuSystemInit), InitPopupWindowCommand(PopupEnum.SystemInit), GetIconPath("SystemInit"), needPermission));
            //systemMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuSystemUserManage), InitPopupWindowCommand(PopupEnum.ManageUser), GetIconPath("UserManage"), needPermission));
            systemMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuSystemModifyPassword), InitPopupWindowCommand(PopupEnum.ChangePassword), GetIconPath("ChangePassword")));
            //systemMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuSystemLanguage), InitPopupWindowCommand(PopupEnum.Language), GetIconPath("Language")));
            systemMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuSystemDeviceTest), InitPopupWindowCommand(PopupEnum.DeviceTest), GetIconPath("DeviceTest")));
            systemMenuItems.Add(new MenuItem("忽略重复卡", InitPopupWindowCommand(PopupEnum.IgnoreRepeat), GetIconPath("Reanalysis")));
            //systemMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuSystemLog), InitShowViewCommand(ViewEnum.SystemLog), GetIconPath("SystemLog"), needPermission));
            systemMenu.SubItems = systemMenuItems;
            //载入帮助菜单
            helpMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuHelpHowToUse), InitPopupWindowCommand(PopupEnum.Help), GetIconPath("Help")));
            //helpMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuHelpHowToStart), InitPopupWindowCommand(PopupEnum.HowToStart), GetIconPath("HowToStart")));
            helpMenuItems.Add(new MenuItem(LanLoader.Load(LanKey.MenuHelpAboutUs), InitPopupWindowCommand(PopupEnum.AboutUs), GetIconPath("AboutUs")));
            helpMenu.SubItems = helpMenuItems;

            MainMenuItems.Clear();
            MainMenuItems.Add(queryMenu);
            MainMenuItems.Add(inputMenu);
            MainMenuItems.Add(dataMenu);
            MainMenuItems.Add(systemMenu);
            MainMenuItems.Add(helpMenu);
            //MainMenuItems.Add(textMenu);
        }
        //返回一个改变当前 界面显示路径的 Command
        private DelegateCommand InitShowViewCommand(ViewEnum viewEnum)
        {
            return new DelegateCommand(() =>
               {
                   AppContentViewModel.Instance.ShowView(viewEnum);
               });
        }
        //返回一个弹窗的 Command
        private DelegateCommand InitPopupWindowCommand(PopupEnum popupEnum)
        {
            return new DelegateCommand(() =>
            {
                AppContentViewModel.Instance.PopupWindow(popupEnum);
            });
        }
        private string GetIconPath(string fileName)
        {
            return "/Resource/Img/MenuIcon/" + fileName + ".png";
        }

        public class MenuItem
        {
            public MenuItem() { SubItems = new ObservableCollection<MenuItem>(); }
            public string Text { get; set; }
            public Uri IconUrl { get; set; }
            public ObservableCollection<MenuItem> SubItems { get; set; }
            public DelegateCommand ClickCommand { get; set; }
            public bool Enable { get; set; }

            /// <summary>
            /// </summary>
            /// <param name="text">菜单文本</param>
            /// <param name="cmd">点击该菜单的操作</param>
            /// <param name="iconPath">icon路径</param>
            public MenuItem(string text, DelegateCommand cmd = null, string iconPath = null, bool enable = true)
            {
                Text = text;
                if (iconPath != null)
                {
                    IconUrl = new Uri(iconPath, UriKind.Relative);
                }
                if (cmd != null)
                {
                    ClickCommand = cmd;
                }
                Enable = enable;
            }
            //public void SetClickCommand(DelegateCommand cmd)
            //{
            //    this.ClickCommand = cmd;
            //}
        }
    }

}
