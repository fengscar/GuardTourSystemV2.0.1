using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace GuardTourSystem.Print
{
    class HeaderFooterPaginator : DocumentPaginator
    {
        readonly DocumentPaginator m_paginator;
        readonly List<TableColumnHeader> Headers;

        public HeaderFooterPaginator(AbstractDocument document)
        {
            m_paginator = ((IDocumentPaginatorSource)document.FlowDocument).DocumentPaginator;
            Headers = document.GetColumns();
        }
        /// <summary>
        /// 在指定位置 画一个 类似这样的 Header
        ///         _____________
        ///         |   Text    |     框的高度为18 , 字体为16, 字的位置为 Top+2
        ///         -------------
        /// </summary>
        /// <param name="text"></param>
        /// <param name="columnLeftPoint"></param>
        /// <param name="columnRightPoint"></param>
        private void DrawHeader(DrawingContext ctx, DocumentPage page)
        {
            var left = page.ContentBox.Left;
            var top = page.ContentBox.Top;
            foreach (TableColumnHeader header in Headers)
            {
                //画框
                ctx.DrawRectangle(Brushes.LightGray, new Pen(Brushes.Black, 0.75), new Rect(new Point(left, top), new Size(header.Width, 18)));
                //写字
                FormattedText text = new FormattedText(header.Text,
                    System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface("Courier New"), 16, Brushes.Black);
                ctx.DrawText(text, new Point(left + 2, top + 2));

                left += header.Width;
            }
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            DocumentPage page = m_paginator.GetPage(pageNumber);
            ContainerVisual newpage = new ContainerVisual();


            //页眉:公司名称
            //DrawingVisual header = new DrawingVisual();
            //using (DrawingContext ctx = header.RenderOpen())
            //{
            //    FormattedText text = new FormattedText("上海ABCD信息技术有限公司",
            //        System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
            //        new Typeface("Courier New"), 14, Brushes.Black);
            //    ctx.DrawText(text, new Point(page.ContentBox.Left, page.ContentBox.Top));
            //    ctx.DrawLine(new Pen(Brushes.Black, 0.5), new Point(page.ContentBox.Left, page.ContentBox.Top + 16), new Point(page.ContentBox.Right, page.ContentBox.Top + 16));
            //}

            //draw headers
            DrawingVisual header = new DrawingVisual();
            using (DrawingContext ctx = header.RenderOpen())
            {
                if (pageNumber != 0)
                {
                    this.DrawHeader(ctx, page);
                }
            }

            //页脚:第几页
            DrawingVisual footer = new DrawingVisual();
            using (DrawingContext ctx = footer.RenderOpen())
            {
                FormattedText text = new FormattedText("第 " + (pageNumber + 1) + " 页 / 共 " + this.PageCount + " 页",
                    System.Globalization.CultureInfo.CurrentCulture, FlowDirection.RightToLeft,
                    new Typeface("Courier New"), 14, Brushes.Black);
                ctx.DrawText(text, new Point(page.ContentBox.Right, page.ContentBox.Bottom - 20));
            }


            //将原页面微略压缩(使用矩阵变换)
            ContainerVisual mainPage = new ContainerVisual();
            mainPage.Children.Add(page.Visual);
            mainPage.Transform = new MatrixTransform(1, 0, 0, 0.95, 0, 0.025 * page.ContentBox.Height);

            //在现页面中加入原页面，页眉和页脚
            newpage.Children.Add(mainPage);
            newpage.Children.Add(header);
            newpage.Children.Add(footer);

            return new DocumentPage(newpage, page.Size, page.BleedBox, page.ContentBox);
        }

        public override bool IsPageCountValid
        {
            get
            {
                return m_paginator.IsPageCountValid;
            }
        }

        public override int PageCount
        {
            get
            {
                //在获取总页数前,先强制计算总页数
                m_paginator.ComputePageCount();
                return m_paginator.PageCount;
            }
        }

        public override Size PageSize
        {
            get
            {
                return m_paginator.PageSize;
            }

            set
            {
                m_paginator.PageSize = value;
            }
        }

        public override IDocumentPaginatorSource Source
        {
            get
            {
                return m_paginator.Source;
            }
        }
    }
}
