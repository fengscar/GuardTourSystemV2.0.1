using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services
{
    interface IUserService
    {
        List<User> GetAllUser();

    }
}
