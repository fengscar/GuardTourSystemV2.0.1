using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Data.Common;
using GuardTourSystem.Utils;
using System.Text.RegularExpressions;
using System.IO;
using GuardTourSystem.ViewModel;

namespace GuardTourSystem.Model
{
    /// <summary>
    ///  引用：System.Data.SQLite.dll
    /// 数据库管理类,包含以下功能
    /// 1. 创建/打开数据库文件: 
    ///     如果在默认位置有DB文件,打开数据库; 如果没有,创建Patrol_Data.db文件并打开.
    /// 
    /// </summary>
    public class SQLiteHelper
    {
        private const string LOG = "SQLiteHelper";
        protected SQLiteHelper() { }

        public virtual string DATABASE_FILE_NAME
        {
            get
            {
                return "PatrolData.db";
            }
        }

        private static SQLiteHelper instance { get; set; }
        public static SQLiteHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SQLiteHelper();
                }
                return instance;
            }
            private set { }
        }

        public string DbFilePath
        {
            get
            {
                System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
                string filePath = asm.Location.Remove(asm.Location.LastIndexOf("\\")) + "\\DATA\\";
                //如果不存在,创建DATA文件夹
                if (Directory.Exists(filePath) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(filePath);
                }
                return filePath + DATABASE_FILE_NAME;
            }
            private set { }
        }

        private string dbSource;
        //使用virtual支持子类重写 ,如果不用virtual修饰, 子类调用该属性时,依然将调用父类的. 
        //具体在于: C# override 和 new 的区别
        public string DbSource
        {
            get
            {
                if (dbSource == null)
                {
                    dbSource = "Data Source=" + DbFilePath;
                }
                return dbSource;
            }
            protected set { dbSource = value; }
        }


        private SQLiteConnection conn = null;
        public SQLiteConnection Conn
        {
            get
            {
                if (conn == null)
                {
                    conn = new SQLiteConnection(DbSource);
                }
                if (conn.State == ConnectionState.Closed)
                {
                    conn.SetPassword("041225"); //使用密码
                    conn.Open();
                }
                return conn;
            }
            set { conn = value; }
        }
        //1.打开指定位置的数据库
        //2.执行传入的Func
        //3.将Func的结果返回(如果操作出现异常,返回NULL)
        //注意: 将断开Conn的连接, 并在操作完成后将自动重新连接到 默认的计数数据库
        public object OperateOtherDatabase(string dataBaseFilePath, Func<object> func)
        {
            Conn = null;
            DbSource = "Data Source=" + dataBaseFilePath; //指向新的数据库
            try
            {
                return func();
            }
            catch (Exception e)
            {
                DBug.w("执行传入的Func时出错:" + e);
                return null;
            }
            finally
            {
                Conn = null;
                DbSource = null;//操作完成,将路径改为系统的数据库文件( 设为null将自动修改)
            }
        }

        public void MotifyPassword(string password)
        {
            Conn.ChangePassword(password);
        }

        /// <summary>
        /// Shortcut method to execute dataset from SQL Statement and object[] arrray of parameter values
        /// </summary>
        /// <param name="commandText">SQL Statement with embedded "@param" style parameter names</param>
        /// <param name="paramList">object[] array of parameter values</param>
        /// <returns></returns>
        public bool ExecuteDataSet(string commandText, object[] paramList, out DataSet dataSet)
        {

            var cmd = GetCommand(commandText, paramList);
            if (Transcation != null)
            {
                cmd.Transaction = Transcation;
            }
            dataSet = new DataSet();
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            try
            {
                da.Fill(dataSet);
                return true;
            }
            catch (Exception e)
            {
                if (Transcation != null)
                {
                    TransactionSuccess = false;
                }
                DBug.w("执行" + commandText + "时出错", e);
                return false;
            }
            finally
            {
                da.Dispose();
                cmd.Dispose();
                //if (Transcation == null) //如果事务已开启,不关闭连接,Commit中还将进行调用
                //{
                //    Conn.Close();
                //}
            }
        }

        /// <summary>
        /// Shortcut to ExecuteScalar with Sql Statement embedded params and object[] param values
        /// </summary>
        /// <param name="commandText">SQL statment with embedded "@param" style parameters</param>
        /// <param name="paramList">object[] array of param values</param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, params  object[] paramList)
        {
            SQLiteCommand cmd = GetCommand(commandText, paramList);
            if (Transcation != null)
            {
                cmd.Transaction = Transcation;
            }
            try
            {
                object result = cmd.ExecuteScalar();
                return result;
            }
            catch (Exception e)
            {
                if (Transcation != null)
                {
                    TransactionSuccess = false;
                }
                DBug.w("执行" + commandText + "时出错", e);
                return null;
            }
            finally
            {
                cmd.Dispose();
                //if (Transcation == null) //如果事务已开启,不关闭连接,Commit中还将进行调用
                //{
                //    Conn.Close();
                //}
            }
        }

        /// <summary>
        /// Shortcut to ExecuteNonQuery with SqlStatement and object[] param values
        /// </summary>
        /// <param name="commandText">Sql Statement with embedded "@param" style parameters</param>
        /// <param name="paramList">object[] array of parameter values</param>
        /// <returns> -1 表示出错</returns>
        public int ExecuteNonQuery(string commandText, params object[] paramList)
        {
            SQLiteCommand cmd = GetCommand(commandText, paramList);
            try
            {
                int result = cmd.ExecuteNonQuery();
                return result;
            }
            catch (Exception e)
            {
                if (Transcation != null)
                {
                    TransactionSuccess = false;
                }
                DBug.w("执行" + commandText + "时出错", e);
                return -1;
            }
            finally
            {
                cmd.Dispose();
                if (Transcation == null) //如果事务已开启,不关闭连接,Commit中还将进行调用
                {
                    //Conn.Close();
                }
            }
        }

        //private static void OpenDatabase()
        //{
        //    Conn.Open();
        //    var sql = "PRAGMA foreign_keys = ON;";
        //    SQLiteCommand cmd = GetCommand(sql, null);
        //    cmd.Connection = Conn;
        //    cmd.ExecuteNonQuery();
        //}
        public void CloseDatabase()
        {
            if (Instance.Conn != null && Instance.Conn.State == ConnectionState.Open)
            {
                Instance.conn.ChangePassword("041225"); //打开后重新,设置密码
                Instance.Conn.Close();
            }
        }

        /// <summary>
        /// 关于本类的事务使用说明
        /// 1. 如果不需要事务中每条SQL的返回值,请直接调用 ExeceteNonQueryWithTransaction(List<SQLiteCommand> commands)
        /// 2. 如果需要事务中每条SQL的返回值 , 请参考以下样例:该样例 可以得到 每个插入语句所返回的ID
        ///    SQLiteHelper.Instance.BeginTransaction();
        //     var sql = "insert into T_DeviceData values(null,@TRead,@Device,@Time,@Card);select last_insert_rowid()";
        //     foreach (var record in deviceRecord)
        //     {
        //         var cmd = SQLiteHelper.GetCommand(sql, new object[] { record.ReadTime, record.Device, record.Time, record.Card });
        //         var id = Convert.ToInt32(SQLiteHelper.ExcuteTransactionCommmand(cmd));
        //         DBug.w("返回值ID为" + id);
        //     }
        //     return SQLiteHelper.CommitTransaction();
        /// </summary>
        public SQLiteTransaction Transcation { get; set; }
        public bool TransactionSuccess { get; set; } //事务操作结果,如果中途出现错误将设置为false
        // 开启 无需返回值 的事务 ,要注意不能在BeginTransaction-CommitTransaction之间调用该函数
        public bool ExeceteNonQueryWithTransaction(List<SQLiteCommand> commands)
        {
            if (Transcation != null)
            {
                throw new Exception("已经开启了自定义事务");
            }
            Transcation = Conn.BeginTransaction();
            SQLiteCommand curCommand = null;
            try
            {
                foreach (var item in commands)
                {
                    curCommand = item;
                    item.Connection = Conn;
                    item.ExecuteNonQuery();
                }
                Transcation.Commit();
            }
            catch (Exception e)
            {
                DBug.w("执行" + curCommand.CommandText + "时出错", e);
                Transcation.Rollback();
                return false;
            }
            finally
            {
                Transcation.Dispose();
                Transcation = null;
            }
            return true;
        }
        //开始事务,设置TranscationSuccess为true,如果在调用committransaction之前有任何操作出错,都将导致RollBack
        // 该类函数API还是有问题,如果 事务之中 嵌套了  另一个BeginTransaction 和CommitTransaction将出现问题,暂时抛出异常吧
        public void BeginTransaction()
        {
            if (Transcation != null)
            {
                throw new Exception("已经开启了自定义事务");
            }
            TransactionSuccess = true;
            Transcation = Conn.BeginTransaction();
        }
        //提交事务,返回事务操作结果
        public bool CommitTransaction()
        {
            if (Transcation == null)
            {
                DBug.w("提交事务失败: Transcation还未初始化");
                return false;
            }
            try
            {
                if (!TransactionSuccess)//事务操作失败,回滚
                {
                    Transcation.Rollback();
                }
                else
                {
                    Transcation.Commit();
                }
            }
            catch (Exception e)
            {
                DBug.w(e);
                return false;
            }
            finally
            {
                Transcation.Dispose();
                Transcation = null;
            }
            return true;
        }

        ///注意:如果出错,可能是正则表达式解析出错.
        ///该方法以下情况下有错误. 
        ///1. 当 sql中包含类似 where ( xxx>@param1 and xxx<@param2) 时, 会将@param2后的 ')' 也替换掉. 如果正确的写法应该是  xxx<@param2 ) ,在括号前加一个空格
        ///
        // 将 sql 转换为 SQLiteCommand ,注意:SQLiteCommand的Conn也将连接
        public SQLiteCommand GetCommand(string commandText, params  object[] paramList)
        {
            SQLiteCommand cmd = new SQLiteCommand(commandText, Conn);//将Cmd连接到Conn
            if (paramList == null || paramList.Length == 0)
                return cmd;

            SQLiteParameterCollection coll = cmd.Parameters;
            string parmString = commandText.Substring(commandText.IndexOf("@"));
            // 删除 复合语句( Scalar) ==> ;后面的语句
            var indexOfNext = parmString.IndexOf(';');
            if (indexOfNext != -1)
            {
                parmString = parmString.Substring(0, indexOfNext + 1);
            }

            // pre-current the string so always at least 1 space after a comma.
            parmString = parmString.Replace(",", " ,");
            // get the named parameters into a match collection
            string pattern = @"(@)\S*(.*?)\b";
            Regex ex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection mc = ex.Matches(parmString);
            string[] paramNames = new string[mc.Count];
            int i = 0;
            foreach (Match m in mc)
            {
                paramNames[i] = m.Value;
                i++;
            }

            // now let's type the parameters
            int j = 0;
            Type t = null;
            foreach (object o in paramList)
            {
                SQLiteParameter parm = new SQLiteParameter();

                if (o == null)
                {
                    parm.ParameterName = paramNames[j];
                    parm.Value = null;
                    coll.Add(parm);
                    j++;
                    continue;
                }

                t = o.GetType();
                switch (t.ToString())
                {
                    case ("DBNull"):
                    case ("Char"):
                    case ("SByte"):
                    case ("UInt16"):
                    case ("UInt32"):
                    case ("UInt64"):
                        throw new SystemException("Invalid data type");


                    case ("System.String"):
                        parm.DbType = DbType.String;
                        parm.ParameterName = paramNames[j];
                        parm.Value = (string)paramList[j];
                        coll.Add(parm);
                        break;

                    case ("System.Byte[]"):
                        parm.DbType = DbType.Binary;
                        parm.ParameterName = paramNames[j];
                        parm.Value = (byte[])paramList[j];
                        coll.Add(parm);
                        break;

                    case ("System.Int32"):
                        parm.DbType = DbType.Int32;
                        parm.ParameterName = paramNames[j];
                        parm.Value = (int)paramList[j];
                        coll.Add(parm);
                        break;

                    case ("System.Boolean"):
                        parm.DbType = DbType.Boolean;
                        parm.ParameterName = paramNames[j];
                        parm.Value = (bool)paramList[j];
                        coll.Add(parm);
                        break;

                    case ("System.DateTime"):
                        parm.DbType = DbType.DateTime;
                        parm.ParameterName = paramNames[j];
                        parm.Value = Convert.ToDateTime(paramList[j]);
                        coll.Add(parm);
                        break;

                    case ("System.Double"):
                        parm.DbType = DbType.Double;
                        parm.ParameterName = paramNames[j];
                        parm.Value = Convert.ToDouble(paramList[j]);
                        coll.Add(parm);
                        break;

                    case ("System.Decimal"):
                        parm.DbType = DbType.Decimal;
                        parm.ParameterName = paramNames[j];
                        parm.Value = Convert.ToDecimal(paramList[j]);
                        break;

                    case ("System.Guid"):
                        parm.DbType = DbType.Guid;
                        parm.ParameterName = paramNames[j];
                        parm.Value = (System.Guid)(paramList[j]);
                        break;

                    case ("System.Object"):

                        parm.DbType = DbType.Object;
                        parm.ParameterName = paramNames[j];
                        parm.Value = paramList[j];
                        coll.Add(parm);
                        break;

                    default:
                        throw new SystemException("Value is of unknown data type");
                } // end switch

                j++;
            }
            return cmd;
        }
    }
}
