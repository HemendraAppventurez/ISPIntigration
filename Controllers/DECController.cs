using CCSHealthFamilyWelfareDept.DAL;
using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCSHealthFamilyWelfareDept.Filters;
using System.Configuration;
using System.IO;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    [AuthorizeUser]
    public class DECController : Controller
    {
        Common_DB comdb = new Common_DB();
        DEC_DB objdb = new DEC_DB();
        Common objComn = new Common();
        SessionManager objSM = new SessionManager();
        Account_DB objAccDb = new Account_DB();
        #region death certificate riya
        #region Method Set Sweet Alert Message
        protected void setSweetAlertMsg(string msg, string msgStatus)
        {
            ViewBag.Msg = msg;
            ViewBag.MsgStatus = msgStatus;
        }
        #endregion
        #region checkfor Registration

        public ActionResult isRegister()
        {
            //var res = objdb.IsRegister();
            //if (res.isExists == 1)
            //{
            //    return RedirectToAction("DECDashBoard", "DEC");
            //}
            //else if (res.isExists == 0)
            //{
            //    return RedirectToAction("RegistrationInstructions", "DEC");
            //}
            return View();
        }
        #endregion
        public ActionResult RegistrationInstructions()
        {
            return View();
        }
        public ActionResult DECDashBoard()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult BindForwardDropdownlist(long rollId, int opdDistrictId)//used in NomineeSearchDEC
        {
            var res = objdb.bindDropdownlist(rollId, opdDistrictId).Select(m => new SelectListItem { Text = m.forwardtoName, Value = m.forwardtoId.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DeathCertificate()
        {
            ViewBag.maritalStatus = comdb.GetDropDownList(19, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Religion = comdb.GetDropDownList(20, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = comdb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = comdb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.forwardTo = Enumerable.Empty<SelectListItem>();
            ViewBag.forwardTypes = objdb.rblforwardType().ToList().Where(m => m.forwardtypeId != 4);
            return View();
        }

        [HttpPost]
        public JsonResult CheckMobileExistence(string mobileNo)
        {
            var user = objdb.CheckEmailMobileExistence(mobileNo, "M");
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [HttpPost]
        public ActionResult DeathCertificate(DECModel model)
        {

            //if (model.isCauseCertified == false)
            //{
            //    ModelState["diseaseNameOrCause"].Errors.Clear();
            //}

            if (objComn.IsValidDateFormat(model.dod))
            {
                long userId = objSM.UserID;
                model.regByusers = userId;
                model.regBytransIp = Common.GetIPAddress();
                model.transIp = Common.GetIPAddress();
                model.requestKey = objSM.AppRequestKey;

                var res = objdb.DECInsertUpdate(model);

                try
                {
                    if (res.RegisIdDEC > 0 && res.RegistrationNo != "")
                    {
                        if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                        {
                            EDistrictServiceClass ed = new EDistrictServiceClass();

                            int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.DEC);

                            bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "DEC");

                            if (result)
                            {
                                 SendSMS(res.RegistrationNo, res.MobileNo); 
                                TempData["RegisIdDEC"] = res.RegisIdDEC;
                                TempData["RegistrationNo"] = res.RegistrationNo;
                                return RedirectToAction("RegistrationConfirmation");
                            }
                            else
                            {
                                int exeRsult = objdb.DeleteRegistrationDEC(res.RegisId);
                                setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                ModelState.Clear();
                            }
                        }
                        else
                        {
                            SendSMS(res.RegistrationNo, res.MobileNo);
                            TempData["RegisIdDEC"] = res.RegisIdDEC;
                            TempData["RegistrationNo"] = res.RegistrationNo;
                            return RedirectToAction("RegistrationConfirmation");
                        }
                    }
                    else
                    {
                        setSweetAlertMsg(res.Msg.ToString(), "error");
                        ModelState.Clear();
                    }
                }
                catch (Exception E)
                {
                    setSweetAlertMsg("Record not save ", "error");
                }
            }
            else
            {
                setSweetAlertMsg("Date of Death is in incorrect format, Date format should be DD/MM/YYYY !", "warning");
            }

            ViewBag.maritalStatus = comdb.GetDropDownList(19, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Religion = comdb.GetDropDownList(20, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = comdb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = comdb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.forwardTo = Enumerable.Empty<SelectListItem>();
            ViewBag.forwardTypes = objdb.rblforwardType().ToList().Where(m => m.forwardtypeId != 4);
            ViewBag.Save = "Submit";
            ViewBag.Reset = "Cancel";



            return RedirectToAction("DeathCertificate");
        }
        public ActionResult ViewDECList()
        {

            long regisByusers = objSM.UserID;
            DECModel model = new DECModel();
            model.DECModelList = objdb.GetDECList(regisByusers);
            return View(model.DECModelList);

        }
        public ActionResult DECDetails(string registrationNo, string regisIdDEC)
        {
            long regisByuser = objSM.UserID;
            registrationNo = OTPL_Imp.CustomCryptography.Decrypt(registrationNo);
            Session["DEC_registration"] = registrationNo;
            Session["regisIdDEC"] = regisIdDEC;
            DECModel model = new DECModel();
            model = objdb.GetDECListBYRegistrationNo(regisByuser, registrationNo);

            return View(model);
        }
        public ActionResult PrintApplicationForm(string registrationNo, string regisIdDEC)
        {
            long regisByuser = objSM.UserID;
            //var resultData = objdb.GetRegisterDetails();
            //registrationNo = resultData.registrationNo.ToString();
            registrationNo = OTPL_Imp.CustomCryptography.Decrypt(registrationNo);
            Session["DEC_registration"] = registrationNo;
            Session["regisIdDEC"] = regisIdDEC;
            DECModel model = new DECModel();
            model = objdb.GetDECListBYRegistrationNo(regisByuser, registrationNo);

            return View(model);
        }
        public ActionResult RegistrationConfirmation()
        {
            ViewBag.DECRegisId = OTPL_Imp.CustomCryptography.Encrypt(TempData["RegisIdDEC"].ToString());
            ViewBag.DECRegistrationNo = TempData["RegistrationNo"];
            return View();
        }
        void SendSMS(string registrationNo, string mobileNo)
        {

            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";

                //txtmsg = "Dear Citizen,\n\nYour Application form has been submitted successfully. Your Application Form Number is " + registrationNo + ", kindly use this further.\n\n Thanks";
                txtmsg = "Dear Citizen,\n\nYour Application form for Issuance of Death Certificate has been submitted successfully. Your Application Number is " + registrationNo + ".\n Please use this Application Number for further references.\n\nTechnical Team\nMHFWD, UP";

                //string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo)); commented on 19/07/2023

                //objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }

        }
        #endregion

        #region Death certificate Nominee By Riya
        public ActionResult NomineeSearchDEC()
        {
            ViewBag.District = comdb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.forwardTo = Enumerable.Empty<SelectListItem>();
            ViewBag.forwardTypes = objdb.rblforwardType().ToList();
            return View();
        }
        public ActionResult NomineeSearchListDEC(string forwardtypeId = "", string healthUnitDistrictId = "", string forwardtoId = "", string deathPersonName = "", string dod = "", string DeathPersonGender = "")
        {
            List<DECModel> DECModelList = new List<DECModel>();

            DECModelList = objdb.GetAllNomineeSearchListDEC(Convert.ToInt64(forwardtypeId), Convert.ToInt32(healthUnitDistrictId), Convert.ToInt64(forwardtoId), deathPersonName, dod, DeathPersonGender).ToList();

            return PartialView("_NomineeSearchListDEC", DECModelList);
        }
        public ActionResult NomineeDEC(string ApplicationIds)
        {
            string regisIdDEC = ApplicationIds;
            //TempData["BookingIds"] = ApplicationIds;

            return RedirectToAction("NomineeDetailDEC", new { regisIdDEC = @OTPL_Imp.CustomCryptography.Encrypt(regisIdDEC) });
        }
        public ActionResult NomineeDetailDEC(string regisIdDEC)
        {
            string FilePath = "";
            DECModel model = new DECModel();
            Session["regisIdDEC"] = regisIdDEC;
            if (TempData["uploadCertificatePath"] != null)
            {
                FilePath = TempData["uploadCertificatePath"].ToString();
            }
            ViewBag.Relation = comdb.GetDropDownList(2, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.IdProof = comdb.GetDropDownList(35, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            model.regisIdDEC = Convert.ToInt64(@OTPL_Imp.CustomCryptography.Decrypt(regisIdDEC));
            model = objdb.getUserDetail(model.regisIdDEC, objSM.UserID);
            model.regisIdDEC = Convert.ToInt64(@OTPL_Imp.CustomCryptography.Decrypt(regisIdDEC));
            model.uploadCertificatePath = FilePath;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NomineeDetailDEC(DECModel model)
        {
            long userId = objSM.UserID;
            model.regByusers = userId;
            model.transIp = Common.GetIPAddress();


            var res = objdb.DECInsertUpdateNomineeDetail(model);

            if (res != null && res.Flag == 1)
            {
                string mobVerify = "";
                Session["RegisId"] = @OTPL_Imp.CustomCryptography.Encrypt(Convert.ToString(res.RegisId));
                Session["mobile"] = res.MobileNo;
                string smsFlag = SendOTP();
                if (smsFlag == "S")
                {
                    mobVerify = "OTP is sent to your registered Mobile No.";
                    return RedirectToAction("VerifyMobile", new { userName = @OTPL_Imp.CustomCryptography.Encrypt(res.userName), type = @OTPL_Imp.CustomCryptography.Encrypt("fresh"), RegisIdDEC = @OTPL_Imp.CustomCryptography.Encrypt("no"), RegisId = @OTPL_Imp.CustomCryptography.Encrypt("no") });
                }
                else if (smsFlag == "OCE")
                {

                    TempData["msg"] = "You have crossed maximum limit for today, Kindly try after 24 hour .";
                    TempData["msgstatus"] = "warning";
                    return RedirectToAction("DECDashBoard");

                }
                else
                {
                    mobVerify = "SMS Service is not working";
                }
            }
            return RedirectToAction("NomineeDetailDEC");
        }

        public ActionResult VerifyMobile(string userName, string type, string RegisIdDEC, string RegisId)
        {
            type = @OTPL_Imp.CustomCryptography.Decrypt(type);
            if (type == "regen")
            {

                userName = @OTPL_Imp.CustomCryptography.Decrypt(userName);

                Session["mobile"] = userName;
                Session["regisIdDEC"] = RegisIdDEC;
                Session["RegisId"] = RegisId;
                string smsFlag = SendOTP();
                if (smsFlag == "OCE")
                {

                    TempData["msg"] = "You have reached your maximum limit of sending OTP for Date :" + DateTime.Now.ToString("dd/MM/yyyy") + ", please try later.";
                    TempData["msgstatus"] = "warning";
                    return RedirectToAction("DECDashBoard");

                }
            }
            if (type == "ReSendOTP")
            {
                string smsFlag = SendOTP();
                if (smsFlag == "OCE")
                {

                    TempData["msg"] = "You have reached your maximum limit of sending OTP for Date :" + DateTime.Now.ToString("dd/MM/yyyy") + ", please try later.";
                    TempData["msgstatus"] = "warning";
                    return RedirectToAction("DECDashBoard");

                }

            }
            ViewBag.PageHeading = "Mobile Verification";
            userName = @OTPL_Imp.CustomCryptography.Decrypt(userName);
            DECotpModel model = new DECotpModel();

            try
            {
                model = objdb.GetUserDetailsDEC(Session["mobile"].ToString(), 1).FirstOrDefault();

            }
            catch (Exception ex)
            {

                //  return RedirectToAction("Login");
            }

            if (model != null)
            {
                objSM.UserID = model.UserId;

                objSM.Username = model.userName;
                objSM.DisplayName = model.DisplayName;
                objSM.MobileNumber = model.mobileNo;


            }

            string mobVerify = "";

            setSweetAlertMsg(mobVerify, "success");

            return View(model);
        }
        [HttpPost]
        public ActionResult VerifyMobile(DECotpModel model)
        {
            ModelState.Clear();


            if (objSM.isMSGSend)
            {
                if (objSM.OTP == model.Opt)
                {

                    TempData["SuccessMsg"] = "Mobile Verified Successfully";

                    string regisIdDEC = Session["regisIdDEC"].ToString();
                    Int64 IDDEC = Convert.ToInt64(@OTPL_Imp.CustomCryptography.Decrypt(regisIdDEC));

                    string regisIdNominee = Session["RegisId"].ToString();
                    Int64 IDNomineeDEC = Convert.ToInt64(@OTPL_Imp.CustomCryptography.Decrypt(regisIdNominee));


                    var res = objdb.InsertNomineeLog(IDNomineeDEC, IDDEC, "Download");

                    if (res != null)
                    {
                        TempData["uploadCertificatePath"] = res.Msg;
                        string id = Convert.ToString(res.RegisId);
                        return RedirectToAction("NomineeDetailDEC", new { regisIdDEC = @OTPL_Imp.CustomCryptography.Encrypt(id) });
                    }

                }
                else
                {
                    setSweetAlertMsg("OTP is not Valid please enter a valid OTP.", "warning");
                }
            }
            return View(model);
        }
        [HttpGet]
        public string SendOTP()
        {
            string res = "";
            string otp = generateno();
            objSM.OTP = otp;

            if (Session["mobile"].ToString() != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                int MaxAllowedSMS = System.Configuration.ConfigurationManager.AppSettings["MaxAllowedSMS"] == null ? 5 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxAllowedSMS"].ToString());
                otpChCount.UserId = objSM.UserID;
                otpChCount.MobileNo = Session["mobile"].ToString();
                otpChCount.flag = 4;
                var otpCount = objAccDb.OTPVarification(otpChCount).ToList().FirstOrDefault();
                if (otpCount != null && otpCount.otpCount <= MaxAllowedSMS)
                {
                    objSM.IsMaxLimit = false;
                    // string txtmsg = "Dear User,\n\nYour OTP for Death Certificate on the portal of Medical Health And Family Welfare Department (MHFWD), U.P is " + otp + ".\nPlease note that this OTP will be valid till next 15 minutes.\nPlease do not share this OTP with anyone for security reasons.\n\n Technical Team\nMHFWD, UP";
                    string txtmsg = "Your OTP for DeathCertificate is " + otp + "-OMNINET TECHNOLOGIES PVT LTD";

                    string status = SMS.SMSSend(txtmsg, Session["mobile"].ToString(), "1607100000000032462");

                    objAccDb.SMSLog(txtmsg, Session["mobile"].ToString(), status, Convert.ToString(objSM.UserID));
                    if (status.ToLower() == "success")
                    {
                        res = "S";
                        try
                        {
                            ForgotPasswordModel model1 = new ForgotPasswordModel();
                            model1.UserId = objSM.UserID;
                            model1.MobileNo = Session["mobile"].ToString();
                            model1.otp = Convert.ToInt64(otp);
                            model1.flag = 3;
                            var a = objAccDb.OTPVarification(model1).ToList().FirstOrDefault();
                        }
                        catch { }
                        objSM.isMSGSend = true;
                    }
                    else
                    {
                        res = "SMS Service is not working currently. please try again later. ! ";
                        objSM.isMSGSend = false;
                    }
                }
                else
                {
                    res = "OCE";
                    objSM.IsMaxLimit = true;
                    objSM.isMSGSend = false;
                }
            }
            else
            {
                res = "Current Session is terminated continue to login and try again...!";
                objSM.isMSGSend = false;
            }
            return res;
        }
        #region Generate OTP

        private string generateno()
        {

            string characters = string.Empty;


            string numbers = "1234567890";
            characters += numbers;



            string otp = string.Empty;
            string otpUser = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;


        }
        #endregion
        public JsonResult UploadFile(HttpPostedFileBase File)
        {


            string Dirpath = "~/Content/writereaddata/DEC/";
            string path = "";
            string filename = "";
            if (!Directory.Exists(Server.MapPath(Dirpath)))
            {
                Directory.CreateDirectory(Server.MapPath(Dirpath));
            }
            string ext = Path.GetExtension(File.FileName);
            if (ext.ToLower() == ".jpg" || ext.ToLower() == ".pdf")
            {

                filename = File.FileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;
                string completepath = Path.Combine(Server.MapPath(Dirpath), filename);
                if (System.IO.File.Exists(completepath))
                {
                    System.IO.File.Delete(completepath);
                }

                long size = File.ContentLength;
                if (size > 2097152)
                {
                    path = "warning_Maximum 2MB file size are allowed";
                }
                else
                {
                    File.SaveAs(completepath);
                    path = Dirpath + filename;
                }
            }
            else
            {
                path = "warning_Invalid File Format only pdf and jpg files are allow!";
            }

            List<string> plist = new List<string> { File.FileName, path };
            return Json(plist);


        }

        public ActionResult NomineeSearchListOfDownloadedCertificateDEC()
        {
            long regisByusers = objSM.UserID;
            DECModel model = new DECModel();
            model.DECModelList = objdb.GetDECListDEC(regisByusers);

            return View(model.DECModelList);
        }
        #endregion
    }
}
