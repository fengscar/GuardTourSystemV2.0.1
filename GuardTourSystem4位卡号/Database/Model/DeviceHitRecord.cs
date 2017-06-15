using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model
{
    //数据库中存放的 
    public class DeviceHitRecord :BindableBase
    {
        private DateTime readTime;

        public DateTime ReadTime
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

    

        public DeviceHitRecord(string d,DateTime dt)
        {
            this.Device = d;
            this.Time = dt;
        }
    }
}
