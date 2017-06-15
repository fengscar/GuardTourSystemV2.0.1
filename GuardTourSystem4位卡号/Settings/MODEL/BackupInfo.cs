using GuardTourSystem.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database
{
    public class BackupInfo
    {
        public int ID { get; set; }
        public DateTime BackupDate { get; set; }
        public string BackupPath { get; set; } //备份文件的存放位置

        public DatabaseInfo DatabaseInfo { get; set; }
    }
}
