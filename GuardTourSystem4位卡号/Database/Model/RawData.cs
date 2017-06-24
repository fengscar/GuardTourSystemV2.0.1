using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model
{
    public class RawData : ICloneable
    {
        public DateTime TRead { get; set; } //读取到软件的时间

        public string Device { get; set; } //计数机的机号

        //public  Worker Worker { get; set; }

        public string RouteName { get; set; }
        public Place Place { get; set; }
        public DateTime PlaceTime { get; set; }

        //public Event Event { get; set; }
        //public DateTime? EventTime { get; set; }


        override public string ToString()
        {
            string str = String.Empty;
            str = String.Concat(str, "TRead = ", TRead, "\r\n");
            str = String.Concat(str, "Device = ", Device, "\r\n");
            //str = String.Concat(str, "Worker = ", Worker, "\r\n");
            str = String.Concat(str, "RouteName = ", RouteName, "\r\n");
            str = String.Concat(str, "Place = ", Place, "\r\n");
            str = String.Concat(str, "PlaceTime = ", PlaceTime, "\r\n");
            //str = String.Concat(str, "Event = ", Event, "\r\n");
            //str = String.Concat(str, "EventTime = ", EventTime, "\r\n");
            return str;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    
    
    //数据库中对应 0, 1,2,3
    public enum CardType //卡类别
    {
        //人员卡,地点卡,事件卡,未知
        Worker, Place, Event, Unknown
    }
}
