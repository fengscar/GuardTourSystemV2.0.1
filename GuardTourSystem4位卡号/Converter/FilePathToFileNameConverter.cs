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
    class FilePathToFileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            var path = value as string;
            if (path == null)
            {
                return null;
            }
            var index = path.LastIndexOf(@"\");
            if (index <= 0)
            {
                return null;
            }
            return path.Substring(index+1);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return object.Equals(value, parameter);
        }
    }
}
