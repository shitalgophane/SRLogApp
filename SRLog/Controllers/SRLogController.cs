using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRLog.Data;
using SRLog.Data.SRLog;
using SRLog.Entities.Account.ViewModels;
using Newtonsoft.Json;

namespace SRLog.Controllers
{
    public class SRLogController : Controller
    {
        //
        // GET: /SRLog/

        public ActionResult Index()
        {
            return View();
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
                        List<string> datax1 = JsonConvert.DeserializeObject<List<string>>(optionValues);

                        //Delete all existing settings

                        //Save new settings

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
    }
}
