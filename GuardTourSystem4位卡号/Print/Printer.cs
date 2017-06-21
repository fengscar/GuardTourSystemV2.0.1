using GuardTourSystem.View;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace GuardTourSystem.Print
{
    //PrintDialog选择页码打印时没有处理, 从网上复制了一个来
    class PrintDialogWithPageRange : PrintDialog
    {
        public new void PrintDocument(DocumentPaginator doc, string desp)
        {
            if (this.PageRangeSelection == PageRangeSelection.AllPages)
            {
                base.PrintDocument(doc, desp);
            }
            else
            {
                string timeStamp = DateTime.Now.DayOfYear.ToString() + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                string pack = "pack://temp" + timeStamp + ".xps";
                using (var ms = new MemoryStream())
                {
                    var package = Package.Open(ms, FileMode.Create, FileAccess.ReadWrite);
                    PackageStore.AddPackage(new Uri(pack), package);
                    using (var xpsDoc = new XpsDocument(package, CompressionOption.SuperFast, pack))
                    {
                        var xpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(xpsDoc);
                        xpsDocumentWriter.Write(doc);
                        var fdsCopy = xpsDoc.GetFixedDocumentSequence();

                        var xdw = System.Printing.PrintQueue.CreateXpsDocumentWriter(this.PrintQueue);
                        var vtxd = (VisualsToXpsDocument)xdw.CreateVisualsCollator();
                        for (int i = this.PageRange.PageFrom - 1; i < this.PageRange.PageTo; i++)
                        {
                            var v = fdsCopy.DocumentPaginator.GetPage(i).Visual;
                            var cv = new ContainerVisual();
                            cv.Children.Add(v);
                            vtxd.Write(cv, this.PrintTicket);
                            cv.Children.Remove(v);
                        }
                        vtxd.EndBatchWrite();
                        //xpsDoc.Close();
                        //ms.Close();
                        //ms.Dispose();
                    }
                }
            }
        }
    }
    class Printer
    {

        public AbstractDocument Document { get; set; }
        public PrintDialogWithPageRange PrintDialog { get; set; }

        public Printer(AbstractDocument doc)
        {
            Document = doc;

            PrintDialog = new PrintDialogWithPageRange();
            PrintDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;
            //PrintDialog.SelectedPagesEnabled = true;
            PrintDialog.UserPageRangeEnabled = true;
        }
        /// <summary>
        /// 显示打印预览窗口
        /// </summary>
        public void ShowPreviewWindow()
        {
            PrintPreviewWindow previewWnd = new PrintPreviewWindow(Document);
            previewWnd.Owner = MainWindow.Instance;
            previewWnd.ShowInTaskbar = false;
            previewWnd.ShowDialog();
        }

        /// <summary>
        /// 直接打印
        /// </summary>
        public void Print(string desc = null)
        {
            PrintDialog.PrintDocument(new HeaderFooterPaginator(Document), desc ?? "计数文件");
        }


        /// <summary>
        /// 显示打印设置对话框
        /// </summary>
        public void ShowSettingDialog(string desc = null)
        {
            if (PrintDialog.ShowDialog() == true)
            {
                PrintDialog.PrintDocument(new HeaderFooterPaginator(Document), desc ?? "计数文件");
            }
        }


        /// <summary>
        /// 打印控件
        /// </summary>
        /// <param name="element"></param>
        public static void PrintVisual(FrameworkElement element,
            PageOrientation ori = PageOrientation.Landscape,
            PageMediaSizeName pageSize = PageMediaSizeName.ISOA4)
        {
            var printDialog = new PrintDialog();
            printDialog.PrintTicket.PageOrientation = ori;
            printDialog.PrintTicket.PageMediaSize = new PageMediaSize(pageSize);
            printDialog.PrintTicket.PageScalingFactor = 90;
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(element, "");
            };
        }
    }
}
