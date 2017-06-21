using GuardTourSystem.Database.Model;
using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace GuardTourSystem.Print
{
    class RouteGridChartDocument : AbstractDocument
    {
        public RouteGridChartDocument(PrintData data, FlowDocument doc = null)
            : base(data, doc)
        {

        }
        /// <summary>
        /// 不需要,直接在AddContent()中添加Header
        /// </summary>
        protected override void AddColumnHeader()
        {

        }


        public override void AddContent()
        {
            var routes = (List<RouteCountInfo>)Data.ContentList;
            if (routes == null)
            {
                return;
            }
            foreach (var route in routes)
            {
                //add route rows
                TableRow routeRow = new TableRow();
                routeRow.Background = Brushes.LightGray;
                var routecell = GetStyleHeader(route.CountName);
                routecell.Padding = new Thickness(10);
                routecell.ColumnSpan = 3;
                routeRow.Cells.Add(routecell);
                ContentTableGroup.Rows.Add(routeRow);

                if (route.PlaceCountInfos != null && route.PlaceCountInfos.Count != 0)
                {
                    //add place headers
                    base.AddColumnHeader();
                }

                //add place rows
                foreach (var place in route.PlaceCountInfos)
                {
                    TableRow placeRow = new TableRow();
                    placeRow.Cells.Add(this.GetStyleCell(place.CountName));
                    placeRow.Cells.Add(this.GetStyleCell(place.DutyCount.ToString()));
                    placeRow.Cells.Add(this.GetStyleCell(place.PatrolCount.ToString()));
                    placeRow.Cells.Add(this.GetStyleCell(place.MissCount.ToString()));
                    placeRow.Cells.Add(this.GetStyleCell(place.PatrolPercent.ToString("#0.##") + "%"));
                    placeRow.Cells.Add(this.GetStyleCell(place.MissPercent.ToString("#0.##") + "%"));

                    ContentTableGroup.Rows.Add(placeRow);
                }
            }
        }

        public override List<TableColumnHeader> GetColumns()
        {
            var list = new List<TableColumnHeader>();
            list.Add(new TableColumnHeader("人员名称", 150));
            list.Add(new TableColumnHeader("应打卡次数", 100));
            list.Add(new TableColumnHeader("实打卡次数", 100));
            list.Add(new TableColumnHeader("漏打卡次数", 100));
            list.Add(new TableColumnHeader("出勤率", 80));
            list.Add(new TableColumnHeader("缺勤率", 80));
            return list;
        }
    }
}
