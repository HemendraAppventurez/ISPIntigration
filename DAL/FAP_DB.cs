using CCSHealthFamilyWelfareDept.Models;
using CCSHealthFamilyWelfareDept.ReportModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class FAP_DB : DbContext
    {
        SessionManager objSM = new SessionManager();
        #region Default Constructor
        public FAP_DB()
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
            var _proc = @"proc_checkuserFAP @regisId";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public List<DropDownList> getCompensationId(int Id)
        {
            var sqlparam = new SqlParameter[] { 
            
            new SqlParameter{ParameterName="@Id",Value=Id}
            };
            var _proc = @"proc_GetCompensationId @Id";
            var slist = this.Database.SqlQuery<DropDownList>(_proc, sqlparam).ToList();
            return slist;
        }

        #region check email existence
        public ResultSet CheckEmailMobileExistence(string checkValue, string Type)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type }

                };

            var sqlQuery = @"proc_checkEmailMobleExistenceFAP @checkValue,@Type";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion

        public ResultSet FAPInsertUpdate(int procId, long regisIdFAP, long regisByuser, int compensationId, string dateOfReleased, string dateOfdeath, string admittedDate, string complicationDetails
          , string healthUnitName, string healthUnitAddress, int stateId, int healthunitDistrictId, string docterName, string claimantName, string claimantDob, int? claimantAge, int relationId, string claimantAddress, string claimantMobile,
         string sterilizedName, string sterilizedDob, int? sterlizedAge, string fatherName, string spouseName, int? spouseAge, string sterilizedGender,string sterilizedAddress,int sterlizedDistrictId, int employementId,
            string serviceType, string operatioDate, int operationTypeId, string operationOtherprocess, bool isConfirm, decimal? claimantAmount, string informationDate, string sevakendraName, string confirmationDate, string sevadoctorName,string accountHolderName, string bankName, string branchName, string accountNo
            , string ifscCode, string opdDistricthospitalfilePath,
            string usgfilePath, string uptfilePath, string antenatalcardfilePath, string affidavitfilePath, string sterilizationcertificatefilePath, string regBytransIp, string transIp
            , string xmlData, string requestKey, string deathCertificateFilePath, string dischargeSummaryFilePath, string claimantAadhaarNo)
        {

            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=procId},
            new SqlParameter{ParameterName="@regisIdFAP",Value=regisIdFAP},
            new SqlParameter{ParameterName="@regisByuser",Value=regisByuser},
            new SqlParameter{ParameterName="@compensationId",Value=compensationId},
            new SqlParameter{ParameterName="@dateOfReleased",Value=dateOfReleased??(object)DBNull.Value},
            new SqlParameter{ParameterName="@dateOfdeath",Value=dateOfdeath??(object)DBNull.Value},
            new SqlParameter{ParameterName="@admittedDate",Value=admittedDate??(object)DBNull.Value},
            new SqlParameter{ParameterName="@complicationDetails",Value=complicationDetails},
            new SqlParameter{ParameterName="@healthUnitName",Value=healthUnitName},
            new SqlParameter{ParameterName="@healthUnitAddress",Value=healthUnitAddress},
             new SqlParameter{ParameterName="@stateId",Value=stateId},
            new SqlParameter{ParameterName="@healthunitDistrictId",Value=healthunitDistrictId},
            new SqlParameter{ParameterName="@docterName",Value=docterName},
            new SqlParameter{ParameterName="@claimantName",Value=claimantName??string.Empty},
            new SqlParameter{ParameterName="@claimantDob",Value=claimantDob??(object)DBNull.Value},
            new SqlParameter{ParameterName="@claimantAge",Value=(claimantAge==null)?0:claimantAge},
            new SqlParameter{ParameterName="@relationId",Value=(relationId==null)?0:relationId},
            new SqlParameter{ParameterName="@claimantAddress",Value=claimantAddress??string.Empty},
            new SqlParameter{ParameterName="@claimantMobile",Value=claimantMobile},
            new SqlParameter{ParameterName="@sterilizedName",Value=sterilizedName},
            new SqlParameter{ParameterName="@sterilizedDob",Value=sterilizedDob??(object)DBNull.Value},
            new SqlParameter{ParameterName="@sterlizedAge",Value=sterlizedAge},
            new SqlParameter{ParameterName="@fatherName",Value=fatherName},
            new SqlParameter{ParameterName="@spouseName",Value=spouseName},
            new SqlParameter{ParameterName="@spouseAge",Value=spouseAge},
            new SqlParameter{ParameterName="@sterilizedGender",Value=sterilizedGender},
            new SqlParameter{ParameterName="@sterilizedAddress",Value=sterilizedAddress},
            new SqlParameter{ParameterName="@sterlizedDistrictId",Value=sterlizedDistrictId},
            new SqlParameter{ParameterName="@employementId",Value=(employementId==null)?0:employementId},
            new SqlParameter{ParameterName="@serviceType",Value=serviceType},
            new SqlParameter{ParameterName="@operatioDate",Value=Convert.ToDateTime(operatioDate)},
            new SqlParameter{ParameterName="@operationTypeId",Value=operationTypeId},
            new SqlParameter{ParameterName="@operationOtherprocess",Value=operationOtherprocess??string.Empty},
            new SqlParameter{ParameterName="@isConfirm",Value=isConfirm},
            new SqlParameter{ParameterName="@claimantAmount",Value=claimantAmount},
            new SqlParameter{ParameterName="@informationDate",Value=informationDate},
            new SqlParameter{ParameterName="@sevakendraName",Value=sevakendraName},
            new SqlParameter{ParameterName="@confirmationDate",Value=confirmationDate},
            new SqlParameter{ParameterName="@sevadoctorName",Value=sevadoctorName},
            new SqlParameter{ParameterName="@accountHolderName",Value=accountHolderName},
            new SqlParameter{ParameterName="@bankName",Value=bankName},
            new SqlParameter{ParameterName="@branchName",Value=branchName},
            new SqlParameter{ParameterName="@accountNo",Value=accountNo},
            new SqlParameter{ParameterName="@ifscCode",Value=ifscCode},
            new SqlParameter{ParameterName="@opdDistricthospitalfilePath",Value=opdDistricthospitalfilePath??string.Empty},
            new SqlParameter{ParameterName="@usgfilePath",Value=usgfilePath??string.Empty},
            new SqlParameter{ParameterName="@uptfilePath",Value=uptfilePath??string.Empty},
            new SqlParameter{ParameterName="@antenatalcardfilePath",Value=antenatalcardfilePath??string.Empty},
            new SqlParameter{ParameterName="@affidavitfilePath",Value=affidavitfilePath??string.Empty},
            new SqlParameter{ParameterName="@sterilizationcertificatefilePath",Value=sterilizationcertificatefilePath??string.Empty},
            new SqlParameter{ParameterName="@regBytransIp",Value=regBytransIp},
            new SqlParameter{ParameterName="@transIp",Value=transIp},
            new SqlParameter{ParameterName="@xmldata",Value=xmlData},
            new SqlParameter { ParameterName = "@requestKey", Value =requestKey??string.Empty},
            new SqlParameter { ParameterName = "@deathCertificateFilePath", Value =deathCertificateFilePath??string.Empty},
            new SqlParameter { ParameterName = "@dischargeSummaryFilePath", Value =dischargeSummaryFilePath??string.Empty},
            new SqlParameter { ParameterName = "@claimantAadhaarNo", Value =claimantAadhaarNo??string.Empty}
            };
            var _proc = @"proc_FAPinsertUpdate @procId,@regisIdFAP,@regisByuser ,@compensationId,@dateOfReleased,@dateOfdeath,@admittedDate,@complicationDetails,@healthUnitName,@healthUnitAddress,@stateId,@healthunitDistrictId,@docterName,@claimantName,@claimantDob,@claimantAge,@relationId ,@claimantAddress,@claimantMobile,@sterilizedName,@sterilizedDob,@sterlizedAge,@fatherName,@spouseName,@spouseAge,@sterilizedGender,@sterilizedAddress,@sterlizedDistrictId,@employementId,@serviceType,@operatioDate,@operationTypeId,@operationOtherprocess,@isConfirm,@claimantAmount,@informationDate,@sevakendraName,@confirmationDate,@sevadoctorName,@accountHolderName,@bankName,@branchName ,@accountNo ,@ifscCode,@opdDistricthospitalfilePath,@usgfilePath,@uptfilePath ,@antenatalcardfilePath,@affidavitfilePath,@sterilizationcertificatefilePath,@regBytransIp,@transIp,@xmldata,@requestKey,@deathCertificateFilePath,@dischargeSummaryFilePath,@claimantAadhaarNo";

            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;

        }
        public List<FAPModel> GetFAPList(long userID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=1}  ,
              new SqlParameter{ParameterName="@userId",Value=userID} 
            };
            var _proc = @"proc_getFAP @procId,@userId";
            var slist = this.Database.SqlQuery<FAPModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public FAPModel GetFAPListBYRegistrationNo(long userId)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=2},
             new SqlParameter{ParameterName="@userId",Value=userId}
            
            };
            var _proc = @"proc_getFAP @procId,@userId";
            var slist = this.Database.SqlQuery<FAPModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public List<FAPModel> getFAPChild(long regisFAP)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisFAP}  
            };
            var _proc = @"proc_getFAPChild @regisId";
            var slist = this.Database.SqlQuery<FAPModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public int UploadAffidavitNUH(long regisIdFAP, string affidavitfilePath)
        {
            int res = 0;
            res = this.Database.ExecuteSqlCommand("proc_UploadAffidavitFAP @regisIdFAP,@affidavitfilePath",
               new SqlParameter("@regisIdFAP", regisIdFAP),
               new SqlParameter("@affidavitfilePath", affidavitfilePath)
               );
            return res;
        }

        public RegisterDetailsModel GetRegisterDetails()
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser}
            };
            var _proc = @"proc_RegisterDetailsFAP @regisId";
            var slist = this.Database.SqlQuery<RegisterDetailsModel>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public List<rptFAPModel> GetDetail(long regisId)
        {
            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisId}  
            };
            var _proc = @"proc_rptFAP @regisId";
            var slist = this.Database.SqlQuery<rptFAPModel>(_proc, sqlParam).ToList();
            return slist;
        }
        
        #endregion

        #region Delete Registration FAP
        public int DeleteRegistrationFAP(long regisIdFAP)
        {
            var sqlParams = new SqlParameter[] {    
                 new SqlParameter { ParameterName = "@regisIdFAP", Value = regisIdFAP} 
            };
            var query = "proc_DeleteRegistration_FAP @regisIdFAP";
            var result = this.Database.ExecuteSqlCommand(query, sqlParams);
            return result;
        }
        #endregion

        public CompancationModel GetCompensationAmont(int compensationCategoryId)
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@compensationCategoryId",Value=compensationCategoryId}
            };
            var _proc = @"Proc_getCompensationAmont @compensationCategoryId";
            try
            {
                var slist = this.Database.SqlQuery<CompancationModel>(_proc, sqlparams).FirstOrDefault();
                return slist;
            }
            catch
            {
                return null;
            }

        }
        
    }
}