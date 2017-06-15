using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.BLL
{
    public class RouteBLL : IRouteService
    {
        public RouteDAO DAO { get; set; }
        public RouteBLL()
        {
            DAO = new RouteDAO();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attachPlaces"> 是否附带添加属于 该Route的 Places</param>
        /// <param name="attachFrequences">是否附带添加属于 该ROute 的Frequences</param>
        /// <returns></returns>
        public List<Route> GetAllRoute(bool attachPlaces = true, bool attachFrequences = true)
        {
            SQLiteHelper.Instance.BeginTransaction();
            var routes = DAO.GetAllRoute();
            if (attachPlaces)
            {
                IPlaceService ps = new PlaceBLL();
                foreach (var route in routes)
                {
                    var places = new ObservableCollection<Place>(ps.GetAllPlace(route));
                    route.Places = places;
                }
            }
            if (attachFrequences)
            {
                IFrequenceService fs = new FrequenceBLL();
                foreach (var route in routes)
                {
                    var freqs = fs.GetAllFrequence(route);
                    route.Frequences = freqs;
                }
            }
            SQLiteHelper.Instance.CommitTransaction();
            return routes;
        }


        public bool AddRoute(Route route, out int id, out string errorInfo)
        {
            errorInfo = "";
            id = -1;
            if (!CheckRouteProp(route, ref errorInfo))
            {
                return false;
            }
            if (DAO.ExistsName(route.RouteName))
            {
                errorInfo = "该线路名称已存在";
                return false;
            }
            return DAO.AddRoute(route, out id);
        }

        public bool UpdateRoute(Route route, out string errorInfo)
        {
            errorInfo = "";
            if (!CheckRouteProp(route, ref errorInfo))
            {
                return false;
            }
            var old = DAO.QueryRoute(route.ID);
            if (!old.RouteName.Equals(route.RouteName)) //如果姓名变更,判断新名称是否已经存在
            {
                if (DAO.ExistsName(route.RouteName))
                {
                    errorInfo = "该线路名称已存在";
                    return false;
                }
                //更新前先获取班次...避免班次的路线名称被改变 导致无法找到指定DUTY
                var oldFrequences = new FrequenceBLL().GetAllFrequence(old);

                if (DAO.UpdateRoute(route))
                {
                    //更新成功,重新生成该班次当天的Duty
                    var bll = new DutyBLL();
                    //更新值班表
                    foreach (var freq in oldFrequences)
                    {
                        bll.GenerateDuty(out errorInfo, freq, DateTime.Now);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else //如果线路名称没变,无需更新
            {
                return true;
            }
        }

        public bool DelRoute(Route route)
        {
            return DAO.DelRoute(route);
        }
        private bool CheckRouteProp(Route route, ref string errorInfo)
        {
            if (route.RouteName != null)
            {
                route.RouteName = route.RouteName.Trim();
            }
            if (String.IsNullOrEmpty(route.RouteName))
            {
                errorInfo = "抱歉,线路名称不能为空";
                return false;
            }
            return true;
        }

        public void Init()
        {
            DAO.Init();
        }
    }
}
