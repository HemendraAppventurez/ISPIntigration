using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class AGC_DB:DbContext
    {
         public AGC_DB()
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
             var _proc = @"proc_checkuserAGC @regisId";
             var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
             return slist;
         }

         #region check email existence
         public ResultSet CheckEmailMobileExistence(string checkValue, string Type, long userId)
         {
             var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@userId", Value = userId } 
                };

             var sqlQuery = @"proc_checkEmailMobleExistenceAGC @checkValue,@Type,@userId";
             var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
             return sDetails;
         }
         #endregion

         public ResultSet AGCInsertUpdate(AGCModel model)
         {
             var sqlparams = new SqlParameter[] { 
                    new  SqlParameter{ParameterName="@ProcId",Value=1},
                    new  SqlParameter{ParameterName="@regByUser",Value=model.regByUser},
	                new  SqlParameter{ParameterName="@applicantTypeId",Value=model.applicantTypeId},
	                new  SqlParameter{ParameterName="@applicantSubTypeId",Value=model.applicantSubTypeId},
	                new  SqlParameter{ParameterName="@orderDetail",Value=model.orderDetail},
                    new  SqlParameter{ParameterName="@orderNo",Value=model.orderNo},
	                new  SqlParameter{ParameterName="@orderDate",Value=model.orderDate},
	                new  SqlParameter{ParameterName="@orderFile",Value=model.orderFilePath},
	                new  SqlParameter{ParameterName="@fullName",Value=model.fullName},
	                new  SqlParameter{ParameterName="@mobileNo",Value=model.mobileNo},
	                new  SqlParameter{ParameterName="@emailId",Value=(model.emailId==null)?"":model.emailId},
	                new  SqlParameter{ParameterName="@gender",Value=model.gender},
	                new  SqlParameter{ParameterName="@address",Value=model.address},
	                new  SqlParameter{ParameterName="@stateId",Value=model.stateId},
	                new  SqlParameter{ParameterName="@districtId",Value=model.districtId},
	                new  SqlParameter{ParameterName="@pinCode",Value=model.pinCode},
	                new  SqlParameter{ParameterName="@idTypeId",Value=model.idTypeId},
	                new  SqlParameter{ParameterName="@idNumber",Value=model.idNumber},
	                new  SqlParameter{ParameterName="@documentFile",Value=model.documentFilePath},
	                new  SqlParameter{ParameterName="@susName",Value=model.susName},
	                new  SqlParameter{ParameterName="@susFatherName",Value=model.susFatherName},
	                new  SqlParameter{ParameterName="@susMotherName",Value=model.susMotherName},
                    new  SqlParameter{ParameterName="@susFatherAge",Value=model.susFatherAge??(object)DBNull.Value},  
	                new  SqlParameter{ParameterName="@susMotherAge",Value=model.susMotherAge??(object)DBNull.Value},  
	                new  SqlParameter{ParameterName="@susMobileNo",Value=model.susMobileNo??string.Empty},  
	                new  SqlParameter{ParameterName="@susEmail",Value=(model.susEmail==null)?"":model.susEmail},
	                new  SqlParameter{ParameterName="@appointmentDate",Value=model.appointmentDate},
                    new  SqlParameter{ParameterName="@susaddress",Value=model.susaddress},
	                new  SqlParameter{ParameterName="@susstateId",Value=model.susstateId},
	                new  SqlParameter{ParameterName="@susdistrictId",Value=model.susdistrictId},
	                new  SqlParameter{ParameterName="@suspinCode",Value=model.suspinCode},
                    new  SqlParameter{ParameterName="@markOfIdentification",Value=model.markOfIdentification},
	                new  SqlParameter{ParameterName="@susphotoFilePath",Value=model.susphotoFilePath},
                    new  SqlParameter{ParameterName="@susThumbFilePath",Value=model.susThumbFilePath},
	                new  SqlParameter{ParameterName="@regByIp",Value=model.regByIp},
	                new  SqlParameter{ParameterName="@transIp",Value=model.transIp},
                    new SqlParameter { ParameterName = "@requestKey", Value =model.requestKey??string.Empty},
                    new SqlParameter { ParameterName = "@applicantSubTypeOther", Value =model.applicantSubTypeOther??string.Empty}

            };

             var _proc = @"proc_AGCDetails_InsertUpdate @ProcId,@regByUser,@applicantTypeId,@applicantSubTypeId,@orderDetail,@orderNo,
@orderDate,@orderFile,@fullName,@mobileNo,@emailId,@gender,@address,@stateId,@districtId,@pinCode,@idTypeId,@idNumber,@documentFile,
@susName,@susFatherName,@susMotherName,@susFatherAge,@susMotherAge,@susMobileNo,@susEmail,@appointmentDate,@susaddress,@susstateId,@susdistrictId,@suspinCode,
@markOfIdentification,@susphotoFilePath,@susThumbFilePath,@regByIp,@transIp,@requestKey,@applicantSubTypeOther";

             var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
             return slist;
         }
         public List<AGCModel> GetAGCList(long userID)
         {
             var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=1}  ,
              new SqlParameter{ParameterName="@userId",Value=userID} 
            };
             var _proc = @"proc_getAGCList @procId,@userId";
             var slist = this.Database.SqlQuery<AGCModel>(_proc, sqlParam).ToList();
             return slist;
         }
         public AGCModel GetAGCListBYRegistrationNo(long regisID, string registration)
         {
             var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=2}  ,
             new SqlParameter{ParameterName="@userId",Value=regisID} ,
              new SqlParameter{ParameterName="@registration",Value=registration} 
            };
             var _proc = @"proc_getAGCList @procId,@userId ,@registration";
             var slist = this.Database.SqlQuery<AGCModel>(_proc, sqlParam).SingleOrDefault();
             return slist;
         }

         public RegisterDetailsModel GetRegisterDetails()
         {
             long regisByuser = objSM.UserID;
             var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser}
            };
             var _proc = @"proc_RegisterDetailsAGC @regisId";
             var slist = this.Database.SqlQuery<RegisterDetailsModel>(_proc, sqlparams).FirstOrDefault();
             return slist;
         }

         #region Delete Registration AGC
         public int DeleteRegistrationAGC(long regisIdAGC)
         {
             var sqlParams = new SqlParameter[] {    
                 new SqlParameter { ParameterName = "@regisIdAGC", Value = regisIdAGC} 
            };
             var query = "proc_DeleteRegistration_AGC @regisIdAGC";
             var result = this.Database.ExecuteSqlCommand(query, sqlParams);
             return result;
         }
         #endregion


         public AGCModel GetAGCListBYRegistrationforumang(string registration)
         {
             var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=1}  ,
            // new SqlParameter{ParameterName="@userId",Value=regisID} ,
              new SqlParameter{ParameterName="@registration",Value=registration} 
            };
             var _proc = @"proc_getAGCListApi @procId,@registration";
             var slist = this.Database.SqlQuery<AGCModel>(_proc, sqlParam).SingleOrDefault();
             return slist;
         }

        public List<ILCModel> GetILCdetailCertificateRpt(long userId)
        {
            var sqlparams = new SqlParameter[] {


            new SqlParameter{ParameterName="@regisIdILC",Value=userId}
             };
            var _proc = @"GetILCdetailForCertificateRpt  @regisIdILC";
            var slist = this.Database.SqlQuery<ILCModel>(_proc, sqlparams).ToList();
            return slist;
        }
    }
}