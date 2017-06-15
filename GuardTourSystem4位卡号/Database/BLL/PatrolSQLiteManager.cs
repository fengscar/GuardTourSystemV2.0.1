using GuardTourSystem.Database;
using GuardTourSystem.Database.BLL;
using GuardTourSystem.Database.DAL;
using GuardTourSystem.Database.Model;
using GuardTourSystem.Model.DAL;
using GuardTourSystem.Services.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services.Database.BLL
{
    public class PatrolSQLiteManager
    {
        //系统首次开启初始化
        public static void Init()
        {
            // 判断是否需要创建表
            BaseDAO bd = new BaseDAO();

            if (bd.GetTableCount() <= 1)
            {
                bd.CreateTable();
                bd.CreateTrigger();
            }
        }
        /// <summary>
        /// 清空所有数据
        /// 1. 删除除了用户表和角色表外的所有表
        /// 2. 重新创建表
        /// </summary>
        public static void Reset()
        {
            BaseDAO bd = new BaseDAO();

            bd.DeleteTable();
            bd.CreateTable();
        }

        /// <summary>
        /// 在 事件,地点,巡检员表中查询 该钮号 是否已存在,如果已存在,返回false,并在errorInfo返回错误信息
        /// </summary>
        /// <param name="Card"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public static bool CheckCardUnique(string Card, ref string errorInfo)
        {
            var eDAO = new EventDAO();
            var pDAO = new PlaceDAO();
            var wDAO = new WorkerDAO();
            if (eDAO.ExistsCard(Card))
            {
                errorInfo = "该钮号已作为事件卡使用";
                return false;
            }
            if (pDAO.ExistsCard(Card))
            {
                errorInfo = "该钮号已作为地点卡使用";
                return false;
            }
            if (wDAO.ExistsCard(Card))
            {
                errorInfo = "该钮号已作为人员卡使用";
                return false;
            }
            return true;
        }
        //获取当前 数据库信息
        public static DatabaseInfo GetCurrentDatabaseInfo()
        {
            var workerCount = new WorkerDAO().GetRowCount();
            var routeCount = new RouteDAO().GetRowCount();
            var placeCount = new PlaceDAO().GetRowCount();
            var eventCount = new EventDAO().GetRowCount();
            var frequenceCount = new FrequenceDAO().GetRowCount();
            var rawDataCount = new RawDataDAO().GetRowCount();
            var recordCount = new RecordDAO().GetRowCount();

            var result = new DatabaseInfo()
            {
                WorkerCount = workerCount,
                RouteCount = routeCount,
                PlaceCount = placeCount,
                EventCount = eventCount,
                FrequenceCount = frequenceCount,
                RawDataCount = rawDataCount,
                RecordCount = recordCount
            };
            return result;
        }
    }
}
