using CCSHealthFamilyWelfareDept.DAL;
using CCSHealthFamilyWelfareDept.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCSHealthFamilyWelfareDept.Controllers
{

    public class PublicController : Controller
    {
        CMO_DB objCMODB = new CMO_DB();
        NUH_DB objNUHDB = new NUH_DB();
        MER_DB objMER_DB = new MER_DB();
        AGC_DB objAGC_DB = new AGC_DB();
        FAP_DB objFAP_DB = new FAP_DB();
        DEC_DB objdb = new DEC_DB();
        CHC_DB objCHCDB = new CHC_DB();
        FIC_DB objFICdb = new FIC_DB();
        ILC_DB objILC_DB = new ILC_DB();
        MLC_DB objMlc = new MLC_DB();
        ICC_DB objICC = new ICC_DB();
        IMC_DB objIMC_DB = new IMC_DB();
        Common_DB objComDB = new Common_DB();
        Report_DB ObjRptDb = new Report_DB();
        //SessionManager SM = new SessionManager();

        #region Report(Replica of Admin drill down report)
        [HttpGet]
        public ActionResult ApplicationReport()
        {
            return View();
        }
        public ActionResult CMOSRVCountReport(string rollId = "")
        {
            if (!string.IsNullOrEmpty(rollId))
            {

                ViewBag.ZoneId = 0;
                ViewBag.DLLZone = objComDB.GetZoneForDLL().ToList();
                ViewBag.RollId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(rollId));
                if (ViewBag.RollId == 2)
                {
                    ViewBag.RollName = "CMO";
                    ViewBag.RollNameHi = "सी.एम.ओ.";
                }
                else if (ViewBag.RollId == 3)
                {
                    ViewBag.RollName = "CHC";
                    ViewBag.RollNameHi = "सी.एच.सी.";
                }
                else if (ViewBag.RollId == 4)
                {
                    ViewBag.RollName = "PHC";
                    ViewBag.RollNameHi = "पी.एच.सी.";
                }
                else if (ViewBag.RollId == 5)
                {
                    ViewBag.RollName = "CMS(DH)";
                    ViewBag.RollNameHi = "सी.एम.एस(डी.एच)";
                }
                return View();
            }
            else
            {
                return RedirectToAction("ApplicationReport");
            }
        }
        public ActionResult CMOSRVCountReportList(long rollId = 0, int zoneId = 0)
        {
            long RollID = 8;
            var result = ObjRptDb.CMOSRVCountReport(zoneId, rollId, RollID);
            return PartialView("_CMOSRVCountReportList", result);
        }

        #region rpt CMO
        public ActionResult CMOSRVCountReportDivisionWise(string zoneId = "", string serviceId = "")
        {
            ViewBag.ZoneId = zoneId;
            if (!string.IsNullOrEmpty(serviceId))
            {
                ViewBag.ServiceId = serviceId;
            }
            return View();
        }
        public ActionResult CMOSRVCountReportDivisionWiseList(string zoneId = "", string serviceId = "")
        {
            int srvId = serviceId != "" ? Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId)) : 0;
            ViewBag.ServiceId = srvId;
            var result = ObjRptDb.CMOSRVCountReport_DivisionWise(Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(zoneId)), 0, srvId);
            return PartialView("_CMOSRVCountReportDivisionWiseList", result);
        }
        #endregion

        public ActionResult ApplicationDetails(string rollId = "", string appCurrStatus = "", string zoneId = "", string districtId = "", string serviceId = "", string isLessFiftyThousan = "", string userId = "")
        {
            ViewBag.CurrLogRollId = 8;

            ApplicationDetailsModel model = new ApplicationDetailsModel();

            ViewBag.RollId = rollId != "" ? Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(rollId)) : 0;

            if (ViewBag.RollId == 3)
            {
                ViewBag.RollName = "CHC";
                ViewBag.RollNameHi = "सी.एच.सी.";
            }
            else if (ViewBag.RollId == 4)
            {
                ViewBag.RollName = "PHC";
                ViewBag.RollNameHi = "पी.एच.सी.";
            }
            else if (ViewBag.RollId == 5)
            {
                ViewBag.RollName = "CMS(DH)";
                ViewBag.RollNameHi = "सी.एम.एस(डी.एच)";
            }
            else
            {
                ViewBag.RollName = "CMO";
                ViewBag.RollNameHi = "सी.एम.ओ.";
            }

            int appCS = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(appCurrStatus));
            int zId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(zoneId));
            int dId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(districtId));
            int srvId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId));

            int isLFT = isLessFiftyThousan != "" ? Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(isLessFiftyThousan)) : 0;
            long uId = userId != "" ? Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(userId)) : 0;

            var result = ObjRptDb.GetApplicationDetails(appCS, zId, dId, srvId, isLFT, uId);

            var resultData = result.Where(m => (m.appCurrStatus == appCS || appCS == 0)).Select(m => new { m.serviceName, m.zoneName, m.DistrictName, m.appCurrStatus, m.serviceId }).FirstOrDefault();

            model.serviceId = resultData.serviceId;
            model.serviceName = resultData.serviceName;
            model.zoneName = resultData.zoneName;
            model.DistrictName = resultData.DistrictName;
            model.appCurrStatus = appCS;

            model.appDetailList = result;

            return View(model);
        }

        #region rpt CMS/CHC/PHC
        public ActionResult SRVCountReportDivisionWise(string rollId = "", string zoneId = "", string serviceId = "")
        {
            ViewBag.RollId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(rollId));
            ViewBag.ZoneId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(zoneId));
            if (!string.IsNullOrEmpty(serviceId))
            {
                ViewBag.ServiceId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId));
            }

            if (ViewBag.RollId == 3)
            {
                ViewBag.RollName = "CHC";
                ViewBag.RollNameHi = "सी.एच.सी.";
            }
            else if (ViewBag.RollId == 4)
            {
                ViewBag.RollName = "PHC";
                ViewBag.RollNameHi = "पी.एच.सी.";
            }
            else if (ViewBag.RollId == 5)
            {
                ViewBag.RollName = "CMS(DH)";
                ViewBag.RollNameHi = "सी.एम.एस(डी.एच)";
            }

            return View();
        }
        public ActionResult SRVCountReportDivisionWiseList(long rollId = 0, int zoneId = 0, int serviceId = 0)
        {
            ViewBag.ServiceId = serviceId;
            var result = ObjRptDb.SRVCountReport_DivisionWise(rollId, zoneId, 0, serviceId);
            return PartialView("_SRVCountReportDivisionWiseList", result);
        }
        public ActionResult SRVCountReportDistrictWise(string rollId = "", string districtId = "", string serviceId = "")
        {
            ViewBag.RollId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(rollId));
            ViewBag.DistrictId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(districtId));
            ViewBag.ServiceId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId));

            if (ViewBag.RollId == 3)
            {
                ViewBag.RollName = "CHC";
                ViewBag.RollNameHi = "सी.एच.सी.";
            }
            else if (ViewBag.RollId == 4)
            {
                ViewBag.RollName = "PHC";
                ViewBag.RollNameHi = "पी.एच.सी.";
            }
            else if (ViewBag.RollId == 5)
            {
                ViewBag.RollName = "CMS(DH)";
                ViewBag.RollNameHi = "सी.एम.एस(डी.एच)";
            }

            return View();
        }
        public ActionResult SRVCountReportDistrictWiseList(long rollId = 0, int districtId = 0, int serviceId = 0)
        {
            ViewBag.ServiceId = serviceId;
            var result = ObjRptDb.SRVCountReport_DistrictWise(rollId, districtId, serviceId);
            return PartialView("_SRVCountReportDistrictWiseList", result);
        }
        #endregion

        #endregion

        public ActionResult ServiceCountReport()
        {
            var result = ObjRptDb.ServiceCount();
            return View(result);
        }

        public ActionResult getServiceWiseCount(string serviceId)
        {
            int srvId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId));
            var result = ObjRptDb.ServiceWiseCount(srvId);
            return View(result);
        }

        public ActionResult getDistrictWiseServiceCount(string serviceId, string zoneId)
        {
            int srvId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId));
            int enZoneId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(zoneId));
            var result = ObjRptDb.DistrictServiceWiseCount(srvId, enZoneId);
            return View(result);
        }

        public ActionResult getDivisionServiceRollWiseCount(string rollId, string zoneId, string serviceId)
        {
            long enRollId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(rollId));
            int enZoneId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(zoneId));
            int srvId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId));

            var result = ObjRptDb.DistrictServiceRollWiseCount(enRollId, enZoneId, srvId);
            return View(result);
        }

        public ActionResult getDistrictServiceRollApplicantDtl(string appCurrStatus, string rollId, string zoneId, string serviceId, string districtId)
        {
            ApplicationDetailsModel model = new ApplicationDetailsModel();

            ViewBag.RollId = rollId != "" ? Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(rollId)) : 0;

            if (ViewBag.RollId == 3)
            {
                ViewBag.RollName = "CHC";
                ViewBag.RollNameHi = "सी.एच.सी.";
            }
            else if (ViewBag.RollId == 4)
            {
                ViewBag.RollName = "PHC";
                ViewBag.RollNameHi = "पी.एच.सी.";
            }
            else if (ViewBag.RollId == 5)
            {
                ViewBag.RollName = "CMS(DH)";
                ViewBag.RollNameHi = "सी.एम.एस(डी.एच)";
            }
            else
            {
                ViewBag.RollName = "CMO";
                ViewBag.RollNameHi = "सी.एम.ओ.";
            }

            int enAppCurrStatus = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(appCurrStatus));
            int enRollId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(rollId));
            int enZoneId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(zoneId));
            int srvId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId));
            int enDistId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(districtId));



            var result = ObjRptDb.DistrictServiceRollApplicantDtl(enAppCurrStatus, enRollId, enZoneId, srvId, enDistId);
            var resultData = result.Where(m => (m.appCurrStatus == enAppCurrStatus || enAppCurrStatus == 0)).Select(m => new { m.serviceName, m.zoneName, m.DistrictName, m.appCurrStatus, m.serviceId }).FirstOrDefault();

            model.serviceId = resultData.serviceId;
            model.serviceName = resultData.serviceName;
            model.zoneName = resultData.zoneName;
            model.DistrictName = resultData.DistrictName;
            model.appCurrStatus = enAppCurrStatus;

            model.appDetailList = result;

            return View(model);
            //return View(result);

        }

        public ActionResult goBack()
        {
            string srvId = Request.QueryString["serviceId"];
            return RedirectToAction("getServiceWiseCount", new { serviceId = srvId });

        }


        // Created by Vinod
        public ActionResult ChangeStatusManually()
        {

            int? appStatus = 0;
            long regisByUser = 103987;
            long regisIdNUH = 564564;
            UPHEALTHNIC.upswp_niveshmitraservices ObjSendAppSubmitStatus = new UPHEALTHNIC.upswp_niveshmitraservices();
            //NiveshMitraSendStatusModel objStatusModel = objCMODB.GetNiveshMitraUserDetailsByID(regisByUser).FirstOrDefault();
            NiveshMitraSendStatusModel objStatusModel = objCMODB.GetNiveshMitraUserDetailsByID(regisIdNUH).FirstOrDefault();
            string StatusResult = string.Empty;



            if (objStatusModel != null)
            {

                if (appStatus == 0)
                {

                    objStatusModel.ProcessIndustryID = objStatusModel.UserName;
                    objStatusModel.ApplicationID = regisIdNUH.ToString(); //objStatusModel.UserID.ToString();

                    objStatusModel.StatusCode = "07";
                    objStatusModel.Remarks = objStatusModel.ReasonName + "  " + "|" + " " + objStatusModel.Remarks + "  " + "|" + "  " + "Application Rejected by department";
                    //objStatusModel.PendencyLevel = "Entrepreneur level pendency";
                    objStatusModel.PendencyLevel = "";

                    objStatusModel.FeeAmount = "";
                    objStatusModel.FeeStatus = "";
                    objStatusModel.TransectionID = "";
                    objStatusModel.TranSactionDate = "";
                    objStatusModel.TransectionDateAndTime = "";
                    objStatusModel.NocCertificateNumber = "";
                    objStatusModel.NocUrl = "";
                    objStatusModel.IsNocUrlActiveYesNo = "";
                    objStatusModel.Passalt = ConfigurationManager.AppSettings["PassKey"].ToString();
                    //objStatusModel.ObjectRejectionCode = "01";
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
                //else if (appStatus == 2)
                //{


                //    objStatusModel.ProcessIndustryID = objStatusModel.UserName;
                //    objStatusModel.ApplicationID = objStatusModel.UserID.ToString();

                //    objStatusModel.StatusCode = "05";
                //    objStatusModel.Remarks = "Inspection Scheduled";
                //    // objStatusModel.PendencyLevel = "Pending at Department";
                //    objStatusModel.PendencyLevel = "Pending at" + " " + objSM.DisignationName + "," + "  " + objSM.DistrictName;

                //    objStatusModel.FeeAmount = "";
                //    objStatusModel.FeeStatus = "";
                //    objStatusModel.TransectionID = "";
                //    objStatusModel.TranSactionDate = "";
                //    objStatusModel.TransectionDateAndTime = "";
                //    objStatusModel.NocCertificateNumber = "";
                //    objStatusModel.NocUrl = "";
                //    objStatusModel.IsNocUrlActiveYesNo = "";
                //    objStatusModel.Passalt = ConfigurationManager.AppSettings["PassKey"].ToString();
                //    objStatusModel.ObjectRejectionCode = "";
                //    objStatusModel.IsCertificateValidLifeTime = "";
                //    objStatusModel.CertificateExpireDateDDMMYYYY = "";
                //    objStatusModel.D1 = "";
                //    objStatusModel.D2 = "";
                //    objStatusModel.D3 = "";
                //    objStatusModel.D4 = "";
                //    objStatusModel.D5 = "";
                //    objStatusModel.D6 = "";
                //    objStatusModel.D7 = "";

                //    StatusResult = ObjSendAppSubmitStatus.WReturn_CUSID_STATUS(objStatusModel.Control_ID, objStatusModel.Unit_Id, objStatusModel.ServiceID, objStatusModel.ProcessIndustryID, objStatusModel.ApplicationID, objStatusModel.StatusCode,
                //           objStatusModel.Remarks, objStatusModel.PendencyLevel, objStatusModel.FeeAmount, objStatusModel.FeeStatus, objStatusModel.TransectionID, objStatusModel.TranSactionDate, objStatusModel.TransectionDateAndTime, objStatusModel.NocCertificateNumber, objStatusModel.NocUrl, objStatusModel.IsNocUrlActiveYesNo, objStatusModel.Passalt, objStatusModel.RequestId, objStatusModel.ObjectRejectionCode
                //            , objStatusModel.IsCertificateValidLifeTime, objStatusModel.CertificateExpireDateDDMMYYYY, objStatusModel.D1, objStatusModel.D2, objStatusModel.D3, objStatusModel.D4, objStatusModel.D5, objStatusModel.D6, objStatusModel.D7);

                //    if (StatusResult.ToUpper() == "SUCCESS")
                //    {
                //        objStatusModel.SendDate = System.DateTime.Now;
                //        objStatusModel.ResStatus = "";
                //        objStatusModel.ServiceStatus = StatusResult;
                //        objStatusModel.StepId = 6;

                //    }
                //    else
                //    {

                //        objStatusModel.SendDate = System.DateTime.Now;
                //        objStatusModel.ResStatus = "";
                //        objStatusModel.ServiceStatus = StatusResult;
                //        objStatusModel.StepId = 6;

                //    }

                //    try
                //    {
                //        objStatusModel = objCMODB.SaveCmoActionAndNiveshStatus(objStatusModel).FirstOrDefault();

                //    }
                //    catch (Exception ex)
                //    {

                //    }


                //}
                //else if (appStatus == 3)
                //{

                //    objStatusModel.ProcessIndustryID = objStatusModel.UserName;
                //    objStatusModel.ApplicationID = objStatusModel.UserID.ToString();

                //    objStatusModel.StatusCode = "05";
                //    objStatusModel.Remarks = "Inspection Report Uploaded";
                //    objStatusModel.PendencyLevel = "Pending at " + " " + objSM.DisignationName + "," + "  " + objSM.DistrictName;

                //    objStatusModel.FeeAmount = "";
                //    objStatusModel.FeeStatus = "";
                //    objStatusModel.TransectionID = "";
                //    objStatusModel.TranSactionDate = "";
                //    objStatusModel.TransectionDateAndTime = "";
                //    objStatusModel.NocCertificateNumber = "";
                //    objStatusModel.NocUrl = "";
                //    objStatusModel.IsNocUrlActiveYesNo = "";
                //    objStatusModel.Passalt = ConfigurationManager.AppSettings["PassKey"].ToString();
                //    objStatusModel.ObjectRejectionCode = "";
                //    objStatusModel.IsCertificateValidLifeTime = "";
                //    objStatusModel.CertificateExpireDateDDMMYYYY = "";
                //    objStatusModel.D1 = "";
                //    objStatusModel.D2 = "";
                //    objStatusModel.D3 = "";
                //    objStatusModel.D4 = "";
                //    objStatusModel.D5 = "";
                //    objStatusModel.D6 = "";
                //    objStatusModel.D7 = "";

                //    StatusResult = ObjSendAppSubmitStatus.WReturn_CUSID_STATUS(objStatusModel.Control_ID, objStatusModel.Unit_Id, objStatusModel.ServiceID, objStatusModel.ProcessIndustryID, objStatusModel.ApplicationID, objStatusModel.StatusCode,
                //           objStatusModel.Remarks, objStatusModel.PendencyLevel, objStatusModel.FeeAmount, objStatusModel.FeeStatus, objStatusModel.TransectionID, objStatusModel.TranSactionDate, objStatusModel.TransectionDateAndTime, objStatusModel.NocCertificateNumber, objStatusModel.NocUrl, objStatusModel.IsNocUrlActiveYesNo, objStatusModel.Passalt, objStatusModel.RequestId, objStatusModel.ObjectRejectionCode
                //            , objStatusModel.IsCertificateValidLifeTime, objStatusModel.CertificateExpireDateDDMMYYYY, objStatusModel.D1, objStatusModel.D2, objStatusModel.D3, objStatusModel.D4, objStatusModel.D5, objStatusModel.D6, objStatusModel.D7);

                //    if (StatusResult.ToUpper() == "SUCCESS")
                //    {
                //        objStatusModel.SendDate = System.DateTime.Now;
                //        objStatusModel.ResStatus = "";
                //        objStatusModel.ServiceStatus = StatusResult;
                //        objStatusModel.StepId = 7;

                //    }
                //    else
                //    {

                //        objStatusModel.SendDate = System.DateTime.Now;
                //        objStatusModel.ResStatus = "";
                //        objStatusModel.ServiceStatus = StatusResult;
                //        objStatusModel.StepId = 7;

                //    }

                //    try
                //    {
                //        objStatusModel = objCMODB.SaveCmoActionAndNiveshStatus(objStatusModel).FirstOrDefault();

                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //}
                //else if (appStatus == 4)
                //{


                //    objStatusModel.ProcessIndustryID = objStatusModel.UserName;
                //    objStatusModel.ApplicationID = objStatusModel.UserID.ToString();

                //    objStatusModel.StatusCode = "07";
                //    objStatusModel.Remarks = objStatusModel.ReasonName + "  " + "|" + " " + objStatusModel.Remarks + "  " + "|" + "  " + "Application Rejected by" + " " + objSM.DisignationName + "," + "  " + objSM.DistrictName + "  " + "on the bahalf of Inspection Report";
                //    // objStatusModel.PendencyLevel = "Entrepreneur level pendency";
                //    objStatusModel.PendencyLevel = "";

                //    objStatusModel.FeeAmount = "";
                //    objStatusModel.FeeStatus = "";
                //    objStatusModel.TransectionID = "";
                //    objStatusModel.TranSactionDate = "";
                //    objStatusModel.TransectionDateAndTime = "";
                //    objStatusModel.NocCertificateNumber = "";
                //    objStatusModel.NocUrl = "";
                //    objStatusModel.IsNocUrlActiveYesNo = "";
                //    objStatusModel.Passalt = ConfigurationManager.AppSettings["PassKey"].ToString();
                //    //objStatusModel.ObjectRejectionCode = "01";
                //    objStatusModel.IsCertificateValidLifeTime = "";
                //    objStatusModel.CertificateExpireDateDDMMYYYY = "";
                //    objStatusModel.D1 = "";
                //    objStatusModel.D2 = "";
                //    objStatusModel.D3 = "";
                //    objStatusModel.D4 = "";
                //    objStatusModel.D5 = "";
                //    objStatusModel.D6 = "";
                //    objStatusModel.D7 = "";

                //    StatusResult = ObjSendAppSubmitStatus.WReturn_CUSID_STATUS(objStatusModel.Control_ID, objStatusModel.Unit_Id, objStatusModel.ServiceID, objStatusModel.ProcessIndustryID, objStatusModel.ApplicationID, objStatusModel.StatusCode,
                //           objStatusModel.Remarks, objStatusModel.PendencyLevel, objStatusModel.FeeAmount, objStatusModel.FeeStatus, objStatusModel.TransectionID, objStatusModel.TranSactionDate, objStatusModel.TransectionDateAndTime, objStatusModel.NocCertificateNumber, objStatusModel.NocUrl, objStatusModel.IsNocUrlActiveYesNo, objStatusModel.Passalt, objStatusModel.RequestId, objStatusModel.ObjectRejectionCode
                //            , objStatusModel.IsCertificateValidLifeTime, objStatusModel.CertificateExpireDateDDMMYYYY, objStatusModel.D1, objStatusModel.D2, objStatusModel.D3, objStatusModel.D4, objStatusModel.D5, objStatusModel.D6, objStatusModel.D7);

                //    if (StatusResult.ToUpper() == "SUCCESS")
                //    {
                //        objStatusModel.SendDate = System.DateTime.Now;
                //        objStatusModel.ResStatus = "";
                //        objStatusModel.ServiceStatus = StatusResult;
                //        objStatusModel.StepId = 9;

                //    }
                //    else
                //    {

                //        objStatusModel.SendDate = System.DateTime.Now;
                //        objStatusModel.ResStatus = "";
                //        objStatusModel.ServiceStatus = StatusResult;
                //        objStatusModel.StepId = 9;

                //    }

                //    try
                //    {
                //        objStatusModel = objCMODB.SaveCmoActionAndNiveshStatus(objStatusModel).FirstOrDefault();

                //    }
                //    catch (Exception ex)
                //    {

                //    }

                //}
                //else if (appStatus == 5)
                //{

                //    objStatusModel.ProcessIndustryID = objStatusModel.UserName;
                //    objStatusModel.ApplicationID = objStatusModel.UserID.ToString();

                //    objStatusModel.StatusCode = "05";
                //    objStatusModel.Remarks = "Application approve on the behalf of Inspection Report";
                //    objStatusModel.PendencyLevel = "Pending at" + " " + objSM.DisignationName + "," + " " + objSM.DistrictName;

                //    objStatusModel.FeeAmount = "";
                //    objStatusModel.FeeStatus = "";
                //    objStatusModel.TransectionID = "";
                //    objStatusModel.TranSactionDate = "";
                //    objStatusModel.TransectionDateAndTime = "";
                //    objStatusModel.NocCertificateNumber = "";
                //    objStatusModel.NocUrl = "";
                //    objStatusModel.IsNocUrlActiveYesNo = "";
                //    objStatusModel.Passalt = ConfigurationManager.AppSettings["PassKey"].ToString();
                //    objStatusModel.ObjectRejectionCode = "";
                //    objStatusModel.IsCertificateValidLifeTime = "";
                //    objStatusModel.CertificateExpireDateDDMMYYYY = "";
                //    objStatusModel.D1 = "";
                //    objStatusModel.D2 = "";
                //    objStatusModel.D3 = "";
                //    objStatusModel.D4 = "";
                //    objStatusModel.D5 = "";
                //    objStatusModel.D6 = "";
                //    objStatusModel.D7 = "";

                //    StatusResult = ObjSendAppSubmitStatus.WReturn_CUSID_STATUS(objStatusModel.Control_ID, objStatusModel.Unit_Id, objStatusModel.ServiceID, objStatusModel.ProcessIndustryID, objStatusModel.ApplicationID, objStatusModel.StatusCode,
                //           objStatusModel.Remarks, objStatusModel.PendencyLevel, objStatusModel.FeeAmount, objStatusModel.FeeStatus, objStatusModel.TransectionID, objStatusModel.TranSactionDate, objStatusModel.TransectionDateAndTime, objStatusModel.NocCertificateNumber, objStatusModel.NocUrl, objStatusModel.IsNocUrlActiveYesNo, objStatusModel.Passalt, objStatusModel.RequestId, objStatusModel.ObjectRejectionCode
                //            , objStatusModel.IsCertificateValidLifeTime, objStatusModel.CertificateExpireDateDDMMYYYY, objStatusModel.D1, objStatusModel.D2, objStatusModel.D3, objStatusModel.D4, objStatusModel.D5, objStatusModel.D6, objStatusModel.D7);

                //    if (StatusResult.ToUpper() == "SUCCESS")
                //    {
                //        objStatusModel.SendDate = System.DateTime.Now;
                //        objStatusModel.ResStatus = "";
                //        objStatusModel.ServiceStatus = StatusResult;
                //        objStatusModel.StepId = 8;

                //    }
                //    else
                //    {

                //        objStatusModel.SendDate = System.DateTime.Now;
                //        objStatusModel.ResStatus = "";
                //        objStatusModel.ServiceStatus = StatusResult;
                //        objStatusModel.StepId = 8;

                //    }

                //    try
                //    {
                //        objStatusModel = objCMODB.SaveCmoActionAndNiveshStatus(objStatusModel).FirstOrDefault();

                //    }
                //    catch (Exception ex)
                //    {

                //    }

                //}

            }
            return null;
        }

        #region

        public ActionResult DarpanDashBoard()
        {
            return View();
        }

        public ActionResult CMOSRVCountReportDivisionWiseNew(string serviceId = "", string rollId = "")
        {
            if (!string.IsNullOrEmpty(serviceId))
            {
                ViewBag.ServiceId = serviceId;
            }
            ViewBag.DLLZone = objComDB.GetZoneForDLL().ToList();

            return View();
        }

        public ActionResult CMOSRVCountReportListDarpan(long rollId = 0, int zoneId = 0)
        {
            if (zoneId == 0)
            {
                List<CountReportModel> list = ObjRptDb.GetTotalServiceCount(0, rollId, rollId);
                ViewData["TotalCountList"] = list;
            }
            else
            {

                ViewData["TotalCountList"] = null;
            }

            var result = ObjRptDb.CMOSRVCountReport(zoneId, rollId, rollId);
            return PartialView("_CMOSRVCountReportList", result);
        }

        public ActionResult CMOSRVCountReportDistrictWiseList(string serviceId = "")
        {
            int srvId = serviceId != "" ? Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId)) : 0;
            ViewBag.ServiceId = srvId;
            var result = ObjRptDb.CMOSRVCountReport_DistrictWise(srvId);
            return PartialView("_CMOSRVCountReportDistrictWiseList", result);
        }


        public ActionResult ExportoExcelDistrictWiseList(string serviceId = "")
        {
            string rptName = null;
            int srvId = serviceId != "" ? Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId)) : 0;
            ViewBag.ServiceId = srvId;
            var result = ObjRptDb.CMOSRVCountReport_DistrictWise(srvId);

            if (srvId == 1)
            {
                rptName = "Medical Establishment";
            }
            else if (srvId == 2)
            {
                rptName = "Illness Certificate ";
            }
            else if (srvId == 3)
            {
                rptName = "Fitness Certificates";
            }
            else if (srvId == 4)
            {
                rptName = "Disability Certificate";
            }
            else if (srvId == 5)
            {
                rptName = "Immunization Certificate";
            }
            else if (srvId == 6)
            {
                rptName = "Death Certificate";
            }
            else if (srvId == 7)
            {
                rptName = "Unsuccessful Family Planning";
            }
            else if (srvId == 8)
            {
                rptName = "Medical Reimbursement";
            }
            else if (srvId == 9)
            {
                rptName = "Medico- Legal Certificate";
            }
            else if (srvId == 10)
            {
                rptName = "Age Certificate";
            }
            else if (srvId == 11)
            {
                rptName = "Immunization Certificate for Children";
            }

            string filename = null;
            if (result != null)
            {
                if (result.Count() > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Sr.No.", typeof(string));
                    dt.Columns.Add("District Name", typeof(string));
                    dt.Columns.Add("Total Received", typeof(string));
                    dt.Columns.Add("Approved", typeof(string));
                    dt.Columns.Add("Rejected", typeof(string));
                    dt.Columns.Add("Pending In-Time Limit", typeof(string));
                    dt.Columns.Add("Pending After-Time Limit", typeof(string));
                    int i = 0;
                    foreach (var res in result)
                    {
                        filename = "Medical&FamilyWelfareContRPT_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xls";
                        i++;
                        DataRow dr = dt.NewRow();
                        dr["Sr.No."] = i;
                        dr["District Name"] = res.DistrictName;
                        dr["Total Received"] = res.totalReceived;
                        dr["Approved"] = res.approved;
                        dr["Rejected"] = res.rejected;
                        dr["Pending In-Time Limit"] = res.pendingInTimeLimit;
                        dr["Pending After-Time Limit"] = res.pendingOverTimeLimit;
                        dt.Rows.Add(dr);
                    }
                    //DataRow dra = dt.NewRow();
                    //dra["Sr.No."] = "";
                    //dra["District Name"] = "";
                    //dra["Total Received"] = result.Sum(m => m.totalReceived);
                    //dra["Approved"] = result.Sum(m => m.approved);
                    //dra["Rejected"] = result.Sum(m => m.rejected);
                    //dra["Pending In-Time Limit"] = result.Sum(m => m.pendingInTimeLimit);
                    //dra["Pending After-Time Limit"] = result.Sum(m => m.pendingOverTimeLimit);
                    //dt.Rows.Add(dra);

                    var gv = new GridView();
                    gv.DataSource = dt;
                    gv.DataBind();


                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                    Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    StringWriter objStringWriter = new StringWriter();
                    string headerTable = @"<Table border=1><tr rowspan=2 align=center><td colspan=7><h3> Department of Medical Health And Family Welfare</h3></td></tr><tr rowspan=2 align=center><td colspan=7><h4>CMO Service District Wise Count Report</h4></tr><tr align=center></td><td colspan=7><h4>" + rptName + "</h4> </td></tr></Table>";

                    // string FooterTable = @"<Table border=1><tr><td align='center' colspan=2><b>OverAll Count</b></td><td align='center'><b>" + result.Sum(m => m.totalReceived) + "<b/></td><td align='center'><b>" + result.Sum(m => m.approved) + "<b/></td><td align='center'><b>" + result.Sum(m => m.rejected) + "</b></td><td align='center'><b/>" + result.Sum(m => m.pendingInTimeLimit) + "<b/></td><td align='center'><b>" + result.Sum(m => m.pendingOverTimeLimit) + "</b></td></tr></Table>";

                    Response.Write(headerTable);
                    HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                    gv.RenderControl(objHtmlTextWriter);
                    Response.Output.Write(objStringWriter.ToString());
                    Response.Flush();
                    Response.End();
                    return View();
                }
                else
                {
                    TempData["Msg"] = "No Records Found to Export";
                    TempData["MsgStatus"] = "warning";
                    return RedirectToAction("CMOSRVCountReportDistrictWiseList", new { serviceId = serviceId });
                }
            }
            else
            {
                TempData["Msg"] = "No Records Found to Export";
                TempData["MsgStatus"] = "warning";
                return RedirectToAction("CMOSRVCountReportDistrictWiseList", new { serviceId = serviceId });
            }
        }

        public ActionResult ExportToPDFDistrictWiseList(string serviceId = "")
        {
            ReportsModel reportModel = new ReportsModel();
            int srvId = serviceId != "" ? Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId)) : 0;
            ViewBag.ServiceId = srvId;
            var result = ObjRptDb.CMOSRVCountReport_DistrictWise(srvId);
            string rptName = null;
            string setPdfName = "";
            if (srvId == 1)
            {
                rptName = "Medical Establishment";
            }
            else if (srvId == 2)
            {
                rptName = "Illness Certificate ";
            }
            else if (srvId == 3)
            {
                rptName = "Fitness Certificates";
            }
            else if (srvId == 4)
            {
                rptName = "Disability Certificate";
            }
            else if (srvId == 5)
            {
                rptName = "Immunization Certificate";
            }
            else if (srvId == 6)
            {
                rptName = "Death Certificate";
            }
            else if (srvId == 7)
            {
                rptName = "Unsuccessful Family Planning";
            }
            else if (srvId == 8)
            {
                rptName = "Medical Reimbursement";
            }
            else if (srvId == 9)
            {
                rptName = "Medico- Legal Certificate";
            }
            else if (srvId == 10)
            {
                rptName = "Age Certificate";
            }
            else if (srvId == 11)
            {
                rptName = "Immunization Certificate for Children";
            }
            if (result != null && result.Count() > 0)
            {
                try
                {
                    ReportDocument rd = new ReportDocument();

                    rd.Load(Path.Combine(Server.MapPath("~/RPT"), "DarpanDistrictWiseList.rpt"));
                    rd.SetDataSource(result);
                    rd.SetParameterValue("rptName", rptName);

                    setPdfName = "DistrictWiseList_" + rptName;

                    string folderpath = "~/Content/writereaddata/DarpanRPT/";

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

        public ActionResult ApplicationDetailsNew(string rollId = "", string appCurrStatus = "", string zoneId = "", string districtId = "", string serviceId = "", string isLessFiftyThousan = "", string userId = "")
        {
            ViewBag.appCS = appCurrStatus;
            ViewBag.zId = zoneId;
            ViewBag.dId = districtId;
            ViewBag.srvId = serviceId;
            ViewBag.isLFT = isLessFiftyThousan;
            ViewBag.uId = userId;

            ApplicationDetailsModel model = new ApplicationDetailsModel();

            ViewBag.RollId = rollId != "" ? Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(rollId)) : 0;

            if (ViewBag.RollId == 3)
            {
                ViewBag.RollName = "CHC";
                ViewBag.RollNameHi = "सी.एच.सी.";
            }
            else if (ViewBag.RollId == 4)
            {
                ViewBag.RollName = "PHC";
                ViewBag.RollNameHi = "पी.एच.सी.";
            }
            else if (ViewBag.RollId == 5)
            {
                ViewBag.RollName = "CMS(DH)";
                ViewBag.RollNameHi = "सी.एम.एस(डी.एच)";
            }
            else
            {
                ViewBag.RollName = "CMO";
                ViewBag.RollNameHi = "सी.एम.ओ.";
            }

            int appCS = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(appCurrStatus));
            int zId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(zoneId));
            int dId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(districtId));
            int srvId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId));

            int isLFT = isLessFiftyThousan != "" ? Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(isLessFiftyThousan)) : 0;
            long uId = userId != "" ? Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(userId)) : 0;

            var result = ObjRptDb.GetApplicationDetailsForDarpan(appCS, zId, dId, srvId, isLFT, uId);

            var resultData = result.Where(m => (m.appCurrStatus == appCS || appCS == 0)).Select(m => new { m.serviceName, m.zoneName, m.DistrictName, m.appCurrStatus, m.serviceId }).FirstOrDefault();

            model.serviceId = resultData.serviceId;
            model.serviceName = resultData.serviceName;
            model.zoneName = resultData.zoneName;
            model.DistrictName = resultData.DistrictName;
            model.appCurrStatus = appCS;

            model.appDetailList = result;

            return View(model);
        }

        public ActionResult ExportoExcelApplicationDetails(string appCurrStatus = "", string zoneId = "", string districtId = "", string serviceId = "")
        {
            string rptName = null;
            int appCS = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(appCurrStatus));
            int zId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(zoneId));
            int dId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(districtId));
            int srvId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId));

            if (srvId == 1)
            {
                rptName = "Medical Establishment";
            }
            else if (srvId == 2)
            {
                rptName = "Illness Certificate ";
            }
            else if (srvId == 3)
            {
                rptName = "Fitness Certificates";
            }
            else if (srvId == 4)
            {
                rptName = "Disability Certificate";
            }
            else if (srvId == 5)
            {
                rptName = "Immunization Certificate";
            }
            else if (srvId == 6)
            {
                rptName = "Death Certificate";
            }
            else if (srvId == 7)
            {
                rptName = "Unsuccessful Family Planning";
            }
            else if (srvId == 8)
            {
                rptName = "Medical Reimbursement";
            }
            else if (srvId == 9)
            {
                rptName = "Medico- Legal Certificate";
            }
            else if (srvId == 10)
            {
                rptName = "Age Certificate";
            }
            else if (srvId == 11)
            {
                rptName = "Immunization Certificate for Children";
            }

            int isLFT = 0;
            long uId = 0;

            var result = ObjRptDb.GetApplicationDetailsForDarpan(appCS, zId, dId, srvId, isLFT, uId);
            string filename = null;
            string headerTable = null;
            if (result != null)
            {
                if (result.Count() > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Sr.No.", typeof(string));
                    dt.Columns.Add("Application No.", typeof(string));
                    dt.Columns.Add("Application Date", typeof(string));
                    dt.Columns.Add("Applicant Name", typeof(string));
                    dt.Columns.Add("Mobile No.", typeof(string));
                    if (result.FirstOrDefault().serviceId == 4 || result.FirstOrDefault().serviceId == 6 || result.FirstOrDefault().serviceId == 8)
                    {
                        dt.Columns.Add("Aadhar No.", typeof(string));
                    }
                    dt.Columns.Add("Status", typeof(string));
                    int i = 0;
                    foreach (var res in result)
                    {
                        filename = "Medical&FamilyWelfareApplicationDetails_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".xls";
                        i++;
                        DataRow dr = dt.NewRow();
                        dr["Sr.No."] = i;
                        dr["Application No."] = res.DistrictName;
                        dr["Application Date"] = res.applicationDate;
                        dr["Applicant Name"] = res.applicantName;
                        dr["Mobile No."] = res.MaskapplicantMobileNo;
                        if (result.FirstOrDefault().serviceId == 4 || result.FirstOrDefault().serviceId == 6 || result.FirstOrDefault().serviceId == 8)
                        {
                            dr["Aadhar No."] = res.MaskAadharNo;
                        }
                        dr["Status"] = res.appliedStatus;
                        dt.Rows.Add(dr);
                    }
                    //DataRow dra = dt.NewRow();
                    //dra["Sr.No."] = "";
                    //dra["District Name"] = "";
                    //dra["Total Received"] = result.Sum(m => m.totalReceived);
                    //dra["Approved"] = result.Sum(m => m.approved);
                    //dra["Rejected"] = result.Sum(m => m.rejected);
                    //dra["Pending In-Time Limit"] = result.Sum(m => m.pendingInTimeLimit);
                    //dra["Pending After-Time Limit"] = result.Sum(m => m.pendingOverTimeLimit);
                    //dt.Rows.Add(dra);

                    var gv = new GridView();
                    gv.DataSource = dt;
                    gv.DataBind();


                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                    Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    StringWriter objStringWriter = new StringWriter();
                    if (result.FirstOrDefault().serviceId == 4 || result.FirstOrDefault().serviceId == 6 || result.FirstOrDefault().serviceId == 8)
                    {
                        headerTable = @"<Table border=1><tr rowspan=2 align=center><td colspan=7><h3> Department of Medical Health And Family Welfare</h3></td></tr><tr rowspan=2 align=center><td colspan=7><h4>CMO Service Application Details Report</h4></td></tr><tr align=center></td><td colspan=7><h4>" + rptName + "</h4> </td></tr></Table>";
                    }
                    else
                    {
                        headerTable = @"<Table border=1><tr rowspan=2 align=center><td colspan=6><h3> Department of Medical Health And Family Welfare</h3></td></tr><tr rowspan=2 align=center><td colspan=6><h4>CMO Service Division District Wise Count Report</h4></td></tr><tr align=center></td><td colspan=6><h4>" + rptName + "</h4> </td></tr></Table>";
                    }
                    Response.Write(headerTable);
                    HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                    gv.RenderControl(objHtmlTextWriter);
                    Response.Output.Write(objStringWriter.ToString());
                    Response.Flush();
                    Response.End();
                    return View();
                }
                else
                {
                    TempData["Msg"] = "No Records Found to Export";
                    TempData["MsgStatus"] = "warning";
                    return RedirectToAction("CMOSRVCountReportDistrictWiseList", new { serviceId = serviceId });
                }
            }
            else
            {
                TempData["Msg"] = "No Records Found to Export";
                TempData["MsgStatus"] = "warning";
                return RedirectToAction("CMOSRVCountReportDistrictWiseList", new { serviceId = serviceId });
            }
        }

        public ActionResult ExportToPDFApplicationDetails(string appCurrStatus = "", string zoneId = "", string districtId = "", string serviceId = "")
        {
            ReportsModel reportModel = new ReportsModel();
            ApplicationDetailsModel model = new ApplicationDetailsModel();
            int appCS = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(appCurrStatus));
            int zId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(zoneId));
            int dId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(districtId));
            int srvId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId));

            int isLFT = 0;
            long uId = 0;

            var result = ObjRptDb.GetApplicationDetailsForDarpan(appCS, zId, dId, srvId, isLFT, uId);

            string rptName = null;
            string setPdfName = "";
            if (srvId == 1)
            {
                rptName = "Medical Establishment";
            }
            else if (srvId == 2)
            {
                rptName = "Illness Certificate ";
            }
            else if (srvId == 3)
            {
                rptName = "Fitness Certificates";
            }
            else if (srvId == 4)
            {
                rptName = "Disability Certificate";
            }
            else if (srvId == 5)
            {
                rptName = "Immunization Certificate";
            }
            else if (srvId == 6)
            {
                rptName = "Death Certificate";
            }
            else if (srvId == 7)
            {
                rptName = "Unsuccessful Family Planning";
            }
            else if (srvId == 8)
            {
                rptName = "Medical Reimbursement";
            }
            else if (srvId == 9)
            {
                rptName = "Medico- Legal Certificate";
            }
            else if (srvId == 10)
            {
                rptName = "Age Certificate";
            }
            else if (srvId == 11)
            {
                rptName = "Immunization Certificate for Children";
            }
            if (result != null && result.Count() > 0)
            {
                try
                {
                    ReportDocument rd = new ReportDocument();

                    if (result.FirstOrDefault().serviceId == 4 || result.FirstOrDefault().serviceId == 6 || result.FirstOrDefault().serviceId == 8)
                    {
                        rd.Load(Path.Combine(Server.MapPath("~/RPT"), "DarpanApplicationDetailsWithAAdhar.rpt"));
                        rd.SetDataSource(result);
                        rd.SetParameterValue("rptName", rptName);
                    }
                    else
                    {
                        rd.Load(Path.Combine(Server.MapPath("~/RPT"), "DarpanApplicationDetails.rpt"));
                        rd.SetDataSource(result);
                        rd.SetParameterValue("rptName", rptName);
                    }


                    setPdfName = "DarpanApplicationDetails_" + rptName;

                    string folderpath = "~/Content/writereaddata/DarpanRPT/";

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

        public ActionResult GetViewApplicationStatusForDarpan(string registrationID, string ServiceId)
        {
            List<string> liststr = new List<string>();
            List<string> liststrD = new List<string>();
            List<string> liststrS = new List<string>();
            ApplicationWorkFlowStepStatusModel model = new ApplicationWorkFlowStepStatusModel();
            registrationID = OTPL_Imp.CustomCryptography.Decrypt(registrationID);
            ServiceId = OTPL_Imp.CustomCryptography.Decrypt(ServiceId);
            if (registrationID != null)
            {
                try
                {
                    //if (Convert.ToInt32(ServiceId) == 7)
                    //{
                    //    model = ObjRptDb.GetAppWorkflowStatusFAP(Convert.ToInt64(registrationID)).FirstOrDefault();
                    //    liststrD = ObjRptDb.GetInspectionPhotoPath(2, Convert.ToInt64(registrationID), "D").ToList();
                    //    model.inspReportFilePhotoPath = liststrD.Select(i => i.ToString()).ToArray();

                    //    liststrS = ObjRptDb.GetInspectionPhotoPath(2, Convert.ToInt64(registrationID), "S").ToList();
                    //    model.inspReportFilePhotoPathCometti = liststrS.Select(i => i.ToString()).ToArray();
                    //}
                    //else
                    //{
                    model = ObjRptDb.GetAppWorkflowStatusMERForDarpan(Convert.ToInt64(registrationID), Convert.ToInt32(ServiceId)).FirstOrDefault();
                    //liststr = ObjRptDb.GetInspectionPhotoPathForDarpan(Convert.ToInt32(ServiceId), Convert.ToInt64(registrationID), "").ToList();
                    // model.inspReportFilePhotoPath = liststr.Select(i => i.ToString()).ToArray();
                    //}

                }
                catch (Exception ex)
                {
                    string message = string.Empty;
                    if (ex.InnerException.Message != null)
                    {
                        message = ex.InnerException.Message;
                    }
                }

            }
            if (Convert.ToInt32(ServiceId) == 1)
            {
                return PartialView("_ViewApplicationWorkFlowStatusNUH", model);
            }
            else if (Convert.ToInt32(ServiceId) == 2)
            {
                return PartialView("_ViewApplicationWorkFlowStatusILC", model);
            }
            else if (Convert.ToInt32(ServiceId) == 3)
            {
                return PartialView("_ViewApplicationWorkFlowStatusFIC", model);
            }
            else if (Convert.ToInt32(ServiceId) == 4)
            {
                return PartialView("_ViewApplicationWorkFlowStatusDIC", model);
            }
            else if (Convert.ToInt32(ServiceId) == 5)
            {
                return PartialView("_ViewApplicationWorkFlowStatusIMC", model);
            }
            else if (Convert.ToInt32(ServiceId) == 6)
            {
                return PartialView("_ViewApplicationWorkFlowStatusDEC", model);
            }
            else if (Convert.ToInt32(ServiceId) == 7)
            {
                return PartialView("_ViewApplicationWorkFlowStatusFAP", model);
            }
            else if (Convert.ToInt32(ServiceId) == 8)
            {
                return PartialView("_ViewApplicationWorkFlowStatusMER", model);
            }
            else if (Convert.ToInt32(ServiceId) == 9)
            {
                return PartialView("_ViewApplicationWorkFlowStatusMLC", model);
            }
            else if (Convert.ToInt32(ServiceId) == 10)
            {
                return PartialView("_ViewApplicationWorkFlowStatusAGC", model);
            }
            else
            {
                return PartialView("_ViewApplicationWorkFlowStatusICC", model);
            }
        }

        public ActionResult PrintApplicationFormNUH(string regisIdNUH)
        {
            regisIdNUH = OTPL_Imp.CustomCryptography.Decrypt(regisIdNUH);
            Session["regisIdNUH"] = regisIdNUH;
            NUH_DB objNUH_DB = new NUH_DB();
            NUHmodel model = new NUHmodel();
            model = objNUH_DB.GetNUHListBYRegistrationNo(Convert.ToInt64(regisIdNUH));

            ViewBag.VBoutpateint = objNUHDB.GetoutPatient(model.regisIdNUH);
            ViewBag.VBolaboratory = objNUHDB.GetNUHlaboratory(model.regisIdNUH);
            ViewBag.VBimaging = objNUHDB.GetNUHimaging(model.regisIdNUH);

            model.NUHPartnerList = objNUHDB.getNUHPartner(Convert.ToInt64(regisIdNUH));
            model.NUHDOCList = objNUHDB.getNUHdoc(Convert.ToInt64(regisIdNUH));
            model.NUHModelList = objNUHDB.getNUHChild(Convert.ToInt64(regisIdNUH));

            return View(model);
        }

        public ActionResult PrintApplicationFormAGC(string regisIdAGC = "")
        {
            long regisId = Convert.ToInt64(regisIdAGC);
            string Registration = "";
            AGCModel model = new AGCModel();
            model = objAGC_DB.GetAGCListBYRegistrationNo(regisId, Registration);
            return View(model);
        }

        public ActionResult PrintApplicationFormDIC(string registrationNo)
        {
            registrationNo = OTPL_Imp.CustomCryptography.Decrypt(registrationNo);
            DICModel model = new DICModel();
            DIC_DB objDB_DIC = new DIC_DB();
            model = objDB_DIC.GetDICListBYRegistrationNo(0, registrationNo);

            return View(model);
        }

        public ActionResult PrintApplicationFormFAP(string regisIdFAP)
        {
            regisIdFAP = OTPL_Imp.CustomCryptography.Decrypt(regisIdFAP);
            Session["regisIdFAP"] = regisIdFAP;

            FAPModel model = new FAPModel();
            model = objFAP_DB.GetFAPListBYRegistrationNo(Convert.ToInt64(regisIdFAP));
            return View(model);
        }

        public ActionResult PrintApplicationFormMER(string regisId)
        {
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            Session["regisIdMER"] = regisId;
            MERModel model = new MERModel();
            model = objMER_DB.getMERByRegistration(Convert.ToInt64(regisId));

            return View(model);
        }

        public ActionResult PrintApplicationFormILC(string regisId)
        {
            regisId = OTPL_Imp.CustomCryptography.Decrypt(regisId);
            ILCModel model = new ILCModel();
            model = objILC_DB.GetILCListBYRegistrationNo(Convert.ToInt64(regisId));
            return View(model);
        }

        public ActionResult PrintApplicationFormFIC(string registrationNo)
        {
            registrationNo = OTPL_Imp.CustomCryptography.Decrypt(registrationNo);
            FICModel model = new FICModel();
            model = objFICdb.GetFICListBYRegistrationNo(registrationNo);
            return View(model);
        }

        public ActionResult PrintApplicationFormDEC(string regisIdDEC)
        {
            DECModel model = new DECModel();
            model = objdb.GetDECListCHC(Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdDEC)));
            TempData["reg"] = model.registrationNo;
            Session["reg"] = model.registrationNo;
            return View(model);
        }

        public ActionResult PrintApplicationFormICC(string regisIdICC)
        {
            ICCModel model = new ICCModel();
            model = objICC.GetICCListBYRegistrationNo(0, "", Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdICC)));
            return View(model);
        }

        public ActionResult PrintApplicationFormIMC(string regisIdIMC)
        {
            regisIdIMC = OTPL_Imp.CustomCryptography.Decrypt(regisIdIMC);
            Session["regisIdIMC"] = regisIdIMC;
            IMC_DB objIMC_DB = new IMC_DB();
            IMCModel model = new IMCModel();
            model = objIMC_DB.IMCDetailsByRegistration(Convert.ToInt64(regisIdIMC));
            return View(model);
        }

        public ActionResult PrintApplicationFormMLC(string regisIdMLC)
        {
            MLCModel model = new MLCModel();

            model = objMlc.GetMLCListBYRegistrationNo(0, "", Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(regisIdMLC)));
            TempData["reg"] = model.registrationNo;
            Session["reg"] = model.registrationNo;
            return View(model);
        }

        #endregion

        #region Cirtificate Verification
        public ActionResult ClericalNUH()
        {
            return View();
        }

        public ActionResult ViewClericalNUH(string meeRegisNo)
        {
            var lstNUHApp = objCMODB.GetAllCertValidityByMeeRegisNo(meeRegisNo);
            return PartialView("_ViewClericalNUH", lstNUHApp);
        }

        public ActionResult DeclarationForm()
        {
            return View();
        }

        public ActionResult Affidavit()
        {
            return View();
        }

        #endregion



        #region Public Report On Darpan



        #region MiS Work Darapan Portal


        private void ValidateMIS()
        {
            try
            {
                MisProcess objProcess = new MisProcess();
                //Variables
                string cpatStr = ""; string sess_Token = ""; string sess_nonce = ""; string sess_timestamp = "";
                //Check Cpat Query String Value
                cpatStr = Request.QueryString["cpat"] == null ? "" : Request.QueryString["cpat"].ToString();

                //Check ClientToken Value
                sess_Token = Session["Clienttoken"] == null ? "" : Session["Clienttoken"].ToString();
                sess_nonce = Session["Clientnonce"] == null ? "" : Session["Clientnonce"].ToString();
                sess_timestamp = Session["Clienttimestamp"] == null ? "" : Session["Clienttimestamp"].ToString();

                RanSchedule("Start Code Tocken" + cpatStr);
                // if Cpat Query String is not empty use the cpat value
                if (cpatStr != "")
                {
                    BoMis ObjMisData = new BoMis();
                    ObjMisData = objProcess.ValidateMIS_Request(cpatStr);
                    if (ObjMisData != null)
                    {
                        // ' Authorized  Access!!
                        string Level1Code = ObjMisData.l1;   // ' StateCode
                        Session["Level1Code"] = Level1Code;
                        string Level2Code = ObjMisData.l2;   // ' DistrictCode
                        Session["Level2Code"] = Level2Code;
                        string ProjectCode = ObjMisData.projcode; // ' ProjectCode  
                        Session["ProjectCode"] = ProjectCode;
                        RanSchedule("Fisrt Time Run");

                    }
                    else
                    {

                        // Not Authorized 
                        Session.Abandon();
                        Response.Redirect("https://up.cmdashboard.nic.in/AuthFailed.html?v=1");
                        // RanSchedule("AuthFailed v=1");
                    }

                }
                // if Cpat is blank and session token is not null


                else if (cpatStr == "" && sess_Token != "" && sess_nonce != "" && sess_timestamp != "")
                {

                    RanSchedule("Without Cpat sess_Token :" + sess_Token);
                    RanSchedule("Without Cpat sess_nonce :" + sess_nonce);
                    RanSchedule("Without Cpat sess_timestamp :" + sess_timestamp);

                    if (objProcess.TokenValidation(sess_Token, sess_nonce, sess_timestamp) == "failure")
                    {
                        Session.Abandon();
                        Response.Redirect("https://up.cmdashboard.nic.in/AuthFailed.html?v=2");
                        //RanSchedule("AuthFailed v=2");

                    }

                    else
                    {

                        RanSchedule("Tocken Validate Success :" + Convert.ToString(Session["ProjectCode"]));

                    }
                }
                else
                {
                    Session.Abandon();
                    Response.Redirect("https://up.cmdashboard.nic.in/AuthFailed.html?v=3");
                    // RanSchedule("AuthFailed v=3");
                }

            }
            catch (Exception ex)
            {
                RanSchedule("V4 Exception Data: " + ex.ToString());
                // Log the ex in you database
                Session.Abandon();
                Response.Redirect("https://up.cmdashboard.nic.in/AuthFailed.html?v=4");
                // RanSchedule("AuthFailed v=4");
            }
        }


        public static void RanSchedule(string cpatStr)
        {

            string path = @"C:\up-health.in\online\DarpanMIS\log\misrequestlog.text";


            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine("\n\n-------Encripted   Response Started at {" + DateTime.Now.ToString() + "}-------");
                writer.WriteLine(cpatStr);
                writer.WriteLine("\n\n-------Encripted Ended at {" + DateTime.Now.ToString() + "}-------");
            }
        }


        string ProductServiceId = "0";
        public void AllServicesCheck()
        {
            if (Session["ProjectCode"] != null)
            {
                if (Convert.ToString(Session["ProjectCode"]) == "2380")
                {
                    ProductServiceId = "10";
                }
                else if (Convert.ToString(Session["ProjectCode"]) == "2375")
                {
                    ProductServiceId = "6";
                }
                else if (Convert.ToString(Session["ProjectCode"]) == "2373")
                {
                    ProductServiceId = "4";
                }
                else if (Convert.ToString(Session["ProjectCode"]) == "2372")
                {
                    ProductServiceId = "3";
                }
                else if (Convert.ToString(Session["ProjectCode"]) == "2371")
                {
                    ProductServiceId = "2";
                }
                else if (Convert.ToString(Session["ProjectCode"]) == "2382")
                {
                    ProductServiceId = "11";
                }
                else if (Convert.ToString(Session["ProjectCode"]) == "2374")
                {
                    ProductServiceId = "5";

                }
                else if (Convert.ToString(Session["ProjectCode"]) == "2378")
                {
                    ProductServiceId = "9";
                }
                else if (Convert.ToString(Session["ProjectCode"]) == "2376")
                {
                    ProductServiceId = "7";
                }
                else if (Convert.ToString(Session["ProjectCode"]) == "2377")
                {
                    ProductServiceId = "8";
                }
                else if (Convert.ToString(Session["ProjectCode"]) == "2159")
                {
                    ProductServiceId = "1";
                }
                else
                {

                    ProductServiceId = "0";
                }
            }
        }




        public ActionResult CMOSRVCountReportDistrictSeviceWise()
        {
            try
            {
                //BoMispayload payload=new BoMispayload();
                //Session["ProjectCode"] = "2380";

                ValidateMIS();

                AllServicesCheck();
                if (Session["ProjectCode"] != null)
                {

                    //RanSchedule("With Cpat ProjectCode :" + Convert.ToString(Session["ProjectCode"]));
                    //RanSchedule("With Cpat Level1Code :" + Convert.ToString(Session["Level1Code"]));
                    //RanSchedule("With Cpat Level2Code :" + Convert.ToString(Session["Level2Code"]));

                    //string Level1Code = Convert.ToString(Session["Level1Code"]);
                    //string Level2Code = Convert.ToString(Session["Level2Code"]);
                    //ViewBag.District = objComDB.GetDropDownList(7, 34).Where(m => m.Id == objSM.districtId).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                    ViewBag.DLLServices = objComDB.GetSevicesForDLL().Where(m => m.Value == ProductServiceId).Select(m => new SelectListItem { Text = m.Text, Value = m.Value.ToString() });
                    ViewBag.SeriviceId = ProductServiceId;
                    ViewBag.DLLDistrict = objComDB.GetDistrictForDLL().ToList();

                    ViewBag.RollName = "All";
                    ViewBag.RollNameHi = "समस्त";
                    ViewBag.FullRollName = "Admin";
                }
            }
            catch (Exception ex)
            {
                RanSchedule(ex.ToString());
                return View();
            }
            return View();

        }





        public ActionResult CMOSRVCountReportDistrictServiceList(long rollId = 0, int DistrictId = 0, int ServiceId = 0, string fromDate = "", string toDate = "", long UserId = 0)
        {
           
            if (fromDate == "" && toDate == "")
            {
                fromDate = "01/01/2018";
                toDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            var result = ObjRptDb.CMOSRVCountReportDistrictServicesWise(DistrictId, ServiceId, rollId, rollId, fromDate, toDate, UserId);
            Session["fromDate"] = fromDate;
            Session["toDate"] = toDate;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            Session["dataDistrictService"] = result;
            return PartialView("_CMOSRVDistrictServiceWiseCountReportList", result);
        }

        public ActionResult CMOSRVCountReportDistrictServiceListCMOCHCPHC(string rollId = "", string appCurrStatus = "", string districtId = "", string serviceId = "", string userId = "", string fromDate = "", string toDate = "")
        {
            //ValidateMIS();
            if (fromDate == "" && toDate == "")
            {
                fromDate = "01/01/2018";
                toDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                fromDate = Convert.ToString(OTPL_Imp.CustomCryptography.Decrypt(fromDate));
                toDate = Convert.ToString(OTPL_Imp.CustomCryptography.Decrypt(toDate));
            }
            int DistrictId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(districtId));
            int ServiceId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId));
            long UserId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(userId));
            long RollId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(userId));
            int AppCurrStatus = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(appCurrStatus));
            ViewBag.AppCurrentStatus = AppCurrStatus;
            var result = ObjRptDb.CMOSRVCountReportDistrictServicesWiseCMOCHCPHC(DistrictId, ServiceId, RollId, RollId, fromDate, toDate, AppCurrStatus, UserId);

            var resultres = result.Where(m => (m.appCurrStatus == AppCurrStatus || AppCurrStatus == 0)).Select(m => new { m.serviceName, m.zoneName, m.DistrictName, m.appCurrStatus, m.serviceId }).FirstOrDefault();
            if (resultres != null)
            {
                Session["ServiceName"] = resultres.serviceName;
                Session["DistrictName"] = resultres.DistrictName;
                ViewBag.DistricName = resultres.DistrictName;
                ViewBag.ServiceName = resultres.serviceName;
                Session["fromDate"] = fromDate;
                Session["toDate"] = toDate;
                ViewBag.fromDate = fromDate;
                ViewBag.toDate = toDate;
                Session["dataDistrictServiceCHCPHCCMO"] = result;
            }
            return View(result);
        }

        public ActionResult ApplicationDetailsDIstrictServices(string rollId = "", string appCurrStatus = "", string zoneId = "", string districtId = "", string serviceId = "", string isLessFiftyThousan = "", string userId = "", string fromDate = "", string toDate = "")
        {
         // ValidateMIS();

            ViewBag.CurrLogRollId = rollId;
            string fromDatef = Convert.ToString(OTPL_Imp.CustomCryptography.Decrypt(fromDate));
            string toDatef = Convert.ToString(OTPL_Imp.CustomCryptography.Decrypt(toDate));
            if (fromDatef == "" && toDatef == "")
            {
                fromDatef = "01/01/2018";
                toDatef = DateTime.Now.ToString("dd/MM/yyyy");

            }

            ApplicationDetailsModel model = new ApplicationDetailsModel();

            ViewBag.RollId = rollId != "" ? Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(rollId)) : 0;

            if (ViewBag.RollId == 3)
            {
                ViewBag.RollName = "CHC";
                ViewBag.RollNameHi = "सी.एच.सी.";
            }
            else if (ViewBag.RollId == 4)
            {
                ViewBag.RollName = "PHC";
                ViewBag.RollNameHi = "पी.एच.सी.";
            }
            else if (ViewBag.RollId == 5)
            {
                ViewBag.RollName = "CMS(DH)";
                ViewBag.RollNameHi = "सी.एम.एस(डी.एच)";
            }
            else
            {
                ViewBag.RollName = "All";
                ViewBag.RollNameHi = "समस्त";
            }

            int appCS = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(appCurrStatus));
            int zId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(zoneId));
            int dId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(districtId));
            int srvId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId));


            int isLFT = isLessFiftyThousan != "" ? Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(isLessFiftyThousan)) : 0;
            long uId = userId != "" ? Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(userId)) : 0;

            int roll = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(rollId));
            var result = ObjRptDb.GetApplicationDetailsDistrictServices(appCS, zId, dId, srvId, isLFT, uId, fromDatef, toDatef, roll);
            Session["dataDistrictServiceApplication"] = result;

            var resultData = result.Where(m => (m.appCurrStatus == appCS || appCS == 0)).Select(m => new { m.serviceName, m.zoneName, m.DistrictName, m.appCurrStatus, m.serviceId }).FirstOrDefault();
            Session["fromDate"] = fromDatef;
            Session["toDate"] = toDatef;
            if (resultData != null)
            {
                model.serviceId = resultData.serviceId;
                model.serviceName = resultData.serviceName;
                model.zoneName = resultData.zoneName;
                model.DistrictName = resultData.DistrictName;
                model.appCurrStatus = appCS;
                TempData["ServiceName"] = resultData.serviceName;
                TempData["DistrictName"] = resultData.DistrictName;
                model.appDetailList = result;
            }
            return View(model);
        }

        public ActionResult ExportToExcelCountCHCPHCCMO()
        {
            string filename = "";
            if (Session["dataDistrictServiceCHCPHCCMO"] != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Sr.No.", typeof(string));
                dt.Columns.Add("District Name", typeof(string));
                dt.Columns.Add("Hospital Name", typeof(string));

                dt.Columns.Add("Total Count", typeof(string));


                int i = 0;
                foreach (var res in (Session["dataDistrictServiceCHCPHCCMO"] as List<CountReportModel>))
                {
                    if (i == 0)
                    {
                        filename = "DistrictWiseServiceApplication.xls";
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    dr["Sr.No."] = i;

                    dr["District Name"] = res.DistrictName;

                    dr["Hospital Name"] = res.HospitalName;

                    dr["Total Count"] = res.totalcount;


                    dt.Rows.Add(dr);
                }

                var gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();


                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                string headerTable = @"<Table border=1><tr rowspan=2 align=center><td colspan=9><h3> Department of Medical Health And Family Welfare</h3></td></tr><tr rowspan=2 align=center><td colspan=9><h4>" + Session["ServiceName"].ToString() + "  Service " + Session["DistrictName"].ToString() + " District Application Count Detail</h4> </td></tr><tr rowspan=2 align=center><td ><h4>From Date</h4> </td><td  colspan=2 >" + Convert.ToString(Session["fromDate"]) + " </td><td ><h4>To Date</h4> </td><td   colspan=2>" + Convert.ToString(Session["toDate"]) + " </td></tr></Table>";
                Response.Write(headerTable);
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
                return View();
            }
            else
            {
                TempData["Msg"] = "No Records Found to Export";
                TempData["MsgStatus"] = "warning";
                return RedirectToAction("CMOSRVCountReportDistrictSeviceWise", new { rollId = "" });
            }
        }
        #endregion  MIS Darapan End




        #region Darpan Report Public
        public ActionResult CMOSRVCountReportDistrictSeviceWisePublic()
        {
            try
            {

                ViewBag.DLLServices = objComDB.GetSevicesForDLL().ToList();
                ViewBag.DLLDistrict = objComDB.GetDistrictForDLL().ToList();

                ViewBag.RollName = "All";
                ViewBag.RollNameHi = "समस्त";
                ViewBag.FullRollName = "Admin";

            }
            catch (Exception ex)
            {
               
                return View();
            }
            return View();

        }

        public ActionResult CMOSRVCountReportDistrictServiceListPublic(long rollId = 0, int DistrictId = 0, int ServiceId = 0, string fromDate = "", string toDate = "", long Userid = 0)
        {
            if (fromDate == "" && toDate == "")
            {
                fromDate = "01/01/2018";
                toDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            var result = ObjRptDb.CMOSRVCountReportDistrictServicesWise(DistrictId, ServiceId, rollId, rollId, fromDate, toDate, Userid);
            Session["fromDate"] = fromDate;
            Session["toDate"] = toDate;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            Session["dataDistrictService"] = result;
            return PartialView("_CMOSRVDistrictServiceWiseCountReportListPublic", result);
        }

        public ActionResult CMOSRVCountReportDistrictServiceListCMOCHCPHCPublic(string rollId = "", string appCurrStatus = "", string districtId = "", string serviceId = "", string userId = "", string fromDate = "", string toDate = "")
        {

            if (fromDate == "" && toDate == "")
            {
                fromDate = "01/01/2018";
                toDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                fromDate = Convert.ToString(OTPL_Imp.CustomCryptography.Decrypt(fromDate));
                toDate = Convert.ToString(OTPL_Imp.CustomCryptography.Decrypt(toDate));
            }
            int DistrictId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(districtId));
            int ServiceId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId));
            long RollId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(rollId));
            long UserId = 0;
            if (RollId == 3 || RollId == 4 || RollId == 5 || RollId == 6)
            {
                UserId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(userId));
            }
            else
            {
                UserId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(userId));
            }
            
            int AppCurrStatus = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(appCurrStatus));
            ViewBag.AppCurrentStatus = AppCurrStatus;
            var result = ObjRptDb.CMOSRVCountReportDistrictServicesWiseCMOCHCPHC(DistrictId, ServiceId, RollId, RollId, fromDate, toDate, AppCurrStatus, UserId);

            var resultres = result.Where(m => (m.appCurrStatus == AppCurrStatus || AppCurrStatus == 0)).Select(m => new { m.serviceName, m.zoneName, m.DistrictName, m.appCurrStatus, m.serviceId, m.rollId }).FirstOrDefault();
            if (resultres != null)
            {
                Session["ServiceName"] = resultres.serviceName;
                Session["DistrictName"] = resultres.DistrictName;
                ViewBag.DistricName = resultres.DistrictName;
                ViewBag.ServiceName = resultres.serviceName;
                Session["fromDate"] = fromDate;
                Session["toDate"] = toDate;
                ViewBag.fromDate = fromDate;
                ViewBag.toDate = toDate;
                Session["dataDistrictServiceCHCPHCCMO"] = result;
            }
            return View(result);
        }

        public ActionResult ApplicationDetailsDIstrictServicesPublic(string rollId = "", string appCurrStatus = "", string zoneId = "", string districtId = "", string serviceId = "", string isLessFiftyThousan = "", string userId = "", string fromDate = "", string toDate = "")
        {


            ViewBag.CurrLogRollId = rollId;
            string fromDatef = Convert.ToString(OTPL_Imp.CustomCryptography.Decrypt(fromDate));
            string toDatef = Convert.ToString(OTPL_Imp.CustomCryptography.Decrypt(toDate));
            if (fromDatef == "" && toDatef == "")
            {
                fromDatef = "01/01/2018";
                toDatef = DateTime.Now.ToString("dd/MM/yyyy");

            }

            ApplicationDetailsModel model = new ApplicationDetailsModel();

            ViewBag.RollId = rollId != "" ? Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(rollId)) : 0;

            if (ViewBag.RollId == 3)
            {
                ViewBag.RollName = "CHC";
                ViewBag.RollNameHi = "सी.एच.सी.";
            }
            else if (ViewBag.RollId == 4)
            {
                ViewBag.RollName = "PHC";
                ViewBag.RollNameHi = "पी.एच.सी.";
            }
            else if (ViewBag.RollId == 5)
            {
                ViewBag.RollName = "CMS(DH)";
                ViewBag.RollNameHi = "सी.एम.एस(डी.एच)";
            }
            else
            {
                ViewBag.RollName = "All";
                ViewBag.RollNameHi = "समस्त";
            }

            int appCS = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(appCurrStatus));
            int zId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(zoneId));
            int dId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(districtId));
            int srvId = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId));


            int isLFT = isLessFiftyThousan != "" ? Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(isLessFiftyThousan)) : 0;
            long uId = userId != "" ? Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(userId)) : 0;
            int roll = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(rollId));

            var result = ObjRptDb.GetApplicationDetailsDistrictServices(appCS, zId, dId, srvId, isLFT, uId, fromDatef, toDatef, roll);
            Session["dataDistrictServiceApplication"] = result;

            var resultData = result.Where(m => (m.appCurrStatus == appCS || appCS == 0)).Select(m => new { m.serviceName, m.zoneName, m.DistrictName, m.appCurrStatus, m.serviceId }).FirstOrDefault();
            Session["fromDate"] = fromDatef;
            Session["toDate"] = toDatef;
            if (resultData != null)
            {
                model.serviceId = resultData.serviceId;
                model.serviceName = resultData.serviceName;
                model.zoneName = resultData.zoneName;
                model.DistrictName = resultData.DistrictName;
                model.appCurrStatus = appCS;
                TempData["ServiceName"] = resultData.serviceName;
                TempData["DistrictName"] = resultData.DistrictName;
                model.appDetailList = result;
            }
            return View(model);
        }

        public ActionResult ExportToExcelDistrictService()
        {
            string filename = "";
            if (Session["dataDistrictService"] != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Sr.No.", typeof(string));
                dt.Columns.Add("District Name", typeof(string));
                dt.Columns.Add("Service Name", typeof(string));
                dt.Columns.Add("Total Recieved", typeof(string));
                dt.Columns.Add("Approved", typeof(string));
                dt.Columns.Add("Rejected", typeof(string));
                dt.Columns.Add("Pending In-Time Limit", typeof(string));
                dt.Columns.Add("Pending After-Time Limit", typeof(string));

                int i = 0;
                foreach (var res in (Session["dataDistrictService"] as List<CountReportModel>))
                {
                    if (i == 0)
                    {
                        filename = "DistrictWiseServiceCountReport.xls";
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    dr["Sr.No."] = i;

                    dr["District Name"] = res.DistrictName;
                    dr["Service Name"] = res.serviceName;
                    dr["Total Recieved"] = res.totalReceived;
                    dr["Approved"] = res.approved;
                    dr["Rejected"] = res.rejected;
                    dr["Pending In-Time Limit"] = res.pendingInTimeLimit;
                    dr["Pending After-Time Limit"] = res.pendingOverTimeLimit;

                    dt.Rows.Add(dr);
                }

                var gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();


                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                string headerTable = @"<Table border=1><tr rowspan=2 align=center><td colspan=9><h3> Department of Medical Health And Family Welfare</h3></td></tr><tr rowspan=2 align=center><td colspan=9><h4>All Service District Wise Count Report</h4> </td></tr>
<tr rowspan=2 align=center><td ><h4>From Date</h4> </td><td  colspan=2 >" + Convert.ToString(Session["fromDate"]) + " </td><td ><h4>To Date</h4> </td><td   colspan=2>" + Convert.ToString(Session["toDate"]) + " </td></tr></Table>";
                Response.Write(headerTable);
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
                return View();
            }
            else
            {
                TempData["Msg"] = "No Records Found to Export";
                TempData["MsgStatus"] = "warning";
                return RedirectToAction("CMOSRVCountReportDistrictSeviceWise", new { rollId = "" });
            }
        }

        public ActionResult ExportToExcelApplication()
        {
            string filename = "";
            if (Session["dataDistrictServiceApplication"] != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Sr.No.", typeof(string));
                dt.Columns.Add("Application No", typeof(string));
                dt.Columns.Add("Application Date", typeof(string));
                dt.Columns.Add("Applicant Name", typeof(string));
                dt.Columns.Add("Mobile No", typeof(string));
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("Facility Name", typeof(string));
                dt.Columns.Add("Application Status", typeof(string));
                dt.Columns.Add("Remark", typeof(string));
             
                int i = 0;
                foreach (var res in (Session["dataDistrictServiceApplication"] as List<ApplicationDetailsModel>))
                {
                    if (i == 0)
                    {
                        filename = "DistrictWiseServiceApplication.xls";
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    dr["Sr.No."] = i;

                    dr["Application No"] = res.registrationNo;
                    dr["Application Date"] = res.applicationDate;
                    dr["Applicant Name"] = res.applicantName;
                    dr["Mobile No"] = res.applicantMobileNo;
                    dr["Type"] = res.HospitalName;
                    dr["Facility Name"] = res.opdAdd;
                    dr["Application Status"] = res.applicationStatus;
                    if (res.rejectedRemark != "" || res.rejectedRemark != null)
                    {
                        dr["Remark"] = res.rejectedRemark;
                    }
                    else
                    {
                        dr["Remark"] = "NA";
                    }
                    dt.Rows.Add(dr);
                }

                var gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();


                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                string headerTable = @"<Table border=1><tr rowspan=2 align=center><td colspan=9><h3> Department of Medical Health And Family Welfare</h3></td></tr><tr rowspan=2 align=center><td colspan=9><h4>" + TempData["ServiceName"].ToString() + "  Service " + TempData["DistrictName"].ToString() + " District Application Detail</h4> </td></tr><tr rowspan=2 align=center><td ><h4>From Date</h4> </td><td  colspan=2 >" + Convert.ToString(Session["fromDate"]) + " </td><td ><h4>To Date</h4> </td><td   colspan=2>" + Convert.ToString(Session["toDate"]) + " </td></tr></Table>";
                Response.Write(headerTable);
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
                return View();
            }
            else
            {
                TempData["Msg"] = "No Records Found to Export";
                TempData["MsgStatus"] = "warning";
                return RedirectToAction("CMOSRVCountReportDistrictSeviceWise", new { rollId = "" });
            }
        }


      
        #endregion

        #endregion



     
    }
}
