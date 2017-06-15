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
    public class TableColumnHeader
    {
        public string Text;
        public double Width;
        public TableColumnHeader(string text, double width)
        {
            this.Text = text;
            this.Width = width;
        }
    }
    /// <summary>
    /// 每个文档分为两部分 :
    /// 1 描述 (包括 打印的内容标题,打印时间,记录数等), 
    /// 2.具体的数据 ( 包括header和 每一条数据对应的 row)
    /// </summary>
    public abstract class AbstractDocument
    {
        public FlowDocument FlowDocument { get; set; }
        public PrintData Data { get; set; }

        public TableRowGroup TitleTableGroup; //描述表的TableRowGroup
        public Table ContentTable; // 数据表
        public TableRowGroup ContentTableGroup; //数据表默认的TableRowGroup

        public Style HeaderStyle;  //列Header的Style
        public Style CellStyle;  //数据单元格的Style

        public AbstractDocument(PrintData data, FlowDocument doc = null)
        {
            this.Data = data;
            this.FlowDocument = doc != null ? doc : (FlowDocument)Application.LoadComponent(new Uri("/Print/FlowDocument.xaml", UriKind.RelativeOrAbsolute));
            this.FlowDocument.PagePadding = new Thickness(50, 50, 50, 30);
            this.FlowDocument.DataContext = data;
        }

        /// <summary>
        /// 不要重写该方法
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="data"></param>
        public void Render()
        {
            InitTableAndStyle();

            AddCustemTitle(this.TitleTableGroup);

            AddTableColumn();
            AddColumnHeader();
            AddContent();
        }

        /// <summary>
        /// 从 流文档的 xaml中获取style和table
        /// </summary>
        protected void InitTableAndStyle()
        {
            TitleTableGroup = FlowDocument.FindName("TitleTableRowGroup") as TableRowGroup;
            ContentTable = FlowDocument.FindName("ContentTable") as Table;
            ContentTableGroup = FlowDocument.FindName("ContentTableRowGroup") as TableRowGroup;
            HeaderStyle = FlowDocument.Resources["HeaderStyle"] as Style;
            CellStyle = FlowDocument.Resources["CellStyle"] as Style;
        }

        /// <summary>
        /// 初始化 表的列
        /// </summary>
        protected virtual void AddTableColumn()
        {
            var columns = GetColumns();
            if (columns != null && columns.Count != 0)
            {
                foreach (var tableColumn in columns)
                {
                    //新增Column
                    ContentTable.Columns.Add(new TableColumn() { Width = new GridLength(tableColumn.Width) });
                }
            }
        }

        /// <summary>
        /// 添加首列
        /// </summary>
        protected virtual void AddColumnHeader()
        {
            var columns = GetColumns();
            if (columns != null && columns.Count != 0)
            {
                //新增header
                TableRow headerRow = new TableRow();
                foreach (var tableColumn in columns)
                {
                    headerRow.Cells.Add(this.GetStyleHeader(tableColumn.Text));
                }
                ContentTableGroup.Rows.Add(headerRow);
            }
        }
        /// <summary>
        /// 添加自定义的Title
        /// </summary>
        public virtual void AddCustemTitle(TableRowGroup titleRowGroup)
        {

        }

        /// <summary>
        /// 初始化 数据表 的列数量和列宽
        /// 
        ///    //以下是一个 Demo
        //ContentTable.Columns.Add(new TableColumn() { Width = new GridLength(100) });
        //ContentTable.Columns.Add(new TableColumn() { Width = new GridLength(200) });
        //ContentTable.Columns.Add(new TableColumn() { Width = new GridLength(200) });
        /// </summary>
        public abstract List<TableColumnHeader> GetColumns();


        /// <summary>
        /// 添加每列数据 的 Cell
        /// 
        //      以下是一个 Demo
        //      var list = (List<Worker>)Data.ContentList;
        //      if (list == null)
        //      {
        //          return;
        //      }
        //      int index = 1;
        //      foreach (var worker in list)
        //      {
        //          TableRow row = new TableRow();
        //          row.Cells.Add(this.GetDefaultCell(index.ToString()));
        //          row.Cells.Add(this.GetDefaultCell(worker.Name));
        //          row.Cells.Add(this.GetDefaultCell(worker.Card));

        //          ContentTableGroup.Rows.Add(row);
        //          index++;
        //      }
        /// </summary>
        public abstract void AddContent();


        /// <summary>
        /// 得到具有 默认 Style的HeaderCell
        /// </summary>
        /// <returns></returns>
        public TableCell GetStyleHeader(string text = null)
        {
            TableCell cell = text == null ? new TableCell() : new TableCell(new Paragraph(new Run(text)));
            if (this.HeaderStyle != null)
            {
                cell.Style = HeaderStyle;
            }
            return cell;
        }
        /// <summary>
        /// 得到具有 默认 Style的cell
        /// </summary>
        /// <returns></returns>
        public TableCell GetStyleCell(string text = null)
        {
            var cell = this.GetDefaultCell(text);
            if (this.CellStyle != null)
            {
                cell.Style = CellStyle;
            }
            return cell;
        }

        public TableCell GetDefaultCell(string text = null)
        {
            TableCell cell = text == null ? new TableCell() : new TableCell(new Paragraph(new Run(text)));
            return cell;
        }
    }
}
