using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
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
                SetProperty(ref this.route, value);
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
                this.Title = LanLoader.Load(LanKey.RouteSettingAddRoute); 
                ConfirmButtonText = LanLoader.Load(LanKey.Add);
                this.CConfirm = new DelegateCommand(this.AddInfo);
            }
            else
            {
                this.Title = LanLoader.Load(LanKey.RouteSettingEditRoute);
                ConfirmButtonText = LanLoader.Load(LanKey.Edit);
                this.CConfirm = new DelegateCommand(this.UpdateInfo);
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
                this.Finish();
            }
            else
            {
                this.ErrorInfo = errorInfo;
            }
        }

        public override void UpdateInfo()
        {
            //操作数据库
            string errorInfo;
            if (RouteService.UpdateRoute(Route, out errorInfo))
            {
                this.Finish(); 
            }
            else
            {
                this.ErrorInfo = "更新失败!";
            }
        }
    }
}
