using GuardTourSystem.Model;
using GuardTourSystem.Services.Database.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Services.Database.BLL
{
    class UserBLL : IUserService
    {
        public UserDAO DAO { get; set; }
        public UserBLL()
        {
            DAO = new UserDAO();
        }
        public List<User> GetAllUser()
        {
            return DAO.QueryAllUser();
        }
            
        public User GetUser(int roleID)
        {
            return DAO.GetUser(roleID);
        }

        public bool UpdateUser(User newUser,out string errorInfo)
        {
            errorInfo = "";
            return DAO.UpdateUser(newUser);
        }
        //修改密码
        //1. 验证旧密码
        //2. 修改数据
        public bool UpdatePassword(Role role,string oldPassword ,string newPassword, out string errorInfo)
        {
            errorInfo = "";
            var curUser = this.GetUser((int)role);
            if (curUser == null)
            {
                throw new Exception("未找到指定用户");
            }
            if (!curUser.Password.Equals(oldPassword))
            {
                errorInfo = "原始密码不正确";
                return false;
            }
            //更新密码
            curUser.Password = newPassword;
            return DAO.UpdateUser(curUser);
        }
    }
}
