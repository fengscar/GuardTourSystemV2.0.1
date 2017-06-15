
using GuardTourSystem.Model;
using GuardTourSystem.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services
{
    interface IDutyService
    {
        //查询T_Duty和T_Record 表, 获取值班信息
        List<Duty> GetAllDuty(DateTime? begin = null, DateTime? end = null);

        /// 生成 指定日期的值班表,并保存
        /// 该操作将 先删除 再添加, 所以日期参数要注意(当Frequence已删除或者地点删除后,都将影响该日期的值班表)
        /// 如果参数End的时间更大,还会更新Frequence的Generated字段
        int GenerateDuty(out string error, Frequence targetFreq = null, DateTime? start = null, DateTime? end = null);

        // 更新考核结果
        bool UpdateDuty(List<RawData> rawdatas);

        // 重新分析( 重新生成指定时间段内的 考核结果, 即将 RawData和 Duty+Record重新配对)
        bool UpdateDuty(DateTime begin, DateTime end);

        void Init();//删除T_duty和T_Record中的数据
    }
}
