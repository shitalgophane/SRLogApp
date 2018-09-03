using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRLog.Entities;
using System.Web.Mvc;

namespace SRLog.Filters
{
    public class ExceptionHandlerAttribute: FilterAttribute, IExceptionFilter
    
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                string controllerName = filterContext.RouteData.Values["controller"].ToString();
                string actionName = filterContext.RouteData.Values["action"].ToString();

                ExceptionLogger logger = new ExceptionLogger()
                {
                    ExceptionMessage = filterContext.Exception.Message,
                    ExceptionStackTrace = filterContext.Exception.StackTrace,
                    ControllerName = controllerName,
                    ActionName = actionName,
                    LogTime = DateTime.Now
                };

                SR_Log_DatabaseSQLEntities ctx = new SR_Log_DatabaseSQLEntities();
                ctx.ExceptionLoggers.Add(logger);
                ctx.SaveChanges();



                Exception custException = new Exception("An error occurred while processing your request.");



                var model = new HandleErrorInfo(custException, controllerName, actionName);

                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Error.cshtml",
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = filterContext.Controller.TempData
                };


                filterContext.ExceptionHandled = true;
            }
        }
    }
}