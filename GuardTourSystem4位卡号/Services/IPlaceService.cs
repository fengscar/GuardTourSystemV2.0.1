using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services
{
    interface IPlaceService
    {
        List<Place> GetAllPlace(Route route);
        List<Place> GetAllPlace();
        bool CanAdd(Place p,out string errorInfo);
        bool AddPlace(Place p, out int id, out int routeOrder,out string errorInfo);
        bool UpdatePlace(Place p,out string errorInfo);
        bool DelPlace(Place p);

        void Init();
    }
}
