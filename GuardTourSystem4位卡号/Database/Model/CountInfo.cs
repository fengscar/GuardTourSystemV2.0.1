using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.Model
{
    class CountInfo //一条 巡检员或 巡检点 的统计信息
    {
        public string CountName { get; set; } //巡检员/巡检点 名称
        public virtual int DutyCount { get; set; }//应巡次数
        public virtual int PatrolCount { get; set; }//实际次数

        public int MissCount//漏巡次数 = 应巡- 实际
        {
            get
            {
                return DutyCount - PatrolCount;
            }
            private set { }
        }
        public double PatrolPercent //出勤率  1-100
        {
            get
            {
                if (DutyCount == 0)
                {
                    return 0;
                }
                return (double)PatrolCount * 100 / DutyCount;
            }
            private set { }
        }

        public double MissPercent //缺勤率  1-100
        {
            get
            {
                if (DutyCount == 0)
                {
                    return 0;
                }
                return (double)MissCount * 100 / DutyCount;
            }
            private set { }
        }
    }
}
