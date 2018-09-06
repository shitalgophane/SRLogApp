using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRLog.Data;
using SRLog.Data.SRLog;
using SRLog.Data.Settings;
using SRLog.Data.Activity;
using SRLog.Entities.Account.ViewModels;
using SRLog.Entities.Settings.ViewModels;
using Newtonsoft.Json;
using SRLog.Filters;
using SRLog.Entities;
using SRLog.Entities.SRLog.ViewModels;
using System.Web.Script.Serialization;
using SRLog.Common;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.IO;

namespace SRLog.Controllers
{
    [AdminFilter]
    public class SRLogController : Controller
    {
        //
        // GET: /SRLog/

        [ExceptionHandler]
        public ActionResult Index(string Flag = "ListAllJobsBySRNumber")
        {
            UserInfoViewModel userinfo = (UserInfoViewModel)Session["UserInfo"];
            if (userinfo != null)
            {
                ViewBag.Flag = Flag;
                SettingsRepository _repository = new SettingsRepository();
                if (Flag == "ListAllJobsBySRNumber")
                {
                    ViewBag.Title = "List All Jobs By SR Number";
                    ViewBag.Sort1 = string.Empty;
                    ViewBag.Sort2 = string.Empty;

                    ViewBag.Order1 = "ASC";
                    ViewBag.Order2 = "ASC";

                    ViewBag.IsDateFilter = false;
                    ViewBag.FromDate = "";
                    ViewBag.ToDate = "";
                }
                else if (Flag == "ListAllJobsByKeywordSearch")
                {
                    ViewBag.Title = "List All Jobs By Keywords";
                    //ViewBag.Sort1 = string.Empty;
                    ViewBag.Sort2 = string.Empty;

                    //ViewBag.Order1 = "ASC";
                    ViewBag.Order2 = "ASC";

                    ViewBag.IsDateFilter = false;
                    //ViewBag.FromDate = System.DateTime.Now.AddMonths(-12).ToString("MM/dd/yyyy");
                    //ViewBag.ToDate = System.DateTime.Now.ToString("MM/dd/yyyy");

                    ViewBag.FromDate = "";
                    ViewBag.ToDate = "";

                    //if (ViewBag.Sort1 == "" && ViewBag.Sort2 == "")
                    //{
                    ViewBag.Sort1 = "SRNumber";
                    ViewBag.Order1 = "DESC";
                    // }

                }
                else if (Flag == "ListAllJobsByFilter")
                {
                    ViewBag.Title = "List All Jobs By Custom Sort";

                    ViewBag.Sort1 = _repository.GetSort("REPORT_BY_SR", "Level1_Field_Name", userinfo.UserId);
                    ViewBag.Sort2 = _repository.GetSort("REPORT_BY_SR", "Level2_Field_Name", userinfo.UserId);

                    ViewBag.Order1 = _repository.GetSort("REPORT_BY_SR", "Level1_Sort_Type", userinfo.UserId);
                    ViewBag.Order2 = _repository.GetSort("REPORT_BY_SR", "Level2_Sort_Type", userinfo.UserId);
                    if (ViewBag.Order1 == "Descending")
                        ViewBag.Order1 = "DESC";

                    if (ViewBag.Order1 == "Ascending")
                        ViewBag.Order1 = "ASC";

                    if (ViewBag.Order2 == "Descending")
                        ViewBag.Order2 = "DESC";

                    if (ViewBag.Order2 == "Ascending")
                        ViewBag.Order2 = "ASC";

                    ViewBag.FromDate = _repository.GetSort("SRJob_Filter_Criteria", "CreationFromDateCond", userinfo.UserId);
                    ViewBag.ToDate = _repository.GetSort("SRJob_Filter_Criteria", "CreationToDateCond", userinfo.UserId);

                    if (string.IsNullOrEmpty(ViewBag.FromDate) == false && string.IsNullOrEmpty(ViewBag.ToDate) == false)
                    {
                        ViewBag.IsDateFilter = true;
                    }
                    else
                        ViewBag.IsDateFilter = false;



                }

                List<tblSRLogColumn> fields = _repository.GetSortableColumnNames(userinfo.UserId);
                tblSRLogColumn temp = new tblSRLogColumn();
                temp.FieldName = "";

                fields.Insert(0, temp);

                ViewBag.FieldList =
          new SelectList((from s in fields
                          select new
                          {
                              FieldName = s.FieldName
                          }),
              "FieldName",
              "FieldName",
              ViewBag.Sort1);

                ViewBag.FieldListTwo =
         new SelectList((from s in fields
                         select new
                         {
                             FieldName = s.FieldName
                         }),
             "FieldName",
             "FieldName",
             ViewBag.Sort2);

                List<OrderByOptions> orderbylist = new List<OrderByOptions>();

                OrderByOptions opt = new OrderByOptions();
                opt.OrderBy = "Ascending";
                opt.Value = "ASC";
                orderbylist.Add(opt);

                opt = new OrderByOptions();
                opt.OrderBy = "Descending";
                opt.Value = "DESC";
                orderbylist.Add(opt);

                ViewBag.OrderList =
          new SelectList((from s in orderbylist
                          select new
                          {
                              OrderBy = s.OrderBy,
                              Value = s.Value
                          }),
              "Value",
              "OrderBy",
              ViewBag.Order1);

                ViewBag.OrderListTwo =
         new SelectList((from s in orderbylist
                         select new
                         {
                             OrderBy = s.OrderBy,
                             Value = s.Value
                         }),
             "Value",
             "OrderBy",
             ViewBag.Order2);

                return View();
            }
            else
                return RedirectToAction("Login", "Account");
        }




        [ExceptionHandler]
        public ActionResult Create(string returnpath)
        {
            ViewBag.ResturnPath = "";
            if (!string.IsNullOrEmpty(returnpath))
            {
                ViewBag.ReturnPath = returnpath;
            }

            SRLogRepository objdata = new SRLogRepository();
            SRLogViewModel objcre = new SRLogViewModel();

            //objcre.Quote = true;
            objcre.CustomerList = objdata.GetCustomer();
            //  objcre.JobsiteAddressList = objdata.GetJobsiteAddress(0);
            objcre.GroupUsersList = objdata.GetGroupUser();
            objcre.GroupUsersListForEstimator = objdata.GetGroupUserForEstimator();
            objcre.CustomerContList = objdata.GetCustomerContact();

            ViewBag.CreatedBy = Convert.ToString(Session["User"]);
            ViewBag.EditedBy = Convert.ToString(Session["User"]);
            //  ViewBag.CreatedDate = System.DateTime.Now.ToString("MM/dd/yyyy");


            CommonFunctions c = new CommonFunctions();
            ViewBag.CreatedDate = c.GetCurrentDate().ToString("MM/dd/yyyy");
            ViewBag.EditedDate = c.GetCurrentDate().ToString("MM/dd/yyyy");


            ViewBag.SubmitValue = "Add";
            //ViewBag.HideButtons = true;
            ViewBag.AddDisable = false;
            ViewBag.UpdateDisable = true;
            ViewBag.NewDisable = true;
            ViewBag.CloseDisable = false;
            ViewBag.PrintDisable = false;
            ViewBag.RegenrateDisable = true;
            ViewBag.FirstDisable = true;
            ViewBag.PreviousDisable = true;
            ViewBag.NextDisable = true;
            ViewBag.LastDisable = true;
            ViewBag.CancelDisable = false;
            ViewBag.EditCustomerDisable = false;

            if (Convert.ToString(Session["Accounting_Rights"]) == "True")
            {
                ViewBag.InActive = false;

            }
            else
            {
                ViewBag.InActive = true;
            }
            ViewBag.PrintView = "";

            //int LastSrNumber = objdata.GetLastSrNumber();
            ViewBag.SRNumber = "";// LastSrNumber;
            ViewBag.NewSRNumber = "";

            return View(objcre);
        }


        //[ExceptionHandler]
        //public ActionResult GoBack(SRLogViewModel model)
        //{
        //    string flag;
        //    if (model.SRNumber != null)
        //    {
        //        flag = "E";
        //        EditSR(model.SRNumber.ToString());
        //    }
        //    else
        //    {
        //        flag = "A";
        //    }
        //    ViewBag.PrintView = "";
        //    return View("Create");
        //}


