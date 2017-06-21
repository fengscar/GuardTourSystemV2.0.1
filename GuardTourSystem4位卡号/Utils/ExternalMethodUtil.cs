using GuardTourSystem.Model;
using GuardTourSystem.ViewModel;
using KaiheSerialPortLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace GuardTourSystem.Utils
{
    /// <summary>
    /// 使用时 记得手动引入 命名空间 ,编译器并不会自动进行提示
    /// </summary>
    public static class ExternalMethodUtil
    {
        //由于BindableBase不能序列化,所以使用自定义的Copy
        //public static RegularItemViewModel DeepCopy(this RegularItemViewModel obj)
        //{
        //    var viewmodel = obj as RegularItemViewModel;
        //    if (viewmodel == null)
        //    {
        //        return null;
        //    }
        //    var copy = new RegularItemViewModel();
        //    var freq = new Frequence() { Worker = viewmodel.Frequence.Worker };
        //    var weekSelect = new WeekSelectViewModel(viewmodel.Frequence.Regular, null);

        //    copy.Frequence = freq;
        //    copy.WeekSelectViewModel = weekSelect;

        //    return copy;
        //}

        //public static IrregularItemViewModel DeepCopy(this IrregularItemViewModel obj)
        //{
        //    var viewmodel = obj as IrregularItemViewModel;
        //    if (viewmodel == null)
        //    {
        //        return null;
        //    }
        //    var copy = new IrregularItemViewModel();
        //    var freq = new Frequence() { Worker = viewmodel.Frequence.Worker };
        //    var monthSelect = new MonthSelectViewModel(viewmodel.Frequence.Irregular.GetMonthPlan(DateTime.Now), null);

        //    copy.Frequence = freq;
        //    copy.MonthSelectViewModel = monthSelect;

        //    return copy;
        //}

        #region DateTime的扩展方法
        /// <summary>
        /// 设置为当天的 00:00
        /// </summary>
        public static DateTime SetBeginOfDay(this DateTime time)
        {
            time = time.Subtract(time.TimeOfDay);
            return time;
        }

        /// <summary>
        /// 设置为当天的 23:59
        /// </summary>
        public static DateTime SetEndOfDay(this DateTime time)
        {
            time = time.Subtract(time.TimeOfDay).AddDays(1).AddMilliseconds(-1);
            return time;
        }

        /// <summary>
        /// 设置该月的 1号
        /// </summary>
        public static DateTime SetBeginOfMonth(this DateTime month)
        {
            month = month.AddDays(-month.Day + 1); //设置为该月第一天
            return month;
        }

        /// <summary>
        /// 设置该月的 最后一天
        /// </summary>
        public static DateTime SetEndOfMonth(this DateTime month)
        {
            var nextMonth = month.AddMonths(1);
            month = nextMonth.AddDays(nextMonth.Day);
            return month;
        }
        #endregion

        /// <summary>
        /// http://techiethings.blogspot.jp/2010/05/get-wpf-datagrid-row-and-cell.html
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
        #region 获取DataGrid显示的数据
        public static DataGridRow GetRow(this DataGrid grid, int index)
        {
            DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // May be virtualized, bring into view and try again.
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.Items[index]);
                row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        public static DataGridCell GetCell(this DataGrid grid, DataGridRow row, int column)
        {
            if (row != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

                if (presenter == null)
                {
                    grid.ScrollIntoView(row, grid.Columns[column]);
                    presenter = GetVisualChild<DataGridCellsPresenter>(row);
                }

                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }
        public static DataGridCell GetCell(this DataGrid grid, int row, int column)
        {
            DataGridRow rowContainer = grid.GetRow(row);
            return grid.GetCell(rowContainer, column);
        }
        //只有这个是自己写的
        public static string GetCellValue(this DataGrid grid, int row, int column)
        {
            var cell = grid.GetCell(row, column);
            if (cell == null)
            {
                return null;
            }
            var cellContent = cell.Content;
            if (cellContent is TextBlock)
            {
                return (cellContent as TextBlock).Text;
            }
            if (cellContent is System.Windows.Controls.Label)
            {
                return (cellContent as System.Windows.Controls.Label).Content.ToString();
            }
            return null;
        }
        public static DataTable GetDataTable(this DataGrid dg)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < dg.Columns.Count; i++)
            {
                dt.Columns.Add(new DataColumn());
            }
            for (int i = 0; i < dg.Items.Count; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < dg.Columns.Count; j++)
                {
                    dr[j] = dg.GetCellValue(i, j);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion


        #region 获取TreeListView的Cell值
        public static GridViewRow GetRow(this RadTreeListView treeList, int index)
        {
            var row = (GridViewRow)treeList.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // May be virtualized, bring into view and try again.
                treeList.UpdateLayout();
                treeList.ScrollIntoView(treeList.Items[index]);
                row = (GridViewRow)treeList.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }
        public static GridViewCell GetCell(this RadTreeListView treeList, GridViewRow row, int column)
        {
            if (row != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

                if (presenter == null)
                {
                    treeList.ScrollIntoView(row, treeList.Columns[column]);
                    presenter = GetVisualChild<DataGridCellsPresenter>(row);
                }
                GridViewCell cell = (GridViewCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }
        public static GridViewCell GetCell(this RadTreeListView grid, int row, int column)
        {
            var rowContainer = grid.GetRow(row);
            return grid.GetCell(rowContainer, column);
        }

        public static string GetCellValue(this RadTreeListView treeListView, int row, int col)
        {
            var value = treeListView.GetCell(row, col);
            return value.ToString();
        }
        #endregion


        public static string GetContentName(this ViewEnum viewEnum)
        {
            switch (viewEnum)
            {
                case ViewEnum.ReadPatrol:
                    return LanLoader.Load(LanKey.MenuQueryReadPatrol);
                case ViewEnum.QueryRawData:
                    return LanLoader.Load(LanKey.MenuQueryRawData);
                case ViewEnum.QueryRawCount:
                    return LanLoader.Load(LanKey.MenuQueryRawCount);
                case ViewEnum.ReadHit:
                    return LanLoader.Load(LanKey.MenuQueryReadHit);
                case ViewEnum.QueryResult:
                    return LanLoader.Load(LanKey.MenuQueryResult);
                case ViewEnum.QueryChart:
                    return LanLoader.Load(LanKey.MenuQueryChart);
                case ViewEnum.SetRoute:
                    return LanLoader.Load(LanKey.MenuPatrolSettingRoute);
                case ViewEnum.SetWorker:
                    return LanLoader.Load(LanKey.MenuPatrolSettingWorker);
                case ViewEnum.SetEvent:
                    return LanLoader.Load(LanKey.MenuPatrolSettingEvent);
                case ViewEnum.SetFrequence:
                    return LanLoader.Load(LanKey.MenuPatrolSettingFrequence);
                case ViewEnum.SetRegular:
                    return LanLoader.Load(LanKey.MenuPatrolSettingRegular);
                case ViewEnum.SetIrregular:
                    return LanLoader.Load(LanKey.MenuPatrolSettingIrregular);
                case ViewEnum.DataManage:
                    return LanLoader.Load(LanKey.MenuDataManageBackupAndRecovery);
                case ViewEnum.HowToStart:
                    return LanLoader.Load(LanKey.MenuHelpHowToStart);
                default:
                    return "";
            }
        }

        public static string GetContentName(this PopupEnum popupEnum)
        {
            switch (popupEnum)
            {
                case PopupEnum.Reanalysis:
                    return LanLoader.Load(LanKey.MenuQueryReanalysis);
                case PopupEnum.ClearPatrolData:
                    return LanLoader.Load(LanKey.MenuDataManageClearPatrolData);
                case PopupEnum.ImportPatrolData:
                    return LanLoader.Load(LanKey.MenuDataManageImportPatrolData);
                case PopupEnum.ExportPatrolData:
                    return LanLoader.Load(LanKey.MenuDataManageExportPatrolData);
                case PopupEnum.SystemInit:
                    return LanLoader.Load(LanKey.MenuSystemInit);
                case PopupEnum.ManageUser:
                    return LanLoader.Load(LanKey.MenuSystemUserManage);
                case PopupEnum.ChangePassword:
                    return LanLoader.Load(LanKey.MenuSystemModifyPassword);
                case PopupEnum.Language:
                    return LanLoader.Load(LanKey.MenuSystemLanguage);
                case PopupEnum.DeviceTest:
                    return LanLoader.Load(LanKey.MenuSystemDeviceTest);
                case PopupEnum.Help:
                    return LanLoader.Load(LanKey.MenuHelpHowToUse);
                case PopupEnum.HowToStart:
                    return LanLoader.Load(LanKey.MenuHelpHowToStart);
                case PopupEnum.AboutUs:
                    return LanLoader.Load(LanKey.MenuHelpAboutUs);
                case PopupEnum.Error:
                    return LanLoader.Load(LanKey.SYSTEM_ERROR);
                default:
                    return "";
            }
        }


        public static void SetPressed(this Button button, bool isPressed)
        {
            typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(button, new object[] { isPressed });
        }


        public static DateTime ToNotNullable(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                throw new Exception("DateTime is null,can't convert!");
            }
            return (DateTime)dateTime;
        }

        //扩展 串口库 ,使其支持英文
        public static string ToLanString(this TResult Result)
        {
            switch (Result)
            {
                case TResult.WAKE_UP_FAILED:
                    return LanLoader.Load(LanKey.PortErrorWakeUp);
                case TResult.PORT_INIT_ERROR:
                    return LanLoader.Load(LanKey.PortErrorInit);
                case TResult.NO_DEVICE:
                    return LanLoader.Load(LanKey.PortErrorNoDevice);
                case TResult.NO_DEVICE_OR_OCCUPY:
                    return LanLoader.Load(LanKey.PortErrorNoDeviceOrOccupy);
                case TResult.MULTI_DEVICE:
                    return LanLoader.Load(LanKey.PortErrorMultiDevice);
                case TResult.WRONG:
                default:
                    return LanLoader.Load(LanKey.PortErrorWrongData);
                case TResult.TIMEOUT:
                    return LanLoader.Load(LanKey.PortErrorTimeOut);
                case TResult.SUCCESS:
                    return LanLoader.Load(LanKey.PortSuccess);
            }
        }
    }
}
