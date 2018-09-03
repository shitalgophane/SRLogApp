using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRLog.Entities.Settings.ViewModels;
using SRLog.Data;
using SRLog.Data.Account;
using SRLog.Data.QuoteLog;
using SRLog.Filters;
using SRLog.Entities;
using System.Web.Script.Serialization;
using SRLog.Data.Activity;
using SRLog.Data.SRLog;
using SRLog.Entities.QuoteLog.ViewModels;
using SRLog.Entities.Account.ViewModels;
using SRLog.Data.Settings;
using System.Data;
using Microsoft.Reporting.WebForms;
using SRLog.Common;
namespace SRLog.Controllers
{
    [AdminFilter]
    public class QuoteLogController : Controller
    {
        [ExceptionHandler]
        public ActionResult ArchivedIndex()
        {
            return View();
        }

        [ExceptionHandler]
        public ActionResult Index()
        {
            return View();
        }

        [ExceptionHandler]
        public ActionResult Create()
        {
            QuoteLogRepository _mainrepo = new QuoteLogRepository();
            QuoteLogViewModel objcre = new QuoteLogViewModel();
            // QuoteLogRepository objdata = new QuoteLogRepository();
            SRLogRepository objdata = new SRLogRepository();
            objcre.CustomerList = objdata.GetCustomer();
            objcre.GroupUsersList = objdata.GetGroupUser();
            objcre.emailids = _mainrepo.GetEmailInfo();

            ViewBag.UpdateDisable = false;




            objcre.Id = 0;

            if (Convert.ToString(Session["SR_Log_ReadOnly"]) == "True" && Convert.ToString(Session["Bid_Log_ReadOnly"]) == "True")
            {
                ViewBag.UpdateDisable = true;

            }

            return View(objcre);
        }

