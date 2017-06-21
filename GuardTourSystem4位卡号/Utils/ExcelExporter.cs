using GuardTourSystem.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Reflection;
using GuardTourSystem.ViewModel;
using Telerik.Windows.Controls;
using Microsoft.Win32;

namespace GuardTourSystem.Utils
{
    public class ExcelExporter
    {
        public async static void TelerikControlExport(Control control, string fileName)
        {
            if (control != null && (control is RadGridView || control is RadTreeListView))
            {
                string extension = "xls";

                SaveFileDialog dialog = new SaveFileDialog()
                {
                    DefaultExt = extension,
                    Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, "Excel"),
                    FilterIndex = 1,
                    FileName = fileName + DateTime.Now.ToString(" yyyy_MM_dd")
                };

                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        AppStatusViewModel.Instance.ShowProgress(true, "正在导出Excel...请稍等");

                        Stream stream = dialog.OpenFile();
                        if (control is RadGridView)
                        {
                            var exportControl = control as RadGridView;
                            exportControl.ExportAsync(stream, new GridViewExportOptions()
                            {
                                Format = ExportFormat.ExcelML,
                                ShowColumnFooters = true,
                                ShowColumnHeaders = true,
                                ShowGroupFooters = true
                            }, true);//最后一个ture参数表示导出成功后,将Dispose掉该Stream
                        }
                        else if (control is RadTreeListView)
                        {
                            var exportControl = control as RadTreeListView;
                            exportControl.ExportAsync(stream, new GridViewExportOptions()
                            {
                                Format = ExportFormat.ExcelML,
                                ShowColumnFooters = true,
                                ShowColumnHeaders = true,
                                ShowGroupFooters = true
                            }, true);
                        }
                        await Task.Factory.StartNew(() =>
                           {
                               while (stream.CanRead) //当stream被dispose时,将退出该循环.显示导出成功
                               {

                               }
                           });
                        //});
                        AppStatusViewModel.Instance.ShowInfo("Excel导出成功!");
                    }
                    catch (Exception)
                    {
                        AppStatusViewModel.Instance.ShowError("Excel导出失败!请先关闭 " + dialog.FileName + " ,再尝试");
                    }
                }
            }
            else
            {
                throw new Exception("不是Telerik控件,不支持导出");
            }
        }

        /// <summary>
        /// CSV格式化
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>格式化数据</returns>
        private static string FormatCsvField(string data)
        {
            return String.Format("\"{0}\"", data.Replace("\"", "\"\"\"").Replace("\n", "").Replace("\r", ""));
        }

        /// <summary>
        /// 导出DataGrid数据到Excel
        /// </summary>
        /// <param name="withHeaders">是否需要表头</param>
        /// <param name="grid">DataGrid</param>
        /// <param name="dataBind"></param>
        /// <returns>Excel内容字符串</returns>
        public static string ExportDataGrid(DataGrid dataGrid, bool showHeader = true, string title = null)
        {
            var strBuilder = new StringBuilder();
            //添加title
            if (title != null)
            {
                strBuilder.Append(title).Append("\r\n");
            }

            //获取Headers
            var headers = new List<string>();
            foreach (var column in dataGrid.Columns)
            {
                headers.Add(FormatCsvField(column.Header.ToString()));
            }
            strBuilder.Append(String.Join(",", headers.ToArray())).Append("\r\n");

            //获取Cell values
            var columnCount = dataGrid.Columns.Count;
            var rowCount = dataGrid.Items.Count;
            for (int row = 0; row < rowCount; row++)
            {
                var csvRow = new List<string>();
                for (int col = 0; col < columnCount; col++)
                {
                    var cellValue = dataGrid.GetCellValue(row, col);
                    csvRow.Add(cellValue);//如果为空,添加一个制表符
                }
                strBuilder.Append(String.Join(",", csvRow.ToArray())).Append("\r\n");//换行
            }
            return strBuilder.ToString();
        }


        public static string ExportRadTreeListView(RadTreeListView treeListView, bool showHeader = true, string title = null)
        {
            var strBuilder = new StringBuilder();
            //添加title
            if (title != null)
            {
                strBuilder.Append(title).Append("\r\n");
            }

            //获取Headers
            var headers = new List<string>();
            foreach (var column in treeListView.Columns)
            {
                headers.Add(FormatCsvField(column.Header.ToString()));
            }
            strBuilder.Append(String.Join(",", headers.ToArray())).Append("\r\n");

            //获取Cell values
            var columnCount = treeListView.Columns.Count;
            var rowCount = treeListView.Items.Count;
            for (int row = 0; row < rowCount; row++)
            {
                var csvRow = new List<string>();
                for (int col = 0; col < columnCount; col++)
                {
                    var cellValue = treeListView.GetCellValue(row, col);
                    csvRow.Add(cellValue);//如果为空,添加一个制表符
                }
                strBuilder.Append(String.Join(",", csvRow.ToArray())).Append("\r\n");//换行
            }

            return strBuilder.ToString();
        }

        /// <summary>
        /// 导出DataGrid数据到Excel为CVS文件
        /// 使用utf8编码 中文是乱码 改用Unicode编码
        /// 
        /// </summary>
        /// <param name="control">要打印的控件</param>
        /// <param name="withHeaders">是否带列头</param>
        /// <param name="title">要在文件中显示的第一行文本</param>
        public static void ExportData(Control control, bool withHeaders = true, string title = null)
        {
            try
            {
                if (control == null)
                {
                    DBug.w("导出失败:未找到Name为ExportGrid的控件");
                    return;
                }
                ///添加第一行文本 : 类似 员工信息 导出时间:2017年3月23日
                string result = title == null ? null : title + @"\r\n";
                if (control is DataGrid)
                {
                    var dataGrid = control as DataGrid;
                    result = ExcelExporter.ExportDataGrid(dataGrid, withHeaders, title);
                }
                else if (control is RadTreeListView)
                {
                    var treeListView = control as RadTreeListView;
                    result = ExcelExporter.ExportRadTreeListView(treeListView, withHeaders, title);
                }
                else
                {
                    DBug.w("无法导出的控件类型:" + control.GetType());
                    return;
                }
                var sfd = new Microsoft.Win32.SaveFileDialog
                {
                    DefaultExt = "csv",
                    Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*",
                    FilterIndex = 1
                };
                if (sfd.ShowDialog() == true)
                {
                    using (Stream stream = sfd.OpenFile())
                    {
                        using (var writer = new StreamWriter(stream, System.Text.Encoding.Unicode))
                        {
                            result = result.Replace(",", "\t");
                            writer.Write(result);
                        }
                    }
                }
                AppStatusViewModel.Instance.ShowInfo("Excel导出成功");
            }
            catch (Exception ex)
            {
                DBug.w(ex);
            }
        }
    }

    public class ReflectionUtil
    {
        /// <summary>
        /// 根据属性名,获取属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static object GetProperty(object obj, string propName)
        {
            if (obj == null)
            {
                return null;
            }
            //如果属性名 是类似 xxx.xxx的,要递归处理
            var propArray = propName.Split('.');
            if (propArray.Length == 1)
            {
                Type type = obj.GetType();
                System.Reflection.PropertyInfo propertyInfo = type.GetProperty(propName);
                return propertyInfo.GetValue(obj, null);
            }
            else
            {
                //如果属性名 是类似 xxx.xxx的,要递归处理
                return GetProperty(GetProperty(obj, propArray[0]), propArray[1]);
            }
        }
    }
}
