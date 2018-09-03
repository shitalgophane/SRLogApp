using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRLog.Entities.BidLog.ViewModels
{
    public class BidChecklistViewModel
    {

        public Nullable<int> SRNumber { get; set; }
        public bool PrequalificationRequired { get; set; }
        public bool IsTelstarPrequalified { get; set; }
        public bool QualificationPacketReceived { get; set; }
        public bool Map { get; set; }
        public bool BidForm { get; set; }
        public bool SubsRequireListing { get; set; }
        public bool EquipmentRequiresListing { get; set; }
        public bool SpareParts { get; set; }
        public string IsAnotherContractorHardSpecdForThisWork { get; set; }
        public string EquipmentRequiresListingText { get; set; }
        public string SparePartsText { get; set; }
        public string ElectricalEngineer { get; set; }
        public string ElectricalEngineeringContact { get; set; }
        public string CustomerContact { get; set; }
        public string PAndSSourcePh { get; set; }
        public string Weblink { get; set; }
        public string PlanholdersList { get; set; }
        public string HowLongDoWeNeedToHoldPricesFor { get; set; }
        public string BidScheduleItemsTelstarIsBiddingOn { get; set; }
        public string Instrumentation { get; set; }
        public string PlcControls { get; set; }
        public string Hmi { get; set; }
        public string Power { get; set; }
        public string PlcProgrammingBy { get; set; }
        public string HmiProgrammingBy { get; set; }
        public string ReportProgrammingBy { get; set; }
        public string WhoAreYouGoingToUseForTheIntegration { get; set; }
        public string WasThereAPrequalProcess { get; set; }
        public string QualificationsProcess { get; set; }
        public string WhenWillYouNotifyUsThatWeWillBeListed { get; set; }
        public string HaveThereBeenAnyAddendumsListedSoFar { get; set; }
        public string AreYouTheOneThatDecidesTheSystemsIntegrator { get; set; }
        public string WhenIsTheNextPrequalification { get; set; }
        public int BIDChecklist_Id { get; set; }
    }
}
