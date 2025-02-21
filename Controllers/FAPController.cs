using CCSHealthFamilyWelfareDept.DAL;
using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OTPL_Imp;
using CCSHealthFamilyWelfareDept.Filters;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using Newtonsoft.Json;
using System.Net;
namespace CCSHealthFamilyWelfareDept.Controllers
{
    [AuthorizeUser]
    public class FAPController : Controller
    {
        FAP_DB objFAPDB = new FAP_DB();
        Common_DB objComDB = new Common_DB();
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

        #region checkfor Registration

        public ActionResult isRegister()
        {
            var resultData = objFAPDB.GetRegisterDetails();

            if (resultData == null)
            {
                return RedirectToAction("RegistrationInstructions", "FAP");
            }
            else if (resultData.regisId > 0 && string.IsNullOrEmpty(resultData.notarizedAffidavitFilePath))
            {
                string _regisId = OTPL_Imp.CustomCryptography.Encrypt(resultData.regisId.ToString());
                return RedirectToAction("UploadAffidavit", "FAP", new { regisId = _regisId });
            }
            else if (!string.IsNullOrEmpty(resultData.notarizedAffidavitFilePath))
            {
                return RedirectToAction("FamilyDashBoard", "FAP");
            }

            return View();
        }

        #endregion

        public ActionResult FamilyDashBoard()
        {
            return View();
        }

