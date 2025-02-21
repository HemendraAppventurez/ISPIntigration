using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;
using CCSHealthFamilyWelfareDept.DAL;
using CCSHealthFamilyWelfareDept.Filters;
using CCSHealthFamilyWelfareDept.Models;

namespace CCSHealthFamilyWelfareDept.Controllers
{ 
    public class UserManagementController : Controller
    {
        //
        // GET: /UserManagement/

        Common_DB objComDB = new Common_DB();
        Common objComn = new Common();
        UserManagement_DB objUMDB = new UserManagement_DB();
        SessionManager objSM = new SessionManager();
        Account_DB objAccDb = new Account_DB();

        #region Method Set Sweet Alert Message
        protected void setSweetAlertMsg(string msg, string msgStatus)
        {
            ViewBag.Msg = msg;
            ViewBag.MsgStatus = msgStatus;
        }
        #endregion

        [AuthorizeAdmin(22)]
        public ActionResult UserManagement()
        {
            return View();
        }

        [AuthorizeAdmin(23)]
        public ActionResult CreateUser()
        {
            if (objSM.RollID != 2)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            ViewBag.State = objComDB.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComDB.GetDropDownList(7, 34).Where(m => m.Id == objSM.districtId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.CategoryDropdownList = objComDB.GetDropDownList(8, 0).Select(e => new SelectListItem() { Text = e.Name, Value = e.Id.ToString() });
            ViewBag.IdProof = objComDB.GetDropDownList(35, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return View();
        }

        #region Method - check user id existence

        [HttpPost]
        public JsonResult CheckUserIdExistence(string userId)
        {
            var user = objUMDB.CheckUserIdExistence(userId);
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        #endregion

        [AuthorizeAdmin(23)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(UserModel model)
        {
            string password = "";

            if (objSM.RollID != 2)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            if (ModelState.IsValid)
            { 
                password = model.password;
                //model.DTDob = Convert.ToDateTime(model.dob);
                model.password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.password, "MD5").ToUpper().Trim();
                model.transIp = Common.GetIPAddress();
                model.insertBy = objSM.UserID;
                model.rollId = objSM.RollID;

                try
                {
                    model = objUMDB.CreateUser(model).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    setSweetAlertMsg("Error in registering !", "error");
                }
            }
            else
            {
                setSweetAlertMsg("Enter Valid Data !", "warning");
            } 

            if (model.queryExeFlag == 1)
            {
                TempData["Message"] = model.msg;
                SendSMS(model.userName, password, model.mobileNo);
                return RedirectToAction("CreateUser");
            }
            else
            {
                ViewBag.State = objComDB.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.District = objComDB.GetDropDownList(7, 34).Where(m => m.Id == objSM.districtId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.CategoryDropdownList = objComDB.GetDropDownList(8, 0).Select(e => new SelectListItem() { Text = e.Name, Value = e.Id.ToString() });
                return View();
            } 
        }

        #region Method - Send SMS
        void SendSMS(string userName, string password, string mobileNo)
        {
            ForgotPasswordModel otpChCount = new ForgotPasswordModel();
            string txtmsg = "Congratulations\n\nYour account for HFWD has been created. Kindly use below credentials for login purpose:\nUser Id: " + userName + "\nPassword: " + password + "\nRegards,HFWD";
            if (!string.IsNullOrEmpty(mobileNo) && !string.IsNullOrEmpty(txtmsg))
            {
                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, userName);
            }
        }
        #endregion

        [AuthorizeAdmin(23)]
        public ActionResult ViewUser()
        {
            return View();
        }

        [AuthorizeAdmin]
        public ActionResult ViewUserList(string name = "", string mobileNo = "")
        {
            if (objSM.UserID > 0)
            {
                var userList = objUMDB.GetUsersList(objSM.UserID, name, mobileNo);
                return PartialView("_ViewUserList", userList);
            }
            else
            {
                return Content("TO");
            }
        }

        [AuthorizeAdmin(23)]
        public ActionResult ServicePermission(string userId)
        {
            long longUserId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(userId));
            ViewBag.UserId = longUserId;

            UserDetailsModel model = new UserDetailsModel();

            model = objUMDB.GetUsersList(objSM.UserID, "", "").Where(m => m.profileId == longUserId).FirstOrDefault();

            if (model != null)
            {
                model.PermissionList = objUMDB.GetServicePermission(objSM.RollID, longUserId);

                return View(model);
            }
            else
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }
        }

        [AuthorizeAdmin]
        public ActionResult UserDetails(long userId)
        {
            var userDetails = objUMDB.GetUsersList(objSM.UserID, "", "").Where(m => m.profileId == userId).FirstOrDefault();
            return PartialView("_UserDetails", userDetails);
        }

        [HttpPost]
        public ActionResult ServicePermission(FormCollection fc)
        {
            int result = 0, exception = 0;
            string transIp = Common.GetIPAddress();
            string services = string.IsNullOrEmpty(fc["hdnServicesXML"]) ? "" : Convert.ToString(fc["hdnServicesXML"]);
            XmlDocument xmlDoc = objComn.ConvertJSONToXML(services, "Services", "Service");
            string servicesXML = xmlDoc.InnerXml;
            long userId = string.IsNullOrEmpty(fc["hdnUserId"]) ? 0 : Convert.ToInt64(fc["hdnUserId"]);
            
            if (userId > 0)
            {
                try
                {
                    result = objUMDB.InsertServicePermission(userId, transIp, servicesXML);
                }
                catch (Exception)
                {
                    TempData["Message"] = "Error in saving.";
                    TempData["MsgStatus"] = "error";
                    exception = 1;
                }

                if (result > 0)
                {
                    TempData["Message"] = "Permission Granted.";
                    return RedirectToAction("ViewUser");
                }
                else if (exception == 0)
                {
                    TempData["Message"] = "Problem in submitting.";
                    TempData["MsgStatus"] = "warning";
                }
            }
            else
            {
                TempData["Message"] = "Problem in submitting.";
                TempData["MsgStatus"] = "warning";
            }

            return RedirectToAction("ServicePermission", new { userId = OTPL_Imp.CustomCryptography.Encrypt(userId.ToString()) });
        }

        [AuthorizeAdmin(26)]
        public ActionResult CMOUser()
        {
            var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;

            ViewBag.District = objComDB.GetDropDownList(7, 34).Where(m => (lstCMODistrict.Any(p => p.districtId == m.Id))).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            
            return View();
        }

        [AuthorizeAdmin]
        public ActionResult CMOUserList(string districtId = "", string userName = "")
        {
            if (objSM.UserID > 0)
            {
                int intDisrictId = string.IsNullOrEmpty(districtId) ? 0 : Convert.ToInt32(districtId);
                var userList = objUMDB.GetUserProfileAD(objSM.UserID, 2, intDisrictId, "", userName);
                return PartialView("_CMOUserList", userList);
            }
            else
            {
                return Content("TO");
            }
        }

        [AuthorizeAdmin(27)]
        public ActionResult CMSUser()
        {
            var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;

            ViewBag.District = objComDB.GetDropDownList(7, 34).Where(m => (lstCMODistrict.Any(p => p.districtId == m.Id))).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            return View();
        }

        [AuthorizeAdmin]
        public ActionResult CMSUserList(string districtId = "", string dhName = "", string userName = "")
        {
            if (objSM.UserID > 0)
            {
                int intDisrictId = string.IsNullOrEmpty(districtId) ? 0 : Convert.ToInt32(districtId);
                var userList = objUMDB.GetUserProfileAD(objSM.UserID, 5, intDisrictId, dhName, userName);
                return PartialView("_CMSUserList", userList);
            }
            else
            {
                return Content("TO");
            }
        }

        [AuthorizeAdmin(28)]
        public ActionResult CHCUser()
        {
            var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;

            ViewBag.District = objComDB.GetDropDownList(7, 34).Where(m => (lstCMODistrict.Any(p => p.districtId == m.Id))).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            return View();
        }

        [AuthorizeAdmin]
        public ActionResult CHCUserList(string districtId = "", string chcName = "", string userName = "")
        {
            if (objSM.UserID > 0)
            {
                int intDisrictId = string.IsNullOrEmpty(districtId) ? 0 : Convert.ToInt32(districtId);
                var userList = objUMDB.GetUserProfileAD(objSM.UserID, 3, intDisrictId, chcName, userName);
                return PartialView("_CHCUserList", userList);
            }
            else
            {
                return Content("TO");
            }
        }

        [AuthorizeAdmin(29)]
        [HttpGet]
        public ActionResult ChangePassword(string profileId = "", string roll = "")
        {
            int rollId = 0;

            if (!string.IsNullOrEmpty(roll))
            {
                rollId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(roll));
            }

            var userDetails = objUMDB.GetUserProfileAD(objSM.UserID, rollId, 0, "", "", Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(profileId))).FirstOrDefault();

