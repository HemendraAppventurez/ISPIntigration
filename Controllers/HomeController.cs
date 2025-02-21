using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCSHealthFamilyWelfareDept.DAL;
using CCSHealthFamilyWelfareDept.Filters;
using CCSHealthFamilyWelfareDept.Models;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    public class HomeController : Controller
    {
        Common_DB comnDB = new Common_DB();
        SessionManager SM = new SessionManager();
        UserManagement_DB objUMDB = new UserManagement_DB();

        [AuthorizeUser]
        public ActionResult Dashboard()
        {

            return View();
        }

        [AuthorizeAdmin]
        public ActionResult AdminDashboard()
        {
            var lstCMODistrict = comnDB.GetCMODistrictMapping(SM.UserID);
            Session["CMODistricts"] = lstCMODistrict;
            var lstUserPermission = objUMDB.GetServicePermission(0, SM.UserID);
            Session["UserPermission"] = lstUserPermission;
            return View();
        }

        [AuthorizeAdmin]
        public ActionResult UnauthoriseAcess(bool isWithoutLayout = false)
        {
            ViewBag.IsWithoutLayout = isWithoutLayout;
            return View();
        }

        [AuthorizeUser]
        public ActionResult UserUnauthoriseAcess(bool isWithoutLayout = false)
        {
            ViewBag.IsWithoutLayout = isWithoutLayout;
            return View();
        }

        [AuthorizeAdmin]
        public ActionResult DigitalSignatureVideo()
        {
            if (SM.RollID == 2 || SM.RollID == 3 || SM.RollID == 4 || SM.RollID == 5)
            {
                return View();
            }
            else
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }
        }

        #region Method Download File By Path
        [AuthorizeAdmin]
        public void DownloadFileByPath(string filePath)
        {
            if (SM.RollID == 2 || SM.RollID == 3 || SM.RollID == 4 || SM.RollID == 5)
            {
                string decrypFilePath = OTPL_Imp.CustomCryptography.Decrypt(Server.UrlDecode(filePath));

                if (System.IO.File.Exists(Server.MapPath(decrypFilePath)))
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(Server.MapPath(decrypFilePath));
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(decrypFilePath));
                    Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
                    Response.WriteFile(decrypFilePath);
                    Response.Flush();
                    Response.Close();
                }
            }
        }
        #endregion
    }
}
