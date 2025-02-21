using CCSHealthFamilyWelfareDept.DAL;
using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCSHealthFamilyWelfareDept.Filters;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using System.Net;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    [AuthorizeUser]
    public class FICController : Controller
    {
        Common_DB objComDB = new Common_DB();

        FIC_DB objdb = new FIC_DB();
        Common objCom = new Common();
        Account_DB objAccDb = new Account_DB();
        SessionManager objSM = new SessionManager();
       
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
                return RedirectToAction("FICDashBoard", "FIC");
            }
            else if (res.isExists == 0)
            {
                return RedirectToAction("RegistrationInstructions", "FIC");
            }
            return View();
        }
        #endregion

        public ActionResult FICDashBoard()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegistrationInstructions(string AppType)
        {
            if (OTPL_Imp.CustomCryptography.Decrypt(AppType) == "Old")
            {
                TempData["AppType"] = "Old";
                TempData["ins"] = "Old";
            }
            else if (OTPL_Imp.CustomCryptography.Decrypt(AppType) == "New")
            {
                TempData["AppType"] = "New";
                TempData["ins"] = "New";
            }
            return View();
        }

        public JsonResult UploadFile(HttpPostedFileBase File)
        {
            string Dirpath = "~/Content/writereaddata/FIC/";
            string path = "";
            string filename = "";
            if (!Directory.Exists(Server.MapPath(Dirpath)))
            {
                Directory.CreateDirectory(Server.MapPath(Dirpath));
            }
            string ext = Path.GetExtension(File.FileName);
            if (ext.ToLower() == ".jpg" || ext.ToLower() == ".pdf")
            {

                filename = Path.GetFileNameWithoutExtension(File.FileName)+ "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;
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

        [HttpGet]
        public ActionResult FitnessCertificate()
        {
            FICModel model = new FICModel();
            #region ISP

            TempData["ISPUSER"] = "N";
            #endregion ISP

            if (TempData["AppType"] == "Old")
            {
                model.AppType = "Old";
            }
            else if (TempData["AppType"] == "New")
            {
                model.AppType = "New";
            }
            else
            {
                return RedirectToAction("FICDashBoard", "FIC");
            }

            #region ISP
            var application = TempData["ApplicantCommanDetails"] as ApplicantCommanDetails;
            TempData["ApplicantCommanDetails1"] = application;



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
                ;
                TempData["Gender"] = model.gender = application.gender;
                model.rbtnGender = model.gender;
                model.categoryId = application.category;
                model.mobileNo = application.mobile;
                model.emailId = application.email;
                model.opdAddress = application.permanent_mohalla;
                //model.opdStateId = application.residential_state;
                model.opdPinCode = application.permanent_pin;
                model.opdDistrictId = application.permanent_district;
                // model.markOfIdentification = application.markOfIdentification;
            }
            #endregion
            ViewBag.State = objComDB.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = Enumerable.Empty<SelectListItem>();
            ViewBag.UPDistrict = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Category = objComDB.GetDropDownList(8, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.InstitutionType = objComDB.GetDropDownList(29, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.forwardTo = Enumerable.Empty<SelectListItem>();
            ViewBag.forwardTypes = objdb.rblforwardType().ToList();
            ViewBag.IdType = objComDB.GetDropDownList(46, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return View(model);
        }

        [HttpPost]        
        public JsonResult CheckMobileExistence(string mobileNo)
        {
            var user = objdb.CheckEmailMobileExistence(mobileNo, "M");
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FitnessCertificate(FICModel model, string btnreset)
        {
            AuditMethods objAud = new AuditMethods();
            string errormsg = "";
            bool valStatus = false;
            model.gender = model.rbtnGender;
            if (model.AppType == "Old")
            {
                TempData["AppType"] = "Old";
                
            }
            else if (model.AppType == "New")
            {
                ModelState["certificateNo"].Errors.Clear();
                TempData["AppType"] = "New";
            }
            if (btnreset != null)
            {
                return RedirectToAction("FitnessCertificate", "FIC");
            }
            ViewBag.State = objComDB.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = Enumerable.Empty<SelectListItem>();
            ViewBag.UPDistrict = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Category = objComDB.GetDropDownList(8, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.InstitutionType = objComDB.GetDropDownList(29, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.forwardTo = Enumerable.Empty<SelectListItem>();
            ViewBag.forwardTypes = objdb.rblforwardType().ToList();
            ViewBag.IdType = objComDB.GetDropDownList(46, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            bool isValidDate = true;

            ModelState["illnessDetail"].Errors.Clear();
            ModelState["rejectedRemarks"].Errors.Clear();
           // ModelState["inspectionDate"].Errors.Clear();
            ModelState["inspectionRpt"].Errors.Clear();
            ModelState["inspectionRejectedRemark"].Errors.Clear();
            ModelState["appmobileNo"].Errors.Clear();


            ModelState["certGenerateByDoc"].Errors.Clear();
            ModelState["certGenerateByDesig"].Errors.Clear();
            ModelState["inspecttionCompeletionDate"].Errors.Clear();
            ModelState["identificationMark"].Errors.Clear();
            if (model.certificateNo == "" || model.certificateNo == null)
            {
                
            }
            else
            {
                ModelState["idFile"].Errors.Clear();
            }
            if (model.isMedicalHistory == "No")
            {
                ModelState["detailsMedicalHistory"].Errors.Clear();
            }
            else if (model.AppType == "Old")
            {
                ModelState["detailsMedicalHistory"].Errors.Clear();
            }
            else
            {
                if (!objCom.IsValidDateFormat(model.opdDate))
                {
                    setSweetAlertMsg("Date of Check-up is in incorrect format, Date format should be DD/MM/YYYY !", "warning");
                    isValidDate = false;
                }
                
            }

            valStatus = objAud.IsValidLink(model.opdFilePath, "OPD File", out errormsg);
            if (!valStatus)
            {
                setSweetAlertMsg(errormsg, "warning");
                isValidDate = false;
            }
            else
            {
                valStatus = objAud.IsValidLink(model.idFilePath, "Photo Id File", out errormsg);
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
                    //var res = objdb.FICInsertUpdate(1, model.regisIdFIC, userId, model.opdName, model.opdRecNo, model.opdDate, model.opdAddress, model.opdPinCode, model.opdStateId, model.opdDistrictId, model.opdFilePath, model.fullName, model.fatherName, model.dob, model.gender, model.categoryId, model.mobileNo, model.emailId, model.forwardtypeId, model.forwardtoId, model.fitnessAdvicedBy, model.treatmentFrom, model.treatmentto, model.applicationReason, model.illnessDetail, model.remarks, IpAddress);
                    ResultSet res;
                    try
                    {
                        if (model.certificateNo == "" || model.certificateNo == null)
                        {
                            res = objdb.FICInsertUpdate(model);
                        }
                        else
                        {
                            res = objdb.FICInsertUpdate_ILCtoFIC(model);
                        }
                         

                        if (res.RegisIdFIC > 0 && res.RegistrationNo != "")
                        {
                            if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                            {
                                EDistrictServiceClass ed = new EDistrictServiceClass();

                                int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.FIC);

                                bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "FIC");

                                if (result)
                                {
                                    SendSMS(res.RegistrationNo, res.MobileNo);
                                    TempData["RegisIdFIC"] = res.RegisIdFIC;
                                    TempData["RegistrationNo"] = res.RegistrationNo;
                                    return RedirectToAction("RegistrationConfirmation");
                                }
                                else
                                {
                                    int exeRsult = objdb.DeleteRegistrationFIC(res.RegisId);
                                    setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                    ModelState.Clear();
                                }
                            }
                            else
                            {
                                SendSMS(res.RegistrationNo, res.MobileNo);
                                TempData["RegisIdFIC"] = res.RegisIdFIC;
                                TempData["RegistrationNo"] = res.RegistrationNo;
                                #region ISP
                                if (res.RegistrationNo != null && TempData["ISPUSER"].ToString() == "Y")
                                {
                                    returnApplicationAcknowledgementtoIspFIC(model, TempData["ApplicantCommanDetails1"] as ApplicantCommanDetails, res.RegistrationNo);
                                }
                                #endregion
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
                    setSweetAlertMsg("Invalid Data Entered", "warning");
                }
            }

            return View(model);
            //return RedirectToAction("FitnessCertificate");
        }


        #region ISP
        public string returnApplicationAcknowledgementtoIspFIC(FICModel model, ApplicantCommanDetails model1, string RegistrationNo)
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

        public JsonResult GetILCdetailByCertNo(string oldCertificateNumber)
        {
            var resultData = objdb.GetILCdetailByCertNo(oldCertificateNumber);
            if (resultData != null)
            {
                Session["regisIdILC"] = resultData.regisIdILC;
                return Json(resultData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                FICModel model = new FICModel();
                model.regisIdILC = 0;
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult binddist(int opdStateId)
        {
            var res = objComDB.GetDropDownList(7, opdStateId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLatestILCDetailByCrtificateNo(string oldCertificateNumber)
        {
            var resultData = objdb.GetLatestILCDetailByCrtificateNo(oldCertificateNumber, objSM.UserID);

            Session["regisIdILC"] = resultData.regisIdILC;

            return Json(resultData, JsonRequestBehavior.AllowGet);

            //if (resultData != null)
            //{
            //    string msg = resultData.Msg.FirstOrDefault().ToString();
            //    if (msg == "1")
            //    {
            //        Session["regisIdILC"] = resultData.regisIdILC;
            //        return Json(resultData, JsonRequestBehavior.AllowGet);
            //    }
            //    else
            //    {
            //        FICModel model = new FICModel();
            //        model.regisIdILC = Convert.ToInt32(msg);
            //        return Json(model, JsonRequestBehavior.AllowGet);
            //    }
            //}
            //else
            //{
            //    FICModel model = new FICModel();
            //    model.regisIdILC = 0;
            //    return Json(model, JsonRequestBehavior.AllowGet);
            //}
        }

        public JsonResult BindForwardDropdownlist(long rollId, int opdDistrictId)
        {
            var res = objdb.bindDropdownlist(rollId, opdDistrictId).Select(m => new SelectListItem { Text = m.forwardtoName, Value = m.forwardtoId.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewFIC()
        {
            FICModel model = new FICModel();
            long userId = objSM.UserID;
            model.FICModelList = objdb.GetFICList(userId);
            return View(model.FICModelList);
        }

        [HttpGet]
        public ActionResult ViewFICdetail(string registrationNo)
        {
            long regisByuser = objSM.UserID;
            registrationNo = OTPL_Imp.CustomCryptography.Decrypt(registrationNo);
            FICModel model = new FICModel();

            model = objdb.GetFICListBYRegistrationNo(registrationNo);

            if (model == null || objSM.UserID != model.regByUser)
            {
                return RedirectToAction("UserUnauthoriseAcess", "Home");
            }

            return View(model);
        }

        public ActionResult PrintApplicationForm(string registrationNo, string regisIdDIC)
        {
            long regisByuser = objSM.UserID;
            registrationNo = OTPL_Imp.CustomCryptography.Decrypt(registrationNo);
            FICModel model = new FICModel();

            model = objdb.GetFICListBYRegistrationNo(registrationNo);

            if (model == null || objSM.UserID != model.regByUser)
            {
                return RedirectToAction("UserUnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        public ActionResult RegistrationConfirmation()
        {
            ViewBag.FICRegisId = OTPL_Imp.CustomCryptography.Encrypt(TempData["RegisIdFIC"].ToString());
            ViewBag.FICRegistrationNo = TempData["RegistrationNo"];
            return View();
        }

        void SendSMS(string registrationNo, string mobileNo)
        {

            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";


                txtmsg = "Dear Citizen,\n\nYour Application form for Issuance of Medical Certificate (Fitness) has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\n MHFWD, UP";


                //string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));commented on 19/07/2023

                //objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }

        }

        #region Certificate Report
        public ActionResult FICgeneratedCertificate(string regisId)
        {
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            string stausMessage = "";
            string setPdfName = "";
            string setDigitalPdfName = "";
            var res = objdb.GetFICdetailCertificateRpt(Convert.ToInt64(regisId));
            //var res2 = objMECDB.GetMERCHILD(res[0].regisIdMER);
            try
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rpt_FICcertificate.rpt"));
                rd.SetDataSource(res);

                String dtnow = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                setPdfName = "FitnessCertificate" + "_" + dtnow;
                setDigitalPdfName = "FitnessCertificateDigitalSigned" + "_" + dtnow;
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
                        Title = "Fitness Certificate Authentication",
                        Subject = "Fitness Certificate",
                        Creator = sigDetails.Creator,
                        Producer = sigDetails.Producer,
                        Keywords = sigDetails.Keywords
                    };

                    string Signaturepath = Server.MapPath(sigDetails.Signaturepath);
                    dcm.signPDF(Server.MapPath(flName), Server.MapPath(digitalFlName), Signaturepath,
                     sigDetails.signpwd, "Authenticate Fitness Certificate", sigDetails.SigContact,
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
            return RedirectToAction("ApprovedApplicationFIC");
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
