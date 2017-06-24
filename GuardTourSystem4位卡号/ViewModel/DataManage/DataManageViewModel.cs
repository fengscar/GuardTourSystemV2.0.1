using GuardTourSystem.Database;
using GuardTourSystem.Database.BLL;
using GuardTourSystem.Database.Model;
using GuardTourSystem.Model;
using GuardTourSystem.Services.Database.BLL;
using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuardTourSystem.ViewModel
{
    class DataManageViewModel : ShowContentViewModel
    {
        public BackupBLL BLL { get; set; }
        public DelegateCommand CBackUp { get; set; } //备份当前db文件
        public DelegateCommand CRecovery { get; set; } //覆盖当前db文件
        public DelegateCommand CSelectRecoveryFile { get; set; } //选择文件进行恢复.
        //public DelegateCommand CClear { get; set; } //清空当前数据库.. 要有选项,选择清空什么表
        //public DelegateCommand CImport { get; set; } //导入 原始数据
        //public DelegateCommand CExport { get; set; } //导出 原始数据

        private DatabaseInfo dbInfo;
        public DatabaseInfo DatabaseInfo //当前数据库信息
        {
            get { return dbInfo; }
            set
            {
                dbInfo = value;
                RaisePropertyChanged("DatabaseInfo");
            }
        }

        private ObservableCollection<BackupInfo> backupInfos;
        public ObservableCollection<BackupInfo> BackupInfos //所有的备份信息
        {
            get { return backupInfos; }
            set
            {
                backupInfos = value;
                RaisePropertyChanged("BackupInfos");
            }
        }

        private BackupInfo backupInfo;
        public BackupInfo SelectBackup //当前所选的 备份信息
        {
            get { return backupInfo; }
            set
            {
                backupInfo = value;
                RaisePropertyChanged("SelectBackup");
                CRecovery.RaiseCanExecuteChanged();
            }
        }

        public DataManageViewModel()
        {
            CBackUp = new DelegateCommand(BackUp);
            CRecovery = new DelegateCommand(() => { Recovery(); }, () => { return SelectBackup != null; });
            CSelectRecoveryFile = new DelegateCommand(SelectRecoveryFile);

            InitInfo();
        }

        private void InitInfo()
        {
            BLL = new BackupBLL();
            DatabaseInfo = PatrolSQLiteManager.GetCurrentDatabaseInfo();
            BackupInfos = new ObservableCollection<BackupInfo>(BLL.GetAllBackupInfo());
        }

        //打开文件夹,并输入文件名,进行备份(文件的Copy)
        public void BackUp()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "数据库文件(*.db)|*.db";
            saveDialog.Title = "请选择存放备份文件的位置,建议不要存放在安装目录下";
            saveDialog.OverwritePrompt = false;//关闭系统默认的检查
            saveDialog.FileName = LanLoader.Load(LanKey.Backup_DefaultFileName) + DateTime.Now.ToString(" yyyy_MM_dd");

            if (DialogResult.OK == saveDialog.ShowDialog())
            {
                if (File.Exists(saveDialog.FileName))//检查文件是否存在,如果存在 进行提示
                {
                    DialogResult result = MessageBox.Show("该文件已存在,是否覆盖该次备份？", "确定", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }
                if (saveDialog.FileName.Equals(SQLiteHelper.Instance.DbFilePath))
                {
                    AppStatusViewModel.Instance.ShowError("不能覆盖当前数据库文件");
                    return;
                }
                if (File.Exists(SQLiteHelper.Instance.DbSource)) //如果数据库源文件不存在,退出
                {
                    AppStatusViewModel.Instance.ShowError("备份失败:数据库文件丢失");
                    return;
                }
                //复制数据库文件到指定位置,如果存在将覆盖
                File.Copy(SQLiteHelper.Instance.DbFilePath, saveDialog.FileName, true);
                AppStatusViewModel.Instance.ShowInfo("数据库文件 备份成功");
                //添加数据库记录
                var backupInfo = BLL.AddBackupInfo(saveDialog.FileName);
                if (backupInfo != null)
                {
                    //更新界面..
                    BackupInfos.Add(backupInfo);
                }
            }
        }

        public void Recovery(string path = null)
        {
            if (path == null)
            {
                path = SelectBackup.BackupPath;
            }
            if (!File.Exists(path))
            {
                AppStatusViewModel.Instance.ShowError("备份失败: 备份文件不存在,可能已被删除.");
                BLL.DeleteBackupInfo(path);
            }
            else if (path.Equals(SQLiteHelper.Instance.DbFilePath))
            {
                AppStatusViewModel.Instance.ShowError("请不要选择当前数据库文件");
                return;
            }
            else
            {
                File.Copy(path, SQLiteHelper.Instance.DbFilePath, true);
                AppStatusViewModel.Instance.ShowInfo("恢复成功");
            }
            InitInfo();
        }

        public void SelectRecoveryFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择先前备份的文件";
            fileDialog.Filter = "数据库文件(*.db)|*.db";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = fileDialog.FileName;
                Recovery(file);
            }
        }

    }
}
