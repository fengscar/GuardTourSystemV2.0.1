using GuardTourSystem.Model.Model;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model
{
    //计数值班表 
    public class Duty:BindableBase
    {
        public int ID { get; set; }

        public Frequence Frequence { get; set; }

        //2017年2月20日15:31:25 为了方便删除值班表时新增
        public DateTime DutyDate { get; set; }

        public DutyTime DutyTime { get; set; }

        public Worker Worker { get; set; } //可能为空( 不指定)

        public List<Record> Records { get; set; }
    }
    //一次巡逻的 时间
    public class DutyTime
    {
        public DateTime PatrolBegin { get; set; } // 本次巡逻的开始时间 ( 日期+时间)
        public DateTime PatrolEnd { get; set; }  //本次巡逻的结束时间

        public DutyTime(DateTime begin, DateTime end)
        {
            this.PatrolBegin = begin;
            this.PatrolEnd = end;
        }

        //指定时间 是否在值班时间内
        public bool InDuty(DateTime datetime)
        {
            return PatrolBegin < datetime && PatrolEnd > datetime;
        }
    }
}
