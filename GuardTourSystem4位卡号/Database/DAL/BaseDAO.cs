using GuardTourSystem.Model;
using GuardTourSystem.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services.Database.DAL
{
    class BaseDAO
    {
        private const string LOG = "BaseDAO";
        /// <summary>
        /// 首次创建数据库时
        /// 1.  创建所有表 
        /// 2.  向角色表添加系统管理员和操作员
        /// 3.  向用户表添加默认的系统管理员和操作员账户.
        /// </summary>
        public void CreateTable()
        {

            string create_worker = "CREATE TABLE IF NOT EXISTS T_Worker("
                                    + "ID integer primary key autoincrement,"
                                    + "Name varchar(20) not null,"
                                    + "Card varchar(4) not null,"
                                    //+ "IsDeviceCard boolean default '0',"
                //+ "Device varchar(10) ,"
                //+ "Deleted boolean default '0'," //默认为 未删除...
                //+ "Unique(Card),Unique(Device))";
                                    + "Unique(Card))";

            string create_route = "CREATE TABLE IF NOT EXISTS T_Route("
                                    + "ID integer primary key autoincrement,"
                                    + "Name varchar(30) not null,"
                //+ "Deleted boolean default '0'," //默认为 未删除...
                                    + "Unique(Name))";

            string create_place = "CREATE TABLE IF NOT EXISTS T_Place ("
                                    + "ID integer primary key autoincrement,"
                                    + "RouteID Integer not null References T_Route(ID) ,"
                                    + "RouteOrder Integer not null,"
                                    + "Name varchar(30) not null,"
                                    + "Card varchar(4) not null)";
            //+ "Deleted boolean default '0')";//默认为 未删除...


            string create_event = "CREATE TABLE IF NOT EXISTS T_Event("
                                    + "ID integer primary key autoincrement,"
                                    + "Name varchar(30) not null,"
                                    + "Card varchar(4) not null,"
                                    + "Unique(Card))";


            // 从机器读上来的数据: 只有 设备号,时间,钮号
            string create_deviceData = "CREATE TABLE IF NOT EXISTS T_DeviceData("
                                    + "ID integer primary key autoincrement,"
                                    + "TRead DateTime not null,"
                                    + "Device varchar(10) not null,"
                                    + "Time DateTime not null,"
                                    + "Card varchar(4) not null)";

            //由 deviceData+ worker+ event + place 生成的原始数据
            // 为何要保存而不是实时生成: 当worker/event/place改变后, 数据不会改变(除非被重新生成)
            string create_rawData = "CREATE TABLE IF NOT EXISTS T_RawData("
                                    + "ID integer primary key autoincrement,"
                                    + "TRead DateTime not null,"
                                    + "Device varchar(2) not null,"
                                    + "WorkerCard varchar(4) not null,"
                                    + "WorkerInfo varchar(10) not null,"
                                    + "RouteInfo varchar(10),"
                                    + "PlaceOrder integer,"
                                    + "PlaceCard varchar(4) not null,"
                                    + "PlaceTime DateTime not null,"
                                    + "PlaceInfo varchar(16)," //不是not null,因为无法识别的卡都将作为地点卡
                                    + "EventCard varchar(4),"
                                    + "EventTime DateTime,"
                                    + "EventInfo varchar(10))";


            //具体的值班表 (精确到 每个班次,每个地点)
            string create_duty = "CREATE TABLE IF NOT EXISTS T_Duty("
                                + "ID integer primary key autoincrement,"
                                + "RouteInfo varchar(10) not null,"
                                + "FrequenceInfo varchar(10) not null,"
                // 该次值班的所属日期:
                //      由于有的班次可能是午夜12点之后开始.所以记录下该班次实际日期.方便删除..
                //      比如今天的排班改变了,要重新生成.
                //      重新生成前要先删掉所有今天已有的班次..
                //      这个字段将作为删除条件..
                //      无法从PatrolBegin和PatrolEnd获取,因为类似午夜1点到午夜三点的值班记录将无法删除.
                                + "DutyDate DateTime not null,"
                                + "PatrolBegin DateTime not null,"
                                + "PatrolEnd DateTime not null,"
                                + "WorkerInfo varchar(10),"
                                + "WorkerCard varchar(4))";


            string create_record = "CREATE TABLE IF NOT EXISTS T_Record("
                                + "ID integer primary key autoincrement,"
                                + "DutyID integer not null References T_Duty(ID),"
                                + "PlaceOrder integer not null,"
                                + "PlaceCard varchar(4) not null,"
                                + "PlaceInfo varchar(10) not null,"
                                + "PlaceTime DateTime ," //读卡时间
                                + "ActualWorkerCard varchar(4) ,"
                                + "ActualWorkerInfo varchar(5) ,"
                                + "EventCard varchar(4)," //事件可以为空
                                + "EventInfo varchar(10),"
                                + "EventTime DateTime,"
                                + "Unique(DutyID,PlaceOrder))";


            string create_Frequence = "CREATE TABLE IF NOT EXISTS T_Frequence("
                                    + "ID integer primary key autoincrement,"
                                    + "RouteID integer  not null References T_Route(ID),"
                                    + "Name varchar(10) not null,"
                                    + "StartDate DateTime not null,"
                                    + "EndDate DateTime,"
                                    + "GeneratedDate DateTime not null,"
                                    + "StartTime integer not null,"
                                    + "EndTime integer not null,"
                                    + "PatrolTime integer not null," //多少分钟..
                                    + "RestTime integer not null,"
                                    + "PatrolCount integer not null,"
                                    + "IsRegular boolean not null)";

            string create_regular = "CREATE TABLE IF NOT EXISTS T_Regular("
                                    + "FrequenceID integer  primary key References T_Frequence(ID) ,"
                                    + "Mon boolean not null,"
                                    + "Tue boolean not null,"
                                    + "Wed boolean not null,"
                                    + "Thu boolean not null,"
                                    + "Fri boolean not null,"
                                    + "Sat boolean not null,"
                                    + "Sun boolean not null)";

            string create_irregular = "CREATE TABLE IF NOT EXISTS T_Irregular("
                                    + "ID integer primary key autoincrement,"
                                    + "FrequenceID integer  not null References T_Frequence(ID),"
                                    + "YearMonth DateTime not null,"
                                    + "Days integer not null,"
                                    + "Unique(FrequenceID,Yearmonth))";

            string create_FrequenceWorker = "CREATE TABLE IF NOT EXISTS T_FrequenceWorker("
                                    + "FrequenceID integer primary key References T_Frequence(ID),"
                                    + "WorkerID integer  not null References T_Worker(ID),"
                                    + "Unique(FrequenceID,WorkerID))";

            string create_hit = "CREATE TABLE IF NOT EXISTS T_Hit("
                                    + "ID integer primary key autoincrement,"
                                    + "Device varchar(10) not null,"
                                    + "HitTime DateTime Not null)";




            List<SQLiteCommand> cmds = new List<SQLiteCommand>();

            cmds.Add(new SQLiteCommand(create_worker));
            cmds.Add(new SQLiteCommand(create_event));
            cmds.Add(new SQLiteCommand(create_route));
            cmds.Add(new SQLiteCommand(create_place));

            cmds.Add(new SQLiteCommand(create_deviceData));
            cmds.Add(new SQLiteCommand(create_rawData));
            cmds.Add(new SQLiteCommand(create_duty));
            cmds.Add(new SQLiteCommand(create_record));


            cmds.Add(new SQLiteCommand(create_Frequence));
            cmds.Add(new SQLiteCommand(create_regular));
            cmds.Add(new SQLiteCommand(create_irregular));
            cmds.Add(new SQLiteCommand(create_FrequenceWorker));

            cmds.Add(new SQLiteCommand(create_hit));

            try
            {
                SQLiteHelper.Instance.ExeceteNonQueryWithTransaction(cmds);
            }
            catch (Exception e)
            {
                DBug.w("创建表", e);
                throw;
            }
        }
        public void CreateTrigger()
        {
            //删除员工后, 在T_FrequenceWorker中删除相应的记录
            var delWorkerTrigger = "create trigger OnWorkerDelete after delete on T_Worker "
                + "For Each Row "
                + "Begin "
                + "    delete from T_FrequenceWorker where WorkerID==OLD.ID; "
                + "End";


            //删除 线路后,删除该线路下的所有地点
            //删除 线路后,删除该线路下的所有排班
            var delRouteTriggerSQL = "create trigger OnRouteDelete after delete on T_Route "
                + "For Each Row "
                + "Begin "
                + "   delete from T_Place where RouteID==OLD.ID; "
                + "   delete from T_Frequence where RouteID=OLD.ID; "
                + "End";

            //删除 排班后,删除该排班下的所有 Irregular/Regular
            // 删除 FrequenceWorker 中对应的记录
            var delFrequenceTriggerSQL = "create trigger OnFrequenceDelete after delete on T_Frequence "
                + "For Each Row "
                + "Begin "
                + "   delete from T_Regular where FrequenceID==OLD.ID; "
                + "   delete from T_Irregular where FrequenceID==OLD.ID; "
                + "   delete from T_FrequenceWorker where FrequenceID==OLD.ID; "
                + "End";

            // 删除地点后 将后续地点的Order-1
            var delPlaceTriggerSQL = "create trigger OnPlaceDelete after delete on T_Place "
                + "For Each Row "
                + "Begin "
                + "   update T_Place set RouteOrder=RouteOrder-1 where RouteID=OLD.RouteID and RouteOrder>OLD.RouteOrder;"
                + "End";

            //删除 值班表后, 删除相应的记录
            var delDutyTriggerSQL = "create trigger OnDutyDelete after delete on T_Duty "
                + "For Each Row "
                + "Begin "
                + "    delete from T_Record where DutyID==OLD.ID; "
                + "End";

            //当地点的路线改变时,将该地点原来所在路线的后续地点 Order往前移1 
            var updatePlaceTriggerSQL = "create trigger OnPlaceRouteUpdate after update on T_Place when old.RouteID!=new.RouteID "
                //+"For Each Row " //如果上面提供了When字句,不能再使用ForEachRow
                + "Begin "
                + "     update T_Place set RouteOrder=RouteOrder-1 where RouteID=OLD.RouteID and RouteOrder>OLD.RouteOrder;"
                + "End";


            SQLiteHelper.Instance.ExecuteNonQuery(delWorkerTrigger, null);
            SQLiteHelper.Instance.ExecuteNonQuery(delRouteTriggerSQL, null);
            SQLiteHelper.Instance.ExecuteNonQuery(delFrequenceTriggerSQL, null);
            SQLiteHelper.Instance.ExecuteNonQuery(delPlaceTriggerSQL, null);
            SQLiteHelper.Instance.ExecuteNonQuery(delDutyTriggerSQL, null);
            SQLiteHelper.Instance.ExecuteNonQuery(updatePlaceTriggerSQL, null);
        }


       

        // 获取表数量,每次应用启动后 判断是否是第一次使用...
        public int GetTableCount()
        {
            var sql = "select count(*) from sqlite_master where type=\"table\"";
            var count = Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, null));
            return count;
        }

        public static void OpenPRAGMA()
        {
            //无效
        }

        /// <summary>
        ///删除所有表(除了用户表、角色表)
        /// </summary>
        public void DeleteTable()
        {
            string get_all_table_name = "select NAME from sqlite_master where TYPE=@TABLE";
            var ds = new DataSet();
            if (!SQLiteHelper.Instance.ExecuteDataSet(get_all_table_name, new object[] { "table" }, out ds))
            {
                return;
            }

            List<SQLiteCommand> cmds = new List<SQLiteCommand>();
            foreach (System.Data.DataRow item in ds.Tables[0].Rows)
            {
                string tableName = item["NAME"].ToString();
                if (tableName.Equals("T_User") || tableName.Equals("T_Role") || tableName.Equals("sqlite_sequence"))
                {
                    continue;
                }
                string sql = "drop table " + tableName;
                cmds.Add(new SQLiteCommand(sql));
            }
            SQLiteHelper.Instance.ExeceteNonQueryWithTransaction(cmds);
        }
    }
}
