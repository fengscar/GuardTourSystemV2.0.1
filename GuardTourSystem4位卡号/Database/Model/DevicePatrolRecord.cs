using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model
{
    //数据库中存放的 巡检数据
    public class DevicePatrolRecord : BindableBase
    {
        private DateTime readTime;

        public DateTime ReadTime //接收时间
        {
            get { return readTime; }
            set
            {
                SetProperty(ref this.readTime, value);
            }
        }


        private DateTime time; //精确到秒
        public DateTime Time
        {
            get { return time; }
            set
            {
                SetProperty(ref this.time, value);
            }
        }

        private string device;

        public string Device
        {
            get { return device; }
            set
            {
                SetProperty(ref this.device, value);
            }
        }

        private string card;

        public string Card
        {
            get { return card; }
            set
            {
                SetProperty(ref this.card, value);
            }
        }

        public DevicePatrolRecord(string device, DateTime dt, string card, DateTime? readTime = null)
        {
            this.ReadTime = readTime ?? DateTime.Now;
            this.Device = device;
            this.Time = dt;
            this.Card = card;
        }
    }
}
