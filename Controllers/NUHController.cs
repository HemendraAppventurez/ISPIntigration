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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    [AuthorizeUser]
    public class NUHController : Controller
    {
        #region Riya
        NUH_DB objNUHDB = new NUH_DB();
        Common_DB comndb = new Common_DB();
        Common objCom = new Common();
        SessionManager objSM = new SessionManager();
        Account_DB objAccDb = new Account_DB();
        Common comModel = new Common();
        CMO_DB objCMODB = new CMO_DB();

        //
        // GET: /NUH/

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
            var resultData = objNUHDB.GetRegisterDetails();



            if (resultData == null)
            {
                //return RedirectToAction("RegistrationInstructions", "NUH");
                TempData["Event"] = "Dash";
                return RedirectToAction("NursingDashBoard", "NUH");
            }
            //else
            //    if (resultData.regisIdNUH > 0 && !string.IsNullOrEmpty(resultData.DeclarationDate))
            //    {
            //        string _regisId = OTPL_Imp.CustomCryptography.Encrypt(resultData.regisIdNUH.ToString());
            //        return RedirectToAction("Affidavit", new { regisIdNUH = _regisId, isRenewal = resultData.isRenewal, operatedId = resultData.operatedId });
            //        //return RedirectToAction("NursingDashBoard", "NUH");
            //        //return RedirectToAction("UploadAffidavit", "NUH", new { regisId = _regisId });
            //    }
            //    else if (resultData.regisIdNUH > 0 && !string.IsNullOrEmpty(resultData.ParamedicalDeclarationDate))
            //    {

            //        string _regisId = OTPL_Imp.CustomCryptography.Encrypt(resultData.regisIdNUH.ToString());
            //        return RedirectToAction("Pairamedical", "NUH", new { regisIdNUH = _regisId, isRenewal = resultData.isRenewal });

            //    }
            //    else if (!string.IsNullOrEmpty(resultData.DeclarationDate) && !string.IsNullOrEmpty(resultData.ParamedicalDeclarationDate))
            //    {
            //        TempData["Event"] = "Dash";
            //        return RedirectToAction("NursingDashBoard", "NUH");
            //    }
            else if ((resultData.regisIdNUH > 0) && (resultData.DeclarationDate == null || resultData.DeclarationDate == ""))
            {
                string _regisId = OTPL_Imp.CustomCryptography.Encrypt(resultData.regisIdNUH.ToString());
                return RedirectToAction("Affidavit", new { regisIdNUH = _regisId, isRenewal = resultData.isRenewal, operatedId = resultData.operatedId});

            }

            else if ((resultData.regisIdNUH > 0) && (resultData.ParamedicalDeclarationDate == null || resultData.ParamedicalDeclarationDate == ""))
            {

                string _regisId = OTPL_Imp.CustomCryptography.Encrypt(resultData.regisIdNUH.ToString());
                return RedirectToAction("Pairamedical", "NUH", new { regisIdNUH = _regisId, isRenewal = resultData.isRenewal });

            }
            else if ((resultData.DeclarationDate != "" || resultData.DeclarationDate != null) && (resultData.ParamedicalDeclarationDate != "" || resultData.ParamedicalDeclarationDate != null))
            {
                TempData["Event"] = "Dash";
                return RedirectToAction("NursingDashBoard", "NUH");
            }
            return View();
        }

        #endregion

        public ActionResult NursingDashBoard()
        {
            NUHmodel model = new NUHmodel();
            SessionManager SM = new SessionManager();
            int procId = 1;

            if (SM.ControlID != "" && SM.UnitID != "" && SM.ServiceID != "" && SM.RequestID != "")
            {
                model.NUHModelList = objNUHDB.GetApplicationByRequestId(procId, SM.ControlID, SM.UnitID, SM.ServiceID, SM.RequestID);
            }

            return View(model.NUHModelList);
            //return View();
        }


        public ActionResult BackToNiveshMitra()
        {
            string Dept_Code = ConfigurationManager.AppSettings["Dept_Code"].ToString();
            string ControlID = objSM.ControlID;
            string UnitID = objSM.UnitID;
            string ServiceID = objSM.ServiceID;
            string PassSalt = ConfigurationManager.AppSettings["PassKey"].ToString();

            ViewBag.ActionURL = ConfigurationManager.AppSettings["UPSwpNiveshMitraServicesBackUrl"].ToString();
            string EncryptionKey = ConfigurationManager.AppSettings["UPSwpNiveshMitraEncrptionKey"].ToString();

            UBNICMethodsLib.CommanClass oc = new UBNICMethodsLib.CommanClass();

            ViewBag.Dept_Code = oc.EncryptString(EncryptionKey, Dept_Code);
            ViewBag.ControlID = oc.EncryptString(EncryptionKey, ControlID);
            ViewBag.UnitID = oc.EncryptString(EncryptionKey, UnitID);
            ViewBag.ServiceID = oc.EncryptString(EncryptionKey, ServiceID);
            ViewBag.PassSalt = oc.EncryptString(EncryptionKey, PassSalt);

            return View();
        }

        public ActionResult RegistrationInstructions()
        {
            return View();
        }

        public ActionResult RegistrationInstructionsRenew()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NursingHome(string type = "")
        {


            if (type == "renewal")
            {
                TempData["isRenewal"] = "True";
            }
            else
            {
                TempData["isRenewal"] = "False";
            }
            var resultData = objNUHDB.GetRegisterDetails();



            if (resultData != null && resultData.regisIdNUH > 0 && string.IsNullOrEmpty(resultData.DeclarationDate))
            {
                string _regisId = OTPL_Imp.CustomCryptography.Encrypt(resultData.regisIdNUH.ToString());
                return RedirectToAction("Affidavit", new { regisIdNUH = _regisId, isRenewal = resultData.isRenewal, operatedId = resultData.operatedId });
                //return RedirectToAction("NursingDashBoard", "NUH");
            }
            else if ((resultData != null)&&(resultData.regisIdNUH > 0) && (resultData.ParamedicalDeclarationDate == null || resultData.ParamedicalDeclarationDate == ""))
            {

                string _regisId = OTPL_Imp.CustomCryptography.Encrypt(resultData.regisIdNUH.ToString());
                return RedirectToAction("Pairamedical", "NUH", new { regisIdNUH = _regisId, isRenewal = resultData.isRenewal });

            }
            else
            {
                NUHmodel model = new NUHmodel();
                SessionManager SM = new SessionManager();
                int procId = 3;
                if (SM.ControlID != "" && SM.UnitID != "" && SM.ServiceID != "" && SM.RequestID != "")
                {
                    model = objNUHDB.GetDistrictByRequestId(procId, SM.ControlID, SM.UnitID, SM.ServiceID, SM.RequestID);
                }

                ViewBag.MedicalEstablishment = comndb.GetDropDownList(5, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.MedicalEstablishmentCategory = comndb.GetDropDownList(9, 0).ToList();
                ViewBag.MedicalEstablishmentCategoryType = Enumerable.Empty<SelectListItem>();
                // ViewBag.SystemOfMedicine = comndb.GetDropDownList(12, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.ClinicalEstablishment = comndb.GetDropDownList(13, 0).ToList();

                ViewBag.ClinicalSubEstablishment = Enumerable.Empty<SelectListItem>();
                //  ViewBag.ClinicalService = comndb.GetDropDownList(11, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.State1 = comndb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.District1 = Enumerable.Empty<SelectListItem>();
                ViewBag.State = comndb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.District = comndb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.Operate = comndb.GetDropDownList(37, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });


                ViewBag.OutPatient = comndb.GetDropDownList(49, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.Laboratory = comndb.GetDropDownList(50, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.Imaging = comndb.GetDropDownList(51, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                return View(model);
            }
        }

        // [HttpPost]
        //public JsonResult CheckMobileExistence(string applicantMobileNo)
        //{
        //    var user = objNUHDB.CheckEmailMobileExistence(applicantMobileNo, "M");
        //    bool isExisting = user.isExists == 0;
        //    return Json(isExisting);
        //}

        [HttpPost]
        public JsonResult CheckMobileExistence(string applicantMobileNo)
        {
            //long regisId = 0; 
            //if (Session["regisIdNUH"] != null)
            //{
            //    regisId = Convert.ToInt64(Session["regisIdNUH"]);
            //}

            var user = objNUHDB.CheckEmailMobileExistence(applicantMobileNo, "M", 0, objSM.MeeRegisNo, objSM.UserID);
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }
        public JsonResult binddist(int ownerStateIdF)
        {
            var res = comndb.GetDropDownList(7, ownerStateIdF).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Dropdown()
        {
            var res = comndb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NursingHome(NUHmodel model, FormCollection form)//insert NUH data
        {
            if (model.numberofBed > 49)
            {
                setSweetAlertMsg("Number of Beds(Inpatient) Can Not Be More Than 49", "info");
                return View(model);
            }
            ModelState["queryFile"].Errors.Clear();
            ModelState["QueryRaisedByCMO"].Errors.Clear();
            if (model.isCertificateFromPortal == true)
            {
                ModelState["outerRegistrationNo"].Errors.Clear();
                ModelState["outerCertificateNo"].Errors.Clear();
                ModelState["outerCertificateFile"].Errors.Clear();
                model.isRenewal = false;
            }
            else
            {
                model.isRenewal = true;
            }
            if (model.isBelongToMedical == true && model.operatedId != 2)
            {
                model.applicantDistrictId = Convert.ToInt32(model.ownerDistrictIdF);
                model.applicantStateId = Convert.ToInt32(model.ownerStateIdF);
            }
            ViewBag.MedicalEstablishment = comndb.GetDropDownList(5, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.MedicalEstablishmentCategory = comndb.GetDropDownList(9, 0).ToList();
            ViewBag.MedicalEstablishmentCategoryType = Enumerable.Empty<SelectListItem>();
            ViewBag.Operate = comndb.GetDropDownList(37, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.ClinicalEstablishment = comndb.GetDropDownList(13, 0).ToList();
            ViewBag.ClinicalSubEstablishment = Enumerable.Empty<SelectListItem>();
            ViewBag.State = comndb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = comndb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State1 = comndb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District1 = Enumerable.Empty<SelectListItem>();

            ViewBag.OutPatient = comndb.GetDropDownList(49, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Laboratory = comndb.GetDropDownList(50, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Imaging = comndb.GetDropDownList(51, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            AuditMethods objAud = new AuditMethods();
            string errormsg = "";
            bool valStatus = false;

            #region Bulk Insertion owner detail
            string XmlDataOwner = "<Owner>";
            if (model.operatedId == 2)
            {
                string valid;

                ModelState["ownerNameF"].Errors.Clear();
                ModelState["ownerAgeF"].Errors.Clear();
                ModelState["ownerFatherNameF"].Errors.Clear();
                ModelState["ownerMobileNoF"].Errors.Clear();
                ModelState["ownerEmailIdF"].Errors.Clear();
                ModelState["ownerAddressF"].Errors.Clear();
                ModelState["ownerStateIdF"].Errors.Clear();
                ModelState["ownerDistrictIdF"].Errors.Clear();
                ModelState["ownerPincodeF"].Errors.Clear();
                //ModelState["ownerPhotograph"].Errors.Clear();
                ModelState["ownerFPhotograph"].Errors.Clear();
                // ModelState["ownerSignature"].Errors.Clear();
                ModelState["ownerFSignature"].Errors.Clear();


                var ownerName = form.GetValues("ownerName");
                var ownerAge = form.GetValues("ownerAge");
                var ownerFatherName = form.GetValues("ownerFatherName");
                var ownerMobileNo = form.GetValues("ownerMobileNo");
                var ownerEmailId = form.GetValues("ownerEmailId");
                var ownerAddress = form.GetValues("ownerAddress");
                var ownerStateId = form.GetValues("ownerStateId");
                var ownerDistrictId = form.GetValues("ownerDistrictId");
                var ownerPincode = form.GetValues("ownerPincode");
                var ownerPhotographPath = form.GetValues("ownerPhotographPath");
                var ownerSignaturePath = form.GetValues("ownerSignaturePath");
                int countOwner = ownerName.Count();

                Regex regemail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

                for (int i = 0; i < countOwner; i++)
                {
                    if (ownerName[i].ToString() == "" && ownerAddress[i] == "" && ownerFatherName[i] == "" && ownerAge[i].ToString() == "" && ownerMobileNo[i] == "" && ownerEmailId[i] == "")
                    {
                        //XmlData = string.Empty;
                    }
                    else if (ownerMobileNo[i].Length == 10 && ownerPincode[i].Length == 6)
                    {
                        Match match = regemail.Match(ownerEmailId[i]);
                        if (match.Success)
                        {

                            valStatus = objAud.IsValidLink(ownerPhotographPath[i], "Photograph", out errormsg);
                            if (!valStatus)
                            {
                                setSweetAlertMsg(errormsg, "warning");
                                return View(model);
                            }
                            valStatus = objAud.IsValidLink(ownerSignaturePath[i], "Signature", out errormsg);
                            if (!valStatus)
                            {
                                setSweetAlertMsg(errormsg, "warning");
                                return View(model);
                            }

                            if (objAud.RemoveSpecialCharactors(ownerName[i]).Length > 49)
                                ownerName[i] = objAud.RemoveSpecialCharactors(ownerName[i]).Substring(0, 49);

                            if (objAud.RemoveSpecialCharactors(ownerFatherName[i]).Length > 49)
                                ownerFatherName[i] = objAud.RemoveSpecialCharactors(ownerFatherName[i]).Substring(0, 49);

                            XmlDataOwner += "<Partner><ownerName>" + objAud.RemoveSpecialCharactors(ownerName[i]) + "</ownerName>"
                                 + "<ownerAge>" + ownerAge[i] + "</ownerAge>"
                                  + "<ownerFatherName>" + objAud.RemoveSpecialCharactors(ownerFatherName[i]) + "</ownerFatherName>"
                                  + "<ownerMobileNo>" + ownerMobileNo[i] + "</ownerMobileNo>"
                                  + "<ownerEmailId>" + ownerEmailId[i] + "</ownerEmailId>"
                                  + "<ownerAddress>" + objAud.FilterForAlphabetNumaric(ownerAddress[i]) + "</ownerAddress>"
                                   + "<ownerStateId>" + ownerStateId[i] + "</ownerStateId>"
                                   + "<ownerDistrictId>" + ownerDistrictId[i] + "</ownerDistrictId>"
                                  + "<ownerPincode>" + ownerPincode[i] + "</ownerPincode>"
                                  + "<ownerPhotograph>" + ownerPhotographPath[i] + "</ownerPhotograph>"
                                   + "<ownerSignature>" + ownerSignaturePath[i] + "</ownerSignature>"
                                    + "</Partner>";
                        }
                    }

                }
            }
            else
            {
                ModelState["ownerName"].Errors.Clear();
                ModelState["ownerAge"].Errors.Clear();
                ModelState["ownerFatherName"].Errors.Clear();
                ModelState["ownerMobileNo"].Errors.Clear();
                ModelState["ownerEmailId"].Errors.Clear();
                ModelState["ownerAddress"].Errors.Clear();
                ModelState["ownerStateId"].Errors.Clear();
                ModelState["ownerDistrictId"].Errors.Clear();
                ModelState["ownerPincode"].Errors.Clear();
                //ModelState["ownerPhotograph"].Errors.Clear();
                ModelState["ownerPhotographPath"].Errors.Clear();
                // ModelState["ownerSignature"].Errors.Clear();
                ModelState["ownerSignaturePath"].Errors.Clear();

                valStatus = objAud.IsValidLink(model.ownerFPhotographPath, "Photograph", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
                valStatus = objAud.IsValidLink(model.ownerFSignaturePath, "Signature", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }

                if (objAud.RemoveSpecialCharactors(model.ownerNameF).Length > 49)
                    model.ownerNameF = objAud.RemoveSpecialCharactors(model.ownerNameF).Substring(0, 49);

                if (objAud.RemoveSpecialCharactors(model.ownerFatherNameF).Length > 49)
                    model.ownerFatherNameF = objAud.RemoveSpecialCharactors(model.ownerFatherNameF).Substring(0, 49);

                XmlDataOwner += "<Partner><ownerName>" + model.ownerNameF + "</ownerName>"
                              + "<ownerAge>" + model.ownerAgeF + "</ownerAge>"
                               + "<ownerFatherName>" + model.ownerFatherNameF + "</ownerFatherName>"
                               + "<ownerMobileNo>" + model.ownerMobileNoF + "</ownerMobileNo>"
                               + "<ownerEmailId>" + model.ownerEmailIdF + "</ownerEmailId>"
                               + "<ownerAddress>" + model.ownerAddressF + "</ownerAddress>"
                                + "<ownerStateId>" + model.ownerStateIdF + "</ownerStateId>"
                                + "<ownerDistrictId>" + model.ownerDistrictIdF + "</ownerDistrictId>"
                               + "<ownerPincode>" + model.ownerPincodeF + "</ownerPincode>"
                               + "<ownerPhotograph>" + model.ownerFPhotographPath + "</ownerPhotograph>"
                                + "<ownerSignature>" + model.ownerFSignaturePath + "</ownerSignature>"
                                 + "</Partner>";
            }
            XmlDataOwner += "</Owner>";

            #endregion

            #region Bulk Insertion Doctor

            var doctorName = form.GetValues("doctorName");
            var doctorFathersName = form.GetValues("doctorFathersName");
            var doctorQualification = form.GetValues("doctorQualification");
            var NameofFoundation = form.GetValues("NameofFoundation");
            var doctorregistrationType = form.GetValues("doctorregistrationType");
            var doctorregistrationNo = form.GetValues("doctorregistrationNo");
            var doctorHprNo = form.GetValues("doctorHprNo");
            var doctorPart_FullTime = form.GetValues("doctorPart_FullTime");
            var docFilePath = form.GetValues("docFilePath");
            var doctorAge = form.GetValues("doctorAge"); //Convert.ToInt32(form.GetValues("doctorAge").ToString());
            var doctoraddress = form.GetValues("doctoraddress");
            var dyear = form.GetValues("dyear");
            var dsignature = form.GetValues("dsignature");


            int countDoc = doctorName.Count();
            string XmlDataDoc = "<DoctorStaff>";
            //if (countDoc > 5)
            //    countDoc = 5;

            for (int i = 0; i < countDoc; i++)
            {
                if (doctorName[i].ToString() == "" && doctorQualification[i] == "" && NameofFoundation[i] == "" && doctorregistrationType[i] == "")
                {
                    //XmlData = string.Empty;
                }
                else
                {

                    valStatus = objAud.IsValidLink(docFilePath[i], "Doctor Qualification Document", out errormsg);
                    if (!valStatus)
                    {
                        setSweetAlertMsg(errormsg, "warning");
                        return View(model);
                    }
                    valStatus = objAud.IsValidLink(dsignature[i], "Doctor Signature", out errormsg);
                    if (!valStatus)
                    {
                        setSweetAlertMsg(errormsg, "warning");
                        return View(model);
                    }

                    if (objAud.RemoveSpecialCharactors(doctorName[i]).Length > 49)
                        doctorName[i] = objAud.RemoveSpecialCharactors(doctorName[i]).Substring(0, 49);

                    if (objAud.RemoveSpecialCharactors(doctorFathersName[i]).Length > 49)
                        doctorFathersName[i] = objAud.RemoveSpecialCharactors(doctorFathersName[i]).Substring(0, 49);

                    if (objAud.RemoveSpecialCharactors(doctorQualification[i]).Length > 49)
                        doctorQualification[i] = objAud.RemoveSpecialCharactors(doctorQualification[i]).Substring(0, 49);

                    if (objAud.RemoveSpecialCharactors(NameofFoundation[i]).Length > 249)
                        NameofFoundation[i] = objAud.RemoveSpecialCharactors(NameofFoundation[i]).Substring(0, 249);

                    if (objAud.RemoveSpecialCharactors(doctorregistrationNo[i]).Length > 19)
                        doctorregistrationNo[i] = objAud.RemoveSpecialCharactors(doctorregistrationNo[i]).Substring(0, 19);



                    XmlDataDoc += "<Doctor><doctorName>" + objAud.RemoveSpecialCharactors(doctorName[i]) + "</doctorName>"
                         + "<doctorFathersName>" + objAud.RemoveSpecialCharactors(doctorFathersName[i]) + "</doctorFathersName>"
                          + "<doctorQualification>" + objAud.RemoveSpecialCharactors(doctorQualification[i]) + "</doctorQualification>"
                          + "<NameofFoundation>" + objAud.RemoveSpecialCharactors(NameofFoundation[i]) + "</NameofFoundation>"
                          + "<doctorregistrationType>" + doctorregistrationType[i] + "</doctorregistrationType>"
                          + "<doctorregistrationNo>" + objAud.FilterForAlphabetNumaric(doctorregistrationNo[i]) + "</doctorregistrationNo>"
                          + "<doctorHprNo>" + doctorHprNo[i] + "</doctorHprNo>"
                          + "<doctorPart_FullTime>" + doctorPart_FullTime[i] + "</doctorPart_FullTime>"
                           + "<docFilePath>" + docFilePath[i] + "</docFilePath>"
                            + "<doctorAge>" + doctorAge[i] + "</doctorAge>"
                          + "<doctoraddress>" + doctoraddress[i] + "</doctoraddress>"
                          //+ "<dyear>" + dyear[i] + "</dyear>"
                          + "<dsignature>" + dsignature[i] + "</dsignature>"
                            + "</Doctor>";
                }

            }
            XmlDataDoc += "</DoctorStaff>";

            #endregion

            #region Bulk Insertion paramedical staff
            var staffName = form.GetValues("staffName");
            var stafffatherName = form.GetValues("stafffatherName");
            var staffqualification = form.GetValues("staffqualification");
            var staffinstitution = form.GetValues("staffinstitution");
            var staffRegistrationType = form.GetValues("staffRegistrationType");
            var staffRegistrationNo = form.GetValues("staffRegistrationNo");
            var staffHprNo = form.GetValues("staffHprNo");
            //var staffNameOfMCI_SMF = form.GetValues("staffNameOfMCI_SMF");
            var part_fullTime = form.GetValues("part_fullTime");
            var filePath = form.GetValues("filePath");
            var staffAge = form.GetValues("staffAge");
            var staffaddress = form.GetValues("staffaddress");
            var syear = form.GetValues("syear");
            var ssign = form.GetValues("ssign");
            int count = staffName.Count();
            int staffRowCount = 0;
            string XmlData = "<ParamedicalStaff>";

            if (count > 5)
                count = 5;
            for (int i = 0; i < count; i++)
            {
                if (staffName[i].ToString() == "" && staffqualification[i] == "" && staffinstitution[i] == "" && staffRegistrationType[i] == "")
                {
                    //XmlData = string.Empty;
                }
                else
                {
                    if (staffRegistrationType[i] == "NA")
                    {
                        valStatus = true;
                    }
                    else
                    {
                        if (model.isRenewal != true)
                        {
                            valStatus = objAud.IsValidLink(filePath[i], " PARAMEDICAL STAFF Qualification Document", out errormsg);
                        }
                       
                    }

                    if (!valStatus)
                    {
                        setSweetAlertMsg(errormsg, "warning");
                        return View(model);
                    }

                    if (model.isRenewal != true) {
                        valStatus = objAud.IsValidLink(ssign[i], "Staff Signature", out errormsg);
                        if (!valStatus)
                        {
                            setSweetAlertMsg(errormsg, "warning");
                            return View(model);
                        }
                    }
                    


                    if (objAud.RemoveSpecialCharactors(staffName[i]).Length > 49)
                        staffName[i] = objAud.RemoveSpecialCharactors(staffName[i]).Substring(0, 49);

                    if (objAud.RemoveSpecialCharactors(stafffatherName[i]).Length > 49)
                        stafffatherName[i] = objAud.RemoveSpecialCharactors(stafffatherName[i]).Substring(0, 49);

                    if (objAud.RemoveSpecialCharactors(staffqualification[i]).Length > 49)
                        staffqualification[i] = objAud.RemoveSpecialCharactors(staffqualification[i]).Substring(0, 49);

                    if (objAud.RemoveSpecialCharactors(staffinstitution[i]).Length > 249)
                        staffinstitution[i] = objAud.RemoveSpecialCharactors(staffinstitution[i]).Substring(0, 249);

                    if (objAud.RemoveSpecialCharactors(staffRegistrationNo[i]).Length > 19)
                        staffRegistrationNo[i] = objAud.RemoveSpecialCharactors(staffRegistrationNo[i]).Substring(0, 19);




                    XmlData += "<Staff><staffName>" + objAud.RemoveSpecialCharactors(staffName[i]) + "</staffName>"
                         + "<stafffatherName>" + objAud.RemoveSpecialCharactors(stafffatherName[i]) + "</stafffatherName>"
                          + "<staffqualification>" + objAud.RemoveSpecialCharactors(staffqualification[i]) + "</staffqualification>"
                          + "<staffinstitution>" + objAud.RemoveSpecialCharactors(staffinstitution[i]) + "</staffinstitution>"
                          + "<staffRegistrationType>" + staffRegistrationType[i] + "</staffRegistrationType>"
                          + "<staffRegistrationNo>" + objAud.FilterForAlphabetNumaric(staffRegistrationNo[i]) + "</staffRegistrationNo>"
                          + "<staffHprNo>" + staffHprNo[i] + "</staffHprNo>"
                          + "<part_fullTime>" + part_fullTime[i] + "</part_fullTime>"
                          + "<filePath>" + filePath[i] + "</filePath>"
                          + "<staffAge>" + staffAge[i] + "</staffAge>"
                          + "<staffaddress>" + staffaddress[i] + "</staffaddress>"
                          + "<syear>" + syear[i] + "</syear>"
                          + "<ssign>" + ssign[i] + "</ssign>"
                          + "</Staff>";

                    staffRowCount++;
                }

            }
            XmlData += "</ParamedicalStaff>";

            #endregion


            #region bulkinsertion OutPatient
            if (model.isOutPatient == true)
            {

                string[] outPatientId = form.GetValues("chkOutPatient");
                if (outPatientId != null)
                {
                    string xmldataOutPatient = "<OutPatients>";
                    for (int i = 0; i < outPatientId.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(outPatientId[i]))
                        {
                            xmldataOutPatient += "<OutPatient><outPatientId>" + outPatientId[i] + "</outPatientId></OutPatient>";
                        }
                    }
                    xmldataOutPatient += "</OutPatients>";
                    model.xmldataOutPatient = xmldataOutPatient;
                }
            }

            #endregion

            #region bulkinsertion Laboratory
            if (model.isLaboratory == true)
            {

                string[] laboratoryId = form.GetValues("chkLaboratory");
                if (laboratoryId != null)
                {
                    string xmldataLaboratory = "<Laboratorys>";
                    for (int i = 0; i < laboratoryId.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(laboratoryId[i]))
                        {
                            xmldataLaboratory += "<Laboratory><laboratoryId>" + laboratoryId[i] + "</laboratoryId></Laboratory>";
                        }
                    }
                    xmldataLaboratory += "</Laboratorys>";
                    model.xmldataLaboratory = xmldataLaboratory;
                }
            }

            #endregion

            #region bulkinsertion Imaging
            if (model.isImaging == true)
            {

                string[] imagingId = form.GetValues("chkImaging");
                if (imagingId != null)
                {
                    string xmldataImaging = "<Imagings>";
                    for (int i = 0; i < imagingId.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(imagingId[i]))
                        {
                            xmldataImaging += "<Imaging><imagingId>" + imagingId[i] + "</imagingId></Imaging>";
                        }
                    }
                    xmldataImaging += "</Imagings>";
                    model.xmldataImaging = xmldataImaging;
                }
            }
            #endregion


            model.regisIdNUH = 0;
            model.regByUser = objSM.UserID;
            model.regBytransIp = Common.GetIPAddress();
            model.transIP = Common.GetIPAddress();
            model.requestKey = objSM.AppRequestKey;
            if (staffRowCount > 0)
            {
                model.xml = XmlData;
            }
            model.XmlDataOwner = XmlDataOwner;
            model.XmlDataDoc = XmlDataDoc;
            ModelState["appMobileNo"].Errors.Clear();

            if (model.isInPatient == false)
            {
                ModelState["numberofBed"].Errors.Clear();
            }

            if (model.isDispose == false)
            {
                ModelState["disposedNo"].Errors.Clear();
                ModelState["disposedFile"].Errors.Clear();
            }

            if (model.medicalEstablishmentId != 9)
            {
                ModelState["medicalEstablishmentOther"].Errors.Clear();
            }

            if (model.establishmentSubCategoriesId != 5 && model.establishmentSubCategoriesId != 11)
            {
                ModelState["otherEstablishmentCategory"].Errors.Clear();
            }

            if (model.isBelongToMedical == true)
            {
                ModelState["piPhotograph"].Errors.Clear();
                model.piPhotographPath = model.ownerFPhotographPath;
            }
            else
            {
                if (model.ownerFPhotographPath != null)
                {
                    valStatus = objAud.IsValidLink(model.ownerFPhotographPath, "Owner photograph", out errormsg);
                    if (!valStatus)
                    {
                        setSweetAlertMsg(errormsg, "warning");
                        return View(model);
                    }
                }
            }

            if (!model.isNOC)
            {
                ModelState["nOCFilePath"].Errors.Clear();
                ModelState["nOCFile"].Errors.Clear();
                ModelState["nocCertificationNo"].Errors.Clear();
            }
            else
            {
                valStatus = objAud.IsValidLink(model.nOCFilePath, "NOC for PCB", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }

            if (!model.isDispose)
            {
                ModelState["disposedFilePath"].Errors.Clear();
                ModelState["disposedFile"].Errors.Clear();
                ModelState["disposedNo"].Errors.Clear();
            }


            if (!model.isFirefightingSystem)
            {
                ModelState["firefightingSystemFilePath"].Errors.Clear();
                ModelState["firefightingSystemFile"].Errors.Clear();
            }
            else
            {
                valStatus = objAud.IsValidLink(model.firefightingSystemFilePath, " Installed Firefighting System in the Establishment File", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }

            valStatus = objAud.IsValidLink(model.addressproofFilePath, "Address Proof", out errormsg);
            if (!valStatus)
            {
                setSweetAlertMsg(errormsg, "warning");
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.disposedFilePath))
            {
                valStatus = objAud.IsValidLink(model.disposedFilePath, "Certificate from agency to Disposal of Medical Waste.", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }

            if (!string.IsNullOrEmpty(model.structuralLyoutFilePath)) //4Jan2021
            {
                valStatus = objAud.IsValidLink(model.structuralLyoutFilePath, "Building Structural Layout", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }
            if (!string.IsNullOrEmpty(model.ElectrycityBillPath)) //Shashi_21_Oct_2022
            {
                valStatus = objAud.IsValidLink(model.ElectrycityBillPath, "Electricity Bill", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }

            if (!string.IsNullOrEmpty(model.RegistryPath)) //Shashi_21_Oct_2022
            {
                valStatus = objAud.IsValidLink(model.RegistryPath, "Registry", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }
            if (!string.IsNullOrEmpty(model.RentalAgreementPath)) //Shashi_21_Oct_2022
            {
                valStatus = objAud.IsValidLink(model.RentalAgreementPath, "Rental Agreement", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }


            valStatus = objAud.IsValidLink(model.piPhotographPath, "In Charge Photograph", out errormsg);
            if (!valStatus)
            {
                setSweetAlertMsg(errormsg, "warning");
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.upmci_smfCertificateFilePath)) //4Jan2021
            {
                valStatus = objAud.IsValidLink(model.upmci_smfCertificateFilePath, "MCI/SMF Certificate", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }

            if (model.establishmentPlace.ToLower() != "other") //4Jan2021
            {
                ModelState["establishmentPlaceOther"].Errors.Clear();
            }
            

            ModelState["Registry"].Errors.Clear();

            ModelState["RentalAgreement"].Errors.Clear();

            ModelState["structuralLyoutFile"].Errors.Clear();
            
            ModelState["ElectrycityBill"].Errors.Clear();
            
            var errors = ModelState.Select(x => x.Value.Errors)
                                                               .Where(y => y.Count > 0)
                                                               .ToList();
            if (ModelState.IsValid)
            {
                var res = objNUHDB.InsertUpdateNursingHome(model);

                try
                {
                    if (res.RegisIdNUH > 0 && res.RegistrationNo != "")
                    {
                        #region Update regisIdNUH in T_OuterpostRequestData
                        SessionManager SM = new SessionManager();
                        NiveshMitraRegistrationModel retval = new NiveshMitraRegistrationModel();
                        if (SM.ControlID != "" && SM.UnitID != "" && SM.ServiceID != "" && SM.RequestID != "")
                        {
                            retval = objNUHDB.UpdateOuterpostRequestData(SM.ControlID, SM.UnitID, SM.ServiceID, SM.RequestID, res.RegisIdNUH).FirstOrDefault();
                        }
                        #endregion

                        //Send Form Submit status with webservice
                        string StatusResult = string.Empty;
                        UPHEALTHNIC.upswp_niveshmitraservices ObjSendAppSubmitStatus = new UPHEALTHNIC.upswp_niveshmitraservices();
                        NiveshMitraSendStatusModel userDetails = new NiveshMitraSendStatusModel();
                        //userDetails = objNUHDB.GetNiveshUserDetailsToSendMedicalRegisStatus(objSM.UserID);
                        userDetails = objNUHDB.GetNiveshUserDetailsToSendMedicalRegisStatus(retval.regisIdNUH);
                        if (userDetails != null)
                        {

                            userDetails.ProcessIndustryID = objSM.Username;
                            userDetails.ApplicationID = retval.regisIdNUH.ToString();  //objSM.UserID.ToString();

                            userDetails.StatusCode = "10";
                            userDetails.Remarks = "Form Submitted Step 1";
                            userDetails.PendencyLevel = "Entrepreneur level pendency";

                            userDetails.FeeAmount = "";
                            userDetails.FeeStatus = "";
                            userDetails.TransectionID = "";
                            userDetails.TranSactionDate = "";
                            userDetails.TransectionDateAndTime = "";
                            userDetails.NocCertificateNumber = "";
                            userDetails.NocUrl = "";
                            userDetails.IsNocUrlActiveYesNo = "";
                            userDetails.Passalt = ConfigurationManager.AppSettings["PassKey"].ToString();
                            userDetails.ObjectRejectionCode = "";
                            userDetails.IsCertificateValidLifeTime = "";
                            userDetails.CertificateExpireDateDDMMYYYY = "";
                            userDetails.D1 = "";
                            userDetails.D2 = "";
                            userDetails.D3 = "";
                            userDetails.D4 = "";
                            userDetails.D5 = "";
                            userDetails.D6 = "";
                            userDetails.D7 = "";

                            StatusResult = ObjSendAppSubmitStatus.WReturn_CUSID_STATUS(userDetails.Control_ID, userDetails.Unit_Id, userDetails.ServiceID, userDetails.ProcessIndustryID, userDetails.ApplicationID, userDetails.StatusCode,
                                userDetails.Remarks, userDetails.PendencyLevel, userDetails.FeeAmount, userDetails.FeeStatus, userDetails.TransectionID, userDetails.TranSactionDate, userDetails.TransectionDateAndTime, userDetails.NocCertificateNumber, userDetails.NocUrl, userDetails.IsNocUrlActiveYesNo, userDetails.Passalt, userDetails.RequestId, userDetails.ObjectRejectionCode
                                 , userDetails.IsCertificateValidLifeTime, userDetails.CertificateExpireDateDDMMYYYY, userDetails.D1, userDetails.D2, userDetails.D3, userDetails.D4, userDetails.D5, userDetails.D6, userDetails.D7);

                            if (StatusResult.ToUpper() == "SUCCESS")
                            {
                                userDetails.SendDate = System.DateTime.Now;
                                userDetails.ResStatus = "";
                                userDetails.ServiceStatus = StatusResult;
                                userDetails.StepId = 2;

                            }
                            else
                            {

                                userDetails.SendDate = System.DateTime.Now;
                                userDetails.ResStatus = "";
                                userDetails.ServiceStatus = StatusResult;
                                userDetails.StepId = 2;

                            }

                            try
                            {

                                userDetails = objAccDb.SendApplicationSubmittedStatus(userDetails).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }

                            //if (userDetails != null)
                            //{
                            //    if (userDetails.QueryExFlag == "1")
                            //    {

                            //    }
                            //}
                        }

                        if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                        {
                            EDistrictServiceClass ed = new EDistrictServiceClass();

                            int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.MEE);

                            bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "MEE");

                            if (result)
                            {
                                SendSMS(res.RegistrationNo, res.MobileNo);
                                TempData["RegisIdNUH"] = res.RegisIdNUH;
                                TempData["RegistrationNo"] = res.RegistrationNo;

                                string _regisIdNUH = OTPL_Imp.CustomCryptography.Encrypt(res.RegisIdNUH.ToString());

                                return RedirectToAction("Affidavit", new { regisIdNUH = _regisIdNUH, operatedId = model.operatedId, isRenewal = false });
                            }
                            else
                            {
                                int exeRsult = objNUHDB.DeleteRegistrationNUH(res.RegisIdNUH);
                                setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                ModelState.Clear();
                            }
                        }
                        else
                        {
                            SendSMS(res.RegistrationNo, res.MobileNo);
                            TempData["RegisIdNUH"] = res.RegisIdNUH;
                            TempData["RegistrationNo"] = res.RegistrationNo;
                            //TempData["RegNUHID"] = res.RegisIdNUH;
                            string _regisIdNUH = OTPL_Imp.CustomCryptography.Encrypt(res.RegisIdNUH.ToString());
                            return RedirectToAction("Affidavit", new { regisIdNUH = _regisIdNUH, operatedId = model.operatedId, isRenewal = false });
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
                setSweetAlertMsg("Invalid Data Entered", "error");
            }

            return View();
        }

        public ActionResult RegistrationConfirmation(bool isRenewal = false)
        {

            ViewBag.NUHRegisId = OTPL_Imp.CustomCryptography.Encrypt(TempData["RegisIdNUH"].ToString());


            //ViewBag.NUHRegistrationNo = TempData["RegistrationNo"].ToString();
            ViewBag.isRenewal = isRenewal;
            //if (isRenewal)
            //{
            //    ViewBag.AffidavitProforma = "~/Content/NUHAffidavitFormat/Affidaite Renewable ME.pdf";
            //}
            //else
            //{
            //    ViewBag.AffidavitProforma = "~/Content/NUHAffidavitFormat/Affidavite New ME.pdf";
            //}

            return View();
        }

        public ActionResult UploadAffidavit(string regisId, bool isRenewal = false)
        {
            NUHmodel model = new NUHmodel();
            long regisByuser = objSM.UserID;
            model.regisIdNUH = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId));
            Session["regisAffidavitNUHId"] = regisId;

            if (isRenewal)
            {
                ViewBag.AffidavitProforma = "~/Content/NUHAffidavitFormat/Affidaite Renewable ME.pdf";
            }
            else
            {
                ViewBag.AffidavitProforma = "~/Content/NUHAffidavitFormat/Affidavite New ME.pdf";
            }

            return View(model);
        }

        public JsonResult DropdownDistNUH()
        {
            var res = comndb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadAffidavit(NUHmodel model)
        {
            AuditMethods objAud = new AuditMethods();
            string StatusResult = string.Empty;
            if (model.regisIdNUH > 0 && !string.IsNullOrEmpty(model.notarizedAffidavitFilePath))
            {
                string errormsg = String.Empty;
                bool valStatus = objAud.IsValidLink(model.notarizedAffidavitFilePath, "Affidavit ", out errormsg);
                if (!valStatus)
                {
                    TempData["Message"] = errormsg;
                    return RedirectToAction("NursingDashBoard");
                }
                int resultData = objNUHDB.UploadAffidavitNUH(model.regisIdNUH, model.notarizedAffidavitFilePath);
                if (resultData > 0)
                {
                    UPHEALTHNIC.upswp_niveshmitraservices ObjSendAppSubmitStatus = new UPHEALTHNIC.upswp_niveshmitraservices();
                    NiveshMitraSendStatusModel userDetails = new NiveshMitraSendStatusModel();
                    //userDetails = objNUHDB.GetNiveshUserDetailsToSendMedicalRegisStatus(objSM.UserID);
                    userDetails = objNUHDB.GetNiveshUserDetailsToSendMedicalRegisStatus(model.regisIdNUH);
                    if (userDetails != null)
                    {


                        userDetails.ProcessIndustryID = objSM.Username;
                        userDetails.ApplicationID = model.regisIdNUH.ToString(); //objSM.UserID.ToString();

                        userDetails.StatusCode = "13";
                        userDetails.Remarks = "Application Submitted by Applicant";
                        //  userDetails.PendencyLevel = "Pending at Department";
                        userDetails.PendencyLevel = "Pending at CMO" + "," + " " + userDetails.CMODistrictName;

                        userDetails.FeeAmount = "";
                        userDetails.FeeStatus = "";
                        userDetails.TransectionID = "";
                        userDetails.TranSactionDate = "";
                        userDetails.TransectionDateAndTime = "";
                        userDetails.NocCertificateNumber = "";
                        userDetails.NocUrl = "";
                        userDetails.IsNocUrlActiveYesNo = "";
                        userDetails.Passalt = ConfigurationManager.AppSettings["PassKey"].ToString();
                        userDetails.ObjectRejectionCode = "";
                        userDetails.IsCertificateValidLifeTime = "";
                        userDetails.CertificateExpireDateDDMMYYYY = "";
                        userDetails.D1 = "";
                        userDetails.D2 = "";
                        userDetails.D3 = "";
                        userDetails.D4 = "";
                        userDetails.D5 = "";
                        userDetails.D6 = "";
                        userDetails.D7 = "";

                        StatusResult = ObjSendAppSubmitStatus.WReturn_CUSID_STATUS(userDetails.Control_ID, userDetails.Unit_Id, userDetails.ServiceID, userDetails.ProcessIndustryID, userDetails.ApplicationID, userDetails.StatusCode,
                               userDetails.Remarks, userDetails.PendencyLevel, userDetails.FeeAmount, userDetails.FeeStatus, userDetails.TransectionID, userDetails.TranSactionDate, userDetails.TransectionDateAndTime, userDetails.NocCertificateNumber, userDetails.NocUrl, userDetails.IsNocUrlActiveYesNo, userDetails.Passalt, userDetails.RequestId, userDetails.ObjectRejectionCode
                                , userDetails.IsCertificateValidLifeTime, userDetails.CertificateExpireDateDDMMYYYY, userDetails.D1, userDetails.D2, userDetails.D3, userDetails.D4, userDetails.D5, userDetails.D6, userDetails.D7);

                        if (StatusResult.ToUpper() == "SUCCESS")
                        {

                            userDetails.SendDate = System.DateTime.Now;
                            userDetails.ResStatus = "";
                            userDetails.ServiceStatus = StatusResult;
                            userDetails.StepId = 3;

                            try
                            {

                                userDetails = objAccDb.SendApplicationSubmittedStatus(userDetails).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }

                            TempData["AffidavitUploadMessage"] = "Affidavit Uploaded Successfully.";
                            //TempData["FormNotSubmitted"] = "Submitted";
                            return RedirectToAction("NursingDashBoard", "NUH");

                        }
                        else
                        {

                            userDetails.SendDate = System.DateTime.Now;
                            userDetails.ResStatus = "";
                            userDetails.ServiceStatus = StatusResult;
                            userDetails.StepId = 3;
                            try
                            {

                                userDetails = objAccDb.SendApplicationSubmittedStatus(userDetails).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }

                        }

                        //if (userDetails != null)
                        //{

                        //    if (userDetails.QueryExFlag == "1")
                        //    {

                        //    }
                        //}
                    }
                    else
                    {
                        TempData["Message"] = "Affidavit Uploaded Successfully.";
                    }

                    return RedirectToAction("NursingDashBoard");

                }
                else
                {
                    setSweetAlertMsg("Error in submitting Affidavit !", "error");
                    return View(model);
                }
            }
            else
            {
                setSweetAlertMsg("Please upload affidavit !", "warning");
                return View(model);
            }
        }

        public JsonResult UploadFile(HttpPostedFileBase File)
        {
            string Dirpath = "~/Content/writereaddata/NUH/";
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

                if (size > 5242880)
                {
                    path = "SNV";//"warning_Maximum 2MB file size are allowed";
                }
                else
                {
                    File.SaveAs(completepath);
                    path = Dirpath + filename;
                }

                //before
                //if (size > 2097152)
                //{
                //    path = "SNV";//"warning_Maximum 2MB file size are allowed";
                //}
                //else
                //{
                //    File.SaveAs(completepath);
                //    path = Dirpath + filename;
                //}
            }
            else
            {
                path = "TNV";//"warning_Invalid File Format only pdf and jpg files are allow!";
            }

            List<string> plist = new List<string> { filename, path };
            return Json(plist);
        }

        #region

        public JsonResult UploadFileJPG(HttpPostedFileBase File)
        {
            string Dirpath = "~/Content/writereaddata/NUH/";
            string path = "";
            string filename = "";
            if (!Directory.Exists(Server.MapPath(Dirpath)))
            {
                Directory.CreateDirectory(Server.MapPath(Dirpath));
            }
            string ext = Path.GetExtension(File.FileName);
            if (ext.ToLower() == ".jpg")
            {

                filename = Path.GetFileNameWithoutExtension(File.FileName) + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;
                string completepath = Path.Combine(Server.MapPath(Dirpath), filename);
                if (System.IO.File.Exists(completepath))
                {
                    System.IO.File.Delete(completepath);
                }

                long size = File.ContentLength;

                if (size > 5242880)
                {
                    path = "SNV";//"warning_Maximum 2MB file size are allowed";
                }
                else
                {
                    File.SaveAs(completepath);
                    path = Dirpath + filename;
                }

                //before
                //if (size > 2097152)
                //{
                //    path = "SNV";//"warning_Maximum 2MB file size are allowed";
                //}
                //else
                //{
                //    File.SaveAs(completepath);
                //    path = Dirpath + filename;
                //}
            }
            else
            {
                path = "TNV";//"warning_Invalid File Format only jpg files are allow!";
            }

            List<string> plist = new List<string> { filename, path };
            return Json(plist);
        }

        #endregion
        #region Aniket

        public JsonResult UploadFileForParaMed(HttpPostedFileBase File)
        {
            string Dirpath = "";
            Dirpath = "~/Content/writereaddata/NUH/";
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

                if (size > 5242880)
                {
                    path = "SNV";//"warning_Maximum 2MB file size are allowed";
                }
                else
                {
                    File.SaveAs(completepath);
                    path = Dirpath + filename;
                }

                //before
                //if (size > 2097152)
                //{
                //    path = "SNV";//"warning_Maximum 2MB file size are allowed";
                //}
                //else
                //{
                //    File.SaveAs(completepath);
                //    path = Dirpath + filename;
                //}
            }
            else
            {
                path = "TNV";//"warning_Invalid File Format only pdf and jpg files are allow!";
            }

            List<string> plist = new List<string> { filename, path };
            return Json(plist);
        }

        public JsonResult UploadFileJPGForParaMed(HttpPostedFileBase File)
        {
            string Dirpath = "";
            Dirpath = "~/Content/writereaddata/NUH/";
            string path = "";
            string filename = "";
            if (!Directory.Exists(Server.MapPath(Dirpath)))
            {
                Directory.CreateDirectory(Server.MapPath(Dirpath));
            }
            string ext = Path.GetExtension(File.FileName);
            if (ext.ToLower() == ".jpg")
            {

                filename = Path.GetFileNameWithoutExtension(File.FileName) + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;
                string completepath = Path.Combine(Server.MapPath(Dirpath), filename);
                if (System.IO.File.Exists(completepath))
                {
                    System.IO.File.Delete(completepath);
                }

                long size = File.ContentLength;

                if (size > 5242880)
                {
                    path = "SNV";//"warning_Maximum 2MB file size are allowed";
                }
                else
                {
                    File.SaveAs(completepath);
                    path = Dirpath + filename;
                }

                //before
                //if (size > 2097152)
                //{
                //    path = "SNV";//"warning_Maximum 2MB file size are allowed";
                //}
                //else
                //{
                //    File.SaveAs(completepath);
                //    path = Dirpath + filename;
                //}
            }
            else
            {
                path = "TNV";//"warning_Invalid File Format only jpg files are allow!";
            }

            List<string> plist = new List<string> { filename, path };
            return Json(plist);
        }

        #endregion
        

        //public JsonResult UploadFile(HttpPostedFileBase File)
        //{
        //    string ImgValidation = comModel.ValidateImageExtWithSizeForDocuments(File);


        //    string Dirpath = "~/Content/writereaddata/NUH/";
        //    string path = "";
        //    string filename = "";
        //    string flag = "";
        //    if (!Directory.Exists(Server.MapPath(Dirpath)))
        //    {
        //        Directory.CreateDirectory(Server.MapPath(Dirpath));
        //    }
        //    string ext = Path.GetExtension(File.FileName);
        //    if (ImgValidation == "Valid")
        //    {

        //        filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + "_" + File.FileName;
        //        string completepath = Path.Combine(Server.MapPath(Dirpath), filename);
        //        if (System.IO.File.Exists(completepath))
        //        {
        //            System.IO.File.Delete(completepath);
        //        }

        //        long size = File.ContentLength;
        //        if (size > 2097152)
        //        {
        //            path = "warning_Maximum 2MB file size are allowed";
        //        }
        //        else
        //        {
        //            File.SaveAs(completepath);
        //            path = Dirpath + filename;
        //            flag = "0";
        //        }

        //    }
        //    else
        //    {
        //        path = ImgValidation;
        //        flag = ImgValidation;
        //    }

        //    //if (ext.ToLower() == ".jpg" || ext.ToLower() == ".pdf")
        //    //{

        //    //    filename = File.FileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;
        //    //    string completepath = Path.Combine(Server.MapPath(Dirpath), filename);
        //    //    if (System.IO.File.Exists(completepath))
        //    //    {
        //    //        System.IO.File.Delete(completepath);
        //    //    }

        //    //    long size = File.ContentLength;
        //    //    if (size > 2097152)
        //    //    {
        //    //        path = "SNV";//"warning_Maximum 2MB file size are allowed";
        //    //    }
        //    //    else
        //    //    {
        //    //        File.SaveAs(completepath);
        //    //        path = Dirpath + filename;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    path = "TNV";//"warning_Invalid File Format only pdf and jpg files are allow!";
        //    //}

        //    //List<string> plist = new List<string> { filename, path };
        //    List<string> plist = new List<string> { File.FileName, path, flag };
        //    return Json(plist);
        //}

        public JsonResult getEsubcatList(int establishmentCategoriesId)
        {
            var returndataset = comndb.GetDropDownList(10, establishmentCategoriesId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(returndataset, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult getCsubcatList(int clinicalEstablishmentTypeId)
        //{
        //    var returndataset = comndb.GetDropDownList(14, clinicalEstablishmentTypeId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
        //    return Json(returndataset, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult ViewNUHList()
        {
            long regisByuser = objSM.UserID;
            NUHmodel model = new NUHmodel();
            SessionManager SM = new SessionManager();
            int procId = 2;

            if (SM.ControlID != "" && SM.UnitID != "" && SM.ServiceID != "" && SM.RequestID != "")
            {
                model.NUHModelList = objNUHDB.GetApplicationByRequestId(procId, SM.ControlID, SM.UnitID, SM.ServiceID, SM.RequestID);
            }
            else
            {
                model.NUHModelList = objNUHDB.GetNUHList(regisByuser);
            }

            return View(model.NUHModelList);

        }

        public ActionResult NUHDetails(string regisId)
        {
            long regisByuser = objSM.UserID;
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            Session["regisIdNUH"] = regisId;
            NUHmodel model = new NUHmodel();

            model = objNUHDB.GetNUHListBYRegistrationNo(Convert.ToInt64(Session["regisIdNUH"].ToString()));

            if (model != null && objSM.UserID != model.regByUser)
            {
                return RedirectToAction("UserUnauthoriseAcess", "Home");
            }

            ViewBag.VBoutpateint = objNUHDB.GetoutPatient(Convert.ToInt64(regisId));
            ViewBag.VBolaboratory = objNUHDB.GetNUHlaboratory(Convert.ToInt64(regisId));
            ViewBag.VBimaging = objNUHDB.GetNUHimaging(Convert.ToInt64(regisId));

            return View(model);
        }

        public ActionResult BindChildPartner()
        {
            NUHPartnerModel model = new NUHPartnerModel();
            model.NUHPartnerList = objNUHDB.getNUHPartner(Convert.ToInt64(Session["regisIdNUH"].ToString()));
            return PartialView("_ViewNUH_Partner", model.NUHPartnerList);
        }

        public ActionResult BindChildList()
        {
            NUHmodel model = new NUHmodel();
            model.NUHModelList = objNUHDB.getNUHChild(Convert.ToInt64(Session["regisIdNUH"].ToString()));
            return PartialView("_ViewNUHChild", model.NUHModelList);
        }

        public ActionResult BindDOCList()
        {
            NUHdoctorModel model = new NUHdoctorModel();
            model.NUHDOCList = objNUHDB.getNUHdoc(Convert.ToInt64(Session["regisIdNUH"].ToString()));
            return PartialView("_ViewNUH_DOC", model.NUHDOCList);
        }

        public ActionResult PrintApplicationForm(string regisIdNUH)
        {
            NUHmodel model = new NUHmodel();

            regisIdNUH = OTPL_Imp.CustomCryptography.Decrypt(regisIdNUH);

            model = objNUHDB.GetNUHListBYRegistrationNo(Convert.ToInt64(regisIdNUH));

            if (model == null || objSM.UserID != model.regByUser)
            {
                return RedirectToAction("UserUnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            Session["regisIdNUH"] = model.regisIdNUH;

            ViewBag.VBoutpateint = objNUHDB.GetoutPatient(model.regisIdNUH);
            ViewBag.VBolaboratory = objNUHDB.GetNUHlaboratory(model.regisIdNUH);
            ViewBag.VBimaging = objNUHDB.GetNUHimaging(model.regisIdNUH);

            model.NUHPartnerList = objNUHDB.getNUHPartner(Convert.ToInt64(regisIdNUH));
            model.NUHDOCList = objNUHDB.getNUHdoc(Convert.ToInt64(regisIdNUH));
            model.NUHModelList = objNUHDB.getNUHChild(Convert.ToInt64(regisIdNUH));

            return View(model);
        }

        #region Report

        public ActionResult NUHAffidavateReport(string regisId)
        {
            string stausMessage = "";
            string setPdfName = "";
            //var res = objNUHDB.GetDetail(2);
            var res = objNUHDB.GetDetail(Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId)));
            var res2 = objNUHDB.getNUHChild(res[0].regisIdNUH);
            try
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rpt_NUHAffidavate.rpt"));
                rd.SetDataSource(res);
                ReportDocument subShows = rd.Subreports["rpt_NUHchild.rpt"];
                subShows.SetDataSource(res2);
                String dtnow = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                setPdfName = "MedicalEstablishment" + "_" + dtnow;
                string folderpath = "~/Content/writereaddata/PDF/";
                if (!System.IO.Directory.Exists(Server.MapPath(folderpath)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(folderpath));
                }
                string flName = folderpath + setPdfName + ".pdf";
                rd.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                rd.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                rd.ExportOptions.DestinationOptions = new DiskFileDestinationOptions();
                ((DiskFileDestinationOptions)rd.ExportOptions.DestinationOptions).DiskFileName = Server.MapPath(flName);
                rd.Export();
                FileInfo fileInfo = new FileInfo(Server.MapPath(flName));
                rd.Close();
                rd.Dispose();
                {
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
            }
            catch (Exception ex)
            {
                stausMessage = "error_Error Occour to Downloading, Please try again.";
            }
            return RedirectToAction("NursingDashBoard");
        }

        #endregion
        void SendSMS(string registrationNo, string mobileNo)
        {

            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";

                //txtmsg = "Dear Citizen,\n\nYour Application form has been submitted successfully. Your Application Form Number is " + registrationNo + ", kindly use this further.\n\n Thanks";
                txtmsg = "Dear Citizen,\n\nYour Application form for Registration of Medical Establishment has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team \nMHFWD, UP";


                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }

        }
        #endregion

        #region Certificate Rpt Riya
        public ActionResult NUHgeneratedCertificate(string regisIdNUH, string certGenrBy)
        {

            string stausMessage = "";
            string setPdfName = "", setDigitalPdfName = "";
            var res = objNUHDB.GetDetails(Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdNUH)));
            var res2 = objNUHDB.getNUHChilds(res[0].regisIdNUH);
            try
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rpt_NUHcertificate.rpt"));
                rd.SetDataSource(res);
                ReportDocument subShows = rd.Subreports["rpt_NUHcertificateChild.rpt"];
                subShows.SetDataSource(res2);
                String dtnow = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                setPdfName = "MedicalEstablishment" + "_" + dtnow;
                setDigitalPdfName = "MedicalEstablishmentCertificateDigitalSigned" + "_" + dtnow;
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

                    var sigDetails = comndb.GetDigitalSignatureDetails(Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(certGenrBy)));

                    float llx = 580;
                    float lly = 290;
                    float urx = 440;
                    float ury = 190;
                    Comman_Classes.DigitalCeritificateManager dcm = new Comman_Classes.DigitalCeritificateManager();
                    Comman_Classes.MetaData md = new Comman_Classes.MetaData()
                    {
                        Author = sigDetails.Author,
                        Title = "Medical Establishment Certificate Authentication",
                        Subject = "Medical Establishment Certificate",
                        Creator = sigDetails.Creator,
                        Producer = sigDetails.Producer,
                        Keywords = sigDetails.Keywords
                    };

                    string Signaturepath = Server.MapPath(sigDetails.Signaturepath);
                    dcm.signPDF(Server.MapPath(flName), Server.MapPath(digitalFlName), Signaturepath,
                     sigDetails.signpwd, "Authenticate Medical Establishment Certificate", sigDetails.SigContact,
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
            return RedirectToAction("ViewNUHList");
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

        public ActionResult SearchDetails()
        {
            var resultData = objNUHDB.GetRegisterDetails(true);

            if ((resultData!=null) && (resultData.regisIdNUH > 0) && (resultData.DeclarationDate == null || resultData.DeclarationDate == ""))
            {
                string _regisId = OTPL_Imp.CustomCryptography.Encrypt(resultData.regisIdNUH.ToString());
                return RedirectToAction("Affidavit", new { regisIdNUH = _regisId, isRenewal = resultData.isRenewal, operatedId = resultData.operatedId });

            }

            else if ((resultData != null) && (resultData.regisIdNUH > 0) && (resultData.ParamedicalDeclarationDate == null || resultData.ParamedicalDeclarationDate == ""))
            {

                string _regisId = OTPL_Imp.CustomCryptography.Encrypt(resultData.regisIdNUH.ToString());
                return RedirectToAction("Pairamedical", "NUH", new { regisIdNUH = _regisId, isRenewal = resultData.isRenewal });

            }
            else
            {
                return View();
            }

            //return View();
        }

        [HttpPost]
        public ActionResult SearchDetails(SearchDetailModel model)
        {
            string _regisIdN = "";
            ModelState["isCertFrmPortal"].Errors.Clear();
            if (ModelState.IsValid)
            {
                var resultData = objNUHDB.SearchDetails(model, objSM.UserID);
                if (resultData != null && resultData.Flag == 1)
                {
                    _regisIdN = OTPL_Imp.CustomCryptography.Encrypt(resultData.RegisId.ToString());
                }
                else if (resultData.Flag == 2)
                {
                    setSweetAlertMsg("Please enter the latest Certificate No. issued by the Department. Entered Certificate No. is either old or incorrect.", "info");
                }
                else
                {
                    setSweetAlertMsg("No record available for given details !", "warning");
                }
            }
            else
            {
                setSweetAlertMsg("Enter Valid Details!", "warning");
            }

            if (string.IsNullOrEmpty(_regisIdN))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Renewal", "NUH", new { regisIdN = _regisIdN });
            }
        }

        #region NUH Renewal Riya
        public ActionResult Renewal(string regisIdN = "")
        {


            NUHmodel model = new NUHmodel();

            long regisIdNUH = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdN));

            model = objNUHDB.GetDetailsNUHByRegisId(regisIdNUH);
            //Changes
            if (model.structuralLyoutFile != null)
            {
                // model.structuralLyoutFilePath = model.structuralLyoutFile;
                objSM.StructuralLayOut = model.structuralLyoutFile;

            }
            if (!string.IsNullOrEmpty(model.ElectrycityBill))
            {
                objSM.ElectricityBills = model.ElectrycityBill;
            }
            if (!string.IsNullOrEmpty(model.Registry))
            {
                objSM.Registries = model.Registry;
            }
            if (!string.IsNullOrEmpty(model.RentalAgreement))
            {
                objSM.RentalAgreements = model.RentalAgreement;
            }

            if (model.piPhotograph != null)
            {
                // model.piPhotographPath = model.piPhotograph;
                objSM.PicPhotoGraph = model.piPhotograph;
            }

            if (model.addressproofFilePath != null)
            {
                //  model.addressproofFilePath = model.addressproofFilePath;
                objSM.AddressProofFile = model.addressproofFilePath;
            }

            if (model.ownerFPhotograph != null)
            {
                //model.ownerFPhotographPath = model.ownerFPhotograph;
                objSM.OwnerFPhotoGrapf = model.ownerFPhotograph;
            }

            if (model.ownerFSignature != null)
            {
                //model.ownerFSignaturePath = model.ownerFSignature;
                objSM.OwnerFSignature = model.ownerFSignature;
            }

            if (model.upmci_smfCertificateFile != null)
            {
                //model.upmci_smfCertificateFilePath = model.upmci_smfCertificateFile;
                objSM.UpMCiCerficate = model.upmci_smfCertificateFile;
            }

            objSM.MeeRegisNo = model.meeRegisNo;

            ViewBag.MedicalEstablishment = comndb.GetDropDownList(5, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.MedicalEstablishmentCategory = comndb.GetDropDownList(9, 0).ToList();
            ViewBag.MedicalEstablishmentCategoryType = Enumerable.Empty<SelectListItem>();
            ViewBag.ClinicalEstablishment = comndb.GetDropDownList(13, 0).ToList();

            ViewBag.ClinicalSubEstablishment = Enumerable.Empty<SelectListItem>();
            ViewBag.State = comndb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = comndb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            ViewBag.StateAll = comndb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.DistrictOwner = comndb.GetDropDownList(7, Convert.ToInt32(model.ownerStateIdF)).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.DistrictPersonIncharge = comndb.GetDropDownList(7, model.applicantStateId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            ViewBag.Operate = comndb.GetDropDownList(37, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            ViewBag.OutPatient = objNUHDB.NUH_FacilityOfferedForRenewal(1, regisIdNUH);
            ViewBag.Laboratory = objNUHDB.NUH_FacilityOfferedForRenewal(2, regisIdNUH);
            ViewBag.Imaging = objNUHDB.NUH_FacilityOfferedForRenewal(3, regisIdNUH);

            ViewBag.PartnerList = objNUHDB.getNUHPartner(regisIdNUH);
            ViewBag.doctorList = objNUHDB.getNUHdoctorList(regisIdNUH);
            ViewBag.staffList = objNUHDB.getNUHParamedicalList(regisIdNUH);

            return View(model);
        }

        [HttpPost]
        public ActionResult Renewal(NUHmodel model, FormCollection form)
        {
            AuditMethods objAud = new AuditMethods();
            model.isCertificateFromPortal = true;
            if (model.numberofBed > 49)
            {
                setSweetAlertMsg("Number of Beds(Inpatient) Can Not Be More Than 49", "info");
                return View(model);
            }
            if (model.isMEEaddressChange == false)
            {
                ModelState["addressproofFile"].Errors.Clear();
            }
            if (model.isNOC == false)
            {
                ModelState["nocCertificationNo"].Errors.Clear();
                model.nocCertificationNo = null;
                ModelState["nOCFilePath"].Errors.Clear();
                model.nOCFilePath = null;
            }
            if (model.isDispose == false)
            {
                ModelState["disposedNo"].Errors.Clear();
                model.disposedNo = null;
                ModelState["disposedFilePath"].Errors.Clear();
                model.disposedFilePath = null;
            }
            if (model.isFirefightingSystem == false)
            {
                ModelState["firefightingSystemFile"].Errors.Clear();
                model.firefightingSystemFile = null;
                ModelState["firefightingSystemFilePath"].Errors.Clear();
                model.firefightingSystemFilePath = null;
            }

            // Set the filePath

           
            if (objSM.PicPhotoGraph != null)
            {
                if (!String.IsNullOrEmpty(model.piPhotographPath))
                { }
                else
                {
                    model.piPhotographPath = objSM.PicPhotoGraph;
                }
            }
            if (objSM.UpMCiCerficate != null)
            {
                if (!String.IsNullOrEmpty(model.upmci_smfCertificateFilePath))
                { }
                else
                {
                    model.upmci_smfCertificateFilePath = objSM.UpMCiCerficate;
                }
            }
            if (objSM.OwnerFPhotoGrapf != null)
            {
                model.ownerFPhotographPath = objSM.OwnerFPhotoGrapf;
            }
            if (objSM.OwnerFSignature != null)
            {
                model.ownerFSignaturePath = objSM.OwnerFSignature;
            }
            if (model.addressproofFile == null || model.addressproofFile == "")
            {
                if (objSM.AddressProofFile != null)
                {
                    model.addressproofFilePath = objSM.AddressProofFile;
                }
                else
                {
                    model.addressproofFilePath = model.addressproofFilePath;
                }
            }
            else
            {
                model.addressproofFilePath = model.addressproofFilePath;
            }
            
                ModelState["ElectrycityBill"].Errors.Clear();
          
           
            ModelState["QueryRaisedByCMO"].Errors.Clear();
            ModelState["queryFile"].Errors.Clear();

            ModelState["outerRegistrationNo"].Errors.Clear();
            ModelState["outerCertificateNo"].Errors.Clear();
            ModelState["outerCertificateFile"].Errors.Clear();

            ModelState["nocCertificationNo"].Errors.Clear();

            ModelState["applicantMobileNo"].Errors.Clear();
            ModelState["appMobileNo"].Errors.Clear();
            //ModelState["upmci_smfCertificateFile"].Errors.Clear();
            ModelState["nOCFile"].Errors.Clear();
            ModelState["disposedFile"].Errors.Clear();
            ModelState["firefightingSystemFile"].Errors.Clear();
            ModelState["otherEstablishmentCategory"].Errors.Clear();
            ModelState["establishmentArea"].Errors.Clear();
            ModelState["establishmentPlace"].Errors.Clear();
            ModelState["landType"].Errors.Clear();
            ModelState["piPhotograph"].Errors.Clear();
            ModelState["ownerFPhotograph"].Errors.Clear();
            ModelState["ownerFSignature"].Errors.Clear();

            ///////////////////////////////
            ModelState["medicalEstablishmentOther"].Errors.Clear();

            if (model.operatedId == 2)
            {
                ModelState["ownerNameF"].Errors.Clear();
                ModelState["ownerAgeF"].Errors.Clear();
                ModelState["ownerFatherNameF"].Errors.Clear();
                ModelState["ownerMobileNoF"].Errors.Clear();
                ModelState["ownerEmailIdF"].Errors.Clear();
                ModelState["ownerAddressF"].Errors.Clear();
                ModelState["ownerStateIdF"].Errors.Clear();
                ModelState["ownerDistrictIdF"].Errors.Clear();
                ModelState["ownerPincodeF"].Errors.Clear();
                ModelState["ownerFPhotograph"].Errors.Clear();
                ModelState["ownerFSignature"].Errors.Clear();

                ModelState["ownerStateId"].Errors.Clear();
                ModelState["ownerDistrictId"].Errors.Clear();
            }

            ModelState["disposedNo"].Errors.Clear();

            ModelState["establishmentPlaceOther"].Errors.Clear(); //5Jan2021

            ViewBag.MedicalEstablishment = comndb.GetDropDownList(5, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.MedicalEstablishmentCategory = comndb.GetDropDownList(9, 0).ToList();
            ViewBag.MedicalEstablishmentCategoryType = Enumerable.Empty<SelectListItem>();
            ViewBag.ClinicalEstablishment = comndb.GetDropDownList(13, 0).ToList();

            ViewBag.ClinicalSubEstablishment = Enumerable.Empty<SelectListItem>();
            ViewBag.State = comndb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = comndb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Operate = comndb.GetDropDownList(37, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            ViewBag.OutPatient = objNUHDB.NUH_FacilityOfferedForRenewal(1, model.regisIdNUH);
            ViewBag.Laboratory = objNUHDB.NUH_FacilityOfferedForRenewal(2, model.regisIdNUH);
            ViewBag.Imaging = objNUHDB.NUH_FacilityOfferedForRenewal(3, model.regisIdNUH);

            ViewBag.PartnerList = objNUHDB.getNUHPartner(model.regisIdNUH);
            ViewBag.doctorList = objNUHDB.getNUHdoctorList(model.regisIdNUH);
            ViewBag.staffList = objNUHDB.getNUHParamedicalList(model.regisIdNUH);

            model.regByUser = objSM.UserID;
            model.regBytransIp = Common.GetIPAddress();
            model.transIP = Common.GetIPAddress();
            model.requestKey = objSM.AppRequestKey;

            //AuditMethods obj = new AuditMethods();
            string errormsg = "";
            bool valStatus = false;
            if (!string.IsNullOrEmpty(model.structuralLyoutFilePath)) //4Jan2021
            {
                valStatus = objAud.IsValidLink(model.structuralLyoutFilePath, "Building Structural Layout", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }
            if (!string.IsNullOrEmpty(model.ElectrycityBillPath)) //Shashi_21_Oct_2022
            {
                valStatus = objAud.IsValidLink(model.ElectrycityBillPath, "Electricity Bill", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }

            if (!string.IsNullOrEmpty(model.RegistryPath)) //Shashi_21_Oct_2022
            {
                valStatus = objAud.IsValidLink(model.RegistryPath, "Registry", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }
            if (!string.IsNullOrEmpty(model.RentalAgreementPath)) //Shashi_21_Oct_2022
            {
                valStatus = objAud.IsValidLink(model.RentalAgreementPath, "Rental Agreement", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }



                ModelState["Registry"].Errors.Clear();

                ModelState["RentalAgreement"].Errors.Clear();
          
                ModelState["structuralLyoutFile"].Errors.Clear();
                ModelState["ElectrycityBill"].Errors.Clear();
          
            if (ModelState.IsValid)
            {
                #region Bulk Insertion Doctor

                var doctorName = form.GetValues("doctorName");
                var doctorFathersName = form.GetValues("doctorFathersName");
                var doctorQualification = form.GetValues("doctorQualification");
                var NameofFoundation = form.GetValues("NameofFoundation");
                var doctorregistrationType = form.GetValues("doctorregistrationType");
                var doctorregistrationNo = form.GetValues("doctorregistrationNo");
                var doctorHprNo = form.GetValues("doctorHprNo");
                var doctorPart_FullTime = form.GetValues("doctorPart_FullTime");
                var docFilePath = form.GetValues("docFilePath");
                var doctorAge = form.GetValues("doctorAge");
                var doctoraddress = form.GetValues("doctoraddress");
                var dyear = form.GetValues("dyear");
                var dsignature = form.GetValues("dsignature");

                int countDoc = doctorName.Count();
                string XmlDataDoc = "<DoctorStaff>";

                for (int i = 0; i < countDoc; i++)
                {
                    if (doctorName[i].ToString() == "" && doctorQualification[i] == "" && NameofFoundation[i] == "" && doctorregistrationType[i] == "")
                    {
                        //XmlData = string.Empty;
                    }
                    else
                    {
                        XmlDataDoc += "<Doctor><doctorName>" + objAud.RemoveSpecialCharactors(doctorName[i]) + "</doctorName>"
                             + "<doctorFathersName>" + objAud.RemoveSpecialCharactors(doctorFathersName[i]) + "</doctorFathersName>"
                              + "<doctorQualification>" + objAud.RemoveSpecialCharactors(doctorQualification[i]) + "</doctorQualification>"
                              + "<NameofFoundation>" + objAud.RemoveSpecialCharactors(NameofFoundation[i]) + "</NameofFoundation>"
                              + "<doctorregistrationType>" + doctorregistrationType[i] + "</doctorregistrationType>"
                              + "<doctorregistrationNo>" + objAud.FilterForAlphabetNumaric(doctorregistrationNo[i]) + "</doctorregistrationNo>"
                              + "<doctorHprNo>" + doctorHprNo[i] + "</doctorHprNo>"
                              + "<doctorPart_FullTime>" + doctorPart_FullTime[i] + "</doctorPart_FullTime>"
                              + "<docFilePath>" + docFilePath[i] + "</docFilePath>"
                              + "<doctorAge>" + doctorAge[i] + "</doctorAge>"
                              + "<doctoraddress>" + doctoraddress[i] + "</doctoraddress>"
                              + "<dyear>" + dyear[i] + "</dyear>"
                              + "<dsignature>" + dsignature[i] + "</dsignature>"

                                + "</Doctor>";
                    }
                }
                XmlDataDoc += "</DoctorStaff>";
                model.XmlDataDoc = XmlDataDoc;
                #endregion

                #region Bulk Insertion paramedical staff

                var staffName = form.GetValues("staffName");
                var stafffatherName = form.GetValues("stafffatherName");
                var staffqualification = form.GetValues("staffqualification");
                var staffinstitution = form.GetValues("staffinstitution");
                var staffRegistrationType = form.GetValues("staffRegistrationType");
                var staffRegistrationNo = form.GetValues("staffRegistrationNo");
                var staffHprNo = form.GetValues("staffHprNo");
                //var staffNameOfMCI_SMF = form.GetValues("staffNameOfMCI_SMF");
                var part_fullTime = form.GetValues("part_fullTime");
                var filePath = form.GetValues("filePath");
                var staffAge = form.GetValues("staffAge");
                var staffaddress = form.GetValues("staffaddress");
                var syear = form.GetValues("syear");
                var ssign = form.GetValues("ssign");
                int count = 0;
                if (staffName != null)
                {
                    count = staffName.Count();
                }
                int staffRowCount = 0;
                string XmlData = "<ParamedicalStaff>";

                for (int i = 0; i < count; i++)
                {
                    if (staffName[i].ToString() == "" && staffqualification[i] == "" && staffinstitution[i] == "" && staffRegistrationType[i] == "")
                    {
                        //XmlData = string.Empty;
                    }
                    else
                    {
                        XmlData += "<Staff><staffName>" + objAud.RemoveSpecialCharactors(staffName[i]) + "</staffName>"
                             + "<stafffatherName>" + objAud.RemoveSpecialCharactors(stafffatherName[i]) + "</stafffatherName>"
                              + "<staffqualification>" + objAud.RemoveSpecialCharactors(staffqualification[i]) + "</staffqualification>"
                              + "<staffinstitution>" + objAud.RemoveSpecialCharactors(staffinstitution[i]) + "</staffinstitution>"
                              + "<staffRegistrationType>" + staffRegistrationType[i] + "</staffRegistrationType>"
                              + "<staffRegistrationNo>" + objAud.FilterForAlphabetNumaric(staffRegistrationNo[i]) + "</staffRegistrationNo>"
                                + "<staffHprNo>" + objAud.FilterForAlphabetNumaric(staffHprNo[i]) + "</staffHprNo>"
                              + "<part_fullTime>" + part_fullTime[i] + "</part_fullTime>"
                              + "<filePath>" + filePath[i] + "</filePath>"
                              + "<staffAge>" + staffAge[i] + "</staffAge>"
                              + "<staffaddress>" + staffaddress[i] + "</staffaddress>"
                              + "<syear>" + syear[i] + "</syear>"
                               + "<ssign>" + ssign[i] + "</ssign>"
                              + "</Staff>";

                        staffRowCount++;
                    }
                }

                XmlData += "</ParamedicalStaff>";
                if (staffRowCount > 0)
                {
                    model.xml = XmlData;
                }
                #endregion

                #region bulkinsertion OutPatient
                if (model.isOutPatient == true)
                {
                    string[] outPatientId = form.GetValues("chkOutPatient");
                    if (outPatientId != null)
                    {
                        string xmldataOutPatient = "<OutPatients>";
                        for (int i = 0; i < outPatientId.Count(); i++)
                        {
                            if (!string.IsNullOrEmpty(outPatientId[i]))
                            {
                                xmldataOutPatient += "<OutPatient><outPatientId>" + outPatientId[i] + "</outPatientId></OutPatient>";
                            }
                        }
                        xmldataOutPatient += "</OutPatients>";
                        model.xmldataOutPatient = xmldataOutPatient;
                    }
                }

                #endregion

                #region bulkinsertion Laboratory
                if (model.isLaboratory == true)
                {
                    string[] laboratoryId = form.GetValues("chkLaboratory");
                    if (laboratoryId != null)
                    {
                        string xmldataLaboratory = "<Laboratorys>";
                        for (int i = 0; i < laboratoryId.Count(); i++)
                        {
                            if (!string.IsNullOrEmpty(laboratoryId[i]))
                            {
                                xmldataLaboratory += "<Laboratory><laboratoryId>" + laboratoryId[i] + "</laboratoryId></Laboratory>";
                            }
                        }
                        xmldataLaboratory += "</Laboratorys>";
                        model.xmldataLaboratory = xmldataLaboratory;
                    }
                }

                #endregion

                #region bulkinsertion Imaging
                if (model.isImaging == true)
                {
                    string[] imagingId = form.GetValues("chkImaging");
                    if (imagingId != null)
                    {
                        string xmldataImaging = "<Imagings>";
                        for (int i = 0; i < imagingId.Count(); i++)
                        {
                            if (!string.IsNullOrEmpty(imagingId[i]))
                            {
                                xmldataImaging += "<Imaging><imagingId>" + imagingId[i] + "</imagingId></Imaging>";
                            }
                        }
                        xmldataImaging += "</Imagings>";
                        model.xmldataImaging = xmldataImaging;
                    }
                }
                #endregion

                var res = objNUHDB.NUHInsertUpdateRenewal(model);

                try
                {
                    if (res.RegisIdNUH > 0 && res.RegistrationNo != "")
                    {
                        if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                        {
                            EDistrictServiceClass ed = new EDistrictServiceClass();

                            int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.MEE);

                            bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "MEE");

                            if (result)
                            {
                                SendSMS(res.RegistrationNo, res.MobileNo);
                                TempData["RegisIdNUH"] = res.RegisIdNUH;
                                TempData["RegistrationNo"] = res.RegistrationNo;
                                // TempData["RegNUHID"] = model.regisIdNUH;
                                //return RedirectToAction("Affidavit", new {operatedId=model.operatedId, isRenewal = true });
                                string _regisIdNUH = OTPL_Imp.CustomCryptography.Encrypt(res.RegisIdNUH.ToString());
                                return RedirectToAction("Affidavit", new { regisIdNUH = _regisIdNUH, operatedId = model.operatedId, isRenewal = true });
                            }
                            else
                            {
                                int exeRsult = objNUHDB.DeleteRegistrationNUH(res.RegisIdNUH);
                                setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                ModelState.Clear();
                            }
                        }
                        else
                        {
                            SendSMS(res.RegistrationNo, res.MobileNo);
                            TempData["RegisIdNUH"] = res.RegisIdNUH;
                            TempData["RegistrationNo"] = res.RegistrationNo;
                            //TempData["RegNUHID"] = model.regisIdNUH;
                            //return RedirectToAction("Affidavit", new {operatedId=model.operatedId, isRenewal = true });
                            string _regisIdNUH = OTPL_Imp.CustomCryptography.Encrypt(res.RegisIdNUH.ToString());
                            return RedirectToAction("Affidavit", new { regisIdNUH = _regisIdNUH, operatedId = model.operatedId, isRenewal = true });
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
                setSweetAlertMsg("Invalid Data Entered", "error");
            }

            return View();
        }

        #endregion

        public ActionResult SaveQueryDetailToApplicant(string RegisNUHID, string QueryRaised, string file)
        {

            string errormsg = "";
            string StatusResult = string.Empty;
            NUHmodel model = new NUHmodel();
            model.regisIdNUH = Convert.ToInt64(RegisNUHID);
            string ip = Common.GetIPAddress();
            model = objNUHDB.GetNUHListBYRegistrationNo(model.regisIdNUH);
            long regByUser = model.regByUser;

            if (file == "queryFilePath")
            {
                file = null;

            }
            model = objCMODB.InsertUpdateQueryRaisedDetail(QueryRaised, file, objSM.UserID.ToString(), ip, Convert.ToInt64(RegisNUHID), "2");
            if (model != null)
            {

                UPHEALTHNIC.upswp_niveshmitraservices ObjSendAppSubmitStatus = new UPHEALTHNIC.upswp_niveshmitraservices();
                //NiveshMitraSendStatusModel objStatusModel = objCMODB.GetNiveshMitraUserDetailsByID(regByUser).FirstOrDefault();
                NiveshMitraSendStatusModel objStatusModel = objCMODB.GetNiveshMitraUserDetailsByID(model.regisIdNUH).FirstOrDefault();
                if (objStatusModel != null)
                {

                    objStatusModel.ProcessIndustryID = objStatusModel.UserName;
                    objStatusModel.ApplicationID = model.regisIdNUH.ToString(); //objStatusModel.UserID.ToString();

                    objStatusModel.StatusCode = "14";
                    // objStatusModel.Remarks = QueryRaised + " " + " |  Replied by " + " " + objSM.DisplayName;
                    objStatusModel.Remarks = QueryRaised + " ";
                    objStatusModel.PendencyLevel = "Pending at CMO" + "," + " " + objStatusModel.CMODistrictName;

                    objStatusModel.FeeAmount = "";
                    objStatusModel.FeeStatus = "";
                    objStatusModel.TransectionID = "";
                    objStatusModel.TranSactionDate = "";
                    objStatusModel.TransectionDateAndTime = "";
                    objStatusModel.NocCertificateNumber = "";
                    objStatusModel.NocUrl = "";
                    objStatusModel.IsNocUrlActiveYesNo = "";
                    objStatusModel.Passalt = ConfigurationManager.AppSettings["PassKey"].ToString();
                    objStatusModel.ObjectRejectionCode = "";
                    objStatusModel.IsCertificateValidLifeTime = "";
                    objStatusModel.CertificateExpireDateDDMMYYYY = "";
                    objStatusModel.D1 = "";
                    objStatusModel.D2 = "";
                    objStatusModel.D3 = "";
                    objStatusModel.D4 = "";
                    objStatusModel.D5 = "";
                    objStatusModel.D6 = "";
                    objStatusModel.D7 = "";

                    StatusResult = ObjSendAppSubmitStatus.WReturn_CUSID_STATUS(objStatusModel.Control_ID, objStatusModel.Unit_Id, objStatusModel.ServiceID, objStatusModel.ProcessIndustryID, objStatusModel.ApplicationID, objStatusModel.StatusCode,
                           objStatusModel.Remarks, objStatusModel.PendencyLevel, objStatusModel.FeeAmount, objStatusModel.FeeStatus, objStatusModel.TransectionID, objStatusModel.TranSactionDate, objStatusModel.TransectionDateAndTime, objStatusModel.NocCertificateNumber, objStatusModel.NocUrl, objStatusModel.IsNocUrlActiveYesNo, objStatusModel.Passalt, objStatusModel.RequestId, objStatusModel.ObjectRejectionCode
                            , objStatusModel.IsCertificateValidLifeTime, objStatusModel.CertificateExpireDateDDMMYYYY, objStatusModel.D1, objStatusModel.D2, objStatusModel.D3, objStatusModel.D4, objStatusModel.D5, objStatusModel.D6, objStatusModel.D7);

                    if (StatusResult.ToUpper() == "SUCCESS")
                    {
                        objStatusModel.SendDate = System.DateTime.Now;
                        objStatusModel.ResStatus = "";
                        objStatusModel.ServiceStatus = StatusResult;
                        objStatusModel.StepId = 5;

                    }
                    else
                    {

                        objStatusModel.SendDate = System.DateTime.Now;
                        objStatusModel.ResStatus = "";
                        objStatusModel.ServiceStatus = StatusResult;
                        objStatusModel.StepId = 5;

                    }

                    try
                    {
                        objStatusModel = objCMODB.SaveCmoActionAndNiveshStatus(objStatusModel).FirstOrDefault();

                    }
                    catch (Exception ex)
                    {

                    }

                }
                errormsg = model.QMessage;
                setSweetAlertMsg(errormsg, "warning");
            }
            else
            {

                errormsg = "0";
            }

            return Json(errormsg, JsonRequestBehavior.AllowGet);
            // return View();

        }

        public string GetQueryDetailsByCMO(long MedicalEstablishmentID)
        {
            string QueryDetail = string.Empty;
            QueryModel objModel = new QueryModel();
            objModel = objNUHDB.GetQueryDetailsByCMO(MedicalEstablishmentID);
            return objModel.QueryDetails;
        }

        //public ActionResult GetQueryDetailsByCMO(long MedicalEstablishmentID)
        //{
        //    string QueryDetail = string.Empty;
        //    QueryModel objModel = new QueryModel();
        //    objModel = objNUHDB.GetQueryDetailsByCMO(MedicalEstablishmentID);

        //    //  return Content(objModel.QueryDetails);
        //    return Json(objModel.QueryDetails, JsonRequestBehavior.AllowGet);

        //}



        public ActionResult DeclarationForm()
        {

            Declaration model = new Declaration();
            if (string.IsNullOrEmpty(objSM.UserID.ToString()))
            {

            }

            model.regisID = Convert.ToInt64(objSM.UserID.ToString());
            model = objNUHDB.ShowDeclarationdata(model.regisID, 2);
            return View(model);
        }

        public ActionResult Affidavit(bool isRenewal, int operatedId, string regisIdNUH)
        {
            Declaration model = new Declaration();
            if (string.IsNullOrEmpty(objSM.UserID.ToString()))
            {

            }
            model.regisIdNUH = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdNUH));
            // model.regisIdNUH = Convert.ToInt64(TempData["RegisIdNUH"]);
            model = objNUHDB.ShowAffidavitData(model.regisIdNUH);
            model.OwnerList = objNUHDB.GetOwnerList(model.regisIdNUH);
            model.isRenewal = isRenewal;
            model.operatedId = operatedId;
            return View(model);
        }

        [HttpPost]
        public ActionResult Affidavit(Declaration model)
        {
            string errormsg = "";
            model.DeclarationUserId = objSM.UserID;
            model.DeclarationIp = Common.GetIPAddress();
            var res = objNUHDB.UpdateAffidavitNUH(model).isSuccess;

            if (res == 1)
            {
                //errormsg = "Declaration Submitted Successfully";
                //setSweetAlertMsg(errormsg, "success");

                TempData["RegisIdNUH"] = model.NuhId;
                TempData["RegistrationNo"] = model.registrationNumber;
                TempData["Message"] = "Declaration Submitted Successfully.";
                string _regisIdNUH = OTPL_Imp.CustomCryptography.Encrypt(model.NuhId.ToString());
                return RedirectToAction("Pairamedical", "NUH", new { regisIdNUH = _regisIdNUH, isRenewal = model.isRenewal });


                 //return RedirectToAction("RegistrationConfirmation", "NUH", new { isRenewal = model.isRenewal });

            }

            else
            {
                errormsg = "Some error Occured";
                setSweetAlertMsg(errormsg, "warning");
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult Receipt(string regisIdNUH)
        {
            ReceiptModel model = new ReceiptModel();

            model.regisIdNUH = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdNUH));

            model = objNUHDB.GetNUHReceipt(model.regisIdNUH);
            model.ReceiptList = objNUHDB.ReceiptList(model.regisIdNUH);
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("RegistrationConfirmation", "NUH", new { isRenewal = true });
            }
        }

        [HttpGet]
        public ActionResult DownloadReceipt(string regisIdNUH)
        {
            ReceiptModel model = new ReceiptModel();

            model.regisIdNUH = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdNUH));

            model = objNUHDB.GetNUHReceipt(model.regisIdNUH);
            model.ReceiptList = objNUHDB.ReceiptList(model.regisIdNUH);
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("RegistrationConfirmation", "NUH", new { isRenewal = true });
            }
        }

        [HttpGet]
        public ActionResult Pairamedical(string regisIdNUH, bool isRenewal)
        {
            PairamedicalModel model = new PairamedicalModel();
           
            //model.regisIdNUH = Convert.ToInt64(regisIdNUH);
            //model.regisIdNUH = Convert.ToInt64(TempData["RegisIdNUH"]);
            model.regisIdNUH = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdNUH));
            model.Paramedicallist = objNUHDB.GetParamedicalDetails(model.regisIdNUH);
          
            model.isRenewal = isRenewal;
            
            return View(model);

        }

        [HttpPost]
        public JsonResult Pairamedical(PairamedicalModel model)
        {

            //model.regisIdNUH=TempData["regisIdNUH"];
            //model.regisIdNUH = Convert.ToInt64(TempData["regisIdNUH"]);

            string errormsg = "";
            string StatusResult = string.Empty;
            model.ParamedicalDeclarationUserId = objSM.UserID;
            model.ParamedicalDeclarationIp = Common.GetIPAddress();
            List<PairamedicalModel> valueSet = JsonConvert.DeserializeObject<List<PairamedicalModel>>(model.doctorlist);

            if (valueSet.Count > 0)
            {
                XElement root = new XElement("root");
                for (int i = 0; i < valueSet.Count; i++)
                {
                  
                    var doctorId = valueSet[i].doctorId;
                    XElement ele = new XElement("doclist",
                    new XElement("doctorId", doctorId)

                    );
                    root.Add(ele);
                }
                XElement xml1 = root;
                model.doctorlist = xml1.ToString();
            }


            model.DeclarationUserId = objSM.UserID;
            model.DeclarationIp = Common.GetIPAddress();

            var res = objNUHDB.UpdateParamedical(model).isSuccess;
            if (res == 1)
            {
                //errormsg = "Declaration Submitted Successfully";
                //setSweetAlertMsg(errormsg, "success");
                //TempData["RegisIdNUH"] = model.NuhId;
                //return RedirectToAction("RegistrationConfirmation", "NUH", new { regisIdNUH = model.NuhId, isRenewal = model.isRenewal });


                UPHEALTHNIC.upswp_niveshmitraservices ObjSendAppSubmitStatus = new UPHEALTHNIC.upswp_niveshmitraservices();
                NiveshMitraSendStatusModel userDetails = new NiveshMitraSendStatusModel();
                //userDetails = objNUHDB.GetNiveshUserDetailsToSendMedicalRegisStatus(objSM.UserID);
                userDetails = objNUHDB.GetNiveshUserDetailsToSendMedicalRegisStatus(model.NuhId);
                if (userDetails != null)
                { 
                    userDetails.ProcessIndustryID = objSM.Username;
                    userDetails.ApplicationID = model.NuhId.ToString(); //objSM.UserID.ToString();

                    userDetails.StatusCode = "13";
                    userDetails.Remarks = "Application Submitted by Applicant";
                    //  userDetails.PendencyLevel = "Pending at Department";
                    userDetails.PendencyLevel = "Pending at CMO" + "," + " " + userDetails.CMODistrictName;

                    userDetails.FeeAmount = "";
                    userDetails.FeeStatus = "";
                    userDetails.TransectionID = "";
                    userDetails.TranSactionDate = "";
                    userDetails.TransectionDateAndTime = "";
                    userDetails.NocCertificateNumber = "";
                    userDetails.NocUrl = "";
                    userDetails.IsNocUrlActiveYesNo = "";
                    userDetails.Passalt = ConfigurationManager.AppSettings["PassKey"].ToString();
                    userDetails.ObjectRejectionCode = "";
                    userDetails.IsCertificateValidLifeTime = "";
                    userDetails.CertificateExpireDateDDMMYYYY = "";
                    userDetails.D1 = "";
                    userDetails.D2 = "";
                    userDetails.D3 = "";
                    userDetails.D4 = "";
                    userDetails.D5 = "";
                    userDetails.D6 = "";
                    userDetails.D7 = "";

                    StatusResult = ObjSendAppSubmitStatus.WReturn_CUSID_STATUS(userDetails.Control_ID, userDetails.Unit_Id, userDetails.ServiceID, userDetails.ProcessIndustryID, userDetails.ApplicationID, userDetails.StatusCode,
                           userDetails.Remarks, userDetails.PendencyLevel, userDetails.FeeAmount, userDetails.FeeStatus, userDetails.TransectionID, userDetails.TranSactionDate, userDetails.TransectionDateAndTime, userDetails.NocCertificateNumber, userDetails.NocUrl, userDetails.IsNocUrlActiveYesNo, userDetails.Passalt, userDetails.RequestId, userDetails.ObjectRejectionCode
                            , userDetails.IsCertificateValidLifeTime, userDetails.CertificateExpireDateDDMMYYYY, userDetails.D1, userDetails.D2, userDetails.D3, userDetails.D4, userDetails.D5, userDetails.D6, userDetails.D7);

                    if (StatusResult.ToUpper() == "SUCCESS")
                    {

                        userDetails.SendDate = System.DateTime.Now;
                        userDetails.ResStatus = "";
                        userDetails.ServiceStatus = StatusResult;
                        userDetails.StepId = 3;

                        try
                        {

                            userDetails = objAccDb.SendApplicationSubmittedStatus(userDetails).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {

                        }
                         
                        //TempData["FormNotSubmitted"] = "Submitted";
                        //return RedirectToAction("NursingDashBoard", "NUH");
                        //return RedirectToAction("RegistrationConfirmation", "NUH", new { isRenewal = model.isRenewal }); 
                    }
                    else
                    { 
                        userDetails.SendDate = System.DateTime.Now;
                        userDetails.ResStatus = "";
                        userDetails.ServiceStatus = StatusResult;
                        userDetails.StepId = 3;
                        try
                        { 
                            userDetails = objAccDb.SendApplicationSubmittedStatus(userDetails).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {

                        } 
                    }

                    TempData["RegisIdNUH"] = model.NuhId;
                    //TempData["Message"] = "Declaration Submitted Successfully.";
                    model.isSuccess = 1;
                    model.message = "Declaration Submitted Successfully.";
                    return Json(model, JsonRequestBehavior.AllowGet);

                    //if (userDetails != null)
                    //{

                    //    if (userDetails.QueryExFlag == "1")
                    //    {

                    //    }
                    //}
                }
                else
                {
                    TempData["RegisIdNUH"] = model.NuhId;  
                    //TempData["Message"] = "Declaration Submitted Successfully";
                    model.isSuccess = 1;
                    model.message = "Declaration Submitted Successfully.";
                    return Json(model,JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("RegistrationConfirmation", "NUH", new { isRenewal = model.isRenewal }); 
                } 
            } 
            else
            {
                errormsg = "Some error Occured";
                setSweetAlertMsg(errormsg, "warning");
                model.isSuccess = 0;
                model.message = "Some error Occured";
                return Json(model, JsonRequestBehavior.AllowGet);
                //return View(model);
            }

            return Json(model, JsonRequestBehavior.AllowGet);

            //return RedirectToAction("RegistrationConfirmation", new { isRenewal = model.isRenewal });
        } 
    }
}
