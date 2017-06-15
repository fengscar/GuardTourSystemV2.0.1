using GuardTourSystem.Model;
using GuardTourSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.Calendar;

namespace GuardTourSystem.Selector
{
    class DayButtonTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate PatrolDayTemplate { get; set; } //工作日的样式
        public DataTemplate RestDayTemplate { get; set; }  //休息日的样式

        public List<Irregular> Irregulars { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var value =container.ToString();

            var calendarButton = item as CalendarButtonContent;
            if (calendarButton.ButtonType == CalendarButtonType.Date)
            {
                return calendarButton.IsSelected ? this.PatrolDayTemplate : this.RestDayTemplate;

            //    if (Irregulars == null || Irregulars.Count<=0)
            //    {
            //        return RestDayTemplate;
            //    }
            //    var tarIrregular = this.Irregulars.Find(ir => ir.YearMonth.Year == curDate.Year && ir.YearMonth.Month == curDate.Month);
            //    if (tarIrregular != null && tarIrregular.NeedPatrol(curDate.Day))
            //    {
            //        return this.PatrolDayTemplate;
            //    }
            //    else
            //    {
            //        return this.RestDayTemplate;
            //    }
            }

            return this.DefaultTemplate;
        }
    }
}
