using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRLog.Models
{
    public class SRLogColumnConfiguration
    {
        public bool IsFileFolderVisible { get; set; }
        public bool IsQuoteVisible { get; set; }
        public bool IsChemFeedVisible { get; set; }
        public bool IsInactiveJobVisible { get; set; }
        public bool IsPWVisible { get; set; }
        public bool IsNotQuotedVisible { get; set; }
        public bool IsClosedVisible { get; set; }
        public bool IsSRNumberVisible { get; set; }
        public bool IsCustomerVisible { get; set; }
        public bool IsProjectDescriptionVisible { get; set; }
        public bool IsCustomerContactVisible { get; set; }
        public bool IsContactPhoneVisible { get; set; }
        public bool IsEstimatorVisible { get; set; }
        public bool IsDivisionVisible { get; set; }
        public bool IsCreationDateVisible { get; set; }
        public bool IsQuoteDueVisible { get; set; }
        public bool IsJobWalkDateVisible { get; set; }
        public bool IsMandatoryJobWalkVisible { get; set; }
        public bool IsWhoJobWalkVisible { get; set; }
        public bool IsBidAsPrimeOrSubVisible { get; set; }
        public bool IsBondingVisible { get; set; }
        public bool IsBondingMailSentVisible { get; set; }
        public bool IsPrevailingMailSentVisible { get; set; }
        public bool IsNotesVisible { get; set; }
        public bool IsProjectManagerVisible { get; set; }
        public bool IsCreatedByVisible { get; set; }
        public bool IsJobOrQuoteVisible { get; set; }
        public bool IsProjectTypeVisible { get; set; }
        public bool IsPrevailingWageTBDVisible { get; set; }
        public bool IsJobsiteAddressVisible { get; set; }
        public bool IsBillingVisible { get; set; }
        public bool IsNewCustomerVisible { get; set; }
        public bool IsContactEmailVisible { get; set; }
        public bool IsQuoteDateVisible { get; set; }
        public bool IsQuoteTimeVisible { get; set; }
        public bool IsQuoteTypeI_CVisible { get; set; }
        public bool IsQuoteTypeElectricalVisible { get; set; }
        public bool IsQuoteTypeNoBidVisible { get; set; }
        public bool IsFollowUpVisible { get; set; }
        public bool IsQuoteTypeVisible { get; set; }
        public bool IsOwnerVisible { get; set; }
        public bool IsAdvertiseDateVisible { get; set; }
        public bool IsNotifyPMVisible { get; set; }
        public bool IsServerJobFolderVisible { get; set; }
        public bool IsSiteForemanVisible { get; set; }

        public SRLogColumnConfiguration()
        {
            IsFileFolderVisible = false;
            IsQuoteVisible = false;
            IsChemFeedVisible = false;
            IsInactiveJobVisible = false;
            IsPWVisible = false;
            IsNotQuotedVisible = false;
            IsClosedVisible = false;
            IsSRNumberVisible = false;
            IsCustomerVisible = false;
            IsProjectDescriptionVisible = false;
            IsCustomerContactVisible = false;
            IsContactPhoneVisible = false;
            IsEstimatorVisible = false;
            IsDivisionVisible = false;
            IsCreationDateVisible = false;
            IsQuoteDueVisible = false;
            IsJobWalkDateVisible = false;
            IsMandatoryJobWalkVisible = false;
            IsWhoJobWalkVisible = false;
            IsBidAsPrimeOrSubVisible = false;
            IsBondingVisible = false;
            IsBondingMailSentVisible = false;
            IsPrevailingMailSentVisible = false;
            IsNotesVisible = false;
            IsProjectManagerVisible = false;
            IsCreatedByVisible = false;
            IsJobOrQuoteVisible = false;
            IsProjectTypeVisible = false;
            IsPrevailingWageTBDVisible = false;
            IsJobsiteAddressVisible = false;
            IsBillingVisible = false;
            IsNewCustomerVisible = false;
            IsContactEmailVisible = false;
            IsQuoteDateVisible = false;
            IsQuoteTimeVisible = false;
            IsQuoteTypeI_CVisible = false;
            IsQuoteTypeElectricalVisible = false;
            IsQuoteTypeNoBidVisible = false;
            IsFollowUpVisible = false;
            IsQuoteTypeVisible = false;
            IsOwnerVisible = false;
            IsAdvertiseDateVisible = false;
            IsNotifyPMVisible = false;
            IsServerJobFolderVisible = false;
            IsSiteForemanVisible = false;
        }
    }
}