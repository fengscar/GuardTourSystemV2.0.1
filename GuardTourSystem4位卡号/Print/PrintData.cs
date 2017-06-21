using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Print
{
    public class PrintData : NotificationObject
    {
        public string Title { get; set; }//打印内容
        //打印时间
        private DateTime printTime;
        public DateTime PrintTime
        {
            get { return DateTime.Now; }
            set
            {
                printTime = value;
                RaisePropertyChanged("PrintTime");
                //SetProperty(ref this.printTime, value);
            }
        }

        //打印的数据 条数
        private int dataCount;
        public int DataCount
        {
            get { return dataCount; }
            set
            {
                dataCount = value;
                RaisePropertyChanged("DataCount");
                //SetProperty(ref this.dataCount, value);
            }
        }

        public DataTable DataTable { get; set; }

        /// <summary>
        /// 要打印的数据内容,一般是List
        /// </summary>
        public object ContentList { get; set; }
    }
}
