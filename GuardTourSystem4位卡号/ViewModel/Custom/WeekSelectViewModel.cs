using GuardTourSystem.Model;
using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.ViewModel.Custom
{
    public class WeekSelectViewModel : BindableBase
    {
        // 数据改变时 执行的 Action
        public Action DataChangeAction { get; set; }

        private bool selectAll;

        public bool SelectAll
        {
            get { return selectAll; }
            set
            {
                SetProperty(ref this.selectAll, value);
                Mon = value;
                Tue = value;
                Wed = value;
                Thu = value;
                Fri = value;
                Sat = value;
                Sun = value;
            }
        }


        #region 星期一 => 星期天的 Propn
        private bool mon;

        public bool Mon
        {
            get { return mon; }
            set
            {
                SetProperty(ref this.mon, value);
                NotifyDataChange();
            }
        }

        private bool tue;

        public bool Tue
        {
            get { return tue; }
            set
            {
                SetProperty(ref this.tue, value);
                NotifyDataChange();
            }
        }

        private bool wed;

        public bool Wed
        {
            get { return wed; }
            set
            {
                SetProperty(ref this.wed, value);
                NotifyDataChange();
            }
        }


        private bool thu;

        public bool Thu
        {
            get { return thu; }
            set
            {
                SetProperty(ref this.thu, value);
                NotifyDataChange();
            }
        }

        private bool fri;

        public bool Fri
        {
            get { return fri; }
            set
            {
                SetProperty(ref this.fri, value);
                NotifyDataChange();
            }
        }
        private bool sat;

        public bool Sat
        {
            get { return sat; }
            set
            {
                SetProperty(ref this.sat, value);
                NotifyDataChange();
            }
        }
        private bool sun;

        public bool Sun
        {
            get { return sun; }
            set
            {
                SetProperty(ref this.sun, value);
                NotifyDataChange();
            }
        }

        #endregion
        private void NotifyDataChange()
        {
            if (DataChangeAction != null)
            {
                DataChangeAction();
            }
        }


        public WeekSelectViewModel(Regular reg, Action onCheckChanged)
        {
            if (reg != null)
            {
                this.Sun = reg.GetPatrol(0);
                this.Mon = reg.GetPatrol(1);
                this.Tue = reg.GetPatrol(2);
                this.Wed = reg.GetPatrol(3);
                this.Thu = reg.GetPatrol(4);
                this.Fri = reg.GetPatrol(5);
                this.Sat = reg.GetPatrol(6);
            }
            this.DataChangeAction = onCheckChanged;
            this.DataChangeAction += selectChange;
        }
        private void selectChange()
        {
            if (Mon && Tue && Wed && Thu && Fri && Sat && Sun)
            {
                selectAll = true;
            }
            else
            {
                selectAll = false;
            }
            this.OnPropertyChanged("SelectAll");
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var viewmodel = obj as WeekSelectViewModel;
            if (viewmodel == null)
            {
                return false;
            }
            return this.Mon == viewmodel.mon
                && this.Tue == viewmodel.Tue
                && this.Wed == viewmodel.Wed
                && this.Thu == viewmodel.Thu
                && this.Fri == viewmodel.Fri
                && this.Sat == viewmodel.Sat
                && this.Sun == viewmodel.Sun;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
