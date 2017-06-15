using GuardTourSystem.Database.DAL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.BLL
{
    public class FrequenceBLL : IFrequenceService
    {
        #region 成员/初始化
        public FrequenceDAO DAO { get; set; }
        public RegularDAO RegularDAO { get; set; }
        public IrregularDAO IrregularDAO { get; set; }
        public FrequenceWorkerDAO FreqWorkerDAO { get; set; }

        public FrequenceBLL()
        {
            DAO = new FrequenceDAO();
            RegularDAO = new RegularDAO();
            IrregularDAO = new IrregularDAO();
            FreqWorkerDAO = new FrequenceWorkerDAO();
        }
        #endregion

        public List<Frequence> GetAllFrequence(Route route)
        {
            var freqs = DAO.GetAllFrequence(route);
            RegularDAO rgDAO = new RegularDAO();
            IrregularDAO irDAO = new IrregularDAO();
            FrequenceWorkerDAO fwDAO = new FrequenceWorkerDAO();

            foreach (var freq in freqs)
            {
                //附带 线路信息
                freq.RouteID = route.ID;
                freq.RouteName = route.RouteName;

                // 获取排班信息
                freq.Worker = fwDAO.GetFrequenceWorker(freq);
                freq.Regular = rgDAO.GetRegular(freq);
                freq.Irregular = irDAO.GetIrregular(freq);
            }
            return freqs;
        }

        public List<Frequence> GetAllFrequence()
        {
            var routes = new RouteBLL().GetAllRoute(false, false);
            var result = new List<Frequence>();
            foreach (var item in routes)
            {
                result.AddRange(this.GetAllFrequence(item));
            }
            return result;
        }

        public bool AddFrequence(Frequence f, out int id, out string errorInfo)
        {
            errorInfo = "";
            id = -1;
            if (!CheckFrequenceProp(f, ref errorInfo))
            {
                return false;
            }
            if (DAO.ExistsName(f.RouteID, f.Name))
            {
                errorInfo = "该线路下已存在同名班次";
                return false;
            }
            return DAO.AddFrequence(f, out id);
        }

        //更新班次的基本信息,并重新生成该班次当天的值班表
        public bool UpdateFrequence(Frequence newFrequence, out string errorInfo)
        {
            errorInfo = "";
            if (!CheckFrequenceProp(newFrequence, ref errorInfo))
            {
                return false;
            }

            var old = DAO.QueryFrequence(newFrequence.ID);
            //附带上线路名称. QueryFrequence只查询T_Frequence
            var route = new RouteDAO().QueryRoute(old.RouteID);
            old.RouteName = route.RouteName;
            if (!old.Name.Equals(newFrequence.Name)) //如果姓名变更,判断新名称是否已经存在
            {
                if (DAO.ExistsName(newFrequence.RouteID, newFrequence.Name))
                {
                    errorInfo = "该线路下已存在同名班次";
                    return false;
                }
            }
            if (DAO.UpdateFrequence(newFrequence))
            {
                //更新成功,重新生成该班次当天的Duty
                var bll = new DutyBLL();
                //删除时的判断条件是 Frequence的路线名称和班次名称,所以第二个参数传入 old .
                bll.GenerateDuty(out errorInfo, old, DateTime.Now); //更新值班表
                return true;
            }
            else
            {
                throw new Exception("UpdateFrequence Error! Frequence:" + newFrequence.ToString());
            }

        }

        public bool UpdateFrequenceRegular(Frequence f, out string errorInfo)
        {
            errorInfo = "";
            UpdateFrequenceWorker(f);
            //更新 T_Regular
            if (RegularDAO.ExistsRegular(f.Regular))
            {
                return RegularDAO.UpdateRegular(f.Regular);
            }
            else
            {
                if (RegularDAO.AddRegular(f.Regular))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool UpdateFrequenceIrregular(Frequence f, out string errorInfo)
        {
            errorInfo = "";
            UpdateFrequenceWorker(f);
            //更新 T_Irregular
            int id = -1;
            foreach (var monthPlan in f.Irregular.MonthPlans)
            {
                if (IrregularDAO.ExistsMonthPlan(f, monthPlan))
                {
                    IrregularDAO.UpdateMonthPlan(f, monthPlan);
                }
                else
                {
                    if (!IrregularDAO.AddMonthPlan(f, monthPlan, out id))
                    {
                        return false;
                    };
                }
            }
            return true;
        }
        //更新 班次巡检员
        public bool UpdateFrequenceWorker(Frequence f)
        {
            if (f.Worker == null || f.Worker.ID <= 0)
            { //如果不指定巡检员,将 T_FrequenceWorker中的记录删除
                return FreqWorkerDAO.DelFrequenceWorker(f);
            }
            else
            {
                // 如果指定了巡检员, 新增或替换
                return FreqWorkerDAO.ReplaceFrequenceWorker(f);
            }
        }

        public bool DeleteFrequence(Frequence f)
        {
            return DAO.DeleteFrequence(f);
        }
        private bool CheckFrequenceProp(Frequence f, ref string errorInfo)
        {
            if (f.Name != null)
            {
                f.Name = f.Name.Trim();
            }
            if (String.IsNullOrEmpty(f.Name))
            {
                errorInfo = "抱歉,班次名称不能为空";
                return false;
            }
            if (f.StartTime == null)
            {
                errorInfo = "抱歉,请输入正确的上班时间.";
                return false;
            }
            if (f.EndTime == null)
            {
                errorInfo = "抱歉,请输入正确的下班时间.";
                return false;
            }
            var startTime = (TimeSpan)f.StartTime;
            var endTime = (TimeSpan)f.EndTime;
            if (endTime.Subtract(startTime).TotalMinutes >= 24 * 60)
            {
                errorInfo = "抱歉,班次时间不能超过24小时";
                return false;
            }
            if (endTime.Subtract(startTime).TotalMinutes <= 0)
            {
                errorInfo = "抱歉,上班时间要早于下班时间";
                return false;
            }
            if (f.PatrolTime <= 0)
            {
                errorInfo = "抱歉,巡逻时间不能少于 1 分钟";
                return false;
            }
            if (f.RestTime < 0)
            {
                errorInfo = "抱歉,休息时间必须大于等于 0 分钟";
                return false;
            }

            if (f.RouteID <= 0)
            {
                throw new ArgumentException("未给班次的RouteID赋值");
            }

            //if( f.StartDate < DateTime.Now){
            //    errorInfo="抱歉,班次开始时间不能早于今日";
            //    return false;
            //}

            var minTime = new DateTime(2000, 01, 01);
            if (f.StartDate < minTime)
            {
                errorInfo = "抱歉,请输入正确的班次开始日期";
                return false;
            }
            if (f.EndDate != null && f.EndDate < f.StartDate)
            {
                errorInfo = "抱歉,班次开始日期需要早于结束日期";
                return false;
            }
            if (f.GeneratedDate == null)
            {
                f.GeneratedDate = DateTime.Now;
            }
            return true;
        }



        public void Init()
        {
            DAO.Init();
            RegularDAO.Init();
            IrregularDAO.Init();
            new FrequenceWorkerDAO().Init();
        }
    }
}
