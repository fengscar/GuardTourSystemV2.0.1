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
using MahApps.Metro.Controls.Dialogs;
using GuardTourSystem.Database.BLL;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace GuardTourSystem.ViewModel
{
    class SharePatrolDataViewModel : AbstractPopupNotificationViewModel
    {
        private static SharePatrolDataViewModel instance;
        public static SharePatrolDataViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SharePatrolDataViewModel();
                }
                return instance;
            }
        }


        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                SetProperty(ref this.username, value);
                AppSetting.Default.ShareUsername = value;
                AppSetting.Default.Save();
            }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                SetProperty(ref this.password, value);
                AppSetting.Default.SharePassword = value;
                AppSetting.Default.Save();
            }
        }


        private string shareFilePath;
        public string ShareFilePath
        {
            get { return shareFilePath; }
            set
            {
                SetProperty(ref this.shareFilePath, value);
                this.CConfirm.RaiseCanExecuteChanged();
            }
        }

        private bool autoShare;
        public bool AutoShare
        {
            get { return autoShare; }
            set
            {
                SetProperty(ref this.autoShare, value);
                AppSetting.Default.AutoShare = value;
                AppSetting.Default.Save();
            }
        }

        public DelegateCommand CSelectShareFilePath { get; set; }//选择共享文件夹的位置

        private SharePatrolDataViewModel()
        {
            // 初始化语言菜单栏
            Title = "共享巡检数据";
            ConfirmButtonText = "上传";
            this.CConfirm = new DelegateCommand(() => { this.Share(); }, this.CheckFilePath);
            this.CSelectShareFilePath = new DelegateCommand(this.SelectShareFilePath);

            //Init data
            this.AutoShare = AppSetting.Default.AutoShare;
            this.ShareFilePath = AppSetting.Default.ShareFilePath;
            this.Username = AppSetting.Default.ShareUsername;
            this.Password = AppSetting.Default.SharePassword;

            //Init Default error Action
            this.DefaultErrorAction = OnError;
        }


        private void SelectShareFilePath()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "巡检数据文件(*.db)|*.db";
            saveDialog.Title = "请选择巡检数据的共享位置,并输入文件名";
            saveDialog.OverwritePrompt = false;//关闭系统默认的检查
            if (DialogResult.OK == saveDialog.ShowDialog())
            {
                ShareFilePath = saveDialog.FileName;
            }
        }
        private Action<string> DefaultErrorAction { get; set; }
        private void OnError(string error)
        {
            System.Windows.MessageBox.Show(error, "巡检数据上传失败", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        //外部调用
        public void SharePatrolData(Action<string> onError = null)
        {
            if (!this.AutoShare)
            {
                return;
            }
            DBug.w("开始共享巡检数据");
            if (onError != null)
            {
                DefaultErrorAction += onError;
            }
            //需要重新判断共享文件夹路径是否合法
            if (!CheckFilePath())
            {
                DefaultErrorAction(this.ErrorInfo);
                return;
            }
            //开始上传
            var error = Share();
            if (error != null)
            {
                DefaultErrorAction(error);
                return;
            }
        }
        /// <summary>
        /// 立即上传到指定位置..(复制数据库文件到指定位置)
        /// </summary>
        private string Share()
        {
            this.ErrorInfo = null;

            if (!File.Exists(SQLiteHelper.Instance.DbFilePath))
            {
                this.ErrorInfo = "上传失败:本地文件丢失。";
                return this.ErrorInfo;
            }

            //连接共享文件夹,准备上传数据
            var folder = Path.GetDirectoryName(ShareFilePath);
            if (ConnectShareFolder(folder, Username, Password))
            {

                SQLiteHelper.Instance.MotifyPassword(null); //取消数据库密码
                File.Copy(SQLiteHelper.Instance.DbFilePath, ShareFilePath, true);
                AppStatusViewModel.Instance.ShowInfo("巡检数据上传成功");
                SQLiteHelper.Instance.MotifyPassword("041225");//重新设置数据库密码
                this.Finish();
                return null;
            }
            else
            {
                this.ErrorInfo = "无法连接到共享文件夹,请确认路径、用户名、密码是否正确。";
                return this.ErrorInfo;
            }
        }

        /// <summary>
        /// 烦死了,草. 只能在SaveDialog中选择路径,就不要判断这么多
        /// </summary>
        /// <returns></returns>
        private bool CheckFilePath()
        {
            this.ErrorInfo = null;
            if (ShareFilePath == null)
            {
                this.ErrorInfo = "请选择巡检数据的共享位置";
                return false;
            }
            //判断 扩展名
            try
            {
                //if (!".db".Equals(Path.GetExtension(ShareFilePath)))
                //{
                //    this.ErrorInfo = "请输入正确的共享文件格式: (扩展名为.db)";
                //    return false;
                //};
                var dir = Path.GetDirectoryName(ShareFilePath);
                if (!Directory.Exists(dir))
                {
                    this.ErrorInfo = "该路径不存在";
                    return false;
                }
                //var InvalidFileChars = Path.GetInvalidFileNameChars();
                //foreach (var item in Path.GetFileNameWithoutExtension(ShareFilePath))
                //{
                //    if (InvalidFileChars.Contains(item))
                //    {
                //        this.ErrorInfo = "文件名不能包含下列字符:" + InvalidFileChars;
                //        return false;
                //    }
                //}
            }
            catch (Exception)
            {
                this.ErrorInfo = "请选择正确的共享位置";
                return false;
            }

            AppSetting.Default.ShareFilePath = ShareFilePath;
            AppSetting.Default.Save();
            return true;
        }

        /// <summary>
        /// 判断能否连接到指定的文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        private bool ConnectShareFolder(string path, string userName = null, string passWord = null)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    userName = "admin";
                }
                if (string.IsNullOrEmpty(password))
                {
                    passWord = "123456";
                }
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                //要先断开所有连接,否则会提示 "不允许一个用户使用一个以上用户名与一个服务器或共享资源的多重连接 "
                string closeConnect = "net use * /del /y";
                proc.StandardInput.WriteLine(closeConnect);
                string dosLine = "net use " + path + " /User:" + userName + " " + passWord + " /PERSISTENT:YES";
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                    throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                DBug.w(ex);
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="src">本地</param>
        /// <param name="dst">共享文件夹</param>
        /// <returns></returns>
        //public string Transport(string src, string dst)
        //{
        //    try
        //    {
        //        FileStream inFileStream = new FileStream(src, FileMode.Open);
        //        if (!Directory.Exists(dst))
        //        {
        //            Directory.CreateDirectory(dst);
        //        }
        //        FileStream outFileStream = new FileStream(dst, FileMode.Create);

        //        byte[] buf = new byte[inFileStream.Length];

        //        int byteCount;

        //        while ((byteCount = inFileStream.Read(buf, 0, buf.Length)) > 0)
        //        {

        //            outFileStream.Write(buf, 0, byteCount);

        //        }
        //        inFileStream.Flush();

        //        inFileStream.Close();

        //        outFileStream.Flush();

        //        outFileStream.Close();
        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Error("上传到共享文件夹失败", e);
        //        return e.ToString();
        //    }
        //}
    }
}
