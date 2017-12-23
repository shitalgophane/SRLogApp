using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SRLog.Entities.Account.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login Name")]
        public string LoginName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public UserInfoViewModel UserInfo { get; set; }
    }

}
