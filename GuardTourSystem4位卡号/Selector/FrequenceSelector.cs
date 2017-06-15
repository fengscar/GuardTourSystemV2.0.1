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
    class FrequenceSelector : DataTemplateSelector
    {
        public DataTemplate RegularTemplate { get; set; }
        public DataTemplate IrregularTemplate { get; set; }


        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var freq = item as IrregularItemViewModel;
            if (freq == null)
            {
                return RegularTemplate;
            }
            return freq.Frequence.IsRegular ? RegularTemplate : IrregularTemplate;
        }
    }
}
