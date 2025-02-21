using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCSHealthFamilyWelfareDept.Models;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class Common_DB : DbContext
    {
         #region Default Constructor
        public Common_DB()
            : base("CMSModule")
        { }
        #endregion

        #region Create Email Log
        public int EmailLog(String EmailId, String Subject, String MailBody, bool MailStatus, String ResponceMessage)
        {
            try
            {
                int count = this.Database.ExecuteSqlCommand("proc_InserEmailLog @EmailId, @Subject, @MailBody,@MailStatus,@ResponceMessage",
                    new SqlParameter("EmailId", EmailId),
                    new SqlParameter("Subject", Subject),
                    new SqlParameter("MailBody", MailBody),
                    new SqlParameter("MailStatus", MailStatus),
                    new SqlParameter("ResponceMessage", ResponceMessage)
                    );
                return count;
            }
            catch
            { return 0; }

        }
        #endregion

        #region Method Get Dropdown List
        public List<DropDownList> GetDropDownList(int procId, int Id)
        {
            var sqlparam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@procId",Value=procId},
              new SqlParameter{ParameterName="@Id",Value=Id}
            };
            var _proc = @"proc_GetDataForDropDownList @procId,@Id";
            var slist = this.Database.SqlQuery<DropDownList>(_proc, sqlparam).ToList();
            return slist;
        } 
        #endregion

        #region Method Get Monday Dates
        public List<SelectListItem> GetMondayDates()
        {
            var _proc = @"proc_GetMondayDates";
            var slist = this.Database.SqlQuery<SelectListItem>(_proc).ToList();
            return slist;
        }
        #endregion

        #region Method Get Application Process By Application Name
        public List<SelectListItem> GetApplicationProcessByAppName(string applicationName)
        {
            var sqlparam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@applicationName",Value=applicationName}
            };
            var _proc = @"proc_GetApplicationProcessByAppName @applicationName";
            var slist = this.Database.SqlQuery<SelectListItem>(_proc, sqlparam).ToList();
            return slist;
        }
        #endregion

        #region Method Get Committee Details For DLL
        public List<SelectListItem> GetCommitteeDetailsForDLL()
        {
            var _proc = @"proc_GetCommitteeDetailsForDLL";
            var slist = this.Database.SqlQuery<SelectListItem>(_proc).ToList();
            return slist;
        }
        #endregion

        #region Method Get Committee Details For DLL
        public List<SelectListItem> GetCommitteeDetailsForDLL_DIC()
        {
            var _proc = @"proc_GetCommitteeDetailsForDLL_DIC";
            var slist = this.Database.SqlQuery<SelectListItem>(_proc).ToList();
            return slist;
        }
        #endregion

        #region Method Get Test Type For DLL
        public List<SelectListItem> GetTestTypeForDLL_DIC()
        {
            var _proc = @"proc_GetTestTypeForDLL_DIC";
            var slist = this.Database.SqlQuery<SelectListItem>(_proc).ToList();
            return slist;
        }
        #endregion

        #region Method Get Committee Member DIC
        public List<DropDownList> GetCommitteeMember_DIC(long regisIdDIC)
        {
            var sqlparam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@regisIdDIC",Value=regisIdDIC}
            };
            var _proc = @"proc_GetCommitteeMember_DIC @regisIdDIC";
            var slist = this.Database.SqlQuery<DropDownList>(_proc, sqlparam).ToList();
            return slist;
        }
        #endregion

        #region Method Get Committee Member DIC
        public DICAppProcessModel GetInspReportPersentageDIC(long regisId)
        {
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisId}
            };
            var _proc = @"proc_GetInspReportPersentage_DIC @regisId";
            var slist = this.Database.SqlQuery<DICAppProcessModel>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        #endregion

        #region Method Get Digital Signature Details
        public DigitalSignatureModel GetDigitalSignatureDetails(long userId)
        {
            SqlParameter param = new SqlParameter("@userId", userId);
            var sqlProc = "proc_GetDigitalSignatureDetails @userId";
            var sList = this.Database.SqlQuery<DigitalSignatureModel>(sqlProc, param).FirstOrDefault();
            return sList;
        } 
        #endregion

        #region Insert Send Request
        public int InsertSendRequest(ServiceModel model)
        {
            int result = 0;

            var sqlParams = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@requestKey",Value=model.requestKey},
                            new SqlParameter{ParameterName="@deptRegistrationId",Value=model.deptRegistrationId},
                            new SqlParameter{ParameterName="@serviceResponse",Value=model.serviceResponse},
                            new SqlParameter{ParameterName="@transIp",Value=model.transIp}
            };

            var query = "proc_InsertSendRequest_EDistrict @requestKey,@deptRegistrationId,@serviceResponse,@transIp";
            result = this.Database.ExecuteSqlCommand(query, sqlParams);
            return result;
        }
        #endregion

        #region Insert Send Request
        public int InsertSendResponse(ServiceModel model)
        {
            int result = 0;

            var sqlParams = new SqlParameter[] { 
                            new SqlParameter{ParameterName="@requestKey",Value=model.requestKey},
                            new SqlParameter{ParameterName="@deptRegistrationId",Value=model.deptRegistrationId},
                            new SqlParameter{ParameterName="@serviceResponse",Value=model.serviceResponse},
                            new SqlParameter{ParameterName="@applicationNumber",Value=model.applicationNumber},
                            new SqlParameter{ParameterName="@serviceCode",Value=model.serviceCode},
                            new SqlParameter{ParameterName="@applicationType",Value=model.applicationType},
                            new SqlParameter{ParameterName="@transIp",Value=model.transIp}
            };

            var query = "proc_InsertSendResponse_EDistrict @requestKey,@deptRegistrationId,@serviceResponse,@applicationNumber,@serviceCode,@applicationType,@transIp";
            result = this.Database.ExecuteSqlCommand(query, sqlParams);
            return result;
        }
        #endregion

        #region Method Get CMO District Mapping
        public List<CMODistrictModel> GetCMODistrictMapping(long userId)
        {
            var sqlparam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@userId",Value=userId}
            };
            var _proc = @"proc_GetCMODistrictMapping @userId";
            var slist = this.Database.SqlQuery<CMODistrictModel>(_proc, sqlparam).ToList();
            return slist;
        }
        #endregion

        #region Method Get Unsigned Certificate Details of DIC
        public List<DICCertificateDetailModel> GetUnsignedCerti_DIC(long cmoProfileId)
        {
            var sqlParam = new SqlParameter[] { 
               new SqlParameter("@cmoProfileId", cmoProfileId)
            };
            var _proc = @"proc_GetUnsignedCerti_DIC @cmoProfileId";
            var slist = this.Database.SqlQuery<DICCertificateDetailModel>(_proc, sqlParam).ToList();
            return slist;
        } 
        #endregion

        #region Method Get Unsigned Certificate Details of AGC
        public List<AGCModel> GetUnsignedCerti_AGC(long cmoProfileId)
        {
            var sqlParam = new SqlParameter[] { 
                    new SqlParameter{ParameterName="@cmoProfileId",Value=cmoProfileId} 
             };
            var _proc = @"proc_GetUnsignedCerti_AGC @cmoProfileId";
            var slist = this.Database.SqlQuery<AGCModel>(_proc, sqlParam).ToList();
            return slist;
        }
        #endregion

        #region Method Get Unsigned Certificate Details of MER
        public List<MERrptModel> GetUnsignedCerti_MER(long cmoProfileId)
        {
            var sqlparams = new SqlParameter[] { 
                    new SqlParameter{ParameterName="@cmoProfileId",Value=cmoProfileId}
             };
            var _proc = @"proc_GetUnsignedCerti_MER  @cmoProfileId";
            var slist = this.Database.SqlQuery<MERrptModel>(_proc, sqlparams).ToList();
            return slist;
        }
        #endregion

        #region Get OTP Count
        public virtual IEnumerable<DropDownList> GetOTPCount(string mobileNo)
        {
            var sqlParams = new SqlParameter[] { 
                 new SqlParameter { ParameterName = "@mobileNo", Value = mobileNo }
              };
            var sqlQuery = @"proc_GetOTPCount @mobileNo";
            var sList = this.Database.SqlQuery<DropDownList>(sqlQuery, sqlParams);
            return sList;
        }
        #endregion

        #region set OTP is Used
        public virtual IEnumerable<DropDownList> UpdateOTPUsedStatus(string mobileNo)
        {
            var sqlParams = new SqlParameter[] { 
                 new SqlParameter { ParameterName = "@mobileNo", Value = mobileNo }
              };
            var sqlQuery = @"proc_UpdateOTPUsedStatus @mobileNo";
            var sList = this.Database.SqlQuery<DropDownList>(sqlQuery, sqlParams);
            return sList;
        }
        #endregion


        #region Method Get Dropdown List PARAM
        public List<DropDownList> GetDropDownListFilled(int procId, Int64 Id)
        {
            var sqlparam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@procId",Value=procId},
              new SqlParameter{ParameterName="@reregisIdNUH",Value=Id}
            };
            var _proc = @"proc_GetDataForDropDownListFilled @procId,@reregisIdNUH";
            var slist = this.Database.SqlQuery<DropDownList>(_proc, sqlparam).ToList();
            return slist;
        }
        #endregion

        #region Method Get Zone Details For DLL
        public List<SelectListItem> GetZoneForDLL()
        {
            var _proc = @"proc_GetZoneForDLL";
            var slist = this.Database.SqlQuery<SelectListItem>(_proc).ToList();
            return slist;
        }
        #endregion

        #region Method Get District Details For DLL
        public List<SelectListItem> GetDistrictForDLL()
        {
            var _proc = @"proc_GetDistrictForDLL";
            var slist = this.Database.SqlQuery<SelectListItem>(_proc).ToList();
            return slist;
        }
        #endregion

        #region Method Get Services Details For DLL
        public List<SelectListItem> GetSevicesForDLL()
        {
            var _proc = @"proc_GetServicesForDLL";
            var slist = this.Database.SqlQuery<SelectListItem>(_proc).ToList();
            return slist;
        }
        #endregion

        #region Method Get Roll DLL
        public List<SelectListItem> GetRollForDLL()
        {
            var _proc = @"GetRollMasterDLL";
            var slist = this.Database.SqlQuery<SelectListItem>(_proc).ToList();
            return slist;
        }
        #endregion
    }
}