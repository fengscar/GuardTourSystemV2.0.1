using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Settings
{
    class SettingManager
    {
        //如果不存在配置文件数据库,创建表
        public static void Init()
        {
            if (!File.Exists(ConstantSQLiteHelper.Instance.DbFilePath)) //不存在,重建表
            {
                var DAO = new SettingDAO();
                DAO.CreateTable();
                DAO.InitRoleAndUser();
            }
        }
    }
}
