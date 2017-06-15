using GuardTourSystem.Database.DAL;
using GuardTourSystem.Model;
using GuardTourSystem.Model.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.DAL;
using GuardTourSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuardTourSystem.Database.BLL
{
    class DutyBLL : IDutyService
    {
        public FrequenceDAO FrequenceDAO { get; set; }
        public DutyDAO DutyDAO { get; set; }
        public RecordDAO RecordDAO { get; set; }
        public DutyBLL()
        {
            FrequenceDAO = new FrequenceDAO();
            DutyDAO = new DutyDAO();
            RecordDAO = new RecordDAO();
        }


        // 查询 T_Duty + T_Record
        public List<Duty> GetAllDuty(DateTime? begin = null, DateTime? end = null)
        {
            var dutys = DutyDAO.GetAllDuty(begin, end);
            foreach (var duty in dutys)
            {
                duty.Records = RecordDAO.GetRecord(duty);
            }
            return dutys;
        }

        /// <summary>
        ///注意: 如果要根据 数据(Frequence,Place,Worker) 重新生成(指的是删除并新增) 值班表,请传入要重新生成的日期参数
        ///注意: 如果要保留已生成的数据,并生成新增的Frequence的值班表,请不要传入任何日期参数.
        /// 0.初始化时间,开始时间设置为0:00. 结束时间设置为23:59:59
        /// 1.先删除已存在的值班表
        /// 2.生成 指定日期的值班表
        /// 3.如果参数End的时间更大,还会更新Frequence的Generated字段
        /// 4.保存生成的值班表
        /// </summary>
        /// <param name="targetFreq"> 要生成值班表的班次,如果没有指定,将默认生成所有班次的.如果有指定,将只生成该班次的</param>
        /// <param name="startDate"> 生成值班表的开始日期,如果有指定,将从该天重新开始生成</param>
        /// <param name="endDate"> 生成值班表的结束日期,如果有指定,生成完该天的值班表后将停止该函数</param>
        /// <returns>返回生成的值班表的条数</returns>
        public int GenerateDuty(out string error, Frequence targetFreq = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (startDate != null && endDate != null && startDate > endDate)
            {
                error = "开始日期要早于结束日期";
                return -1;
            }
            /// 0.初始化时间,开始时间设置为0:00. 结束时间设置为23:59:59
            // 改动: 在没有指定结束日期时,无论是有规律还是无规律,只生成到当天
            DateTime endTime = endDate ?? DateTime.Now;
            // 默认都设置为当天的23:59:59
            endTime = endTime.Subtract(endTime.TimeOfDay).AddDays(1).AddMilliseconds(-1);

            //获取线路,地点,班次
            var routes = new RouteBLL().GetAllRoute(true, true);

            /// 1.先删除已存在的值班表, 删除时的判断条件是 旧的 Frequence的路线名称和班次名称
            if (startDate != null || endDate != null) //指定了时间,将删除已有的
            {
                DateTime deleteBeginDate = startDate ?? DateTime.Now;
                if (targetFreq != null)
                {
                    this.ClearDuty(targetFreq, deleteBeginDate, endTime);
                }
                else
                {
                    //get all frequence from route
                    var frequences = routes.SelectMany(route => route.Frequences);
                    frequences.ToList().ForEach((freq) => { this.ClearDuty(freq, deleteBeginDate, endTime); });
                }
            }

            /// 2.生成 指定日期的值班表
            var result = new List<Duty>();
            SQLiteHelper.Instance.BeginTransaction();//开启事务
            foreach (var route in routes)
            {
                var places = route.Places.ToList();
                //遍历该线路下的 每个班次
                foreach (var frequence in route.Frequences)
                {
                    //如果有指定要生成的班次,并且当前循环到的班次不是该指定的班次,不进行生成 continue
                    if (targetFreq != null && targetFreq.ID != frequence.ID)
                    {
                        continue;
                    }
                    //开始生成的日期: 如果有指定,使用参数中的; 如果没指定, 默认从上次生成到的日期的后一天开始
                    DateTime startTime = startDate ?? frequence.GeneratedDate.AddDays(1);
                    // 默认都设置为当天的0:00时
                    startTime = startTime.Subtract(startTime.TimeOfDay);

                    //如果该班次生成日期比 实时时间大,说明日期被调整了, 提示出错
                    //if (frequence.GeneratedDate > DateTime.Now)
                    //{
                    //    error = "请确认电脑的系统时间是否正确";
                    //    SQLiteHelper.Instance.CommitTransaction();
                    //    return -1;
                    //}

                    while (endTime.Subtract(startTime).TotalMinutes > 0)
                    {
                        /// 生成指定班次,指定日期 的值班表
                        var duty = this.GenerateOneDayDuty(startTime, frequence, places);
                        startTime = startTime.AddDays(1);

                        result.AddRange(duty);
                    }

                    /// 3.如果参数End的时间更大,还会更新Frequence的Generated字段
                    if (endTime > frequence.GeneratedDate)
                    {
                        frequence.GeneratedDate = endTime;
                        FrequenceDAO.UpdateFrequence(frequence);
                    }
                }
            }

            /// 4.保存生成的值班表
            this.SaveDuty(result);
            SQLiteHelper.Instance.CommitTransaction(); //提交事务
            error = null;
            return result.Count;
        }

        /// <summary>
        /// 生成 指定班次,指定日期 的值班表
        /// </summary>
        /// <param name="date">该计划的日期</param>
        /// <param name="freq">该计划的班次</param>
        /// <param name="places">该计划要巡逻的地点</param>
        /// <returns></returns>
        private List<Duty> GenerateOneDayDuty(DateTime date, Frequence freq, List<Place> places)
        {
            var result = new List<Duty>();
            foreach (var dutyTime in freq.GetDutyTime(date))
            {
                var duty = new Duty();
                duty.Frequence = freq;
                duty.DutyDate = date;
                duty.DutyTime = dutyTime;
                duty.Worker = freq.Worker;
                duty.Records = new List<Record>(); // n个地点对应n个record
                foreach (var place in places)
                {
                    var record = new Record();
                    record.Place = place;
                    duty.Records.Add(record);
                }
                if (duty.Records.Count == 0) //如果该次值班没有任何巡逻地点,无需添加
                {
                    continue;
                }
                result.Add(duty);
            }
            return result;
        }
        //移除 start->end 时间段内 指定班次 已生成的duty(如果存在)
        private void ClearDuty(Frequence frequence, DateTime start, DateTime end)
        {
            //DELETE 指定日期内的值班表 ( 00:00 => 23:59:59)
            DutyDAO.DeleteDuty(frequence, start.Subtract(start.TimeOfDay), end.Subtract(start.TimeOfDay).AddDays(1).AddMilliseconds(-1));
        }
        // 保存生成的Duty
        private void SaveDuty(List<Duty> dutys)
        {
            //ADD 使用事务
            foreach (var duty in dutys)
            {
                int id = -1;
                DutyDAO.AddDuty(duty, out id);
                if (id == -1)
                {
                    throw new Exception("添加数据到T_Duty失败");
                }
                duty.ID = id;
                RecordDAO.AddRecord(duty);
            }
            return;
        }

        // 更新考核结果 (将由计划生成的数据补全)
        // 1. 查找适合匹配的RecordList:  需要先过滤一次 , 使用 原始数据的 PlaceTime来过滤
        // 2. 对每一条 原始数据进行匹配
        public bool UpdateDuty(List<RawData> rawDatas)
        {
            if (rawDatas.Count == 0)
            {
                return true;
            }
            var minTime = rawDatas.Min(raw => raw.PlaceTime);//最早的原始数据
            var maxTime = rawDatas.Max(raw => raw.PlaceTime);//最迟的原始数据

            //获取适合匹配的 RecordList(弃用,没有值班时间,无法与原始数据匹配)
            //var patrolRecords = RecordDAO.GetAllRecord(minTime, maxTime);

            //获取适合匹配的所有 值班
            var dutys = DutyDAO.GetAllDuty(minTime, maxTime);

            //所有要更新的record 
            var records = new List<Record>();
            //对每一条 原始数据进行匹配
            foreach (var raw in rawDatas)
            {
                //先找到适合的值班 
                var targetDuty = dutys.Find(duty =>
                {
                    //该原始记录的时间 是否在该次值班时间内
                    var inDuty = duty.DutyTime.InDuty(raw.PlaceTime);
                    //巡检员是否匹配: 如果值班没有指定巡检员,返回true;如果有指定,必须和原始数据的巡检员钮号匹配
                    var workerMatch = duty.Worker == null ? true : duty.Worker.Card.Equals(raw.Worker.Card);
                    //非foreach的return,这个return是属于匿名函数的...
                    return inDuty && workerMatch;
                });
                //找到该次值班中对应的那条Record
                if (targetDuty == null || targetDuty.Records == null || targetDuty.Records.Count == 0)
                {
                    continue;
                }
                //找到对应地点的record
                var targetRecord = targetDuty.Records.Find(record => { return record.Place.Card.Equals(raw.Place.Card); });
                if (targetRecord == null)
                {
                    continue;
                }

                //更新该Record的信息
                targetRecord.Device = raw.Device;//似乎没有用到
                targetRecord.PlaceTime = raw.PlaceTime;
                targetRecord.ActualWorker = raw.Worker;
                if (targetRecord.Event == null && raw.Event != null) //如果原始数据没有事件,而记录已有事件,则不更新事件
                {
                    targetRecord.Event = raw.Event;
                    targetRecord.EventTime = raw.PlaceTime;
                }
                records.Add(targetRecord);
            }

            return RecordDAO.UpdateRecord(records);
        }

        // IDutyService
        // 重新分析( 重新生成指定时间段内的 考核结果, 即将 RawData和 Duty+Record重新配对)
        public bool UpdateDuty(DateTime begin, DateTime end)
        {
            var rawdatas = new RawDataDAO().GetAllRawData(begin, end);
            return this.UpdateDuty(rawdatas);
        }


        public void Init()
        {
            DutyDAO.Init();
            RecordDAO.Init();

            new RegularDAO().Init();
            new IrregularDAO().Init();
        }
    }
}
