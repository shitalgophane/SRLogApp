using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace SRLog.Entities.Customer.ViewModels
{
    public class CustomerViewModel
    {

        public CustomerViewModel()
       {
           lstCustAddress = new List<tblCustAddress>();
           lstCustContact = new List<tblCustContact>();
         
       }

        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please Enter Customer Name.")]
        public string CustomerName { get; set; }
        public bool InActive { get; set; }
        public string Notes { get; set; }
        public List<tblCustAddress> lstCustAddress;
        public List<tblCustContact> lstCustContact;

    }
}
