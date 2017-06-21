using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services
{
    public interface IFrequenceService
    {
        List<Frequence> GetAllFrequence();//获取所有的班次信息

        List<Frequence> GetAllFrequence(Route route); //获取指定线路的班次信息,包括有规律排班,无规律排班,班次计数员信息

        bool AddFrequence(Frequence f, out int id, out string errorInfo);

        //更新班次的基本信息,并重新生成该班次当天的值班表
        bool UpdateFrequence(Frequence f, out string errorInfo); 

        bool DeleteFrequence(Frequence f);

        bool UpdateFrequenceWorker(Frequence f); //更新班次的计数员(如果不指定,将删除之前指定的计数员记录)

        bool UpdateFrequenceRegular(Frequence f, out string errorInfo); //更新按周排班信息

        bool UpdateFrequenceIrregular(Frequence f, out string errorInfo); //更新无规律排班信息

        void Init();
    }
}
