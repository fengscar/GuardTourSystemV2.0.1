using GuardTourSystem.Model;
using GuardTourSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GuardTourSystem.Selector
{
    // 无规律排班和有规律排班的 TemplateSelector
    class ChartSelector : DataTemplateSelector
    {
        public DataTemplate GridTemplate { get; set; }
        public DataTemplate PieTemplate { get; set; }
        public DataTemplate BarTemplate { get; set; }



        public string ChartType { get; set; }


        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            switch (ChartType)
            {
                case "Pie":
                    return PieTemplate;
                case "Bar":
                    return BarTemplate;
                default:
                    return GridTemplate;
            }
        }
    }
}
