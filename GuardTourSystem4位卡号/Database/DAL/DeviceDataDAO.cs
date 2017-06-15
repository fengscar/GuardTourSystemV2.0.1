using GuardTourSystem.Model;
using GuardTourSystem.Model.Model;
using GuardTourSystem.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services.Database.DAL
{
    // 巡检设备数据, 支持  查 增 删
    public class DeviceDataDAO
    {
        public List<DevicePatrolRecord> GetAllDeviceRecord(DateTime? begin = null, DateTime? end = null)
        {
            var records = new List<DevicePatrolRecord>();

            var sql = "select * from T_DeviceData ";
            object[] paras = null;
            if (begin != null && end != null)
            {
                sql += " where Time>=@begin and Time<=@end";
                paras = new object[] { begin, end };
            }
            var ds = new DataSet();
            if (!SQLiteHelper.Instance.ExecuteDataSet(sql, paras, out ds))
            {
                return records;
            }
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                var device = item["Device"].ToString();
                var time = (DateTime)item["time"];
                var card = item["Card"].ToString();
                var readTime = (DateTime)item["TRead"]; //接收时间
                var record = new DevicePatrolRecord(device, time, card, readTime);

                records.Add(record);
            }
            return records;
        }

        public bool AddDeviceRecord(List<DevicePatrolRecord> deviceRecord)
        {
            var sql = "insert into T_DeviceData values(null,@TRead,@Device,@Time,@Card);";
            var cmds = new List<SQLiteCommand>();
            foreach (var record in deviceRecord)
            {
                var cmd = SQLiteHelper.Instance.GetCommand(sql, new object[] { record.ReadTime, record.Device, record.Time, record.Card });
                cmds.Add(cmd);
            }
            return SQLiteHelper.Instance.ExeceteNonQueryWithTransaction(cmds);
        }
        public void Init()
        {
            var sql = "delete from T_DeviceData;update sqlite_sequence set seq=0 where name='T_DeviceData';";
            SQLiteHelper.Instance.ExecuteNonQuery(sql, null);
        }
    }
}
