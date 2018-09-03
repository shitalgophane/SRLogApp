using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRLog.Entities.BidLog.ViewModels;
using SRLog.Entities.Settings.ViewModels;
using SRLog.Entities.SRLog.ViewModels;
using SRLog.Data;
using SRLog.Data.Account;
using SRLog.Data.BidLog;
using SRLog.Data.SRLog;
using SRLog.Filters;
using SRLog.Entities;
using System.Web.Script.Serialization;
using SRLog.Data.Activity;
using SRLog.Entities.SRLog;
using SRLog.Entities.Account.ViewModels;
using SRLog.Data.Settings;
using System.Data;
using Microsoft.Reporting.WebForms;
using SRLog.Common;
namespace SRLog.Controllers
{
    [AdminFilter]
    public class BidLogController : Controller
    {
        //
        // GET: /BidLog/
        [ExceptionHandler]
        public ActionResult Index()
        {
            UserInfoViewModel userinfo = (UserInfoViewModel)Session["UserInfo"];
            if (userinfo != null)
            {
                SettingsRepository _repository = new SettingsRepository();
                string BiddingAs = _repository.GetSort("Filter_Criteria", "BiddingAsCond", userinfo.UserId);
                if (string.IsNullOrEmpty(BiddingAs))
                {
                    ViewBag.BiddingAs = "";
                }
                else
                {
                    string[] BidAs = BiddingAs.Split('#');
                    string SortByBiddingAs = "";
                    foreach (var i in BidAs)
                    {
                        if (i == "0")
                        {
                            SortByBiddingAs += "I&C" + "|";
                        }
                        if (i == "1")
                        {
                            SortByBiddingAs += "Electrical" + "|";
                        }
                        if (i == "2")
                        {
                            SortByBiddingAs += "Prime" + "|";
                        }
                        if (i == "3")
                        {
                            SortByBiddingAs += "Unknown" + "|";
                        }
                        if (i == "4")
                        {
                            SortByBiddingAs += "Not Bidding" + "|";
                        }
                        if (i == "5")
                        {
                            SortByBiddingAs += "Not Qualified" + "|";
                        }
                        if (i == "6")
                        {
                            SortByBiddingAs += "Mechanical" + "|";
                        }
                    }
                    ViewBag.BiddingAs = SortByBiddingAs;

                }


                string Division = _repository.GetSort("Filter_Criteria", "DivisionCond", userinfo.UserId);
                if (string.IsNullOrEmpty(Division))
                {
                    ViewBag.Division = "";
                }
                else
                {
                    string[] Divs = Division.Split('#');
                    string SortByDivision = "";
                    foreach (var i in Divs)
                    {
                        if (string.IsNullOrEmpty(i) == false)
                        {
                            SortByDivision += i + "|";
                        }
                    }
                    ViewBag.Division = SortByDivision;
                }
            }
            else
                return RedirectToAction("Login", "Account");
            return View();
        }

        //   [HttpPost]
        //   [AllowAnonymous]
        //   public JsonResult GetBidLogsList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        //   {
        //       try
        //       {
        //           BidLogRepository _repository = new BidLogRepository();
        //           var bidCount = _repository.GetBidlogCount();

        //           var bidlogs = _repository.GetBidLogsList(jtStartIndex, jtPageSize, jtSorting);

        //           //  List<tblBID_Log> bidlogs = _repository .GetBidLogs();
        //           return Json(new { Result = "OK", Records = bidlogs, TotalRecordCount = bidCount });
        //       }
        //       catch (Exception ex)
        //       {
        //           return Json(new { Result = "ERROR", Message = ex.Message });
        //       }
        //   }

        //   [HttpGet]
        //   public ActionResult GetBidlogDetails(JQueryDataTableParamModel model)
        //{
        //       //BidLogRepository _repository = new BidLogRepository();
        //       //var bid = _repository.GetBidLogsData();
        //       //var jsonData = bid.Select(x =>
        //       //         new
        //       //         {
        //       //             Id = x.BidId,
        //       //             ProjectName = x.ProjectName,
        //       //             Division = x.Division

        //       //         }).ToArray();
        //       //var result = Json(jsonData, JsonRequestBehavior.AllowGet);
        //       //return result;



        //       //BidLogRepository _repository = new BidLogRepository();
        //       //var bid = _repository.GetBidLogsData();
        //       ////List<tblBID_Log> b = _repository.GetBidLogsData();
        //       ////return Json(b, JsonRequestBehavior.AllowGet);

        //       //var jsonData = new
        //       //{
        //       //    data = bid
        //       //};
        //       //return Json(jsonData, JsonRequestBehavior.AllowGet);


        //       BidLogRepository _repository = new BidLogRepository();
        //       var bid = _repository.GetBidLogsData();
        //       var totalCount = bid.Count();


        //       if (!string.IsNullOrEmpty(model.sSearch))
        //       {

        //           bid = bid.Where(c => (c.BidDate != null && Convert.ToString(Convert.ToDateTime(c.BidDate).ToString("M/d/yyyy")) == (model.sSearch)) ||
        //                  (c.DOW != null && c.DOW.Contains(model.sSearch)) ||
        //                 (c.BiddingAs != null && c.BiddingAs.Contains(model.sSearch)) ||
        //               (c.BidTo != null && c.BidTo.Contains(model.sSearch)) || 
        //                (c.CityState != null && Convert.ToString(c.CityState).Contains(model.sSearch)) ||
        //                        (c.OwnerName != null && c.OwnerName.Contains(model.sSearch)) || 
        //                          (c.ProjectName != null && c.ProjectName.Contains(model.sSearch)) || 
        //                            (c.IAndCEstimate != null && c.IAndCEstimate.Contains(model.sSearch)) || 
        //                               (c.Division != null && c.Division.Contains(model.sSearch)) || 
        //                                (c.LastAddendumRecvd != null && Convert.ToString(c.LastAddendumRecvd).Contains(model.sSearch)) || 
        //                                  (c.Estimator != null && Convert.ToString(c.Estimator).Contains(model.sSearch)) || 
        //                                      (c.AdvertiseDate != null && Convert.ToString(Convert.ToDateTime(c.AdvertiseDate).ToString("M/d/yyyy")) == (model.sSearch)) ||
        //                                           (c.ProjectFolder != null && Convert.ToString(c.ProjectFolder).Contains(model.sSearch)) || 
        //                                             (c.QuoteNumber != null && Convert.ToString(c.QuoteNumber).Contains(model.sSearch))) ;


        //           //bid = bid.Where(c => Convert.ToString(c.BidDate) == (model.sSearch)
        //           //                              || c.DOW.Contains(model.sSearch)
        //           //                              || c.BiddingAs.Contains(model.sSearch)
        //           //                               || c.BidTo.Contains(model.sSearch)
        //           //                                || c.CityState.Contains(model.sSearch)
        //           //                                 || c.OwnerName.Contains(model.sSearch)
        //           //                                  || c.ProjectName.Contains(model.sSearch)
        //           //                                   || c.IAndCEstimate.Contains(model.sSearch)
        //           //                                    || c.Division.Contains(model.sSearch)
        //           //                                     || Convert.ToString(c.LastAddendumRecvd) == (model.sSearch)
        //           //                                           || Convert.ToString(c.Estimator) == (model.sSearch)
        //           //                                           || Convert.ToString(c.AdvertiseDate) == (model.sSearch)
        //           //                                             || c.ProjectFolder.Contains(model.sSearch)
        //           //                              || Convert.ToString(c.QuoteNumber).Contains(model.sSearch));


        //       }

