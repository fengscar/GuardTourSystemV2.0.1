using GuardTourSystem.Database.BLL;

using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Utils;
using GuardTourSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GuardTourSystem.View
{
    /// <summary>
    /// TestView.xaml 的交互逻辑
    /// </summary>
    public partial class TestView : ShowContentView
    {
        public TestView()
        {
            InitializeComponent();
            this.StartTime.SelectedValue = DateTime.Now.Subtract(DateTime.Now.TimeOfDay);
            this.EndTime.SelectedValue = DateTime.Now.Subtract(DateTime.Now.TimeOfDay).AddDays(1);
        }


        //开始生成 随机数据
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AppStatusViewModel.Instance.ShowProgress(true, "正在生成随机巡检机数据");
            var count = Convert.ToInt32(this.Count.Text);
            DateTime startTime = (DateTime)this.StartTime.SelectedValue;
            DateTime endTime = (DateTime)this.EndTime.SelectedValue;
            new Task(() =>
           {
               try
               {
                   if (startTime == null || endTime == null || startTime > endTime)
                   {
                       MessageBox.Show("请输入正确的时间");
                       return;
                   }

                   var random = new Random();
                   var workers = new WorkerBLL().GetAllWorker();
                   var places = new PlaceBLL().GetAllPlace();
                   var events = new EventBLL().GetAllEvent();

                   if (workers.Count == 0 || places.Count == 0 || events.Count == 0)
                   {
                       MessageBox.Show("请先去编辑地点/事件/人员信息,确认每个类型都有数据");
                       return;
                   }

                   var deviceID = "ACDCACDCAC";
                   var result = new List<DevicePatrolRecord>();
                   for (int i = 0; i < count; i++)
                   {
                       //随机生成 一个类型
                       int type = random.Next(10);
                       //随机生成一个 时间
                       var seconds = endTime.Subtract(startTime).TotalSeconds;
                       DateTime ranTime = startTime.AddSeconds(random.Next((int)seconds));
                       //随机获取一个钮号
                       string cardByte;
                       switch (type)
                       {
                           case 0://人员卡
                               int index0 = random.Next(workers.Count);
                               cardByte = workers[index0].Card;
                               break;
                           case 1: //事件卡
                               int index1 = random.Next(events.Count);
                               cardByte = events[index1].Card;
                               break;

                           case 2://地点卡
                           default:
                               int index2 = random.Next(places.Count);
                               cardByte = places[index2].Card;
                               break;
                       }
                       //生成一个 MachineRecord
                       var machineRecord = new DevicePatrolRecord(deviceID, ranTime, cardByte);
                       result.Add(machineRecord);
                   }
                   Application.Current.Dispatcher.BeginInvoke(new Action(() => { new ReadPatrolViewModel().HandleDeviceData(result); }));
               }
               catch (Exception ec)
               {
                   DBug.w(ec);
                   MessageBox.Show("请输入正确的信息");
               }
               AppStatusViewModel.Instance.ShowCompany();
           }).Start();
        }

        private void Button_Click_Reanalysis(object sender, RoutedEventArgs e)
        {
            DateTime startTime = (DateTime)this.StartTime.SelectedValue;
            DateTime endTime = (DateTime)this.EndTime.SelectedValue;
            new DutyBLL().UpdateDuty(startTime, endTime);
        }


        private void Button_Click_Regenerate(object sender, RoutedEventArgs e)
        {
            DateTime startTime = (DateTime)this.StartTime.SelectedValue;
            DateTime endTime = (DateTime)this.EndTime.SelectedValue;
            string error;
            new DutyBLL().GenerateDuty(out error, null, startTime, endTime);
        }

        private void Add_Random_Worker(object sender, RoutedEventArgs e)
        {
            var bll = new WorkerBLL();
            Random random = new Random();
            int id;
            string error;
            for (int i = 0; i < 100; i++)
            {
                var worker = new Worker();
                worker.Name = "随机" + random.Next(10000);
                worker.Card = "FFFFFF" + random.Next(10000).ToString();
                if (!bll.AddWorker(worker, out id, out error))
                {
                    i--;
                };
            }
        }

        private void Add_Random_Route(object sender, RoutedEventArgs e)
        {
            var bll = new RouteBLL();
            Random random = new Random();
            int id;
            string error;
            for (int i = 0; i < 10; i++)
            {
                var route = new Route();
                route.RouteName = "路线" + random.Next(1000);
                if (!bll.AddRoute(route, out id, out error))
                {
                    i--;
                };
            }
        }

        private void Add_Random_Place(object sender, RoutedEventArgs e)
        {
            var bll = new PlaceBLL();
            Random random = new Random();
            int id;
            string error;
            var routes = new RouteBLL().GetAllRoute();
            if (routes.Count == 0)
            {
                return;
            }
            for (int i = 0; i < 100; i++)
            {
                var routeIndex = random.Next(routes.Count);
                var place = new Place();
                place.RouteID = routes[routeIndex].ID;
                place.Name = "随机" + random.Next(10000);
                place.Card = "FFFFFF" + random.Next(10000).ToString();
                if (!bll.AddPlace(place, out id, out id, out error))
                {
                    i--;
                };
            }
        }
    }
}
