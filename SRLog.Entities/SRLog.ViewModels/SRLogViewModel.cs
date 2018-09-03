using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SRLog.Entities.SRLog.ViewModels
{
    public class SRLogViewModel
    {


        public bool Quote { get; set; }
        public bool Job { get; set; }//use other
        public List<tblCustomer> CustomerList;
        public bool NewCustomerYes { get; set; }
        public bool NewCustomerNo { get; set; }
        public bool NewCustomer { get; set; }
        public List<tblCustAddress> JobsiteAddressList;
        public List<tblGroupUser> GroupUsersList;
        public List<tblGroupUser> GroupUsersListForEstimator;
        public List<tblCustContact> CustomerContList;
        public string ProjectType { get; set; }
        public bool ProjectTypeLumpType { get; set; }
        public bool ProjectTypeTAndM { get; set; }
        public bool ProjectTypeTAndMNTE { get; set; }
        public string Division { get; set; }
        public bool DivisionConcord { get; set; }
        public bool DivisionHanford { get; set; }
        public bool DivisionSacramento { get; set; }
        public Nullable<bool> FileFolder { get; set; }
        public Nullable<bool> ChemFeed { get; set; }
        public bool InactiveJob { get; set; }
        public Nullable<bool> PW { get; set; }


        public Nullable<bool> NotQuoted { get; set; }
        public Nullable<bool> Closed { get; set; }
        public Nullable<decimal> SRNumber { get; set; }

        //[Required(ErrorMessage = "Please Select Customer Name.")]
        public string Customer { get; set; }

        [Required(ErrorMessage = "Please Enter Project Description or Job Name.")]
        public string ProjectDescription { get; set; }
        //[Required(ErrorMessage = "Please Enter Contact Name.")]
        public string CustomerContact { get; set; }
        [Required(ErrorMessage = "Please Enter Contact Phone.")]
        public string ContactPhone { get; set; }
        [Required(ErrorMessage = "Please Enter Contact Email.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string ContactEmail { get; set; }


        [Required(ErrorMessage = "Please Enter Estimator Name.")]
        public string Estimator { get; set; }

        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.DateTime> QuoteDue { get; set; }
        public Nullable<System.DateTime> JobWalkDate { get; set; }
        public Nullable<bool> MandatoryJobWalk { get; set; }
        public string WhoJobWalk { get; set; }
        public Nullable<bool> BidAsPrimeOrSub { get; set; }
        public string Bonding { get; set; }
        public Nullable<bool> BondingMailSent { get; set; }
        public Nullable<bool> PrevailingMailSent { get; set; }
        public string Notes { get; set; }

        [Required(ErrorMessage = "Please Select Project Manager.")]
        public string ProjectManager { get; set; }
        public string CreatedBy { get; set; }
        public string EditedBy { get; set; }

        
        public string JobOrQuote { get; set; }
        public bool PrevailingWageYes { get; set; }
        public bool PrevailingWageNo { get; set; }
        public bool PrevailingWageTBD { get; set; }
        public bool PrevailingWage { get; set; }
           //[Required(ErrorMessage = "Please Enter Job Site Address.")]
        public string JobsiteAddress { get; set; }
        public Nullable<bool> Billing { get; set; }


        public Nullable<System.DateTime> QuoteDate { get; set; }
        public Nullable<System.DateTime> QuoteTime { get; set; }
        public Nullable<bool> QuoteTypeI_C { get; set; }
        public Nullable<bool> QuoteTypeElectrical { get; set; }
        public Nullable<bool> QuoteTypeNoBid { get; set; }
        public Nullable<bool> FollowUp { get; set; }
        public string QuoteType { get; set; }

        //[Required(ErrorMessage = "Please Select Owner Name.")]
        public string Owner { get; set; }
        public Nullable<System.DateTime> AdvertiseDate { get; set; }
        public bool NotifyPM { get; set; }

        [Required(ErrorMessage = "Please select Server Job Folder.")]
        public string ServerJobFolder { get; set; }
        public string SiteForeman { get; set; }
        public int Id { get; set; }

     
    }
}
