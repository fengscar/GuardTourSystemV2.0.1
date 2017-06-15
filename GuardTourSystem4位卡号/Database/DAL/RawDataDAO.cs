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
    public class RawDataDAO
    {
        public List<RawData> GetAllRawData(DateTime? begin = null, DateTime? end = null)
        {
            var sql = "select * from T_RawData ";
            object[] param = null;
            if (begin != null && end != null)
            {
                sql += " where PlaceTime>@Begin and PlaceTime<@End ";
                param = new object[] { begin, end };
            }

            List<RawData> rawDatas = new List<RawData>();
            var ds = new DataSet();
            if (!SQLiteHelper.Instance.ExecuteDataSet(sql, param, out ds))
            {
                return rawDatas;
            };
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                //获取员工信息
                var worker = new Worker();
                worker.Card = item["WorkerCard"].ToString();
                worker.Name = item["WorkerInfo"].ToString();
                //获取地点信息
                var place = new Place();
                place.Order = Convert.ToInt32(item["PlaceOrder"]);
                place.Card = item["PlaceCard"].ToString();
                place.Name = item["PlaceInfo"].ToString();
                var placeTime = Convert.ToDateTime(item["PlaceTime"]);
                //获取事件信息
                Event evt = null;
                DateTime? eventTime = null;
                if (!(item["EventCard"] is DBNull))
                {
                    evt = new Event();
                    evt.Card = item["EventCard"].ToString();
                    evt.Name = item["EventInfo"].ToString();
                    eventTime = Convert.ToDateTime(item["EventTime"]);
                };

                //初始化一条原始数据
                var raw = new RawData()
                {
                    TRead = Convert.ToDateTime(item["TRead"]),
                    Device = item["Device"].ToString(),
                    RouteName = item["RouteInfo"].ToString(),
                };
                raw.Worker = worker;
                raw.Place = place;
                raw.PlaceTime = placeTime;
                raw.Event = evt;
                raw.EventTime = eventTime;

                rawDatas.Add(raw);
            }

            return rawDatas;
        }

        //public bool AddRawData(RawData data,out int id)
        //{
        //    id = -1;
        //    return false;
        //}

        //批量添加 使用事务
        public bool AddRawData(List<RawData> rawDatas, out string errorInfo)
        {
            errorInfo = "";
            var sql = "insert into T_RawData values(null,@Tread,@Device,@WCARD,@WNAME,@RNAME,@PORDER,@PCARD,@PTIME,@PNAME,@ECARD,@ETIME,@NAME)";

            var cmds = new List<SQLiteCommand>();
            foreach (var item in rawDatas)
            {
                var param = new object[12];
                param[0] = item.TRead;
                param[1] = item.Device;
                param[2] = item.Worker != null ? item.Worker.Card : null;
                param[3] = item.Worker != null ? item.Worker.Name : null;
                param[4] = item.RouteName;

                param[5] = item.Place != null ? item.Place.Order : 0;
                param[6] = item.Place != null ? item.Place.Card : null;
                param[7] = item.PlaceTime;
                param[8] = item.Place != null ? item.Place.Name : null;

                param[9] = item.Event != null ? item.Event.Card : null;
                param[10] = item.EventTime;
                param[11] = item.Event != null ? item.Event.Name : null;

                cmds.Add(SQLiteHelper.Instance.GetCommand(sql, param));
            }
            if (!SQLiteHelper.Instance.ExeceteNonQueryWithTransaction(cmds))
            {
                errorInfo = "插入原始数据时出错";
                return false;
            }
            return true;
        }


        public int GetRowCount()
        {
            var sql = "select Count(*) from T_RawData ";
            return Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, null));
        }

        public bool DelRawData(DateTime begin, DateTime end)
        {
            var sql = "delete from T_RawData where PlaceTime>=@begin and PlaceTime<=@end";
            SQLiteHelper.Instance.ExecuteNonQuery(sql, new object[] { begin, end });
            return true;
        }

        public void Init()
        {
            var sql = "delete from T_RawData;update sqlite_sequence set seq=0 where name='T_RawData';";
            SQLiteHelper.Instance.ExecuteNonQuery(sql, null);
        }
    }
}
