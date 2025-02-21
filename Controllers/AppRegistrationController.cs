using CCSHealthFamilyWelfareDept.DAL;
using CCSHealthFamilyWelfareDept.Filters;
using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    //[AuthorizeAdmin()]
    public class AppRegistrationController : Controller
    {
        FAP_DB objFAPDB = new FAP_DB();
        //
        // GET: /AppRegistration/

        Account_DB objAccDb = new Account_DB();
        Common_DB objComnDb = new Common_DB();
        Common objComn = new Common();
        AppRegistration_DB appDB = new AppRegistration_DB();
        SessionManager objSM = new SessionManager();
        FIC_DB objFICdb = new FIC_DB();
        NUH_DB objNUHDB = new NUH_DB();

        #region Method Set Sweet Alert Message
        protected void setSweetAlertMsg(string msg, string msgStatus)
        {
            ViewBag.Msg = msg;
            ViewBag.MsgStatus = msgStatus;
        }
        #endregion

        #region CMO

        #region NUH  (abhijeet)
        [HttpPost]
        public JsonResult CheckMobileExistence(string appMobileNo)
        {
            long regisId = 0;

            if (Session["regisIdNUH"] != null)
            {
                regisId = Convert.ToInt64(Session["regisIdNUH"]);
            }
            //var user = appDB.CheckEmailMobileExistence(appMobileNo, "M", regisId);
            var user = objNUHDB.CheckEmailMobileExistence(appMobileNo, "M", regisId, objSM.MeeRegisNo);
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [AuthorizeAdmin(1)]
        [HttpGet]
        public ActionResult NUHRegistration(string regisId = "", int stepValue = -1, string type = "")
        {

            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);

            NUHmodel model = new NUHmodel();
            if (type == "renewal")
            {
                model.isRenewal = true;
            }
            if (regisId != "")
            {
                model = appDB.getNUHStep((Convert.ToInt64(regisId)));
                if (model.applicantDistrictId != 0)
                {
                    TempData["applicantDistrictId"] = model.applicantDistrictId;
                }
                if (model.ownerDistrictIdF != null)
                {
                    TempData["ownerDistrictIdF"] = model.ownerDistrictIdF;
                }
                if (model != null && objSM.UserID != model.regByUser)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home");
                }

                Session["regisIdNUH"] = model.regisIdNUH;
                ViewBag.IsNOCC = model.isNOC;
                ViewBag.IsDisposeq = model.isDispose;
                ViewBag.isFirefightingSystems = model.isFirefightingSystem;
                ViewBag.isOutPatientw = model.isOutPatient;
                ViewBag.isLaboratoryw = model.isLaboratory;
                ViewBag.isImagingw = model.isImaging;
                ViewBag.isInPatientW = model.isInPatient;
            }
            else
            {
                Session["regisIdNUH"] = null;
            }
            if (model.isRenewal == true)
            {
                ViewBag.Toptitle = "Application form for Renewal  of ";
            }
            else
            {
                ViewBag.Toptitle = "Application form for Registration  of ";
            }
            if (model.step == 3)
            {
                ViewBag.doctorList = appDB.getNUHdoctorList(Convert.ToInt64(regisId));
                ViewBag.staffList = appDB.getNUHParamedicalList(Convert.ToInt64(regisId));

                ViewBag.doctorListCount = appDB.getNUHdoctorList(Convert.ToInt64(regisId)).Count() - 1;
                ViewBag.staffListCount = appDB.getNUHParamedicalList(Convert.ToInt64(regisId)).Count() - 1;
                Common_DB comndb = new Common_DB();
                ViewBag.OutPatient = comndb.GetDropDownListFilled(1, Convert.ToInt64(regisId));
                //.Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString(), Value1 = m.Id.ToString() });
                ViewBag.Laboratory = comndb.GetDropDownListFilled(2, Convert.ToInt64(regisId));//...Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.Imaging = comndb.GetDropDownListFilled(3, Convert.ToInt64(regisId));//.Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() }); 

            }
            else
            {
                //ViewBag.CheckList = objComnDb.GetDropDownList(38, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                Common_DB comndb = new Common_DB();
                ViewBag.OutPatient = comndb.GetDropDownList(49, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.Laboratory = comndb.GetDropDownList(50, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.Imaging = comndb.GetDropDownList(51, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            }
            string st = "";
            string dt = "";
            if (model.step == 2 || model.step == 3)
            {
                model.NUHPartnerList = appDB.getNUHOwnerList(regisId);
                for (int i = 0; i < model.NUHPartnerList.Count(); i++)
                {
                    st = st + model.NUHPartnerList[i].StateName + ",";
                    dt = dt + model.NUHPartnerList[i].DistrictName + ",";
                }
                ViewBag.st = st;
                ViewBag.dt = dt;
                ViewBag.OwnerList = appDB.getNUHOwnerList(regisId);
            }

            if (stepValue >= 0)
            {
                model.stepValue = stepValue;
                Session["stepValue"] = model.stepValue;
                //Session["nextStep"] = model.stepValue;
                //model.stepValue--;
                //Session["backStep"]=model.stepValue;
            }
            else
            {
                Session["stepValue"] = model.step;
                model.stepValue = model.step;
                //model.stepValue = stepValue;
            }

            ViewBag.State = objComnDb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComnDb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.DistrictEStList = objComnDb.GetDropDownList(7, 34).Where(e => e.Id == objSM.districtId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Operate = objComnDb.GetDropDownList(37, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.MedicalEstablishment = objComnDb.GetDropDownList(5, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.CheckList = objComnDb.GetDropDownList(38, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            model.districtid = objSM.districtId;
            ViewBag.State1 = objComnDb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District1 = Enumerable.Empty<SelectListItem>();
            //Common_DB comndb = new Common_DB();
            //ViewBag.OutPatient = comndb.GetDropDownList(49, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            //ViewBag.Laboratory = comndb.GetDropDownList(50, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            //ViewBag.Imaging = comndb.GetDropDownList(51, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });


            return View(model);
        }
        public JsonResult binddistNUH(int ownerStateIdF)
        {
            var res = objComnDb.GetDropDownList(7, ownerStateIdF).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DropdownNUH()
        {
            var res = objComnDb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeAdmin(1)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NUHRegistration(NUHmodel model, FormCollection form)
        {
            AuditMethods objAudit = new AuditMethods();
            var result = appDB.getNUHStep(model.regisIdNUH);
            if (model.numberofBed > 49)
            {
                setSweetAlertMsg("Number of Beds(Inpatient) Can Not Be More Than 49", "info");
                return View(model);
            }
            if (result != null && objSM.UserID != result.regByUser)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }
            if (model.isCertificateFromPortal == true)
            {
                ModelState["outerRegistrationNo"].Errors.Clear();
                ModelState["outerCertificateNo"].Errors.Clear();
                ModelState["outerCertificateFile"].Errors.Clear();
                model.isRenewal = false;
            }
            long _regisId = model.regisIdNUH;
            model.regByUser = objSM.UserID;
            model.regBytransIp = Common.GetIPAddress();

            model.applicantMobileNo = model.appMobileNo;
            if (model.medicalEstablishmentId != 9)
            {
                ModelState["medicalEstablishmentOther"].Errors.Clear();
            }

            if (model.step == 2 && (model.stepValue == -1 || model.stepValue == 2))
            {
                #region Bulk Insertion Doctor

                var doctorName = form.GetValues("doctorName");
                var doctorFathersName = form.GetValues("doctorFathersName");
                var doctorQualification = form.GetValues("doctorQualification");
                var NameofFoundation = form.GetValues("NameofFoundation");
                var doctorregistrationType = form.GetValues("doctorregistrationType");
                var doctorregistrationNo = form.GetValues("doctorregistrationNo");
                var doctorPart_FullTime = form.GetValues("doctorPart_FullTime");
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

                        XmlDataDoc += "<Doctor><doctorName>" + objAudit.RemoveSpecialCharactors(doctorName[i]) + "</doctorName>"
                             + "<doctorFathersName>" + objAudit.RemoveSpecialCharactors(doctorFathersName[i]) + "</doctorFathersName>"
                              + "<doctorQualification>" + objAudit.RemoveSpecialCharactors(doctorQualification[i]) + "</doctorQualification>"
                              + "<NameofFoundation>" + objAudit.RemoveSpecialCharactors(NameofFoundation[i]) + "</NameofFoundation>"
                              + "<doctorregistrationType>" + doctorregistrationType[i] + "</doctorregistrationType>"
                              + "<doctorregistrationNo>" + objAudit.FilterForAlphabetNumaric(doctorregistrationNo[i]) + "</doctorregistrationNo>"
                              + "<doctorPart_FullTime>" + doctorPart_FullTime[i] + "</doctorPart_FullTime>"
                              + "<doctorAge>" + doctorAge[i] + "</doctorAge>"
                              + "<doctoraddress>" + doctoraddress[i] + "</doctoraddress>"
                               + "<dyear>" + dyear[i] + "</dyear>"
                                + "<dsignature>" + dsignature[i] + "</dsignature>"
                                + "</Doctor>";
                    }

                }
                XmlDataDoc += "</DoctorStaff>";
                model.xmldatadoctor = XmlDataDoc;
                #endregion

                #region Bulk Insertion paramedical staff
                var staffName = form.GetValues("staffName");
                var stafffatherName = form.GetValues("stafffatherName");
                var staffqualification = form.GetValues("staffqualification");
                var staffinstitution = form.GetValues("staffinstitution");
                var staffRegistrationType = form.GetValues("staffRegistrationType");
                var staffRegistrationNo = form.GetValues("staffRegistrationNo");
                //var staffNameOfMCI_SMF = form.GetValues("staffNameOfMCI_SMF");
                var part_fullTime = form.GetValues("part_fullTime");
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
                        XmlData += "<Staff><staffName>" + objAudit.RemoveSpecialCharactors(staffName[i]) + "</staffName>"
                             + "<stafffatherName>" + objAudit.RemoveSpecialCharactors(stafffatherName[i]) + "</stafffatherName>"
                              + "<staffqualification>" + objAudit.RemoveSpecialCharactors(staffqualification[i]) + "</staffqualification>"
                              + "<staffinstitution>" + objAudit.RemoveSpecialCharactors(staffinstitution[i]) + "</staffinstitution>"
                              + "<staffRegistrationType>" + staffRegistrationType[i] + "</staffRegistrationType>"
                              + "<staffRegistrationNo>" + objAudit.FilterForAlphabetNumaric(staffRegistrationNo[i]) + "</staffRegistrationNo>"
                              + "<part_fullTime>" + part_fullTime[i] + "</part_fullTime>"
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
                    model.xmldataParmedical = XmlData;
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
            }
            else if (model.step == 3 && model.stepValue == 2)
            {
                #region Bulk Insertion Doctor

                var doctorName = form.GetValues("doctorName");
                var doctorFathersName = form.GetValues("doctorFathersName");
                var doctorQualification = form.GetValues("doctorQualification");
                var NameofFoundation = form.GetValues("NameofFoundation");
                var doctorregistrationType = form.GetValues("doctorregistrationType");
                var doctorregistrationNo = form.GetValues("doctorregistrationNo");
                var doctorPart_FullTime = form.GetValues("doctorPart_FullTime");
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

                        XmlDataDoc += "<Doctor><doctorName>" + objAudit.RemoveSpecialCharactors(doctorName[i]) + "</doctorName>"
                             + "<doctorFathersName>" + objAudit.RemoveSpecialCharactors(doctorFathersName[i]) + "</doctorFathersName>"
                              + "<doctorQualification>" + objAudit.RemoveSpecialCharactors(doctorQualification[i]) + "</doctorQualification>"
                              + "<NameofFoundation>" + objAudit.RemoveSpecialCharactors(NameofFoundation[i]) + "</NameofFoundation>"
                              + "<doctorregistrationType>" + doctorregistrationType[i] + "</doctorregistrationType>"
                              + "<doctorregistrationNo>" + objAudit.FilterForAlphabetNumaric(doctorregistrationNo[i]) + "</doctorregistrationNo>"
                              + "<doctorPart_FullTime>" + doctorPart_FullTime[i] + "</doctorPart_FullTime>"
                              + "<doctorAge>" + doctorAge[i] + "</doctorAge>"
                              + "<doctoraddress>" + doctoraddress[i] + "</doctoraddress>"
                               + "<dyear>" + dyear[i] + "</dyear>"
                                + "<dsignature>" + dsignature[i] + "</dsignature>"

                                + "</Doctor>";
                    }

                }
                XmlDataDoc += "</DoctorStaff>";
                model.xmldatadoctor = XmlDataDoc;
                #endregion

                #region Bulk Insertion paramedical staff
                var staffName = form.GetValues("staffName");
                var stafffatherName = form.GetValues("stafffatherName");
                var staffqualification = form.GetValues("staffqualification");
                var staffinstitution = form.GetValues("staffinstitution");
                var staffRegistrationType = form.GetValues("staffRegistrationType");
                var staffRegistrationNo = form.GetValues("staffRegistrationNo");
                //var staffNameOfMCI_SMF = form.GetValues("staffNameOfMCI_SMF");
                var part_fullTime = form.GetValues("part_fullTime");
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

                        XmlData += "<Staff><staffName>" + objAudit.RemoveSpecialCharactors(staffName[i]) + "</staffName>"
                             + "<stafffatherName>" + objAudit.RemoveSpecialCharactors(stafffatherName[i]) + "</stafffatherName>"
                              + "<staffqualification>" + objAudit.RemoveSpecialCharactors(staffqualification[i]) + "</staffqualification>"
                              + "<staffinstitution>" + objAudit.RemoveSpecialCharactors(staffinstitution[i]) + "</staffinstitution>"
                              + "<staffRegistrationType>" + staffRegistrationType[i] + "</staffRegistrationType>"
                              + "<staffRegistrationNo>" + objAudit.FilterForAlphabetNumaric(staffRegistrationNo[i]) + "</staffRegistrationNo>"
                              + "<part_fullTime>" + part_fullTime[i] + "</part_fullTime>"
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
                    model.xmldataParmedical = XmlData;
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
            }
            else if (model.step == 3 && (model.stepValue == -1 || model.stepValue == 3))
            {
                #region Bulk Insertion checkList
                var checkListId = form.GetValues("chk_checkList");

                int chk_count = checkListId.Count();
                string XmlDatacheckList = "<CheckList>";


                for (int i = 0; i < chk_count; i++)
                {
                    if (checkListId[i].ToString() == "" || checkListId[i].ToString() == "0")
                    {
                        //XmlDatacheckList = string.Empty;
                    }
                    else
                    {

                        XmlDatacheckList += "<CheckData><checkListId>" + checkListId[i] + "</checkListId></CheckData>";
                    }

                }
                XmlDatacheckList += "</CheckList>";
                model.xmldatacheckList = XmlDatacheckList;
                #endregion


            }
            AuditMethods objAud = new AuditMethods();
            string errormsg = "";
            bool valStatus = false;
            if ((model.step == 1 || model.step == 2) && model.stepValue == 1)
            {
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

                #region Bulk Insertion owner detail
                string XmlDataOwner = "<Owner>";
                if (model.operatedId == 2)
                {
                    //string valid;

                    //ModelState["ownerName"].Errors.Clear();
                    //ModelState["ownerAge"].Errors.Clear();
                    //ModelState["ownerFatherNameF"].Errors.Clear();
                    //ModelState["ownerMobileNoF"].Errors.Clear();
                    //ModelState["ownerEmailIdF"].Errors.Clear();
                    //ModelState["ownerAddressF"].Errors.Clear();
                    //ModelState["ownerStateIdF"].Errors.Clear();
                    //ModelState["ownerDistrictIdF"].Errors.Clear();
                    //ModelState["ownerPincodeF"].Errors.Clear();

                    var ownerName = form.GetValues("ownerName");
                    var ownerAge = form.GetValues("ownerAge");
                    var ownerFatherName = form.GetValues("ownerFatherName");
                    var ownerMobileNo = form.GetValues("ownerMobileNo");
                    var ownerEmailId = form.GetValues("ownerEmailId");
                    var ownerAddress = form.GetValues("ownerAddress");
                    var ownerStateId = form.GetValues("ownerStateId");
                    var ownerDistrictId = form.GetValues("ownerDistrictId");
                    var ownerPincode = form.GetValues("ownerPincode");
                    var ownerSignature = form.GetValues("ownerSignature");
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


                                XmlDataOwner += "<Partner><ownerName>" + objAudit.RemoveSpecialCharactors(ownerName[i]) + "</ownerName>"
                                     + "<ownerAge>" + ownerAge[i] + "</ownerAge>"
                                      + "<ownerFatherName>" + objAudit.RemoveSpecialCharactors(ownerFatherName[i]) + "</ownerFatherName>"
                                      + "<ownerMobileNo>" + ownerMobileNo[i] + "</ownerMobileNo>"
                                      + "<ownerEmailId>" + ownerEmailId[i] + "</ownerEmailId>"
                                      + "<ownerAddress>" + objAudit.FilterForAlphabetNumaric(ownerAddress[i]) + "</ownerAddress>"
                                       + "<ownerStateId>" + ownerStateId[i] + "</ownerStateId>"
                                       + "<ownerDistrictId>" + ownerDistrictId[i] + "</ownerDistrictId>"
                                      + "<ownerPincode>" + ownerPincode[i] + "</ownerPincode>"

                                        + "</Partner>";
                            }
                        }

                    }
                }
                else
                {
                    //////riya
                    //ModelState["ownerName"].Errors.Clear();
                    //ModelState["ownerAge"].Errors.Clear();
                    //ModelState["ownerFatherName"].Errors.Clear();
                    //ModelState["ownerMobileNo"].Errors.Clear();
                    //ModelState["ownerEmailId"].Errors.Clear();
                    //ModelState["ownerAddress"].Errors.Clear();
                    //ModelState["ownerStateId"].Errors.Clear();
                    //ModelState["ownerDistrictId"].Errors.Clear();
                    //ModelState["ownerPincode"].Errors.Clear(); 

                    XmlDataOwner += "<Partner><ownerName>" + model.ownerNameF + "</ownerName>"
                                  + "<ownerAge>" + model.ownerAgeF + "</ownerAge>"
                                   + "<ownerFatherName>" + model.ownerFatherNameF + "</ownerFatherName>"
                                   + "<ownerMobileNo>" + model.ownerMobileNoF + "</ownerMobileNo>"
                                   + "<ownerEmailId>" + model.ownerEmailIdF + "</ownerEmailId>"
                                   + "<ownerAddress>" + model.ownerAddressF + "</ownerAddress>"
                                    + "<ownerStateId>" + model.ownerStateIdF + "</ownerStateId>"
                                    + "<ownerDistrictId>" + model.ownerDistrictIdF + "</ownerDistrictId>"
                                   + "<ownerPincode>" + model.ownerPincodeF + "</ownerPincode>"
                                  + "<ownerSignature>" + model.ownerSignature + "</ownerSignature>"
                        //+ "<ownerPhotograph>" + model.ownerFPhotographPath + "</ownerPhotograph>"
                        // + "<ownerSignature>" + model.ownerFSignaturePath + "</ownerSignature>"
                                     + "</Partner>";
                }
                XmlDataOwner += "</Owner>";
                model.XmlDataOwner = XmlDataOwner;
                #endregion

            }

            try
            {
                ModelState.Clear();
                model.districtid = objSM.districtId;
                var res = appDB.InsertUpdateNUH(model);

                if (res.Flag == 1)
                {
                    if (res.RegisId != 0)
                    {
                        _regisId = res.RegisId;
                    }
                    TempData["Message"] = res.Msg;
                    if (model.step == 3 && res.appStatus == 1)
                    {
                        string _regisIdnuh = OTPL_Imp.CustomCryptography.Encrypt(model.regisIdNUH.ToString());
                        return RedirectToAction("Affidavit", new { isRenewal = res.isRenewal, operatedId = res.operatedId, RegisIdNUH = _regisIdnuh, });
                    }
                    else
                    {
                        return RedirectToAction("NUHRegistration", new { regisId = OTPL_Imp.CustomCryptography.Encrypt(_regisId.ToString()), stepValue = Convert.ToInt32(Session["stepValue"]) + 1 });
                    }
                }
                else
                {
                    ViewBag.State = objComnDb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                    ViewBag.District = objComnDb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                    ViewBag.Operate = objComnDb.GetDropDownList(37, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                    ViewBag.MedicalEstablishment = objComnDb.GetDropDownList(5, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                    ViewBag.CheckList = objComnDb.GetDropDownList(38, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                    model.districtid = objSM.districtId;
                    ViewBag.State1 = objComnDb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                    ViewBag.District1 = Enumerable.Empty<SelectListItem>();
                    ViewBag.DistrictEStList = objComnDb.GetDropDownList(7, 34).Where(e => e.Id == objSM.districtId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                    setSweetAlertMsg(res.Msg, "success");
                    ModelState.Clear();
                }
            }
            catch
            {
                setSweetAlertMsg("Some Error Occurr", "error");
            }
            return View(model);

        }

        [AuthorizeAdmin(1)]
        public ActionResult ViewNUHComplete(string regisIdN = "")
        {
            NUHmodel model = new NUHmodel();
            model.NUHModelList = appDB.getNUHCompleteRegistration(objSM.UserID);









            //    if (item.regisIdNUH != null && item.regisIdNUH > 0)
            //    {

            //        string _regisId = OTPL_Imp.CustomCryptography.Encrypt(item.regisIdNUH.ToString());
            //        return RedirectToAction("Pairamedical", "NUH", new { regisIdNUH = _regisId, isRenewal = item.isRenewal });

            //    }
            //}
            return View(model.NUHModelList);
        }

        [AuthorizeAdmin(1)]
        public ActionResult ViewNUHInComplete()
        {
            NUHmodel model = new NUHmodel();
            model.NUHModelList = appDB.getNUHInCompleteRegistration(objSM.UserID);
            return View(model.NUHModelList);
        }

        void SendSMSforNUH(string registrationNo, string mobileNo)
        {

            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";

                //txtmsg = "Dear Citizen,\n\nYour Application form has been submitted successfully. Your Application Form Number is " + registrationNo + ", kindly use this further.\n\n Thanks";
                txtmsg = "Dear Citizen,\n\nYour Application form for Medical Establishment has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\n MHFWD, UP";


                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }

        }
        #endregion

        #region NUH Renewal
        [AuthorizeAdmin(1)]
        public ActionResult SearchNUHDetails()
        {
            return View();
        }

        [AuthorizeAdmin(1)]
        [HttpPost]
        public ActionResult SearchNUHDetails(SearchDetailModel model)
        {
            string _regisIdN = "";
            ModelState["isCertFrmPortal"].Errors.Clear();
            if (ModelState.IsValid)
            {
                var resultData = appDB.SearchDetails(model, objSM.districtId);//objSM.UserID);
                if (resultData != null && resultData.Flag == 1)
                {
                    _regisIdN = OTPL_Imp.CustomCryptography.Encrypt(resultData.RegisId.ToString());
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
                return RedirectToAction("NUHRenewalRegistration", "AppRegistration", new { regisIdN = _regisIdN });
            }
        }

        #region
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




        #region NUH Renewal
        [AuthorizeAdmin(1)]
        public ActionResult NUHRenewalRegistration(string regisIdN = "")
        {
            NUHmodel model = new NUHmodel();

            long regisIdNUH = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdN));


            model = appDB.GetDetailsNUHByRegisId(regisIdNUH);
            if (model.applicantDistrictId != 0)
            {
                TempData["applicantDistrictId"] = model.applicantDistrictId;
            }
            if (model.ownerDistrictIdF != null)
            {
                TempData["ownerDistrictIdF"] = model.ownerDistrictIdF;
            }
            objSM.MeeRegisNo = model.meeRegisNo;

            ViewBag.MedicalEstablishment = objComnDb.GetDropDownList(5, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.MedicalEstablishmentCategory = objComnDb.GetDropDownList(9, 0).ToList();
            ViewBag.MedicalEstablishmentCategoryType = Enumerable.Empty<SelectListItem>();
            ViewBag.ClinicalEstablishment = objComnDb.GetDropDownList(13, 0).ToList();

            ViewBag.ClinicalSubEstablishment = Enumerable.Empty<SelectListItem>();
            ViewBag.State = objComnDb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComnDb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Operate = objComnDb.GetDropDownList(37, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            ViewBag.OutPatient = appDB.NUH_FacilityOfferedForRenewal(1, regisIdNUH);
            ViewBag.Laboratory = appDB.NUH_FacilityOfferedForRenewal(2, regisIdNUH);
            ViewBag.Imaging = appDB.NUH_FacilityOfferedForRenewal(3, regisIdNUH);

            ViewBag.PartnerList = appDB.getNUHPartner(regisIdNUH);
            ViewBag.doctorList = appDB.getNUHdoctorList(regisIdNUH);
            ViewBag.staffList = appDB.getNUHParamedicalList(regisIdNUH);
            ViewBag.CheckListRenewal = objComnDb.GetDropDownList(63, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State1 = objComnDb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District1 = Enumerable.Empty<SelectListItem>();
            return View(model);
        }

        [AuthorizeAdmin(1)]
        [HttpPost]
        public ActionResult NUHRenewalRegistration(NUHmodel model, FormCollection form)
        {
            AuditMethods objAudit = new AuditMethods();
            if (model.numberofBed > 49)
            {
                setSweetAlertMsg("Number of Beds(Inpatient) Can Not Be More Than 49", "info");
                return View(model);
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

            if (objSM.StructuralLayOut != null)
            {
                if (!String.IsNullOrEmpty(model.structuralLyoutFilePath))
                { }
                else
                {
                    model.structuralLyoutFilePath = objSM.StructuralLayOut;
                }
            }
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

            ModelState["addressproofFile"].Errors.Clear();
            ModelState["nocCertificationNo"].Errors.Clear();
            //ModelState["structuralLyoutFile"].Errors.Clear();
            ModelState["applicantMobileNo"].Errors.Clear();
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
            ModelState["queryFile"].Errors.Clear();
            ModelState["QueryRaisedByCMO"].Errors.Clear();
            ///////////////////////////////
            ModelState["medicalEstablishmentOther"].Errors.Clear();

            ModelState["outerRegistrationNo"].Errors.Clear();
            ModelState["outerCertificateNo"].Errors.Clear();
            ModelState["outerCertificateFile"].Errors.Clear();
            ModelState["Registry"].Errors.Clear();

            ModelState["RentalAgreement"].Errors.Clear();
            ModelState["ElectrycityBill"].Errors.Clear();

            ModelState["structuralLyoutFile"].Errors.Clear();

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
            if (model.isInPatient == false)
            {
                ModelState["numberofBed"].Errors.Clear();
            }
            ModelState["disposedNo"].Errors.Clear();

            ModelState["establishmentPlaceOther"].Errors.Clear(); //6Jan2021
            if (model.establishmentPlace.ToLower() != "Rental") //4Jan2021
            {
                ModelState["Registry"].Errors.Clear();
            }

            ViewBag.MedicalEstablishment = objComnDb.GetDropDownList(5, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.MedicalEstablishmentCategory = objComnDb.GetDropDownList(9, 0).ToList();
            ViewBag.MedicalEstablishmentCategoryType = Enumerable.Empty<SelectListItem>();
            ViewBag.ClinicalEstablishment = objComnDb.GetDropDownList(13, 0).ToList();

            ViewBag.ClinicalSubEstablishment = Enumerable.Empty<SelectListItem>();
            ViewBag.State = objComnDb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComnDb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Operate = objComnDb.GetDropDownList(37, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            ViewBag.OutPatient = appDB.NUH_FacilityOfferedForRenewal(1, model.regisIdNUH);
            ViewBag.Laboratory = appDB.NUH_FacilityOfferedForRenewal(2, model.regisIdNUH);
            ViewBag.Imaging = appDB.NUH_FacilityOfferedForRenewal(3, model.regisIdNUH);

            ViewBag.PartnerList = appDB.getNUHPartner(model.regisIdNUH);
            ViewBag.doctorList = appDB.getNUHdoctorList(model.regisIdNUH);
            ViewBag.staffList = appDB.getNUHParamedicalList(model.regisIdNUH);
            ViewBag.CheckListRenewal = objComnDb.GetDropDownList(63, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State1 = objComnDb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District1 = Enumerable.Empty<SelectListItem>();
            model.regByUser = objSM.UserID;
            model.regBytransIp = Common.GetIPAddress();
            model.transIP = Common.GetIPAddress();
            model.requestKey = objSM.AppRequestKey;
            model.isCertificateFromPortal = true;

            if (ModelState.IsValid)
            {
                #region Bulk Insertion Doctor

                var doctorName = form.GetValues("doctorName");
                var doctorFathersName = form.GetValues("doctorFathersName");
                var doctorQualification = form.GetValues("doctorQualification");
                var NameofFoundation = form.GetValues("NameofFoundation");
                var doctorregistrationType = form.GetValues("doctorregistrationType");
                var doctorregistrationNo = form.GetValues("doctorregistrationNo");
                var doctorPart_FullTime = form.GetValues("doctorPart_FullTime");
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
                        XmlDataDoc += "<Doctor><doctorName>" + objAudit.RemoveSpecialCharactors(doctorName[i]) + "</doctorName>"
                             + "<doctorFathersName>" + objAudit.RemoveSpecialCharactors(doctorFathersName[i]) + "</doctorFathersName>"
                              + "<doctorQualification>" + objAudit.RemoveSpecialCharactors(doctorQualification[i]) + "</doctorQualification>"
                              + "<NameofFoundation>" + objAudit.RemoveSpecialCharactors(NameofFoundation[i]) + "</NameofFoundation>"
                              + "<doctorregistrationType>" + doctorregistrationType[i] + "</doctorregistrationType>"
                              + "<doctorregistrationNo>" + objAudit.FilterForAlphabetNumaric(doctorregistrationNo[i]) + "</doctorregistrationNo>"
                              + "<doctorPart_FullTime>" + doctorPart_FullTime[i] + "</doctorPart_FullTime>"
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
                //var staffNameOfMCI_SMF = form.GetValues("staffNameOfMCI_SMF");
                var part_fullTime = form.GetValues("part_fullTime");
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
                        XmlData += "<Staff><staffName>" + objAudit.RemoveSpecialCharactors(staffName[i]) + "</staffName>"
                             + "<stafffatherName>" + objAudit.RemoveSpecialCharactors(stafffatherName[i]) + "</stafffatherName>"
                              + "<staffqualification>" + objAudit.RemoveSpecialCharactors(staffqualification[i]) + "</staffqualification>"
                              + "<staffinstitution>" + objAudit.RemoveSpecialCharactors(staffinstitution[i]) + "</staffinstitution>"
                              + "<staffRegistrationType>" + staffRegistrationType[i] + "</staffRegistrationType>"
                              + "<staffRegistrationNo>" + objAudit.FilterForAlphabetNumaric(staffRegistrationNo[i]) + "</staffRegistrationNo>"
                              + "<part_fullTime>" + part_fullTime[i] + "</part_fullTime>"
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

                #region Bulk Insertion checkList
                var checkListId = form.GetValues("chk_checkList");

                int chk_count = checkListId.Count();
                string XmlDatacheckList = "<CheckList>";


                for (int i = 0; i < chk_count; i++)
                {
                    if (checkListId[i].ToString() == "" || checkListId[i].ToString() == "0")
                    {
                        //XmlDatacheckList = string.Empty;
                    }
                    else
                    {

                        XmlDatacheckList += "<CheckData><checkListId>" + checkListId[i] + "</checkListId></CheckData>";
                    }

                }
                XmlDatacheckList += "</CheckList>";
                model.xmldatacheckList = XmlDatacheckList;
                #endregion

                var res = appDB.NUHInsertUpdateRenewal(model);

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
                                //SendSMS(res.RegistrationNo, res.MobileNo);
                                TempData["RegisIdNUH"] = res.RegisIdNUH;
                                TempData["RegistrationNo"] = res.RegistrationNo;
                                string _regisIdnuh = OTPL_Imp.CustomCryptography.Encrypt(res.RegisIdNUH.ToString());
                                return RedirectToAction("Affidavit", new { isRenewal = true, operatedId = res.operatedId, RegisIdNUH = _regisIdnuh, });
                                //return RedirectToAction("RegistrationConfirmation", "AppRegistration", new { isRenewal = true });
                            }
                            else
                            {
                                //int exeRsult = objNUHDB.DeleteRegistrationNUH(res.RegisIdNUH);///NEED TO ASK
                                setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                ModelState.Clear();
                            }
                        }
                        else
                        {
                            // SendSMS(res.RegistrationNo, res.MobileNo);
                            TempData["RegisIdNUH"] = res.RegisIdNUH;
                            TempData["RegistrationNo"] = res.RegistrationNo;
                            string _regisIdnuh = OTPL_Imp.CustomCryptography.Encrypt(res.RegisIdNUH.ToString());
                            return RedirectToAction("Affidavit", new { isRenewal = true, operatedId = res.operatedId, RegisIdNUH = _regisIdnuh, });
                            //return RedirectToAction("ViewNUHComplete");
                            //return RedirectToAction("RegistrationConfirmation", "AppRegistration", new { isRenewal = true });
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
        #endregion

        #region FAP riya
        [HttpPost]
        public JsonResult CheckMobileExistenceFAP(string strMobileNo)
        {
            long regisId = 0;

            if (Session["regisIdFAP"] != null)
            {
                regisId = Convert.ToInt64(Session["regisIdFAP"]);
            }
            var user = appDB.CheckEmailMobileExistenceFAP(strMobileNo, "M", regisId);
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [AuthorizeAdmin(7)]
        public ActionResult FAPRegistration(string regisId = "", int stepValue = -1, int UpdateStep = 0)
        {
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            FAPModel model = new FAPModel();
            Session["UpdateStep"] = UpdateStep;
            if (regisId != "" && regisId != null && regisId != "0")
            {
                model = appDB.getFAPStep((Convert.ToInt64(regisId)));
                Session["regisIdFAP"] = model.regisIdFAP;

                if (model != null && objSM.UserID != model.regisByuser)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home");
                }
            }
            else
            {
                Session["regisIdFAP"] = null;
            }

            model.claimantMobileNo = model.strMobileNo;

            if (model.step == 1 || model.step == 2)
            {
                ViewBag.chlidrenlist = appDB.getFAPdoctorList(Convert.ToInt64(regisId));
                ViewBag.chlidrenlistCount = appDB.getFAPdoctorList(Convert.ToInt64(regisId)).Count() - 1;
            }

            if (stepValue >= 0)
            {
                model.stepValue = stepValue;
                Session["stepValue"] = model.stepValue;
            }
            else
            {
                Session["stepValue"] = model.step;
                model.stepValue = stepValue;
            }

            int y = model.step;

            ViewBag.Compensation = objComnDb.GetDropDownList(1, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Relation = objComnDb.GetDropDownList(2, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.OperationType = objComnDb.GetDropDownList(4, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Employement = objComnDb.GetDropDownList(3, 0).ToList();
            ViewBag.District = objComnDb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.HealthunitDistrictList = objComnDb.GetDropDownList(7, 34).Where(m => m.Id == objSM.districtId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = objComnDb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.DocumnetCheckList = objComnDb.GetDropDownList(39, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            model.healthunitDistrictId = objSM.districtId;
            return View(model);
        }

        [AuthorizeAdmin(7)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FAPRegistration(FAPModel model, FormCollection form)
        {
            model.healthunitDistrictId = objSM.districtId;
            AuditMethods objAudit = new AuditMethods();
            var result = appDB.getFAPStep(model.regisIdFAP);

            if (result != null && objSM.UserID != result.regisByuser)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            string IpAddress = Common.GetIPAddress();
            long regisByuser = objSM.UserID;
            string XmlData = "";

            if (Session["regisIdFAP"] == null)
            {
                model.step = 0;
            }
            else
            {
                model.regisIdFAP = Convert.ToInt64(Session["regisIdFAP"].ToString());

                int x = model.step;
            }

            model.UpdateStep = Convert.ToInt32(Session["UpdateStep"].ToString());

            if (model.step == 0 || model.UpdateStep == 1)
            {
                int count = 1;
                model.claimantMobileNo = model.strMobileNo;
                ModelState["complicationsDetails"].Errors.Clear();
                ModelState["informationDate"].Errors.Clear();
                ModelState["confirmationDate"].Errors.Clear();
                ModelState["sevakendraName"].Errors.Clear();
                ModelState["sevadoctorName"].Errors.Clear();
                //ModelState["compensationCategoryId"].Errors.Clear();
                ModelState["dateofDeath"].Errors.Clear();
                ModelState["claimantName"].Errors.Clear();
                ModelState["claimantAge"].Errors.Clear();
                ModelState["claimantAddress"].Errors.Clear();
                ModelState["claimantAadhaarNo"].Errors.Clear();
                //ModelState["relationId"].Errors.Clear();
                ModelState["accountHolderName"].Errors.Clear();
                ModelState["bankName"].Errors.Clear();
                ModelState["branchName"].Errors.Clear();
                ModelState["accountNo"].Errors.Clear();
                ModelState["ifscCode"].Errors.Clear();

                //model.claimantMobileNo = model.strMobileNo;
                #region Bulk Insertion
                var childname = form.GetValues("childName");
                var childage = form.GetValues("childAge");
                var gender = form.GetValues("gender");
                var maritalstatus = form.GetValues("maritalStatus");

                if (count != 0 && childname != null && childage != null && childage != null && maritalstatus != null)
                {
                    count = childname.Count();
                    XmlData = "<Children>";
                    //long regisByuser = objSM.UserID;
                    for (int i = 0; i < count; i++)
                    {
                        if (childname[i].ToString() == "" && childage[i] == "" && gender[i] == "")
                        {
                            setSweetAlertMsg("Child Details Are Required", "error");
                            TempData["msg"] = "Child Details Are Required";
                            TempData["msgstatus"] = "error";
                            return RedirectToAction("FAPRegistration", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(Convert.ToString(model.regisIdFAP)), stepValue = Convert.ToInt32(Session["stepValue"]), UpdateStep = 0 });

                        }
                        else
                        {

                            XmlData += "<Child><RegisByUser>" + regisByuser + "</RegisByUser>" +
                                "<ChildName>" + objAudit.RemoveSpecialCharactors(childname[i]) + "</ChildName>"
                                  + "<ChildAge>" + childage[i] + "</ChildAge>"
                                  + "<Gender>" + gender[i] + "</Gender>"
                                   + "<MaritalStatus>" + maritalstatus[i] + "</MaritalStatus>"
                                    + "</Child>";
                        }

                    }
                    XmlData += "</Children>";

                #endregion
                    model.step = 1;
                    var res = appDB.FAPInsertUpdate(model.regisIdFAP, regisByuser, model.sterilizedName, model.sterilizedAge, model.fatherName, model.spouseName, model.spouseAge,
                        model.claimantMobileNo, model.sterilizedGender, model.sterilizedAddress, model.stateId, model.sterlizedDistrictId, model.heathUnitName, model.heathUnitAddress,
                        model.doctorName, model.admittedDate, model.operationDate, model.operationTypeId, model.dateofReleased, model.healthunitDistrictId, IpAddress, XmlData, objSM.AppRequestKey, model.step,
                        model.complicationsDetails, model.informationDate, model.confirmationDate, model.sevakendraName, model.sevadoctorName, model.compensationCategoryId,
                        model.dateofDeath, model.claimAmount, model.claimantName, model.claimantAge, model.claimantAddress, model.claimantAadhaarNo, model.relationId,
                        model.accountHolderName, model.bankName, model.branchName, model.accountNo, model.ifscCode, model.xmldatacheckList, model.UpdateStep);
                    if (res != null)
                    {
                        if (res.Flag == 2 && res.MobileNo != "" && res.RegistrationNo != null && res.MobileNo != "" && res.RegistrationNo != null)
                        {
                            SendSMSforFAP(res.RegistrationNo, res.MobileNo);
                        }
                        setSweetAlertMsg(res.Msg.ToString(), "error");
                        TempData["msg"] = res.Msg.ToString();
                        TempData["msgstatus"] = "success";
                    }
                    return RedirectToAction("FAPRegistration", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(Convert.ToString(res.RegisId)), stepValue = Convert.ToInt32(Session["stepValue"]) + 1, UpdateStep = 0 });
                }
                else
                {
                    setSweetAlertMsg("Staff Details Are Required", "error");
                    TempData["msg"] = "Staff Details Are Required";
                    TempData["msgstatus"] = "error";
                    return RedirectToAction("FAPRegistration", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(Convert.ToString(model.regisIdFAP)), stepValue = Convert.ToInt32(Session["stepValue"]), UpdateStep = 0 });
                }
            }
            else if (model.step == 1 || model.UpdateStep == 2)
            {
                ModelState["sterilizedName"].Errors.Clear();
                ModelState["sterilizedAge"].Errors.Clear();
                ModelState["fatherName"].Errors.Clear();
                ModelState["spouseName"].Errors.Clear();
                ModelState["spouseAge"].Errors.Clear();

                ModelState["sterilizedGender"].Errors.Clear();
                ModelState["sterilizedAddress"].Errors.Clear();
                //ModelState["stateId"].Errors.Clear();
                //ModelState["sterlizedDistrictId"].Errors.Clear();
                ModelState["strMobileNo"].Errors.Clear();

                ModelState["heathUnitName"].Errors.Clear();
                ModelState["heathUnitAddress"].Errors.Clear();
                ModelState["doctorName"].Errors.Clear();
                ModelState["admittedDate"].Errors.Clear();
                ModelState["operationDate"].Errors.Clear();
                //ModelState["operationTypeId"].Errors.Clear();
                //ModelState["dateofReleased "].Errors.Clear();

                if (model.compensationCategoryId != 3)
                {
                    ModelState["claimantMobileNo"].Errors.Clear();
                    ModelState["dateofDeath"].Errors.Clear();
                    ModelState["claimantName"].Errors.Clear();
                    ModelState["claimantAge"].Errors.Clear();
                    ModelState["claimantAddress"].Errors.Clear();
                    ModelState["claimantAadhaarNo"].Errors.Clear();
                }

                decimal clameAmt = 0;
                bool isSucc = objComn.GetClameAmountFAP(model.compensationCategoryId, model.dateofDeath, model.operationDate, out clameAmt);
                if (isSucc && clameAmt > 0)
                {
                    model.claimAmount = clameAmt;
                }
                else
                {
                    setSweetAlertMsg("Invalid Claimed Amount", "warning");
                    return View(model);
                }

                model.step = 2;
                //model.claimantMobileNo = model.strMobileNo;
                var res = appDB.FAPInsertUpdate(model.regisIdFAP, regisByuser, model.sterilizedName, model.sterilizedAge, model.fatherName, model.spouseName, model.spouseAge,
                    model.claimantMobileNo, model.sterilizedGender, model.sterilizedAddress, model.stateId, model.sterlizedDistrictId, model.heathUnitName, model.heathUnitAddress,
                    model.doctorName, model.admittedDate, model.operationDate, model.operationTypeId, model.dateofReleased, model.healthunitDistrictId, IpAddress, XmlData, objSM.AppRequestKey, model.step,
                    model.complicationsDetails, model.informationDate, model.confirmationDate, model.sevakendraName, model.sevadoctorName, model.compensationCategoryId,
                    model.dateofDeath, model.claimAmount, model.claimantName, model.claimantAge, model.claimantAddress, model.claimantAadhaarNo, model.relationId,
                    model.accountHolderName, model.bankName, model.branchName, model.accountNo, model.ifscCode, model.xmldatacheckList, model.UpdateStep);
                if (res != null)
                {
                    setSweetAlertMsg(res.Msg.ToString(), "error");
                    TempData["msg"] = res.Msg.ToString();
                    TempData["msgstatus"] = "success";
                }
                return RedirectToAction("FAPRegistration", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(Convert.ToString(res.RegisId)), stepValue = Convert.ToInt32(Session["stepValue"]) + 1, UpdateStep = 0 });

            }

            else if (model.step == 2)
            {
                model.step = 3;

                #region Bulk Insertion checkList
                var checkListId = form.GetValues("chk_checkList");

                int chk_count = checkListId.Count();
                string XmlDatacheckList = "<CheckList>";


                for (int i = 0; i < chk_count; i++)
                {
                    if (checkListId[i].ToString() == "" || checkListId[i].ToString() == "0")
                    {
                        //XmlDatacheckList = string.Empty;
                    }
                    else
                    {

                        XmlDatacheckList += "<CheckData><checkListId>" + checkListId[i] + "</checkListId></CheckData>";
                    }

                }
                XmlDatacheckList += "</CheckList>";
                model.xmldatacheckList = XmlDatacheckList;
                #endregion

                var res = appDB.FAPInsertUpdate(model.regisIdFAP, regisByuser, model.sterilizedName, model.sterilizedAge, model.fatherName, model.spouseName, model.spouseAge,
                   model.claimantMobileNo, model.sterilizedGender, model.sterilizedAddress, model.stateId, model.sterlizedDistrictId, model.heathUnitName, model.heathUnitAddress,
                   model.doctorName, model.admittedDate, model.operationDate, model.operationTypeId, model.dateofReleased, model.healthunitDistrictId, IpAddress, XmlData, objSM.AppRequestKey, model.step,
                   model.complicationsDetails, model.informationDate, model.confirmationDate, model.sevakendraName, model.sevadoctorName, model.compensationCategoryId,
                   model.dateofDeath, model.claimAmount, model.claimantName, model.claimantAge, model.claimantAddress, model.claimantAadhaarNo, model.relationId,
                   model.accountHolderName, model.bankName, model.branchName, model.accountNo, model.ifscCode, model.xmldatacheckList, model.UpdateStep);

                string applicationNo = "";
                if (res != null)
                {
                    setSweetAlertMsg(res.Msg.ToString(), "error");
                    TempData["msg"] = res.Msg.ToString();
                    TempData["msgstatus"] = "success";
                    applicationNo = res.RegistrationNo;
                }
                return RedirectToAction("ViewFAPComplete", new { applicationNo = applicationNo });
            }
            return View();
        }

        [AuthorizeAdmin(7)]
        public ActionResult ViewFAPComplete(string applicationNo = "")
        {
            FAPModel model = new FAPModel();
            if (applicationNo != "")
                model.FAPList = appDB.getFAPCompleteRegistrationbyAppNo(objSM.UserID, applicationNo);
            else
                model.FAPList = appDB.getFAPCompleteRegistration(objSM.UserID);

            return View(model.FAPList);
        }

        [AuthorizeAdmin(7)]
        [HttpPost]
        public ActionResult UploadAffidavit(int applicationId, HttpPostedFileBase AffidavitFile, string ApplicationNo)
        {

            if (applicationId > 0 && AffidavitFile != null)
            {
                string Dirpath = "~/Content/writereaddata/NUH/";
                string path = "";
                string filename = "";
                if (!Directory.Exists(Server.MapPath(Dirpath)))
                    Directory.CreateDirectory(Server.MapPath(Dirpath));

                string ext = Path.GetExtension(AffidavitFile.FileName);
                if (ext.ToLower() == ".jpg" || ext.ToLower() == ".pdf")
                {

                    filename = Path.GetFileNameWithoutExtension(AffidavitFile.FileName) + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;
                    string completepath = Path.Combine(Server.MapPath(Dirpath), filename);
                    if (System.IO.File.Exists(completepath))
                        System.IO.File.Delete(completepath);

                    long size = AffidavitFile.ContentLength;
                    if (size > 2097152)
                        path = "SNV";//"warning_Maximum 2MB file size are allowed";               
                    else
                    {
                        AffidavitFile.SaveAs(completepath);
                        path = Dirpath + filename;
                    }
                }
                else
                    path = "TNV";//"warning_Invalid File Format only pdf and jpg files are allow!";


                AuditMethods objAud = new AuditMethods();
                if (applicationId > 0 && !string.IsNullOrEmpty(path))
                {
                    string errormsg = String.Empty;
                    bool valStatus = objAud.IsValidLink(path, "Affidavit ", out errormsg);
                    if (!valStatus)
                        TempData["Message"] = errormsg;
                    int resultData = objFAPDB.UploadAffidavitNUH(applicationId, path);
                    if (resultData > 0)

                        TempData["Message"] = "Affidavit Uploaded Successfully.";
                    else
                        TempData["Error"] = "Error in submitting Affidavit !";

                }
                else
                    TempData["Warning"] = "Please upload affidavit !";
            }
            else
                // {
                TempData["Warning"] = "Please upload affidavit !";
            // ApplicationNo = "";
            // }



            return RedirectToAction("ViewFAPComplete", new { applicationNo = ApplicationNo });
        }

        [AuthorizeAdmin(7)]
        [HttpPost]
        public ActionResult ViewFAPComplete(string applicationNo = "", int? SM = 0)
        {
            FAPModel model = new FAPModel();
            model.FAPList = appDB.getFAPCompleteRegistrationbyAppNo(objSM.UserID, applicationNo);
            return View(model.FAPList);
        }

        [AuthorizeAdmin(7)]
        public ActionResult ViewFAPInComplete()
        {
            FAPModel model = new FAPModel();
            model.FAPList = appDB.getFAPInCompleteRegistration(objSM.UserID);
            return View(model.FAPList);
        }

        void SendSMSforFAP(string registrationNo, string mobileNo)
        {

            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";

                //txtmsg = "Dear Citizen,\n\nYour Application form has been submitted successfully. Your Application Form Number is " + registrationNo + ", kindly use this further.\n\n Thanks";
                txtmsg = "Dear Citizen,\n\nYour Application form for Payment of Unsuccessful Family Planning has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\n MHFWD, UP";


                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }

        }
        #endregion

        #region AGC Riya
        [HttpPost]
        public JsonResult CheckMobileExistenceAGC(string applicantMobileNo, string idNumber)
        {
            long regisId = 0;

            if (Session["regisIdAGC"] != null)
            {
                regisId = Convert.ToInt64(Session["regisIdAGC"]);
            }
            var user = appDB.CheckEmailMobileExistenceAGC(applicantMobileNo, "M", regisId, idNumber);
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [AuthorizeAdmin(10)]
        public ActionResult AGCRegistration(string regisId = "", int stepValue = -1, int UpdateStep = 0)
        {

            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            AGCModel model = new AGCModel();
            Session["UpdateStep"] = UpdateStep;
            TempData["UpdateStep"] = UpdateStep;

            if (regisId != "" && regisId != null && regisId != "0")
            {
                model = appDB.getAGCStep((Convert.ToInt64(regisId)));
                Session["regisIdAGC"] = model.regisIdAGC;

                if (model != null && objSM.UserID != model.regByUser)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home");
                }
            }
            else
            {
                Session["regisIdAGC"] = null;
            }

            model.applicantMobileNo = model.mobileNo;

            if (stepValue >= 0)
            {
                model.stepValue = stepValue;
                Session["stepValue"] = model.stepValue;
            }
            else
            {
                Session["stepValue"] = model.step;
                model.stepValue = stepValue;
            }

            if (model.UpdateStep == 1)
            {
                ModelState["documentFile"].Errors.Clear();
                ModelState["orderFile"].Errors.Clear();
            }

            ViewBag.ApplicantType = objComnDb.GetDropDownList(26, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.ApplicantSubType = Enumerable.Empty<SelectListItem>();
            ViewBag.IdType = objComnDb.GetDropDownList(28, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = objComnDb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComnDb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            model.susdistrictId = objSM.districtId;
            ViewBag.DocumnetCheckListAGC = objComnDb.GetDropDownList(41, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            return View(model);
        }

        [AuthorizeAdmin(10)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AGCRegistration(AGCModel model, FormCollection form)
        {
            if (model.regisIdAGC != 0)
            {
                var result = appDB.getAGCStep(model.regisIdAGC);
                if (result != null && objSM.UserID != result.regByUser)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home");
                }
            }
            string IpAddress = Common.GetIPAddress();
            long regisByuser = objSM.UserID;

            //var res = objdb.AGCInsertUpdate(model);

            if (Session["regisIdAGC"] == null)
            {

                model.step = 0;
            }
            else
            {
                model.regisIdAGC = Convert.ToInt64(Session["regisIdAGC"].ToString());

                model.step = Convert.ToInt32(Session["stepValue"].ToString());
            }
            model.UpdateStep = Convert.ToInt32(Session["UpdateStep"].ToString());

            ModelState.Clear();
            model.susdistrictId = objSM.districtId;
            if (model.step == 0 || model.UpdateStep == 1)
            {
                //if (model.UpdateStep == 1)
                //{
                //    ModelState["documentFile"].Errors.Clear();
                //    ModelState["orderFile"].Errors.Clear();
                //}
                model.step = 1;
                model.mobileNo = model.applicantMobileNo;
                var res = appDB.AGCInsertUpdate(model.regisIdAGC, regisByuser, model.applicantTypeId, model.applicantSubTypeId, model.applicantSubTypeOther, model.orderDetail, model.orderNo, model.orderDate,
                    model.fullName, model.gender, model.idNumber, model.idTypeId, model.mobileNo, model.emailId, model.address, model.stateId, model.districtId, model.pinCode,
                    model.susName, model.susFatherName, model.susMotherName, model.susFatherAge, model.susMotherAge, model.susMobileNo, model.susEmail, model.appointmentDate,
                    model.susaddress, model.susstateId, model.susdistrictId, model.suspinCode, model.markOfIdentification, IpAddress, model.step, model.UpdateStep, model.xmldatacheckList);



                if (res != null)
                {
                    if (res.Flag == 2 && res.MobileNo != "" && res.RegistrationNo != null && res.MobileNo != "" && res.RegistrationNo != null)
                    {
                        SendSMSforAGC(res.RegistrationNo, res.MobileNo);
                    }
                    setSweetAlertMsg(res.Msg.ToString(), "error");
                    TempData["msg"] = res.Msg.ToString();
                    TempData["msgstatus"] = "success";
                }
                return RedirectToAction("AGCRegistration", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(Convert.ToString(res.RegisId)), stepValue = Convert.ToInt32(Session["stepValue"]) + 1, UpdateStep = 0 });

            }


            else if (model.step == 1)
            {
                model.step = 2;

                #region Bulk Insertion checkList
                var checkListId = form.GetValues("chk_checkList");

                int chk_count = checkListId.Count();
                string XmlDatacheckList = "<CheckList>";


                for (int i = 0; i < chk_count; i++)
                {
                    if (checkListId[i].ToString() == "" || checkListId[i].ToString() == "0")
                    {
                        //XmlDatacheckList = string.Empty;
                    }
                    else
                    {

                        XmlDatacheckList += "<CheckData><checkListId>" + checkListId[i] + "</checkListId></CheckData>";
                    }

                }
                XmlDatacheckList += "</CheckList>";
                model.xmldatacheckList = XmlDatacheckList;
                #endregion

                var res = appDB.AGCInsertUpdate(model.regisIdAGC, regisByuser, model.applicantTypeId, model.applicantSubTypeId, model.applicantSubTypeOther, model.orderDetail, model.orderNo, model.orderDate,
                     model.fullName, model.gender, model.idNumber, model.idTypeId, model.mobileNo, model.emailId, model.address, model.stateId, model.districtId, model.pinCode,
                     model.susName, model.susFatherName, model.susMotherName, model.susFatherAge, model.susMotherAge, model.susMobileNo, model.susEmail, model.appointmentDate,
                     model.susaddress, model.susstateId, model.susdistrictId, model.suspinCode, model.markOfIdentification, IpAddress, model.step, model.UpdateStep, model.xmldatacheckList);


                if (res != null)
                {
                    setSweetAlertMsg(res.Msg.ToString(), "error");
                    TempData["msg"] = res.Msg.ToString();
                    TempData["msgstatus"] = "success";
                }
                return RedirectToAction("ViewAGCComplete");
            }
            return View();

        }

        [AuthorizeAdmin(10)]
        public ActionResult ViewAGCComplete()
        {
            AGCModel model = new AGCModel();
            model.AGCModelList = appDB.getAGCCompleteRegistration(objSM.UserID);
            return View(model.AGCModelList);
        }

        [AuthorizeAdmin(10)]
        public ActionResult ViewAGCInComplete()
        {
            AGCModel model = new AGCModel();
            model.AGCModelList = appDB.getAGCInCompleteRegistration(objSM.UserID);
            return View(model.AGCModelList);
        }
        public JsonResult getEsubcatList(int applicantTypeId)
        {
            var returndataset = objComnDb.GetDropDownList(27, applicantTypeId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(returndataset, JsonRequestBehavior.AllowGet);
        }

        void SendSMSforAGC(string registrationNo, string mobileNo)
        {

            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";

                //txtmsg = "Dear Citizen,\n\nYour Application form has been submitted successfully. Your Application Form Number is " + registrationNo + ", kindly use this further.\n\n Thanks";
                txtmsg = "Dear Citizen,\n\nYour Application form for Payment of Unsuccessful Family Planning has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\n MHFWD, UP";


                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }

        }
        #endregion

        #region DIC (abhijeet)
        [HttpPost]
        public JsonResult CheckMobileExistenceDIC(string appmobileNo)
        {
            long regisId = 0;
            if (Session["regisIdDIC"] != null)
            {
                regisId = Convert.ToInt64(Session["regisIdDIC"]);
            }

            var user = appDB.CheckEmailMobileExistenceDIC(appmobileNo, "M", regisId);
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [HttpGet]
        [AuthorizeAdmin(4)]
        public ActionResult DICRegistration(string regisId = "", int stepValue = -1)
        {

            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            DICModel model = new DICModel();
            ViewBag.Category = objComnDb.GetDropDownList(8, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = objComnDb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComnDb.GetDropDownList(7, 34).Where(m => m.Id == objSM.districtId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.IdProof = objComnDb.GetDropDownList(35, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.AddressProof = objComnDb.GetDropDownList(36, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            // model.appDisabilityType = objComnDb.GetDropDownList(40, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            //ViewBag.DocumnetCheckListDIC = objComnDb.GetDropDownList(42, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Disability = objComnDb.GetDropDownList(40, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Submit = "Save and Continue";
            if (regisId != "")
            {
                model = appDB.getDICStep((Convert.ToInt64(regisId)));

                if (model != null && objSM.UserID != model.regByUser)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home");
                }

                if (model.ApplyingFor == "2" && model.isCertFromPortal == "No")//need old cert 
                {
                    ViewBag.DocumnetCheckListDIC = objComnDb.GetDropDownList(62, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                }
                else
                {
                    ViewBag.DocumnetCheckListDIC = objComnDb.GetDropDownList(42, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                }

                Session["regisIdDIC"] = regisId;
            }
            else
            {
                ViewBag.DocumnetCheckListDIC = objComnDb.GetDropDownList(42, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                Session["regisIdDIC"] = null;
            }


            if (model.step == 2 && (stepValue == 2 || stepValue == -1))
            {
                ViewBag.Submit = "Submit";


            }
            if (stepValue >= 0)
            {

                model.stepValue = stepValue;
                Session["stepValue"] = model.stepValue;

            }
            else
            {
                Session["stepValue"] = model.step;
                model.stepValue = stepValue;
            }



            return View(model);

        }

        [AuthorizeAdmin(4)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DICRegistration(DICModel model, FormCollection form)
        {
            var result = appDB.getDICStep(model.regisIdDIC);
            if (result != null && objSM.UserID != result.regByUser)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            model.transIp = Common.GetIPAddress();
            model.regByUser = objSM.UserID;
            model.mobileNo = model.appmobileNo;

            ModelState["passportsizephoto"].Errors.Clear();
            ModelState["thumbImpPath"].Errors.Clear();
            ModelState["documentPath"].Errors.Clear();
            ModelState["idProofPath"].Errors.Clear();
            if (model.step == 2 && (model.stepValue == 2 || model.stepValue == -1))
            {
                #region Bulk Insertion checkList
                var checkListId = form.GetValues("chk_checkList");

                int chk_count = checkListId.Count();
                string XmlDatacheckList = "<CheckList>";


                for (int i = 0; i < chk_count; i++)
                {
                    if (checkListId[i].ToString() == "" || checkListId[i].ToString() == "0")
                    {
                        //XmlDatacheckList = string.Empty;
                    }
                    else
                    {

                        XmlDatacheckList += "<CheckData><checkListId>" + checkListId[i] + "</checkListId></CheckData>";
                    }

                }
                XmlDatacheckList += "</CheckList>";
                model.xmldatacheckList = XmlDatacheckList;
                #endregion
            }

            try
            {
                ModelState.Clear();
                model.districtId = objSM.districtId;
                ResultSet res;
                //if (model.ApplyingFor == "2" && model.isCertFromPortal == "Yes" && model.oldCertificateNumber != null)
                if (model.oldCertificateNumber != "" && model.oldCertificateNumber != null)
                {
                    model.regisIdDIC = Convert.ToInt64(Session["regisIdDIC"]);
                    res = appDB.DICInsertUpdateRenewal(model);
                }
                else
                {
                    res = appDB.DICInsertUpdate(model);
                }


                if (res.Flag == 1 && model.step == 2 && res.appStatus == 1)
                {
                    TempData["Message"] = res.Msg.ToString();
                    TempData["msgstatus"] = "success";
                    SendSMSforDIC(res.RegistrationNo, res.MobileNo);
                    return RedirectToAction("ViewDICComplete");
                }
                else if (res.Flag == 1)
                {

                    TempData["Message"] = res.Msg.ToString();
                    TempData["msgstatus"] = "success";
                    return RedirectToAction("DICRegistration", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(Convert.ToString(res.RegisIdDIC)), stepValue = Convert.ToInt32(Session["stepValue"]) + 1 });

                }

            }
            catch
            {
                TempData["Message"] = "Some Error Occur! ";
                TempData["msgstatus"] = "error";

            }

            return View();

        }

        [AuthorizeAdmin(4)]
        public ActionResult ViewDICComplete()
        {
            DICModel model = new DICModel();
            model.DICModelList = appDB.getDICCompleteRegistration(objSM.UserID);
            return View(model.DICModelList);
        }

        [AuthorizeAdmin(4)]
        public ActionResult ViewDICInComplete()
        {
            DICModel model = new DICModel();
            model.DICModelList = appDB.getDICInCompleteRegistration(objSM.UserID);
            return View(model.DICModelList);
        }

        void SendSMSforDIC(string registrationNo, string mobileNo)
        {

            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";


                txtmsg = "Dear Citizen,\n\nYour Application form for Disability Certificate has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\n MHFWD, UP";


                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }

        }

        public JsonResult GetDICdetailByCertNo(string oldCertificateNumber)
        {

            var resultData = appDB.GetDICdetailByCertNo(oldCertificateNumber, objSM.UserID);
            if (resultData != null)
            {
                Session["regisIdDIC"] = resultData.regisIdDIC;
                return Json(resultData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                DICModel model = new DICModel();
                model.regisIdDIC = 0;
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region MER  (abhijeet)
        [HttpPost]
        public JsonResult CheckMobileExistenceMER(string appMobile)
        {
            long regisId = 0;

            if (Session["regisIdMER"] != null)
            {
                regisId = Convert.ToInt64(Session["regisIdMER"]);
            }
            var user = appDB.CheckEmailMobileExistenceMER(appMobile, "M", regisId);
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        public JsonResult Dropdown()
        {
            var res = objComnDb.GetDropDownList(23, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAdmin(8)]
        [HttpGet]
        public ActionResult MERRegistration(string regisId = "", int stepValue = -1)
        {
            TempData["StepValue"] = stepValue;
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);

            MERModel model = new MERModel();
            ViewBag.TreatmentType = objComnDb.GetDropDownList(22, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComnDb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.PostingDistrict = objComnDb.GetDropDownList(7, 34).Where(m => m.Id == objSM.districtId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = objComnDb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.BillType = objComnDb.GetDropDownList(23, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.CheckList = objComnDb.GetDropDownList(43, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State1 = objComnDb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District1 = Enumerable.Empty<SelectListItem>();
            ViewBag.Submit = "Save and Continue";


            if (regisId != "")
            {
                model = appDB.getMERStep((Convert.ToInt64(regisId)));

                if (model != null && objSM.UserID != model.regByUser)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home");
                }

                Session["regisIdMER"] = model.regisIdMER;

                TempData["permDistrictId"] = model.permDistrictId;
            }
            else
            {
                Session["regisIdMER"] = null;
            }

            if (model.step == 2 && (stepValue == 2 || stepValue == -1))
            {
                ViewBag.Submit = "Submit";
            }
            else if (model.step == 2 && stepValue == 1)
            {
                ViewBag.MERChildList = appDB.getMERChild(Convert.ToInt64(regisId));
                model.MERModelList = appDB.getMERChild(Convert.ToInt64(regisId));
                ViewBag.MERChildListCount = ViewBag.MERChildList.Count - 1;
            }

            if (stepValue >= 0)
            {
                model.stepValue = stepValue;
                Session["stepValue"] = model.stepValue;
            }
            else
            {
                Session["stepValue"] = model.step;
                model.stepValue = stepValue;
            }

            model.ROLL = Session["RollID"].ToString();
            return View(model);
        }

        [AuthorizeAdmin(8)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MERRegistration(MERModel model, FormCollection form)
        {
            AuditMethods objAud = new AuditMethods();
            var result = appDB.getMERStep(model.regisIdMER);
            if (result != null && objSM.UserID != result.regByUser)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            string XmlData = "";
            if (model.patientType != null && model.patientType != "")
            {
                if (model.patientType == "Self")
                {
                    model.patientgender = model.gender;
                }
            }
            if (model.step == 1 && (model.stepValue == 1 || model.stepValue == -1))
            {
                #region Bulk Insertion
                var billId = form.GetValues("billId");
                var billNo = form.GetValues("billNo");
                var billdate = form.GetValues("billdate");
                var billAmount = form.GetValues("billAmount");
                XmlData = "<Bills>";


                for (int i = 0; i < billId.Count(); i++)
                {
                    if (billId[i].ToString() == "" && billNo[i] == "" && billdate[i] == "" && billAmount[i] == "")
                    {
                        //XmlData = string.Empty;
                    }
                    else
                    {

                        XmlData += "<Bill><BillId>" + billId[i] + "</BillId>"
                              + "<BillNo>" + objAud.FilterForAlphabetNumaric(billNo[i]) + "</BillNo>"
                              + "<BillDate>" + billdate[i] + "</BillDate>"
                               + "<BillAmount>" + billAmount[i] + "</BillAmount></Bill>";
                    }
                }
                XmlData += "</Bills>";

                #endregion
            }
            else if (model.step == 2 && model.stepValue == 1)
            {
                #region Bulk Insertion
                var billId = form.GetValues("billId");
                var billNo = form.GetValues("billNo");
                var billdate = form.GetValues("billdate");
                var billAmount = form.GetValues("billAmount");
                XmlData = "<Bills>";


                for (int i = 0; i < billId.Count(); i++)
                {
                    if (billId[i].ToString() == "" && billNo[i] == "" && billdate[i] == "" && billAmount[i] == "")
                    {
                        //XmlData = string.Empty;
                    }
                    else
                    {

                        XmlData += "<Bill><BillId>" + billId[i] + "</BillId>"
                              + "<BillNo>" + objAud.FilterForAlphabetNumaric(billNo[i]) + "</BillNo>"
                              + "<BillDate>" + billdate[i] + "</BillDate>"
                               + "<BillAmount>" + billAmount[i] + "</BillAmount></Bill>";
                    }
                }
                XmlData += "</Bills>";

                #endregion
            }
            else if (model.step == 2 && (model.stepValue == 2 || model.stepValue == -1))
            {
                #region Bulk Insertion checkList
                var checkListId = form.GetValues("chk_checkList");

                int chk_count = checkListId.Count();
                XmlData = "<CheckList>";


                for (int i = 0; i < chk_count; i++)
                {
                    if (checkListId[i].ToString() == "" || checkListId[i].ToString() == "0")
                    {
                        //XmlDatacheckList = string.Empty;
                    }
                    else
                    {

                        XmlData += "<CheckData><checkListId>" + checkListId[i] + "</checkListId></CheckData>";
                    }

                }
                XmlData += "</CheckList>";

                #endregion
            }

            long _regisId = model.regisIdMER;
            model.transIP = Common.GetIPAddress();
            model.regisByuser = objSM.UserID;
            model.mobileNo = model.appMobile;
            model.xmlData = XmlData;

            try
            {
                ModelState.Clear();
                model.postingDistrictId = objSM.districtId;
                var res = appDB.Insert_UpdateMER(model, Session["RollID"].ToString());
                if (res.Msg != "Total Bill Amount can not be less than Rs.50000")
                {
                    if (res.RegisId != 0)
                    {
                        _regisId = res.RegisId;
                    }
                    TempData["Message"] = res.Msg;
                    if (model.step == 2 && res.appStatus == 1)
                    {
                        SendSMSforMER(res.RegistrationNo, res.MobileNo);
                        return RedirectToAction("ViewMERComplete");
                    }
                    else if (res.Flag == 1)
                    {
                        return RedirectToAction("MERRegistration", new { regisId = OTPL_Imp.CustomCryptography.Encrypt(_regisId.ToString()), stepValue = Convert.ToInt32(Session["stepValue"]) + 1 });
                    }
                }
                else
                {
                    TempData["MessageError"] = res.Msg;
                    return RedirectToAction("MERRegistration", new { regisId = OTPL_Imp.CustomCryptography.Encrypt(_regisId.ToString()), stepValue = 1 });
                }
            }
            catch
            {
                TempData["MessageError"] = "Some Error Occur";
            }

            return View();
        }

        public JsonResult binddist(int permStateId)
        {
            var res = objComnDb.GetDropDownList(7, permStateId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAdmin(8)]
        public ActionResult ViewMERComplete()
        {
            MERModel model = new MERModel();
            model.MERModelList = appDB.getMERCompleteRegistration(objSM.UserID, Session["RollID"].ToString());
            return View(model.MERModelList);
        }

        [AuthorizeAdmin(8)]
        public ActionResult ViewMERInComplete()
        {
            MERModel model = new MERModel();
            model.MERModelList = appDB.getMERInCompleteRegistration(objSM.UserID, Session["RollID"].ToString());
            return View(model.MERModelList);
        }

        void SendSMSforMER(string registrationNo, string mobileNo)
        {

            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";


                txtmsg = "Dear Citizen,\n\nYour Application form for Medical Reimbursement  has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\n MHFWD, UP";


                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }

        }
        #endregion



        #region CHC

        #region ILC riya
        [HttpPost]
        public JsonResult CheckMobileExistenceILC(string appmobileNo)
        {
            long regisId = 0;

            if (Session["regisIdILC"] != null)
            {
                regisId = Convert.ToInt64(Session["regisIdILC"]);
            }
            var user = appDB.CheckEmailMobileExistenceILC(appmobileNo, "M", regisId);
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [AuthorizeAdmin(2)]
        [HttpGet]
        public ActionResult ILCRegistration(string regisId = "", int stepValue = -1, string AppType = "")
        {

            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            AppType = OTPL_Imp.CustomCryptography.Decrypt(AppType);
            ILCModel model = new ILCModel();
            ViewBag.UPDistrict = objComnDb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = objComnDb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = Enumerable.Empty<SelectListItem>();
            ViewBag.InstitutionType = objComnDb.GetDropDownList(29, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Category = objComnDb.GetDropDownList(8, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.CheckListILC = appDB.GetCheckList_ILC().Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.IdType = objComnDb.GetDropDownList(46, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            if (regisId != "" && AppType == "")
            {
                model = appDB.getILCStep((Convert.ToInt64(regisId)));

                if (model != null && objSM.UserID != model.regByUser)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home");
                }

                TempData["distILC"] = model.opdDistrictId;
                Session["regisIdILC"] = model.regisIdILC;
            }
            else if (regisId != "" && AppType != "")
            {
                model = appDB.getILCForExtendDateCHC((Convert.ToInt64(regisId)));
                //TempData["dist"] = model.opdDistrictId;
                TempData["opdDate"] = model.opdDate;
                Session["regisIdILC"] = model.regisIdILC;

            }
            else
            {
                Session["regisIdILC"] = null;
            }
            ViewBag.Submit = "Save and Continue";
            if (stepValue >= 0)
            {
                model.stepValue = stepValue;
                Session["stepValue"] = model.stepValue;
            }
            else
            {
                Session["stepValue"] = model.step;
                model.stepValue = stepValue;
            }
            if (model.step == 2 && (stepValue == 2 || stepValue == -1))
            {
                ViewBag.Submit = "Submit";
            }
            return View(model);
        }

        [AuthorizeAdmin(2)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ILCRegistration(ILCModel model, FormCollection form)
        {
            var result = appDB.getILCStep(model.regisIdILC);
            if (result != null && objSM.UserID != result.regByUser)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            long _regisId = model.regisIdILC;
            model.transIp = Common.GetIPAddress();
            model.regByUser = objSM.UserID;
            model.mobileNo = model.appmobileNo;
            model.forwardtoId = objSM.UserID;
            model.forwardtypeId = objSM.RollID;
            model.healthUnitDistrictId = objSM.districtId;

            if (model.Extstep == 0)
            {
                if (model.step == 2 && (model.stepValue == 2 || model.stepValue == -1))
                {
                    #region Bulk Insertion checkList
                    var checkListId = form.GetValues("chk_checkList");

                    int chk_count = checkListId.Count();
                    string XmlDatacheckList = "<CheckList>";

                    for (int i = 0; i < chk_count; i++)
                    {
                        if (checkListId[i].ToString() == "" || checkListId[i].ToString() == "0")
                        {
                            //XmlDatacheckList = string.Empty;
                        }
                        else
                        {

                            XmlDatacheckList += "<CheckData><checkListId>" + checkListId[i] + "</checkListId></CheckData>";
                        }

                    }
                    XmlDatacheckList += "</CheckList>";
                    model.XmlCheckList = XmlDatacheckList;
                    #endregion
                }

                try
                {
                    var res = appDB.Insert_UpdateILC(model);
                    if (res != null && res.Flag > 0)
                    {
                        TempData["Message"] = res.Msg.ToUpper().ToString();
                        TempData["status"] = "success";
                    }
                    if (res.RegisId != 0)
                    {
                        _regisId = res.RegisId;
                    }

                    if (model.step == 2 && res.appStatus == 1)
                    {
                        //SendSMSforILC(res.RegistrationNo, res.MobileNo);
                        return RedirectToAction("ViewILCComplete");
                    }
                    else if (res.Flag == 1)
                    {
                        return RedirectToAction("ILCRegistration", new { regisId = OTPL_Imp.CustomCryptography.Encrypt(_regisId.ToString()), stepValue = Convert.ToInt32(Session["stepValue"]) + 1, AppType = "" });
                    }
                }
                catch
                {
                    TempData["Message"] = "Some Error Occur";
                    TempData["status"] = "error";
                }
                ///////////////
            }
            else
            {
                if (model.Extstep == 1)
                {
                    ModelState["extOpdFile"].Errors.Clear();
                    if (model.isNewOPDFile == true && model.extPhotoId == true)
                    {

                        model.extOpdFilePath = "True";
                    }
                    else
                    {
                        model.extOpdFilePath = "NaN";
                    }
                    var res = appDB.Insert_UpdateExtDateILC(model.regisIdILC, model.regByUser, model.forwardtypeId, model.forwardtoId,
      model.healthUnitDistrictId, model.reason, model.idNumber, model.extOpdReceiptno, model.extInspectedDate, model.extOpdFilePath, model.extDoctorName, model.extTreatmentFrom,
      model.extTreatmentto, model.extNoOfDays, model.transIp, model.Extstep, model.appmobileNo);

                    if (res != null && res.Flag == 1)
                    {
                        TempData["Message"] = res.Msg.ToUpper().ToString();
                        TempData["status"] = "success";
                        long regisId = res.RegisId;
                        return RedirectToAction("ViewILCComplete");
                    }

                }
            }
            return View();
        }

        public JsonResult binddistILC(int opdStateId)
        {
            var res = objComnDb.GetDropDownList(7, opdStateId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAdmin(2)]
        public ActionResult ViewILCComplete()
        {
            ILCModel model = new ILCModel();
            // model.ILCModelList = appDB.GetComplete_ILC(objSM.UserID);
            return View();
        }

        public ActionResult ViewILCListComplete(string certificateno = "")//list of All Received Application
        {
            List<ILCModel> lstILCDetails = new List<ILCModel>();

            lstILCDetails = appDB.GetComplete_ILC(objSM.UserID, certificateno);

            return PartialView("_ViewILCListComplete", lstILCDetails);
        }

        [AuthorizeAdmin(2)]
        public ActionResult ViewILCInComplete()
        {
            ILCModel model = new ILCModel();
            model.ILCModelList = appDB.GetInComplete_ILC(objSM.UserID);
            return View(model.ILCModelList);
        }

        void SendSMSforILC(string registrationNo, string mobileNo)
        {
            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";


                txtmsg = "Dear Citizen,\n\nYour Application form for Illness Certificate   has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\n MHFWD, UP";


                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }
        }
        #endregion

        #region DEC riya
        [HttpPost]
        public JsonResult CheckMobileExistenceDEC(string mobileNo)
        {
            long regisId = 0;

            if (Session["regisIdDEC"] != null)
            {
                regisId = Convert.ToInt64(Session["regisIdDEC"]);
            }
            var user = appDB.CheckEmailMobileExistenceDEC(mobileNo, "M", regisId);
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [AuthorizeAdmin(6)]
        [HttpGet]
        public ActionResult DECRegistration(string regisId = "", int stepValue = -1, string AppType = "")
        {

            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);

            DECModel model = new DECModel();
            ViewBag.maritalStatus = objComnDb.GetDropDownList(19, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Religion = objComnDb.GetDropDownList(20, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = objComnDb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = Enumerable.Empty<SelectListItem>();
            ViewBag.Relation = objComnDb.GetDropDownList(56, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            if (regisId != "" && AppType == "")
            {
                model = appDB.getDECStep((Convert.ToInt64(regisId)));

                if (model != null && objSM.UserID != model.regByuser)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home");
                }

                if (model.step == 1)
                {
                    TempData["distval"] = "1";
                    TempData["dist"] = model.districtid;
                    TempData["State"] = model.stateId;
                    TempData["pinCode"] = model.pinCode;
                }
                Session["regisIdDEC"] = model.regisIdDEC;
            }

            else
            {
                Session["regisIdDEC"] = null;
            }

            if (stepValue >= 0)
            {
                model.stepValue = stepValue;
                Session["stepValue"] = model.stepValue;
            }
            else
            {
                Session["stepValue"] = model.step;
                model.stepValue = stepValue;
            }

            return View(model);
        }

        [AuthorizeAdmin(6)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DECRegistration(DECModel model, FormCollection form)
        {
            var result = appDB.getDECStep(model.regisIdDEC);

            if (result != null && objSM.UserID != result.regByuser)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            long _regisId = model.regisIdDEC;
            model.transIp = Common.GetIPAddress();
            model.regByusers = objSM.UserID;
            model.forwardtypeId = objSM.RollID;
            model.healthUnitDistrictId = objSM.districtId;
            model.forwardtoId = objSM.UserID;

            if (model.addressType == "Same")
            {
                model.deathPersonStateId = model.stateId;
                model.deathPersonDistrictId = model.districtid;
            }

            try
            {
                var res = appDB.Insert_UpdateDEC(model);
                if (res != null && res.Flag > 0)
                {
                    TempData["Message"] = res.Msg.ToUpper().ToString();
                    TempData["status"] = "success";
                }
                if (res.RegisId != 0)
                {
                    _regisId = res.RegisId;
                }

                if (res.Flag == 2)
                {
                    //SendSMSforILC(res.RegistrationNo, res.MobileNo);
                    return RedirectToAction("ViewDECComplete");
                }
                else if (res.Flag == 1)
                {
                    return RedirectToAction("DECRegistration", new { regisId = OTPL_Imp.CustomCryptography.Encrypt(_regisId.ToString()), stepValue = Convert.ToInt32(Session["stepValue"]) + 1, AppType = "" });
                }
            }
            catch
            {
                TempData["Message"] = "Some Error Occur";
                TempData["status"] = "error";
            }

            return View();
        }

        [AuthorizeAdmin(6)]
        public ActionResult ViewDECComplete()
        {
            DECModel model = new DECModel();
            model.DECModelList = appDB.GetComplete_DEC(objSM.UserID);
            return View(model.DECModelList);
        }

        [AuthorizeAdmin(6)]
        public ActionResult ViewDECInComplete()
        {
            DECModel model = new DECModel();
            model.DECModelList = appDB.GetInComplete_DEC(objSM.UserID);
            return View(model.DECModelList);
        }

        public JsonResult BindDistrictlistDEC(int stateId)
        {
            var res = objComnDb.GetDropDownList(7, stateId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        void SendSMSforDEC(string registrationNo, string mobileNo)
        {
            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";


                txtmsg = "Dear Citizen,\n\nYour Application form for Death Certificate   has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\n MHFWD, UP";


                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }
        }
        #endregion

        #region FIC

        [HttpPost]
        public JsonResult CheckMobileExistenceFIC(string appmobileNo)
        {
            long regisId = 0;

            if (Session["regisIdFIC"] != null)
            {
                regisId = Convert.ToInt64(Session["regisIdFIC"]);
            }
            var user = appDB.CheckEmailMobileExistenceFIC(appmobileNo, "M", regisId);
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [AuthorizeAdmin(3)]
        [HttpGet]
        public ActionResult FICRegistration(string regisId = "", int stepValue = -1)
        {
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            FICModel model = new FICModel();
            ViewBag.State = objComnDb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = Enumerable.Empty<SelectListItem>();
            ViewBag.Category = objComnDb.GetDropDownList(8, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.InstitutionType = objComnDb.GetDropDownList(29, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.CheckListILC = objComnDb.GetDropDownList(45, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.IdType = objComnDb.GetDropDownList(46, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            if (regisId != "")
            {
                model = appDB.GetRegistration_FIC((Convert.ToInt64(regisId)));

                if (model != null && objSM.UserID != model.regByUser)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home");
                }

                TempData["distFIC"] = model.opdDistrictId;
                Session["regisIdFIC"] = model.regisIdFIC;
            }
            else
            {
                Session["regisIdFIC"] = null;
            }

            ViewBag.Submit = "Save and Continue";

            if (stepValue >= 0)
            {
                model.stepValue = stepValue;
                Session["stepValue"] = model.stepValue;
            }
            else
            {
                Session["stepValue"] = model.step;
                model.stepValue = stepValue;
            }

            if (model.step == 2 && (model.stepValue == -1 || model.stepValue == 2))
            {
                ViewBag.Submit = "Submit";
            }

            return View(model);
        }

        [AuthorizeAdmin(3)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FICRegistration(FICModel model, FormCollection form)
        {
            var result = appDB.GetRegistration_FIC(model.regisIdFIC);

            if (result != null && objSM.UserID != result.regByUser)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            long _regisId = model.regisIdFIC;
            model.transIp = Common.GetIPAddress();
            model.regByUser = objSM.UserID;
            model.mobileNo = model.appmobileNo;
            model.forwardtoId = objSM.UserID;
            model.forwardtypeId = objSM.RollID;
            model.healthUnitDistrictId = objSM.districtId;
            model.mobileNo = model.appmobileNo;
            if (model.step == 1)
            {
                ModelState["markOfIdentification"].Errors.Clear();
            }
            if (model.step == 2 && (model.stepValue == 2 || model.stepValue == -1))
            {
                #region Bulk Insertion checkList
                var checkListId = form.GetValues("chk_checkList");

                int chk_count = checkListId.Count();
                string XmlDatacheckList = "<CheckList>";

                for (int i = 0; i < chk_count; i++)
                {
                    if (checkListId[i].ToString() == "" || checkListId[i].ToString() == "0")
                    {
                        //XmlDatacheckList = string.Empty;
                    }
                    else
                    {

                        XmlDatacheckList += "<CheckData><checkListId>" + checkListId[i] + "</checkListId></CheckData>";
                    }

                }
                XmlDatacheckList += "</CheckList>";
                model.XmlCheckList = XmlDatacheckList;
                #endregion
            }

            try
            {
                var res = appDB.Insert_UpdateFIC(model);
                if (res != null && res.Flag > 0)
                {
                    TempData["Message"] = res.Msg.ToUpper().ToString();
                    TempData["status"] = "success";
                }
                if (res.RegisId != 0)
                {
                    _regisId = res.RegisId;
                }

                if ((model.step == 2 || model.step == 1) && res.appStatus == 1)
                {
                    SendSMSforFIC(res.RegistrationNo, res.MobileNo);
                    return RedirectToAction("ViewFICComplete");
                }
                else if (res.Flag == 1)
                {
                    return RedirectToAction("FICRegistration", new { regisId = OTPL_Imp.CustomCryptography.Encrypt(_regisId.ToString()), stepValue = Convert.ToInt32(Session["stepValue"]) + 1 });
                }
            }
            catch
            {
                TempData["Message"] = "An Error Occurred!";
                TempData["status"] = "error";
            }

            return View();
        }

        public JsonResult binddistFIC(int opdStateId)
        {
            var res = objComnDb.GetDropDownList(7, opdStateId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAdmin(3)]
        public ActionResult ViewFICComplete()
        {
            FICModel model = new FICModel();
            model.FICModelList = appDB.GetComplete_FIC(objSM.UserID);
            return View(model.FICModelList);
        }

        [AuthorizeAdmin(3)]
        public ActionResult ViewFICInComplete()
        {
            FICModel model = new FICModel();
            model.FICModelList = appDB.GetInComplete_FIC(objSM.UserID);
            return View(model.FICModelList);
        }

        void SendSMSforFIC(string registrationNo, string mobileNo)
        {
            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";


                txtmsg = "Dear Citizen,\n\nYour Application form for Fitness Certificate has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\n MHFWD, UP";


                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }
        }
        #endregion

        #region ILC to FIC
        [AuthorizeAdmin(3)]
        [HttpGet]
        public ActionResult ILCtoFICRegistration(string regisId = "", int stepValue = -1)
        {
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            FICModel model = new FICModel();
            ViewBag.State = objComnDb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = Enumerable.Empty<SelectListItem>();
            ViewBag.Category = objComnDb.GetDropDownList(8, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.InstitutionType = objComnDb.GetDropDownList(29, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.CheckListILC = objComnDb.GetDropDownList(45, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.IdType = objComnDb.GetDropDownList(46, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            if (regisId != "")
            {
                model.step = 3;
                //model = appDB.GetRegistration_FIC((Convert.ToInt64(regisId)));
                Session["regisIdFIC"] = regisId;
                model.regisIdFIC = Convert.ToInt64(regisId);
            }
            else
            {
                model.step = 2;
                //Session["regisIdFIC"] = null;
            }
            if (stepValue == 1)
            {
                model = appDB.GetRegistration_FIC((Convert.ToInt64(regisId)));

                if (model != null && objSM.UserID != model.regByUser)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home");
                }

                TempData["distILCtoFIC"] = model.opdDistrictId;
                TempData["stepValue"] = "1";
                TempData["gender"] = model.gender;

            }
            model.stepValue = stepValue;
            //ViewBag.Submit = "Save and Continue";

            //if (stepValue >= 0)
            //{
            //    model.stepValue = stepValue;
            //    Session["stepValue"] = model.stepValue;
            //}
            //else
            //{
            //    Session["stepValue"] = model.step;
            //    model.stepValue = stepValue;
            //}

            //if (model.step == 2 && (model.stepValue == -1 || model.stepValue == 2))
            //{
            //    ViewBag.Submit = "Submit";
            //}

            return View(model);
        }

        [AuthorizeAdmin(3)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ILCtoFICRegistration(FICModel model, FormCollection form)
        {
            var result = appDB.GetRegistration_FIC(model.regisIdFIC);

            if (result != null && objSM.UserID != result.regByUser)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            ModelState["isMedicalHistory"].Errors.Clear();
            ModelState["idFile"].Errors.Clear();
            long _regisId = model.regisIdFIC;
            model.transIp = Common.GetIPAddress();
            model.regByUser = objSM.UserID;
            model.mobileNo = model.appmobileNo;
            model.forwardtoId = objSM.UserID;
            model.forwardtypeId = objSM.RollID;
            model.healthUnitDistrictId = objSM.districtId;

            if (model.step == 3)
            {
                #region Bulk Insertion checkList
                var checkListId = form.GetValues("chk_checkList");

                int chk_count = checkListId.Count();
                string XmlDatacheckList = "<CheckList>";

                for (int i = 0; i < chk_count; i++)
                {
                    if (checkListId[i].ToString() == "" || checkListId[i].ToString() == "0")
                    {
                        //XmlDatacheckList = string.Empty;
                    }
                    else
                    {

                        XmlDatacheckList += "<CheckData><checkListId>" + checkListId[i] + "</checkListId></CheckData>";
                    }

                }
                XmlDatacheckList += "</CheckList>";
                model.XmlCheckList = XmlDatacheckList;
                #endregion
            }

            try
            {
                var res = appDB.Insert_UpdateFICextended(model);
                if (res != null && res.Flag > 0)
                {
                    TempData["Message"] = res.Msg.ToUpper().ToString();
                    TempData["status"] = "success";
                }
                if (res.RegisId != 0)
                {
                    _regisId = res.RegisId;
                }

                if (model.step == 3 && res.Flag == 2)
                {
                    SendSMSforFIC(res.RegistrationNo, res.MobileNo);
                    return RedirectToAction("ViewFICComplete");
                }
                else if (res.Flag == 1)
                {
                    return RedirectToAction("ILCtoFICRegistration", new { regisId = OTPL_Imp.CustomCryptography.Encrypt(_regisId.ToString()), stepValue = -1 });
                }
            }
            catch
            {
                TempData["Message"] = "An Error Occurred!";
                TempData["status"] = "error";
            }

            return View();
        }

        public JsonResult binddistILCtoFIC(int opdStateId)
        {
            var res = objComnDb.GetDropDownList(7, opdStateId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLatestILCDetailByCrtificateNo(string oldCertificateNumber)
        {
            var resultData = objFICdb.GetLatestILCDetailByCrtificateNo(oldCertificateNumber, objSM.UserID);

            Session["regisIdILC"] = resultData.regisIdILC;
            TempData["distILCtoFIC"] = resultData.opdDistrictId;
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
            //        model.Msg = Convert.ToInt32(msg);
            //        //model.regisIdILC = Convert.ToInt32(msg);
            //        return Json(model, JsonRequestBehavior.AllowGet);
            //    }
            //}
            //else
            //{
            //    FICModel model = new FICModel();
            //    //model.regisIdILC = 0;
            //    model.Msg = 0;
            //    return Json(model, JsonRequestBehavior.AllowGet);
            //}
        }
        #endregion

        #region  IMC
        [HttpPost]
        public JsonResult CheckMobileExistenceIMC(string appmobileNo)
        {
            long regisId = 0;

            if (Session["regisIdIMC"] != null)
            {
                regisId = Convert.ToInt64(Session["regisIdIMC"]);
            }
            var user = appDB.CheckEmailMobileExistenceIMC(appmobileNo, "M", regisId);
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [AuthorizeAdmin(5)]
        [HttpGet]
        public ActionResult IMCRegistration(string regisId = "", int stepValue = -1, int UpdateStep = 0)
        {
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            IMCModel model = new IMCModel();
            ViewBag.State = objComnDb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = Enumerable.Empty<SelectListItem>();
            ViewBag.Category = objComnDb.GetDropDownList(8, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.InstitutionType = objComnDb.GetDropDownList(29, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            //ViewBag.CheckListILC = objComnDb.GetDropDownList(45, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            // ViewBag.CheckListIMC= objComnDb.GetDropDownList(55, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.ImmunizationType = objComnDb.GetDropDownList(54, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            // model.appImmunList = appDB.BindImmunizationDetailsIMC();
            Session["UpdateStep"] = UpdateStep;
            TempData["UpdateStep"] = UpdateStep;

            if (regisId != "" && regisId != null && regisId != "0")
            {
                ViewBag.ImmunList = appDB.getIMCImmuCHC(Convert.ToInt64(regisId));

            }
            if (regisId != "")
            {
                model = appDB.GetRegistration_IMC((Convert.ToInt32(regisId)));

                if (model != null && objSM.UserID != model.regByUser)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home");
                }

                ViewBag.CheckListIMC = objComnDb.GetDropDownList(55, (Convert.ToInt32(regisId)));//.Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString(), Selected = m.isRequiredData });
                if (model.step == 2)
                {
                    TempData["showtb"] = "shw";
                }
                TempData["dist"] = model.districtId;
                Session["regisIdIMC"] = model.regisIdIMC;
            }
            else
            {
                Session["regisIdIMC"] = null;
            }

            ViewBag.Submit = "Save and Continue";

            if (stepValue >= 0)
            {
                model.stepValue = stepValue;
                Session["stepValue"] = model.stepValue;
            }
            else
            {
                Session["stepValue"] = model.step;
                model.stepValue = stepValue;
            }

            if (model.step == 1 && (model.stepValue == -1 || model.stepValue == 1))
            {
                ViewBag.Submit = "Submit";
            }

            return View(model);
        }

        [AuthorizeAdmin(5)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IMCRegistration(IMCModel model, FormCollection frm)
        {
            AuditMethods objAudit = new AuditMethods();
            var result = appDB.GetRegistration_IMC(model.regisIdIMC);

            if (result != null && objSM.UserID != result.regByUser)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            string xmldata = "";
            long _regisId = model.regisIdIMC;
            model.transIp = Common.GetIPAddress();
            model.regisByuser = objSM.UserID;
            model.mobileNo = model.appmobileNo;
            model.forwardtoId = objSM.UserID;
            model.forwardtypeId = objSM.RollID;
            model.forwardDistrictId = objSM.districtId;

            if (Session["regisIdIMC"] == null)
            {

                model.step = 0;
            }
            else
            {
                model.regisIdIMC = Convert.ToInt64(Session["regisIdIMC"].ToString());

                model.step = Convert.ToInt32(Session["stepValue"].ToString());
            }
            model.UpdateStep = Convert.ToInt32(Session["UpdateStep"].ToString());


            if (model.step == 0 || model.UpdateStep == 1)
            {

                model.step = 1;
                var res = appDB.Insert_UpdateIMC(model);



                if (res != null)
                {
                    setSweetAlertMsg(res.Msg.ToString(), "error");
                    TempData["msg"] = res.Msg.ToString();
                    TempData["msgstatus"] = "success";
                }

                return RedirectToAction("IMCRegistration", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(Convert.ToString(res.RegisId)), stepValue = Convert.ToInt32(Session["stepValue"]) + 1, UpdateStep = 0 });
            }
            else if (model.step == 1)
            {
                model.step = 2;
                #region bulkinsertion
                xmldata = "<vaccines>";

                var vaccineId = frm.GetValues("addimmunId");
                var ImmunizationDate = frm.GetValues("adddateOfImmunization");
                var doctorName = frm.GetValues("addimmunizationDoctor");

                int count = ImmunizationDate.Count();

                if (count == 0)
                {
                    TempData["msg"] = "Please Select Atleast One Immunization Detail ";
                    TempData["msgstatus"] = "warning";
                    return RedirectToAction("IMCRegistration");
                }
                for (int i = 0; i < count; i++)
                {
                    if (ImmunizationDate[i] != "")
                    {
                        xmldata += "<vaccine><vaccineId>" + vaccineId[i] + "</vaccineId>"
                         + "<ImmunizationDate>" + ImmunizationDate[i] + "</ImmunizationDate>"
                         + "<doctorName>" + objAudit.RemoveSpecialCharactors(doctorName[i]) + "</doctorName>"

                           + "</vaccine>";
                    }
                    else
                    {

                        TempData["msg"] = "Immunization Date Required!";
                        TempData["msgstatus"] = "warning";
                        return RedirectToAction("IMCRegistration");
                    }


                }


                xmldata += "</vaccines>";
                model.xmldata = xmldata;

                #endregion
                var res = appDB.Insert_UpdateIMC(model);
                if (res != null)
                {
                    if (res.Flag == 2 && res.MobileNo != "" && res.RegistrationNo != null && res.MobileNo != "" && res.RegistrationNo != null)
                    {
                        // SendSMSforIMC(res.RegistrationNo, res.MobileNo);
                    }

                }

                return RedirectToAction("IMCRegistration", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(Convert.ToString(res.RegisId)), stepValue = Convert.ToInt32(Session["stepValue"]) + 1, UpdateStep = 0 });
            }
            else if (model.step == 2)
            {
                model.step = 3;
                #region Bulk Insertion checkList
                var checkListId = frm.GetValues("chk_checkList");
                var ChkType = frm.GetValues("chk_Type");
                int chk_count = checkListId.Count();
                string XmlDataChecklist = "<CheckList>";

                for (int i = 0; i < chk_count; i++)
                {
                    if (checkListId[i].ToString() == "" || checkListId[i].ToString() == "0")
                    {
                        //XmlDatacheckList = string.Empty;
                    }
                    else
                    {

                        XmlDataChecklist += "<CheckData><checkListId>" + checkListId[i] + "</checkListId>"
                             + "<ChkType>" + ChkType[i] + "</ChkType>" + "</CheckData>";
                    }

                }
                XmlDataChecklist += "</CheckList>";
                model.XmlDataChecklist = XmlDataChecklist;
                #endregion


                var res = appDB.Insert_UpdateIMC(model);
                if (res != null)
                {
                    if (res.Flag == 2 && res.MobileNo != "" && res.RegistrationNo != null && res.MobileNo != "" && res.RegistrationNo != null)
                    {
                        SendSMSforIMC(res.RegistrationNo, res.MobileNo);
                    }

                }
                //return RedirectToAction("ICCRegistration", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(Convert.ToString(res.RegisId)), stepValue = Convert.ToInt32(Session["stepValue"]) + 1, UpdateStep = 1 });
                return RedirectToAction("ViewIMCComplete");
            }

            return View();
        }

        public JsonResult BindDistrictlist(int stateId)
        {
            var res = objComnDb.GetDropDownList(7, stateId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAdmin(5)]
        public ActionResult ViewIMCComplete()
        {
            IMCModel model = new IMCModel();
            model.IMCModelList = appDB.GetComplete_IMC(objSM.UserID);
            return View(model.IMCModelList);
        }

        [AuthorizeAdmin(5)]
        public ActionResult ViewIMCInComplete()
        {
            IMCModel model = new IMCModel();
            model.IMCModelList = appDB.GetInComplete_IMC(objSM.UserID);
            return View(model.IMCModelList);
        }

        void SendSMSforIMC(string registrationNo, string mobileNo)
        {
            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";


                txtmsg = "Dear Citizen,\n\nYour Application form for Immunization Certificate has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\n MHFWD, UP";


                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }
        }
        #endregion

        #region  MLC
        [HttpPost]
        public JsonResult CheckMobileExistenceMLC(string appmobileNo, string idNo)
        {
            long regisId = 0;

            if (Session["regisIdMLC"] != null)
            {
                regisId = Convert.ToInt64(Session["regisIdMLC"]);
            }
            var user = appDB.CheckEmailMobileExistenceMLC(appmobileNo, "M", regisId, idNo);
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [AuthorizeAdmin(9)]
        [HttpGet]
        public ActionResult MLCRegistration(string regisId = "", int stepValue = -1)
        {
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            long userId = objSM.UserID;
            MLCModel model = new MLCModel();
            ViewBag.Tehsil = objComnDb.GetDropDownList(21, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = objComnDb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComnDb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.BroughtByDistrict = Enumerable.Empty<SelectListItem>();
            ViewBag.IdType = appDB.GetIdType().Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            if (model.step == 2)
            {
                ViewBag.EnquiryDetails = appDB.getMLCEnquiry(Int64.Parse(regisId));
                ViewBag.RowCount = ViewBag.EnquiryDetails.Count - 1;
            }
            if (regisId != "")
            {
                model = appDB.getMLCStep((Convert.ToInt64(regisId)));

                if (model != null && objSM.UserID != model.regByUser)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home");
                }

                Session["regisIdMLC"] = model.regisIdMLC;
            }
            else
            {
                Session["regisIdMLC"] = null;
            }
            ViewBag.Submit = "Save and Continue";
            if (stepValue >= 0)
            {
                model.stepValue = stepValue;
                Session["stepValue"] = model.stepValue;
            }
            else
            {
                Session["stepValue"] = model.step;
                model.stepValue = stepValue;
            }
            if (model.step == 2 && (stepValue == 2 || stepValue == -1))
            {
                ViewBag.Submit = "Submit";
            }

            return View(model);
        }

        public JsonResult BindDistricDropdownlist(int stateId)
        {
            var res = objComnDb.GetDropDownList(7, stateId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAdmin(9)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MLCRegistration(MLCModel model, FormCollection form)
        {
            AuditMethods objAudit = new AuditMethods();
            var result = appDB.getMLCStep(model.regisIdMLC);

            if (result != null && objSM.UserID != result.regByUser)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            string xmldata = "";
            long _regisId = model.regisIdMLC;
            model.transIp = Common.GetIPAddress();
            model.regByUser = objSM.UserID;
            model.mobileNo = model.appmobileNo;
            model.forwardtoId = objSM.UserID;
            model.forwardtypeId = objSM.RollID;
            model.healthUnitDistrictId = objSM.districtId;
            #region Bulk Insertion
            if (model.step == 2 && (model.stepValue == 2 || model.stepValue == -1))
            {
                var enquirydetails = form.GetValues("enquiryDetails");
                int count = enquirydetails.Count();
                xmldata = "<EnquiryDetails>";
                long regisByuser = objSM.UserID;
                for (int i = 0; i < count; i++)
                {
                    if (enquirydetails[i].ToString() == "")
                    {
                        //xmldata = string.Empty;
                    }
                    else
                    {
                        xmldata += "<Enquiry><RegisByUser>" + regisByuser + "</RegisByUser>" +
                            "<EnquiryDetails>" + objAudit.FilterForAlphabetNumaric(enquirydetails[i]) + "</EnquiryDetails>"
                                + "</Enquiry>";
                    }
                }
                xmldata += "</EnquiryDetails>";
            }
            #endregion
            model.xmldata = xmldata;
            try
            {
                var res = appDB.Insert_UpdateMLC(model);
                if (res != null && res.Flag > 0)
                {
                    TempData["Message"] = res.Msg.ToUpper().ToString();
                    TempData["status"] = "success";
                }
                if (res.RegisId != 0)
                {
                    _regisId = res.RegisId;
                }

                if (model.step == 2 && res.appStatus == 1)
                {
                    SendSMSforMLC(res.RegistrationNo, res.MobileNo);
                    return RedirectToAction("ViewMLCComplete");
                }
                else if (res.Flag == 1)
                {
                    return RedirectToAction("MLCRegistration", new { regisId = OTPL_Imp.CustomCryptography.Encrypt(_regisId.ToString()), stepValue = Convert.ToInt32(Session["stepValue"]) + 1 });
                }
            }
            catch
            {
                TempData["Message"] = "An Error Occurred!";
                TempData["status"] = "error";
            }
            return View();
        }

        [AuthorizeAdmin(9)]
        public ActionResult ViewMLCComplete()
        {
            MLCModel model = new MLCModel();
            model.MLCModelList = appDB.GetComplete_MLC(objSM.UserID);
            return View(model.MLCModelList);
        }

        [AuthorizeAdmin(9)]
        public ActionResult ViewMLCInComplete()
        {
            MLCModel model = new MLCModel();
            model.MLCModelList = appDB.GetInComplete_MLC(objSM.UserID);
            return View(model.MLCModelList);
        }

        void SendSMSforMLC(string registrationNo, string mobileNo)
        {
            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";


                txtmsg = "Dear Citizen,\n\nYour Application form for Medico - Legal Certificate has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\n MHFWD, UP";


                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }
        }
        #endregion

        #region ICC

        [HttpPost]
        public JsonResult CheckMobileExistenceICC(string applicantMobileNo)
        {
            long regisId = 0;

            if (Session["regisIdICC"] != null)
            {

                regisId = Convert.ToInt64(Session["regisIdICC"]);
            }
            var user = appDB.CheckEmailMobileExistenceICC(applicantMobileNo, "M", regisId);
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [AuthorizeAdmin(11)]
        [HttpGet]
        public ActionResult ICCRegistration(string regisId = "", int stepValue = -1, int UpdateStep = 0)
        {

            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            ICCModel model = new ICCModel();
            Session["UpdateStep"] = UpdateStep;
            TempData["UpdateStep"] = UpdateStep;

            if (regisId != "" && regisId != null && regisId != "0")
            {

                model = appDB.getICCStep((Convert.ToInt64(regisId)));

                if (model != null && objSM.UserID != model.regisByuser)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home");
                }

                TempData["dist"] = model.districtId;
                Session["regisIdICC"] = model.regisIdICC;
                if (model.step == 2)
                {
                    TempData["showtb"] = "shw";
                }
            }
            else
            {
                Session["regisIdICC"] = null;
            }
            model.applicantMobileNo = model.mobileNo;
            if (stepValue >= 0)
            {

                model.stepValue = stepValue;
                Session["stepValue"] = model.stepValue;

            }
            else
            {
                Session["stepValue"] = model.step;
                model.stepValue = stepValue;
            }

            //ViewBag.State = objComnDb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            //ViewBag.District = objComnDb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = objComnDb.GetDropDownList(6, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = Enumerable.Empty<SelectListItem>();
            ViewBag.ImmunizationType = objComnDb.GetDropDownList(52, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.CheckListICC = objComnDb.GetDropDownList(53, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            model.appImmunList = appDB.BindImmunizationDetails();
            if (regisId != "" && regisId != null && regisId != "0")
            {
                ViewBag.ImmunList = appDB.getICCImmuCHC(Convert.ToInt64(regisId));

            }
            return View(model);

        }

        [AuthorizeAdmin(11)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ICCRegistration(ICCModel model, FormCollection form)
        {
            var result = appDB.getICCStep(model.regisIdICC);

            if (result != null && objSM.UserID != result.regisByuser)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            model.transIP = Common.GetIPAddress();
            model.regisByuser = objSM.UserID;
            model.forwardtoId = objSM.UserID;
            model.forwardtypeId = objSM.RollID;
            model.healthUnitDistrictId = objSM.districtId;

            if (Session["regisIdICC"] == null)
            {
                model.step = 0;
            }
            else
            {
                model.regisIdICC = Convert.ToInt64(Session["regisIdICC"].ToString());
                model.step = Convert.ToInt32(Session["stepValue"].ToString());
            }

            model.UpdateStep = Convert.ToInt32(Session["UpdateStep"].ToString());

            if (model.step == 0 || model.UpdateStep == 1)
            {
                model.step = 1;
                model.mobileNo = model.applicantMobileNo;
                var res = appDB.ICCInsertUpdate(model);

                if (res != null)
                {
                    setSweetAlertMsg(res.Msg.ToString(), "error");
                    TempData["msg"] = res.Msg.ToString();
                    TempData["msgstatus"] = "success";
                }

                return RedirectToAction("ICCRegistration", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(Convert.ToString(res.RegisId)), stepValue = Convert.ToInt32(Session["stepValue"]) + 1, UpdateStep = 0 });
            }
            else if (model.step == 1)
            {
                model.step = 2;
                #region Bulk Insertion
                var isExsistImmuName = form.GetValues("addimmunId");
                var dateOfImmunization = form.GetValues("adddateOfImmunization");

                int count = isExsistImmuName.Count();
                string XmlData = "<ChildVaccine>";
                if (count == 0)
                {
                    TempData["msg"] = "Please Select Atleast One Immunization Detail ";
                    TempData["msgstatus"] = "warning";
                    return RedirectToAction("ICCRegistration", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(Convert.ToString(model.regisIdICC)), stepValue = Convert.ToInt32(Session["stepValue"]), UpdateStep = 0 });

                }
                for (int i = 0; i < count; i++)
                {
                    if (dateOfImmunization[i] != "")
                    {
                        XmlData += "<CHild><immuId>" + isExsistImmuName[i] + "</immuId>"
                         + "<isExsistImmuName>1</isExsistImmuName>"
                         + "<dateOfImmunization>" + dateOfImmunization[i] + "</dateOfImmunization>"
                           + "</CHild>";
                    }
                    else
                    {
                        //TempData["msgalert"] = "Checked Date Required!";
                        //setSweetAlertMsg("Checked Date Required!", "warning");
                        TempData["msg"] = "Immunization Date Required!";
                        TempData["msgstatus"] = "warning";
                        return RedirectToAction("ICCRegistration", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(Convert.ToString(model.regisIdICC)), stepValue = Convert.ToInt32(Session["stepValue"]), UpdateStep = 0 });
                    }


                }

                XmlData += "</ChildVaccine>";

                #endregion

                model.XmlData = XmlData;

                var res = appDB.ICCInsertUpdate(model);
                if (res != null)
                {
                    if (res.Flag == 2 && res.MobileNo != "" && res.RegistrationNo != null && res.MobileNo != "" && res.RegistrationNo != null)
                    {
                        SendSMSforICC(res.RegistrationNo, res.MobileNo);
                    }
                }

                return RedirectToAction("ICCRegistration", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(Convert.ToString(res.RegisId)), stepValue = Convert.ToInt32(Session["stepValue"]) + 1, UpdateStep = 0 });
            }
            else if (model.step == 2)
            {
                model.step = 3;
                #region Bulk Insertion checkList
                var checkListId = form.GetValues("chk_checkList");

                int chk_count = checkListId.Count();
                string XmlDataChecklist = "<CheckList>";

                for (int i = 0; i < chk_count; i++)
                {
                    if (checkListId[i].ToString() == "" || checkListId[i].ToString() == "0")
                    {
                        //XmlDatacheckList = string.Empty;
                    }
                    else
                    {

                        XmlDataChecklist += "<CheckData><checkListId>" + checkListId[i] + "</checkListId></CheckData>";
                    }

                }
                XmlDataChecklist += "</CheckList>";
                model.XmlDataChecklist = XmlDataChecklist;
                #endregion

                var res = appDB.ICCInsertUpdate(model);
                if (res != null)
                {
                    if (res.Flag == 2 && res.MobileNo != "" && res.RegistrationNo != null && res.MobileNo != "" && res.RegistrationNo != null)
                    {
                        SendSMSforICC(res.RegistrationNo, res.MobileNo);
                    }
                }
                //return RedirectToAction("ICCRegistration", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(Convert.ToString(res.RegisId)), stepValue = Convert.ToInt32(Session["stepValue"]) + 1, UpdateStep = 1 });
                return RedirectToAction("ViewICCComplete");
            }
            return View();
        }

        public JsonResult BindDistrictlistICC(int stateId)
        {
            var res = objComnDb.GetDropDownList(7, stateId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAdmin(11)]
        public ActionResult ViewICCComplete()
        {
            ICCModel model = new ICCModel();
            model.ICCModelList = appDB.getICCCompleteRegistration(objSM.UserID);
            return View(model.ICCModelList);
        }

        [AuthorizeAdmin(11)]
        public ActionResult ViewICCInComplete()
        {
            ICCModel model = new ICCModel();
            model.ICCModelList = appDB.getICCInCompleteRegistration(objSM.UserID);
            return View(model.ICCModelList);
        }

        void SendSMSforICC(string registrationNo, string mobileNo)
        {

            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";

                //txtmsg = "Dear Citizen,\n\nYour Application form has been submitted successfully. Your Application Form Number is " + registrationNo + ", kindly use this further.\n\n Thanks";
                txtmsg = "Dear Citizen,\n\nYour Application form for Payment of Immunization Certificate for children has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\n MHFWD, UP";


                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }

        }
        #endregion
        #endregion
        //PARAM
        public JsonResult GetOwnerDetailList(string regisIdNUH)
        {
            return Json(appDB.getNUHOwnerList(regisIdNUH), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DropdownDistNUH()
        {
            Common_DB comndb = new Common_DB();
            var res = comndb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CalculateReturnAmountFAP(int CompancationCatId, string dateOfDeath, string DateOfOpration)
        {
            decimal clameAmt = 0;
            bool isSucc = false;
            isSucc = objComn.GetClameAmountFAP(CompancationCatId, dateOfDeath, DateOfOpration, out clameAmt);

            return Json(new { res = isSucc, clameAmount = clameAmt });
        }
        #endregion


        public ActionResult Affidavit(bool isRenewal, int operatedId, string RegisIdNUH)
        {
            Declaration model = new Declaration();
            if (string.IsNullOrEmpty(objSM.UserID.ToString()))
            {

            }
            model.regisIdNUH = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(RegisIdNUH));
            // model.regisIdNUH = Convert.ToInt64(TempData["RegisIdNUH"]);
            model = appDB.ShowAffidavitData(model.regisIdNUH);
            model.OwnerList = appDB.GetOwnerList(model.regisIdNUH);
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
            var res = appDB.UpdateAffidavitNUH(model).isSuccess;

            if (res == 1)
            {
                //errormsg = "Declaration Submitted Successfully";
                //setSweetAlertMsg(errormsg, "success");

                TempData["RegisIdNUH"] = model.NuhId;
                TempData["RegistrationNo"] = model.registrationNumber;
                TempData["Message"] = "Declaration Submitted Successfully.";
                string _regisIdNUH = OTPL_Imp.CustomCryptography.Encrypt(model.NuhId.ToString());
                return RedirectToAction("Pairamedical", "AppRegistration", new { regisIdNUH = _regisIdNUH, isRenewal = model.isRenewal });


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
        public ActionResult Pairamedical(string regisIdNUH, bool isRenewal)
        {
            PairamedicalModel model = new PairamedicalModel();

            //model.regisIdNUH = Convert.ToInt64(regisIdNUH);
            //model.regisIdNUH = Convert.ToInt64(TempData["RegisIdNUH"]);
            model.regisIdNUH = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdNUH));
            model.Paramedicallist = appDB.GetParamedicalDetails(model.regisIdNUH);

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

            var res = appDB.UpdateParamedical(model).isSuccess;
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
                    return Json(model, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public ActionResult Receipt(string NuhId)
        {
            ReceiptModel model = new ReceiptModel();

            model.regisIdNUH = Convert.ToInt64(NuhId);

            model = objNUHDB.GetNUHReceipt(model.regisIdNUH);
            model.ReceiptList = objNUHDB.ReceiptList(model.regisIdNUH);
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("ViewNUHComplete", "AppRegistration", new { isRenewal = true });
            }
        }
    }
}

