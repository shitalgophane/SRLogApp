using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace SRLog.Entities.Settings.ViewModels
{
    public class MailInfoViewModel
    {
        public List<EMail_Info> PersonList;
        public List<SelectListItem> Person { get; set; }
        public List<Bidlog_Result_Mail> bidlogResultMailList;
        public List<Customer_Info_Mail> CustomerInfoMailList;
        // public List<Bonding_Mail_CC> BondingMailCC;

        public List<Bonding_Mail_TO> bondingMailToList;
       
        public string Name;
     
        public string Email;
        public string BondingMailTO;
        public string CustomerInfoMail;
        public string BidlogResultMail;

    }
}
