using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Model.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Utils;
using GuardTourSystem.Services.Database.DAL;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Printing;
using System.Drawing.Printing;
using GuardTourSystem.Print;

namespace GuardTourSystem.ViewModel
{
    /// <summary>
    /// 考核结果. 以下功能
    /// 1. 查询: 重新查询数据库.
    /// 2. 显示漏巡/正常数据: 筛选已查询到的数据源,避免再次查询
    /// </summary>
    public class QueryResultViewModel : ShowContentViewModel
    {
        private IDutyService DataService;
        /// <summary>
        /// 当前查询到的Duty
        /// </summary>
        public List<Duty> Dutys { get; set; }

        ///// <summary>
        ///// 当前要显示的记录
        ///// </summary>
        //private ObservableCollection<ResultItemViewModel> results;
        //public ObservableCollection<ResultItemViewModel> Results
        //{
        //    get { return results; }
        //    set
        //    {
        //        SetProperty(ref this.results, value);
        //    }
        //}
        private ObservableCollection<ResultItem> results;
        public ObservableCollection<ResultItem> Results
        {
            get { return results; }
            set
            {
                SetProperty(ref this.results, value);
            }
        }


        public DateQueryInfo DateQueryInfo { get; set; }


        /// <summary>
        /// 是否显示正常数据,value改变时将触发FilterData()
        /// </summary>
        private bool showInDuty;
        public bool ShowInDuty
        {
            get { return showInDuty; }
            set
            {
                SetProperty(ref this.showInDuty, value);
                FilterData();
            }
        }

        /// <summary>
        /// 是否显示漏巡数据,value改变时将触发FilterData()
        /// </summary>
        private bool showOutDuty;
        public bool ShowOutDuty //显示漏巡记录
        {
            get { return showOutDuty; }
            set
            {
                SetProperty(ref this.showOutDuty, value);
                FilterData();
            }
        }

        public DelegateCommand CQuery { get; set; }
        public DelegateCommand CPrint { get; set; }
        public DelegateCommand CReset { get; set; }

        public QueryResultViewModel()
        {
            DataService = new DutyBLL();
            this.Dutys = new List<Duty>();
            this.CQuery = new DelegateCommand(this.Query, this.CanQuery);
            this.CPrint = new DelegateCommand(this.Print, () => Results != null && Results.Count != 0);
            this.CReset = new DelegateCommand(this.Reset);

            var now = DateTime.Now;
            DateQueryInfo = new DateQueryInfo(now.SetBeginOfMonth(), now, () => { this.CQuery.RaiseCanExecuteChanged(); });

            this.ShowInDuty = true;
            this.ShowOutDuty = true;

            this.Query();
        }

        /// <summary>
        /// 查询数据库
        /// </summary>
        private void Query()
        {
            Dutys = DataService.GetAllDuty(DateQueryInfo.Begin, DateQueryInfo.End);
            AppStatusViewModel.Instance.ShowInfo("查询完成", 2);
            //显示数据
            FilterData();
        }

        private bool CanQuery()
        {
            string error = null;
            if (DateQueryInfo.CanQuery(out error))
            {
                this.Error = null;
                return true;
            }
            else
            {
                this.Error = error;
                return false;
            }
        }

        private void Print()
        {
            //var printData = new PrintData() { ContentList = Results.ToList(), Title = "考核结果", DataCount = Results.Count };
            //var queryInfo = this.DateQueryInfo.GetQueryTime() + "的考核结果";
            //var printer = new Printer(new ResultDocument(printData, queryInfo));
            //printer.ShowPreviewWindow();
        }


        private void FilterData()
        {
            this.Results = new ObservableCollection<ResultItem>();
            //如果没有数据 ,或者[显示漏巡][显示正常]都没选中,什么都不显示
            if (this.Dutys.Count == 0 || (!ShowOutDuty && !ShowInDuty))
            {
                return;
            }
            foreach (var duty in Dutys)
            {
                if (duty.Records != null)
                {
                    foreach (var record in duty.Records)
                    {
                        if ((ShowInDuty == false && record.PlaceTime != null) || (ShowOutDuty == false && record.PlaceTime == null))
                        {
                            continue;
                        }
                        var item = new ResultItem();
                        item.RouteName = duty.Frequence.RouteName;
                        item.FrequenceName = duty.Frequence.Name;
                        item.WorkerName = duty.Worker == null ? "(未指定)" : duty.Worker.Name;
                        item.SetPlanTime(duty.DutyTime.PatrolBegin, duty.DutyTime.PatrolEnd);

                        item.PlaceName = record.Place.Name;
                        item.EventName = record.Event == null ? "无" : record.Event.Name;
                        item.SetPatrolTime(record.PlaceTime);

                        this.Results.Add(item);
                    }
                }
            }
        }

        private void Reset()
        {
            AppContentViewModel.Instance.CloseView();
            AppContentViewModel.Instance.ShowView(ViewEnum.QueryResult);
        }


        //public class ResultItemViewModel//考核结果的VM
        //{
        //    public string RouteName { get; set; }
        //    public string FrequenceName { get; set; }
        //    public string PlanTime { get; set; }//开始时间+结束时间
        //    public string WorkerName { get; set; }

        //    public List<ResultDetailViewModel> Details { get; set; }

        //    public void SetPlanTime(DateTime begin, DateTime end)
        //    {
        //        this.PlanTime = begin.ToString() + " - " + end.ToString();
        //    }
        //}
        //public class ResultDetailViewModel
        //{
        //    public string PlaceName { get; set; }
        //    public DateTime? PatrolTime { get; set; } //巡逻时间. 
        //    public string PatrolResult { get; set; } //巡逻结果. 如果有巡逻,显示准时;如果没有,显示"漏巡"
        //    public string EventName { get; set; }

        //    public void SetPatrolTime(DateTime? time)
        //    {
        //        PatrolTime = time;
        //        PatrolResult = time != null ? "准时" : "漏巡";
        //    }
        //}

        public class ResultItem
        {
            public string RouteName { get; set; }
            public string FrequenceName { get; set; }
            public string PlanTime { get; set; }//开始时间+结束时间
            public string WorkerName { get; set; }
            public string PlaceName { get; set; }
            public DateTime? PatrolTime { get; set; } //巡逻时间. 
            public string PatrolResult { get; set; } //巡逻结果. 如果有巡逻,显示准时;如果没有,显示"漏巡"
            public string EventName { get; set; }

            public void SetPatrolTime(DateTime? time)
            {
                PatrolTime = time;
                PatrolResult = time != null ? "准时" : "漏巡";
            }
            public void SetPlanTime(DateTime begin, DateTime end)
            {
                this.PlanTime = begin.ToString() + " - " + end.ToString();
            }
        }
    }
}
