using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.DAL
{
    public class HitDAO
    {
        public List<DeviceHitRecord> GetAllHit(DateTime? begin=null, DateTime? end=null)
        {
            var sql = "select * from T_Hit";
            object[] param = null;
            if (begin != null && end != null)
            {
                sql += " where HitTime>@begin and HitTime<@end ";
                param = new object[] { begin, end };
            }
            sql += " order by Device ";
            var result = new List<DeviceHitRecord>();
            DataSet ds;
            if (SQLiteHelper.Instance.ExecuteDataSet(sql, param, out ds))
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    var device = item["Device"].ToString();
                    var time = (DateTime)item["HitTime"];
                    result.Add(new DeviceHitRecord(device, time));
                }
            };
            return result;
        }
        public bool AddHits(List<DeviceHitRecord> hits)
        {
            var sql = "insert into T_Hit values(null,@Device,@HitTIme)";
            var cmds = new List<SQLiteCommand>();
            foreach (var item in hits)
            {
                cmds.Add(SQLiteHelper.Instance.GetCommand(sql, new object[] { item.Device, item.Time }));
            }
            return SQLiteHelper.Instance.ExeceteNonQueryWithTransaction(cmds);
        }

        // 去掉重复 数据,只保留id最小的数据, 建议 每次添加完Hit数据都执行
        public void DistinctHits()
        {
            var sql = "delete from T_Hit where ID not in( select min(ID) from T_Hit group by device,HitTime) ";
            SQLiteHelper.Instance.ExecuteNonQuery(sql, null);
        }
    }
}
