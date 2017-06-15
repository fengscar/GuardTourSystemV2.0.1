using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services
{
    public interface IEventService
    {
        List<Event> GetAllEvent();

        bool CanAdd(Event e, out string errorInfo);

        bool AddEvent(Event e, out int id,out string errorInfo);

        bool UpdateEvent(Event e,out string errorInfo);

        bool DelEvent(Event e);

        void Init();
    }
}
