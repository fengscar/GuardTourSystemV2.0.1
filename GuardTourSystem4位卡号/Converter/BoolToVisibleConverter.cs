using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GuardTourSystem.Converter
{
    // 根据输入的 value 来返回 Visibility
    //  null => Collapsed
    //  true => Visible
    //  false=> Hidden
    class BoolToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? visible = value as bool?;
            if (visible == true)
            {
                return Visibility.Visible;
            }
            else if (visible == false)
            {
                return Visibility.Hidden;
            }
            else
            {
                return Visibility.Collapsed;
            }

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return object.Equals(value, parameter);
        }
    }
}