        [ExceptionHandler]
        [HttpPost]
        public ActionResult Create(SRLogViewModel model, FormCollection form)
        {

            string NewSR = Convert.ToString(form["hdnSRNumber"]);
            string Print = form["hdnPrint"].ToString();
            string returnpath = form["hdnReturnPath"].ToString();

            if (Print == "Print")
            {
                string creationdate = Convert.ToString(form["txtCreationDate"]);
                string editeddate = Convert.ToString(form["txtEditedDate"]);

                if (Convert.ToString(form["hdnSiteForman"]) != "")
                {
                    model.SiteForeman = Convert.ToString(form["hdnSiteForman"]).ToUpper();
                }


                if (Convert.ToString(form["hdnJobSiteAddress"]) != "")
                {
                    model.JobsiteAddress = Convert.ToString(form["hdnJobSiteAddress"]).ToUpper();
                }


                model.Customer = Convert.ToString(form["hdnCustomer"]);
                //if (Convert.ToString(form["hdnCustomer"]) !="")
                //{
                //    model.Customer = Convert.ToString(form["hdnCustomer"]);
                //}
                if (Convert.ToString(form["hdnOwner"]) != "")
                {
                    model.Owner = Convert.ToString(form["hdnOwner"]).ToUpper();
                }

                if (Convert.ToString(form["hdnContactName"]) != "")
                {
                    model.CustomerContact = Convert.ToString(form["hdnContactName"]).ToUpper();
                }


                string AddDisable;
                string UpdateDisable;
                string NewDisable;
                string CloseDisable;
                string PrintDisable;
                string RegenrateDisable;
                string FirstDisable;
                string PreviousDisable;
                string NextDisable;
                string LastDisable;
                string CancelDisable;
                string EditCustomerDisable;
                // if (model.SRNumber != null)
                if (!string.IsNullOrEmpty(NewSR))
                {
                    // flag = "E";

                    AddDisable = "true";
                    UpdateDisable = "false";
                    NewDisable = "false";
                    CloseDisable = "false";
                    PrintDisable = "false";
                    RegenrateDisable = "false";
                    FirstDisable = "false";
                    PreviousDisable = "false";
                    NextDisable = "true";
                    LastDisable = "true";
                    CancelDisable = "true";
                    EditCustomerDisable = "false";
                }
                else
                {
                    //flag = "A";

                    AddDisable = "false";
                    UpdateDisable = "true";
                    NewDisable = "true";
                    CloseDisable = "false";
                    PrintDisable = "false";
                    RegenrateDisable = "true";
                    FirstDisable = "true";
                    PreviousDisable = "true";
                    NextDisable = "true";
                    LastDisable = "true";
                    CancelDisable = "false";
                    EditCustomerDisable = "false";
                }
                string regenratesr = Convert.ToString(form["hdnReSRNumber"]);
                if (regenratesr == "Renegenrate")
                {

                    AddDisable = "false";
                    UpdateDisable = "true";
                    NewDisable = "true";
                    CloseDisable = "false";
                    PrintDisable = "false";
                    RegenrateDisable = "true";
                    FirstDisable = "true";
                    PreviousDisable = "true";
                    NextDisable = "true";
                    LastDisable = "true";
                    CancelDisable = "false";
                    EditCustomerDisable = "false";
                    //flag = "A";
                }


                ReportViewer reportViewer = new ReportViewer();
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.SizeToReportContent = true;
                reportViewer.BorderWidth = 0;
                reportViewer.Width = 650;
                reportViewer.Height = 300;
                reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Report1.rdlc";
                // DataTable dt = new DataTable();

                DataTable dt = new DataTable();

                dt.Columns.Add("Quote");
                dt.Columns.Add("Job");
                dt.Columns.Add("NewCustomerYes");
                dt.Columns.Add("NewCustomerNo");
                dt.Columns.Add("NewCustomer");
                dt.Columns.Add("ProjectType");
                dt.Columns.Add("ProjectTypeLumpType");
                dt.Columns.Add("ProjectTypeTAndM");
                dt.Columns.Add("ProjectTypeTAndMNTE");
                dt.Columns.Add("Division");
                dt.Columns.Add("DivisionConcord");
                dt.Columns.Add("DivisionHanford");

                dt.Columns.Add("DivisionSacramento");
                dt.Columns.Add("FileFolder");
                dt.Columns.Add("ChemFeed");
                dt.Columns.Add("InactiveJob");
                dt.Columns.Add("PW");
                dt.Columns.Add("NotQuoted");
                dt.Columns.Add("Closed");

                dt.Columns.Add("SRNumber");
                dt.Columns.Add("Customer");
                dt.Columns.Add("ProjectDescription");
                dt.Columns.Add("CustomerContact");
                dt.Columns.Add("ContactPhone");

                dt.Columns.Add("ContactEmail");

                dt.Columns.Add("Estimator");
                dt.Columns.Add("CreationDate");
                dt.Columns.Add("EditedDate");
                dt.Columns.Add("QuoteDue");

                dt.Columns.Add("JobWalkDate");
                dt.Columns.Add("MandatoryJobWalk");
                dt.Columns.Add("WhoJobWalk");
                dt.Columns.Add("BidAsPrimeOrSub");
                dt.Columns.Add("Bonding");
                dt.Columns.Add("BondingMailSent");
                dt.Columns.Add("PrevailingMailSent");
                dt.Columns.Add("Notes");
                dt.Columns.Add("ProjectManager");
                dt.Columns.Add("CreatedBy");
                dt.Columns.Add("EditedBy");
                dt.Columns.Add("JobOrQuote");

                dt.Columns.Add("PrevailingWageYes");
                dt.Columns.Add("PrevailingWageNo");
                dt.Columns.Add("PrevailingWageTBD");
                dt.Columns.Add("PrevailingWage");
                dt.Columns.Add("JobsiteAddress");
                dt.Columns.Add("Billing");
                dt.Columns.Add("QuoteDate");
                dt.Columns.Add("QuoteTime");
                dt.Columns.Add("QuoteTypeI_C");
                dt.Columns.Add("QuoteTypeElectrical");
                dt.Columns.Add("QuoteTypeNoBid");
                dt.Columns.Add("FollowUp");
                dt.Columns.Add("QuoteType");
                dt.Columns.Add("Owner");
                dt.Columns.Add("AdvertiseDate");
                dt.Columns.Add("NotifyPM");
                dt.Columns.Add("ServerJobFolder");
                dt.Columns.Add("SiteForeman");
                dt.Columns.Add("Id");
                DataRow dr = dt.NewRow();
                dr["Quote"] = model.Quote;
                dr["Job"] = model.Job;
                dr["NewCustomerYes"] = model.NewCustomerYes;
                dr["NewCustomerNo"] = model.NewCustomerNo;
                dr["NewCustomer"] = model.NewCustomer;
                dr["ProjectType"] = model.ProjectType;
                dr["ProjectTypeLumpType"] = model.ProjectTypeLumpType;
                dr["ProjectTypeTAndM"] = model.ProjectTypeTAndM;
                dr["ProjectTypeTAndMNTE"] = model.ProjectTypeTAndMNTE;
                dr["Division"] = model.Division;
                dr["DivisionConcord"] = model.DivisionConcord;
                dr["DivisionHanford"] = model.DivisionHanford;

                dr["DivisionSacramento"] = model.DivisionSacramento;
                dr["FileFolder"] = model.FileFolder;
                dr["ChemFeed"] = model.ChemFeed;
                dr["InactiveJob"] = model.InactiveJob;
                dr["PW"] = model.PW;
                dr["NotQuoted"] = model.NotQuoted;

                dr["Closed"] = model.Closed;

                dr["SRNumber"] = model.SRNumber;
                if (string.IsNullOrEmpty(model.Customer) == false)
                {
                    dr["Customer"] = model.Customer.ToUpper();
                }
                else
                {
                    dr["Customer"] = model.Customer;
                }

                if (string.IsNullOrEmpty(model.ProjectDescription) == false)
                {
                    dr["ProjectDescription"] = model.ProjectDescription.ToUpper();
                }
                else
                {
                    dr["ProjectDescription"] = model.ProjectDescription;
                }

                if (string.IsNullOrEmpty(model.CustomerContact) == false)
                {
                    dr["CustomerContact"] = model.CustomerContact.ToUpper();
                }
                else
                {
                    dr["CustomerContact"] = model.CustomerContact;
                }

                if (string.IsNullOrEmpty(model.ContactPhone) == false)
                {
                    dr["ContactPhone"] = model.ContactPhone.ToUpper();
                }
                else
                {
                    dr["ContactPhone"] = model.ContactPhone;
                }


                if (string.IsNullOrEmpty(model.ContactEmail) == false)
                {
                    dr["ContactEmail"] = model.ContactEmail.ToUpper();
                }
                else
                {
                    dr["ContactEmail"] = model.ContactEmail;
                }


                dr["Estimator"] = model.Estimator;
                dr["CreationDate"] = creationdate;
                dr["EditedDate"] = editeddate;

                dr["QuoteDue"] = model.QuoteDue;

                dr["JobWalkDate"] = model.JobWalkDate;
                dr["MandatoryJobWalk"] = model.MandatoryJobWalk;
                dr["WhoJobWalk"] = model.WhoJobWalk;
                dr["BidAsPrimeOrSub"] = model.BidAsPrimeOrSub;
                dr["Bonding"] = model.Bonding;
                dr["BondingMailSent"] = model.BondingMailSent;
                dr["PrevailingMailSent"] = model.PrevailingMailSent;
                if (string.IsNullOrEmpty(model.Notes) == false)
                {
                    dr["Notes"] = model.Notes.ToUpper();
                }
                else
                {
                    dr["Notes"] = model.Notes;
                }
                dr["ProjectManager"] = model.ProjectManager;
                dr["CreatedBy"] = Convert.ToString(form["txtCreatedBy"]);
                dr["EditedBy"] = Convert.ToString(form["txtEditedBy"]);
                dr["JobOrQuote"] = model.JobOrQuote;

                dr["PrevailingWageYes"] = model.PrevailingWageYes;
                dr["PrevailingWageNo"] = model.PrevailingWageNo;
                dr["PrevailingWageTBD"] = model.PrevailingWageTBD;
                dr["PrevailingWage"] = model.PrevailingWage;
                dr["JobsiteAddress"] = model.JobsiteAddress;
                dr["Billing"] = model.Billing;
                dr["QuoteDate"] = model.QuoteDate;
                dr["QuoteTime"] = model.QuoteTime;
                dr["QuoteTypeI_C"] = model.QuoteTypeI_C;
                dr["QuoteTypeElectrical"] = model.QuoteTypeElectrical;
                dr["QuoteTypeNoBid"] = model.QuoteTypeNoBid;
                dr["FollowUp"] = model.FollowUp;
                dr["QuoteType"] = model.QuoteType;
                dr["Owner"] = model.Owner;
                dr["AdvertiseDate"] = model.AdvertiseDate;
                dr["NotifyPM"] = model.NotifyPM;
                dr["ServerJobFolder"] = model.ServerJobFolder;
                dr["SiteForeman"] = model.SiteForeman;
                dr["Id"] = model.Id;

                dt.Rows.Add(dr);
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
                ReportParameter rp1 = new ReportParameter("AddDisable", AddDisable);

                ReportParameter rp2 = new ReportParameter("UpdateDisable", UpdateDisable);

                ReportParameter rp3 = new ReportParameter("NewDisable", NewDisable);

                ReportParameter rp4 = new ReportParameter("CloseDisable", CloseDisable);

                ReportParameter rp5 = new ReportParameter("PrintDisable", PrintDisable);

                ReportParameter rp6 = new ReportParameter("RegenrateDisable", RegenrateDisable);

                ReportParameter rp7 = new ReportParameter("FirstDisable", FirstDisable);

                ReportParameter rp8 = new ReportParameter("PreviousDisable", PreviousDisable);
                ReportParameter rp9 = new ReportParameter("NextDisable", NextDisable);
                ReportParameter rp10 = new ReportParameter("LastDisable", LastDisable);
                ReportParameter rp11 = new ReportParameter("CancelDisable", CancelDisable);
                ReportParameter rp12 = new ReportParameter("EditCustomerDisable", EditCustomerDisable);

                //ReportParameter rp2 = new ReportParameter("ToDate", EndDate.ToString());
                //UserViewModel user = _mapper.Map<UserViewModel>(_userService.GetUserById(Convert.ToInt32(form.GetValue("UserList").AttemptedValue)));
                //string Name = (user.FirstName != null ? user.FirstName : "") + " " + (user.LastName != null ? user.LastName : "");
                //ReportParameter empName = new ReportParameter("empName", Name);
                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, rp3, rp4, rp5, rp6, rp7, rp8, rp9, rp10, rp11, rp12 });
                ViewBag.ReportViewer = reportViewer;
                ViewBag.ReportViewerFlag = true;
                
                return View("Print");
            }
            #region validation
            //bool bjob = model.Job;
            //bool bQuote = model.Quote;

            //if (bjob == false && bQuote == false)
            //{
            //    ModelState.AddModelError("JobOrQuote", "Please Select Job or Quote.");
            //    model.JobOrQuote = "";
            //}
            //else
            //{
            //    if (bjob == true)
            //    {
            //        bool bPrevailingWageYes = model.PrevailingWageYes;
            //        bool bPrevailingWageNo = model.PrevailingWageNo;
            //        if (bPrevailingWageYes==false && bPrevailingWageNo==false)
            //        {
            //            ModelState.AddModelError("PrevailingWage", "Please Select Prevailing wage .");
            //        }
            //    }
            //    else if (bQuote == true)
            //    {
            //        bool bPrevailingWageYes = model.PrevailingWageYes;
            //        bool bPrevailingWageNo = model.PrevailingWageNo;
            //        bool bPrevailingWageTBD = model.PrevailingWageTBD;
            //        if (bPrevailingWageYes == false && bPrevailingWageNo == false && bPrevailingWageTBD==false)
            //        {
            //            ModelState.AddModelError("PrevailingWage", "Please Select Prevailing wage .");
            //        }
            //    }
            //}
            //bool bNewCustYes = model.NewCustomerYes;
            //bool bNewCustNo = model.NewCustomerNo;

            //if (bNewCustYes == false && bNewCustNo == false)
            //{
            //    ModelState.AddModelError("NewCustomer", "Please Select Yes or No for New Customer.");
            //}
            //string strJobSiteAddress =Convert.ToString(form["ddlJobSiteAddress"]);

            //if (strJobSiteAddress==null)
            //{
            //    ModelState.AddModelError("JobsiteAddress", "Please Select either Jobsite address or TBD or Various Location.");
            //}

            //bool bDivisionConcord = model.DivisionConcord;
            //bool bDivisionHanford = model.DivisionHanford;
            //bool bDivisionSacramento = model.DivisionSacramento;
            //if (bDivisionConcord == false && bDivisionHanford == false && bDivisionSacramento==false)
            //{
            //    ModelState.AddModelError("Division", "Please select division.");
            //}

            //bool bProjectTypeLumpType = model.ProjectTypeLumpType;
            //bool bProjectTypeTAndM = model.ProjectTypeTAndM;
            //bool bProjectTypeTAndMNTE = model.ProjectTypeTAndMNTE;
            //if (bProjectTypeLumpType == false && bProjectTypeTAndM == false && bProjectTypeTAndMNTE == false)
            //{
            //    ModelState.AddModelError("ProjectType", "Please Select Project Type.");
            //}

            #endregion

            #region Save Customer Details
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();

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

            string Address1 = "";
            if (string.IsNullOrEmpty(form["Address1"]) == false)
            {
                Address1 = Convert.ToString(form["Address1"]).ToUpper();
            }
            else
            {
                Address1 = Convert.ToString(form["Address1"]);
            }

            string City = "";
            if (string.IsNullOrEmpty(form["City"]) == false)
            {
                City = Convert.ToString(form["City"]).ToUpper();
            }
            else
            {
                City = Convert.ToString(form["City"]);
            }

            string State = "";
            if (string.IsNullOrEmpty(form["State"]) == false)
            {
                State = Convert.ToString(form["State"]).ToUpper();
            }
            else
            {
                State = Convert.ToString(form["State"]);
            }

            string ZipCode = "";
            if (string.IsNullOrEmpty(form["ZipCode"]) == false)
            {
                ZipCode = Convert.ToString(form["ZipCode"]).ToUpper();
            }
            else
            {
                ZipCode = Convert.ToString(form["ZipCode"]);
            }

            string txtCustomerCustomerContact = "";
            if (string.IsNullOrEmpty(form["txtCustomerCustomerContact"]) == false)
            {
                txtCustomerCustomerContact = Convert.ToString(form["txtCustomerCustomerContact"]).ToUpper();
            }
            else
            {
                txtCustomerCustomerContact = Convert.ToString(form["txtCustomerCustomerContact"]);
            }

