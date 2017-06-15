using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.DAL
{
    class DutyDAO
    {
        /// <summary>
        /// 获取 巡逻时间与指定时间段有重叠的 Duty,将附带查询Record表的信息 (使用事务)
        /// 
        /// 注意: 返回的Duty的班次信息只有: 线路名称,班次名称,值班时间
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Duty> GetAllDuty(DateTime? begin, DateTime? end)
        {
            var RecordDAO = new RecordDAO();

            var sql = "select * from T_Duty";
            object[] param = null;
            if (begin != null && end != null)
            {
                //因为 巡检时间和指定时间段有重叠
                //所以 
                //  巡检开始时间在指定时间段内 
                //或者 
                //  巡检结束时间在指定时间段内
                //或者
                //  指定时间段被包含在某次巡检时间段之间
                sql += " where "
                      + "(PatrolBegin>=@begin and PatrolBegin<=@end ) "
                      + "or "
                      + "(PatrolEnd>=@begin2 and PatrolEnd<=@end2 ) "
                      + "or "
                      + "(PatrolBegin<=@begin3 and PatrolEnd>=@End3 )";
                param = new object[] { begin, end, begin, end, begin, end };
            }
            sql += " order by DutyDate";
            var dutys = new List<Duty>();
            DataSet ds;
            SQLiteHelper.Instance.BeginTransaction();
            if (SQLiteHelper.Instance.ExecuteDataSet(sql, param, out ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var duty = InitDuty(row);
                    duty.Records = RecordDAO.GetRecord(duty);
                    dutys.Add(duty);
                }
            };
            SQLiteHelper.Instance.CommitTransaction();
            return dutys;
        }


        private Duty InitDuty(DataRow row)
        {
            var result = new Duty();
            result.ID = Convert.ToInt32(row["ID"]);
            result.Frequence = new Frequence() { RouteName = row["RouteInfo"].ToString(), Name = row["FrequenceInfo"].ToString() };
            result.DutyDate = Convert.ToDateTime(row["DutyDate"]);
            var patrolBegin = Convert.ToDateTime(row["PatrolBegin"]);
            var patrolEnd = Convert.ToDateTime(row["PatrolEnd"]);
            result.DutyTime = new DutyTime(patrolBegin, patrolEnd);
            if (!(row["WorkerCard"] is DBNull))
            {
                result.Worker = new Worker() { Card = row["WorkerCard"].ToString(), Name = row["WorkerInfo"].ToString() };
            }
            return result;
        }

        public bool AddDuty(Duty duty, out int id)
        {
            var sql = "insert into T_Duty(ID,RouteInfo,FrequenceInfo,DutyDate,PatrolBegin,PatrolEnd,WorkerInfo,WorkerCard) "
                    + " values(null,@RouteInfo,@FrequenceInfo,@DutyDate,@PatrolBegin,@PatrolEnd,@WorkerInfo,@WorkerCard);select last_insert_rowid();";
            string workercard = duty.Worker != null ? duty.Worker.Card : null;
            string workerName = duty.Worker != null ? duty.Worker.Name : null;
            var result = SQLiteHelper.Instance.ExecuteScalar(sql,
                new object[] { duty.Frequence.RouteName, duty.Frequence.Name, duty.DutyDate, duty.DutyTime.PatrolBegin, duty.DutyTime.PatrolEnd, workerName, workercard });
            if (result == null)
            {
                id = -1;
                return false;
            }
            else
            {
                id = Convert.ToInt32(result);
                return true;
            }
        }

        /// <summary>
        /// 删除指定班次,指定时间段内的排班信息. 
        ///     ( 根据路线名称和班次名称 查找班次,所以如果班次或者线路名称改变,将无法正常删除)
        ///     ( 为了避免上述情况,在线路名称或者班次名称改变时,应该同时修改 !当天! 值班表的信息
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="dutyDateBegin"></param>
        /// <param name="dutyDateEnd"></param>
        /// <returns></returns>
        public bool DeleteDuty(Frequence freq, DateTime dutyDateBegin, DateTime dutyDateEnd)
        {
            var sql = "delete from T_Duty where RouteInfo=@Route and FrequenceInfo=@Frequence and DutyDate>=@begin and DutyDate<=@end ";
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, new object[] { freq.RouteName, freq.Name, dutyDateBegin, dutyDateEnd }) >= 1;
        }

        public int GetRowCount()
        {
            var sql = "select count(*) from T_Duty";
            return Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, null));
        }
        public void Init()
        {
            var sql = "delete from T_Duty;update sqlite_sequence set seq=0 where name='T_Duty';";
            SQLiteHelper.Instance.ExecuteNonQuery(sql, null);
        }
    }
}
