using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.ViewModel
{
    class PlaceInfoViewModel : AbstractInfoNotificationViewModel
    {
        private Place place;

        public Place Place
        {
            get { return place; }
            set
            {
                SetProperty(ref this.place, value);
            }
        }
        public List<Route> Routes { get; set; } // 作为ComboBox的选项

        private Route route;

        public Route Route  //当前选中的Route
        {
            get { return route; }
            set
            {
                SetProperty(ref this.route, value);
            }
        }


        public IPlaceService DataService { get; set; }

        public PlaceInfoViewModel(List<Route> routes, Place place)
            : base()
        {
            this.DataService = new PlaceBLL();
            Routes = routes;

            Place = place;
            //传NULL,表示 添加地点
            if (Place == null)
            {
                Route = Routes[0];

                Title = "新增地点";
                Place = new Place() { ID = -1 };
                ConfirmButtonText = LanLoader.Load(LanKey.Add);
                this.CConfirm = new DelegateCommand(new Action(this.AddInfo));
            }
            // 更新人员
            else
            {
                Route = Routes.Find(r => { return r.ID == place.RouteID; });

                Title = "编辑地点信息";
                ConfirmButtonText = LanLoader.Load(LanKey.Edit);
                this.CConfirm = new DelegateCommand(new Action(this.UpdateInfo));
            }
        }
        public override void AddInfo()
        {
            int placeID,routeOrder;
            string errorInfo;
            Place.RouteID = Route.ID;
            if (DataService.AddPlace(Place, out placeID, out routeOrder,out errorInfo))
            {
                Place.ID = placeID;
                Place.Order = routeOrder;
                this.Finish();
            }
            else
            {
                this.ErrorInfo = errorInfo;
            }
        }

        /// <summary>
        /// 如果修改了线路,要另外做处理
        /// </summary>
        public override void UpdateInfo()
        {
            string errorInfo;
            Place.RouteID = Route.ID;
            if (DataService.UpdatePlace(Place, out errorInfo)) // 更新地点到新线路 
            {
                this.Finish();
            }
            else
            {
                this.ErrorInfo = errorInfo;
            }
        }
    }
}
