/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Infrastructure;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BaseEAM.Web.Framework.Controllers
{
    public class AccessibleAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null || filterContext.HttpContext == null)
                return;

            HttpRequestBase request = filterContext.HttpContext.Request;
            if (request == null)
                return;

            string actionName = filterContext.ActionDescriptor.ActionName;
            if (string.IsNullOrEmpty(actionName))
                return;

            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            if (string.IsNullOrEmpty(controllerName))
                return;

            //don't apply filter to child methods
            if (filterContext.IsChildAction)
                return;

            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            //don't apply for Login page
            if (workContext.CurrentUser == null)
                return;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                throw new UnauthorizedAccessException("Access Denied.");
            }
            else
            {
                //redirect to AccessDenied page
                filterContext.Result = new RedirectToRouteResult(
                                       new RouteValueDictionary
                                       {
                                       { "action", "AccessDenied" },
                                       { "controller", "Home" }
                                       });
            }
        }
    }
}
