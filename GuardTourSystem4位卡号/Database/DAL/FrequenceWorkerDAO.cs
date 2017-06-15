using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Database.DAL
{
    public class FrequenceWorkerDAO
    {
        public Worker GetFrequenceWorker(Frequence f)
        {
            var sql = String.Format("select * from T_Worker where ID=(select WorkerID from T_FrequenceWorker where FrequenceID={0})", f.ID);
            var ds = new DataSet();
            if (!SQLiteHelper.Instance.ExecuteDataSet(sql, null, out ds) || ds.Tables[0].Rows.Count<=0)
            {
                return null;
            }
            
            DataRow item = ds.Tables[0].Rows[0];
            var result = new Worker();
            result.ID = Convert.ToInt32(item["ID"]);
            result.Name = item["Name"].ToString();
            result.Card = item["Card"].ToString();

            return result;
        }
        //Replace是删掉旧记录,重新添加新纪录, 所以 ID 会发生改变.
        // 当前没有其他表 依赖于该表的ID,所以可以用Replace,否则会引起异常
        public bool ReplaceFrequenceWorker(Frequence f) 
        {
            if (f.Worker == null)
            {
                throw new Exception("该班次未指定巡检员,应调用DelFrequenceWorker");
            }
            var sql = String.Format("Replace into T_FrequenceWorker(FrequenceID,WorkerID) values({0},{1})",f.ID,f.Worker.ID);
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, null)==1;
        }

        public bool DelFrequenceWorker(Frequence f)
        {
            var sql = String.Format("Delete from T_FrequenceWorker where FrequenceID={0}", f.ID);
            return SQLiteHelper.Instance.ExecuteNonQuery(sql, null) == 1;
        }
        public void Init()
        {
            var sql = "delete from T_FrequenceWorker;update sqlite_sequence set seq=0 where name='T_FrequenceWorker';";
            SQLiteHelper.Instance.ExecuteNonQuery(sql, null);
        }

    }
}
