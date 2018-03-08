using BaseEAM.Core.Infrastructure;
using BaseEAM.Services;
using System;
using System.Web.Mvc;

namespace BaseEAM.Web.Framework.Filters
{
    public class UrlAccessedAttribute : FilterAttribute, IActionFilter
    {
        public virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null || filterContext.HttpContext == null || filterContext.HttpContext.Request == null
                || filterContext.HttpContext.Request.IsAjaxRequest())
                return;

            // don't apply filter to child methods
            if (filterContext.IsChildAction)
                return;

            // only GET requests
            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            var userActivityService = EngineContext.Current.Resolve<IUserActivityService>();
            var urlAccessed = filterContext.HttpContext.Request.RawUrl;
            if (urlAccessed.Contains("Export") || urlAccessed.Contains("Preview"))
                return;
            var activityLog = userActivityService.GetActivityLog("UrlAccessed", urlAccessed);
            if (activityLog == null)
            {
                userActivityService.InsertActivityLog("UrlAccessed", urlAccessed);
            }
            else
            {
                userActivityService.UpdateActivityLog(activityLog);
            }
        }

        public virtual void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}
