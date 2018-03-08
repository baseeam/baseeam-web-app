/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Infrastructure;
using BaseEAM.Services;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BaseEAM.Web.Framework.Filters
{
    public class BaseEamAuthorizeAttribute : AuthorizeAttribute
    {
        public string PermissionNames { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isAuthenticated = httpContext.User.Identity.IsAuthenticated;
            bool isAuthorized = false;
            if (isAuthenticated == true)
            {
                //BaseEAM authorization
                var permissionService = EngineContext.Current.Resolve<IPermissionService>();
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                var permissions = PermissionNames.Split(',');
                foreach (var permission in permissions)
                {
                    if (permissionService.Authorize(permission, workContext.CurrentUser))
                    {
                        isAuthorized = true;
                        break;
                    }
                }
            }

            return isAuthorized;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            var request = httpContext.Request;
            var response = httpContext.Response;
            var user = httpContext.User;

            if (request.IsAjaxRequest())
            {
                if (user.Identity.IsAuthenticated == false)
                    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                else
                    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            else
            {
                if (user.Identity.IsAuthenticated == false)
                {
                    //redirect to Login page
                    filterContext.Result = new RedirectToRouteResult(
                                           new RouteValueDictionary
                                           {
                                               { "action", "Login" },
                                               { "controller", "User" }
                                           });
                }
                else
                {
                    //redirect to AccessDenied page
                    filterContext.Result = new RedirectToRouteResult(
                                           new RouteValueDictionary
                                           {
                                               { "action", "AccessDenied" },
                                               { "controller", "Common" }
                                           });
                }
            }
        }
    }
}
