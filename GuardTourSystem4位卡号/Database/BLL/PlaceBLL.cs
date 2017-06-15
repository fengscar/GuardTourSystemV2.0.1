using GuardTourSystem.Database.DAL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.BLL
{
    public class PlaceBLL : IPlaceService
    {
        public PlaceDAO DAO { get; set; }
        public PlaceBLL()
        {
            DAO = new PlaceDAO();
        }
        public List<Place> GetAllPlace(Route route)
        {
            return DAO.GetAllPlace(route);
        }
        public List<Place> GetAllPlace()
        {
            return DAO.GetAllPlace();
        }


        public bool CanAdd(Place p, out string errorInfo)
        {
            errorInfo = "";
            if (!CheckPlaceProp(p, ref errorInfo))
            {
                return false;
            }
            if (DAO.ExistsName(p.RouteID, p.Name))
            {
                errorInfo = "该线路下已有同名地点";
                return false;
            }
            if (!PatrolSQLiteManager.CheckCardUnique(p.Card, ref errorInfo))
            {
                return false;
            }
            return true;
        }

        public bool AddPlace(Place p, out int id, out int routeOrder, out string errorInfo)
        {
            id = -1;
            routeOrder = -1;
            if (!CanAdd(p, out errorInfo))
            {
                return false;
            }
            return DAO.AddPlace(p, out id, out routeOrder);
        }

        public bool UpdatePlace(Place p, out string errorInfo)
        {
            errorInfo = "";
            if (!CheckPlaceProp(p, ref errorInfo))
            {
                return false;
            }
            var old = DAO.QueryPlace(p.ID);
            if (old == null)
            {
                throw new ArgumentException("未找到ID为" + p.ID + "的地点");
            }
            if (!old.Name.Equals(p.Name) && DAO.ExistsName(p.RouteID, p.Name))
            {
                errorInfo = "该线路下已有同名地点";
                return false;
            }
            if (!old.Card.Equals(p.Card))
            {
                if (!PatrolSQLiteManager.CheckCardUnique(p.Card, ref errorInfo))
                {
                    return false;
                }
            }
            if (p.RouteID != old.RouteID) //  修改了线路,需要同时修改Order
            {
                p.Order = DAO.GetMaxRouteOrder(p.RouteID) + 1;
            }
            if (!DAO.UpdatePlace(p))
            {
                return false;
            }

            //-----已经使用 BaseDAO 中的 触发器  OnPlaceRouteUpdate 实现
            //地点的线路如果改变, 要将后续线路的Order -1
            //Place.RouteID = Route.ID;
            return true;

        }

        public bool DelPlace(Place p)
        {
            return DAO.DelPlace(p);
        }
        private bool CheckPlaceProp(Place place, ref string errorInfo)
        {
            if (place.Name != null)
            {
                place.Name = place.Name.Trim();
            }
            if (String.IsNullOrEmpty(place.Name))
            {
                errorInfo = "抱歉,地点名称不能为空";
                return false;
            }
            if (String.IsNullOrEmpty(place.Card))
            {
                errorInfo = "抱歉,钮号不能为空";
                return false;
            }
            if (place.Card.Length != 4)
            {
                errorInfo = "请输入4位钮号";
                return false;
            }
            if (place.RouteID <= 0)
            {
                throw new ArgumentException("未给地点的RouteID赋值");
            }
            return true;
        }


        public void Init()
        {
            DAO.Init();
        }
    }
}
