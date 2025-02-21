using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCSHealthFamilyWelfareDept.Models;
using CCSHealthFamilyWelfareDept.DAL;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    public class UmangApplicationController : Controller
    {
        //
        // GET: /UmangApplication/

        AGC_DB objdb = new AGC_DB();
        EncryptionDecryptionUtils objEncDecUti = new EncryptionDecryptionUtils();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegistrationPrintout(string registrationNo = "")
        {  
            AGCModel model = new AGCModel();
            string EncDecKey = System.Configuration.ConfigurationManager.AppSettings["EncDecKey"].ToString();

            try
            {
                // model.registrationNo = objEncDecUti.Encrypt(registrationNo, EncDecKey);
                if (!string.IsNullOrEmpty(registrationNo))
                {
                    var registrationN = objEncDecUti.Decrypt(registrationNo, EncDecKey);
                    model = objdb.GetAGCListBYRegistrationforumang(registrationN);
                }
            }
            catch(Exception ex)
            {
                model = null;
            }

            return View(model);
        }

    }
}
