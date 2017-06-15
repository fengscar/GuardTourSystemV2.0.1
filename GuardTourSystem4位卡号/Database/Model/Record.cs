using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model.Model
{
    //一条巡逻记录 
    // 属性有: 巡逻安排 + 实际巡逻
    public class Record : BindableBase
    {
        #region 从巡检计划 加载
        public int ID { get; set; }

        public int DutyID { get; set; }//值班信息 ID

        public Place Place { get; set; } //巡检地点

        #endregion

        #region 由 原始数据 更新
        public string Device { get; set; } //设备号

        public Worker ActualWorker { get; set; } //实际的巡检员

        public DateTime? PlaceTime { get; set; }// 实际的巡逻时间

        public Event Event { get; set; } //巡逻事件 可为空
        public DateTime? EventTime { get; set; }// 实际的事件时间

        #endregion


    }
}
