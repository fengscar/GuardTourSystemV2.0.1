using GuardTourSystem.Model.Model;
using GuardTourSystem.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model.DAL
{
    public class WorkerDAO
    {
        private const string LOG = "DAO";


        public List<Worker> GetAllWorker()
        {
            string sql = "select * from T_Worker ";

            List<Worker> workerList = new List<Worker>();
            var ds = new DataSet();
            if (!SQLiteHelper.Instance.ExecuteDataSet(sql, null, out ds))
            {
                return workerList;
            };

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                workerList.Add(InitWorker(item));
            }
            return workerList;
        }

        public Worker QueryWorker(int id)
        {
            var sql = String.Format("select * from T_Worker where ID={0}", id);
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
            return InitWorker(item); ;
        }

        public List<Worker> QueryWorker(string str)
        {
            return this.QueryWorker(str, str);
        }

        // 使用Like来查询
        public List<Worker> QueryWorker(string name, string card)
        {

            StringBuilder sql = new StringBuilder("select * from T_Worker ");
            List<object> objs = new List<object>();

            if (!string.IsNullOrEmpty(name))
            {
                sql.Append(" where NAME Like \"%@Name%\"");
                objs.Add(name);
            }
            if (!string.IsNullOrEmpty(card))
            {
                sql.Append(" or Card Like \"%@Card%\"");
                objs.Add(card);
            }
            var ds = new DataSet();
            List<Worker> workerList = new List<Worker>();
            if (!SQLiteHelper.Instance.ExecuteDataSet(sql.ToString(), objs.ToArray(), out ds))
            {
                return workerList;
            };

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                workerList.Add(InitWorker(item));
            }
            return workerList;
        }

        private Worker InitWorker(DataRow dr)
        {
            Worker worker = new Worker();
            worker.ID = Convert.ToInt32(dr["ID"]);
            worker.Name = dr["Name"].ToString();
            worker.Card = dr["Card"].ToString();

            return worker;
        }

        // 返回新增的Worker的ID
        public bool AddWorker(Worker worker, out int id)
        {
            string sql = "insert into T_Worker(ID,Name,Card) values(null,@NAME,@CARD);select last_insert_rowid();";
            object result = SQLiteHelper.Instance.ExecuteScalar(sql, new object[] { worker.Name, worker.Card });
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
        /// <summary>
        /// ID 不变,其他属性update
        /// </summary>
        /// <param name="worker"></param>
        /// <returns></returns>
        public bool UpdateWorker(Worker worker)
        {
            string sql = "update T_Worker set Name=@NAME,Card=@CARD where Id=@ID";
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, new object[] { worker.Name, worker.Card, worker.ID }) == 1;
        }

        public bool DelWorker(Worker worker)
        {
            string sql = "delete from T_Worker where ID=@ID";
            //string sql = "update T_Worker set Deleted='1' where ID=@ID";
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, worker.ID) == 1;
        }

        public bool ExistsName(string Name)
        {
            string sql = String.Format("select Count(*) from T_Worker where Name=@Name ");
            var count = Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, Name));
            return count >= 1;
        }
        public bool ExistsCard(string card)
        {
            string sql = String.Format("select Count(*) from T_Worker where Card=@Card ");
            var count = Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, card));
            return count >= 1;
        }

        public int GetRowCount()
        {
            var sql = "select count(*) from T_Worker";
            return Convert.ToInt32(SQLiteHelper.Instance.ExecuteScalar(sql, null));
        }
        public void Init()
        {
            var sql = "delete from T_Worker;update sqlite_sequence set seq=0 where name='T_Worker';";
            SQLiteHelper.Instance.ExecuteNonQuery(sql, null);
        }
    }
}
