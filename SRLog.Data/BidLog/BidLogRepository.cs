using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRLog.Entities;
using SRLog.Entities.BidLog.ViewModels;
using System.Data.Objects;
using System.Data.Entity.Validation;
using System.Data;
using SRLog.Common;

namespace SRLog.Data.BidLog
{
    public class BidLogRepository
    {
        SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();

        public List<tblBID_Log> GetBidLogs()
        {
            db = new SR_Log_DatabaseSQLEntities();
            var bidlogs = (from u in db.tblBID_Logs
                           select u).ToList();

            return bidlogs;

        }

        //public List<tblBID_Log> GetBidLogsList(int startIndex, int count, string sorting)
        //{
        //    IEnumerable<tblBID_Log> query = db.tblBID_Logs;
        //    List<tblBID_Log> list = count > 0
        //               ? query.Skip(startIndex).Take(count).ToList() //Paging
        //               : query.ToList(); //No paging

        //    foreach (tblBID_Log b in list)
        //    {
        //        b.DOW = Convert.ToDateTime(b.BidDate).ToString("ddd");
        //        string[] strsplit = b.BiddingAs.Split('#');

        //        string strBidAs = "";
        //        if (strsplit.Length > 1)
        //        {
        //            for (int i = 0; i < strsplit.Length - 1; i++)
        //            {
        //                switch (strsplit[i])
        //                {
        //                    case "0":
        //                        strBidAs = strBidAs + "I&C, ";
        //                        break;
        //                    case "1":
        //                        strBidAs = strBidAs + "Electrical, ";
        //                        break;
        //                    case "2":
        //                        strBidAs = strBidAs + "Prime, ";
        //                        break;
        //                    case "3":
        //                        strBidAs = strBidAs + "Unknown, ";
        //                        break;
        //                    case "4":
        //                        strBidAs = strBidAs + "Not Bidding, ";
        //                        break;
        //                    case "5":
        //                        strBidAs = strBidAs + "Not Qualified, ";
        //                        break;
        //                    case "6":
        //                        strBidAs = strBidAs + "Mechanical, ";
        //                        break;
        //                }
        //            }
        //            strBidAs = strBidAs.Remove(strBidAs.LastIndexOf(','));
        //            b.BiddingAs = strBidAs;
        //        }
        //        else if (strsplit.Length == 1)
        //        {
        //            switch (strsplit[0])
        //            {
        //                case "0":
        //                    strBidAs = "I&C ";
        //                    break;
        //                case "1":
        //                    strBidAs = "Electrical ";
        //                    break;
        //                case "2":
        //                    strBidAs = "Prime ";
        //                    break;
        //                case "3":
        //                    strBidAs = "Unknown ";
        //                    break;
        //                case "4":
        //                    strBidAs = "Not Bidding ";
        //                    break;
        //                case "5":
        //                    strBidAs = "Not Qualified ";
        //                    break;
        //                case "6":
        //                    strBidAs = "Mechanical ";
        //                    break;
        //            }
        //            b.BiddingAs = strBidAs;
        //        }
        //    }

        //    return list;


        //}

        public int GetBidlogCount()
        {
            db = new SR_Log_DatabaseSQLEntities();
            return db.tblBID_Logs.Count();
        }
        public IEnumerable<tblBID_Log> GetBidLogsData(string orderby)
        {



            //var bidlogs = (from u in db.tblBID_Logs
            //               select new test
            //               {
            //                   BidId=u.BidId,
            //                   ProjectName=u.ProjectName,
            //                   Division=u.Division
            //               }).ToList();

            //return bidlogs.ToList<test>();
            db = new SR_Log_DatabaseSQLEntities();

            IEnumerable<tblBID_Log> query;//= db.tblBID_Logs.OrderBy(x => x.BidDate).ThenByDescending(x => x.QuoteNumber);
            if (orderby == "Division,Bid Date")
            {
                query = db.tblBID_Logs.OrderBy(x => x.Division).ThenBy(x => x.BidDate).ThenByDescending(x => x.QuoteNumber);
            }
            else
            {
                query = db.tblBID_Logs.OrderBy(x => x.BidDate).ThenByDescending(x => x.QuoteNumber);
            }
            //List<tblBID_Log> list = query.ToList(); //No paging

            foreach (tblBID_Log b in query.ToArray())
            {


                // b.DOW = Convert.ToDateTime(b.BidDate).ToString("ddd");
                string[] strsplit = b.BiddingAs.Split('#');

                string strBidAs = "";
                if (strsplit.Length > 1)
                {
                    for (int i = 0; i < strsplit.Length - 1; i++)
                    {
                        switch (strsplit[i])
                        {
                            case "0":
                                strBidAs = strBidAs + "I&C, ";
                                break;
                            case "1":
                                strBidAs = strBidAs + "Electrical, ";
                                break;
                            case "2":
                                strBidAs = strBidAs + "Prime, ";
                                break;
                            case "3":
                                strBidAs = strBidAs + "Unknown, ";
                                break;
                            case "4":
                                strBidAs = strBidAs + "Not Bidding, ";
                                break;
                            case "5":
                                strBidAs = strBidAs + "Not Qualified, ";
                                break;
                            case "6":
                                strBidAs = strBidAs + "Mechanical, ";
                                break;
                        }
                    }
                    strBidAs = strBidAs.Remove(strBidAs.LastIndexOf(','));
                    b.BiddingAs = strBidAs;
                }
                else if (strsplit.Length == 1)
                {
                    switch (strsplit[0])
                    {
                        case "0":
                            strBidAs = "I&C ";
                            break;
                        case "1":
                            strBidAs = "Electrical ";
                            break;
                        case "2":
                            strBidAs = "Prime ";
                            break;
                        case "3":
                            strBidAs = "Unknown ";
                            break;
                        case "4":
                            strBidAs = "Not Bidding ";
                            break;
                        case "5":
                            strBidAs = "Not Qualified ";
                            break;
                        case "6":
                            strBidAs = "Mechanical ";
                            break;
                    }
                    b.BiddingAs = strBidAs;
                }
            }

            return query;



        }