            if (userDetails != null)
            {
                PasswordModel model = new PasswordModel();
                model.profileId = profileId;
                model.UserName = @OTPL_Imp.CustomCryptography.Encrypt(userDetails.userName);

                model.ActionTo = rollId == 2 ? "CMOUser" : rollId == 3 ? "CHCUser" : rollId == 5 ? "CMSUser" : "";

                return View(model);
            }
            else
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }
        }

        [AuthorizeAdmin(29)]
        [HttpPost] 
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(PasswordModel model)
        { 
            if (model.newPassword == model.confirmPassword)
            {
                UserDetailsModel user = objAccDb.GetAdminDetails(OTPL_Imp.CustomCryptography.Decrypt(model.UserName)).FirstOrDefault();
                if (user != null)
                { 
                    if (model.newPassword == model.confirmPassword)
                    {
                        if (model.newPassword.ToUpper() != user.password.ToUpper())
                        {
                            int res = 0;
                            model.UserId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(model.profileId));//user.UserId;
                            model.transIp = Common.GetIPAddress();
                            res = objUMDB.ChangePassword(model);
                            if (res > 0)
                            {
                                TempData["SuccessMsg"] = "Password Changed Successfully";

                                return RedirectToAction(model.ActionTo, "UserManagement");
                            }
                            else
                            {
                                setSweetAlertMsg("Error in Change Password.", "error");
                                return View(model);
                            }
                        }
                        else
                        {
                            setSweetAlertMsg("Entered Password is old password, Kindly enter another Password", "warning");
                            return View(model);
                        } 
                    }
                    else
                    {
                        setSweetAlertMsg("Password and confirm Password did not Match", "warning");
                        return View(model);
                    }
                }
            }

            setSweetAlertMsg("Check Your Inputs", "warning");
            return View(model);
        }

        [AuthorizeAdmin(30)]
        public ActionResult Committee(string userId = "")
        {
            CommitteeModel model = new CommitteeModel();

            if (userId != "")
            {
                model.commMemId = Convert.ToInt64(@OTPL_Imp.CustomCryptography.Decrypt(userId));
                model = objUMDB.GetCommitteeMemberById(model.commMemId);
                ViewBag.btntext = "UPDATE";
            }
            else
            {
                ViewBag.btntext = "SAVE";
            }

            if (model != null && model.userId > 0 && model.userId != objSM.UserID)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            return View(model); 
        }

        [AuthorizeAdmin(30)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Committee(CommitteeModel model)
        {
            model.userId = objSM.UserID;
            model.transIp = Common.GetIPAddress();

            var resultData = objUMDB.GetCommitteeMemberById(model.commMemId);
            if (resultData != null && resultData.userId != objSM.UserID)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }
             
            var res = objUMDB.InsertCommittee(model);
            try
            {
                if (res.Flag == 1)
                {
                    TempData["Message"] = res.Msg;
                    return RedirectToAction("Committee");
                }
                else
                {
                    setSweetAlertMsg(res.Msg , "error");
                }
            }
            catch (Exception E)
            {
                setSweetAlertMsg("Record not save ", "error");
            }
            return View();

        }

        [AuthorizeAdmin(30)]
        [HttpGet]
        public ActionResult ViewCommittee()
        {
            return View();
        }

        [AuthorizeAdmin]
        [HttpGet]
        public ActionResult ViewCommitteeMemberList(string name = "", string designation = "")
        {

            if (objSM.UserID > 0)
            {
                var userList = objUMDB.GetCommitteeMemberList(objSM.UserID,name,designation);
                return PartialView("_ViewCommitteeMemberList", userList);
            }
            else
            {
                return Content("TO");
            }
        }

        [AuthorizeAdmin]
        [HttpPost]
        public ActionResult ActiveDeactiveMember(string userId, string active)
        {
            if (Request.IsAjaxRequest())
            {
                CommitteeModel model = new CommitteeModel();
                model.commMemId = Convert.ToInt64(@OTPL_Imp.CustomCryptography.Decrypt(userId));
                string isActive = @OTPL_Imp.CustomCryptography.Decrypt(active);
                model.isActive = Convert.ToBoolean(isActive);
                try
                {
                    var result = objUMDB.ActiveDeactiveMember(model.commMemId, model.isActive);
                    return Json(new { res = result.Flag, msg = result.Msg });
                }
                catch
                {
                    return Json(new { res = 0, msg = "Fail to process the request" });
                }
            }
            else
            {
                return Content("Access Not Allowed");
            }
        }

        [AuthorizeAdmin(32)]
        [HttpGet]
        public ActionResult ManageAccount()
        {
            return View();
        }

        [AuthorizeAdmin(32)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageAccount(string userId = "", string actionName = "")
        {
            string msg = "";

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(actionName))
            {
                long accountId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(userId));

                string password = objComn.GetPlainPassword();
                string encPassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5").ToUpper().Trim();

                var result = objUMDB.ResetAccount(accountId, actionName.ToLower(), encPassword, objSM.UserID, Common.GetIPAddress());

                if (result > 0)
                {
                    if (actionName.ToLower() == "unlock")
                    {
                        msg = "success_1";
                    }
                    else
                    {
                        msg = "success_2_" + password;
                    }
                }
                else
                {
                    msg = "warning_aaa";
                } 
            }
            else
            {
                msg = "warning_invalid";
            }

            return Content(msg);
        }

        [AuthorizeAdmin]
        [HttpGet]
        public ActionResult AccountDetails(string userName = "")
        {
            var userList = objUMDB.GetAccountDetailsByUsername(userName, objSM.RollID, objSM.UserID);
            return PartialView("_AccountDetails", userList);
        }
    }
}
