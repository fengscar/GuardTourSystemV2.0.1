using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.ViewModel
{
    public abstract class BaseFrequenceViewModel : ShowContentViewModel
    {
        public IFrequenceService FrequenceService { get; set; }
        public IWorkerService WorkerService { get; set; }
        public IRouteService RouteService { get; set; }

        public BaseFrequenceViewModel()
        {
            this.FrequenceService = new FrequenceBLL();
            this.WorkerService = new WorkerBLL();
            this.RouteService = new RouteBLL();
        }

        public List<Frequence> GetFrequenceData()
        {
            return FrequenceService.GetAllFrequence();
        }
        // 得到所有可选的计数员,包括 <不指定>
        public List<Worker> GetWorkerData()
        {
            var workers = WorkerService.GetAllWorker();
            workers.Insert(0, Worker.UndefineWorker);
            return workers;
        }

        public abstract void NotifyChange(ChangeEvent ce);

    }
}
