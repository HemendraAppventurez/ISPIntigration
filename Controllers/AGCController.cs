using CCSHealthFamilyWelfareDept.DAL;
using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCSHealthFamilyWelfareDept.Filters;
using CrystalDecisions.Shared;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.Net;
using Newtonsoft.Json;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    [AuthorizeUser]
    public class AGCController : Controller
    {
        Common_DB objComDB = new Common_DB();
        AGC_DB objdb = new AGC_DB();
        Common objCom = new Common();
        Account_DB objAccDb = new Account_DB();
        SessionManager objSM = new SessionManager();
        CMO_DB objCMODB = new CMO_DB();
        
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
            var res = objdb.IsRegister();
            if (res.isExists == 1)
            {
                return RedirectToAction("AGCDashBoard", "AGC");
            }
            else if (res.isExists == 0)
            {
                return RedirectToAction("RegistrationInstructions", "AGC");
            }
            return View();
        }
        #endregion

        public ActionResult RegistrationInstructions()
        {
            return View();
        }

        public ActionResult AGCDashBoard()
        {
            return View();
        }

        public ActionResult Index()
        {
            
            return View();
        }

        [HttpGet]
        public ActionResult AgeCertificate()
        {
            #region ISP

            long userId = objSM.UserID;
            TempData["ISPUSER"] = "N";

            AGCModel model = new AGCModel();


            var application = TempData["ApplicantCommanDetails"] as ApplicantCommanDetails;
            TempData["ApplicantCommanDetails1"] = application;

            if (@TempData["ApplicantCommanDetails"] != null)
            {
                TempData["ISPUSER"] = "Y";
                model.fullName = application.first_name_eng + " " + application.middle_name_eng + " " + application.last_name_eng;
                //model.fatherName = application.father_or_husband_or_guardian_name_eng;
                //model.dob = application.dob.ToString("dd/MM/yyyy");

                if (application.gender == "M")
                {
                    application.gender = "Male";
                }
                if (application.gender == "F")
                {
                    application.gender = "Female";
                }
                if (application.gender == "T")
                {
                    application.gender = "Transgender";
                }
                TempData["Gender"] = model.gender = application.gender;
                model.gender = application.gender;
                //model.rbtnGender = model.gender;
                //model.categoryId = application.category;
                model.mobileNo = application.mobile;
                model.emailId = application.email;
                model.address = application.permanent_mohalla;
                //model.opdStateId = application.residential_state;
                model.pinCode = application.permanent_pin;

                //model.opdDistrictId = application.permanent_district;
                // model.markOfIdentification = application.markOfIdentification;
            }

            #endregion

            ViewBag.ApplicantType = objComDB.GetDropDownList(26,0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.ApplicantSubType = Enumerable.Empty<SelectListItem>();
            ViewBag.IdType = objComDB.GetDropDownList(28,0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = objComDB.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return View(model);
        }

        [HttpPost]
        public JsonResult CheckMobileExistence(string mobileNo)
        {
            var user = objdb.CheckEmailMobileExistence(mobileNo, "M", objSM.UserID);
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgeCertificate(AGCModel model)
        {
            AuditMethods objAud = new AuditMethods();
            string errormsg = "";
            bool valStatus = false;

            valStatus = objAud.IsValidLink(model.orderFilePath, "Upload Order Copy", out errormsg);
            if (!valStatus)
            {
                setSweetAlertMsg(errormsg, "warning");
                return View(model);
            }

            valStatus = objAud.IsValidLink(model.documentFilePath, "Upload Id of Brought by Person", out errormsg);
            if (!valStatus)
            {
                setSweetAlertMsg(errormsg, "warning");
                return View(model);
            }

            valStatus = objAud.IsValidLink(model.susphotoFilePath, "Upload Photograph", out errormsg);
            if (!valStatus)
            {
                setSweetAlertMsg(errormsg, "warning");
                return View(model);
            }

            valStatus = objAud.IsValidLink(model.susThumbFilePath, "Upload Thumb Impression", out errormsg);
            if (!valStatus)
            {
                setSweetAlertMsg(errormsg, "warning");
                return View(model);
            }

            if (objCom.IsValidDateFormat(model.appointmentDate))
            {
                model.regByUser = objSM.UserID;
                model.regByIp = Common.GetIPAddress();
                model.transIp = Common.GetIPAddress();
                model.requestKey = objSM.AppRequestKey;

                var res = objdb.AGCInsertUpdate(model);

                try
                {
                    if (res.RegisId > 0 && res.RegistrationNo != "")
                    {
                        if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                        {
                            EDistrictServiceClass ed = new EDistrictServiceClass();

                            int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.AGC);

                            bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "AGC");

                            if (result)
                            {
                                SendSMS(res.RegistrationNo, res.MobileNo);
                                TempData["RegisIdAGC"] = res.RegisId;
                                TempData["RegistrationNo"] = res.RegistrationNo;
                                return RedirectToAction("RegistrationConfirmation");
                            }
                            else
                            {
                                int exeRsult = objdb.DeleteRegistrationAGC(res.RegisId);
                                setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                TempData["msg"] = "Invalid Request or Service Unavailable";
                                TempData["msgstatus"] = "warning";
                            }
                        }
                        else
                        {
                            SendSMS(res.RegistrationNo, res.MobileNo);
                            TempData["RegisIdAGC"] = res.RegisId;
                            TempData["RegistrationNo"] = res.RegistrationNo;
                            #region ISP
                            if (res.RegistrationNo != null && TempData["ISPUSER"].ToString() == "Y")
                            {
                                returnApplicationAcknowledgementtoIspAGC(model, TempData["ApplicantCommanDetails1"] as ApplicantCommanDetails, res.RegistrationNo);
                            }
                            #endregion
                            return RedirectToAction("RegistrationConfirmation");
                        }
                    }
                    else
                    {
                        setSweetAlertMsg(res.Msg.ToString(), "error");
                        TempData["msg"] = res.Msg.ToString();
                        TempData["msgstatus"] = "success";

                    }
                }
                catch (Exception E)
                {
                    setSweetAlertMsg("Record not save ", "error");
                    TempData["msg"] = res.Msg.ToString();
                    TempData["msgstatus"] = "success";

                }
            }
            else
            {
                setSweetAlertMsg("Appointment Date is in incorrect format, Date format should be DD/MM/YYYY !", "warning");
            }
            ViewBag.ApplicantType = objComDB.GetDropDownList(26, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.ApplicantSubType = Enumerable.Empty<SelectListItem>();
            ViewBag.IdType = objComDB.GetDropDownList(28, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = objComDB.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });


            return RedirectToAction("AgeCertificate");
           
           
        }

        #region ISP
        public string returnApplicationAcknowledgementtoIspAGC(AGCModel model, ApplicantCommanDetails model1, string RegistrationNo)
        {

            var result = "";

            AcknowledgementReq req = new AcknowledgementReq();
            string service_code = TempData["service_code"].ToString();
            string request_id = TempData["request_id"].ToString();

            string applicant_id = TempData["applicant_id"].ToString();

            req.request_id = request_id; // "Y7745381196232";
            req.applicant_id = applicant_id;//"UPFFIY0628";
            req.service_code = service_code;
            req.application_id = TempData["RegistrationNo"].ToString(); // "test";
            req.application_url = "";
            req.status_code = "S104";
            req.remarks = "";
            req.pendency_level = "4";
            req.pending_with_officer = "NA";
            req.action_taken_time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            req.selected_district_for_processing_application = "157";//model.districtId.ToString();//"157"; //Need to be discussed
            req.designated_code = "7647"; //Need to be discussed
            req.designated_location_code = "157";//Need to be discussed
            req.designated_target_date = DateTime.Now.ToString("yyyy-MM-dd");
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


            //req.status_code = "S104";
            //req.request_id = "Y7745381196232";//as received from validation API
            //req.applicant_id = "UPFFIY0628";
            //req.application_id = "test";
            //req.service_code = "JIJ74";
            //req.application_url = "";
            //req.remarks = "";
            //req.pendency_level = "4";
            //req.pending_with_officer = "NA";
            //req.action_taken_time = "2023-05-27 02:31:29";
            //req.selected_district_for_processing_application = "157"; //Need to be discussed
            //req.designated_code = "7647"; //Need to be discussed
            //req.designated_location_code = "157";//Need to be discussed
            //req.designated_target_date = "2023-05-27";
            //req.first_appellate_code = "2526";//Need to be discussed
            //req.first_appellate_location_code = "157";//Need to be discussed
            //req.first_appellate_days = "5";
            //req.second_appellate_code = "2526";//Need to be discussed
            //req.second_appellate_location_code = "157";//Need to be discussed
            //req.second_appellate_days = "10";//Need to be discussed
            //req.d1 = "";
            //req.d10 = "";
            //req.d11 = "";
            //req.d12 = "";
            //req.d13 = "";
            //req.d14 = "";
            //req.d15 = "";
            //req.d16 = "";
            //req.d17 = "";
            //req.d18 = "";
            //req.d19 = "";
            string token = TempData["token"].ToString();
            try
            {

                string jsonpara = "{\"request_id\":\"" + req.request_id + "\",\"applicant_id\":\"" + req.applicant_id + "\",\"application_url\":\"" + req.application_url + "\",\"status_code\":\"" + req.status_code + "\"" +
                    ",\"remarks\":\"" + req.remarks + "\",\"pendency_level\":\"" + req.pendency_level + "\",\"pending_with_officer\":\"" + req.pending_with_officer + "\",\"action_taken_time\":\"" + req.action_taken_time + "\",\"selected_district_for_processing_application\":\"" + req.selected_district_for_processing_application + "\",\"designated_code\":\"" + req.designated_code + "\",\"designated_location_code\":\"" + req.designated_location_code + "\"," +
                    "\"designated_target_date\":\"" + req.designated_target_date + "\",\"first_appellate_code\":\"" + req.first_appellate_code + "\",\"first_appellate_location_code \":\"" + req.first_appellate_location_code + "\",\"first_appellate_days\":\"" + req.first_appellate_days + "\",\"second_appellate_code\":\"" + req.second_appellate_code + "\",\"second_appellate_location_code\":\"" + req.second_appellate_location_code + "\",\"second_appellate_days\":\"" + req.second_appellate_days + "\",\"d1\":\"" + req.d1 + "\",\"d10\":\"" + req.d10 + "\"," +
                    ",\"d11\":\"" + req.d11 + "\",\"d12\":\"" + req.d12 + "\",\"d13\":\"" + req.d13 + "\",\"d14\":\"" + req.d14 + "\",\"d15\":\"" + req.d15 + "\",\"d16\":\"" + req.d16 + "\",\"d17\":\"" + req.d17 + "\",\"d18\":\"" + req.d18 + "\",\"d19\":\"" + req.d19 + "\"}";
                jsonpara = ISPController.encryptString(JsonConvert.SerializeObject(req), "");
                string APIurl = "http://164.100.181.91/ispws/isp/v1/returnApplicationAcknowledgement";
                string result1 = Services.HitAPIWithTokennew(jsonpara, APIurl, token);
                var ValidateRes = JsonConvert.DeserializeObject<ValidateResponse>(result1);
                if (ValidateRes.statusMessage == "SUCCESSFUL")
                {
                    objAccDb.UpdateISPDetails(RegistrationNo, req.service_code, req.request_id);
                }
            }
            catch (WebException ex)
            {
                result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            return result;
        }
        #endregion

        [HttpGet]
        public ActionResult ViewAGCList()
        {

            long regisByuser = objSM.UserID;
            AGCModel model = new AGCModel();
            model.AGCModelList = objdb.GetAGCList(regisByuser);
            return View(model.AGCModelList);

        }

        public ActionResult AGCDetails(string registrationNo = "", string regisIdAGC = "")
        {
            AGCModel model = new AGCModel();

            model = objdb.GetAGCListBYRegistrationNo(Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdAGC)), OTPL_Imp.CustomCryptography.Decrypt(registrationNo));

            if (model == null || objSM.UserID != model.regByUser)
            {
                return RedirectToAction("UserUnauthoriseAcess", "Home");
            }

            return View(model);
        }

        public ActionResult PrintApplicationForm(string registrationNo = "", string regisIdAGC = "")
        {
            AGCModel model = new AGCModel();

            model = objdb.GetAGCListBYRegistrationNo(Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdAGC)), OTPL_Imp.CustomCryptography.Decrypt(registrationNo));

            if (model == null || objSM.UserID != model.regByUser)
            {
                return RedirectToAction("UserUnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        public JsonResult UploadFile(HttpPostedFileBase File)
        {


            string Dirpath = "~/Content/writereaddata/AGC/";
            string path = "";
            string filename = "";
            if (!Directory.Exists(Server.MapPath(Dirpath)))
            {
                Directory.CreateDirectory(Server.MapPath(Dirpath));
            }
            string ext = Path.GetExtension(File.FileName);
            if (ext.ToLower() == ".jpg" || ext.ToLower() == ".pdf")
            {

                filename = Path.GetFileNameWithoutExtension(File.FileName) + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;
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

        public JsonResult getEsubcatList(int applicantTypeId)
        {
            var returndataset = objComDB.GetDropDownList(27, applicantTypeId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(returndataset, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegistrationConfirmation()
        {
            ViewBag.AGCRegisId = OTPL_Imp.CustomCryptography.Encrypt(TempData["RegisIdAGC"].ToString());
            ViewBag.AGCRegistrationNo = TempData["RegistrationNo"];
            return View();
        }

        void SendSMS(string registrationNo, string mobileNo)
        {

            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";

                //txtmsg = "Dear Citizen,\n\nYour Application form has been submitted successfully. Your Application Form Number is " + registrationNo + ", kindly use this further.\n\n Thanks";
                txtmsg = "Dear Citizen,\n\nYour Application form for Issuance of Age Certificate  has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\nMHFWD, UP";


                //string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo)); commented on 19/07/2023

                //objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }

        }

        #region Method GeneratedCertificateAGC
        public ActionResult GeneratedCertificateAGC(string regisId, string certGenrBy)
        {

            string setPdfName = "", setDigitalPdfName = "";
            var res = objCMODB.GetCertificateDetialAGC(Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId)));
            try
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rptCertificateAGC.rpt"));
                rd.SetDataSource(res);
                String dtnow = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                setPdfName = "AgeCertificate" + "_" + dtnow;
                setDigitalPdfName = "AgeCertificateDigitalSigned" + "_" + dtnow;
                string folderpath = "~/Content/writereaddata/PDF/";
                if (!System.IO.Directory.Exists(Server.MapPath(folderpath)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(folderpath));
                }
                string flName = folderpath + setPdfName + ".pdf";
                string digitalFlName = folderpath + setDigitalPdfName + ".pdf";
                rd.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                rd.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                rd.ExportOptions.DestinationOptions = new DiskFileDestinationOptions();
                ((DiskFileDestinationOptions)rd.ExportOptions.DestinationOptions).DiskFileName = Server.MapPath(flName);
                rd.Export();
                //FileInfo fileInfo = new FileInfo(Server.MapPath(flName));
                rd.Close();
                rd.Dispose();
                if (ConfigurationManager.AppSettings["IsDigitalSign"].ToString() == "N")
                {
                    FileInfo fileInfo = new FileInfo(Server.MapPath(flName));

                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + setPdfName + ".pdf");
                    Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
                    Response.WriteFile(flName);
                    Response.Flush();
                    Response.Close();
                    if (System.IO.File.Exists(Server.MapPath(flName)))
                    {
                        System.IO.File.Delete(Server.MapPath(flName));
                    }
                }
                else
                {
                    ////////////digital sign

                    var sigDetails = objComDB.GetDigitalSignatureDetails(Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(certGenrBy)));

                    float llx = 230;
                    float lly = 320;
                    float urx = 350;
                    float ury = 220;
                    Comman_Classes.DigitalCeritificateManager dcm = new Comman_Classes.DigitalCeritificateManager();
                    Comman_Classes.MetaData md = new Comman_Classes.MetaData()
                    {
                        Author = sigDetails.Author,
                        Title = "Age Certificate Authentication",
                        Subject = "Age Certificate",
                        Creator = sigDetails.Creator,
                        Producer = sigDetails.Producer,
                        Keywords = sigDetails.Keywords
                    };

                    string Signaturepath = Server.MapPath(sigDetails.Signaturepath);
                    dcm.signPDF(Server.MapPath(flName), Server.MapPath(digitalFlName), Signaturepath,
                     sigDetails.signpwd, "Authenticate Age Certificate", sigDetails.SigContact,
                     sigDetails.SigLocation, true, llx, lly, urx, ury, 1, md);

                    FileInfo fileInfo = new FileInfo(Server.MapPath(digitalFlName));
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + setDigitalPdfName + ".pdf");
                    Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
                    Response.WriteFile(Server.MapPath(digitalFlName));
                    Response.Flush();
                    Response.Close();

                    if (System.IO.File.Exists(Server.MapPath(flName)))
                    {
                        System.IO.File.Delete(Server.MapPath(flName));
                    }

                    if (System.IO.File.Exists(Server.MapPath(digitalFlName)))
                    {
                        System.IO.File.Delete(Server.MapPath(digitalFlName));
                    }

                    //////////////////////////digital sign end
                }
            }
            catch (Exception ex)
            {
                setSweetAlertMsg(ex.Message, "warning");
            }
            return RedirectToAction("ApprovedApplicationAGC");
        }
        #endregion

        #region Method Download File By Path
        public void DownloadFileByPath(string filePath)
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
        }
        #endregion
    }
}
