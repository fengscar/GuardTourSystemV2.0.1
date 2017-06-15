using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model
{
    //班次
    public class Frequence : BindableBase, ICloneable
    {
        private int id;
        public int ID
        {
            get { return id; }
            set
            {
                SetProperty(ref this.id, value);
            }
        }

        private int routeID;
        public int RouteID
        {
            get { return routeID; }
            set
            {
                SetProperty(ref this.routeID, value);
            }
        }

        private string routeName;
        public string RouteName
        {
            get { return routeName; }
            set
            {
                SetProperty(ref this.routeName, value);
            }
        }

        private DateTime startDate;
        public DateTime StartDate //该班次的 开始日期 
        {
            get { return startDate; }
            set
            {
                SetProperty(ref this.startDate, value);
            }
        }
        private DateTime? endDate;
        public DateTime? EndDate //该班次的 结束日期
        {
            get { return endDate; }
            set
            {
                SetProperty(ref this.endDate, value);
            }
        }

        private DateTime generatedDate; // 该班次已生成到的日期,在新增一个班次时,该日期将设置为当前时间的前一天

        public DateTime GeneratedDate
        {
            get { return generatedDate; }
            set
            {
                SetProperty(ref this.generatedDate, value);
            }
        }

        private string name;
        public string Name//班次名称
        {
            get { return name; }
            set
            {
                SetProperty(ref this.name, value);
            }
        }

        private TimeSpan? start;
        public TimeSpan? StartTime//上班时间
        {
            get { return start; }
            set
            {
                SetProperty(ref this.start, value);
                this.OnPropertyChanged("PatrolCount");
            }
        }

        private bool nextDay;
        public bool NextDay //次日 checkbox
        {
            get
            {
                if (this.EndTime != null && this.StartTime != null)
                {
                    var startTime = (TimeSpan)this.StartTime;
                    var endTime = (TimeSpan)this.EndTime;
                    nextDay = endTime.Days > startTime.Days;
                    return nextDay;
                }
                else
                {
                    return nextDay;
                }
            }
            set
            {
                var oneDay = TimeSpan.FromDays(1);
                if (value && this.EndTime != null)
                {
                    this.endTime = ((TimeSpan)this.endTime).Add(oneDay);
                }
                else
                {
                    this.endTime = ((TimeSpan)this.endTime).Subtract(oneDay);
                }
                SetProperty(ref this.nextDay, value);
                this.OnPropertyChanged("PatrolCount");
            }
        }

        private TimeSpan? endTime;
        public TimeSpan? EndTime//下班时间
        {
            get { return endTime; }
            set
            {
                if (nextDay && value != null)
                {
                    value = ((TimeSpan)value).Add(TimeSpan.FromDays(1));
                }
                SetProperty(ref this.endTime, value);
                this.OnPropertyChanged("PatrolCount");
            }
        }

        private int patrolTime;
        public int PatrolTime //巡检时间-分钟
        {
            get { return patrolTime; }
            set
            {
                SetProperty(ref this.patrolTime, value);
                this.OnPropertyChanged("PatrolCount");
            }
        }

        private int restTime;
        public int RestTime//休息时间-分钟
        {
            get { return restTime; }
            set
            {
                SetProperty(ref this.restTime, value);
                this.OnPropertyChanged("PatrolCount");
            }
        }

        private int? patrolCount;
        public int? PatrolCount  //巡逻次数 (自动计算)
        {
            get
            {
                if (this.StartTime == null || this.EndTime == null)
                {
                    return null;
                }
                var startTime = (TimeSpan)this.StartTime;
                var endTime = (TimeSpan)this.endTime;
                var workSeconds = endTime.Subtract(startTime).TotalSeconds;
                if (workSeconds <= 0 || workSeconds >= 24 * 60 * 60 || this.PatrolTime <= 0) //如果超过24小时或者小于0 ,返回0
                {
                    return null;
                }
                var oneTimeSeconds = (this.PatrolTime + this.RestTime) * 60;

                patrolCount = (int)(workSeconds / oneTimeSeconds);
                patrolCount += workSeconds % oneTimeSeconds > 1 ? 1 : 0;
                return patrolCount;
            }
            set
            {
                SetProperty(ref this.patrolCount, value);
            }

        }

        private bool isRegular;
        public bool IsRegular // 是否是 有规律排班    ,默认为是    
        {
            get { return isRegular; }
            set
            {
                SetProperty(ref this.isRegular, value);
            }
        }


        //============================该班次的排班信息======================================//
        //当巡检员信息改变时 触发的Action. (有规律排班,无规律排班中选择不同巡检员将会改变Worker)
        private Action OnWorkerChange;
        public void SetWorkerChangeAction(Action action)
        {
            this.OnWorkerChange = action;
        }
        // 巡检员 ,默认是NULL(未指定)
        private Worker worker;
        public Worker Worker
        {
            get { return worker; }
            set
            {
                SetProperty(ref this.worker, value);
                if (OnWorkerChange != null)
                {
                    OnWorkerChange();
                }
            }
        }

        private Regular regular;
        public Regular Regular // 有规律排班的信息
        {
            get { return regular; }
            set
            {
                SetProperty(ref this.regular, value);
            }
        }

        private Irregular irregular;
        public Irregular Irregular  //表示 所有月份的无规律排班
        {
            get { return irregular; }
            set
            {
                SetProperty(ref this.irregular, value);
            }
        }

        public Frequence()
        {
            this.RouteID = -1;
        }

        public Frequence(DateTime StartFreq, DateTime? EndFreq, int startWorkHour, int startWorkerMin, int endWorkHour, int endWorkMin, int patrolTimeMin, int restTimeMin)
        {
            this.GeneratedDate = StartFreq.AddDays(-1);//设置为前一天,这样 当系统要生成当天值班表时,可以正确生成
            this.StartDate = StartFreq;
            this.EndDate = EndFreq;
            this.StartTime = new TimeSpan(startWorkHour, startWorkerMin, 0);
            this.EndTime = new TimeSpan(endWorkHour, endWorkMin, 0);
            this.PatrolTime = patrolTimeMin;
            this.RestTime = restTimeMin;
            this.RouteID = -1;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        // 输入一个日期, 返回该班次在该日期所有的 巡逻时间
        public List<DutyTime> GetDutyTime(DateTime date)
        {
            //将 日期设置为0时
            date = date.Subtract(date.TimeOfDay);
            var result = new List<DutyTime>();
            //1.验证是否需要巡逻
            if (this.IsRegular && !Regular.GetPatrol(date.DayOfWeek))   // 按周排班,查看该天是星期几即可
            {
                return result;
            }
            if (!this.IsRegular && !Irregular.HasPlan(date))  //按月排班,使用 Irregular的方法来查询
            {
                return result;
            }
            //2.需要巡逻的话,返回该天所有 单次巡逻的 开始时间/结束时间
            var oneDutyStart = date.Add((TimeSpan)this.StartTime);
            var oneDutyEnd = oneDutyStart.AddMinutes(patrolTime);
            var finishWorkTime = date.Add((TimeSpan)this.EndTime); //该天的下班时间
            while (oneDutyStart < finishWorkTime) //还未下班..
            {
                result.Add(new DutyTime(oneDutyStart, oneDutyEnd));

                oneDutyStart = oneDutyEnd.AddMinutes(restTime);
                oneDutyEnd = oneDutyStart.AddMinutes(patrolTime);
            }
            return result;
        }

     
    }
}
