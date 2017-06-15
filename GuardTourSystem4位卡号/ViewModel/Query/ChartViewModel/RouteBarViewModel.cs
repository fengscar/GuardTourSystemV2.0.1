using GuardTourSystem.Database.Model;
using GuardTourSystem.Model;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.ViewModel.Query.ChartViewModel
{
    class RouteBarViewModel : BindableBase
    {
        public ObservableCollection<AssetData> BarDatas { get; set; }


        public RouteBarViewModel()
        {
            this.BarDatas = new ObservableCollection<AssetData>();
        }


        public void InitBar(List<CountInfo> placeCountInfo)
        {
            BarDatas.Clear();

            foreach (var countInfo in placeCountInfo)
            {
                BarDatas.Add(new AssetData(true) { Value = countInfo.PatrolCount, MissValue = countInfo.MissCount, Label = countInfo.CountName });
            }
        }
        public void InitBar(CountInfo countInfo)
        {
            var info = new List<CountInfo>();
            info.Add(countInfo);
            this.InitBar(info);
        }

        public void InitBar(RouteCountInfo countInfo)
        {
            var info = new List<CountInfo>();
            if (countInfo.PlaceCountInfos.Count == 0)
            {
                info.Add(countInfo);
            }
            foreach (var item in countInfo.PlaceCountInfos)
            {
                info.Add(item);
            }
            this.InitBar(info);
        }

    }
}
