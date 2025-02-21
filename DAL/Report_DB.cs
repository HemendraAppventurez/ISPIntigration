using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCSHealthFamilyWelfareDept.Models;
using System.Data;



namespace CCSHealthFamilyWelfareDept.DAL
{
    public class Report_DB : DbContext
    {
        #region Default Constructor
        public Report_DB()
            : base("CMSModule")
        { }
        #endregion





        #region Vinod Kumar

        public List<string> GetInspectionPhotoPath(int ProcId, long regisNUHId, string ComettiType)
        {
            var sqlParam = new SqlParameter[] { 
           
                new SqlParameter{ParameterName="@ProcId",Value=ProcId} , 
             new SqlParameter{ParameterName="@RegistrationId",Value=regisNUHId} ,
             new SqlParameter{ParameterName="@ComettiType",Value=ComettiType??string.Empty} ,
             
            };
            var _proc = @"Proc_GetInspectionPhotoPath @ProcId,@RegistrationId,@ComettiType";
            var slist = this.Database.SqlQuery<string>(_proc, sqlParam).ToList();
            return slist;
        }

        public List<ApplicationWorkFlowStepStatusModel> GetAppWorkflowStatusNUH(long regisID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@ProcId",Value=1}  ,
             new SqlParameter{ParameterName="@RegistrationId",Value=regisID} 
            };
            var _proc = @"Proc_GetApplicationStatusByRegisNo @ProcId,@RegistrationId";
            var slist = this.Database.SqlQuery<ApplicationWorkFlowStepStatusModel>(_proc, sqlParam).ToList();
            return slist;
        }


