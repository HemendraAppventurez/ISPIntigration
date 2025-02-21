using CCSHealthFamilyWelfareDept.DAL;
using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCSHealthFamilyWelfareDept.Filters;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    [AuthorizeUser]
    public class MLCController : Controller
    {
        Common_DB objComDB = new Common_DB();
        MLC_DB objdb = new MLC_DB();
        Common objCom = new Common();
        Common_DB OBJcOMdb = new Common_DB();
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
                return RedirectToAction("MLCDashBoard", "MLC");
            }
            else if (res.isExists == 0)
            {
                return RedirectToAction("RegistrationInstructions", "MLC");
            }
            return View();
        }

        #endregion

        public ActionResult RegistrationInstructions()
        {
            return View();
        }

        public ActionResult MLCDashBoard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MedicoLegalCertificate()
        {
            long userId = objSM.UserID;
            MLCModel model = new MLCModel();
            ViewBag.Tehsil = OBJcOMdb.GetDropDownList(21, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = objComDB.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = OBJcOMdb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.IdType = objdb.GetIdType().Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.forwardTo = Enumerable.Empty<SelectListItem>();
            ViewBag.forwardTypes = objCMODB.rblforwardType().Where(m => m.forwardtypeId == 3 || m.forwardtypeId == 4 || m.forwardtypeId == 5).ToList();
            return View(model);
        }

        public JsonResult BindForwardDropdownlist(long rollId, int opdDistrictId)
        {
            var res = objdb.bindDropdownlist(rollId, opdDistrictId).Select(m => new SelectListItem { Text = m.forwardtoName, Value = m.forwardtoId.ToString() });
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckMobileExistence(string mobileNo)
        {
            var user = objdb.CheckEmailMobileExistence(mobileNo, "M");
            bool isExisting = user.isExists == 0;
            return Json(isExisting);
        }

        [HttpPost]
        public ActionResult MedicoLegalCertificate(MLCModel model, FormCollection form)
        {

            #region Bulk Insertion Kailash Joshi
            var enquirydetails = form.GetValues("enquiryDetails");
            int count = enquirydetails.Count();
            string XmlData = "<EnquiryDetails>";
            long regisByuser = objSM.UserID;
            for (int i = 0; i < count; i++)
            {
                if (enquirydetails[i].ToString() == "")
                {
                    //XmlData = string.Empty;
                }
                else
                {
                    XmlData += "<Enquiry><RegisByUser>" + regisByuser + "</RegisByUser>" +
                        "<EnquiryDetails>" + enquirydetails[i] + "</EnquiryDetails>"
                            + "</Enquiry>";
                }
            }
            XmlData += "</EnquiryDetails>";
            #endregion
            model.regByUser = objSM.UserID;
            model.regBytransIp = Common.GetIPAddress();
            model.transIp = Common.GetIPAddress();
            model.requestKey = objSM.AppRequestKey;

            if (model.patientBroughtBy != "Relative")
            {
                ModelState["broughtByPersonrelation"].Errors.Clear();
            }
            var res = objdb.MLCInsertUpdate(model, XmlData);

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
                            //SendSMS(res.RegistrationNo, res.MobileNo);
                            TempData["RegisIdMLC"] = res.RegisId;
                            TempData["RegistrationNo"] = res.RegistrationNo;
                            return RedirectToAction("RegistrationConfirmation");
                        }
                        else
                        {
                            int exeRsult = objdb.DeleteRegistrationMLC(res.RegisId);
                            setSweetAlertMsg("Invalid Request or Service Unavailable", "warning");
                            ModelState.Clear();
                        }
                    }
                    else
                    {
                        //SendSMS(res.RegistrationNo, res.MobileNo);
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

            ViewBag.Tehsil = OBJcOMdb.GetDropDownList(21, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.State = objComDB.GetDropDownList(6, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = OBJcOMdb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.IdType = objdb.GetIdType().Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.forwardTo = Enumerable.Empty<SelectListItem>();
            ViewBag.forwardTypes = objCMODB.rblforwardType().Where(m => m.forwardtypeId == 3 || m.forwardtypeId == 4 || m.forwardtypeId == 5).ToList();
            return RedirectToAction("MedicoLegalCertificate");
        }

        [HttpGet]
        public ActionResult ViewMLCList()
        {
            long regisByuser = objSM.UserID;
            MLCModel model = new MLCModel();
            model.MLCModelList = objdb.GetMLCList(regisByuser);
            return View(model.MLCModelList);
        }

        public ActionResult MLCDetails(string registrationNo, string regisIdMLC)
        { 
            long regisByuser = objSM.UserID;
            registrationNo = OTPL_Imp.CustomCryptography.Decrypt(registrationNo);
            MLCModel model = new MLCModel();

            model = objdb.GetMLCListBYRegistrationNo(regisByuser, registrationNo);

            if (model == null || objSM.UserID != model.regByUser)
            {
                return RedirectToAction("UserUnauthoriseAcess", "Home");
            }

            TempData["reg"] = registrationNo;

            return View(model);
        }

        public ActionResult PrintApplicationForm(string registrationNo, string regisIdMLC)
        {
            long regisByuser = objSM.UserID;
            registrationNo = OTPL_Imp.CustomCryptography.Decrypt(registrationNo);
            MLCModel model = new MLCModel();

            model = objdb.GetMLCListBYRegistrationNo(regisByuser, registrationNo);

            if (model == null || objSM.UserID != model.regByUser)
            {
                return RedirectToAction("UserUnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            TempData["reg"] = registrationNo;

            return View(model);
        }

        public ActionResult BindChildList(string registrationNo)
        {
            MLCModel model = new MLCModel();

            model.MLCModelList = objdb.getMLCChild(registrationNo);
            return PartialView("_ViewMLCChild", model.MLCModelList);
        }

        #region Certificate Rpt Riya
        public ActionResult MLCgeneratedCertificate(string regisIdMLC, string certGenrBy)
        {

            string stausMessage = "";
            string setPdfName = "", setDigitalPdfName = "";
            var res = objdb.GetMLCDetails(Convert.ToInt64(regisIdMLC));
            var res2 = objdb.getMLCChilds(res[0].regisIdMLC);
            try
            {
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/RPT"), "rpt_MLCcertificate.rpt"));
                rd.SetDataSource(res);
                ReportDocument subShows = rd.Subreports["rpt_MLCchildCertificate.rpt"];
                subShows.SetDataSource(res2);
                String dtnow = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                setPdfName = "MedcoLegal" + "_" + dtnow;
                setDigitalPdfName = "MedcoLegalCertificateDigitalSigned" + "_" + dtnow;
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

                    var sigDetails = OBJcOMdb.GetDigitalSignatureDetails(Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(certGenrBy)));

                    float llx = 580;
                    float lly = 290;
                    float urx = 440;
                    float ury = 190;
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

                    //////////////////////////digital sign end
                }
            }
            catch (Exception ex)
            {
                stausMessage = "error_Error Occour to Downloading, Please try again.";
            }
            return RedirectToAction("ViewMLCList");
        }
        #endregion

        public ActionResult RegistrationConfirmation()
        {
            ViewBag.MLCRegisId = OTPL_Imp.CustomCryptography.Encrypt(TempData["RegisIdMLC"].ToString());
            ViewBag.MLCRegistrationNo = TempData["RegistrationNo"];
            return View();
        }

        void SendSMS(string registrationNo, string mobileNo)
        {

            if (mobileNo != "")
            {
                ForgotPasswordModel otpChCount = new ForgotPasswordModel();
                string txtmsg = "";


                txtmsg = "Dear Citizen,\n\nYour Application form for Issuance of Medico-Legal Certificate has been submitted successfully. Your Application Number is " + registrationNo + ".\nPlease use this Application Number for further references.\n\nTechnical Team\nMHFWD, UP";


                string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo));

                objAccDb.SMSLog(txtmsg, mobileNo, status, registrationNo);
            }

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

        public ActionResult GetCertificateMLC()
        {
            ViewBag.forwardTypes = objCMODB.rblforwardType().Where(m => m.forwardtypeId == 3 || m.forwardtypeId == 4 || m.forwardtypeId == 5).ToList();
            ViewBag.District = OBJcOMdb.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.forwardTo = Enumerable.Empty<SelectListItem>();

            return View();
        }

        public ActionResult GetCertificateListMLC(string OPDNumber = "", string patientName = "", string patientGender = "", string MLCDate = "", int districtId = 0, long healthUnitTypeId = 0, long healthUnitId = 0)
        {
            string msg = "";
            List<MLCDetailsModel> lstMLCDetails = new List<MLCDetailsModel>();

            if (!string.IsNullOrEmpty(OPDNumber) && !string.IsNullOrEmpty(OPDNumber) && !string.IsNullOrEmpty(OPDNumber) && !string.IsNullOrEmpty(OPDNumber) && districtId > 0 && healthUnitTypeId > 0 && healthUnitId > 0)
            {
                try
                {
                    lstMLCDetails = objdb.GetCertificateMLC(OPDNumber, patientName, patientGender, MLCDate, districtId, healthUnitTypeId, healthUnitId).ToList();
                }
                catch (Exception ex)
                {
                    msg = "error";
                }
            }
            else
            {
                msg = "empty";
            }

            if (string.IsNullOrEmpty(msg))
            {
                return PartialView("_GetCertificateListMLC", lstMLCDetails);
            }
            else
            {
                return Content(msg);
            }
        }

        #region Download Certificate MLC
        public ActionResult DownloadCertificateMLC(string regisId = "", string nomId = "", int step = 0)
        {
            bool isValid = true;

            MLCNomineeModel model = new MLCNomineeModel();

            if (step == 0 && !string.IsNullOrEmpty(regisId))
            {
                ViewBag.IdentityProof = objComDB.GetDropDownList(46, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                model = objdb.GetNomineeMLC(objSM.UserID);
                model.regisIdMLC = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId));
                model.downloadStep = step;
            }
            else if (step == 1 && !string.IsNullOrEmpty(nomId) && !string.IsNullOrEmpty(regisId))
            {
                long longNomineeId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(nomId));

                model = objdb.GetDetailByNomneeIdMLC(longNomineeId);
                model.regisIdMLC = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisId));
                model.downloadStep = step;
                string smsFlag = SendOTP(model.mobileNumber);
                if (smsFlag == "success")
                {
                    setSweetAlertMsg("OTP is sent to your registered Mobile No.", "success");
                }
                else if (smsFlag == "exit")
                {
                    TempData["WarningMsg"] = "You have reached your maximum limit of sending OTP for Date :" + DateTime.Now.ToString("dd/MM/yyyy") + ", please try later.";
                    return RedirectToAction("GetCertificateMLC");
                }
                else
                {
                    setSweetAlertMsg(smsFlag, "warning");
                }
            }
            else if (step == 2 && !string.IsNullOrEmpty(nomId))
            {
                model = objdb.GetDetailByNomneeIdMLC(Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(nomId)));
                model.downloadStep = step;
            }
            else
            {
                isValid = false;
            }

            if (isValid)
            {
                return View(model);
            }
            else
            {
                TempData["WarningMsg"] = "Problem Occur, Kindly try again.";
                return RedirectToAction("GetCertificateMLC");
            }
        }

        [HttpGet]
        public string SendOTP(string mobileNo = "")
        {
            string res = "";
            string otp = generateno();
            objSM.OTP = otp;

            if (!string.IsNullOrEmpty(mobileNo))
            { 
                int MaxAllowedSMS = System.Configuration.ConfigurationManager.AppSettings["MaxAllowedSMS"] == null ? 5 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxAllowedSMS"].ToString());

                var a = OBJcOMdb.GetOTPCount(mobileNo).ToList().FirstOrDefault();
                
                if (a != null && a.Id <= MaxAllowedSMS)
                { 
                   // string txtmsg = "Dear User,\n\nYour OTP for download Medico-Legal Certificate is " + otp + ".\nPlease do not share this OTP with anyone for security reasons.\n\n Technical Team\nMHFWD, UP";
                    string txtmsg = "Your OTP for MedicoLegal is " + otp + "-OMNINET TECHNOLOGIES PVT LTD";

                    string status = SMS.SMSSend(txtmsg, Convert.ToString(mobileNo), "1607100000000032462");

                    objAccDb.SMSLog(txtmsg, mobileNo, status, Convert.ToString(objSM.UserID));

                    if (status.ToLower() == "success")
                    {
                        res = "success"; 
                    }
                    else
                    {
                        res = "SMS Service is not working currently. please try again later. ! ";
                    }
                }
                else
                {
                    res = "exit";
                    //res = "You have reached your maximum limit of sending OTP for Date :" + DateTime.Now.ToString("dd/MM/yyyy") + ", please try later.";
                }
            }
            else
            {
                res = "Current Session is terminated continue to login and try again...!";
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

        #region Resend OTP

        [HttpPost]
        public ActionResult ResendOTP(string mobileNo = "")
        {

            if (!string.IsNullOrEmpty(mobileNo))
            {
                string res = SendOTP(mobileNo);
                if (res == "success")
                {
                    return Content("success_OTP is sent to your registered Mobile No.");
                }
                else if (res == "exit")
                {
                    return Content("EXIT_You have reached your maximum limit of sending OTP for Date :" + DateTime.Now.ToString("dd/MM/yyyy") + ", please try later.");
                }
                else
                {
                    return Content("warning_" + res);
                }
            }
            else
            {
                return Content("warning_Problem to re-send OTP !");
            }
        }

        #endregion

        public JsonResult UploadFile(HttpPostedFileBase File)
        {
            string Dirpath = "~/Content/writereaddata/MLC/";
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

        [HttpPost]
        public ActionResult DownloadCertificateMLC(MLCNomineeModel model)
        {
            bool isRedirect = false, isValid = true;
            string msg = "", _regisId = "", _nomineeId = "";
            int _downloadStep = model.downloadStep + 1;
            model.downloadedBy = objSM.UserID;
            model.transIp = Common.GetIPAddress(); 
            
            try
            { 
                if (model.downloadStep == 1)
                {
                    if (objSM.OTP != model.OTP)
                    {
                        isValid = false;
                        msg = "OTP is not Valid please enter a valid OTP.";
                    } 
                }

                if (isValid)
                {
                    var result = objdb.InsertNomineeDetailsMLC(model);
                    if (result != null && result.Flag == 1)
                    {
                        _nomineeId = OTPL_Imp.CustomCryptography.Encrypt(result.RegisId.ToString());
                        _regisId = OTPL_Imp.CustomCryptography.Encrypt(model.regisIdMLC.ToString());
                        isRedirect = true;

                        if (model.downloadStep == 1 && !string.IsNullOrEmpty(model.mobileNumber))
                        {
                            var r = OBJcOMdb.UpdateOTPUsedStatus(model.mobileNumber);
                        }
                    }
                    else
                    {
                        msg = "Problem occur, Try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "Problem occur, Try again.";
            }

            if (isRedirect)
            {
                TempData["WarningMsg"] = msg;
                return RedirectToAction("DownloadCertificateMLC", new { regisId = _regisId, nomId = _nomineeId, step = _downloadStep });
            }
            else
            {
                setSweetAlertMsg(msg, "warning");
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult ViewDownloadedCertificateMLC()
        { 
            return View();
        }

        public ActionResult ViewDownloadedCertificateListMLC()
        {
            var result = objdb.GetNomineeLog(objSM.UserID);
            return PartialView("_ViewDownloadedCertificateListMLC", result);
        }

        #endregion 
    }
}
