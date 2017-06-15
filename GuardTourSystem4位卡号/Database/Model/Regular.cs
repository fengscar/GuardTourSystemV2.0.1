using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model
{
    public class Regular // 有规律的排班信息
    {
        public int FrequenceID { get; set; } // 班次ID, 每个班次只能有一个有规律拍板信息, 数据库中将 FrequenceID作为主键
        public HashSet<DayOfWeek> WeekDutys { get; set; }

        public Regular(Frequence f)
        {
            this.FrequenceID = f.ID;
            //this.WeekDutys = new List<DayOfWeek>();
            this.WeekDutys = new HashSet<DayOfWeek>();
        }

        // 获取该天是否需要巡逻
        public bool GetPatrol(DayOfWeek day)
        {
            return WeekDutys.Contains(day);
        }

        public bool GetPatrol(int dayOfWeekIndex)
        {
            return WeekDutys.ToList().Exists(dow =>
            {
                return dayOfWeekIndex == (int)dow;
            });
        }

        public void SetPatrol(bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun)
        {
            InitOneDay(DayOfWeek.Monday, mon);
            InitOneDay(DayOfWeek.Tuesday, tue);
            InitOneDay(DayOfWeek.Wednesday, wed);
            InitOneDay(DayOfWeek.Thursday, thu);
            InitOneDay(DayOfWeek.Friday, fri);
            InitOneDay(DayOfWeek.Saturday, sat);
            InitOneDay(DayOfWeek.Sunday, sun);
        }
        private void InitOneDay(DayOfWeek day, bool needPatrol)
        {
            if (needPatrol)
            {
                WeekDutys.Add(day);
            }
            else
            {
                WeekDutys.Remove(day);
            }
        }
    }
}
