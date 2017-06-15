using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services.Database.DAL
{
    public class FrequenceDAO
    {
        private Frequence InitFrequence(DataRow item)
        {
            var result = new Frequence()
            {
                ID = Convert.ToInt32(item["ID"]),
                RouteID = Convert.ToInt32(item["RouteID"]),
                Name = item["Name"].ToString(),
                StartDate = (DateTime)item["StartDate"],
                GeneratedDate = (DateTime)item["GeneratedDate"],
                StartTime = TimeSpan.FromMinutes(Convert.ToInt32(item["StartTime"])),
                EndTime = TimeSpan.FromMinutes(Convert.ToInt32(item["EndTime"])),
                PatrolTime = Convert.ToInt32(item["PatrolTime"]),
                RestTime = Convert.ToInt32(item["RestTime"]),
                PatrolCount = Convert.ToInt32(item["PatrolCount"]),
                IsRegular = (bool)item["IsRegular"]
            };
            if (!item.IsNull("EndDate"))
            {
                result.EndDate = (DateTime)item["EndDate"];
            }
            return result;
        }
        public List<Frequence> GetAllFrequence(Route route)
        {
            var frequences = new List<Frequence>();
            var sql = String.Format("select * from T_Frequence where RouteID={0}", route.ID);
            DataSet ds;
            if (SQLiteHelper.Instance.ExecuteDataSet(sql, null, out ds))
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    frequences.Add(InitFrequence(item));
                }
            }
            return frequences;
        }
        public Frequence QueryFrequence(int id)
        {
            var sql = String.Format("select * from T_Frequence where ID={0}", id);
            var ds = new DataSet();
            if (!SQLiteHelper.Instance.ExecuteDataSet(sql, null, out ds))
            {
                return null;
            }
            if (ds.Tables[0].Rows.Count != 1)
            {
                return null;
            }
            DataRow item = ds.Tables[0].Rows[0];
            return InitFrequence(item);
        }

        public bool AddFrequence(Frequence f, out int id)
        {
            var sql = "insert into T_Frequence(ID,RouteID,Name,StartDate,EndDate,GeneratedDate,StartTime,EndTime,PatrolTime,RestTime,PatrolCount,IsRegular) "
                                    + "values(null,@RouteID,@Name,@StartFreq,@EndFreq,@GeneratedDate,@StartTime,@EndTime,@PatrolTime,@RestTime,@PatrolCount,@IsRegular);"
                                    + "select last_insert_rowid();";
            var result = SQLiteHelper.Instance.ExecuteScalar(sql, new object[] { 
                        f.RouteID, f.Name,f.StartDate,f.EndDate, f.GeneratedDate, 
                        ((TimeSpan)f.StartTime).TotalMinutes, ((TimeSpan)f.EndTime).TotalMinutes, 
                        f.PatrolTime, f.RestTime, f.PatrolCount, f.IsRegular });
            if (result != null)
            {
                id = Convert.ToInt32(result);
                return true;
            }
            else
            {
                id = -1;
                return false;
            }
        }


        public bool UpdateFrequence(Frequence f)
        {
            string sql = "update T_Frequence set "
                    + "RouteID=@routeID,Name=@name,StartDate=@StartFreq,EndDate=@EndFreq,GeneratedDate=@generatedDate,StartTime=@startWork,EndTime=@endWork, "
                    + "PatrolTime=@patrolTime,RestTime=@RestTime,PatrolCount=@patrolCountDS,IsRegular=@isregular "
                    + "where Id=@ID";
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, new object[] {
                          f.RouteID, f.Name,f.StartDate,f.EndDate,f.GeneratedDate,
                         ((TimeSpan)f.StartTime).TotalMinutes, ((TimeSpan)f.EndTime).TotalMinutes, 
                          f.PatrolTime, f.RestTime, f.PatrolCount, f.IsRegular ,   
                          f.ID}) == 1;
        }

        // 将 生成日期 修改为指定日期( 只允许改大..)
        public bool UpdateFrequenceGenerated(DateTime date)
        {
            var sql = "update T_Frequence set GeneratedDate=@newDate where GeneratedDate<@newDate";
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, new object[] { date, date }) >= 0;
        }

        public bool DeleteFrequence(Frequence f)
        {
            var sql = String.Format("delete from T_Frequence where ID={0}", f.ID);
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, null) == 1;
        }
        public bool ExistsName(int routeID, string frequenceName)
        {
            string sql = String.Format("select Count(*) from T_Frequence where RouteID=@ID and Name=@Name ");
            var count = Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, new object[] { routeID, frequenceName }));
            return count >= 1;
        }

        public int GetRowCount()
        {
            var sql = "select count(*) from T_Frequence";
            return Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, null));
        }
        public void Init()
        {
            var sql = "delete from T_Frequence;update sqlite_sequence set seq=0 where name='T_Frequence';";
            SQLiteHelper.Instance.ExecuteNonQuery(sql, null);
        }
    }
}
