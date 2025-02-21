using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class FIC_DB : DbContext
    {
        public FIC_DB()
            : base("CMSModule")
        {
        }
        SessionManager objSM = new SessionManager();
        public ResultSet IsRegister()
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser}
            };
            var _proc = @"proc_checkuserFIC @regisId";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }


        public FICModel GetFICById(long Id)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@Id",Value=@Id}
            };
            var _proc = @"proc_getuserCommondetails @Id";
            var slist = this.Database.SqlQuery<FICModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }

        //public ResultSet FICInsertUpdate(int procid, long regisIdFIC, long regByUser, string opdName, string opdRecNo, string opdDate, string opdAddress, string opdPinCode, int opdStateId, int opdDistrictId, string opdFilePath, string fullName, string fatherName, string dob, string gender, int categoryId,
        //    string mobileNo, string emailId, long forwardType,
        //long forwardtoId, string fitnessAdvicedBy, string treatmentFrom, string treatmentto, string applicationReason, string illnessDetail, string remarks, string Ip, string isMedicalHistory, int healthUnitDistrictId)
        //{
        //    var sqlparams = new SqlParameter[] { 
        //            new  SqlParameter{ParameterName="@procId",Value=procid},
        //            new  SqlParameter{ParameterName="@regisIdFIC",Value=regisIdFIC},
        //            new  SqlParameter{ParameterName="@regByUser",Value=regByUser},
        //            new  SqlParameter{ParameterName="@opdName",Value=(opdName==null)?"":opdName},
        //            new  SqlParameter{ParameterName="@opdRecNo",Value=(opdRecNo==null)?"":opdRecNo},
        //            new  SqlParameter{ParameterName="@opdDate",Value=(opdDate==null)?"":opdDate},
        //            new  SqlParameter{ParameterName="@opdAddress",Value=(opdAddress==null)?"":opdAddress},
        //            new  SqlParameter{ParameterName="@opdPinCode",Value=(opdPinCode==null)?"":opdPinCode},
        //            new  SqlParameter{ParameterName="@opdStateId",Value=(opdStateId==null)?0:opdStateId},
        //            new  SqlParameter{ParameterName="@opdDistrictId",Value=(opdDistrictId==null)?0:opdDistrictId},
        //            new SqlParameter{ParameterName="@opdFilePath",Value=opdFilePath},
        //            new  SqlParameter{ParameterName="@fullName",Value=(fullName==null)?"":fullName},
        //            new  SqlParameter{ParameterName="@fatherName",Value=(fatherName==null)?"":fatherName},
        //            new  SqlParameter{ParameterName="@dob",Value=(dob==null)?"":dob},
        //            new  SqlParameter{ParameterName="@gender",Value=(gender==null)?"":gender},
        //            new  SqlParameter{ParameterName="@categoryId",Value=(categoryId==null)?0:categoryId},
        //            new  SqlParameter{ParameterName="@mobileNo",Value=(mobileNo==null)?"":mobileNo},
        //            new  SqlParameter{ParameterName="@emailId",Value=emailId??string.Empty},
        //            new SqlParameter{ParameterName="@forwardType",Value=forwardType},
        //            new SqlParameter{ParameterName="@forwardtoId",Value=forwardtoId},
        //            new  SqlParameter{ParameterName="@fitnessAdvicedBy",Value=(fitnessAdvicedBy==null)?"":fitnessAdvicedBy},
        //            new  SqlParameter{ParameterName="@treatmentFrom",Value=(treatmentFrom==null)?"":treatmentFrom},
        //            new  SqlParameter{ParameterName="@treatmentto",Value=(treatmentto==null)?"":treatmentto},
        //            new  SqlParameter{ParameterName="@applicationReason",Value=applicationReason},
        //            new  SqlParameter{ParameterName="@illnessDetail",Value=illnessDetail}, 
        //            new  SqlParameter{ParameterName="@remarks",Value=remarks??string.Empty},
        //            new  SqlParameter{ParameterName="@regByIp",Value=Ip},
        //            new  SqlParameter{ParameterName="@transIp",Value=Ip},
        //            new  SqlParameter{ParameterName="@isMedicalHistory",Value=isMedicalHistory},
        //            new  SqlParameter{ParameterName="@healthUnitDistrictId",Value=healthUnitDistrictId}
        //    };

        //    var _proc = @"proc_FIC_InsertUpdate  @procId, @regisIdFIC , @regByUser, @opdName, @opdRecNo , @opdDate,  @opdAddress ,  @opdPinCode, @opdStateId, @opdDistrictId ,@opdFilePath, @fullName , @fatherName , @dob ,@gender,@categoryId, @mobileNo, @emailId ,@forwardType,@forwardtoId, @fitnessAdvicedBy , @treatmentFrom , @treatmentto , @applicationReason , @illnessDetail, @remarks, @regByIp , @transIp,@isMedicalHistory,@healthUnitDistrictId ";

        //    var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
        //    return slist;
        //}

        #region check email existence
        public ResultSet CheckEmailMobileExistence(string checkValue, string Type)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type }

                };

            var sqlQuery = @"proc_checkEmailMobleExistenceFIC @checkValue,@Type";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion

        public ResultSet FICInsertUpdate(FICModel model)
        {
            var sqlparams = new SqlParameter[] { 
                    new  SqlParameter{ParameterName="@procId",Value=1},
                    new  SqlParameter{ParameterName="@regisIdFIC",Value=model.regisIdFIC},
                    new  SqlParameter{ParameterName="@regByUser",Value=model.regByUser},
                    new  SqlParameter{ParameterName="@opdName",Value=model.opdName??string.Empty},
                    new  SqlParameter{ParameterName="@opdRecNo",Value=model.opdRecNo??string.Empty},
                    new  SqlParameter{ParameterName="@opdDate",Value=model.opdDate??string.Empty},
                    new  SqlParameter{ParameterName="@opdAddress",Value=model.opdAddress??string.Empty},
                   // new  SqlParameter{ParameterName="@opdPinCode",Value=model.opdPinCode??string.Empty},
                    new  SqlParameter{ParameterName="@opdStateId",Value=model.opdStateId},
                    new  SqlParameter{ParameterName="@opdDistrictId",Value=model.opdDistrictId},
                    new SqlParameter{ParameterName="@opdFilePath",Value=model.opdFilePath??string.Empty},
                    new  SqlParameter{ParameterName="@fullName",Value=model.fullName},
                    new  SqlParameter{ParameterName="@fatherName",Value=model.fatherName},
                    new  SqlParameter{ParameterName="@dob",Value=model.dob},
                    new  SqlParameter{ParameterName="@gender",Value=model.gender},
                    new  SqlParameter{ParameterName="@categoryId",Value=model.categoryId},
                    new  SqlParameter{ParameterName="@mobileNo",Value=model.mobileNo},
                    new  SqlParameter{ParameterName="@emailId",Value=model.emailId??string.Empty},
                    new SqlParameter{ParameterName="@forwardType",Value=model.forwardtypeId},
                    new SqlParameter{ParameterName="@forwardtoId",Value=model.forwardtoId},
                    new  SqlParameter{ParameterName="@fitnessAdvicedBy",Value=model.fitnessAdvicedBy??string.Empty},
                    //new  SqlParameter{ParameterName="@treatmentFrom",Value=model.treatmentFrom??string.Empty},
                    //new  SqlParameter{ParameterName="@treatmentto",Value=model.treatmentto??string.Empty},
                    new  SqlParameter{ParameterName="@applicationReason",Value=model.applicationReason},
                    new  SqlParameter{ParameterName="@illnessDetail",Value=model.illnessDetail??string.Empty}, 
                    new  SqlParameter{ParameterName="@remarks",Value=model.remarks??string.Empty},
                    new  SqlParameter{ParameterName="@regByIp",Value=model.transIp},
                    new  SqlParameter{ParameterName="@transIp",Value=model.transIp},
                    new  SqlParameter{ParameterName="@isMedicalHistory",Value=model.isMedicalHistory},
                    new  SqlParameter{ParameterName="@healthUnitDistrictId",Value=model.healthUnitDistrictId},
                    new SqlParameter { ParameterName = "@requestKey", Value =model.requestKey??string.Empty},

                    new SqlParameter { ParameterName = "@idTypeId", Value =model.idTypeId},
                    new SqlParameter { ParameterName = "@idNumber", Value =(model.idNumber==null)?"":model.idNumber},
                    new SqlParameter { ParameterName = "@idFile", Value =model.idFilePath},
                    new SqlParameter { ParameterName = "@markOfIdentification", Value =model.markOfIdentification},
                    new SqlParameter { ParameterName = "@ILCcertificateNo", Value =model.certificateNo!=null ?model.certificateNo:(object)DBNull.Value},
                    new SqlParameter { ParameterName = "@detailsMedicalHistory", Value =model.detailsMedicalHistory!=null ?model.detailsMedicalHistory:(object)DBNull.Value}
            };

            var _proc = @"proc_FIC_InsertUpdate  @procId, @regisIdFIC , @regByUser, @opdName, @opdRecNo , @opdDate,  @opdAddress , @opdStateId, @opdDistrictId ,@opdFilePath, @fullName , @fatherName , @dob ,@gender,@categoryId, @mobileNo, @emailId ,@forwardType,@forwardtoId, @fitnessAdvicedBy  , @applicationReason , @illnessDetail, @remarks, @regByIp , @transIp,@isMedicalHistory,@healthUnitDistrictId,@requestKey,@idTypeId,@idNumber,@idFile ,@markOfIdentification,@ILCcertificateNo,@detailsMedicalHistory";

            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public ResultSet FICInsertUpdate_ILCtoFIC(FICModel model)
        {
            var sqlparams = new SqlParameter[] { 
                    
                    new  SqlParameter{ParameterName="@regisIdFIC",Value=model.regisIdFIC},
                    new  SqlParameter { ParameterName ="@ILCcertificateNo", Value =model.certificateNo!=null ?model.certificateNo:(object)DBNull.Value},
                    new  SqlParameter{ParameterName="@regByUser",Value=model.regByUser},
                    new  SqlParameter{ParameterName="@applicationReason",Value=model.applicationReason},
                    new  SqlParameter{ParameterName="@opdRecNo",Value=model.opdRecNo??string.Empty},
                    new  SqlParameter{ParameterName="@opdFilePath",Value=model.opdFilePath??string.Empty},
                    new  SqlParameter{ParameterName="@fitnessAdvicedBy",Value=model.fitnessAdvicedBy??string.Empty},
                    new  SqlParameter{ParameterName="@opdDate",Value=model.opdDate??string.Empty},
                    new  SqlParameter{ParameterName="@mobileNo",Value=model.mobileNo},
                    new  SqlParameter{ParameterName="@emailId",Value=model.emailId??string.Empty},
                    new  SqlParameter{ParameterName="@transIp",Value=model.transIp},
            };

            var _proc = @"proc_FIC_InsertUpdate_ILCtoFIC  @regisIdFIC,@ILCcertificateNo,@regByUser,@applicationReason,@opdRecNo,@opdFilePath,
@fitnessAdvicedBy,@opdDate,@mobileNo,@emailId ,@transIp";

            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }

        public List<FICModel> GetFICList(long userId)
        {

            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@userId",Value=userId}
            };
            var _proc = @"proc_getFIC_Details @userId";
            var slist = this.Database.SqlQuery<FICModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public FICModel GetFICListBYRegistrationNo(string reg)
        {
            var sqlParam = new SqlParameter[] {  
             new SqlParameter{ParameterName="@registrationNo",Value=reg}  
            };
            var _proc = @"proc_getFIC_ListDetails @registrationNo";
            var slist = this.Database.SqlQuery<FICModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }

        public RegisterDetailsModel GetRegisterDetails()
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser}
            };
            var _proc = @"proc_RegisterDetailsFIC @regisId";
            var slist = this.Database.SqlQuery<RegisterDetailsModel>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public List<FICModel> rblforwardType()
        {
            var _proc = @"proc_IMC_forwardType";
            var slist = this.Database.SqlQuery<FICModel>(_proc).ToList();
            return slist;
        }
        public List<FICModel> bindDropdownlist(long rollId, int opdDistrictId)
        {
            var sqlparam = new SqlParameter[] {
            new SqlParameter{ParameterName="@procId",Value=1},
            new SqlParameter{ParameterName="@Id1",Value=rollId},
            new SqlParameter{ParameterName="@Id2",Value=opdDistrictId}
            };
            var _proc = @"proc_GetDataForDropDownListForwardTo @procId,@Id1,@Id2";
            var slist = this.Database.SqlQuery<FICModel>(_proc, sqlparam).ToList();
            return slist;
        }
        public ILCModel GetILCdetailByCertNo(string oldCertificateNumber)
        {
            SessionManager SM = new SessionManager();
            int UID = Convert.ToInt32(SM.UserID);
            var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@certificateNo",Value=oldCertificateNumber} ,
              new SqlParameter{ParameterName="@regByUser",Value=UID} 
            };
            var _proc = @"GetILCDetailByCrtificateNo @certificateNo";
            var slist = this.Database.SqlQuery<ILCModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public ILCModel GetLatestILCDetailByCrtificateNo(string oldCertificateNumber, long userId)
        { 
            var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@certificateNo",Value=oldCertificateNumber} ,
              new SqlParameter{ParameterName="@regByUser",Value=userId} 
            };
            var _proc = @"GetLatestILCDetailByCrtificateNo @certificateNo,@regByUser";
            var slist = this.Database.SqlQuery<ILCModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }

        public List<FICModel> GetFICdetailCertificateRpt(long userId)
        {
            var sqlparams = new SqlParameter[] { 
            
           
            new SqlParameter{ParameterName="@regisIdFIC",Value=userId}
             };
            var _proc = @"GetFICdetailForCertificateRpt  @regisIdFIC";
            var slist = this.Database.SqlQuery<FICModel>(_proc, sqlparams).ToList();
            return slist;
        }
        #region Delete Registration FIC
        public int DeleteRegistrationFIC(long regisIdFIC)
        {
            var sqlParams = new SqlParameter[] {    
                 new SqlParameter { ParameterName = "@regisIdFIC", Value = regisIdFIC} 
            };
            var query = "proc_DeleteRegistration_FIC @regisIdFIC";
            var result = this.Database.ExecuteSqlCommand(query, sqlParams);
            return result;
        }
        #endregion
    }
}