        public IEnumerable<tblObsolete_BID_Log> GetBidLogsArchiveData(string orderby)
        {
            db = new SR_Log_DatabaseSQLEntities();
            IEnumerable<tblObsolete_BID_Log> query;// = db.tblObsolete_BID_Logs.OrderByDescending(x => x.BidDate);
            if (orderby == "Division,Bid Date")
            {
                query = db.tblObsolete_BID_Logs.OrderBy(x => x.Division).ThenByDescending(x => x.BidDate).ThenByDescending(x => x.QuoteNumber);
            }
            else
            {
                query = db.tblObsolete_BID_Logs.OrderByDescending(x => x.BidDate).ThenByDescending(x => x.QuoteNumber);
            }

            //List<tblBID_Log> list = query.ToList(); //No paging

            foreach (tblObsolete_BID_Log b in query.ToArray())
            {


                // b.DOW = Convert.ToDateTime(b.BidDate).ToString("ddd");
                string[] strsplit = b.BiddingAs.Split('#');

                string strBidAs = "";
                if (strsplit.Length > 1)
                {
                    for (int i = 0; i < strsplit.Length - 1; i++)
                    {
                        switch (strsplit[i])
                        {
                            case "0":
                                strBidAs = strBidAs + "I&C, ";
                                break;
                            case "1":
                                strBidAs = strBidAs + "Electrical, ";
                                break;
                            case "2":
                                strBidAs = strBidAs + "Prime, ";
                                break;
                            case "3":
                                strBidAs = strBidAs + "Unknown, ";
                                break;
                            case "4":
                                strBidAs = strBidAs + "Not Bidding, ";
                                break;
                            case "5":
                                strBidAs = strBidAs + "Not Qualified, ";
                                break;
                            case "6":
                                strBidAs = strBidAs + "Mechanical, ";
                                break;
                        }
                    }
                    strBidAs = strBidAs.Remove(strBidAs.LastIndexOf(','));
                    b.BiddingAs = strBidAs;
                }
                else if (strsplit.Length == 1)
                {
                    switch (strsplit[0])
                    {
                        case "0":
                            strBidAs = "I&C ";
                            break;
                        case "1":
                            strBidAs = "Electrical ";
                            break;
                        case "2":
                            strBidAs = "Prime ";
                            break;
                        case "3":
                            strBidAs = "Unknown ";
                            break;
                        case "4":
                            strBidAs = "Not Bidding ";
                            break;
                        case "5":
                            strBidAs = "Not Qualified ";
                            break;
                        case "6":
                            strBidAs = "Mechanical ";
                            break;
                    }
                    b.BiddingAs = strBidAs;
                }
            }

            return query;



        }
        public int AddBidLog(BidLogViewModel model, string Flag)
        {
            int BidId = 0;
            using (var context = new SR_Log_DatabaseSQLEntities())
            {
                ObjectParameter BidID = new ObjectParameter("result", typeof(int));

                var bidlog = context.USP_TT_InsertUpdateBidLog(model.BidId, model.BidDate, model.BiddingAs, model.BidTo, model.ProjectName, model.LastAddendumRecvd, model.Estimator
                    , model.QuoteNumber, model.UID, model.Notes, model.EngineersEstimate, model.Division, "", "", false, false, false, model.MandetoryJobWalk, model.JobWalkDate, model.Bid_Rev,
                    model.Bid_Duration, model.Bid_LiqDamage, model.Bid_Warranty, model.Bid_BondingReq, model.Bid_LicenseReqd, model.Bid_Qualified,
                    model.Bid_PrevailingWage, model.Bid_UL508Reqd, model.QADeadLineDateTime, model.OwnerName, model.AdvertiseDate, model.ProjectFolder,
                    model.ModifiedDate, model.CityState, model.PrimeSIEstimate, model.IAndCEstimate, Flag, BidID).ToString();
                BidId = Convert.ToInt32(BidID.Value);
                //if(Flag=="A")
                //{
                //    BidId = Convert.ToInt32(BidID.Value);
                //}
                //    else
                //{
                //    BidId= Convert.ToInt32(model.BidId);
                //}

            }
            return BidId;
        }


        public BidLogViewModel GetBidRecords(int BidId)
        {
            BidLogViewModel vwBid = new BidLogViewModel();
            using (var db = new SR_Log_DatabaseSQLEntities())
            {

                var bidlist = (from b in db.tblBID_Logs
                               where b.BidId == BidId
                               select b).ToList();


                vwBid.BiddingAsIandC = false;
                vwBid.BiddingAsElectircal = false;
                vwBid.BiddingAsPrime = false;
                vwBid.BiddingAsUnKnown = false;
                vwBid.BiddingAsNotBidding = false;
                vwBid.BiddingAsNotQualified = false;
                vwBid.BiddingAsMechanical = false;

                foreach (var i in bidlist)
                {

                    vwBid.BidDate = i.BidDate;
                    string BiddingAs = i.BiddingAs;
                    string[] arrBiddingAs = BiddingAs.Split('#');
                    foreach (string x in arrBiddingAs)
                    {

                        if (x == "0")
                        {
                            vwBid.BiddingAsIandC = true;
                        }


                        if (x == "1")
                        {
                            vwBid.BiddingAsElectircal = true;
                        }


                        if (x == "2")
                        {
                            vwBid.BiddingAsPrime = true;
                        }

                        if (x == "3")
                        {
                            vwBid.BiddingAsUnKnown = true;
                        }

                        if (x == "4")
                        {
                            vwBid.BiddingAsNotBidding = true;
                        }

                        if (x == "5")
                        {
                            vwBid.BiddingAsNotQualified = true;
                        }

                        if (x == "6")
                        {
                            vwBid.BiddingAsMechanical = true;
                        }

                    }

                    if (i.Division == "Concord")
                    {
                        vwBid.DivisionConcord = true;
                    }
                    if (i.Division == "Hanford")
                    {
                        vwBid.DivisionHanford = true;
                    }
                    if (i.Division == "Sacramento")
                    {
                        vwBid.DivisionSacramento = true;
                    }
                    vwBid.OwnerName = i.OwnerName;
                    vwBid.ProjectName = i.ProjectName;
                    vwBid.CityState = i.CityState;
                    vwBid.LastAddendumRecvd = i.LastAddendumRecvd;
                    vwBid.QuoteNumber = i.QuoteNumber;
                    vwBid.Bid_Rev = i.Bid_Rev;
                    vwBid.Estimator = i.Estimator;
                    vwBid.IAndCEstimate = i.IAndCEstimate;
                    vwBid.PrimeSIEstimate = i.PrimeSIEstimate;
                    vwBid.EngineersEstimate = i.EngineersEstimate;
                    vwBid.Bid_Duration = i.Bid_Duration;
                    vwBid.Bid_LiqDamage = i.Bid_LiqDamage;
                    vwBid.Bid_Warranty = i.Bid_Warranty;
                    vwBid.Bid_Qualified = (i.Bid_Qualified == true ? true : false);
                    vwBid.Bid_PrevailingWage = (i.Bid_PrevailingWage == true ? true : false);
                    vwBid.Bid_LicenseReqd = i.Bid_LicenseReqd;
                    vwBid.Bid_BondingReq = i.Bid_BondingReq;
                    vwBid.Notes = i.Notes;
                    vwBid.MandetoryJobWalk = (i.MandetoryJobWalk == true ? true : false);
                    vwBid.JobWalkDate = i.JobWalkDate;
                    vwBid.BidTo = i.BidTo;
                    vwBid.BidId = i.BidId;
                    vwBid.UID = i.UID;
                    vwBid.Bid_UL508Reqd = (i.Bid_UL508Reqd == true ? true : false);
                    vwBid.QADeadLineDateTime = i.QADeadLineDateTime;
                    vwBid.AdvertiseDate = i.AdvertiseDate;
                    vwBid.ModifiedDate = i.ModifiedDate;
                }
                return vwBid;
            }

        }


