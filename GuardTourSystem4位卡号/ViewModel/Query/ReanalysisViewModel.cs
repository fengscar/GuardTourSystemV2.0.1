using GuardTourSystem.Database.BLL;
using GuardTourSystem.Services;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuardTourSystem.Utils;

namespace GuardTourSystem.ViewModel.Popup
{
    class ReanalysisViewModel : AbstractPopupNotificationViewModel
    {
        public IDutyService DutyService { get; set; }


        public DateQueryInfo DateQueryInfo { get; set; }


        public ReanalysisViewModel()
        {
            DutyService = new DutyBLL();

            Title = "重新分析";
            this.ConfirmButtonText = "开始";
            this.CConfirm = new DelegateCommand(this.Reanalysis, this.CanExecute); //该按键在日期改变时,实时判断能否执行

            this.DateQueryInfo = new DateQueryInfo(DateTime.Now.SetBeginOfDay(), DateTime.Now, () => { this.CConfirm.RaiseCanExecuteChanged(); });

        }

        public async void Reanalysis()
        {
            this.Finish();
            //弹出确认框,让用户点击确定
            var result = await this.ShowConfirmDialog("确定要重新分析吗?",
                "将重新分析 " + DateQueryInfo.GetQueryTime() + " 的计数数据,该操作无法撤销!");
            if (result == MessageDialogResult.Negative) //用户取消
            {
                return;
            }
            else
            {
                if (new DutyBLL().UpdateDuty(DateQueryInfo.Begin.ToNotNullable(), DateQueryInfo.End.ToNotNullable()))
                {
                    AppStatusViewModel.Instance.ShowInfo("重新分析成功");
                }
                else
                {
                    AppStatusViewModel.Instance.ShowError("重新分析失败");
                }
            }

        }

        private bool CanExecute()
        {
            string error = null;
            if (DateQueryInfo.CanQuery(out error))
            {
                this.ErrorInfo = null;
                return true;
            }
            else
            {
                this.ErrorInfo = error;
                return false;
            }
        }
    }
}
