﻿using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model
{
    //数据库中存放的 计数数据
    public class DevicePatrolRecord : NotificationObject
    {
        private DateTime readTime;

        public DateTime ReadTime //接收时间
        {
            get { return readTime; }
            set
            {
                readTime = value;
                RaisePropertyChanged("ReadTime");
                //SetProperty(ref this.readTime, value);
            }
        }


        private DateTime time; //精确到秒
        public DateTime Time
        {
            get { return time; }
            set
            {
                time = value;
                RaisePropertyChanged("Time");
                //SetProperty(ref this.time, value);
            }
        }

        private string device;

        public string Device
        {
            get { return device; }
            set
            {
                device = value;
                RaisePropertyChanged("Device");
                //SetProperty(ref this.device, value);
            }
        }

        private string card;

        public string Card
        {
            get { return card; }
            set
            {
                card = value;
                RaisePropertyChanged("Card");
                //SetProperty(ref this.card, value);
            }
        }

        public DevicePatrolRecord(string device, DateTime dt, string card, DateTime? readTime = null)
        {
            ReadTime = readTime ?? DateTime.Now;
            Device = device;
            Time = dt;
            Card = card;
        }
    }
}
