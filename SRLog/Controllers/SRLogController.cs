using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRLog.Data;
using SRLog.Data.SRLog;
using SRLog.Data.Settings;
using SRLog.Entities.Account.ViewModels;
using Newtonsoft.Json;
using SRLog.Models;

namespace SRLog.Controllers
{
    public class SRLogController : Controller
    {
        //
        // GET: /SRLog/

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

        public ActionResult ConfigureColumns()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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
        }

        /// <summary>
        /// Returns a list of columns in the order in which they need to be bound
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
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

                        List<string> SRLogColumns = _repository.GetExistingColumns(userinfo.UserId);
                        List<fields> JFields = new List<fields>();

                        foreach (string s in SRLogColumns)
                        {
                            fields f = new fields();
                            f.fieldname = s;
                            JFields.Add(f);
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
    }
}