        public List<ApplicationWorkFlowStepStatusModel> GetAppWorkflowStatusFAP(long regisID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@ProcId",Value=2}  ,
             new SqlParameter{ParameterName="@RegistrationId",Value=regisID} 
            };
            var _proc = @"Proc_GetApplicationStatusByRegisNo @ProcId,@RegistrationId";
            var slist = this.Database.SqlQuery<ApplicationWorkFlowStepStatusModel>(_proc, sqlParam).ToList();
            return slist;
        }


        public List<ApplicationWorkFlowStepStatusModel> GetAppWorkflowStatusDIC(long regisID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@ProcId",Value=3}  ,
             new SqlParameter{ParameterName="@RegistrationId",Value=regisID} 
            };
            var _proc = @"Proc_GetApplicationStatusByRegisNo @ProcId,@RegistrationId";
            var slist = this.Database.SqlQuery<ApplicationWorkFlowStepStatusModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public List<ApplicationWorkFlowStepStatusModel> GetAppWorkflowStatusAGC(long regisID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@ProcId",Value=4}  ,
             new SqlParameter{ParameterName="@RegistrationId",Value=regisID} 
            };
            var _proc = @"Proc_GetApplicationStatusByRegisNo @ProcId,@RegistrationId";
            var slist = this.Database.SqlQuery<ApplicationWorkFlowStepStatusModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public List<ApplicationWorkFlowStepStatusModel> GetAppWorkflowStatusMER(long regisID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@ProcId",Value=5}  ,
             new SqlParameter{ParameterName="@RegistrationId",Value=regisID} 
            };
            var _proc = @"Proc_GetApplicationStatusByRegisNo @ProcId,@RegistrationId";
            var slist = this.Database.SqlQuery<ApplicationWorkFlowStepStatusModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public List<ApplicationStatusReportDetailsModel> GetAllApplicationCountList(string ReportName, string dist, string FromDate, string ToDate, Int64 AppTypeID)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@action",Value=ReportName},
                          new SqlParameter {ParameterName="@dist",Value=dist},
                           new SqlParameter {ParameterName="@fromdate",Value=FromDate},
                            new SqlParameter {ParameterName="@todate",Value=ToDate},
                            new SqlParameter {ParameterName="@AppTypeID",Value=AppTypeID}
              };
            var _proc = @"Proc_GetAllCountApplicationList @action,@dist,@fromdate,@todate,@AppTypeID";
            var res = this.Database.SqlQuery<ApplicationStatusReportDetailsModel>(_proc, sqlparams).ToList();
            return res;
        }

        #endregion


        #region RIYA
        public List<ReportsModel> ProcessCountReportCMO(int id, string dist, string fromdate, string todate)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@id",Value=id},
                          new SqlParameter {ParameterName="@dist",Value=dist},
                          new SqlParameter {ParameterName="@fromdate",Value=fromdate},
                              new SqlParameter {ParameterName="@todate",Value=todate}
              };
            var _proc = @"CMO_RPT  @id,@dist,@fromdate,@todate";
            var res = this.Database.SqlQuery<ReportsModel>(_proc, sqlparams).ToList();
            return res;
        }
        #region 
        public List<ReportsModel> ProcessCountReportCMODrillDown(int id, string dist, string fromdate, string todate)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@id",Value=id},
                          new SqlParameter {ParameterName="@dist",Value=dist},
                          new SqlParameter {ParameterName="@fromdate",Value=fromdate},
                              new SqlParameter {ParameterName="@todate",Value=todate}
              };
            var _proc = @"CMO_RPTForCMO  @id,@dist,@fromdate,@todate";
            var res = this.Database.SqlQuery<ReportsModel>(_proc, sqlparams).ToList();
            return res;
        }
        #endregion
        public List<ReportsModel> ProcessCountReportforCMS(int id, long userId, string fromdate, string todate)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@id",Value=id},
                          new SqlParameter {ParameterName="@area",Value=userId},
                          
                          new SqlParameter {ParameterName="@fromdate",Value=fromdate},
                              new SqlParameter {ParameterName="@todate",Value=todate}
              };
            var _proc = @"CMS_Rpt  @id,@area,@fromdate,@todate";
            var res = this.Database.SqlQuery<ReportsModel>(_proc, sqlparams).ToList();
            return res;
        }
        public List<ReportsModel> ProcessCountReportforCMS_DistrictWise(int id, long userId, int HealthUnitId, string dist, string fromdate, string todate)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@id",Value=id},
                          new SqlParameter {ParameterName="@area",Value=userId},
                           new SqlParameter {ParameterName="@HealthUnitId",Value=HealthUnitId},
                          new SqlParameter {ParameterName="@dist",Value=dist},
                          new SqlParameter {ParameterName="@fromdate",Value=fromdate},
                              new SqlParameter {ParameterName="@todate",Value=todate}
              };
            var _proc = @"CMS_Rpt_distWise @id,@area,@HealthUnitId,@dist,@fromdate,@todate";
            var res = this.Database.SqlQuery<ReportsModel>(_proc, sqlparams).ToList();
            return res;
        }
        #endregion

        public List<CountReportModel> CMOSRVCountReport(int zoneId, long rollId, long logedInRollId)
        {
            var sqlparams = new SqlParameter[]{ 
                          new SqlParameter {ParameterName="@zoneId",Value=zoneId},
                          new SqlParameter {ParameterName="@rollId",Value=rollId},
                          new SqlParameter {ParameterName="@logedInRollId",Value=logedInRollId}
              };
            var _proc = @"proc_CMOSRVCountReport @zoneId,@rollId,@logedInRollId";
            var res = this.Database.SqlQuery<CountReportModel>(_proc, sqlparams).ToList();
            return res;
        }



        public List<CountReportModel> GetTotalServiceCount(int zoneId, long rollId, long logedInRollId)
        {
            var sqlparams = new SqlParameter[]{ 
                          new SqlParameter {ParameterName="@zoneId",Value=zoneId},
                          new SqlParameter {ParameterName="@rollId",Value=rollId},
                          new SqlParameter {ParameterName="@logedInRollId",Value=logedInRollId}
              };
            var _proc = @"Proc_GetTotalServiceCount @zoneId,@rollId,@logedInRollId";
            var res = this.Database.SqlQuery<CountReportModel>(_proc, sqlparams).ToList();
            return res;
        }

        public List<CountReportModel> CMOSRVCountReport_DivisionWise(int zoneId, int districtId, int serviceId)
        {
            var sqlparams = new SqlParameter[]{ 
                          new SqlParameter {ParameterName="@zoneId",Value=zoneId},
                          new SqlParameter {ParameterName="@districtId",Value=districtId},
                          new SqlParameter {ParameterName="@serviceId",Value=serviceId}
              };
            var _proc = @"proc_CMOSRVCountReport_DivisionWise @zoneId,@districtId,@serviceId";
            var res = this.Database.SqlQuery<CountReportModel>(_proc, sqlparams).ToList();
            return res;
        }

        #region

        public List<CountReportModel> CMOSRVCountReport_DistrictWise( int serviceId)
        {
            var sqlparams = new SqlParameter[]{ 
                          new SqlParameter {ParameterName="@serviceId",Value=serviceId}
              };
            var _proc = @"proc_CMOSRVCountReport_DistrictWise @serviceId";
            var res = this.Database.SqlQuery<CountReportModel>(_proc, sqlparams).ToList();
            return res;
        }

        public List<ApplicationDetailsModel> GetApplicationDetailsForDarpan(int appCurrStatus, int zoneId, int districtId, int serviceId, int isLessFiftyThousan, long userId)
        {
            var sqlparams = new SqlParameter[]{ 
                          new SqlParameter {ParameterName="@appCurrStatus",Value=appCurrStatus},
                          new SqlParameter {ParameterName="@zoneId",Value=zoneId},
                          new SqlParameter {ParameterName="@districtId",Value=districtId},
                          new SqlParameter {ParameterName="@serviceId",Value=serviceId},
                          new SqlParameter {ParameterName="@isLessFiftyThousan",Value=isLessFiftyThousan},
                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_GetApplicationDetailsForDarpanView @appCurrStatus,@zoneId,@districtId,@serviceId,@isLessFiftyThousan,@userId";
            var res = this.Database.SqlQuery<ApplicationDetailsModel>(_proc, sqlparams).ToList();
            return res;
        }

        public List<ApplicationWorkFlowStepStatusModel> GetAppWorkflowStatusMERForDarpan(long regisID,int serviceId)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@ServiceId",Value=serviceId}  ,
             new SqlParameter{ParameterName="@RegistrationId",Value=regisID} 
            };
            var _proc = @"Proc_GetApplicationStatusByRegisNoForDarpan @ServiceId,@RegistrationId";
            var slist = this.Database.SqlQuery<ApplicationWorkFlowStepStatusModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public List<string> GetInspectionPhotoPathForDarpan(int ProcId, long regisNUHId, string ComettiType)
        {
            var sqlParam = new SqlParameter[] { 
           
                new SqlParameter{ParameterName="@ServiceId",Value=ProcId} , 
             new SqlParameter{ParameterName="@RegistrationId",Value=regisNUHId} ,
             new SqlParameter{ParameterName="@ComettiType",Value=ComettiType??string.Empty} ,
             
            };
            var _proc = @"Proc_GetInspectionPhotoPathForDarpan @ServiceId,@RegistrationId,@ComettiType";
            var slist = this.Database.SqlQuery<string>(_proc, sqlParam).ToList();
            return slist;
        }

        #endregion

        public List<ApplicationDetailsModel> GetApplicationDetails(int appCurrStatus, int zoneId, int districtId, int serviceId, int isLessFiftyThousan, long userId)
        {
            var sqlparams = new SqlParameter[]{ 
                          new SqlParameter {ParameterName="@appCurrStatus",Value=appCurrStatus},
                          new SqlParameter {ParameterName="@zoneId",Value=zoneId},
                          new SqlParameter {ParameterName="@districtId",Value=districtId},
                          new SqlParameter {ParameterName="@serviceId",Value=serviceId},
                          new SqlParameter {ParameterName="@isLessFiftyThousan",Value=isLessFiftyThousan},
                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_GetApplicationDetails @appCurrStatus,@zoneId,@districtId,@serviceId,@isLessFiftyThousan,@userId";
            var res = this.Database.SqlQuery<ApplicationDetailsModel>(_proc, sqlparams).ToList();
            return res;
        }

        public List<CountReportModel> SRVCountReport_DivisionWise(long rollId, int zoneId, int districtId, int serviceId)
        {
            var sqlparams = new SqlParameter[]{ 
                          new SqlParameter {ParameterName="@rollId",Value=rollId},
                          new SqlParameter {ParameterName="@zoneId",Value=zoneId},
                          new SqlParameter {ParameterName="@districtId",Value=districtId},
                          new SqlParameter {ParameterName="@serviceId",Value=serviceId}
              };
            var _proc = @"proc_SRVCountReport_DivisionWise @rollId,@zoneId,@districtId,@serviceId";
            var res = this.Database.SqlQuery<CountReportModel>(_proc, sqlparams).ToList();
            return res;
        }

        public List<CountReportModel> SRVCountReport_DistrictWise(long rollId, int districtId, int serviceId)
        {
            var sqlparams = new SqlParameter[]{ 
                          new SqlParameter {ParameterName="@rollId",Value=rollId},
                          new SqlParameter {ParameterName="@districtId",Value=districtId},
                          new SqlParameter {ParameterName="@serviceId",Value=serviceId}
              };
            var _proc = @"proc_SRVCountReport_DistrictWise @rollId,@districtId,@serviceId";
            var res = this.Database.SqlQuery<CountReportModel>(_proc, sqlparams).ToList();
            return res;
        }
        #region CMO DrillDown
        public List<CountReportModel> SRVCountReport_DivisionWiseForCMO(long rollId, int districtId, int serviceId)
        {
            var sqlparams = new SqlParameter[]{ 
                          new SqlParameter {ParameterName="@rollId",Value=rollId},
                          new SqlParameter {ParameterName="@districtId",Value=districtId},
                          new SqlParameter {ParameterName="@serviceId",Value=serviceId}
              };
            var _proc = @"proc_SRVCountReport_DivisionWiseForCMO @rollId,@districtId,@serviceId";
            var res = this.Database.SqlQuery<CountReportModel>(_proc, sqlparams).ToList();
            return res;
        }
        public List<CountReportModel> SRVCountReport_DistrictWiseForCMO(long rollId, int districtId, int serviceId)
        {
            var sqlparams = new SqlParameter[]{ 
                          new SqlParameter {ParameterName="@rollId",Value=rollId},
                          new SqlParameter {ParameterName="@districtId",Value=districtId},
                          new SqlParameter {ParameterName="@serviceId",Value=serviceId}
              };
            var _proc = @"proc_SRVCountReport_DistrictWiseForCMO @rollId,@districtId,@serviceId";
            var res = this.Database.SqlQuery<CountReportModel>(_proc, sqlparams).ToList();
            return res;
        }
        #endregion



        public List<CountReportModel> ServiceCount()
        {
            var _proc = @"Proc_GetServiceCount";
            var res = this.Database.SqlQuery<CountReportModel>(_proc).ToList();
            return res;
        }



        public List<CountReportModel> ServiceWiseCount(int serviceId)
        {
            var sqlParam = new SqlParameter[]{
                new SqlParameter{ParameterName="@serviceId", Value=serviceId}
            };
            var _proc = @"Proc_getServiceWiseCount @serviceId";
            var res = this.Database.SqlQuery<CountReportModel>(_proc, sqlParam).ToList();
            return res;
        }

        public List<CountReportModel> DistrictServiceWiseCount(int serviceId, int zoneId)
        {
            var sqlParam = new SqlParameter[]{
                new SqlParameter{ParameterName="@serviceId", Value=serviceId},
                new SqlParameter{ParameterName="@zoneId", Value=zoneId}
            };
            var _proc = @"Proc_getServiceDistrictWiseCount @serviceId,@zoneId";
            var res = this.Database.SqlQuery<CountReportModel>(_proc, sqlParam).ToList();
            return res;
        }



        public List<CountReportModel> DistrictServiceRollWiseCount(long rollId, int zoneId, int serviceId)
        {
            var sqlParam = new SqlParameter[]{
                 new SqlParameter{ParameterName="@rollId", Value=rollId},
                new SqlParameter{ParameterName="@zoneId", Value=zoneId},
                  new SqlParameter{ParameterName="@serviceId", Value=serviceId} 
            };
            var _proc = @"Proc_getServiceRollDistrictCount @rollId, @zoneId, @serviceId";
            var res = this.Database.SqlQuery<CountReportModel>(_proc, sqlParam).ToList();
            return res;
        }


        public List<ApplicationDetailsModel> DistrictServiceRollApplicantDtl(int appCurrStatus, int rollId, int zoneId, int serviceId, int districtId)
        {
            var sqlParam = new SqlParameter[]{
                    new SqlParameter{ParameterName="@appCurrStatusllId", Value=appCurrStatus},
                    new SqlParameter{ParameterName="@zoneId", Value=zoneId},
                    new SqlParameter{ParameterName="@districtId", Value=districtId},
                    new SqlParameter{ParameterName="@serviceId", Value=serviceId} ,
                 new SqlParameter{ParameterName="@rollId", Value=rollId} 
                
                  
            };
            var _proc = @"proc_GetDistrictRollWiseApplicationDetails @appCurrStatusllId, @zoneId, @districtId, @serviceId, @rollId";
            var res = this.Database.SqlQuery<ApplicationDetailsModel>(_proc, sqlParam).ToList();
            return res;
        }

        //=======================================

        public List<CountReportModel> CMOSRVDistrictWiseCountReport(int zoneId, long rollId, long logedInRollId)
        {
            var sqlparams = new SqlParameter[]{ 
                          new SqlParameter {ParameterName="@zoneId",Value=zoneId},
                          new SqlParameter {ParameterName="@rollId",Value=rollId},
                          new SqlParameter {ParameterName="@logedInRollId",Value=logedInRollId}
              };
            var _proc = @"proc_CMOSRVDistrictWiseCountReport @zoneId,@rollId,@logedInRollId";
            var res = this.Database.SqlQuery<CountReportModel>(_proc, sqlparams).ToList();
            return res;
        }

        #region Akanksha
        public List<ReportsModel> DistrictWise_AllServiceCountReport(string ReportName, int id, string dist, string fromdate, string todate)
        {
            var sqlparams = new SqlParameter[]{
                 new SqlParameter {ParameterName="@action",Value=ReportName},
                 new SqlParameter {ParameterName="@id",Value=id},
                 new SqlParameter {ParameterName="@dist",Value=dist},
                 new SqlParameter {ParameterName="@fromdate",Value=fromdate},
                 new SqlParameter {ParameterName="@todate",Value=todate}
              };
            var _proc = @"DistrictWise_AllServiceCountReportForCMO @action,@id,@dist,@fromdate,@todate";
            var res = this.Database.SqlQuery<ReportsModel>(_proc, sqlparams).ToList();
            return res;
        }

        public List<ApplicationStatusReportDetailsModel> GetAllCMOfficeAppCountApplicationList(int appCurrStatus, string ReportName, string dist, string FromDate, string ToDate, Int64 AppTypeID)
        {
            var sqlparams = new SqlParameter[]{
                new SqlParameter {ParameterName="@appCurrStatus",Value=appCurrStatus},
                          new SqlParameter {ParameterName="@action",Value=ReportName},
                          new SqlParameter {ParameterName="@dist",Value=dist},
                           new SqlParameter {ParameterName="@fromdate",Value=FromDate},
                            new SqlParameter {ParameterName="@todate",Value=ToDate},
                            new SqlParameter {ParameterName="@AppTypeID",Value=AppTypeID}
              };
            var _proc = @"Proc_GetAllCMOfficeAppCountApplicationList @appCurrStatus,@action,@dist,@fromdate,@todate,@AppTypeID";
            var res = this.Database.SqlQuery<ApplicationStatusReportDetailsModel>(_proc, sqlparams).ToList();
            return res;
        }

        public List<cmoOfficeRpt> GetAllCMOfficeAppCountApplicationListForRpt(int appCurrStatus, string ReportName, string dist, string FromDate, string ToDate, Int64 AppTypeID)
        {
            var sqlparams = new SqlParameter[]{
                new SqlParameter {ParameterName="@appCurrStatus",Value=appCurrStatus},
                          new SqlParameter {ParameterName="@action",Value=ReportName},
                          new SqlParameter {ParameterName="@dist",Value=dist},
                           new SqlParameter {ParameterName="@fromdate",Value=FromDate},
                            new SqlParameter {ParameterName="@todate",Value=ToDate},
                            new SqlParameter {ParameterName="@AppTypeID",Value=AppTypeID}
              };
            var _proc = @"Proc_GetAllCMOfficeAppCountApplicationList @appCurrStatus,@action,@dist,@fromdate,@todate,@AppTypeID";
            var res = this.Database.SqlQuery<cmoOfficeRpt>(_proc, sqlparams).ToList();
            return res;
        }


        //public DECModel GetDECList(long userId)
        //{
        //    var sqlparams = new SqlParameter[] { 
            
        //    new SqlParameter{ParameterName="@procId",Value=2},
        //    new SqlParameter{ParameterName="@userId",Value=userId}
        //     };
        //    var _proc = @"proc_getMER @procId, @userId";
        //    var slist = this.Database.SqlQuery<DECModel>(_proc, sqlparams).SingleOrDefault();
        //    return slist;
        //}
        #endregion

        #region work on 2aug23 Shyam

        public List<CountReportModel> CMOSRVCountReportDistrictServicesWise(int DistrictId, int ServiceId, long rollId, long logedInRollId, string fromDate, string toDate, long Userid)
        {
            

            var sqlparams = new SqlParameter[]{ 
                          new SqlParameter {ParameterName="@DistrictId",Value=DistrictId},
                          new SqlParameter {ParameterName="@ServiceId",Value=ServiceId},
                          new SqlParameter {ParameterName="@rollId",Value=rollId},
                          new SqlParameter {ParameterName="@logedInRollId",Value=logedInRollId},
                           new SqlParameter {ParameterName="@fromDate",Value=fromDate},
                          new SqlParameter {ParameterName="@toDate",Value=toDate},
                          new SqlParameter {ParameterName="@Userid",Value=Userid}
              };
            var _proc = @"Proc_GetTotalServiceCountDistrictWise @DistrictId,@ServiceId,@rollId,@logedInRollId,@fromDate,@toDate,@Userid";
            var res = this.Database.SqlQuery<CountReportModel>(_proc, sqlparams).ToList();
            return res;
        }


        public List<CountReportModel> CMOSRVCountReportDistrictServicesWiseCMOCHCPHC(int DistrictId, int ServiceId, long rollId, long logedInRollId, string fromDate, string toDate, int appCurrStatus, long Userid)
        {


            var sqlparams = new SqlParameter[]{ 
                          new SqlParameter {ParameterName="@DistrictId",Value=DistrictId},
                          new SqlParameter {ParameterName="@ServiceId",Value=ServiceId},
                          new SqlParameter {ParameterName="@rollId",Value=rollId},
                          new SqlParameter {ParameterName="@logedInRollId",Value=logedInRollId},
                           new SqlParameter {ParameterName="@fromDate",Value=fromDate},
                          new SqlParameter {ParameterName="@toDate",Value=toDate},
                          new SqlParameter {ParameterName="@appCurrStatus",Value=appCurrStatus},
                           new SqlParameter {ParameterName="@Userid",Value=Userid}
              };
            var _proc = @"proc_GetApplicationCountDistrictServices @DistrictId,@ServiceId,@rollId,@logedInRollId,@fromDate,@toDate,@appCurrStatus,@Userid";
            var res = this.Database.SqlQuery<CountReportModel>(_proc, sqlparams).ToList();
            return res;
        }


        public List<ApplicationDetailsModel> GetApplicationDetailsDistrictServices(int appCurrStatus, int zoneId, int districtId, int serviceId, int isLessFiftyThousan, long userId, string fromDate, string toDate, int roll)
        {
            var sqlparams = new SqlParameter[]{ 
                          new SqlParameter {ParameterName="@appCurrStatus",Value=appCurrStatus},
                          new SqlParameter {ParameterName="@zoneId",Value=zoneId},
                          new SqlParameter {ParameterName="@districtId",Value=districtId},
                          new SqlParameter {ParameterName="@serviceId",Value=serviceId},
                          new SqlParameter {ParameterName="@isLessFiftyThousan",Value=isLessFiftyThousan},
                          new SqlParameter {ParameterName="@userId",Value=userId},
                          new SqlParameter {ParameterName="@fromDate",Value=fromDate},
                          new SqlParameter {ParameterName="@toDate",Value=toDate},
                          new SqlParameter {ParameterName="@roll",Value=roll}
              };
            var _proc = @"proc_GetApplicationDetailsDistrictServices @appCurrStatus,@zoneId,@districtId,@serviceId,@isLessFiftyThousan,@userId,@fromDate,@toDate,@roll";
            var res = this.Database.SqlQuery<ApplicationDetailsModel>(_proc, sqlparams).ToList();
            return res;
        }





        public List<CountReportModel> CMOCHCPHCSRVCountReportDistrictServicesWise(int DistrictId, int ServiceId, long rollId, long logedInRollId, string fromDate, string toDate, long UserId)
        {


            var sqlparams = new SqlParameter[]{ 
                          new SqlParameter {ParameterName="@DistrictId",Value=DistrictId},
                          new SqlParameter {ParameterName="@ServiceId",Value=ServiceId},
                          new SqlParameter {ParameterName="@rollId",Value=rollId},
                          new SqlParameter {ParameterName="@logedInRollId",Value=logedInRollId},
                           new SqlParameter {ParameterName="@fromDate",Value=fromDate},
                          new SqlParameter {ParameterName="@toDate",Value=toDate},
                           new SqlParameter {ParameterName="@UserId",Value=UserId}
              };
            var _proc = @"Proc_GetTotalServiceCountDistrictWiseCMOCHCPHC @DistrictId,@ServiceId,@rollId,@logedInRollId,@fromDate,@toDate,@UserId";
            var res = this.Database.SqlQuery<CountReportModel>(_proc, sqlparams).ToList();
            return res;
        }
        #endregion
    }
}