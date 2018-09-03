using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SRLog.Entities.Customer.ViewModels
{
   public class CustomerAddressViewModel
    {
      
       public int Id { get; set; }
       public int CustomerId { get; set; }
       public string Address1 { get; set; }
       public string Address2 { get; set; }
       public string City { get; set; }
       public string State { get; set; }
       public string ZipCode { get; set; }
       public string Country { get; set; }
       public bool IsPrimaryAddress { get; set; }
       public DateTime DateAdded { get; set; }
       public string SiteName { get; set; }
       public string ProjectManager { get; set; }
       public List<tblGroupUser> ProjectManagerList;
    }
}
