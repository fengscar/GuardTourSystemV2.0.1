using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Utils
{
    public class DateQueryInfo : NotificationObject
    {
        /// <summary>
        /// 查询的 起始时间
        /// </summary>
        private DateTime? begin;
        public DateTime? Begin
        {
            get { return begin; }
            set
            {
                //if (value != null)
                //{
                //    value = value.ToNotNullable().SetBeginOfDay();
                //}
                begin = value;
                RaisePropertyChanged("Begin");
                if (OnDateChange != null)
                {
                    OnDateChange();
                }
            }
        }
        /// <summary>
        /// 查询的结束时间
        /// </summary>
        private DateTime? end;
        public DateTime? End
        {
            get { return end; }
            set
            {
                //if (value != null)
                //{
                //    value = value.ToNotNullable().SetEndOfDay();
                //}
                end = value;
                RaisePropertyChanged("End");
                //SetProperty(ref this.end, value);
                if (OnDateChange != null)
                {
                    OnDateChange();
                }
            }
        }

        public Action OnDateChange { get; set; }


        public DateQueryInfo(DateTime begin, DateTime end, Action onDateChange)
        {
            Begin = begin;
            End = end;
            OnDateChange = onDateChange;
        }

        public bool CanQuery(out string error)
        {
            if (Begin == null)
            {
                error = LanLoader.Load(LanKey.QueryDateErrorBegin);
                return false;
            }
            if (End == null)
            {
                error = LanLoader.Load(LanKey.QueryDateErrorEnd);
                return false;
            }
            if (End.ToNotNullable().Subtract(Begin.ToNotNullable()).TotalSeconds < 0)
            {
                error = LanLoader.Load(LanKey.QueryDateErrorDate);
                return false;
            }
            error = null;
            return true;
        }

        public string GetQueryTime()
        {
            return Begin.ToNotNullable().ToShortDateString() + " " + LanLoader.Load(LanKey.QueryDateTo) + " " + End.ToNotNullable().ToShortDateString();
        }
    }
}
