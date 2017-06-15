using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services.Database.DAL
{
    public class EventDAO 
    {
        private const string LOG = "EventDAO";

        private static Event InitEvent(DataRow item)
        {
            var newEvent = new Event();
            newEvent.ID = Convert.ToInt32(item["ID"]);
            newEvent.Name = item["Name"].ToString();
            newEvent.Card = item["Card"].ToString();
            return newEvent;
        }


        public List<Event> GetAllEvent()
        {
            var events = new List<Event>();
            string sql = "select *  from T_Event ";
            var ds = new DataSet();
            if (!SQLiteHelper.Instance.ExecuteDataSet(sql, null, out ds))
            {
                return events;
            };

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                var newEvent = InitEvent(item);
                events.Add(newEvent);
            }
            return events;
        }

      
        public bool AddEvent(Event e, out int id)
        {
            string sql = "insert into T_Event(ID,Name,Card) values(null,@NAME,@CARD);select last_insert_rowid();";
            object result = SQLiteHelper.Instance.ExecuteScalar(sql, new object[] { e.Name, e.Card });
            if (result != null)
            {
                id = Convert.ToInt32(result);
                return true;
            }
            else
            {
                id = -1;
                return false;
            }
        }
        public Event QueryEvent(int id)
        {
            var sql = String.Format("select * from T_Event where ID={0}", id);
            var ds = new DataSet();
            if (!SQLiteHelper.Instance.ExecuteDataSet(sql, null, out ds))
            {
                return null;
            }
            if (ds.Tables[0].Rows.Count != 1)
            {
                return null;
            }
            DataRow item = ds.Tables[0].Rows[0];
            return InitEvent(item);
        }

        public bool UpdateEvent(Event e)
        {
            string sql = "update T_Event set Name=@NAME,Card=@CARD where Id=@ID";
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, new object[] { e.Name, e.Card, e.ID }) == 1;
        }

        public bool DelEvent(Event e)
        {
            string sql = "delete from T_Event where ID=@ID";
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, e.ID) == 1;
        }

        public bool ExistsName(string name)
        {
            string sql = String.Format("select Count(*) from T_Event where Name=@Name ");
            var count = Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, name));
            return count >= 1;
        }
        public bool ExistsCard(string card)
        {
            string sql = String.Format("select Count(*) from T_Event where Card=@card");
            var count = Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, card));
            return count >= 1;
        }


        public int GetRowCount()
        {
            var sql = "select count(*) from T_Event";
            return Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, null));
        }
        public void Init()
        {
            var sql = "delete from T_Event;update sqlite_sequence set seq=0 where name='T_Event';";
            SQLiteHelper.Instance.ExecuteNonQuery(sql, null);
        }
    }
}
