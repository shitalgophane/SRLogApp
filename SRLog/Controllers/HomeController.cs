using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using SRLog.Data.Settings;
using SRLog.Entities.Settings.ViewModels;
using SRLog.Data.TestSite;

namespace SRLog.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            TestSiteRepository _repository = new TestSiteRepository();
            ViewBag.IsScheduledMaintenance = _repository.GetMaintenanceDetails();

           



            ViewBag.User = ViewBag.UserName;
            ViewBag.Admin_Rights = Session["Admin_Rights"].ToString();
            ViewBag.SR_Log_ReadOnly = Session["SR_Log_ReadOnly"].ToString();
            ViewBag.Bid_Log_ReadOnly = Session["Bid_Log_ReadOnly"].ToString();
            return View();
        }

    }
}