        public ObsoleteBidLogViewModel GetArchiveBidRecords(int BidId)
        {
            ObsoleteBidLogViewModel vwBid = new ObsoleteBidLogViewModel();
            using (var db = new SR_Log_DatabaseSQLEntities())
            {

                var bidlist = (from b in db.tblObsolete_BID_Logs
                               where b.ObsoleteBID_Id == BidId
                               select b).ToList();


                vwBid.BiddingAsIandC = false;
                vwBid.BiddingAsElectircal = false;
                vwBid.BiddingAsPrime = false;
                vwBid.BiddingAsUnKnown = false;
                vwBid.BiddingAsNotBidding = false;
                vwBid.BiddingAsNotQualified = false;
                vwBid.BiddingAsMechanical = false;

                foreach (var i in bidlist)
                {
                    vwBid.BidDate = i.BidDate;
                    string BiddingAs = i.BiddingAs;
                    string[] arrBiddingAs = BiddingAs.Split('#');
                    foreach (string x in arrBiddingAs)
                    {

                        if (x == "0")
                        {
                            vwBid.BiddingAsIandC = true;
                        }


                        if (x == "1")
                        {
                            vwBid.BiddingAsElectircal = true;
                        }


                        if (x == "2")
                        {
                            vwBid.BiddingAsPrime = true;
                        }

                        if (x == "3")
                        {
                            vwBid.BiddingAsUnKnown = true;
                        }

                        if (x == "4")
                        {
                            vwBid.BiddingAsNotBidding = true;
                        }

                        if (x == "5")
                        {
                            vwBid.BiddingAsNotQualified = true;
                        }

                        if (x == "6")
                        {
                            vwBid.BiddingAsMechanical = true;
                        }

                    }

                    if (i.Division == "Concord")
                    {
                        vwBid.DivisionConcord = true;
                    }
                    if (i.Division == "Hanford")
                    {
                        vwBid.DivisionHanford = true;
                    }
                    if (i.Division == "Sacramento")
                    {
                        vwBid.DivisionSacramento = true;
                    }
                    vwBid.OwnerName = i.OwnerName;
                    vwBid.ProjectName = i.ProjectName;
                    vwBid.CityState = i.CityState;
                    vwBid.LastAddendumRecvd = i.LastAddendumRecvd;
                    vwBid.QuoteNumber = i.QuoteNumber;
                    vwBid.Bid_Rev = i.Bid_Rev;
                    vwBid.Estimator = i.Estimator;
                    vwBid.IAndCEstimate = i.IAndCEstimate;
                    vwBid.PrimeSIEstimate = i.PrimeSIEstimate;
                    vwBid.EngineersEstimate = i.EngineersEstimate;
                    vwBid.Bid_Duration = i.Bid_Duration;//not added
                    vwBid.Bid_LiqDamage = i.Bid_LiqDamage;//not require
                    vwBid.Bid_Warranty = i.Bid_Warranty;//not require
                    vwBid.Bid_Qualified = (i.Bid_Qualified == true ? true : false);//not rewuire
                    vwBid.Bid_PrevailingWage = (i.Bid_PrevailingWage == true ? true : false);//not require
                    vwBid.Bid_LicenseReqd = i.Bid_LicenseReqd;//not require
                    vwBid.Bid_BondingReq = i.Bid_BondingReq;//not require
                    vwBid.Notes = i.Notes;
                    vwBid.MandetoryJobWalk = (i.Mandatory_Job_Walk == true ? true : false);
                    vwBid.JobWalkDate = i.Job_Walk_Date;
                    vwBid.BidTo = i.BidTo;
                    vwBid.BidId = i.ObsoleteBID_Id;
                    vwBid.UID = i.UID;
                    vwBid.Bid_UL508Reqd = (i.Bid_UL508Reqd == true ? true : false);
                    vwBid.QADeadLineDateTime = i.QADeadLineDateTime;
                    vwBid.AdvertiseDate = i.AdvertiseDate;

                }
                return vwBid;
            }

        }
        public IList<tblBID_ResultsTemp> GetBidResults(int BidType, int UID, int UserId)
        {
            db = new SR_Log_DatabaseSQLEntities();

            //IList<tblBID_Result> query = db.tblBID_Results.Where(x => x.UID == UID && x.BidType == BidType).ToList();
            IList<tblBID_ResultsTemp> query = db.tblBID_ResultsTemps.Where(x => x.UID == UID && x.BidType == BidType && x.UserID == 0).ToList();
            IList<tblBID_ResultsTemp> querytemp = db.tblBID_ResultsTemps.Where(x => x.UID == UID && x.BidType == BidType && x.UserID == UserId).ToList();
            foreach (var i in querytemp)
            {

                tblBID_ResultsTemp b = new tblBID_ResultsTemp();
                b.UID = i.UID;
                b.BidStatus = i.BidStatus;
                b.TelstarQuote = i.TelstarQuote;
                b.LowGC = i.LowGC;
                b.LowElecSub = i.LowElecSub;
                b.BidType = i.BidType;
                b.Description = i.Description;
                b.Column1 = i.Column1;
                b.Column2 = i.Column2;
                b.Column3 = i.Column3;
                b.Column4 = i.Column4;
                b.Column5 = i.Column5;
                b.Column6 = i.Column6;
                b.Column7 = i.Column7;
                b.Column8 = i.Column8;
                b.Column9 = i.Column9;
                b.Column10 = i.Column10;
                b.ModifiedDate = i.ModifiedDate;
                b.BIDResults_Id = i.BIDResults_Id;
                query.Add(b);

            }
            return query;
        }


