using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRLog.Entities;
using SRLog.Entities.Account.ViewModels;
using System.Data;

namespace SRLog.Data.Account
{
    public class LoginRepository
    {
        SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
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
                    DatabaseUpdate_Rights = user[0].DatabaseUpdate_Rights,
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


        public DataTable GetSMTPDetails()
        {
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            var mail = (from m in db.SMTPDetails
                        select m).ToList();

            DataTable table = new DataTable();
            table.Columns.Add("SMTPAddress", typeof(string));
            table.Columns.Add("SMTPPort", typeof(string));
            table.Columns.Add("SMTPUserName", typeof(string));
            table.Columns.Add("SMTPPassword", typeof(string));
            table.Columns.Add("FromAddress", typeof(string));


            foreach (var m in mail)
            {
                table.Rows.Add(m.SMTPAddress, m.SMTPPort, m.SMTPUserName, m.SMTPPassword, m.FromAddress);
            }
            return table;
        }


    }
}