            string txtCustomerContactPhone = "";
            if (string.IsNullOrEmpty(form["txtCustomerContactPhone"]) == false)
            {
                txtCustomerContactPhone = Convert.ToString(form["txtCustomerContactPhone"]).ToUpper();
            }
            else
            {
                txtCustomerContactPhone = Convert.ToString(form["txtCustomerContactPhone"]);
            }

            string txtCustomerContactEmail = "";
            if (string.IsNullOrEmpty(form["txtCustomerContactEmail"]) == false)
            {
                txtCustomerContactEmail = Convert.ToString(form["txtCustomerContactEmail"]).ToUpper();
            }
            else
            {
                txtCustomerContactEmail = Convert.ToString(form["txtCustomerContactEmail"]);
            }

            string OwnerName = "";
            if (string.IsNullOrEmpty(form["OwnerName"]) == false)
            {
                OwnerName = Convert.ToString(form["OwnerName"]).ToUpper();
            }
            else
            {
                OwnerName = Convert.ToString(form["OwnerName"]);
            }

            string hdnOwnerUpdate = Convert.ToString(form["hdnOwnerUpdate"]);
            string CreationDate = Convert.ToString(form["txtCreationDate"]);
            string CreatedBy = Convert.ToString(form["txtCreatedBy"]);
            string EditedDate = Convert.ToString(form["txtEditedDate"]);
            string EditedBy = Convert.ToString(form["txtEditedBy"]);

            if (!string.IsNullOrEmpty(hdnOwnerUpdate))
            {
                //Add or Update Customer
                tblCustomer owner = (from cust in db.tblCustomers
                                     where cust.CustomerName == OwnerName
                                     select cust).FirstOrDefault();
                CommonFunctions c = new CommonFunctions();
                if (owner == null)
                {
                    tblCustomer owneradd = new tblCustomer();
                    owneradd.CustomerName = OwnerName;
                    // owneradd.DateAdded = System.DateTime.Now;
                    owneradd.DateAdded = c.GetCurrentDate();
                    owneradd.IsInActive = false;
                    owneradd.Notes = null;
                    db.tblCustomers.Add(owneradd);
                    db.SaveChanges();
                }

            }
            int nCustmer;
            if (!string.IsNullOrEmpty(hdnCustUpdate))
            {
                //Add or Update Customer
                tblCustomer cu = (from cust in db.tblCustomers
                                  where cust.CustomerName == hdnCustomer
                                  select cust).FirstOrDefault();

                if (cu == null)
                {
                    tblCustomer custad = new tblCustomer();
                    custad.CustomerName = hdnCustomer;
                    CommonFunctions c = new CommonFunctions();
                    custad.DateAdded = c.GetCurrentDate();
                    custad.IsInActive = false;
                    custad.Notes = null;
                    db.tblCustomers.Add(custad);
                    db.SaveChanges();
                    nCustmer = custad.CustomerId;

                }
                else
                {
                    nCustmer = cu.CustomerId;
                }

                if (Address1 != "" && Address1.ToLower() != "tbd" && Address1.ToLower() != "various locations")
                {
                    if (!string.IsNullOrEmpty(City) && !string.IsNullOrEmpty(State) && !string.IsNullOrEmpty(ZipCode))
                    {


                        //Add or Update Customer Address.....................................................
                        tblCustAddress custadd = (from a in db.tblCustAddresses
                                                  where a.Address1 == Address1 && a.CustomerId == nCustmer
                                                  select a).FirstOrDefault();
                        if (custadd == null)
                        {
                            tblCustAddress ca = new tblCustAddress();
                            ca.Address1 = Address1;
                            ca.City = City;
                            ca.State = State;
                            ca.ZipCode = ZipCode;
                            ca.SiteName = null;
                            ca.ProjectManager = null;
                            ca.IsPrimaryAddress = false;
                            ca.CustomerId = nCustmer;
                            CommonFunctions c = new CommonFunctions();
                            ca.DateAdded = c.GetCurrentDate();
                            db.tblCustAddresses.Add(ca);
                            db.SaveChanges();

                        }
                        else
                        {
                            custadd.City = City;
                            custadd.State = State;
                            custadd.ZipCode = ZipCode;
                            db.SaveChanges();
                        }
                    }
                }
                //..............................................................


                //Add or Update Customer Contact.....................................................
                if (txtCustomerCustomerContact != "" && txtCustomerCustomerContact.ToLower() != "tbd")
                {
                    tblCustContact custcontact = (from c in db.tblCustContacts
                                                  where c.CustomerContact == txtCustomerCustomerContact && c.CustomerId == nCustmer
                                                  select c).FirstOrDefault();
                    CommonFunctions com = new CommonFunctions();
                    if (custcontact == null)
                    {
                        tblCustContact c = new tblCustContact();
                        c.CustomerContact = txtCustomerCustomerContact;
                        c.ContactEmail = txtCustomerContactEmail;
                        c.ContactPhone = txtCustomerContactPhone;
                        c.CustomerId = nCustmer;

                        c.DateAdded = com.GetCurrentDate();
                        db.tblCustContacts.Add(c);
                        db.SaveChanges();

                    }
                    else
                    {

                        custcontact.ContactEmail = txtCustomerContactEmail;
                        custcontact.ContactPhone = txtCustomerContactPhone;
                        db.SaveChanges();
                    }
                }
                //..............................................................
            }


            #endregion
            string SrNumber = "";
            string flag = "";
            bool bsentbondingmail = false;
            //model.Customer = Convert.ToString(form["ddlCustomer"]);

            // string jobsite = Convert.ToString(form["hdnJobSiteAddress"]);
            // model.JobsiteAddress = Convert.ToString(form["ddlJobSiteAddress"]);
            //model.CustomerContact = Convert.ToString(form["ddlContactName"]);

            if (Convert.ToString(form["hdnSiteForman"]) != "")
            {
                model.SiteForeman = Convert.ToString(form["hdnSiteForman"]).ToUpper();
            }


            if (Convert.ToString(form["hdnJobSiteAddress"]) != "")
            {
                model.JobsiteAddress = Convert.ToString(form["hdnJobSiteAddress"]).ToUpper();
            }


            model.Customer = Convert.ToString(form["hdnCustomer"]);
            //if (Convert.ToString(form["hdnCustomer"]) !="")
            //{
            //    model.Customer = Convert.ToString(form["hdnCustomer"]);
            //}
            if (Convert.ToString(form["hdnOwner"]) != "")
            {
                model.Owner = Convert.ToString(form["hdnOwner"]).ToUpper();
            }

            if (Convert.ToString(form["hdnContactName"]) != "")
            {
                model.CustomerContact = Convert.ToString(form["hdnContactName"]).ToUpper();
            }


            // string s = ViewBag.Regenerate;

            string strRenerate = Convert.ToString(form["hdnRegenerate"]);

            SRLogRepository objdata = new SRLogRepository();

            if (strRenerate == "Regenerate")
            {
                if (model.SRNumber != null)
                {
                    int LastSrNumber = objdata.GetLastSrNumber();
                    //model.Notes = model.Notes + "This SR Record is regenerated from original SR Number " + model.SRNumber;
                    ViewBag.Notes = "This SR Record is regenerated from original SR Number " + model.SRNumber;
                    model.SRNumber = LastSrNumber;
                    //HD
                    //5 Sept 2018
                    //9/4/18	32	Regenerating an SR record must uncheck the "inactive" field.
                    model.InactiveJob = false;
                    ViewBag.ReSRNumber = "Renegenrate";
                    ViewBag.Message = null;
                    ViewBag.SubmitValue = "Add";
                    ViewBag.HideButtons = true;
                    ViewBag.SRNumber = "";//LastSrNumber;
                    ViewBag.Renegerate = "Renegenrate";
                    ViewBag.AddDisable = false;
                    ViewBag.UpdateDisable = true;
                    ViewBag.NewDisable = true;
                    ViewBag.CloseDisable = false;
                    ViewBag.PrintDisable = false;
                    ViewBag.RegenrateDisable = true;
                    ViewBag.FirstDisable = true;
                    ViewBag.PreviousDisable = true;
                    ViewBag.NextDisable = true;
                    ViewBag.LastDisable = true;
                    ViewBag.CancelDisable = false;
                    ViewBag.EditCustomerDisable = false;
                    ViewBag.CreatedBy = Convert.ToString(Session["User"]);

                    CommonFunctions cm = new CommonFunctions();
                    ViewBag.CreatedDate = cm.GetCurrentDate().ToString("MM/dd/yyyy");
                    ViewBag.EditedBy = Convert.ToString(Session["User"]);
                    ViewBag.EditedDate = cm.GetCurrentDate().ToString("MM/dd/yyyy");
                }
                else
                {
                    ViewBag.Message = "Selected Sr number is empty to generate new sr number";
                    ViewBag.SubmitValue = "Add";
                    ViewBag.HideButtons = true;
                    ViewBag.SRNumber = "";
                    ViewBag.Renegerate = "Renegenrate";
                    ViewBag.AddDisable = false;
                    ViewBag.UpdateDisable = true;
                    ViewBag.NewDisable = true;
                    ViewBag.CloseDisable = false;
                    ViewBag.PrintDisable = false;
                    ViewBag.RegenrateDisable = true;
                    ViewBag.FirstDisable = true;
                    ViewBag.PreviousDisable = true;
                    ViewBag.NextDisable = true;
                    ViewBag.LastDisable = true;
                    ViewBag.CancelDisable = false;
                    ViewBag.EditCustomerDisable = false;
                    ViewBag.CreatedBy = Convert.ToString(Session["User"]);
                    CommonFunctions cm = new CommonFunctions();
                    ViewBag.CreatedDate = cm.GetCurrentDate().ToString("MM/dd/yyyy");
                    ViewBag.EditedBy = Convert.ToString(Session["User"]);
                    ViewBag.EditedDate = cm.GetCurrentDate().ToString("MM/dd/yyyy");
                }
                ViewBag.NewSRNumber = "";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    string hdnReSRNumber = Convert.ToString(form["hdnReSRNumber"]);
                    // if (model.SRNumber != null)
                    if (!string.IsNullOrEmpty(NewSR))
                    {
                        CommonFunctions cm = new CommonFunctions();
                        model.CreatedBy = CreatedBy;
                        model.CreationDate = Convert.ToDateTime(CreationDate);

                        ViewBag.CreatedBy = CreatedBy;
                        ViewBag.CreatedDate = Convert.ToDateTime(CreationDate).ToString("MM/dd/yyyy");

                        model.EditedBy = Convert.ToString(Session["User"]); ;
                        model.EditedDate = cm.GetCurrentDate();

                        ViewBag.EditedBy = Convert.ToString(Session["User"]); ;
                        ViewBag.EditedDate = cm.GetCurrentDate().ToString("MM/dd/yyyy");

                        flag = "E";
                    }
                    else
                    {
                        model.CreatedBy = Convert.ToString(Session["User"]);
                        ViewBag.CreatedBy = Convert.ToString(Session["User"]);

                        CommonFunctions cm = new CommonFunctions();
                        ViewBag.CreatedDate = cm.GetCurrentDate().ToString("MM/dd/yyyy");
                        model.CreationDate = Convert.ToDateTime(CreationDate);

                        model.EditedBy = Convert.ToString(Session["User"]);
                        ViewBag.EditedBy = Convert.ToString(Session["User"]);

                        ViewBag.EditedDate = cm.GetCurrentDate().ToString("MM/dd/yyyy");
                        model.EditedDate = Convert.ToDateTime(EditedDate);

                        flag = "A";
                    }

                    if (string.IsNullOrEmpty(model.Notes) == false)
                        model.Notes = model.Notes.ToUpper();


                    if (string.IsNullOrEmpty(model.Customer) == false)
                        model.Customer = model.Customer.ToUpper();


                    if (string.IsNullOrEmpty(model.ProjectDescription) == false)
                        model.ProjectDescription = model.ProjectDescription.ToUpper();


                    if (string.IsNullOrEmpty(model.CustomerContact) == false)
                        model.CustomerContact = model.CustomerContact.ToUpper();

                    if (string.IsNullOrEmpty(model.ContactEmail) == false)
                        model.ContactEmail = model.ContactEmail.ToUpper();

                    if (string.IsNullOrEmpty(model.Owner) == false)
                        model.Owner = model.Owner.ToUpper();

                    if (hdnReSRNumber == "Renegenrate")
                    {
                        model.CreatedBy = Convert.ToString(Session["User"]);
                        ViewBag.CreatedBy = Convert.ToString(Session["User"]);
                        
                        CommonFunctions cm = new CommonFunctions();
                        ViewBag.CreatedDate = cm.GetCurrentDate().ToString("MM/dd/yyyy");
                        model.EditedDate = Convert.ToDateTime(EditedDate);
                        flag = "A";
                    }
                    SrNumber = objdata.AddSRLog(model, flag);

                    // 'Adding new entry to Quote Log or Absolute Quote Log using Stored procedure
                    if (model.Quote == true)
                    {
                        if (flag == "A")
                        {
                            model.SRNumber = Convert.ToDecimal(SrNumber);
                        }
                        objdata.AddQuoteLog(model, flag);
                    }

                    //Log activity in database
                    var act = new ActivityRepository();
                    if (flag == "A")
                    {

                        act.AddActivityLog(Convert.ToString(Session["User"]), "Create SRLog", "Create", "User " + Convert.ToString(Session["User"]) + " created new SR Number " + SrNumber + ".");
                        if (model.Quote == true)
                        {
                            act.AddActivityLog(Convert.ToString(Session["User"]), "Create Quote", "Create", "New quote added using SR Number " + SrNumber + " by user " + Convert.ToString(Session["User"]) + ".");

                        }
                    }
                    else
                    {
                        SrNumber = Convert.ToString(model.SRNumber);
                        act.AddActivityLog(Convert.ToString(Session["User"]), "Update SRLog", "Create", "User " + Convert.ToString(Session["User"]) + " updated SR Number " + model.SRNumber.ToString() + ".");

                    }


                    if (model.Quote == true)
                    {
                        if (model.PrevailingWageTBD == true)
                        {
                            SendTBDMailSMTP(model.Customer, Convert.ToString(model.SRNumber), model.ProjectDescription);
                            bsentbondingmail = true;
                        }
                    }



                }

