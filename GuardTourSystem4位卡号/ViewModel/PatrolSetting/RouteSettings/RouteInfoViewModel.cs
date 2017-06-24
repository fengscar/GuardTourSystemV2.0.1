using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GuardTourSystem.ViewModel
{
    class RouteInfoViewModel : AbstractInfoNotificationViewModel
    {
        private Route route;
        public Route Route
        {
            get { return route; }
            set
            {
                route = value;
                RaisePropertyChanged("Route");
                //SetProperty(ref this.route, value);
            }
        }
        public IRouteService RouteService { get; set; }

        public RouteInfoViewModel(Route route)
            : base()
        {
            RouteService = new RouteBLL();

            Route = route;
            if (Route == null)
            {
                Route = new Route();
                Title = LanLoader.Load(LanKey.RouteSettingAddRoute); 
                ConfirmButtonText = LanLoader.Load(LanKey.Add);
                CConfirm = new DelegateCommand(AddInfo);
            }
            else
            {
                Title = LanLoader.Load(LanKey.RouteSettingEditRoute);
                ConfirmButtonText = LanLoader.Load(LanKey.Edit);
                CConfirm = new DelegateCommand(UpdateInfo);
            }
        }

        public override void AddInfo()
        {
            //操作数据库
            int routeID;
            string errorInfo;
            if (RouteService.AddRoute(Route, out routeID, out errorInfo))
            {
                Route.ID = routeID;
                Finish();
            }
            else
            {
                ErrorInfo = errorInfo;
            }
        }

        public override void UpdateInfo()
        {
            //操作数据库
            string errorInfo;
            if (RouteService.UpdateRoute(Route, out errorInfo))
            {
                Finish(); 
            }
            else
            {
                ErrorInfo = "更新失败!";
            }
        }
    }
}
