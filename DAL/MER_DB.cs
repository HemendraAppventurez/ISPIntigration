using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class MER_DB : DbContext
    {
        SessionManager objSM = new SessionManager();
        #region Default Constructor
        public MER_DB()
            : base("CMSModule")
        {

        }
        #endregion

        #region abhijeet code
        public ResultSet IsRegister()
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser}
            };
            var _proc = @"proc_checkuserMER @regisId";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }

        #region check email existence
        public ResultSet CheckEmailMobileExistence(string checkValue, string Type, long regisId)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@regisId", Value = regisId } 
                };

            var sqlQuery = @"proc_checkEmailMobleExistenceMER @checkValue,@Type,@regisId";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion

        public ResultSet MER_Insertion(int procId, long regisIdMER, long regisByuser, int treatmentId, string empfullName, string designation, string manavSampda_AadharNo,
            bool isRetirement, string ppotreament, string father_HusbandName, string officeName, string deptName, decimal basicSalary, string dob, string gender, string mobileNo, string postingAddress, int postingDistrictId, string postingPinCode,
            int postingStateId, string permAddress, int permDistrictId, string permPinCode, int permStateId, string patientType, string patientName, int patientage,
            string patientgender, string patientrelationsWithEmployee, string patientAadhaarNo, string patientdiseaseName, string patienttreatmentFromDate,
            string patienttreatmentToDate, string patientplaceOfDisease, string patienthospitalName, string patientdoctorName, string bankName, string branchName, string accountNo
            , string ifscCode, string xmldata, string regisByIp, string transIp, string requestKey,
           string officeInchargeName,string hospitalType,string isAdvanceTaken,decimal? advanceAmount,string advanceLetterNo,string advanceDate,string ForwardedToId)
        {
            var sqlparams = new SqlParameter[] { 
            
            new SqlParameter{ParameterName="@procId",Value=procId},
            new SqlParameter{ParameterName="@regisIdMER",Value=regisIdMER},
            new SqlParameter{ParameterName="@regisByuser",Value=regisByuser},
            new SqlParameter{ParameterName="@treatmentId",Value=treatmentId},
            new SqlParameter{ParameterName="@empfullName",Value=empfullName},
            new SqlParameter{ParameterName="@designation",Value=designation},
            new SqlParameter{ParameterName="@manavSampda_AadharNo",Value=manavSampda_AadharNo},
            new SqlParameter{ParameterName="@isRetirement",Value=isRetirement},
            new SqlParameter{ParameterName="@ppotreament",Value=(ppotreament==null)?"":ppotreament},
            new SqlParameter{ParameterName="@father_HusbandName",Value=father_HusbandName},
            new SqlParameter{ParameterName="@officeName",Value=officeName??(object)DBNull.Value},
            new SqlParameter{ParameterName="@deptName",Value=deptName},
            new SqlParameter{ParameterName="@basicSalary",Value=basicSalary},
            new SqlParameter{ParameterName="@dob",Value=dob},
            new SqlParameter{ParameterName="@gender",Value=gender},
            new SqlParameter{ParameterName="@mobileNo",Value=mobileNo},
            new SqlParameter{ParameterName="@postingAddress",Value=postingAddress},
            new SqlParameter{ParameterName="@postingDistrictId",Value=postingDistrictId},
            new SqlParameter{ParameterName="@postingPinCode",Value=postingPinCode},
            new SqlParameter{ParameterName="@postingStateId",Value=postingStateId},
            new SqlParameter{ParameterName="@permAddress",Value=permAddress},
            new SqlParameter{ParameterName="@permDistrictId",Value=permDistrictId},
            new SqlParameter{ParameterName="@permPinCode",Value=permPinCode},
            new SqlParameter{ParameterName="@permStateId",Value=permStateId},
            new SqlParameter{ParameterName="@patientType",Value=patientType},
            new SqlParameter{ParameterName="@patientName",Value=patientName},
            new SqlParameter{ParameterName="@patientage",Value=patientage},
            new SqlParameter{ParameterName="@patientgender",Value=patientgender},
            new SqlParameter{ParameterName="@patientrelationsWithEmployee",Value=patientrelationsWithEmployee},
            new SqlParameter{ParameterName="@patientAadhaarNo",Value=patientAadhaarNo??(object)DBNull.Value},
            new SqlParameter{ParameterName="@patientdiseaseName",Value=patientdiseaseName},
            new SqlParameter{ParameterName="@patienttreatmentFromDate",Value=Convert.ToDateTime(patienttreatmentFromDate)},
            new SqlParameter{ParameterName="@patienttreatmentToDate",Value=Convert.ToDateTime(patienttreatmentToDate)},
            new SqlParameter{ParameterName="@patientplaceOfDisease",Value=patientplaceOfDisease},
            new SqlParameter{ParameterName="@patienthospitalName",Value=patienthospitalName},
            new SqlParameter{ParameterName="@patientdoctorName",Value=patientdoctorName},
            new SqlParameter{ParameterName="@bankName",Value=bankName},
            new SqlParameter{ParameterName="@branchName",Value=branchName},
            new SqlParameter{ParameterName="@accountNo",Value=accountNo},
            new SqlParameter{ParameterName="@ifscCode",Value=ifscCode},
            new SqlParameter{ParameterName="@xmldata",Value=xmldata},
            new SqlParameter{ParameterName="@regisByIp",Value=regisByIp},
            new SqlParameter{ParameterName="@transIp",Value=transIp},
            new SqlParameter { ParameterName = "@requestKey", Value =requestKey??string.Empty},

            new SqlParameter { ParameterName = "@officeInchargeName", Value =officeInchargeName??(object)DBNull.Value},
            new SqlParameter { ParameterName = "@hospitalType", Value =hospitalType??string.Empty},
            new SqlParameter { ParameterName = "@isAdvanceTaken", Value =isAdvanceTaken??string.Empty},
            new SqlParameter { ParameterName = "@advanceAmount", Value =advanceAmount??(object)DBNull.Value},
            new SqlParameter { ParameterName = "@advanceLetterNo", Value =advanceLetterNo??(object)DBNull.Value},
            new SqlParameter { ParameterName = "@advanceDate", Value =advanceDate??string.Empty},
             new SqlParameter { ParameterName = "@ForwardedToId", Value =ForwardedToId??string.Empty}
            };

            var _proc = @"proc_insertupdateMER @procId,@regisIdMER,@regisByuser,@treatmentId,@empfullName,@designation,@manavSampda_AadharNo,@isRetirement,@ppotreament,@father_HusbandName,
@officeName,@deptName,@basicSalary,@dob,@gender,@mobileNo,@postingAddress,@postingDistrictId,@postingPinCode,@postingStateId,@permAddress,@permDistrictId,@permPinCode,
@permStateId,@patientType,@patientName,@patientage ,@patientgender,@patientrelationsWithEmployee,@patientAadhaarNo ,@patientdiseaseName,@patienttreatmentFromDate,
@patienttreatmentToDate,@patientplaceOfDisease,@patienthospitalName,@patientdoctorName,@bankName,@branchName ,@accountNo ,@ifscCode,@xmldata,@regisByIp,@transIp,@requestKey,
@officeInchargeName,@hospitalType,@isAdvanceTaken,@advanceAmount,@advanceLetterNo,@advanceDate,@ForwardedToId";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }

        public List<MERModel> getMERList(long userId)
        {
            var sqlparams = new SqlParameter[] { 
            
            new SqlParameter{ParameterName="@procId",Value=1},
            new SqlParameter{ParameterName="@userId",Value=userId}
             };
            var _proc = @"proc_getMER @procId, @userId";
            var slist = this.Database.SqlQuery<MERModel>(_proc, sqlparams).ToList();
            return slist;
        }


        public MERModel getMERByRegistration(long userId)
        {
            var sqlparams = new SqlParameter[] { 
            
            new SqlParameter{ParameterName="@procId",Value=2},
            new SqlParameter{ParameterName="@userId",Value=userId}
             };
            var _proc = @"proc_getMER @procId, @userId";
            var slist = this.Database.SqlQuery<MERModel>(_proc, sqlparams).SingleOrDefault();
            return slist;
        }
        public List<MERModel> getMERChild(long regisId)
        {
            var sqlparams = new SqlParameter[] { 
            
            new SqlParameter{ParameterName="@regisId",Value=regisId}
           
             };
            var _proc = @"proc_getMERChild @regisId";
            var slist = this.Database.SqlQuery<MERModel>(_proc, sqlparams).ToList();
            return slist;
        }
        public RegisterDetailsModel GetRegisterDetails()
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser}
            };
            var _proc = @"proc_RegisterDetailsMER @regisId";
            var slist = this.Database.SqlQuery<RegisterDetailsModel>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        #endregion

        #region Delete Registration MER
        public int DeleteRegistrationMER(long regisIdMER)
        {
            var sqlParams = new SqlParameter[] {    
                 new SqlParameter { ParameterName = "@regisIdMER", Value = regisIdMER} 
            };
            var query = "proc_DeleteRegistration_MER @regisIdMER";
            var result = this.Database.ExecuteSqlCommand(query, sqlParams);
            return result;
        }
        #endregion

        #region Riya
        public List<MERrptModel> GetMERdetailRpt(long userId)
        {
            var sqlparams = new SqlParameter[] { 
            
           
            new SqlParameter{ParameterName="@regisIdMER",Value=userId}
             };
            var _proc = @"GetMERdetailRpt  @regisIdMER";
            var slist = this.Database.SqlQuery<MERrptModel>(_proc, sqlparams).ToList();
            return slist;
        }
        public List<MERChildRptModel> GetMERCHILD(long regisid)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisIdMER",Value=regisid}  
            };
            var _proc = @"GetMERdetailChildRpt @regisIdMER";
            var slist = this.Database.SqlQuery<MERChildRptModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public List<MERrptModel> GetMERdetailCertificateRpt(long userId)
        {
            var sqlparams = new SqlParameter[] { 
            
           
            new SqlParameter{ParameterName="@regisIdMER",Value=userId}
             };
            var _proc = @"GetMERdetailForCertificateRpt  @regisIdMER";
            var slist = this.Database.SqlQuery<MERrptModel>(_proc, sqlparams).ToList();
            return slist;
        }
        #endregion

    }
}
