using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Settings
{
    class SettingDAO
    {
        public void CreateTable()
        {
            string create_role = "CREATE TABLE IF NOT EXISTS T_Role("
                              + "ID integer primary key,"
                              + "Role varchar(20) not null unique);";

            string create_user = "CREATE TABLE IF NOT EXISTS T_User("
                                    + "ID integer primary key autoincrement,"
                                    + "RoleID integer not null References T_Role(ID) unique,"
                                    + "Password varchar(12) not null )";

            string create_log = "CREATE TABLE IF NOT EXISTS T_LOG("
                              + "ID integer primary key autoincrement,"
                              + "Name varchar(20) not null,"
                              + "LogText varchar(100) not null, "
                              + "LogTime datetime  default current_timestamp)";

            string create_backup = "CREATE TABLE IF NOT EXISTS T_Backup("
                                 + "ID integer primary key autoincrement,"
                                 + "BackupDate DateTime  not null,"
                                 + "BackupPath varchar(100) not null unique, "
                                 //+ "WorkerCount integer not null,"
                                 + "RouteCount integer not null,"
                                 + "PlaceCount integer not null,"
                //+ "EventCount integer not null,"
                //+ "FrequenceCount integer not null,"
                                 + "RawDataCount integer not null)";
                                 //+ "RecordCount integer not null)";

            ConstantSQLiteHelper.Instance.ExecuteNonQuery(create_role);
            ConstantSQLiteHelper.Instance.ExecuteNonQuery(create_user);
            ConstantSQLiteHelper.Instance.ExecuteNonQuery(create_backup);
            ConstantSQLiteHelper.Instance.ExecuteNonQuery(create_log);
        }

        public void InitRoleAndUser()
        {
            // 添加两个固定角色 , 如果不分开写,好像会出错..
            string init_role_admin = "INSERT INTO T_Role(ID,Role) values(0,\"Admin\");";
            string init_role_opera = "INSERT INTO T_Role(ID,Role) values(1,\"Operator\");";

            string init_user_manager = "INSERT INTO T_User(Password,RoleID) values(@pwd,0)";
            string init_user_opera = "INSERT INTO T_User(Password,RoleID) values(@pwd,1)";

            ConstantSQLiteHelper.Instance.ExecuteNonQuery(init_role_admin);
            ConstantSQLiteHelper.Instance.ExecuteNonQuery(init_role_opera);
            ConstantSQLiteHelper.Instance.ExecuteNonQuery(init_user_manager, new object[] { User.DEF_PWD_ADMIN });
            ConstantSQLiteHelper.Instance.ExecuteNonQuery(init_user_opera, new object[] { User.DEF_PWD_OPERATOR });
        }
    }
}
