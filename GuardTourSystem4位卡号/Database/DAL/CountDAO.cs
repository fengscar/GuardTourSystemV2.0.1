using GuardTourSystem.Database.Model;
using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.DAL
{
    class CountDAO
    {
        //public List<CountInfo> GetWorkerCountInfo(DateTime start, DateTime end)
        //{
        //    var recordSQL = "select * from T_Duty d left join T_Record r on d.[ID]=r.[DutyID] where PatrolBegin>@begin and PatrolEnd<@end ";
        //    var sql = " select workerInfo,count(1) as 'DutyCount',sum(case when PlaceTime!='NULL' then 1 else 0 end) as 'PatrolCount' "
        //             + " from ( " + recordSQL + " ) "
        //             + " group by workerInfo ";
        //    DataSet ds = new DataSet();
        //    var result = new List<CountInfo>();
        //    if (SQLiteHelper.Instance.ExecuteDataSet(sql, new object[] { start, end }, out ds))
        //    {
        //        foreach (DataRow item in ds.Tables[0].Rows)
        //        {
        //            result.Add(new CountInfo()
        //            {
        //                CountName = item["WorkerInfo"].ToString(),
        //                DutyCount = Convert.ToInt32(item["DutyCount"]),
        //                PatrolCount = Convert.ToInt32(item["PatrolCount"])
        //            });
        //        }
        //    }
        //    return result;
        //}



        /// <summary>
        /// 返回的数据格式如下 
        ///                         计划次数    实际次数
        /// RouteInfo	PlaceInfo	DutyCount	PatrolCount
        //  线路1	    地点1	    1418	    26
        //  线路1	    地点2	    1418	    26
        //  线路2	    地点3	    2451	    18
        //  线路1	    地点4	    1352	    0
        //  线路2	    地点5	    2366	    0
        //  线路2	    地点6	    2366	    0
        //  线路1	    地点7	    1352	    0
        //  线路1	    地点8	    1352	    0
        //  线路1	    地点9	    1352	    0
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        //public List<RouteCountInfo> GetRouteCountInfo(DateTime start, DateTime end)
        //{
        //    var recordSQL = "select * from T_Duty d left join T_Record r on d.[ID]=r.[DutyID] where PatrolBegin>@begin and PatrolEnd<@end ";
        //    var sql = " select "
        //            + " RouteInfo , PlaceInfo,Count(1) as 'DutyCount', Sum(case when PlaceTime!='NULL' then 1 else 0 end) as 'PatrolCount' "
        //            + " from ( " + recordSQL + " ) "
        //            + " group by PlaceInfo";
        //    DataSet ds = new DataSet();
        //    var result = new List<RouteCountInfo>();
        //    if (SQLiteHelper.Instance.ExecuteDataSet(sql, new object[] { start, end }, out ds))
        //    {
        //        foreach (DataRow item in ds.Tables[0].Rows)
        //        {
        //            //先查看是否有该线路的统计信息,如果没有,则新增一条
        //            var routeName = item["RouteInfo"].ToString();
        //            var findRoute = result.Find((route) => { return route.CountName.Equals(routeName); });
        //            if (findRoute == null)
        //            {
        //                findRoute = new RouteCountInfo() { CountName = routeName };
        //                result.Add(findRoute);
        //            }
        //            //统计该条记录的地点信息
        //            var placeCountInfo = new CountInfo()
        //            {
        //                CountName = item["PlaceInfo"].ToString(),
        //                DutyCount = Convert.ToInt32(item["DutyCount"]),
        //                PatrolCount = Convert.ToInt32(item["PatrolCount"])
        //            };
        //            findRoute.PlaceCountInfos.Add(placeCountInfo);
        //        }
        //    }
        //    foreach (var routeInfo in result)
        //    {
        //        if (routeInfo.PlaceCountInfos == null || routeInfo.PlaceCountInfos.Count == 0)
        //        {
        //            routeInfo.PatrolCount = 0;
        //            routeInfo.DutyCount = 0;
        //        }
        //        else
        //        {
        //            routeInfo.DutyCount = routeInfo.PlaceCountInfos.Sum((placeCount) => { return placeCount.DutyCount; });
        //            routeInfo.PatrolCount = routeInfo.PlaceCountInfos.Sum((placeCount) => { return placeCount.PatrolCount; });
        //        }
        //    }
        //    return result;
        //}

        //在T_RawData中进行统计
        public List<RawCountInfo> GetRowCountInfo(DateTime? start, DateTime? end)
        {
            var result = new List<RawCountInfo>();
            var sql = "select routeinfo,placeorder,placeinfo,EmployeeNumber,count(*) as RawCount from T_RawData ";
            if (start != null && end != null)
            {
                sql += " where placeTime>@Start and placeTime<@End";
            }
            sql += " group by EmployeeNumber order by routeinfo,EmployeeNumber";

            DataSet ds = new DataSet();
            if (SQLiteHelper.Instance.ExecuteDataSet(sql, new object[] { start, end }, out ds))
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    result.Add(new RawCountInfo()
                    {
                        RouteName = item["RouteInfo"].ToString(),
                        PlaceOrder = Convert.ToInt32(item["PlaceOrder"]),
                        PlaceName = item["PlaceInfo"].ToString(),
                        EmployeeNumber = item["EmployeeNumber"].ToString(),
                        Count = Convert.ToInt32(item["RawCount"])
                    });
                }
            }
            return result;
        }
    }
}
