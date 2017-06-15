using GuardTourSystem.Model;
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
    class CardTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (CardType)value;
            switch (type)
            {
                case CardType.Worker:
                    return "人员卡";
                case CardType.Place:
                    return "地点卡";
                case CardType.Event:
                    return "事件卡";
                case CardType.Unknown:
                default:
                    return "无效卡";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return object.Equals(value, parameter);
        }
    }
}
