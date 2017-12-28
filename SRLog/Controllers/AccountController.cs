using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRLog.Entities.Account.ViewModels;
using SRLog.Data;
using SRLog.Data.Account;
using SRLog.Data.Activity;
using SRLog.Data.Settings;
using SRLog.Filters;

namespace SRLog.Controllers
{
    [Authorize]
    
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        [AllowAnonymous]
        [SkipCustomAuthFilter]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [SkipCustomAuthFilter]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EventLog.LogData(DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + " Login - Validating user.", true);

                    var user = new LoginRepository();
                    var act = new ActivityRepository();

                    LoginViewModel l = user.GetUserById(model.LoginName, model.Password);
                    if (l == null)
                    {
                        ViewBag.Message = "User Name or Password is incorrect";
                        ModelState.AddModelError("", "User Name or Password is incorrect");
                        return View(model);
                    }
                    else
                    {
                        EventLog.LogData(DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + " Login - User Validation completed.", true);
                        //Store user details and previleges in userinfo object
                        Session["User"] = l.LoginName;
                        ViewBag.UserName = l.LoginName;
                        Session["UserInfo"] = l.UserInfo;

                        //Log activity in database
                        act.AddActivityLog(l.UserInfo.User_Name, "Login", "Login", "User " + l.UserInfo.User_Name + " Logged in.");

                        //Check if ini file settings exist for this user. Else make an entry in database
                        CheckINIFile(l.UserInfo.UserId);
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View(model);
                //return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                //SetLoginViewMdelForError(model);
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public void CheckINIFile(int userid)
        {
            var setting = new SettingsRepository();
            bool isSettingsExists = setting.IsSettingsExistsForUser(userid);
            if (isSettingsExists == false)
            {
                IniFile MyIni = new IniFile();
                //Read all sections of ini file and store in settings table

                //Read reportquery
                string ReportQuery = MyIni.Read("QueryName1", "ReportQuery");
                if (string.IsNullOrEmpty(ReportQuery) == false)
                {
                    //We will store the column order in seperate table
                    ReportQuery = ReportQuery.Replace("SELECT", "");

                    ReportQuery = ReportQuery.Replace("From [tblSR Log]", "");

                    ReportQuery = ReportQuery.Replace("case when  [JobOrQuote] = 1 then 'Job' else 'Quote' end as [JobOrQuote]", "[JobOrQuote]");

                    ReportQuery = ReportQuery.Replace("[", "");

                    ReportQuery = ReportQuery.Replace("]", "");

                    string[] queryfields = ReportQuery.Split(',');
                    for (int i = 0; i < queryfields.Length; i++)
                    {
                        if (Enum.IsDefined(typeof(GlobalSettings.FixedColumnsInSRLog), queryfields[i].ToString().Trim()))
                        {
                            setting.AddSetting(userid, "ReportQuery", queryfields[i].ToString().Trim(), queryfields[i].ToString().Trim(), true);
                        }
                        else
                        {
                            setting.AddSetting(userid, "ReportQuery", queryfields[i].ToString().Trim(), queryfields[i].ToString().Trim());
                        }
                    }                    
                }

                //Read columnsettings
                string QuoteDueDate = MyIni.Read("QuoteDueDate", "COLUMNSETTINGS");
                if (string.IsNullOrEmpty(QuoteDueDate) == false)
                {
                    setting.AddSetting(userid, "COLUMNSETTINGS", "QuoteDueDate", QuoteDueDate);
                }

                string BiddingAs = MyIni.Read("BiddingAs", "COLUMNSETTINGS");
                if (string.IsNullOrEmpty(BiddingAs) == false)
                {
                    setting.AddSetting(userid, "COLUMNSETTINGS", "BiddingAs", BiddingAs);
                }

                string Customer = MyIni.Read("Customer", "COLUMNSETTINGS");
                if (string.IsNullOrEmpty(Customer) == false)
                {
                    setting.AddSetting(userid, "COLUMNSETTINGS", "Customer", Customer);
                }

                string ProjectDescription = MyIni.Read("ProjectDescription", "COLUMNSETTINGS");
                if (string.IsNullOrEmpty(ProjectDescription) == false)
                {
                    setting.AddSetting(userid, "COLUMNSETTINGS", "ProjectDescription", ProjectDescription);
                }

                string Division = MyIni.Read("Division", "COLUMNSETTINGS");
                if (string.IsNullOrEmpty(Division) == false)
                {
                    setting.AddSetting(userid, "COLUMNSETTINGS", "Division", Division);
                }

                string LastAddendumReceived = MyIni.Read("LastAddendumReceived", "COLUMNSETTINGS");
                if (string.IsNullOrEmpty(LastAddendumReceived) == false)
                {
                    setting.AddSetting(userid, "COLUMNSETTINGS", "LastAddendumReceived", LastAddendumReceived);
                }

                string EstimatorInitials = MyIni.Read("EstimatorInitials", "COLUMNSETTINGS");
                if (string.IsNullOrEmpty(EstimatorInitials) == false)
                {
                    setting.AddSetting(userid, "COLUMNSETTINGS", "EstimatorInitials", EstimatorInitials);
                }

                string QuoteNumber = MyIni.Read("QuoteNumber", "COLUMNSETTINGS");
                if (string.IsNullOrEmpty(QuoteNumber) == false)
                {
                    setting.AddSetting(userid, "COLUMNSETTINGS", "QuoteNumber", QuoteNumber);
                }

                string AdvertiseDate = MyIni.Read("AdvertiseDate", "COLUMNSETTINGS");
                if (string.IsNullOrEmpty(AdvertiseDate) == false)
                {
                    setting.AddSetting(userid, "COLUMNSETTINGS", "AdvertiseDate", AdvertiseDate);
                }

                string Owner = MyIni.Read("Owner", "COLUMNSETTINGS");
                if (string.IsNullOrEmpty(Owner) == false)
                {
                    setting.AddSetting(userid, "COLUMNSETTINGS", "Owner", Owner);
                }

                string ProjectFolder = MyIni.Read("ProjectFolder", "COLUMNSETTINGS");
                if (string.IsNullOrEmpty(ProjectFolder) == false)
                {
                    setting.AddSetting(userid, "COLUMNSETTINGS", "ProjectFolder", ProjectFolder);
                }

                string DOW = MyIni.Read("DOW", "COLUMNSETTINGS");
                if (string.IsNullOrEmpty(DOW) == false)
                {
                    setting.AddSetting(userid, "COLUMNSETTINGS", "DOW", DOW);
                }

                string CityState = MyIni.Read("CityState", "COLUMNSETTINGS");
                if (string.IsNullOrEmpty(CityState) == false)
                {
                    setting.AddSetting(userid, "COLUMNSETTINGS", "CityState", CityState);
                }

                //Read REPORT_BY_SR
                string Level1_Field_Name = MyIni.Read("Level1_Field_Name", "REPORT_BY_SR");
                if (string.IsNullOrEmpty(Level1_Field_Name) == false)
                {
                    setting.AddSetting(userid, "REPORT_BY_SR", "Level1_Field_Name", Level1_Field_Name);
                }

                string Level1_Sort_Type = MyIni.Read("Level1_Sort_Type", "REPORT_BY_SR");
                if (string.IsNullOrEmpty(Level1_Sort_Type) == false)
                {
                    setting.AddSetting(userid, "REPORT_BY_SR", "Level1_Sort_Type", Level1_Sort_Type);
                }

                string Level2_Field_Name = MyIni.Read("Level2_Field_Name", "REPORT_BY_SR");
                if (string.IsNullOrEmpty(Level2_Field_Name) == false)
                {
                    setting.AddSetting(userid, "REPORT_BY_SR", "Level2_Field_Name", Level2_Field_Name);
                }

                string Level2_Sort_Type = MyIni.Read("Level2_Sort_Type", "REPORT_BY_SR");
                if (string.IsNullOrEmpty(Level2_Sort_Type) == false)
                {
                    setting.AddSetting(userid, "REPORT_BY_SR", "Level2_Sort_Type", Level2_Sort_Type);
                }

                //Read QUOTECOLUMNSETTINGS
                QuoteDueDate = MyIni.Read("QuoteDueDate", "QUOTECOLUMNSETTINGS");
                if (string.IsNullOrEmpty(QuoteDueDate) == false)
                {
                    setting.AddSetting(userid, "QUOTECOLUMNSETTINGS", "QuoteDueDate", QuoteDueDate);
                }

                string PlansRequested = MyIni.Read("PlansRequested", "QUOTECOLUMNSETTINGS");
                if (string.IsNullOrEmpty(PlansRequested) == false)
                {
                    setting.AddSetting(userid, "QUOTECOLUMNSETTINGS", "PlansRequested", PlansRequested);
                }

                string PlansReceived = MyIni.Read("PlansReceived", "QUOTECOLUMNSETTINGS");
                if (string.IsNullOrEmpty(PlansReceived) == false)
                {
                    setting.AddSetting(userid, "QUOTECOLUMNSETTINGS", "PlansReceived", PlansReceived);
                }

                string PlansReceivedBy = MyIni.Read("PlansReceivedBy", "QUOTECOLUMNSETTINGS");
                if (string.IsNullOrEmpty(PlansReceivedBy) == false)
                {
                    setting.AddSetting(userid, "QUOTECOLUMNSETTINGS", "PlansReceivedBy", PlansReceivedBy);
                }

                BiddingAs = MyIni.Read("BiddingAs", "QUOTECOLUMNSETTINGS");
                if (string.IsNullOrEmpty(BiddingAs) == false)
                {
                    setting.AddSetting(userid, "QUOTECOLUMNSETTINGS", "BiddingAs", BiddingAs);
                }

                Customer = MyIni.Read("Customer", "QUOTECOLUMNSETTINGS");
                if (string.IsNullOrEmpty(Customer) == false)
                {
                    setting.AddSetting(userid, "QUOTECOLUMNSETTINGS", "Customer", Customer);
                }

                ProjectDescription = MyIni.Read("ProjectDescription", "QUOTECOLUMNSETTINGS");
                if (string.IsNullOrEmpty(ProjectDescription) == false)
                {
                    setting.AddSetting(userid, "QUOTECOLUMNSETTINGS", "ProjectDescription", ProjectDescription);
                }

                Division = MyIni.Read("Division", "QUOTECOLUMNSETTINGS");
                if (string.IsNullOrEmpty(Division) == false)
                {
                    setting.AddSetting(userid, "QUOTECOLUMNSETTINGS", "Division", Division);
                }

                LastAddendumReceived = MyIni.Read("LastAddendumReceived", "QUOTECOLUMNSETTINGS");
                if (string.IsNullOrEmpty(LastAddendumReceived) == false)
                {
                    setting.AddSetting(userid, "QUOTECOLUMNSETTINGS", "LastAddendumReceived", LastAddendumReceived);
                }

                EstimatorInitials = MyIni.Read("EstimatorInitials", "QUOTECOLUMNSETTINGS");
                if (string.IsNullOrEmpty(EstimatorInitials) == false)
                {
                    setting.AddSetting(userid, "QUOTECOLUMNSETTINGS", "EstimatorInitials", EstimatorInitials);
                }

                QuoteNumber = MyIni.Read("QuoteNumber", "QUOTECOLUMNSETTINGS");
                if (string.IsNullOrEmpty(QuoteNumber) == false)
                {
                    setting.AddSetting(userid, "QUOTECOLUMNSETTINGS", "QuoteNumber", QuoteNumber);
                }

                //Read Filter_Criteria
                string BiddingAsCond = MyIni.Read("BiddingAsCond", "Filter_Criteria");
                if (string.IsNullOrEmpty(BiddingAsCond) == false)
                {
                    setting.AddSetting(userid, "Filter_Criteria", "BiddingAsCond", BiddingAsCond);
                }

                string DivisionCond = MyIni.Read("BiddingAsCond", "Filter_Criteria");
                if (string.IsNullOrEmpty(BiddingAsCond) == false)
                {
                    setting.AddSetting(userid, "Filter_Criteria", "BiddingAsCond", BiddingAsCond);
                }

                //Read SRJob_Filter_Criteria
                string CreationFromDateCond = MyIni.Read("BiddingAsCond", "SRJob_Filter_Criteria");
                if (string.IsNullOrEmpty(CreationFromDateCond) == false)
                {
                    setting.AddSetting(userid, "SRJob_Filter_Criteria", "CreationFromDateCond", CreationFromDateCond);
                }

                string CreationToDateCond = MyIni.Read("CreationToDateCond", "SRJob_Filter_Criteria");
                if (string.IsNullOrEmpty(CreationToDateCond) == false)
                {
                    setting.AddSetting(userid, "SRJob_Filter_Criteria", "CreationToDateCond", CreationToDateCond);
                }

                //Read SRJob_Filter_Criteria
                string Col_1 = MyIni.Read("Col_1", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_1) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_1", Col_1);
                }

                string Col_2 = MyIni.Read("Col_2", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_2) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_2", Col_2);
                }

                string Col_3 = MyIni.Read("Col_3", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_3) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_3", Col_3);
                }

