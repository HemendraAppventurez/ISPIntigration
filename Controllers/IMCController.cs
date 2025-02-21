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
    public class IMCController : Controller
    {

        Common_DB objComDB = new Common_DB();
        Common objCom = new Common();
        SessionManager objSM = new SessionManager();
        IMC_DB objIMC_DB = new IMC_DB();
        Account_DB objAccDb = new Account_DB();
        Common comModel = new Common();

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
            var res = objIMC_DB.IsRegister();
            if (res.isExists == 1)
            {
                return RedirectToAction("ImmunizationDashBoard", "IMC");
            }
            else if (res.isExists == 0)
            {
                return RedirectToAction("RegistrationInstructions", "IMC");
            }
            return View();
        }
        #endregion

        public ActionResult RegistrationInstructions()
        { 
            return View();
        }

        public ActionResult ImmunizationDashBoard()
        {
            return View();
        }

        public JsonResult UploadFile(HttpPostedFileBase File)
        {

            string ImgValidation = comModel.ValidateImageExtWithSizeForDocuments(File);


            string Dirpath = "~/Content/writereaddata/IMC/";
            string path = "";
            string filename = "";
            string flag="";
            if (!Directory.Exists(Server.MapPath(Dirpath)))
            {
                Directory.CreateDirectory(Server.MapPath(Dirpath));
            }
            string ext = Path.GetExtension(File.FileName);
            if (ImgValidation == "Valid")
            {

                filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + File.FileName;
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
                    flag = "0";
                }
           
            }
            else {
                path = ImgValidation;
                flag = ImgValidation;
            }
            
            List<string> plist = new List<string> { File.FileName, path ,flag};
            return Json(plist);


        }

        [HttpGet]
        public ActionResult IMCRegistration()
        {
            long userId = objSM.UserID;
            IMCModel model = new IMCModel();

            #region ISP
            TempData["ISPUSER"] = "N";
            var application = TempData["ApplicantCommanDetails"] as ApplicantCommanDetails;
            TempData["ApplicantCommanDetails1"] = application;
            //ILCModel model = new ILCModel();
            if (@TempData["ApplicantCommanDetails"] != null)
            {
                TempData["ISPUSER"] = "Y";
                model.fullName = application.first_name_eng + " " + application.middle_name_eng + " " + application.last_name_eng;
                model.fatherName = application.father_or_husband_or_guardian_name_eng;
                model.dob = application.dob.ToString("dd/MM/yyyy");
                model.mobileNo = application.mobile;
                model.emailId = application.email;
                model.address = application.permanent_mohalla;
                model.pinCode = application.permanent_pin;
            }
            #endregion

            if (userId != 0)
            {
                //model = objIMC_DB.GetIMCById(userId);
               
                ViewBag.DistrictCHCPCH = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.State = objComDB.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
               ViewBag.ImmunizationVaccine = objComDB.GetDropDownList(44, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.forwardTo = Enumerable.Empty<SelectListItem>();
                ViewBag.District = Enumerable.Empty<SelectListItem>();
                ViewBag.forwardTypes = objIMC_DB.rblforwardType().ToList();
               // model.appImmunList = objIMC_DB.BindImmunizationDetails();
                ViewBag.ImmunizationType = objComDB.GetDropDownList(54, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.Submit = "Submit";
                ViewBag.Cancel = "Cancel";
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult CheckMobileExistence(string mobileNo)
        {
            var user = objIMC_DB.CheckEmailMobileExistence(mobileNo, "M");
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IMCRegistration(IMCModel model,FormCollection frm)
        {
            
            AuditMethods objAud = new AuditMethods();
            string errormsg = "";
            bool valStatus = false;
            ModelState["appmobileNo"].Errors.Clear();
            long userId = objSM.UserID;
            string IpAddress = Common.GetIPAddress();
            model.requestKey = objSM.AppRequestKey;
            #region bulkinsertion
            string xmldata = "<vaccines>";
            if (model.isVaccined == true)
            {
                var vaccineId = frm.GetValues("addimmunId");
                var ImmunizationDate = frm.GetValues("adddateOfImmunization");
                var doctorName = frm.GetValues("addimmunizationDoctor");
                var immunizationProof = frm.GetValues("addimmunizationProofFilePath");
                int count = ImmunizationDate.Count();

                if (count == 0)
                {
                    TempData["msg"] = "Please Select Atleast One Immunization Detail ";
                    TempData["msgstatus"] = "warning";
                    return RedirectToAction("IMCRegistration");
                }

                valStatus = objAud.IsValidLink(model.opdFilePath, "OPD Receipt", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }

                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(immunizationProof[i]))
                    {
                        valStatus = objAud.IsValidLink(immunizationProof[i], "Immunization Proof", out errormsg);
                        if (!valStatus)
                        {
                            setSweetAlertMsg(errormsg, "warning");
                            return View(model);
                        }
                    }
                    if (ImmunizationDate[i] != "")
                    {
                        xmldata += "<vaccine><vaccineId>" + vaccineId[i] + "</vaccineId>"
                         + "<ImmunizationDate>" + ImmunizationDate[i] + "</ImmunizationDate>"
                         + "<doctorName>" + objAud.RemoveSpecialCharactors(doctorName[i]) + "</doctorName>"
                         + "<immunizationProof>" + immunizationProof[i] + "</immunizationProof>"
                           + "</vaccine>";
                    }
                    else
                    {
                        
                        TempData["msg"] = "Immunization Date Required!";
                        TempData["msgstatus"] = "warning";
                        return RedirectToAction("IMCRegistration");
                    }


                }
            }
            else
            {
                string[] vaccineId = frm.GetValues("chkvaccine");
               
                for (int i = 0; i < vaccineId.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(vaccineId[i]))
                    {
                        xmldata += "<vaccine><vaccineId>" + vaccineId[i] + "</vaccineId></vaccine>";
                    }
                }
            }
            
            xmldata += "</vaccines>";


            #endregion
                if (!model.isAlreadyTaken)
                {

                    ModelState["DH_PHC_CHCProofFile"].Errors.Clear();
                    ModelState["DH_PHC_CHCDate"].Errors.Clear();
                    ModelState["hospitalEstablishment"].Errors.Clear();
                }
                if (model.isVaccined == false)
                {
                    ModelState["opdReciept"].Errors.Clear();
                    //ModelState["vaccineDate"].Errors.Clear();
                    //ModelState["vaccineOldName"].Errors.Clear();
                    ModelState["opdFile"].Errors.Clear();
                }

            if (ModelState.IsValid)
            {
                if (model.DH_PHC_CHCDate != null)
                {
                    if (objCom.IsValidDateFormat(model.DH_PHC_CHCDate))
                    {
                        model.DH_PHC_CHCDate = Convert.ToDateTime(model.DH_PHC_CHCDate).ToString();

                        try
                        {

                            var res = objIMC_DB.InsertUpdate_IMC(model.regisIdIMC, userId,
                            model.isVaccined, model.reason, model.opdReciept, model.opdFilePath,
                            model.fullName, model.fatherName, model.dob, model.age, model.mobileNo, model.emailId, model.passportNo,
                            model.address, model.stateId, model.districtId, model.pinCode, model.markOfIdentification,
                            model.forwardtypeId, model.forwardtoId, model.forwardDistrictId,
                            IpAddress, model.requestKey, xmldata);
                            if (res.RegisId > 0 && res.RegistrationNo != "")
                            {

                                if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                                {
                                    EDistrictServiceClass ed = new EDistrictServiceClass();

                                    int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.IMC);

                                    bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "IMC");

                                    if (result)
                                    {
                                        SendSMS(res.RegistrationNo, res.MobileNo);
                                        TempData["RegisId"] = res.RegisId;
                                        TempData["RegistrationNo"] = res.RegistrationNo;
                                        TempData["msg"] = "Record Save Successfully";
                                        TempData["status"] = "success";
                                        #region ISP
                                        if (res.RegistrationNo != null && TempData["ISPUSER"].ToString() == "Y")
                                        {
                                            returnApplicationAcknowledgementtoIspIMC(model, TempData["ApplicantCommanDetails1"] as ApplicantCommanDetails, res.RegistrationNo);
                                        }
                                        #endregion
                                        return RedirectToAction("RegistrationConfirmation");
                                    }
                                    else
                                    {
                                        int exeRsult = objIMC_DB.DeleteRegistrationIMC(res.RegisId);
                                        setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                        ModelState.Clear();
                                    }
                                }
                                else
                                {
                                    SendSMS(res.RegistrationNo, res.MobileNo);
                                    TempData["RegisId"] = res.RegisId;
                                    TempData["RegistrationNo"] = res.RegistrationNo;
                                    TempData["msg"] = "Record Save Successfully";
                                    TempData["status"] = "success";
                                    return RedirectToAction("RegistrationConfirmation");
                                }
                            }
                            else
                            {
                                TempData["msg"] = "Record not Save";
                                TempData["status"] = "error";

                            }
                        }
                        catch (Exception E)
                        {
                            TempData["msg"] = "Some error Occur!";
                            TempData["status"] = "error";

                        }


                    }
                    else
                    {
                        TempData["msg"] = "Date is incorrect format, Date format should be DD/MM/YYYY !";
                        TempData["status"] = "warning";
                    }
                }
                else
                {
                    try
                    {
                       
                        
                        //var res = objIMC_DB.InsertUpdate_IMC(1, model.regisIdIMC, userId, model.isVaccined, model.fullName, model.fatherName, model.dob, model.age, model.mobileNo, model.emailId,
                        //        model.passportNo, model.stateId, model.districtId, model.address, model.pinCode, model.markOfIdentification, model.immuCertiTypeId, model.reason, model.opdReciept, model.opdFilePath,
                        //        model.isAlreadyTaken,
                        //        model.forwardtypeId, model.forwardtoId, model.DH_PHC_CHCProofFilePath, model.DH_PHC_CHCDate, model.hospitalEstablishment, model.isCertify, IpAddress, IpAddress,
                        //        model.forwardDistrictId, model.requestKey, xmldata);
                        var res = objIMC_DB.InsertUpdate_IMC(model.regisIdIMC, userId,
                            model.isVaccined, model.reason, model.opdReciept, model.opdFilePath, 
                            model.fullName, model.fatherName,model.dob, model.age, model.mobileNo, model.emailId,model.passportNo, 
                            model.address, model.stateId, model.districtId, model.pinCode, model.markOfIdentification,  
                            model.forwardtypeId, model.forwardtoId, model.forwardDistrictId,  
                            IpAddress,model.requestKey, xmldata);
                        if (res.RegisId > 0 && res.RegistrationNo != "")
                        {
                            if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                            {
                                EDistrictServiceClass ed = new EDistrictServiceClass();

                                int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.IMC);

                                bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "IMC");

                                if (result)
                                {
                                    SendSMS(res.RegistrationNo, res.MobileNo);
                                    TempData["RegisId"] = res.RegisId;
                                    TempData["RegistrationNo"] = res.RegistrationNo;
                                    TempData["msg"] = "Record Save Successfully";
                                    TempData["status"] = "success";
                                    return RedirectToAction("RegistrationConfirmation");
                                }
                                else
                                {
                                    int exeRsult = objIMC_DB.DeleteRegistrationIMC(res.RegisId);
                                    setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                    ModelState.Clear();
                                }
                            }
                            else
                            {
                            SendSMS(res.RegistrationNo,res.MobileNo);
                            TempData["RegisId"] = res.RegisId;
                            TempData["RegistrationNo"] = res.RegistrationNo;
                            TempData["msg"] = "Record Save Successfully";
                            TempData["status"] = "success";
                            #region ISP
                            if (res.RegistrationNo != null && TempData["ISPUSER"].ToString() == "Y")
                            {
                                returnApplicationAcknowledgementtoIspIMC(model, TempData["ApplicantCommanDetails1"] as ApplicantCommanDetails, res.RegistrationNo);
                            }
                            #endregion
                            return RedirectToAction("RegistrationConfirmation");
                        }
                        }
                        else
                        {
                            TempData["msg"] = "Record not Save";
                            TempData["status"] = "error";

                        }
                    }
                    catch (Exception E)
                    {
                        TempData["msg"] = "Some error Occur!";
                        TempData["status"] = "error";

                    }
                }

            }
                
            else
            {
                TempData["msg"] = "Invalid Data Entered!";
                TempData["status"] = "error";
            }


            return RedirectToAction("IMCRegistration");
        }

        #region ISP
        public string returnApplicationAcknowledgementtoIspIMC(IMCModel model, ApplicantCommanDetails model1, string RegistrationNo)
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

        public ActionResult RegistrationConfirmation()
        {
            ViewBag.IMCRegistrationNo = TempData["RegistrationNo"];
            return View();
        }

        public ActionResult ViewIMCList()
        {
            long userId = objSM.UserID;
            IMCModel model = new IMCModel();
            model.IMCModelList = objIMC_DB.getIMCList(userId);
            return View(model.IMCModelList);
        }

        public ActionResult IMCDetails(string regisId)
        {
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            Session["regisId"] = regisId;
            IMCModel model = new IMCModel();

            model = objIMC_DB.IMCDetailsByRegistration(Convert.ToInt64(regisId));

            if (model == null || objSM.UserID != model.regByUser)
            {
                return RedirectToAction("UserUnauthoriseAcess", "Home");
            }

            return View(model);
        }

        public ActionResult BindImmunList()
        {
            IMCimmunizationModel model = new IMCimmunizationModel();
            model.appImmunizationList = objIMC_DB.getIMCimmunList(Convert.ToInt64(Session["regisId"].ToString()));
            return PartialView("_ViewImmunization", model.appImmunizationList);
        }

        public ActionResult PrintApplicationForm(string regisId)
        {
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            Session["regisId"] = regisId;
            IMCModel model = new IMCModel();

            model = objIMC_DB.IMCDetailsByRegistration(Convert.ToInt64(regisId));

            if (model == null || objSM.UserID != model.regByUser)
            {
                return RedirectToAction("UserUnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        public JsonResult BindForwardDropdownlist(long rollId, int DistrictId)
        {
            var res = objIMC_DB.bindDropdownlist(rollId, DistrictId).Select(m => new SelectListItem { Text = m.forwardtoName, Value = m.forwardtoId.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindDistrictlist(int stateId)
        {
            var res = objComDB.GetDropDownList(7, stateId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        void SendSMS(string registrationNo, string mobileNo)
        {

            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";


                txtmsg = "Dear Citizen,\n\nYour Application form for Issuance of Immunization Certificate has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\nMHFWD, UP";


                //string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));commented on 19/07/2023

                //objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }

        }
        
        #region Immunization Certificate Riya
        public ActionResult IMCgeneratedCertificate(long regisIdIMC)
        {
            string stausMessage = "";
            string setPdfName = "", setDigitalPdfName = "";
            var res = objIMC_DB.GetDetailcertificate(regisIdIMC);
            try
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rptCertificateIMC.rpt"));
                rd.SetDataSource(res);
                //ReportDocument subShows = rd.Subreports["rpt_NUHcertificateChild.rpt"];
                //subShows.SetDataSource(res2);
                String dtnow = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                setPdfName = "ImmunizationCertificate" + "_" + dtnow;
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



                //////////////////////////digital sign end

            }

            catch (Exception ex)
            {
                stausMessage = "error_Error Occour to Downloading, Please try again.";
            }
            return RedirectToAction("ApprovedApplicationNUH");
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
