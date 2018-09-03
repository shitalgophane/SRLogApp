using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace SRLog.Entities.QuoteLog.ViewModels
{
   public class QuoteLogViewModel
    {

        public List<tblCustomer> CustomerList;
        public List<tblGroupUser> GroupUsersList;
        public List<EMail_Info> emailids;

        public Nullable<System.DateTime> BidDate { get; set; }


        public string BiddingAs { get; set; }
        public bool BiddingAsIandC { get; set; }
        public bool BiddingAsElectircal { get; set; }
        public bool BiddingAsPrime { get; set; }
        public bool BiddingAsUnKnown { get; set; }
        public bool BiddingAsNotBidding { get; set; }
        public bool BiddingAsNotQualified { get; set; }
        public bool BiddingAsMechanical { get; set; }
        public string BidTo { get; set; }
        public string ProjectName { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public Nullable<short> LastAddendumRecvd { get; set; }
        public string Estimator { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public Nullable<int> QuoteNumber { get; set; }
        public Nullable<int> UID { get; set; }
        public string Notes { get; set; }
        public string EngineersEstimate { get; set; }
        public string Division { get; set; }
        public bool DivisionConcord { get; set; }
        public bool DivisionHanford { get; set; }
        public bool DivisionSacramento { get; set; }
        public bool MandetoryJobWalk { get; set; }
        public Nullable<System.DateTime> JobWalkDate { get; set; }
        public Nullable<System.DateTime> QADeadLineDateTime { get; set; }
        public string QuoteStatus { get; set; }
        public Nullable<System.DateTime> dtpLastDateFollowup { get; set; }
        public string LastFollowupBy { get; set; }
        public string FollowupNote { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }




       
     
    }
}
