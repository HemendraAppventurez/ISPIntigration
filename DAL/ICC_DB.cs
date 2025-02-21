using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class ICC_DB : DbContext
    {
        public ICC_DB()
            : base("CMSModule")
        {
        }
        SessionManager objSM = new SessionManager();
        #region Riya
        public ResultSet IsRegister()
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser}
            };
            var _proc = @"proc_checkuserICC @regisId";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }

        public ICCModel GetICCById(long Id)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@Id",Value=@Id}
            };
            var _proc = @"proc_getuserCommondetails @Id";
            var slist = this.Database.SqlQuery<ICCModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }

        #region check email existence
        public ResultSet CheckEmailMobileExistence(string checkValue, string Type)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type }

                };

            var sqlQuery = @"proc_checkEmailMobleExistenceICC @checkValue,@Type";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion

        public ResultSet ICCInsertUpdate(ICCModel model)
        {
            var sqlparams = new SqlParameter[] { 
                    new  SqlParameter{ParameterName="@ProcId",Value=1},
                    new  SqlParameter{ParameterName="@regisByuser",Value=model.regisByuser},
	                new  SqlParameter{ParameterName="@fullName",Value=model.fullName??string.Empty},

	                new  SqlParameter{ParameterName="@fatherName ",Value=(model.fatherName==null)?"":model.fatherName},
                    new  SqlParameter{ParameterName="@motherName ",Value=(model.motherName==null)?"":model.motherName},
                   // new SqlParameter{ParameterName="@relation",Value=model.relation},
	                new  SqlParameter{ParameterName="@dob",Value=(model.dob==null)?"":model.dob},
	                new  SqlParameter{ParameterName="@mobileNo",Value=(model.mobileNo==null)?"":model.mobileNo},
	                new  SqlParameter{ParameterName="@emailId",Value=model.emailId??string.Empty},
	                new  SqlParameter{ParameterName="@stateId",Value=model.stateId},
	                new  SqlParameter{ParameterName="@districtId",Value=model.districtId},
	                new  SqlParameter{ParameterName="@address",Value=model.address},
	                new  SqlParameter{ParameterName="@pinCode",Value=model.pinCode??string.Empty},
                    new SqlParameter{ParameterName="@immunizationBook",Value=model.immunizationBookpath},
                    new SqlParameter{ParameterName="@immunizationBackSideBook",Value=model.immunizationBackSideBookpath},
	                new  SqlParameter{ParameterName="@regBytransIp",Value=model.regBytransIp},
	                new  SqlParameter{ParameterName="@transIP",Value=model.transIP},
	                new  SqlParameter{ParameterName="@isConfirm",Value=model.isConfirm},
                    new SqlParameter { ParameterName = "@ImmunizationDetails", Value =model.XmlData} ,
                    new SqlParameter { ParameterName = "@requestKey", Value =model.requestKey??string.Empty},
                    new  SqlParameter{ParameterName="@forwardtypeId", Value =model.forwardtypeId} ,
                    new  SqlParameter{ParameterName="@forwardtoId", Value =model.forwardtoId} ,
                    new  SqlParameter{ParameterName="@healthUnitDistrictId", Value =model.healthUnitDistrictId}                    
            };

            var _proc = @"proc_ICC_InsertUpdate @ProcId,@regisByuser,@fullName,@fatherName,@motherName,@dob,@mobileNo,@emailId,@stateId,@districtId,@address,@pinCode,@immunizationBook,@immunizationBackSideBook,@regBytransIp,@transIP,@isConfirm,@ImmunizationDetails,@requestKey,@forwardtypeId,@forwardtoId,@healthUnitDistrictId";

            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public List<ICCModel> GetICCList(long userID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=1}  ,
              new SqlParameter{ParameterName="@userId",Value=userID} 
            };
            var _proc = @"proc_getICCList @procId,@userId";
            var slist = this.Database.SqlQuery<ICCModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public ICCModel GetICCListBYRegistrationNo(long regisID, string registration, long regisIdICC = 0)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=2}  ,
             new SqlParameter{ParameterName="@userId",Value=regisID} ,
              new SqlParameter{ParameterName="@registration",Value=registration} ,
              new SqlParameter{ParameterName="@regisIdICC",Value=regisIdICC} 
            };
            var _proc = @"proc_getICCList @procId,@userId ,@registration,@regisIdICC";
            var slist = this.Database.SqlQuery<ICCModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public List<ICCModel> getICCChild(string reg)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@reg",Value=reg}  
            };
            var _proc = @"proc_getICCchild @reg";
            var slist = this.Database.SqlQuery<ICCModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public RegisterDetailsModel GetRegisterDetails()
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser}
            };
            var _proc = @"proc_RegisterDetailsICC @regisId";
            var slist = this.Database.SqlQuery<RegisterDetailsModel>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public List<ICCModel> BindImmunizationDetails()
        {

            var sqlProc = @"Proc_GetImmunizationType";
            var list = this.Database.SqlQuery<ICCModel>(sqlProc).ToList();
            return list;

        }
       
        #endregion
        #region Delete Registration ICC
        public int DeleteRegistrationICC(long regisIdICC)
        {
            var sqlParams = new SqlParameter[] {    
                 new SqlParameter { ParameterName = "@regisIdICC", Value = regisIdICC} 
            };
            var query = "proc_DeleteRegistration_ICC @regisIdICC";
            var result = this.Database.ExecuteSqlCommand(query, sqlParams);
            return result;
        }
        #endregion

        public List<ICCModel> bindDropdownlist(long rollId, int opdDistrictId)
        {
            var sqlparam = new SqlParameter[] {
            new SqlParameter{ParameterName="@procId",Value=1},
            new SqlParameter{ParameterName="@Id1",Value=rollId},
            new SqlParameter{ParameterName="@Id2",Value=opdDistrictId}
            };
            var _proc = @"proc_GetDataForDropDownListForwardTo @procId,@Id1,@Id2";
            var slist = this.Database.SqlQuery<ICCModel>(_proc, sqlparam).ToList();
            return slist;
        }
        public List<ICCModel> bindDropdownlistICC(long rollId, int opdDistrictId)
        {
            var sqlparam = new SqlParameter[] {
            new SqlParameter{ParameterName="@procId",Value=2},
            new SqlParameter{ParameterName="@Id1",Value=rollId},
            new SqlParameter{ParameterName="@Id2",Value=opdDistrictId}
            };
            var _proc = @"proc_GetDataForDropDownListForwardTo @procId,@Id1,@Id2";
            var slist = this.Database.SqlQuery<ICCModel>(_proc, sqlparam).ToList();
            return slist;
        }

        #region Muheeb 20/07/2018
         public List<ICCModel> GetDetailcertificate(long regisId)
         {
             var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regidicc",Value=Convert.ToInt32( regisId)}  
            };
             var _proc = @"proc_GetImmuChildCertificate @regidicc";
             var slist = this.Database.SqlQuery<ICCModel>(_proc, sqlParam).ToList();
             return slist;
         } 
        #endregion

         public int InsertUnSignedCertiPath_ICC(long regisId, string certificateFilePath)
         {
             var sqlParam = new SqlParameter[] { 
                 new SqlParameter{ParameterName="@regisIdICC",Value=regisId},  
                 new SqlParameter{ParameterName="@certificateFilePath",Value=certificateFilePath}  
            };
             var _proc = @"proc_InsertUnSignedCertiPath_ICC @regisIdICC,@certificateFilePath";
             var result = this.Database.ExecuteSqlCommand(_proc, sqlParam);
             return result;
         }
    }
}