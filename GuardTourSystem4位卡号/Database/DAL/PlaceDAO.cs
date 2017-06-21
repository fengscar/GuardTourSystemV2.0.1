using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.DAL
{
    public class PlaceDAO
    {
        // 获取线路的 地点
        public List<Place> GetAllPlace(Route route)
        {
            var places = new List<Place>();
            var sqlPlace = String.Format("select * from T_Place where RouteID={0} order by RouteOrder", route.ID);
            var placeDS = new DataSet();
            if (!SQLiteHelper.Instance.ExecuteDataSet(sqlPlace, null, out placeDS))
            {
                return places;
            }
            else
            {
                foreach (DataRow p in placeDS.Tables[0].Rows)
                {
                    Place place = InitPlace(p);
                    places.Add(place);
                }
                return places;
            }
        }
        //获取所有地点
        public List<Place> GetAllPlace(string queryStr = null)
        {
            var places = new List<Place>();
            var sqlPlace = String.Format("select * from T_Place ");
            object[] param = null;
            if (!string.IsNullOrEmpty(queryStr))
            {
                sqlPlace += " where NAME Like \"%@Name%\" or Card Like \"%@Card%\" or EmployeeNumber like \"%@EN%\"";
                param = new object[] { queryStr, queryStr, queryStr };
            }
            var placeDS = new DataSet();
            if (!SQLiteHelper.Instance.ExecuteDataSet(sqlPlace, param, out placeDS))
            {
                return places;
            }
            else
            {
                foreach (DataRow p in placeDS.Tables[0].Rows)
                {
                    Place place = InitPlace(p);
                    places.Add(place);
                }
                return places;
            }
        }


        private static Place InitPlace(DataRow p)
        {
            Place place = new Place()
            {
                ID = Convert.ToInt32(p["ID"]),
                RouteID = Convert.ToInt32(p["RouteID"]),
                Order = Convert.ToInt32(p["RouteOrder"]),
                Name = p["Name"].ToString(),
                Card = p["Card"].ToString(),
                EmployeeNumber = p["EmployeeNumber"].ToString()
            };
            return place;
        }
        public Place QueryPlace(int id)
        {
            var sql = String.Format("select * from T_Place where ID={0}", id);
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
            return InitPlace(item);
        }

        public bool AddPlace(Place p, out int id, out int order)
        {
            ///弃用: SQLite连接会在执行自定义函数时断开 ,除非重构SQLiteHelper  
            /// GetNotNullCount为 SQLiteCoutomFunction中的自定义函数, 可以将null转为0 
            //string sql = String.Format("insert into T_Place(ID,RouteID,RouteOrder,Name,Card) "
            //    + " values(null,@routeID, GetMaxRouteOrder({0}),@NAME,@CARD);select last_insert_rowid();", routes.ID);

            //获取当前 RouteOrder
            var maxOrderSQL = String.Format("select Max(RouteOrder) from T_Place where routeID={0}", p.RouteID);
            var orderResult = SQLiteHelper.Instance.ExecuteScalar(maxOrderSQL, null);
            if (orderResult == null || orderResult is DBNull)
            {
                order = 1;
            }
            else
            {
                order = Convert.ToInt32(orderResult) + 1;
            }
            var sqlAdd = String.Format("insert into T_Place(ID,RouteID,RouteOrder,Name,Card,EmployeeNumber) values(null,@routeID, {0} ,@NAME,@CARD,@EN);select last_insert_rowid();", order);
            object result = SQLiteHelper.Instance.ExecuteScalar(sqlAdd, new object[] { p.RouteID, p.Name, p.Card, p.EmployeeNumber });
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

        public bool UpdatePlace(Place p)
        {
            string sql = "update T_Place set RouteID=@RouteID,RouteOrder=@Order,Name=@NAME,Card=@CARD,EmployeeNumber=@EN where Id=@ID";
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, new object[] { p.RouteID, p.Order, p.Name, p.Card, p.EmployeeNumber, p.ID }) == 1;
        }

        public bool DelPlace(Place p)
        {
            // 删除该地点,并在成功后 更新所有后续地点的RouteOrder(有触发器)
            string sql = "delete from T_Place where ID=@ID";
            if (SQLiteHelper.Instance.ExecuteNonQuery(sql, p.ID) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int GetMaxRouteOrder(int routeID)
        {
            var maxOrderSQL = String.Format("select Max(RouteOrder) from T_Place where routeID={0}", routeID); // 获取新的线路的Order
            var orderResult = SQLiteHelper.Instance.ExecuteScalar(maxOrderSQL, null);
            if (orderResult == null || orderResult is DBNull)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(orderResult);
            }

        }

        public bool ExistsName(int routeID, string name)
        {
            string sql = String.Format("select Count(*) from T_Place where RouteID=@RouteID and Name=@Name ");
            var count = Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, new object[] { routeID, name }));
            return count >= 1;
        }
        public bool ExistsCard(string card)
        {
            string sql = String.Format("select Count(*) from T_Place where Card=@Card ");
            var count = Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, card));
            return count >= 1;
        }
        public bool ExistsEmployeeNumnber(string en)
        {
            if (string.IsNullOrWhiteSpace(en))
            {
                return false;
            }
            string sql = String.Format("select Count(*) from T_Place where EmployeeNumber=@EN");
            var count = Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, en));
            return count >= 1;
        }

        public int GetRowCount()
        {
            var sql = "select count(*) from T_Place";
            return Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, null));
        }
        public void Init()
        {
            var sql = "delete from T_Place;update sqlite_sequence set seq=0 where name='T_Place';";
            SQLiteHelper.Instance.ExecuteNonQuery(sql, null);
        }
    }
}
