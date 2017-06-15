using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.Model
{
    class RouteCountInfo : CountInfo //一条巡检线路 统计信息
    {
        public List<CountInfo> PlaceCountInfos { get; set; } // 所有属于该线路的地点的 统计信息 

        public RouteCountInfo()
        {
            PlaceCountInfos = new List<CountInfo>();
        }
    }
}
