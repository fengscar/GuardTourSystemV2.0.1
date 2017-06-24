using GuardTourSystem.Database.BLL;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.BLL;
using GuardTourSystem.Services.Database.DAL;
using GuardTourSystem.Utils;
using KaiheSerialPortLibrary;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GuardTourSystem.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class BatchAddViewModel : ShowContentViewModel, ISerialPortStateChangedListener
    {
        // 标题 ( 比如批量添加地点)
        private string title;
        public new string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
                //SetProperty(ref this.title, value);
            }
        }
        // 读取后清空
        private bool clearAfterRead;
        public bool ClearAfterRead
        {
            get { return clearAfterRead; }
            set
            {
                clearAfterRead = value;
                RaisePropertyChanged("ClearAfterRead");
                //SetProperty(ref this.clearAfterRead, value);
            }
        }

        //读取到的Items
        private ObservableCollection<AddItem> addItems;
        public ObservableCollection<AddItem> AddItems
        {
            get { return addItems; }
            set
            {
                addItems = value;
                RaisePropertyChanged("AddItems");
                //SetProperty(ref this.addItems, value);
            }
        }

        private bool selectAll;
        public bool SelectAll
        {
            get { return selectAll; }
            set
            {
                //如果值没有改变,return 
                var changed = value != selectAll;
                if (!changed)
                {
                    return;
                }
                selectAll = value;
                RaisePropertyChanged("SelectAll");
                //SetProperty(ref this.selectAll, value);

                foreach (var item in AddItems)
                {
                    item.Select = value;
                }
            }
        }
        /// <summary>
        /// 当每个选项的 Select改变时:
        /// 1. SelectAll重新赋值 ( 当且仅当Items全部选中=> true)
        /// 2. CBatchAdd重新判断能否执行 ( 当且仅当 有任意一个Items选中)
        /// </summary>
        private void OnAddItemSelectChanged()
        {
            // 使用手动触发...(改变selectAll的值,然后OnPropertyChanged,避免触发 SelectAll中 Set的For循环)
            selectAll = AddItems.All((item) => { return item.Select == true; });
            RaisePropertyChanged("SelectAll");

            CBatchAdd.RaiseCanExecuteChanged();
        }

        // 是否隐藏已存在的卡 2017年3月15日12:54:04 默认就是隐藏
        //private bool hideExistsCard;

        //public bool HideExistsCard
        //{
        //    get { return hideExistsCard; }
        //    set
        //    {
        //        SetProperty(ref this.hideExistsCard, value);
        //    }
        //}

        /// <summary>
        /// 获取当前巡更机数据,并进行自动去重
        /// </summary>
        public DelegateCommand CReadRecords { get; set; }
        /// <summary>
        /// 清空当前所有 AddItems
        /// </summary>
        public DelegateCommand CClearRecords { get; set; }
        /// <summary>
        /// 批量添加当前所选的 AddItems
        /// </summary>
        public DelegateCommand CBatchAdd { get; set; }

        /// <summary>
        /// 单个读卡功能,在5秒内监听计数机读到的卡,如果没有读卡,则返回 5个0x00 
        /// </summary>
        //public DelegateCommand CSingleRead { get; set; }


        /// <summary>
        /// 点击批量添加, 对所有选中的AddItem执行的操作
        /// </summary>
        private Func<List<AddItem>, bool> BatchAddFunc { get; set; }
        /// <summary>
        /// 获取到计数数据并去重后 调用的方法
        /// 注意: 如果获取数据失败,将返回NULL; 如果没有可用数据,将返回Count为0的一个List
        /// </summary>
        private Action<List<AddItem>> GetRecordsAction { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OnBatchAdd"> 点击批量添加, 对所有选中的AddItem执行的操作 </param>
        /// <param name="OnGetRecords"> 获取到计数数据并去重后 调用的方法 </param>
        /// <param name="title"></param>
        public BatchAddViewModel(string title, Func<List<AddItem>, bool> OnBatchAdd, Action<List<AddItem>> OnGetRecords = null)
        {
            SerialPortManager.Instance.AddListener(this);

            if (OnBatchAdd == null)
            {
                throw new Exception("未定义批量添加方法");
            }
            BatchAddFunc = OnBatchAdd;

            if (OnGetRecords != null)
            {
                GetRecordsAction = OnGetRecords;
            }

            Title = title;
            AddItems = new ObservableCollection<AddItem>();
            ClearAfterRead = true;

            CReadRecords = new DelegateCommand(GetRecords, () => !SerialPortManager.Instance.IsWritting);
            CClearRecords = new DelegateCommand(ClearRecords);
            CBatchAdd = new DelegateCommand(BatchAdd, () => { return AddItems.Any((item) => { return item.Select == true; }); });
            //this.CSingleRead = new DelegateCommand(this.SingleRead, () => !SerialPortManager.Instance.IsWritting);
        }


        /// <summary>
        /// 1. 获取计数记录,并进行去重.
        /// 2. 调用 GetRecordsAction() 来通知调用者
        /// 3. 添加数据到 AddItems
        /// 4. 清空计数机数据(如果选中)
        /// </summary>
        private async void GetRecords()
        {
            ClearRecords();

            //获取计数机的计数数据
            var flow = await AppSerialPortUtil.GetAllPatrolRecord();

            // 获取数据错误
            if (!flow.Check())
            {
                ShowMessageDialog(LanLoader.Load(LanKey.BatchAddReadErrorConnect), LanLoader.Load(LanKey.BatchAddReadErrorConnectExp));
                return;
            }
            var patrolRecords = (List<PatrolRecord>)flow.Value;
            bool needClearMachine = patrolRecords.Count != 0;
            if (patrolRecords.Count != 0)
            {
                // 使用Distinct 去除相同钮号的数据
                patrolRecords = patrolRecords.Distinct(new PatrolRecordComparer()).ToList();

                // 去除 当前数据库中已使用过的钮号 
                patrolRecords.RemoveAll((record) =>
                {
                    string error = "";
                    return !PatrolSQLiteManager.CheckCardUnique(record.Card, ref error);
                });

                //转成 AddItem
                int index = 1;
                foreach (var record in patrolRecords)
                {
                    var Item = new AddItem(OnAddItemSelectChanged);
                    Item.Index = index++;
                    Item.Card = record.Card;
                    Item.Select = true;//默认是选中的

                    AddItems.Add(Item);
                }
            }

            if (AddItems.Count == 0)
            {
                ShowMessageDialog(LanLoader.Load(LanKey.BatchAddReadErrorEmpty), LanLoader.Load(LanKey.BatchAddReadErrorEmptyExp));
            }

            if (GetRecordsAction != null)
            {
                GetRecordsAction(AddItems.ToList());
            }

            //读取后清空计数器
            if (ClearAfterRead && needClearMachine)
            {
                AppStatusViewModel.Instance.ShowProgress(true, "请稍等...正在清空打卡机数据", 40000);
                await SerialPortUtil.Write(new ClearPatrolRecord());
                AppStatusViewModel.Instance.ShowInfo("打卡机数据清空完成.");
            }
        }

        /// <summary>
        /// 清空当前的 AddItems
        /// </summary>
        private void ClearRecords()
        {
            AddItems.Clear();
            CBatchAdd.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// 批量添加当前所选的 Item
        /// </summary>
        private void BatchAdd()
        {
            // 重置 Error
            AddItems.ToList().ForEach((item) => { item.Error = null; });

            ////获取选中的Item
            var selectItems = from item in AddItems
                              where item.Select == true
                              select item;

            //获取所有名称不为空并且有重复的item ( 也可以简单的遍历每个元素 并调用 Single来判断)
            var repeatNameItems = selectItems.Where(item => { return !string.IsNullOrEmpty(item.Name); }) //获取所有选中的名称不为空的Item
                                      .GroupBy(item => { return item.Name; }) //根据名称进行分组 : 该条命令将返回多条IGroupping结构的数据,IGroupping结构为<TKey,TElement> .就是说一个名称(KEY)对应多个AddItem项(Element);
                                      .Where(item => item.Count() > 1) //获取所有名称有重复的分组
                                      .SelectMany(item => { return item; }); //将分组转换成 list

            var repeatEmployeeItems = selectItems.Where(item => { return !string.IsNullOrEmpty(item.EmployeeNumber); }) //获取所有选中的名称不为空的Item
                                    .GroupBy(item => { return item.EmployeeNumber; }) //根据名称进行分组 : 该条命令将返回多条IGroupping结构的数据,IGroupping结构为<TKey,TElement> .就是说一个名称(KEY)对应多个AddItem项(Element);
                                    .Where(item => item.Count() > 1) //获取所有名称有重复的分组
                                    .SelectMany(item => { return item; }); //将分组转换成 list


            if (repeatNameItems.Count() > 0)
            {
                repeatNameItems.ToList().ForEach(item => { item.Error = LanLoader.Load(LanKey.BatchAddReadErrorExists); });
            }
            else if (repeatEmployeeItems.Count() > 0)
            {
                repeatEmployeeItems.ToList().ForEach(item => item.Error = "该工号在选中项中出现多次");
            }

            if (repeatEmployeeItems.Count() > 0 || repeatEmployeeItems.Count() > 0)
            {
                return; //不再继续判断其他项
            }


            //调用传入的BatchAddFunc来判断添加是否成功
            if (BatchAddFunc(selectItems.ToList())) //添加成功,从Items中移除当前被选中的项
            {
                for (int i = AddItems.Count - 1; i >= 0; i--)
                {
                    if (AddItems[i].Select)
                    {
                        AddItems.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// 在5秒内监听计数机信息
        /// </summary>
        //private async void SingleRead()
        //{
        //    var singleReadFlow = await SerialPortUtil.Write(new GetSingleRead() { WaitTime = 5000 });
        //    if (!singleReadFlow.Check())
        //    {
        //        ShowMessageDialog(LanLoader.Load(LanKey.BatchAddReadErrorConnect), LanLoader.Load(LanKey.BatchAddReadErrorConnectExp));
        //        return;
        //    }

        //    var card = ((GetSingleRead)singleReadFlow).Card;
        //    if ("0000000000".Equals(card)) //表示没有 读到卡
        //    {
        //        ShowMessageDialog(LanLoader.Load(LanKey.BatchAddSingleReadErrorNoCard), LanLoader.Load(LanKey.BatchAddSingleReadErrorNoCardExp));
        //        return;
        //    }

        //    //如果该钮号已在 批量添加中,或者已经在数据库中,进行提示
        //    var existsInBatchAdd = AddItems.Any(item => { return item.Card.Equals(card); });
        //    if (existsInBatchAdd)
        //    {
        //        ShowMessageDialog(LanLoader.Load(LanKey.BatchAddSingleReadErrorCantAdd), LanLoader.Load(LanKey.BatchAddSingleReadErrorCantAddExp));
        //        return;
        //    }
        //    string error = "";
        //    var existsInDataBase = !PatrolSQLiteManager.CheckCardUnique(card, ref error);
        //    if (existsInDataBase)
        //    {
        //        ShowMessageDialog(LanLoader.Load(LanKey.BatchAddSingleReadErrorCantAdd), error);
        //        return;
        //    }


        //    //读卡成功,添加到AddItems中
        //    var Item = new AddItem(this.OnAddItemSelectChanged);
        //    Item.Card = card;
        //    Item.Select = true;//默认是选中的
        //    AddItems.Add(Item);
        //}


        public void onPortWrittingStateChanged(bool isWrite)
        {
            Application.Current.Dispatcher.BeginInvoke(
          new Action(() =>
          {
              //在这里操作UI
              CReadRecords.RaiseCanExecuteChanged();
              //this.CSingleRead.RaiseCanExecuteChanged();
          })
          , null);
        }
    }


    public class PatrolRecordComparer : IEqualityComparer<PatrolRecord>
    {
        public bool Equals(PatrolRecord x, PatrolRecord y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
            return x.Card == y.Card;
        }
        public int GetHashCode(PatrolRecord obj)
        {
            if (obj == null)
            {
                return 0;
            }
            return obj.Card.GetHashCode();
        }
    }

    /// <summary>
    /// 从计数机上读取到 ,准备批量添加的Item
    /// </summary>
    public class AddItem : NotificationObject
    {
        //当每个选项的Select改变时,调用预设的Action
        public Action SelectChangedAction { get; set; }

        private bool select;
        public bool Select
        {
            get { return select; }
            set
            {
                var isChanged = select != value;
                select = value;
                RaisePropertyChanged("Select");

                if (isChanged && SelectChangedAction != null)
                {
                    SelectChangedAction();
                }
            }
        }

        private int index;
        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                RaisePropertyChanged("Index");
            }
        }


        private string card;
        public string Card //钮号
        {
            get { return card; }
            set
            {
                card = value;
                RaisePropertyChanged("Card");
            }
        }
        private string name;
        public string Name// 名称( 可以是姓名,地点名,事件名)
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string employeeNumber;
        public string EmployeeNumber
        {
            get { return employeeNumber; }
            set
            {
                InputChecker.CheckEmployeeNumber(ref value);
                employeeNumber = value;
                RaisePropertyChanged("EmployeeNumber");
            }
        }


        private string error;
        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                RaisePropertyChanged("Error");
            }
        }

        public AddItem(Action onSelectChange)
        {
            SelectChangedAction = onSelectChange;
        }
    }
}
