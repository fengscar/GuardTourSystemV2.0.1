using GuardTourSystem.Database.Model;
using GuardTourSystem.Model;
using GuardTourSystem.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.DAL
{
    class BackupInfoDAO
    {
        public bool AddBackupInfo(BackupInfo backupInfo)
        {
            var sql = " insert into T_Backup(ID,BackupDate,BackupPath,RouteCount,PlaceCount,RawDataCount) "
                    + " values(null,@date,@path,@RouteCount,@PlaceCount,@RawDataCount) ";
            var dbInfo = backupInfo.DatabaseInfo;
            var param = new object[] { backupInfo.BackupDate, backupInfo.BackupPath, dbInfo.RouteCount, dbInfo.PlaceCount, dbInfo.RawDataCount };
            return ConstantSQLiteHelper.Instance.ExecuteNonQuery(sql, param) == 1;
        }

        public List<BackupInfo> GetAllBackupInfo()
        {
            var sql = "select * from T_Backup";
            DataSet ds;
            var result = new List<BackupInfo>();
            if (ConstantSQLiteHelper.Instance.ExecuteDataSet(sql, null, out ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    result.Add(InitBackupInfo(row));
                }
            }
            return result;
        }

        public BackupInfo GetBackupInfo(string path)
        {
            var sql = "select * from T_Backup where BackupPath=@path";
            DataSet ds;
            if (ConstantSQLiteHelper.Instance.ExecuteDataSet(sql, new object[] { path }, out ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    return InitBackupInfo(row);
                }
            }
            return null;
        }
        public bool DeleteBackupInfo(string path)
        {
            var sql = "delete from T_Backup where BackupPath=@path";
            return ConstantSQLiteHelper.Instance.ExecuteNonQuery(sql, new object[] { path }) == 1;
        }

        private BackupInfo InitBackupInfo(DataRow row)
        {
            var info = new BackupInfo();
            info.ID = Convert.ToInt32(row["ID"]);
            info.BackupDate = (DateTime)row["BackupDate"];
            info.BackupPath = row["BackupPath"].ToString();

            var dbInfo = new DatabaseInfo();
            dbInfo.RouteCount = Convert.ToInt32(row["RouteCount"]);
            dbInfo.PlaceCount = Convert.ToInt32(row["PlaceCount"]);
            dbInfo.RawDataCount = Convert.ToInt32(row["RawDataCount"]);
            //dbInfo.WorkerCount = Convert.ToInt32(row["WorkerCount"]);
            //dbInfo.EventCount = Convert.ToInt32(row["EventCount"]);
            //dbInfo.FrequenceCount = Convert.ToInt32(row["FrequenceCount"]);
            //dbInfo.RecordCount = Convert.ToInt32(row["RecordCount"]);

            info.DatabaseInfo = dbInfo;
            return info;
        }
    }
}
