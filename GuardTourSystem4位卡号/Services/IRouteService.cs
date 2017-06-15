using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services
{
    public interface IRouteService
    {
        List<Route> GetAllRoute(bool attachPlaces=true,bool attachFrequences=true);
        bool AddRoute(Route route, out int id,out string errorInfo); // 添加成功,out RouteID ;失败 out -1;
        bool UpdateRoute(Route route,out string errorInfo);
        bool DelRoute(Route routeww);

        void Init();//清空Route表,并将自增ID从SQLite_SEQUENCE中删除
    }
}
