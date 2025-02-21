using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class DEC_DB : DbContext
    {
         public DEC_DB()
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
             var _proc = @"proc_checkuserDEC @regisId";
             var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
             return slist;
         }
         public DECModel GetDECById(long Id)
         {
             var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@Id",Value=@Id}
            };
             var _proc = @"proc_getuserCommondetails @Id";
             var slist = this.Database.SqlQuery<DECModel>(_proc, sqlParam).SingleOrDefault();
             return slist;
         }

         #region check email existence
         public ResultSet CheckEmailMobileExistence(string checkValue, string Type)
         {
             var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type }

                };

             var sqlQuery = @"proc_checkEmailMobleExistenceDEC @checkValue,@Type";
             var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
             return sDetails;
         }
         #endregion

         public ResultSet DECInsertUpdate(DECModel model)
         {
                    var sqlparams = new SqlParameter[] {                  
                    new  SqlParameter{ParameterName="@ProcId",Value=1},
                    new  SqlParameter{ParameterName="@regByUser",Value=model.regByusers},                    
	                new  SqlParameter{ParameterName="@fullName",Value=model.fullName},
	                new  SqlParameter{ParameterName="@mobileNo",Value=model.mobileNo},
                    new  SqlParameter{ParameterName="@emailId",Value=model.emailId},
                    new  SqlParameter{ParameterName="@address",Value=model.address},
                    new  SqlParameter{ParameterName="@stateId",Value=model.stateId},
                    new  SqlParameter{ParameterName="@districtId",Value=model.districtid},
                    new  SqlParameter{ParameterName="@Pincode",Value=model.pinCode},
                    new  SqlParameter{ParameterName="@relationWithDeathPerson",Value=model.relationId},
                    new  SqlParameter{ParameterName="@forwardtypeId",Value=model.forwardtypeId},
                    new  SqlParameter{ParameterName="@healthUnitDistrictId",Value=model.healthUnitDistrictId},
                    new  SqlParameter{ParameterName="@forwardtoId",Value=model.forwardtoId},
                    new  SqlParameter{ParameterName="@DeathPersonName",Value=model.deathPersonName},
                    new  SqlParameter{ParameterName="@DeathPersonAadhaarNo",Value=model.aadhaarNo??String.Empty},
                    new  SqlParameter{ParameterName="@DeathPersonGender",Value=model.DeathPersonGender},
                    new  SqlParameter{ParameterName="@maritalStatusId",Value=model.maritalStatusId},
                    new  SqlParameter{ParameterName="@religionId",Value=model.religionId},
                    new  SqlParameter{ParameterName="@spouseName",Value=model.spouseName??string.Empty},
                    new  SqlParameter{ParameterName="@fatherName",Value=model.fathersName},
                    new  SqlParameter{ParameterName="@motherName",Value=model.motherName},
                    new  SqlParameter{ParameterName="@dod",Value=model.dod},
                    //new  SqlParameter{ParameterName="@isCauseCertified",Value=model.isCauseCertified},
                    //new  SqlParameter{ParameterName="@diseaseNameOrCause",Value=model.diseaseNameOrCause??String.Empty},
	                new  SqlParameter{ParameterName="@isInfoCorrect",Value=model.isInfoCorrect},
	                new  SqlParameter{ParameterName="@regBytransIp",Value=model.regBytransIp},
	                new  SqlParameter{ParameterName="@transIp",Value=model.transIp},
                    new SqlParameter {ParameterName = "@requestKey", Value =model.requestKey??string.Empty}                 
         
            };

                    var _proc = @"dbo.proc_DEC_InsertUpdate  @ProcId , @regByuser , @fullName, @mobileNo, @emailId , @address , @stateId ,@districtId , @Pincode , @relationWithDeathPerson ,@forwardtypeId,@healthUnitDistrictId,@forwardtoId, @DeathPersonName , @DeathPersonAadhaarNo , @DeathPersonGender , @maritalStatusId ,@religionId,@spouseName , @fatherName ,@motherName , @dod , @isCauseCertified , @diseaseNameOrCause , @isInfoCorrect , @regBytransIp ,@transIp,@requestKey";
             var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
             return slist;
         }
         public List<DECModel> GetDECList(long userID)
         {
             var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=1}  ,
              new SqlParameter{ParameterName="@userId",Value=userID} 
            };
             var _proc = @"proc_getDECList @procId,@userId";
             var slist = this.Database.SqlQuery<DECModel>(_proc, sqlParam).ToList();
             return slist;
         }
         public DECModel GetDECListBYRegistrationNo(long regisID, string registration)
         {
             var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=2}  ,
             new SqlParameter{ParameterName="@userId",Value=regisID} ,
              new SqlParameter{ParameterName="@registration",Value=registration} 
            };
             var _proc = @"proc_getDECList @procId,@userId ,@registration";
             var slist = this.Database.SqlQuery<DECModel>(_proc, sqlParam).SingleOrDefault();
             return slist;
         }
         public RegisterDetailsModel GetRegisterDetails()
         {
             long regisByuser = objSM.UserID;
             var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser}
            };
             var _proc = @"proc_RegisterDetailsDEC @regisId";
             var slist = this.Database.SqlQuery<RegisterDetailsModel>(_proc, sqlparams).FirstOrDefault();
             return slist;
         }
         public List<DECModel> rblforwardType()
         {
             var _proc = @"proc_IMC_forwardType";
             var slist = this.Database.SqlQuery<DECModel>(_proc).ToList();
             return slist;
         }
         public List<DECModel> bindDropdownlist(long rollId, int opdDistrictId)
         {
             var sqlparam = new SqlParameter[] {
            new SqlParameter{ParameterName="@procId",Value=1},
            new SqlParameter{ParameterName="@Id1",Value=rollId},
            new SqlParameter{ParameterName="@Id2",Value=opdDistrictId}
            };
             var _proc = @"proc_GetDataForDropDownListForwardTo @procId,@Id1,@Id2";
             var slist = this.Database.SqlQuery<DECModel>(_proc, sqlparam).ToList();
             return slist;
         }
         #region Delete Registration DEC
         public int DeleteRegistrationDEC(long regisIdDEC)
         {
             var sqlParams = new SqlParameter[] {    
                 new SqlParameter { ParameterName = "@regisIdFAP", Value = regisIdDEC} 
            };
             var query = "proc_DeleteRegistration_DEC @regisIdDEC";
             var result = this.Database.ExecuteSqlCommand(query, sqlParams);
             return result;
         }
         #endregion

         public List<DECModel> GetDECDetails(long regisIdDEC)
         {
             var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisIdDEC",Value=regisIdDEC}  
            };
             var _proc = @"GetDECdetailForCertificateRpt @regisIdDEC";
             var slist = this.Database.SqlQuery<DECModel>(_proc, sqlParam).ToList();
             return slist;
         }
         public DECModel GetDECListCHC(long regisID)
         {
             var sqlParam = new SqlParameter[] { 
         
             new SqlParameter{ParameterName="@userId",Value=regisID}
            };
             var _proc = @"proc_getDECListCHC @userId ";
             var slist = this.Database.SqlQuery<DECModel>(_proc, sqlParam).SingleOrDefault();
             return slist;
         }

        #region Nominee

         public List<DECModel> GetAllNomineeSearchListDEC(long forwardtypeId, int healthUnitDistrictId, long forwardtoId, string deathPersonName, string dod, string DeathPersonGender)
         {
             var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@forwardtypeId",Value=forwardtypeId}  ,
             new SqlParameter{ParameterName="@healthUnitDistrictId",Value=healthUnitDistrictId}  ,
             new SqlParameter{ParameterName="@forwardtoId",Value=forwardtoId},
             new SqlParameter{ParameterName="@deathPersonName",Value=deathPersonName}  ,
             new SqlParameter{ParameterName="@dod",Value=dod}  ,
             new SqlParameter{ParameterName="@DeathPersonGender",Value=DeathPersonGender}
             
            };
             var _proc = @"proc_GetAllNomineeSearchListDEC @forwardtypeId,@healthUnitDistrictId,@forwardtoId,@deathPersonName,@dod,@DeathPersonGender";
             var slist = this.Database.SqlQuery<DECModel>(_proc, sqlParam).ToList();
             return slist;
         }
         public ResultSet DECInsertUpdateNomineeDetail(DECModel model)
         {
             var sqlparams = new SqlParameter[] { 
                              
	                //new  SqlParameter{ParameterName="@regisIdDEC",Value=model.regisIdDEC},
	                new  SqlParameter{ParameterName="@nomineeName",Value=model.nomineeName},
                    new  SqlParameter{ParameterName="@bloodRelationId",Value=model.bloodRelationId},
                    new  SqlParameter{ParameterName="@NomineeMobileNo",Value=model.NomineeMobileNo},
                    new  SqlParameter{ParameterName="@idProofId",Value=model.idProofId},
                    new  SqlParameter{ParameterName="@idProofFile",Value=model.idProofFilePath},
                    new  SqlParameter{ParameterName="@regByUser",Value=model.regByusers}, 
	                new  SqlParameter{ParameterName="@transIp",Value=model.transIp}               
         
            };

             var _proc = @"dbo.proc_DEC_InsertUpdate_NomineeDeatil  @nomineeName,@bloodRelationId ,@NomineeMobileNo ,@idProofId ,@idProofFile ,@regByUser ,@transIp";
             var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
             return slist;
         }
         public ResultSet InsertNomineeLog(Int64 regisIdNomineeDEC,Int64 regisIdDEC,string dType)
         {
             var sqlparams = new SqlParameter[] { 
                              
	                //new  SqlParameter{ParameterName="@regisIdDEC",Value=model.regisIdDEC},
	                new  SqlParameter{ParameterName="@regisIdNomineeDEC",Value=regisIdNomineeDEC},
                    new  SqlParameter{ParameterName="@regisIdDEC",Value=regisIdDEC},
                    new  SqlParameter{ParameterName="@dType",Value=dType}              
         
            };

             var _proc = @"dbo.proc_InsertNomineeLog_DEC  @regisIdNomineeDEC,@regisIdDEC,@dType";
             var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
             return slist;
         }
         public DECModel getUserDetail(long regisIdDEC, long UserID)
         {
             var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisIdDEC",Value=regisIdDEC} ,
              new SqlParameter{ParameterName="@UserID",Value=UserID} 
            };
             var _proc = @"proc_getUserDetail @regisIdDEC,@UserID";
             var slist = this.Database.SqlQuery<DECModel>(_proc, sqlParam).SingleOrDefault();
             return slist;
         }
     
         public List<DECotpModel> GetUserDetailsDEC(string UserName, Int64 rollId)
         {
             var sqlParams = new SqlParameter[] {                 
                new SqlParameter { ParameterName = "@userName", Value =UserName  },
                 new SqlParameter { ParameterName = "@rollId", Value =rollId  }
            };

             var sqlProc = "Proc_GetUserLoginDetailsDEC @userName,@rollId";
             var sList = this.Database.SqlQuery<DECotpModel>(sqlProc, sqlParams).ToList();
             return sList;
         }
         public List<DECModel> GetDECListDEC(long userID)
         {
             var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@userId",Value=userID} 
            };
             var _proc = @"proc_getDECnomineeList @userId";
             var slist = this.Database.SqlQuery<DECModel>(_proc, sqlParam).ToList();
             return slist;
         }
         public int InsertUnSignedCertiPath_DEC(long regisId, string certificateFilePath)
         {
             var sqlParam = new SqlParameter[] { 
                 new SqlParameter{ParameterName="@regisIdDEC",Value=regisId},  
                 new SqlParameter{ParameterName="@certificateFilePath",Value=certificateFilePath}  
            };
             var _proc = @"proc_InsertUnSignedCertiPath_DEC @regisIdDEC,@certificateFilePath";
             var result = this.Database.ExecuteSqlCommand(_proc, sqlParam);
             return result;
         }
        #endregion

    }
}