        [ExceptionHandler]
        [HttpGet]
        public ActionResult GetArchivedQuotelogData()
        {
            QuoteLogRepository _repository = new QuoteLogRepository();
            var archivequote = _repository.GetArchiveQuoteLogsData();
            var jsonData = new
            {
                data = archivequote
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        [ExceptionHandler]
        [HttpGet]
        public ActionResult GetQuotelogData(string orderby)
        {
            QuoteLogRepository _repository = new QuoteLogRepository();
            var quote = _repository.GetQuoteLogsData(orderby);
            var jsonData = new
            {
                data = quote
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        public ActionResult ActivateQuote(string Id)
        {
            int nObsoleteId = Convert.ToInt32(Id);
            tblQuoteLog result = new tblQuoteLog();
            SR_Log_DatabaseSQLEntities objdb = new SR_Log_DatabaseSQLEntities();
            var quote = (from b in objdb.tblObsolete_Quotes
                         where b.Id == nObsoleteId
                         select b).ToList();


            var uid_quotelog = objdb.tblQuoteLogs.OrderByDescending(u => u.UID).FirstOrDefault();
            var uid_obsquotelog = objdb.tblObsolete_Quotes.OrderByDescending(u => u.UID).FirstOrDefault();

            string bidate = "1/1/1753";
            if (quote[0].BidDate != null)
            {
                bidate = Convert.ToString(quote[0].BidDate);
            }

            result.BidDate = Convert.ToDateTime(bidate);
            result.BiddingAs = quote[0].BiddingAs;
            result.BidTo = quote[0].BidTo;
            result.ProjectName = quote[0].ProjectName;
            result.Division = quote[0].Division;
            result.LastAddendumRecvd = quote[0].LastAddendumRecvd;
            result.Estimator = quote[0].Estimator;
            result.QuoteNumber = quote[0].QuoteNumber;
            result.UID = quote[0].UID;
            result.EngineersEstimate = quote[0].EngineersEstimate;
            result.Notes = quote[0].Notes;
            result.JobWalkDate = quote[0].Job_Walk_Date;
            result.QADeadLineDateTime = quote[0].QADeadLineDateTime;
            result.QuoteStatus = quote[0].QuoteStatus;
            result.dtpLastDateFollowup = quote[0].dtpLastDateFollowup;
            result.LastFollowupBy = quote[0].LastFollowupBy;
            result.FollowupNote = quote[0].FollowupNote;
            result.EmailAddress = quote[0].EmailAddress;

            QuoteLogRepository _repo = new QuoteLogRepository();

            _repo.AddActivateQuoteLog(result, "T");

            var act = new ActivityRepository();
            act.AddActivityLog(Convert.ToString(Session["User"]), "Archived Quote", "Activate Quote ", "Archived Quote " + quote[0].UID.ToString() + " activated by user " + Convert.ToString(Session["User"]) + ".");

            foreach (var detail in quote)
            {
                objdb.tblObsolete_Quotes.Remove(detail);
            }
            objdb.SaveChanges();
            act.AddActivityLog(Convert.ToString(Session["User"]), "Delete Archived Quote", "Activate Quote ", "Archived Quote " + quote[0].UID.ToString() + " deleted after moving to current by user " + Convert.ToString(Session["User"]) + ".");


            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result1 = javaScriptSerializer.Serialize(result);
            return Json(result1, JsonRequestBehavior.AllowGet);

        }

        [ExceptionHandler]
        public ActionResult PreviewReport()
        {
            QuoteLogRepository _repository = new QuoteLogRepository();
            DataTable dt = _repository.GetQuoteLogReportDetails();
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.BorderWidth = 0;
            reportViewer.Width = 650;
            reportViewer.Height = 300;
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\QuoteLog.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));

            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, empName });
            ViewBag.ReportViewer = reportViewer;
            ViewBag.ReportViewerFlag = true;

            return View("Print");
        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult Create(QuoteLogViewModel model, FormCollection form)
        {
            QuoteLogRepository _repo = new QuoteLogRepository();
            SRLogRepository objdata = new SRLogRepository();
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                SR_Log_DatabaseSQLEntities objdb1 = new SR_Log_DatabaseSQLEntities();

                string hdnCustomer = "";
                if (string.IsNullOrEmpty(form["hdnCustomer"]) == false)
                {
                    hdnCustomer = Convert.ToString(form["hdnCustomer"]).ToUpper();
                }
                else
                {
                    hdnCustomer = Convert.ToString(form["hdnCustomer"]);
                }

                string hdnCustUpdate = Convert.ToString(form["hdnCustUpdate"]);

                if (!string.IsNullOrEmpty(hdnCustUpdate))
                {
                    //Add or Update Customer
                    tblCustomer cu = (from cust in objdb1.tblCustomers
                                      where cust.CustomerName == hdnCustomer
                                      select cust).FirstOrDefault();
                    CommonFunctions c = new CommonFunctions();
                    if (cu == null)
                    {
                        tblCustomer custad = new tblCustomer();
                        custad.CustomerName = hdnCustomer;
                        custad.DateAdded = c.GetCurrentDate();
                        custad.IsInActive = false;
                        custad.Notes = null;
                        objdb1.tblCustomers.Add(custad);
                        objdb1.SaveChanges();
                    }

                }
                model.BidTo = "";
                if (string.IsNullOrEmpty(form["hdnCustomer"]) == false)
                {
                    model.BidTo = Convert.ToString(form["hdnCustomer"]).ToUpper();
                }
                else
                {
                    model.BidTo = Convert.ToString(form["hdnCustomer"]);
                }


                string strBidAs = "";
                if (model.BiddingAsIandC == true)
                {
                    strBidAs = "0#";
                }
                if (model.BiddingAsElectircal == true)
                {
                    strBidAs = strBidAs + "1#";
                }
                if (model.BiddingAsPrime == true)
                {
                    strBidAs = strBidAs + "2#";
                }
                if (model.BiddingAsUnKnown == true)
                {
                    strBidAs = strBidAs + "3#";
                }
                if (model.BiddingAsNotBidding == true)
                {
                    strBidAs = strBidAs + "4#";
                }

                if (model.BiddingAsNotQualified == true)
                {
                    strBidAs = strBidAs + "5#";
                }
                if (model.BiddingAsMechanical == true)
                {
                    strBidAs = strBidAs + "6#";
                }

                model.BiddingAs = strBidAs;

                if (model.DivisionConcord == true)
                {
                    model.Division = "Concord";
                }
                else if (model.DivisionHanford == true)
                {
                    model.Division = "Hanford";
                }
                else if (model.DivisionSacramento == true)
                {
                    model.Division = "Sacramento";
                }

                if (string.IsNullOrEmpty(model.ProjectName) == false)
                    model.ProjectName = model.ProjectName.ToUpper();

                if (string.IsNullOrEmpty(model.QuoteStatus) == false)
                    model.QuoteStatus = model.QuoteStatus.ToUpper();

                if (string.IsNullOrEmpty(model.LastFollowupBy) == false)
                    model.LastFollowupBy = model.LastFollowupBy.ToUpper();

                if (string.IsNullOrEmpty(model.FollowupNote) == false)
                    model.FollowupNote = model.FollowupNote.ToUpper();

                if (string.IsNullOrEmpty(model.EngineersEstimate) == false)
                    model.EngineersEstimate = model.EngineersEstimate.ToUpper();

                if (string.IsNullOrEmpty(model.Notes) == false)
                    model.Notes = model.Notes.ToUpper();

                _repo.UpdateQuoteLog(model);
                ViewBag.UId = model.UID;
                ViewBag.ID = model.Id;


                string dtpLastDateFollowup = "";
                if (model.dtpLastDateFollowup == null)
                {
                    dtpLastDateFollowup = "";
                }
                else
                {
                    dtpLastDateFollowup = Convert.ToDateTime(model.dtpLastDateFollowup).ToString("MM-dd-yyyy");
                }

                string bidDate = "";
                if (model.BidDate == null)
                {
                    bidDate = "";
                }
                else
                {
                    bidDate = Convert.ToDateTime(model.BidDate).ToString("MM-dd-yyyy");
                }

                SendQuoteModifyMailSMTP(dtpLastDateFollowup, model.Email, model.BidTo, model.ProjectName, bidDate, model.QuoteStatus, model.LastFollowupBy, model.FollowupNote);
                var act = new ActivityRepository();
                act.AddActivityLog(Convert.ToString(Session["User"]), "Update Quote", "Create", "Quote " + model.UID + " updated by user " + Convert.ToString(Session["User"]) + ".");


                ViewBag.Message = "Record Updated Successfully And Quote Modified Status Mail Sent Succesfully";


            }

            //BidLogViewModel objcre = new BidLogViewModel();
            objdata = new SRLogRepository();
            model.CustomerList = objdata.GetCustomer();
            model.GroupUsersList = objdata.GetGroupUser();
            bool bExistEstimator = false;
            foreach (var i in model.GroupUsersList)
            {
                if (i.UserName == model.Estimator)
                {
                    bExistEstimator = true;
                }
            }
            if (bExistEstimator == false)
            {
                tblGroupUser g = new tblGroupUser();
                g.UserName = model.Estimator;
                g.Userid = model.Estimator;
                g.Group_Name = "";

                model.GroupUsersList.Add(g);
            }


            ViewBag.UpdateDisable = false;
            if (Convert.ToString(Session["SR_Log_ReadOnly"]) == "True" && Convert.ToString(Session["Bid_Log_ReadOnly"]) == "True")
            {
                ViewBag.UpdateDisable = true;

            }
            model.emailids = _repo.GetEmailInfo();
            return View("Create", model);
        }

