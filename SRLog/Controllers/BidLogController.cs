using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRLog.Entities.BidLog.ViewModels;
using SRLog.Data;
using SRLog.Data.Account;
using SRLog.Data.BidLog;

namespace SRLog.Controllers
{
    public class BidLogController : Controller
    {
        //
        // GET: /BidLog/

        public ActionResult Index()
        {           
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetBidLogsList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                BidLogRepository _repository = new BidLogRepository();
                var bidCount = _repository.GetBidlogCount();

                var bidlogs = _repository.GetBidLogsList(jtStartIndex, jtPageSize, jtSorting);

              //  List<tblBID_Log> bidlogs = _repository .GetBidLogs();
                return Json(new { Result = "OK", Records = bidlogs, TotalRecordCount = bidCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

    }
}
