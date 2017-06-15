using GuardTourSystem.Model;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.ViewModel
{
    // 按月排班 日期勾选 控件 的ViewModel
    // 有以下功能和属性
    //1. MonthPlan : 当前控件所显示的 年份+月份 + 计划
    //2. DayViewModel : 当前显示月份中的 每一天 所对应的 VM 
    //3. ShowMonth() : 显示指定月份的计划
    public class MonthSelectViewModel : BindableBase
    {
        /// <summary>
        /// 日期勾选控件中的 每一天 所对应的ViewModel
        /// </summary>
        public class DayViewModel : BindableBase
        {
            /// <summary>
            /// 当该天是否被选中 改变后的 回调
            /// </summary>
            private Action OnCheckChanged;

            /// <summary>
            /// 该天是否 被选中
            /// </summary>
            private bool selected;
            public bool Selected
            {
                get { return selected; }
                set
                {
                    SetProperty(ref this.selected, value);
                    if (OnCheckChanged != null)
                    {
                        OnCheckChanged();
                    }
                }
            }

            /// <summary>
            /// 该天能否被改变 (今天之前的都不行)
            /// </summary>
            private bool enabled;
            public bool Enabled
            {
                get { return enabled; }
                set
                {
                    SetProperty(ref this.enabled, value);
                }
            }

            /// <summary>
            /// 该天是 几号
            /// </summary>
            private int day;
            public int Day
            {
                get { return day; }
                set
                {
                    SetProperty(ref this.day, value);
                }
            }

            public DayViewModel(int day, bool select, bool enable, Action checkChange)
            {
                this.Day = day;
                this.Selected = select;
                this.Enabled = enable;
                this.OnCheckChanged = checkChange;
            }
        }

        public Action DataChangeAction { get; set; }

        public ObservableCollection<DayViewModel> DayItems { get; set; }

        private bool selectAll;

        public bool SelectAll
        {
            get { return selectAll; }
            set
            {
                //如果值没有改变,return 
                var changed = value != selectAll;
                if (!changed)
                {
                    return;
                }
                SetProperty(ref this.selectAll, value);

                //只改变 能改变的值
                foreach (var item in DayItems)
                {
                    if (item.Enabled)
                    {
                        item.Selected = value;
                    }
                }
            }
        }

        /// <summary>
        /// 当前显示的 月份计划
        /// </summary>
        private MonthPlan monthPlan;
        public MonthPlan MonthPlan
        {
            get
            {
                UpdateMonthPlanFromUI(); //获取计划时, 先从UI更新数据
                return monthPlan;
            }
            set
            {
                SetProperty(ref this.monthPlan, value);
                UpdateUIFromMonthPlan(); //设置完计划后, 更新UI
            }
        }


        public MonthSelectViewModel(MonthPlan mp, Action dataChange)
        {
            DayItems = new ObservableCollection<DayViewModel>();
            ShowMonth(mp);

            DataChangeAction = dataChange;
        }

        //显示参数的数据
        public void ShowMonth(MonthPlan monthPlan)
        {
            MonthPlan = monthPlan;
        }

        private void UpdateMonthPlanFromUI()
        {
            foreach (var day in DayItems)
            {
                monthPlan.SetPlan(day.Day, day.Selected);
            }
        }


        public void UpdateUIFromMonthPlan()
        {
            //新的月份的天数
            var dayCount = monthPlan.GetDaysInMonth();
            //如果当前月份天数比较多,移除 左后多出来的那几天.
            if (DayItems.Count > dayCount)
            {
                for (int i = DayItems.Count; i >= dayCount; i--)
                {
                    DayItems.RemoveAt(i - 1);
                }
            }

            //初始化每一项.如果新的月份天数比较多,将新增.否则将修改原来的
            for (int i = 1; i <= dayCount; i++)
            {
                if (DayItems.Count < i)
                {
                    DayItems.Add(new DayViewModel(i, monthPlan.HasPlan(i), !monthPlan.IsPassed(i), this.DataChangeAction + this.OnDayItemSelectChanged));
                }
                else
                {
                    DayItems[i - 1].Selected = monthPlan.HasPlan(i);
                    DayItems[i - 1].Enabled = !monthPlan.IsPassed(i);
                }
            }
        }


        private void OnDayItemSelectChanged()
        {
            // 使用手动触发...(改变selectAll的值,然后OnPropertyChanged,避免触发 SelectAll中 Set的For循环)
            selectAll = DayItems.ToList()
                .FindAll(item => item.Enabled) //找到所有可以修改的值
                .All(item => item.Selected); //判断是否都被选中
            this.OnPropertyChanged("SelectAll");
        }
    }
}
