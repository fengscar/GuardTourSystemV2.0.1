using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.DAL
{
    // 操作无规律排班表的DAO,有以下功能
    //1. 获取一个班次(Frequence)的无规律排班(Irregular)
    //2. 新增班次的一个月份排班
    public class IrregularDAO
    {
        public Irregular GetIrregular(Frequence f)
        {
            var result = new Irregular(f);
            var sql = String.Format("select * from T_Irregular where FrequenceID={0}", f.ID);
            DataSet ds;
            if (SQLiteHelper.Instance.ExecuteDataSet(sql, null, out ds) && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var ym = (DateTime)row["YearMonth"];
                    var days = Convert.ToInt32(row["Days"]);
                    var monthPlan = new MonthPlan(ym, days);
                    result.UpdateMonthPlan(monthPlan);
                }
            }
            return result;
        }

        //新增 该班次+该月份的 记录
        public bool AddMonthPlan(Frequence f, MonthPlan mp, out int id)
        {
            id = -1;
            var sql = String.Format("insert into T_Irregular(ID,FrequenceID,YearMonth,Days) values(null,{0},@YearMonth,@Days);select last_insert_rowid();", f.ID);
            var result = SQLiteHelper.Instance.ExecuteScalar(sql, new object[] { mp.YearMonth, mp.Plan });
            if (result != null)
            {
                id = Convert.ToInt32(result);
                return true;
            }
            else
            {
                return false;
            }
        }

        // 更新 该班次+该月份的 记录
        public bool UpdateMonthPlan(Frequence f, MonthPlan mp)
        {
            var sql = String.Format("update T_Irregular set Days={0} where FrequenceID={1} and YearMonth=@yearMonth", mp.Plan, f.ID);
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, mp.YearMonth) == 1;
        }

        //是否存在 该班次+该月份的 记录
        public bool ExistsMonthPlan(Frequence f, MonthPlan mp)
        {
            var sql = String.Format("select count(*) from T_Irregular where FrequenceID={0} and YearMonth=@yearMonth", f.ID);
            var result = SQLiteHelper.Instance.ExecuteScalar(sql, mp.YearMonth);
            if (result == null || Convert.ToInt32(result) <= 0)
            {
                return false;
            }
            return true;
        }

        public void Init()
        {
            var sql = "delete from T_Irregular;update sqlite_sequence set seq=0 where name='T_Irregular';";
            SQLiteHelper.Instance.ExecuteNonQuery(sql, null);
        }
    }
}