                //if (SrNumber.Contains("."))
                //{
                //    int index = SrNumber.IndexOf(".");
                //    if (index > 0)
                //        SrNumber = SrNumber.Substring(0, index);

                //    //model.SRNumber = Convert.ToDecimal(SrNumber);
                //    model.SRNumber = Convert.ToInt32(SrNumber);
                //}
                //else
                //{
                //    model.SRNumber = Convert.ToInt32(SrNumber);
                //}

                if (SrNumber.Contains("."))
                {
                    string[] arrsr = SrNumber.Split('.');

                    int srafterdecimal = Convert.ToInt16(arrsr[1]);
                    if (srafterdecimal > 0)
                    {
                        model.SRNumber = Convert.ToDecimal(SrNumber);
                    }
                    else
                    {
                        int index = SrNumber.IndexOf(".");
                        SrNumber = SrNumber.Substring(0, index);
                        model.SRNumber = Convert.ToInt32(SrNumber);
                    }
                }
                else
                {
                    model.SRNumber = Convert.ToInt32(SrNumber);
                }


                ViewBag.SRNumber = SrNumber;
                if (flag == "A")
                {
                    if (bsentbondingmail == true)
                    {
                        ViewBag.Message = "New SR Number " + SrNumber + " is Added Successfully And Prevailing Wage TBD Mail Sent Succesfully";
                    }
                    else
                    {
                        ViewBag.Message = "New SR Number " + SrNumber + " is Added Successfully.";
                    }


                }
                else
                {
                    if (bsentbondingmail == true)
                    {
                        ViewBag.Message = "Record Updated Successfully And Prevailing Wage TBD Mail Sent Succesfully";
                    }
                    else
                    {
                        ViewBag.Message = "Record Updated Successfully.";
                    }

                }

                ViewBag.SubmitValue = "Update";
                //ViewBag.HideButtons = false;
                ViewBag.AddDisable = true;
                ViewBag.UpdateDisable = false;
                ViewBag.NewDisable = false;
                ViewBag.CloseDisable = false;
                ViewBag.PrintDisable = false;
                ViewBag.RegenrateDisable = false;

                double firstsr = objdata.GetFirstSrNumber();
                double lastsr = objdata.GetMaxSrNumber();

                if (Convert.ToDouble(model.SRNumber) == firstsr)
                {
                    ViewBag.FirstDisable = true;
                    ViewBag.PreviousDisable = true;
                }
                else
                {
                    ViewBag.FirstDisable = false;
                    ViewBag.PreviousDisable = false;
                }

                if (Convert.ToDouble(model.SRNumber) == lastsr)
                {
                    ViewBag.NextDisable = true;
                    ViewBag.LastDisable = true;
                }
                else
                {
                    ViewBag.NextDisable = false;
                    ViewBag.LastDisable = false;
                }



