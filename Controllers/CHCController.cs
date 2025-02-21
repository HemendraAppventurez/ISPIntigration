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
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Net;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    public class CHCController : Controller
    {
        //
        // GET: /CHC/

        CHCModel model = new CHCModel();
        MLC_DB objMlc = new MLC_DB();
        MLCModel Mlcmodel = new MLCModel();
        Common objComn = new Common();
        DEC_DB objdb = new DEC_DB();
        CHC_DB objCHCDB = new CHC_DB();
        FIC_DB objFICdb = new FIC_DB();
        ILC_DB objILC_DB = new ILC_DB();
        Common_DB comndb = new Common_DB();
        Account_DB objAccDb = new Account_DB();
        Common objCom = new Common();
        SessionManager objSM = new SessionManager();
        ICC_DB objICC = new ICC_DB();
        IMC_DB objIMC_DB = new IMC_DB();

        #region Method Set Sweet Alert Message
        protected void setSweetAlertMsg(string msg, string msgStatus)
        {
            ViewBag.Msg = msg;
            ViewBag.MsgStatus = msgStatus;
        }
        #endregion

        #region FIC

        [AuthorizeAdmin(3)]
        public ActionResult AppliedApplicationFIC()//CHC FIC dashboard
        {
            FICModel model = new FICModel();
            if (objSM.RollID == 8)
            {
                model.institutionTypeId = 0;
            }
            else
            {
                model.institutionTypeId = Convert.ToInt32(objSM.UserID);
            }
            //model.institutionTypeId = Convert.ToInt32(objSM.UserID);
            model = objCHCDB.getApplicationCountFIC(model.institutionTypeId);
            return View(model);
        }

        [AuthorizeAdmin(3)]
        public ActionResult ReceivedApplicationFIC()//All Received Application
        {
            ViewBag.DLLAppStatus = comndb.GetApplicationProcessByAppName("FIC").Where(m => m.Value == "-1" || m.Value == "1").ToList();
            return View();
        }

        [AuthorizeAdmin(3)]
        public ActionResult ReceivedApplicationListFIC(string registrationNo = "", string status = "", string requestDate = "", string appType = "")//list of All Received Application
        {
            List<FICDetailsModel> lstFICDetails = new List<FICDetailsModel>();
            int Appstatus = 0;
            //int institutionTypeId = Convert.ToInt32(objSM.UserID);
            int institutionTypeId;
            if (objSM.RollID == 8)
            {
                institutionTypeId = 0;
            }
            else
            {
                institutionTypeId = Convert.ToInt32(objSM.UserID);
            }
            if (status != "")
            {
                Appstatus = Convert.ToInt16(status);
            }
            if (registrationNo != "" || appType != "" || requestDate != "" || Appstatus != 0)
            {
                if (appType == "")
                {
                    lstFICDetails = objCHCDB.GetAllFICList(1, registrationNo, 0, requestDate, Appstatus, institutionTypeId).ToList();
                }
                else
                {
                    lstFICDetails = objCHCDB.GetAllFICList(1, registrationNo, Convert.ToInt16(appType), requestDate, Appstatus, institutionTypeId).ToList();
                }
            }
            else
            {
                lstFICDetails = objCHCDB.GetAllFICList(1, registrationNo, 0, requestDate, Appstatus, institutionTypeId).ToList();
            }


            return PartialView("_ReceivedApplicationListFIC", lstFICDetails);
        }

        [AuthorizeAdmin(3)]
        [HttpGet]
        public ActionResult UpdateAppProcessFIC(string regisId, int status)
        {
            FICModel model = new FICModel();
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);

            model = objCHCDB.GetFICList(regisId, status);

            if (model == null || objSM.UserID != model.forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            if (model.appStatus == 1)
            {
                model.appStatus = 3;
            }
            return View(model);
        }

        [AuthorizeAdmin(3)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAppProcessFIC(FICModel model)//Update Status For FIC
        {
            int appStatus = 0;
            #region ISP
            string status_code = "";
            #endregion
            if (model.appStatus == 3)
            {
                appStatus = 1;
            }
            else
            {
                appStatus = model.appStatus;
            }

            var result = objCHCDB.GetFICList(model.registrationNo, appStatus);

            if (result == null || objSM.UserID != result.forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            model.regByUser = objSM.UserID;
            model.transIp = Common.GetIPAddress();
            if (model.appStatus == -1)//pending
            {
                if (model.rejectedRemarks != null)
                {
                    //ModelState["inspectionDate"].Errors.Clear();
                    ModelState["inspectionRpt"].Errors.Clear();
                    ModelState["inspectionRejectedRemark"].Errors.Clear();
                    model.appStatus = 0;
                    var res = objCHCDB.FICAppStatusInsertUpdate(model.regisIdFIC, model.appStatus, model.rejectedRemarks, model.inspectionRptPath, model.inspectionRejectedRemark, model.certGenerateByDoc, model.certGenerateByDesig, model.inspecttionCompeletionDate, model.identificationMark, model.regByUser, model.transIp);
                    if (res != null)
                    {
                        setSweetAlertMsg(res.Msg.ToString(), "error");
                        TempData["msg"] = res.Msg.ToString();
                        TempData["msgstatus"] = "success";
                        SendSMSUpdateProcessFIC(model.registrationNo, res.MobileNo, model.appStatus);

                        #region ISP
                        status_code = "s110";//Rejected
                        returnServiceStatus(model.registrationNo, status_code);//virendra
                        #endregion
                    }
                    return RedirectToAction("RejectedApplicationFIC");
                }
                else
                {
                    ModelState["rejectedRemarks"].Errors.Clear();
                    //ModelState["inspectionDate"].Errors.Clear();
                    ModelState["inspectionRpt"].Errors.Clear();
                    ModelState["inspectionRejectedRemark"].Errors.Clear();
                    model.appStatus = 1;

                    #region ISP
                    status_code = "s111";//Application Accepted
                    returnServiceStatus(model.registrationNo, status_code);
                    #endregion

                    var res = objCHCDB.FICAppStatusInsertUpdate(model.regisIdFIC, model.appStatus, model.rejectedRemarks, model.inspectionRptPath, model.inspectionRejectedRemark, model.certGenerateByDoc, model.certGenerateByDesig, model.inspecttionCompeletionDate, model.identificationMark, model.regByUser, model.transIp);
                    if (res != null)
                    {
                        setSweetAlertMsg(res.Msg.ToString(), "error");
                        TempData["msg"] = res.Msg.ToString();
                        TempData["msgstatus"] = "success";

                    }
                    return RedirectToAction("UpdateAppProcessFIC", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(model.registrationNo), status = model.appStatus });
                }

            }
            else
                if (model.appStatus == 1)//accept
            {
                ModelState["rejectedRemarks"].Errors.Clear();
                ModelState["inspectionRpt"].Errors.Clear();
                ModelState["inspectionRejectedRemark"].Errors.Clear();
                model.appStatus = 2;
                var res = objCHCDB.FICAppStatusInsertUpdate(model.regisIdFIC, model.appStatus, model.rejectedRemarks, model.inspectionRptPath, model.inspectionRejectedRemark, model.certGenerateByDoc, model.certGenerateByDesig, model.inspecttionCompeletionDate, model.identificationMark, model.regByUser, model.transIp);
                if (res != null)
                {
                    setSweetAlertMsg(res.Msg.ToString(), "error");
                    TempData["msg"] = res.Msg.ToString();
                    TempData["msgstatus"] = "success";

                    SendSMSUpdateProcessFIC(model.registrationNo, res.MobileNo, model.appStatus);
                }
                return RedirectToAction("InProcessApplicationFIC");
            }
            else
                    if (model.appStatus == 2)//inspection schedule
            {
                ModelState["rejectedRemarks"].Errors.Clear();
                //ModelState["inspectionDate"].Errors.Clear();
                ModelState["inspectionRejectedRemark"].Errors.Clear();
                model.appStatus = 3;
                var res = objCHCDB.FICAppStatusInsertUpdate(model.regisIdFIC, model.appStatus, model.rejectedRemarks, model.inspectionRptPath, model.inspectionRejectedRemark, model.certGenerateByDoc, model.certGenerateByDesig, model.inspecttionCompeletionDate, model.identificationMark, model.regByUser, model.transIp);
                if (res != null)
                {
                    setSweetAlertMsg(res.Msg.ToString(), "error");
                    TempData["msg"] = res.Msg.ToString();
                    TempData["msgstatus"] = "success";
                }
                return RedirectToAction("UpdateAppProcessFIC", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(model.registrationNo), status = model.appStatus });
            }
            else
                        if (model.appStatus == 3)//inspection rpt uploaded
            {
                if (model.inspectionRejectedRemark == null)//POSITIVE
                {
                    ModelState["rejectedRemarks"].Errors.Clear();
                    //ModelState["inspectionDate"].Errors.Clear();
                    ModelState["inspectionRpt"].Errors.Clear();
                    ModelState["inspectionRejectedRemark"].Errors.Clear();
                    model.appStatus = 5;//CERTIFICATE GEN
                    var res = objCHCDB.FICAppStatusInsertUpdate(model.regisIdFIC, model.appStatus, model.rejectedRemarks, model.inspectionRptPath, model.inspectionRejectedRemark, model.certGenerateByDoc, model.certGenerateByDesig, model.inspecttionCompeletionDate, model.identificationMark, model.regByUser, model.transIp);
                    if (res != null)
                    {
                        setSweetAlertMsg(res.Msg.ToString(), "error");
                        TempData["msg"] = res.Msg.ToString();
                        TempData["msgstatus"] = "success";
                        #region ISP
                        status_code = "s115";//Approved
                        returnServiceStatus(model.registrationNo, status_code);//virendra
                        #endregion

                        SendSMSUpdateProcessFIC(model.registrationNo, res.MobileNo, model.appStatus);
                    }
                    return RedirectToAction("UpdateAppProcessFIC", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(model.registrationNo), status = model.appStatus });
                }
                else
                {
                    if (model.inspectionRejectedRemark != null)
                    {
                        ModelState["rejectedRemarks"].Errors.Clear();
                        //ModelState["inspectionDate"].Errors.Clear();
                        ModelState["inspectionRpt"].Errors.Clear();
                        model.appStatus = 4;//reject
                        var res = objCHCDB.FICAppStatusInsertUpdate(model.regisIdFIC, model.appStatus, model.rejectedRemarks, model.inspectionRptPath, model.inspectionRejectedRemark, model.certGenerateByDoc, model.certGenerateByDesig, model.inspecttionCompeletionDate, model.identificationMark, model.regByUser, model.transIp);
                        if (res != null)
                        {
                            setSweetAlertMsg(res.Msg.ToString(), "error");
                            TempData["msg"] = res.Msg.ToString();
                            TempData["msgstatus"] = "success";
                            #region ISP
                            status_code = "s110";//rejected
                            returnServiceStatus(model.registrationNo, status_code);
                            #endregion
                            SendSMSUpdateProcessFIC(model.registrationNo, res.MobileNo, model.appStatus);
                        }
                        return RedirectToAction("RejectedApplicationFIC");
                    }
                }
            }
            else if (model.appStatus == 5)
            {
                ModelState["rejectedRemarks"].Errors.Clear();
                //ModelState["inspectionDate"].Errors.Clear();
                ModelState["inspectionRejectedRemark"].Errors.Clear();
                model.appStatus = 6;
                var res = objCHCDB.FICAppStatusInsertUpdate(model.regisIdFIC, model.appStatus, model.rejectedRemarks, model.inspectionRptPath, model.inspectionRejectedRemark, model.certGenerateByDoc, model.certGenerateByDesig, model.inspecttionCompeletionDate, model.identificationMark, model.regByUser, model.transIp);
                if (res != null)
                {
                    setSweetAlertMsg(res.Msg.ToString(), "error");
                    TempData["msg"] = res.Msg.ToString();
                    TempData["msgstatus"] = "success";
                    TempData["DatSetFIC"] = res.RegisId;

                    #region ISP

                    status_code = "s113"; //Certificate issued 

                    returnServiceStatus(model.registrationNo, status_code);
                    #endregion
                }
                return RedirectToAction("UpdateAppProcessFIC", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(model.registrationNo), status = model.appStatus });
            }

            return View();

        }

        [AuthorizeAdmin(3)]
        public ActionResult InProcessApplicationFIC()//All In process Application
        {
            ViewBag.DLLAppStatus = comndb.GetApplicationProcessByAppName("FIC").Where(m => m.Value == "5").ToList();
            return View();
        }

        [AuthorizeAdmin(3)]
        public ActionResult InProcessApplicationListFIC(string registrationNo = "", string status = "", string requestDate = "")//LIst Of all In Process Application
        {
            int intStatus = 0;
            List<FICDetailsModel> lstFICDetails = new List<FICDetailsModel>();

            //int institutionTypeId = Convert.ToInt32(objSM.UserID);
            int institutionTypeId;
            if (objSM.RollID == 8)
            {
                institutionTypeId = 0;
            }
            else
            {
                institutionTypeId = Convert.ToInt32(objSM.UserID);
            }
            lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.appStatus == 2 || m.appStatus == 3 || m.appStatus == 5).ToList();

            if (!string.IsNullOrEmpty(registrationNo) && !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(requestDate))//111
            {
                intStatus = Convert.ToInt32(status);
                lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.appStatus == intStatus && m.registrationNo == registrationNo && m.applicationDate == requestDate).ToList();
            }
            else if (!string.IsNullOrEmpty(registrationNo) && string.IsNullOrEmpty(status) && string.IsNullOrEmpty(requestDate))//100
            {
                lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.registrationNo == registrationNo && (m.appStatus == 2 || m.appStatus == 3 || m.appStatus == 5)).ToList();
            }
            else if (string.IsNullOrEmpty(registrationNo) && !string.IsNullOrEmpty(status) && string.IsNullOrEmpty(requestDate))//010
            {
                intStatus = Convert.ToInt32(status);
                lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.appStatus == intStatus).ToList();
            }
            else if (string.IsNullOrEmpty(registrationNo) && string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(requestDate))//001
            {

                lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.applicationDate == requestDate && (m.appStatus == 2 || m.appStatus == 3 || m.appStatus == 5)).ToList();
            }
            else if (!string.IsNullOrEmpty(registrationNo) && !string.IsNullOrEmpty(status) && string.IsNullOrEmpty(requestDate))//110
            {
                intStatus = Convert.ToInt32(status);
                lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.registrationNo == registrationNo && m.appStatus == intStatus).ToList();
            }
            else if (string.IsNullOrEmpty(registrationNo) && !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(requestDate))//011
            {
                intStatus = Convert.ToInt32(status);
                lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.appStatus == intStatus && m.applicationDate == requestDate).ToList();
            }
            else if (!string.IsNullOrEmpty(registrationNo) && string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(requestDate))//101
            {

                lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.registrationNo == registrationNo && m.applicationDate == requestDate && (m.appStatus == 2 || m.appStatus == 3 || m.appStatus == 5)).ToList();
            }


            return PartialView("_InProcessApplicationListFIC", lstFICDetails);
        }

        [AuthorizeAdmin(3)]
        public ActionResult ApprovedApplicationFIC()//All Approved Application
        {
            ViewBag.DLLAppStatus = comndb.GetApplicationProcessByAppName("FIC").Where(m => m.Value == "6").ToList();
            return View();
        }

        [AuthorizeAdmin(3)]
        public ActionResult ApprovedApplicationListFIC(string registrationNo = "", string requestDate = "")//LIst Of Approved Application
        {
            int intStatus = 0;
            List<FICDetailsModel> lstFICDetails = new List<FICDetailsModel>();

            //int institutionTypeId = Convert.ToInt32(objSM.UserID);
            int institutionTypeId;
            if (objSM.RollID == 8)
            {
                institutionTypeId = 0;
            }
            else
            {
                institutionTypeId = Convert.ToInt32(objSM.UserID);
            }
            lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.appStatus == 6).ToList();

            if (!string.IsNullOrEmpty(registrationNo) && !string.IsNullOrEmpty(requestDate))//11
            {

                lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.registrationNo == registrationNo && m.applicationDate == requestDate && (m.appStatus == 6)).ToList();
            }
            else if (!string.IsNullOrEmpty(registrationNo) && string.IsNullOrEmpty(requestDate))//10
            {
                lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.registrationNo == registrationNo && (m.appStatus == 6)).ToList();
            }
            else if (string.IsNullOrEmpty(registrationNo) && !string.IsNullOrEmpty(requestDate))//01
            {

                lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.applicationDate == requestDate && (m.appStatus == 6)).ToList();
            }


            return PartialView("_ApprovedApplicationListFIC", lstFICDetails);
        }

        [AuthorizeAdmin(3)]
        public ActionResult RejectedApplicationFIC()//All Rejected Application
        {
            ViewBag.DLLAppStatus = comndb.GetApplicationProcessByAppName("FIC").Where(m => m.Value == "0" || m.Value == "4").ToList();
            return View();
        }

        [AuthorizeAdmin(3)]
        public ActionResult RejectedApplicationListFIC(string registrationNo = "", string requestDate = "")//LIst Of Rejected Application
        {
            int intStatus = 0;
            List<FICDetailsModel> lstFICDetails = new List<FICDetailsModel>();

            //int institutionTypeId = Convert.ToInt32(objSM.UserID);
            int institutionTypeId;
            if (objSM.RollID == 8)
            {
                institutionTypeId = 0;
            }
            else
            {
                institutionTypeId = Convert.ToInt32(objSM.UserID);
            }
            lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.appStatus == 0 || m.appStatus == 4).ToList();

            if (!string.IsNullOrEmpty(registrationNo) && !string.IsNullOrEmpty(requestDate))//11
            {

                lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.registrationNo == registrationNo && m.applicationDate == requestDate && (m.appStatus == 0 || m.appStatus == 4)).ToList();
            }
            else if (!string.IsNullOrEmpty(registrationNo) && string.IsNullOrEmpty(requestDate))//10
            {
                lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.registrationNo == registrationNo && (m.appStatus == 0 || m.appStatus == 4)).ToList();
            }
            else if (string.IsNullOrEmpty(registrationNo) && !string.IsNullOrEmpty(requestDate))//01
            {

                lstFICDetails = objCHCDB.GetAllFICListchc(institutionTypeId).Where(m => m.applicationDate == requestDate && (m.appStatus == 0 || m.appStatus == 4)).ToList();
            }

            return PartialView("_RejectedApplicationListFIC", lstFICDetails);
        }

        [AuthorizeAdmin(3)]
        public ActionResult FitnessDetailFIC(string registration)//list of All Received Application
        {
            FICModel model = new FICModel();
            model = objFICdb.GetFICListBYRegistrationNo(registration);

            return PartialView("_FitnessDetailFIC", model);
        }

        public JsonResult UploadFile(HttpPostedFileBase File)
        {


            string Dirpath = "~/Content/writereaddata/FIC/InspectionReport/";
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

        [HttpGet]
        public string SendSMSUpdateProcessFIC(string registrationNo, string mobileNo, int msgType)
        {
            string res = "";
            string txtmsg = "";
            string CHC = objSM.DisplayName;
            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();

                if (registrationNo != null)
                {
                    if (msgType == 0 || msgType == 4)
                    {
                        txtmsg = "Dear Citizen,\n\nYour Application Form Number " + registrationNo + " for Issuance of Medical (Fitness) Certificate has been rejected. Kindly get in touch with the office of Community Health Centre/ District Hospital for more details. You can also login to your dashboard for more details.\n\n" + CHC + "\n MHFWD, UP";

                    }
                    //else
                    //    if (msgType == 1)
                    //    {
                    //        txtmsg = "Dear User, your Application " + registrationNo + " has been Approved. For more informnation please login to your account.";
                    //    }
                    //else
                    //    if (msgType == 2)
                    //    {
                    //        txtmsg = "Dear Citizen,\n\nAs per your application for Issuance of Medical Certificate (Fitness), a committee for Inspection has been scheduled. We request to kindly be present at Office of Community Health Centre /District Hospital on the inspection date " + inspectionDate + " and coordinate accordingly.\n\n" + CHC + "\n MHFWD, UP";
                    //    }
                    //else
                    //    if (msgType == 5)
                    //    {
                    //        txtmsg = "Dear User, your Inspection Report for application number " + registrationNo + " has been approved. For more informnation please login to your account.";
                    //    }
                    else
                        if (msgType == 6)
                    {
                        txtmsg = "Dear Citizen,\n\nYour Application Form Number " + registrationNo + " for Issuance of Medical (Fitness) Certificate has been approved. Please get in touch with the office of Community Health Centre/ District Hospital to collect your certificate. You can also download the Certificate from your dashboard.\n\n" + CHC + "\n MHFWD, UP";
                    }
                    string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                    objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
                    if (status.ToLower() == "success")
                    {
                        res = "S";
                        try
                        {

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

        [AuthorizeAdmin]
        public ActionResult PrintApplicationFormFIC(string registrationNo, string regisIdDIC)
        {
            long regisByuser = objSM.UserID;
            registrationNo = OTPL_Imp.CustomCryptography.Decrypt(registrationNo);
            FICModel model = new FICModel();
            model = objFICdb.GetFICListBYRegistrationNo(registrationNo);

            if (objSM.RollID >= 3 && objSM.RollID <= 5)
            {
                if (model == null || objSM.UserID != model.forwardtoId)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (objSM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.districtId).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (objSM.RollID != 8)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        #region Certificate Report
        [AuthorizeAdmin(3)]
        public ActionResult FICgeneratedCertificate(long regisId)
        {
            string setPdfName = "", setDigitalPdfName = "";

            var res = objCHCDB.GetFICdetailCertificateRpt(regisId);

            if (res != null && objSM.UserID != res[0].forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            if (res != null && res.Count > 0)
            {
                try
                {
                    ReportDocument rd = new ReportDocument();

                    rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rpt_FICcertificate.rpt"));
                    rd.SetDataSource(res);

                    setPdfName = "UnSigned_" + res[0].certificateNo;

                    string folderpath = "~/Content/writereaddata/UnSignedCertificate/FIC/" + objSM.DistrictName + "/";

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
                    if (System.IO.File.Exists(Server.MapPath(flName)))
                    {
                        int result = objCHCDB.InsertUnSignedCertiPath_FIC(res[0].regisIdFIC, flName);
                    }
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
                    }
                    else
                    {
                        //digital sign

                        setDigitalPdfName = "FitnessCertificateDigitalSigned" + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        string digitalFlName = folderpath + setDigitalPdfName + ".pdf";

                        var sigDetails = comndb.GetDigitalSignatureDetails(objSM.UserID);

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

                        //digital sign end
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error Occour to Downloading.";
                }
            }
            else
            {
                ViewBag.Message = "Detail not found.";
            }

            return View("DownloadFile");
        }
        #endregion

        #endregion

        #region ILC

        [AuthorizeAdmin(2)]
        public ActionResult AppliedApplicationILC()//CHC ILC dashboard
        {
            ILCDetailsModel model = new ILCDetailsModel();
            int institutionTypeId;
            if (objSM.RollID == 8)
            {
                institutionTypeId = 0;
            }
            else
            {
                institutionTypeId = Convert.ToInt32(objSM.UserID);
            }

            model = objCHCDB.getApplicationCountILC(institutionTypeId);
            return View(model);
        }

        [AuthorizeAdmin(2)]
        public ActionResult ReceivedApplicationILC()//All Received Application
        {
            ViewBag.DLLAppStatus = comndb.GetApplicationProcessByAppName("ILC").Where(m => m.Value == "-1").ToList();
            return View();
        }

        [AuthorizeAdmin(2)]
        public ActionResult ReceivedApplicationListILC(string registrationNo = "", string appType = "", string requestDate = "")//list of All Received Application
        {

            List<ILCDetailsModel> lstILCDetails = new List<ILCDetailsModel>();

            //int institutionTypeId = Convert.ToInt32(objSM.UserID);
            int institutionTypeId;
            if (objSM.RollID == 8)
            {
                institutionTypeId = 0;
            }
            else
            {
                institutionTypeId = Convert.ToInt32(objSM.UserID);
            }

            if (registrationNo != "" || appType != "" || requestDate != "")
            {
                if (appType == "")
                {
                    lstILCDetails = objCHCDB.GetAllLICList(3, registrationNo, 0, requestDate, 0, institutionTypeId).ToList();
                }
                else
                {
                    lstILCDetails = objCHCDB.GetAllLICList(3, registrationNo, Convert.ToInt16(appType), requestDate, 0, institutionTypeId).ToList();
                }
            }
            else
            {
                lstILCDetails = objCHCDB.GetAllLICList(3, registrationNo, 0, requestDate, 0, institutionTypeId).ToList();
            }

            return PartialView("_ReceivedApplicationListILC", lstILCDetails);

        }

        [AuthorizeAdmin(2)]
        [HttpGet]
        public ActionResult UpdateAppProcessILC(string regisId, int status)
        {
            ILCDetailsModel model = new ILCDetailsModel();
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);

            model = objCHCDB.GetILCList(regisId, status);

            if (model == null || objSM.UserID != model.forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            if (model.appStatus == 1)
            {
                model.appStatus = 2;
            }
            else if (model.appStatus == 3)
            {
                model.appStatus = 5;
            }
            return View(model);
        }

        [AuthorizeAdmin(2)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAppProcessILC(ILCDetailsModel model)//Update Status For FIC
        {

            int appStatus = 0;
            #region ISP
            string status_code = "";
            #endregion
            if (model.appStatus == 2)
            {
                appStatus = 1;
            }
            else
            {
                appStatus = model.appStatus;
            }

            var result = objCHCDB.GetILCList(model.registrationNo, appStatus);

            if (result == null || objSM.UserID != result.forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            model.regByUser = Convert.ToInt32(objSM.UserID);
            model.transIp = Common.GetIPAddress();
            if (model.appStatus == -1)//pending
            {
                if (model.rejectedRemarks != null)
                {
                    ModelState["inspectionDate"].Errors.Clear();
                    ModelState["inspectionRpt"].Errors.Clear();
                    ModelState["inspectionRejectedRemark"].Errors.Clear();
                    model.appStatus = 0;
                    var res = objCHCDB.ILCAppStatusInsertUpdate(model.regisIdILC, model.appStatus, model.rejectedRemarks, model.inspectionDate, model.inspectionRptPath, model.inspectionRejectedRemark, model.certGenerateByDoc, model.certGenerateByDesig, model.inspecttionCompeletionDate, model.diseaseName, model.identificationMark, model.bedRest, model.regByUser, model.transIp);
                    if (res != null)
                    {
                        setSweetAlertMsg(res.Msg.ToString(), "error");
                        TempData["msg"] = res.Msg.ToString();
                        TempData["msgstatus"] = "success";
                        #region ISP
                        status_code = "s110";//rejected
                        returnServiceStatus(model.registrationNo, status_code);//virendra
                        #endregion
                        SendSMSUpdateProcessILC(model.registrationNo, model.inspectionDate, res.MobileNo, model.appStatus);
                    }
                    return RedirectToAction("RejectedApplicationILC");
                }
                else
                {
                    ModelState["rejectedRemarks"].Errors.Clear();
                    ModelState["inspectionDate"].Errors.Clear();
                    ModelState["inspectionRpt"].Errors.Clear();
                    ModelState["inspectionRejectedRemark"].Errors.Clear();
                    model.appStatus = 1;
                    #region ISP
                    status_code = "s111";//Application Accepted
                    returnServiceStatus(model.registrationNo, status_code);
                    #endregion
                    var res = objCHCDB.ILCAppStatusInsertUpdate(model.regisIdILC, model.appStatus, model.rejectedRemarks, model.inspectionDate, model.inspectionRptPath, model.inspectionRejectedRemark, model.certGenerateByDoc, model.certGenerateByDesig, model.inspecttionCompeletionDate, model.diseaseName, model.identificationMark, model.bedRest, model.regByUser, model.transIp);
                    if (res != null)
                    {
                        setSweetAlertMsg(res.Msg.ToString(), "error");
                        TempData["msg"] = res.Msg.ToString();
                        TempData["msgstatus"] = "success";

                    }
                    return RedirectToAction("InProcessApplicationILC");
                }

            }
            else
                if (model.appStatus == 1)//accept
            {
                ModelState["rejectedRemarks"].Errors.Clear();
                ModelState["inspectionRpt"].Errors.Clear();
                ModelState["inspectionRejectedRemark"].Errors.Clear();
                model.appStatus = 2;
                var res = objCHCDB.ILCAppStatusInsertUpdate(model.regisIdILC, model.appStatus, model.rejectedRemarks, model.inspectionDate, model.inspectionRptPath, model.inspectionRejectedRemark, model.certGenerateByDoc, model.certGenerateByDesig, model.inspecttionCompeletionDate, model.diseaseName, model.identificationMark, model.bedRest, model.regByUser, model.transIp);
                if (res != null)
                {
                    setSweetAlertMsg(res.Msg.ToString(), "error");
                    TempData["msg"] = res.Msg.ToString();
                    TempData["msgstatus"] = "success";

                    #region ISP
                    status_code = "s106";//pending for approval
                    returnServiceStatus(model.registrationNo, status_code);// zubair2
                    #endregion

                    SendSMSUpdateProcessILC(model.registrationNo, model.inspectionDate, res.MobileNo, model.appStatus);
                }
                return RedirectToAction("InProcessApplicationILC");
            }
            else
                    if (model.appStatus == 2)//inspection schedule
            {
                if (model.inspectionRejectedRemark == null)
                {
                    ModelState["rejectedRemarks"].Errors.Clear();
                    ModelState["inspectionDate"].Errors.Clear();
                    ModelState["inspectionRejectedRemark"].Errors.Clear();
                    model.appStatus = 5;
                    var res = objCHCDB.ILCAppStatusInsertUpdate(model.regisIdILC, model.appStatus, model.rejectedRemarks, model.inspectionDate, model.inspectionRptPath, model.inspectionRejectedRemark, model.certGenerateByDoc, model.certGenerateByDesig, model.inspecttionCompeletionDate, model.diseaseName, model.identificationMark, model.bedRest, model.regByUser, model.transIp);
                    if (res != null)
                    {
                        setSweetAlertMsg(res.Msg.ToString(), "error");
                        TempData["msg"] = res.Msg.ToString();
                        TempData["msgstatus"] = "success";
                        #region ISP
                        status_code = "s115"; //Application approved
                        returnServiceStatus(model.registrationNo, status_code); //zubair3
                        #endregion
                    }
                    return RedirectToAction("UpdateAppProcessILC", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(model.registrationNo), status = model.appStatus });

                }
                else
                {
                    ModelState["rejectedRemarks"].Errors.Clear();
                    ModelState["inspectionDate"].Errors.Clear();
                    ModelState["inspectionRpt"].Errors.Clear();
                    model.appStatus = 4;//reject
                    var res = objCHCDB.ILCAppStatusInsertUpdate(model.regisIdILC, model.appStatus, model.rejectedRemarks, model.inspectionDate, model.inspectionRptPath, model.inspectionRejectedRemark, model.certGenerateByDoc, model.certGenerateByDesig, model.inspecttionCompeletionDate, model.diseaseName, model.identificationMark, model.bedRest, model.regByUser, model.transIp);
                    if (res != null)
                    {
                        setSweetAlertMsg(res.Msg.ToString(), "error");
                        TempData["msg"] = res.Msg.ToString();
                        TempData["msgstatus"] = "success";

                        #region ISP
                        status_code = "s110";//rejected
                        returnServiceStatus(model.registrationNo, status_code); //zubair4
                        #endregion

                        SendSMSUpdateProcessILC(model.registrationNo, model.inspectionDate, res.MobileNo, model.appStatus);
                    }
                    return RedirectToAction("RejectedApplicationILC");
                }
            }
            else
                        if (model.appStatus == 5)//inspection rpt uploaded
            {
                //if (model.inspectionRptStatus == true)//POSITIVE
                //{
                ModelState["rejectedRemarks"].Errors.Clear();
                ModelState["inspectionDate"].Errors.Clear();
                ModelState["inspectionRpt"].Errors.Clear();
                ModelState["inspectionRejectedRemark"].Errors.Clear();
                model.appStatus = 6;//CERTIFICATE GEN
                var res = objCHCDB.ILCAppStatusInsertUpdate(model.regisIdILC, model.appStatus, model.rejectedRemarks, model.inspectionDate, model.inspectionRptPath, model.inspectionRejectedRemark, model.certGenerateByDoc, model.certGenerateByDesig, model.inspecttionCompeletionDate, model.diseaseName, model.identificationMark, model.bedRest, model.regByUser, model.transIp);
                if (res != null)
                {
                    setSweetAlertMsg(res.Msg.ToString(), "error");
                    TempData["msg"] = res.Msg.ToString();
                    TempData["msgstatus"] = "success";
                    TempData["DatSetILC"] = res.RegisId;
                    SendSMSUpdateProcessILC(model.registrationNo, model.inspectionDate, res.MobileNo, model.appStatus);
                    #region ISP
                    int r = ILCgeneratedCertificateInt(model.regisIdILC);

                    if (r == 1) // Changed by  Hemendra   
                    {

                        status_code = "s113"; //Certificate issued
                        returnServiceStatus(model.registrationNo, status_code);//zubair5

                    }

                    #endregion
                }
                return RedirectToAction("UpdateAppProcessILC", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(model.registrationNo), status = model.appStatus });
                //return RedirectToAction("ApprovedApplicationILC");
                //}
                //else
                //{
                //    if (model.inspectionRejectedRemark != null)
                //    {
                //        ModelState["rejectedRemarks"].Errors.Clear();
                //        ModelState["inspectionDate"].Errors.Clear();
                //        ModelState["inspectionRpt"].Errors.Clear();
                //        model.appStatus = 4;//reject
                //        var res = objCHCDB.ILCAppStatusInsertUpdate(model.regisIdILC, model.appStatus, model.rejectedRemarks, model.inspectionDate, model.inspectionRptPath, model.inspectionRejectedRemark, model.certGenerateByDoc, model.certGenerateByDesig, model.inspecttionCompeletionDate, model.diseaseName, model.identificationMark, model.bedRest, model.regByUser, model.transIp);
                //        if (res != null)
                //        {
                //            setSweetAlertMsg(res.Msg.ToString(), "error");
                //            TempData["msg"] = res.Msg.ToString();
                //            TempData["msgstatus"] = "success";

                //            SendSMSUpdateProcessILC(model.registrationNo, model.inspectionDate, res.MobileNo, model.appStatus);
                //        }
                //        return RedirectToAction("RejectedApplicationILC");
                //    }
                //}
            }

            return View();

        }

        [AuthorizeAdmin(2)]
        public ActionResult InProcessApplicationILC()//All In process Application
        {
            //ViewBag.DLLAppStatus = comndb.GetApplicationProcessByAppName("NUH").Where(m => m.Value == "1" || m.Value == "2" || m.Value == "3").ToList();
            ViewBag.DLLAppStatus = comndb.GetApplicationProcessByAppName("ILC").Where(m => m.Value == "1" || m.Value == "5").ToList();
            return View();
        }

        [AuthorizeAdmin(2)]
        public ActionResult InProcessApplicationListILC(string registrationNo = "", string status = "", string requestDate = "", string appType = "")//LIst Of all In Process Application
        {
            List<ILCDetailsModel> lstILCDetails = new List<ILCDetailsModel>();
            int Appstatus = 0;
            //int institutionTypeId = Convert.ToInt32(objSM.UserID);
            int institutionTypeId;
            if (objSM.RollID == 8)
            {
                institutionTypeId = 0;
            }
            else
            {
                institutionTypeId = Convert.ToInt32(objSM.UserID);
            }
            if (status != "")
            {
                Appstatus = Convert.ToInt16(status);
            }
            if (registrationNo != "" || appType != "" || requestDate != "" || Appstatus != 0)
            {
                if (appType == "")
                {
                    lstILCDetails = objCHCDB.GetAllLICList(4, registrationNo, 0, requestDate, Appstatus, institutionTypeId).ToList();
                }
                else
                {
                    lstILCDetails = objCHCDB.GetAllLICList(4, registrationNo, Convert.ToInt16(appType), requestDate, Appstatus, institutionTypeId).ToList();
                }
            }
            else
            {
                lstILCDetails = objCHCDB.GetAllLICList(4, registrationNo, 0, requestDate, Appstatus, institutionTypeId).ToList();
            }

            return PartialView("_ReceivedApplicationListILC", lstILCDetails);

        }

        [AuthorizeAdmin(2)]
        public ActionResult ApprovedApplicationILC()//All Approved Application
        {
            ViewBag.DLLAppStatus = comndb.GetApplicationProcessByAppName("ILC").Where(m => m.Value == "6").ToList();
            return View();
        }

        [AuthorizeAdmin(2)]
        public ActionResult ApprovedApplicationListILC(string registrationNo = "", string requestDate = "", string appType = "")//LIst Of Approved Application
        {

            List<ILCDetailsModel> lstILCDetails = new List<ILCDetailsModel>();

            //int institutionTypeId = Convert.ToInt32(objSM.UserID);
            int institutionTypeId;
            if (objSM.RollID == 8)
            {
                institutionTypeId = 0;
            }
            else
            {
                institutionTypeId = Convert.ToInt32(objSM.UserID);
            }


            if (registrationNo != "" || appType != "" || requestDate != "")
            {
                if (appType == "")
                {
                    lstILCDetails = objCHCDB.GetAllLICList(2, registrationNo, 0, requestDate, 0, institutionTypeId).ToList();
                }
                else
                {
                    lstILCDetails = objCHCDB.GetAllLICList(2, registrationNo, Convert.ToInt16(appType), requestDate, 0, institutionTypeId).ToList();
                }
            }
            else
            {
                lstILCDetails = objCHCDB.GetAllLICList(1, registrationNo, 0, requestDate, 0, institutionTypeId).ToList();
            }

            return PartialView("_ApprovedApplicationListILC", lstILCDetails);

        }

        [AuthorizeAdmin(2)]
        public ActionResult RejectedApplicationILC()//All Rejected Application
        {
            ViewBag.DLLAppStatus = comndb.GetApplicationProcessByAppName("ILC").Where(m => m.Value == "0" || m.Value == "4").ToList();
            return View();
        }

        [AuthorizeAdmin(2)]
        public ActionResult RejectedApplicationListILC(string registrationNo = "", string requestDate = "", string appType = "")//LIst Of Rejected Application
        {

            List<ILCDetailsModel> lstILCDetails = new List<ILCDetailsModel>();

            //int institutionTypeId = Convert.ToInt32(objSM.UserID);
            int institutionTypeId;
            if (objSM.RollID == 8)
            {
                institutionTypeId = 0;
            }
            else
            {
                institutionTypeId = Convert.ToInt32(objSM.UserID);
            }

            if (registrationNo != "" || appType != "" || requestDate != "")
            {
                if (appType == "")
                {
                    lstILCDetails = objCHCDB.GetAllLICList(2, registrationNo, 0, requestDate, 0, institutionTypeId).ToList();
                }
                else
                {
                    lstILCDetails = objCHCDB.GetAllLICList(2, registrationNo, Convert.ToInt16(appType), requestDate, 0, institutionTypeId).ToList();
                }

            }
            else
            {
                lstILCDetails = objCHCDB.GetAllLICList(2, registrationNo, 0, requestDate, 0, institutionTypeId).ToList();
            }

            return PartialView("_RejectedApplicationListILC", lstILCDetails);
        }

        [AuthorizeAdmin(2)]
        public ActionResult MedicalCertificateILC(string registration)//list of All Received Application
        {
            ILCModel model = new ILCModel();
            model = objILC_DB.GetILCListBYRegistrationNo(Convert.ToInt64(registration));

            return PartialView("_MedicalCertificateILC", model);
        }

        public JsonResult UploadFileILC(HttpPostedFileBase File)
        {


            string Dirpath = "~/Content/writereaddata/ILC/InspectionReport/";
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

        public string SendSMSUpdateProcessILC(string registrationNo, string inspectionDate, string mobileNo, int msgType)
        {
            string res = "";
            string txtmsg = "";
            string CHC = objSM.DisplayName;
            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();

                if (registrationNo != null)
                {
                    if (msgType == 0 || msgType == 4)
                    {
                        txtmsg = "Dear Citizen,\n\nYour Application Form Number " + registrationNo + " for Issuance of Medical (Illness) Certificate has been rejected. Kindly get in touch with the office of Community Health Centre/ District Hospital for more details. You can also login to your dashboard for more details.\n\n" + CHC + "\n MHFWD, UP";

                    }
                    //else
                    //    if (msgType == 1)
                    //    {
                    //        txtmsg = "Dear User, your Application " + registrationNo + " has been Approved. For more informnation please login to your account.";
                    //    }
                    else
                        if (msgType == 2)
                    {
                        txtmsg = "Dear Citizen,\n\nAs per your application for Issuance of Medical Certificate (Illness), a committee for Inspection has been scheduled. We request to kindly be present at Office of Community Health Centre /District Hospital on the inspection date  " + inspectionDate + " and coordinate accordingly.\n\n" + CHC + "\n MHFWD, UP";
                    }
                    //else
                    //    if (msgType == 5)
                    //    {
                    //        txtmsg = "Dear User, your Inspection Report for application number " + registrationNo + " has been approved. For more informnation please login to your account.";
                    //    }
                    else
                            if (msgType == 6)
                    {
                        txtmsg = "Dear Citizen,\n\nYour Application Form Number " + registrationNo + " for Issuance of Medical (Illness) Certificate has been approved. Please get in touch with the office of Community Health Centre/ District Hospital to collect your certificate. You can also download the Certificate from your dashboard.\n\n" + CHC + "\n MHFWD, UP";
                    }
                    //string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));  commented on 19/07/2023
                    string status = "success";
                    //objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
                    if (status.ToLower() == "success")
                    {
                        res = "S";
                        try
                        {

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

        [AuthorizeAdmin]
        public ActionResult PrintApplicationFormILC(string regisId)
        {
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            ILCModel model = new ILCModel();
            model = objILC_DB.GetILCListBYRegistrationNo(Convert.ToInt64(regisId));

            if (objSM.RollID >= 3 && objSM.RollID <= 5)
            {
                if (model == null || objSM.UserID != model.forwardtoId)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (objSM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.districtId).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (objSM.RollID != 8)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }
        #region Certificate Report with int return type, Hemendra 
        public int ILCgeneratedCertificateInt(long regisId)
        {
            string setPdfName = "", setDigitalPdfName = "";

            var res = objCHCDB.GetILCdetailCertificateRpt(regisId);

            if (res != null && objSM.UserID != res[0].forwardtoId)
            {
                return -1; // Unauthorized access
            }

            if (res != null && res.Count > 0)
            {
                try
                {
                    ReportDocument rd = new ReportDocument();
                    if (res[0].oldCertificateNumber == null || res[0].oldCertificateNumber == "")
                    {
                        rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rpt_ILCcertificate.rpt"));
                    }
                    else
                    {
                        rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rpt_ILCextendedCertificate.rpt"));
                    }
                    rd.SetDataSource(res);

                    setPdfName = "UnSigned_" + res[0].certificateNo;

                    string folderpath = "~/Content/writereaddata/UnSignedCertificate/ILC/" + objSM.DistrictName + "/";

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
                    if (System.IO.File.Exists(Server.MapPath(flName)))
                    {
                        int result = objCHCDB.InsertUnSignedCertiPath_ILC(res[0].regisIdILC, flName);
                    }
                    rd.Close();
                    rd.Dispose();

                    if (ConfigurationManager.AppSettings["IsDigitalSign"].ToString() == "N")
                    {
                        //FileInfo fileInfo = new FileInfo(Server.MapPath(flName));
                        //Response.ClearContent();
                        //Response.ClearHeaders();
                        //Response.ContentType = "application/pdf";
                        //Response.AppendHeader("Content-Disposition", "attachment; filename=" + setPdfName + ".pdf");
                        //Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
                        //Response.WriteFile(flName);
                        //Response.Flush();
                        //Response.Close();
                        return 1; // Success without digital sign
                    }
                    else
                    {
                        // Digital sign

                        setDigitalPdfName = "IllnessCertificateDigitalSigned" + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        string digitalFlName = folderpath + setDigitalPdfName + ".pdf";

                        var sigDetails = comndb.GetDigitalSignatureDetails(objSM.UserID);

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

                        return 1; // Success with digital sign
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error Occurred to Downloading.";
                    return -2; // Error occurred
                }
            }
            else
            {
                ViewBag.Message = "Detail not found.";
                return -3; // Detail not found
            }
        }

        #endregion


        #region Certificate Report
        [AuthorizeAdmin(2)]
        #endregion
        public ActionResult ILCgeneratedCertificate(long regisId)
        {
            string setPdfName = "", setDigitalPdfName = "";

            var res = objCHCDB.GetILCdetailCertificateRpt(regisId);

            if (res != null && objSM.UserID != res[0].forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            if (res != null && res.Count > 0)
            {
                try
                {
                    ReportDocument rd = new ReportDocument();
                    if (res[0].oldCertificateNumber == null || res[0].oldCertificateNumber == "")
                    {
                        rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rpt_ILCcertificate.rpt"));
                    }
                    else
                    {
                        rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rpt_ILCextendedCertificate.rpt"));
                    }
                    rd.SetDataSource(res);

                    setPdfName = "UnSigned_" + res[0].certificateNo;

                    string folderpath = "~/Content/writereaddata/UnSignedCertificate/ILC/" + objSM.DistrictName + "/";

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
                    if (System.IO.File.Exists(Server.MapPath(flName)))
                    {
                        int result = objCHCDB.InsertUnSignedCertiPath_ILC(res[0].regisIdILC, flName);
                    }
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
                    }
                    else
                    {
                        //digital sign

                        setDigitalPdfName = "IllnessCertificateDigitalSigned" + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        string digitalFlName = folderpath + setDigitalPdfName + ".pdf";

                        var sigDetails = comndb.GetDigitalSignatureDetails(objSM.UserID);

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

                        //digital sign end
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error Occour to Downloading.";
                }
            }
            else
            {
                ViewBag.Message = "Detail not found.";
            }

            return View("DownloadFile");
        }
        #endregion



        #region DEC

        [AuthorizeAdmin(6)]
        public ActionResult AppliedApplicationDEC()//CHC DEC dashboard
        {
            DECModel model = new DECModel();
            //model.institutionTypeId = Convert.ToInt32(objSM.UserID);
            if (objSM.RollID == 8)
            {
                model.institutionTypeId = 0;
            }
            else
            {
                model.institutionTypeId = Convert.ToInt32(objSM.UserID);
            }
            model = objCHCDB.getApplicationCountDEC(model.institutionTypeId);
            return View(model);
        }

        [AuthorizeAdmin(6)]
        public ActionResult ReceivedApplicationDEC()
        {
            ViewBag.DLLAppStatus = comndb.GetApplicationProcessByAppName("NUH").Where(m => m.Value == "-1" || m.Value == "1").ToList();
            return View();
        }

        [AuthorizeAdmin(6)]
        public ActionResult ReceivedApplicationListDEC(string registrationNo = "", string appDate = "")
        {
            List<DECModel> DECModelList = new List<DECModel>();
            DataTable Ids = new DataTable();
            int id;
            if (objSM.RollID == 8)
            {
                id = 0;
            }
            else
            {
                id = Convert.ToInt32(objSM.UserID);
            }

            DECModelList = objCHCDB.GetAllRegistration_DEC(id, registrationNo, appDate).Where(m => m.appStatus == -1).ToList();


            return PartialView("_ReceivedApplicationListDEC", DECModelList);
        }

        [AuthorizeAdmin(6)]
        public ActionResult ApprovedApplicationDEC()
        {
            return View();
        }

        [AuthorizeAdmin(6)]
        public ActionResult ApprovedApplicationListDEC(string registrationNo = "", string appDate = "")
        {
            List<DECModel> DECModelList = new List<DECModel>();
            DataTable Ids = new DataTable();
            int id;
            if (objSM.RollID == 8)
            {
                id = 0;
            }
            else
            {
                id = Convert.ToInt32(objSM.UserID);
            }

            DECModelList = objCHCDB.GetAllRegistration_DEC(id, registrationNo, appDate).Where(m => m.appStatus == 1).ToList();

            return PartialView("_ApprovedApplicationListDEC", DECModelList);
        }

        [AuthorizeAdmin]
        public ActionResult PrintApplicationFormDEC(string regisIdDEC)
        {
            DECModel model = new DECModel();

            model = objdb.GetDECListCHC(Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdDEC)));

            if (objSM.RollID >= 3 && objSM.RollID <= 5)
            {
                if (model == null || objSM.UserID != model.forwardtoId)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (objSM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.districtid).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (objSM.RollID != 8)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            TempData["reg"] = model.registrationNo;
            Session["reg"] = model.registrationNo;
            return View(model);
        }

        [AuthorizeAdmin(6)]
        public ActionResult GenrateCertificateDEC(string regisId)
        {
            if (!string.IsNullOrEmpty(regisId))
            {
                DECModel model = new DECModel();

                model.appStatus = 1;
                model.regByuser = objSM.UserID;
                model.transIp = Common.GetIPAddress();
                model.regisIdDEC = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId));

                ResultSet resultData = objCHCDB.GenrateCertificateDEC(model);

                if (resultData != null && resultData.Flag > 0)
                {
                    return Content("success_" + model.regisIdDEC);
                }
                else
                {
                    return Content("error_0");
                }
            }
            else
            {
                return Content("error_0");
            }
        }

        [AuthorizeAdmin(6)]
        [HttpGet]
        public ActionResult ViewDownloadedCertificateDEC(string regisIdDEC)
        {
            DECModel model = new DECModel();
            model = objCHCDB.GetDECList(Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdDEC)));
            return View(model);
        }

        public ActionResult ViewDownloadedCertificateListDEC(string regisIdDEC)//list of All Received Application
        {
            List<DECModel> DECModelList = new List<DECModel>();
            DECModelList = objCHCDB.GetAllDECList(Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdDEC)));

            return PartialView("_ViewDownloadedCertificateListDEC", DECModelList);
        }

        #region DEC Certificate Rpt Riya
        [AuthorizeAdmin(6)]
        public ActionResult DECgeneratedCertificate(long regisIdDEC)
        {
            string setPdfName = "", setDigitalPdfName = "";
            var res = objdb.GetDECDetails(regisIdDEC);

            if (res != null && objSM.UserID != res[0].forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            if (res != null && res.Count > 0)
            {
                //  var res2 = objMlc.getMLCChilds(res[0].regisIdMLC);
                try
                {
                    ReportDocument rd = new ReportDocument();

                    rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rpt_DECcertificate.rpt"));
                    rd.SetDataSource(res);

                    // ReportDocument subShows = rd.Subreports["rpt_MLCchildCertificate.rpt"];
                    //  subShows.SetDataSource(res2);

                    setPdfName = "UnSigned_" + res[0].certificateNo;

                    string folderpath = "~/Content/writereaddata/UnSignedCertificate/DEC/" + objSM.DistrictName + "/";

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
                    if (System.IO.File.Exists(Server.MapPath(flName)))
                    {
                        int result = objdb.InsertUnSignedCertiPath_DEC(res[0].regisIdDEC, flName);
                    }
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
                    }
                    else
                    {
                        //digital sign

                        setDigitalPdfName = "DeathCertificateDigitalSigned" + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        string digitalFlName = folderpath + setDigitalPdfName + ".pdf";

                        var sigDetails = comndb.GetDigitalSignatureDetails(objSM.UserID);

                        float llx = 580;
                        float lly = 300;
                        float urx = 440;
                        float ury = 200;
                        Comman_Classes.DigitalCeritificateManager dcm = new Comman_Classes.DigitalCeritificateManager();
                        Comman_Classes.MetaData md = new Comman_Classes.MetaData()
                        {
                            Author = sigDetails.Author,
                            Title = "Death Certificate Authentication",
                            Subject = "Death Certificate",
                            Creator = sigDetails.Creator,
                            Producer = sigDetails.Producer,
                            Keywords = sigDetails.Keywords
                        };

                        string Signaturepath = Server.MapPath(sigDetails.Signaturepath);
                        dcm.signPDF(Server.MapPath(flName), Server.MapPath(digitalFlName), Signaturepath,
                         sigDetails.signpwd, "Authenticate Medco Legal Certificate", sigDetails.SigContact,
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

                        //digital sign end 
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error Occour to Downloading.";
                }
            }
            else
            {
                ViewBag.Message = "Detail not found.";
            }

            return View("DownloadFile");
        }
        #endregion
        #endregion

        #region IMC

        [AuthorizeAdmin(5)]
        public ActionResult AppliedApplicationIMC()
        {
            ProcessType model = new ProcessType();
            //int institutionTypeId = Convert.ToInt32(objSM.UserID);
            int institutionTypeId;
            if (objSM.RollID == 8)
            {
                institutionTypeId = 0;
            }
            else
            {
                institutionTypeId = Convert.ToInt32(objSM.UserID);
            }
            model = objCHCDB.getIMCprocessCount(institutionTypeId);
            return View(model);
        }

        [AuthorizeAdmin(5)]
        public ActionResult ReceivedApplicationIMC()
        {
            ViewBag.DLLAppStatus = comndb.GetApplicationProcessByAppName("IMC").Where(m => m.Value == "-1").ToList();


            return View();
        }

        [AuthorizeAdmin(5)]
        public ActionResult ReceivedApplicationListIMC(string registrationNo = "", string mobile = "", string requestDate = "", string status = "")
        {

            IMCModel model = new IMCModel();
            //int institutionTypeId = Convert.ToInt32(objSM.UserID);
            int institutionTypeId;
            if (objSM.RollID == 8)
            {
                institutionTypeId = 0;
            }
            else
            {
                institutionTypeId = Convert.ToInt32(objSM.UserID);
            }
            if (registrationNo != "" || mobile != "" || requestDate != "" | status != "")
            {
                if (status == "")
                {
                    model.IMCModelList = objCHCDB.GetAllIMCList(1, 0, registrationNo, mobile, requestDate, 0, institutionTypeId).ToList();

                }
                else
                {
                    model.IMCModelList = objCHCDB.GetAllIMCList(1, 0, registrationNo, mobile, requestDate, Convert.ToInt32(status), institutionTypeId).ToList();
                }
            }
            else
            {
                model.IMCModelList = objCHCDB.GetAllIMCList(1, 0, registrationNo, mobile, requestDate, 0, institutionTypeId).ToList();
            }


            return PartialView("_TotalReceivedListIMC", model.IMCModelList);
        }

        [AuthorizeAdmin(5)]
        public ActionResult InProcessApplicationIMC()
        {
            ViewBag.DLLAppStatus = comndb.GetApplicationProcessByAppName("IMC").Where(m => m.Value == "1" || m.Value == "3" || m.Value == "5").ToList();
            return View();
        }

        [AuthorizeAdmin(5)]
        public ActionResult InProcessApplicationListIMC(string registrationNo = "", string mobile = "", string requestDate = "", string status = "")
        {

            IMCModel model = new IMCModel();

            //int forwardType = Convert.ToInt32(objSM.UserID);
            int forwardType;
            if (objSM.RollID == 8)
            {
                forwardType = 0;
            }
            else
            {
                forwardType = Convert.ToInt32(objSM.UserID);
            }
            if (registrationNo != "" || mobile != "" || requestDate != "" | status != "")
            {
                if (status == "")
                {
                    model.IMCModelList = objCHCDB.GetAllInprocessIMCList(1, 0, registrationNo, mobile, requestDate, 0, forwardType).ToList();

                }
                else
                {
                    model.IMCModelList = objCHCDB.GetAllInprocessIMCList(1, 0, registrationNo, mobile, requestDate, Convert.ToInt32(status), forwardType).ToList();
                }
            }
            else
            {
                model.IMCModelList = objCHCDB.GetAllInprocessIMCList(1, 0, registrationNo, mobile, requestDate, 0, forwardType).ToList();
            }
            return PartialView("_InProcessApplicationListIMC", model.IMCModelList);
        }

        [AuthorizeAdmin(5)]
        public ActionResult ApprovedApplicationIMC()
        {

            return View();
        }

        [AuthorizeAdmin(5)]
        public ActionResult ApprovedApplicationListIMC(string registrationNo = "", string mobile = "", string requestDate = "")
        {

            IMCModel model = new IMCModel();
            //int forwardType = Convert.ToInt32(objSM.UserID);
            int forwardType;
            if (objSM.RollID == 8)
            {
                forwardType = 0;
            }
            else
            {
                forwardType = Convert.ToInt32(objSM.UserID);
            }
            if (registrationNo != "" || mobile != "" || requestDate != "")
            {

                model.IMCModelList = objCHCDB.GetAllApprovedIMCList(1, registrationNo, mobile, requestDate, forwardType).ToList();

            }
            else
            {
                model.IMCModelList = objCHCDB.GetAllApprovedIMCList(1, registrationNo, mobile, requestDate, forwardType).ToList();
            }
            return PartialView("_ApprovedApplicationListIMC", model.IMCModelList);
        }

        [AuthorizeAdmin(5)]
        public ActionResult RejectedApplicationIMC()
        {

            return View();
        }

        [AuthorizeAdmin(5)]
        public ActionResult RejectedApplicationListIMC(string registrationNo = "", string mobile = "", string requestDate = "")
        {
            IMCModel model = new IMCModel();
            //int forwardType = Convert.ToInt32(objSM.UserID);
            int forwardType;
            if (objSM.RollID == 8)
            {
                forwardType = 0;
            }
            else
            {
                forwardType = Convert.ToInt32(objSM.UserID);
            }
            if (registrationNo != "" || mobile != "" || requestDate != "")
            {
                model.IMCModelList = objCHCDB.GetAllRejectedIMCList(1, registrationNo, mobile, requestDate, forwardType).ToList();
            }
            else
            {
                model.IMCModelList = objCHCDB.GetAllRejectedIMCList(1, registrationNo, mobile, requestDate, forwardType).ToList();
            }
            return PartialView("_RejectedApplicationListIMC", model.IMCModelList);
        }

        [AuthorizeAdmin]
        public ActionResult IMCAppDetails(long regisIdIMC)
        {
            IMCModel model = new IMCModel();
            int institutionTypeId = Convert.ToInt32(objSM.UserID);
            model = objCHCDB.GetAllIMCList(2, regisIdIMC, "", "", "", 0, institutionTypeId).ToList().FirstOrDefault();

            return PartialView("_IMCAppDetails", model);
        }

        [AuthorizeAdmin]
        public ActionResult IMCDetailsById(long regisIdIMC)
        {
            IMCModel model = new IMCModel();
            int institutionTypeId = Convert.ToInt32(objSM.UserID);
            Session["regisIdIMC"] = regisIdIMC;

            model = objCHCDB.GetAllIMCList(2, regisIdIMC, "", "", "", 0, institutionTypeId).FirstOrDefault();
            //ViewBag.Vaccine = objIMC_DB.IMCVaccineById(Convert.ToInt64(regisIdIMC));
            return PartialView("_IMCDetail", model);
        }

        [AuthorizeAdmin(5)]
        [HttpGet]
        public ActionResult UpdateAppProcessIMC(string regisId)
        {
            IMCAppProcessModel model = new IMCAppProcessModel();

            if (regisId == null || regisId == "")
            {
                regisId = OTPL_Imp.CustomCryptography.Decrypt(Session["regisId"].ToString());
                model = objCHCDB.getIMCStatus(Convert.ToInt64(regisId));
            }
            else
            {
                Session["regisId"] = regisId;
                regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
                model = objCHCDB.getIMCStatus(Convert.ToInt64(regisId));
                if (model.appStatus == 1)
                {
                    model.appStatus = 2;
                    model.updateIMCList = objIMC_DB.getIMCimmunTableForCHC(Convert.ToInt64(regisId));
                }
                ViewBag.Vaccine = objIMC_DB.IMCVaccineById(Convert.ToInt64(regisId));
            }

            if (model == null || objSM.UserID != model.forwardType)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            return View(model);
        }

        [AuthorizeAdmin(5)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAppProcessIMC(IMCAppProcessModel model, FormCollection frm)
        {
            var result = objCHCDB.getIMCStatus(model.regisIdIMC);

            #region ISP

            string status_code = "";

            #endregion

            int appStatus = 0;
            if (model.appStatus == 2)
            {
                appStatus = 1;
            }
            else
            {
                appStatus = model.appStatus;
            }

            if (result != null && (objSM.UserID != result.forwardType || appStatus != result.appStatus))
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            string IpAddress = Common.GetIPAddress();
            long userId = objSM.UserID;
            string xmldata = "";
            //string[] vaccineId = frm.GetValues("vaccineId");
            //string[] ImmunizationDate = frm.GetValues("ImmunizationDate");
            if (model.appStatus == 3)
            {
                //#region bulkinsertion
                //xmldata = "<vaccines>";
                //for (int i = 0; i < vaccineId.Count(); i++)
                //{
                //    if (!string.IsNullOrEmpty(vaccineId[i]) && !string.IsNullOrEmpty(ImmunizationDate[i]))
                //    {
                //        xmldata += "<vaccine><vaccineId>" + vaccineId[i] + "</vaccineId>"

                //        + "<ImmunizationDate>" + ImmunizationDate[i] + "</ImmunizationDate></vaccine>";
                //    }
                //}
                //xmldata += "</vaccines>";


                //#endregion
            }
            if (model.appStatus == 2)
            {
                XElement XML = new XElement("table");
                if (model.updateIMCList.Count() > 0)
                {

                    for (int i = 0; i < model.updateIMCList.Count(); i++)
                    {
                        XElement elements = new XElement("Immunization",
                                    new XElement("vaccineId", model.updateIMCList[i].vaccineId)

                          , new XElement("immunizationRemark", model.updateIMCList[i].immunizationRemark)
                               );


                        XML.Add(elements);
                    }
                }

                model.ImmunXML = XML.ToString();
            }
            if (model.rejectedRemarks == null)
            {
                model.status = true;
            }
            else
            {
                model.status = false;
            }

            if (model.appStatus == -1 || model.appStatus == 3 || model.appStatus == 5)
            {
                ModelState["DH_PHC_CHCProofFile"].Errors.Clear();
                ModelState["DH_PHC_CHCDate"].Errors.Clear();
                ModelState["hospitalEstablishment"].Errors.Clear();
                ModelState["rejectedRemarks"].Errors.Clear();

            }
            if (model.appStatus == 1)
            {
                ModelState["rejectedRemarks"].Errors.Clear();
                ModelState["DH_PHC_CHCProofFile"].Errors.Clear();
                ModelState["DH_PHC_CHCDate"].Errors.Clear();
            }
            if (model.appStatus == 2)
            {
                if (model.updateIMCList.Count() > 0)
                {
                    for (int i = 0; i < model.updateIMCList.Count(); i++)
                    {
                        ModelState["updateIMCList[" + i + "].hospitalEstablishment"].Errors.Clear();
                        ModelState["updateIMCList[" + i + "].DH_PHC_CHCProofFile"].Errors.Clear();
                        ModelState["updateIMCList[" + i + "].rejectedRemarks"].Errors.Clear();
                        ModelState["updateIMCList[" + i + "].DH_PHC_CHCDate"].Errors.Clear();

                    }
                }
                ModelState["DH_PHC_CHCProofFile"].Errors.Clear();
                ModelState["DH_PHC_CHCDate"].Errors.Clear();
                ModelState["hospitalEstablishment"].Errors.Clear();
                ModelState["rejectedRemarks"].Errors.Clear();
            }
            if (ModelState.IsValid)
            {
                var res = objCHCDB.UpdateIMCProcess(model.regisIdIMC, model.appStatus, model.status, model.DH_PHC_CHCDate, model.hospitalEstablishment, model.DH_PHC_CHCProofFilePath, model.rejectedRemarks, userId, IpAddress, model.ImmunXML, xmldata);
                TempData["Msg"] = res.Msg.ToString();
                TempData["MsgStatus"] = "success";
                SendSMSUpdateProcessIMC(res.RegistrationNo, res.inspectionDate, res.MobileNo, res.appStatus);

                #region ISP
                if (model.appStatus == -1 && model.status == true)
                {

                    status_code = "s111";//Application Accepted 
                }
                else if (model.appStatus == -1 && model.status == false)
                {
                    status_code = "s110";//rejected
                }
                else if (model.appStatus == 2)
                {
                    // status_code = "s106";//pending for approval

                }
                else if (model.appStatus == 3 && model.status == true)
                {
                    status_code = "s115";//application approved
                }

                else if (model.appStatus == 3 && model.status == false)
                {
                    status_code = "s110";//rejected
                }


                else if (model.appStatus == 5)
                {
                    IMCgeneratedCertificateInt(model.regisIdIMC);
                    status_code = "s113";//certificate issued
                }
                returnServiceStatus(res.RegistrationNo, status_code);
                #endregion

                if (res.Flag == 1)
                {

                    if ((model.appStatus == -1 || model.appStatus == 3) && model.rejectedRemarks != null)
                    {

                        return RedirectToAction("RejectedApplicationIMC", "CHC");
                    }
                    else if (model.appStatus == -1 && model.rejectedRemarks == null)
                    {

                        return RedirectToAction("UpdateAppProcessIMC", "CHC", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(model.regisIdIMC.ToString()) });
                    }
                    else if (model.appStatus == 1)
                    {

                        return RedirectToAction("InProcessApplicationIMC");
                    }
                    else if (model.appStatus == 2)
                    {
                        return RedirectToAction("UpdateAppProcessIMC", "CHC", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(model.regisIdIMC.ToString()) });
                    }
                    else if (model.appStatus == 3 && model.rejectedRemarks == null)
                    {

                        return Content(res.Msg.ToString() + "_" + model.appStatus);
                        ViewBag.Vaccine = objIMC_DB.IMCVaccineById(model.regisIdIMC);
                    }
                    else if (model.appStatus == 5)
                    {
                        TempData["DatSetIMC"] = res.RegisId;
                        return RedirectToAction("UpdateAppProcessIMC", "CHC", new { regisId = @OTPL_Imp.CustomCryptography.Encrypt(model.regisIdIMC.ToString()) });
                        // return RedirectToAction("ApprovedApplicationIMC");
                    }
                    else
                    {

                        return RedirectToAction("AppliedApplicationIMC");
                    }
                }
                else
                {
                    TempData["Msg"] = "Process not Update";
                    TempData["MsgStatus"] = "error";
                    return RedirectToAction("UpdateAppProcessIMC", "CHC");
                }
            }
            else
            {
                TempData["Msg"] = "Some Error occur";
                TempData["MsgStatus"] = "error";
                return RedirectToAction("UpdateAppProcessIMC", "CHC");
            }

        }

        #region ISP

        public string returnServiceStatus(string registrationNo, string status_code)
        {

            var result = "";
            try
            {
                var UserDetails = objCHCDB.GetUserDetailsForStatusPush(registrationNo);
                if (UserDetails.Count != 0)
                {
                    AcknowledgementReq req = new AcknowledgementReq();
                    req.status_code = status_code;//"s117";
                    req.request_id = UserDetails[0].request_id;//"Y7745381196232";//as received from validation API
                    req.applicant_id = UserDetails[0].applicant_id;//"UPFFIY0628";
                    req.application_id = UserDetails[0].application_id; //"test";
                    req.service_code = UserDetails[0].service_code;//"JIJ74";
                    req.application_url = UserDetails[0].certificate_url;
                    req.remarks = "";
                    req.pendency_level = "4";
                    req.pending_with_officer = "NA";
                    req.action_taken_time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");    //"2023-05-27 02:31:29";
                    var token = ISPController.GetISPToken();



                    string jsonpara = "{\"request_id\":\"" + req.request_id + "\",\"applicant_id\":\"" + req.applicant_id + "\",\"application_url\":\"" + req.application_url + "\",\"status_code\":\"" + req.status_code + "\"" +
                        ",\"remarks\":\"" + req.remarks + "\",\"pendency_level\":\"" + req.pendency_level + "\",\"pending_with_officer\":\"" + req.pending_with_officer + "\",\"action_taken_time\":\"" + req.action_taken_time + "\"}";
                    jsonpara = ISPController.encryptString(JsonConvert.SerializeObject(req), "");
                    string APIurl = "http://164.100.181.91/ispws/isp/v1/returnServiceStatus";
                    string result1 = Services.HitAPIWithTokennew(jsonpara, APIurl, token);
                    var ValidateRes = JsonConvert.DeserializeObject<ValidateResponse>(result1);
                    if (ValidateRes.statusMessage == "SUCCESSFUL")
                    {
                        if (status_code == "s113")
                        {

                            returnDeliveredServiceStatusSuccessfully(req.application_id);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            return result;
        }

        public string returnDeliveredServiceStatusSuccessfully(string registrationNo)
        {

            var result = "";
            try
            {
                var UserDetails = objCHCDB.GetUserDetailsForStatusPush(registrationNo);
                if (UserDetails.Count != 0)
                {
                    AcknowledgementReq req = new AcknowledgementReq();
                    req.request_id = UserDetails[0].request_id;//"Y7745381196232";//as received from validation API
                    req.applicant_id = UserDetails[0].applicant_id;//"UPFFIY0628";
                    req.service_code = UserDetails[0].service_code;//"JIJ74";
                    req.application_id = UserDetails[0].application_id; //"test";
                                                                        // req.certificate_url = "http://www.up-health.in/online";
                    req.certificate_url = "http://localhost:6411" + UserDetails[0].certificate_url;
                    req.certificate_no = "456677";
                    req.status_code = "s112";//"s117";service deliverd
                    req.action_taken_time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");   // "2023-05-27 02:31:29";
                    req.dbt_amount = "10";
                    req.remarks = "";
                    req.certificate_expiry_date = "999999";
                    var token = ISPController.GetISPToken();

                    string jsonpara = "{\"request_id\":\"" + req.request_id + "\",\"applicant_id\":\"" + req.applicant_id + "\",\"service_code\":\"" + req.service_code + "\",\"application_id\":\"" + req.application_id + "\"" +
                        ",\"certificate_url\":\"" + req.certificate_url + "\",\"certificate_no\":\"" + req.certificate_no + "\",\"status_code\":\"" + req.status_code + "\",\"action_taken_time\":\"" + req.action_taken_time + "\",\"dbt_amount\":\"" + req.dbt_amount + "\",\"certificate_no\":\"" + req.certificate_no + "\"\"dbt_amount\":\"" + req.dbt_amount + "\",\"remarks\":\"" + req.remarks + "\",\"certificate_no\":\"" + req.certificate_no + "\"\"certificate_expiry_date\":\"" + req.certificate_expiry_date + "\"}";
                    jsonpara = ISPController.encryptString(JsonConvert.SerializeObject(req), "");
                    string APIurl = "http://164.100.181.91/ispws/isp/v1/returnDeliveredServiceStatus";
                    string result1 = Services.HitAPIWithTokennew(jsonpara, APIurl, token);
                    var ValidateRes = JsonConvert.DeserializeObject<ValidateResponse>(result1);
                    if (ValidateRes.statusMessage == "SUCCESSFUL")
                    {

                    }
                }
            }
            catch (WebException ex)
            {
                result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            return result;
        }

        #endregion

        public JsonResult UploadFileIMC(HttpPostedFileBase File)
        {
            string Dirpath = "~/Content/writereaddata/IMC/";
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

        void SendSMSUpdateProcessIMC(string registrationNo, string inspectionDate, string mobileNo, int appstatus)
        {

            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";
                if (appstatus == 0 || appstatus == 4)
                {
                    txtmsg = "Dear Citizen,\n\n Your Application Form Number" + registrationNo + " for Issuance of Immunization Certificate has been rejected. Kindly get in touch with the office of Community Health Centre/ District Hospital for more details. You can also login to your dashboard for more details.\n\n" + objSM.DisplayName + "\nMHFWD, UP";
                    string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                    objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
                }
                else if (appstatus == 2)
                {
                    txtmsg = "Dear Citizen,\n\n As per your application for Issuance of Medical Immunization Certificate , a committee for Inspection has been scheduled. We request to kindly be present at Office of Community Health Centre /District Hospital on the inspection date" + inspectionDate + " & coordinate accordingly. \n\n" + objSM.DisplayName + "\nMHFWD, UP";
                    string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                    objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
                }
                else if (appstatus == 6)
                {
                    txtmsg = "Dear Citizen,\n\n Your Application Form " + registrationNo + " for Issuance of Immunization Certificate has been approved. Please get in touch with the office of Community Health Centre/ District Hospital to collect your certificate. You can also download the Certificate from your dashboard.\n\n" + objSM.DisplayName + "\nMHFWD, UP";
                    string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                    objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
                }


            }

        }

        [AuthorizeAdmin]
        public ActionResult PrintApplicationFormIMC(string regisIdIMC)
        {
            regisIdIMC = OTPL_Imp.CustomCryptography.Decrypt(regisIdIMC);
            Session["regisIdIMC"] = regisIdIMC;
            IMC_DB objIMC_DB = new IMC_DB();
            IMCModel model = new IMCModel();
            model = objIMC_DB.IMCDetailsByRegistration(Convert.ToInt64(regisIdIMC));

            if (objSM.RollID >= 3 && objSM.RollID <= 5)
            {
                if (model == null || objSM.UserID != model.forwardType)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (objSM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.districtId).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (objSM.RollID != 8)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        [AuthorizeAdmin]
        public ActionResult BindImmunList()//view IMC detail on print form and view(partial)
        {
            IMCimmunizationModel model = new IMCimmunizationModel();
            model.appImmunizationList = objIMC_DB.getIMCimmunList(Convert.ToInt64(Session["regisIdIMC"].ToString()));
            return PartialView("_ViewImmunization", model.appImmunizationList);
        }

        //public ActionResult IMCgetImmunizationDetail(long regisIdIMC)
        //{

        //    IMCAppProcessModel model = new IMCAppProcessModel();
        //    model.updateIMCList = objIMC_DB.getIMCimmunTableForCHC(regisIdIMC);
        //    return PartialView("_IMCImmunDetailForRemark", model.updateIMCList);
        //}

        #region Immunization Certificate
        [AuthorizeAdmin(5)]
        public ActionResult IMCgeneratedCertificate(long regisIdIMC)
        {
            string setPdfName = "", setDigitalPdfName = "";

            var res = objIMC_DB.GetDetailcertificate(regisIdIMC);

            if (res != null && objSM.UserID != res[0].forwardType)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            if (res != null && res.Count > 0)
            {
                try
                {
                    ReportDocument rd = new ReportDocument();

                    rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rptCertificateIMC.rpt"));
                    rd.SetDataSource(res);

                    setPdfName = "UnSigned_" + res[0].certificateNo;

                    string folderpath = "~/Content/writereaddata/UnSignedCertificate/IMC/" + objSM.DistrictName + "/";

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
                    if (System.IO.File.Exists(Server.MapPath(flName)))
                    {
                        int result = objIMC_DB.InsertUnSignedCertiPath_IMC(res[0].regisIdIMC, flName);
                    }
                    rd.Close();
                    rd.Dispose();

                    if (ConfigurationManager.AppSettings["IsDigitalSign"].ToString() != "Y")
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
                    }
                    else
                    {
                        //digital sign

                        setDigitalPdfName = "ImmunizationCertificateDigitalSigned" + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        string digitalFlName = folderpath + setDigitalPdfName + ".pdf";

                        var sigDetails = comndb.GetDigitalSignatureDetails(objSM.UserID);

                        float llx = 580;
                        float lly = 300;
                        float urx = 440;
                        float ury = 200;
                        Comman_Classes.DigitalCeritificateManager dcm = new Comman_Classes.DigitalCeritificateManager();
                        Comman_Classes.MetaData md = new Comman_Classes.MetaData()
                        {
                            Author = sigDetails.Author,
                            Title = "Immunization Certificate Authentication",
                            Subject = "Immunization Certificate",
                            Creator = sigDetails.Creator,
                            Producer = sigDetails.Producer,
                            Keywords = sigDetails.Keywords
                        };

                        string Signaturepath = Server.MapPath(sigDetails.Signaturepath);
                        dcm.signPDF(Server.MapPath(flName), Server.MapPath(digitalFlName), Signaturepath,
                        sigDetails.signpwd, "Authenticate Immunization Certificate", sigDetails.SigContact,
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

                        //digital sign end 
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error Occour to Downloading.";
                }
            }
            else
            {
                ViewBag.Message = "Detail not found.";
            }

            return View("DownloadFile");
        }
        #endregion
        #endregion

        #region Immunization Generate Url  Certificate  Hemendra

        public int IMCgeneratedCertificateInt(long regisId)
        {
            string setPdfName = "", setDigitalPdfName = "";

            var res = objIMC_DB.GetDetailcertificate(regisId);

            if (res != null && objSM.UserID != res[0].forwardType)
            {
                return -1;  //RedirectToAction("UnauthoriseAcess", "Home");
            }

            if (res != null && res.Count > 0)
            {
                try
                {
                    ReportDocument rd = new ReportDocument();

                    rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rptCertificateIMC.rpt"));
                    rd.SetDataSource(res);

                    setPdfName = "UnSigned_" + res[0].certificateNo;

                    string folderpath = "~/Content/writereaddata/UnSignedCertificate/IMC/" + objSM.DistrictName + "/";

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
                    if (System.IO.File.Exists(Server.MapPath(flName)))
                    {
                        int result = objIMC_DB.InsertUnSignedCertiPath_IMC(res[0].regisIdIMC, flName);
                    }
                    rd.Close();
                    rd.Dispose();

                    if (ConfigurationManager.AppSettings["IsDigitalSign"].ToString() != "Y")
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
                    }
                    else
                    {
                        //digital sign

                        setDigitalPdfName = "ImmunizationCertificateDigitalSigned" + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        string digitalFlName = folderpath + setDigitalPdfName + ".pdf";

                        var sigDetails = comndb.GetDigitalSignatureDetails(objSM.UserID);

                        float llx = 580;
                        float lly = 300;
                        float urx = 440;
                        float ury = 200;
                        Comman_Classes.DigitalCeritificateManager dcm = new Comman_Classes.DigitalCeritificateManager();
                        Comman_Classes.MetaData md = new Comman_Classes.MetaData()
                        {
                            Author = sigDetails.Author,
                            Title = "Immunization Certificate Authentication",
                            Subject = "Immunization Certificate",
                            Creator = sigDetails.Creator,
                            Producer = sigDetails.Producer,
                            Keywords = sigDetails.Keywords
                        };

                        string Signaturepath = Server.MapPath(sigDetails.Signaturepath);
                        dcm.signPDF(Server.MapPath(flName), Server.MapPath(digitalFlName), Signaturepath,
                        sigDetails.signpwd, "Authenticate Immunization Certificate", sigDetails.SigContact,
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

                        //digital sign end 
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error Occour to Downloading.";
                }
            }
            else
            {
                ViewBag.Message = "Detail not found.";
            }

            return  1;
        }
        #endregion

        #region Muheeb

        [HttpGet]
        public ActionResult DeathCertificate()
        {
            ViewBag.maritalStatus = comndb.GetDropDownList(19, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Religion = comndb.GetDropDownList(20, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = comndb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = comndb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.forwardTo = Enumerable.Empty<SelectListItem>();
            ViewBag.forwardTypes = objCHCDB.rblforwardType().ToList().Where(m => m.forwardtypeId != 4);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeathCertificate(CHCModel model)
        {

            if (model.isCauseCertified == false)
            {
                ModelState["diseaseNameOrCause"].Errors.Clear();
            }

            if (objComn.IsValidDateFormat(model.dod))
            {
                long userId = objSM.UserID;
                model.regByusers = userId;
                model.regBytransIp = Common.GetIPAddress();
                model.transIp = Common.GetIPAddress();
                model.requestKey = objSM.AppRequestKey;

                //added by muheeb
                model.districtid = objSM.districtId;
                model.forwardtoId = objSM.UserID;
                model.forwardtypeId = objSM.RollID;
                model.healthUnitDistrictId = objSM.districtId;
                //added by muheeb

                var res = objCHCDB.CHCInsertUpdate(model);

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
                                int exeRsult = objCHCDB.DeleteRegistrationCHC(res.RegisId);
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

            ViewBag.maritalStatus = comndb.GetDropDownList(19, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Religion = comndb.GetDropDownList(20, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = comndb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = comndb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.forwardTo = Enumerable.Empty<SelectListItem>();
            ViewBag.forwardTypes = objCHCDB.rblforwardType().ToList().Where(m => m.forwardtypeId != 4);
            ViewBag.Save = "Submit";
            ViewBag.Reset = "Cancel";



            return RedirectToAction("DeathCertificate");
        }

        void SendSMS(string registrationNo, string mobileNo)
        {

            if (mobileNo != "")
            {
                string txtmsg = "";

                //txtmsg = "Dear Citizen,\n\nYour Application form has been submitted successfully. Your Application Form Number is " + registrationNo + ", kindly use this further.\n\n Thanks";
                txtmsg = "Dear Citizen,\n\nYour Application form for Issuance of Death Certificate has been submitted successfully. Your Application Number is " + registrationNo + ".\n Please use this Application Number for further references.\n\nTechnical Team\nMHFWD, UP";

                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }
        }

        public ActionResult RegistrationConfirmation()
        {

            if (TempData["RegisIdDEC"] != null)
            {
                ViewBag.DECRegisId = OTPL_Imp.CustomCryptography.Encrypt(TempData["RegisIdDEC"].ToString());
                ViewBag.CHCRegistrationNo = TempData["RegistrationNo"];
                return View();
            }
            else
            {
                ViewBag.MLCRegisId = OTPL_Imp.CustomCryptography.Encrypt(TempData["RegisIdMLC"].ToString());
                ViewBag.MLCRegistrationNo = TempData["RegistrationNo"];
                return View();
            }
        }

        [HttpGet]
        public ActionResult MedicoLegalCertificate()
        {
            long userId = objSM.UserID;
            MLCModel model = new MLCModel();
            ViewBag.Tehsil = comndb.GetDropDownList(21, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = comndb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = comndb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.IdType = objMlc.GetIdType().Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            //model = objdb.GetDICById(userId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MedicoLegalCertificate(MLCModel model, FormCollection form)
        {
            #region Bulk Insertion Kailash Joshi
            var enquirydetails = form.GetValues("enquiryDetails");
            int count = enquirydetails.Count();
            string XmlData = "<EnquiryDetails>";
            long regisByuser = objSM.UserID;

            AuditMethods objAudit = new AuditMethods();

            for (int i = 0; i < count; i++)
            {
                if (enquirydetails[i].ToString() == "")
                {
                    //XmlData = string.Empty;
                }
                else
                {
                    XmlData += "<Enquiry><RegisByUser>" + regisByuser + "</RegisByUser>" +
                        "<EnquiryDetails>" + objAudit.FilterForAlphabetNumaric(enquirydetails[i]) + "</EnquiryDetails>"
                            + "</Enquiry>";
                }
            }
            XmlData += "</EnquiryDetails>";
            #endregion
            model.regByUser = objSM.UserID;
            model.regBytransIp = Common.GetIPAddress();
            model.transIp = Common.GetIPAddress();
            model.requestKey = objSM.AppRequestKey;
            //added by muheeb
            model.forwardtoId = objSM.UserID;
            model.forwardtypeId = objSM.RollID;
            model.healthUnitDistrictId = objSM.districtId;
            //added by muheeb
            if (model.patientBroughtBy != "Relative")
            {
                ModelState["broughtByPersonrelation"].Errors.Clear();
            }
            var res = objCHCDB.MLCInsertUpdate(model, XmlData);

            try
            {
                if (res.RegisId > 0 && res.RegistrationNo != "")
                {
                    if (!string.IsNullOrEmpty(objSM.AppRequestKey) && ConfigurationManager.AppSettings["AllowEDistrict"].ToString() == "Y")
                    {
                        EDistrictServiceClass ed = new EDistrictServiceClass();

                        int serviceCode = Convert.ToInt32(EDistrict_ServiceCode.MLC);

                        bool result = ed.postServiceResponse(objSM.AppRequestKey, res.RegistrationNo, serviceCode.ToString(), "MLC");

                        if (result)
                        {
                            SendSMS(res.RegistrationNo, res.MobileNo);
                            TempData["RegisIdMLC"] = res.RegisId;
                            TempData["RegistrationNo"] = res.RegistrationNo;
                            return RedirectToAction("RegistrationConfirmation");
                        }
                        else
                        {
                            int exeRsult = objCHCDB.DeleteRegistrationMLC(res.RegisId);
                            setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                            ModelState.Clear();
                        }
                    }
                    else
                    {
                        SendSMS(res.RegistrationNo, res.MobileNo);
                        TempData["RegisIdMLC"] = res.RegisId;
                        TempData["RegistrationNo"] = res.RegistrationNo;
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

            ViewBag.Tehsil = comndb.GetDropDownList(21, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = comndb.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = comndb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.IdType = objMlc.GetIdType().Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return RedirectToAction("MedicoLegalCertificate");
        }

        public ActionResult ViewDeathcertificateCHC()
        {
            return View();
        }

        public ActionResult Partials_ViewCHCDeath(string regisno = "", string Reqdate = "")
        {
            long userid = Convert.ToInt32(objSM.UserID);

            string applicationNo = regisno;
            string requesdate = Reqdate;
            var result = objCHCDB.ViewDeathCertificate(userid, 3, requesdate, applicationNo).ToList();
            model.DECModelList = result;

            return View("_ViewDeathcertificateCHC", model.DECModelList);
        }

        public ActionResult ViewMedicoLegalCertificate()
        {
            return View();
        }

        public ActionResult partial_ViewMLCDetails(string regisno = "", string Reqdate = "")
        {
            long userid = Convert.ToInt32(objSM.UserID);

            string applicationNo = regisno;
            string requesdate = Reqdate;
            var result = objCHCDB.ViewMLCList(userid, 3, requesdate, applicationNo).ToList();
            Mlcmodel.MLCModelList = result;

            return View("_ViewMedicoLegalCertificate", Mlcmodel.MLCModelList);
        }

        public ActionResult DCDetails(string registrationNo, string regisIdDEC)
        {
            long regisByuser = objSM.UserID;
            registrationNo = OTPL_Imp.CustomCryptography.Decrypt(registrationNo);
            Session["DEC_registration"] = registrationNo;
            Session["regisIdDEC"] = regisIdDEC;
            DECModel model = new DECModel();
            model = objdb.GetDECListBYRegistrationNo(regisByuser, registrationNo);

            return View(model);
        }

        public ActionResult MLCDetails(string registrationNo, string regisIdMLC)
        {
            //  var resultData = objdb.GetRegisterDetails();
            long regisByuser = objSM.UserID;
            registrationNo = OTPL_Imp.CustomCryptography.Decrypt(registrationNo);
            MLCModel model = new MLCModel();
            model = objMlc.GetMLCListBYRegistrationNo(regisByuser, registrationNo);

            return View(model);
        }

        #endregion

        #region MLC

        [AuthorizeAdmin(9)]
        public ActionResult AppliedApplicationMLC()
        {
            ProcessType model = new ProcessType();
            int id;
            if (objSM.RollID == 8)
            {
                id = 0;
            }
            else
            {
                id = Convert.ToInt32(objSM.UserID);
            }
            model = objCHCDB.RegistrationCount_MLC(id);

            return View(model);
        }

        [AuthorizeAdmin(9)]
        public ActionResult ReceivedApplicationMLC()
        {
            ViewBag.DLLAppStatus = comndb.GetApplicationProcessByAppName("NUH").Where(m => m.Value == "-1").ToList();
            return View();
        }

        [AuthorizeAdmin(9)]
        public ActionResult ReceivedApplicationListMLC(string registrationNo = "", string appDate = "", string status = "")
        {
            List<MLCDetailsModel> lstMLCDetails = new List<MLCDetailsModel>();
            DataTable Ids = new DataTable();
            string strIds = "";

            if (string.IsNullOrEmpty(status))
            {
                strIds = "-1,1";
            }
            else
            {
                strIds = status;
            }

            int id;
            if (objSM.RollID == 8)
            {
                id = 0;
            }
            else
            {
                id = Convert.ToInt32(objSM.UserID);
            }
            var arrIds = strIds.Split(',');
            Ids = objCom.strArrayToDataTable(arrIds);
            lstMLCDetails = objCHCDB.GetAllRegistration_MLC(id, Ids, registrationNo, appDate).ToList();

            return PartialView("_ReceivedApplicationListMLC", lstMLCDetails);
        }

        [AuthorizeAdmin]
        public ActionResult ViewAppDetailsMLC(long regisId)
        {
            MLCModel model = new MLCModel();

            model = objMlc.GetMLCListBYRegistrationNo(0, "", regisId);

            return PartialView("_ViewAppDetailsMLC", model);
        }

        [AuthorizeAdmin(9)]
        public ActionResult InProcessApplicationMLC()
        {
            ViewBag.DLLAppStatus = comndb.GetApplicationProcessByAppName("NUH").Where(m => m.Value == "2" || m.Value == "3" || m.Value == "5").ToList();
            return View();
        }

        [AuthorizeAdmin(9)]
        public ActionResult InProcessApplicationListMLC(string registrationNo = "", string appDate = "", string status = "")
        {
            List<MLCDetailsModel> lstMLCDetails = new List<MLCDetailsModel>();
            DataTable Ids = new DataTable();
            string strIds = "";

            if (string.IsNullOrEmpty(status))
            {
                strIds = "2,3,5";
            }
            else
            {
                strIds = status;
            }

            var arrIds = strIds.Split(',');
            Ids = objCom.strArrayToDataTable(arrIds);
            lstMLCDetails = objCHCDB.GetAllRegistration_MLC(objSM.UserID, Ids, registrationNo, appDate).ToList();

            return PartialView("_ReceivedApplicationListMLC", lstMLCDetails);
        }

        [AuthorizeAdmin]
        public ActionResult MLCAppDetails(long regisId)
        {
            MLCModel model = new MLCModel();

            model = objMlc.GetMLCListBYRegistrationNo(0, "", regisId);

            return PartialView("_MLCAppDetails", model);
        }

        [AuthorizeAdmin(9)]
        public ActionResult UpdateAppProcessMLC(string regisId, string status)
        {
            MLCAppProcessModel model = new MLCAppProcessModel();
            model.regisIdMLC = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId));
            model.appStatus = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(status));
            ViewBag.DLLCommittee = objCHCDB.proc_GetCommitteeDetailsForDLL_MLC().ToList();

            //model = objCMODB.GetNUHList(model.regisIdNUH, model.appStatus);

            if (model.appStatus == 2)
            {
                ViewBag.designation = objCHCDB.GetCommitteeMember_MLC(model.regisIdMLC).ToList();
            }

            if (model.appStatus == -1)
            {
                ViewBag.PageTitle = "Accept/Reject Application";
            }
            else if (model.appStatus == 1)
            {
                //model = objCHCDB.GetILCList(regisId, status);
                ViewBag.PageTitle = "Schedule Inspection Application";
            }
            else if (model.appStatus == 2)
            {
                ViewBag.PageTitle = "Upload Inspection Report";
            }
            else if (model.appStatus == 3)
            {
                ViewBag.PageTitle = "Accept/Reject Inspection Report";
            }
            else if (model.appStatus == 5)
            {
                ViewBag.PageTitle = "Generate Certificate";
            }

            return View(model);
        }

        [AuthorizeAdmin(9)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAppProcessMLC(MLCAppProcessModel model, string button, FormCollection form)
        {
            ResultSet resultData = new ResultSet();
            int currAppStatus = model.appStatus;
            int? appStatus = null;
            bool isRedirectNewAction = false;
            string message = "", actionName = "";

            if (!string.IsNullOrEmpty(button))
            {
                ModelState["rejectedRemarks"].Errors.Clear();
                ModelState["inspectionDate"].Errors.Clear();
                ModelState["inspReportFile"].Errors.Clear();
                ModelState["inspReportFile"].Errors.Clear();

                if (button.ToLower() == "appaccept")
                {
                    ModelState["treatmentFrom"].Errors.Clear();
                    ModelState["treatmentto"].Errors.Clear();
                    // ModelState["restFor"].Errors.Clear();
                    appStatus = 1;
                    message = "Application Accepted";
                    actionName = "UpdateAppProcessMLC";
                }
                else if (button.ToLower() == "insaccept")
                {
                    ModelState["treatmentFrom"].Errors.Clear();
                    ModelState["treatmentto"].Errors.Clear();
                    // ModelState["restFor"].Errors.Clear();
                    appStatus = 5;
                    message = "Inspection Report Accepted";
                    actionName = "UpdateAppProcessMLC";
                }
                else if (button.ToLower() == "gencertificate")
                {

                    appStatus = 6;
                    message = "Certificate Generated";
                    actionName = "UpdateAppProcessMLC";
                    TempData["DatSetSec"] = model.regisIdMLC;

                    // isRedirectNewAction = true;
                    //NUHgeneratedCertificate(model.regisIdNUH);
                }
            }
            else
            {
                if (model.appStatus == -1)
                {
                    ModelState["inspectionDate"].Errors.Clear();
                    ModelState["inspReportFile"].Errors.Clear();

                    appStatus = 0;
                    message = "Application Rejected";
                    actionName = "RejectedApplicationMLC";
                    isRedirectNewAction = true;
                }
                else if (model.appStatus == 1)
                {
                    ModelState["treatmentFrom"].Errors.Clear();
                    ModelState["treatmentto"].Errors.Clear();
                    //  ModelState["restFor"].Errors.Clear();
                    ModelState["rejectedRemarks"].Errors.Clear();
                    ModelState["inspReportFile"].Errors.Clear();

                    appStatus = 2;
                    //Session["COMMITTEEID"] = model.committeeId;
                    message = "Inspection Scheduled";
                    actionName = "InProcessApplicationMLC";
                    isRedirectNewAction = true;
                }
                else if (model.appStatus == 2)
                {
                    ModelState["treatmentFrom"].Errors.Clear();
                    ModelState["treatmentto"].Errors.Clear();
                    // ModelState["restFor"].Errors.Clear();
                    ModelState["inspectionDate"].Errors.Clear();
                    ModelState["rejectedRemarks"].Errors.Clear();

                    appStatus = 3;

                    #region Riya bulkinsertion

                    string XmlData1 = "";


                    var committeeMembderId = form.GetValues("chkdesig");

                    int count = committeeMembderId.Count();
                    XmlData1 = "<Members>";
                    long regisByuser = objSM.UserID;
                    for (int i = 0; i < count; i++)
                    {
                        if (committeeMembderId[i] == "")
                        {
                            //XmlData = string.Empty;
                        }
                        else
                        {

                            XmlData1 += "<Member><committeeMembderId>" + committeeMembderId[i] + "</committeeMembderId></Member>";
                        }

                    }
                    XmlData1 += "</Members>";
                    model.XmlData = XmlData1;
                    #endregion
                    message = "Inspection Report Uploaded";
                    actionName = "UpdateAppProcessMLC";
                }
                else if (model.appStatus == 3)
                {
                    ModelState["treatmentFrom"].Errors.Clear();
                    ModelState["treatmentto"].Errors.Clear();
                    //ModelState["restFor"].Errors.Clear();
                    ModelState["inspectionDate"].Errors.Clear();
                    ModelState["inspReportFile"].Errors.Clear();

                    appStatus = 4;

                    message = "Inspection Report Rejected";
                    actionName = "RejectedApplicationMLC";
                    isRedirectNewAction = true;
                }
            }

            if (ModelState.IsValid && appStatus != null)
            {
                model.appStatus = Convert.ToInt32(appStatus);
                model.userId = objSM.UserID;
                model.transIp = Common.GetIPAddress();

                try
                {
                    resultData = objCHCDB.UpdateAppProcessMLC(model.regisIdMLC, model.appStatus, model.committeeId, model.inspectionDate, model.inspReportFilePath, model.certificateFilePath, model.rejectedRemarks, model.userId, model.transIp, model.XmlData, model.treatmentFrom, model.treatmentto, model.restFor);
                }
                catch (Exception ex)
                {
                    setSweetAlertMsg(ex.Message, "warning");
                }
            }
            else
            {
                setSweetAlertMsg("Enter valid data !", "warning");
            }

            if (resultData != null && resultData.Flag > 0)
            {
                //SendSMSUpdateProcessMLC(resultData.RegistrationNo, resultData.inspectionDate, resultData.MobileNo, resultData.appStatus);
                TempData["SuccessMsg"] = message;
                if (isRedirectNewAction)
                {
                    return RedirectToAction(actionName);
                }
                else
                {
                    if (resultData.appStatus == 6)
                    {
                        TempData["DatSetMLC"] = resultData.RegisId;
                    }
                    return RedirectToAction(actionName, new { regisId = OTPL_Imp.CustomCryptography.Encrypt(model.regisIdMLC.ToString()), status = OTPL_Imp.CustomCryptography.Encrypt(model.appStatus.ToString()) });
                }
            }
            else
            {
                model.appStatus = Convert.ToInt32(currAppStatus);
                ViewBag.DLLCommittee = comndb.GetCommitteeDetailsForDLL().ToList();
                return View(model);
            }
        }

        [AuthorizeAdmin]
        public ActionResult BindScheduleOfCommitteeMLC(long committeeId = 0, string inspectiondate = "")
        {
            if (committeeId > 0)
            {
                var lstScheduleOfCommittee = objCHCDB.GetScheduleOfCommittee_MLC(committeeId, inspectiondate).ToList();
                return PartialView("_BindScheduleOfCommitteeMLC", lstScheduleOfCommittee);
            }
            else
            {
                return Content("NF");
            }
        }

        public JsonResult UploadFileMLC(HttpPostedFileBase File)
        {
            string Dirpath = "~/Content/writereaddata/MLC/InspectionReport/";
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
                    path = "SNV";
                }
                else
                {
                    File.SaveAs(completepath);
                    path = Dirpath + filename;
                }
            }
            else
            {
                path = "TNV";
            }

            List<string> plist = new List<string> { filename, path };
            return Json(plist);
        }

        [AuthorizeAdmin(9)]
        public ActionResult ApprovedApplicationMLC()
        {
            return View();
        }

        [AuthorizeAdmin(9)]
        public ActionResult ApprovedApplicationListMLC(string registrationNo = "", string appDate = "")
        {
            List<MLCDetailsModel> lstMLCDetails = new List<MLCDetailsModel>();
            DataTable Ids = new DataTable();
            string strIds = "6";
            int id;
            if (objSM.RollID == 8)
            {
                id = 0;
            }
            else
            {
                id = Convert.ToInt32(objSM.UserID);
            }
            var arrIds = strIds.Split(',');
            Ids = objCom.strArrayToDataTable(arrIds);
            lstMLCDetails = objCHCDB.GetAllRegistration_MLC(id, Ids, registrationNo, appDate).ToList();

            return PartialView("_ApprovedApplicationListMLC", lstMLCDetails);
        }

        [AuthorizeAdmin(9)]
        public ActionResult RejectedApplicationMLC()
        {
            return View();
        }

        [AuthorizeAdmin(9)]
        public ActionResult RejectedApplicationListMLC(string registrationNo = "", string appDate = "")
        {
            List<MLCDetailsModel> lstMLCDetails = new List<MLCDetailsModel>();
            DataTable Ids = new DataTable();
            string strIds = "0,4";

            var arrIds = strIds.Split(',');
            Ids = objCom.strArrayToDataTable(arrIds);
            lstMLCDetails = objCHCDB.GetAllRegistration_MLC(objSM.UserID, Ids, registrationNo, appDate).ToList();

            return PartialView("_RejectedApplicationListMLC", lstMLCDetails);
        }

        void SendSMSUpdateProcessMLC(string registrationNo, string inspectionDate, string mobileNo, int appstatus)
        {
            ForgotPasswordModel otpChCount = new ForgotPasswordModel();
            string txtmsg = "";
            if (appstatus == 0 || appstatus == 4)
            {
                txtmsg = "Dear Citizen,\n\nYour Application Form No. " + registrationNo + " for Registration of Medico-Legal has been rejected. Kindly get in touch with the office of Chief Medical Officer for more details. You can also login to your dashboard for more details." + objSM.DisplayName + "\nMHFWD, UP";
            }
            else if (appstatus == 2)
            {
                txtmsg = "Dear Citizen,\n\n As per your application for Registration of Medico-Legal, a committee for Inspection has been scheduled. We request to kindly be present at your medical establishment on the inspection date " + inspectionDate + " and  coordinate accordingly.\n\n" + objSM.DisplayName + "\nMHFWD, UP";
            }
            else if (appstatus == 6)
            {
                txtmsg = "Dear Citizen,\n\n Your Application Form No. " + registrationNo + " for Registration of Medico-Legal has been approved. Please get in touch with the office of Chief Medical Officer to collect your certificate. You can also download the Certificate from your dashboard.\n\n" + objSM.DisplayName + "\nMHFWD, UP";
            }

            if (!string.IsNullOrEmpty(mobileNo) && !string.IsNullOrEmpty(txtmsg))
            {
                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }
        }

        [AuthorizeAdmin]
        public ActionResult PrintApplicationFormMLC(string regisIdMLC)
        {
            MLCModel model = new MLCModel();

            model = objMlc.GetMLCListBYRegistrationNo(0, "", Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdMLC)));

            if (objSM.RollID >= 3 && objSM.RollID <= 5)
            {
                if (model == null || objSM.UserID != model.forwardtoId)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (objSM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.districtId).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (objSM.RollID != 8)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            TempData["reg"] = model.registrationNo;
            Session["reg"] = model.registrationNo;
            return View(model);
        }

        [AuthorizeAdmin]
        public ActionResult BindCHCChildList(string registrationNo)
        {
            MLCModel model = new MLCModel();

            model.MLCModelList = objMlc.getMLCChild(registrationNo);
            return PartialView("_ViewMLCChild", model.MLCModelList);
        }

        //public JsonResult UploadCertificateFileNUH(HttpPostedFileBase File)
        //{
        //    long regisId = Convert.ToInt64(Session["registrationIdNUH"].ToString());

        //    var lstNUHDetails = objCMODB.GetNUHCertificate(regisId);

        //    string Dirpath = "~/Content/writereaddata/" + objSM.DistrictName + "/";
        //    string path = "";
        //    string filename = "";
        //    if (!Directory.Exists(Server.MapPath(Dirpath)))
        //    {
        //        Directory.CreateDirectory(Server.MapPath(Dirpath));
        //    }
        //    string ext = Path.GetExtension(File.FileName);
        //    if (ext.ToLower() == ".jpg" || ext.ToLower() == ".pdf")
        //    {

        //        filename = lstNUHDetails.CertificateNo + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;
        //        string completepath = Path.Combine(Server.MapPath(Dirpath), filename);
        //        if (System.IO.File.Exists(completepath))
        //        {
        //            System.IO.File.Delete(completepath);
        //        }

        //        long size = File.ContentLength;
        //        if (size > 2097152)
        //        {
        //            path = "SNV";//"warning_Maximum 2MB file size are allowed";
        //        }
        //        else
        //        {
        //            File.SaveAs(completepath);
        //            path = Dirpath + filename;
        //        }
        //    }
        //    else
        //    {
        //        path = "TNV";//"warning_Invalid File Format only pdf and jpg files are allow!";
        //    }

        //    List<string> plist = new List<string> { filename, path };
        //    return Json(plist);
        //}
        //[HttpGet]
        //public ActionResult UploadCertificateNUH(string regisId)
        //{
        //    Session["registrationIdNUH"] = OTPL_Imp.CustomCryptography.Decrypt(regisId);
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult UploadCertificateNUH(FormCollection frm)
        //{
        //    var CertificatePath = frm.GetValues("hdnfileUploadCertificate");
        //    string IPAddress = Common.GetIPAddress();

        //    var res = objCMODB.UpdateNUHCertificate(Convert.ToInt64(Session["registrationIdNUH"]), CertificatePath[0], objSM.UserID, IPAddress);
        //    if (res.Flag == 1)
        //    {
        //        TempData["Message"] = res.Msg;
        //    }
        //    return RedirectToAction("ApprovedApplicationNUH", "CMO");
        //}

        //#region Certificate Rpt Riya
        //public ActionResult MLCGeneratedCertificate(long regisIdMLC)
        //{
        //    string stausMessage = "";
        //    string setPdfName = "", setDigitalPdfName = "";
        //    var res = objCMODB.GetDetail(regisIdMLC);
        //    var res2 = objCMODB.getNUHChildRpt(res[0].regisIdNUH);
        //    try
        //    {
        //        ReportDocument rd = new ReportDocument();
        //        rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rpt_NUHcertificate.rpt"));
        //        rd.SetDataSource(res);
        //        ReportDocument subShows = rd.Subreports["rpt_NUHcertificateChild.rpt"];
        //        subShows.SetDataSource(res2);
        //        String dtnow = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        //        setPdfName = "MedicalEstablishment" + "_" + dtnow;
        //        setDigitalPdfName = "MedicalEstablishmentCertificateDigitalSigned" + "_" + dtnow;
        //        string folderpath = "~/Content/writereaddata/PDF/";
        //        if (!System.IO.Directory.Exists(Server.MapPath(folderpath)))
        //        {
        //            System.IO.Directory.CreateDirectory(Server.MapPath(folderpath));
        //        }
        //        string flName = folderpath + setPdfName + ".pdf";
        //        string digitalFlName = folderpath + setDigitalPdfName + ".pdf";
        //        rd.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //        rd.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //        rd.ExportOptions.DestinationOptions = new DiskFileDestinationOptions();
        //        ((DiskFileDestinationOptions)rd.ExportOptions.DestinationOptions).DiskFileName = Server.MapPath(flName);
        //        rd.Export();
        //        //FileInfo fileInfo = new FileInfo(Server.MapPath(flName));
        //        rd.Close();
        //        rd.Dispose();
        //        if (ConfigurationManager.AppSettings["IsDigitalSign"].ToString() == "N")
        //        {
        //            FileInfo fileInfo = new FileInfo(Server.MapPath(flName));
        //            Response.ClearContent();
        //            Response.ClearHeaders();
        //            Response.ContentType = "application/pdf";
        //            Response.AppendHeader("Content-Disposition", "attachment; filename=" + setPdfName + ".pdf");
        //            Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
        //            Response.WriteFile(flName);
        //            Response.Flush();
        //            Response.Close();
        //            if (System.IO.File.Exists(Server.MapPath(flName)))
        //            {
        //                System.IO.File.Delete(Server.MapPath(flName));
        //            }
        //        }
        //        else
        //        {
        //            ////////////digital sign

        //            var sigDetails = comndb.GetDigitalSignatureDetails(objSM.UserID);

        //            float llx = 580;
        //            float lly = 300;
        //            float urx = 440;
        //            float ury = 200;
        //            Comman_Classes.DigitalCeritificateManager dcm = new Comman_Classes.DigitalCeritificateManager();
        //            Comman_Classes.MetaData md = new Comman_Classes.MetaData()
        //            {
        //                Author = sigDetails.Author,
        //                Title = "Medical Establishment Certificate Authentication",
        //                Subject = "Medical Establishment Certificate",
        //                Creator = sigDetails.Creator,
        //                Producer = sigDetails.Producer,
        //                Keywords = sigDetails.Keywords
        //            };

        //            string Signaturepath = Server.MapPath(sigDetails.Signaturepath);
        //            dcm.signPDF(Server.MapPath(flName), Server.MapPath(digitalFlName), Signaturepath,
        //             sigDetails.signpwd, "Authenticate Medical Establishment Certificate", sigDetails.SigContact,
        //             sigDetails.SigLocation, true, llx, lly, urx, ury, 1, md);

        //            FileInfo fileInfo = new FileInfo(Server.MapPath(digitalFlName));
        //            Response.ClearContent();
        //            Response.ClearHeaders();
        //            Response.ContentType = "application/pdf";
        //            Response.AppendHeader("Content-Disposition", "attachment; filename=" + setDigitalPdfName + ".pdf");
        //            Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
        //            Response.WriteFile(Server.MapPath(digitalFlName));
        //            Response.Flush();
        //            Response.Close();

        //            if (System.IO.File.Exists(Server.MapPath(flName)))
        //            {
        //                System.IO.File.Delete(Server.MapPath(flName));
        //            }

        //            if (System.IO.File.Exists(Server.MapPath(digitalFlName)))
        //            {
        //                System.IO.File.Delete(Server.MapPath(digitalFlName));
        //            }

        //            //////////////////////////digital sign end

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        stausMessage = "error_Error Occour to Downloading, Please try again.";
        //    }
        //    return RedirectToAction("ApprovedApplicationNUH");
        //}
        //#endregion

        public ActionResult GenrateCertificateMLC(string regisId)
        {
            if (!string.IsNullOrEmpty(regisId))
            {
                MLCAppProcessModel model = new MLCAppProcessModel();

                model.appStatus = 6;
                model.userId = objSM.UserID;
                model.transIp = Common.GetIPAddress();
                model.regisIdMLC = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId));

                ResultSet resultData = objCHCDB.GenrateCertificateMLC(model);

                if (resultData != null && resultData.Flag > 0)
                {
                    return Content("success_" + model.regisIdMLC);
                }
                else
                {
                    return Content("error_0");
                }
            }
            else
            {
                return Content("error_0");
            }
        }

        #region MLC Certificate Rpt
        [AuthorizeAdmin(9)]
        public ActionResult MLCgeneratedCertificate(long regisIdMLC)
        {
            string setPdfName = "", setDigitalPdfName = "";
            var res = objMlc.GetMLCDetails(regisIdMLC);

            if (res != null && objSM.UserID != res[0].forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            if (res != null && res.Count > 0)
            {
                var res2 = objMlc.getMLCChilds(res[0].regisIdMLC);
                try
                {
                    ReportDocument rd = new ReportDocument();

                    rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rpt_MLCcertificate.rpt"));
                    rd.SetDataSource(res);

                    ReportDocument subShows = rd.Subreports["rpt_MLCchildCertificate.rpt"];
                    subShows.SetDataSource(res2);

                    setPdfName = "UnSigned_" + res[0].certificateNo;

                    string folderpath = "~/Content/writereaddata/UnSignedCertificate/MLC/" + objSM.DistrictName + "/";

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
                    if (System.IO.File.Exists(Server.MapPath(flName)))
                    {
                        int result = objMlc.InsertUnSignedCertiPath_MLC(res[0].regisIdMLC, flName);
                    }
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
                    }
                    else
                    {
                        //digital sign

                        setDigitalPdfName = "MedcoLegalCertificateDigitalSigned" + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        string digitalFlName = folderpath + setDigitalPdfName + ".pdf";

                        var sigDetails = comndb.GetDigitalSignatureDetails(objSM.UserID);

                        float llx = 580;
                        float lly = 300;
                        float urx = 440;
                        float ury = 200;
                        Comman_Classes.DigitalCeritificateManager dcm = new Comman_Classes.DigitalCeritificateManager();
                        Comman_Classes.MetaData md = new Comman_Classes.MetaData()
                        {
                            Author = sigDetails.Author,
                            Title = "Medco Legal Certificate Authentication",
                            Subject = "Medco Legal Certificate",
                            Creator = sigDetails.Creator,
                            Producer = sigDetails.Producer,
                            Keywords = sigDetails.Keywords
                        };

                        string Signaturepath = Server.MapPath(sigDetails.Signaturepath);
                        dcm.signPDF(Server.MapPath(flName), Server.MapPath(digitalFlName), Signaturepath,
                         sigDetails.signpwd, "Authenticate Medco Legal Certificate", sigDetails.SigContact,
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

                        //digital sign end 
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error Occour to Downloading.";
                }
            }
            else
            {
                ViewBag.Message = "Detail not found.";
            }

            return View("DownloadFile");
        }
        #endregion

        [AuthorizeAdmin(9)]
        [HttpGet]
        public ActionResult ViewDownloadedCertificateMLC(string regisId = "")
        {
            ViewBag.RegisId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId));
            return View();
        }

        [AuthorizeAdmin(9)]
        public ActionResult ViewDownloadedCertificateListMLC(long regisId)
        {
            var result = objCHCDB.GetNomineeDetails(regisId);
            return PartialView("_ViewDownloadedCertificateListMLC", result);
        }

        #endregion

        #region ICC

        [AuthorizeAdmin(11)]
        public ActionResult AppliedApplicationICC()
        {
            ProcessType model = new ProcessType();
            int id;
            if (objSM.RollID == 8)
            {
                id = 0;
            }
            else
            {
                id = Convert.ToInt32(objSM.UserID);
            }
            model = objCHCDB.RegistrationCount_ICC(id);

            return View(model);
        }

        [AuthorizeAdmin(11)]
        public ActionResult ReceivedApplicationICC()
        {
            return View();
        }

        [AuthorizeAdmin(11)]
        public ActionResult ReceivedApplicationListICC(string registrationNo = "", string appDate = "", string status = "")
        {
            List<ICCDetailsModel> lstMLCDetails = new List<ICCDetailsModel>();
            DataTable Ids = new DataTable();
            string strIds = "-1";

            var arrIds = strIds.Split(',');
            Ids = objCom.strArrayToDataTable(arrIds);
            int id;
            if (objSM.RollID == 8)
            {
                id = 0;
            }
            else
            {
                id = Convert.ToInt32(objSM.UserID);
            }
            lstMLCDetails = objCHCDB.GetAllRegistration_ICC(id, Ids, registrationNo, appDate).ToList();

            return PartialView("_ReceivedApplicationListICC", lstMLCDetails);
        }

        [AuthorizeAdmin]
        public ActionResult ViewAppDetailsICC(long regisId)
        {
            ICCModel model = new ICCModel();

            model = objICC.GetICCListBYRegistrationNo(0, "", regisId);

            return PartialView("_ViewAppDetailsICC", model);
        }

        [AuthorizeAdmin(11)]
        public ActionResult InProcessApplicationICC()
        {
            ViewBag.DLLAppStatus = comndb.GetApplicationProcessByAppName("ICC").Where(m => m.Value == "1" || m.Value == "2").ToList();
            return View();
        }

        [AuthorizeAdmin(11)]
        public ActionResult InProcessApplicationListICC(string registrationNo = "", string appDate = "", string status = "")
        {
            List<ICCDetailsModel> lstMLCDetails = new List<ICCDetailsModel>();
            DataTable Ids = new DataTable();
            string strIds = "";

            if (string.IsNullOrEmpty(status))
            {
                strIds = "1,2";
            }
            else
            {
                strIds = status;
            }

            int id;
            if (objSM.RollID == 8)
            {
                id = 0;
            }
            else
            {
                id = Convert.ToInt32(objSM.UserID);
            }

            var arrIds = strIds.Split(',');
            Ids = objCom.strArrayToDataTable(arrIds);
            lstMLCDetails = objCHCDB.GetAllRegistration_ICC(id, Ids, registrationNo, appDate).ToList();

            return PartialView("_ReceivedApplicationListICC", lstMLCDetails);
        }

        [AuthorizeAdmin]
        public ActionResult ICCAppDetails(long regisId)
        {
            ICCModel model = new ICCModel();

            model = objICC.GetICCListBYRegistrationNo(0, "", regisId);

            return PartialView("_ICCAppDetails", model);
        }

        [AuthorizeAdmin(11)]
        public ActionResult UpdateAppProcessICC(string regisId, string status)
        {
            ICCAppProcessModel model = new ICCAppProcessModel();
            model.regisIdICC = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId));
            model.appStatus = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(status));

            model = objCHCDB.GetICCList(model.regisIdICC, model.appStatus);

            if (model == null || objSM.UserID != model.forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            return View(model);
        }

        [AuthorizeAdmin(11)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAppProcessICC(ICCAppProcessModel model, string button, FormCollection form)
        {
            #region ISP
            string status_code = "";
            #endregion
            var result = objCHCDB.GetICCList(model.regisIdICC, model.appStatus);

            if (result == null || objSM.UserID != result.forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            ResultSet resultData = new ResultSet();
            int currAppStatus = model.appStatus;
            int? appStatus = null;
            bool isRedirectNewAction = false;
            string message = "", actionName = "";

            if (!string.IsNullOrEmpty(button))
            {
                ModelState["rejectedRemarks"].Errors.Clear();

                if (button.ToLower() == "appaccept")
                {
                    appStatus = 1;
                    message = "Application Accepted";
                    actionName = "InProcessApplicationICC";
                    isRedirectNewAction = true;
                    #region ISP
                    status_code = "s111";//Application Accepted
                    #endregion
                }
                else if (button.ToLower() == "insaccept")
                {
                    appStatus = 2;
                    message = "Application Accepted";
                    actionName = "UpdateAppProcessICC";
                    #region ISP
                    status_code = "s115";//approved 
                    #endregion
                }
                else if (button.ToLower() == "gencertificate")
                {
                     //  ICC heme
                    appStatus = 4;
                    message = "Certificate Generated";
                    actionName = "UpdateAppProcessICC";
                    TempData["DatSetSec"] = model.regisIdICC;
                    #region ISP
                    ICCgeneratedCertificateInt(model.regisIdICC);
                    status_code = "s113";//certificate issue
                    #endregion
                }
            }
            else
            {
                if (model.appStatus == -1)
                {
                    appStatus = 0;

                    message = "Application Rejected";
                    actionName = "RejectedApplicationICC";
                    isRedirectNewAction = true;
                    #region ISP
                    status_code = "s110";//Rejected
                    #endregion
                }
                else if (model.appStatus == 1)
                {
                    appStatus = 3;

                    message = "Application Rejected";
                    actionName = "RejectedApplicationICC";
                    isRedirectNewAction = true;
                    #region ISP
                    status_code = "s110";//Rejected
                    #endregion
                }
            }

            if (ModelState.IsValid && appStatus != null)
            {
                model.appStatus = Convert.ToInt32(appStatus);
                model.userId = objSM.UserID;
                model.transIp = Common.GetIPAddress();

                try
                {
                    resultData = objCHCDB.UpdateAppProcessICC(model.regisIdICC, model.appStatus, model.certificateFilePath, model.rejectedRemarks, model.userId, model.transIp);
                    #region ISP
                    returnServiceStatus(resultData.RegistrationNo, status_code);
                    #endregion
                }
                catch (Exception ex)
                {
                    setSweetAlertMsg(ex.Message, "warning");
                }
            }
            else
            {
                setSweetAlertMsg("Enter valid data !", "warning");
            }

            if (resultData != null && resultData.Flag > 0)
            {
                //SendSMSUpdateProcessICC(resultData.RegistrationNo, resultData.inspectionDate, resultData.MobileNo, resultData.appStatus);
                TempData["SuccessMsg"] = message;
                if (isRedirectNewAction)
                {
                    return RedirectToAction(actionName);
                }
                else
                {
                    if (resultData.appStatus == 4)
                    {
                        TempData["DatSetICC"] = resultData.RegisId;
                    }
                    return RedirectToAction(actionName, new { regisId = OTPL_Imp.CustomCryptography.Encrypt(model.regisIdICC.ToString()), status = OTPL_Imp.CustomCryptography.Encrypt(model.appStatus.ToString()) });
                }
            }
            else
            {
                model.appStatus = Convert.ToInt32(currAppStatus);
                ViewBag.DLLCommittee = comndb.GetCommitteeDetailsForDLL().ToList();
                return View(model);
            }
        }

        [AuthorizeAdmin(11)]
        public ActionResult ApprovedApplicationICC()
        {
            return View();
        }

        [AuthorizeAdmin(11)]
        public ActionResult ApprovedApplicationListICC(string registrationNo = "", string appDate = "")
        {
            List<ICCDetailsModel> lstMLCDetails = new List<ICCDetailsModel>();
            DataTable Ids = new DataTable();
            string strIds = "4";
            int id;
            if (objSM.RollID == 8)
            {
                id = 0;
            }
            else
            {
                id = Convert.ToInt32(objSM.UserID);
            }
            var arrIds = strIds.Split(',');
            Ids = objCom.strArrayToDataTable(arrIds);
            lstMLCDetails = objCHCDB.GetAllRegistration_ICC(id, Ids, registrationNo, appDate).ToList();

            return PartialView("_ApprovedApplicationListICC", lstMLCDetails);
        }

        [AuthorizeAdmin(11)]
        public ActionResult RejectedApplicationICC()
        {
            return View();
        }

        [AuthorizeAdmin(11)]
        public ActionResult RejectedApplicationListICC(string registrationNo = "", string appDate = "")
        {
            List<ICCDetailsModel> lstMLCDetails = new List<ICCDetailsModel>();
            DataTable Ids = new DataTable();
            string strIds = "0,3";
            int id;
            if (objSM.RollID == 8)
            {
                id = 0;
            }
            else
            {
                id = Convert.ToInt32(objSM.UserID);
            }
            var arrIds = strIds.Split(',');
            Ids = objCom.strArrayToDataTable(arrIds);
            lstMLCDetails = objCHCDB.GetAllRegistration_ICC(id, Ids, registrationNo, appDate).ToList();

            return PartialView("_RejectedApplicationListICC", lstMLCDetails);
        }

        void SendSMSUpdateProcessICC(string registrationNo, string inspectionDate, string mobileNo, int appstatus)
        {
            ForgotPasswordModel otpChCount = new ForgotPasswordModel();
            string txtmsg = "";
            if (appstatus == 0 || appstatus == 4)
            {
                txtmsg = "Dear Citizen,\n\nYour Application Form No. " + registrationNo + " for Registration of Issuance of Immunization Certificate for Children has been rejected. Kindly get in touch with the office of Chief Medical Officer for more details. You can also login to your dashboard for more details." + objSM.DisplayName + "\nMHFWD, UP";
            }
            else if (appstatus == 2)
            {
                txtmsg = "Dear Citizen,\n\n As per your application for Registration of Issuance of Immunization Certificate for Children, a committee for Inspection has been scheduled. We request to kindly be present at your medical establishment on the inspection date " + inspectionDate + " and  coordinate accordingly.\n\n" + objSM.DisplayName + "\nMHFWD, UP";
            }
            else if (appstatus == 6)
            {
                txtmsg = "Dear Citizen,\n\n Your Application Form No. " + registrationNo + " for Registration of Issuance of Immunization Certificate for Children has been approved. Please get in touch with the office of Chief Medical Officer to collect your certificate. You can also download the Certificate from your dashboard.\n\n" + objSM.DisplayName + "\nMHFWD, UP";
            }

            if (!string.IsNullOrEmpty(mobileNo) && !string.IsNullOrEmpty(txtmsg))
            {
                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }
        }

        [AuthorizeAdmin]
        public ActionResult PrintApplicationFormICC(string regisIdICC)
        {
            ICCModel model = new ICCModel();

            model = objICC.GetICCListBYRegistrationNo(0, "", Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdICC)));

            if (objSM.RollID >= 3 && objSM.RollID <= 5)
            {
                if (model == null || objSM.UserID != model.forwardtoId)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (objSM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.districtId).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (objSM.RollID != 8 && objSM.RollID != 13)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        [AuthorizeAdmin]
        public ActionResult BindICCChildList(string registrationNo)
        {
            ICCModel model = new ICCModel();
            model.appImmunList = objICC.getICCChild(registrationNo);
            return PartialView("_ViewICCChild", model.appImmunList);
        }

        #region ICC Certificate Rpt
        [AuthorizeAdmin(11)]
        public ActionResult ICCgeneratedCertificate(long regisIdICC)
        {
            string setPdfName = "", setDigitalPdfName = "";

            var res = objICC.GetDetailcertificate(regisIdICC);

            if (res != null && objSM.UserID != res[0].forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            if (res != null && res.Count > 0)
            {
                try
                {
                    ReportDocument rd = new ReportDocument();

                    rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rptCertificateICC.rpt"));
                    rd.SetDataSource(res);

                    setPdfName = "UnSigned_" + res[0].certificateNo;

                    string folderpath = "~/Content/writereaddata/UnSignedCertificate/ICC/" + objSM.DistrictName + "/";

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
                    if (System.IO.File.Exists(Server.MapPath(flName)))
                    {
                        int result = objICC.InsertUnSignedCertiPath_ICC(res[0].regisIdICC, flName);
                    }
                    rd.Close();
                    rd.Dispose();

                    if (ConfigurationManager.AppSettings["IsDigitalSign"].ToString() != "Y")
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
                    }
                    else
                    {
                        //digital sign

                        setDigitalPdfName = "ImmunizationChildCertificate" + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        string digitalFlName = folderpath + setDigitalPdfName + ".pdf";

                        var sigDetails = comndb.GetDigitalSignatureDetails(objSM.UserID);

                        float llx = 580;
                        float lly = 300;
                        float urx = 440;
                        float ury = 200;
                        Comman_Classes.DigitalCeritificateManager dcm = new Comman_Classes.DigitalCeritificateManager();
                        Comman_Classes.MetaData md = new Comman_Classes.MetaData()
                        {
                            Author = sigDetails.Author,
                            Title = "Immunization Child Certificate Authentication",
                            Subject = "Immunization Child Certificate",
                            Creator = sigDetails.Creator,
                            Producer = sigDetails.Producer,
                            Keywords = sigDetails.Keywords
                        };

                        string Signaturepath = Server.MapPath(sigDetails.Signaturepath);
                        dcm.signPDF(Server.MapPath(flName), Server.MapPath(digitalFlName), Signaturepath,
                         sigDetails.signpwd, "Authenticate Immunization Child Certificate", sigDetails.SigContact,
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

                        //digital sign end 
                    }

                    //digital sign end 
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error Occour to Downloading.";
                }
            }
            else
            {
                ViewBag.Message = "Detail not found.";
            }

            return View("DownloadFile");
        }
        #endregion

        #region ICC Certificate Rpt
        public int ICCgeneratedCertificateInt(long regisId)
        {
            string setPdfName = "", setDigitalPdfName = "";

            var res = objICC.GetDetailcertificate(regisId);

            if (res != null && objSM.UserID != res[0].forwardtoId)
            {
                return -1; // RedirectToAction("UnauthoriseAcess", "Home");
            }

            if (res != null && res.Count > 0)
            {
                try
                {
                    ReportDocument rd = new ReportDocument();

                    rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rptCertificateICC.rpt"));
                    rd.SetDataSource(res);

                    setPdfName = "UnSigned_" + res[0].certificateNo;

                    string folderpath = "~/Content/writereaddata/UnSignedCertificate/ICC/" + objSM.DistrictName + "/";

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
                    if (System.IO.File.Exists(Server.MapPath(flName)))
                    {
                        int result = objICC.InsertUnSignedCertiPath_ICC(res[0].regisIdICC, flName);
                    }
                    rd.Close();
                    rd.Dispose();

                    if (ConfigurationManager.AppSettings["IsDigitalSign"].ToString() != "Y")
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
                    }
                    else
                    {
                        //digital sign

                        setDigitalPdfName = "ImmunizationChildCertificate" + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        string digitalFlName = folderpath + setDigitalPdfName + ".pdf";

                        var sigDetails = comndb.GetDigitalSignatureDetails(objSM.UserID);

                        float llx = 580;
                        float lly = 300;
                        float urx = 440;
                        float ury = 200;
                        Comman_Classes.DigitalCeritificateManager dcm = new Comman_Classes.DigitalCeritificateManager();
                        Comman_Classes.MetaData md = new Comman_Classes.MetaData()
                        {
                            Author = sigDetails.Author,
                            Title = "Immunization Child Certificate Authentication",
                            Subject = "Immunization Child Certificate",
                            Creator = sigDetails.Creator,
                            Producer = sigDetails.Producer,
                            Keywords = sigDetails.Keywords
                        };

                        string Signaturepath = Server.MapPath(sigDetails.Signaturepath);
                        dcm.signPDF(Server.MapPath(flName), Server.MapPath(digitalFlName), Signaturepath,
                         sigDetails.signpwd, "Authenticate Immunization Child Certificate", sigDetails.SigContact,
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

                        //digital sign end 
                    }

                    //digital sign end 
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error Occour to Downloading.";
                }
            }
            else
            {
             
            }

            return 1; 
        }
        #endregion

        //public JsonResult UploadCertificateFileNUH(HttpPostedFileBase File)
        //{
        //    long regisId = Convert.ToInt64(Session["registrationIdNUH"].ToString());

        //    var lstNUHDetails = objCMODB.GetNUHCertificate(regisId);

        //    string Dirpath = "~/Content/writereaddata/" + objSM.DistrictName + "/";
        //    string path = "";
        //    string filename = "";
        //    if (!Directory.Exists(Server.MapPath(Dirpath)))
        //    {
        //        Directory.CreateDirectory(Server.MapPath(Dirpath));
        //    }
        //    string ext = Path.GetExtension(File.FileName);
        //    if (ext.ToLower() == ".jpg" || ext.ToLower() == ".pdf")
        //    {

        //        filename = lstNUHDetails.CertificateNo + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;
        //        string completepath = Path.Combine(Server.MapPath(Dirpath), filename);
        //        if (System.IO.File.Exists(completepath))
        //        {
        //            System.IO.File.Delete(completepath);
        //        }

        //        long size = File.ContentLength;
        //        if (size > 2097152)
        //        {
        //            path = "SNV";//"warning_Maximum 2MB file size are allowed";
        //        }
        //        else
        //        {
        //            File.SaveAs(completepath);
        //            path = Dirpath + filename;
        //        }
        //    }
        //    else
        //    {
        //        path = "TNV";//"warning_Invalid File Format only pdf and jpg files are allow!";
        //    }

        //    List<string> plist = new List<string> { filename, path };
        //    return Json(plist);
        //}
        //[HttpGet]
        //public ActionResult UploadCertificateNUH(string regisId)
        //{
        //    Session["registrationIdNUH"] = OTPL_Imp.CustomCryptography.Decrypt(regisId);
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult UploadCertificateNUH(FormCollection frm)
        //{
        //    var CertificatePath = frm.GetValues("hdnfileUploadCertificate");
        //    string IPAddress = Common.GetIPAddress();

        //    var res = objCMODB.UpdateNUHCertificate(Convert.ToInt64(Session["registrationIdNUH"]), CertificatePath[0], objSM.UserID, IPAddress);
        //    if (res.Flag == 1)
        //    {
        //        TempData["Message"] = res.Msg;
        //    }
        //    return RedirectToAction("ApprovedApplicationNUH", "CMO");
        //}

        //#region Certificate Rpt Riya
        //public ActionResult MLCGeneratedCertificate(long regisIdMLC)
        //{
        //    string stausMessage = "";
        //    string setPdfName = "", setDigitalPdfName = "";
        //    var res = objCMODB.GetDetail(regisIdMLC);
        //    var res2 = objCMODB.getNUHChildRpt(res[0].regisIdNUH);
        //    try
        //    {
        //        ReportDocument rd = new ReportDocument();
        //        rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rpt_NUHcertificate.rpt"));
        //        rd.SetDataSource(res);
        //        ReportDocument subShows = rd.Subreports["rpt_NUHcertificateChild.rpt"];
        //        subShows.SetDataSource(res2);
        //        String dtnow = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        //        setPdfName = "MedicalEstablishment" + "_" + dtnow;
        //        setDigitalPdfName = "MedicalEstablishmentCertificateDigitalSigned" + "_" + dtnow;
        //        string folderpath = "~/Content/writereaddata/PDF/";
        //        if (!System.IO.Directory.Exists(Server.MapPath(folderpath)))
        //        {
        //            System.IO.Directory.CreateDirectory(Server.MapPath(folderpath));
        //        }
        //        string flName = folderpath + setPdfName + ".pdf";
        //        string digitalFlName = folderpath + setDigitalPdfName + ".pdf";
        //        rd.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //        rd.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //        rd.ExportOptions.DestinationOptions = new DiskFileDestinationOptions();
        //        ((DiskFileDestinationOptions)rd.ExportOptions.DestinationOptions).DiskFileName = Server.MapPath(flName);
        //        rd.Export();
        //        //FileInfo fileInfo = new FileInfo(Server.MapPath(flName));
        //        rd.Close();
        //        rd.Dispose();
        //        if (ConfigurationManager.AppSettings["IsDigitalSign"].ToString() == "N")
        //        {
        //            FileInfo fileInfo = new FileInfo(Server.MapPath(flName));
        //            Response.ClearContent();
        //            Response.ClearHeaders();
        //            Response.ContentType = "application/pdf";
        //            Response.AppendHeader("Content-Disposition", "attachment; filename=" + setPdfName + ".pdf");
        //            Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
        //            Response.WriteFile(flName);
        //            Response.Flush();
        //            Response.Close();
        //            if (System.IO.File.Exists(Server.MapPath(flName)))
        //            {
        //                System.IO.File.Delete(Server.MapPath(flName));
        //            }
        //        }
        //        else
        //        {
        //            ////////////digital sign

        //            var sigDetails = comndb.GetDigitalSignatureDetails(objSM.UserID);

        //            float llx = 580;
        //            float lly = 300;
        //            float urx = 440;
        //            float ury = 200;
        //            Comman_Classes.DigitalCeritificateManager dcm = new Comman_Classes.DigitalCeritificateManager();
        //            Comman_Classes.MetaData md = new Comman_Classes.MetaData()
        //            {
        //                Author = sigDetails.Author,
        //                Title = "Medical Establishment Certificate Authentication",
        //                Subject = "Medical Establishment Certificate",
        //                Creator = sigDetails.Creator,
        //                Producer = sigDetails.Producer,
        //                Keywords = sigDetails.Keywords
        //            };

        //            string Signaturepath = Server.MapPath(sigDetails.Signaturepath);
        //            dcm.signPDF(Server.MapPath(flName), Server.MapPath(digitalFlName), Signaturepath,
        //             sigDetails.signpwd, "Authenticate Medical Establishment Certificate", sigDetails.SigContact,
        //             sigDetails.SigLocation, true, llx, lly, urx, ury, 1, md);

        //            FileInfo fileInfo = new FileInfo(Server.MapPath(digitalFlName));
        //            Response.ClearContent();
        //            Response.ClearHeaders();
        //            Response.ContentType = "application/pdf";
        //            Response.AppendHeader("Content-Disposition", "attachment; filename=" + setDigitalPdfName + ".pdf");
        //            Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
        //            Response.WriteFile(Server.MapPath(digitalFlName));
        //            Response.Flush();
        //            Response.Close();

        //            if (System.IO.File.Exists(Server.MapPath(flName)))
        //            {
        //                System.IO.File.Delete(Server.MapPath(flName));
        //            }

        //            if (System.IO.File.Exists(Server.MapPath(digitalFlName)))
        //            {
        //                System.IO.File.Delete(Server.MapPath(digitalFlName));
        //            }

        //            //////////////////////////digital sign end

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        stausMessage = "error_Error Occour to Downloading, Please try again.";
        //    }
        //    return RedirectToAction("ApprovedApplicationNUH");
        //}
        //#endregion

        #endregion

        #region AmitendraSingh
        //------ILC
        [AuthorizeAdmin(2)]
        [HttpGet]
        public ActionResult UploadCertificateILC(string regisId)
        {
            long regId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId));

            var res = objCHCDB.GetILCdetailCertificateRpt(regId);

            if (res != null && objSM.UserID != res[0].forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            Session["registrationIdILC"] = regId;

            return View();
        }

        [AuthorizeAdmin(2)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadCertificateILC(FormCollection frm)
        {
            var CertificatePath = frm.GetValues("hdnfileUploadCertificate");
            string IPAddress = Common.GetIPAddress();
            if (CertificatePath[0] != null || CertificatePath[0] != "")
            {
                var res = objCHCDB.UpdateILCCertificate(Convert.ToInt64(Session["registrationIdILC"]), CertificatePath[0], objSM.UserID, IPAddress);
                if (res.Flag == 1)
                {
                    TempData["Message"] = res.Msg;
                }
                return RedirectToAction("ApprovedApplicationILC", "CHC");
            }
            else
            {
                TempData["msg"] = "Please choose a file!";
                TempData["msgstatus"] = "warning";
                return RedirectToAction("UploadCertificateILC");
            }
        }

        public JsonResult UploadCertificateFileILC(HttpPostedFileBase File)
        {
            long regisId = Convert.ToInt64(Session["registrationIdILC"].ToString());

            var lstNUHDetails = objCHCDB.GetCertificate("ILC", regisId);

            string Dirpath = "~/Content/SignedCertificate/ILC/" + objSM.DistrictName + "/";
            string path = "";
            string filename = "";
            if (!Directory.Exists(Server.MapPath(Dirpath)))
            {
                Directory.CreateDirectory(Server.MapPath(Dirpath));
            }
            string ext = Path.GetExtension(File.FileName);
            if (ext.ToLower() == ".jpg" || ext.ToLower() == ".pdf")
            {

                filename = "Signed_" + lstNUHDetails.CertificateNo + ext;
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

        //---FIC
        [AuthorizeAdmin(3)]
        public ActionResult UploadCertificateFIC(string regisId)
        {
            long regId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId));

            var res = objCHCDB.GetFICdetailCertificateRpt(regId);

            if (res != null && objSM.UserID != res[0].forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            Session["registrationIdFIC"] = regId;

            return View();
        }

        [AuthorizeAdmin(3)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadCertificateFIC(FormCollection frm)
        {
            var CertificatePath = frm.GetValues("hdnfileUploadCertificate");
            string IPAddress = Common.GetIPAddress();
            if (CertificatePath[0] != null || CertificatePath[0] != "")
            {
                var res = objCHCDB.UpdateFICCertificate(Convert.ToInt64(Session["registrationIdFIC"]), CertificatePath[0], objSM.UserID, IPAddress);
                if (res.Flag == 1)
                {
                    TempData["Message"] = res.Msg;
                }
                return RedirectToAction("ApprovedApplicationFIC", "CHC");
            }
            else
            {
                TempData["msg"] = "Please choose a file!";
                TempData["msgstatus"] = "warning";
                return RedirectToAction("UploadCertificateFIC");
            }
        }

        public JsonResult UploadCertificateFileFIC(HttpPostedFileBase File)
        {
            long regisId = Convert.ToInt64(Session["registrationIdFIC"].ToString());

            var lstNUHDetails = objCHCDB.GetCertificate("FIC", regisId);

            string Dirpath = "~/Content/SignedCertificate/FIC/" + objSM.DistrictName + "/";
            string path = "";
            string filename = "";
            if (!Directory.Exists(Server.MapPath(Dirpath)))
            {
                Directory.CreateDirectory(Server.MapPath(Dirpath));
            }
            string ext = Path.GetExtension(File.FileName);
            if (ext.ToLower() == ".jpg" || ext.ToLower() == ".pdf")
            {

                filename = "Signed_" + lstNUHDetails.CertificateNo + ext;
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

        //-----MLC
        [AuthorizeAdmin(9)]
        public ActionResult UploadCertificateMLC(string regisId)
        {
            long regId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId));

            var res = objMlc.GetMLCDetails(regId);

            if (res != null && objSM.UserID != res[0].forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            Session["registrationIdMLC"] = regId;

            return View();
        }

        [AuthorizeAdmin(9)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadCertificateMLC(FormCollection frm)
        {
            var CertificatePath = frm.GetValues("hdnfileUploadCertificate");
            string IPAddress = Common.GetIPAddress();
            if (CertificatePath[0] != null || CertificatePath[0] != "")
            {
                var res = objCHCDB.UpdateMLCCertificate(Convert.ToInt64(Session["registrationIdMLC"]), CertificatePath[0], objSM.UserID, IPAddress);
                if (res.Flag == 1)
                {
                    TempData["Message"] = res.Msg;
                }
                return RedirectToAction("ApprovedApplicationMLC", "CHC");
            }
            else
            {
                TempData["msg"] = "Please choose a file!";
                TempData["msgstatus"] = "warning";
                return RedirectToAction("UploadCertificateMLC");
            }
        }

        public JsonResult UploadCertificateFileMLC(HttpPostedFileBase File)
        {
            long regisId = Convert.ToInt64(Session["registrationIdMLC"].ToString());

            var lstNUHDetails = objCHCDB.GetCertificate("MLC", regisId);

            string Dirpath = "~/Content/SignedCertificate/MLC/" + objSM.DistrictName + "/";
            string path = "";
            string filename = "";
            if (!Directory.Exists(Server.MapPath(Dirpath)))
            {
                Directory.CreateDirectory(Server.MapPath(Dirpath));
            }
            string ext = Path.GetExtension(File.FileName);
            if (ext.ToLower() == ".jpg" || ext.ToLower() == ".pdf")
            {

                filename = "Signed_" + lstNUHDetails.CertificateNo + ext;
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

        //-----DEC
        [AuthorizeAdmin(6)]
        public ActionResult UploadCertificateDEC(string regisId)
        {
            long regId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId));

            var res = objdb.GetDECDetails(regId);

            if (res != null && objSM.UserID != res[0].forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            Session["registrationIdDEC"] = regId;

            return View();
        }

        [AuthorizeAdmin(6)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadCertificateDEC(FormCollection frm)
        {
            var CertificatePath = frm.GetValues("hdnfileUploadCertificate");
            string IPAddress = Common.GetIPAddress();
            if (CertificatePath[0] != null || CertificatePath[0] != "")
            {
                var res = objCHCDB.UpdateDECCertificate(Convert.ToInt64(Session["registrationIdDEC"]), CertificatePath[0], objSM.UserID, IPAddress);
                if (res.Flag == 1)
                {
                    TempData["Message"] = res.Msg;
                }
                return RedirectToAction("ApprovedApplicationDEC", "CHC");
            }
            else
            {
                TempData["msg"] = "Please choose a file!";
                TempData["msgstatus"] = "warning";
                return RedirectToAction("UploadCertificateDEC");
            }
        }

        public JsonResult UploadCertificateFileDEC(HttpPostedFileBase File)
        {
            long regisId = Convert.ToInt64(Session["registrationIdDEC"].ToString());

            var lstNUHDetails = objCHCDB.GetCertificate("DEC", regisId);

            string Dirpath = "~/Content/SignedCertificate/DEC/" + objSM.DistrictName + "/";
            string path = "";
            string filename = "";
            if (!Directory.Exists(Server.MapPath(Dirpath)))
            {
                Directory.CreateDirectory(Server.MapPath(Dirpath));
            }
            string ext = Path.GetExtension(File.FileName);
            if (ext.ToLower() == ".jpg" || ext.ToLower() == ".pdf")
            {

                filename = "Signed_" + lstNUHDetails.CertificateNo + ext;
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

        //-----ICC
        [AuthorizeAdmin(11)]
        public ActionResult UploadCertificateICC(string regisId)
        {
            long regId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId));

            var res = objICC.GetDetailcertificate(regId);

            if (res != null && objSM.UserID != res[0].forwardtoId)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            Session["registrationIdICC"] = regId;

            return View();
        }

        [AuthorizeAdmin(11)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadCertificateICC(FormCollection frm)
        {
            var CertificatePath = frm.GetValues("hdnfileUploadCertificate");
            string IPAddress = Common.GetIPAddress();
            if (CertificatePath[0] != null || CertificatePath[0] != "")
            {
                var res = objCHCDB.UpdateICCCertificate(Convert.ToInt64(Session["registrationIdICC"]), CertificatePath[0], objSM.UserID, IPAddress);
                if (res.Flag == 1)
                {
                    TempData["Message"] = res.Msg;
                }
                return RedirectToAction("ApprovedApplicationICC", "CHC");
            }
            else
            {
                TempData["msg"] = "Please choose a file!";
                TempData["msgstatus"] = "warning";
                return RedirectToAction("UploadCertificateICC");
            }
        }

        public JsonResult UploadCertificateFileICC(HttpPostedFileBase File)
        {
            long regisId = Convert.ToInt64(Session["registrationIdICC"].ToString());

            var lstNUHDetails = objCHCDB.GetCertificate("ICC", regisId);

            string Dirpath = "~/Content/SignedCertificate/ICC/" + objSM.DistrictName + "/";
            string path = "";
            string filename = "";
            if (!Directory.Exists(Server.MapPath(Dirpath)))
            {
                Directory.CreateDirectory(Server.MapPath(Dirpath));
            }
            string ext = Path.GetExtension(File.FileName);
            if (ext.ToLower() == ".jpg" || ext.ToLower() == ".pdf")
            {

                filename = "Signed_" + lstNUHDetails.CertificateNo + ext;
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

        //-----IMC
        [AuthorizeAdmin(5)]
        public ActionResult UploadCertificateIMC(string regisId)
        {
            long regId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId));

            var res = objIMC_DB.GetDetailcertificate(regId);

            if (res != null && objSM.UserID != res[0].forwardType)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            Session["registrationIdIMC"] = regId;

            return View();
        }

        [AuthorizeAdmin(5)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadCertificateIMC(FormCollection frm)
        {
            var CertificatePath = frm.GetValues("hdnfileUploadCertificate");
            string IPAddress = Common.GetIPAddress();
            if (CertificatePath[0] != null || CertificatePath[0] != "")
            {
                var res = objCHCDB.UpdateIMCCertificate(Convert.ToInt64(Session["registrationIdIMC"]), CertificatePath[0], objSM.UserID, IPAddress);
                if (res.Flag == 1)
                {
                    TempData["Message"] = res.Msg;
                }
                return RedirectToAction("ApprovedApplicationIMC", "CHC");
            }
            else
            {
                TempData["msg"] = "Please choose a file!";
                TempData["msgstatus"] = "warning";
                return RedirectToAction("UploadCertificateIMC");
            }
        }

        public JsonResult UploadCertificateFileIMC(HttpPostedFileBase File)
        {
            long regisId = Convert.ToInt64(Session["registrationIdIMC"].ToString());

            var lstNUHDetails = objCHCDB.GetCertificate("IMC", regisId);

            string Dirpath = "~/Content/SignedCertificate/IMC/" + objSM.DistrictName + "/";
            string path = "";
            string filename = "";
            if (!Directory.Exists(Server.MapPath(Dirpath)))
            {
                Directory.CreateDirectory(Server.MapPath(Dirpath));
            }
            string ext = Path.GetExtension(File.FileName);
            if (ext.ToLower() == ".jpg" || ext.ToLower() == ".pdf")
            {

                filename = "Signed_" + lstNUHDetails.CertificateNo + ext;
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
