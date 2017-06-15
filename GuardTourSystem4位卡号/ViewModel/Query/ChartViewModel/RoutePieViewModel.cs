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
    class RoutePieViewModel : BindableBase
    {
        public ObservableCollection<AssetData> Data { get; set; }

        public RoutePieViewModel()
        {
            this.Data = new ObservableCollection<AssetData>();
        }

        public void InitPie(CountInfo countInfo)
        {
            Data.Clear();
            Data.Add(new AssetData(true) { Value = countInfo.PatrolPercent, Label = String.Format("出勤率: {0:N2} %", countInfo.PatrolPercent) });
            Data.Add(new AssetData(false) { Value = countInfo.MissPercent, Label = String.Format("缺勤率: {0:N2} %", countInfo.MissPercent) });
        }
    }
}