        public JsonResult UploadFile(HttpPostedFileBase File)
        {
            string Dirpath = "~/Content/writereaddata/FAP/";
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
                    path = "SNV";//"warning_Maximum 2MB file size are allowed";
                }
                else
                {
                    File.SaveAs(completepath);
                    path = Dirpath + filename;
                }
            }
            else
            {
                path = "TNV";//"warning_Invalid File Format only pdf and jpg files are allow!";
            }
            List<string> plist = new List<string> { filename, path };
            return Json(plist);
        }

        [HttpGet]
        public ActionResult RegistrationInstructions()
        {

            return View();
        }

        public ActionResult UploadAffidavit(string regisId)
        {
            FAPModel model = new FAPModel();
            long regisByuser = objSM.UserID;
            model.regisIdFAP = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId));
            Session["regisAffidavitFAPId"] = regisId;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadAffidavit(FAPModel model)
        {
            if (model.regisIdFAP > 0 && !string.IsNullOrEmpty(model.affidavitfilePath))
            {
                int resultData = objFAPDB.UploadAffidavitNUH(model.regisIdFAP, model.affidavitfilePath);
                if (resultData > 0)
                {
                    TempData["Message"] = "Affidavit Uploaded Successfully.";
                    return RedirectToAction("FamilyDashBoard");
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

        public ActionResult FAPRegistration()
        {
            long userId = objSM.UserID;

            #region ISP

            var application = TempData["ApplicantCommanDetails"] as ApplicantCommanDetails;
            TempData["ApplicantCommanDetails1"] = application;
            FAPModel model = new FAPModel();
            TempData["ISPUSER"] = "N";
            if (@TempData["ApplicantCommanDetails"] != null)
            {
                TempData["ISPUSER"] = "Y";
                model.sterilizedName = application.first_name_eng + " " + application.middle_name_eng + " " + application.last_name_eng;
                model.fatherName = application.father_or_husband_or_guardian_name_eng;
                model.sterilizedDob = application.dob.ToString("dd/MM/yyyy");

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
                model.sterilizedGender = model.gender;
                model.sterilizedAddress = application.permanent_mohalla;

            }


            #endregion

            if (userId != 0)
            {

                ViewBag.Compensation = objComDB.GetDropDownList(1, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.Relation = objComDB.GetDropDownList(2, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.OperationType = objComDB.GetDropDownList(4, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.Employement = objComDB.GetDropDownList(3, 0).ToList();
                ViewBag.District = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.State = objComDB.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                ViewBag.Save = "Submit";
                ViewBag.Cancel = "Reset";

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult CheckMobileExistence(string claimantMobileNo)
        {
            var user = objFAPDB.CheckEmailMobileExistence(claimantMobileNo, "M");
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FAPRegistration(FAPModel model, FormCollection form)
        {
             AuditMethods objAud = new AuditMethods();

             ViewBag.Compensation = objComDB.GetDropDownList(1, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
             ViewBag.Relation = objComDB.GetDropDownList(2, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
             ViewBag.OperationType = objComDB.GetDropDownList(4, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
             ViewBag.Employement = objComDB.GetDropDownList(3, 0).ToList();
             ViewBag.District = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
             ViewBag.State = objComDB.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
             ViewBag.Save = "Submit";

            string errormsg = "";
            bool valStatus = false;
            #region Bulk Insertion by abhijeet
            var childname = form.GetValues("childName");
            var childage = form.GetValues("childAge");
            var gender = form.GetValues("gender");
            var maritalstatus = form.GetValues("maritalStatus");
            int count = childname.Count();
            string XmlData = "<Children>";
            long regisByuser = objSM.UserID;
            for (int i = 0; i < count; i++)
            {
                if (childname[i].ToString() == "" && childage[i] == "" && gender[i] == "")
                {
                    //XmlData = string.Empty;
                }
                else
                {

                    XmlData += "<Child><RegisByUser>" + regisByuser + "</RegisByUser>" +
                        "<ChildName>" + objAud.RemoveSpecialCharactors(childname[i]) + "</ChildName>"
                          + "<ChildAge>" + childage[i] + "</ChildAge>"
                          + "<Gender>" + gender[i] + "</Gender>"
                           + "<MaritalStatus>" + maritalstatus[i] + "</MaritalStatus>"
                            + "</Child>";
                }

            }
            XmlData += "</Children>";
            #endregion
            if (model.operationTypeId != 5)
            {
                ModelState["operationOtherprocess"].Errors.Clear();
            }
            if (model.compensationCategoryId != 6)
            {
                //ModelState["dateofReleased"].Errors.Clear();
                ModelState["dateofDeath"].Errors.Clear();
                //ModelState["admittedDate"].Errors.Clear();
            }

            if (model.compensationCategoryId != 3)
            {
                ModelState["dischargeSummaryFile"].Errors.Clear();
                ModelState["dateofDeath"].Errors.Clear();
                ModelState["claimantName"].Errors.Clear();
                ModelState["claimantAge"].Errors.Clear();
                ModelState["relationId"].Errors.Clear();
                ModelState["claimantAddress"].Errors.Clear();
                ModelState["claimantAadhaarNo"].Errors.Clear();
                ModelState["strMobileNo"].Errors.Clear();
                //model.claimantMobileNo = model.strMobileNo;
            }

           
            if (!string.IsNullOrEmpty(model.usgfilePath))
            {
                valStatus = objAud.IsValidLink(model.usgfilePath, "Document File", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }

            if (!string.IsNullOrEmpty(model.sterilizationcertificatefilePath))
            {
                valStatus = objAud.IsValidLink(model.sterilizationcertificatefilePath, "Document File", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }

            if (!string.IsNullOrEmpty(model.deathCertificateFilePath))
            {
                valStatus = objAud.IsValidLink(model.deathCertificateFilePath, "Document File", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }

            if (!string.IsNullOrEmpty(model.dischargeSummaryFilePath))
            {
                valStatus = objAud.IsValidLink(model.dischargeSummaryFilePath, "Document File", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }
                      

            if (!string.IsNullOrEmpty(model.uptfilePath))
            {
                valStatus = objAud.IsValidLink(model.uptfilePath, "Document File", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }

            if (!string.IsNullOrEmpty(model.antenatalcardfilePath))
            {
                valStatus = objAud.IsValidLink(model.antenatalcardfilePath, "Document File", out errormsg);
                if (!valStatus)
                {
                    setSweetAlertMsg(errormsg, "warning");
                    return View(model);
                }
            }

            if (model.compensationCategoryId == 3)
            {
                ModelState["strMobileNo"].Errors.Clear();
            }
            //if (model.compensationCategoryId == 3)
            //{
            //    ModelState["strMobileNo"].Errors.Clear();
            //}
            if (ModelState.IsValid)
            {
                string IpAddress = Common.GetIPAddress();

                ModelState.Clear();
                decimal clameAmt = 0;
                bool isSucc = objCom.GetClameAmountFAP(model.compensationCategoryId, model.dateofDeath, model.operationDate, out clameAmt);
                if (isSucc && clameAmt > 0)
                {
                    model.claimAmount = clameAmt;
                }
                else
                {
                    setSweetAlertMsg("Invalid Claimed Amount", "warning");
                    return View(model);
                }
                if (model.compensationCategoryId == 6)
                {
                    if (objCom.IsValidDateFormat(model.dateofReleased))
                    {
                        model.dateofReleased = Convert.ToDateTime(model.dateofReleased).ToString();
                        if (objCom.IsValidDateFormat(model.dateofDeath))
                        {
                            model.dateofDeath = Convert.ToDateTime(model.dateofDeath).ToString();
                            if (objCom.IsValidDateFormat(model.admittedDate))
                            {
                                model.admittedDate = Convert.ToDateTime(model.admittedDate).ToString();
                                if (model.sterilizedDob != null && model.claimantDob != null)
                                {
                                    if (objCom.IsValidDateFormat(model.claimantDob))
                                    {
                                        model.claimantDob = Convert.ToDateTime(model.claimantDob).ToString();
                                        if (objCom.IsValidDateFormat(model.sterilizedDob))
                                        {
                                            model.sterilizedDob = Convert.ToDateTime(model.sterilizedDob).ToString();
                                            if (objCom.IsValidDateFormat(model.informationDate))
                                            {
                                                model.informationDate = Convert.ToDateTime(model.informationDate).ToString();
                                                if (objCom.IsValidDateFormat(model.confirmationDate))
                                                {
                                                    model.confirmationDate = Convert.ToDateTime(model.confirmationDate).ToString();
                                                    var res = objFAPDB.FAPInsertUpdate(1, model.regisIdFAP, regisByuser, model.compensationCategoryId, model.dateofReleased, model.dateofDeath, model.admittedDate, model.complicationsDetails, model.heathUnitName, model.heathUnitAddress, model.stateId, model.healthunitDistrictId, model.doctorName, model.claimantName, model.claimantDob, model.claimantAge, model.relationId, model.claimantAddress, model.claimantMobileNo, model.sterilizedName, model.sterilizedDob, model.sterilizedAge, model.fatherName, model.spouseName, model.spouseAge, model.sterilizedGender, model.sterilizedAddress, model.sterlizedDistrictId, model.employementId, model.serviceType, model.operationDate, model.operationTypeId, model.operationOtherprocess, model.isConfirm, model.claimAmount, model.informationDate, model.sevakendraName, model.confirmationDate, model.sevadoctorName, model.accountHolderName, model.bankName, model.branchName, model.accountNo, model.ifscCode, model.opdDistricthospitalfilePath, model.usgfilePath, model.uptfilePath, model.antenatalcardfilePath, model.affidavitfilePath, model.sterilizationcertificatefilePath, IpAddress, IpAddress, XmlData, objSM.AppRequestKey, model.deathCertificateFilePath, model.dischargeSummaryFilePath, model.claimantAadhaarNo);
                                                    try
                                                    {
                                                        if (res.RegisId > 0 && res.RegistrationNo != "")
                                                        {
                                                            if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                                                            {
                                                                EDistrictServiceClass ed = new EDistrictServiceClass();

                                                                int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.FAP);

                                                                bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "FAP");

                                                                if (result)
                                                                {
                                                                    SendSMS(res.RegistrationNo, res.MobileNo);
                                                                    setSweetAlertMsg("Record Saved Successfully", "success");
                                                                    TempData["RegisId"] = res.RegisId;
                                                                    TempData["RegistrationNo"] = res.RegistrationNo;
                                                                    return RedirectToAction("RegistrationConfirmation");
                                                                }
                                                                else
                                                                {
                                                                    int exeRsult = objFAPDB.DeleteRegistrationFAP(res.RegisId);
                                                                    setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                                                    ModelState.Clear();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                SendSMS(res.RegistrationNo, res.MobileNo);
                                                                setSweetAlertMsg("Record Saved Successfully", "success");
                                                                ModelState.Clear();
                                                                TempData["RegisId"] = res.RegisId;
                                                                TempData["RegistrationNo"] = res.RegistrationNo;
                                                                return RedirectToAction("RegistrationConfirmation");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            setSweetAlertMsg("Some Error Occur", "error");
                                                        }
                                                    }
                                                    catch (Exception E)
                                                    {
                                                        setSweetAlertMsg("Record not save ", "error");
                                                    }
                                                }
                                                else
                                                {
                                                    TempData["msg"] = "Confirmation Date is incorrect format, Date format should be DD/MM/YYYY !";
                                                    TempData["msgstatus"] = "warning";
                                                }
                                            }
                                            else
                                            {
                                                 TempData["msg"] = "Information Date is incorrect format, Date format should be DD/MM/YYYY !";
                                                 TempData["msgstatus"] = "warning";
                                            }
                                            
                                        }
                                        else
                                        {
                                            TempData["msg"] = "Claimant Date of Birth is incorrect format, Date format should be DD/MM/YYYY !";
                                            TempData["msgstatus"] = "warning";
                                        }
                                    }
                                    else
                                    {
                                        TempData["msg"] = "Sterilized Date of Birth is incorrect format, Date format should be DD/MM/YYYY !";
                                        TempData["msgstatus"] = "warning";
                                    }

                                }
                                else
                                {
                                    if (model.claimantDob != null)
                                    {
                                        if (objCom.IsValidDateFormat(model.claimantDob))
                                        {
                                            model.claimantDob = Convert.ToDateTime(model.claimantDob).ToString();
                                            if (objCom.IsValidDateFormat(model.informationDate))
                                            {
                                                model.informationDate = Convert.ToDateTime(model.informationDate).ToString();
                                                if (objCom.IsValidDateFormat(model.confirmationDate))
                                                {
                                                    model.confirmationDate = Convert.ToDateTime(model.confirmationDate).ToString();
                                                    var res = objFAPDB.FAPInsertUpdate(1, model.regisIdFAP, regisByuser, model.compensationCategoryId, model.dateofReleased, model.dateofDeath, model.admittedDate, model.complicationsDetails, model.heathUnitName, model.heathUnitAddress, model.stateId, model.healthunitDistrictId, model.doctorName, model.claimantName, model.claimantDob, model.claimantAge, model.relationId, model.claimantAddress, model.claimantMobileNo, model.sterilizedName, model.sterilizedDob, model.sterilizedAge, model.fatherName, model.spouseName, model.spouseAge, model.sterilizedGender, model.sterilizedAddress, model.sterlizedDistrictId, model.employementId, model.serviceType, model.operationDate, model.operationTypeId, model.operationOtherprocess, model.isConfirm, model.claimAmount, model.informationDate, model.sevakendraName, model.confirmationDate, model.sevadoctorName, model.accountHolderName, model.bankName, model.branchName, model.accountNo, model.ifscCode, model.opdDistricthospitalfilePath, model.usgfilePath, model.uptfilePath, model.antenatalcardfilePath, model.affidavitfilePath, model.sterilizationcertificatefilePath, IpAddress, IpAddress, XmlData, objSM.AppRequestKey, model.deathCertificateFilePath, model.dischargeSummaryFilePath,model.claimantAadhaarNo);
                                                  try
                                                    {
                                                        if (res.RegisId > 0 && res.RegistrationNo != "")
                                                        {
                                                            if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                                                            {
                                                                EDistrictServiceClass ed = new EDistrictServiceClass();

                                                                int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.FAP);

                                                                bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "FAP");

                                                                if (result)
                                                                {
                                                                    SendSMS(res.RegistrationNo, res.MobileNo);
                                                                    setSweetAlertMsg("Record Saved Successfully", "success");
                                                                    TempData["RegisId"] = res.RegisId;
                                                                    TempData["RegistrationNo"] = res.RegistrationNo;
                                                                    return RedirectToAction("RegistrationConfirmation");
                                                                }
                                                                else
                                                                {
                                                                    int exeRsult = objFAPDB.DeleteRegistrationFAP(res.RegisId);
                                                                    setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                                                    ModelState.Clear();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                SendSMS(res.RegistrationNo, res.MobileNo);
                                                                setSweetAlertMsg("Record Saved Successfully", "success");
                                                                ModelState.Clear();
                                                                TempData["RegisId"] = res.RegisId;
                                                                TempData["RegistrationNo"] = res.RegistrationNo;
                                                                return RedirectToAction("RegistrationConfirmation");
                                                            }
                                                        }

                                                        else
                                                        {
                                                            setSweetAlertMsg("Some Error Occur", "error");

                                                        }
                                                    }
                                                    catch (Exception E)
                                                    {
                                                        setSweetAlertMsg("Record not save ", "error");
                                                    }
                                                }
                                                else
                                                {
                                                    TempData["msg"] = "Confirmation Date is incorrect format, Date format should be DD/MM/YYYY !";
                                                    TempData["msgstatus"] = "warning";
                                                }
                                            }
                                            else
                                            {
                                                TempData["msg"] = "Information Date is incorrect format, Date format should be DD/MM/YYYY !";
                                                TempData["msgstatus"] = "warning";
                                            }
                                        }
                                        else
                                        {
                                            TempData["msg"] = "Claimant Date of Birth is incorrect format, Date format should be DD/MM/YYYY !";
                                            TempData["msgstatus"] = "warning";
                                        }

                                    }

                                    else if (model.sterilizedDob != null)
                                    {
                                        if (objCom.IsValidDateFormat(model.sterilizedDob))
                                        {
                                            model.sterilizedDob = Convert.ToDateTime(model.sterilizedDob).ToString();
                                            if (objCom.IsValidDateFormat(model.informationDate))
                                            {
                                                model.informationDate = Convert.ToDateTime(model.informationDate).ToString();
                                                if (objCom.IsValidDateFormat(model.confirmationDate))
                                                {
                                                    model.confirmationDate = Convert.ToDateTime(model.confirmationDate).ToString();
                                                    var res = objFAPDB.FAPInsertUpdate(1, model.regisIdFAP, regisByuser, model.compensationCategoryId, model.dateofReleased, model.dateofDeath, model.admittedDate, model.complicationsDetails, model.heathUnitName, model.heathUnitAddress, model.stateId, model.healthunitDistrictId, model.doctorName, model.claimantName, model.claimantDob, model.claimantAge, model.relationId, model.claimantAddress, model.claimantMobileNo, model.sterilizedName, model.sterilizedDob, model.sterilizedAge, model.fatherName, model.spouseName, model.spouseAge, model.sterilizedGender, model.sterilizedAddress, model.sterlizedDistrictId, model.employementId, model.serviceType, model.operationDate, model.operationTypeId, model.operationOtherprocess, model.isConfirm, model.claimAmount, model.informationDate, model.sevakendraName, model.confirmationDate, model.sevadoctorName, model.accountHolderName, model.bankName, model.branchName, model.accountNo, model.ifscCode, model.opdDistricthospitalfilePath, model.usgfilePath, model.uptfilePath, model.antenatalcardfilePath, model.affidavitfilePath, model.sterilizationcertificatefilePath, IpAddress, IpAddress, XmlData, objSM.AppRequestKey, model.deathCertificateFilePath, model.dischargeSummaryFilePath, model.claimantAadhaarNo);
                                                    try
                                                    {
                                                        if (res.RegisId > 0 && res.RegistrationNo != "")
                                                        {
                                                            if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                                                            {
                                                                EDistrictServiceClass ed = new EDistrictServiceClass();

                                                                int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.FAP);

                                                                bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "FAP");

                                                                if (result)
                                                                {
                                                                    SendSMS(res.RegistrationNo, res.MobileNo);
                                                                    setSweetAlertMsg("Record Saved Successfully", "success");
                                                                    TempData["RegisId"] = res.RegisId;
                                                                    TempData["RegistrationNo"] = res.RegistrationNo;
                                                                    return RedirectToAction("RegistrationConfirmation");
                                                                }
                                                                else
                                                                {
                                                                    int exeRsult = objFAPDB.DeleteRegistrationFAP(res.RegisId);
                                                                    setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                                                    ModelState.Clear();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                SendSMS(res.RegistrationNo, res.MobileNo);
                                                                setSweetAlertMsg("Record Saved Successfully", "success");
                                                                ModelState.Clear();
                                                                TempData["RegisId"] = res.RegisId;
                                                                TempData["RegistrationNo"] = res.RegistrationNo;
                                                                return RedirectToAction("RegistrationConfirmation");
                                                            }
                                                        }

                                                        else
                                                        {
                                                            setSweetAlertMsg("Some Error Occur", "error");

                                                        }
                                                    }
                                                    catch (Exception E)
                                                    {
                                                        setSweetAlertMsg("Record not save ", "error");
                                                    }
                                                }
                                                else
                                                {
                                                    TempData["msg"] = "Confirmation Date is incorrect format, Date format should be DD/MM/YYYY !";
                                                    TempData["msgstatus"] = "warning";
                                                }
                                            }
                                            else
                                            {
                                                TempData["msg"] = "Information Date is incorrect format, Date format should be DD/MM/YYYY !";
                                                TempData["msgstatus"] = "warning";
                                            }
                                        }
                                        else
                                        {

                                            TempData["msg"] = "Sterilized Date of Birth is incorrect format, Date format should be DD/MM/YYYY !";
                                            TempData["msgstatus"] = "warning";
                                        }
                                    }
                                    else
                                    {
                                        var res = objFAPDB.FAPInsertUpdate(1, model.regisIdFAP, regisByuser, model.compensationCategoryId, model.dateofReleased, model.dateofDeath, model.admittedDate, model.complicationsDetails, model.heathUnitName, model.heathUnitAddress, model.stateId, model.healthunitDistrictId, model.doctorName, model.claimantName, model.claimantDob, model.claimantAge, model.relationId, model.claimantAddress, model.claimantMobileNo, model.sterilizedName, model.sterilizedDob, model.sterilizedAge, model.fatherName, model.spouseName, model.spouseAge, model.sterilizedGender, model.sterilizedAddress, model.sterlizedDistrictId, model.employementId, model.serviceType, model.operationDate, model.operationTypeId, model.operationOtherprocess, model.isConfirm, model.claimAmount, model.informationDate, model.sevakendraName, model.confirmationDate, model.sevadoctorName, model.accountHolderName, model.bankName, model.branchName, model.accountNo, model.ifscCode, model.opdDistricthospitalfilePath, model.usgfilePath, model.uptfilePath, model.antenatalcardfilePath, model.affidavitfilePath, model.sterilizationcertificatefilePath, IpAddress, IpAddress, XmlData, objSM.AppRequestKey, model.deathCertificateFilePath, model.dischargeSummaryFilePath, model.claimantAadhaarNo);
                                        try
                                        {
                                            if (res.RegisId > 0 && res.RegistrationNo != "")
                                            {
                                                if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                                                {
                                                    EDistrictServiceClass ed = new EDistrictServiceClass();

                                                    int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.FAP);

                                                    bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "FAP");

                                                    if (result)
                                                    {
                                                        SendSMS(res.RegistrationNo, res.MobileNo);
                                                        setSweetAlertMsg("Record Saved Successfully", "success");
                                                        TempData["RegisId"] = res.RegisId;
                                                        TempData["RegistrationNo"] = res.RegistrationNo;
                                                        return RedirectToAction("RegistrationConfirmation");
                                                    }
                                                    else
                                                    {
                                                        int exeRsult = objFAPDB.DeleteRegistrationFAP(res.RegisId);
                                                        setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                                        ModelState.Clear();
                                                    }
                                                }
                                                else
                                                {
                                                    SendSMS(res.RegistrationNo, res.MobileNo);
                                                    setSweetAlertMsg("Record Saved Successfully", "success");
                                                    ModelState.Clear();
                                                    TempData["RegisId"] = res.RegisId;
                                                    TempData["RegistrationNo"] = res.RegistrationNo;
                                                    return RedirectToAction("RegistrationConfirmation");
                                                }
                                            }

                                            else
                                            {
                                                setSweetAlertMsg("Some Error Occur", "error");

                                            }
                                        }
                                        catch (Exception E)
                                        {
                                            setSweetAlertMsg("Record not save ", "error");
                                        }
                                    }

                                }
                            }
                            else
                            {
                                TempData["msg"] = "Admitted Date is incorrect format, Date format should be DD/MM/YYYY !";
                                TempData["msgstatus"] = "warning";
                            }
                        }
                        else
                        {
                            TempData["msg"] = "Death Date is incorrect format, Date format should be DD/MM/YYYY !";
                            TempData["msgstatus"] = "warning";
                        }
                    }
                    else
                    {
                        TempData["msg"] = "Release Date is incorrect format, Date format should be DD/MM/YYYY !";
                        TempData["msgstatus"] = "warning";
                    }
                    ////}

                }
                    ///////////rkuie/////
                else
                {
                    if (model.sterilizedDob != null && model.claimantDob != null)
                    {
                        if (objCom.IsValidDateFormat(model.claimantDob))
                        {
                            model.claimantDob = Convert.ToDateTime(model.claimantDob).ToString();
                            if (objCom.IsValidDateFormat(model.sterilizedDob))
                            {
                                model.sterilizedDob = Convert.ToDateTime(model.sterilizedDob).ToString();
                                if (objCom.IsValidDateFormat(model.informationDate))
                                {
                                    model.informationDate = Convert.ToDateTime(model.informationDate).ToString();
                                    if (objCom.IsValidDateFormat(model.confirmationDate))
                                    {
                                        model.confirmationDate = Convert.ToDateTime(model.confirmationDate).ToString();
                                        var res = objFAPDB.FAPInsertUpdate(1, model.regisIdFAP, regisByuser, model.compensationCategoryId, model.dateofReleased, model.dateofDeath, model.admittedDate, model.complicationsDetails, model.heathUnitName, model.heathUnitAddress, model.stateId, model.healthunitDistrictId, model.doctorName, model.claimantName, model.claimantDob, model.claimantAge, model.relationId, model.claimantAddress, model.claimantMobileNo, model.sterilizedName, model.sterilizedDob, model.sterilizedAge, model.fatherName, model.spouseName, model.spouseAge, model.sterilizedGender, model.sterilizedAddress, model.sterlizedDistrictId, model.employementId, model.serviceType, model.operationDate, model.operationTypeId, model.operationOtherprocess, model.isConfirm, model.claimAmount, model.informationDate, model.sevakendraName, model.confirmationDate, model.sevadoctorName, model.accountHolderName, model.bankName, model.branchName, model.accountNo, model.ifscCode, model.opdDistricthospitalfilePath, model.usgfilePath, model.uptfilePath, model.antenatalcardfilePath, model.affidavitfilePath, model.sterilizationcertificatefilePath, IpAddress, IpAddress, XmlData, objSM.AppRequestKey, model.deathCertificateFilePath, model.dischargeSummaryFilePath, model.claimantAadhaarNo);
                                        try
                                        {
                                            if (res.RegisId > 0 && res.RegistrationNo != "")
                                            {
                                                if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                                                {
                                                    EDistrictServiceClass ed = new EDistrictServiceClass();

                                                    int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.FAP);

                                                    bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "FAP");

                                                    if (result)
                                                    {
                                                        SendSMS(res.RegistrationNo, res.MobileNo);
                                                        setSweetAlertMsg("Record Saved Successfully", "success");
                                                        TempData["RegisId"] = res.RegisId;
                                                        TempData["RegistrationNo"] = res.RegistrationNo;
                                                        return RedirectToAction("RegistrationConfirmation");
                                                    }
                                                    else
                                                    {
                                                        int exeRsult = objFAPDB.DeleteRegistrationFAP(res.RegisId);
                                                        setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                                        ModelState.Clear();
                                                    }
                                                }
                                                else
                                                {
                                                    SendSMS(res.RegistrationNo, res.MobileNo);
                                                    setSweetAlertMsg("Record Saved Successfully", "success");
                                                    ModelState.Clear();
                                                    TempData["RegisId"] = res.RegisId;
                                                    TempData["RegistrationNo"] = res.RegistrationNo;
                                                    return RedirectToAction("RegistrationConfirmation");
                                                }
                                            }

                                            else
                                            {
                                                setSweetAlertMsg("Some Error Occur", "error");

                                            }
                                        }
                                        catch (Exception E)
                                        {
                                            setSweetAlertMsg("Record not save ", "error");
                                        }
                                    }
                                    else
                                    {
                                        TempData["msg"] = "Confirmation Date is incorrect format, Date format should be DD/MM/YYYY !";
                                        TempData["msgstatus"] = "warning";
                                    }
                                }
                                else
                                {
                                    TempData["msg"] = "Information Date is incorrect format, Date format should be DD/MM/YYYY !";
                                    TempData["msgstatus"] = "warning";
                                }
                            }
                            else
                            {
                                TempData["msg"] = "Claimant Date of Birth is incorrect format, Date format should be DD/MM/YYYY !";
                                TempData["msgstatus"] = "warning";
                            }
                        }
                        else
                        {
                            TempData["msg"] = "Sterilized Date of Birth is incorrect format, Date format should be DD/MM/YYYY !";
                            TempData["msgstatus"] = "warning";
                        }

                    }
                    else
                    {
                        if (model.claimantDob != null)
                        {
                            if (objCom.IsValidDateFormat(model.claimantDob))
                            {
                                model.claimantDob = Convert.ToDateTime(model.claimantDob).ToString();
                                if (objCom.IsValidDateFormat(model.informationDate))
                                {
                                    model.informationDate = Convert.ToDateTime(model.informationDate).ToString();
                                    if (objCom.IsValidDateFormat(model.confirmationDate))
                                    {
                                        model.confirmationDate = Convert.ToDateTime(model.confirmationDate).ToString();
                                        var res = objFAPDB.FAPInsertUpdate(1, model.regisIdFAP, regisByuser, model.compensationCategoryId, model.dateofReleased, model.dateofDeath, model.admittedDate, model.complicationsDetails, model.heathUnitName, model.heathUnitAddress, model.stateId, model.healthunitDistrictId, model.doctorName, model.claimantName, model.claimantDob, model.claimantAge, model.relationId, model.claimantAddress, model.claimantMobileNo, model.sterilizedName, model.sterilizedDob, model.sterilizedAge, model.fatherName, model.spouseName, model.spouseAge, model.sterilizedGender, model.sterilizedAddress, model.sterlizedDistrictId, model.employementId, model.serviceType, model.operationDate, model.operationTypeId, model.operationOtherprocess, model.isConfirm, model.claimAmount, model.informationDate, model.sevakendraName, model.confirmationDate, model.sevadoctorName, model.accountHolderName, model.bankName, model.branchName, model.accountNo, model.ifscCode, model.opdDistricthospitalfilePath, model.usgfilePath, model.uptfilePath, model.antenatalcardfilePath, model.affidavitfilePath, model.sterilizationcertificatefilePath, IpAddress, IpAddress, XmlData, objSM.AppRequestKey, model.deathCertificateFilePath, model.dischargeSummaryFilePath, model.claimantAadhaarNo);
                                        try
                                        {
                                            if (res.RegisId > 0 && res.RegistrationNo != "")
                                            {
                                                if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                                                {
                                                    EDistrictServiceClass ed = new EDistrictServiceClass();

                                                    int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.FAP);

                                                    bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "FAP");

                                                    if (result)
                                                    {
                                                        SendSMS(res.RegistrationNo, res.MobileNo);
                                                        setSweetAlertMsg("Record Saved Successfully", "success");
                                                        TempData["RegisId"] = res.RegisId;
                                                        TempData["RegistrationNo"] = res.RegistrationNo;
                                                        return RedirectToAction("RegistrationConfirmation");
                                                    }
                                                    else
                                                    {
                                                        int exeRsult = objFAPDB.DeleteRegistrationFAP(res.RegisId);
                                                        setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                                        ModelState.Clear();
                                                    }
                                                }
                                                else
                                                {
                                                    SendSMS(res.RegistrationNo, res.MobileNo);
                                                    setSweetAlertMsg("Record Saved Successfully", "success");
                                                    ModelState.Clear();
                                                    TempData["RegisId"] = res.RegisId;
                                                    TempData["RegistrationNo"] = res.RegistrationNo;
                                                    return RedirectToAction("RegistrationConfirmation");
                                                }
                                            }

                                            else
                                            {
                                                setSweetAlertMsg("Some Error Occur", "error");

                                            }
                                        }
                                        catch (Exception E)
                                        {
                                            setSweetAlertMsg("Record not save ", "error");
                                        }
                                    }
                                    else
                                    {
                                        TempData["msg"] = "Confirmation Date is incorrect format, Date format should be DD/MM/YYYY !";
                                        TempData["msgstatus"] = "warning";
                                    }
                                }
                                else
                                {
                                    TempData["msg"] = "Information Date is incorrect format, Date format should be DD/MM/YYYY !";
                                    TempData["msgstatus"] = "warning";
                                }
                            }
                            else
                            {
                                TempData["msg"] = "Claimant Date of Birth is incorrect format, Date format should be DD/MM/YYYY !";
                                TempData["msgstatus"] = "warning";
                            }

                        }

                        else if (model.sterilizedDob != null)
                        {
                            if (objCom.IsValidDateFormat(model.sterilizedDob))
                            {
                                model.sterilizedDob = Convert.ToDateTime(model.sterilizedDob).ToString();
                                if (objCom.IsValidDateFormat(model.informationDate))
                                {
                                    model.informationDate = Convert.ToDateTime(model.informationDate).ToString();
                                    if (objCom.IsValidDateFormat(model.confirmationDate))
                                    {
                                        model.confirmationDate = Convert.ToDateTime(model.confirmationDate).ToString();
                                        var res = objFAPDB.FAPInsertUpdate(1, model.regisIdFAP, regisByuser, model.compensationCategoryId, model.dateofReleased, model.dateofDeath, model.admittedDate, model.complicationsDetails, model.heathUnitName, model.heathUnitAddress, model.stateId, model.healthunitDistrictId, model.doctorName, model.claimantName, model.claimantDob, model.claimantAge, model.relationId, model.claimantAddress, model.claimantMobileNo, model.sterilizedName, model.sterilizedDob, model.sterilizedAge, model.fatherName, model.spouseName, model.spouseAge, model.sterilizedGender, model.sterilizedAddress, model.sterlizedDistrictId, model.employementId, model.serviceType, model.operationDate, model.operationTypeId, model.operationOtherprocess, model.isConfirm, model.claimAmount, model.informationDate, model.sevakendraName, model.confirmationDate, model.sevadoctorName, model.accountHolderName, model.bankName, model.branchName, model.accountNo, model.ifscCode, model.opdDistricthospitalfilePath, model.usgfilePath, model.uptfilePath, model.antenatalcardfilePath, model.affidavitfilePath, model.sterilizationcertificatefilePath, IpAddress, IpAddress, XmlData, objSM.AppRequestKey, model.deathCertificateFilePath, model.dischargeSummaryFilePath, model.claimantAadhaarNo);
                                        try
                                        {
                                            if (res.RegisId > 0 && res.RegistrationNo != "")
                                            {
                                                if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                                                {
                                                    EDistrictServiceClass ed = new EDistrictServiceClass();

                                                    int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.FAP);

                                                    bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "FAP");

                                                    if (result)
                                                    {
                                                        SendSMS(res.RegistrationNo, res.MobileNo);
                                                        setSweetAlertMsg("Record Saved Successfully", "success");
                                                        TempData["RegisId"] = res.RegisId;
                                                        TempData["RegistrationNo"] = res.RegistrationNo;
                                                        return RedirectToAction("RegistrationConfirmation");
                                                    }
                                                    else
                                                    {
                                                        int exeRsult = objFAPDB.DeleteRegistrationFAP(res.RegisId);
                                                        setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                                        ModelState.Clear();
                                                    }
                                                }
                                                else
                                                {
                                                    SendSMS(res.RegistrationNo, res.MobileNo);
                                                    setSweetAlertMsg("Record Saved Successfully", "success");
                                                    ModelState.Clear();
                                                    TempData["RegisId"] = res.RegisId;
                                                    TempData["RegistrationNo"] = res.RegistrationNo;
                                                    #region ISP
                                                    if (res.RegistrationNo != null && TempData["ISPUSER"].ToString() == "Y")
                                                    {
                                                        returnApplicationAcknowledgementtoIspFAP(model, TempData["ApplicantCommanDetails1"] as ApplicantCommanDetails, res.RegistrationNo);
                                                    }
                                                    #endregion
                                                    return RedirectToAction("RegistrationConfirmation");
                                                }
                                            }

                                            else
                                            {
                                                setSweetAlertMsg("Some Error Occur", "error");

                                            }
                                        }
                                        catch (Exception E)
                                        {
                                            setSweetAlertMsg("Record not save ", "error");
                                        }
                                    }
                                    else
                                    {
                                        TempData["msg"] = "Confirmation Date is incorrect format, Date format should be DD/MM/YYYY !";
                                        TempData["msgstatus"] = "warning";
                                    }
                                }
                                else
                                {
                                    TempData["msg"] = "Information Date is incorrect format, Date format should be DD/MM/YYYY !";
                                    TempData["msgstatus"] = "warning";
                                }
                            }
                            else
                            {

                                TempData["msg"] = "Sterilized Date of Birth is incorrect format, Date format should be DD/MM/YYYY !";
                                TempData["msgstatus"] = "warning";
                            }
                        }
                        else
                        {
                            if (objCom.IsValidDateFormat(model.informationDate))
                            {
                                model.informationDate = Convert.ToDateTime(model.informationDate).ToString();
                                if (objCom.IsValidDateFormat(model.confirmationDate))
                                {
                                    model.confirmationDate = Convert.ToDateTime(model.confirmationDate).ToString();
                                    var res = objFAPDB.FAPInsertUpdate(1, model.regisIdFAP, regisByuser, model.compensationCategoryId, model.dateofReleased, model.dateofDeath, model.admittedDate, model.complicationsDetails, model.heathUnitName, model.heathUnitAddress, model.stateId, model.healthunitDistrictId, model.doctorName, model.claimantName, model.claimantDob, model.claimantAge, model.relationId, model.claimantAddress, model.claimantMobileNo, model.sterilizedName, model.sterilizedDob, model.sterilizedAge, model.fatherName, model.spouseName, model.spouseAge, model.sterilizedGender, model.sterilizedAddress, model.sterlizedDistrictId, model.employementId, model.serviceType, model.operationDate, model.operationTypeId, model.operationOtherprocess, model.isConfirm, model.claimAmount, model.informationDate, model.sevakendraName, model.confirmationDate, model.sevadoctorName, model.accountHolderName, model.bankName, model.branchName, model.accountNo, model.ifscCode, model.opdDistricthospitalfilePath, model.usgfilePath, model.uptfilePath, model.antenatalcardfilePath, model.affidavitfilePath, model.sterilizationcertificatefilePath, IpAddress, IpAddress, XmlData, objSM.AppRequestKey, model.deathCertificateFilePath, model.dischargeSummaryFilePath, model.claimantAadhaarNo);
                                    try
                                    {
                                        if (res.RegisId > 0 && res.RegistrationNo != "")
                                        {
                                            if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                                            {
                                                EDistrictServiceClass ed = new EDistrictServiceClass();

                                                int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.FAP);

                                                bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "FAP");

                                                if (result)
                                                {
                                                    SendSMS(res.RegistrationNo, res.MobileNo);
                                                    setSweetAlertMsg("Record Saved Successfully", "success");
                                                    TempData["RegisId"] = res.RegisId;
                                                    TempData["RegistrationNo"] = res.RegistrationNo;
                                                    return RedirectToAction("RegistrationConfirmation");
                                                }
                                                else
                                                {
                                                    int exeRsult = objFAPDB.DeleteRegistrationFAP(res.RegisId);
                                                    setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                                                    ModelState.Clear();
                                                }
                                            }
                                            else
                                            {
                                                SendSMS(res.RegistrationNo, res.MobileNo);
                                                setSweetAlertMsg("Record Saved Successfully", "success");
                                                ModelState.Clear();
                                                TempData["RegisId"] = res.RegisId;
                                                TempData["RegistrationNo"] = res.RegistrationNo;
                                                return RedirectToAction("RegistrationConfirmation");
                                            }
                                        }
                                        else
                                        {
                                            setSweetAlertMsg("Some Error Occur", "error");
                                        }
                                    }
                                    catch (Exception E)
                                    {
                                        setSweetAlertMsg("Record not save ", "error");
                                    }
                                }
                                else
                                {
                                    TempData["msg"] = "Confirmation Date is incorrect format, Date format should be DD/MM/YYYY !";
                                    TempData["msgstatus"] = "warning";
                                }
                            }
                            else
                            {
                                TempData["msg"] = "Information Date is incorrect format, Date format should be DD/MM/YYYY !";
                                TempData["msgstatus"] = "warning";
                            }
                        }
                    }

                }

            }
            else
            {
                setSweetAlertMsg("Invalid Data Entered", "error");
                
            }

            ViewBag.Compensation = objComDB.GetDropDownList(1, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Relation = objComDB.GetDropDownList(2, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.OperationType = objComDB.GetDropDownList(4, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Employement = objComDB.GetDropDownList(3, 0).ToList();
            ViewBag.District = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = objComDB.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Save = "Submit";
            ViewBag.Cancel = "Reset";
            return View();
        }

        #region ISP
        public string returnApplicationAcknowledgementtoIspFAP(FAPModel model, ApplicantCommanDetails model1, string RegistrationNo)
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
            ViewBag.FAPRegisId = OTPL_Imp.CustomCryptography.Encrypt(TempData["RegisId"].ToString());
            ViewBag.FAPRegistrationNo = TempData["RegistrationNo"];
            return View();
        }

        [HttpGet]
        public ActionResult ViewFAPList()
        {
            long regisByuser = objSM.UserID;
            FAPModel model = new FAPModel();
            model.FAPList = objFAPDB.GetFAPList(regisByuser);

            return View(model.FAPList);
        }

        public ActionResult FAPDetails(string regisId)
        {
            long regisByuser = objSM.UserID;
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);

            Session["regisIdFAP"] = regisId;
            FAPModel model = new FAPModel();

            model = objFAPDB.GetFAPListBYRegistrationNo(Convert.ToInt64(Session["regisIdFAP"].ToString()));

            if (model == null || objSM.UserID != model.regisByuser)
            {
                return RedirectToAction("UserUnauthoriseAcess", "Home");
            }

            return View(model);
        }

        public ActionResult BindChildList()
        {
            FAPModel model = new FAPModel();

            model.FAPList = objFAPDB.getFAPChild(Convert.ToInt64(Session["regisIdFAP"].ToString()));
            return PartialView("_ViewFAPChild", model.FAPList);
        }

        public ActionResult PrintApplicationForm()
        {
            var resultData = objFAPDB.GetRegisterDetails();

            Session["regisIdFAP"] = resultData.regisId;
            FAPModel model = new FAPModel();

            model = objFAPDB.GetFAPListBYRegistrationNo(Convert.ToInt64(Session["regisIdFAP"].ToString()));

            if (model == null || objSM.UserID != model.regisByuser)
            {
                return RedirectToAction("UserUnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        public ActionResult FAPAffidavateReport(string regisId)
        {
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            string statusMessage = "";
            string setPdfName = "";
            var res = objFAPDB.GetDetail(Convert.ToInt64(regisId));

            try
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rpt_FAPAffidavit.rpt"));
                rd.SetDataSource(res);
                String dtnow = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                setPdfName = "FamilyPlanning" + "_" + dtnow;

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
                statusMessage = "error_Error Occour to Downloading, Please try again.";
            }
            return RedirectToAction("FamilyDashBoard");
        }

        void SendSMS(string registrationNo, string mobileNo)
        {

            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";

                //txtmsg = "Dear Citizen,\n\nYour Application form has been submitted successfully. Your Application Form Number is " + registrationNo + ", kindly use this further.\n\n Thanks";
                txtmsg = "Dear Citizen,\n\nYour Application form for Payment of Unsuccessful Family Planning has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\n MHFWD, UP";


                //string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));commented on 19/07/2023

                //objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }

        }

        [HttpPost]
        public ActionResult CalculateReturnAmountFAP(int CompancationCatId, string dateOfDeath, string DateOfOpration)
        {
            decimal clameAmt = 0;
            bool isSucc = false;
            isSucc = objCom.GetClameAmountFAP(CompancationCatId, dateOfDeath, DateOfOpration, out clameAmt);

            return Json(new { res = isSucc, clameAmount = clameAmt });
        }
    }
}
