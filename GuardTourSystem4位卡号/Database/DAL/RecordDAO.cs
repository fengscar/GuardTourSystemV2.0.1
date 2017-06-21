using GuardTourSystem.Model;
using GuardTourSystem.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.DAL
{
    class RecordDAO
    {
        //========================================
        //           数据库中的Record
        //========================================
        //ID	integer	                      在生成计划时赋值
        //DutyID	integer                   在生成计划时赋值
        //PlaceOrder	integer	              在生成计划时赋值
        //PlaceCard	varchar(10)	              在生成计划时赋值
        //PlaceInfo	varchar(10)               在生成计划时赋值

        //PlaceTime	DateTime	              在读取计数机数据后更新
        //ActualWorkerCard	                  在读取计数机数据后更新
        //ActualWorkerInfo	                  在读取计数机数据后更新
        //EventTime	DateTime	              在读取计数机数据后更新
        //EventCard	varchar(10)	              在读取计数机数据后更新
        //EventInfo	varchar(10)	              在读取计数机数据后更新


        //获取指定值班的所有记录
        public List<Record> GetRecord(Duty duty)
        {
            var res = new List<Record>();
            var sql = String.Format("select * from T_Record where DutyID={0}", duty.ID);
            DataSet ds;
            if (SQLiteHelper.Instance.ExecuteDataSet(sql, null, out ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    res.Add(InitRecord(row));
                }
            }
            return res;
        }
        //获取指定日期段内的所有记录
        public List<Record> GetAllRecord(DateTime start, DateTime end)
        {
            var res = new List<Record>();
            var sql = String.Format("select * from T_Record where DutyID in( select ID from T_Duty where PatrolBegin>=@start and PatrolEnd<=@end )");
            DataSet ds;
            if (SQLiteHelper.Instance.ExecuteDataSet(sql, new object[] { start, end }, out ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    res.Add(InitRecord(row));
                }
            }
            return res;
        }

        // 生成计数计划时,添加值班安排
        public bool AddRecord(Duty duty)
        {
            var sql = String.Format("insert into T_Record(ID,DutyID,PlaceOrder,PlaceCard,PlaceInfo) values(null,@dutyID,@placeOrder,@PlaceCard,@PlaceInfo)");
            foreach (var record in duty.Records)
            {
                SQLiteHelper.Instance.ExecuteNonQuery(sql, duty.ID, record.Place.Order, record.Place.Card, record.Place.Name);
            }
            return false;
        }

        // 更新值班表的 巡逻情况
        public bool UpdateRecord(List<Record> records)
        {
            var sql = "update T_Record set PlaceTime=@a,ActualWorkerCard=@b,ActualWorkerInfo=@c,EventTime=@d,EventCard=@e,EventInfo=@f  where ID=@id ";
            var cmds = new List<SQLiteCommand>();
            foreach (var record in records)
            {
                var param = record.Event == null ?
                    new object[] { record.PlaceTime, record.ActualWorker.Card, record.ActualWorker.Name, null, null, null, record.ID } :
                    new object[] { record.PlaceTime, record.ActualWorker.Card, record.ActualWorker.Name, record.EventTime, record.Event.Card, record.Event.Name, record.ID };
                cmds.Add(SQLiteHelper.Instance.GetCommand(sql, param));
            }
            return SQLiteHelper.Instance.ExeceteNonQueryWithTransaction(cmds);
        }


        private Record InitRecord(DataRow row)
        {
            var record = new Record();
            record.ID = Convert.ToInt32(row["ID"]);
            record.DutyID = Convert.ToInt32(row["DutyID"]);
            record.Place = new Place()
            {
                Order = Convert.ToInt32(row["PlaceOrder"]),
                Card = row["PlaceCard"].ToString(),
                Name = row["PlaceInfo"].ToString(),
            };
            if (!(row["PlaceTime"] is DBNull))
            {
                record.PlaceTime = Convert.ToDateTime(row["PlaceTime"]);
            }
            if (!(row["ActualWorkerCard"] is DBNull))
            {
                record.ActualWorker = new Worker()
                {
                    Card = row["ActualWorkerCard"].ToString(),
                    Name = row["ActualWorkerInfo"].ToString()
                };
            }
            if (!(row["EventCard"] is DBNull))
            {
                record.Event = new Event()
                {
                    Card = row["EventCard"].ToString(),
                    Name = row["EventInfo"].ToString()
                };
                record.EventTime = Convert.ToDateTime(row["EventTime"]);
            }
            return record;
        }

        public int GetRowCount()
        {
            var sql = "select count(*) from T_Record";
            return Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, null));
        }
        
        public void Init()
        {
            var sql = "delete from T_Record;update sqlite_sequence set seq=0 where name='T_Record';";
            SQLiteHelper.Instance.ExecuteNonQuery(sql, null);
        }
    }
}