        public int AddUpdateBidChecklist(BidChecklistViewModel model, string Flag)
        {
            int id = 0;
            tblBID_Checklist bchklist = new tblBID_Checklist();
            bchklist.SRNumber = model.SRNumber;
            bchklist.PrequalificationRequired = (model.PrequalificationRequired == true ? true : false);
            bchklist.IsTelstarPrequalified = (model.IsTelstarPrequalified == true ? true : false);
            bchklist.QualificationPacketReceived = (model.QualificationPacketReceived == true ? true : false);
            bchklist.Map = (model.Map == true ? true : false);
            bchklist.BidForm = (model.BidForm == true ? true : false);
            bchklist.SubsRequireListing = (model.SubsRequireListing == true ? true : false);
            bchklist.EquipmentRequiresListing = (model.EquipmentRequiresListing == true ? true : false);
            bchklist.SpareParts = (model.SpareParts == true ? true : false);
            bchklist.IsAnotherContractorHardSpecdForThisWork = model.IsAnotherContractorHardSpecdForThisWork;
            bchklist.EquipmentRequiresListingText = model.EquipmentRequiresListingText;
            bchklist.SparePartsText = model.SparePartsText;
            bchklist.ElectricalEngineer = model.ElectricalEngineer;
            bchklist.ElectricalEngineeringContact = model.ElectricalEngineeringContact;
            bchklist.CustomerContact = model.CustomerContact;
            bchklist.PAndSSourcePh = model.PAndSSourcePh;
            bchklist.Weblink = model.Weblink;
            bchklist.PlanholdersList = model.PlanholdersList;
            bchklist.HowLongDoWeNeedToHoldPricesFor = model.HowLongDoWeNeedToHoldPricesFor;
            bchklist.BidScheduleItemsTelstarIsBiddingOn = model.BidScheduleItemsTelstarIsBiddingOn;
            bchklist.Instrumentation = model.Instrumentation;
            bchklist.PlcControls = model.PlcControls;
            bchklist.Hmi = model.Hmi;
            bchklist.Power = model.Power;
            bchklist.PlcProgrammingBy = model.PlcProgrammingBy;
            bchklist.HmiProgrammingBy = model.HmiProgrammingBy;
            bchklist.ReportProgrammingBy = model.ReportProgrammingBy;
            bchklist.WhoAreYouGoingToUseForTheIntegration = model.WhoAreYouGoingToUseForTheIntegration;
            bchklist.WasThereAPrequalProcess = model.WasThereAPrequalProcess;
            bchklist.QualificationsProcess = model.QualificationsProcess;
            bchklist.WhenWillYouNotifyUsThatWeWillBeListed = model.WhenWillYouNotifyUsThatWeWillBeListed;
            bchklist.HaveThereBeenAnyAddendumsListedSoFar = model.HaveThereBeenAnyAddendumsListedSoFar;
            bchklist.AreYouTheOneThatDecidesTheSystemsIntegrator = model.AreYouTheOneThatDecidesTheSystemsIntegrator;
            bchklist.WhenIsTheNextPrequalification = model.WhenIsTheNextPrequalification;


            using (var context = new SR_Log_DatabaseSQLEntities())
            {
                if (Flag == "A")
                {
                    context.tblBID_Checklists.Add(bchklist);
                    //  context.tblBID_Checklists.InsertOnSubmit(bchklist);
                    context.SaveChanges();
                }
                else
                {
                    var clist = (from c in context.tblBID_Checklists
                                 where c.SRNumber == bchklist.SRNumber
                                 select c).FirstOrDefault();
                    clist.SRNumber = bchklist.SRNumber;
                    clist.PrequalificationRequired = bchklist.PrequalificationRequired;
                    clist.IsTelstarPrequalified = bchklist.IsTelstarPrequalified;
                    clist.QualificationPacketReceived = bchklist.QualificationPacketReceived;
                    clist.Map = bchklist.Map;
                    clist.BidForm = bchklist.BidForm;
                    clist.SubsRequireListing = bchklist.SubsRequireListing;
                    clist.EquipmentRequiresListing = bchklist.EquipmentRequiresListing;
                    clist.SpareParts = bchklist.SpareParts;
                    clist.IsAnotherContractorHardSpecdForThisWork = bchklist.IsAnotherContractorHardSpecdForThisWork;
                    clist.EquipmentRequiresListingText = bchklist.EquipmentRequiresListingText;
                    clist.SparePartsText = bchklist.SparePartsText;
                    clist.ElectricalEngineer = bchklist.ElectricalEngineer;
                    clist.ElectricalEngineeringContact = bchklist.ElectricalEngineeringContact;
                    clist.CustomerContact = bchklist.CustomerContact;
                    clist.PAndSSourcePh = bchklist.PAndSSourcePh;
                    clist.Weblink = bchklist.Weblink;
                    clist.PlanholdersList = bchklist.PlanholdersList;
                    clist.HowLongDoWeNeedToHoldPricesFor = bchklist.HowLongDoWeNeedToHoldPricesFor;
                    clist.BidScheduleItemsTelstarIsBiddingOn = bchklist.BidScheduleItemsTelstarIsBiddingOn;
                    clist.Instrumentation = bchklist.Instrumentation;
                    clist.PlcControls = bchklist.PlcControls;
                    clist.Hmi = bchklist.Hmi;
                    clist.Power = bchklist.Power;
                    clist.PlcProgrammingBy = bchklist.PlcProgrammingBy;
                    clist.HmiProgrammingBy = bchklist.HmiProgrammingBy;
                    clist.ReportProgrammingBy = bchklist.ReportProgrammingBy;
                    clist.WhoAreYouGoingToUseForTheIntegration = bchklist.WhoAreYouGoingToUseForTheIntegration;
                    clist.WasThereAPrequalProcess = bchklist.WasThereAPrequalProcess;
                    clist.QualificationsProcess = bchklist.QualificationsProcess;
                    clist.WhenWillYouNotifyUsThatWeWillBeListed = bchklist.WhenWillYouNotifyUsThatWeWillBeListed;
                    clist.HaveThereBeenAnyAddendumsListedSoFar = bchklist.HaveThereBeenAnyAddendumsListedSoFar;
                    clist.AreYouTheOneThatDecidesTheSystemsIntegrator = bchklist.AreYouTheOneThatDecidesTheSystemsIntegrator;
                    clist.WhenIsTheNextPrequalification = bchklist.WhenIsTheNextPrequalification;
                    context.SaveChanges();

                }
                id = Convert.ToInt32(bchklist.SRNumber);
            }

            return id;
        }

