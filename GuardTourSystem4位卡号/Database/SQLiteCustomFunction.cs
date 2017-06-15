using GuardTourSystem.Model;
using GuardTourSystem.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services.Database
{
    class SQLiteCustomFunction
    {
        /// <summary>
        /// 获取 线路当前的 OrderID
        /// </summary>
        //[SQLiteFunction(Name = "GetMaxRouteOrder", Arguments = 1, FuncType = FunctionType.Scalar)]
        //public class GetMaxRouteOrder : SQLiteFunction
        //{
        //    public override object Invoke(object[] args)
        //    {
        //        var routeID = Convert.ToInt32(args[0]);
        //        var sql =String.Format(,routeID);
        //        var result=SQLiteHelper.Instance.ExecuteScalar(sql,null);
        //        if (result == null || result is DBNull)
        //        {
        //            return 0;
        //        }
        //        else
        //        {
        //            return Convert.ToInt32(result);
        //        }
        //    } 
        //}
    }
}
