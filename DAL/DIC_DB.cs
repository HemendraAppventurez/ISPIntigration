using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.DAL
{ 
    public class DIC_DB : DbContext
    {
        SessionManager objSM = new SessionManager();
        public DIC_DB()
            : base("CMSModule")
        {
        }
        #region Riya
        public ResultSet IsRegister()
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser}
            };
            var _proc = @"proc_checkuserDIC @regisId";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        //public DICModel GetDICById(long Id)
        //{
        //    var sqlParam = new SqlParameter[] { 
        //    new SqlParameter{ParameterName="@Id",Value=@Id}
        //    };
        //    var _proc = @"proc_getuserCommondetails @Id";
        //    var slist = this.Database.SqlQuery<DICModel>(_proc, sqlParam).SingleOrDefault();
        //    return slist;
        //}

        #region check email existence
        //public ResultSet CheckEmailMobileExistence(string checkValue, string Type)
        //{
        //    var sqlParams = new SqlParameter[] { 
        //            new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
        //            new SqlParameter { ParameterName = "@Type", Value = Type }

        //        };

        //    var sqlQuery = @"proc_checkEmailMobleExistenceDIC @checkValue,@Type";
        //    var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
        //    return sDetails;
        //}
        public ResultSet CheckEmailMobileExistence(string checkValue, string Type, long regisId)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@regisId", Value = regisId }

                };

            var sqlQuery = @"proc_checkEmailMobileExistenceDIC @checkValue,@Type,@regisId";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion
        public ResultSet DICInsertUpdateForRenewal(DICModel model)
        {
            var sqlparams = new SqlParameter[] { 
                    new  SqlParameter{ParameterName="@procId",Value=1},
                    new  SqlParameter{ParameterName="@regisIdDIC",Value=model.regisIdDIC},
                    new  SqlParameter{ParameterName="@regByUser", Value =model.regByUser} ,
                    new  SqlParameter{ParameterName="@ApplyingFor", Value =model.ApplyingFor} ,
                    new SqlParameter{ParameterName="@isCertFromPortal", Value=model.isCertFromPortal??string.Empty},//
	                new  SqlParameter{ParameterName="@releventProof", Value =model.releventProofpath??string.Empty} ,
                    new SqlParameter{ParameterName="@oldCertificateNumber", Value=model.oldCertificateNumber??string.Empty},//
                    //new  SqlParameter{ParameterName="@fullName", Value =model.fullName??string.Empty} ,
                    //new  SqlParameter{ParameterName="@fatherName", Value =model.fatherName??string.Empty} ,
                    //new  SqlParameter{ParameterName="@dob", Value =model.dob??string.Empty} ,
                    new  SqlParameter{ParameterName="@age",Value=model.age},//
                    //new  SqlParameter{ParameterName="@gender", Value =model.gender??string.Empty} ,
                    //new  SqlParameter{ParameterName="@categoryId", Value =model.categoryId} ,
                    //new  SqlParameter{ParameterName="@mobileNo", Value =model.mobileNo??string.Empty} ,
	                new  SqlParameter{ParameterName="@emailId", Value =model.emailId??string.Empty} ,
                    //new  SqlParameter{ParameterName="@adharNumber", Value =model.adharNumber??string.Empty},
	                new  SqlParameter{ParameterName="@stateId", Value =model.stateId} ,
	                new  SqlParameter{ParameterName="@districtId", Value =model.districtId} ,
	                new  SqlParameter{ParameterName="@address ", Value =model.address} ,
	                new  SqlParameter{ParameterName="@pinCode ", Value =model.pinCode} ,
                    new  SqlParameter{ParameterName="@disabilityTypeId ", Value =model.disabilityTypeId},
	                new  SqlParameter{ParameterName="@disabilityType ", Value =model.disabilityType??string.Empty} ,
	                new  SqlParameter{ParameterName="@disabilityDetail ", Value =model.disabilityDetail} ,
	                new  SqlParameter{ParameterName="@photoPath ", Value =model.photoPathpath??string.Empty} ,
                    new  SqlParameter{ParameterName="@passportsizephoto", Value =model.passportsizephotopath??string.Empty} ,
                    new  SqlParameter{ParameterName="@idProofId", Value =model.idProofId},
	                new  SqlParameter{ParameterName="@idProofPath ", Value =model.idProofPathpath??string.Empty} ,
                    new  SqlParameter{ParameterName="@photoIdNo ", Value =model.photoIdNo??string.Empty} ,
                    new  SqlParameter{ParameterName="@addressProofId", Value =model.addressProofId},
	                new  SqlParameter{ParameterName="@documentPath", Value =model.documentPathpath??string.Empty} ,
	                new  SqlParameter{ParameterName="@appearDate ", Value =model.appearDate??string.Empty} ,
                    new  SqlParameter{ParameterName="@thumbImpPath", Value =model.thumbImpPathpath??string.Empty} ,
	                new  SqlParameter{ParameterName="@isNotAppeared", Value =model.isNotAppeared} ,
	                new  SqlParameter{ParameterName="@regByTransIp", Value =model.regByTransIp} ,
	                new  SqlParameter{ParameterName="@transIp", Value =model.transIp} ,
                    new  SqlParameter{ParameterName="@requestKey", Value =model.requestKey??string.Empty}
         
            };

            var _proc = @"proc_DIC_InsertUpdateRenewal @procId,@regisIdDIC,@regByUser,@ApplyingFor,@isCertFromPortal,@releventProof,@oldCertificateNumber,@age,@emailId,@stateId,@districtId,@address ,@pinCode ,@disabilityTypeId,@disabilityType ,@disabilityDetail ,@photoPath ,@passportsizephoto,@idProofId,@idProofPath,@photoIdNo ,@addressProofId,@documentPath,@appearDate ,@thumbImpPath,@isNotAppeared,@regByTransIp,@transIp,@requestKey";

            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public ResultSet DICInsertUpdate(DICModel model)
        {
            var sqlparams = new SqlParameter[] { 
                    new  SqlParameter{ParameterName="@procId",Value=1},
                    new  SqlParameter{ParameterName="@regisIdDIC",Value=model.regByUser},
                    new  SqlParameter{ParameterName="@regByUser", Value =model.regByUser} ,
                    new  SqlParameter{ParameterName="@ApplyingFor", Value =model.ApplyingFor} ,
                    new SqlParameter{ParameterName="@isCertFromPortal", Value=model.isCertFromPortal??string.Empty},//
	                new  SqlParameter{ParameterName="@releventProof", Value =model.releventProofpath??string.Empty} ,
                    new SqlParameter{ParameterName="@oldCertificateNumber", Value=model.oldCertificateNumber??string.Empty},//
	                new  SqlParameter{ParameterName="@fullName", Value =model.fullName??string.Empty} ,
	                new  SqlParameter{ParameterName="@fatherName", Value =model.fatherName??string.Empty} ,
	                new  SqlParameter{ParameterName="@dob", Value =model.dob??string.Empty} ,
                    new  SqlParameter{ParameterName="@age",Value=model.age},//
	                new  SqlParameter{ParameterName="@gender", Value =model.gender??string.Empty} ,
	                new  SqlParameter{ParameterName="@categoryId", Value =model.categoryId} ,
	                new  SqlParameter{ParameterName="@mobileNo", Value =model.mobileNo??string.Empty} ,
	                new  SqlParameter{ParameterName="@emailId", Value =model.emailId??string.Empty} ,
                    new  SqlParameter{ParameterName="@adharNumber", Value =model.adharNumber??string.Empty},
	                new  SqlParameter{ParameterName="@stateId", Value =model.stateId} ,
	                new  SqlParameter{ParameterName="@districtId", Value =model.districtId} ,
	                new  SqlParameter{ParameterName="@address ", Value =model.address} ,
	                new  SqlParameter{ParameterName="@pinCode ", Value =model.pinCode} ,
                    new  SqlParameter{ParameterName="@disabilityTypeId ", Value =model.disabilityTypeId},
	                new  SqlParameter{ParameterName="@disabilityType ", Value =model.disabilityType??string.Empty} ,
	                new  SqlParameter{ParameterName="@disabilityDetail ", Value =model.disabilityDetail} ,
	                new  SqlParameter{ParameterName="@photoPath ", Value =model.photoPathpath??string.Empty} ,
                    new  SqlParameter{ParameterName="@passportsizephoto", Value =model.passportsizephotopath??string.Empty} ,
                    new  SqlParameter{ParameterName="@idProofId", Value =model.idProofId},
	                new  SqlParameter{ParameterName="@idProofPath ", Value =model.idProofPathpath??string.Empty} ,
                    new  SqlParameter{ParameterName="@photoIdNo ", Value =model.photoIdNo??string.Empty} ,
                    new  SqlParameter{ParameterName="@addressProofId", Value =model.addressProofId},
	                new  SqlParameter{ParameterName="@documentPath", Value =model.documentPathpath??string.Empty} ,
	                new  SqlParameter{ParameterName="@appearDate ", Value =model.appearDate??string.Empty} ,
                    new  SqlParameter{ParameterName="@thumbImpPath", Value =model.thumbImpPathpath??string.Empty} ,
	                new  SqlParameter{ParameterName="@isNotAppeared", Value =model.isNotAppeared} ,
	                new  SqlParameter{ParameterName="@regByTransIp", Value =model.regByTransIp} ,
	                new  SqlParameter{ParameterName="@transIp", Value =model.transIp} ,
                    new  SqlParameter{ParameterName="@requestKey", Value =model.requestKey??string.Empty}
         
            };

            var _proc = @"proc_DIC_InsertUpdate @procId,@regisIdDIC,@regByUser,@ApplyingFor,@isCertFromPortal,@releventProof,@oldCertificateNumber,@fullName,@fatherName,@dob,@age,@gender,@categoryId,@mobileNo,@emailId,@adharNumber,@stateId,@districtId,@address ,@pinCode ,@disabilityTypeId,@disabilityType ,@disabilityDetail ,@photoPath ,@passportsizephoto,@idProofId,@idProofPath,@photoIdNo ,@addressProofId,@documentPath,@appearDate ,@thumbImpPath,@isNotAppeared,@regByTransIp,@transIp,@requestKey";

            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public List<DICModel> GetDICList(long userID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=1}  ,
              new SqlParameter{ParameterName="@userId",Value=userID} 
            };
            var _proc = @"proc_getDICList @procId,@userId";
            var slist = this.Database.SqlQuery<DICModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public DICModel GetDICListBYRegistrationNo(long regisID, string registration)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=2}  ,
             new SqlParameter{ParameterName="@userId",Value=regisID} ,
              new SqlParameter{ParameterName="@registration",Value=registration} 
            };
            var _proc = @"proc_getDICList @procId,@userId ,@registration";
            var slist = this.Database.SqlQuery<DICModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public RegisterDetailsModel GetRegisterDetails()
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser}
            };
            var _proc = @"proc_RegisterDetailsDIC @regisId";
            var slist = this.Database.SqlQuery<RegisterDetailsModel>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public DICModel GetDICdetailByCertNo(string oldCertificateNumber, long UserID)
        {
            var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@certificateNo",Value=oldCertificateNumber} ,
              new SqlParameter{ParameterName="@UserID",Value=UserID} 
            };
            var _proc = @"GetDICDetailByCrtificateNo @certificateNo,@UserID";
            var slist = this.Database.SqlQuery<DICModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        #endregion

        #region Delete Registration DIC
        public int DeleteRegistrationDIC(long regisIdDIC)
        {
            var sqlParams = new SqlParameter[] {    
                 new SqlParameter { ParameterName = "@regisIdDIC", Value = regisIdDIC} 
            };
            var query = "proc_DeleteRegistration_DIC @regisIdDIC";
            var result = this.Database.ExecuteSqlCommand(query, sqlParams);
            return result;
        }
        #endregion
    }
}