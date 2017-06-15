using GuardTourSystem.Model;
using GuardTourSystem.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services
{
    public interface IWorkerService
    {
        List<Worker> GetAllWorker();

        bool CanAdd(Worker worker, out string errorInfo);

        bool AddWorker(Worker worker, out int id, out string errorInfo);

        bool UpdateWorker(Worker worker, out string errorInfo);

        bool DelWorker(Worker worker);

        List<Worker> QueryWorker(string str);

        void Init(); //初始化T_worker表
    }
}
