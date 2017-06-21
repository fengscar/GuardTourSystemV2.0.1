using GuardTourSystem.Database.DAL;
using GuardTourSystem.Database.Model;
using GuardTourSystem.Model;
using GuardTourSystem.Settings;
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
        //public static List<CountInfo> GetWorkerCountInfo(DateTime start, DateTime end, Worker worker = null)
        //{
        //    var list = new CountDAO().GetWorkerCountInfo(start, end);
        //    foreach (var item in list)
        //    {
        //        if (string.IsNullOrEmpty(item.CountName))
        //        {
        //            item.CountName = "(未指定)";
        //        }
        //    }

        //    if (worker != null)
        //    {
        //        list = list.FindAll(countInfo => { return countInfo.CountName.Equals(worker.Name); });
        //    }
        //    else
        //    {
        //        var total = new CountInfo();
        //        total.CountName = "总计";
        //        total.DutyCount = list.Sum(item => item.DutyCount);
        //        total.PatrolCount = list.Sum(item => item.PatrolCount);
        //        list.Insert(0, total);
        //    }
        //    return list;
        //}

        //public static List<RouteCountInfo> GetRouteCountInfo(DateTime start, DateTime end, Route route = null)
        //{
        //    var list = new CountDAO().GetRouteCountInfo(start, end);
        //    if (route != null)
        //    {
        //        list = list.FindAll(countInfo => { return countInfo.CountName.Equals(route.RouteName); });
        //    }
        //    else
        //    {
        //        var total = new RouteCountInfo();
        //        total.CountName = "总计";
        //        total.DutyCount = list.Sum(item => item.DutyCount);
        //        total.PatrolCount = list.Sum(item => item.PatrolCount);
        //        list.Insert(0, total);
        //    }
        //    foreach (var r in list)
        //    {
        //        if (r.CountName.Equals("总计"))
        //        {
        //            continue;
        //        }
        //        var placeTotal = new CountInfo();
        //        placeTotal.CountName = "总计";
        //        placeTotal.DutyCount = r.PlaceCountInfos.Sum(item => item.DutyCount);
        //        placeTotal.PatrolCount = r.PlaceCountInfos.Sum(item => item.PatrolCount);
        //        r.PlaceCountInfos.Insert(0, placeTotal);
        //    }
        //    return list;
        //}

        public static IEnumerable<RawCountInfo> GetRawCountInfo(DateTime? start = null, DateTime? end = null, int? ignoreTime = null)
        {
            //var result = new CountDAO().GetRowCountInfo(start, end);
            //result.Where(r => string.IsNullOrEmpty(r.RouteName)).ToList().ForEach(r => { r.RouteName = "(未设置)"; r.PlaceName = "(未设置的人员)"; r.EmployeeNumber = "(未设置的人员)"; });

            var datas = new RawDataBLL().GetAllRawData(start, end);
            datas = datas.OrderBy(data => data.PlaceTime).ToList();// 按照时间排序

            var filtedData = new List<RawData>();
            if (ignoreTime != null && ignoreTime > 0)
            {
                foreach (var data in datas)
                {
                    if (!filtedData.Exists(r => data.PlaceTime.Subtract(r.PlaceTime).TotalSeconds < ignoreTime * 60 && data.Place.Card.Equals(r.Place.Card)))
                    {
                        filtedData.Add(data);
                    }
                }
            }
            else
            {
                filtedData = datas;
            }

            var result = new List<RawCountInfo>();
            foreach (var item in filtedData)
            {
                var target = result.Find(r => r.EmployeeNumber.Equals(item.Place.EmployeeNumber));
                if (target != null)
                {
                    target.Count++;
                }
                else
                {
                    var place = item.Place;
                    result.Add(new RawCountInfo() { Count = 1, EmployeeNumber = place.EmployeeNumber, PlaceName = place.Name, PlaceOrder = place.Order, RouteName = item.RouteName });
                }
            }
            return result.OrderBy(r => r.EmployeeNumber);
        }

        public bool Count(List<RawData> rawDatas)
        {
            int ignoreTime = 0;
            if (AppSetting.Default.IsIgnore && AppSetting.Default.IgnoreTime > 0)
            {
                ignoreTime = AppSetting.Default.IgnoreTime;
            }
            //获取可能出现数据重复的时间段
            var firstTime = rawDatas[0].PlaceTime.Subtract(TimeSpan.FromMinutes(ignoreTime));
            var lastTime = rawDatas[rawDatas.Count - 1].PlaceTime.Add(TimeSpan.FromMinutes(ignoreTime));
            //获取该时间段内的现有数据
            var exisitDatas = new RawDataBLL().GetAllRawData(firstTime, lastTime);
            //相加并排序
            if (exisitDatas.Count != 0)
            {
                rawDatas.AddRange(exisitDatas);
                rawDatas = rawDatas.OrderBy(raw => raw.PlaceTime).ToList();
            }


            return false;
        }
    }
}
