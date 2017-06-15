using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace GuardTourSystem.Print
{
    class EventDocument : AbstractDocument
    {
        public EventDocument(PrintData data, FlowDocument doc = null)
            : base(data, doc)
        {

        }
        public override void AddContent()
        {
            var list = (List<Event>)Data.ContentList;
            if (list == null)
            {
                return;
            }
            int index = 1;
            foreach (var eventItem in list)
            {
                TableRow row = new TableRow();
                row.Cells.Add(this.GetStyleCell(index.ToString()));
                row.Cells.Add(this.GetStyleCell(eventItem.Name));
                row.Cells.Add(this.GetStyleCell(eventItem.Card));

                ContentTableGroup.Rows.Add(row);
                index++;
            }
        }

        public override List<TableColumnHeader> GetColumns()
        {
            var list = new List<TableColumnHeader>();
            list.Add(new TableColumnHeader("序号", 100));
            list.Add(new TableColumnHeader("事件名称", 200));
            list.Add(new TableColumnHeader("事件钮号", 200));
            return list;
        }
    }
}
