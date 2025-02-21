using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CCSHealthFamilyWelfareDept.DAL;
using CCSHealthFamilyWelfareDept.Filters;
using CCSHealthFamilyWelfareDept.Models;
using SRVTextToImage;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    public class AdminAccountController : Controller
    {
        //
        // GET: /AdminAccount/

        Account_DB objAccDb = new Account_DB();
        Common_DB objComnDb = new Common_DB();
        Common objComn = new Common();
        SessionManager objSM = new SessionManager();

        #region Method Set Sweet Alert Message
        protected void setSweetAlertMsg(string msg, string msgStatus)
        {
            ViewBag.Msg = msg;
            ViewBag.MsgStatus = msgStatus;
        }
        #endregion

        public FileResult GetCaptchaimage()
        {
            CaptchaRandomImage ci = new CaptchaRandomImage();
            this.Session["capimagetext"] = ci.GetRandomString(5).ToUpper();
            ci.GenerateImage(this.Session["capimagetext"].ToString(), 150, 40, Color.Black, Color.White);
            MemoryStream stream = new MemoryStream();
            ci.Image.Save(stream, ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(stream, "image/png");
        }

        public ActionResult Login()
        {
            ModelState.Clear();

            Random rd = new Random();
            string seed = rd.Next(100000, 999999).ToString();
            objSM.SeedValue = seed;
            AdminLoginModel model = new AdminLoginModel();
            model.seed = seed;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            int passwordLyf = System.Configuration.ConfigurationManager.AppSettings["PasswordLife"] == null ? 90 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PasswordLife"].ToString());
            int allowedWrongAttampt = System.Configuration.ConfigurationManager.AppSettings["AllowedMaxWrongAtt"] == null ? 5 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AllowedMaxWrongAtt"].ToString());

            if (ModelState.IsValid)
            {
                if (Convert.ToString(Session["capimagetext"]) == model.Captcha)
                {
                    string oldseed = model.seed.ToString();

                    UserDetailsModel user = objAccDb.GetAdminDetails(model.UserName).FirstOrDefault();
                    if (user != null)
                    {
                        if (model.Password.ToString().ToUpper().Trim() == FormsAuthentication.HashPasswordForStoringInConfigFile(user.password.ToUpper() + oldseed, "MD5").ToUpper().Trim())
                        {
                            if (user.rollId > 1)
                            {
                                //update login log
                                UserDetailsModel verifiduser = objAccDb.ManageloginAndGetStatus(user.UserId, "S", allowedWrongAttampt);
                                if (verifiduser.Flag == 1)
                                {
                                    DateTime lastLogindate = user.LastLoginDate;
                                    if (verifiduser.isFirstLogin == 0)
                                    {
                                        #region set Sessions
                                        objSM.IsLoginUser = true;
                                        objSM.UserID = user.UserId; //profileid(farwardtoid)
                                        objSM.DisplayName = user.DisplayName;
                                        objSM.ProfilePicPath = user.profilePicPath;
                                        objSM.Username = user.UserName;
                                        objSM.RollID = user.rollId; //farwardtypid(rollid)
                                        objSM.RollName = user.rollName;
                                        objSM.RollAbbrName = user.rollAbbrName;
                                        objSM.RollAbbrNameHi = user.rollAbbrNameHi;
                                        objSM.DistrictName = user.DistrictName;
                                        objSM.DistrictNameHi = user.DistrictNameHi;
                                        objSM.Transdate = Convert.ToString(lastLogindate);
                                        objSM.districtId = user.districtId; 
                                        objSM.ZoneId = user.zoneId;

                                        objSM.DisignationName = user.designation;
                                        

                                        #endregion
                                        return RedirectToAction("AdminDashboard", "Home");
                                    }
                                    else if (verifiduser.isFirstLogin == 1)
                                    {
                                        //force to Change Password

                                        objSM.Username = verifiduser.UserName;
                                        objSM.DisplayName = verifiduser.DisplayName;
                                        objSM.UserIDUserPolcy = verifiduser.UserId;
                                        objSM.RollID = 0;

                                        return RedirectToAction("ChangePassword");
                                    }
                                }
                                else
                                {
                                    setSweetAlertMsg(verifiduser.Msg, "warning");
                                }
                            }
                            else
                            {
                                setSweetAlertMsg("Invalid User. !", "warning");
                            }
                        }
                        else
                        {
                            UserDetailsModel verifiduser = objAccDb.ManageloginAndGetStatus(user.UserId, "F", allowedWrongAttampt);
                            if (verifiduser != null)
                            {
                                setSweetAlertMsg(verifiduser.Msg, "warning");
                            }
                            else
                            {
                                setSweetAlertMsg("Incorrect Password. !", "warning");
                            }
                        }
                    }
                    else
                    {
                        setSweetAlertMsg("Invalid Login Details. !", "warning");
                    }
                }
                else
                {
                    setSweetAlertMsg("Captcha Text is not Valid. !", "warning");
                }
            }
            else
            {
                setSweetAlertMsg("Enter Valid Data !", "warning");
            }

            return View();
        }

        public ActionResult LogOut()
        {
            if (Session["UserExceptionSession"] != null && Session["UserExceptionSession"].ToString() != "")
            {
                ViewBag.mesg = Session["UserExceptionSession"].ToString();
            }
            else
            {
                ViewBag.mesg = "You have successfully Log Out.";
            }

            objSM.RemoveSession();

            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (objSM.UserIDUserPolcy == 0 && objSM.UserID == 0)
            {
                return RedirectToAction("Login");
            }
            
            Random rd = new Random();
            string seed = rd.Next(100000, 999999).ToString();
            objSM.SeedValue = seed;
            PasswordChangeModel model = new PasswordChangeModel();
            model.seed = seed;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(PasswordChangeModel model)
        {
            string oldseed = model.seed.ToString();
            Random rd = new Random();
            string seed = rd.Next(100000, 999999).ToString();
            objSM.SeedValue = seed;
            model.seed = seed;
            if (objSM.UserIDUserPolcy > 0)
            {
                model.UserId = objSM.UserIDUserPolcy;
            }
            else
            {
                model.UserId = objSM.UserID;
            }

            string message = "";

            if (!string.IsNullOrEmpty(model.oldPassword) && !string.IsNullOrEmpty(model.newPassword) && !string.IsNullOrEmpty(model.confirmPassword))
            {
                UserDetailsModel user = objAccDb.GetAdminDetails(objSM.Username).FirstOrDefault();
                if (user != null)
                {
                    if (model.oldPassword.ToString().ToUpper().Trim() == FormsAuthentication.HashPasswordForStoringInConfigFile(user.password.ToUpper() + oldseed, "MD5").ToUpper().Trim())
                    {
                        if (model.newPassword == model.confirmPassword)
                        {
                            if (model.newPassword.ToUpper() != user.password.ToUpper())
                            {
                                int res = 0;
                                model.transIp = Common.GetIPAddress();
                                model.UserId = user.UserId;
                                res = objAccDb.ChangePassword(model);
                                if (res > 0)
                                {
                                    TempData["SuccessMsg"] = "Password Changed Successfully";
                                    if (user.rollId != 1)
                                    {
                                        TempData["ComeFrom"] = "adminarea";
                                    }
                                    return RedirectToAction("Confirmation", "Account");
                                }
                                else
                                {
                                    setSweetAlertMsg("Error in Change Password.", "error");
                                    return View(model);
                                }
                            }
                            else
                            {
                                setSweetAlertMsg("Old Password and new password must differ", "warning");
                                return View(model);
                            }

                        }
                        else
                        {
                            setSweetAlertMsg("Password and confirm Password did not Match", "warning");
                            return View(model);
                        }
                    }
                    else
                    {
                        setSweetAlertMsg("Old Password did not Match", "warning");
                        return View(model);
                    }

                }
            }

            setSweetAlertMsg("Check Your Inputs", "warning");
            return View(model);
        }

        [HttpGet]
        [AuthorizeAdmin]
        public ActionResult UpdatePassword()
        {
            Random rd = new Random();
            string seed = rd.Next(100000, 999999).ToString();
            objSM.SeedValue = seed;
            PasswordChangeModel model = new PasswordChangeModel();
            model.seed = seed;
            model.UserName = objSM.DisplayName;
            return View(model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassword(PasswordChangeModel model)
        {
            string oldseed = model.seed.ToString();

            if (model.newPassword == model.confirmPassword)
            {
                UserDetailsModel user = objAccDb.GetAdminDetails(objSM.Username).FirstOrDefault();
                if (user != null)
                {
                    if (model.oldPassword.ToString().ToUpper().Trim() == FormsAuthentication.HashPasswordForStoringInConfigFile(user.password.ToUpper() + oldseed, "MD5").ToUpper().Trim())
                    {
                        if (model.newPassword == model.confirmPassword)
                        {
                            if (model.newPassword.ToUpper() != user.password.ToUpper())
                            {
                                int res = 0;
                                model.UserId = user.UserId;
                                model.transIp = Common.GetIPAddress();
                                res = objAccDb.ChangePassword(model);
                                if (res > 0)
                                {
                                    TempData["SuccessMsg"] = "Password Changed Successfully";
                                    if (user.rollId != 1)
                                    {
                                        TempData["ComeFrom"] = "adminarea";
                                    }
                                    return RedirectToAction("Confirmation", "Account");
                                }
                                else
                                {
                                    setSweetAlertMsg("Error in Change Password.", "error");
                                    return View(model);
                                }
                            }
                            else
                            {
                                setSweetAlertMsg("Old Password and new password must differ", "warning");
                                return View(model);
                            }

                        }
                        else
                        {
                            setSweetAlertMsg("Password and confirm Password did not Match", "warning");
                            return View(model);
                        }
                    }
                    else
                    {
                        setSweetAlertMsg("Old Password did not Match", "warning");
                        return View(model);
                    }

                }
            }

            setSweetAlertMsg("Check Your Inputs", "warning");
            return View(model);
        }

        //AdminAccount/CreatePassword?rollId=2&prifileId=0
        public ActionResult CreatePassword(long rollId,long prifileId)
        {
            string msg = "Enter Valid Id";
            if (rollId > 0)
            {
                try
                {
                    PasswordChangeModel model = new PasswordChangeModel();
                    var US = objAccDb.GetUsersForCreatePassword(rollId, prifileId);
                    int a = 0;
                    if (US != null && US.Count > 0)
                    {
                        foreach (var itm in US)
                        {
                            model.UserId = itm.UserId;
                            string passw = itm.newPassword;
                            model.newPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(passw, "MD5").ToString().ToUpper();
                            model.transIp = Common.GetIPAddress();
                            int i = objAccDb.CreatePasswordByAdminFirstTime(model);
                            a = a + i;
                        }
                    }

                    if (a > 0)
                    {
                        msg = a + " Users Password has been Change Successfully";
                    }
                    else
                    {
                        msg = "ER";
                    }
                }
                catch (Exception ex)
                {
                    msg = "EX";
                }
            }
            TempData["msg"] = msg;
            return View();
        }
    }
}
