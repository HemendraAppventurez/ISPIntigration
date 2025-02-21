using CCSHealthFamilyWelfareDept.DAL;
using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CCSHealthFamilyWelfareDept.Filters;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    public class ReportController : Controller
    {
        Report_DB ObjRptDb = new Report_DB();
        Common_DB objComDB = new Common_DB();
        SessionManager SM = new SessionManager();

        NUH_DB objNUHDB = new NUH_DB();
        CMO_DB objCMODB = new CMO_DB();
        MER_DB objMER_DB = new MER_DB();
        DEC_DB objdb = new DEC_DB();
        FIC_DB objFICdb = new FIC_DB();
        ILC_DB objILC_DB = new ILC_DB();
        ICC_DB objICC = new ICC_DB();
        MLC_DB objMlc = new MLC_DB();
        AGC_DB objAGC_DB = new AGC_DB();
        FAP_DB objFAP_DB = new FAP_DB();

        [HttpGet]
        [AuthorizeAdmin(12)]
        public ActionResult ApplicationReport()
        {
            return View();
        }

        #region CMO Process Count Report

        [HttpGet]
        [AuthorizeAdmin(13)]
        public ActionResult ProcessCountReport()
        {
            if (SM.RollID == 2)
            {
                ViewBag.Application = objComDB.GetDropDownList(47, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            }

            ViewBag.District = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ReportsModel model = new ReportsModel();
            TempData["RollID"] = SM.RollID;
            return View(model);//(result);
        }

        [HttpPost]
        [AuthorizeAdmin(13)]
        public ActionResult ProcessCountReport(ReportsModel model)
        {
            string dist = "";
            if (SM.RollID == 2)
            {
                dist = Convert.ToString(SM.districtId) + ",";
            }
            else
            {
                foreach (var v in model.DistrictIds)
                {
                    dist = dist + v + ",";
                }
            }

            TempData["DistrictIds"] = dist;

            switch (model.appTypeId)
            {
                case 1: return RedirectToAction("ProcessCountReportNUH", new { model.fromDate, model.toDate });
                    break;
                case 2: return RedirectToAction("ProcessCountReportFAP", new { model.fromDate, model.toDate });
                    break;
                case 3: return RedirectToAction("ProcessCountReportDIC", new { model.fromDate, model.toDate });
                    break;
                case 4: return RedirectToAction("ProcessCountReportAGC", new { model.fromDate, model.toDate });
                    break;
                case 5: return RedirectToAction("ProcessCountReportMER", new { model.fromDate, model.toDate });
                    break;
                case 12: return RedirectToAction("ProcessCountReportALL", new { model.fromDate, model.toDate });
                    break;
                default: return RedirectToAction("UnauthoriseAcess", "Home");
                    break;
            }

            ViewBag.Application = objComDB.GetDropDownList(47, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            return View();
        }

        public ActionResult ProcessCountReportNUH(string fromdate, string todate)
        {
            if (TempData["DistrictIds"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            string dist = TempData["DistrictIds"].ToString();

            var result = ObjRptDb.ProcessCountReportCMO(1, dist, fromdate, todate);

            if (SM.RollID == 2)
            {
                TempData["RollID"] = "CMO";
            }
            else if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                ViewBag.Division = ", Division - " + SM.DisplayName;
            }
            else if (SM.RollID == 8)
            {
                TempData["RollID"] = "admin";
            }

            return View(result);
        }

        public ActionResult ProcessCountReportFAP(string fromdate, string todate)
        {
            if (TempData["DistrictIds"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            string dist = TempData["DistrictIds"].ToString();

            var result = ObjRptDb.ProcessCountReportCMO(2, dist, fromdate, todate);

            if (SM.RollID == 2)
            {
                TempData["RollID"] = "CMO";
            }
            else if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                ViewBag.Division = ", Division - " + SM.DisplayName;
            }
            else if (SM.RollID == 8)
            {
                TempData["RollID"] = "admin";
            }
            return View(result);
        }

        public ActionResult ProcessCountReportDIC(string fromdate, string todate)
        {
            if (TempData["DistrictIds"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            string dist = TempData["DistrictIds"].ToString();

            var result = ObjRptDb.ProcessCountReportCMO(3, dist, fromdate, todate);

            if (SM.RollID == 2)
            {
                TempData["RollID"] = "CMO";
            }
            else if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                ViewBag.Division = ", Division - " + SM.DisplayName;
            }
            else if (SM.RollID == 8)
            {
                TempData["RollID"] = "admin";
            }
            return View(result);
        }

        public ActionResult ProcessCountReportAGC(string fromdate, string todate)
        {
            if (TempData["DistrictIds"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            string dist = TempData["DistrictIds"].ToString();

            var result = ObjRptDb.ProcessCountReportCMO(4, dist, fromdate, todate);

            if (SM.RollID == 2)
            {
                TempData["RollID"] = "CMO";
            }
            else if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                ViewBag.Division = ", Division - " + SM.DisplayName;
            }
            else if (SM.RollID == 8)
            {
                TempData["RollID"] = "admin";
            }
            return View(result);
        }

        public ActionResult ProcessCountReportMER(string fromdate, string todate)
        {
            if (TempData["DistrictIds"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            string dist = TempData["DistrictIds"].ToString();

            var result = ObjRptDb.ProcessCountReportCMO(5, dist, fromdate, todate);

            if (SM.RollID == 2)
            {
                TempData["RollID"] = "CMO";
            }
            else if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                ViewBag.Division = ", Division - " + SM.DisplayName;
            }
            else if (SM.RollID == 8)
            {
                TempData["RollID"] = "admin";
            }
            return View(result);
        }

        public ActionResult ProcessCountReportALL(string fromdate, string todate)
        {
            if (TempData["DistrictIds"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                ViewBag.Division = ", Division - " + SM.DisplayName;
            }
            else if (SM.RollID == 2)
            {
                TempData["RollID"] = "CMO";
                ViewBag.Division = SM.DisplayName;
            }

            string dist = TempData["DistrictIds"].ToString();

            var result = ObjRptDb.ProcessCountReportCMO(0, dist, fromdate, todate);
            return View(result);
        }

        #endregion

        #region Services Under Janhit Guarantee Adhiniyam Reprot for Admin

        [AuthorizeAdmin(14)]
        public ActionResult ProcessCountReportCMoffice(string fromdate = "", string todate = "")
        {
            if (TempData["DistrictIds"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            ViewBag.RptType = TempData["RptType"];

            string dist = TempData["DistrictIds"].ToString();

            var result = ObjRptDb.ProcessCountReportCMO(12, dist, fromdate, todate);

            if (SM.RollID == 2)
            {
                TempData["RollID"] = "CMO";
            }
            else if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                TempData["time"] = DateTime.Now.ToString("hh:mm:ss tt");
            }
            else if (SM.RollID == 8)
            {
                TempData["RollID"] = "admin";
                TempData["time"] = DateTime.Now.ToString("hh:mm:ss tt");
            }
            return View(result);
        }

        #endregion

        #region Akanksha CM Office Report

        [HttpGet]
        [AuthorizeAdmin(15)]
        public ActionResult CMOfficeCountReport()
        {
            ReportsModel model = new ReportsModel();
            model.fromDate = SM.FromDate;
            model.toDate = SM.ToDate;

            ViewBag.Application = objComDB.GetDropDownList(48, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            return View(model);
        }

        [HttpPost]
        public ActionResult CMOfficeCountReport(ReportsModel model)
        {

            if (model != null)
            {
                List<ReportsModel> list = ObjRptDb.ProcessCountReportCMODrillDown(12, "", model.fromDate, model.toDate);
                if (list != null)
                {
                    SM.FromDate = model.fromDate;
                    SM.ToDate = model.toDate;
                    ViewData["CMOfficeCountReportList"] = list;
                    Session["listdata"] = list;
                }
                else
                {

                    ViewData["CMOfficeCountReportList"] = null;
                }
            }
            return View();
        }

        public ActionResult CMOfficeCountProcessDetailsList(string ModuleId = "", string FromDate = "", string ToDate = "", string ReportName = "")
        {
            ReportsModel reportModel = new ReportsModel();
            ModuleId = OTPL_Imp.CustomCryptography.Decrypt(ModuleId);
            ReportName = OTPL_Imp.CustomCryptography.Decrypt(ReportName);
            SM.ReportName = ReportName;
            SM.AppTypeID = Convert.ToInt32(ModuleId);
            reportModel.appTypeId = SM.AppTypeID;
            reportModel.fromDate = OTPL_Imp.CustomCryptography.Decrypt(FromDate);
            reportModel.toDate = OTPL_Imp.CustomCryptography.Decrypt(ToDate);
            ViewBag.CurrLogRollId = SM.RollID;
            string dist = "";

            var districtLst = objComDB.GetDropDownList(7, 34);

            if (districtLst != null && districtLst.Count > 0)
            {
                foreach (var v in districtLst)
                {
                    dist = dist + v.Id + ",";
                }
            }


            List<ReportsModel> list = ObjRptDb.DistrictWise_AllServiceCountReport(ReportName, SM.AppTypeID, dist, reportModel.fromDate, reportModel.toDate);

            //Session["DisticRpt"] = list;

            if (reportModel.appTypeId == 1)
            {
                ViewBag.ServiceName = "Medical Establishment";
            }
            else if (reportModel.appTypeId == 2)
            {
                ViewBag.ServiceName = "Unsuccessful Family Planning";
            }
            else if (reportModel.appTypeId == 3)
            {
                ViewBag.ServiceName = "Disability Certificate";
            }
            else if (reportModel.appTypeId == 4)
            {
                ViewBag.ServiceName = "Age Certificate";
            }
            else if (reportModel.appTypeId == 5)
            {
                ViewBag.ServiceName = "Medical Reimbursement";
            }
            else if (reportModel.appTypeId == 6)
            {
                ViewBag.ServiceName = "Fitness Certificate";
            }
            else if (reportModel.appTypeId == 7)
            {
                ViewBag.ServiceName = "Illness Certificate";
            }
            else if (reportModel.appTypeId == 8)
            {
                ViewBag.ServiceName = "Death Certificate";
            }
            else if (reportModel.appTypeId == 9)
            {
                ViewBag.ServiceName = "Immunization Children Certificate";
            }
            else if (reportModel.appTypeId == 10)
            {
                ViewBag.ServiceName = "Immunization Certificate";
            }
            else if (reportModel.appTypeId == 11)
            {
                ViewBag.ServiceName = "Medico Legal Certificate";
            }
            if (ViewBag.CurrLogRollId == 8)
            {
                ViewBag.RollName = "Admin";
                ViewBag.RollNameHi = "व्यवस्थापक";
            }
            else
            {
                ViewBag.RollName = "CMO";
                ViewBag.RollNameHi = "सी.एम.ओ.";
            }
            if (list != null)
            {
                SM.AppTypeID = reportModel.appTypeId;
                SM.FromDate = reportModel.fromDate;
                SM.ToDate = reportModel.toDate;
                ViewData["CMOfficeCountReportList"] = list;

            }
            else
            {

                ViewData["CMOfficeCountReportList"] = null;
            }
            return View();
        }


        public ActionResult CMOfficeAppCountProcessDetails(string appCurrStatus = "", string registrationNo = "", string appDate = "", string status = "", string ReportName = "", string DistrictID = "")
        {
            ApplicationStatusReportDetailsModel model = new ApplicationStatusReportDetailsModel();
            ReportsModel ReportModel = new ReportsModel();
            ViewBag.DLLAppStatus = objComDB.GetApplicationProcessByAppName("NUH").Where(m => m.Value == "-1" || m.Value == "1").ToList();
            ReportModel.appTypeId = SM.AppTypeID;
            ReportName = OTPL_Imp.CustomCryptography.Decrypt(ReportName);
            SM.ReportName = ReportName;
            ViewBag.CurrLogRollId = SM.RollID;

            int appCS = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(appCurrStatus));
            model.hdnappCS = appCS;
            ViewBag.AppStatus = appCS;
            if (ReportModel.appTypeId == 1)
            {
                ViewBag.ServiceName = "Medical Establishment";
            }
            else if (ReportModel.appTypeId == 2)
            {
                ViewBag.ServiceName = "Unsuccessful Family Planning";
            }
            else if (ReportModel.appTypeId == 3)
            {
                ViewBag.ServiceName = "Disability Certificate";
            }
            else if (ReportModel.appTypeId == 4)
            {
                ViewBag.ServiceName = "Age Certificate";
            }
            else if (ReportModel.appTypeId == 5)
            {
                ViewBag.ServiceName = "Medical Reimbursement";
            }
            else if (ReportModel.appTypeId == 6)
            {
                ViewBag.ServiceName = "Fitness Certificate";
            }
            else if (ReportModel.appTypeId == 7)
            {
                ViewBag.ServiceName = "Illness Certificate";
            }
            else if (ReportModel.appTypeId == 8)
            {
                ViewBag.ServiceName = "Death Certificate";
            }
            else if (ReportModel.appTypeId == 9)
            {
                ViewBag.ServiceName = "Immunization Children Certificate";
            }
            else if (ReportModel.appTypeId == 10)
            {
                ViewBag.ServiceName = "Immunization Certificate";
            }
            else if (ReportModel.appTypeId == 11)
            {
                ViewBag.ServiceName = "Medico Legal Certificate";
            }
            if (ViewBag.CurrLogRollId == 8)
            {
                ViewBag.RollName = "Admin";
                ViewBag.RollNameHi = "व्यवस्थापक";
            }
            else
            {
                ViewBag.RollName = "CMO";
                ViewBag.RollNameHi = "सी.एम.ओ.";
            }
            return View(model);

        }

        public ActionResult CMOfficeAppCountProcessDetailsList(string appCurrStatus = "", string registrationNo = "", string appDate = "", string status = "", string ReportName = "", string DistrictID = "", string buttonSearchValue = "")
        {
            int intStatus = 0;
            ApplicationStatusReportDetailsModel ReportModel = new ApplicationStatusReportDetailsModel();
            if (buttonSearchValue == null || buttonSearchValue == "")
            {
                appDate = OTPL_Imp.CustomCryptography.Decrypt(appDate);
                status = OTPL_Imp.CustomCryptography.Decrypt(status);
            }
            int appCS = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(appCurrStatus));
            ReportModel.hdnappCS = appCS;
            ReportName = OTPL_Imp.CustomCryptography.Decrypt(ReportName);
            DistrictID = OTPL_Imp.CustomCryptography.Decrypt(DistrictID);
            SM.RDistrictID = DistrictID;

            // string dist = SM.RDistrictID;
            string dist = DistrictID;
            if (!string.IsNullOrEmpty(SM.FromDate) && SM.AppTypeID >= 0)
            {
                try
                {

                    SM.ReportName = ReportName;
                    ReportModel.appTypeId = SM.AppTypeID;
                    ReportModel.ReportModelList = ObjRptDb.GetAllCMOfficeAppCountApplicationList(appCS, ReportName, dist, SM.FromDate, SM.ToDate, SM.AppTypeID);
                    intStatus = !string.IsNullOrEmpty(status) ? Convert.ToInt32(status) : 0;
                    ReportModel.ReportModelList = ReportModel.ReportModelList.Where(m => (m.appStatus == intStatus || intStatus == 0) && (m.appliedDate == appDate || appDate == "") && (m.registrationNo == registrationNo || registrationNo == "")).ToList();
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                ReportModel.ReportModelList = null;
            }

            if (SM.AppTypeID == 1)
            {

                return PartialView("_CMOfficeAppProcessCountDetailsList", ReportModel.ReportModelList);
            }

            if (SM.AppTypeID == 2)
            {
                return PartialView("_CMOfficeAppProcessCountDetailsFAPList", ReportModel.ReportModelList);
            }
            if (SM.AppTypeID == 3)
            {

                return PartialView("_CMOfficeAppProcessCountDetailsDICList", ReportModel.ReportModelList);
            }
            if (SM.AppTypeID == 4)
            {
                return PartialView("_CMOfficeAppProcessCountDetailsAGCList", ReportModel.ReportModelList);
            }

            if (SM.AppTypeID == 5)
            {
                return PartialView("_CMOfficeAppProcessCountDetailsMERList", ReportModel.ReportModelList);
            }
            if (SM.AppTypeID == 6)
            {
                return PartialView("_CMOfficeAppProcessCountDetailsFCList", ReportModel.ReportModelList);
            }
            if (SM.AppTypeID == 7)
            {
                return PartialView("_CMOfficeAppProcessCountDetailsICList", ReportModel.ReportModelList);
            }
            if (SM.AppTypeID == 8)
            {
                return PartialView("_CMOfficeAppProcessCountDetailsDCList", ReportModel.ReportModelList);
            }
            if (SM.AppTypeID == 9)
            {
                return PartialView("_CMOfficeAppProcessCountDetailsICCList", ReportModel.ReportModelList);
            }
            if (SM.AppTypeID == 10)
            {
                return PartialView("_CMOfficeAppProcessCountDetailsIMCList", ReportModel.ReportModelList);
            }
            if (SM.AppTypeID == 11)
            {
                return PartialView("_CMOfficeAppProcessCountDetailsMLCList", ReportModel.ReportModelList);
            }
            return View(ReportModel);
        }

        [AuthorizeAdmin]
        public ActionResult PrintApplicationFormDIC(string registrationNo)
        {
            registrationNo = OTPL_Imp.CustomCryptography.Decrypt(registrationNo);
            DICModel model = new DICModel();
            DIC_DB objDB_DIC = new DIC_DB();
            model = objDB_DIC.GetDICListBYRegistrationNo(0, registrationNo);

            if (SM.RollID == 2 || SM.RollID == 20)
            {
                if (model == null || SM.districtId != model.districtId)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.districtId).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID != 8)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        [AuthorizeAdmin]
        public ActionResult PrintApplicationFormFAP(string RegistrationID)
        {
            RegistrationID = OTPL_Imp.CustomCryptography.Decrypt(RegistrationID);
            Session["regisIdFAP"] = RegistrationID;

            FAPModel model = new FAPModel();
            model = objFAP_DB.GetFAPListBYRegistrationNo(Convert.ToInt64(RegistrationID));

            if (SM.RollID == 20)
            {
                if (model == null || SM.districtId != model.healthunitDistrictId)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID >= 2 && SM.RollID <= 5)
            {
                if (model == null || SM.districtId != model.healthunitDistrictId || !(SM.RollID != 2 ? SM.UserID == model.forwardTo : true))
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.healthunitDistrictId).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID != 8 && SM.RollID != 13)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        [AuthorizeAdmin]
        public ActionResult PrintApplicationFormNUH(string RegistrationID)
        {
            RegistrationID = OTPL_Imp.CustomCryptography.Decrypt(RegistrationID);
            Session["regisIdNUH"] = RegistrationID;
            NUH_DB objNUH_DB = new NUH_DB();
            NUHmodel model = new NUHmodel();
            model = objNUH_DB.GetNUHListBYRegistrationNo(Convert.ToInt64(RegistrationID));

            if (SM.RollID == 2 || SM.RollID == 20)
            {
                if (model == null || SM.districtId != model.districtid)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.districtid).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID != 8)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            ViewBag.VBoutpateint = objNUHDB.GetoutPatient(model.regisIdNUH);
            ViewBag.VBolaboratory = objNUHDB.GetNUHlaboratory(model.regisIdNUH);
            ViewBag.VBimaging = objNUHDB.GetNUHimaging(model.regisIdNUH);

            model.NUHPartnerList = objNUHDB.getNUHPartner(Convert.ToInt64(RegistrationID));
            model.NUHDOCList = objNUHDB.getNUHdoc(Convert.ToInt64(RegistrationID));
            model.NUHModelList = objNUHDB.getNUHChild(Convert.ToInt64(RegistrationID));

            return View(model);
        }

        public ActionResult PrintApplicationFormMER(string RegistrationID)
        {
            RegistrationID = OTPL_Imp.CustomCryptography.Decrypt(RegistrationID);
            Session["regisIdMER"] = RegistrationID;
            MERModel model = new MERModel();
            model = objMER_DB.getMERByRegistration(Convert.ToInt64(RegistrationID));

            if (SM.RollID == 20)
            {
                if (model == null || SM.districtId != model.postingDistrictId)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID >= 2 && SM.RollID <= 5)
            {
                if (model == null || SM.districtId != model.postingDistrictId || !(SM.RollID != 2 ? SM.UserID == Convert.ToInt64(model.ForwardedToId) : model.TotalBillAmount >= 50000))
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.postingDistrictId).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID != 8)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        public ActionResult PrintApplicationFormAGC(string RegistrationID = "")
        {
            long regisId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(RegistrationID));
            string Registration = "";
            AGCModel model = new AGCModel();
            model = objAGC_DB.GetAGCListBYRegistrationNo(regisId, Registration);

            if (SM.RollID == 2 || SM.RollID == 20)
            {
                if (model == null || SM.districtId != model.susdistrictId)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.susdistrictId).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID != 8)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        [AuthorizeAdmin]
        public ActionResult PrintApplicationFormMLC(string RegistrationID)
        {
            MLCModel model = new MLCModel();

            model = objMlc.GetMLCListBYRegistrationNo(0, "", Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(RegistrationID)));

            if (SM.RollID >= 3 && SM.RollID <= 5)
            {
                if (model == null || SM.UserID != model.forwardtoId)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.districtId).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID != 8)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            TempData["reg"] = model.registrationNo;
            Session["reg"] = model.registrationNo;
            return View(model);
        }

        [AuthorizeAdmin]
        public ActionResult PrintApplicationFormIMC(string RegistrationID)
        {
            RegistrationID = OTPL_Imp.CustomCryptography.Decrypt(RegistrationID);
            Session["regisIdIMC"] = RegistrationID;
            IMC_DB objIMC_DB = new IMC_DB();
            IMCModel model = new IMCModel();
            model = objIMC_DB.IMCDetailsByRegistration(Convert.ToInt64(RegistrationID));

            if (SM.RollID >= 3 && SM.RollID <= 5)
            {
                if (model == null || SM.UserID != model.forwardType)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.districtId).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID != 8)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        [AuthorizeAdmin]
        public ActionResult PrintApplicationFormICC(string RegistrationID)
        {
            ICCModel model = new ICCModel();

            model = objICC.GetICCListBYRegistrationNo(0, "", Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(RegistrationID)));

            if (SM.RollID >= 3 && SM.RollID <= 5)
            {
                if (model == null || SM.UserID != model.forwardtoId)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.districtId).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID != 8 && SM.RollID != 13)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        [AuthorizeAdmin]
        public ActionResult PrintApplicationFormILC(string RegistrationID)
        {
            RegistrationID = OTPL_Imp.CustomCryptography.Decrypt(RegistrationID);
            ILCModel model = new ILCModel();
            model = objILC_DB.GetILCListBYRegistrationNo(Convert.ToInt64(RegistrationID));

            if (SM.RollID >= 3 && SM.RollID <= 5)
            {
                if (model == null || SM.UserID != model.forwardtoId)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.districtId).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID != 8)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        [AuthorizeAdmin]
        public ActionResult PrintApplicationFormFIC(string registrationNo)
        {
            long regisByuser = SM.UserID;
            registrationNo = OTPL_Imp.CustomCryptography.Decrypt(registrationNo);
            FICModel model = new FICModel();
            model = objFICdb.GetFICListBYRegistrationNo(registrationNo);

            if (SM.RollID >= 3 && SM.RollID <= 5)
            {
                if (model == null || SM.UserID != model.forwardtoId)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.districtId).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID != 8)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            return View(model);
        }

        [AuthorizeAdmin]
        public ActionResult PrintApplicationFormDEC(string RegistrationID)
        {
            DECModel model = new DECModel();

            model = objdb.GetDECListCHC(Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(RegistrationID)));

            if (SM.RollID >= 3 && SM.RollID <= 5)
            {
                if (model == null || SM.UserID != model.forwardtoId)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID == 18)
            {
                var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
                int count = lstCMODistrict.Where(e => e.districtId == model.districtid).ToList().Count();
                if (count == 0)
                {
                    return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
                }
            }
            else if (SM.RollID != 8)
            {
                return RedirectToAction("UnauthoriseAcess", "Home", new { isWithoutLayout = true });
            }

            TempData["reg"] = model.registrationNo;
            Session["reg"] = model.registrationNo;
            return View(model);
        }

        public ActionResult CMOfficeCountProcessRpt(string FromDate = "", string ToDate = "")
        {
            ReportsModel reportModel = new ReportsModel();
            //reportModel.fromDate = OTPL_Imp.CustomCryptography.Decrypt(SM.FromDate);
            //reportModel.toDate = OTPL_Imp.CustomCryptography.Decrypt(SM.ToDate);
            string setPdfName = "", setDigitalPdfName = "";

            var res = ObjRptDb.ProcessCountReportCMODrillDown(12, "", SM.FromDate, SM.ToDate);


            if (res != null && res.Count > 0)
            {
                try
                {
                    ReportDocument rd = new ReportDocument();

                    rd.Load(Path.Combine(Server.MapPath("~/RPT"), "CMOfficeTotalCountServices.rpt"));
                    rd.SetDataSource(res);

                    setPdfName = "CMOffice_CountReport";

                    string folderpath = "~/Content/writereaddata/CMO/" + SM.ReportName + "/";

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
                    //if (System.IO.File.Exists(Server.MapPath(flName)))
                    //{
                    //    int result = objIMC_DB.InsertUnSignedCertiPath_IMC(res[0].cmo, flName);
                    //}
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

                        setDigitalPdfName = "CMOfficeCountProccessReport" + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                        string digitalFlName = folderpath + setDigitalPdfName + ".pdf";


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

        public ActionResult CMOfficeCountProcessExcel()
        {
            List<ReportsModel> list = ObjRptDb.ProcessCountReportCMODrillDown(12, "", SM.FromDate, SM.ToDate);
            string filename = "";
            if (list != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Sr.No.", typeof(string));
                dt.Columns.Add("Service Name", typeof(string));
                dt.Columns.Add("Health Portal", typeof(string));
                dt.Columns.Add("E-District", typeof(string));
                dt.Columns.Add("Total", typeof(string));
                dt.Columns.Add("Resolved", typeof(string));
                dt.Columns.Add("Pending", typeof(string));
                dt.Columns.Add("Pending Percentage", typeof(string));

                int i = 0;
                foreach (var res in list)
                {
                    if (i == 0)
                    {
                        filename = "CMOfficeCountProcessExcel.xls";
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    dr["Sr.No."] = i;
                    dr["Service Name"] = res.Module;
                    dr["Health Portal"] = res.AppFromPortal;
                    dr["E-District"] = res.AppNotFromPortal;
                    dr["Total"] = res.total;
                    dr["Resolved"] = res.Nistarit;
                    dr["Pending"] = res.Lambit;
                    dr["Pending Percentage"] = res.Percentage;

                    dt.Rows.Add(dr);
                }
                DataRow dr1 = dt.NewRow();
                dr1["Sr.No."] = "";
                dr1["Service Name"] = "Total Service Wise Count";
                dr1["Health Portal"] = list.Sum(a => a.AppFromPortal);
                dr1["E-District"] = list.Sum(a => a.AppNotFromPortal);
                dr1["Total"] = list.Sum(a => a.total);
                dr1["Resolved"] = list.Sum(a => a.Nistarit);
                dr1["Pending"] = list.Sum(a => a.Lambit);
                dr1["Pending Percentage"] = list.Sum(a => a.Percentage);
                dt.Rows.Add(dr1);
                var gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();


                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                string headerTable = @"<Table border=1><tr rowspan=2 align=center><td colspan=8><h3> Department of Medical Health And Family Welfare </h3></td></tr><tr rowspan=2 align=center><td colspan=8><h4>Goverment of Uttar Pradesh</h4></td></tr><tr rowspan=2 align=center><td colspan=8><h4>Services Under Janhit Gaurantee Adhiniyam</h4></td></tr><tr rowspan=2 align=center><td colspan=8><h4>CMO Office Count Report</h4> </td></tr><tr><td colspan=8>From :-" + SM.FromDate + " &nbsp;To :-" + SM.ToDate + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Report Code:-UPH-001</td></tr></Table>";
                Response.Write(headerTable);
                //Response.Write(headerTable);
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
                return RedirectToAction("CMOfficeCountReport", new { SM.FromDate, SM.ToDate });
            }
        }

        public ActionResult CMOfficeCountProcessDeathRpt(string ReportName = "", string FromDate = "", string ToDate = "")
        {
            ReportsModel reportModel = new ReportsModel();
            //reportModel.fromDate = OTPL_Imp.CustomCryptography.Decrypt(SM.FromDate);
            //reportModel.toDate = OTPL_Imp.CustomCryptography.Decrypt(SM.ToDate);
            string rptName = null;
            ReportName = OTPL_Imp.CustomCryptography.Decrypt(ReportName);
            reportModel.appTypeId = SM.AppTypeID;
            reportModel.fromDate = OTPL_Imp.CustomCryptography.Decrypt(FromDate);
            reportModel.toDate = OTPL_Imp.CustomCryptography.Decrypt(ToDate);

            string setPdfName = "", setDigitalPdfName = "";

            string dist = "";

            var districtLst = objComDB.GetDropDownList(7, 34);

            if (districtLst != null && districtLst.Count > 0)
            {
                foreach (var v in districtLst)
                {
                    dist = dist + v.Id + ",";
                }
            }

            var res = ObjRptDb.DistrictWise_AllServiceCountReport(ReportName, reportModel.appTypeId, dist, reportModel.fromDate, reportModel.toDate);

            if (SM.AppTypeID == 1)
            {
                rptName = "Medical_Establishment";
            }
            else if (SM.AppTypeID == 2)
            {
                rptName = "Unsuccessful_Family_Planning";
            }
            else if (SM.AppTypeID == 3)
            {
                rptName = "Disability_Certificate";
            }
            else if (SM.AppTypeID == 4)
            {
                rptName = "Age_Certificate";
            }
            else if (SM.AppTypeID == 5)
            {
                rptName = "Medical_Reimbursement";
            }
            else if (SM.AppTypeID == 6)
            {
                rptName = "Fitness Certificate";
            }
            else if (SM.AppTypeID == 7)
            {
                rptName = "Illness_Certificate";
            }
            else if (SM.AppTypeID == 8)
            {
                rptName = "Death_Certificate";
            }
            else if (SM.AppTypeID == 9)
            {
                rptName = "Immunization_Children_Certificate";
            }
            else if (SM.AppTypeID == 10)
            {
                rptName = "Immunization_Certificate";
            }
            else if (SM.AppTypeID == 11)
            {
                rptName = "Medico_Legal_Certificate";
            }

            if (ReportName == "total")
            {
                if (res != null && res.Count() > 0)
                {
                    try
                    {
                        ReportDocument rd = new ReportDocument();

                        rd.Load(Path.Combine(Server.MapPath("~/RPT"), "CMOfficeTotalCountDeathRpt.rpt"));
                        rd.SetDataSource(res);

                        rd.SetParameterValue("rptName", rptName);
                        // rd.SetParameterValue("districtName", res.FirstOrDefault().DistrictName);

                        setPdfName = "CMOffice_" + rptName + "_" + ReportName;

                        string folderpath = "~/Content/writereaddata/CMO/" + SM.ReportName + "/";

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
                        //if (System.IO.File.Exists(Server.MapPath(flName)))
                        //{
                        //    int result = objIMC_DB.InsertUnSignedCertiPath_IMC(res[0].cmo, flName);
                        //}
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
            }
            else if (ReportName == "Resolved")
            {
                if (res != null && res.Count() > 0)
                {
                    try
                    {
                        ReportDocument rd = new ReportDocument();

                        rd.Load(Path.Combine(Server.MapPath("~/RPT"), "CMOfficeTotalCountDeathAcceptRpt.rpt"));
                        rd.SetDataSource(res);

                        rd.SetParameterValue("rptName", rptName);
                        // rd.SetParameterValue("districtName", res.FirstOrDefault().DistrictName);

                        setPdfName = "CMOffice_" + rptName + "_" + ReportName;

                        string folderpath = "~/Content/writereaddata/CMO/" + SM.ReportName + "/";

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
                        //if (System.IO.File.Exists(Server.MapPath(flName)))
                        //{
                        //    int result = objIMC_DB.InsertUnSignedCertiPath_IMC(res[0].cmo, flName);
                        //}
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
            }
            else if (ReportName == "pending")
            {
                if (res != null && res.Count() > 0)
                {
                    try
                    {
                        ReportDocument rd = new ReportDocument();

                        rd.Load(Path.Combine(Server.MapPath("~/RPT"), "CMOfficeTotalCountDeathPendingRpt.rpt"));
                        rd.SetDataSource(res);

                        rd.SetParameterValue("rptName", rptName);
                        // rd.SetParameterValue("districtName", res.FirstOrDefault().DistrictName);

                        setPdfName = "CMOffice_" + rptName + "_" + ReportName;

                        string folderpath = "~/Content/writereaddata/CMO/" + SM.ReportName + "/";

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
                        //if (System.IO.File.Exists(Server.MapPath(flName)))
                        //{
                        //    int result = objIMC_DB.InsertUnSignedCertiPath_IMC(res[0].cmo, flName);
                        //}
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
            }

            return View("DownloadFile");
        }

        public ActionResult CMOfficeCountProcessDeathExcel(string ReportName = "", string FromDate = "", string ToDate = "")
        {
            string RptName = OTPL_Imp.CustomCryptography.Decrypt(ReportName);
            string FDate = OTPL_Imp.CustomCryptography.Decrypt(FromDate);
            string TDate = OTPL_Imp.CustomCryptography.Decrypt(ToDate);
            string dist = "";

            var districtLst = objComDB.GetDropDownList(7, 34);

            if (districtLst != null && districtLst.Count > 0)
            {
                foreach (var v in districtLst)
                {
                    dist = dist + v.Id + ",";
                }
            }
            List<ReportsModel> list = ObjRptDb.DistrictWise_AllServiceCountReport(RptName, SM.AppTypeID, dist, FDate, TDate);
            string filename = "";
            if (list != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Sr.No.", typeof(string));
                dt.Columns.Add("District Wise", typeof(string));
                dt.Columns.Add("Total Application's", typeof(string));

                int i = 0;
                foreach (var res in list)
                {
                    if (i == 0)
                    {
                        filename = "CMOfficeCountProcess" + RptName + ".xls";
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    dr["Sr.No."] = i;
                    dr["District Wise"] = res.DistrictName;
                    if (SM.ReportName == "total")
                    {
                        dr["Total Application's"] = res.total;
                    }
                    else if (SM.ReportName == "Resolved")
                    {
                        dr["Total Application's"] = res.Application_Accepted;
                    }
                    else if (SM.ReportName == "pending")
                    {
                        dr["Total Application's"] = res.Pending;
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
                string headerTable = @"<Table border=1><tr rowspan=2 align=center><td colspan=3><h3> Department of Medical Health And Family Welfare</h3></td></tr><tr rowspan=2 align=center><td colspan=3><h4>CMO Office Count Report</h4> </td></tr><tr><td colspan=3>From :-" + FDate + " &nbsp; To :-" + TDate + "</td></tr></Table>";
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
                return RedirectToAction("CMOfficeCountReport", new { SM.FromDate, SM.ToDate });
            }
        }

        public ActionResult CMOfficeCountProcessApplicationRpt(int hdnappCS = 0, string registrationNo = "", string appDate = "", string status = "", string ReportName = "", string DistrictID = "", string buttonSearchValue = "")
        {
            ReportsModel reportModel = new ReportsModel();
            string rptName = null;
            //int appCS = Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(appCurrStatus));
            int appCS = hdnappCS;
            ReportName = OTPL_Imp.CustomCryptography.Decrypt(ReportName);
            DistrictID = OTPL_Imp.CustomCryptography.Decrypt(DistrictID);
            SM.RDistrictID = DistrictID;
            string dist = DistrictID;
            string setPdfName = "", setDigitalPdfName = "";

            var res = ObjRptDb.GetAllCMOfficeAppCountApplicationListForRpt(appCS, ReportName, dist, SM.FromDate, SM.ToDate, SM.AppTypeID);


            if (SM.AppTypeID == 1)
            {
                rptName = "Medical Establishment";
            }
            else if (SM.AppTypeID == 2)
            {
                rptName = "Unsuccessful Family Planning";
            }
            else if (SM.AppTypeID == 3)
            {
                rptName = "Disability Certificate";
            }
            else if (SM.AppTypeID == 4)
            {
                rptName = "Age Certificate";
            }
            else if (SM.AppTypeID == 5)
            {
                rptName = "Medical Reimbursement";
            }
            else if (SM.AppTypeID == 6)
            {
                rptName = "Fitness Certificate";
            }
            else if (SM.AppTypeID == 7)
            {
                rptName = "Illness Certificate";
            }
            else if (SM.AppTypeID == 8)
            {
                rptName = "Death Certificate";
            }
            else if (SM.AppTypeID == 9)
            {
                rptName = "Immunization Children Certificate";
            }
            else if (SM.AppTypeID == 10)
            {
                rptName = "Immunization Certificate";
            }
            else if (SM.AppTypeID == 11)
            {
                rptName = "Medico Legal Certificate";
            }

            //if (ReportName == "total")
            // {
            if (res != null && res.Count() > 0)
            {
                try
                {
                    ReportDocument rd = new ReportDocument();

                    rd.Load(Path.Combine(Server.MapPath("~/RPT"), "CMOfficeCountProcessApplicationRpt.rpt"));
                    rd.SetDataSource(res);

                    rd.SetParameterValue("rptName", rptName);

                    rd.SetParameterValue("districtName", res.FirstOrDefault().DistrictName);

                    setPdfName = "CMOffice_" + rptName + "_" + ReportName;

                    string folderpath = "~/Content/writereaddata/CMO/" + SM.ReportName + "/";

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
                    //if (System.IO.File.Exists(Server.MapPath(flName)))
                    //{
                    //    int result = objIMC_DB.InsertUnSignedCertiPath_IMC(res[0].cmo, flName);
                    //}
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

        #endregion

        #region Admin Count Report

        [HttpGet]
        [AuthorizeAdmin(15)]
        public ActionResult ProcessCountReportAdmin(string RptType = "")
        {
            ReportsModel model = new ReportsModel();
            if (RptType != "")
            {
                TempData["RptType"] = RptType;
                model.RptType = Convert.ToInt32(RptType);
            }

            ViewBag.Application = objComDB.GetDropDownList(48, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            return View(model);
        }

        #region Vinod Process Count Report
        [HttpPost]
        [AuthorizeAdmin(15)]
        public ActionResult ProcessCountReportAdmin(ReportsModel model)
        {
            string dist = "";

            if (model.DistrictIds != null)
            {
                foreach (var v in model.DistrictIds)
                {
                    dist = dist + v + ",";
                }
            }

            TempData["RptType"] = model.RptType;
            TempData["DistrictIds"] = dist;
            //Vk
            SM.RDistrictID = dist;

            if (model.appTypeId == 0)
            {
                switch (model.appTypeId)
                {
                    case 0: return RedirectToAction("ProcessCountReportCMoffice", new { model.fromDate, model.toDate });
                        break;
                    case 1: return RedirectToAction("ProcessCountReportNUH", new { model.fromDate, model.toDate });
                        break;
                    case 2: return RedirectToAction("ProcessCountReportFAP", new { model.fromDate, model.toDate });
                        break;
                    case 3: return RedirectToAction("ProcessCountReportDIC", new { model.fromDate, model.toDate });
                        break;
                    case 4: return RedirectToAction("ProcessCountReportAGC", new { model.fromDate, model.toDate });
                        break;
                    case 5: return RedirectToAction("ProcessCountReportMER", new { model.fromDate, model.toDate });
                        break;
                    case 12: return RedirectToAction("ProcessCountReportALL", new { model.fromDate, model.toDate });
                        break;
                    default: return RedirectToAction("UnauthoriseAcess", "Home");
                        break;
                }
            }
            else
            {
                #region Bind Total Report By Application Type
                if (model.appTypeId == 0)
                {
                    List<ReportsModel> list = ObjRptDb.ProcessCountReportCMO(12, dist, model.fromDate, model.toDate);
                    if (list != null)
                    {
                        SM.AppTypeID = model.appTypeId;
                        SM.FromDate = model.fromDate;
                        SM.ToDate = model.toDate;
                        ViewData["ProcessCountReportList"] = list;

                    }
                    else
                    {

                        ViewData["ProcessCountReportList"] = null;
                    }
                }

                if (model.appTypeId == 1)
                {
                    List<ReportsModel> list = ObjRptDb.ProcessCountReportCMO(1, dist, model.fromDate, model.toDate);
                    if (list != null)
                    {
                        SM.AppTypeID = model.appTypeId;
                        SM.FromDate = model.fromDate;
                        SM.ToDate = model.toDate;
                        ViewData["ProcessCountReportList"] = list;

                    }
                    else
                    {

                        ViewData["ProcessCountReportList"] = null;
                    }
                }

                if (model.appTypeId == 2)
                {
                    List<ReportsModel> list = ObjRptDb.ProcessCountReportCMO(2, dist, model.fromDate, model.toDate);
                    if (list != null)
                    {
                        SM.AppTypeID = model.appTypeId;
                        SM.FromDate = model.fromDate;
                        SM.ToDate = model.toDate;
                        ViewData["ProcessCountReportList"] = list;

                    }
                    else
                    {

                        ViewData["ProcessCountReportList"] = null;
                    }
                }

                if (model.appTypeId == 3)
                {

                    List<ReportsModel> list = ObjRptDb.ProcessCountReportCMO(3, dist, model.fromDate, model.toDate);
                    if (list != null)
                    {
                        SM.AppTypeID = model.appTypeId;
                        SM.FromDate = model.fromDate;
                        SM.ToDate = model.toDate;
                        ViewData["ProcessCountReportList"] = list;

                    }
                    else
                    {

                        ViewData["ProcessCountReportList"] = null;
                    }
                }

                if (model.appTypeId == 4)
                {
                    List<ReportsModel> list = ObjRptDb.ProcessCountReportCMO(4, dist, model.fromDate, model.toDate);
                    if (list != null)
                    {
                        SM.AppTypeID = model.appTypeId;
                        SM.FromDate = model.fromDate;
                        SM.ToDate = model.toDate;
                        ViewData["ProcessCountReportList"] = list;

                    }
                    else
                    {

                        ViewData["ProcessCountReportList"] = null;
                    }
                }
                if (model.appTypeId == 5)
                {
                    List<ReportsModel> list = ObjRptDb.ProcessCountReportCMO(5, dist, model.fromDate, model.toDate);
                    if (list != null)
                    {
                        SM.AppTypeID = model.appTypeId;
                        SM.FromDate = model.fromDate;
                        SM.ToDate = model.toDate;
                        ViewData["ProcessCountReportList"] = list;

                    }
                    else
                    {

                        ViewData["ProcessCountReportList"] = null;
                    }
                }

                if (model.appTypeId == 12)
                {
                    if (TempData["DistrictIds"] == null)
                    {
                        return RedirectToAction("UnauthoriseAcess", "Home");
                    }
                    List<ReportsModel> list = ObjRptDb.ProcessCountReportCMO(0, dist, model.fromDate, model.toDate);
                    if (list != null)
                    {
                        SM.AppTypeID = model.appTypeId;
                        SM.FromDate = model.fromDate;
                        SM.ToDate = model.toDate;
                        ViewData["ProcessCountReportList"] = list;
                    }
                    else
                    {
                        ViewData["ProcessCountReportList"] = null;
                    }

                }

                #endregion
            }

            if (SM.RollID == 2)
            {
                TempData["RollID"] = "CMO";
            }
            else if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                ViewBag.Division = ", Division - " + SM.DisplayName;
            }
            else if (SM.RollID == 8)
            {
                TempData["RollID"] = "admin";
            }
            // End VK

            ViewBag.Application = objComDB.GetDropDownList(47, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            return View();
        }

        public ActionResult ApplicationCountProcessDetails(string registrationNo = "", string appDate = "", string status = "", string ReportName = "", string DistrictID = "")
        {

            ReportsModel ReportModel = new ReportsModel();
            ViewBag.DLLAppStatus = objComDB.GetApplicationProcessByAppName("NUH").Where(m => m.Value == "-1" || m.Value == "1").ToList();
            ReportModel.appTypeId = SM.AppTypeID;
            ReportName = OTPL_Imp.CustomCryptography.Decrypt(ReportName);
            SM.ReportName = ReportName;
            ViewBag.CurrLogRollId = SM.RollID;
            if (ReportModel.appTypeId == 1)
            {
                ViewBag.ServiceName = "Medical Establishment";
            }
            else if (ReportModel.appTypeId == 2)
            {
                ViewBag.ServiceName = "Unsuccessful Family Planning";
            }
            else if (ReportModel.appTypeId == 3)
            {
                ViewBag.ServiceName = "Disability Certificate";
            }
            else if (ReportModel.appTypeId == 4)
            {
                ViewBag.ServiceName = "Age Certificate";
            }
            else if (ReportModel.appTypeId == 5)
            {
                ViewBag.ServiceName = "Medical Reimbursement";
            }

            if (ViewBag.CurrLogRollId == 8)
            {
                ViewBag.RollName = "Admin";
                ViewBag.RollNameHi = "व्यवस्थापक";
            }
            else
            {
                ViewBag.RollName = "CMO";
                ViewBag.RollNameHi = "सी.एम.ओ.";
            }
            return View();

        }

        public ActionResult ApplicationCountProcessDetailsList(string registrationNo = "", string appDate = "", string status = "", string ReportName = "", string DistrictID = "", string buttonSearchValue = "")
        {
            int intStatus = 0;
            ApplicationStatusReportDetailsModel ReportModel = new ApplicationStatusReportDetailsModel();
            if (buttonSearchValue == null || buttonSearchValue == "")
            {
                appDate = OTPL_Imp.CustomCryptography.Decrypt(appDate);
                status = OTPL_Imp.CustomCryptography.Decrypt(status);
            }

            ReportName = OTPL_Imp.CustomCryptography.Decrypt(ReportName);
            DistrictID = OTPL_Imp.CustomCryptography.Decrypt(DistrictID);
            SM.RDistrictID = DistrictID;

            // string dist = SM.RDistrictID;
            string dist = DistrictID;
            if (!string.IsNullOrEmpty(SM.FromDate) && SM.AppTypeID >= 0)
            {
                try
                {

                    SM.ReportName = ReportName;
                    ReportModel.appTypeId = SM.AppTypeID;
                    ReportModel.ReportModelList = ObjRptDb.GetAllApplicationCountList(ReportName, dist, SM.FromDate, SM.ToDate, SM.AppTypeID);
                    intStatus = !string.IsNullOrEmpty(status) ? Convert.ToInt32(status) : 0;
                    ReportModel.ReportModelList = ReportModel.ReportModelList.Where(m => (m.appStatus == intStatus || intStatus == 0) && (m.appliedDate == appDate || appDate == "") && (m.registrationNo == registrationNo || registrationNo == "")).ToList();
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                ReportModel.ReportModelList = null;
            }

            if (SM.AppTypeID == 1)
            {

                return PartialView("_ApplicationCountProcessDetailsList", ReportModel.ReportModelList);
            }

            if (SM.AppTypeID == 2)
            {
                return PartialView("_ApplicationCountProcessDetailsFAPList", ReportModel.ReportModelList);
            }
            if (SM.AppTypeID == 3)
            {

                return PartialView("_ApplicationCountProcessDetailsDICList", ReportModel.ReportModelList);
            }
            if (SM.AppTypeID == 4)
            {
                return PartialView("_ApplicationCountProcessDetailsAGCList", ReportModel.ReportModelList);
            }

            if (SM.AppTypeID == 5)
            {
                return PartialView("_ApplicationCountProcessDetailsMERList", ReportModel.ReportModelList);
            }

            return View(ReportModel);
        }

        public ActionResult GetApplicationStatusList(string RegistrationID)
        {
            ApplicationStatusReportDetailsModel ReportModel = new ApplicationStatusReportDetailsModel();
            RegistrationID = OTPL_Imp.CustomCryptography.Decrypt(RegistrationID);
            if (SM.AppTypeID == 1)
            {
                NUHmodel model = new NUHmodel();
                model = objNUHDB.GetNUHListBYRegistrationNo(Convert.ToInt64(RegistrationID));
                //ViewBag.ReasonList = comndb.GetDropDownList(65, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                model.NUHModelList = objNUHDB.getNUHChild(Convert.ToInt64(RegistrationID));
                ViewBag.VBoutpateint = objNUHDB.GetoutPatient(Convert.ToInt64(RegistrationID));
                ViewBag.VBolaboratory = objNUHDB.GetNUHlaboratory(Convert.ToInt64(RegistrationID));
                ViewBag.VBimaging = objNUHDB.GetNUHimaging(Convert.ToInt64(RegistrationID));

                model.NUHPartnerList = objNUHDB.getNUHPartner(Convert.ToInt64(RegistrationID));
                model.NUHDOCList = objNUHDB.getNUHdoc(Convert.ToInt64(RegistrationID));

                Session["regisIdNUH"] = RegistrationID;

                return PartialView("_ViewAppDetailsNUHReport", model);//_ViewAppDetailsNUHReport
            }
            if (SM.AppTypeID == 2)
            {

                FAPModel model = new FAPModel();
                Session["regisIdFAP"] = RegistrationID;
                model = objCMODB.GetAllFAPList(2, Convert.ToInt64(RegistrationID), "", "", "", 0).Where(m => m.regisIdFAP == Convert.ToInt64(RegistrationID)).FirstOrDefault();
                return PartialView("_FAPDetailReport", model);//_FAPDetailReport

            }
            if (SM.AppTypeID == 3)
            {
                DICModel model = new DICModel();
                model = objCMODB.GetAllRegistrationDIC().Where(m => m.regisIdDIC == Convert.ToInt64(RegistrationID)).FirstOrDefault();
                return PartialView("_ViewAppDetailsDICReport", model);//_ViewAppDetailsDICReport
            }
            if (SM.AppTypeID == 4)
            {
                AGCModel model = new AGCModel();
                model = objCMODB.GetAGCListBYRegistrationNo(RegistrationID);
                return PartialView("_AgeCertificateDetailAGCReport", model);//_AgeCertificateDetailAGCReport
            }
            if (SM.AppTypeID == 5)
            {
                MERModel model = new MERModel();
                model = objMER_DB.getMERByRegistration(Convert.ToInt64(RegistrationID));
                model.MERModelList = objMER_DB.getMERChild(model.regisIdMER);
                return PartialView("_MedicalReimbursementMERReport", model);//_MedicalReimbursementMERReport
            }
            return View(ReportModel);
        }

        public ActionResult GetViewApplicationStatusNUH(string registrationID)
        {
            ApplicationWorkFlowStepStatusModel model = new ApplicationWorkFlowStepStatusModel();
            List<string> liststr = new List<string>();
            registrationID = OTPL_Imp.CustomCryptography.Decrypt(registrationID);
            if (registrationID != null)
            {
                try
                {
                    model = ObjRptDb.GetAppWorkflowStatusNUH(Convert.ToInt64(registrationID)).FirstOrDefault();
                    liststr = ObjRptDb.GetInspectionPhotoPath(1, Convert.ToInt64(registrationID), "").ToList();
                    model.inspReportFilePhotoPath = liststr.Select(i => i.ToString()).ToArray();
                }
                catch (Exception ex)
                {

                }
            }
            return PartialView("_ViewApplicationWorkFlowStatusNUH", model);

        }

        //public JsonResult GetViewApplicationStatusNUH(string registrationID)
        //{
        //    ApplicationWorkFlowStepStatusModel model = new ApplicationWorkFlowStepStatusModel();
        //    if (registrationID != null)
        //    {
        //        try
        //        {
        //            model.AppStatusList = ObjRptDb.GetAppWorkflowStatusNUH(Convert.ToInt64(registrationID));
        //        }
        //        catch (Exception ex)
        //        {

        //        }

        //    }
        //    return Json(model.AppStatusList,JsonRequestBehavior.AllowGet);

        //}

        public ActionResult GetViewApplicationStatusFAP(string registrationID)
        {

            List<string> liststrD = new List<string>();
            List<string> liststrS = new List<string>();
            ApplicationWorkFlowStepStatusModel model = new ApplicationWorkFlowStepStatusModel();
            registrationID = OTPL_Imp.CustomCryptography.Decrypt(registrationID);
            if (registrationID != null)
            {
                try
                {
                    model = ObjRptDb.GetAppWorkflowStatusFAP(Convert.ToInt64(registrationID)).FirstOrDefault();
                    liststrD = ObjRptDb.GetInspectionPhotoPath(2, Convert.ToInt64(registrationID), "D").ToList();
                    model.inspReportFilePhotoPath = liststrD.Select(i => i.ToString()).ToArray();

                    liststrS = ObjRptDb.GetInspectionPhotoPath(2, Convert.ToInt64(registrationID), "S").ToList();
                    model.inspReportFilePhotoPathCometti = liststrS.Select(i => i.ToString()).ToArray();
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
            return PartialView("_ViewApplicationWorkFlowStatusFAP", model);

        }



        public ActionResult GetViewApplicationStatusDIC(string registrationID)
        {
            List<string> liststr = new List<string>();
            ApplicationWorkFlowStepStatusModel model = new ApplicationWorkFlowStepStatusModel();
            registrationID = OTPL_Imp.CustomCryptography.Decrypt(registrationID);
            if (registrationID != null)
            {
                try
                {
                    model = ObjRptDb.GetAppWorkflowStatusDIC(Convert.ToInt64(registrationID)).FirstOrDefault();
                    liststr = ObjRptDb.GetInspectionPhotoPath(3, Convert.ToInt64(registrationID), "").ToList();
                    model.inspReportFilePhotoPath = liststr.Select(i => i.ToString()).ToArray();
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
            return PartialView("_ViewApplicationWorkFlowStatusDIC", model);
        }

        public ActionResult GetViewApplicationStatusAGC(string registrationID)
        {
            List<string> liststr = new List<string>();
            ApplicationWorkFlowStepStatusModel model = new ApplicationWorkFlowStepStatusModel();
            registrationID = OTPL_Imp.CustomCryptography.Decrypt(registrationID);
            if (registrationID != null)
            {
                try
                {
                    model = ObjRptDb.GetAppWorkflowStatusAGC(Convert.ToInt64(registrationID)).FirstOrDefault();
                    liststr = ObjRptDb.GetInspectionPhotoPath(4, Convert.ToInt64(registrationID), "").ToList();
                    model.inspReportFilePhotoPath = liststr.Select(i => i.ToString()).ToArray();
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
            return PartialView("_ViewApplicationWorkFlowStatusAGC", model);
        }


        public ActionResult GetViewApplicationStatusMER(string registrationID)
        {
            List<string> liststr = new List<string>();
            ApplicationWorkFlowStepStatusModel model = new ApplicationWorkFlowStepStatusModel();
            registrationID = OTPL_Imp.CustomCryptography.Decrypt(registrationID);
            if (registrationID != null)
            {
                try
                {
                    model = ObjRptDb.GetAppWorkflowStatusMER(Convert.ToInt64(registrationID)).FirstOrDefault();
                    liststr = ObjRptDb.GetInspectionPhotoPath(5, Convert.ToInt64(registrationID), "").ToList();
                    model.inspReportFilePhotoPath = liststr.Select(i => i.ToString()).ToArray();
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
            return PartialView("_ViewApplicationWorkFlowStatusMER", model);
        }


        #endregion
        [HttpGet]
        [AuthorizeAdmin(16)]
        public ActionResult ProcessCountReportCMSatAdmin(string RptType = "")
        {
            if (RptType != "")
            {
                TempData["RptType"] = RptType;
            }

            ViewBag.forwardTypes = objComDB.GetDropDownList(59, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Application = Enumerable.Empty<SelectListItem>();

            ReportsModel model = new ReportsModel();

            return View(model);
        }

        public JsonResult bindddl(long rollId)
        {
            if (rollId == 5)
            {
                var res = objComDB.GetDropDownList(58, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var res = objComDB.GetDropDownList(58, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() }).Where(m => m.Value != "5").ToList();
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        [AuthorizeAdmin(16)]
        public ActionResult ProcessCountReportCMSatAdmin(ReportsModel model)
        {
            int id = model.appTypeId;
            TempData["RedirectFromAction"] = "";
            string dist = "";

            if (model.DistrictIds != null)
            {
                foreach (var v in model.DistrictIds)
                {
                    dist = dist + v + ",";
                }
            }

            TempData["DistrictIds"] = dist;
            TempData["HealthUnitId"] = model.forwardtypeId;

            switch (id)
            {
                case 2: return RedirectToAction("ProcessCountReportFAPCMS", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 5: return RedirectToAction("ProcessCountReportMERCMS", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 6: return RedirectToAction("ProcessCountReportFIC", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 7: return RedirectToAction("ProcessCountReportILC", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 8: return RedirectToAction("ProcessCountReportDEC", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 9: return RedirectToAction("ProcessCountReportICC", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 10: return RedirectToAction("ProcessCountReportIMC", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 11: return RedirectToAction("ProcessCountReportMLC", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 12: return RedirectToAction("ProcessCountReportALLCMS", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                default: return RedirectToAction("UnauthoriseAcess", "Home");
                    break;
            }

            ViewBag.Application = objComDB.GetDropDownList(47, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComDB.GetDropDownList(7, 34).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            return View();
        }



        #region Additional Director Count Report

        [HttpGet]
        [AuthorizeAdmin(17)]
        public ActionResult ProcessCountReport_AD()
        {
            ViewBag.Application = objComDB.GetDropDownList(48, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;

            ViewBag.District = objComDB.GetDropDownList(7, 34).Where(m => (lstCMODistrict.Any(p => p.districtId == m.Id))).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ReportsModel model = new ReportsModel();

            return View(model);
        }

        [HttpPost]
        [AuthorizeAdmin(17)]
        public ActionResult ProcessCountReport_AD(ReportsModel model)
        {
            string dist = "";

            if (model.DistrictIds != null)
            {
                foreach (var v in model.DistrictIds)
                {
                    dist = dist + v + ",";
                }
            }

            TempData["DistrictIds"] = dist;

            switch (model.appTypeId)
            {
                case 0: return RedirectToAction("ProcessCountReportCMoffice", new { model.fromDate, model.toDate });
                    break;
                case 1: return RedirectToAction("ProcessCountReportNUH", new { model.fromDate, model.toDate });
                    break;
                case 2: return RedirectToAction("ProcessCountReportFAP", new { model.fromDate, model.toDate });
                    break;
                case 3: return RedirectToAction("ProcessCountReportDIC", new { model.fromDate, model.toDate });
                    break;
                case 4: return RedirectToAction("ProcessCountReportAGC", new { model.fromDate, model.toDate });
                    break;
                case 5: return RedirectToAction("ProcessCountReportMER", new { model.fromDate, model.toDate });
                    break;
                case 12: return RedirectToAction("ProcessCountReportALL", new { model.fromDate, model.toDate });
                    break;
                default: return RedirectToAction("UnauthoriseAcess", "Home");
                    break;
            }

            ViewBag.Application = objComDB.GetDropDownList(47, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;

            ViewBag.District = objComDB.GetDropDownList(7, 34).Where(m => (lstCMODistrict.Any(p => p.districtId == m.Id))).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            return View();
        }

        [HttpGet]
        [AuthorizeAdmin(18)]
        public ActionResult ProcessCountReportCMSatAD()
        {
            //ViewBag.Application = objComDB.GetDropDownList(58, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            var lstCMODistrict = Session["CMODistricts"] as List<CMODistrictModel>;
            ViewBag.forwardTypes = objComDB.GetDropDownList(59, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.District = objComDB.GetDropDownList(7, 34).Where(m => (lstCMODistrict.Any(p => p.districtId == m.Id))).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
            ViewBag.Application = Enumerable.Empty<SelectListItem>();
            ReportsModel model = new ReportsModel();

            return View(model);
        }

        [HttpPost]
        [AuthorizeAdmin(18)]
        public ActionResult ProcessCountReportCMSatAD(ReportsModel model)
        {
            int id = model.appTypeId;
            TempData["RedirectFromAction"] = "";
            string dist = "";

            if (model.DistrictIds != null)
            {
                foreach (var v in model.DistrictIds)
                {
                    dist = dist + v + ",";
                }
            }

            TempData["DistrictIds"] = dist;
            TempData["HealthUnitId"] = model.forwardtypeId;

            switch (id)
            {
                case 2: return RedirectToAction("ProcessCountReportFAPCMS", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 5: return RedirectToAction("ProcessCountReportMERCMS", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 6: return RedirectToAction("ProcessCountReportFIC", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 7: return RedirectToAction("ProcessCountReportILC", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 8: return RedirectToAction("ProcessCountReportDEC", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 9: return RedirectToAction("ProcessCountReportICC", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 10: return RedirectToAction("ProcessCountReportIMC", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 11: return RedirectToAction("ProcessCountReportMLC", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 12: return RedirectToAction("ProcessCountReportALLCMS", new { model.forwardtypeName, model.fromDate, model.toDate });
                    break;
                default: return RedirectToAction("UnauthoriseAcess", "Home");
                    break;
            }

            return View();
        }

        #endregion

        #region CMS Process Count Report

        [HttpGet]
        [AuthorizeAdmin(19)]
        public ActionResult ProcessCountReportCMS()
        {
            if (SM.RollID == 5)
            {
                ViewBag.Application = objComDB.GetDropDownList(57, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            }
            else
            {
                ViewBag.Application = objComDB.GetDropDownList(57, 0).Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() }).Where(m => m.Value != "5").ToList();
            }
            return View();
        }

        [HttpPost]
        [AuthorizeAdmin(19)]
        public ActionResult ProcessCountReportCMS(ReportsModel model)
        {
            int id = model.appTypeId;
            string forwardtypeName = "";

            TempData["RedirectFromAction"] = "";

            switch (id)
            {
                case 2: return RedirectToAction("ProcessCountReportFAPCMS", new { forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 5: return RedirectToAction("ProcessCountReportMERCMS", new { forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 6: return RedirectToAction("ProcessCountReportFIC", new { forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 7: return RedirectToAction("ProcessCountReportILC", new { forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 8: return RedirectToAction("ProcessCountReportDEC", new { forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 9: return RedirectToAction("ProcessCountReportICC", new { forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 10: return RedirectToAction("ProcessCountReportIMC", new { forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 11: return RedirectToAction("ProcessCountReportMLC", new { forwardtypeName, model.fromDate, model.toDate });
                    break;
                case 12: return RedirectToAction("ProcessCountReportALLCMS", new { forwardtypeName, model.fromDate, model.toDate });
                    break;
                default: return RedirectToAction("UnauthoriseAcess", "Home");
                    break;
            }

            return View();
        }

        public ActionResult ProcessCountReportFAPCMS(string forwardtypeName, string fromdate, string todate)
        {
            if (TempData["RedirectFromAction"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            long userid = 0;
            ViewBag.Division = SM.DisplayName;
            ViewBag.DistrictName = SM.DistrictName;
            ViewBag.RollAbbrName = SM.RollAbbrName;
            ViewBag.forwardtypeName = forwardtypeName;

            string dist = !string.IsNullOrEmpty(Convert.ToString(TempData["DistrictIds"])) ? TempData["DistrictIds"].ToString() : "";
            int HealthUnitId = !string.IsNullOrEmpty(Convert.ToString(TempData["HealthUnitId"])) ? Convert.ToInt32(TempData["HealthUnitId"]) : 0;

            if (SM.RollID == 3 || SM.RollID == 4 || SM.RollID == 5)
            {
                TempData["RollID"] = "CHC/PHC/DH";
                userid = SM.UserID;
                var result = ObjRptDb.ProcessCountReportforCMS(2, userid, fromdate, todate);
                return View(result);
            }
            else if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                ViewBag.Division = ", Division - " + SM.DisplayName;
                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(2, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);
            }
            else if (SM.RollID == 8)
            {
                TempData["RollID"] = "admin";

                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(2, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);
            }

            return View();
        }

        public ActionResult ProcessCountReportMERCMS(string forwardtypeName, string fromdate, string todate)
        {
            if (TempData["RedirectFromAction"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            long userid = 0;
            ViewBag.Division = SM.DisplayName;
            ViewBag.DistrictName = SM.DistrictName;
            ViewBag.RollAbbrName = SM.RollAbbrName;
            ViewBag.forwardtypeName = forwardtypeName;

            string dist = !string.IsNullOrEmpty(Convert.ToString(TempData["DistrictIds"])) ? TempData["DistrictIds"].ToString() : "";
            int HealthUnitId = !string.IsNullOrEmpty(Convert.ToString(TempData["HealthUnitId"])) ? Convert.ToInt32(TempData["HealthUnitId"]) : 0;

            if (SM.RollID == 3 || SM.RollID == 4 || SM.RollID == 5)//
            {
                TempData["RollID"] = "CHC/PHC/DH";
                userid = SM.UserID;
                var result = ObjRptDb.ProcessCountReportforCMS(5, userid, fromdate, todate);
                return View(result);
            }
            else if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                ViewBag.Division = ", Division - " + SM.DisplayName;
                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(5, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);
            }
            else if (SM.RollID == 8)//
            {
                TempData["RollID"] = "admin";
                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(5, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);


            }
            //var result = ObjRptDb.ProcessCountReportforCMS(6, userid, HealthUnitId, dist, fromdate, todate);
            return View();
        }

        public ActionResult ProcessCountReportFIC(string forwardtypeName, string fromdate, string todate)
        {
            if (TempData["RedirectFromAction"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            long userid = 0;
            ViewBag.Division = SM.DisplayName;
            ViewBag.DistrictName = SM.DistrictName;
            ViewBag.RollAbbrName = SM.RollAbbrName;
            ViewBag.forwardtypeName = forwardtypeName;

            string dist = !string.IsNullOrEmpty(Convert.ToString(TempData["DistrictIds"])) ? TempData["DistrictIds"].ToString() : "";
            int HealthUnitId = !string.IsNullOrEmpty(Convert.ToString(TempData["HealthUnitId"])) ? Convert.ToInt32(TempData["HealthUnitId"]) : 0;

            if (SM.RollID == 3 || SM.RollID == 4 || SM.RollID == 5)
            {
                TempData["RollID"] = "CHC/PHC/DH";
                userid = SM.UserID;
                var result = ObjRptDb.ProcessCountReportforCMS(6, userid, fromdate, todate);
                return View(result);
            }
            else if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                ViewBag.Division = ", Division - " + SM.DisplayName;
                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(6, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);
            }
            else if (SM.RollID == 8)
            {
                TempData["RollID"] = "admin";
                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(6, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);


            }
            //var result = ObjRptDb.ProcessCountReportforCMS(6, userid, HealthUnitId, dist, fromdate, todate);
            return View();
        }

        public ActionResult ProcessCountReportILC(string forwardtypeName, string fromdate, string todate)
        {
            if (TempData["RedirectFromAction"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            long userid = 0;
            ViewBag.Division = SM.DisplayName;
            ViewBag.DistrictName = SM.DistrictName;
            ViewBag.RollAbbrName = SM.RollAbbrName;
            ViewBag.forwardtypeName = forwardtypeName;

            string dist = !string.IsNullOrEmpty(Convert.ToString(TempData["DistrictIds"])) ? TempData["DistrictIds"].ToString() : "";
            int HealthUnitId = !string.IsNullOrEmpty(Convert.ToString(TempData["HealthUnitId"])) ? Convert.ToInt32(TempData["HealthUnitId"]) : 0;

            if (SM.RollID == 3 || SM.RollID == 4 || SM.RollID == 5)
            {
                TempData["RollID"] = "CHC/PHC/DH";
                userid = SM.UserID;
                var result = ObjRptDb.ProcessCountReportforCMS(7, userid, fromdate, todate);
                return View(result);

            }
            else if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                ViewBag.Division = ", Division - " + SM.DisplayName;
                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(7, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);
            }
            else if (SM.RollID == 8)
            {
                TempData["RollID"] = "admin";
                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(7, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);
            }

            return View();
        }

        public ActionResult ProcessCountReportDEC(string forwardtypeName, string fromdate, string todate)
        {
            if (TempData["RedirectFromAction"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            long userid = 0;
            ViewBag.Division = SM.DisplayName;
            ViewBag.DistrictName = SM.DistrictName;
            ViewBag.RollAbbrName = SM.RollAbbrName;
            ViewBag.forwardtypeName = forwardtypeName;

            string dist = !string.IsNullOrEmpty(Convert.ToString(TempData["DistrictIds"])) ? TempData["DistrictIds"].ToString() : "";
            int HealthUnitId = !string.IsNullOrEmpty(Convert.ToString(TempData["HealthUnitId"])) ? Convert.ToInt32(TempData["HealthUnitId"]) : 0;

            if (SM.RollID == 3 || SM.RollID == 4 || SM.RollID == 5)
            {
                TempData["RollID"] = "CHC/PHC/DH";
                userid = SM.UserID;
                var result = ObjRptDb.ProcessCountReportforCMS(8, userid, fromdate, todate);
                return View(result);
            }
            else if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                ViewBag.Division = ", Division - " + SM.DisplayName;
                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(8, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);
            }
            else if (SM.RollID == 8)
            {
                TempData["RollID"] = "admin";

                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(8, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);
            }

            return View();
        }

        public ActionResult ProcessCountReportICC(string forwardtypeName, string fromdate, string todate)
        {
            if (TempData["RedirectFromAction"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            long userid = 0;
            ViewBag.Division = SM.DisplayName;
            ViewBag.DistrictName = SM.DistrictName;
            ViewBag.RollAbbrName = SM.RollAbbrName;
            ViewBag.forwardtypeName = forwardtypeName;

            string dist = !string.IsNullOrEmpty(Convert.ToString(TempData["DistrictIds"])) ? TempData["DistrictIds"].ToString() : "";
            int HealthUnitId = !string.IsNullOrEmpty(Convert.ToString(TempData["HealthUnitId"])) ? Convert.ToInt32(TempData["HealthUnitId"]) : 0;

            if (SM.RollID == 3 || SM.RollID == 4 || SM.RollID == 5)
            {
                TempData["RollID"] = "CHC/PHC/DH";
                userid = SM.UserID;
                var result = ObjRptDb.ProcessCountReportforCMS(9, userid, fromdate, todate);
                return View(result);
            }
            else if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                ViewBag.Division = ", Division - " + SM.DisplayName;
                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(9, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);
            }
            else if (SM.RollID == 8)
            {
                TempData["RollID"] = "admin";

                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(9, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);
            }

            return View();
        }

        public ActionResult ProcessCountReportIMC(string forwardtypeName, string fromdate, string todate)
        {
            if (TempData["RedirectFromAction"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            long userid = 0;
            ViewBag.Division = SM.DisplayName;
            ViewBag.DistrictName = SM.DistrictName;
            ViewBag.RollAbbrName = SM.RollAbbrName;
            ViewBag.forwardtypeName = forwardtypeName;

            string dist = !string.IsNullOrEmpty(Convert.ToString(TempData["DistrictIds"])) ? TempData["DistrictIds"].ToString() : "";
            int HealthUnitId = !string.IsNullOrEmpty(Convert.ToString(TempData["HealthUnitId"])) ? Convert.ToInt32(TempData["HealthUnitId"]) : 0;

            if (SM.RollID == 3 || SM.RollID == 4 || SM.RollID == 5)
            {
                TempData["RollID"] = "CHC/PHC/DH";
                userid = SM.UserID;
                var result = ObjRptDb.ProcessCountReportforCMS(10, userid, fromdate, todate);
                return View(result);
            }
            else if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                ViewBag.Division = ", Division - " + SM.DisplayName;
                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(10, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);
            }
            else if (SM.RollID == 8)
            {
                TempData["RollID"] = "admin";

                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(10, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);
            }

            return View();
        }

        public ActionResult ProcessCountReportMLC(string forwardtypeName, string fromdate, string todate)
        {
            if (TempData["RedirectFromAction"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            long userid = 0;
            ViewBag.Division = SM.DisplayName;
            ViewBag.DistrictName = SM.DistrictName;
            ViewBag.RollAbbrName = SM.RollAbbrName;
            ViewBag.forwardtypeName = forwardtypeName;

            string dist = !string.IsNullOrEmpty(Convert.ToString(TempData["DistrictIds"])) ? TempData["DistrictIds"].ToString() : "";
            int HealthUnitId = !string.IsNullOrEmpty(Convert.ToString(TempData["HealthUnitId"])) ? Convert.ToInt32(TempData["HealthUnitId"]) : 0;

            if (SM.RollID == 3 || SM.RollID == 4 || SM.RollID == 5)
            {
                TempData["RollID"] = "CHC/PHC/DH";
                userid = SM.UserID;
                var result = ObjRptDb.ProcessCountReportforCMS(11, userid, fromdate, todate);
                return View(result);
            }
            else if (SM.RollID == 18)
            {
                TempData["RollID"] = "ADM";
                ViewBag.Division = ", Division - " + SM.DisplayName;
                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(11, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);
            }
            else if (SM.RollID == 8)
            {
                TempData["RollID"] = "admin";

                var result = ObjRptDb.ProcessCountReportforCMS_DistrictWise(11, userid, HealthUnitId, dist, fromdate, todate);
                return View(result);
            }

            return View();
        }

        public ActionResult ProcessCountReportALLCMS(string forwardtypeName, string fromdate, string todate)//cmo
        {
            if (TempData["RedirectFromAction"] == null)
            {
                return RedirectToAction("UnauthoriseAcess", "Home");
            }

            long userid = 0;

            ViewBag.DistrictName = SM.DistrictName;
            ViewBag.RollAbbrName = SM.RollAbbrName;

            if (SM.RollID == 3 || SM.RollID == 4 || SM.RollID == 5)
            {
                TempData["RollID"] = "CHC/PHC/DH";
                userid = SM.UserID;
                ViewBag.Division = SM.DisplayName;
            }

            var result = ObjRptDb.ProcessCountReportforCMS(12, userid, fromdate, todate);

            return View(result);
        }

        #endregion

        #region Drill Down Report for Admin/DGFW

        [AuthorizeAdmin(20)]
        public ActionResult CMOSRVCountReport(string rollId = "")
        {
            if (!string.IsNullOrEmpty(rollId))
            {
                if (SM.RollID == 18)
                {
                    ViewBag.ZoneId = SM.ZoneId;
                }
                else
                {
                    ViewBag.ZoneId = 0;
                }
                ViewBag.DLLZone = objComDB.GetZoneForDLL().ToList();
                ViewBag.RollId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(rollId));
                long roo = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(rollId));
                if (ViewBag.RollId == 2)
                {
                    ViewBag.RollName = "CMO";
                    ViewBag.RollNameHi = "सी.एम.ओ.";
                    ViewBag.FullRollName = "Chief Medical Officer";
                }
                else if (ViewBag.RollId == 3)
                {
                    ViewBag.RollName = "CHC";
                    ViewBag.RollNameHi = "सी.एच.सी.";
                    ViewBag.FullRollName = "Community Health Center";
                }
                else if (ViewBag.RollId == 4)
                {
                    ViewBag.RollName = "PHC";
                    ViewBag.RollNameHi = "पी.एच.सी.";
                    ViewBag.FullRollName = "Primary Health Centre";
                }
                else if (ViewBag.RollId == 5)
                {
                    ViewBag.RollName = "CMS(DH)";
                    ViewBag.RollNameHi = "सी.एम.एस(डी.एच)";
                    ViewBag.FullRollName = "Chief Medical Superintendent (District Hospital)";
                }
                return View();
            }
            else
            {
                return RedirectToAction("ApplicationReport");
            }
        }

        [AuthorizeAdmin(20)]
        public ActionResult CMOSRVCountReportList(long rollId = 0, int zoneId = 0)
        {
            if (zoneId == 0)
            {
                List<CountReportModel> list = ObjRptDb.GetTotalServiceCount(0, rollId, SM.RollID);
                ViewData["TotalCountList"] = list;
            }
            else
            {

                ViewData["TotalCountList"] = null;
            }

            var result = ObjRptDb.CMOSRVCountReport(zoneId, rollId, SM.RollID);
            return PartialView("_CMOSRVCountReportList", result);
        }

        [AuthorizeAdmin(20)]
        public ActionResult CMOSRVCountReportDivisionWise(string zoneId = "", string serviceId = "")
        {
            ViewBag.ZoneId = zoneId;
            if (!string.IsNullOrEmpty(serviceId))
            {
                ViewBag.ServiceId = serviceId;
            }
            return View();
        }

        [AuthorizeAdmin(20)]
        public ActionResult CMOSRVCountReportDivisionWiseList(string zoneId = "", string serviceId = "")
        {
            int srvId = serviceId != "" ? Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(serviceId)) : 0;
            ViewBag.ServiceId = srvId;
            var result = ObjRptDb.CMOSRVCountReport_DivisionWise(Convert.ToInt32(OTPL_Imp.CustomCryptography.Decrypt(zoneId)), 0, srvId);
            return PartialView("_CMOSRVCountReportDivisionWiseList", result);
        }

        [AuthorizeAdmin(20)]
        public ActionResult ApplicationDetails(string rollId = "", string appCurrStatus = "", string zoneId = "", string districtId = "", string serviceId = "", string isLessFiftyThousan = "", string userId = "")
        {
            ViewBag.CurrLogRollId = SM.RollID;

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
            Session["dataDistrictServiceApplication"] = result;
            var resultData = result.Where(m => (m.appCurrStatus == appCS || appCS == 0)).Select(m => new { m.serviceName, m.zoneName, m.DistrictName, m.appCurrStatus, m.serviceId }).FirstOrDefault();

            model.serviceId = resultData.serviceId;
            model.serviceName = resultData.serviceName;
            model.zoneName = resultData.zoneName;
            model.DistrictName = resultData.DistrictName;
            model.appCurrStatus = appCS;

            model.appDetailList = result;

            return View(model);
        }


        [AuthorizeAdmin(20)]
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

        [AuthorizeAdmin(20)]
        public ActionResult SRVCountReportDivisionWiseList(long rollId = 0, int zoneId = 0, int serviceId = 0)
        {
            ViewBag.ServiceId = serviceId;
            var result = ObjRptDb.SRVCountReport_DivisionWise(rollId, zoneId, 0, serviceId);
            return PartialView("_SRVCountReportDivisionWiseList", result);
        }

        [AuthorizeAdmin(20)]
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

        [AuthorizeAdmin(20)]
        public ActionResult SRVCountReportDistrictWiseList(long rollId = 0, int districtId = 0, int serviceId = 0)
        {
            ViewBag.ServiceId = serviceId;
            var result = ObjRptDb.SRVCountReport_DistrictWise(rollId, districtId, serviceId);
            return PartialView("_SRVCountReportDistrictWiseList", result);
        }

        #endregion

        #region CMO Drill Down Report

        [AuthorizeAdmin(21)]
        public ActionResult CMOReportDashboard()
        {
            return View();
        }

        [AuthorizeAdmin(21)]
        public ActionResult CMOSRVCountReportForCMO(string rollId = "")
        {
            if (!string.IsNullOrEmpty(rollId))
            {

                ViewBag.RollId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(rollId));
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
                ViewBag.RollId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(rollId));
                ViewBag.DistrictId = SM.districtId;
                return View();
            }
            else
            {
                return RedirectToAction("CMOReportDashboard");
            }
        }

        [AuthorizeAdmin(21)]
        public ActionResult CMOSRVCountReportListForCMO(long rollId = 0, int districtId = 0, int serviceId = 0)
        {
            ViewBag.ServiceId = serviceId;
            var result = ObjRptDb.SRVCountReport_DivisionWiseForCMO(rollId, districtId, serviceId);
            return PartialView("_SRVCountReportDivisionWiseListForCMO", result);
        }

        [AuthorizeAdmin(21)]
        public ActionResult SRVCountReportDistrictWiseForCMO(string rollId = "", string districtId = "", string serviceId = "")
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

        [AuthorizeAdmin(21)]
        public ActionResult SRVCountReportDistrictWiseListForCMO(long rollId = 0, int districtId = 0, int serviceId = 0)
        {
            ViewBag.ServiceId = serviceId;
            var result = ObjRptDb.SRVCountReport_DistrictWiseForCMO(rollId, districtId, serviceId);
            return PartialView("_SRVCountReportDistrictWiseListForCMO", result);
        }

        #endregion

        //=====================================================================================================================================
        #endregion

        [AuthorizeAdmin(20)]
        public ActionResult CMOSRVDistrictWiseCountReport(string rollId = "")
        {
            if (!string.IsNullOrEmpty(rollId))
            {
                if (SM.RollID == 18)
                {
                    ViewBag.ZoneId = SM.ZoneId;
                }
                else
                {
                    ViewBag.ZoneId = 0;
                }
                ViewBag.DLLZone = objComDB.GetZoneForDLL().ToList();
                ViewBag.RollId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(rollId));

                if (ViewBag.RollId == 2)
                {
                    ViewBag.RollName = "CMO";
                    ViewBag.RollNameHi = "सी.एम.ओ.";
                    ViewBag.FullRollName = "Chief Medical Officer";
                }
                else if (ViewBag.RollId == 3)
                {
                    ViewBag.RollName = "CHC";
                    ViewBag.RollNameHi = "सी.एच.सी.";
                    ViewBag.FullRollName = "Community Health Center";
                }
                else if (ViewBag.RollId == 4)
                {
                    ViewBag.RollName = "PHC";
                    ViewBag.RollNameHi = "पी.एच.सी.";
                    ViewBag.FullRollName = "Primary Health Centre";
                }
                else if (ViewBag.RollId == 5)
                {
                    ViewBag.RollName = "CMS(DH)";
                    ViewBag.RollNameHi = "सी.एम.एस(डी.एच)";
                    ViewBag.FullRollName = "Chief Medical Superintendent (District Hospital)";
                }

                return View();
            }
            else
            {
                return RedirectToAction("ApplicationReport");
            }
        }

        [AuthorizeAdmin(20)]
        public ActionResult CMOSRVDistrictWiseCountReportList(long rollId = 0, int zoneId = 0)
        {
            var result = ObjRptDb.CMOSRVDistrictWiseCountReport(zoneId, rollId, SM.RollID);
            Session["data"] = result;
            return PartialView("_CMOSRVDistrictWiseCountReportList", result);
        }

        public ActionResult ExportToExcel()
        {
            string filename = "";
            if (Session["data"] != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Sr.No.", typeof(string));
                dt.Columns.Add("Division Name", typeof(string));
                dt.Columns.Add("District Name", typeof(string));
                dt.Columns.Add("Service Name", typeof(string));
                dt.Columns.Add("Total Recieved", typeof(string));
                dt.Columns.Add("Approved", typeof(string));
                dt.Columns.Add("Rejected", typeof(string));
                dt.Columns.Add("Pending In-Time Limit", typeof(string));
                dt.Columns.Add("Pending After-Time Limit", typeof(string));

                int i = 1;
                foreach (var res in (Session["data"] as List<CountReportModel>))
                {
                    if (i == 1)
                    {
                        filename = "DivisionDistrictWiseServiceCountReport.xls";
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    dr["Sr.No."] = i;
                    dr["Division Name"] = res.zoneName;
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
                string headerTable = @"<Table border=1><tr rowspan=2 align=center><td colspan=9><h3> Department of Medical Health And Family Welfare</h3></td></tr><tr rowspan=2 align=center><td colspan=9><h4>CMO Service Division District Wise Count Report</h4> </td></tr></Table>";
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
                return RedirectToAction("CMOSRVDistrictWiseCountReport", new { rollId = "" });
            }
        }


        //==============================================================================================================================================

        // work on Shyam



        [AuthorizeAdmin]
        public ActionResult CMOSRVCountReportDistrictSeviceWise(string rollId = "")
        {
            if (!string.IsNullOrEmpty(rollId))
            {
                string District = "0";
                if (SM.RollID == 2 || SM.RollID == 3 || SM.RollID == 4 || SM.RollID == 5 || SM.RollID == 6)
                {
                    ViewBag.districtId = SM.districtId;
                    District = SM.districtId.ToString();
                }
                else
                {
                    ViewBag.districtId = 0;
                    District = "0";
                }
                //ViewBag.DLLZone = objComDB.GetZoneForDLL().ToList();
                ViewBag.DLLServices = objComDB.GetSevicesForDLL().ToList();


                if (District != "0")
                {
                    ViewBag.DLLDistrict = objComDB.GetDistrictForDLL().Where(m => m.Value == District).Select(m => new SelectListItem { Text = m.Text, Value = m.Value.ToString() });
                }
                else
                {
                    ViewBag.DLLDistrict = objComDB.GetDistrictForDLL().ToList();
                }
                ViewBag.RollId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(rollId));
                ViewBag.roll = SM.RollID;
                if (SM.RollID == 8)
                {
                    ViewBag.RollName = "All";
                    ViewBag.RollNameHi = "समस्त";
                    ViewBag.FullRollName = "Admin";
                    ViewBag.useid = 0;
                }
                else if (SM.RollID == 2)
                {
                    ViewBag.RollName = "CMO";
                    ViewBag.RollNameHi = "सी.एम.ओ.";
                    ViewBag.FullRollName = "Chief Medical Officer";
                    ViewBag.useid = 0;
                }
                else if (SM.RollID == 3)
                {
                    ViewBag.RollName = "CHC";
                    ViewBag.RollNameHi = "सी.एच.सी.";
                    ViewBag.FullRollName = "Community Health Center";
                    ViewBag.useid = SM.UserID;
                }
                else if (SM.RollID == 4)
                {
                    ViewBag.RollName = "PHC";
                    ViewBag.RollNameHi = "पी.एच.सी.";
                    ViewBag.FullRollName = "Primary Health Centre";
                    ViewBag.useid = SM.UserID;
                }
                else if (SM.RollID == 5)
                {
                    ViewBag.RollName = "CMS(DH)";
                    ViewBag.RollNameHi = "सी.एम.एस(डी.एच)";
                    ViewBag.FullRollName = "Chief Medical Superintendent (District Hospital)";
                    ViewBag.useid = SM.UserID;
                }
                return View();
            }
            else
            {
                return RedirectToAction("ApplicationReport");
            }
        }


        [AuthorizeAdmin]
        public ActionResult CMOSRVCountReportDistrictServiceList(long rollId = 0, int DistrictId = 0, int ServiceId = 0, string fromDate = "", string toDate = "", long Userid = 0)
        {
            if (fromDate == "" && toDate == "")
            {
                fromDate = "01/01/2018";
                toDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            int roll = 0;
            if (SM.RollID == 2 || SM.RollID == 8)
            {
                roll = 0;
            }
            else
            {
                roll = Convert.ToInt32(SM.RollID);
            }

            var result = ObjRptDb.CMOSRVCountReportDistrictServicesWise(DistrictId, ServiceId, roll, roll, fromDate, toDate, Userid);
            Session["fromDate"] = fromDate;
            Session["toDate"] = toDate;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            Session["dataDistrictService"] = result;
            return PartialView("_CMOSRVDistrictServiceWiseCountReportList", result);
        }

        public ActionResult CMOSRVCountReportDistrictServiceListCMOCHCPHC(string rollId = "", string appCurrStatus = "", string districtId = "", string serviceId = "", string userId = "", string fromDate = "", string toDate = "")
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
            long UserId = 0;
            if (SM.RollID == 3 || SM.RollID == 4 || SM.RollID == 5 || SM.RollID == 6)
            {
                UserId = SM.UserID;
            }
            else
            {
                UserId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(userId));
            }
            long RollId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(rollId));
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


        [AuthorizeAdmin]
        public ActionResult ApplicationDetailsDIstrictServices(string rollId = "", string appCurrStatus = "", string zoneId = "", string districtId = "", string serviceId = "", string isLessFiftyThousan = "", string userId = "", string fromDate = "", string toDate = "")
        {
            ViewBag.CurrLogRollId = SM.RollID;
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
            long uId = 0;
            if (SM.RollID == 3 || SM.RollID == 4 || SM.RollID == 5 || SM.RollID == 6)
            {
                uId = SM.UserID;
            }
            else
            {
                uId = Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(userId));
            }
            // long uId = userId != "" ? Convert.ToInt64(OTPL_Imp.CustomCryptography.Decrypt(userId)) : 0;

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
                dt.Columns.Add("Name", typeof(string));

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
                    dr["Name"] = res.opdAdd;

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
    }
}
