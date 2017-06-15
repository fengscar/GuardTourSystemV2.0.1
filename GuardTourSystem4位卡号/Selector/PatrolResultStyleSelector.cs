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
    class PatrolResultStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item == null || !(item is GuardTourSystem.ViewModel.QueryResultViewModel.ResultItem))
            {
                return MissDutyStyle;
            }
            var resultItem = item as GuardTourSystem.ViewModel.QueryResultViewModel.ResultItem;
            return resultItem.PatrolTime == null ? MissDutyStyle : OnDutyStyle;
        }

        public Style OnDutyStyle { get; set; }
        public Style MissDutyStyle { get; set; }
    }
}
