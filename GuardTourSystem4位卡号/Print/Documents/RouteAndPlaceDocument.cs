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
    class RouteAndPlaceDocument : AbstractDocument
    {
        public RouteAndPlaceDocument(PrintData data, FlowDocument doc = null)
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
            var routes = (List<Route>)Data.ContentList;
            if (routes == null)
            {
                return;
            }
            foreach (var route in routes)
            {
                //add route rows
                TableRow routeRow = new TableRow();
                routeRow.Background = Brushes.LightGray;
                var routecell = GetStyleHeader(route.RouteName);
                routecell.Padding = new Thickness(10);
                routecell.ColumnSpan = 3;
                routeRow.Cells.Add(routecell);
                ContentTableGroup.Rows.Add(routeRow);

                //add headers
                base.AddColumnHeader();

                //add place rows
                int index = 1;
                foreach (var place in route.Places)
                {
                    TableRow placeRow = new TableRow();
                    placeRow.Cells.Add(this.GetStyleCell(index.ToString()));
                    placeRow.Cells.Add(this.GetStyleCell(place.Name));
                    placeRow.Cells.Add(this.GetStyleCell(place.Card));

                    ContentTableGroup.Rows.Add(placeRow);
                    index++;
                }
            }
        }

        public override List<TableColumnHeader> GetColumns()
        {
            var list = new List<TableColumnHeader>();
            list.Add(new TableColumnHeader("人员序号", 100));
            list.Add(new TableColumnHeader("人员名称", 200));
            list.Add(new TableColumnHeader("人员钮号", 200));
            return list;
        }
    }
}