        public BidChecklistViewModel GetBidChecklistRecords(int SrNumber)
        {
            BidChecklistViewModel vwBidChecklist = new BidChecklistViewModel();
            using (var db = new SR_Log_DatabaseSQLEntities())
            {

                var chklist = (from e in db.tblBID_Checklists
                               where e.SRNumber == SrNumber
                               select e).ToList();

                if (chklist.Count > 0)
                {



                    foreach (var i in chklist)
                    {
                        vwBidChecklist.SRNumber = i.SRNumber;
                        vwBidChecklist.PrequalificationRequired = (i.PrequalificationRequired == true ? true : false);
                        vwBidChecklist.IsTelstarPrequalified = (i.IsTelstarPrequalified == true ? true : false);
                        vwBidChecklist.QualificationPacketReceived = (i.QualificationPacketReceived == true ? true : false);
                        vwBidChecklist.Map = (i.Map == true ? true : false);
                        vwBidChecklist.BidForm = (i.BidForm == true ? true : false);
                        vwBidChecklist.SubsRequireListing = (i.SubsRequireListing == true ? true : false);
                        vwBidChecklist.EquipmentRequiresListing = (i.EquipmentRequiresListing == true ? true : false);
                        vwBidChecklist.SpareParts = (i.SpareParts == true ? true : false);
                        vwBidChecklist.IsAnotherContractorHardSpecdForThisWork = i.IsAnotherContractorHardSpecdForThisWork;
                        vwBidChecklist.EquipmentRequiresListingText = i.EquipmentRequiresListingText;
                        vwBidChecklist.SparePartsText = i.SparePartsText;
                        vwBidChecklist.ElectricalEngineer = i.ElectricalEngineer;
                        vwBidChecklist.ElectricalEngineeringContact = i.ElectricalEngineeringContact;
                        vwBidChecklist.CustomerContact = i.CustomerContact;
                        vwBidChecklist.PAndSSourcePh = i.PAndSSourcePh;
                        vwBidChecklist.Weblink = i.Weblink;
                        vwBidChecklist.PlanholdersList = i.PlanholdersList;
                        vwBidChecklist.HowLongDoWeNeedToHoldPricesFor = i.HowLongDoWeNeedToHoldPricesFor;
                        vwBidChecklist.BidScheduleItemsTelstarIsBiddingOn = i.BidScheduleItemsTelstarIsBiddingOn;
                        vwBidChecklist.Instrumentation = i.Instrumentation;
                        vwBidChecklist.PlcControls = i.PlcControls;
                        vwBidChecklist.Hmi = i.Hmi;
                        vwBidChecklist.Power = i.Power;
                        vwBidChecklist.PlcProgrammingBy = i.PlcProgrammingBy;
                        vwBidChecklist.HmiProgrammingBy = i.HmiProgrammingBy;
                        vwBidChecklist.ReportProgrammingBy = i.ReportProgrammingBy;
                        vwBidChecklist.WhoAreYouGoingToUseForTheIntegration = i.WhoAreYouGoingToUseForTheIntegration;
                        vwBidChecklist.WasThereAPrequalProcess = i.WasThereAPrequalProcess;
                        vwBidChecklist.QualificationsProcess = i.QualificationsProcess;
                        vwBidChecklist.WhenWillYouNotifyUsThatWeWillBeListed = i.WhenWillYouNotifyUsThatWeWillBeListed;
                        vwBidChecklist.HaveThereBeenAnyAddendumsListedSoFar = i.HaveThereBeenAnyAddendumsListedSoFar;
                        vwBidChecklist.AreYouTheOneThatDecidesTheSystemsIntegrator = i.AreYouTheOneThatDecidesTheSystemsIntegrator;
                        vwBidChecklist.WhenIsTheNextPrequalification = i.WhenWillYouNotifyUsThatWeWillBeListed;
                        vwBidChecklist.BIDChecklist_Id = i.BIDChecklist_Id;

                    }
                }
                else
                {
                    return null;
                }


                return vwBidChecklist;
            }

        }


        public BidResultViewModel GetBidResultsRecords(int UId, int UserID)
        {
            BidResultViewModel vwBidResult = new BidResultViewModel();
            using (var db = new SR_Log_DatabaseSQLEntities())
            {

                var chkResults = (from a in db.tblBID_Results
                                  where a.UID == UId
                                  select a).OrderByDescending(x => x.BidType).ToList();




                foreach (var i in chkResults)
                {
                    vwBidResult.UID = i.UID;
                    vwBidResult.BidStatus = i.BidStatus;
                    if (i.BidStatus == 1)
                    {
                        vwBidResult.BidStatusLowBldder = true;

                    }
                    if (i.BidStatus == 2)
                    {
                        vwBidResult.BidStatusHighBladder = true;
                    }
                    if (i.BidStatus == 3)
                    {
                        vwBidResult.BidStatusDidNotGet = true;
                    }


                    vwBidResult.TelstarQuote = i.TelstarQuote;
                    vwBidResult.LowGC = i.LowGC;
                    vwBidResult.LowElecSub = i.LowElecSub;
                    vwBidResult.BidType = i.BidType;
                    vwBidResult.Description = i.Description;
                    vwBidResult.Column1 = i.Column1;
                    vwBidResult.Column2 = i.Column2;
                    vwBidResult.Column3 = i.Column3;
                    vwBidResult.Column4 = i.Column4;
                    vwBidResult.Column5 = i.Column5;
                    vwBidResult.Column6 = i.Column6;
                    vwBidResult.Column7 = i.Column7;
                    vwBidResult.Column8 = i.Column8;
                    vwBidResult.Column9 = i.Column9;
                    vwBidResult.Column10 = i.Column10;
                    vwBidResult.ModifiedDate = i.ModifiedDate;

                    vwBidResult.BIDResults_Id = i.BIDResults_Id;
                }

                var remove = (from aremove in db.tblBID_ResultsTemps
                              where aremove.UID == UId
                              select aremove).ToList();
                if (remove != null)
                {
                    foreach (var detail in remove)
                    {
                        db.tblBID_ResultsTemps.Remove(detail);
                    }
                    db.SaveChanges();
                }

                foreach (var i in chkResults)
                {
                    using (var context = new SR_Log_DatabaseSQLEntities())
                    {
                        tblBID_ResultsTemp bidresult = new tblBID_ResultsTemp();
                        bidresult.UID = i.UID;



                        bidresult.BidStatus = i.BidStatus;



                        bidresult.TelstarQuote = i.TelstarQuote;
                        bidresult.LowGC = i.LowGC;
                        bidresult.LowElecSub = i.LowElecSub;
                        bidresult.BidType = i.BidType;
                        bidresult.Description = i.Description;
                        bidresult.Column1 = i.Column1;
                        bidresult.Column2 = i.Column2;
                        bidresult.Column3 = i.Column3;
                        bidresult.Column4 = i.Column4;
                        bidresult.Column5 = i.Column5;
                        bidresult.Column6 = i.Column6;
                        bidresult.Column7 = i.Column7;
                        bidresult.Column8 = i.Column8;
                        bidresult.Column9 = i.Column9;
                        bidresult.Column10 = i.Column10;
                        //bidresult.ModifiedDate = System.DateTime.Now;
                        CommonFunctions c = new CommonFunctions();
                        bidresult.ModifiedDate = c.GetCurrentDate();


                        bidresult.UserID = 0;
                        context.tblBID_ResultsTemps.Add(bidresult);
                        context.SaveChanges();
                    }
                }

                return vwBidResult;
            }

        }



