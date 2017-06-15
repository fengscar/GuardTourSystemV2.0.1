using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Model.DAL;
using GuardTourSystem.Print;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using GuardTourSystem.Utils;
using KaiheSerialPortLibrary;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GuardTourSystem.ViewModel
{
    /// <summary>
    /// 合并了 Route 与 Place的操作
    /// </summary>
    class RoutePlaceViewModel : ShowContentViewModel, ISerialPortStateChangedListener
    {
        public IRouteService RouteService { get; set; }
        public IPlaceService PlaceService { get; set; }

        private ObservableCollection<Route> routes;
        public ObservableCollection<Route> Routes
        {
            get { return routes; }
            set
            {
                SetProperty(ref this.routes, value);
            }
        }

        private object selectItem;
        public object SelectItem
        {
            get { return selectItem; }
            set
            {
                this.CDelete.RaiseCanExecuteChanged();
                this.CUpdate.RaiseCanExecuteChanged();
                this.CShiftUp.RaiseCanExecuteChanged();
                this.CShiftDown.RaiseCanExecuteChanged();
                SetProperty(ref this.selectItem, value);
            }
        }

        public DelegateCommand CAddRoute { get; set; } //添加线路 
        public DelegateCommand CAddPlace { get; set; }

        public DelegateCommand CDelete { get; set; }  //删除 线路/地点
        public DelegateCommand CUpdate { get; set; }  //更新 线路/地点
        //public DelegateCommand CSendMap { get; set; } //发送地图给巡检机

        public DelegateCommand CShiftUp { get; set; } // 上移 地点
        public DelegateCommand CShiftDown { get; set; } // 下移 地点

        public DelegateCommand CPrint { get; set; } // 下移 地点

        public InteractionRequest<INotification> RouteInfoPopupRequest { get; private set; }
        public InteractionRequest<INotification> PlaceInfoPopupRequest { get; private set; }

        //public PlaceViewModel PlaceViewModel { get; set; }　//包含对地点的增删改

        public RoutePlaceViewModel()
        {
            SerialPortManager.Instance.AddListener(this);
            PatrolSettingViewModel.Instance.RouteViewModel = this;

            InitBatchAdd();

            RouteService = new RouteBLL();
            PlaceService = new PlaceBLL();
            this.Routes = new ObservableCollection<Route>(RouteService.GetAllRoute());
            this.RefreshBatchAddRoute();

            this.CAddRoute = new DelegateCommand(this.AddRoute);
            this.CAddPlace = new DelegateCommand(this.AddPlace, () => { return Routes.Count != 0; });

            this.CDelete = new DelegateCommand(this.Delete, () => { return SelectItem != null; });
            this.CUpdate = new DelegateCommand(this.Update, () => { return SelectItem != null; });
            //this.CSendMap = new DelegateCommand(this.SendMap, () => { return Routes.Count != 0 && !SerialPortManager.Instance.IsWritting; });

            this.CShiftUp = new DelegateCommand(this.ShiftUp, this.CanShiftUp);
            this.CShiftDown = new DelegateCommand(this.ShiftDown, this.CanShiftDown);

            this.CPrint = new DelegateCommand(this.Print);

            this.RouteInfoPopupRequest = new InteractionRequest<INotification>();
            this.PlaceInfoPopupRequest = new InteractionRequest<INotification>();
        }



        // 判断是更新 线路还是地点
        public void Update()
        {
            if (SelectItem is Route)
            {
                UpdateRoute(SelectItem as Route);
            }
            if (SelectItem is Place)
            {
                UpdatePlace(SelectItem as Place);
            }
        }

        // 判断是删除 线路还是地点
        public void Delete()
        {
            if (SelectItem is Route)
            {
                DelRoute(SelectItem as Route);
            }
            if (SelectItem is Place)
            {
                DelPlace(SelectItem as Place);
            }
        }

        #region 对 线路的增删改
        public void AddRoute()
        {
            var infoVM = new RouteInfoViewModel(null);
            this.RouteInfoPopupRequest.Raise(infoVM,
                notification =>
                {
                    var routeInfoViewModel = notification as RouteInfoViewModel;
                    if (!routeInfoViewModel.IsCancel)
                    {
                        routes.Add(routeInfoViewModel.Route);
                        //通知 线路已发生改变
                        PatrolSettingViewModel.Instance.PublishDataChange(ChangeEvent.RoutesChange);
                    }
                });
            // 刷新按键状态
            this.CAddPlace.RaiseCanExecuteChanged();
            //this.CSendMap.RaiseCanExecuteChanged();
            this.CAddPlace.RaiseCanExecuteChanged();
        }

        public void UpdateRoute(Route route)
        {
            this.RouteInfoPopupRequest.Raise(new RouteInfoViewModel(route.Clone() as Route),
                  notification =>
                  {
                      var routeInfoViewModel = notification as RouteInfoViewModel;
                      if (!routeInfoViewModel.IsCancel)
                      {
                          var newRoute = routeInfoViewModel.Route;
                          var updatedRoute = Routes.First(r => { return r.ID == newRoute.ID; });
                          updatedRoute.RouteName = newRoute.RouteName;
                          //SelectItem = newRoute;//重新选中该 线路
                      }
                  });
        }
        public async void DelRoute(Route route)
        {
            // ShowConfirmDialogAction() 返回一个 Task<MessageDialogResult> 类型
            var result = ShowConfirmDialog(LanLoader.Load(LanKey.RouteSettingDelRouteConfirm), LanLoader.Load(LanKey.RouteSettingDelRouteConfirmExp));

            // 使用 await关键字等待用户操作... 
            // 当用户点击后 , result将返回 用户的点击结果
            if (await result == MessageDialogResult.Negative) //用户取消
            {
                return;
            }
            else
            {
                if (RouteService.DelRoute(route)) // 删除成功 (数据库中 Deleted设置为 1)
                {
                    this.Routes.Remove(route);
                    route = null;
                    PatrolSettingViewModel.Instance.PublishDataChange(ChangeEvent.RoutesChange);
                }
                else
                {
                    ShowMessageDialog(LanLoader.Load(LanKey.RouteSettingDelRouteError), null);
                };
            }
            // 刷新按键状态
        
            //this.CSendMap.RaiseCanExecuteChanged();
            this.CAddPlace.RaiseCanExecuteChanged();
            this.RefreshBatchAddRoute();
        }
        #endregion

        #region 对 地点的增删改
        public void AddPlace()
        {
            var infoVM = new PlaceInfoViewModel(this.Routes.ToList(), null);
            //默认选中第一个线路
            this.PlaceInfoPopupRequest.Raise(infoVM,
                notification =>
                {
                    var placeInfoViewModel = notification as PlaceInfoViewModel;
                    if (!placeInfoViewModel.IsCancel)
                    {
                        var newPlace = placeInfoViewModel.Place;
                        var route = Routes.First(r => { return r.ID == newPlace.RouteID; });
                        route.Places.Add(newPlace);
                    }
                });
        }

        public void UpdatePlace(Place place)
        {
            var infoVM = new PlaceInfoViewModel(this.Routes.ToList(), place.Clone() as Place);
            this.PlaceInfoPopupRequest.Raise(infoVM,
                notification =>
                {
                    var placeInfoViewModel = notification as PlaceInfoViewModel;
                    if (!placeInfoViewModel.IsCancel)
                    {
                        var newPlace = placeInfoViewModel.Place; //更新后的地点 
                        int updatedRouteIndex = 0, updatedPlaceIndex = 0;
                        for (int i = 0; i < Routes.Count; i++)
                        {
                            updatedPlaceIndex = Routes[i].Places.ToList().FindIndex(p => { return p.ID == newPlace.ID; });//根据ID 寻找该地点
                            if (updatedPlaceIndex != -1)
                            {
                                updatedRouteIndex = i;
                                break;
                            }
                        }
                        //如果线路没有改变,只要修改Place 属性
                        if (Routes[updatedRouteIndex].Places[updatedPlaceIndex].RouteID == newPlace.RouteID)
                        {
                            Routes[updatedRouteIndex].Places[updatedPlaceIndex] = newPlace;
                        }
                        else //否则要重新获取所有线路 (方便一点)
                        {
                            this.Routes = new ObservableCollection<Route>(RouteService.GetAllRoute());
                            RefreshBatchAddRoute();
                        }
                        //SelectItem = newPlace;//重新选中该 地点
                    }
                });
        }

        public async void DelPlace(Place place)
        {
            // ShowConfirmDialogAction() 返回一个 Task<MessageDialogResult> 类型

            var result = ShowConfirmDialog(LanLoader.Load(LanKey.RouteSettingDelPlaceConfirm), LanLoader.Load(LanKey.RouteSettingDelPlaceConfirmExp));

            // 使用 await关键字等待用户操作... 
            // 当用户点击后 , result将返回 用户的点击结果
            if (await result == MessageDialogResult.Negative) //用户取消
            {
                return;
            }
            else
            {
                if (PlaceService.DelPlace(place))
                {
                    //var routes = this.Routes.First(r => { return r.ID == place.RouteID; });
                    //routes.Places.Remove(place);
                    //place = null;
                    // 重新获取, 因为order已经改变
                    this.Routes = new ObservableCollection<Route>(RouteService.GetAllRoute());
                    this.RefreshBatchAddRoute();
                }
                else
                {
                    ShowMessageDialog(LanLoader.Load(LanKey.RouteSettingDelPlaceError), null);
                };
            }
            // 刷新按键状态
            this.CAddPlace.RaiseCanExecuteChanged();
        }
        #endregion

        #region 地点的上下移动
        public void ShiftUp()
        {
            // 进行两次更新
            //1 . 将选中的Place 的Order-1
            var selectPlace = SelectItem as Place;
            selectPlace.Order--;
            //2 . 将上一个 Place的Order+1
            var selectRoute = Routes.First(r => { return r.ID == selectPlace.RouteID; }); // 得到所属线路
            var upPlace = selectRoute.Places.First(p => { return p.Order == selectPlace.Order; });// 得到上一个 地点
            upPlace.Order++;

            string error;
            PlaceService.UpdatePlace(selectPlace, out error);
            PlaceService.UpdatePlace(upPlace, out error);

            //刷新UI
            this.Routes = new ObservableCollection<Route>(RouteService.GetAllRoute());
            this.RefreshBatchAddRoute();
            //重新选中该地点,方便继续上下移动操作
            var findPlace = this.Routes.SelectMany(r => r.Places).First(p => p.Card.Equals(selectPlace.Card));
            if (findPlace != null)
            {
                //重新选中该地点
                SelectItem = findPlace;
            }
            //批量添加地点 重新选中 线路

        }

        public void ShiftDown()
        {
            // 进行两次更新
            //1 . 将选中的Place 的Order+1
            var selectPlace = (SelectItem as Place).Clone() as Place; // 为什么下移要用克隆,上移却不要??? 
            selectPlace.Order++;
            //2 . 将上一个 Place的Order+1
            var selectRoute = Routes.First(r => { return r.ID == selectPlace.RouteID; }); // 得到所属线路
            var downPlace = (selectRoute.Places.First(p => { return p.Order == selectPlace.Order; })).Clone() as Place; ;// 得到上一个 地点
            downPlace.Order--;

            string error;
            PlaceService.UpdatePlace(selectPlace, out error);
            PlaceService.UpdatePlace(downPlace, out error);
            //刷新UI
            this.Routes = new ObservableCollection<Route>(RouteService.GetAllRoute());
            this.RefreshBatchAddRoute();
            //重新选中该地点,方便继续上下移动操作
            var findPlace = this.Routes.SelectMany(r => r.Places).First(p => p.Card.Equals(selectPlace.Card));
            if (findPlace != null)
            {
                //重新选中该地点
                SelectItem = findPlace;
            }
        }

        public bool CanShiftUp()
        {
            if (SelectItem is Place)
            {
                var place = SelectItem as Place;
                return place.Order != 1; // 如果已经是第一个,则不能上移
            }
            return false;
        }

        public bool CanShiftDown()
        {
            if (SelectItem is Place)
            {
                var place = SelectItem as Place;
                var route = this.Routes.First(r => { return r.ID == place.RouteID; });
                var index = route.Places.IndexOf(place);
                return route.Places.Count != index + 1; //如果已经是最后一个,则不能下移
            }
            return false;
        }
        #endregion

        //public async void SendMap()
        //{

        //    var result = await ShowConfirmDialog(LanLoader.Load(LanKey.RouteSettingSendToDeviceConfirm), LanLoader.Load(LanKey.RouteSettingSendToDeviceConfirmExp));

        //    if (result == MessageDialogResult.Negative) //用户取消
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        AppStatusViewModel.Instance.ShowProgress(true, LanLoader.Load(LanKey.RouteSettingSendindToDevice), 59000);
        //        string errorInfo = null;
        //        //clear infos
        //        var clearPlaceInfoFlow = await SerialPortUtil.Write(new ClearPlaceInfo());
        //        if (clearPlaceInfoFlow.Check())
        //        {
        //            /// 2. 设置路线数
        //            var setRouteCountFlow = await SerialPortUtil.Write(new SetRouteCount(Routes.Count));
        //            if (setRouteCountFlow.Check())
        //            {
        //                // 3. 设置每条线路 地点数
        //                for (int i = 0; i < Routes.Count; i++)
        //                {
        //                    var route = Routes[i];
        //                    var setPlaceCountFlow = await SerialPortUtil.Write(new SetPlaceCount(i, route.Places.Count));
        //                    if (!setPlaceCountFlow.Check())
        //                    {
        //                        errorInfo = setPlaceCountFlow.ResultInfo;
        //                        break;
        //                    }
        //                }
        //                if (errorInfo == null)
        //                {
        //                    // 4. 发送每个地点
        //                    for (int i = 0; i < Routes.Count; i++)
        //                    {
        //                        var route = Routes[i];
        //                        for (int j = 0; j < route.Places.Count; j++)
        //                        {
        //                            var place = route.Places[j];
        //                            var setPlaceInfoFlow = await SerialPortUtil.Write(new SetPlaceInfo(i, j, place.Card, place.Name));
        //                            if (!setPlaceInfoFlow.Check())
        //                            {
        //                                errorInfo = setPlaceInfoFlow.ResultInfo;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    AppStatusViewModel.Instance.ShowInfo(LanLoader.Load(LanKey.RouteSettingSendToDeviceSuccess));
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                errorInfo = setRouteCountFlow.ResultInfo;
        //            }
        //        }
        //        else
        //        {
        //            errorInfo = clearPlaceInfoFlow.ResultInfo;
        //        }
        //        AppStatusViewModel.Instance.ShowError(LanLoader.Load(LanKey.RouteSettingSendToDeviceFail, errorInfo));
        //        return;
        //    }
        //}

        private void Print()
        {
            var printData = new PrintData() { ContentList = Routes.ToList(), Title = LanLoader.Load(LanKey.Route), DataCount = Routes.Count };
            var printer = new Printer(new RouteAndPlaceDocument(printData));
            printer.ShowPreviewWindow();
        }

        #region 批量添加地点
        private Route batchAddRoute;
        public Route BatchAddRoute //批量添加选中的 线路
        {
            get { return batchAddRoute; }
            set
            {
                SetProperty(ref this.batchAddRoute, value);
            }
        }
        private void RefreshBatchAddRoute()
        {
            if (this.Routes != null && Routes.Count != 0)
            {
                this.BatchAddRoute = Routes[0];
            }
        }


        public BatchAddViewModel BatchAddVM { get; set; }
        private void InitBatchAdd()
        {
            this.BatchAddVM = new BatchAddViewModel(LanLoader.Load(LanKey.BatchAddReadPlace), OnBatchAdd, OnGetRecords);
        }
        public bool OnBatchAdd(List<AddItem> selectItems)
        {
            //是否所有item都能添加
            bool allCanAdd = true;
            List<Place> places = new List<Place>();
            //验证每个Item能否添加
            foreach (var item in selectItems)
            {
                string error = "";
                if (BatchAddRoute == null || BatchAddRoute.ID <= 0)
                {
                    item.Error = LanLoader.Load(LanKey.BatchAddReadErrorNoRouteSelect);
                    allCanAdd = false;
                    continue;
                }
                var newPlace = new Place() { Card = item.Card, Name = item.Name, RouteID = BatchAddRoute.ID };
                places.Add(newPlace);
                if (!PlaceService.CanAdd(newPlace, out error)) //有任意一个 不能添加
                {
                    item.Error = error;
                    allCanAdd = false;//为了一次性验证完成,不使用return
                }
            }
            if (!allCanAdd)
            {
                return false;
            }
            //验证通过, 添加每个选中的Item
            int id, routeID;
            string errorInfo;
            places.ForEach((place) => { PlaceService.AddPlace(place, out id, out routeID, out errorInfo); });
            //添加完成,刷新界面 
            this.Routes = new ObservableCollection<Route>(RouteService.GetAllRoute());
            this.RefreshBatchAddRoute();

            return true;
        }

        public void OnGetRecords(List<AddItem> items)
        {

        }
        #endregion


        public void onPortWrittingStateChanged(bool isWrite)
        {
            Application.Current.Dispatcher.BeginInvoke(
           new Action(
               () =>
               {
                   //在这里操作UI
                   //this.CSendMap.RaiseCanExecuteChanged();
               })
                 , null);
        }
    }
}
