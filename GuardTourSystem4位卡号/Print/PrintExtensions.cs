using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.FormatProviders.Html;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.UI;

namespace GuardTourSystem.Print
{
    public static class PrintExtensions
    {
        /// <summary>
        /// 直接打印
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="settings"></param>
        public static void Print(this RadGridView grid, PrintSettings settings = null, PageOrientation orientation = PageOrientation.Portrait)
        {
            var rtb = CreateRadRichTextBox(grid, settings, orientation);
            var window = new RadWindow() { Height = 0, Width = 0, Opacity = 0, Content = rtb };
            rtb.PrintCompleted += (s, e) => { window.Close(); };
            window.Show();
            if (settings == null)
            {
                settings = new PrintSettings() { DocumentName = "打印" };
            }
            rtb.Print(settings);
        }

        ///打印预览
        //public static void PrintPreview(this RadGridView grid, PrintSettings settings, PageOrientation orientation = PageOrientation.Portrait)
        //{
        //var rtb = CreateRadRichTextBox(grid, settings,orientation);
        //var window = CreatePreviewWindow(rtb);
        //window.ShowDialog();
        //}

        private static RadRichTextBox CreateRadRichTextBox(RadGridView grid, PrintSettings settings, PageOrientation orientation)
        {
            var result = new RadRichTextBox()
            {
                IsReadOnly = true,
                LayoutMode = DocumentLayoutMode.Paged,
                IsSelectionEnabled = false,
                IsSpellCheckingEnabled = false,
                Document = CreateDocument(grid)
            };
            result.ChangeSectionPageOrientation(orientation);
            return result;
        }

        private static RadDocument CreateDocument(RadGridView grid)
        {
            RadDocument document = null;
            using (var stream = new MemoryStream())
            {
                grid.Export(stream, new GridViewExportOptions()
                {
                    Format = Telerik.Windows.Controls.ExportFormat.Html,
                    ShowColumnFooters = grid.ShowColumnFooters,
                    ShowColumnHeaders = grid.ShowColumnHeaders,
                    ShowGroupFooters = grid.ShowGroupFooters
                });

                stream.Position = 0;

                document = new HtmlFormatProvider().Import(stream);
            }
            return document;
        }
    }
}
