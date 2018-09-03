using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SRLog.Entities.BidLog.ViewModels
{
    public class BidLogViewModel
    {
        public List<tblCustomer> CustomerList;
        public List<tblGroupUser> GroupUsersList;
        public Nullable<System.DateTime> BidDate { get; set; }        
        public string BiddingAs { get; set; }
        public bool BiddingAsIandC { get; set; }
        public bool BiddingAsElectircal { get; set; }
        public bool BiddingAsPrime { get; set; }
        public bool BiddingAsUnKnown { get; set; }
        public bool BiddingAsNotBidding { get; set; }
        public bool BiddingAsNotQualified { get; set; }
        public bool BiddingAsMechanical { get; set; }
        public string Division { get; set; }
        public bool DivisionConcord { get; set; }
        public bool DivisionHanford { get; set; }
        public bool DivisionSacramento { get; set; }      
        public string OwnerName { get; set; }
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Please Enter CityState.")]
        public string CityState { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public Nullable<short> LastAddendumRecvd { get; set; }

            [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public Nullable<int> QuoteNumber { get; set; }
        public string Bid_Rev { get; set; }
        public string Estimator { get; set; }
        public string IAndCEstimate { get; set; }
        public string PrimeSIEstimate { get; set; }
        public string EngineersEstimate { get; set; }
        public string Bid_Duration { get; set; }
        public string Bid_LiqDamage { get; set; }
        public string Bid_Warranty { get; set; }
        public bool Bid_Qualified { get; set; }
        public bool Bid_PrevailingWage { get; set; }
        public string Bid_LicenseReqd { get; set; }
        public string Bid_BondingReq { get; set; }
        public string Notes { get; set; }
        public bool MandetoryJobWalk { get; set; }
        public Nullable<System.DateTime> JobWalkDate { get; set; }
        public string BidTo { get; set; }
        public int BidId { get; set; }
        public Nullable<int> UID { get; set; }
        public bool Bid_UL508Reqd { get; set; }
        public Nullable<System.DateTime> QADeadLineDateTime { get; set; }
        [Required(ErrorMessage = "Please Enter Advertise Date.")]
        public Nullable<System.DateTime> AdvertiseDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ProjectFolder { get; set; }

        //public string RedSheet { get; set; }
        //public string ScopeLetter { get; set; }
        //public Nullable<bool> BidTypeIandC { get; set; }
        //public Nullable<bool> BidTypeElectrical { get; set; }
        //public Nullable<bool> BidTypeNOBID { get; set; }           
      
       
     
   
      
    }
}
