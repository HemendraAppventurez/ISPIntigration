using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class IMC_DB : DbContext
    {
        SessionManager objSM = new SessionManager();
        #region Default Constructor
        public IMC_DB()
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
            var _proc = @"proc_checkuserIMC @regisId";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public IMCModel GetIMCById(long Id)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@Id",Value=@Id}
            };
            var _proc = @"proc_getuserCommondetails @Id";
            var slist = this.Database.SqlQuery<IMCModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public List<IMCModel> bindDropdownlist(long rollId,int DistrictId)
        {
            var sqlparam = new SqlParameter[] {
            new SqlParameter{ParameterName="@procId",Value=1},
            new SqlParameter{ParameterName="@Id1",Value=rollId},
            new SqlParameter{ParameterName="@Id2",Value=DistrictId}
            };
            var _proc = @"proc_GetDataForDropDownListForwardTo @procId,@Id1,@Id2";
            var slist = this.Database.SqlQuery<IMCModel>(_proc, sqlparam).ToList();
            return slist;
        }
        public List<IMCModel> rblforwardType()
        {
            var _proc = @"proc_IMC_forwardType";
            var slist = this.Database.SqlQuery<IMCModel>(_proc).ToList();
            return slist;
        }

        #region check email existence
        public ResultSet CheckEmailMobileExistence(string checkValue, string Type)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type }

                };

            var sqlQuery = @"proc_checkEmailMobleExistenceIMC @checkValue,@Type";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion
       
        //public ResultSet InsertUpdate_IMC(int procId, long regisIdIMC, long regisByuser,bool isVaccined, string fullName, string fatherName, string dob, int  age, string mobileNo,
        //    string emailId, string passportNo, int stateId, int districtId, string address, string pinCode,string markOfIdentification, int immuCertiTypeId, string reason,
        //    string opdReciept,string opdFilePath, bool isAlreadyTaken,long forwardType,
        //    long forwardtoId, string DH_PHC_CHCProofFilePath, string DH_PHC_CHCDate, string hospitalEstablishment,  bool isCertify,
        //    string regByTransIp, string transIp, long forwardDistrictId, string requestKey,string xmldata)
        public ResultSet InsertUpdate_IMC( long regisIdIMC, long regisByuser, 
            bool isVaccined, string reason,string opdReciept, string opdFilePath,
            string fullName, string fatherName, string dob, int age, string mobileNo,string emailId, string passportNo, 
            string address,int stateId, int districtId,  string pinCode, string markOfIdentification, 
        long forwardType,long forwardtoId,long forwardDistrictId, string transIp,string requestKey, string xmldata)
        {
            var sqlparams = new SqlParameter[] { 
            
            new SqlParameter{ParameterName="@regisIdIMC",Value=regisIdIMC},
            new SqlParameter{ParameterName="@regByUser",Value=regisByuser},
            new SqlParameter{ParameterName="@isVaccined",Value=isVaccined},
            new SqlParameter{ParameterName="@reason",Value=reason},
            new SqlParameter { ParameterName = "@opdReciept",Value=opdReciept??string.Empty},
            new SqlParameter { ParameterName = "@opdFile",Value=opdFilePath??string.Empty},


            new SqlParameter{ParameterName="@fullName",Value=fullName},
            new SqlParameter{ParameterName="@fatherName",Value=fatherName},
            new SqlParameter{ParameterName="@dob",Value=Convert.ToDateTime(dob)},
            new SqlParameter{ParameterName="@age",Value=age},
            new SqlParameter{ParameterName="@mobileNo",Value=mobileNo},
            new SqlParameter{ParameterName="@emailId",Value=emailId??string.Empty},
            new SqlParameter{ParameterName="@passportNo",Value=passportNo??string.Empty},
            new SqlParameter{ParameterName="@address",Value=address},
            new SqlParameter{ParameterName="@stateId",Value=stateId},
            new SqlParameter{ParameterName="@districtId",Value=districtId},
            new SqlParameter{ParameterName="@pinCode",Value=pinCode??string.Empty},
             new SqlParameter { ParameterName = "@markOfIdentification",Value=markOfIdentification},
            
           new SqlParameter{ParameterName="@DH_PHC_CHCTypeId",Value=forwardType},
            new SqlParameter{ParameterName="@forwardDistrictId",Value=forwardDistrictId},
            new SqlParameter{ParameterName="@forwardtoId",Value=forwardtoId},
            
            
            new SqlParameter{ParameterName="@transIp",Value=transIp},
            new SqlParameter { ParameterName = "@requestKey", Value =requestKey??string.Empty},
            new SqlParameter { ParameterName = "@xmldata", Value =xmldata??string.Empty},

           
           
            };
            var _proc = @"proc_InsertUpdateIMC  @regisIdIMC ,@regByUser,@isVaccined ,@reason ,@opdReciept ,@opdFile ,
@fullName,@fatherName ,@dob ,@age ,@mobileNo ,@emailId ,@passportNo ,@address ,@stateId ,@districtId ,@pinCode ,@markOfIdentification ,@forwardtoId,@forwardDistrictId ,@DH_PHC_CHCTypeId ,
@transIp ,@requestKey ,@xmldata ";
//            var _proc = @"proc_InsertUpdateIMC  @procId,@regisIdIMC,@regisByuser,@fullName,@fatherName,@dob,@age,@mobileNo,@emailId,@passportNo,@stateId,@districtId,@address,
//@pinCode,@immuCertiTypeId,@reason,@isAlreadyTaken,@forwardtoId,@forwardDistrictId,@DH_PHC_CHCTypeId,@DH_PHC_CHCProofFilePath,@DH_PHC_CHCDate,@hospitalEstablishment,@isCertify,
//@regByTransIp,@transIp,@requestKey,@xmldata,@isVaccined,@opdReciept,@opdFile,@markOfIdentification";

            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;

        }
        public List<IMCModel> getIMCList(long userId)
        {
            var sqlparams = new SqlParameter[] {
             new SqlParameter{ParameterName="@procId",Value=1},
             new SqlParameter{ParameterName="@userId",Value=userId}
            };
            var _proc = @"proc_getIMC @procId,@userId";
            var slist = this.Database.SqlQuery<IMCModel>(_proc,sqlparams).ToList();
            return slist;
        }
        public IMCModel IMCDetailsByRegistration(long userId)
        {
            var sqlparams = new SqlParameter[] {
             new SqlParameter{ParameterName="@procId",Value=2},
             new SqlParameter{ParameterName="@userId",Value=userId}
              
            };
            var _proc = @"proc_getIMC @procId,@userId";
            var slist = this.Database.SqlQuery<IMCModel>(_proc, sqlparams).SingleOrDefault();
            return slist;
        }
        public List<IMCModel> IMCVaccineById(long regisId)
        {
            var sqlparams = new SqlParameter[] {            
             new SqlParameter{ParameterName="@regisId",Value=regisId}
              
            };
            var _proc = @"proc_getIMCVaccine @regisId";
            var slist = this.Database.SqlQuery<IMCModel>(_proc, sqlparams).ToList();
            return slist;
        }
        public RegisterDetailsModel GetRegisterDetails()
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser}
            };
            var _proc = @"proc_RegisterDetailsIMC @regisId";
            var slist = this.Database.SqlQuery<RegisterDetailsModel>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        #endregion
        #region Delete Registration IMC
        public int DeleteRegistrationIMC(long regisIdIMC)
        {
            var sqlParams = new SqlParameter[] {    
                 new SqlParameter { ParameterName = "@regisIdIMC", Value = regisIdIMC} 
            };
            var query = "proc_DeleteRegistration_IMC @regisIdIMC";
            var result = this.Database.ExecuteSqlCommand(query, sqlParams);
            return result;
        }
        #endregion

        #region Riya 
        public List<IMCModel> GetDetailcertificate(long regisIdIMC)
        {
            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisIdIMC",Value=Convert.ToInt32( regisIdIMC)}  
            };
            var _proc = @"proc_rptIMC_Certificate @regisIdIMC";
            var slist = this.Database.SqlQuery<IMCModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public int InsertUnSignedCertiPath_IMC(long regisId, string certificateFilePath)
        {
            var sqlParam = new SqlParameter[] { 
                 new SqlParameter{ParameterName="@regisIdIMC",Value=regisId},  
                 new SqlParameter{ParameterName="@certificateFilePath",Value=certificateFilePath}  
            };
            var _proc = @"proc_InsertUnSignedCertiPath_IMC @regisIdIMC,@certificateFilePath";
            var result = this.Database.ExecuteSqlCommand(_proc, sqlParam);
            return result;
        }
        public List<IMCModel> BindImmunizationDetails()
        {

            var sqlProc = @"Proc_GetImmunizationTypeIMC";
            var list = this.Database.SqlQuery<IMCModel>(sqlProc).ToList();
            return list;

        }

        public List<IMCimmunizationModel> getIMCimmunList(long regisNUHId)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisNUHId}  
            };
            var _proc = @"proc_getIMCimmunList @regisId";
            var slist = this.Database.SqlQuery<IMCimmunizationModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public List<IMCAppProcessModel> getIMCimmunTableForCHC(long regisIdIMC)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisIdIMC}  
            };
            var _proc = @"proc_getIMCimmunTableForCHC @regisId";
            var slist = this.Database.SqlQuery<IMCAppProcessModel>(_proc, sqlParam).ToList();
            return slist;
        }
        #endregion
    }
}