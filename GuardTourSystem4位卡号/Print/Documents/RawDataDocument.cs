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
    class RawDataDocument : AbstractDocument
    {
        private string QueryInfo { get; set; }

        public RawDataDocument(PrintData data, string queryInfo, FlowDocument doc = null)
            : base(data, doc)
        {
            this.QueryInfo = queryInfo;
        }
        public override void AddCustemTitle(TableRowGroup titleRowGroup)
        {
            var titleRow = new TableRow();
            titleRow.Cells.Add(GetDefaultCell("查询条件"));
            titleRow.Cells.Add(GetDefaultCell(QueryInfo));
            titleRowGroup.Rows.Add(titleRow);
        }

        public override void AddContent()
        {
            var list = (List<RawData>)Data.ContentList;
            if (list == null)
            {
                return;
            }
            int index = 1;
            foreach (var raw in list)
            {
                TableRow row = new TableRow();
                row.Cells.Add(this.GetStyleCell(index.ToString()));
                row.Cells.Add(this.GetStyleCell(raw.PlaceTime.ToString()));
                row.Cells.Add(this.GetStyleCell(raw.Device));
                row.Cells.Add(this.GetStyleCell(raw.Worker.Name));
                row.Cells.Add(this.GetStyleCell(raw.Worker.Card));
                row.Cells.Add(this.GetStyleCell(raw.RouteName));
                row.Cells.Add(this.GetStyleCell(raw.Place.Order.ToString()));
                row.Cells.Add(this.GetStyleCell(raw.Place.Name));
                row.Cells.Add(this.GetStyleCell(raw.Place.Card));

                var eventName = raw.Event == null ? "" : raw.Event.Name;
                row.Cells.Add(this.GetStyleCell(eventName));

                var eventCard = raw.Event == null ? "" : raw.Event.Card;
                row.Cells.Add(this.GetStyleCell(eventCard));

                ContentTableGroup.Rows.Add(row);
                index++;
            }
        }

        //总计大概650长度
        public override List<TableColumnHeader> GetColumns()
        {
            var list = new List<TableColumnHeader>();
            list.Add(new TableColumnHeader("序号", 30));
            list.Add(new TableColumnHeader("巡检时间", 120));
            list.Add(new TableColumnHeader("机号", 80));
            list.Add(new TableColumnHeader("巡检员", 60));
            list.Add(new TableColumnHeader("巡检员钮号", 80));
            list.Add(new TableColumnHeader("线路名称", 60));
            list.Add(new TableColumnHeader("地点序号", 30));
            list.Add(new TableColumnHeader("地点名称", 120));
            list.Add(new TableColumnHeader("地点钮号", 80));
            list.Add(new TableColumnHeader("事件名称", 60));
            list.Add(new TableColumnHeader("事件钮号", 80));
            return list;
        }
    }
}
