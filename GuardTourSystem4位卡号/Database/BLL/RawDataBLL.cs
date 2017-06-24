using GuardTourSystem.Database.DAL;
using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using GuardTourSystem.Settings;
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
            var result = RawDataDAO.GetAllRawData(begin, end);
            result.Where(r => string.IsNullOrEmpty(r.RouteName)).ToList().ForEach(r =>
            {
                r.RouteName = "(未设置)";
                r.Place.Name = "(未设置的人员)";
                r.Place.EmployeeNumber = "(未设置的人员)";
            });
            return result;
        }

        //datas已经按 时间排序了
        public List<RawData> GenerateRawData(List<DevicePatrolRecord> datas)
        {
            var routes = new RouteBLL().GetAllRoute(true, false);
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
            foreach (var data in datas)
            {
                var oneRawData = new RawData()
                {
                    TRead = DateTime.Now,
                    Device = deviceID,
                    //Worker = curWorker,
                    PlaceTime = data.Time,
                };

                foreach (var route in routes)
                {
                    var place = route.Places.ToList().Find(p => { return p.Card.Equals(data.Card); });
                    if (place != null)
                    {
                        oneRawData.RouteName = route.RouteName;
                        oneRawData.Place = place;
                        break;
                    }
                }
                if (oneRawData.Place == null)
                {
                    oneRawData.Place = new Place() { Name = "", Card = data.Card };
                }
                rawDatas.Add(oneRawData);
            }
            //保存原始数据
            string error;
            if (!AddRawData(rawDatas, out error))
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
