using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model
{
    //地点
    public class Place : NotificationObject, ICloneable
    {
        private int id;
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                RaisePropertyChanged("ID");
            }
        }

        public int RouteID { get; set; }

        private int order;
        public int Order
        {
            get { return order; }
            set
            {
                order = value;
                RaisePropertyChanged("Order");
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string card;

        public string Card
        {
            get { return card; }
            set
            {
                InputChecker.CheckRfidCard(ref value);
                card = value;
                RaisePropertyChanged("Card");
            }
        }

        private string employeeNumber;
        public string EmployeeNumber
        {
            get { return employeeNumber; }
            set
            {
                InputChecker.CheckEmployeeNumber(ref value);
                employeeNumber = value;
                RaisePropertyChanged("EmployeeNumber");
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