        [ExceptionHandler]
        public void SendQuoteModifyMailSMTP(string dtpLastDateFollowup, string EmailIds, string bidTo, string ProjectName, string BidDate, string QuoteStatus, string LastFollowupby, string FollowupNote)
        {
            string rcptPrevailingTo = "";
            string rcptPrevailingCC = "";
            string sbody = "";
            // rcptPrevailingCC = UserInfo.UserName;
            rcptPrevailingCC = Session["User"].ToString();
            sbody = "<html><head></head>";
            sbody = sbody + "<body><div style=" + "font-family: 'Calibri', 'sans-serif';font-size: 10px;" + "><span><span style='color:#1F497D'>";
            sbody = sbody + "<b>The Following Quote Points are Modified  by: " + Session["User"] + "</b>";
            sbody = sbody + "<table><tr><td><b>Customer<b></td><td>: </td><td>" + bidTo + "</td></tr>";
            sbody = sbody + "<tr><td><b>Project Description<b></td><td>: </td><td>" + ProjectName + "</td></tr>";
            sbody = sbody + "<tr><td><b>Quote Date<b></td><td>: </td><td>" + BidDate + "</td></tr>";
            sbody = sbody + "<tr><td><b>Quote Status<b></td><td>: </td><td>" + QuoteStatus + "</td></tr>";

            if (dtpLastDateFollowup != "")
            {
                sbody = sbody + "<tr><td><b>Date of Last Followup<b></td><td>: </td><td>" + dtpLastDateFollowup + "</td></tr>";

            }

            sbody = sbody + "<tr><td><b>Last Followup by<b></td><td>: </td><td>" + LastFollowupby + "</td></tr>";
            sbody = sbody + "<tr><td><b>Followup Notes<b></td><td>: </td><td>" + FollowupNote + "</td></tr></table>";

            sbody = sbody + "<b>This E-mail is automatically sent by SR Log Application.<b></span></span></div></body></html>";
            rcptPrevailingTo = EmailIds;
            MailSend m = new MailSend();
            m.sendMail(rcptPrevailingTo, "Quote Modified Status", sbody, rcptPrevailingCC, false);

        }


