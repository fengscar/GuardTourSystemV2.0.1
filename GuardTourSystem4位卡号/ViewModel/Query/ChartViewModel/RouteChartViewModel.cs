using GuardTourSystem.Database.Model;
using GuardTourSystem.Model;
using GuardTourSystem.Print;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.ViewModel.Query.ChartViewModel
{
    class RouteChartViewModel : BindableBase
    {
        //表格的ViewModel就使用本类,方便一点,不用再新建一个类
        private ObservableCollection<RouteCountInfo> routeCountInfos;
        public ObservableCollection<RouteCountInfo> RouteCountInfos
        {
            get { return routeCountInfos; }
            set
            {
                SetProperty(ref this.routeCountInfos, value);
            }
        }
        private RouteCountInfo selectRoute;
        public RouteCountInfo SelectRoute
        {
            get { return selectRoute; }
            set
            {
                SetProperty(ref this.selectRoute, value);
                if (value == null)
                {
                    return;
                }
                Places = new ObservableCollection<CountInfo>(value.PlaceCountInfos);
                if (value.CountName.Equals("总计"))
                {
                    SelectPlace = null;
                }
                else
                {
                    SelectPlace = SelectRoute.PlaceCountInfos.First();
                }
                this.PieViewModel.InitPie(value);
                this.BarViewModel.InitBar(value);
            }
        }


        private ObservableCollection<CountInfo> places;
        public ObservableCollection<CountInfo> Places
        {
            get { return places; }
            set
            {
                SetProperty(ref this.places, value);
            }
        }

        private CountInfo selectPlace;
        public CountInfo SelectPlace
        {
            get { return selectPlace; }
            set
            {
                SetProperty(ref this.selectPlace, value);
                ShowPlaceSelector = value != null;
                if (value != null)
                {
                    this.PieViewModel.InitPie(value);
                    this.BarViewModel.InitBar(value);
                }
            }
        }

        private bool showPlaceSelector;
        public bool ShowPlaceSelector
        {
            get { return showPlaceSelector; }
            set
            {
                SetProperty(ref this.showPlaceSelector, value);
            }
        }



        private RouteBarViewModel barViewModel;
        public RouteBarViewModel BarViewModel
        {
            get { return barViewModel; }
            set
            {
                SetProperty(ref this.barViewModel, value);
            }
        }

        private RoutePieViewModel pieViewModel;
        public RoutePieViewModel PieViewModel
        {
            get { return pieViewModel; }
            set
            {
                SetProperty(ref this.pieViewModel, value);
            }
        }


        public RouteChartViewModel(List<RouteCountInfo> countInfos)
        {
            this.PieViewModel = new RoutePieViewModel();
            this.BarViewModel = new RouteBarViewModel();

            this.Init(countInfos);
        }

        public void Init(List<RouteCountInfo> infos)
        {
            this.RouteCountInfos = new ObservableCollection<RouteCountInfo>(infos);
            if (infos != null)
            {
                SelectRoute = infos.First();
            }
        }
        public void Print()
        {
            var printData = new PrintData() { ContentList = RouteCountInfos.ToList(), Title = "路线统计信息", DataCount = RouteCountInfos.Count };
            var printer = new Printer(new RouteGridChartDocument(printData));
            printer.ShowPreviewWindow();
        }
    }
}
