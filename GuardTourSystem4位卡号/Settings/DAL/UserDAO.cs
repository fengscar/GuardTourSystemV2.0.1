using GuardTourSystem.Model;
using GuardTourSystem.Settings;
using GuardTourSystem.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services.Database.DAL
{
    public class UserDAO
    {
        private const string LOG = "UserDAO";

        // 获取所有用户信息
        public List<User> QueryAllUser()
        {
            string sql = "select * from T_User";
            object[] para = null;
            //if (str != null)
            //{
            //    sql += "where Name like @SearchText";
            //    para = new object[] { "%"+str+"%" };
            //}
            List<User> userList = new List<User>();
            var ds = new DataSet();
            if (!ConstantSQLiteHelper.Instance.ExecuteDataSet(sql, para, out ds))
            {
                return userList;
            };
            
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                userList.Add(InitUser(item));
            }
            return userList;
        }
        public User GetUser(int roleID)
        {
            string sql = "select * from T_User where RoleID=@RoleID";
            if (ConstantSQLiteHelper.Instance.ExecuteDataSet(sql, new object[] { roleID }, out DataSet ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    return InitUser(row);
                }
            }
            return null;
        }


        public bool AddUser(User user)
        {
            string sql = "insert into T_User(ID,Password,RoleID) values(null,@PASSWORD,@ROLEID);";

            var res = ConstantSQLiteHelper.Instance.ExecuteNonQuery(sql, new object[] {  user.Password, user.UserRole == Role.Manager ? 0 : 1 });

            return res == 1;
        }

        public bool DelUser(User user)
        {
            string sql = "delete from T_User where ID=@userID";
            return ConstantSQLiteHelper.Instance.ExecuteNonQuery(sql, user.ID) == 1;
        }

        public bool UpdateUser(User user)
        {
            string sql = "update T_User set Password=@pwd where ID=@id";
            return ConstantSQLiteHelper.Instance.ExecuteNonQuery(sql, new object[] { user.Password, user.ID }) == 1;
        }


        private static User InitUser(DataRow row)
        {
            User user = new User(Convert.ToInt32(row["RoleID"]))
            {
                ID = Convert.ToInt32(row["ID"]),
                Password = row["Password"].ToString()
            };
            return user;
        }


    }
}
