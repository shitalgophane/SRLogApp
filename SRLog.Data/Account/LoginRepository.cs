using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRLog.Data;
using SRLog.Entities.Account.ViewModels;

namespace SRLog.Data.Account
{
    public class LoginRepository
    {
        SR_Log_DatabaseSQLEntities1 db = new SR_Log_DatabaseSQLEntities1();
        public LoginViewModel GetUserById(string username, string password)
        {
            var user = (from u in db.tblUsers
                        where u.User_Name == username && u.User_Password == password
                        select u).ToList();

            if (user.Count() > 0)
            {
                UserInfoViewModel userinfo = new UserInfoViewModel
                {
                    User_Password = user[0].User_Password,
                    User_Name = user[0].User_Name,
                    SR_Log_ReadOnly = user[0].SR_Log_ReadOnly,
                    Admin_Rights = user[0].Admin_Rights,
                    Bid_Log_ReadOnly = user[0].Bid_Log_ReadOnly,
                    Accounting_Rights = user[0].Accounting_Rights,
                    UserId = user[0].UserId
                };

                var s = new LoginViewModel()
              {
                  LoginName = user[0].User_Name,
                  Password = user[0].User_Password,                  
                  UserInfo = userinfo
              };
                return s;
            }


            return null;


        }

    }
}
