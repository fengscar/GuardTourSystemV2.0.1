using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services.Database.DAL
{
    public class RouteDAO
    {
        private const string LOG = "RouteDAO";

        public List<Route> GetAllRoute()
        {
            var sqlroute = "select * from T_Route order by ID";

            List<Route> routes = new List<Route>();
            var routeDS = new DataSet();
            if (!SQLiteHelper.Instance.ExecuteDataSet(sqlroute, null, out routeDS))
            {
                return routes;
            };

            foreach (DataRow item in routeDS.Tables[0].Rows)
            {
                Route route = new Route();
                route.ID = Convert.ToInt32(item["ID"]);
                route.RouteName = item["Name"].ToString();

                routes.Add(route);
            }
            return routes;
        }
        public Route QueryRoute(int routeID)
        {
            var sql = String.Format("select * from T_Route where ID={0}", routeID);
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
            Route route = new Route();
            route.ID = Convert.ToInt32(item["ID"]);
            route.RouteName = item["Name"].ToString();
            return route;
        }

        public bool AddRoute(Route route, out int id)
        {
            var sql = "insert into T_Route(ID,NAME) values(null,@NAME);select last_insert_rowid();";
            var result = SQLiteHelper.Instance.ExecuteScalar(sql, new object[] { route.RouteName });
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

        public bool UpdateRoute(Route route)
        {
            string sql = "update T_Route set Name=@Name where Id=@ID";
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, new object[] { route.RouteName, route.ID }) == 1;
        }
        public bool DelRoute(Route route)
        {
            string sql = "delete from T_Route where ID=@ID";
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, new object[] { route.ID }) == 1;
        }
        public bool ExistsName(string name)
        {
            string sql = String.Format("select Count(*) from T_Route where Name=@Name ");
            var count = Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, name));
            return count >= 1;
        }

        public int GetRowCount()
        {
            var sql = "select count(*) from T_Route";
            return Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, null));
        }

        public void Init()
        {
            var sql = "delete from T_Route;update sqlite_sequence set seq=0 where name='T_Route';";
            SQLiteHelper.Instance.ExecuteNonQuery(sql, null);
        }
    }
}
