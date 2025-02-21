using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CCSHealthFamilyWelfareDept
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            Application["RollWiseServices"] = CCSHealthFamilyWelfareDept.Models.RollPermissionModel.FillPermission();
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs //Test

            HttpContext.Current.Response.Headers.Remove("Server");
            HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");

            string ErrorConfig = System.Configuration.ConfigurationManager.AppSettings["ErrorConfig"] != null ? System.Configuration.ConfigurationManager.AppSettings["ErrorConfig"].ToString().ToUpper() : "ON";

            if (ErrorConfig == "ON")
            {
                Exception exc = Server.GetLastError();

                if (exc != null)
                {
                    Response.Write("<h2>Global Page Error</h2>\n");
                    Response.Write(
                        "<p>" + exc.Message + "</p>\n");
                    Response.Write("Return to the <a href='" + HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "'>" +
                        "Home Page</a>\n");

                    // Log the exception and notify system operators
                }
                // Clear the error from the server
                Server.ClearError();
            }
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("Server");
            HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");

            //ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
            //ServicePointManager.DefaultConnectionLimit = 200;
            ServicePointManager.MaxServicePointIdleTime = 5000;
            
            ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls
                   | SecurityProtocolType.Ssl3;

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        void Application_EndRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("Server");
            HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
        }

        void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            Response.Headers.Remove("Server");
            Response.Headers.Set("Server", "My httpd server");
            Response.Headers.Remove("X-Powered-By");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");

            Response.AppendHeader("Cache-Control", "must-revalidate");
            Response.AppendHeader("Cache-Control", "post-check=0"); // HTTP 1.1 
            Response.AppendHeader("Cache-Control", "pre-check=0"); // HTTP 1.1 
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddMonths(-20));
            Response.Cache.SetNoStore();
            Response.Cache.SetMaxAge(new TimeSpan(0, 0, 30));

        }

    }
}