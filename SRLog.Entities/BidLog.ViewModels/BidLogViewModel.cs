﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRLog.Entities.BidLog.ViewModels
{
   public class BidLogViewModel
    {
        public Nullable<System.DateTime> BidDate { get; set; }
        public string BiddingAs { get; set; }
        public string BidTo { get; set; }
        public string ProjectName { get; set; }
        public Nullable<short> LastAddendumRecvd { get; set; }
        public string Estimator { get; set; }
        public Nullable<int> QuoteNumber { get; set; }
        public Nullable<int> UID { get; set; }
        public string Notes { get; set; }
        public string EngineersEstimate { get; set; }
        public string Division { get; set; }
        public string RedSheet { get; set; }
        public string ScopeLetter { get; set; }
        public Nullable<bool> BidTypeIandC { get; set; }
        public Nullable<bool> BidTypeElectrical { get; set; }
        public Nullable<bool> BidTypeNOBID { get; set; }
        public Nullable<bool> MandetoryJobWalk { get; set; }
        public Nullable<System.DateTime> JobWalkDate { get; set; }
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
        public string OwnerName { get; set; }
        public Nullable<System.DateTime> AdvertiseDate { get; set; }
        public string ProjectFolder { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string CityState { get; set; }
        public string PrimeSIEstimate { get; set; }
        public string IAndCEstimate { get; set; }
        public int BidId { get; set; }
    }
}