        //       var filteredCount = bid.Count();
        //       if (model.iSortCol_0 != null)
        //       {
        //           var sortColumns = new Dictionary<int, Action>()
        //           {
        //               { 0, () => bid = model.sSortDir_0 == "asc" ? bid.OrderBy(c => c.BidDate) : bid.OrderByDescending(c => c.BidDate) },
        //               { 1, () => bid = model.sSortDir_0 == "asc" ? bid.OrderBy(c => c.DOW) : bid.OrderByDescending(c => c.DOW) },
        //               { 2, () => bid = model.sSortDir_0 == "asc" ? bid.OrderBy(c => c.BiddingAs) : bid.OrderByDescending(c => c.BiddingAs) },
        //               { 3, () => bid = model.sSortDir_0 == "asc" ? bid.OrderBy(c => c.BidTo) : bid.OrderByDescending(c => c.BidTo) },
        //               { 4, () => bid = model.sSortDir_0 == "asc" ? bid.OrderBy(c => c.CityState) : bid.OrderByDescending(c => c.CityState) },
        //               { 5, () => bid = model.sSortDir_0 == "asc" ? bid.OrderBy(c => c.OwnerName) : bid.OrderByDescending(c => c.OwnerName) },
        //               { 6, () => bid = model.sSortDir_0 == "asc" ? bid.OrderBy(c => c.ProjectName) : bid.OrderByDescending(c => c.ProjectName) },
        //               { 7, () => bid = model.sSortDir_0 == "asc" ? bid.OrderBy(c => c.IAndCEstimate) : bid.OrderByDescending(c => c.IAndCEstimate) },
        //               { 8, () => bid = model.sSortDir_0 == "asc" ? bid.OrderBy(c => c.Division) : bid.OrderByDescending(c => c.Division) },
        //               { 9, () => bid = model.sSortDir_0 == "asc" ? bid.OrderBy(c => c.LastAddendumRecvd) : bid.OrderByDescending(c => c.LastAddendumRecvd) },
        //               { 10, () => bid = model.sSortDir_0 == "asc" ? bid.OrderBy(c => c.Estimator) : bid.OrderByDescending(c => c.Estimator) },
        //               { 11, () => bid = model.sSortDir_0 == "asc" ? bid.OrderBy(c => c.AdvertiseDate) : bid.OrderByDescending(c => c.AdvertiseDate) },
        //               { 12, () => bid = model.sSortDir_0 == "asc" ? bid.OrderBy(c => c.ProjectFolder) : bid.OrderByDescending(c => c.ProjectFolder) },
        //               { 13, () => bid = model.sSortDir_0 == "asc" ? bid.OrderBy(c => c.QuoteNumber) : bid.OrderByDescending(c => c.QuoteNumber) }
        //           };

        //           sortColumns[model.iSortCol_0.Value].Invoke();
        //       }


        //       bid = bid.Skip(model.iDisplayStart)
        //                          .Take(model.iDisplayLength);




        //       var result = bid.Select(x =>
        //           new
        //           {
        //               BidDate = x.BidDate,
        //               DOW = x.DOW,
        //               BiddingAs = x.BiddingAs,
        //               BidTo = x.BidTo,
        //               CityState = x.CityState,
        //               OwnerName = x.OwnerName,
        //               ProjectName = x.ProjectName,
        //               IAndCEstimate = x.IAndCEstimate,
        //               Division = x.Division,
        //               LastAddendumRecvd = x.LastAddendumRecvd,
        //               Estimator = x.Estimator,
        //               AdvertiseDate = x.AdvertiseDate,
        //               ProjectFolder = x.ProjectFolder,
        //               QuoteNumber = x.QuoteNumber


        //           }).ToArray();

        //       return Json(new
        //       {
        //           sEcho = model.sEcho,
        //           iTotalRecords = totalCount,
        //           iTotalDisplayRecords = filteredCount,
        //           aaData = result
        //       }, JsonRequestBehavior.AllowGet);


