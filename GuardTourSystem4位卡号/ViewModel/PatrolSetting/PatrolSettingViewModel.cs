using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.ViewModel
{
    class PatrolSettingViewModel
    {
        private IRouteService RouteService { get; set; }
        private IFrequenceService FrequenceService { get; set; }
        private IWorkerService WorkerService { get; set; }

        public List<Frequence> AllFrequence { get; set; }
        public List<Frequence> RegularFrequence { get; set; }
        public List<Frequence> IrregularFrequence { get; set; }

        private static PatrolSettingViewModel instance = null;

        public static PatrolSettingViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PatrolSettingViewModel();
                }
                return instance;
            }
        }

        public RoutePlaceViewModel RouteViewModel { get; set; }
        public WorkerViewModel WorkerViewModel { get; set; }
        public EventViewModel EventViewModel { get; set; }
        public BaseFrequenceViewModel FrequenceViewModel { get; set; }
        public BaseFrequenceViewModel RegularViewModel { get; set; }
        public BaseFrequenceViewModel IrregularViewModel { get; set; }

        public PatrolSettingViewModel()
        {
            RouteService = new RouteBLL();
            FrequenceService = new FrequenceBLL();

            FrequenceViewModel = new FrequenceViewModel();
            RegularViewModel = new RegularFrequenceViewModel();
            IrregularViewModel = new IrregularFrequenceViewModel();
        }

        //发布data改变的通知. 
        // 当前实现界面自动刷新的方式:  某个界面的数据改变-> 调用该方法 -> 其他ViewModel进行数据刷新
        public void PublishDataChange(ChangeEvent ce)
        {
            //switch (ce)
            //{
            //    case ChangeEvent.RoutesChange:
            //        FrequenceViewModel.NotifyChange(ce);
            //        RegularViewModel.NotifyChange(ce);
            //        IrregularViewModel.NotifyChange(ce);
            //        break;
            //    case ChangeEvent.WorkersChange:
            //        RegularViewModel.NotifyChange(ce);
            //        IrregularViewModel.NotifyChange(ce);
            //        break;
            //    case ChangeEvent.FrequenceChange:
            //        RegularViewModel.NotifyChange(ce);
            //        IrregularViewModel.NotifyChange(ce);

            //        break;
            //    default:
            //        break;
            //}
        }
    }
    public enum ChangeEvent
    {
        RoutesChange, WorkersChange, FrequenceChange
    }
}
