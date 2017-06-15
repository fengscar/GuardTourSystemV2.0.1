using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace GuardTourSystem.Print
{
    class FrequenceDocument : AbstractDocument
    {
        public FrequenceDocument(PrintData data, FlowDocument doc = null)
            : base(data, doc)
        {

        }

        public override void AddContent()
        {
            var columnCount = Data.DataTable.Columns.Count;
            foreach (DataRow dataRow in Data.DataTable.Rows)
            {
                TableRow row = new TableRow();
                for (int i = 0; i < columnCount; i++)
                {
                    row.Cells.Add(this.GetStyleCell(dataRow[i] == null ? "" : dataRow[i].ToString()));
                }
                ContentTableGroup.Rows.Add(row);
            }
            //foreach (var freq in list)
            //{
            //    TableRow row = new TableRow();
            //    row.Cells.Add(this.GetStyleCell(freq.RouteName));
            //    row.Cells.Add(this.GetStyleCell(freq.Name));
            //    row.Cells.Add(this.GetStyleCell(freq.IsRegular?"按周排班"));
            //    row.Cells.Add(this.GetStyleCell(freq.RouteName));
            //    row.Cells.Add(this.GetStyleCell(freq.RouteName));
            //    row.Cells.Add(this.GetStyleCell(worker.Name));
            //    row.Cells.Add(this.GetStyleCell(worker.Name));
            //    row.Cells.Add(this.GetStyleCell(worker.Name));
            //    row.Cells.Add(this.GetStyleCell(worker.Card));

            //    ContentTableGroup.Rows.Add(row);
            //    index++;
            //}
        }

        public override List<TableColumnHeader> GetColumns()
        {
            var list = new List<TableColumnHeader>();
            list.Add(new TableColumnHeader("巡检线路", 80));
            list.Add(new TableColumnHeader("班次名称", 80));
            list.Add(new TableColumnHeader("排班方式", 100));
            list.Add(new TableColumnHeader("上班时间", 80));
            list.Add(new TableColumnHeader("下班时间", 100));
            list.Add(new TableColumnHeader("巡检时间", 80));
            list.Add(new TableColumnHeader("休息时间", 80));
            list.Add(new TableColumnHeader("巡逻次数", 80));
            return list;
        }
    }
}
