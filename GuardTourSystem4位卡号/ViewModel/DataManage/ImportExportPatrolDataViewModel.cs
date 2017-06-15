using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Services.Database.BLL;
using GuardTourSystem.Services.Database.DAL;
using GuardTourSystem.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuardTourSystem.ViewModel.DataManage
{
    class ImportExportPatrolDataViewModel
    {
        public RawDataBLL RawDataBLL { get; set; }
        public ImportExportPatrolDataViewModel()
        {
            RawDataBLL = new RawDataBLL();
        }

        /// <summary>
        /// 导入巡检数据
        /// 1. 打开对话框让用户选择文件.
        /// 2. 将该文件作为数据库文件并打开
        /// 3. 判断是否有巡检数据
        /// 4. 获取巡检数据
        /// 5. 重新打开PatrolDATA数据库,向其写入巡检数据,并同时进行分析(更新值班表)
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Import(out string error)
        {
            error = null;
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择要导入的巡检数据文件";
            fileDialog.Filter = "巡检数据文件(*.db)|*.db";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = fileDialog.FileName;
                if (path.Equals(SQLiteHelper.Instance.DbFilePath))
                {
                    error = "请不要选择当前数据库文件";
                    return false;
                }
                var readResult = SQLiteHelper.Instance.OperateOtherDatabase(path,
                    () =>
                    {
                        return new DeviceDataDAO().GetAllDeviceRecord();
                    });
                if (readResult == null || (readResult as List<DevicePatrolRecord>) == null)
                {
                    error = "读取巡检数据时出错,请确认导入的文件是否正确.";
                    return false;
                }
                var datas = readResult as List<DevicePatrolRecord>;
                if (datas.Count == 0)
                {
                    error = "要导入的文件中不存在巡检数据";
                    return false;
                }
                new ReadPatrolViewModel().HandleDeviceData(datas);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 导出巡检数据
        /// 1.如果没有巡检数据,退出并提示错误
        /// 2.如果文件已存在,提示是否覆盖
        /// 3.不能覆盖当前数据库文件
        /// 4.删除该文件,并新建一个DB数据库.创建所有表
        /// 5.向T_DeviceData写入数据
        /// </summary>
        public bool Export(out string error)
        {
            error = null;
            var records = RawDataBLL.GetAllDeviceRecord();
            if (records.Count == 0)
            {
                error = "系统中没有巡检数据";
                return false;
            }
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "巡检数据文件(*.db)|*.db";
            saveDialog.Title = "请选择导出的位置";
            saveDialog.OverwritePrompt = false;//关闭系统默认的检查
            saveDialog.FileName = LanLoader.Load(LanKey.ExportPatrol_DefaultFileName) + DateTime.Now.ToString(" yyyy_MM_dd");

            if (DialogResult.OK == saveDialog.ShowDialog())
            {
                var filePath = saveDialog.FileName;
                if (File.Exists(filePath))//检查文件是否存在,如果存在 进行提示
                {
                    DialogResult result = MessageBox.Show("该文件已存在,是否覆盖该次备份？", "确定", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        error = "取消导出巡检数据";
                        return false;
                    }
                }
                if (filePath.Equals(SQLiteHelper.Instance.DbFilePath))
                {
                    error = "不能覆盖当前数据库文件";
                    return false;
                }

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                var databaseResult = SQLiteHelper.Instance.OperateOtherDatabase(filePath,
                    () =>
                    {
                        PatrolSQLiteManager.Init();//在新的位置打开数据库并创建表
                        //写入数据文件
                        return new DeviceDataDAO().AddDeviceRecord(records);
                    });
                if (!(bool)databaseResult)
                {
                    error = "写入数据文件时出错";
                    return false;
                }
                //操作成功
                FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(filePath);
                FileInfo file = new FileInfo(filePath);
                return true;
            }
            return false;
        }
    }
}
