using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GuardTourSystem.Utils;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Data;
using System.Windows.Controls;
using System.Printing;
using System.Windows.Documents;
using GuardTourSystem.Print;
using GuardTourSystem.Database.Model;
using GuardTourSystem.Settings;

namespace GuardTourSystem.ViewModel
{
    /// <summary>
    /// 查询原始数据 ( 数据查询 => 计数记录)
    /// </summary>
    public class QueryRawCountViewModel : ShowContentViewModel
    {
        private ObservableCollection<RawCountInfo> rawCounts;
        public ObservableCollection<RawCountInfo> RawCounts
        {
            get { return rawCounts; }
            set
            {
                rawCounts = value;
                RaisePropertyChanged("RawCounts");
            }
        }

        private string queryText;
        public string QueryText
        {
            get { return queryText; }
            set
            {
                queryText = value;
                RaisePropertyChanged("QueryText");
            }
        }

        public DateQueryInfo DateQueryInfo { get; set; }

        /// <summary>
        /// 查询
        /// </summary>
        public DelegateCommand CCount { get; set; }
        public DelegateCommand CReset { get; set; }


        public QueryRawCountViewModel()
        {
            CCount = new DelegateCommand(Count, CanQuery);

            CReset = new DelegateCommand(Reset);

            var now = DateTime.Now;
            DateQueryInfo = new DateQueryInfo(now.SetBeginOfMonth().SetBeginOfDay(), now.SetEndOfDay(), () => { CCount.RaiseCanExecuteChanged(); });

            //Count();
        }

        private async void Count()
        {
            AppStatusViewModel.Instance.ShowProgress(true, "正在统计数据...");
            IEnumerable<RawCountInfo> result = null;
            if (AppSetting.Default.IsIgnore && AppSetting.Default.IgnoreTime > 0)
            {
                var it = AppSetting.Default.IgnoreTime;
                result = await Task.Factory.StartNew(() => CountBLL.GetRawCountInfo(DateQueryInfo.Begin, DateQueryInfo.End, it));
            }
            else
            {
                result = await Task.Factory.StartNew(() => CountBLL.GetRawCountInfo(DateQueryInfo.Begin, DateQueryInfo.End, null));
            }
            if (!string.IsNullOrWhiteSpace(QueryText))
            {
                result = result.Where(countInfo => { return countInfo.PlaceName.Contains(QueryText) || countInfo.EmployeeNumber.Contains(QueryText); }).ToList();
            }
            RawCounts = new ObservableCollection<RawCountInfo>(result);
            //增加一条总计
            var total = new RawCountInfo() { RouteName = "-----总计-----", PlaceName = "", EmployeeNumber = "" };
            foreach (var item in result)
            {
                total.Count += item.Count;
            }
            RawCounts.Insert(0, total);
            AppStatusViewModel.Instance.ShowInfo("统计完成", 2);
        }


        private void Reset()
        {
            AppContentViewModel.Instance.CloseView();
            AppContentViewModel.Instance.ShowView(ViewEnum.QueryRawCount);
        }
        /// <summary>
        /// 判断能否查询.
        /// </summary>
        /// <returns></returns>
        private bool CanQuery()
        {
            string error = null;
            if (DateQueryInfo.CanQuery(out error))
            {
                Error = null;
                return true;
            }
            else
            {
                Error = error;
                return false;
            }
        }
    }
}