        //   }
        [ExceptionHandler]
        public ActionResult PreviewReport()
        {
            BidLogRepository _repository = new BidLogRepository();
            DataTable dt = _repository.GetBidLogReportDetails();
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.BorderWidth = 0;
            reportViewer.Width = 650;
            reportViewer.Height = 300;
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\BidLog.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));

            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, empName });
            ViewBag.ReportViewer = reportViewer;
            ViewBag.ReportViewerFlag = true;

            return View("Print");
        }

        [ExceptionHandler]
        [HttpGet]
        public ActionResult GetBidlogData(string orderby)
        {
            //BidLogRepository _repository = new BidLogRepository();
            //var bid = _repository.GetBidLogsData();
            //var jsonData = bid.Select(x =>
            //         new
            //         {
            //             Id = x.BidId,
            //             ProjectName = x.ProjectName,
            //             Division = x.Division

            //         }).ToArray();
            //var result = Json(jsonData, JsonRequestBehavior.AllowGet);
            //return result;



            BidLogRepository _repository = new BidLogRepository();
            var bid = _repository.GetBidLogsData(orderby);
            //List<tblBID_Log> b = _repository.GetBidLogsData();
            //return Json(b, JsonRequestBehavior.AllowGet);

            var jsonData = new
            {
                data = bid
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);




        }
        [ExceptionHandler]
        public ActionResult Create()
        {
            BidLogViewModel objcre = new BidLogViewModel();
            SRLogRepository objdata = new SRLogRepository();
            objcre.CustomerList = objdata.GetCustomer();
            objcre.GroupUsersList = objdata.GetGroupUser();
            @ViewBag.SubmitValue = "Add";
            ViewBag.AddDisable = false;
            ViewBag.UpdateDisable = true;
            ViewBag.NewDisable = true;
            ViewBag.PrintDisable = true;
            ViewBag.BidResults = true;
            ViewBag.BidChecklists = true;
            ViewBag.SetPRojectFolder = true;
            ViewBag.CancelDisable = false;
            ViewBag.EditCustomerDisable = true;
            objcre.BidId = 0;



            if (Convert.ToString(Session["Bid_Log_ReadOnly"]) == "True")
            {
                ViewBag.AddDisable = true;
                ViewBag.UpdateDisable = true;
                ViewBag.NewDisable = true;
                ViewBag.BidResults = true;
                ViewBag.BidChecklists = true;
            }

            if (Convert.ToString(Session["SR_Log_ReadOnly"]) == "True" && Convert.ToString(Session["Bid_Log_ReadOnly"]) == "True")
            {
                ViewBag.EditCustomerDisable = true;
            }

            return View(objcre);
        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult Create(BidLogViewModel model, FormCollection form, HttpPostedFileBase postedFile)
        {
            // string s = System.IO.Path.GetFullPath(postedFile.FileName);
            //string sr = Server.MapPath(postedFile.FileName);

            SRLogRepository objdata = new SRLogRepository();
            ModelState.Remove("BidId");

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
                string OwnerName = "";
                if (string.IsNullOrEmpty(form["Owner"]) == false)
                {
                    OwnerName = Convert.ToString(form["Owner"]).ToUpper();
                }
                else
                {
                    OwnerName = Convert.ToString(form["Owner"]);
                }

                string hdnOwnerUpdate = Convert.ToString(form["hdnOwnerUpdate"]);
                if (!string.IsNullOrEmpty(hdnOwnerUpdate))
                {
                    //Add or Update Customer
                    tblCustomer owner = (from cust in objdb1.tblCustomers
                                         where cust.CustomerName == OwnerName
                                         select cust).FirstOrDefault();
                    CommonFunctions c = new CommonFunctions();
                    if (owner == null)
                    {
                        tblCustomer owneradd = new tblCustomer();
                        owneradd.CustomerName = OwnerName;
                        owneradd.DateAdded = c.GetCurrentDate();
                        owneradd.IsInActive = false;
                        owneradd.Notes = null;
                        objdb1.tblCustomers.Add(owneradd);
                        objdb1.SaveChanges();
                    }

                }
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

                //if (Convert.ToString(form["hdnCustomer"]) !="")
                //{
                //    model.Customer = Convert.ToString(form["hdnCustomer"]);
                //}
                if (Convert.ToString(form["hdnOwner"]) != "")
                {
                    model.OwnerName = Convert.ToString(form["hdnOwner"]).ToUpper();
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



                BidLogRepository _repo = new BidLogRepository();
                int BidId = 0;

                if (model.BidId == 0)
                {
                    SR_Log_DatabaseSQLEntities objdb = new SR_Log_DatabaseSQLEntities();
                    var uid_bidlog = objdb.tblBID_Logs.OrderByDescending(u => u.UID).FirstOrDefault();
                    var uid_obsbidlog = objdb.tblObsolete_BID_Logs.OrderByDescending(u => u.UID).FirstOrDefault();
                    int UIDNum;
                    if (uid_bidlog != null)
                    {
                        UIDNum = Convert.ToInt32(uid_bidlog.UID) + 1;
                    }
                    else
                    {
                        UIDNum = 1;
                    }
                    if (uid_bidlog != null && uid_obsbidlog != null)
                    {
                        if (UIDNum < Convert.ToInt32(uid_obsbidlog.UID) + 1)
                        {
                            UIDNum = Convert.ToInt32(uid_obsbidlog.UID) + 1;
                        }
                    }

                    if (string.IsNullOrEmpty(model.BidTo) == false)
                        model.BidTo = model.BidTo.ToUpper();

                    if (string.IsNullOrEmpty(model.OwnerName) == false)
                        model.OwnerName = model.OwnerName.ToUpper();

                    if (string.IsNullOrEmpty(model.ProjectName) == false)
                        model.ProjectName = model.ProjectName.ToUpper();

                    if (string.IsNullOrEmpty(model.CityState) == false)
                        model.CityState = model.CityState.ToUpper();

                    if (string.IsNullOrEmpty(model.Bid_Rev) == false)
                        model.Bid_Rev = model.Bid_Rev.ToUpper();

                    if (string.IsNullOrEmpty(model.IAndCEstimate) == false)
                        model.IAndCEstimate = model.IAndCEstimate.ToUpper();

                    if (string.IsNullOrEmpty(model.PrimeSIEstimate) == false)
                        model.PrimeSIEstimate = model.PrimeSIEstimate.ToUpper();

                    if (string.IsNullOrEmpty(model.EngineersEstimate) == false)
                        model.EngineersEstimate = model.EngineersEstimate.ToUpper();

                    if (string.IsNullOrEmpty(model.Bid_Duration) == false)
                        model.Bid_Duration = model.Bid_Duration.ToUpper();

                    if (string.IsNullOrEmpty(model.Bid_LiqDamage) == false)
                        model.Bid_LiqDamage = model.Bid_LiqDamage.ToUpper();

                    if (string.IsNullOrEmpty(model.Bid_Warranty) == false)
                        model.Bid_Warranty = model.Bid_Warranty.ToUpper();

                    if (string.IsNullOrEmpty(model.Bid_BondingReq) == false)
                        model.Bid_BondingReq = model.Bid_BondingReq.ToUpper();

                    if (string.IsNullOrEmpty(model.Notes) == false)
                        model.Notes = model.Notes.ToUpper();

                    model.UID = UIDNum;

                    BidId = _repo.AddBidLog(model, "A");
                    model.BidId = BidId;
                    ViewBag.UId = UIDNum;
                    ViewBag.BidID = BidId;
                    ViewBag.Message = "Record Added Successfully.";
                    var act = new ActivityRepository();
                    act.AddActivityLog(Convert.ToString(Session["User"]), "Create Bids", "Create", "New bid  " + model.UID + " added by user " + Convert.ToString(Session["User"]) + ".");
                    act.AddActivityLog(Convert.ToString(Session["User"]), "Generate SR", "Create", "Bid " + model.UID + " updated with quote number " + model.QuoteNumber + "by user " + Convert.ToString(Session["User"]) + ".");

                    //Call SaveActivityLog("frmViewCreateBIDS", "Create Bids", "New bid " & Val(txtUIDNum.Text) & " added by user " & UserInfo.UserName & ".")
                    //Call SaveActivityLog("frmViewCreateBIDS", "Generate SR", "Bid " & txtUIDNum.Text & " updated with quote number " + Quotenumber & " by user " & UserInfo.UserName & ".")

                }
                else
                {
                    // model.BidId = Convert.ToInt32(frm["hdnBidID"]);
                    //  model.UID = Convert.ToInt32(frm["hdnUId"]);

                    SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();

                    List<tblBID_Log> bidlog = db.tblBID_Logs.Where(x => x.BidId == model.BidId).ToList();

                    if (bidlog[0].Notes != null)
                    {
                        if (bidlog[0].Notes != "")
                        {
                            if (bidlog[0].Notes != model.Notes)
                            {
                                if (model.Notes != "")
                                {
                                    if (model.Notes != null)
                                    {
                                        model.ModifiedDate = DateTime.Now;
                                    }

                                }
                            }

                        }
                        else if (model.Notes != "")
                        {
                            if (model.Notes != null)
                            {
                                model.ModifiedDate = DateTime.Now;
                            }
                        }
                    }
                    else if (model.Notes != "")
                    {
                        if (model.Notes != null)
                        {
                            model.ModifiedDate = DateTime.Now;
                        }
                    }


                    BidId = _repo.AddBidLog(model, "E");
                    ViewBag.UId = model.UID;
                    ViewBag.BidID = BidId;
                    // Call SaveActivityLog("frmViewCreateBIDS", "Update Bid", "Bid " & Val(txtUIDNum.Text) & " updated by user " & UserInfo.UserName & ".")
                    var act = new ActivityRepository();
                    act.AddActivityLog(Convert.ToString(Session["User"]), "Update Bids", "Create", "Bid " + model.UID + " updated by user " + Convert.ToString(Session["User"]) + ".");

                    ViewBag.Message = "Record Updated Successfully.";
                }

                @ViewBag.SubmitValue = "Update";
                ViewBag.AddDisable = true;
                ViewBag.UpdateDisable = false;
                ViewBag.NewDisable = false;
                ViewBag.PrintDisable = false;
                ViewBag.BidResults = false;
                ViewBag.BidChecklists = false;
                ViewBag.SetPRojectFolder = false;
                ViewBag.CancelDisable = false;
                ViewBag.EditCustomerDisable = false;
                model.CustomerList = objdata.GetCustomer();
                model.GroupUsersList = objdata.GetGroupUser();

                bool bExistEstimator1 = false;
                foreach (var i in model.GroupUsersList)
                {
                    if (i.UserName == model.Estimator)
                    {
                        bExistEstimator1 = true;
                    }
                }
                if (bExistEstimator1 == false)
                {
                    tblGroupUser g = new tblGroupUser();
                    g.UserName = model.Estimator;
                    g.Userid = model.Estimator;
                    g.Group_Name = "";

                    model.GroupUsersList.Add(g);
                }



                return View("Create", model);
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

            return View(model);
        }

        [ExceptionHandler]
        public ActionResult EditBid(string id)
        {
            SRLogRepository objdata = new SRLogRepository();
            BidLogRepository _repos = new BidLogRepository();
            int BidId = Convert.ToInt32(id);

            BidLogViewModel objcre = _repos.GetBidRecords(BidId);
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

            @ViewBag.SubmitValue = "Update";
            ViewBag.AddDisable = true;
            ViewBag.UpdateDisable = false;
            ViewBag.NewDisable = false;
            ViewBag.PrintDisable = false;
            ViewBag.BidResults = false;
            ViewBag.BidChecklists = false;
            ViewBag.SetPRojectFolder = false;
            ViewBag.CancelDisable = false;
            ViewBag.EditCustomerDisable = false;

            objcre.BidId = BidId;
            ViewBag.UId = objcre.UID;
            ViewBag.BidID = objcre.BidId;

            if (Convert.ToString(Session["Bid_Log_ReadOnly"]) == "True")
            {
                ViewBag.AddDisable = true;
                ViewBag.UpdateDisable = true;
                ViewBag.NewDisable = true;
                ViewBag.BidResults = true;
                ViewBag.BidChecklists = true;
            }

            if (Convert.ToString(Session["SR_Log_ReadOnly"]) == "True" && Convert.ToString(Session["Bid_Log_ReadOnly"]) == "True")
            {
                ViewBag.EditCustomerDisable = true;
            }

            return View("Create", objcre);
        }

        [ExceptionHandler]
        public ActionResult GenerateSR(string BidId)
        {
            int nBidId = Convert.ToInt32(BidId);
            SRLogRepository objdata = new SRLogRepository();
            SR_Log_DatabaseSQLEntities objdb = new SR_Log_DatabaseSQLEntities();
            var bid = (from b in objdb.tblBID_Logs
                       where b.BidId == nBidId
                       select b).ToList();

            string BiddingAs = bid[0].BiddingAs;
            string[] arrBiddingAs = BiddingAs.Split('#');
            bool bidIAndC = false;
            bool bidElectrical = false;
            bool bidNoBid = false;
            foreach (string x in arrBiddingAs)
            {

                if (x == "0")
                {
                    bidIAndC = true;
                }


                if (x == "1")
                {
                    bidElectrical = true;
                }



                if (x == "4")
                {
                    bidNoBid = true;
                }


            }

            int LastSrNumber = objdata.GetLastSrNumber();

            SRLogViewModel objsr = new SRLogViewModel();
            objsr.JobOrQuote = "2";
            objsr.PrevailingWageTBD = (bid[0].Bid_PrevailingWage == true ? true : false);
            objsr.PW = (bid[0].Bid_PrevailingWage == true ? true : false);
            objsr.ProjectType = "1";
            objsr.NewCustomer = false;
            objsr.Billing = true;
            objsr.JobsiteAddress = "";
            objsr.QuoteTypeI_C = bidIAndC;
            objsr.QuoteTypeElectrical = bidElectrical;
            objsr.QuoteTypeNoBid = bidNoBid;
            objsr.QuoteDate = (bid[0].BidDate == null ? Convert.ToDateTime("1/1/1753") : bid[0].BidDate);
            objsr.QuoteTime = (bid[0].BidDate == null ? Convert.ToDateTime("1/1/1753") : bid[0].BidDate);
            //objsr.SRNumber = bid[0].QuoteNumber;
            objsr.SRNumber = LastSrNumber;
            if (string.IsNullOrEmpty(bid[0].BidTo) == false)
            {
                objsr.Customer = bid[0].BidTo.ToUpper();
            }
            else
            {
                objsr.Customer = bid[0].BidTo;
            }

            
            if (string.IsNullOrEmpty(bid[0].ProjectName) == false)
            {
                objsr.ProjectDescription = bid[0].ProjectName.ToUpper();
            }
            else
            {
                objsr.ProjectDescription = bid[0].ProjectName;
            }

            objsr.CustomerContact = "NA";
            objsr.ContactPhone = "";
            objsr.ContactEmail = "";
            objsr.Estimator = bid[0].Estimator;
            objsr.Division = bid[0].Division;
            CommonFunctions c = new CommonFunctions();
            objsr.CreationDate = c.GetCurrentDate();
            objsr.QuoteDue = (bid[0].BidDate == null ? Convert.ToDateTime("1/1/1753") : bid[0].BidDate);
            objsr.JobWalkDate = (bid[0].JobWalkDate == null ? Convert.ToDateTime("1/1/1753") : bid[0].JobWalkDate);
            objsr.MandatoryJobWalk = bid[0].MandetoryJobWalk;
            objsr.Bonding = bid[0].Bid_BondingReq;
            objsr.ProjectManager = "";
            if (String.IsNullOrEmpty(bid[0].Notes) == false)
            {
                objsr.Notes = bid[0].Notes.ToUpper();
            }
            else
            {
                objsr.Notes = bid[0].Notes;
            }

            objsr.CreatedBy = Convert.ToString(Session["User"]);
            //   objsr.ContactPhone_Fax_Email=
            objsr.FollowUp = true;
            objsr.InactiveJob = false;
            objsr.QuoteType = bid[0].BiddingAs;
            if (string.IsNullOrEmpty(bid[0].OwnerName) == false)
            {
                objsr.Owner = bid[0].OwnerName.ToUpper();
            }
            else
            {
                objsr.Owner = bid[0].OwnerName;
            }


            
            objsr.AdvertiseDate = (bid[0].AdvertiseDate == null ? Convert.ToDateTime("1/1/1753") : bid[0].AdvertiseDate);
            objsr.NotifyPM = false;
            objsr.ServerJobFolder = "N/A";
            objsr.SiteForeman = "";

            string Quotenumber = LastSrNumber.ToString();
            //Quotenumber = objdata.AddSRLogFromSRGenerated(objsr, "A", BiddingAs);
            objdata.AddSRLogFromSRGenerated(objsr, "A", BiddingAs);

            tblBID_Log updatequote = (from b in objdb.tblBID_Logs
                                      where b.BidId == nBidId
                                      select b).SingleOrDefault();




            updatequote.QuoteNumber = LastSrNumber;
            objdb.SaveChanges();



            string Uid = bid[0].UID.ToString();
            var act = new ActivityRepository();
            act.AddActivityLog(Convert.ToString(Session["User"]), "Generate SR", "GenerateSR", "SR Number " + Quotenumber + " generated from bid " + Uid + " by user " + Convert.ToString(Session["User"]) + ".");

            //Call SaveActivityLog("frmViewCreateBIDS", "Generate SR", "SR Number " + Quotenumber + " generated from bid " & txtUIDNum.Text & " by user " & UserInfo.UserName & ".")
            string EngineersEstimate = bid[0].EngineersEstimate;
            DateTime? QADeadLineDateTime = (bid[0].BidDate == null ? Convert.ToDateTime("1/1/1753") : bid[0].BidDate);
            objsr.SRNumber = Convert.ToDecimal(LastSrNumber);

            if (Quotenumber.Contains("."))
            {
                int index = Quotenumber.IndexOf(".");
                if (index > 0)
                    Quotenumber = Quotenumber.Substring(0, index);
                //nQuotenumber = Quotenumber;
                //model.SRNumber = Convert.ToDecimal(SrNumber);

            }

            objdata.AddQuoteLogFromSRGenetated(objsr, "A", bid[0].LastAddendumRecvd, Convert.ToInt32(Quotenumber), EngineersEstimate, QADeadLineDateTime);

            act.AddActivityLog(Convert.ToString(Session["User"]), "Generate SR", "GenerateSR", "New quote added using SR Number " + Quotenumber + " generated from bid " + Uid + " by user" + Convert.ToString(Session["User"]) + ".");

            //   Call SaveActivityLog("frmViewCreateBIDS", "Generate SR", "New quote added using SR Number " & Quotenumber & " generated from bid " & txtUIDNum.Text & " by user " & UserInfo.UserName & ".")

            if (!string.IsNullOrEmpty(objsr.Bonding))
            {
                SendBondingMailSMTP(objsr.Customer, objsr.ProjectDescription, objsr.Bonding, Convert.ToString(objsr.SRNumber));
            }

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(bid[0].QuoteNumber);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [ExceptionHandler]
        public void SendBondingMailSMTP(string Customer, string ProjectDescription, string Bonding, string SRNumber)
        {
            SettingsRepository setting = new SettingsRepository();
            string rcptBondingTo = "";
            string rcptBondingCC = "";
            string sbody = "";
            List<Bonding_Mail_TO> lstbondingmailto = setting.GetBondingMailTo();
            foreach (var i in lstbondingmailto)
            {
                rcptBondingTo += i.BondingMailTO + ";";
            }
            List<Bonding_Mail_CC> lstbondingmailcc = setting.GetBondingMailCC();
            foreach (var i in lstbondingmailcc)
            {
                rcptBondingCC += i.BondingMailCC + ";";
            }
            sbody = "<html><head><meta http-equiv=Content-Type content='text/html; charset=us-ascii'><meta name=Generator content='Microsoft Word 12 (filtered medium)'></head>";
            sbody = sbody + "<P style='color:#1F497D';><body lang=EN-US link=blue vlink=purple><div class=Section1><p class=MsoNormal><span style='font-size:12pt;font-family:'tahoma, verdana, 'sans-serif'''><span style='color:#1F497D'>";
            sbody = sbody + "<b> The Following Job Requires Bonding :</b><br><br>";
            sbody = sbody + "<table style='font-size:12pt;font-family:'tahoma, verdana, 'sans-serif'''><tr><td><b>SR Number<b></td><td>: </td><td>" + SRNumber + "</td></tr>";
            sbody = sbody + "<tr><td><b>Customer<b></td><td>: </td><td>" + Customer + "</td></tr>";
            sbody = sbody + "<tr><td><b>Project Description<b></td><td>: </td><td>" + ProjectDescription + "</td></tr>";
            sbody = sbody + "<tr><td><b>Bonding Required<b></td><td>: </td><td>" + Bonding + "</td></tr></table>";
            sbody = sbody + "<b>This E-mail is automatically sent by SR Log Database.<b></body></html>";

            MailSend m = new MailSend();
            m.sendMail(rcptBondingTo, "SR Log Database Mail - URGENT -BONDING REQUIREDn", sbody, rcptBondingCC, true);

        }

        [ExceptionHandler]
        public ActionResult ArchieveRecord(string BidId)
        {
            //  ObsoleteBidLogViewModel result = new ObsoleteBidLogViewModel();

            int nBidId = Convert.ToInt32(BidId);
            BidLogRepository _repo = new BidLogRepository();
            tblObsolete_BID_Log result = new tblObsolete_BID_Log();
            SR_Log_DatabaseSQLEntities objdb = new SR_Log_DatabaseSQLEntities();
            var bid = (from b in objdb.tblBID_Logs
                       where b.BidId == nBidId
                       select b).ToList();

            string UID = bid[0].UID.ToString();
            string bidate = "1/1/1753";
            if (bid[0].BidDate != null)
            {
                bidate = Convert.ToString(bid[0].BidDate);
            }

            result.BidDate = Convert.ToDateTime(bidate);


            result.BiddingAs = bid[0].BiddingAs;
            result.BidTo = bid[0].BidTo;
            result.CityState = bid[0].CityState;
            result.ProjectName = bid[0].ProjectName;
            result.Estimator = bid[0].Estimator;
            result.QuoteNumber = bid[0].QuoteNumber;
            result.Notes = bid[0].Notes;
            result.EngineersEstimate = bid[0].EngineersEstimate;
            result.Division = bid[0].Division;
            result.PrimeSIEstimate = bid[0].PrimeSIEstimate;
            result.IAndCEstimate = bid[0].IAndCEstimate;
            result.UID = bid[0].UID;
            result.RedSheet = bid[0].RedSheet;
            result.ScopeLetter = bid[0].ScopeLetter;
            result.Job_Walk_Date = bid[0].JobWalkDate;
            result.Bid_Rev = bid[0].Bid_Rev;
            result.Bid_Duration = bid[0].Bid_Duration;
            result.Bid_Warranty = bid[0].Bid_Warranty;
            result.Bid_LiqDamage = bid[0].Bid_LiqDamage;
            result.Bid_LicenseReqd = bid[0].Bid_LicenseReqd;
            result.Bid_BondingReq = bid[0].Bid_BondingReq;
            result.Bid_Qualified = (bid[0].Bid_Qualified == null ? false : true);
            result.Bid_PrevailingWage = (bid[0].Bid_PrevailingWage == null ? false : true);
            result.Bid_UL508Reqd = (bid[0].Bid_UL508Reqd == null ? false : true);
            result.QADeadLineDateTime = bid[0].QADeadLineDateTime;
            result.ProjectFolder = bid[0].ProjectFolder;
            result.OwnerName = bid[0].OwnerName;
            result.AdvertiseDate = bid[0].AdvertiseDate;
            result.Mandatory_Job_Walk = (bid[0].MandetoryJobWalk == null ? false : true);
            result.LastAddendumRecvd = bid[0].LastAddendumRecvd;

            _repo.AddArchiveBidLog(result);
            var act = new ActivityRepository();
            act.AddActivityLog(Convert.ToString(Session["User"]), "Archive Bid", "ArchieveRecord", "Bid " + bid[0].UID + " archived by user " + Convert.ToString(Session["User"]) + ".");
            foreach (var detail in bid)
            {
                objdb.tblBID_Logs.Remove(detail);
            }
            objdb.SaveChanges();

            act.AddActivityLog(Convert.ToString(Session["User"]), "Delete Bid", "ArchieveRecord ", " Bid " + UID + " deleted after archiving by user " + Convert.ToString(Session["User"]) + ".");



            // Call SaveActivityLog("frmViewCreateBIDS", "Archive Bid", "Bid " & rsObsoleteBid.Fields("UID") & " archived by user " & UserInfo.UserName & ".")                         
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result1 = javaScriptSerializer.Serialize(result);
            return Json(result1, JsonRequestBehavior.AllowGet);

        }

        [ExceptionHandler]
        public ActionResult ActivateBid(string ObsoleteBIDId)
        {
            int nObsoleteBIDId = Convert.ToInt32(ObsoleteBIDId);
            tblBID_Log result = new tblBID_Log();
            SR_Log_DatabaseSQLEntities objdb = new SR_Log_DatabaseSQLEntities();
            var bid = (from b in objdb.tblObsolete_BID_Logs
                       where b.ObsoleteBID_Id == nObsoleteBIDId
                       select b).ToList();

            //Dim rs As New ADODB.Recordset
            //  Dim RS1 As New ADODB.Recordset
            //  If LUID = 0 Then
            //      rs.Open "select max(UID) from [tblBID Log]", cnnSrLogApplication
            //      RS1.Open "select max(UID) from [tblObsolete BID Log]", cnnSrLogApplication
            //      If rs.EOF = False And rs.BOF = False Then
            //          LUID = Val(rs.Fields(0) & "") + 1
            //      Else
            //          LUID = 1
            //      End If
            //      If RS1.EOF = False And RS1.BOF = False Then
            //          If (LUID) < Val((RS1.Fields(0) & "") + 1) Then
            //              LUID = Val((RS1.Fields(0) & "") + 1)
            //          End If
            //      End If
            //      RS1.Close
            //      rs.Close
            //  End If


            var uid_bidlog = objdb.tblBID_Logs.OrderByDescending(u => u.UID).FirstOrDefault();
            var uid_obsbidlog = objdb.tblObsolete_BID_Logs.OrderByDescending(u => u.UID).FirstOrDefault();
            //int UIDNum;
            //if (uid_bidlog != null)
            //{
            //    UIDNum = Convert.ToInt32(uid_bidlog.UID) + 1;
            //}
            //else
            //{
            //    UIDNum = 1;
            //}
            //if (uid_bidlog != null && uid_obsbidlog != null)
            //{
            //    if (UIDNum < Convert.ToInt32(uid_obsbidlog.UID) + 1)
            //    {
            //        UIDNum = Convert.ToInt32(uid_obsbidlog.UID) + 1;
            //    }
            //}

            string bidate = "1/1/1753";
            if (bid[0].BidDate != null)
            {
                bidate = Convert.ToString(bid[0].BidDate);
            }

            result.BidDate = Convert.ToDateTime(bidate);
            result.BiddingAs = bid[0].BiddingAs;
            result.BidTo = bid[0].BidTo;
            result.CityState = bid[0].CityState;
            result.OwnerName = bid[0].OwnerName;
            result.ProjectName = bid[0].ProjectName;
            result.Division = bid[0].Division;
            result.LastAddendumRecvd = bid[0].LastAddendumRecvd;
            result.Estimator = bid[0].Estimator;
            result.QuoteNumber = bid[0].QuoteNumber;
            result.UID = bid[0].UID;
            result.EngineersEstimate = bid[0].EngineersEstimate;
            result.PrimeSIEstimate = bid[0].PrimeSIEstimate;
            result.IAndCEstimate = bid[0].IAndCEstimate;
            result.Notes = bid[0].Notes;
            result.RedSheet = bid[0].RedSheet;
            result.ScopeLetter = bid[0].ScopeLetter;
            result.JobWalkDate = bid[0].Job_Walk_Date;
            result.MandetoryJobWalk = (bid[0].Mandatory_Job_Walk == null ? false : true);
            result.AdvertiseDate = bid[0].AdvertiseDate;
            result.ProjectFolder = bid[0].ProjectFolder;

            BidLogRepository _repo = new BidLogRepository();
            int BidId;
            BidId = _repo.AddActivateBidLog(result);

            var act = new ActivityRepository();
            act.AddActivityLog(Convert.ToString(Session["User"]), "Activate Archived Bid", "Archived Bid ", "Bid " + bid[0].UID.ToString() + " activated by user " + Convert.ToString(Session["User"]) + ".");




            foreach (var detail in bid)
            {
                objdb.tblObsolete_BID_Logs.Remove(detail);
            }
            objdb.SaveChanges();
            act.AddActivityLog(Convert.ToString(Session["User"]), "Delete Archived Bid", "Archived Bid ", "Archived Bid " + bid[0].UID.ToString() + " deleted after moving to current by user " + Convert.ToString(Session["User"]) + ".");

            //result.Bid_Rev = bid[0].Bid_Rev;
            //result.Bid_Duration = bid[0].Bid_Duration;
            //result.Bid_Warranty = bid[0].Bid_Warranty;
            //result.Bid_LiqDamage = bid[0].Bid_LiqDamage;
            //result.Bid_LicenseReqd = bid[0].Bid_LicenseReqd;
            //result.Bid_BondingReq = bid[0].Bid_BondingReq;
            //result.Bid_Qualified = (bid[0].Bid_Qualified == null ? false : true);
            //result.Bid_PrevailingWage = (bid[0].Bid_PrevailingWage == null ? false : true);
            //result.Bid_UL508Reqd = (bid[0].Bid_UL508Reqd == null ? false : true);
            //result.QADeadLineDateTime = bid[0].QADeadLineDateTime;
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result1 = javaScriptSerializer.Serialize(result);
            return Json(result1, JsonRequestBehavior.AllowGet);

        }
        #region BId Results
        [ExceptionHandler]
        [HttpGet]
        public ActionResult GetBidResults(string BidType, string UID)
        {


            BidLogRepository _repository = new BidLogRepository();
            int nBidType = 0;
            if (BidType == "I & C")
            {
                nBidType = 1;
            }
            if (BidType == "Electrical")
            {
                nBidType = 2;
            }
            if (BidType == "Prime")
            {
                nBidType = 3;
            }
            if (BidType == "General")
            {

                nBidType = 4;
            }

            if (UID == "")
            {
                UID = "0";

            }
            int UserId = Convert.ToInt32(Session["UserId"]);
            var bid = _repository.GetBidResults(nBidType, Convert.ToInt16(UID), UserId);

            var jsonData = new
            {
                data = bid
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        public ActionResult CreateBidResults(string UID)
        {
            BidLogRepository _repository = new BidLogRepository();
            BidResultViewModel objcrebidresult = new BidResultViewModel();
            ViewBag.UID = UID;
            int UserId = Convert.ToInt32(Session["UserId"]);
            objcrebidresult = _repository.GetBidResultsRecords(Convert.ToInt32(UID), UserId);
            objcrebidresult.Description = "";
            objcrebidresult.Column1 = "";
            objcrebidresult.Column2 = "";
            objcrebidresult.Column3 = "";
            objcrebidresult.Column4 = "";
            objcrebidresult.Column5 = "";
            objcrebidresult.Column6 = "";
            objcrebidresult.Column7 = "";
            objcrebidresult.Column8 = "";
            objcrebidresult.Column9 = "";
            objcrebidresult.Column10 = "";
            if (objcrebidresult.BidType == 1)
            {
                ViewBag.BidType = "I & C";
            }
            else if (objcrebidresult.BidType == 2)
            {
                ViewBag.BidType = "Electrical";
            }
            else if (objcrebidresult.BidType == 3)
            {
                ViewBag.BidType = "Prime";
            }
            else if (objcrebidresult.BidType == 4)
            {
                ViewBag.BidType = "General";
            }
            else
            {
                ViewBag.BidType = "I & C";
            }
            return View("BidResults", objcrebidresult);
        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult AddBidResults(BidResultViewModel model, FormCollection frm)
        {

            BidLogRepository _repository = new BidLogRepository();
            BidResultViewModel objcrebidresult = new BidResultViewModel();
            if (model.BidStatusLowBldder == true)
            {
                model.BidStatus = 1;

            }
            if (model.BidStatusHighBladder == true)
            {
                model.BidStatus = 2;
            }
            if (model.BidStatusDidNotGet == true)
            {
                model.BidStatus = 3;
            }

            string BidType = Convert.ToString(frm["hdnBidType"]);
            if (BidType == "I & C")
            {
                model.BidType = 1;
            }
            else if (BidType == "Electrical")
            {
                model.BidType = 2;
            }
            else if (BidType == "Prime")
            {
                model.BidType = 3;
            }
            else if (BidType == "General")
            {
                model.BidType = 4;
            }
            int UserId = Convert.ToInt32(Session["UserId"]);
            ViewBag.UID = model.UID;
            model.UserId = UserId;
            _repository.AddBidResults(model);
            ViewBag.BidType = BidType;
            //model.BidStatusLowBldder = false;
            //model.BidStatusHighBladder = false;
            //model.BidStatusDidNotGet = false;
            //model.TelstarQuote = "";
            //model.LowGC ="";
            //model.LowElecSub ="";
            //model.Description = "";
            //model.Column1 = "";
            //    model.Column2="";
            //model.Column3="";
            //model.Column4="";
            //model.Column5="";
            //model.Column6="";
            //model.Column7="";
            //model.Column8="";
            //model.Column9="";

            model.Description = "";
            model.Column1 = "";
            model.Column2 = "";
            model.Column3 = "";
            model.Column4 = "";
            model.Column5 = "";
            model.Column6 = "";
            model.Column7 = "";
            model.Column8 = "";
            model.Column9 = "";
            model.Column10 = "";
            return View("BidResults", model);

        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult DeleteBidResult(string bidResultId)
        {
            int nBidResultID = Convert.ToInt32(bidResultId);
            using (var ctx = new SR_Log_DatabaseSQLEntities())
            {
                var x = (from y in ctx.tblBID_ResultsTemps
                         where y.BIDResults_Id == nBidResultID
                         select y).FirstOrDefault();
                ctx.tblBID_ResultsTemps.Remove(x);
                ctx.SaveChanges();
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        public ActionResult SaveBidResult(string id, string bidtype)
        {


            BidLogRepository _repository = new BidLogRepository();
            _repository.SaveBidResults(Convert.ToInt32(id));
            ViewBag.Message = "BID Results saved successfully.";

            var act = new ActivityRepository();
            act.AddActivityLog(Convert.ToString(Session["User"]), "Delete obsolute bid results", "SaveBidResult", "All bid results for bid " + id + " deleted by " + Convert.ToString(Session["User"]));
            act.AddActivityLog(Convert.ToString(Session["User"]), "Create obsolute bid results", "SaveBidResult", "Bid results for bid " + id + " added by " + Convert.ToString(Session["User"]));

            BidResultViewModel objcrebidresult = new BidResultViewModel();
            ViewBag.UID = id;
            int UserId = Convert.ToInt32(Session["UserId"]);
            objcrebidresult = _repository.GetBidResultsRecords(Convert.ToInt32(id), UserId);

            ViewBag.BidType = (bidtype == "IC" ? "I & C" : bidtype);
            return View("BidResults", objcrebidresult);
        }
        #endregion



        #region Bid Checklist
        [ExceptionHandler]
        public ActionResult CreateBidChecklist(string SRNumber)
        {
            BidLogRepository _repository = new BidLogRepository();

            BidChecklistViewModel objcrebidresult = new BidChecklistViewModel();
            if (SRNumber == "")
            {

                //BidChecklistViewModel objcrebidresult = new BidChecklistViewModel();
                objcrebidresult.PrequalificationRequired = false;
                objcrebidresult.QualificationPacketReceived = false;
                objcrebidresult.BidForm = false;
                objcrebidresult.EquipmentRequiresListing = false;
                objcrebidresult.IsTelstarPrequalified = false;
                objcrebidresult.Map = false;
                objcrebidresult.SubsRequireListing = false;
                objcrebidresult.SpareParts = false;
                objcrebidresult.SRNumber = 0;
                ViewBag.SRNumber = 0;
            }
            else
            {

                objcrebidresult = _repository.GetBidChecklistRecords(Convert.ToInt32(SRNumber));
                if (objcrebidresult != null)
                {
                    ViewBag.SubmitValue = "Update";
                }
                else
                {
                    ViewBag.SubmitValue = "Add";
                }
                ViewBag.SRNumber = Convert.ToInt32(SRNumber);

            }
            return View("BidChecklists", objcrebidresult);
        }


        [ExceptionHandler]
        [HttpPost]
        public ActionResult CreateBidChecklist(BidChecklistViewModel model, FormCollection frm)
        {
            BidLogRepository _repository = new BidLogRepository();
            string SRNumber = model.SRNumber.ToString();
            if (Convert.ToString(frm["hdnSubmitValue"]) == "Add")
            {
                _repository.AddUpdateBidChecklist(model, "A");
                var act = new ActivityRepository();
                act.AddActivityLog(Convert.ToString(Session["User"]), "Create bid checklist", "CreateBidChecklist", "Bid check list for SR Number " + SRNumber + " created by " + Convert.ToString(Session["User"]));

                ViewBag.Message = "Checklist Saved.";
            }
            else if (Convert.ToString(frm["hdnSubmitValue"]) == "Update")
            {
                _repository.AddUpdateBidChecklist(model, "E");
                ViewBag.Message = "Checklist Updated.";
                var act = new ActivityRepository();
                act.AddActivityLog(Convert.ToString(Session["User"]), "Update bid checklist", "CreateBidChecklist", "Bid check list for SR Number " + SRNumber + " updated by " + Convert.ToString(Session["User"]));
            }


            ViewBag.SubmitValue = "Update";
            ViewBag.SRNumber = model.SRNumber;

            return View("BidChecklists", model);
        }

        #endregion


        #region Archive Bid
        [ExceptionHandler]
        public ActionResult IndexArchiveBids()
        {
            return View();
        }

        [ExceptionHandler]
        [HttpGet]
        public ActionResult GetBidlogArchiveData(string orderby)
        {


            BidLogRepository _repository = new BidLogRepository();
            var bid = _repository.GetBidLogsArchiveData(orderby);

            var jsonData = new
            {
                data = bid
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);




        }


        [ExceptionHandler]
        public ActionResult EditArchiveBid(string id)
        {
            SRLogRepository objdata = new SRLogRepository();
            BidLogRepository _repos = new BidLogRepository();
            int BidId = Convert.ToInt32(id);

            ObsoleteBidLogViewModel objcre = _repos.GetArchiveBidRecords(BidId);
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



            ViewBag.SubmitValue = "Update";
            objcre.BidId = BidId;
            ViewBag.UId = objcre.UID;
            ViewBag.BidID = objcre.BidId;
            ViewBag.UpdateDisable = false;

            if (Convert.ToString(Session["Bid_Log_ReadOnly"]) == "True")
            {
                ViewBag.UpdateDisable = true;
            }

            return View("CreateArchiveBids", objcre);
        }


        [ExceptionHandler]
        [HttpPost]
        public ActionResult UpdateArchiveBid(ObsoleteBidLogViewModel model, FormCollection form)
        {

            SRLogRepository objdata = new SRLogRepository();
            ModelState.Remove("BidId");

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
                string OwnerName = "";
                if (string.IsNullOrEmpty(form["Owner"]) == false)
                {
                    OwnerName = Convert.ToString(form["Owner"]).ToUpper();
                }
                else
                {
                    OwnerName = Convert.ToString(form["Owner"]);
                }

                string hdnOwnerUpdate = Convert.ToString(form["hdnOwnerUpdate"]);
                if (!string.IsNullOrEmpty(hdnOwnerUpdate))
                {
                    //Add or Update Customer
                    tblCustomer owner = (from cust in objdb1.tblCustomers
                                         where cust.CustomerName == OwnerName
                                         select cust).FirstOrDefault();
                    CommonFunctions c = new CommonFunctions();
                    if (owner == null)
                    {
                        tblCustomer owneradd = new tblCustomer();
                        owneradd.CustomerName = OwnerName;
                        owneradd.DateAdded = c.GetCurrentDate();
                        owneradd.IsInActive = false;
                        owneradd.Notes = null;
                        objdb1.tblCustomers.Add(owneradd);
                        objdb1.SaveChanges();
                    }

                }
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


                model.BidTo = Convert.ToString(form["hdnCustomer"]);
                //if (Convert.ToString(form["hdnCustomer"]) !="")
                //{
                //    model.Customer = Convert.ToString(form["hdnCustomer"]);
                //}
                if (Convert.ToString(form["hdnOwner"]) != "")
                {
                    model.OwnerName = Convert.ToString(form["hdnOwner"]);
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

                if (string.IsNullOrEmpty(model.BidTo) == false)
                    model.BidTo = model.BidTo.ToUpper();

                if (string.IsNullOrEmpty(model.OwnerName) == false)
                    model.OwnerName = model.OwnerName.ToUpper();

                if (string.IsNullOrEmpty(model.ProjectName) == false)
                    model.ProjectName = model.ProjectName.ToUpper();

                if (string.IsNullOrEmpty(model.CityState) == false)
                    model.CityState = model.CityState.ToUpper();

                if (string.IsNullOrEmpty(model.Bid_Rev) == false)
                    model.Bid_Rev = model.Bid_Rev.ToUpper();

                if (string.IsNullOrEmpty(model.IAndCEstimate) == false)
                    model.IAndCEstimate = model.IAndCEstimate.ToUpper();

                if (string.IsNullOrEmpty(model.PrimeSIEstimate) == false)
                    model.PrimeSIEstimate = model.PrimeSIEstimate.ToUpper();

                if (string.IsNullOrEmpty(model.EngineersEstimate) == false)
                    model.EngineersEstimate = model.EngineersEstimate.ToUpper();

                if (string.IsNullOrEmpty(model.Bid_Duration) == false)
                    model.Bid_Duration = model.Bid_Duration.ToUpper();

                if (string.IsNullOrEmpty(model.Bid_LiqDamage) == false)
                    model.Bid_LiqDamage = model.Bid_LiqDamage.ToUpper();

                if (string.IsNullOrEmpty(model.Bid_Warranty) == false)
                    model.Bid_Warranty = model.Bid_Warranty.ToUpper();

                if (string.IsNullOrEmpty(model.Bid_BondingReq) == false)
                    model.Bid_BondingReq = model.Bid_BondingReq.ToUpper();

                if (string.IsNullOrEmpty(model.Notes) == false)
                    model.Notes = model.Notes.ToUpper();

                BidLogRepository _repo = new BidLogRepository();
                SR_Log_DatabaseSQLEntities objdb = new SR_Log_DatabaseSQLEntities();
                var uid_bidlog = objdb.tblBID_Logs.OrderByDescending(u => u.UID).FirstOrDefault();
                var uid_obsbidlog = objdb.tblObsolete_BID_Logs.OrderByDescending(u => u.UID).FirstOrDefault();
                int UIDNum;
                if (uid_bidlog != null)
                {
                    UIDNum = Convert.ToInt32(uid_bidlog.UID) + 1;
                }
                else
                {
                    UIDNum = 1;
                }
                if (uid_bidlog != null && uid_obsbidlog != null)
                {
                    if (UIDNum < Convert.ToInt32(uid_obsbidlog.UID) + 1)
                    {
                        UIDNum = Convert.ToInt32(uid_obsbidlog.UID) + 1;
                    }
                }

                model.UID = UIDNum;


                int ObsoleteBID_Id;
                ObsoleteBID_Id = _repo.UpdateArchiveBidLog(model);
                ViewBag.UId = model.UID;
                ViewBag.BidID = ObsoleteBID_Id;
                //   Call SaveActivityLog("frmViewArchivedBIDS", "Update Archived Bid", "Archived Bid " & txtUIDNum.Text & " updated by user " & UserInfo.UserName & ".")
                ViewBag.Message = "Record Updated Successfully.";
                // }

                ViewBag.SubmitValue = "Update";
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
                if (Convert.ToString(Session["Bid_Log_ReadOnly"]) == "True")
                {
                    ViewBag.UpdateDisable = true;
                }
                ViewBag.UId = model.UID;
                ViewBag.BidID = model.BidId;
                return View("CreateArchiveBids", model);
            }

            //BidLogViewModel objcre = new BidLogViewModel();
            objdata = new SRLogRepository();
            model.CustomerList = objdata.GetCustomer();
            model.GroupUsersList = objdata.GetGroupUser();
            ViewBag.UpdateDisable = false;
            ViewBag.UId = model.UID;
            ViewBag.BidID = model.BidId;

            if (Convert.ToString(Session["Bid_Log_ReadOnly"]) == "True")
            {
                ViewBag.UpdateDisable = true;
            }
            return View(model);


        }


        #endregion



        #region Archive BId Results
        [ExceptionHandler]
        [HttpGet]
        public ActionResult GetBidArchiveResults(string BidType, string UID)
        {


            BidLogRepository _repository = new BidLogRepository();
            int nBidType = 0;
            if (BidType == "I & C")
            {
                nBidType = 1;
            }
            if (BidType == "Electrical")
            {
                nBidType = 2;
            }
            if (BidType == "Prime")
            {
                nBidType = 3;
            }
            if (BidType == "General")
            {

                nBidType = 4;
            }

            if (UID == "")
            {
                UID = "0";

            }
            int UserId = Convert.ToInt32(Session["UserId"]);
            var bid = _repository.GetArchiveBidResults(nBidType, Convert.ToInt16(UID), UserId);

            var jsonData = new
            {
                data = bid
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        [ExceptionHandler]
        public ActionResult CreateArchiveBidResults(string UID)
        {
            BidLogRepository _repository = new BidLogRepository();
            BidResultViewModel objcrebidresult = new BidResultViewModel();
            ViewBag.UID = UID;
            int UserId = Convert.ToInt32(Session["UserId"]);
            objcrebidresult = _repository.GetArchiveBidResultsRecords(Convert.ToInt32(UID), UserId);
            if (objcrebidresult.BidType == 1)
            {
                ViewBag.BidTypeArchive = "I & C";
            }
            else if (objcrebidresult.BidType == 2)
            {
                ViewBag.BidTypeArchive = "Electrical";
            }
            else if (objcrebidresult.BidType == 3)
            {
                ViewBag.BidTypeArchive = "Prime";
            }
            else if (objcrebidresult.BidType == 4)
            {
                ViewBag.BidTypeArchive = "General";
            }
            else
            {
                ViewBag.BidTypeArchive = "I & C";
            }

            return View("ArchiveBidResults", objcrebidresult);
        }


        [ExceptionHandler]
        [HttpPost]
        public ActionResult AddArchiveBidResults(BidResultViewModel model, FormCollection frm)
        {

            BidLogRepository _repository = new BidLogRepository();
            BidResultViewModel objcrebidresult = new BidResultViewModel();
            if (model.BidStatusLowBldder == true)
            {
                model.BidStatus = 1;

            }
            if (model.BidStatusHighBladder == true)
            {
                model.BidStatus = 2;
            }
            if (model.BidStatusDidNotGet == true)
            {
                model.BidStatus = 3;
            }

            string BidType = Convert.ToString(frm["hdnBidType"]);
            if (BidType == "I & C")
            {
                model.BidType = 1;
            }
            else if (BidType == "Electrical")
            {
                model.BidType = 2;
            }
            else if (BidType == "Prime")
            {
                model.BidType = 3;
            }
            else if (BidType == "General")
            {
                model.BidType = 4;
            }
            int UserId = Convert.ToInt32(Session["UserId"]);
            ViewBag.UID = model.UID;
            ViewBag.BidTypeArchive = BidType;
            model.UserId = UserId;
            _repository.AddArchiveBidResults(model);


            return View("ArchiveBidResults", model);
        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult DeleteArchiveBidResult(string bidResultId)
        {
            int nBidResultID = Convert.ToInt32(bidResultId);
            using (var ctx = new SR_Log_DatabaseSQLEntities())
            {
                var x = (from y in ctx.tblObsoluteBID_Results_Temps
                         where y.ObsoluteBIDResults_Id == nBidResultID
                         select y).FirstOrDefault();
                ctx.tblObsoluteBID_Results_Temps.Remove(x);
                ctx.SaveChanges();
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        public ActionResult SaveArchiveBidResult(string id, string bidType)
        {
            BidLogRepository _repository = new BidLogRepository();
            _repository.SaveArchiveBidResults(Convert.ToInt32(id));
            ViewBag.Message = "BID Results saved successfully.";

            var act = new ActivityRepository();
            act.AddActivityLog(Convert.ToString(Session["User"]), "Delete obsolute bid results", "SaveBidResult", "All bid results for bid " + id + " deleted by " + Convert.ToString(Session["User"]));
            act.AddActivityLog(Convert.ToString(Session["User"]), "Create obsolute bid results", "SaveBidResult", "Bid results for bid " + id + " added by " + Convert.ToString(Session["User"]));

            BidResultViewModel objcrebidresult = new BidResultViewModel();
            ViewBag.UID = id;
            int UserId = Convert.ToInt32(Session["UserId"]);
            objcrebidresult = _repository.GetArchiveBidResultsRecords(Convert.ToInt32(id), UserId);
            ViewBag.BidTypeArchive = (bidType == "IC" ? "I & C" : bidType);
            return View("ArchiveBidResults", objcrebidresult);
        }
        #endregion


        [ExceptionHandler]
        [HttpPost]
        public JsonResult SaveFilterCriteria(string BiddingAs, string ByDivision)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    UserInfoViewModel userinfo = (UserInfoViewModel)Session["UserInfo"];
                    if (userinfo != null)
                    {

                        SettingsRepository _repository = new SettingsRepository();
                        _repository.UpdateSetting(userinfo.UserId, "Filter_Criteria", "BiddingAsCond", BiddingAs);
                        _repository.UpdateSetting(userinfo.UserId, "Filter_Criteria", "DivisionCond", ByDivision);



                        return Json("Filter Criteria order saved successfully.", JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { Result = "ERROR", Message = "Session expired. Please login again" });
                }
                else
                    return Json(new { Result = "ERROR", Message = "Session expired. Please login again" });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

    }
}
