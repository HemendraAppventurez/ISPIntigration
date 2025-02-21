using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CCSHealthFamilyWelfareDept.DAL;
using CCSHealthFamilyWelfareDept.Filters;
using CCSHealthFamilyWelfareDept.Models;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using SRVTextToImage;
using WebMatrix.WebData;
using System.Configuration;
using System.Data;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Security.Cryptography;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login

        Account_DB objAccDb = new Account_DB();
        Common_DB objComnDb = new Common_DB();
        Common objComn = new Common();
        SessionManager objSM = new SessionManager();
        //UPHEALTHNIC.upswp_niveshmitraservices objNivesh = new UPHEALTHNIC.upswp_niveshmitraservices();

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

        [HttpGet]
        public ActionResult TestPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TestPage(string Post = "")
        {
            //string controlId = Request.Form["TxtControlID"]; 

            return RedirectToAction("niveshmitraregistration", "Account");

        }

        public ActionResult niveshmitraregistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult niveshmitraregistration(string Post = "")
        {

            System.Data.DataSet ds = new System.Data.DataSet();
            UPHEALTHNIC.upswp_niveshmitraservices obj = new UPHEALTHNIC.upswp_niveshmitraservices();

            string controlId = Request.Form["TxtControlID"];
            string unitID = Request.Form["TxtUnitID"];
            string serviceID = Request.Form["TxtServiceID"];
            string requestID = Request.Form["TxtRequestID"];
            string passKey = ConfigurationManager.AppSettings["PassKey"].ToString();

            // string processIndustryID = Request.Form["TxtRequestID"]; //ProfileID
            //   string applicationID = Request.Form["TxtApplicationID"]; //UserID
            #region Get Pending application

            NUHmodel pendingApplication = new NUHmodel();
            int procId = 1;
            pendingApplication = objAccDb.GetPendingApplicationByRequestId(procId, controlId, unitID, serviceID, requestID);

            #endregion

            if (pendingApplication != null && pendingApplication.regisIdNUH > 0)
            {
                NiveshMitraUserDetailsModel userDetails = objAccDb.GetUserDetailsByUserId(pendingApplication.regByUser).FirstOrDefault();
                if (userDetails != null)
                {
                    DateTime lastLogindate = userDetails.LastLoginDate;

                    objSM.IsLoginUser = true;
                    objSM.UserID = userDetails.UserID;
                    objSM.Username = userDetails.UserName;
                    objSM.Transdate = Convert.ToString(lastLogindate);
                    objSM.RollID = userDetails.RollID;
                    objSM.DisplayName = userDetails.DisplayName;
                    objSM.ControlID = userDetails.ControlID;
                    objSM.UnitID = userDetails.UnitId;
                    objSM.ServiceID = userDetails.ServiceID;
                    objSM.RequestID = userDetails.RequestID;
                    objSM.NiveshMitraFlag = true;
                    if (pendingApplication.notarizedAffidavitFilePath != "")
                    {
                        TempData["affidavitFile"] = pendingApplication.notarizedAffidavitFilePath;
                        return RedirectToAction("NursingDashBoard", "NUH");
                    }
                    else
                    {
                        string medicalRegisId = OTPL_Imp.CustomCryptography.Encrypt(pendingApplication.regisIdNUH.ToString());
                        return RedirectToAction("UploadAffidavit", "NUH", new { regisId = medicalRegisId });
                    }
                }
            }
            else
            {
                ds = obj.WGetBasicDetails(controlId, unitID, serviceID, requestID, passKey);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    NiveshMitraRegistrationModel model = new NiveshMitraRegistrationModel();

                    model.Control_ID = ds.Tables[0].Rows[0]["Control_ID"].ToString();
                    model.Unit_Id = ds.Tables[0].Rows[0]["Unit_Id"].ToString();
                    model.ServiceID = Request.Form["TxtServiceID"];
                    model.RequestID = Request.Form["TxtRequestID"];

                    model.ProcessIndustryID = Request.Form["TxtRequestID"];
                    model.ApplicationID = "";
                    model.Company_Name = ds.Tables[0].Rows[0]["Company_Name"].ToString();
                    model.Industry_District = ds.Tables[0].Rows[0]["Industry_District"].ToString();
                    model.Industry_District_Id = ds.Tables[0].Rows[0]["Industry_District_Id"].ToString();
                    model.Industry_Address = ds.Tables[0].Rows[0]["Industry_Address"].ToString();
                    model.Pin_Code = ds.Tables[0].Rows[0]["Pin_Code"].ToString();
                    model.Occupier_Name = ds.Tables[0].Rows[0]["Occupier_Name"].ToString();
                    model.Occupier_Father_Mother_Name = ds.Tables[0].Rows[0]["Occupier_Father_Mother_Name"].ToString();
                    model.Occupier_Email_ID = ds.Tables[0].Rows[0]["Occupier_Email_ID"].ToString();
                    model.Occupier_Mobile_No = ds.Tables[0].Rows[0]["Occupier_Mobile_No"].ToString();

                    model.Occupier_DOB = ds.Tables[0].Rows[0]["Occupier_DOB"].ToString();
                    model.Occupier_PAN = ds.Tables[0].Rows[0]["Occupier_PAN"].ToString();
                    model.Occupier_Gender = ds.Tables[0].Rows[0]["Occupier_Gender"].ToString();
                    model.Occupier_Address = ds.Tables[0].Rows[0]["Occupier_Address"].ToString();
                    model.Occupier_Country_Id = "";// ds.Tables[0].Rows[0]["Occupier_Country_Id"].ToString();
                    model.Occupier_State_ID = "";// ds.Tables[0].Rows[0]["Occupier_State_ID"].ToString();
                    model.Occupier_District_ID = ds.Tables[0].Rows[0]["Occupier_District_ID"].ToString();
                    model.Occupier_District_Name = ds.Tables[0].Rows[0]["Occupier_District_Name"].ToString();
                    model.Occupier_Pin_Code = ds.Tables[0].Rows[0]["Occupier_Pin_Code"].ToString();
                    model.Nature_of_Activity = ds.Tables[0].Rows[0]["Installed_Capacity"].ToString();
                    model.Employees = ds.Tables[0].Rows[0]["Employees"].ToString();
                    model.Nature_of_Operation = ds.Tables[0].Rows[0]["Nature_of_Operation"].ToString();
                    model.Project_Cost = ds.Tables[0].Rows[0]["Project_Cost"].ToString();
                    model.Organization_Type_ID = ds.Tables[0].Rows[0]["Organization_Type_ID"].ToString();
                    model.Organization_Type = ds.Tables[0].Rows[0]["Organization_Type"].ToString();
                    model.Industry_Type_ID = ds.Tables[0].Rows[0]["Industry_Type_ID"].ToString();
                    model.Industry_Type_Name = ds.Tables[0].Rows[0]["Industry_Type_Name"].ToString();

                    model.Expected_date_construction = ds.Tables[0].Rows[0]["Expected_date_construction"].ToString();

                    string Expected_date_constructionYear = model.Expected_date_construction.Substring(0, 4);
                    string Expected_date_constructionMonth = model.Expected_date_construction.Substring(4, 2);
                    string Expected_date_constructionDate = model.Expected_date_construction.Substring(6, 2);

                    model.Expected_date_construction = Expected_date_constructionDate + "/" + Expected_date_constructionMonth + "/" + Expected_date_constructionYear;

                    model.Project_Status = ds.Tables[0].Rows[0]["Project_Status"].ToString();
                    model.Industry_Color = ds.Tables[0].Rows[0]["Industry_Color"].ToString();
                    model.Expected_date_production = ds.Tables[0].Rows[0]["Expected_date_production"].ToString();

                    string Expected_date_productionYear = model.Expected_date_production.Substring(0, 4);
                    string Expected_date_productionMonth = model.Expected_date_production.Substring(4, 2);
                    string Expected_date_productionDate = model.Expected_date_production.Substring(6, 2);

                    model.Expected_date_production = Expected_date_productionDate + "/" + Expected_date_productionMonth + "/" + Expected_date_productionYear;

                    model.Unit_Category = ds.Tables[0].Rows[0]["Unit_Category"].ToString();

                    model.Items_Manufactured = ds.Tables[0].Rows[0]["Items_Manufactured"].ToString();
                    model.Annual_Turnover = ds.Tables[0].Rows[0]["Annual_Turnover"].ToString();
                    model.insertDate = "";
                    model.isUpdated = "";
                    model.lastUpdatedDate = "";
                    model.sourceofRegistration = "Nivesh Mitra";
                    string firstName = ds.Tables[0].Rows[0]["Occupier_First_Name"].ToString();
                    string middleName = ds.Tables[0].Rows[0]["Occupier_Middle_Name"].ToString();
                    string lastName = ds.Tables[0].Rows[0]["Occupier_Last_Name"].ToString();
                    model.fullName = firstName + " " + middleName + "" + lastName;
                    model.Occupier_First_Name = ds.Tables[0].Rows[0]["Occupier_First_Name"].ToString();
                    model.Occupier_Middle_Name = ds.Tables[0].Rows[0]["Occupier_Middle_Name"].ToString();
                    model.Occupier_Last_Name = ds.Tables[0].Rows[0]["Occupier_Last_Name"].ToString();

                    model.fatherName = ds.Tables[0].Rows[0]["Occupier_Father_Mother_Name"].ToString();
                    model.categoryId = 0;
                    model.Password = "Niveshmitra41@12";
                    model.confirmPassword = "Niveshmitra41@12";

                    model.passsalt = ConfigurationManager.AppSettings["PassKey"].ToString();

                    model.requestKey = "";
                    model.NtransIp = "";

                    string year = model.Occupier_DOB.Substring(0, 4);
                    string month = model.Occupier_DOB.Substring(4, 2);
                    string date = model.Occupier_DOB.Substring(6, 2);
                    model.Occupier_DOB = date + "/" + month + "/" + year;


                    if (objComn.IsValidDateFormat(model.Occupier_DOB))
                    {
                        model.DTDob = Convert.ToDateTime(model.Occupier_DOB);
                        model.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "MD5").ToUpper().Trim();
                        model.transIp = Common.GetIPAddress();
                        model.requestKey = objSM.AppRequestKey;
                        try
                        {
                            model = objAccDb.NiveshMitraRegistration(model).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            setSweetAlertMsg("Error in registering !", "error");
                        }

                        if (model != null)
                        {
                            NiveshMitraUserDetailsModel userDetails = objAccDb.GetNiveshMitraUserDetails(model.Control_ID, model.Unit_Id, model.ServiceID, model.RequestID, model.userName).FirstOrDefault();
                            if (model.queryExeFlag == 1)
                            {
                                //First time user
                                if (userDetails != null)
                                {
                                    DateTime lastLogindate = userDetails.LastLoginDate;
                                    if (model.RequestID == userDetails.RequestID)
                                    {
                                        objSM.IsLoginUser = true;
                                        objSM.UserID = userDetails.UserID;
                                        objSM.Username = userDetails.UserName;
                                        objSM.Transdate = Convert.ToString(lastLogindate);
                                        objSM.RollID = userDetails.RollID;

                                        objSM.DisplayName = userDetails.DisplayName;

                                        objSM.ControlID = userDetails.ControlID;
                                        objSM.UnitID = userDetails.UnitId;
                                        objSM.ServiceID = userDetails.ServiceID;
                                        objSM.RequestID = userDetails.RequestID;
                                        objSM.NiveshMitraFlag = true;
                                        bool result = SendApplicationStatus(ds);
                                        if (result == true)
                                        {
                                            // return RedirectToAction("RegistrationInstructions", "NUH");
                                            return RedirectToAction("NursingDashBoard", "NUH");
                                        }

                                    }
                                    // TempData["Message"] = model.msg;
                                }

                            }
                            else if (model.queryExeFlag == 2)
                            {
                                //Existing user
                                if (userDetails != null)
                                {

                                    DateTime lastLogindate = userDetails.LastLoginDate;
                                    objSM.IsLoginUser = true;
                                    objSM.UserID = userDetails.UserID;
                                    objSM.Username = userDetails.UserName;
                                    objSM.Transdate = Convert.ToString(lastLogindate);
                                    objSM.RollID = userDetails.RollID;

                                    objSM.DisplayName = userDetails.DisplayName;

                                    objSM.ControlID = userDetails.ControlID;
                                    objSM.UnitID = userDetails.UnitId;
                                    objSM.ServiceID = userDetails.ServiceID;
                                    objSM.RequestID = userDetails.RequestID;
                                    objSM.NiveshMitraFlag = true;
                                    //MedicalRegistrationDetailsModel objNiveshMedicalReg = objAccDb.GetExistingMedicalEstabRegistration(objSM.UserID).FirstOrDefault();
                                    MedicalRegistrationDetailsModel objNiveshMedicalReg = objAccDb.GetExistingMedicalEstabRegistration(model.regisIdNUH).FirstOrDefault();
                                    if (objNiveshMedicalReg != null)
                                    {
                                        if (objNiveshMedicalReg.QueryFlag == 1)
                                        {
                                            if (objNiveshMedicalReg.regByUser == userDetails.UserID)
                                            {
                                                if (objNiveshMedicalReg.notarizedAffidavitFilePath != "")
                                                {
                                                    TempData["affidavitFile"] = objNiveshMedicalReg.notarizedAffidavitFilePath;
                                                    return RedirectToAction("NursingDashBoard", "NUH");
                                                }
                                                else
                                                {
                                                    string medicalRegisId = OTPL_Imp.CustomCryptography.Encrypt(objNiveshMedicalReg.regisIdNUH.ToString());
                                                    return RedirectToAction("UploadAffidavit", "NUH", new { regisId = medicalRegisId });
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // return RedirectToAction("RegistrationInstructions", "NUH");
                                        return RedirectToAction("NursingDashBoard", "NUH");

                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        setSweetAlertMsg("Date of Birth Date is incorrect format, Date format should be DD/MM/YYYY !", "warning");
                    }

                }
            }

            return View();
        }

        public bool SendApplicationStatus(System.Data.DataSet ds)
        {

            bool isSuccess = false;
            NiveshMitraSendStatusModel nModel = new NiveshMitraSendStatusModel();
            UPHEALTHNIC.upswp_niveshmitraservices objStatus = new UPHEALTHNIC.upswp_niveshmitraservices();
            DataTable dt = new DataTable();

            nModel.Control_ID = ds.Tables[0].Rows[0]["Control_ID"].ToString();
            nModel.Unit_Id = ds.Tables[0].Rows[0]["Unit_Id"].ToString();
            nModel.ServiceID = Request.Form["TxtServiceID"];
            nModel.ProcessIndustryID = objSM.Username;
            nModel.ApplicationID = "";  //objSM.UserID.ToString();
            nModel.StatusCode = "10";
            nModel.Remarks = "User Registered";
            nModel.PendencyLevel = "Entrepreneur level pendency";
            nModel.FeeAmount = "";
            nModel.FeeStatus = "";
            nModel.TransectionID = "";
            nModel.TranSactionDate = "";
            nModel.TransectionDateAndTime = "";
            nModel.NocCertificateNumber = "";
            nModel.NocUrl = "";
            nModel.IsNocUrlActiveYesNo = "";
            nModel.Passalt = ConfigurationManager.AppSettings["PassKey"].ToString();
            nModel.RequestId = Request.Form["TxtRequestID"];
            nModel.ObjectRejectionCode = "";
            nModel.IsCertificateValidLifeTime = "";
            nModel.CertificateExpireDateDDMMYYYY = "";
            nModel.D1 = "";
            nModel.D2 = "";
            nModel.D3 = "";
            nModel.D4 = "";
            nModel.D5 = "";
            nModel.D6 = "";
            nModel.D7 = "";

            string StatusResult = objStatus.WReturn_CUSID_STATUS(nModel.Control_ID, nModel.Unit_Id, nModel.ServiceID, nModel.ProcessIndustryID, nModel.ApplicationID, nModel.StatusCode, nModel.Remarks, nModel.PendencyLevel, nModel.FeeAmount, nModel.FeeStatus,
                         nModel.TransectionID, nModel.TranSactionDate, nModel.TransectionDateAndTime, nModel.NocCertificateNumber, nModel.NocUrl, nModel.IsNocUrlActiveYesNo, nModel.Passalt, nModel.RequestId, nModel.ObjectRejectionCode
                         , nModel.IsCertificateValidLifeTime, nModel.CertificateExpireDateDDMMYYYY, nModel.D1, nModel.D2, nModel.D3, nModel.D4, nModel.D5, nModel.D6, nModel.D7);

            if (StatusResult.ToUpper() == "SUCCESS")
            {
                nModel.SendDate = System.DateTime.Now;
                nModel.ResStatus = "";
                nModel.ServiceStatus = StatusResult;
                nModel.StepId = 1;
                isSuccess = true;
            }
            else
            {
                nModel.SendDate = System.DateTime.Now;
                nModel.ResStatus = "";
                nModel.ServiceStatus = StatusResult;
                nModel.StepId = 1;
                isSuccess = false;
            }
            try
            {
                nModel = objAccDb.SendRegistrationStatus(nModel).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            if (isSuccess)
            {
                return true;
            }
            else
            {

                return false;
            }
        }

        public ActionResult DownloadFileByIDAndPath(string filePath)
        {
            string decrypFilePath = OTPL_Imp.CustomCryptography.Decrypt(Server.UrlDecode(filePath));

            if (System.IO.File.Exists(Server.MapPath(decrypFilePath)))
            {
                FileInfo fileInfo = new FileInfo(Server.MapPath(decrypFilePath));
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(decrypFilePath));
                Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
                Response.WriteFile(decrypFilePath);
                Response.Flush();
                Response.Close();
            }

            return View("DownloadFile");
        }


        public ActionResult Login(string appSlug = "")
        {
            ModelState.Clear();
            objSM.AppSlug = appSlug;
            Random rd = new Random();
            string seed = rd.Next(100000, 999999).ToString();
            objSM.SeedValue = seed;
            LoginModel model = new LoginModel();
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

                    UserDetailsModel user = objAccDb.GetUserDetails(model.UserName, 3).FirstOrDefault();
                    if (user != null)
                    {
                        if (model.Password.ToString().ToUpper().Trim() == FormsAuthentication.HashPasswordForStoringInConfigFile(user.password.ToUpper() + oldseed, "MD5").ToUpper().Trim())
                        {
                            //update login log
                            UserDetailsModel verifiduser = objAccDb.ManageloginAndGetStatus(user.UserId, "S", allowedWrongAttampt);
                            if (verifiduser.Flag == 1)
                            {
                                DateTime lastLogindate = user.LastLoginDate;

                                if (verifiduser.IsMobileVarified == 1)
                                {
                                    #region set Sessions
                                    objSM.IsLoginUser = true;
                                    objSM.UserID = user.UserId;
                                    objSM.DisplayName = user.DisplayName;
                                    objSM.ProfilePicPath = user.profilePicPath;
                                    objSM.Username = user.UserName;
                                    objSM.RollID = user.rollId;
                                    objSM.Transdate = Convert.ToString(lastLogindate);
                                    objSM.NiveshMitraFlag = user.NiveshMitraFlag;

                                    #endregion
                                    if (!string.IsNullOrEmpty(objSM.AppSlug))
                                    {
                                        return RedirectToAction("isRegister", objSM.AppSlug);
                                    }
                                    else
                                    {
                                        return RedirectToAction("Dashboard", "Home");
                                    }
                                }
                                else if (verifiduser.IsMobileVarified == 0)
                                {

                                    objSM.Username = verifiduser.UserName;
                                    objSM.DisplayName = verifiduser.DisplayName;
                                    objSM.UserIDUserPolcy = verifiduser.UserId;
                                    objSM.RollID = verifiduser.rollId;
                                    objSM.IsEmailVerified = verifiduser.IsEmailVarified == 0 ? false : true;
                                    objSM.isMobileVerified = verifiduser.IsMobileVarified == 0 ? false : true;
                                    objSM.EmailId = verifiduser.emailId;
                                    objSM.MobileNumber = verifiduser.mobileNo;
                                    objSM.NiveshMitraFlag = verifiduser.NiveshMitraFlag;
                                    return RedirectToAction("VerifyMobile", "Account", new { userName = objSM.Username });
                                }
                            }
                            else
                            {
                                setSweetAlertMsg(verifiduser.Msg, "warning");
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

        #region ISP
        #region  IllnessCertificate
        public ActionResult IllnessCertificateLogin(LoginModel model)
        {
            string sent = "0";
            string service_code = "";


            try
            {
                string sessionKey = Request.QueryString["sessionkey"];
                string token = "";
                DateTime timestamp;

                ISPLoginRequest req = new ISPLoginRequest();
                if (!string.IsNullOrEmpty(sessionKey))
                {
                    string encrypt = "{\"sessionkey\":\"" + sessionKey + "\"}";
                    string encryptdata = encryptString(encrypt, "");

                    req.username = "6240616";
                    req.password = "SWwXW7trJaMigcPWSIdH";
                    string dsata = Services.PostValidateISPLogin(req, "http://164.100.181.91/ispws/authenticate");

                    var Response = JsonConvert.DeserializeObject<ISPLoginResponse>(dsata);
                    if (Response.statusMessage == "SUCCESSFUL")
                    {
                        timestamp = Response.timestamp;
                        token = Response.token;
                        TempData["token"] = token;
                        TempData["timestamp"] = Response.timestamp;
                        objSM.IspFlag = true;

                        string json = "{\"dept_id\":\"" + req.username + "\",\"e_data\":\"" + encryptdata + "\"}";
                        string APIurl = "http://164.100.181.91/ispws/isp/v1/getRequestValidatedAndRequestID";
                        string result = Services.HitAPIWithToken(json, APIurl, token);
                        var ValidateRes = JsonConvert.DeserializeObject<ValidateResponse>(result);
                        if (ValidateRes.statusMessage == "SUCCESSFUL")
                        {
                            string decryptData = decryptString(ValidateRes.data, "");
                            var decryptResponse = JsonConvert.DeserializeObject<DecryptResponse>(decryptData);
                            service_code = decryptResponse.service_code;
                            TempData["service_code"] = service_code;
                            TempData["request_id"] = decryptResponse.request_id;
                            TempData["applicant_id"] = decryptResponse.applicant_id;
                            objSM.ServiceCode = service_code; 
                            string plainReqId = "{\"request_id\":\"" + decryptResponse.request_id + "\"}";
                            string encryptReqId = encryptString(plainReqId, "");

                            json = "{\"dept_id\":\"" + req.username + "\",\"e_data\":\"" + encryptReqId + "\"}";
                            APIurl = "http://164.100.181.91/ispws/isp/v1/getApplicantCommonDetails";
                            result = Services.HitAPIWithToken(json, APIurl, token);
                            var Usercommondetails = JsonConvert.DeserializeObject<ValidateResponse>(result);


                            if (Usercommondetails.statusMessage == "SUCCESSFUL")
                            {
                                decryptData = decryptString(Usercommondetails.data, "");

                                var ApplicantCommanDetails = JsonConvert.DeserializeObject<ApplicantCommanDetails>(decryptData);
                                objAccDb.AddISPLoginDetails(sessionKey, encryptdata, token, decryptResponse.applicant_id, decryptResponse.request_id, decryptData, decryptResponse.service_code);


                                sent = "1"; //allow to redirection to the service
                                TempData["ApplicantCommanDetails"] = ApplicantCommanDetails;
                            }
                            //
                            // string abc = decryptString(ValidateRes.data, "");



                        }
                    }



                }
            }
            catch (Exception ex)
            {
                return View("login");
            }
            int passwordLyf = System.Configuration.ConfigurationManager.AppSettings["PasswordLife"] == null ? 90 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PasswordLife"].ToString());
            int allowedWrongAttampt = System.Configuration.ConfigurationManager.AppSettings["AllowedMaxWrongAtt"] == null ? 5 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AllowedMaxWrongAtt"].ToString());
            if (sent == "1")
            {

                if (1 == 1)
                {
                    string oldseed = "134554";

                    UserDetailsModel user = objAccDb.GetUserDetails("8303086781", 1).FirstOrDefault();
                    if (user != null)
                    {
                        //if (model.Password.ToString().ToUpper().Trim() == FormsAuthentication.HashPasswordForStoringInConfigFile(user.password.ToUpper() + oldseed, "MD5").ToUpper().Trim())
                        //{
                        //update login log
                        UserDetailsModel verifiduser = objAccDb.ManageloginAndGetStatus(user.UserId, "S", allowedWrongAttampt);
                        if (verifiduser.Flag == 1)
                        {
                            DateTime lastLogindate = user.LastLoginDate;

                            if (verifiduser.IsMobileVarified == 1)
                            {
                                #region set Sessions
                                objSM.IsLoginUser = true;
                                objSM.UserID = user.UserId;
                                objSM.DisplayName = user.DisplayName;
                                objSM.ProfilePicPath = user.profilePicPath;
                                objSM.Username = user.UserName;
                                objSM.RollID = user.rollId;
                                objSM.Transdate = Convert.ToString(lastLogindate);
                                objSM.NiveshMitraFlag = user.NiveshMitraFlag;

                                #endregion
                                if (!string.IsNullOrEmpty(objSM.AppSlug))
                                {
                                    return RedirectToAction("isRegister", objSM.AppSlug);
                                }
                                else
                                {
                                    //returnApplicationAcknowledgementtoIsp();
                                    //return RedirectToAction("ILCRegistration", "ILC");
                                    //returnServiceStatus();
                                    if (service_code == "IVT53")
                                    {
                                        return RedirectToAction("RegistrationInstructions", "ILC");
                                        //return RedirectToAction("RegistrationInstructions", "FIC");
                                    }
                                    else if (service_code == "ISY70")
                                    {
                                        return RedirectToAction("RegistrationInstructions", "FIC");
                                    }
                                    else if (service_code == "DRP71")
                                    {
                                        return RedirectToAction("RegistrationInstructions", "DIC");
                                    }
                                    else if (service_code == "IPN95")
                                    {
                                        return RedirectToAction("RegistrationInstructions", "IMC");
                                    }
                                    else if (service_code == "DVI93")
                                    {
                                        return RedirectToAction("DECDashBoard", "DEC");
                                    }
                                    else if (service_code == "UVI24")
                                    {
                                        return RedirectToAction("RegistrationInstructions", "FAP");
                                    }
                                    else if (service_code == "MYX79")
                                    {
                                        return RedirectToAction("RegistrationInstructions", "MER");
                                    }
                                    else if (service_code == "MUD38")
                                    {
                                        return RedirectToAction("MLCDashBoard", "MLC");
                                    }
                                    else if (service_code == "AEF80")
                                    {
                                        return RedirectToAction("RegistrationInstructions", "AGC");
                                    }
                                    else if (service_code == "IBV84")
                                    {
                                        return RedirectToAction("RegistrationInstructions", "ICC");
                                    }
                                    //else if (service_code == "JIJ74")
                                    //{
                                    //    return RedirectToAction("RegistrationInstructions", "ILC");
                                    //}
                                    else if (service_code == "JIJ74")
                                    {
                                        return RedirectToAction("RegistrationInstructions", "IFC");
                                    }
                                }
                            }
                            else if (verifiduser.IsMobileVarified == 0)
                            {

                                objSM.Username = verifiduser.UserName;
                                objSM.DisplayName = verifiduser.DisplayName;
                                objSM.UserIDUserPolcy = verifiduser.UserId;
                                objSM.RollID = verifiduser.rollId;
                                objSM.IsEmailVerified = verifiduser.IsEmailVarified == 0 ? false : true;
                                objSM.isMobileVerified = verifiduser.IsMobileVarified == 0 ? false : true;
                                objSM.EmailId = verifiduser.emailId;
                                objSM.MobileNumber = verifiduser.mobileNo;
                                objSM.NiveshMitraFlag = verifiduser.NiveshMitraFlag;
                                return RedirectToAction("VerifyMobile", "Account", new { userName = objSM.Username });
                            }
                        }
                        else
                        {
                            setSweetAlertMsg(verifiduser.Msg, "warning");
                        }

                    }
                    else
                    {
                        setSweetAlertMsg("Invalid Login Details. !", "warning");
                    }
                }

            }

            return View("login");

        }



        public static string returnApplicationAcknowledgementtoIsp()
        {

            var result = "";
            AcknowledgementReq req = new AcknowledgementReq();
            req.status_code = "S104";
            req.request_id = "Y7745381196232";//as received from validation API
            req.applicant_id = "UPFFIY0628";
            req.application_id = "test";
            req.service_code = "JIJ74";
            req.application_url = "";
            req.remarks = "";
            req.pendency_level = "4";
            req.pending_with_officer = "NA";
            req.action_taken_time = "2023-05-27 02:31:29";
            req.selected_district_for_processing_application = "157"; //Need to be discussed
            req.designated_code = "7647"; //Need to be discussed
            req.designated_location_code = "157";//Need to be discussed
            req.designated_target_date = "2023-05-27";
            req.first_appellate_code = "2526";//Need to be discussed
            req.first_appellate_location_code = "157";//Need to be discussed
            req.first_appellate_days = "5";
            req.second_appellate_code = "2526";//Need to be discussed
            req.second_appellate_location_code = "157";//Need to be discussed
            req.second_appellate_days = "10";//Need to be discussed
            req.d1 = "";
            req.d10 = "";
            req.d11 = "";
            req.d12 = "";
            req.d13 = "";
            req.d14 = "";
            req.d15 = "";
            req.d16 = "";
            req.d17 = "";
            req.d18 = "";
            req.d19 = "";
            var token = "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiI2MjQwNjE2IiwiZXhwIjoxNjg1MTk0MjA1LCJpYXQiOjE2ODUxOTA2MDV9.S0IlRK_jgUGWYRj48WwtbV0PyzsKh5oZSYfTtg1OphEPu6ILDw8VF6ro3i0wJX6icTUdmkI5QG6xcKeREMLSDQ";

            try
            {

                string jsonpara = "{\"request_id\":\"" + req.request_id + "\",\"applicant_id\":\"" + req.applicant_id + "\",\"application_url\":\"" + req.application_url + "\",\"status_code\":\"" + req.status_code + "\"" +
                    ",\"remarks\":\"" + req.remarks + "\",\"pendency_level\":\"" + req.pendency_level + "\",\"pending_with_officer\":\"" + req.pending_with_officer + "\",\"action_taken_time\":\"" + req.action_taken_time + "\",\"selected_district_for_processing_application\":\"" + req.selected_district_for_processing_application + "\",\"designated_code\":\"" + req.designated_code + "\",\"designated_location_code\":\"" + req.designated_location_code + "\"," +
                    "\"designated_target_date\":\"" + req.designated_target_date + "\",\"first_appellate_code\":\"" + req.first_appellate_code + "\",\"first_appellate_location_code \":\"" + req.first_appellate_location_code + "\",\"first_appellate_days\":\"" + req.first_appellate_days + "\",\"second_appellate_code\":\"" + req.second_appellate_code + "\",\"second_appellate_location_code\":\"" + req.second_appellate_location_code + "\",\"second_appellate_days\":\"" + req.second_appellate_days + "\",\"d1\":\"" + req.d1 + "\",\"d10\":\"" + req.d10 + "\"," +
                    ",\"d11\":\"" + req.d11 + "\",\"d12\":\"" + req.d12 + "\",\"d13\":\"" + req.d13 + "\",\"d14\":\"" + req.d14 + "\",\"d15\":\"" + req.d15 + "\",\"d16\":\"" + req.d16 + "\",\"d17\":\"" + req.d17 + "\",\"d18\":\"" + req.d18 + "\",\"d19\":\"" + req.d19 + "\"}";
                jsonpara = encryptString(JsonConvert.SerializeObject(req), "");
                string APIurl = "http://164.100.181.91/ispws/isp/v1/returnApplicationAcknowledgement";
                string result1 = Services.HitAPIWithTokennew(jsonpara, APIurl, token);
                var ValidateRes = JsonConvert.DeserializeObject<ValidateResponse>(result1);
                if (ValidateRes.statusMessage == "SUCCESSFUL")
                {

                }
            }
            catch (WebException ex)
            {
                result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            return result;
        }
        public static string returnServiceStatus()
        {

            var result = "";
            AcknowledgementReq req = new AcknowledgementReq();
            req.status_code = "s117";
            req.request_id = "Y7745381196232";//as received from validation API
            req.applicant_id = "UPFFIY0628";
            req.application_id = "test";
            req.service_code = "JIJ74";
            req.application_url = "";
            req.remarks = "";
            req.pendency_level = "4";
            req.pending_with_officer = "NA";
            req.action_taken_time = "2023-05-27 02:31:29";

            var token = "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiI2MjQwNjE2IiwiZXhwIjoxNjg1MjYzNTMxLCJpYXQiOjE2ODUyNTk5MzF9.srJy1MupLq1chttoz4jUhNOSWgYlwjvZW4kRM_r0EJyfVMYGZqVKPH6UsCao9AnmS-BWzcU8py5YLquI6F0mww";

            try
            {

                string jsonpara = "{\"request_id\":\"" + req.request_id + "\",\"applicant_id\":\"" + req.applicant_id + "\",\"application_url\":\"" + req.application_url + "\",\"status_code\":\"" + req.status_code + "\"" +
                    ",\"remarks\":\"" + req.remarks + "\",\"pendency_level\":\"" + req.pendency_level + "\",\"pending_with_officer\":\"" + req.pending_with_officer + "\",\"action_taken_time\":\"" + req.action_taken_time + "\"}";
                jsonpara = encryptString(JsonConvert.SerializeObject(req), "");
                string APIurl = "http://164.100.181.91/ispws/isp/v1/returnServiceStatus";
                string result1 = Services.HitAPIWithTokennew(jsonpara, APIurl, token);
                var ValidateRes = JsonConvert.DeserializeObject<ValidateResponse>(result1);
                if (ValidateRes.statusMessage == "SUCCESSFUL")
                {

                }
            }
            catch (WebException ex)
            {
                result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            return result;
        }


        public ActionResult ApplicationAck(AcknowledgementReq acknowledgementReq)
        {
            string token = "";
            acknowledgementReq.applicant_id = "ss";
            string url = "http://164.100.181.91/ispws/isp/v1/returnApplicationAcknowledgement";
            string result = Services.HitAPIWithToken(JsonConvert.SerializeObject(acknowledgementReq), url, token);
            return null;
        }

        public static string encryptString(string plainText, string pass)
        {
            byte[] salt = new byte[] {
            0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30 ,
            0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31};
            byte[] encrypted;
            // byte[] key = new Rfc2898DeriveBytes(pass, salt, 1000, HashAlgorithmName.SHA512).GetBytes(32);
            pass = "Gl7H01dpIRJ1YPZhmNQJ03nUu/NKxkjIFtP6XHDomJY=";
            byte[] Key = Convert.FromBase64String(pass);
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 };
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {

                    swEncrypt.Write(plainText);
                    swEncrypt.Flush();
                    csEncrypt.FlushFinalBlock();
                    encrypted = msEncrypt.ToArray();
                    Console.WriteLine(encrypted);
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        public static string decryptString(string _cipherText, string pass)
        {
            if (_cipherText == null || _cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");

            byte[] cipherText = Convert.FromBase64String(_cipherText);
            byte[] IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 };
            byte[] salt = new byte[] {
            0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31};
            //   byte[] Key = new Rfc2898DeriveBytes(pass, salt, 1000, HashAlgorithmName.SHA512).GetBytes(32);
            // Check arguments.
            pass = "Gl7H01dpIRJ1YPZhmNQJ03nUu/NKxkjIFtP6XHDomJY=";
            byte[] Key = Convert.FromBase64String(pass);
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            string plaintext = null;
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.Mode = CipherMode.CBC;
                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {

                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
        #endregion

        #endregion

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
        public ActionResult Registration()
        {
            ViewBag.CategoryDropdownList = objComnDb.GetDropDownList(8, 0).Select(e => new SelectListItem() { Text = e.Name, Value = e.Id.ToString() });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(RegistrationModel model)
        {
            ViewBag.CategoryDropdownList = objComnDb.GetDropDownList(8, 0).Select(e => new SelectListItem() { Text = e.Name, Value = e.Id.ToString() });

            if (ModelState.IsValid)
            {
                if (objComn.IsValidDateFormat(model.dob))
                {
                    model.DTDob = Convert.ToDateTime(model.dob);
                    model.password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.password, "MD5").ToUpper().Trim();
                    model.transIp = Common.GetIPAddress();
                    model.requestKey = objSM.AppRequestKey;

                    try
                    {
                        model = objAccDb.Registration(model).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        setSweetAlertMsg("Error in registering !", "error");
                    }

                    if (model != null)
                    {
                        if (model.queryExeFlag == 1)
                        {
                            setSweetAlertMsg(model.msg, "success");
                            return RedirectToAction("VerifyMobile", new { userName = model.userName });
                        }
                        TempData["Message"] = model.msg;
                    }
                }
                else
                {
                    setSweetAlertMsg("Date of Birth Date is incorrect format, Date format should be DD/MM/YYYY !", "warning");
                }
            }
            else
            {
                setSweetAlertMsg("Enter Valid Data !", "warning");
            }

            return View();
        }

        #region Method - check email and mobile existence
        [HttpPost]
        public JsonResult CheckEmailExistence(string emailId)
        {
            var user = objAccDb.CheckEmailMobileExistence(emailId, "E");
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [HttpPost]
        public JsonResult CheckMobileExistence(string mobileNo)
        {
            var user = objAccDb.CheckEmailMobileExistence(mobileNo, "M");
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }
        #endregion

        public ActionResult VerifyMobile(string userName)
        {
            ViewBag.PageHeading = "Mobile Verification";
            userPolicyModel model = new userPolicyModel();
            UserDetailsModel userModel = new UserDetailsModel();

            try
            {
                userModel = objAccDb.GetUserDetails(userName, 1).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //TempData["Message"] = "Error in Registration !";
                return RedirectToAction("Login");
            }

            if (userModel != null)
            {
                objSM.UserID = userModel.UserId;
                objSM.UserIDUserPolcy = userModel.UserId;
                objSM.Username = userModel.UserName;
                objSM.DisplayName = userModel.DisplayName;
                objSM.MobileNumber = userModel.mobileNo;
                objSM.EmailId = userModel.emailId;
                objSM.IsEmailVerified = userModel.IsEmailVarified == 0 ? false : true;
                objSM.isMobileVerified = userModel.IsEmailVarified == 0 ? false : true;

                model.UserId = userModel.UserId;
                model.Displayname = userModel.DisplayName;
                model.Mobile = userModel.mobileNo.Substring(0, 2) + "XXXXX" + userModel.mobileNo.Substring(7, 3);
                model.IsEmailVarified = Convert.ToBoolean(userModel.IsEmailVarified);
                model.IsMobileVarified = Convert.ToBoolean(userModel.IsMobileVarified);
            }

            string mobVerify = "";
            string smsFlag = SendOTP();
            if (smsFlag == "S")
            {
                mobVerify = "OTP is sent to your registered Mobile No.";
            }
            else
            {
                mobVerify = "SMS Service is not working";
            }

            setSweetAlertMsg(mobVerify, "success");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyMobile(userPolicyModel model)
        {
            ModelState.Clear();
            model.IsEmailSend = objSM.IsEmailSend;
            model.IsMsgSend = objSM.isMSGSend;
            model.IsEmailVarified = objSM.IsEmailVerified;

            if (objSM.isMSGSend)
            {
                if (objSM.OTP == model.Opt)
                {
                    var a = objAccDb.UpdateMobileverify(objSM.UserIDUserPolcy, "M", Common.GetIPAddress());
                    if (a != null && a.Flag == 1)
                    {
                        objSM.isMobileVerified = true;
                        model.IsMobileVarified = true;
                        TempData["SuccessMsg"] = "Mobile Verified Successfully";
                        return RedirectToAction("Login", "Account", new { Area = "" });
                    }
                    else
                    {
                        setSweetAlertMsg("Mobile could not Verify", "warning");
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

            if (objSM.MobileNumber != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                int MaxAllowedSMS = System.Configuration.ConfigurationManager.AppSettings["MaxAllowedSMS"] == null ? 5 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxAllowedSMS"].ToString());
                otpChCount.UserId = objSM.UserIDUserPolcy;
                otpChCount.MobileNo = objSM.MobileNumber;
                otpChCount.flag = 2;
                var otpCount = objAccDb.OTPVarification(otpChCount).ToList().FirstOrDefault();
                if (otpCount != null && otpCount.otpCount <= MaxAllowedSMS)
                {
                    objSM.IsMaxLimit = false;
                    //string txtmsg = "Dear User,\n\nYour OTP for Registration on the portal of Medical Health And Family Welfare Department (MHFWD), U.P is " + otp + ".\nPlease note that this OTP will be valid till next 15 minutes.\nPlease do not share this OTP with anyone for security reasons.\n\n Technical Team\nMHFWD, UP";
                    //string txtmsg = "Dear User,\n\nYour OTP for Registration on UP MHFWD portal is " + otp + ".\nThe OTP will be valid for next 15 minutes. Kindly do the needful.\nRegards, UP MHFWD";
                    //string txtmsg = "Your OTP for Registration is " + otp + "-OMNINET TECHNOLOGIES PVT LTD";
                   // 08/08/23 string txtmsg = "Dear Applicant ,\n" + otp + "-is your OTP for the registration of Janhit Health Portal,UP";

                    // string status = SMS.SMSSend(txtmsg, Convert.ToString(objSM.MobileNumber), "1607100000000032462");
                   //08/08/23 string status = SMS.SMSSendNew(txtmsg, Convert.ToString(objSM.MobileNumber), "1607100000000032462");

                    string txtmsg = "" + otp + " is your OTP to get register on Janhit Health Portal, U.P.&entityid=1201159098715451886&templateid=1207169129912877042";

                   //10/08/2023 string status = SMS.SMSSendNew(txtmsg, Convert.ToString(objSM.MobileNumber), "1207169129912877042");
                    string status = SMS.SMSSendNewOTP(txtmsg, Convert.ToString(objSM.MobileNumber), "1207169129912877042");
                    objAccDb.SMSLog(txtmsg, objSM.MobileNumber, status, Convert.ToString(objSM.UserID));
                    if (status.ToLower() == "success")
                    {
                        res = "S";
                        try
                        {
                            ForgotPasswordModel model1 = new ForgotPasswordModel();
                            model1.UserId = objSM.UserIDUserPolcy;
                            model1.MobileNo = objSM.MobileNumber;
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

        #region ForgetPassword
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            ModelState["UserName"].Errors.Clear();
            ModelState["Password"].Errors.Clear();
            ModelState["cPassword"].Errors.Clear();
            ModelState["otp"].Errors.Clear();
            ModelState["EmailId"].Errors.Clear();



            if (ModelState.IsValid)
            {
                if (Convert.ToString(Session["capimagetext"]) == model.Captcha)
                {
                    ForgotPasswordModel model1 = new ForgotPasswordModel();

                    //fetching records for Mobile Number Verification

                    model.flag = 1;
                    model1 = objAccDb.GetUserDetailByEmailIdOrMobile(model.flag, model.MobileNo, model.EmailId, model.MobileNo); //
                    if (model1 != null && model1.MobileNo != null)
                    {
                        if (model1.MobileNo == model.MobileNo)
                        {
                            if (model1.IsMobileVarified)
                            {
                                //Updating records for Mobile Number Verification and Sent OTP on Mobile No
                                model.flag = 2;
                                model1.RandonNo = Convert.ToInt64(generateno());
                                model1 = objAccDb.GetUserDetailByEmailIdOrMobile(model.flag, model.MobileNo, model.EmailId, model.MobileNo);
                                DateTime datecheck = model1.OTPSendDate ?? DateTime.Now;
                                datecheck = datecheck.AddHours(24);
                                if (datecheck < DateTime.Now)
                                {
                                    model1.otpCount = 0;
                                }

                                if (model1.otpCount >= 5)
                                {
                                    ViewBag.Msg = "You crossed maximum limit of sending OTP Please try After 24 hours ! Thank You";
                                    return View(model);
                                }
                                if (!(string.IsNullOrEmpty(Convert.ToString(model1.RandonNo))))
                                {
                                    model1.otp = Convert.ToInt64(generateno());
                                    model1.flag = 1;
                                    model1.MobileNo = model.MobileNo;
                                    var a = objAccDb.OTPVarification(model1).ToList().FirstOrDefault();
                                    objSM.MobileNumber = a.MobileNo;
                                    objSM.OTP = Convert.ToString(model1.otp);
                                    objSM.UserIDUserPolcy = a.UserId;
                                    objSM.isMSGSend = false;
                                    return RedirectToAction("OTPVarification", "Account");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("MobileNo", "Your Mobile No is not Verified kindly Contact to Department for Reset Your Password.");
                                return View(model);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("MobileNo", " This Mobile No. is not registered with us.");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("MobileNo", "This Mobile No. is not registered with us.");
                        return View(model);
                    }

                    model.seed = "1";

                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("Captcha", "Captcha Text is not Valid. !");
                }
            }
            else
            {
                ModelState.AddModelError("Captcha", "Enter Valid Data. !");
            }

            return View(model);
        }

        #endregion

        [HttpGet]
        public ActionResult OTPVarification(string isResend = "")
        {
            ForgotPasswordModel model = new ForgotPasswordModel();
            model.flag = 2;
            model.UserId = objSM.UserIDUserPolcy;
            model.MobileNo = objSM.MobileNumber;

            var a = objAccDb.OTPVarification(model).ToList().FirstOrDefault();
            model.UserName = a.UserName;
            model.MobileNo = a.MobileNo;
            if (isResend == "yes")
            {
                objSM.isMSGSend = false;
            }
            if (objSM.OTP != "" && objSM.OTP != "0")
            {
                if (!objSM.isMSGSend)
                {
                    string status = sendopt(objSM.OTP, objSM.MobileNumber);
                    if (status.ToLower() == "success")
                    {
                        objSM.isMSGSend = true;
                        setSweetAlertMsg("An OTP is sent to your registered mobile no.", "success");
                    }
                    else
                    {
                        setSweetAlertMsg("SMS service is not working please try again later. !", "error");
                        ModelState.AddModelError("otp", "SMS service is not working please try again later. !");
                    }
                }
            }
            else
            {
                setSweetAlertMsg("We can't get generated OTP Please go to Login and try again.!", "error");
                ModelState.AddModelError("otp", "We can't get generated OTP Please go to Login and try again.!");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OTPVarification(ForgotPasswordModel model)
        {
            model.flag = 2;
            model.UserId = objSM.UserIDUserPolcy;
            model.MobileNo = objSM.MobileNumber;
            var a = objAccDb.OTPVarification(model).ToList().FirstOrDefault();
            if (model.otp.ToString() == objSM.OTP)
            {
                Session["msg"] = "OV";
                return RedirectToAction("ResetPassword");
            }
            else
            {
                ViewBag.Msg = "OTP is not correct enter valid OTP";
                model.UserName = a.UserName;
                model.MobileNo = a.MobileNo;
            }
            return View(model);
        }

        #region Send OTP
        private string sendopt(string otpNo, string mobileNo)
        {
            //string txtmsg = "Dear User, your one time password(OTP) is " + otpNo + " and it is valid till next 15 mins please do not share this OTP with any one. Thank You!  Health and Family Welfare Dept.";
          //  string txtmsg = "Dear User,\n\nYour OTP for Password recovery on the portal of Medical Health And Family Welfare Department (MHFWD), U.P is " + otpNo + ".\nPlease note that this OTP will be valid till next 15 minutes.\nPlease do not share this OTP with anyone for security reasons.\n\n Technical Team\nMHFWD, UP";
            //08/08/23 string txtmsg = "Your OTP for Password is " + otpNo + "- Janhit Health Portal,UP";
            //08/08/23 string status = SMS.SMSSend(txtmsg, mobileNo, "1607100000000032462");

            string txtmsg = "" + otpNo + " is your OTP for Resetting Login Password on Janhit Health Portal, U.P.&entityid=1201159098715451886&templateid=1207169129921314690";
           //10/08/2023 string status = SMS.SMSSend(txtmsg, mobileNo, "1207169129921314690");

            string status = SMS.SMSSendOTP(txtmsg, mobileNo, "1207169129921314690");
            return status;
        }
        #endregion

        [HttpGet]
        public ActionResult ResetPassword()
        {
            PasswordChangeModel model = new PasswordChangeModel();

            ViewBag.DisplayName = objSM.DisplayName;

            if ((objSM.IsSessionActive("msg")) && (Convert.ToString(Session["msg"]) == "OV"))
            {

            }
            else
            {
                TempData["FailMsg"] = "Request time out or Invalid request please try again later, Thank You";
                return RedirectToAction("Confirmation", "Account");

            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(PasswordChangeModel model)
        {
            if (Convert.ToString(Session["capimagetext"]) == model.Captcha)
            {
                if (model.newPassword == model.confirmPassword)
                {
                    model.transIp = Common.GetIPAddress();
                    model.newPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(model.newPassword, "MD5").ToUpper().Trim();
                    if (objSM.UserIDUserPolcy != 0)
                    {
                        model.UserId = objSM.UserIDUserPolcy;
                        int res = 0;
                        res = objAccDb.ChangePassword(model);
                        if (res > 0)
                        {
                            TempData["SuccessMsg"] = "Password Changed Successfully";
                            return RedirectToAction("Confirmation", "Account");
                        }
                    }
                    else
                    {
                        TempData["FailMsg"] = "Request time out or Invalid request please try again later, Thank You";
                        return RedirectToAction("Confirmation", "Account");
                    }
                }
                else
                {
                    ViewBag.Msg = "Password and confirm password did not match";
                    ModelState.AddModelError("confirmPassword", "Password and confirm password did not match");
                }
            }
            else
            {
                ViewBag.Msg = "Captcha Text is not Valid.";
                ModelState.AddModelError("Captcha", "Captcha Text is not Valid.");
            }

            return View();
        }

        public ActionResult Confirmation()
        {
            if (TempData["SuccessMsg"] != null)
            {
                try
                {
                    ViewBag.Success = TempData["SuccessMsg"].ToString();

                }
                catch
                {
                    ViewBag.Success = "Success";
                }
                return View();
            }
            else
            {
                try
                {
                    ViewBag.Fail = TempData["FailMsg"].ToString();
                }
                catch
                {
                    ViewBag.Fail = "Fail";
                }
                objSM.RemoveSession();
                return View();
            }
        }

        [HttpGet]
        [AuthorizeUser]
        public ActionResult MyProfile()
        {
            MyAccountModel model = new MyAccountModel();
            var resultData = objAccDb.GetUserById(objSM.UserID);
            if (resultData != null)
            {
                model.userName = resultData.DisplayName;
                model.mobileNo = resultData.mobileNo;
                model.profileId = resultData.profileId;
                model.fatherName = resultData.fatherName;
                model.designation = resultData.designation;
                model.emailId = resultData.emailId;
                model.profilePicName = !string.IsNullOrEmpty(resultData.profilePicPath) ? resultData.profilePicPath.Split('/').Last() : null;
                model.profilePicPath = resultData.profilePicPath;
                ViewBag.EmailId = resultData.emailId;
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public ActionResult MyProfile(MyAccountModel model)
        {
            QueryExecuteModel executeModel = new QueryExecuteModel();

            if (ModelState.IsValid)
            {
                model.transIp = Common.GetIPAddress();
                model.profileId = objSM.UserID;
                model.profilePicPath = "~/Content/writereaddata/Users/Userprofile/" + model.profilePicName;

                try
                {
                    executeModel = objAccDb.UpdateUserProfile(model).FirstOrDefault();
                }
                catch (Exception)
                {
                    setSweetAlertMsg("Error in Updating Profile. !", "error");
                }

                if (executeModel != null)
                {
                    if (executeModel.queryExe == "U")
                    {
                        objSM.DisplayName = executeModel.returnValue;
                        objSM.ProfilePicPath = model.profilePicPath;
                        setSweetAlertMsg(executeModel.msgText, executeModel.msgStatus);
                    }
                    else
                    {
                        setSweetAlertMsg(executeModel.msgText, executeModel.msgStatus);
                    }
                }
            }
            else
            {
                setSweetAlertMsg("Details are not valid. !", "warning");
            }

            return View(model);
        }

        [HttpGet]
        [AuthorizeUser]
        public ActionResult ChangePassword()
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
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(PasswordChangeModel model)
        {
            string oldseed = model.seed.ToString();

            if (model.newPassword == model.confirmPassword)
            {
                UserDetailsModel user = objAccDb.GetUserDetails(objSM.Username, 1).FirstOrDefault();
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
                                    return RedirectToAction("Confirmation", "Account", new { Area = "" });
                                }
                                else
                                {
                                    ViewBag.msg = "Error in Change Password.";
                                    return View(model);
                                }
                            }
                            else
                            {
                                ViewBag.msg = "Old Password and new password must differ";
                                return View(model);
                            }

                        }
                        else
                        {
                            ViewBag.msg = "Password and confirm Password did not Match";
                            return View(model);
                        }
                    }
                    else
                    {
                        ViewBag.msg = "Old Password did not Match";
                        return View(model);
                    }

                }
            }

            ViewBag.msg = "Check Your Inputs";
            return View(model);
        }

        [HttpPost]
        public JsonResult UploadProfilePic()
        {
            Common objcm = new Common();

            HttpPostedFileBase Imgfile = Request.Files["Imgfile"];
            string size = "51200";
            string ImgValidation = objcm.ValidateImageExtWithSize(Imgfile, Convert.ToInt32(string.IsNullOrEmpty(size) ? "51200" : size) / 1024);
            string prefixFileName = Request.Form["prefixFileName"];
            string path = "";
            string filename = "";
            string base64String = null;
            if (ImgValidation == "Valid")
            {
                string ext = Path.GetExtension(Imgfile.FileName);
                filename = prefixFileName + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;
                string folderpath = "~/Content/writereaddata/Users/Userprofile";
                string completepath = Path.Combine(Server.MapPath(folderpath), filename);
                if (!System.IO.Directory.Exists(Server.MapPath(folderpath)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(folderpath));
                }

                if (System.IO.File.Exists(completepath))
                {
                    System.IO.File.Delete(completepath);
                }
                Imgfile.SaveAs(completepath);
                path = folderpath + "/" + filename;

                base64String = objcm.ConvertImageToBase64String(Server.MapPath(path));
            }

            List<string> Res = new List<string> { ImgValidation, base64String, filename };
            return Json(Res);
        }


    }
}
           