        public void AddBidResults(BidResultViewModel model)
        {
            int id = 0;
            tblBID_ResultsTemp bidresult = new tblBID_ResultsTemp();
            bidresult.UID = model.UID;

            //if (model.BidStatusLowBldder == true)
            //{
            //    model.BidStatus = 1;

            //}
            //if (model.BidStatusHighBladder == true)
            //{
            //    model.BidStatus = 2;

            //}
            //if (model.BidStatusDidNotGet == true)
            //{
            //    model.BidStatus = 3;
            //}

            bidresult.BidStatus = model.BidStatus;



            bidresult.TelstarQuote = model.TelstarQuote;
            bidresult.LowGC = model.LowGC;
            bidresult.LowElecSub = model.LowElecSub;
            bidresult.BidType = model.BidType;
            bidresult.Description = model.Description;
            bidresult.Column1 = model.Column1;
            bidresult.Column2 = model.Column2;
            bidresult.Column3 = model.Column3;
            bidresult.Column4 = model.Column4;
            bidresult.Column5 = model.Column5;
            bidresult.Column6 = model.Column6;
            bidresult.Column7 = model.Column7;
            bidresult.Column8 = model.Column8;
            bidresult.Column9 = model.Column9;
            bidresult.Column10 = model.Column10;
            // bidresult.ModifiedDate = System.DateTime.Now;
            CommonFunctions c = new CommonFunctions();
            bidresult.ModifiedDate = c.GetCurrentDate();

            bidresult.UserID = model.UserId;

            using (var context = new SR_Log_DatabaseSQLEntities())
            {

                context.tblBID_ResultsTemps.Add(bidresult);
                context.SaveChanges();
            }

        }


        public void SaveBidResults(int UID)
        {

            using (var ctx = new SR_Log_DatabaseSQLEntities())
            {
                var bidresults = (from y in ctx.tblBID_Results
                                  where y.UID == UID
                                  select y).ToList();

                foreach (var x in bidresults)
                {
                    ctx.tblBID_Results.Remove(x);
                    ctx.SaveChanges();
                }

                IList<tblBID_ResultsTemp> query = db.tblBID_ResultsTemps.Where(b => b.UID == UID).OrderByDescending(b => b.BIDResults_Id).ToList();

                if (query.Count > 0)
                {
                    int BidStatus = (query[0].BidStatus == null ? 3 : Convert.ToInt32(query[0].BidStatus));
                    string TelstarQuote = query[0].TelstarQuote;
                    string LowGC = query[0].LowGC;
                    string LowElecSub = query[0].LowElecSub;

                    foreach (var i in query)
                    {

                        tblBID_Result bidresult = new tblBID_Result();
                        bidresult.UID = i.UID;
                        bidresult.BidStatus = i.BidStatus;
                        bidresult.TelstarQuote = i.TelstarQuote;
                        bidresult.LowGC = i.LowGC;
                        bidresult.LowElecSub = i.LowElecSub;
                        bidresult.BidType = i.BidType;
                        bidresult.Description = i.Description;
                        bidresult.Column1 = i.Column1;
                        bidresult.Column2 = i.Column2;
                        bidresult.Column3 = i.Column3;
                        bidresult.Column4 = i.Column4;
                        bidresult.Column5 = i.Column5;
                        bidresult.Column6 = i.Column6;
                        bidresult.Column7 = i.Column7;
                        bidresult.Column8 = i.Column8;
                        bidresult.Column9 = i.Column9;
                        bidresult.Column10 = i.Column10;
                        //bidresult.ModifiedDate = System.DateTime.Now;
                        CommonFunctions c = new CommonFunctions();
                        bidresult.ModifiedDate = c.GetCurrentDate();

                        using (var context = new SR_Log_DatabaseSQLEntities())
                        {

                            context.tblBID_Results.Add(bidresult);
                            context.SaveChanges();
                        }


                    }


                    var bidresultstemp = (from y in ctx.tblBID_ResultsTemps
                                          where y.UID == UID
                                          select y).ToList();

                    foreach (var x in bidresultstemp)
                    {
                        ctx.tblBID_ResultsTemps.Remove(x);
                        ctx.SaveChanges();
                    }
                }


            }
        }

