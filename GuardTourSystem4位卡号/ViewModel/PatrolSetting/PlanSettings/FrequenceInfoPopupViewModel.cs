using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.ViewModel
{
    // 班次信息 弹出框的ViewModel
    class FrequenceInfoPopupViewModel : AbstractInfoNotificationViewModel
    {
        private Frequence frequence;

        public Frequence Frequence
        {
            get { return frequence; }
            set
            {
                SetProperty(ref this.frequence, value);
            }
        }

        public List<Route> Routes { get; set; }

        private Route route;
        public Route Route //当前选中的Route
        {
            get { return route; }
            set
            {
                SetProperty(ref this.route, value);
            }
        }


        public IFrequenceService FrequenceService { get; set; }
        public FrequenceInfoPopupViewModel(List<Route> routes, Frequence frequence)
            : base()
        {
            FrequenceService = new FrequenceBLL();
            Routes = routes;

            Frequence = frequence;
            if (Frequence == null)
            {
                Route = Routes[0];

                Title = "新增班次";
                Frequence = new Frequence(DateTime.Now, null, 9, 00, 17, 30, 60, 0);
                Frequence.IsRegular = true;
                ConfirmButtonText = LanLoader.Load(LanKey.Add);
                this.CConfirm = new DelegateCommand(this.AddInfo);
            }
            else
            {
                Route = Routes.Find(r => { return r.ID == frequence.RouteID; });

                this.Title = "编辑班次";
                ConfirmButtonText = LanLoader.Load(LanKey.Edit);
                this.CConfirm = new DelegateCommand(this.UpdateInfo);
            }
        }

        public override void AddInfo()
        {
            Frequence.RouteID = Route.ID;
            Frequence.RouteName = Route.RouteName;

            int id;
            string errorInfo;
            if (FrequenceService.AddFrequence(Frequence, out id, out errorInfo))
            {
                Frequence.ID = id;
                this.Finish();
            }
            else
            {
                this.ErrorInfo = errorInfo;
            }
        }

        public override void UpdateInfo()
        {
            Frequence.RouteID = Route.ID;
            Frequence.RouteName = Route.RouteName;

            string errorInfo;
            if (FrequenceService.UpdateFrequence(Frequence, out errorInfo))
            {
                //更新成功,重新生成该班次当天的Duty
                var result = new DutyBLL().GenerateDuty(out errorInfo, Frequence, DateTime.Now) != -1; //更新值班表
                if (result)
                {
                    AppStatusViewModel.Instance.ShowInfo("班次信息修改成功,已重新生成该班次今天的值班表");
                }
                else
                {
                    throw new Exception("重新生成当天的值班表失败: " + Frequence + "\n" + errorInfo);
                }
                this.Finish();
            }
            else
            {
                this.ErrorInfo = errorInfo;
            }
        }

       
    }
}
