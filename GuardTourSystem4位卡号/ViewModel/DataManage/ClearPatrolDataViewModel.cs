using GuardTourSystem.Model.Settings;
using GuardTourSystem.Utils;
using GuardTourSystem.Properties;
using GuardTourSystem.Settings;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GuardTourSystem.Services.Database.BLL;
using GuardTourSystem.Model;
using MahApps.Metro.Controls.Dialogs;
using GuardTourSystem.Database.BLL;

namespace GuardTourSystem.ViewModel
{
    class ClearPatrolDataViewModel : AbstractPopupNotificationViewModel
    {
        public IRawDataService RawDataService { get; set; }

        public DateQueryInfo DateQueryInfo { get; set; }


        /// <summary>
        /// Constractor
        /// </summary>
        public ClearPatrolDataViewModel()
        {
            RawDataService = new RawDataBLL();

            // 初始化语言菜单栏
            Title = "清理巡检数据";
            ConfirmButtonText = "确定";
            this.CConfirm = new DelegateCommand(this.ClearPatrol, this.CheckInput);

            this.DateQueryInfo = new DateQueryInfo(DateTime.Now, DateTime.Now, () => { this.CConfirm.RaiseCanExecuteChanged(); });

        }

        private async void ClearPatrol()
        {
            //关闭弹出窗
            this.Finish();
            //弹出确认框,让用户点击确定
            var result = await this.ShowConfirmDialog("确定要清理巡检数据吗?",
                 "将清理 " + DateQueryInfo.GetQueryTime() + " 的巡检数据,清理后将无法恢复!");
            if (result == MessageDialogResult.Negative) //用户取消
            {
                return;
            }
            else
            {
                //操作数据库
                if (RawDataService.DelRawData(DateQueryInfo.Begin.ToNotNullable(), DateQueryInfo.End.ToNotNullable()))
                {
                    AppStatusViewModel.Instance.ShowInfo("数据清理成功");
                }
                else
                {
                    AppStatusViewModel.Instance.ShowError("数据清理失败");
                }
            }
        }
        private bool CheckInput()
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
