using GuardTourSystem.Database.BLL;
using GuardTourSystem.Database.Model;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GuardTourSystem.Utils;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GuardTourSystem.View.Query.ChartControls;
using GuardTourSystem.ViewModel.Query.ChartViewModel;

namespace GuardTourSystem.ViewModel
{
    class QueryChartViewModel : ShowContentViewModel
    {
        public DateQueryInfo DateQueryInfo { get; set; }

        private ObservableCollection<CountInfo> workerCounts;
        public ObservableCollection<CountInfo> WorkerCountInfos
        {
            get { return workerCounts; }
            set
            {
                SetProperty(ref this.workerCounts, value);
            }
        }

        private ObservableCollection<RouteCountInfo> routeCounts;
        public ObservableCollection<RouteCountInfo> RouteCountInfos
        {
            get { return routeCounts; }
            set
            {
                SetProperty(ref this.routeCounts, value);
            }
        }

        //public List<ChartType> ChartTypes { get; set; } //显示图表类型
        //private ChartType selectChartType;
        //public ChartType SelectChartType
        //{
        //    get { return selectChartType; }
        //    set
        //    {
        //        SetProperty(ref this.selectChartType, value);
        //        InitContent();
        //    }
        //}

        public List<CountType> CountTypes { get; set; } //显示的统计信息( 人员/线路)
        private CountType countType;
        public CountType SelectCountType
        {
            get { return countType; }
            set
            {
                SetProperty(ref this.countType, value);
                InitContent();
            }
        }

        public DelegateCommand CCount { get; set; }

        private UserControl content;
        public UserControl Content
        {
            get { return content; }
            set
            {
                SetProperty(ref this.content, value);
            }
        }


        public QueryChartViewModel()
        {
            var now = DateTime.Now;
            DateQueryInfo = new DateQueryInfo(now.SetBeginOfMonth(), now, () => { this.CCount.RaiseCanExecuteChanged(); });

            this.CCount = new DelegateCommand(this.Count,this.CanCount);
            this.Count();

            InitCountType();

        }

        private void InitContent()
        {
            if (SelectCountType == null)
            {
                return;
            }
            if (SelectCountType.Type.Equals("Worker"))
            {
                if (Content != null && Content is WorkerChart)
                {
                    var workerChartViewModel = Content.DataContext as WorkerChartViewModel;
                    workerChartViewModel.Init(this.WorkerCountInfos.ToList());
                }
                else
                {
                    Content = new WorkerChart() { DataContext = new WorkerChartViewModel(this.WorkerCountInfos.ToList()) };
                }
            }
            else if (SelectCountType.Type.Equals("Route"))
            {
                if (Content != null && Content is RouteChart)
                {
                    var routeChartViewModel = Content.DataContext as RouteChartViewModel;
                    routeChartViewModel.Init(this.RouteCountInfos.ToList());
                }
                else
                {
                    Content = new RouteChart() { DataContext = new RouteChartViewModel(this.RouteCountInfos.ToList()) };
                }
            }
        }

        private void InitCountType()
        {
            //ChartTypes = new List<ChartType>();
            //ChartTypes.Add(new ChartType() { ShowName = "表格", Type = "Table" });
            //ChartTypes.Add(new ChartType() { ShowName = "饼图", Type = "Pie" });
            //ChartTypes.Add(new ChartType() { ShowName = "柱形图", Type = "Bar" });

            CountTypes = new List<CountType>();
            CountTypes.Add(new CountType() { ShowName = "人员", Type = "Worker" });
            CountTypes.Add(new CountType() { ShowName = "线路", Type = "Route" });

            //设置当前选项 为 表格
            //SelectChartType = ChartTypes.Find(type => { return type.Type.Equals("Table"); });
            //设置当前选中为 人员统计
            SelectCountType = CountTypes.Find(type => { return type.Type.Equals("Worker"); });
        }

        private void Count()
        {
            WorkerCountInfos = new ObservableCollection<CountInfo>(CountBLL.GetWorkerCountInfo(DateQueryInfo.Begin.ToNotNullable(), DateQueryInfo.End.ToNotNullable()));
            RouteCountInfos = new ObservableCollection<RouteCountInfo>(CountBLL.GetRouteCountInfo(DateQueryInfo.Begin.ToNotNullable(), DateQueryInfo.End.ToNotNullable()));
            InitContent();
            AppStatusViewModel.Instance.ShowInfo("统计完成", 3);
        }


        private bool CanCount()
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

        //public class ChartType
        //{
        //    public string ShowName { get; set; } //要在Combox中显示的字段,使用LanStrLoader来载入
        //    public string Type { get; set; } //Key
        //}

        public class CountType
        {
            public string ShowName { get; set; } //要在Combox中显示的字段,使用LanStrLoader来载入
            public string Type { get; set; } //Key
        }
    }
}
