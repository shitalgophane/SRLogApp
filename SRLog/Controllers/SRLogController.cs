using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRLog.Data;
using SRLog.Data.SRLog;
using SRLog.Data.Settings;
using SRLog.Entities.Account.ViewModels;
using SRLog.Entities.Settings.ViewModels;
using Newtonsoft.Json;
using SRLog.Filters;

namespace SRLog.Controllers
{
    [AdminFilter]
    public class SRLogController : Controller
    {
        //
        // GET: /SRLog/
        [AdminFilter]
        public ActionResult Index()
        {
            UserInfoViewModel userinfo = (UserInfoViewModel)Session["UserInfo"];
            if (userinfo != null)
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Account");
        }

        [AdminFilter]
        public ActionResult ConfigureColumns()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [AdminFilter]
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

        [HttpPost]
        [AllowAnonymous]
        [AdminFilter]
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
        [HttpPost]
        [AllowAnonymous]
        [AdminFilter]
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

        [HttpPost]
        [AllowAnonymous]
        [AdminFilter]
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

        [HttpPost]
        [AllowAnonymous]
        [AdminFilter]
        public JsonResult GetSRLogsList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    UserInfoViewModel userinfo = (UserInfoViewModel)Session["UserInfo"];
                    if (userinfo != null)
                    {
                        SRLogRepository _repository = new SRLogRepository();
                        var srCount = _repository.GetSRLogcount();

                        var srlogs = _repository.GetSRLogsList(userinfo.UserId, jtStartIndex, jtPageSize, jtSorting);

                        //  List<tblBID_Log> bidlogs = _repository .GetBidLogs();
                        return Json(new { Result = "OK", Records = srlogs, TotalRecordCount = srCount });
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

        [HttpPost]
        [AdminFilter]
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
    }
}
