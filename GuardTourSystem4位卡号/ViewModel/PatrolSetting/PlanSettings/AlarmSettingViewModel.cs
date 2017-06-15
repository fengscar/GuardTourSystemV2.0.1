using GuardTourSystem.Database.BLL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Utils;
using KaiheSerialPortLibrary;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GuardTourSystem.ViewModel
{
    // 班次信息 弹出框的ViewModel
    public class AlarmSettingViewModel : AbstractPopupNotificationViewModel, ISerialPortStateChangedListener
    {

        public List<FrequenceItem> Frequences { get; set; }

        public IFrequenceService FrequenceService { get; set; }

        public DelegateCommand CClearAlarm { get; set; }
        public DelegateCommand CLoadFrequenceAlarm { get; set; }

        public Visibility LoadFrequenceVisible { get; set; }

        private string info;
        public string Info
        {
            get { return info; }
            set
            {
                SetProperty(ref this.info, value);
            }
        }

        private ObservableCollection<AlarmItem> alarms;
        public ObservableCollection<AlarmItem> Alarms
        {
            get { return alarms; }
            set
            {
                SetProperty(ref this.alarms, value);
            }
        }


        public AlarmSettingViewModel(List<Frequence> frequences)
            : base()
        {
            Title = "闹钟设置";


            InitFrequence(frequences);

            InitAlarm();

            this.CConfirm = new DelegateCommand(this.SetAlarm, () => !SerialPortManager.Instance.IsWritting && Alarms.Count(alarm => alarm.IsSelect && alarm.AlarmTime != null) > 0);
            this.CClearAlarm = new DelegateCommand(this.ClearAlarm, () => !SerialPortManager.Instance.IsWritting);
            this.CLoadFrequenceAlarm = new DelegateCommand(this.LoadFrequenceAlarm);

        }

        private void InitFrequence(List<Frequence> frequences)
        {
            Frequences = new List<FrequenceItem>();
            foreach (var item in frequences)
            {
                Frequences.Add(new FrequenceItem() { Frequence = item });
            }
            LoadFrequenceVisible = Frequences.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void InitAlarm()
        {
            Alarms = new ObservableCollection<AlarmItem>();
            for (int i = 0; i < 24; i++)
            {
                Alarms.Add(new AlarmItem(true, TimeSpan.FromHours(i), () => this.CConfirm.RaiseCanExecuteChanged()));
            }
        }

        private async void SetAlarm()
        {
            ErrorInfo = null;
            Info = "正在设置巡检机闹钟...";

            //先清除
            var clearAlarmFlow = await SerialPortUtil.Write(new ClearAlarm() { WaitTime = 2000 });
            if (!clearAlarmFlow.Check())
            {
                MessageBox.Show("闹钟设置失败:" + clearAlarmFlow.ResultInfo);
                return;
            }

            int index = 0;
            foreach (var item in Alarms)
            {
                if (!item.IsSelect || item.AlarmTime == null)
                {
                    continue;
                }

                var setAlarmFlow = await SerialPortUtil.Write(new SetAlarm(index, (TimeSpan)item.AlarmTime));
                if (!setAlarmFlow.Check())
                {
                    MessageBox.Show("闹钟设置失败:" + setAlarmFlow.ResultInfo);
                    return;
                }
                index++;
            }
            MessageBox.Show("成功设置了" + index + "个闹钟");
            Info = null;
        }

        private async void ClearAlarm()
        {
            ErrorInfo = null;
            Info = "正在清空巡检机闹钟...";

            var clearAlarmFlow = await SerialPortUtil.Write(new ClearAlarm() { WaitTime = 2000 });
            if (clearAlarmFlow.Check())
            {
                MessageBox.Show("巡检机闹钟清空成功!");
            }
            else
            {
                MessageBox.Show("巡检机闹钟清空失败:" + clearAlarmFlow.ResultInfo);
            }
            Info = null;
        }
        private void LoadFrequenceAlarm()
        {
            this.ErrorInfo = null;
            foreach (var item in Alarms)
            {
                item.IsSelect = false;
            }
            var selectFrequence = Frequences.Where(f => f.IsSelect).ToList();
            int index = 0;
            foreach (var item in selectFrequence)
            {
                var freq = item.Frequence;
                var patrolTimes = GetStartPatrolTime(freq);
                foreach (var time in patrolTimes)
                {
                    if (index >= 24)
                    {
                        index++;
                        this.Info = "当前选中的班次可设置" + index + "个闹钟";
                        this.ErrorInfo = "最多只能设置24个闹钟,超出的将被忽略!";
                        break;
                    }
                    var alarm = new AlarmItem(true, time, () => this.CConfirm.RaiseCanExecuteChanged());
                    if (!Alarms.Contains(alarm))
                    {
                        Alarms[index] = alarm;
                        index++;
                    }
                }
            }
        }
        private List<TimeSpan> GetStartPatrolTime(Frequence freq)
        {
            var result = new List<TimeSpan>();
            var begin = (TimeSpan)freq.StartTime;
            var end = (TimeSpan)freq.EndTime;
            var oneTime = TimeSpan.FromMinutes(freq.PatrolTime + freq.RestTime);
            while (end > begin)
            {
                result.Add(begin);
                begin += oneTime;
            }
            return result;
        }


        public void onPortWrittingStateChanged(bool isWrite)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(
              () =>
              {
                  //在这里操作UI
                  this.CConfirm.RaiseCanExecuteChanged();
                  this.CClearAlarm.RaiseCanExecuteChanged();
              }), null);
        }

    }
    public class AlarmItem : BindableBase
    {
        public Action OnItemChange { get; set; }

        private bool isSelect;
        public bool IsSelect
        {
            get { return isSelect; }
            set
            {
                SetProperty(ref this.isSelect, value);
                if (OnItemChange != null)
                {
                    OnItemChange();
                }
            }
        }

        private TimeSpan? alarmTime;

        public TimeSpan? AlarmTime
        {
            get { return alarmTime; }
            set
            {
                SetProperty(ref this.alarmTime, value);
                if (alarmTime == null)
                {
                    this.IsSelect = false;
                }
                else
                {
                    this.IsSelect = true;
                }
            }
        }

        public AlarmItem(bool isSelect, TimeSpan alarm, Action onItemChange)
        {
            this.IsSelect = isSelect;
            this.AlarmTime = alarm;
            this.OnItemChange = onItemChange;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is AlarmItem))
            {
                return false;
            }
            var item = obj as AlarmItem;
            if (item.AlarmTime == null || this.AlarmTime == null)
            {
                return false;
            }
            return item.AlarmTime.Value.TotalSeconds == this.AlarmTime.Value.TotalSeconds
                && item.IsSelect == this.IsSelect;
        }
    }
    public class FrequenceItem : BindableBase
    {
        public Frequence Frequence { get; set; }
        private bool isSelect;

        public bool IsSelect
        {
            get { return isSelect; }
            set
            {
                SetProperty(ref this.isSelect, value);
            }
        }

    }
}
