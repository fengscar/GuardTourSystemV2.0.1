using GuardTourSystem.Utils;
using GuardTourSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
       public static async void Print(this RadGridView grid, PrintSettings settings = null, PageOrientation orientation = PageOrientation.Portrait)
        {
            AppStatusViewModel.Instance.ShowProgress(true, "正在载入打印数据,请稍等...");
            var rtb = await CreateRadRichTextBox(grid, settings, orientation);
            var window = new RadWindow() { Height = 0, Width = 0, Opacity = 0, Content = rtb };
            rtb.PrintStarted += (s, e) =>
            {
                AppStatusViewModel.Instance.ShowProgress(true, "正在打印...");
            };
            rtb.PrintCompleted += (s, e) =>
            {
                AppStatusViewModel.Instance.ShowCompany();
            };

            window.Show();
            if (settings == null)
            {
                settings = new PrintSettings() { DocumentName = "打印", PrintMode = PrintMode.Native };
            }
            AppStatusViewModel.Instance.ShowCompany();
            rtb.Print(settings);

            //释放资源
            rtb = null;
            if (window != null)
            {
                window.Close();
            }
        }

        ///打印预览
        //public static void PrintPreview(this RadGridView grid, PrintSettings settings, PageOrientation orientation = PageOrientation.Portrait)
        //{
        //var rtb = CreateRadRichTextBox(grid, settings,orientation);
        //var window = CreatePreviewWindow(rtb);
        //window.ShowDialog();
        //}

        private static async Task<RadRichTextBox> CreateRadRichTextBox(RadGridView grid, PrintSettings settings, PageOrientation orientation)
        {
            DBug.w("正在创建富文本");
            var result = new RadRichTextBox()
            {
                IsReadOnly = true,
                LayoutMode = DocumentLayoutMode.Paged,
                IsSelectionEnabled = false,
                IsSpellCheckingEnabled = false,
                FontSize = 6,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center

            };
            //先设置方向,避免文档初始化完成后再去转换
            result.ChangeSectionPageOrientation(orientation);


            DBug.w("正在创建文档");
            var stream = new MemoryStream();
            grid.ExportAsync(stream, new GridViewExportOptions()
            {
                Format = Telerik.Windows.Controls.ExportFormat.Html,
                ShowColumnFooters = grid.ShowColumnFooters,
                ShowColumnHeaders = grid.ShowColumnHeaders,
                ShowGroupFooters = grid.ShowGroupFooters
            }, false);


            await Task.Factory.StartNew(() =>
            {
                long curSize = 0;
                while (stream.CanRead)
                {
                    Thread.Sleep(1000);
                    // 当stream大小不在改变时,将退出该循环.
                    if (stream.Length <= curSize)
                    {
                        stream.Position = 0;
                        break;
                    }
                    else
                    {
                        curSize = stream.Length;
                    }
                }
            });

            result.Document = new HtmlFormatProvider().Import(stream);
            result.Document.SectionDefaultPageOrientation = orientation;
            return result;
        }

    }
}