        public int UpdateArchiveBidLog(ObsoleteBidLogViewModel model)
        {
            try
            {
                int BidId = 0;
                using (var context = new SR_Log_DatabaseSQLEntities())
                {
                    tblObsolete_BID_Log result = (from c in context.tblObsolete_BID_Logs
                                                  where c.ObsoleteBID_Id == model.BidId
                                                  select c).SingleOrDefault();
                    //  DateTime bidate = System.DateTime.Now.Date;
                    CommonFunctions com = new CommonFunctions();


                    DateTime bidate = com.GetCurrentDate().Date;


                    if (model.BidDate != null)
                    {
                        bidate = model.BidDate;
                    }
                    result.BidDate = bidate;
                    result.BiddingAs = model.BiddingAs;
                    result.Bid_Rev = model.Bid_Rev;

                    result.QADeadLineDateTime = model.QADeadLineDateTime;
                    result.Division = model.Division;
                    result.Mandatory_Job_Walk = model.MandetoryJobWalk;
                    result.Job_Walk_Date = model.JobWalkDate;
                    result.BidTo = model.BidTo;
                    result.OwnerName = model.OwnerName;
                    result.CityState = model.CityState;
                    result.AdvertiseDate = model.AdvertiseDate;
                    result.ProjectName = model.ProjectName;
                    result.LastAddendumRecvd = model.LastAddendumRecvd;
                    result.Estimator = model.Estimator;
                    result.QuoteNumber = model.QuoteNumber;

                    result.Notes = model.Notes;

                    result.EngineersEstimate = model.EngineersEstimate;

                    result.PrimeSIEstimate = model.PrimeSIEstimate;

                    result.IAndCEstimate = model.IAndCEstimate;

                    result.Bid_Rev = model.Bid_Rev;

                    result.UID = model.UID;

                    //result.RedSheet = "";
                    //result.ScopeLetter = "";
                    //result.Bid_Duration = "";
                    //result.Bid_LicenseReqd = "";
                    //result.Bid_Warranty = "";
                    //result.Bid_LiqDamage = "";
                    //result.Bid_BondingReq = "";
                    //result.Bid_Qualified = false;
                    //result.Bid_PrevailingWage = false;
                    //result.Bid_UL508Reqd = false;
                    //result.ProjectFolder = "";
                    context.SaveChanges();
                }
                return BidId;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public int AddArchiveBidLog(tblObsolete_BID_Log model)
        {
            try
            {
                int BidId = 0;
                using (var context = new SR_Log_DatabaseSQLEntities())
                {
                    context.tblObsolete_BID_Logs.Add(model);
                    context.SaveChanges();
                }
                return BidId;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public IList<tblObsoluteBID_Results_Temp> GetArchiveBidResults(int BidType, int UID, int UserId)
        {

            //IList<tblBID_Result> query = db.tblBID_Results.Where(x => x.UID == UID && x.BidType == BidType).ToList();
            IList<tblObsoluteBID_Results_Temp> query = db.tblObsoluteBID_Results_Temps.Where(x => x.UID == UID && x.BidType == BidType && x.UserID == 0).ToList();
            IList<tblObsoluteBID_Results_Temp> querytemp = db.tblObsoluteBID_Results_Temps.Where(x => x.UID == UID && x.BidType == BidType && x.UserID == UserId).ToList();
            foreach (var i in querytemp)
            {

                tblObsoluteBID_Results_Temp b = new tblObsoluteBID_Results_Temp();
                b.UID = i.UID;
                b.BidStatus = i.BidStatus;
                b.TelstarQuote = i.TelstarQuote;
                b.LowGC = i.LowGC;
                b.LowElecSub = i.LowElecSub;
                b.BidType = i.BidType;
                b.Description = i.Description;
                b.Column1 = i.Column1;
                b.Column2 = i.Column2;
                b.Column3 = i.Column3;
                b.Column4 = i.Column4;
                b.Column5 = i.Column5;
                b.Column6 = i.Column6;
                b.Column7 = i.Column7;
                b.Column8 = i.Column8;
                b.Column9 = i.Column9;
                b.Column10 = i.Column10;
                b.ObsoluteBIDResults_Id = i.ObsoluteBIDResults_Id;
                query.Add(b);

            }
            return query;
        }


        public BidResultViewModel GetArchiveBidResultsRecords(int UId, int UserID)
        {
            BidResultViewModel vwBidResult = new BidResultViewModel();
            using (var db = new SR_Log_DatabaseSQLEntities())
            {

                var chkResults = (from a in db.tblObsoluteBID_Results
                                  where a.UID == UId
                                  select a).OrderByDescending(x => x.BidType).ToList();




                foreach (var i in chkResults)
                {
                    vwBidResult.UID = i.UID;
                    vwBidResult.BidStatus = i.BidStatus;
                    if (i.BidStatus == 1)
                    {
                        vwBidResult.BidStatusLowBldder = true;

                    }
                    if (i.BidStatus == 2)
                    {
                        vwBidResult.BidStatusHighBladder = true;
                    }
                    if (i.BidStatus == 3)
                    {
                        vwBidResult.BidStatusDidNotGet = true;
                    }


                    vwBidResult.TelstarQuote = i.TelstarQuote;
                    vwBidResult.LowGC = i.LowGC;
                    vwBidResult.LowElecSub = i.LowElecSub;
                    vwBidResult.BidType = i.BidType;
                    vwBidResult.Description = i.Description;
                    vwBidResult.Column1 = i.Column1;
                    vwBidResult.Column2 = i.Column2;
                    vwBidResult.Column3 = i.Column3;
                    vwBidResult.Column4 = i.Column4;
                    vwBidResult.Column5 = i.Column5;
                    vwBidResult.Column6 = i.Column6;
                    vwBidResult.Column7 = i.Column7;
                    vwBidResult.Column8 = i.Column8;
                    vwBidResult.Column9 = i.Column9;
                    vwBidResult.Column10 = i.Column10;
                    vwBidResult.BIDResults_Id = i.ObsoluteBIDResults_Id;
                }

                //var remove = (from aremove in db.tblObsoluteBID_Results_Temps
                //             where aremove.UID == UId
                //             select aremove).ToList();

                var remove = (from aremove in db.tblObsoluteBID_Results_Temps
                              where aremove.UID == UId
                              select aremove).ToList();

                if (remove != null)
                {
                    foreach (var detail in remove)
                    {
                        db.tblObsoluteBID_Results_Temps.Remove(detail);
                    }
                    db.SaveChanges();
                }
                foreach (var i in chkResults)
                {
                    using (var context = new SR_Log_DatabaseSQLEntities())
                    {
                        tblObsoluteBID_Results_Temp bidresult = new tblObsoluteBID_Results_Temp();
                        bidresult.UID = i.UID;



                        bidresult.BidStatus = i.BidStatus;



                        bidresult.TelstarQuote = i.TelstarQuote;
                        bidresult.LowGC = i.LowGC;
                        bidresult.LowElecSub = i.LowElecSub;
                        bidresult.BidType = i.BidType;
                        bidresult.Description = i.Description;
                        bidresult.Column1 = i.Column1;
                        bidresult.Column2 = i.Column2;
                        bidresult.Column3 = i.Column3;
                        bidresult.Column4 = i.Column4;
                        bidresult.Column5 = i.Column5;
                        bidresult.Column6 = i.Column6;
                        bidresult.Column7 = i.Column7;
                        bidresult.Column8 = i.Column8;
                        bidresult.Column9 = i.Column9;
                        bidresult.Column10 = i.Column10;
                        bidresult.UserID = 0;
                        context.tblObsoluteBID_Results_Temps.Add(bidresult);
                        context.SaveChanges();
                    }
                }

                return vwBidResult;
            }

        }


        public void AddArchiveBidResults(BidResultViewModel model)
        {
            int id = 0;
            tblObsoluteBID_Results_Temp bidresult = new tblObsoluteBID_Results_Temp();
            bidresult.UID = model.UID;
            bidresult.BidStatus = model.BidStatus;
            bidresult.TelstarQuote = model.TelstarQuote;
            bidresult.LowGC = model.LowGC;
            bidresult.LowElecSub = model.LowElecSub;
            bidresult.BidType = model.BidType;
            bidresult.Description = model.Description;
            bidresult.Column1 = model.Column1;
            bidresult.Column2 = model.Column2;
            bidresult.Column3 = model.Column3;
            bidresult.Column4 = model.Column4;
            bidresult.Column5 = model.Column5;
            bidresult.Column6 = model.Column6;
            bidresult.Column7 = model.Column7;
            bidresult.Column8 = model.Column8;
            bidresult.Column9 = model.Column9;
            bidresult.Column10 = model.Column10;
            bidresult.UserID = model.UserId;

            using (var context = new SR_Log_DatabaseSQLEntities())
            {

                context.tblObsoluteBID_Results_Temps.Add(bidresult);
                context.SaveChanges();
            }

        }


        public void SaveArchiveBidResults(int UID)
        {

            using (var ctx = new SR_Log_DatabaseSQLEntities())
            {
                var bidresults = (from y in ctx.tblObsoluteBID_Results
                                  where y.UID == UID
                                  select y).ToList();

                foreach (var x in bidresults)
                {
                    ctx.tblObsoluteBID_Results.Remove(x);
                    ctx.SaveChanges();
                }

                IList<tblObsoluteBID_Results_Temp> query = db.tblObsoluteBID_Results_Temps.Where(b => b.UID == UID).OrderByDescending(b => b.ObsoluteBIDResults_Id).ToList();

                if (query.Count > 0)
                {
                    int BidStatus = (query[0].BidStatus == null ? 3 : Convert.ToInt32(query[0].BidStatus));
                    string TelstarQuote = query[0].TelstarQuote;
                    string LowGC = query[0].LowGC;
                    string LowElecSub = query[0].LowElecSub;

                    foreach (var i in query)
                    {

                        tblObsoluteBID_Result bidresult = new tblObsoluteBID_Result();
                        bidresult.UID = i.UID;
                        bidresult.BidStatus = i.BidStatus;
                        bidresult.TelstarQuote = i.TelstarQuote;
                        bidresult.LowGC = i.LowGC;
                        bidresult.LowElecSub = i.LowElecSub;
                        bidresult.BidType = i.BidType;
                        bidresult.Description = i.Description;
                        bidresult.Column1 = i.Column1;
                        bidresult.Column2 = i.Column2;
                        bidresult.Column3 = i.Column3;
                        bidresult.Column4 = i.Column4;
                        bidresult.Column5 = i.Column5;
                        bidresult.Column6 = i.Column6;
                        bidresult.Column7 = i.Column7;
                        bidresult.Column8 = i.Column8;
                        bidresult.Column9 = i.Column9;
                        bidresult.Column10 = i.Column10;


                        using (var context = new SR_Log_DatabaseSQLEntities())
                        {

                            context.tblObsoluteBID_Results.Add(bidresult);
                            context.SaveChanges();
                        }


                    }


                    var bidresultstemp = (from y in ctx.tblBID_ResultsTemps
                                          where y.UID == UID
                                          select y).ToList();

                    foreach (var x in bidresultstemp)
                    {
                        ctx.tblBID_ResultsTemps.Remove(x);
                        ctx.SaveChanges();
                    }

                }
            }
        }


        public int AddActivateBidLog(tblBID_Log model)
        {
            int BidId = 0;
            using (var context = new SR_Log_DatabaseSQLEntities())
            {
                ObjectParameter BidID = new ObjectParameter("result", typeof(int));

                var bidlog = context.USP_TT_InsertUpdateBidLog(0, model.BidDate, model.BiddingAs, model.BidTo, model.ProjectName, model.LastAddendumRecvd, model.Estimator
                    , model.QuoteNumber, model.UID, model.Notes, model.EngineersEstimate, model.Division, "", "", false, false, false, model.MandetoryJobWalk, model.JobWalkDate, model.Bid_Rev,
                    model.Bid_Duration, model.Bid_LiqDamage, model.Bid_Warranty, model.Bid_BondingReq, model.Bid_LicenseReqd, model.Bid_Qualified,
                    model.Bid_PrevailingWage, model.Bid_UL508Reqd, model.QADeadLineDateTime, model.OwnerName, model.AdvertiseDate, model.ProjectFolder,
                    model.ModifiedDate, model.CityState, model.PrimeSIEstimate, model.IAndCEstimate, "A", BidID).ToString();
                //BidId =BidID;
                BidId = Convert.ToInt32(BidID.Value);


            }
            return BidId;
        }


        public DataTable GetBidLogReportDetails()
        {
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();

            DataTable dt = new DataTable();

            dt.Columns.Add("BidDate");
            dt.Columns.Add("BiddingAs");
            dt.Columns.Add("BidTo");
            dt.Columns.Add("ProjectName");
            dt.Columns.Add("Division");
            dt.Columns.Add("LastAddendumRecvd");
            dt.Columns.Add("Estimator");
            dt.Columns.Add("QuoteNumber");
            dt.Columns.Add("EngineersEstimate");




            IEnumerable<tblBID_Log> query = db.tblBID_Logs;
            //List<tblBID_Log> list = query.ToList(); //No paging

            foreach (tblBID_Log b in query.ToArray())
            {


                // b.DOW = Convert.ToDateTime(b.BidDate).ToString("ddd");
                string[] strsplit = b.BiddingAs.Split('#');

                string strBidAs = "";
                if (strsplit.Length > 1)
                {
                    for (int i = 0; i < strsplit.Length - 1; i++)
                    {
                        switch (strsplit[i])
                        {
                            case "0":
                                strBidAs = strBidAs + "I&C, ";
                                break;
                            case "1":
                                strBidAs = strBidAs + "Electrical, ";
                                break;
                            case "2":
                                strBidAs = strBidAs + "Prime, ";
                                break;
                            case "3":
                                strBidAs = strBidAs + "Unknown, ";
                                break;
                            case "4":
                                strBidAs = strBidAs + "Not Bidding, ";
                                break;
                            case "5":
                                strBidAs = strBidAs + "Not Qualified, ";
                                break;
                            case "6":
                                strBidAs = strBidAs + "Mechanical, ";
                                break;
                        }
                    }
                    strBidAs = strBidAs.Remove(strBidAs.LastIndexOf(','));
                    b.BiddingAs = strBidAs;
                }
                else if (strsplit.Length == 1)
                {
                    switch (strsplit[0])
                    {
                        case "0":
                            strBidAs = "I&C ";
                            break;
                        case "1":
                            strBidAs = "Electrical ";
                            break;
                        case "2":
                            strBidAs = "Prime ";
                            break;
                        case "3":
                            strBidAs = "Unknown ";
                            break;
                        case "4":
                            strBidAs = "Not Bidding ";
                            break;
                        case "5":
                            strBidAs = "Not Qualified ";
                            break;
                        case "6":
                            strBidAs = "Mechanical ";
                            break;
                    }
                    b.BiddingAs = strBidAs;
                }
            }

            foreach (var i in query)
            {
                DataRow dr = dt.NewRow();

                dr["BidDate"] = i.BidDate;
                dr["BiddingAs"] = i.BiddingAs;
                dr["BidTo"] = i.BidTo;
                dr["ProjectName"] = i.ProjectName;
                dr["Division"] = i.Division;
                dr["LastAddendumRecvd"] = i.LastAddendumRecvd;
                dr["Estimator"] = i.Estimator;
                dr["QuoteNumber"] = i.QuoteNumber;
                dr["EngineersEstimate"] = i.EngineersEstimate;

                dt.Rows.Add(dr);
            }






            return dt;

        }

    }
}
