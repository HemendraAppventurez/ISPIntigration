using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCSHealthFamilyWelfareDept.Models;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    public class EDistrictController : Controller
    {
        SessionManager objSM = new SessionManager();
        //
        // GET: /EDistrict/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Book()
        {
            objSM.AppRequestKey = null;
            if (this.Request.UrlReferrer != null && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
            {
                if (this.Request.UrlReferrer.ToString().StartsWith(System.Configuration.ConfigurationManager.AppSettings["EDistrictIP"].ToString()))
                {
                    string RequestKey = Convert.ToString(this.HttpContext.Request.Form["RequestKey"]);

                    //System.IO.File.AppendAllText(this.HttpContext.Server.MapPath("~\\Content\\ReadWriteData\\ErrorLog\\brdcrmsv" + DateTime.Now.Ticks.ToString() + ".txt"), RequestKey);

                    objSM.AppRequestKey = RequestKey;
                }
                else
                {
                    //System.IO.File.AppendAllText(this.HttpContext.Server.MapPath("~\\Content\\ReadWriteData\\ErrorLog\\brdcrmsv" + DateTime.Now.Ticks.ToString() + ".txt"), "Wrong Domain");

                    this.Response.Redirect(this.Request.UrlReferrer.ToString());
                }
                ViewBag.AllowEDistrict = "Y";
            }
            else
            {
                ViewBag.AllowEDistrict = "N";
                //System.IO.File.AppendAllText(this.HttpContext.Server.MapPath("~\\Content\\ReadWriteData\\ErrorLog\\brdcrmsv" + DateTime.Now.Ticks.ToString() + ".txt"), "Direct");
            }

            //string tmpv = "";

            //foreach (var x in this.HttpContext.Request.Form.AllKeys)
            //{
            //    tmpv = tmpv + x + "=" + this.HttpContext.Request.Form[x] + "_";
            //}

            //System.IO.File.AppendAllText(this.HttpContext.Server.MapPath("~\\Content\\ReadWriteData\\ErrorLog\\brdcrmsv.txt"), tmpv);

            //string RequestKey = this.HttpContext.Request.Form["RequestKey"].ToString();

            //objSM.AppRequestKey = RequestKey;

            return View();
        }

        public ActionResult Select(string command)
        {
            ModelState.Clear();

            EDistrictServiceClass eds = new EDistrictServiceClass();
            
            string RequestKey = objSM.AppRequestKey;

            if (string.IsNullOrEmpty(RequestKey))
            {
                ModelState.AddModelError("", "Invalid Request or Service Unavailable");
                
                return View();
            }
            else
            {
                try
                {
                    if (command == "NUH")
                    {
                        bool sts = eds.sendServiceRequest(RequestKey);

                        if (sts)
                        {
                            objSM.AppRequestKey = RequestKey;

                            return RedirectToAction("Login", "Account", new { appSlug = command });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Request or Service Unavailable");
                        }
                    }
                    else if (command == "ILC")
                    {
                        bool sts = eds.sendServiceRequest(RequestKey);

                        if (sts)
                        {
                            objSM.AppRequestKey = RequestKey;
                            
                            return RedirectToAction("Login", "Account", new { appSlug = command });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Request or Service Unavailable");
                        }
                    }
                    else if (command == "FIC")
                    {
                        bool sts = eds.sendServiceRequest(RequestKey);

                        if (sts)
                        {
                            objSM.AppRequestKey = RequestKey;

                            return RedirectToAction("Login", "Account", new { appSlug = command });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Request or Service Unavailable");
                        }
                    }
                    else if (command == "DIC")
                    {
                        bool sts = eds.sendServiceRequest(RequestKey);

                        if (sts)
                        {
                            objSM.AppRequestKey = RequestKey;

                            return RedirectToAction("Login", "Account", new { appSlug = command });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Request or Service Unavailable");
                        }
                    }
                    else if (command == "IMC")
                    {
                        bool sts = eds.sendServiceRequest(RequestKey);

                        if (sts)
                        {
                            objSM.AppRequestKey = RequestKey;

                            return RedirectToAction("Login", "Account", new { appSlug = command });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Request or Service Unavailable");
                        }
                    }
                    else if (command == "DEC")
                    {
                        bool sts = eds.sendServiceRequest(RequestKey);

                        if (sts)
                        {
                            objSM.AppRequestKey = RequestKey;

                            return RedirectToAction("Login", "Account", new { appSlug = command });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Request or Service Unavailable");
                        }
                    }
                    else if (command == "FAP")
                    {
                        bool sts = eds.sendServiceRequest(RequestKey);

                        if (sts)
                        {
                            objSM.AppRequestKey = RequestKey;

                            return RedirectToAction("Login", "Account", new { appSlug = command });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Request or Service Unavailable");
                        }
                    }
                    else if (command == "MER")
                    {
                        bool sts = eds.sendServiceRequest(RequestKey);

                        if (sts)
                        {
                            objSM.AppRequestKey = RequestKey;

                            return RedirectToAction("Login", "Account", new { appSlug = command });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Request or Service Unavailable");
                        }
                    }
                    else if (command == "MLC")
                    {
                        bool sts = eds.sendServiceRequest(RequestKey);

                        if (sts)
                        {
                            objSM.AppRequestKey = RequestKey;

                            return RedirectToAction("Login", "Account", new { appSlug = command });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Request or Service Unavailable");
                        }
                    }
                    else if (command == "AGC")
                    {
                        bool sts = eds.sendServiceRequest(RequestKey);

                        if (sts)
                        {
                            objSM.AppRequestKey = RequestKey;

                            return RedirectToAction("Login", "Account", new { appSlug = command });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Request or Service Unavailable");
                        }
                    }
                    else if (command == "ICC")
                    {
                        bool sts = eds.sendServiceRequest(RequestKey);

                        if (sts)
                        {
                            objSM.AppRequestKey = RequestKey;

                            return RedirectToAction("Login", "Account", new { appSlug = command });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Request or Service Unavailable");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Service Unavailable - " + ex.Message);
                }
            }

            return View();
        }

    }
}
