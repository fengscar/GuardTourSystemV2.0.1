﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GuardTourSystem.Converter
{
    // 将 地点序号为 0 转成 " "
    class ZeroToEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var order = (int)value;
                if (order == 0)
                {
                    return "";
                }
            }
            return value + "";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return object.Equals(value, parameter);
        }
    }
}
