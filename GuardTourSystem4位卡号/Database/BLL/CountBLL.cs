using GuardTourSystem.Database.DAL;
using GuardTourSystem.Database.Model;
using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.BLL
{
    //获取统计图表的数据
    class CountBLL
    {
        public static List<CountInfo> GetWorkerCountInfo(DateTime start, DateTime end, Worker worker = null)
        {
            var list = new CountDAO().GetWorkerCountInfo(start, end);
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.CountName))
                {
                    item.CountName = "(未指定)";
                }
            }

            if (worker != null)
            {
                list = list.FindAll(countInfo => { return countInfo.CountName.Equals(worker.Name); });
            }
            else
            {
                var total = new CountInfo();
                total.CountName = "总计";
                total.DutyCount = list.Sum(item => item.DutyCount);
                total.PatrolCount = list.Sum(item => item.PatrolCount);
                list.Insert(0, total);
            }
            return list;
        }

        public static List<RouteCountInfo> GetRouteCountInfo(DateTime start, DateTime end, Route route = null)
        {
            var list = new CountDAO().GetRouteCountInfo(start, end);
            if (route != null)
            {
                list = list.FindAll(countInfo => { return countInfo.CountName.Equals(route.RouteName); });
            }
            else
            {
                var total = new RouteCountInfo();
                total.CountName = "总计";
                total.DutyCount = list.Sum(item => item.DutyCount);
                total.PatrolCount = list.Sum(item => item.PatrolCount);
                list.Insert(0, total);
            }
            foreach (var r in list)
            {
                if (r.CountName.Equals("总计"))
                {
                    continue;
                }
                var placeTotal = new CountInfo();
                placeTotal.CountName = "总计";
                placeTotal.DutyCount = r.PlaceCountInfos.Sum(item => item.DutyCount);
                placeTotal.PatrolCount = r.PlaceCountInfos.Sum(item => item.PatrolCount);
                r.PlaceCountInfos.Insert(0, placeTotal);
            }
            return list;
        }
    }
}
