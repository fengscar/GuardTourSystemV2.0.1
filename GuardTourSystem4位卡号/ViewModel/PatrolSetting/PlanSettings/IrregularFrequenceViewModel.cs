using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Utils;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.ViewModel
{
    // 无规律排班 界面的 ViewMODel
    public class IrregularFrequenceViewModel : BaseFrequenceViewModel
    {
        #region 当前选中 月份 的控制
        //对 选中月份 进行中文格式化
        public CultureInfo YearMonthFormat { get; set; }
        // 用户所选的日期
        //    不放在 IrregularItemViewModel 中, 因为用户选择的日期改变, 所有对应的Item都要改变
        private DateTime selectedDate;
        public DateTime SelectDate
        {
            get { return selectedDate; }
            set
            {
                SetProperty(ref this.selectedDate, value);
                //重新设置 DaysHeader
                InitDayHeader(value);
                // 将每条记录的 月份切换到该月
                foreach (var item in IrregularItems)
                {
                    item.SwitchMonth(SelectDate);
                }
            }
        }
        public DelegateCommand CLastMonth { get; set; } //选中上一个月
        public DelegateCommand CNextMonth { get; set; } //选中下一个月
        private void NextMonth()
        {
            SelectDate = SelectDate.AddMonths(1);
        }
        private void LastMonth()
        {
            SelectDate = SelectDate.AddMonths(-1);
        }

        /// <summary>
        /// 当前月份的每一天的Header ( 1, 2, 3, 4 ..... 31) 
        /// </summary>
        private ObservableCollection<string> daysHeader;
        public ObservableCollection<string> DaysHeader
        {
            get { return daysHeader; }
            set
            {
                SetProperty(ref this.daysHeader, value);
            }
        }
        private void InitDayHeader(DateTime date)
        {
            if (DaysHeader == null)
            {
                DaysHeader = new ObservableCollection<string>();
            }
            DaysHeader.Clear();
            var daycount = DateTime.DaysInMonth(date.Year, date.Month);
            for (int i = 1; i <= daycount; i++)
            {
                DaysHeader.Add(Convert.ToString(i));
            }
        }
        #endregion



        /// <summary>
        /// 无规律排班数据中每一列的ViewModel
        /// </summary>
        private ObservableCollection<IrregularItemViewModel> irregularItems;
        public ObservableCollection<IrregularItemViewModel> IrregularItems
        {
            get { return irregularItems; }
            set
            {
                SetProperty(ref this.irregularItems, value);
            }
        }



        public DelegateCommand CSave { get; set; }  //保存 数据到数据库
        public DelegateCommand CCancel { get; set; } //撤销, 重新载入数据
        //public DelegateCommand DataChanged { get; set; } //当数据(CommoBox的Worker,RegularSelectControl的安排) 被修改后 所绑定的Command

        public IrregularFrequenceViewModel()
        {
            //初始化显示格式
            YearMonthFormat = new System.Globalization.CultureInfo("ZH-cn");
            YearMonthFormat.DateTimeFormat.ShortDatePattern = "yyyy - MM ";

            this.CSave = new DelegateCommand(this.Save, this.CanSave);
            this.CCancel = new DelegateCommand(this.Cancel, this.CanSave);//能保存,就能撤销

            this.CLastMonth = new DelegateCommand(this.LastMonth);
            this.CNextMonth = new DelegateCommand(this.NextMonth);

            InitData();
        }

        private void InitData()
        {
            this.IrregularItems = new ObservableCollection<IrregularItemViewModel>();
            var irregularFreqs = GetFrequenceData().FindAll(f => { return f.IsRegular == false; });

            var selectionWorkers = GetWorkerData();

            this.SelectDate = DateTime.Now;//选中当前月份
            foreach (var freq in irregularFreqs)
            {
                var itemVM = new IrregularItemViewModel(freq, selectionWorkers, () =>
                {
                    this.CSave.RaiseCanExecuteChanged();
                    this.CCancel.RaiseCanExecuteChanged();
                });

                this.IrregularItems.Add(itemVM);
            }
        }

        /// <summary>
        /// 保存有改动的排班信息,并让用户选择是否重新生成当天的值班表
        /// 1. 保存排班信息
        /// 2. 如果保存成功,重新拷贝并初始化按键状态(显示为不可用状态)
        /// 3. 如果用户要重新生成,那就重新生成之前 !有改变! 的排班当天的值班表
        /// </summary>
        public async void Save()
        {
            string error = "";
            foreach (var item in IrregularItems)
            {
                //保存 班次巡检员
                FrequenceService.UpdateFrequenceWorker(item.Frequence);
                //保存 有规律排班表
                item.UpdateFrequence(); //将界面数据 更新到 item.Frequence
                var result = FrequenceService.UpdateFrequenceIrregular(item.Frequence, out error);
            }

            var changedItems = IrregularItems.ToList().FindAll(item => item.IsChanged);
            //重新深拷贝,来初始化IsChanged)
            changedItems.ForEach(item => item.SaveInitValue());
            this.CSave.RaiseCanExecuteChanged();
            this.CCancel.RaiseCanExecuteChanged();

            /// 如果用户要重新生成,那就重新生成之前 !有改变! 的排班当天的值班表
            var message = new StringBuilder();
            int i = 1;
            foreach (var item in changedItems)
            {
                message.Append(i + ".    " + item.Frequence.Name + "\n");
                i++;
            }
            var userClick = await this.ShowConfirmDialog("排班信息修改成功! 是否重新生成以下班次今天的值班表?", message.ToString());
            if (MessageDialogResult.Affirmative == userClick) //用户确认重新生成
            {
                //更新所有有改变的班次当天的值班表
                var bll = new DutyBLL();
                changedItems.ForEach(item =>
                {
                    bll.GenerateDuty(out error, item.Frequence, DateTime.Now); //更新值班表
                });
                AppStatusViewModel.Instance.ShowInfo("当天值班表已重新生成.");
            }
        }


        private bool CanSave()
        {
            if (IrregularItems == null || IrregularItems.Count == 0)
            {
                return false;
            }
            return IrregularItems.Any(item => item.IsChanged);
        }

        public void Cancel()
        {
            InitData();
        }


        public override void NotifyChange(ChangeEvent ce)
        {
            switch (ce)
            {
                case ChangeEvent.RoutesChange:
                case ChangeEvent.WorkersChange:
                case ChangeEvent.FrequenceChange:
                    InitData();
                    break;
                default:
                    break;
            }
        }
    }

    // 每条数据 的ViewModel
    public class IrregularItemViewModel : BindableBase
    {
        public IrregularItemViewModel() { }
        private Action ItemChangeAction;

        private Frequence frequence;
        public Frequence Frequence
        {
            get { return frequence; }
            set
            {
                SetProperty(ref this.frequence, value);
            }
        }

        //可供选择的巡检员
        public List<Worker> Workers { get; set; }

        private MonthSelectViewModel monthSelectViewModel;
        public MonthSelectViewModel MonthSelectViewModel
        {
            get { return monthSelectViewModel; }
            set
            {
                SetProperty(ref this.monthSelectViewModel, value);
            }
        }

        //存放初始的Item
        private IrregularItemViewModel OldItem;
        //判断该Item是否有改变
        public bool IsChanged
        {
            get
            {
                return !this.Equals(OldItem);
            }
        }
        //保存初始值
        public void SaveInitValue()
        {
            this.OldItem = this.DeepCopy();
        }

        public IrregularItemViewModel(Frequence freq, List<Worker> selectionWorkers, Action OnItemChanged)
        {
            //初始化数据改变时的Action
            this.ItemChangeAction = OnItemChanged;

            this.Frequence = freq;
            this.Frequence.SetWorkerChangeAction(OnItemChanged);

            //初始化 巡检员的可选项 和 选中项
            this.Workers = selectionWorkers;
            int workerID = Frequence.Worker == null ? -1 : Frequence.Worker.ID;
            var findedWorker = selectionWorkers.Find(w => { return w.ID == workerID; });
            Frequence.Worker = findedWorker;


            MonthSelectViewModel = new MonthSelectViewModel(Frequence.Irregular.GetMonthPlan(DateTime.Now), OnItemChanged);

            //保存初始值,用来判断是否有修改
            SaveInitValue();
        }


        // 切换所选的月份
        public void SwitchMonth(DateTime yearMonth)
        {
            UpdateFrequence(); //切换之前,保存数据到本地Irregular
            MonthSelectViewModel.ShowMonth(Frequence.Irregular.GetMonthPlan(yearMonth));
        }

        // 将 排班数据保存
        public void UpdateFrequence()
        {
            this.Frequence.Irregular.UpdateMonthPlan(MonthSelectViewModel.MonthPlan);
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var vm = obj as IrregularItemViewModel;
            if (vm == null)
            {
                return false;
            }
            this.UpdateFrequence();
            return this.Frequence.Worker.Equals(vm.Frequence.Worker)
                && this.Frequence.Irregular.Equals(vm.Frequence.Irregular);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