        [ExceptionHandler]
        public ActionResult ArchieveRecord(string Id)
        {
            //  ObsoleteBidLogViewModel result = new ObsoleteBidLogViewModel();

            int nId = Convert.ToInt32(Id);
            QuoteLogRepository _repo = new QuoteLogRepository();
            tblObsolete_Quote result = new tblObsolete_Quote();
            SR_Log_DatabaseSQLEntities objdb = new SR_Log_DatabaseSQLEntities();
            var quote = (from b in objdb.tblQuoteLogs
                         where b.Id == nId
                         select b).ToList();

            string UID = quote[0].UID.ToString();
            string bidate = "1/1/1753";
            if (quote[0].BidDate != null)
            {
                bidate = Convert.ToString(quote[0].BidDate);
            }

            result.BidDate = Convert.ToDateTime(bidate);
            result.BiddingAs = quote[0].BiddingAs;
            result.BidTo = quote[0].BidTo;
            result.ProjectName = quote[0].ProjectName;
            result.Estimator = quote[0].Estimator;
            result.QuoteNumber = quote[0].QuoteNumber;
            result.Notes = quote[0].Notes;
            result.EngineersEstimate = quote[0].EngineersEstimate;
            result.Division = quote[0].Division;
            result.UID = quote[0].UID;
            result.RedSheet = quote[0].RedSheet;
            result.ScopeLetter = quote[0].ScopeLetter;
            result.Job_Walk_Date = quote[0].JobWalkDate;
            result.QADeadLineDateTime = quote[0].QADeadLineDateTime;
            result.Mandatory_Job_Walk = (quote[0].MandetoryJobWalk == null ? false : true);
            result.LastAddendumRecvd = quote[0].LastAddendumRecvd;
            result.QuoteStatus = quote[0].QuoteStatus;
            result.dtpLastDateFollowup = quote[0].dtpLastDateFollowup;
            result.LastFollowupBy = quote[0].LastFollowupBy;
            result.FollowupNote = quote[0].FollowupNote;
            result.EmailAddress = quote[0].EmailAddress;

            _repo.AddArchiveQuoteLog(result, "F");
            var act = new ActivityRepository();
            act.AddActivityLog(Convert.ToString(Session["User"]), "Archive Quote", "ArchieveRecord", "Quote  " + quote[0].UID + " archived by user " + Convert.ToString(Session["User"]) + ".");
            foreach (var detail in quote)
            {
                objdb.tblQuoteLogs.Remove(detail);
            }
            objdb.SaveChanges();

            act.AddActivityLog(Convert.ToString(Session["User"]), "Delete Quote", "ArchieveRecord ", " Quote " + UID + " deleted after archiving by user " + Convert.ToString(Session["User"]) + ".");

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result1 = javaScriptSerializer.Serialize(result);
            return Json(result1, JsonRequestBehavior.AllowGet);

        }

        [ExceptionHandler]
        public ActionResult EditQuote(string id)
        {
            SRLogRepository objdata = new SRLogRepository();
            QuoteLogRepository _repos = new QuoteLogRepository();
            int QuoteId = Convert.ToInt32(id);

            QuoteLogViewModel objcre = _repos.GetQuoteRecords(QuoteId);
            objcre.CustomerList = objdata.GetCustomer();
            objcre.GroupUsersList = objdata.GetGroupUser();
            bool bExistEstimator = false;
            foreach (var i in objcre.GroupUsersList)
            {
                if (i.UserName == objcre.Estimator)
                {
                    bExistEstimator = true;
                }
            }
            if (bExistEstimator == false)
            {
                tblGroupUser g = new tblGroupUser();
                g.UserName = objcre.Estimator;
                g.Userid = objcre.Estimator;
                g.Group_Name = "";

                objcre.GroupUsersList.Add(g);
            }
            objcre.emailids = _repos.GetEmailInfo();

            ViewBag.UpdateDisable = false;
            if (Convert.ToString(Session["SR_Log_ReadOnly"]) == "True" && Convert.ToString(Session["Bid_Log_ReadOnly"]) == "True")
            {
                ViewBag.UpdateDisable = true;

            }
            objcre.Id = QuoteId;
            ViewBag.UId = objcre.UID;
            ViewBag.QuoteId = objcre.Id;
            return View("Create", objcre);
        }
    }
}
