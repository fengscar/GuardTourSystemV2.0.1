using GuardTourSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model
{
    //系统用户
    public class User
    {
        public const string DEF_PWD_ADMIN = "123456";
        public const string DEF_PWD_OPERATOR = "888888";

        public int ID { get; set; }
        //public string Name { get; set; }
        public string Password { get; set; }
        public Role UserRole { get; set; }


        public User(int roleID)
        {
            this.UserRole = roleID == 0 ? Role.Manager : Role.Operator;
        }
        public User(string password, Role role)
        {
            //this.Name = username;
            this.Password = password;
            this.UserRole = role;
        }
        public User(string password, int roleID)
        {
            //this.Name = username;
            this.Password = password;
            this.UserRole = roleID == 0 ? Role.Manager : Role.Operator;
        }

        public string UserName
        {
            get
            {
                return LanLoader.Load(this.UserRole == Role.Manager ? LanKey.RoleManager : LanKey.RoleOperator);
            }
        }
    }
    public enum Role
    {
        Manager = 0,
        Operator = 1
    };

}