                string Col_4 = MyIni.Read("Col_4", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_4) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_4", Col_4);
                }

                string Col_5 = MyIni.Read("Col_5", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_5) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_5", Col_5);
                }

                string Col_6 = MyIni.Read("Col_6", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_6) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_6", Col_6);
                }

                string Col_7 = MyIni.Read("Col_7", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_7) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_7", Col_7);
                }

                string Col_8 = MyIni.Read("Col_8", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_8) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_8", Col_8);
                }

                string Col_9 = MyIni.Read("Col_9", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_9) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_9", Col_9);
                }

                string Col_10 = MyIni.Read("Col_10", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_10) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_10", Col_10);
                }

                string Col_11 = MyIni.Read("Col_11", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_11) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_11", Col_11);
                }

                string Col_12 = MyIni.Read("Col_12", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_12) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_12", Col_12);
                }

                string Col_13 = MyIni.Read("Col_13", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_13) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_13", Col_13);
                }

                string Col_14 = MyIni.Read("Col_14", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_14) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_14", Col_14);
                }

                string Col_15 = MyIni.Read("Col_15", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_15) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_15", Col_15);
                }

                string Col_16 = MyIni.Read("Col_16", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_16) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_16", Col_16);
                }

                string Col_17 = MyIni.Read("Col_17", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_17) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_17", Col_17);
                }

                string Col_18 = MyIni.Read("Col_18", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_18) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_18", Col_18);
                }

                string Col_19 = MyIni.Read("Col_19", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_19) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_19", Col_19);
                }

                string Col_20 = MyIni.Read("Col_20", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_20) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_20", Col_20);
                }

                string Col_21 = MyIni.Read("Col_21", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_21) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_21", Col_21);
                }

                string Col_22 = MyIni.Read("Col_22", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_22) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_22", Col_22);
                }

                string Col_23 = MyIni.Read("Col_23", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_23) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_23", Col_23);
                }

                string Col_24 = MyIni.Read("Col_24", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_24) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_24", Col_24);
                }

                string Col_25 = MyIni.Read("Col_25", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_25) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_25", Col_25);
                }

                string Col_26 = MyIni.Read("Col_26", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_26) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_26", Col_26);
                }

                string Col_27 = MyIni.Read("Col_27", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_27) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_27", Col_27);
                }

                string Col_28 = MyIni.Read("Col_28", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_28) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_28", Col_28);
                }

                string Col_29 = MyIni.Read("Col_29", "Grid_Dsp_COLUMNSETTINGS1");
                if (string.IsNullOrEmpty(Col_29) == false)
                {
                    setting.AddSetting(userid, "Grid_Dsp_COLUMNSETTINGS1", "Col_29", Col_29);
                }

            }
        }


    }
}
