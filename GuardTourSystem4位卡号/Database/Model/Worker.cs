using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model
{
    //计数员
    public class Worker : BindableBase, ICloneable
    {
        // 钮号 / 设备号 2选1
        private int id;

        public int ID
        {
            get { return id; }
            set
            {
                SetProperty(ref this.id, value);
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref this.name, value);
            }
        }


        private string card;

        public string Card
        {
            get { return card; }
            set
            {
                InputChecker.CheckRfidCard(ref value);
                SetProperty(ref this.card, value);
            }
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }


        public static Worker UndefineWorker //在排班时用到
        {
            get
            {
                return new Worker() { ID = -1, Name = "(不指定)" };
            }
        }
        public static Worker DefaultWorker //在显示时用到
        {
            get
            {
                return new Worker() { ID = -1, Name = "默认管理卡" };
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var worker = obj as Worker;
            if (worker == null)
            {
                return false;
            }
            if (worker.Card != null && !worker.Card.Equals(this.Card))
            {
                return false;
            }
            if (worker.Name != null && !worker.Name.Equals(this.Name))
            {
                return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

}
