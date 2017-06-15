using GuardTourSystem.Database.Model;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.ViewModel.Query.ChartViewModel
{
    class WorkerChartViewModel : BindableBase
    {
        //表格的ViewModel就使用本类,方便一点,不用再新建一个类
        private ObservableCollection<CountInfo> workerCountInfos;
        public ObservableCollection<CountInfo> WorkerCountInfos
        {
            get { return workerCountInfos; }
            set
            {
                SetProperty(ref this.workerCountInfos, value);
            }
        }

        private WorkerBarViewModel barViewModel;
        public WorkerBarViewModel BarViewModel
        {
            get { return barViewModel; }
            set
            {
                SetProperty(ref this.barViewModel, value);
            }
        }

        private WorkerPieViewModel pieViewModel;
        public WorkerPieViewModel PieViewModel
        {
            get { return pieViewModel; }
            set
            {
                SetProperty(ref this.pieViewModel, value);
            }
        }

        public WorkerChartViewModel(List<CountInfo> countInfos)
        {
            this.Init(countInfos);
        }

        public void Init(List<CountInfo> infos)
        {
            this.WorkerCountInfos = new ObservableCollection<CountInfo>(infos);
            this.BarViewModel = new WorkerBarViewModel(infos);
            this.PieViewModel = new WorkerPieViewModel(infos);
        }
    }
}
