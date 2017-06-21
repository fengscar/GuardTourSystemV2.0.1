using GuardTourSystem.Database.DAL;
using GuardTourSystem.Model;
using GuardTourSystem.Model.DAL;
using GuardTourSystem.Services.Database.BLL;
using GuardTourSystem.Services.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services
{
    public class WorkerBLL : IWorkerService
    {
        public WorkerDAO DAO { get; set; }
        public WorkerBLL()
        {
            DAO = new WorkerDAO();
        }

        public bool CanAdd(Worker worker, out string errorInfo)
        {
            errorInfo = "";

            if (!CheckWorkerProp(worker, ref errorInfo))
            {
                return false;
            }
            if (!PatrolSQLiteManager.CheckCardUnique(worker.Card, ref errorInfo))
            {
                return false;
            }

            if (DAO.ExistsName(worker.Name))
            {
                errorInfo = "该管理卡名称已被使用";
                return false;
            }
            return true;
        }

        public List<Worker> GetAllWorker()
        {
            return DAO.GetAllWorker();
        }
        public List<Worker> QueryWorker(string str)
        {
            return DAO.QueryWorker(str);
        }

        public bool AddWorker(Worker worker, out int id, out string errorInfo)
        {
            id = -1;
            if (CanAdd(worker, out errorInfo))
            {
                return DAO.AddWorker(worker, out id);
            }
            return false;
        }

        public bool UpdateWorker(Worker worker, out string errorInfo)
        {
            errorInfo = "";
            if (!CheckWorkerProp(worker, ref errorInfo))
            {
                return false;
            }
            var old = DAO.QueryWorker(worker.ID);
            if (!old.Card.Equals(worker.Card)) //如果钮号变更,判断新钮号是否已经存在
            {
                if (!PatrolSQLiteManager.CheckCardUnique(worker.Card, ref errorInfo))
                {
                    return false;
                }
            }
            if (!old.Name.Equals(worker.Name)) //如果姓名变更,判断新名称是否已经存在
            {
                if (DAO.ExistsName(worker.Name))
                {
                    errorInfo = "该管理卡名称已被使用";
                    return false;
                }
            }
            if (old.Name.Equals(worker.Name) && old.Card.Equals(worker.Card) ) // 不需要更新
            {
                return true;
            }

            return DAO.UpdateWorker(worker);
        }

        public bool DelWorker(Worker worker)
        {
            return DAO.DelWorker(worker);
        }

        /// <summary>
        /// 检查 worker 属性是否有空值
        /// 如果有空值,返回false,并在errorInfo附带错误信息
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        private bool CheckWorkerProp(Worker worker, ref string errorInfo)
        {
            if (worker.Name != null)
            {
                worker.Name = worker.Name.Trim();
            }
            if (String.IsNullOrEmpty(worker.Name))
            {
                errorInfo = "抱歉,名称不能为空";
                return false;
            }
            if (String.IsNullOrEmpty(worker.Card))
            {
                errorInfo = "抱歉,钮号不能为空";
                return false;
            }
            if (worker.Card.Length != 4)
            {
                errorInfo = "请输入4位钮号";
                return false;
            }
            return true;
        }


        public void Init()
        {
            DAO.Init();
            new FrequenceWorkerDAO().Init();
        }
    }
}
