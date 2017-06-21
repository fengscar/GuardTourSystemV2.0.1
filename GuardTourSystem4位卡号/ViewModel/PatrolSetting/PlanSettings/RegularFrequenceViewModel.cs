using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Utils;
using GuardTourSystem.ViewModel.Custom;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GuardTourSystem.ViewModel
{
    //按周排班界面的 ViewModel
    public class RegularFrequenceViewModel : BaseFrequenceViewModel
    {
        // 该界面不会 动态的添加 Item
        // 其他界面添加班次 导致item count 变化时
        // 将调用 init() ,直接重新赋值
        // 所以不需要用ObservableCollection
        private List<RegularItemViewModel> regularItems;
        public List<RegularItemViewModel> RegularItems
        {
            get { return regularItems; }
            set
            {
                SetProperty(ref this.regularItems, value);
            }
        }

        //private bool? showSaveAndCancel;
        //public bool? ShowSaveAndCancel //是否显示 保存和撤销按键  ( 请使用 ShowSaveCancelButton 方法 来改变显示模式 )
        //{
        //    get { return showSaveAndCancel; }
        //    set
        //    {
        //        SetProperty(ref this.showSaveAndCancel, value);
        //    }
        //}

        public DelegateCommand CSave { get; set; }  //保存 数据到数据库
        public DelegateCommand CCancel { get; set; } //撤销, 重新载入数据
        //public DelegateCommand DataChanged { get; set; } //当数据(CommoBox的Worker,RegularSelectControl的安排) 被修改后 所绑定的Command


        public RegularFrequenceViewModel()
        {
            //先初始化按键,再初始化数据,因为数据改变时需要传入按键
            this.CSave = new DelegateCommand(this.Save, this.CanSave);
            this.CCancel = new DelegateCommand(this.Cancel, this.CanSave); //能保存,就能撤销
            InitData();

            //this.DataChanged = new DelegateCommand(() => { ShowSaveCancelButton(true); });
            //ShowSaveCancelButton(false);
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            var regularFreq = GetFrequenceData().FindAll(f => { return f.IsRegular == true; });
            this.RegularItems = new List<RegularItemViewModel>();

            var selectionWorkers = GetWorkerData();
            foreach (var freq in regularFreq)
            {
                var itemVM = new RegularItemViewModel(freq, selectionWorkers, () =>
                {
                    this.CSave.RaiseCanExecuteChanged();
                    this.CCancel.RaiseCanExecuteChanged();
                });
                this.RegularItems.Add(itemVM);
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
            /// 1. 保存排班信息
            string error = "";
            foreach (var item in RegularItems)
            {
                //保存 班次计数员
                FrequenceService.UpdateFrequenceWorker(item.Frequence);
                //保存 有规律排班表
                var success = FrequenceService.UpdateFrequenceRegular(item.Frequence, out error);
                //如果有错,抛出异常
                if (!success)
                {
                    throw new Exception("保存按周排班信息时出错");
                }
            }

            /// 2. 如果保存成功,重新拷贝并初始化按键状态(显示为不可用状态)
            //找到有改变的Items
            var changedItems = RegularItems.FindAll(item => item.IsChanged);
            //重新深拷贝,来初始化IsChanged)
            changedItems.ForEach(item => item.SaveInitValue());
            this.CSave.RaiseCanExecuteChanged();
            this.CCancel.RaiseCanExecuteChanged();


            /// 3. 如果用户要重新生成,那就重新生成之前 !有改变! 的排班当天的值班表
            var message = new StringBuilder();
            int i = 1;
            foreach (var item in changedItems)
            {
                message.Append(i + ".    " + item.Frequence.Name + "\n");
                i++;
            }
            //弹出提示框,让用户进行选择
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
        /// <summary>
        /// 如果没有改动,不需要保存
        ///     通过判断每个Item来确定是否有改动...
        /// </summary>
        /// <returns></returns>
        private bool CanSave()
        {
            if (RegularItems == null || RegularItems.Count == 0)
            {
                return false;
            }
            return RegularItems.Any(item => item.IsChanged);
        }

        /// <summary>
        /// 撤销修改
        /// </summary>
        public void Cancel()
        {
            InitData();
            //ShowSaveCancelButton(false);
        }

        //public void ShowSaveCancelButton(bool show)
        //{
        //    if (show)
        //    {
        //        ShowSaveAndCancel = true;
        //    }
        //    else
        //    {
        //        ShowSaveAndCancel = null;
        //    }
        //}

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
    //每条数据的 ViewModel
    public class RegularItemViewModel : BindableBase
    {
        public RegularItemViewModel() { }

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

        //可供选择的计数员
        public List<Worker> Workers { get; set; }

        //按周排班的 7个 CheckBox对应的ViewModel
        private WeekSelectViewModel weekSelectViewModel;
        public WeekSelectViewModel WeekSelectViewModel
        {
            get { return weekSelectViewModel; }
            set
            {
                SetProperty(ref this.weekSelectViewModel, value);
            }
        }

        //存放初始的Item
        private RegularItemViewModel OldItem;
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
            this.OldItem = (RegularItemViewModel)this.DeepCopy();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="selectionWorkers"></param>
        /// <param name="OnItemChanged">当Item的值改变时,要执行的外部方法</param>
        public RegularItemViewModel(Frequence freq, List<Worker> selectionWorkers, Action OnItemChanged)
        {
            //初始化数据改变时的Action
            this.ItemChangeAction = OnItemChanged;

            this.Frequence = freq;
            this.Frequence.SetWorkerChangeAction(OnItemChanged);//监听Worker的改变

            //初始化 计数员的可选项 和 选中项
            this.Workers = selectionWorkers;
            int workerID = Frequence.Worker == null ? -1 : Frequence.Worker.ID;
            var findedWorker = selectionWorkers.Find(w => { return w.ID == workerID; });
            this.Frequence.Worker = findedWorker;

            //初始化按周排班的原始数据
            WeekSelectViewModel = new WeekSelectViewModel(freq.Regular, this.ItemChangeAction += OnCheckChanged);

            //保存初始值,用来判断是否有修改
            SaveInitValue();
        }



        private void OnCheckChanged()
        {
            this.Frequence.Regular.SetPatrol(
                          WeekSelectViewModel.Mon,
                          WeekSelectViewModel.Tue,
                          WeekSelectViewModel.Wed,
                          WeekSelectViewModel.Thu,
                          WeekSelectViewModel.Fri,
                          WeekSelectViewModel.Sat,
                          WeekSelectViewModel.Sun);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var vm = obj as RegularItemViewModel;
            if (vm == null)
            {
                return false;
            }
            return this.Frequence.Worker.Equals(vm.Frequence.Worker)
                && this.WeekSelectViewModel.Equals(vm.WeekSelectViewModel);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