                ViewBag.CancelDisable = true;
                ViewBag.EditCustomerDisable = false;
                ViewBag.NewSRNumber = "Edit";
            }

            if (Convert.ToString(Session["Accounting_Rights"]) == "True")
            {
                ViewBag.InActive = false;

            }
            else
            {
                ViewBag.InActive = true;
            }
            objdata = new SRLogRepository();
            model.CustomerList = objdata.GetCustomer();

            model.GroupUsersList = objdata.GetGroupUser();
            model.GroupUsersListForEstimator = objdata.GetGroupUserForEstimator();
            bool bExistEstimator = false;
            foreach (var i in model.GroupUsersListForEstimator)
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

                model.GroupUsersListForEstimator.Add(g);
            }
            model.CustomerContList = objdata.GetCustomerContact();
            //   model.JobsiteAddressList = objdata.GetJobsiteAddress(Convert.ToInt32(model.Customer));    


            //var customer = objdata.GetCustomer(Convert.ToInt32(model.Customer));
            //string strcustomer = "";
            //foreach (var c in customer)
            //{
            //    strcustomer = c.CustomerName;
            //}
            //ViewBag.CustomerName = strcustomer;

            ViewBag.CustomerName = model.Customer;
            ViewBag.PrintView = "";
            ViewBag.ReturnPath = returnpath;

            return View(model);
        }


        [ExceptionHandler]
        public void SendTBDMailSMTP(string Customer, string SRNumber, string ProjectDescription)
        {
            string rcptBondingTo = "";
            string rcptBondingCC = "";

            SettingsRepository setting = new SettingsRepository();
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
            string sbody = "";
            sbody = "<html><head><meta http-equiv=Content-Type content='text/html; charset=us-ascii'><meta name=Generator content='Microsoft Word 12 (filtered medium)'></head>";
            sbody = sbody + "<P style='color:#1F497D';><body lang=EN-US link=blue vlink=purple><div class=Section1><p class=MsoNormal><span style='font-size:12pt;font-family:'tahoma, verdana, 'sans-serif'''><span style='color:#1F497D'>";
            sbody = sbody + "<b> A quote was created with Prevailing Wage set as TBD </b><br><br>";
            sbody = sbody + "<table style='font-size:12pt;font-family:'tahoma, verdana, 'sans-serif'''><tr><td><b>SR Number<b></td><td>: </td><td>" + SRNumber + "</td></tr>";
            sbody = sbody + "<tr><td><b>Customer Name<b></td><td>: </td><td>" + Customer + "</td></tr>";
            sbody = sbody + "<tr><td><b>Project Description<b></td><td>: </td><td>" + ProjectDescription + "</td></tr>";
            sbody = sbody + "</table>";
            sbody = sbody + "<b>This E-mail is automatically sent by SR Log Database.<b></body></html>";

            MailSend m = new MailSend();
            m.sendMail(rcptBondingTo, "Notification from SR Log Application", sbody, rcptBondingCC, true);
        }


        [ExceptionHandler]
        public ActionResult GetFirst(string returnpath)
        {
            SRLogRepository _repository = new SRLogRepository();
            SRLogViewModel objSRLog = new SRLogViewModel();

            ViewBag.ResturnPath = "";
            if (!string.IsNullOrEmpty(returnpath))
            {
                ViewBag.ReturnPath = returnpath;
            }


            objSRLog = _repository.GetSrRecords("F", 0);
            if (objSRLog.PrevailingWageTBD == true)
            {
                objSRLog.PrevailingWageYes = false;
                objSRLog.PrevailingWageNo = false;

            }
            objSRLog.CustomerList = _repository.GetCustomer();
            objSRLog.GroupUsersList = _repository.GetGroupUser();
            objSRLog.GroupUsersListForEstimator = _repository.GetGroupUserForEstimator();
            if (String.IsNullOrEmpty(objSRLog.CustomerContact) == false)
            {
                if (objSRLog.CustomerContact.ToUpper() == "NA")
                {
                    objSRLog.CustomerContact = "";

                }
            }

            bool bExistEstimator = false;
            foreach (var i in objSRLog.GroupUsersListForEstimator)
            {
                if (i.UserName == objSRLog.Estimator)
                {
                    bExistEstimator = true;
                }
            }
            if (bExistEstimator == false)
            {
                tblGroupUser g = new tblGroupUser();
                g.UserName = objSRLog.Estimator;
                g.Userid = objSRLog.Estimator;
                g.Group_Name = "";

                objSRLog.GroupUsersListForEstimator.Add(g);
            }

            objSRLog.CustomerContList = _repository.GetCustomerContact();
            string SRNumber = Convert.ToString(objSRLog.SRNumber);
            string[] arrsr = SRNumber.Split('.');
            int srafterdecimal = Convert.ToInt16(arrsr[1]);
            if (srafterdecimal > 0)
            {
                ViewBag.SRNumber = objSRLog.SRNumber;
            }
            else
            {
                int index = SRNumber.IndexOf(".");
                SRNumber = SRNumber.Substring(0, index);
                ViewBag.SRNumber = Convert.ToInt32(SRNumber);
            }


            //string SRNumber = Convert.ToString(objSRLog.SRNumber);
            //if (SRNumber.Contains("."))
            //{
            //    int index = SRNumber.IndexOf(".");
            //    if (index > 0)
            //        SRNumber = SRNumber.Substring(0, index);


            //    ViewBag.SRNumber = Convert.ToInt32(SRNumber);

            //}
            //else
            //{
            //    ViewBag.SRNumber = objSRLog.SRNumber;
            //}


            //ViewBag.SRNumber = objSRLog.SRNumber;
            ViewBag.CreatedBy = objSRLog.CreatedBy;
            ViewBag.CreatedDate = Convert.ToDateTime(objSRLog.CreationDate).ToString("MM/dd/yyyy");

            if (objSRLog.EditedBy != null)
            {
                ViewBag.EditedBy = objSRLog.EditedBy;
            }

            if (objSRLog.EditedDate != null)
            {
                ViewBag.EditedDate = Convert.ToDateTime(objSRLog.EditedDate).ToString("MM/dd/yyyy");
            }

            ViewBag.SubmitValue = "Update";
            // ViewBag.HideButtons = false;

            ViewBag.AddDisable = true;
            ViewBag.UpdateDisable = false;
            ViewBag.NewDisable = false;
            ViewBag.CloseDisable = false;
            ViewBag.PrintDisable = false;
            ViewBag.RegenrateDisable = false;
            ViewBag.FirstDisable = true;
            ViewBag.PreviousDisable = true;
            ViewBag.NextDisable = false;
            ViewBag.LastDisable = false;
            ViewBag.CancelDisable = true;
            ViewBag.EditCustomerDisable = false;
            if (Convert.ToString(Session["SR_Log_ReadOnly"]) == "True")
            {
                ViewBag.AddDisable = true;
                ViewBag.UpdateDisable = true;
                ViewBag.NewDisable = true;
                ViewBag.RegenrateDisable = true;
            }
            if (Convert.ToString(Session["Accounting_Rights"]) == "True")
            {
                ViewBag.InActive = false;

            }
            else
            {
                ViewBag.InActive = true;
            }
            if (Convert.ToString(Session["SR_Log_ReadOnly"]) == "True" && Convert.ToString(Session["Bid_Log_ReadOnly"]) == "True")
            {
                ViewBag.EditCustomerDisable = true;
            }


            ViewBag.PrintView = "";
            ViewBag.NewSRNumber = "Edit";
            return View("Create", objSRLog);

        }

        [ExceptionHandler]
        public ActionResult GetLast(string returnpath)
        {
            ViewBag.ResturnPath = "";
            if (!string.IsNullOrEmpty(returnpath))
            {
                ViewBag.ReturnPath = returnpath;
            }

            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            SRLogRepository _repository = new SRLogRepository();
            SRLogViewModel objSRLog = new SRLogViewModel();

            objSRLog = _repository.GetSrRecords("L", 0);

            if (objSRLog.PrevailingWageTBD == true)
            {
                objSRLog.PrevailingWageYes = false;
                objSRLog.PrevailingWageNo = false;

            }
            bool bLastRecord = false;
            var LastRecord = (from l in db.tblSR_Logs
                              select l).OrderByDescending(x => x.SRNumber).Take(1).ToList();
            if (LastRecord[0].SRNumber == objSRLog.SRNumber)
            {
                bLastRecord = true;
            }

            if (string.IsNullOrEmpty(objSRLog.CustomerContact) == false)
            {
                if (objSRLog.CustomerContact.ToUpper() == "NA")
                {
                    objSRLog.CustomerContact = "";

                }
            }

            objSRLog.CustomerList = _repository.GetCustomer();
            objSRLog.GroupUsersList = _repository.GetGroupUser();
            objSRLog.GroupUsersListForEstimator = _repository.GetGroupUserForEstimator();
            bool bExistEstimator = false;
            foreach (var i in objSRLog.GroupUsersListForEstimator)
            {
                if (i.UserName == objSRLog.Estimator)
                {
                    bExistEstimator = true;
                }
            }
            if (bExistEstimator == false)
            {
                tblGroupUser g = new tblGroupUser();
                g.UserName = objSRLog.Estimator;
                g.Userid = objSRLog.Estimator;
                g.Group_Name = "";

                objSRLog.GroupUsersListForEstimator.Add(g);
            }

            objSRLog.CustomerContList = _repository.GetCustomerContact();


            //string SRNumber = Convert.ToString(objSRLog.SRNumber);
            //if (SRNumber.Contains("."))
            //{
            //    int index = SRNumber.IndexOf(".");
            //    if (index > 0)
            //        SRNumber = SRNumber.Substring(0, index);


            //    ViewBag.SRNumber = Convert.ToInt32(SRNumber);
            //}
            //else
            //{
            //    ViewBag.SRNumber = objSRLog.SRNumber;
            //}

            string SRNumber = Convert.ToString(objSRLog.SRNumber);
            string[] arrsr = SRNumber.Split('.');
            int srafterdecimal = Convert.ToInt16(arrsr[1]);
            if (srafterdecimal > 0)
            {
                ViewBag.SRNumber = objSRLog.SRNumber;
            }
            else
            {
                int index = SRNumber.IndexOf(".");
                SRNumber = SRNumber.Substring(0, index);
                ViewBag.SRNumber = Convert.ToInt32(SRNumber);
            }


            //ViewBag.SRNumber = objSRLog.SRNumber;
            ViewBag.CreatedBy = objSRLog.CreatedBy;
            ViewBag.CreatedDate = Convert.ToDateTime(objSRLog.CreationDate).ToString("MM/dd/yyyy");

            if (objSRLog.EditedBy != null)
            {
                ViewBag.EditedBy = objSRLog.EditedBy;
            }

            if (objSRLog.EditedDate != null)
            {
                ViewBag.EditedDate = Convert.ToDateTime(objSRLog.EditedDate).ToString("MM/dd/yyyy");
            }

            ViewBag.SubmitValue = "Update";
            // ViewBag.HideButtons = false;

            ViewBag.AddDisable = true;
            ViewBag.UpdateDisable = false;
            ViewBag.NewDisable = false;
            ViewBag.CloseDisable = false;
            ViewBag.PrintDisable = false;
            ViewBag.RegenrateDisable = false;
            ViewBag.FirstDisable = false;
            ViewBag.PreviousDisable = false;

            if (bLastRecord == true)
            {
                ViewBag.NextDisable = true;
            }
            else
            {
                ViewBag.NextDisable = false;
            }

            ViewBag.LastDisable = true;
            ViewBag.CancelDisable = true;
            ViewBag.EditCustomerDisable = false;
            if (Convert.ToString(Session["SR_Log_ReadOnly"]) == "True")
            {
                ViewBag.AddDisable = true;
                ViewBag.UpdateDisable = true;
                ViewBag.NewDisable = true;
                ViewBag.RegenrateDisable = true;
            }
            if (Convert.ToString(Session["Accounting_Rights"]) == "True")
            {
                ViewBag.InActive = false;

            }
            else
            {
                ViewBag.InActive = true;
            }

            if (Convert.ToString(Session["SR_Log_ReadOnly"]) == "True" && Convert.ToString(Session["Bid_Log_ReadOnly"]) == "True")
            {
                ViewBag.EditCustomerDisable = true;
            }

            ViewBag.PrintView = "";
            ViewBag.NewSRNumber = "Edit";
            return View("Create", objSRLog);

        }

        [ExceptionHandler]
        public ActionResult GetPrevious(string id,string returnpath)
        {
            ViewBag.ResturnPath = "";
            if (!string.IsNullOrEmpty(returnpath))
            {
                ViewBag.ReturnPath = returnpath;
            }


            // string firstid = "0";
            SRLogRepository _repository = new SRLogRepository();
            SRLogViewModel objSRLog = new SRLogViewModel();
            string SRNumber = "";
            if (id == "")
            {
                ViewBag.SRNumber = "";
                ViewBag.Message = "Previous Record not Found";


            }
            else
            {
                SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
                //var first = (from f in db.tblSR_Logs
                //             select f).OrderBy(x => x.SRNumber).Take(1).ToList();
                //firstid=first[0].SRNumber.ToString();
                objSRLog = _repository.GetSrRecords("P", Convert.ToDecimal(id));
                //objSRLog.CustomerList = _repository.GetCustomer();
                //objSRLog.GroupUsersList = _repository.GetGroupUser();
                //objSRLog.CustomerContList = _repository.GetCustomerContact();
                //ViewBag.SRNumber = objSRLog.SRNumber;

                bool bFirstRecord = false;
                var chkFirstRecord = (from f in db.tblSR_Logs
                                      select f).OrderBy(x => x.SRNumber).Take(1).ToList();
                if (chkFirstRecord[0].SRNumber == objSRLog.SRNumber)
                {
                    bFirstRecord = true;
                }
                if (objSRLog.PrevailingWageTBD == true)
                {
                    objSRLog.PrevailingWageYes = false;
                    objSRLog.PrevailingWageNo = false;
                }
                SRNumber = Convert.ToString(objSRLog.SRNumber);

                string[] arrsr = SRNumber.Split('.');
                int srafterdecimal = Convert.ToInt16(arrsr[1]);
                if (srafterdecimal > 0)
                {

                    ViewBag.SRNumber = objSRLog.SRNumber;
                }
                else
                {
                    int index = SRNumber.IndexOf(".");
                    SRNumber = SRNumber.Substring(0, index);
                    ViewBag.SRNumber = Convert.ToInt32(SRNumber);
                }


                //if (SRNumber.Contains("."))
                //{
                //    int index = SRNumber.IndexOf(".");
                //    if (index > 0)
                //        SRNumber = SRNumber.Substring(0, index);


                //    ViewBag.SRNumber = objSRLog.SRNumber;
                //}
                //else
                //{
                //    ViewBag.SRNumber = objSRLog.SRNumber;
                //}
                ViewBag.CreatedBy = objSRLog.CreatedBy;
                ViewBag.CreatedDate = Convert.ToDateTime(objSRLog.CreationDate).ToString("MM/dd/yyyy");

                if (objSRLog.EditedBy != null)
                {
                    ViewBag.EditedBy = objSRLog.EditedBy;
                }

                if (objSRLog.EditedDate != null)
                {
                    ViewBag.EditedDate = Convert.ToDateTime(objSRLog.EditedDate).ToString("MM/dd/yyyy");
                }

                if (bFirstRecord == true)
                {
                    ViewBag.PreviousDisable = true;
                }
                else
                {
                    ViewBag.PreviousDisable = false;
                }
                if (!string.IsNullOrEmpty(objSRLog.CustomerContact))
                {
                    if (objSRLog.CustomerContact.ToUpper() == "NA")
                    {
                        objSRLog.CustomerContact = "";

                    }
                }
            }

            ViewBag.SubmitValue = "Update";

            //  ViewBag.HideButtons = false;


            //ViewBag.PreviousDisable = false;
            ViewBag.AddDisable = true;
            ViewBag.UpdateDisable = false;
            ViewBag.NewDisable = false;
            ViewBag.CloseDisable = false;
            ViewBag.PrintDisable = false;
            ViewBag.RegenrateDisable = false;

            double firstsr = _repository.GetFirstSrNumber();
            double lastsr = _repository.GetMaxSrNumber();

            if (Convert.ToDouble(objSRLog.SRNumber) == firstsr)
            {
                ViewBag.FirstDisable = true;
                ViewBag.PreviousDisable = true;
            }
            else
            {
                ViewBag.FirstDisable = false;
                ViewBag.PreviousDisable = false;
            }

            if (Convert.ToDouble(objSRLog.SRNumber) == lastsr)
            {
                ViewBag.NextDisable = true;
                ViewBag.LastDisable = true;
            }
            else
            {
                ViewBag.NextDisable = false;
                ViewBag.LastDisable = false;
            }

            ViewBag.CancelDisable = true;
            ViewBag.EditCustomerDisable = false;
            objSRLog.CustomerList = _repository.GetCustomer();
            objSRLog.GroupUsersList = _repository.GetGroupUser();
            objSRLog.GroupUsersListForEstimator = _repository.GetGroupUserForEstimator();
            bool bExistEstimator = false;
            foreach (var i in objSRLog.GroupUsersListForEstimator)
            {
                if (i.UserName == objSRLog.Estimator)
                {
                    bExistEstimator = true;
                }
            }
            if (bExistEstimator == false)
            {
                tblGroupUser g = new tblGroupUser();
                g.UserName = objSRLog.Estimator;
                g.Userid = objSRLog.Estimator;
                g.Group_Name = "";

                objSRLog.GroupUsersListForEstimator.Add(g);
            }

            objSRLog.CustomerContList = _repository.GetCustomerContact();
            if (Convert.ToString(Session["SR_Log_ReadOnly"]) == "True")
            {
                ViewBag.AddDisable = true;
                ViewBag.UpdateDisable = true;
                ViewBag.NewDisable = true;
                ViewBag.RegenrateDisable = true;
            }
            if (Convert.ToString(Session["Accounting_Rights"]) == "True")
            {
                ViewBag.InActive = false;

            }
            else
            {
                ViewBag.InActive = true;
            }
                                                                                                                                                                                                                                     
            if (Convert.ToString(Session["SR_Log_ReadOnly"]) == "True" && Convert.ToString(Session["Bid_Log_ReadOnly"]) == "True")
            {
                ViewBag.EditCustomerDisable = true;
            }

            ViewBag.PrintView = "";
            ViewBag.NewSRNumber = "Edit";
            return View("Create", objSRLog);

        }
        public ActionResult GetNext(string id,string returnpath)
        {
            ViewBag.ResturnPath = "";
            if (!string.IsNullOrEmpty(returnpath))
            {
                ViewBag.ReturnPath = returnpath;
            }

            SRLogRepository _repository = new SRLogRepository();
            SRLogViewModel objSRLog = new SRLogViewModel();
            string SRNumber = "";
            if (id == "")
            {
                ViewBag.SRNumber = "";
                ViewBag.Message = "Next Record not Found";

            }
            else
            {
                objSRLog = _repository.GetSrRecords("N", Convert.ToDecimal(id));

                if (objSRLog.PrevailingWageTBD == true)
                {
                    objSRLog.PrevailingWageYes = false;
                    objSRLog.PrevailingWageNo = false;

                }

                //objSRLog.CustomerList = _repository.GetCustomer();
                //objSRLog.GroupUsersList = _repository.GetGroupUser();
                //objSRLog.CustomerContList = _repository.GetCustomerContact();
                //ViewBag.SRNumber = objSRLog.SRNumber;

                //string SRNumber = Convert.ToString(objSRLog.SRNumber);
                //if (SRNumber.Contains("."))
                //{
                //    int index = SRNumber.IndexOf(".");
                //    if (index > 0)
                //        SRNumber = SRNumber.Substring(0, index);


                //    ViewBag.SRNumber = Convert.ToInt32(SRNumber);
                //}
                //else
                //{
                //    ViewBag.SRNumber = objSRLog.SRNumber;
                //}
                SRNumber = Convert.ToString(objSRLog.SRNumber);
                string[] arrsr = SRNumber.Split('.');
                int srafterdecimal = Convert.ToInt16(arrsr[1]);
                if (srafterdecimal > 0)
                {
                    ViewBag.SRNumber = objSRLog.SRNumber;
                }
                else
                {
                    int index = SRNumber.IndexOf(".");
                    SRNumber = SRNumber.Substring(0, index);
                    ViewBag.SRNumber = Convert.ToInt32(SRNumber);
                }


                ViewBag.CreatedBy = objSRLog.CreatedBy;
                ViewBag.CreatedDate = Convert.ToDateTime(objSRLog.CreationDate).ToString("MM/dd/yyyy");

                if (objSRLog.EditedBy != null)
                {
                    ViewBag.EditedBy = objSRLog.EditedBy;
                }

                if (objSRLog.EditedDate != null)
                {
                    ViewBag.EditedDate = Convert.ToDateTime(objSRLog.EditedDate).ToString("MM/dd/yyyy");
                }

                if (string.IsNullOrEmpty(objSRLog.CustomerContact) == false)
                {
                    if (objSRLog.CustomerContact.ToUpper() == "NA")
                    {
                        objSRLog.CustomerContact = "";

                    }
                }
            }

            objSRLog.CustomerList = _repository.GetCustomer();
            objSRLog.GroupUsersList = _repository.GetGroupUser();
            objSRLog.GroupUsersListForEstimator = _repository.GetGroupUserForEstimator();

            bool bExistEstimator = false;
            foreach (var i in objSRLog.GroupUsersListForEstimator)
            {
                if (i.UserName == objSRLog.Estimator)
                {
                    bExistEstimator = true;
                }
            }
            if (bExistEstimator == false)
            {
                tblGroupUser g = new tblGroupUser();
                g.UserName = objSRLog.Estimator;
                g.Userid = objSRLog.Estimator;
                g.Group_Name = "";

                objSRLog.GroupUsersListForEstimator.Add(g);
            }

            objSRLog.CustomerContList = _repository.GetCustomerContact();
            ViewBag.SubmitValue = "Update";
            //ViewBag.HideButtons = false;


            ViewBag.AddDisable = true;
            ViewBag.UpdateDisable = false;
            ViewBag.NewDisable = false;
            ViewBag.CloseDisable = false;
            ViewBag.PrintDisable = false;
            ViewBag.RegenrateDisable = false;
            double firstsr = _repository.GetFirstSrNumber();
            double lastsr = _repository.GetMaxSrNumber();

            if (Convert.ToDouble(objSRLog.SRNumber) == firstsr)
            {
                ViewBag.FirstDisable = true;
                ViewBag.PreviousDisable = true;
            }
            else
            {
                ViewBag.FirstDisable = false;
                ViewBag.PreviousDisable = false;
            }

            if (Convert.ToDouble(objSRLog.SRNumber) == lastsr)
            {
                ViewBag.NextDisable = true;
                ViewBag.LastDisable = true;
            }
            else
            {
                ViewBag.NextDisable = false;
                ViewBag.LastDisable = false;
            }

            ViewBag.CancelDisable = true;
            ViewBag.EditCustomerDisable = false;
            objSRLog.CustomerList = _repository.GetCustomer();
            objSRLog.GroupUsersList = _repository.GetGroupUser();
            objSRLog.CustomerContList = _repository.GetCustomerContact();

            if (Convert.ToString(Session["SR_Log_ReadOnly"]) == "True")
            {
                ViewBag.AddDisable = true;
                ViewBag.UpdateDisable = true;
                ViewBag.NewDisable = true;
                ViewBag.RegenrateDisable = true;
            }
            if (Convert.ToString(Session["Accounting_Rights"]) == "True")
            {
                ViewBag.InActive = false;

            }
            else
            {
                ViewBag.InActive = true;
            }

            if (Convert.ToString(Session["SR_Log_ReadOnly"]) == "True" && Convert.ToString(Session["Bid_Log_ReadOnly"]) == "True")
            {
                ViewBag.EditCustomerDisable = true;
            }

            ViewBag.PrintView = "";
            ViewBag.NewSRNumber = "Edit";
            return View("Create", objSRLog);

        }


        [ExceptionHandler]
        public ActionResult EditSR(string id)
        {
            decimal SRNo = 0;
            ViewBag.ResturnPath = "";
            if (!string.IsNullOrEmpty(id))
            {
                string[] sr = id.Split('^');
                SRNo = Convert.ToDecimal(sr[0]);
                ViewBag.ReturnPath = (sr.Length == 2 ? sr[1] : "");

            }

            //  decimal SRNo = Convert.ToDecimal(id);

            SRLogRepository _repository = new SRLogRepository();
            SRLogViewModel objSRLog = new SRLogViewModel();

            objSRLog = _repository.GetSrRecords("S", SRNo);
            if (objSRLog.PrevailingWageTBD == true)
            {
                objSRLog.PrevailingWageYes = false;
                objSRLog.PrevailingWageNo = false;

            }
            objSRLog.CustomerList = _repository.GetCustomer();
            objSRLog.GroupUsersList = _repository.GetGroupUser();
            objSRLog.GroupUsersListForEstimator = _repository.GetGroupUserForEstimator();
            if (string.IsNullOrEmpty(objSRLog.Customer) == false)
            {
                if (objSRLog.CustomerContact.ToUpper() == "NA")
                {
                    objSRLog.CustomerContact = "";
                }
            }
            else
            {
                objSRLog.CustomerContact = "";
            }

            bool bExistEstimator = false;
            foreach (var i in objSRLog.GroupUsersListForEstimator)
            {
                if (i.UserName == objSRLog.Estimator)
                {
                    bExistEstimator = true;
                }
            }
            if (bExistEstimator == false)
            {
                tblGroupUser g = new tblGroupUser();
                g.UserName = objSRLog.Estimator;
                g.Userid = objSRLog.Estimator;
                g.Group_Name = "";

                objSRLog.GroupUsersListForEstimator.Add(g);
            }


            objSRLog.CustomerContList = _repository.GetCustomerContact();


            //string SRNumber = Convert.ToString(objSRLog.SRNumber);
            //if (SRNumber.Contains("."))
            //{
            //    int index = SRNumber.IndexOf(".");
            //    if (index > 0)
            //        SRNumber = SRNumber.Substring(0, index);


            //    ViewBag.SRNumber = Convert.ToInt32(SRNumber);
            //}
            //else
            //{
            //    ViewBag.SRNumber = objSRLog.SRNumber;
            //}
            string SRNumber = Convert.ToString(objSRLog.SRNumber);
            string[] arrsr = SRNumber.Split('.');
            int srafterdecimal = Convert.ToInt16(arrsr[1]);
            if (srafterdecimal > 0)
            {
                ViewBag.SRNumber = objSRLog.SRNumber;
            }
            else
            {
                int index = SRNumber.IndexOf(".");
                SRNumber = SRNumber.Substring(0, index);
                ViewBag.SRNumber = Convert.ToInt32(SRNumber);
            }


            CommonFunctions c = new CommonFunctions();
            //ViewBag.SRNumber = objSRLog.SRNumber;
            ViewBag.CreatedBy = objSRLog.CreatedBy;
            ViewBag.CreatedDate = Convert.ToDateTime(objSRLog.CreationDate).ToString("MM/dd/yyyy");

            if (objSRLog.EditedBy != null)
            {
                ViewBag.EditedBy = objSRLog.EditedBy;
            }

            if (objSRLog.EditedDate != null)
            {
                ViewBag.EditedDate = Convert.ToDateTime(objSRLog.EditedDate).ToString("MM/dd/yyyy");
            }

            ViewBag.SubmitValue = "Update";
            // ViewBag.HideButtons = false;


            ViewBag.AddDisable = true;
            ViewBag.UpdateDisable = false;
            ViewBag.NewDisable = false;
            ViewBag.CloseDisable = false;
            ViewBag.PrintDisable = false;
            ViewBag.RegenrateDisable = false;

            double firstsr = _repository.GetFirstSrNumber();
            double lastsr = _repository.GetMaxSrNumber();
            if (Convert.ToDouble(objSRLog.SRNumber) == firstsr)
            {
                ViewBag.FirstDisable = true;
                ViewBag.PreviousDisable = true;
            }
            else
            {
                ViewBag.FirstDisable = false;
                ViewBag.PreviousDisable = false;
            }
            if (Convert.ToDouble(objSRLog.SRNumber) == lastsr)
            {
                ViewBag.NextDisable = true;
                ViewBag.LastDisable = true;
            }
            else
            {
                ViewBag.NextDisable = false;
                ViewBag.LastDisable = false;
            }
            ViewBag.CancelDisable = true;
            ViewBag.EditCustomerDisable = false;
            if (Convert.ToString(Session["SR_Log_ReadOnly"]) == "True")
            {
                ViewBag.AddDisable = true;
                ViewBag.UpdateDisable = true;
                ViewBag.NewDisable = true;
                ViewBag.RegenrateDisable = true;
            }
            if (Convert.ToString(Session["Accounting_Rights"]) == "True")
            {
                ViewBag.InActive = false;

            }
            else
            {
                ViewBag.InActive = true;
            }
            if (Convert.ToString(Session["SR_Log_ReadOnly"]) == "True" && Convert.ToString(Session["Bid_Log_ReadOnly"]) == "True")
            {
                ViewBag.EditCustomerDisable = true;
            }

            ViewBag.PrintView = "";
            ViewBag.NewSRNumber = "Edit";
            return View("Create", objSRLog);
        }


        [ExceptionHandler]
        [HttpPost]
        public ActionResult GetCustomer()
        {
            SRLogRepository _repository = new SRLogRepository();
            var lstCust = _repository.GetCustomer();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstCust);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult GetJobSiteAddress(string customerid)
        {
            SRLogRepository _repository = new SRLogRepository();
            var lstCustAdd = _repository.GetJobsiteAddress(Convert.ToInt32(customerid)).OrderBy(a => a.IsPrimaryAddress != true);
            // var lstCustAdd = _repository.GetJobsiteAddress(Convert.ToInt32(customerid)).OrderBy(a => a.IsPrimaryAddress != true);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstCustAdd);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult GetNote(string customerid)
        {
            SRLogRepository _repository = new SRLogRepository();
            var lstcustomer = _repository.GetCustomer(Convert.ToInt32(customerid));
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstcustomer);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult GetCustomerContact(string customerid)
        {
            SRLogRepository _repository = new SRLogRepository();
            var lstCustConact = _repository.GetCustomerContact(Convert.ToInt32(customerid)).OrderBy(a => a.IsPrimaryContact != true);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstCustConact);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult GetJobSiteAddressWithName(string customerid)
        {
            SRLogRepository _repository = new SRLogRepository();
            var lstCustAdd = _repository.GetJobsiteAddressWithName(customerid).OrderBy(a => a.IsPrimaryAddress != true);

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstCustAdd);
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        [ExceptionHandler]
        [HttpPost]
        public ActionResult GetCustomerContactPhoneAndEmail(string customerid, string contactid)
        {
            SRLogRepository _repository = new SRLogRepository();
            var lstCustConact = _repository.GetCustomerContactEmailWithName(customerid, contactid);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstCustConact);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult GetCustomerContactDetails(string customerid, string CustomerContact, string phone, string email)
        {
            SRLogRepository _repository = new SRLogRepository();
            SR_Log_DatabaseSQLEntities db = new SR_Log_DatabaseSQLEntities();
            var cust = (from c in db.tblCustomers
                        where c.CustomerName == customerid
                        select c).ToList();
            List<string> lstResult = new List<string>();
            if (cust.Count > 0)
            {
                var lstCustConact = _repository.GetCustomerContactWithName(customerid).OrderBy(a => a.IsPrimaryContact != true);

                foreach (var i in lstCustConact)
                {
                    if (i.CustomerContact == CustomerContact)
                    {
                        if (i.ContactPhone == phone)
                        {
                            lstResult.Add("ExistPhone");
                        }
                        if (i.ContactEmail.ToUpper() == email.ToUpper())
                        {
                            lstResult.Add("ExistEmail");
                        }
                    }
                }
                if (!lstResult.Contains("ExistPhone"))
                {
                    lstResult.Add("NotExistPhone");

                }
                if (!lstResult.Contains("ExistEmail"))
                {
                    lstResult.Add("NotExistEmail");
                }
            }
            else
            {
                lstResult.Add("NotExistPhone");
                lstResult.Add("NotExistEmail");
            }
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            //    string result = javaScriptSerializer.Serialize(lstResult);
            return Json(lstResult, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        [HttpPost]
        public ActionResult GetCustomerContactWithName(string customerid)
        {
            SRLogRepository _repository = new SRLogRepository();
            var lstCustConact = _repository.GetCustomerContactWithName(customerid).OrderBy(a => a.IsPrimaryContact != true);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstCustConact);
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        [ExceptionHandler]
        [HttpPost]
        public ActionResult GetAddressDetails(string CustomerName, string CustomerAddress)
        {
            SRLogRepository _repository = new SRLogRepository();

            bool existaddress = _repository.GetAddressDetails(CustomerName, CustomerAddress);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(existaddress);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public ActionResult GetPM(string jobsiteAddressid, string customerid)
        //{
        //    SRLogRepository _repository = new SRLogRepository();
        //    var lstPM = _repository.GetPM(Convert.ToInt32(jobsiteAddressid), Convert.ToInt32(customerid));
        //    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        //    string result = javaScriptSerializer.Serialize(lstPM);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        [ExceptionHandler]
        [HttpPost]
        public JsonResult GetSRLogsList(string keyword = "", string sortby1 = "", string sortby2 = "", string FromDate = "", string ToDate = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                JsonResult result = new JsonResult();
                string search = Request.Form.GetValues("search[value]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
                string draw = Request.Form.GetValues("draw")[0];
                //if (startRec >= pageSize)
                //{
                //    startRec = 0;
                //}

                if (Session["UserInfo"] != null)
                {
                    UserInfoViewModel userinfo = (UserInfoViewModel)Session["UserInfo"];
                    SRLogRepository _repository = new SRLogRepository();
                    if (userinfo != null)
                    {

                        if (keyword == "")
                        {
                            var srCount = _repository.GetSRLogcount(FromDate, ToDate);
                            if (startRec > srCount)
                            {
                                startRec = 0;
                            }
                            //jtStartIndex = 0;
                            //jtPageSize = 500;

                            //if (FromDate == "")
                            //{
                            //    DateTime today = DateTime.Now;
                            //    DateTime sixMonthsBack = today.AddYears(-2);
                            //    FromDate = sixMonthsBack.ToString();
                            //    ToDate = today.ToString();
                            //}


                            DateTime start = DateTime.Now;

                            var srlogs = _repository.GetSRLogsList(userinfo.UserId, sortby1, sortby2, FromDate, ToDate, startRec, pageSize, jtSorting);

                            DateTime end = DateTime.Now;
                            TimeSpan span = new TimeSpan();
                            span = end.Subtract(start);
                            EventLog.LogData("Total time to get records from database " + span.TotalSeconds.ToString(), true);


                            //  List<tblBID_Log> bidlogs = _repository .GetBidLogs();
                            //return Json(new { Result = "OK", Records = srlogs, TotalRecordCount = srCount });

                            //var jsonData = new
                            //{
                            //    data = srlogs
                            //};

                            //var json = Json(jsonData, JsonRequestBehavior.AllowGet);
                            ////var json = Json(new { Result = "OK", Records = srlogs, TotalRecordCount = srCount });

                            ////json.MaxJsonLength = int.MaxValue;
                            //return json;
                            //return Json(jsonData, JsonRequestBehavior.AllowGet);

                            result = this.Json(new
                            {
                                draw = Convert.ToInt32(draw),
                                recordsTotal = srCount,
                                recordsFiltered = srCount,
                                data = srlogs
                            }, JsonRequestBehavior.AllowGet);


                        }
                        else if (keyword == "RemoveFilter")
                        {
                            //var srlogs = _repository.GetSRLogsList(userinfo.UserId, sortby1, sortby2, FromDate, ToDate, jtStartIndex, jtPageSize, jtSorting);
                            var srCount = _repository.GetSRLogcount(FromDate, ToDate);
                            if (startRec > srCount)
                            {
                                startRec = 0;
                            }
                            DateTime start = DateTime.Now;
                            var srlogs = _repository.GetSRLogsList(userinfo.UserId, sortby1, sortby2, "", "", startRec, pageSize, jtSorting);
                            DateTime end = DateTime.Now;
                            TimeSpan span = new TimeSpan();
                            span = end.Subtract(start);
                            EventLog.LogData("Total time to get records from database " + span.TotalSeconds.ToString(), true);

                            //var jsonData = new
                            //{
                            //    data = srlogs
                            //};

                            //var json = Json(jsonData, JsonRequestBehavior.AllowGet);
                            //var json = Json(new { Result = "OK", Records = srlogs, TotalRecordCount = srCount });
                            //json.MaxJsonLength = int.MaxValue;
                            //return json;
                            result = this.Json(new
                            {
                                draw = Convert.ToInt32(draw),
                                recordsTotal = srCount,
                                recordsFiltered = srCount,
                                data = srlogs
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            var srCount = _repository.GetSRLogcountByFilter(keyword, FromDate, ToDate);
                            var totalrecords = _repository.GetSRLogcount(FromDate, ToDate);
                            if (startRec > srCount)
                            {
                                startRec = 0;
                            }
                            DateTime start = DateTime.Now;
                            var srlogs = _repository.GetSRLogsListByFilter(keyword, sortby1, sortby2, FromDate, ToDate, userinfo.UserId, startRec, pageSize, jtSorting);

                            DateTime end = DateTime.Now;
                            TimeSpan span = new TimeSpan();
                            span = end.Subtract(start);
                            EventLog.LogData("Total time to get records from database " + span.TotalSeconds.ToString(), true);

                            //var jsonData = new
                            //{
                            //    data = srlogs
                            //};
                            //var json = Json(jsonData, JsonRequestBehavior.AllowGet);
                            //var json = Json(new { Result = "OK", Records = srlogs, TotalRecordCount = srCount });
                            //json.MaxJsonLength = int.MaxValue;
                            //return json;
                            //  List<tblBID_Log> bidlogs = _repository .GetBidLogs();
                            // return Json(new { Result = "OK", Records = srlogs, TotalRecordCount = srCount });

                            result = this.Json(new
                            {
                                draw = Convert.ToInt32(draw),
                                recordsTotal = totalrecords,
                                recordsFiltered = srCount,
                                data = srlogs
                            }, JsonRequestBehavior.AllowGet);
                        }

                        return result;
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

        [ExceptionHandler]
        public ActionResult PreviewReport(string Sort1by, string Sort2by, string FromDate, string ToDate, string searchby)
        {
            SRLogRepository _repository = new SRLogRepository();
            DataTable dt = _repository.GetSRLogsReportDetails(Sort1by, Sort2by, FromDate, ToDate, searchby);
            //  DataTable dt = new DataTable();
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.BorderWidth = 0;
            reportViewer.Width = 650;
            reportViewer.Height = 300;
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\SRLogDetails.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));

            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, empName });
            ViewBag.ReportViewer = reportViewer;
            ViewBag.ReportViewerFlag = true;

            return View("Print");
        }




        //public ActionResult PreviewReport(string id)
        //{
        //    SRLogRepository _repository = new SRLogRepository();
        //    DataTable dt = _repository.GetSRLogsReportDetails(id);
        //    ReportViewer reportViewer = new ReportViewer();
        //    reportViewer.ProcessingMode = ProcessingMode.Local;
        //    reportViewer.SizeToReportContent = true;
        //    reportViewer.BorderWidth = 0;
        //    reportViewer.Width = 650;
        //    reportViewer.Height = 300;
        //    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\SRLogDetails.rdlc";
        //    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));

        //    //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2, empName });
        //    ViewBag.ReportViewer = reportViewer;
        //    ViewBag.ReportViewerFlag = true;

        //    return View("Print");
        //}


        //public ActionResult Print()
        //{


        //    Warning[] warnings;
        //    string[] streamIds;
        //    string mimeType = string.Empty;
        //    string encoding = string.Empty;
        //    string extension = string.Empty;
        //    ReportViewer viewer = new ReportViewer();
        //    viewer.ProcessingMode = ProcessingMode.Local;
        //    string reportPath = Server.MapPath(@"~\Reports\Report1.rdlc");
        //    viewer.LocalReport.ReportPath = reportPath;
        //    DataTable ds = new DataTable();
        //    // ReportDataSource rds = new ReportDataSource("dsWMS", (DataTable)wmsDs.tblGatePass);
        //    ReportDataSource rds = new ReportDataSource("DataSet1", ds);
        //    viewer.LocalReport.DataSources.Clear();
        //    viewer.LocalReport.DataSources.Add(rds);
        //    viewer.LocalReport.EnableExternalImages = true;
        //    //ReportParameter[] param = new ReportParameter[10];


        //    //param[0] = new ReportParameter("ImgPath", "File:///" + Server.MapPath("~/Images") + "\\SpearLogo-TM.jpg");
        //    //param[1] = new ReportParameter("BarcodeImg", "File:///" + Server.MapPath("~/Picklist_Barcode") + "\\" + clientId + "_" + gatePassNo + ".jpg");
        //    //param[2] = new ReportParameter("GatePassNo", gatePassNo);
        //    //param[3] = new ReportParameter("GatePassDate", model.GatePassDate.ToString("dd/MMM/yyyy HH:mm"));
        //    //param[4] = new ReportParameter("Transporter", model.str_Transporter);
        //    //param[5] = new ReportParameter("VehicleNo", model.VehicleNo);
        //    //param[6] = new ReportParameter("LRNo", model.LRNo);
        //    //param[7] = new ReportParameter("LRDate", model.LRDate.ToString("dd/MMM/yyyy HH:mm"));
        //    //param[8] = new ReportParameter("ContactPerson", model.ContactPerson);
        //    //param[9] = new ReportParameter("ContactNo", model.ContactNo);
        //    // viewer.LocalReport.SetParameters(param);
        //    viewer.LocalReport.Refresh();


        //    byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
        //    if (!Directory.Exists(Server.MapPath("~/Reports_PDF")))
        //    {
        //        Directory.CreateDirectory(Server.MapPath("~/Reports_PDF"));
        //    }
        //    string path = Server.MapPath("~/Reports_PDF");
        //    //  string file_name = gatePassNo + "_" + clientId + "_Details.pdf"; //save the file in unique name 
        //    string file_name = "_Details.pdf"; //save the file in unique name 
        //    if (System.IO.File.Exists(path + "/" + file_name))
        //        System.IO.File.Delete(path + "/" + file_name);

        //    //After that use file stream to write from bytes to pdf file on your server path
        //    FileStream file = new FileStream(path + "/" + file_name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        //    file.Write(bytes, 0, bytes.Length);
        //    file.Dispose();

        //    string filePath = path + "/" + file_name;

        //    Response.AppendHeader("Content-Disposition", "inline; filename=" + file_name + ";");
        //    byte[] pdfByte = GetBytesFromFile(filePath);
        //    //  return File(pdfByte, "application/pdf");
        //    return View();




        //}

        //public static byte[] GetBytesFromFile(string fullFilePath)
        //{
        //    // this method is limited to 2^32 byte files (4.2 GB)

        //    FileStream fs = null;
        //    try
        //    {
        //        fs = System.IO.File.OpenRead(fullFilePath);
        //        byte[] bytes = new byte[fs.Length];
        //        fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
        //        return bytes;
        //    }
        //    finally
        //    {
        //        if (fs != null)
        //        {
        //            fs.Close();
        //            fs.Dispose();
        //        }
        //    }

        //}






        [ExceptionHandler]
        public ActionResult ConfigureColumns()
        {
            return View();
        }

        [ExceptionHandler]
        [HttpPost]
        public JsonResult GetSRLogColumns()
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    UserInfoViewModel userinfo = (UserInfoViewModel)Session["UserInfo"];
                    if (userinfo != null)
                    {
                        SRLogRepository _repository = new SRLogRepository();

                        List<string> SRLogColumns = _repository.GetColumns(userinfo.UserId);

                        return Json(SRLogColumns, JsonRequestBehavior.AllowGet);
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


        [ExceptionHandler]
        [HttpPost]
        public JsonResult GetConfiguredSRLogColumns()
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    UserInfoViewModel userinfo = (UserInfoViewModel)Session["UserInfo"];
                    if (userinfo != null)
                    {
                        SRLogRepository _repository = new SRLogRepository();

                        List<string> SRLogColumns = _repository.GetConfiguredSRLogColumns(userinfo.UserId);

                        return Json(SRLogColumns, JsonRequestBehavior.AllowGet);
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

        //This class is used to dynamically bind fields to SRLog jtable
        public class fields
        {
            public string fieldname { get; set; }
            public string width { get; set; }
            public string displayname { get; set; }


        }

        /// <summary>
        /// Returns a list of columns in the order in which they need to be bound
        /// </summary>
        /// <returns></returns>
        [ExceptionHandler]
        [HttpPost]
        public JsonResult GetAllSRLogColumns()
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    UserInfoViewModel userinfo = (UserInfoViewModel)Session["UserInfo"];
                    if (userinfo != null)
                    {
                        SettingsRepository _repository = new SettingsRepository();

                        List<SRLogDisplayViewModel> SRLogColumns = _repository.GetExistingColumnsWithDisplayNames(userinfo.UserId);

                        List<string> SRLogColumnsWidth = _repository.GetExistingColumnsWidth(userinfo.UserId);
                        int totalwidth = 0;
                        if (SRLogColumns.Count <= 28)
                        {
                            for (int i = 0; i < SRLogColumns.Count; i++)
                            {
                                totalwidth += Convert.ToInt32(SRLogColumnsWidth[i]);
                            }
                        }
                        else
                        {
                            foreach (string w in SRLogColumnsWidth)
                            {
                                totalwidth += Convert.ToInt32(w);
                            }
                        }

                        List<fields> JFields = new List<fields>();
                        if (SRLogColumns.Count <= 28)
                        {
                            for (int i = 0; i < SRLogColumns.Count; i++)
                            {
                                fields f = new fields();
                                f.fieldname = SRLogColumns[i].FieldName;
                                f.displayname = SRLogColumns[i].DisplayName;
                                f.width = (Convert.ToDouble(SRLogColumnsWidth[i]) / Convert.ToDouble(totalwidth) * 100).ToString("0.00") + "%";
                                JFields.Add(f);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < SRLogColumns.Count; i++)
                            {
                                fields f = new fields();
                                f.fieldname = SRLogColumns[i].FieldName;
                                f.displayname = SRLogColumns[i].DisplayName;
                                if (f.fieldname == "ProjectDescription" || f.fieldname == "Notes")
                                {
                                    f.width = "10%";
                                }
                                else if (f.fieldname == "CreationDate" || f.fieldname == "QuoteDue" || f.fieldname == "JobWalkDate" || f.fieldname == "QuoteDate" || f.fieldname == "QuoteTime" || f.fieldname == "AdvertiseDate")
                                {
                                    f.width = "5%";
                                }
                                else
                                    f.width = "2%";
                                JFields.Add(f);
                            }
                        }

                        return Json(JFields, JsonRequestBehavior.AllowGet);
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


        [ExceptionHandler]
        [HttpPost]
        public JsonResult SaveConfiguredColumns(string optionValues)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    UserInfoViewModel userinfo = (UserInfoViewModel)Session["UserInfo"];
                    if (userinfo != null)
                    {
                        List<string> columns = JsonConvert.DeserializeObject<List<string>>(optionValues);

                        SettingsRepository _setRepo = new SettingsRepository();
                        _setRepo.SaveColumnOrdering(userinfo.UserId, columns);


                        return Json("Column order set successfully.", JsonRequestBehavior.AllowGet);
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



        //[HttpGet]
        //public ActionResult GetSRLogsList(JQueryDataTableParamModel model, string fromdate,string todate,string sort1,string sort2)
        //{
        //    try
        //    {
        //        string s=Request.QueryString["name"];
        //        if (Session["UserInfo"] != null)
        //        {
        //            UserInfoViewModel userinfo = (UserInfoViewModel)Session["UserInfo"];

        //            if (userinfo != null)
        //            {
        //                SRLogRepository _repository = new SRLogRepository();
        //                var srlogs = _repository.GetSRLogsList(userinfo.UserId, sort1, sort2, fromdate, todate);
        //                var totalCount = srlogs.Count();
        //                if (!string.IsNullOrEmpty(model.sSearch))
        //                {



        //                }

        //                var filteredCount = srlogs.Count();
        //                if (model.iSortCol_0 != null)
        //                {

        //                }


        //                srlogs = srlogs.Skip(model.iDisplayStart)
        //                                   .Take(model.iDisplayLength);




        //                var result = srlogs.Select(x =>
        //                    new
        //                    {
        //                        SRNumber = x.SRNumber,
        //                        Customer = x.Customer,
        //                        Owner = x.Owner,

        //                        ProjectDescription = x.ProjectDescription,

        //                        Division = x.Division,

        //                        InactiveJob = x.InactiveJob,

        //                        ProjectManager = x.ProjectManager,
        //                        Estimator = x.Estimator,
        //                         CreationDate = x.CreationDate

        //                        //JobOrQuote = x.JobOrQuote,
        //                        //ContactEmail = x.ContactEmail,
        //                        //SiteForeman = x.SiteForeman,
        //                        //CustomerContact = x.CustomerContact,
        //                        //FileFolder = x.FileFolder,
        //                        //PW = x.PW,
        //                        //ChemFeed = x.ChemFeed,
        //                        //ContactPhone = x.ContactPhone,

        //                        //QuoteDue = x.QuoteDue,
        //                        //WhoJobWalk = x.WhoJobWalk,
        //                        //NotQuoted = x.NotQuoted,
        //                        //closed = x.Closed,
        //                        //BidAsPrimeOrSub = x.BidAsPrimeOrSub,
        //                        //Bonding = x.Bonding,
        //                        //BondingMailSent = x.BondingMailSent,
        //                        //PrevailingMailSent = x.PrevailingMailSent,
        //                        //Notes = x.Notes,
        //                        //JobsiteAddress = x.JobsiteAddress,
        //                        //ServerJobFolder = x.ServerJobFolder
        //                    }).ToArray();

        //                return Json(new
        //                {
        //                    sEcho = model.sEcho,
        //                    iTotalRecords = totalCount,
        //                    iTotalDisplayRecords = filteredCount,
        //                    aaData = result
        //                }, JsonRequestBehavior.AllowGet);

        //                //    SRLogRepository _repository = new SRLogRepository();
        //                //    if (keyword == "")
        //                //    {
        //                //        var srCount = _repository.GetSRLogcount(FromDate, ToDate);
        //                //        jtStartIndex = 0;
        //                //        jtPageSize = 500;
        //                //        var srlogs = _repository.GetSRLogsList(userinfo.UserId, sortby1, sortby2, FromDate, ToDate, jtStartIndex, jtPageSize, jtSorting);

        //                //        //  List<tblBID_Log> bidlogs = _repository .GetBidLogs();
        //                //        //return Json(new { Result = "OK", Records = srlogs, TotalRecordCount = srCount });

        //                //        var jsonData = new
        //                //        {
        //                //            data = srlogs
        //                //        };
        //                //        return Json(jsonData, JsonRequestBehavior.AllowGet);

        //                //    }
        //                //    else
        //                //    {
        //                //        var srCount = _repository.GetSRLogcountByFilter(keyword, FromDate, ToDate);

        //                //        var srlogs = _repository.GetSRLogsListByFilter(keyword, sortby1, sortby2, FromDate, ToDate, userinfo.UserId, jtStartIndex, jtPageSize, jtSorting);

        //                //        //  List<tblBID_Log> bidlogs = _repository .GetBidLogs();
        //                //        return Json(new { Result = "OK", Records = srlogs, TotalRecordCount = srCount });
        //                //    }
        //            }
        //            else
        //                return Json(new { Result = "ERROR", Message = "Session expired. Please login again" });
        //        }
        //        else
        //            return Json(new { Result = "ERROR", Message = "Session expired. Please login again" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        [ExceptionHandler]
        [HttpPost]
        public string SaveColumnWidth(string fieldsettings)
        {
            int colno = 2;
            UserInfoViewModel userinfo = (UserInfoViewModel)Session["UserInfo"];
            SettingsRepository _repository = new SettingsRepository();
            if (string.IsNullOrEmpty(fieldsettings) == false)
            {
                string[] Columns = fieldsettings.Split('|');
                if (Columns.Length > 0)
                {
                    List<string> SRLogColumnsWidth = _repository.GetExistingColumnsWidth(userinfo.UserId);
                    int totalwidth = 0;
                    if ((Columns.Length - 1) <= 28)
                    {
                        for (int i = 0; i < Columns.Length - 1; i++)
                        {
                            totalwidth += Convert.ToInt32(SRLogColumnsWidth[i]);
                        }
                    }
                    else
                    {
                        foreach (string w in SRLogColumnsWidth)
                        {
                            totalwidth += Convert.ToInt32(w);
                        }
                    }

                    for (int i = 0; i < Columns.Length; i++)
                    {
                        string[] widths = Columns[i].Split('=');
                        if (widths.Length == 2)
                        {
                            //Save setting                            
                            if (userinfo != null)
                            {
                                if (colno <= 29)
                                {
                                    if (widths[1] != "undefined" && widths[1] != "NaN")
                                    {
                                        double widthinpixel = (Convert.ToDouble(widths[1]) * totalwidth) / 100;

                                        _repository.UpdateSetting(userinfo.UserId, "Grid_Dsp_COLUMNSETTINGS1", "Col_" + colno.ToString(), widthinpixel.ToString("0").Trim());
                                        colno++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            EventLog.LogData("Column width changed successfully by " + userinfo.User_Name + " on " + DateTime.Now, true);
            return "Settings saved successfully";

        }
        [ExceptionHandler]
        [HttpPost]
        public JsonResult SaveOrderBy(string sort1, string order1, string sort2, string order2, string FromDate, string ToDate)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    UserInfoViewModel userinfo = (UserInfoViewModel)Session["UserInfo"];
                    if (userinfo != null)
                    {
                        if (sort1 == "")
                            order1 = "Ascending";

                        if (sort2 == "")
                            order2 = "Ascending";

                        if (order1 == "ASC")
                            order1 = "Ascending";

                        if (order2 == "ASC")
                            order2 = "Ascending";

                        if (order1 == "DESC")
                            order1 = "Descending";

                        if (order2 == "DESC")
                            order2 = "Descending";

                        SettingsRepository _repository = new SettingsRepository();
                        _repository.UpdateSetting(userinfo.UserId, "REPORT_BY_SR", "Level1_Field_Name", sort1);
                        _repository.UpdateSetting(userinfo.UserId, "REPORT_BY_SR", "Level1_Sort_Type", order1);
                        _repository.UpdateSetting(userinfo.UserId, "REPORT_BY_SR", "Level2_Field_Name", sort2);
                        _repository.UpdateSetting(userinfo.UserId, "REPORT_BY_SR", "Level2_Sort_Type", order2);

                        if (FromDate != "" && ToDate != "")
                        {
                            _repository.UpdateSetting(userinfo.UserId, "SRJob_Filter_Criteria", "CreationFromDateCond", FromDate);
                            _repository.UpdateSetting(userinfo.UserId, "SRJob_Filter_Criteria", "CreationToDateCond", ToDate);
                        }
                        else
                        {
                            _repository.UpdateSetting(userinfo.UserId, "SRJob_Filter_Criteria", "CreationFromDateCond", "");
                            _repository.UpdateSetting(userinfo.UserId, "SRJob_Filter_Criteria", "CreationToDateCond", "");
                        }

                        return Json("Sort order saved successfully.", JsonRequestBehavior.AllowGet);
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
        [ExceptionHandler]
        public ActionResult ScrollingTable()
        {
            return View();
        }



        //public JsonResult GetSRLogsData(string keyword = "", string sortby1 = "", string sortby2 = "", string FromDate = "", string ToDate = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        //{
        //    try
        //    {
        //        if (Session["UserInfo"] != null)
        //        {
        //            UserInfoViewModel userinfo = (UserInfoViewModel)Session["UserInfo"];
        //            if (userinfo != null)
        //            {
        //                SRLogRepository _repository = new SRLogRepository();
        //                if (keyword == "")
        //                {
        //                    // var srCount = _repository.GetSRLogcount(FromDate, ToDate);

        //                    var srlogs = _repository.GetSRLogsList(userinfo.UserId, sortby1, sortby2, FromDate, ToDate);

        //                    //  List<tblBID_Log> bidlogs = _repository .GetBidLogs();
        //                    var jsonData = new
        //                    {
        //                        data = srlogs
        //                    };
        //                    return Json(jsonData, JsonRequestBehavior.AllowGet);
        //                    //return Json(new { Result = "OK", Records = srlogs, TotalRecordCount = srCount });
        //                }
        //                else
        //                {
        //                    var srCount = _repository.GetSRLogcountByFilter(keyword, FromDate, ToDate);

        //                    var srlogs = _repository.GetSRLogsListByFilter(keyword, sortby1, sortby2, FromDate, ToDate, userinfo.UserId, jtStartIndex, jtPageSize, jtSorting);

        //                    //  List<tblBID_Log> bidlogs = _repository .GetBidLogs();
        //                    var jsonData = new
        //                    {
        //                        data = srlogs
        //                    };
        //                    return Json(jsonData, JsonRequestBehavior.AllowGet);
        //                    // return Json(new { Result = "OK", Records = srlogs, TotalRecordCount = srCount });
        //                }
        //            }
        //            else
        //                return Json(new { Result = "ERROR", Message = "Session expired. Please login again" });
        //        }
        //        else
        //            return Json(new { Result = "ERROR", Message = "Session expired. Please login again" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
    }
}
