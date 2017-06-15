using GuardTourSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuardTourSystem.Model
{
    /// <summary>
    /// 一个班次的无规律排班信息( 所有月份的 )
    /// </summary>
    public class Irregular
    {
        public int ID { get; set; }
        public int FrequenceID { get; set; }

        public List<MonthPlan> MonthPlans { get; set; }// 所有月份的排班计划

        //获取 指定月份的 排班计划 
        // 如果已经有,返回在List中查找到的值 ; 
        // 如果没有, 新生成一个加入到List中 并返回
        public MonthPlan GetMonthPlan(DateTime yearMonth)
        {
            var result = MonthPlans.Find(mp => { return mp.IsIdenticalMonth(yearMonth); });
            if (result == null)
            {
                result = new MonthPlan(yearMonth);
                MonthPlans.Add(result);
            }
            return result;
        }

        // 更新一个月份的 计划,有则修改,无则添加
        public void UpdateMonthPlan(MonthPlan mp)
        {
            //查看是否已有该计划
            var result = MonthPlans.FindIndex(find => { return find.IsIdenticalMonth(mp.YearMonth); });
            if (result == -1)
            {
                MonthPlans.Add(mp);
            }
            else
            {
                MonthPlans[result] = mp;
            }
        }

        public Irregular(Frequence f)
        {
            this.FrequenceID = f.ID;
            this.MonthPlans = new List<MonthPlan>();
        }
        //获取指定日期是否需要巡逻
        public bool HasPlan(DateTime date)
        {
            var result = MonthPlans.Find(mp => { return mp.IsIdenticalMonth(date); });
            if (result == null)
            {
                return false;
            }
            else
            {
                return result.HasPlan(date.Day);
            }
        }
    }

    // 1个月份的 排班情况
    public class MonthPlan
    {
        public DateTime YearMonth { get; set; } //具体的年月
        public Int32 Plan { get; set; } // 使用2^31位来 表示 31天的排班情况  最低位为每月的第一天.

        public MonthPlan(DateTime yearMonth, Int32 planValue = 0)
        {
            this.YearMonth = new DateTime(yearMonth.Year, yearMonth.Month, 1);
            this.Plan = planValue;
        }

        /// <summary>
        /// 设置 第 day 天是否需要巡逻
        /// 如果是, 设置该天对应的Int32位置为 1, 使用 | (或运算符)
        /// 如果否, 设置该天对应的Int32位置为 0, 使用 & (与运算符)
        /// </summary>
        /// <param name="day"></param>
        /// <param name="needPatrol"></param>
        public void SetPlan(int day, bool needPatrol)
        {
            int param;
            if (needPatrol)
            {
                param = (int)Math.Pow(2, (day - 1));
                Plan = Plan | param;
            }
            else
            {
                param = (int)Math.Pow(2, 32) - 1;//设置Int32每一位为1
                param -= (int)Math.Pow(2, (day - 1)); // 将该天Int32位置设置为0
                Plan = Plan & param;
            }
        }

        /// <summary>
        /// 判断第 day天是否需要巡逻
        /// 将该天与 param:000010...000 取与
        /// 如果param值没变, 表示该天需要巡逻</summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public bool HasPlan(int day)
        {
            if (day <= 0 || day > 31)
            {
                throw new Exception("无法获取 第" + day + "天的计划,请传入1-31内的值");
            }
            int param = (int)Math.Pow(2, (day - 1));
            return (Plan & param) == param;
        }

        /// <summary>
        /// 判断第 day 天是否已经过去
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public bool IsPassed(int day)
        {
            return DateTime.Now.Subtract(DateTime.Now.TimeOfDay).Subtract(YearMonth.AddDays(day - 1)).TotalDays > 0;
        }

        /// <summary>
        /// 该对象对应的年份+月份,是否与参数相同
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsIdenticalMonth(DateTime date)
        {
            return this.YearMonth.Year == date.Year && this.YearMonth.Month == date.Month;
        }

        /// <summary>
        /// 获取该月份的天数
        /// </summary>
        /// <returns></returns>
        public int GetDaysInMonth()
        {
            return DateTime.DaysInMonth(this.YearMonth.Year, this.YearMonth.Month);
        }
    }
}
