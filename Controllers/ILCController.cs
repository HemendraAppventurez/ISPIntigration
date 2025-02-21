using CCSHealthFamilyWelfareDept.DAL;
using CCSHealthFamilyWelfareDept.Filters;
using CCSHealthFamilyWelfareDept.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    [AuthorizeUser]
    public class ILCController : Controller
    {
        Common_DB objComDB = new Common_DB();
        ILC_DB objILC_DB = new ILC_DB();
        Common objCom = new Common();
        SessionManager objSM = new SessionManager();
        Account_DB objAccDb = new Account_DB();

        #region Method Set Sweet Alert Message
        protected void setSweetAlertMsg(string msg, string msgStatus)
        {
            ViewBag.Msg = msg;
            ViewBag.MsgStatus = msgStatus;
        }
        #endregion

        public JsonResult UploadFile(HttpPostedFileBase File)
        {


            string Dirpath = "~/Content/writereaddata/ILC/";
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

        #region checkfor Registration
        public ActionResult isRegister()
        {
            var res = objILC_DB.IsRegister();
            if (res.isExists == 1)
            {
                return RedirectToAction("ILCDashBoard", "ILC");
            }
            else if (res.isExists == 0)
            {
                return RedirectToAction("RegistrationInstructions", "ILC");
            }
            return View();
        }
        #endregion

        public ActionResult ILCDashBoard()
        {
            return View();
        }

        public ActionResult RegistrationInstructions(string AppType = "")
        {
            if (OTPL_Imp.CustomCryptography.Decrypt(AppType) == "Old")
            {
                TempData["AppType"] = "Old";
            }
            else if (OTPL_Imp.CustomCryptography.Decrypt(AppType) == "New")
            {
                TempData["AppType"] = "New";
            }
            return View();
        }

        [HttpGet]
        public ActionResult ILCRegistration(string AppType="")
        {
            #region ISP
            TempData["ISPUSER"] = "N";
            #endregion
            if (AppType=="Old")
            {
                TempData["AppType"]= "Old";
            }
            else if (AppType == "New")
            {
                TempData["AppType"] = "New";
            }
            #region ISP
            var application = TempData["ApplicantCommanDetails"] as ApplicantCommanDetails;
            TempData["ApplicantCommanDetails1"] = application;
            ILCModel model = new ILCModel();
            if (@TempData["ApplicantCommanDetails"] != null)
            {
                TempData["ISPUSER"] = "Y";
                model.fullName = application.first_name_eng + " " + application.middle_name_eng + " " + application.last_name_eng;
                model.fatherName = application.father_or_husband_or_guardian_name_eng;
                model.dob = application.dob.ToString("dd/MM/yyyy");

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
                model.rbtnGender = model.gender;
                model.categoryId = application.category;
                model.mobileNo = application.mobile;
                model.emailId = application.email;
                model.opdAddress = application.permanent_mohalla;
                //model.opdStateId = application.residential_state;
                model.opdPincode = application.permanent_pin;
                model.opdDistrictId = application.permanent_district;
                // model.markOfIdentification = application.markOfIdentification;
            }


            #endregion

            ViewBag.UPDistrict = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = objComDB.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = Enumerable.Empty<SelectListItem>(); //objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.InstitutionType = objComDB.GetDropDownList(29, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.forwardTo = Enumerable.Empty<SelectListItem>();
            ViewBag.forwardTypes = objILC_DB.rblforwardType().ToList();
            ViewBag.Category = objComDB.GetDropDownList(8, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.IdType = objComDB.GetDropDownList(46, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return View(model);
        }

        [HttpPost]
        public JsonResult CheckMobileExistence(string mobileNo)
        {
            var user = objILC_DB.CheckEmailMobileExistence(mobileNo, "M", objSM.UserID);
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ILCRegistration(ILCModel model,FormCollection frm)
        {
            bool isValidDate = true;
            ViewBag.UPDistrict = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = objComDB.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = Enumerable.Empty<SelectListItem>(); //objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.InstitutionType = objComDB.GetDropDownList(29, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.forwardTo = Enumerable.Empty<SelectListItem>();
            ViewBag.forwardTypes = objILC_DB.rblforwardType().ToList();
            ViewBag.Category = objComDB.GetDropDownList(8, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.IdType = objComDB.GetDropDownList(46, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            if (model.oldCertificateNumber == "" || model.oldCertificateNumber == null)
            {
                ModelState["appmobileNo"].Errors.Clear();
                ModelState["extOpdReceiptno"].Errors.Clear();
                ModelState["extInspectedDate"].Errors.Clear();
                ModelState["extOpdFile"].Errors.Clear();
                ModelState["extDoctorName"].Errors.Clear();
                ModelState["extTreatmentFrom"].Errors.Clear();
                ModelState["extTreatmentto"].Errors.Clear();
                ModelState["extNoOfDays"].Errors.Clear();
                ModelState["oldCertificateNumber"].Errors.Clear();
                ModelState["gender"].Errors.Clear();
                
                model.gender = model.rbtnGender;
            }
            else
            {
                //ModelState["extPhoto"].Errors.Clear();
                //ModelState["thumbSign"].Errors.Clear();
                ModelState["idFile"].Errors.Clear();
                ModelState["opdFile"].Errors.Clear();
                model.forwardtypeId =Convert.ToInt64(frm["hdnforwardtypeId"]);
                ModelState["gender"].Errors.Clear();

            }
          
           ModelState["appmobileNo"].Errors.Clear();
            if (!objCom.IsValidDateFormat(model.opdDate))
            {
                setSweetAlertMsg("Date of Check-up is in incorrect format, Date format should be DD/MM/YYYY !", "warning");
                isValidDate = false;
            }
            else if (!objCom.IsValidDateFormat(model.treatmentFrom))
            {
                setSweetAlertMsg("Treatment From Date is in incorrect format, Date format should be DD/MM/YYYY !", "warning");
                isValidDate = false;
            }
            else if (!objCom.IsValidDateFormat(model.treatmentto))
            {
                setSweetAlertMsg("Treatment To Date is in incorrect format, Date format should be DD/MM/YYYY !", "warning");
                isValidDate = false;
            }

            AuditMethods objAud = new AuditMethods();
            string errormsg = "";
            bool valStatus = objAud.IsValidLink(model.opdFilePath, "OPD File", out errormsg);
            if (!valStatus)
            {
                setSweetAlertMsg(errormsg, "warning");
                isValidDate = false;
            }

            else
            {
                valStatus = objAud.IsValidLink(model.idFilePath, "OPD File", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    isValidDate = false;
                }
            }

            if (isValidDate)
            {
                if (ModelState.IsValid)
                {
                    model.regByUser = objSM.UserID;
                    model.transIp = Common.GetIPAddress();
                    model.requestKey = objSM.AppRequestKey;
                    ResultSet res;
                    //var res = objILC_DB.ILCInsertUpdate(1, userId, model.opdName, model.opdReceiptno, model.opdDate, model.opdDistrictId, model.opdAddress, model.opdPincode, model.opdStateId, model.opdFilePath, model.fullName, model.fatherName, model.dob, model.gender, model.categoryId, model.mobileNo, model.emailId, model.forwardtypeId, model.forwardtoId, model.doctorName, model.reason, model.treatmentFrom, model.treatmentto, model.remarks, IpAddress);
                    if (model.oldCertificateNumber == "" || model.oldCertificateNumber == null)
                    {
                         res = objILC_DB.ILCInsertUpdate(model);
                    }
                    else
                    {
                         res = objILC_DB.ILCInsertUpdateExtendedDays(model);
                    }
                   

                    try
                    {
                        if (res.RegisId > 0 && res.RegistrationNo != "")
                        {

                            if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                            {
                                EDistrictServiceClass ed = new EDistrictServiceClass();

                                int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.ILC);

                                bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "ILC");

                                if (result)
                                {
                                    SendSMS(res.RegistrationNo, res.MobileNo);
                                    TempData["RegisId"] = res.RegisId;
                                    TempData["RegistrationNo"] = res.RegistrationNo;
                                    TempData["AppType"] = res.Flag;
                                    return RedirectToAction("RegistrationConfirmation");
                                }
                                else
                                {
                                    int exeRsult = objILC_DB.DeleteRegistrationILC(res.RegisId);
                                    setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                    ModelState.Clear();
                                }
                            }
                            else
                            {
                            SendSMS(res.RegistrationNo, res.MobileNo);
                            TempData["RegisId"] = res.RegisId;
                            TempData["RegistrationNo"] = res.RegistrationNo;
                            TempData["AppType"] = res.Flag;
                            #region ISP
                            if (res.RegistrationNo != null && TempData["ISPUSER"].ToString() == "Y")
                            {
                                returnApplicationAcknowledgementtoIspILC(model, TempData["ApplicantCommanDetails1"] as ApplicantCommanDetails, res.RegistrationNo);
                            }
                            #endregion
                            return RedirectToAction("RegistrationConfirmation");
                        }
                        }
                        else
                        {
                            setSweetAlertMsg("Record not save !", "error");
                            ModelState.Clear();
                        }
                    }
                    catch (Exception E)
                    {
                        setSweetAlertMsg("Some error occur !", "error");
                        ModelState.Clear();
                    }
                }
                else
                {
                    setSweetAlertMsg("Invalid Data Entered !", "warning");
                    ModelState.Clear();
                }
            }

            return View();
            //return RedirectToAction("ILCRegistration");
        }

        #region ISP
        public string returnApplicationAcknowledgementtoIspILC(ILCModel model, ApplicantCommanDetails model1, string RegistrationNo)
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
                Account_DB obj = new Account_DB();
                obj.ISPErrorLog("HitAPIWithTokennew", ex.Message, RegistrationNo, "User Action", "01");
                result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            return result;
        }
        #endregion

        public JsonResult BindForwardDropdownlist(long rollId, int opdDistrictId)
        {
            var res = objILC_DB.bindDropdownlist(rollId, opdDistrictId).Select(m => new SelectListItem { Text = m.forwardtoName, Value = m.forwardtoId.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult binddist(int opdStateId)
        {
            var res = objComDB.GetDropDownList(7, opdStateId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ViewILC()
        {
            ILCModel model = new ILCModel();
            long userId = objSM.UserID;
            model.ILCModelList = objILC_DB.GetILCList(userId);
            return View(model.ILCModelList);
        }

        public ActionResult ILCDetails(string regisId)
        { 
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            ILCModel model = new ILCModel();

            model = objILC_DB.GetILCListBYRegistrationNo(Convert.ToInt64(regisId));

            if (model == null || objSM.UserID != model.regByUser)
            {
                return RedirectToAction("UserUnauthoriseAcess", "Home");
            }

            return View(model);
        }

        public ActionResult RegistrationConfirmation()
        {
            ViewBag.ILCRegistrationNo = TempData["RegistrationNo"];
            ViewBag.RegisId = TempData["RegisId"];
            ViewBag.AppType = TempData["AppType"];
            return View();
        }

        public ActionResult PrintApplicationForm(string regisId)
        {
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            ILCModel model = new ILCModel();

            model = objILC_DB.GetILCListBYRegistrationNo(Convert.ToInt64(regisId));

            if (model == null || objSM.UserID != model.regByUser)
            {
                return RedirectToAction("UserUnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        void SendSMS(string registrationNo, string mobileNo)
        {

            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";


                txtmsg = "Dear Citizen,\n\nYour Application form for Issuance of Medical Certificate (Illness) has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\nMHFWD, UP";


                //string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo)); commented on 19/07/2023

                //objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }

        }

        #region Certificate Report
        public ActionResult ILCgeneratedCertificate(string regisId)
        {
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            string stausMessage = "";
            string setPdfName = "";
            string setDigitalPdfName = "";
            var res = objILC_DB.GetILCdetailCertificateRpt(Convert.ToInt64(regisId));
            //var res2 = objMECDB.GetMERCHILD(res[0].regisIdMER);
            try
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rpt_ILCcertificate.rpt"));
                rd.SetDataSource(res);

                String dtnow = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                setPdfName = "IllnessCertificate" + "_" + dtnow;
                setDigitalPdfName = "IllnessCertificateDigitalSigned" + "_" + dtnow;
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

                    var sigDetails = objComDB.GetDigitalSignatureDetails(objSM.UserID);

                    float llx = 570;
                    float lly = 320;
                    float urx = 450;
                    float ury = 220;
                    Comman_Classes.DigitalCeritificateManager dcm = new Comman_Classes.DigitalCeritificateManager();
                    Comman_Classes.MetaData md = new Comman_Classes.MetaData()
                    {
                        Author = sigDetails.Author,
                        Title = "Illness Certificate Authentication",
                        Subject = "Illness Certificate",
                        Creator = sigDetails.Creator,
                        Producer = sigDetails.Producer,
                        Keywords = sigDetails.Keywords
                    };

                    string Signaturepath = Server.MapPath(sigDetails.Signaturepath);
                    dcm.signPDF(Server.MapPath(flName), Server.MapPath(digitalFlName), Signaturepath,
                     sigDetails.signpwd, "Authenticate Illness Certificate", sigDetails.SigContact,
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
                stausMessage = "error_Error Occour to Downloading, Please try again.";
            }
            return RedirectToAction("ApprovedApplicationILC");
        }
        #endregion

        public JsonResult GetILCdetailByCertNo(string oldCertificateNumber)
        {
            var resultData = objILC_DB.GetILCdetailByCertNo(oldCertificateNumber, objSM.UserID);

            Session["regisIdILC"] = resultData.regisIdILC;
            TempData["dist"] = resultData.opdDistrictId;
            return Json(resultData, JsonRequestBehavior.AllowGet);

            //if (resultData != null)
            //{
            //    Session["regisIdILC"] = resultData.regisIdILC;
            //    return Json(resultData, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    ILCModel model = new ILCModel(); 
            //    model.regisIdILC = 0; 
            //    return Json(model, JsonRequestBehavior.AllowGet);
            //}
        }

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
