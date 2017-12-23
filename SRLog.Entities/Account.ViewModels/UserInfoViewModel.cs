using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRLog.Entities.Account.ViewModels
{
    public class UserInfoViewModel
    {
        public string User_Password { get; set; }
        public bool SR_Log_ReadOnly { get; set; }
        public string User_Name { get; set; }
        public bool Admin_Rights { get; set; }
        public bool Bid_Log_ReadOnly { get; set; }
        public Nullable<bool> Accounting_Rights { get; set; }
        public int UserId { get; set; }
    }
}
