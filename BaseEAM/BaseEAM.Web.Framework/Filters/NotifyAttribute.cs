/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Web.Framework.UI;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BaseEAM.Web.Framework.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class NotifyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var successMessages = new List<string>();
                //success messages
                if (filterContext.Controller.TempData[string.Format("baseeam.notifications.{0}", NotifyType.Success)] != null)
                {
                    successMessages.AddRange(filterContext.Controller.TempData[string.Format("baseeam.notifications.{0}", NotifyType.Success)] as IList<string>);
                }
                if (filterContext.Controller.ViewData[string.Format("baseeam.notifications.{0}", NotifyType.Success)] != null)
                {
                    successMessages.AddRange(filterContext.Controller.ViewData[string.Format("baseeam.notifications.{0}", NotifyType.Success)] as IList<string>);
                }

                if (successMessages.Count > 0)
                {
                    filterContext.HttpContext.Response.AddHeader("X-Message-Type", NotifyType.Success.ToString().ToLower());
                    filterContext.HttpContext.Response.AddHeader("X-Message", successMessages[0]);
                    return;
                }

                //error messages
                var errorMessages = new List<string>();
                if (filterContext.Controller.TempData[string.Format("baseeam.notifications.{0}", NotifyType.Error)] != null)
                {
                    errorMessages.AddRange(filterContext.Controller.TempData[string.Format("baseeam.notifications.{0}", NotifyType.Error)] as IList<string>);
                }
                if (filterContext.Controller.ViewData[string.Format("baseeam.notifications.{0}", NotifyType.Error)] != null)
                {
                    errorMessages.AddRange(filterContext.Controller.ViewData[string.Format("baseeam.notifications.{0}", NotifyType.Error)] as IList<string>);
                }

                if (errorMessages.Count > 0)
                {
                    filterContext.HttpContext.Response.AddHeader("X-Message-Type", NotifyType.Error.ToString().ToLower());
                    filterContext.HttpContext.Response.AddHeader("X-Message", errorMessages[0]);
                    return;
                }
            }                
        }
    }
}
