using GuardTourSystem.Model;
using GuardTourSystem.Settings;
using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GuardTourSystem.ViewModel
{
    //应用底部的ViewModel  , 有以下功能
    //1. 显示 公司信息: 当没有提示信息时, 显示当前状态( 操作员, 公司电话等)
    //2. 显示进度条和文本: 当处理耗时数据时,显示 进度条( 正在干什么 + ProgressBar + 进度文本)
    //3. 显示错误提示 : 当用户输入出错,或者需要提示信息时,要在N秒内显示 错误提示.
    //4. 显示操作完成提示: 当一个操作完成,比如保存...要在N秒内显示 保存成功..
    public class AppStatusViewModel : NotificationObject
    {
        #region 单例模式
        private static AppStatusViewModel instance = null;
        public static AppStatusViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppStatusViewModel();
                }
                return instance;
            }
        }
        #endregion

        public class BaseStatusViewModel : NotificationObject
        {
            private bool isShow;

            public bool IsShow
            {
                get { return isShow; }
                set
                {
                    isShow = value;
                    RaisePropertyChanged("IsShow");
                }
            }
        }

        public class CompanyViewModel : BaseStatusViewModel
        {
            private User user;

            public User User
            {
                get { return user; }
                set
                {
                    user = value;
                    RaisePropertyChanged("User");
                }
            }

            private string copyRight;

            public string CopyRight
            {
                get { return copyRight; }
                set
                {
                    copyRight = value;
                    RaisePropertyChanged("CopyRight");
                }
            }


            public CompanyViewModel()
            {
                this.User = MainWindowViewModel.Instance.User;
                this.CopyRight = AppSetting.Default.CopyrightInfo;
            }
        }

        public class ProgressViewModel : BaseStatusViewModel
        {
            private bool indeterminate;

            public bool Indeterminate
            {
                get { return indeterminate; }
                set
                {
                    indeterminate = value;
                    RaisePropertyChanged("Indeterminate");
                }
            }


            private string title;

            public string Title
            {
                get { return title; }
                set
                {
                    title = value;
                    RaisePropertyChanged("Title");
                }
            }
            private string progressText;

            public string ProgressText
            {
                get { return progressText; }
                set
                {
                    progressText = value;
                    RaisePropertyChanged("ProgressText");
                }
            }
            private int progress;

            public int Progress
            {
                get { return progress; }
                set
                {
                    if (value > 100 || value < 0)
                    {
                        return;
                    }
                    progress = value;
                    RaisePropertyChanged("Progress");
                }
            }


            public void Reset()
            {
                Title = "";
                ProgressText = "";
                Progress = 0;
            }
        }

        public class ErrorViewModel : BaseStatusViewModel
        {
            private string error;

            public string Error
            {
                get { return error; }
                set
                {
                    error = value;
                    RaisePropertyChanged("Error");
                }
            }
        }

        public class InfoViewModel : BaseStatusViewModel
        {
            private string info;

            public string Info
            {
                get { return info; }
                set
                {
                    info = value;
                    RaisePropertyChanged("Info");
                }
            }
        }

        public CompanyViewModel CompanyVM { get; set; }
        public ProgressViewModel ProgressVM { get; set; }
        public ErrorViewModel ErrorVM { get; set; }
        public InfoViewModel InfoVM { get; set; }

        #region 定时器
        private Timer ShowTimer { get; set; }
        private void ResetTimer(int showSecond = 5)
        {
            ShowTimer.Stop();
            ShowTimer.Close();

            if (showSecond > 0)
            {
                ShowTimer.Interval = showSecond * 1000;
                ShowTimer.Start();
            }
        }
        private void timeout_event(object source, System.Timers.ElapsedEventArgs e)
        {
            // 显示Company 
            ShowCompany();
        }
        #endregion

        public AppStatusViewModel()
        {
            // 初始化定时器
            ShowTimer = new Timer();
            ShowTimer.AutoReset = false;
            ShowTimer.Elapsed += new ElapsedEventHandler(timeout_event);

            // 初始化 ViewModel
            this.CompanyVM = new CompanyViewModel();
            this.ProgressVM = new ProgressViewModel();
            this.ErrorVM = new ErrorViewModel();
            this.InfoVM = new InfoViewModel();

            ShowCompany();
        }

        /// <summary>
        /// 显示公司名称. 将永久隐藏未完成的Progress
        /// </summary>
        public void ShowCompany()
        {
            CompanyVM.IsShow = true;
            ProgressVM.IsShow = false;
            ErrorVM.IsShow = false;
            InfoVM.IsShow = false;

            ShowTimer.Stop();
            ShowTimer.Close();
        }

        /// <summary>
        /// 在APP下方显示进度条
        /// </summary>
        /// <param name="indeterminate"></param>
        /// <param name="title"></param>
        /// <param name="expectMilsec">预计时间</param>
        public void ShowProgress(bool indeterminate, string title, int? expectMilsec = null)
        {
            CompanyVM.IsShow = false;
            ProgressVM.IsShow = true;
            ErrorVM.IsShow = false;
            InfoVM.IsShow = false;

            ResetTimer(0);
            ProgressVM.Reset();

            ProgressVM.Indeterminate = indeterminate;
            ProgressVM.Title = title;
            if (expectMilsec != null && expectMilsec > 1000)
            {
                ProgressVM.ProgressText = "预计需要 " + (double)expectMilsec / 1000 + " 秒";
            }
        }

        //以下是一个 在子线程中调用 该函数的 例子
        //          DispatcherTimer _mainTimer = new DispatcherTimer();
        //           int i = 0;
        //           _mainTimer.Interval = TimeSpan.FromMilliseconds(200);
        //           _mainTimer.Tick += new EventHandler((param, ev) => { i++; VM.ActionUpdateProgress(i+"/"+100, i); });
        //           _mainTimer.IsEnabled = true;
        public void UpdateProgress(string progressText, int curProgress, int maxProgress = 100)
        {
            ProgressVM.ProgressText = progressText;

            ProgressVM.Indeterminate = false;
            //计算实时 进度百分比
            var percent = (int)((double)curProgress * 100 / maxProgress);
            ProgressVM.Progress = percent;
        }


        /// <summary>
        /// 在指定的 时间(showSecond)内显示 错误信息(errorInfo)
        /// </summary>
        /// <param name="errorInfo"></param>
        /// <param name="showSecond"></param>
        /// <param name="hideProgress"></param>
        public void ShowError(string errorInfo, int showSecond = 10)
        {
            CompanyVM.IsShow = false;
            ProgressVM.IsShow = false;
            ErrorVM.IsShow = true;
            InfoVM.IsShow = false;

            ErrorVM.Error = errorInfo;

            ResetTimer(showSecond);

        }
        public void ShowInfo(string info, int showSecond = 5)
        {
            CompanyVM.IsShow = false;
            ProgressVM.IsShow = false;
            ErrorVM.IsShow = false;
            InfoVM.IsShow = true;

            InfoVM.Info = info;

            ResetTimer(showSecond);
        }

        public void InitUser()
        {
            this.CompanyVM.User = MainWindowViewModel.Instance.User;
        }
    }
}
