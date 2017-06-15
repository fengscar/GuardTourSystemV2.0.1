using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model
{
    public class Event : BindableBase,ICloneable
    {
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

        private int id;

        public int ID
        {
            get { return id; }
            set
            {
                SetProperty(ref this.id, value);
            }
        }

        public Event()
        {
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
