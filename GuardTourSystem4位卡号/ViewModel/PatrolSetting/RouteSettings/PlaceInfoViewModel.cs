using GuardTourSystem.Database.BLL;
using GuardTourSystem.Database.DAL;
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
                place = value;
                RaisePropertyChanged("Place");
            }
        }


        public List<Route> Routes { get; set; } // 作为ComboBox的选项

        private Route route;

        public Route Route  //当前选中的Route
        {
            get { return route; }
            set
            {
                route = value;
                RaisePropertyChanged("Route");
            }
        }


        public IPlaceService DataService { get; set; }

        public PlaceInfoViewModel(List<Route> routes, Place place)
            : base()
        {
            DataService = new PlaceBLL();
            Routes = routes;

            Place = place;
            //传NULL,表示 添加地点
            if (Place == null)
            {
                Route = Routes[0];

                Title = "新增人员";
                Place = new Place() { ID = -1 };
                ConfirmButtonText = LanLoader.Load(LanKey.Add);
                CConfirm = new DelegateCommand(new Action(AddInfo));
            }
            // 更新人员
            else
            {
                Route = Routes.Find(r => { return r.ID == place.RouteID; });

                Title = "编辑人员信息";
                ConfirmButtonText = LanLoader.Load(LanKey.Edit);
                CConfirm = new DelegateCommand(new Action(UpdateInfo));
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
                Finish();
            }
            else
            {
                ErrorInfo = errorInfo;
            }
        }
        public void CheckEmployeeNumnber(string employeeNumber)
        {
            //string 
            //if (string.IsNullOrWhiteSpace(employeeNumber)){
                
            //}
            //new PlaceDAO().ExistsEmployeeNumnber();
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
                Finish();
            }
            else
            {
                ErrorInfo = errorInfo;
            }
        }
    }
}
