using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SRLog.Entities.Customer.ViewModels
{
    public class CustomerContactViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Please Enter Full Name.")]
        public string CustomerContact { get; set; }

        [Required(ErrorMessage = "Please Enter Contact Phone.")]
        public string ContactPhone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required(ErrorMessage = "Please Enter Contact Email.")]
        public string ContactEmail { get; set; }
        public bool IsPrimaryContact { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
