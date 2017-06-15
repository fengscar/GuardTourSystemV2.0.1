using GuardTourSystem.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GuardTourSystem.Converter
{
    //  修改 , 删除 功能的转换类 ,根据当前的 value 实时转换
    class FilterValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return LanLoader.Load(LanKey.FilterValueNull);
            }
            if (value is string)
            {
                var str = value as string;
                if (string.IsNullOrEmpty(str))
                {
                    return LanLoader.Load(LanKey.FilterValueEmpty);
                }
            }
            return value.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return object.Equals(value, parameter);
        }
    }
}
