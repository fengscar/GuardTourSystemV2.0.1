using GuardTourSystem.ViewModel;
using KaiheSerialPortLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem
{
    //当获取巡检数据和敲击记录时,将显示预设的进度条
    public class AppSerialPortUtil : KaiheSerialPortLibrary.SerialPortUtil
    {
        //#region 单例模式
        //private static readonly object locker = new object();
        //protected AppSerialPortUtil() { }
        //private static AppSerialPortUtil instance = null;
        //public new static AppSerialPortUtil Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            lock (locker)
        //            {
        //                if (instance == null)
        //                {
        //                    instance = new AppSerialPortUtil();
        //                }
        //            }
        //        }
        //        return instance;
        //    }
        //}
        //#endregion

        private static int GetCount = 0;


        private static void ShowGetPatrolCount(int count)
        {
            GetCount = count;
            if (GetCount != 0)
            {
                AppStatusViewModel.Instance.ShowProgress(false, "读取巡检记录中...总计" + count + "条");
            }
        }
        private static void UpdateGetPatrolProgress(int index)
        {
            if (index >= GetCount)
            {
                AppStatusViewModel.Instance.ShowInfo("获取巡检记录成功");
                GetCount = 0;
            }
            else
            {
                AppStatusViewModel.Instance.UpdateProgress("正在获取第" + index + "条记录", index, GetCount);
            }
        }
        private static void ShowError(string str)
        {
            AppStatusViewModel.Instance.ShowError(str, 5);
        }
        public new static async Task<BundleFlow> GetAllPatrolRecord(Action<int> GetCountAction = null, Action<int> ProgressAction = null, Action<string> onError = null)
        {
            if (GetCountAction == null)
            {
                GetCountAction = ShowGetPatrolCount;
            }
            if (ProgressAction == null)
            {
                ProgressAction = UpdateGetPatrolProgress;
            }
            if (onError == null)
            {
                onError = ShowError;
            }
            return await SerialPortUtil.GetAllPatrolRecord(GetCountAction, ProgressAction, onError);
        }


        private static void ShowGetHitCount(int count)
        {
            GetCount = count;
            if (GetCount != 0)
            {
                AppStatusViewModel.Instance.ShowProgress(false, "读取敲击记录中...总计" + count + "条");
            }
        }
        private static void UpdateGetHitProgress(int index)
        {
            if (index >= GetCount)
            {
                AppStatusViewModel.Instance.ShowInfo("获取敲击记录成功");
                GetCount = 0;
            }
            else
            {
                AppStatusViewModel.Instance.UpdateProgress("正在获取第" + index + "条记录", index, GetCount);
            }
        }
        public new static async Task<BundleFlow> GetAllHitRecords(Action<int> GetCountAction = null, Action<int> ProgressAction = null, Action<string> onError = null)
        {
            if (GetCountAction == null)
            {
                GetCountAction = ShowGetHitCount;
            }
            if (ProgressAction == null)
            {
                ProgressAction = UpdateGetHitProgress;
            }
            if (onError == null)
            {
                onError = ShowError;
            }
            return await SerialPortUtil.GetAllHitRecords(GetCountAction, ProgressAction, onError);
        }
    }
}
