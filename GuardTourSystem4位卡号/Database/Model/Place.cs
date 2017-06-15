using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model
{
    //地点
    public class Place :BindableBase,ICloneable
    {
        private int id;
        public int ID
        {
            get { return id; }
            set
            {
                SetProperty(ref this.id, value);
            }
        }

        public int RouteID { get; set; }

        private int order;
        public int Order
        {
            get { return order; }
            set
            {
                SetProperty(ref this.order, value);
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
     

        public Place()
        {
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }

}
