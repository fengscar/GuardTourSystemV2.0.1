using GuardTourSystem.Model;
using GuardTourSystem.Services;
using GuardTourSystem.Services.Database.BLL;
using GuardTourSystem.Services.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.BLL
{
    public class EventBLL : IEventService
    {
        public EventDAO DAO { get; set; }
        public EventBLL()
        {
            DAO = new EventDAO();
        }

        public List<Event> GetAllEvent()
        {
            return DAO.GetAllEvent();
        }

        public bool CanAdd(Event e, out string errorInfo)
        {
            errorInfo = "";
            if (!CheckEventProp(e, ref errorInfo))
            {
                return false;
            }
            if (!PatrolSQLiteManager.CheckCardUnique(e.Card, ref errorInfo))
            {
                return false;
            }
            if (DAO.ExistsName(e.Name))
            {
                errorInfo = "该事件名称已存在";
                return false;
            }
            return true;
        }

        public bool AddEvent(Event e, out int id, out string errorInfo)
        {
            id = -1;
            if (CanAdd(e, out errorInfo))
            {
                return DAO.AddEvent(e, out id);
            }
            return false;
        }

        public bool UpdateEvent(Event e, out string errorInfo)
        {
            errorInfo = "";
            if (!CheckEventProp(e, ref errorInfo))
            {
                return false;
            }
            var old = DAO.QueryEvent(e.ID);
            if (!old.Card.Equals(e.Card)) //如果钮号变更,判断新钮号是否已经存在
            {
                if (!PatrolSQLiteManager.CheckCardUnique(e.Card, ref errorInfo))
                {
                    return false;
                }
            }
            if (!old.Name.Equals(e.Name)) //如果姓名变更,判断新名称是否已经存在
            {
                if (DAO.ExistsName(e.Name))
                {
                    errorInfo = "该事件名称已存在";
                    return false;
                }
            }
            if (old.Name.Equals(e.Name) && old.Card.Equals(e.Card)) // 不需要更新
            {
                return true;
            }
            return DAO.UpdateEvent(e);
        }

        public bool DelEvent(Event e)
        {
            return DAO.DelEvent(e);
        }

        /// <summary>
        /// 检查 event 属性是否有空值
        /// 如果有空值,返回false,并在errorInfo附带错误信息
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        private bool CheckEventProp(Event evt, ref string errorInfo)
        {
            if (evt.Name != null)
            {
                evt.Name = evt.Name.Trim();
            }
            if (String.IsNullOrEmpty(evt.Name))
            {
                errorInfo = "抱歉,事件名称不能为空";
                return false;
            }
            if (String.IsNullOrEmpty(evt.Card))
            {
                errorInfo = "抱歉,钮号不能为空";
                return false;
            }
            if (evt.Card.Length != 4)
            {
                errorInfo = "请输入4位钮号";
                return false;
            }
            return true;
        }


        public void Init()
        {
            DAO.Init();
        }



    }
}
