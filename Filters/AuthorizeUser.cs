using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CCSHealthFamilyWelfareDept.Models;

namespace CCSHealthFamilyWelfareDept.Filters
{
    public class AuthorizeUser : AuthorizeAttribute
    {
        SessionManager SM = new SessionManager();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (SM.UserID != 0 && SM.RollID == 1)
            {
                //if (filterContext.HttpContext.Request.UrlReferrer == null || filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
                //{
                //    if (filterContext.HttpContext.Request.IsAjaxRequest())
                //    {

                //    }
                //    else
                //    {
                //        HttpContext.Current.Session["UserExceptionSession"] = "URL Tempering is not allowed Please don't refresh or try to edit Url!";
                //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "LogOut", area = "" }));
                //    }
                //}


            }
            else
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    HttpContext.Current.Session["UserExceptionSession"] = "Your session has been expired and You have been Logged Out !";
                    filterContext.Result = new RedirectToRouteResult(
                                       new RouteValueDictionary 
                                   {
                                       { "action", "LogOut" },
                                       { "controller", "Account" } 
                                   });
                }

            }

        }
    }

    public class AuthorizeAdmin : AuthorizeAttribute
    {
        private int _serviceId = 0;
        public AuthorizeAdmin(int serviceId = 0)
        {
            _serviceId = serviceId;
        }
        SessionManager SM = new SessionManager();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (SM.UserID != 0 && SM.RollID >= 2)
            {
                if (_serviceId > 0)
                {
                    var permission = (List<RollPermissionModel>)filterContext.HttpContext.Application["RollWiseServices"];
                    int count = permission.Where(e => e.rollId == SM.RollID && e.serviceId == _serviceId).ToList().Count();
                    if (count == 0)
                    {
                        filterContext.Result = new RedirectToRouteResult(
                                           new RouteValueDictionary 
                                   {
                                       { "action", "UnauthoriseAcess" },
                                       { "controller", "Home" } 
                                   });
                    }
                }
              

                //if (filterContext.HttpContext.Request.UrlReferrer == null || filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
                //{
                //    if (filterContext.HttpContext.Request.IsAjaxRequest())
                //    {

                //    }
                //    else
                //    {
                //        HttpContext.Current.Session["UserExceptionSession"] = "URL Tempering is not allowed Please don't refresh or try to edit Url!";
                //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AdminAccount", action = "LogOut", area = "" }));
                //    }
                //}  
            }
            else
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.HttpContext.Response.End();
                }
                else
                {

                    HttpContext.Current.Session["UserExceptionSession"] = "Your session has been expired and You have been Logged Out !";
                    filterContext.Result = new RedirectToRouteResult(
                                       new RouteValueDictionary 
                                   {
                                       { "action", "LogOut" },
                                       { "controller", "AdminAccount" } 
                                   });
                }

            }

        }
    }
}