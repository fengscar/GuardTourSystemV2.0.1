using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GuardTourSystem.Converter
{
    class RegularToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var regular = value as Regular;
            var dayOfWeekIndex = System.Convert.ToInt32(parameter);
            if (regular == null || dayOfWeekIndex < 0)
            {
                return false;
            }
            return regular.GetPatrol(dayOfWeekIndex);

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
