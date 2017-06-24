using GuardTourSystem.Model;
using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model
{
    //计数 线路
    public class Route :NotificationObject,ICloneable
    {
        public int ID { get; set; }

        private string name;
        public string RouteName
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("RouteName");
            }
        }

        //public List<Frequence> Frequences { get; set; } //2个班次信息 .  其中 每个班次又有n次巡逻安排 (根据开始/结束/间隔时间生成)

        private ObservableCollection<Place> places;
        public ObservableCollection<Place> Places
        {
            get { return places; }
            set
            {
                places = value;
                RaisePropertyChanged("Places");
            }
        }



        public Route()
        {
            //this.Frequences = new List<Frequence>();
            Places = new ObservableCollection<Place>();
        }


        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
