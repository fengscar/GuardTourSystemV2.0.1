using GuardTourSystem.Model;
using GuardTourSystem.Model.Model;
using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model
{
    //巡检 线路
    public class Route :BindableBase,ICloneable
    {
        public int ID { get; set; }

        private string name;
        public string RouteName
        {
            get { return name; }
            set
            {
                SetProperty(ref this.name, value);
            }
        }

        public List<Frequence> Frequences { get; set; } //2个班次信息 .  其中 每个班次又有n次巡逻安排 (根据开始/结束/间隔时间生成)

        public ObservableCollection<Place> Places { get; set; } // 有5个地点


        public Route()
        {
            this.Frequences = new List<Frequence>();
            this.Places = new ObservableCollection<Place>();
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
