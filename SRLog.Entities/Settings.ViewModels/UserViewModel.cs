using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SRLog.Entities.Settings.ViewModels
{
   public class UserViewModel
    {
       [Required(ErrorMessage = "Please Enter User Password .")]
        public string User_Password { get; set; }
        public bool SR_Log_ReadOnly { get; set; }
        public bool SRlogReadonly { get; set; }
        public bool SRLogChange { get; set; }
          [Required(ErrorMessage = "Please Enter User Name .")]
        public string User_Name { get; set; }
        public bool Admin_Rights { get; set; }
        public bool Bid_Log_ReadOnly { get; set; }
        public bool BidlogReadonly { get; set; }
        public bool BidLogChange { get; set; }
        public bool Accounting_Rights { get; set; }
        public bool DatabaseUpdate_Rights { get; set; }
        public int? UserId { get; set; }
        public List<tblUser> UserList;
    }
}
