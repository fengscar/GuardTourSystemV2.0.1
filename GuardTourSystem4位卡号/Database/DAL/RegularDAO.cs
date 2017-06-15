using GuardTourSystem.Model;
using GuardTourSystem.Services.Database.BLL;
using GuardTourSystem.Services.Database.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.DAL
{
    public class RegularDAO
    {
        public Regular GetRegular(Frequence frequence)
        {
            var result = new Regular(frequence);
            var sql = String.Format("select * from T_Regular where FrequenceID={0}", frequence.ID);
            DataSet ds;
            if (!SQLiteHelper.Instance.ExecuteDataSet(sql, null, out ds) || ds.Tables[0].Rows.Count == 0)
            {
                return result;
            }
            DataRow item = ds.Tables[0].Rows[0];
            result.FrequenceID = Convert.ToInt32(item["FrequenceID"]);
            bool Mon = (bool)item["Mon"];
            bool Tue = (bool)item["Tue"];
            bool Wed = (bool)item["Wed"];
            bool Thu = (bool)item["Thu"];
            bool Fri = (bool)item["Fri"];
            bool Sat = (bool)item["Sat"];
            bool Sun = (bool)item["Sun"];
            result.SetPatrol(Mon, Tue, Wed, Thu, Fri, Sat, Sun);
            return result;
        }

        public bool AddRegular(Regular r)
        {
            string sql = "insert into T_Regular(FrequenceID,Mon,Tue,Wed,Thu,Fri,Sat,Sun) values(@FreqId,@Mon,@Tue,@Wed,@Thu,@Fri,@Sat,@Sun);";
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, new object[] {r.FrequenceID,
                r.GetPatrol(DayOfWeek.Monday),r.GetPatrol(DayOfWeek.Tuesday),r.GetPatrol(DayOfWeek.Wednesday),
                r.GetPatrol(DayOfWeek.Thursday),r.GetPatrol(DayOfWeek.Friday),r.GetPatrol(DayOfWeek.Saturday),
                r.GetPatrol(DayOfWeek.Sunday)})==1;
        }
        public bool ExistsRegular(Regular r)
        {
            var sql = String.Format("select count(*) from T_Regular where FrequenceID={0}", r.FrequenceID);
            var result=SQLiteHelper.Instance.ExecuteScalar(sql, null);
            if (result == null || Convert.ToInt32(result) <= 0)
            {
                return false;
            }
            return true;
        }

        public bool UpdateRegular(Regular r)
        {
            var sql = String.Format("update T_Regular set Mon=@mon,Tue=@tue,Wed=@wed,Thu=@thu,Fri=@fri,Sat=@sat,Sun=@sun where FrequenceID={0}", r.FrequenceID);
            object[] param = new object[] {  
                r.GetPatrol(DayOfWeek.Monday),r.GetPatrol(DayOfWeek.Tuesday),r.GetPatrol(DayOfWeek.Wednesday),
                r.GetPatrol(DayOfWeek.Thursday),r.GetPatrol(DayOfWeek.Friday),r.GetPatrol(DayOfWeek.Saturday),r.GetPatrol(DayOfWeek.Sunday)};
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, param) == 1;
        }

        public bool DeleteRegular(Regular r)
        {
            throw new Exception("只有在删除Frequence时才删除Regular,并且请使用触发器删除");
        }
        public void Init()
        {
            var sql = "delete from T_Regular;update sqlite_sequence set seq=0 where name='T_Regular';";
            SQLiteHelper.Instance.ExecuteNonQuery(sql, null);
        }

    }
}
