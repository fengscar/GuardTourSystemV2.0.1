using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace GuardTourSystem.Print
{
    class HitDocument : AbstractDocument
    {
        public HitDocument(PrintData data, FlowDocument doc = null)
            : base(data, doc)
        {

        }

        public override void AddContent()
        {
            var list = (List<DeviceHitRecord>)Data.ContentList;
            if (list == null)
            {
                return;
            }
            int index = 1;
            foreach (var hit in list)
            {
                TableRow row = new TableRow();
                row.Cells.Add(this.GetStyleCell(index.ToString()));
                row.Cells.Add(this.GetStyleCell(hit.Device));
                row.Cells.Add(this.GetStyleCell(hit.Time.ToString()));

                ContentTableGroup.Rows.Add(row);
                index++;
            }
        }

        public override List<TableColumnHeader> GetColumns()
        {
            var list = new List<TableColumnHeader>();
            list.Add(new TableColumnHeader("序号", 100));
            list.Add(new TableColumnHeader("机号", 200));
            list.Add(new TableColumnHeader("敲击时间", 200));
            return list;
        }
    }
}
