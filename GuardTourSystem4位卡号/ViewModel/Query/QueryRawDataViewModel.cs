using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GuardTourSystem.Utils;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Data;
using System.Windows.Controls;
using System.Printing;
using System.Windows.Documents;
using GuardTourSystem.Print;

namespace GuardTourSystem.ViewModel
{
    /// <summary>
    /// 查询原始数据 ( 数据查询 => 计数记录)
    /// </summary>
    public class QueryRawDataViewModel : ShowContentViewModel
    {
        private IRawDataService DataService;
        //当前显示的原始数据
        private ObservableCollection<RawData> rawdatas;
        public ObservableCollection<RawData> RawDatas
        {
            get { return rawdatas; }
            set
            {
                rawdatas = value;
                RaisePropertyChanged("RawDatas");
                //SetProperty(ref this.rawdatas, value);
            }
        }
        private string queryText;
        public string QueryText
        {
            get { return queryText; }
            set
            {
                queryText = value;
                RaisePropertyChanged("QueryText");
                //SetProperty(ref this.queryText, value);
            }
        }


        public DateQueryInfo DateQueryInfo { get; set; }

        /// <summary>
        /// 查询
        /// </summary>
        public DelegateCommand CQuery { get; set; }
        public DelegateCommand CReset { get; set; }
        public DelegateCommand CPrint { get; set; }



        public QueryRawDataViewModel()
        {
            DataService = new RawDataBLL();
            this.CQuery = new DelegateCommand(this.Query, this.CanQuery);
            this.CPrint = new DelegateCommand(this.Print);

            this.CReset = new DelegateCommand(this.Reset);

            var now = DateTime.Now;
            DateQueryInfo = new DateQueryInfo(now.SetBeginOfMonth().SetBeginOfDay(), now.SetEndOfDay(), () => { this.CQuery.RaiseCanExecuteChanged(); });

            Query();
        }

        private void Print()
        {
            //var printData = new PrintData() { ContentList = RawDatas.ToList(), Title = LanLoader.Load(LanKey.RawData), DataCount = RawDatas.Count };
            //var queryInfo = LanLoader.Load(LanKey.RawData) + " : " + DateQueryInfo.GetQueryTime();
            //var printer = new Printer(new RawDataDocument(printData, queryInfo));
            //printer.ShowPreviewWindow();
        }

        private void Query()
        {
            var result = DataService.GetAllRawData(this.DateQueryInfo.Begin, this.DateQueryInfo.End);
            if (!string.IsNullOrWhiteSpace(QueryText))
            {
                result = result.Where(rawData => { return rawData.Place.Card.Contains(QueryText) || rawData.Place.EmployeeNumber.Contains(QueryText) || rawData.Place.Name.Contains(QueryText); }).ToList();
            }
            this.RawDatas = new ObservableCollection<RawData>(result);
            AppStatusViewModel.Instance.ShowInfo(LanLoader.Load(LanKey.QueryComplete), 2);
        }


        private void Reset()
        {
            AppContentViewModel.Instance.CloseView();
            AppContentViewModel.Instance.ShowView(ViewEnum.QueryRawData);
        }
        /// <summary>
        /// 判断能否查询.
        /// </summary>
        /// <returns></returns>
        private bool CanQuery()
        {
            string error = null;
            if (DateQueryInfo.CanQuery(out error))
            {
                this.Error = null;
                return true;
            }
            else
            {
                this.Error = error;
                return false;
            }
        }
    }
}
