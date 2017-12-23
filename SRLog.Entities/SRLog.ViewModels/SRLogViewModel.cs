﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRLog.Entities.SRLog.ViewModels
{
    public class SRLogViewModel
    {
        public Nullable<bool> FileFolder { get; set; }
        public Nullable<bool> Quote { get; set; }
        public Nullable<bool> ChemFeed { get; set; }
        public Nullable<bool> InactiveJob { get; set; }
        public Nullable<bool> PW { get; set; }
        public Nullable<bool> NotQuoted { get; set; }
        public Nullable<bool> Closed { get; set; }
        public Nullable<decimal> SRNumber { get; set; }
        public string Customer { get; set; }
        public string ProjectDescription { get; set; }
        public string CustomerContact { get; set; }
        public string ContactPhone { get; set; }
        public string Estimator { get; set; }
        public string Division { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> QuoteDue { get; set; }
        public Nullable<System.DateTime> JobWalkDate { get; set; }
        public Nullable<bool> MandatoryJobWalk { get; set; }
        public string WhoJobWalk { get; set; }
        public Nullable<bool> BidAsPrimeOrSub { get; set; }
        public string Bonding { get; set; }
        public Nullable<bool> BondingMailSent { get; set; }
        public Nullable<bool> PrevailingMailSent { get; set; }
        public string Notes { get; set; }
        public string ProjectManager { get; set; }
        public string CreatedBy { get; set; }
        public string JobOrQuote { get; set; }
        public string ProjectType { get; set; }
        public Nullable<bool> PrevailingWageTBD { get; set; }
        public string JobsiteAddress { get; set; }
        public Nullable<bool> Billing { get; set; }
        public Nullable<bool> NewCustomer { get; set; }
        public string ContactEmail { get; set; }
        public Nullable<System.DateTime> QuoteDate { get; set; }
        public Nullable<System.DateTime> QuoteTime { get; set; }
        public Nullable<bool> QuoteTypeI_C { get; set; }
        public Nullable<bool> QuoteTypeElectrical { get; set; }
        public Nullable<bool> QuoteTypeNoBid { get; set; }
        public Nullable<bool> FollowUp { get; set; }
        public string QuoteType { get; set; }
        public string Owner { get; set; }
        public Nullable<System.DateTime> AdvertiseDate { get; set; }
        public Nullable<bool> NotifyPM { get; set; }
        public string ServerJobFolder { get; set; }
        public string SiteForeman { get; set; }
        public int Id { get; set; }
    }
}
