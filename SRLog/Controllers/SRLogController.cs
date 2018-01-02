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
                }
                else if (Flag == "ListAllJobsByKeywordSearch")
                {
                    ViewBag.Title = "List All Jobs By Keywords";
                    ViewBag.Sort1 = string.Empty;
                    ViewBag.Sort2 = string.Empty;

                    ViewBag.Order1 = "ASC";
                    ViewBag.Order2 = "ASC";
                }
                else if (Flag == "ListAllJobsByFilter")
                {
                    ViewBag.Title = "List All Jobs By Custom Sort";

                    ViewBag.Sort1 = _repository.GetSort("Level1_Field_Name");
                    ViewBag.Sort2 = _repository.GetSort("Level2_Field_Name");

                    ViewBag.Order1 = _repository.GetSort("Level1_Sort_Type");
                    ViewBag.Order2 = _repository.GetSort("Level2_Sort_Type");
                    if (ViewBag.Order1 == "Descending")
                        ViewBag.Order1 = "DESC";

                    if (ViewBag.Order1 == "Ascending")
                        ViewBag.Order1 = "ASC";

                    if (ViewBag.Order2 == "Descending")
                        ViewBag.Order2 = "DESC";

                    if (ViewBag.Order2 == "Ascending")
                        ViewBag.Order2 = "ASC";

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

        public ActionResult ConfigureColumns()
        {
            return View();
        }

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

        [HttpPost]
        public JsonResult GetSRLogsList(string keyword = "", string sortby1 = "", string sortby2 = "", int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                if (Session["UserInfo"] != null)
                {
                    UserInfoViewModel userinfo = (UserInfoViewModel)Session["UserInfo"];
                    if (userinfo != null)
                    {
                        SRLogRepository _repository = new SRLogRepository();
                        if (keyword == "")
                        {
                            var srCount = _repository.GetSRLogcount();

                            var srlogs = _repository.GetSRLogsList(userinfo.UserId, sortby1, sortby2, jtStartIndex, jtPageSize, jtSorting);

                            //  List<tblBID_Log> bidlogs = _repository .GetBidLogs();
                            return Json(new { Result = "OK", Records = srlogs, TotalRecordCount = srCount });
                        }
                        else
                        {
                            var srCount = _repository.GetSRLogcountByFilter(keyword);

                            var srlogs = _repository.GetSRLogsListByFilter(keyword, sortby1, sortby2, userinfo.UserId, jtStartIndex, jtPageSize, jtSorting);

                            //  List<tblBID_Log> bidlogs = _repository .GetBidLogs();
                            return Json(new { Result = "OK", Records = srlogs, TotalRecordCount = srCount });
                        }
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

        [HttpPost]
        public JsonResult SaveOrderBy(string sort1, string order1, string sort2, string order2)
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
    }
}
