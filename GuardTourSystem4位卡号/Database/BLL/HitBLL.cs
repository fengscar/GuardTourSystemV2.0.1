using GuardTourSystem.Database.DAL;
using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.BLL
{
    public class HitBLL
    {
        public HitDAO DAO { get; set; }
        public HitBLL()
        {
            DAO = new HitDAO();
        }
        public bool AddHits(List<DeviceHitRecord> hits)
        {
            if (DAO.AddHits(hits))
            {
                DAO.DistinctHits();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<DeviceHitRecord> GetAllHits()
        {
            return DAO.GetAllHit();
        }
    }
}
