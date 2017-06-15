using GuardTourSystem.Database.DAL;
using GuardTourSystem.Database.Model;
using GuardTourSystem.Model.DAL;
using GuardTourSystem.Services.Database.BLL;
using GuardTourSystem.Services.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.BLL
{
    /// <summary>
    ///  管理备份信息
    /// </summary>
    class BackupBLL
    {
        public BackupInfoDAO DAO { get; set; }
        public BackupBLL()
        {
            DAO = new BackupInfoDAO();
        }

        public List<BackupInfo> GetAllBackupInfo()
        {
            return DAO.GetAllBackupInfo();
        }


        //备份当前数据库.
        public BackupInfo AddBackupInfo(string filePath)
        {
            var dbInfo = PatrolSQLiteManager.GetCurrentDatabaseInfo();
            var backup = new BackupInfo();
            backup.DatabaseInfo = dbInfo;
            backup.BackupDate = DateTime.Now;
            backup.BackupPath = filePath;
            DAO.DeleteBackupInfo(filePath);//如果该路径已有备份,先删除该条数据
            if (DAO.AddBackupInfo(backup))
            {
                return backup;
            }
            else
            {
                return null;
            }
        }
        public void DeleteBackupInfo(string filePath)
        {
            DAO.DeleteBackupInfo(filePath);
        }
    }
}
