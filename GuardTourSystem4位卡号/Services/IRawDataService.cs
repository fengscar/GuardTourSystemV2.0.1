using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
namespace GuardTourSystem.Database.BLL
{
    interface IRawDataService
    {
        ////获取 时间 ( begin-end 之间的 )原始数据
        List<RawData> GetAllRawData(DateTime? begin = null, DateTime? end = null);

        //// 巡检机数据 + 线路,时间,巡检员信息 ==> 原始数据 
        List<RawData> GenerateRawData(List<DevicePatrolRecord> patrols);

        //删除指定日期的 原始数据 (只操作 T_RawData)
        bool DelRawData(DateTime begin, DateTime end);

        //删除T_DeviceRecord和T_RawData中的数据
        void Init();
    }
}
