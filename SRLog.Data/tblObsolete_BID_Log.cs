//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SRLog.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblObsolete_BID_Log
    {
        public bool Mandatory_Job_Walk { get; set; }
        public System.DateTime BidDate { get; set; }
        public string BiddingAs { get; set; }
        public string BidTo { get; set; }
        public string ProjectName { get; set; }
        public Nullable<short> LastAddendumRecvd { get; set; }
        public string Estimator { get; set; }
        public Nullable<int> QuoteNumber { get; set; }
        public string Division { get; set; }
        public Nullable<int> UID { get; set; }
        public string Notes { get; set; }
        public string EngineersEstimate { get; set; }
        public string RedSheet { get; set; }
        public string ScopeLetter { get; set; }
        public Nullable<System.DateTime> Job_Walk_Date { get; set; }
        public string Bid_Rev { get; set; }
        public string Bid_Duration { get; set; }
        public string Bid_LiqDamage { get; set; }
        public string Bid_Warranty { get; set; }
        public string Bid_BondingReq { get; set; }
        public string Bid_LicenseReqd { get; set; }
        public Nullable<bool> Bid_Qualified { get; set; }
        public Nullable<bool> Bid_PrevailingWage { get; set; }
        public Nullable<bool> Bid_UL508Reqd { get; set; }
        public Nullable<System.DateTime> QADeadLineDateTime { get; set; }
        public string ProjectFolder { get; set; }
        public string OwnerName { get; set; }
        public Nullable<System.DateTime> AdvertiseDate { get; set; }
        public string CityState { get; set; }
        public string PrimeSIEstimate { get; set; }
        public string IAndCEstimate { get; set; }
    }
}
