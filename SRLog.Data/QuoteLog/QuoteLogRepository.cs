using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRLog.Entities;
using System.Data.Objects;
using SRLog.Entities.QuoteLog.ViewModels;
using System.Data;

namespace SRLog.Data.QuoteLog
{
   public class QuoteLogRepository
    {
       SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
       public IEnumerable<tblObsolete_Quote> GetArchiveQuoteLogsData()
       {


           db = new SR_Log_DatabaseSQLEntities();

           IEnumerable<tblObsolete_Quote> query = db.tblObsolete_Quotes;

           foreach (tblObsolete_Quote b in query.ToArray())
           {
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


       public string UpdateQuoteLog(QuoteLogViewModel model)
       {
           string resultofquery;
           using (var context = new SR_Log_DatabaseSQLEntities())
           {
               ObjectParameter result = new ObjectParameter("result", typeof(int));
               DateTime LastDateFollowup = Convert.ToDateTime("1/1/1753");
               if (model.dtpLastDateFollowup != null)
               {
                   LastDateFollowup = Convert.ToDateTime(model.dtpLastDateFollowup);
               }
               DateTime QADeadLineDateTime = Convert.ToDateTime("1/1/1753");
               if (model.QADeadLineDateTime != null)
               {
                   QADeadLineDateTime = Convert.ToDateTime(model.QADeadLineDateTime);
               }
               var quotelog = context.USP_TT_InsertUpdateQuote(model.BidDate, model.BiddingAs, model.BidTo, model.ProjectName, model.LastAddendumRecvd, model.Estimator
                     , model.QuoteNumber, model.UID, model.Notes, model.EngineersEstimate, model.Division, model.MandetoryJobWalk,
                     model.JobWalkDate,QADeadLineDateTime, model.QuoteStatus, LastDateFollowup, model.LastFollowupBy,
                     model.FollowupNote, model.Email, "E", "", result).ToString();

              resultofquery=Convert.ToString( result.Value);
            
           }
           return resultofquery;
       }

       public DataTable GetQuoteLogReportDetails()
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
           dt.Columns.Add("Note");



           IEnumerable<tblQuoteLog> query = db.tblQuoteLogs;
           //List<tblBID_Log> list = query.ToList(); //No paging

           foreach (tblQuoteLog b in query.ToArray())
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
               dr["Note"] = i.Notes;
               dt.Rows.Add(dr);
           }






           return dt;

       }
       public IEnumerable<tblQuoteLog> GetQuoteLogsData(string orderby)
       {


           db = new SR_Log_DatabaseSQLEntities();

           IEnumerable<tblQuoteLog> query;// = db.tblQuoteLogs;
           if (orderby == "Division,Bid Date")
           {
               query = db.tblQuoteLogs.OrderBy(x => x.Division).ThenBy(x => x.BidDate).ThenByDescending(x => x.QuoteNumber);
           }
           else
           {
               query = db.tblQuoteLogs.OrderBy(x => x.BidDate).ThenByDescending(x => x.QuoteNumber);
           }


           foreach (tblQuoteLog b in query.ToArray())
           {
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
       public string AddActivateQuoteLog(tblQuoteLog model,string FlagFollowUp)
       {
          // int QuoteId = 0;
           string resultofquery;
           using (var context = new SR_Log_DatabaseSQLEntities())
           {
               ObjectParameter result = new ObjectParameter("result", typeof(string));

               var bidlog = context.USP_TT_InsertUpdateQuote(model.BidDate,model.BiddingAs,model.BidTo,model.ProjectName,model.LastAddendumRecvd,model.Estimator
                   ,model.QuoteNumber,model.UID,model.Notes,model.EngineersEstimate,model.Division,model.MandetoryJobWalk,
                   model.JobWalkDate,model.QADeadLineDateTime,model.QuoteStatus,model.dtpLastDateFollowup,model.LastFollowupBy,
                   model.FollowupNote, model.EmailAddress, "A", FlagFollowUp, result).ToString();
               resultofquery = Convert.ToString(result.Value);


           }
           return resultofquery;
       }


       public string AddArchiveQuoteLog(tblObsolete_Quote model, string FlagFollowUp)
       {
          // int QuoteId = 0;
           string resultofquery;
           using (var context = new SR_Log_DatabaseSQLEntities())
           {
               ObjectParameter result = new ObjectParameter("result", typeof(int));

               var bidlog = context.USP_TT_InsertUpdateQuote(model.BidDate, model.BiddingAs, model.BidTo, model.ProjectName, model.LastAddendumRecvd, model.Estimator
                   , model.QuoteNumber, model.UID, model.Notes, model.EngineersEstimate, model.Division, model.MandetoryJobWalk,
                   model.JobWalkDate, model.QADeadLineDateTime, model.QuoteStatus, model.dtpLastDateFollowup, model.LastFollowupBy,
                   model.FollowupNote, model.EmailAddress, "A", FlagFollowUp, result).ToString();
               resultofquery = Convert.ToString(result.Value);


           }
           return resultofquery;
       }


       public QuoteLogViewModel GetQuoteRecords(int Id)
       {
           QuoteLogViewModel vw = new QuoteLogViewModel();
           using (var db = new SR_Log_DatabaseSQLEntities())
           {

               var quptelist = (from b in db.tblQuoteLogs
                              where b.Id == Id
                              select b).ToList();


               vw.BiddingAsIandC = false;
               vw.BiddingAsElectircal = false;
               vw.BiddingAsPrime = false;
               vw.BiddingAsUnKnown = false;
               vw.BiddingAsNotBidding = false;
               vw.BiddingAsNotQualified = false;
               vw.BiddingAsMechanical = false;

               foreach (var i in quptelist)
               {

                   vw.BidDate = i.BidDate;
                   string BiddingAs = i.BiddingAs;
                   string[] arrBiddingAs = BiddingAs.Split('#');
                   foreach (string x in arrBiddingAs)
                   {

                       if (x == "0")
                       {
                           vw.BiddingAsIandC = true;
                       }


                       if (x == "1")
                       {
                           vw.BiddingAsElectircal = true;
                       }


                       if (x == "2")
                       {
                           vw.BiddingAsPrime = true;
                       }

                       if (x == "3")
                       {
                           vw.BiddingAsUnKnown = true;
                       }

                       if (x == "4")
                       {
                           vw.BiddingAsNotBidding = true;
                       }

                       if (x == "5")
                       {
                           vw.BiddingAsNotQualified = true;
                       }

                       if (x == "6")
                       {
                           vw.BiddingAsMechanical = true;
                       }

                   }

                   if (i.Division == "Concord")
                   {
                       vw.DivisionConcord = true;
                   }
                   if (i.Division == "Hanford")
                   {
                       vw.DivisionHanford = true;
                   }
                   if (i.Division == "Sacramento")
                   {
                       vw.DivisionSacramento = true;
                   }
                  
                   vw.ProjectName = i.ProjectName;                
                   vw.LastAddendumRecvd = i.LastAddendumRecvd;
                   vw.QuoteNumber = i.QuoteNumber;
                
                   vw.Estimator = i.Estimator;
              
        
                   vw.EngineersEstimate = i.EngineersEstimate;
                 
                   vw.Notes = i.Notes;
                   vw.MandetoryJobWalk = (i.MandetoryJobWalk == true ? true : false);
                   vw.JobWalkDate = i.JobWalkDate;
                   vw.BidTo = i.BidTo;
                   vw.Id = i.Id;
                   vw.UID = i.UID;
                
                   vw.QADeadLineDateTime = i.QADeadLineDateTime;
                   vw.QuoteStatus = i.QuoteStatus;
                   vw.dtpLastDateFollowup = i.dtpLastDateFollowup;
                   vw.LastFollowupBy = i.LastFollowupBy;
                   vw.FollowupNote = i.FollowupNote;
                   vw.Email = i.EmailAddress;
               }
               return vw;
           }

       }


       public List<EMail_Info> GetEmailInfo()
       {
           db = new SR_Log_DatabaseSQLEntities();
           db.Configuration.ProxyCreationEnabled = false;
           List<EMail_Info> emailinfo = (db.EMail_Infoes).ToList();
           return emailinfo;
       }
    }
}
