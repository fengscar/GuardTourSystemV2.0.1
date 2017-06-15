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
    class ResultDocument : AbstractDocument
    {
        private string QueryInfo { get; set; }

        public ResultDocument(PrintData data, string queryInfo, FlowDocument doc = null)
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
            //var list = (List<GuardTourSystem.ViewModel.QueryResultViewModel.ResultItemViewModel>)Data.ContentList;
            //if (list == null)
            //{
            //    return;
            //}
            //int index = 1;
            //foreach (var raw in list)
            //{
            //    TableRow row = new TableRow();
            //    row.Cells.Add(this.GetStyleCell(index.ToString()));
            //    row.Cells.Add(this.GetStyleCell(raw.RouteName));
            //    row.Cells.Add(this.GetStyleCell(raw.FrequenceName));
            //    row.Cells.Add(this.GetStyleCell(raw.PlanTime));
            //    row.Cells.Add(this.GetStyleCell(raw.WorkerName));
            //    row.Cells.Add(this.GetStyleCell(raw.PlaceName));
            //    row.Cells.Add(this.GetStyleCell(raw.PatrolTime.ToString()));
            //    row.Cells.Add(this.GetStyleCell(raw.PatrolResult));
            //    row.Cells.Add(this.GetStyleCell(raw.EventName));

            //    ContentTableGroup.Rows.Add(row);
            //    index++;
            //}
        }

        //总计大概650长度
        public override List<TableColumnHeader> GetColumns()
        {
            var list = new List<TableColumnHeader>();
            list.Add(new TableColumnHeader("序号", 30));
            list.Add(new TableColumnHeader("线路", 60));
            list.Add(new TableColumnHeader("班次", 60));
            list.Add(new TableColumnHeader("计划时间", 100));
            list.Add(new TableColumnHeader("巡检员", 80));
            list.Add(new TableColumnHeader("地点名称", 100));
            list.Add(new TableColumnHeader("实训时间", 100));
            list.Add(new TableColumnHeader("考核结果", 40));
            list.Add(new TableColumnHeader("事件", 80));
            return list;
        }
    }
}
