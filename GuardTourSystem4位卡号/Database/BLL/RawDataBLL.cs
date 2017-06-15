using GuardTourSystem.Database.DAL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using GuardTourSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.BLL
{
    public class RawDataBLL : IRawDataService
    {
        public RawDataDAO RawDataDAO { get; set; }
        public DeviceDataDAO DeviceDataDAO { get; set; }
        public RawDataBLL()
        {
            RawDataDAO = new RawDataDAO();
            DeviceDataDAO = new DeviceDataDAO();
        }
        public List<RawData> GetAllRawData(DateTime? begin = null, DateTime? end = null)
        {
            return RawDataDAO.GetAllRawData(begin, end);
        }

        public List<RawData> GenerateRawData(List<DevicePatrolRecord> datas)
        {
            var routes = new RouteBLL().GetAllRoute(true, false);
            var events = new EventBLL().GetAllEvent();
            var workers = new WorkerBLL().GetAllWorker();

            var places = new List<Place>();
            foreach (var item in routes)
            {
                places.AddRange(item.Places);
            }

            //生成原始数据
            var rawDatas = new List<RawData>();
            if (datas.Count <= 0)
            {
                return rawDatas;
            }

            string deviceID = null;
            Worker curWorker = null;
            Event curEvent = null;
            DateTime? curEventTime = null;
            foreach (var data in datas)
            {
                var findWorker = workers.Find(w => { return w.Card.Equals(data.Card); });
                //如果机号改变 ==> 改变当前机号,当前巡检员,并清空事件( 机号都变了,事件肯定不能再用)
                if (!data.Device.Equals(deviceID))
                {
                    deviceID = data.Device;
                    // 设置当前巡检员 
                    curWorker = Worker.DefaultWorker;
                    curWorker.Card = "00" + data.Device;
                    curEvent = null;
                }

                //如果是人员卡 ==> 改变当前巡检员,并continue
                if (findWorker != null)
                {
                    curWorker = findWorker;
                    continue;
                }

                //如果是事件卡 ==> 改变当前事件,并continue
                var findEvent = events.Find(e => { return e.Card.Equals(data.Card); });
                if (findEvent != null)
                {
                    curEvent = findEvent;
                    curEventTime = data.Time;
                    continue;
                }

                //如果是地点卡 或者 无法识别的卡, 添加一条记录...
                var oneRawData = new RawData()
                {
                    TRead = DateTime.Now,
                    Device = deviceID,
                    Worker = curWorker,
                    PlaceTime = data.Time,
                };

                var findPlace = places.Find(p => { return p.Card.Equals(data.Card); });
                if (findPlace != null) //如果该地点有存档,查找线路名称并添加
                {
                    oneRawData.Place = findPlace;
                    foreach (var route in routes)
                    {
                        if (route.Places.ToList().Find(p => { return p.Card.Equals(data.Card); }) != null)
                        {
                            oneRawData.RouteName = route.RouteName;
                            break;
                        }
                    }
                }
                else//否则添加一条默认的地点信息
                {
                    oneRawData.Place = new Place() { Name = "", Card = data.Card };
                }
                if (curEvent != null)
                {
                    oneRawData.Event = curEvent;
                    oneRawData.EventTime = curEventTime;
                    curEvent = null;
                }
                rawDatas.Add(oneRawData);

            }
            //保存原始数据
            string error;
            if (!this.AddRawData(rawDatas, out error))
            {
                AppStatusViewModel.Instance.ShowError("生成原始数据时出错:" + error);
            }

            return rawDatas;
        }

        public bool AddRawData(List<RawData> rawDatas, out string errorInfo)
        {
            return RawDataDAO.AddRawData(rawDatas, out errorInfo);
        }


        public void Init()
        {
            RawDataDAO.Init();
            DeviceDataDAO.Init();
        }
        public bool DelRawData(DateTime begin, DateTime end)
        {
            var result = RawDataDAO.DelRawData(begin, end);
            return result;
        }

        public List<DevicePatrolRecord> GetAllDeviceRecord()
        {
            return DeviceDataDAO.GetAllDeviceRecord();
        }

    }
}
