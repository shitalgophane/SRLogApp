using System.Web;
using System.Web.Mvc;
using SRLog.Filters;

namespace SRLog
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomAuthFilterAttribute());
        }
    }
}