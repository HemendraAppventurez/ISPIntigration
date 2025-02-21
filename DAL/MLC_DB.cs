using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class MLC_DB : DbContext
    {
        SessionManager objSM = new SessionManager();
        public MLC_DB()
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
            var _proc = @"proc_checkuserMLC @regisId";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public MLCModel GetDICById(long Id)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@Id",Value=@Id}
            };
            var _proc = @"proc_getuserCommondetails @Id";
            var slist = this.Database.SqlQuery<MLCModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        
        #region check email existence
        public ResultSet CheckEmailMobileExistence(string checkValue, string Type)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type }

                };

            var sqlQuery = @"proc_checkEmailMobleExistenceMLC @checkValue,@Type";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion
        public ResultSet MLCInsertUpdate(MLCModel model, string xmlData)
        {
            var sqlparams = new SqlParameter[] { 
                    new  SqlParameter{ParameterName="@procId",Value=1},
                    new  SqlParameter{ParameterName="@regByUser", Value =model.regByUser} ,
                    new  SqlParameter{ParameterName="@patientBroughtBy", Value =model.patientBroughtBy} ,
                    new  SqlParameter{ParameterName="@broughtByPersonrelation", Value =model.broughtByPersonrelation??string.Empty} ,
                    new  SqlParameter{ParameterName="@fullName", Value =model.fullName??string.Empty} ,
                    new  SqlParameter{ParameterName="@mobileNo", Value =model.mobileNo??string.Empty} ,
                    new  SqlParameter{ParameterName="@emailId", Value =model.emailId??string.Empty} ,
                    new  SqlParameter{ParameterName="@broughtByDesignation", Value =model.broughtByDesignation??string.Empty} ,
                    new  SqlParameter{ParameterName="@idNo", Value =model.idNo??string.Empty} ,
                    new  SqlParameter{ParameterName="@broughtByaddress", Value =model.broughtByaddress},
                    new  SqlParameter{ParameterName="@broughtBystateId", Value =model.broughtBystateId},
                    new  SqlParameter{ParameterName="@broughtBydistrictId", Value =model.broughtBydistrictId},
                    new  SqlParameter{ParameterName="@pinCode", Value =model.pinCode},
                    //new  SqlParameter{ParameterName="@aadhaarNo", Value =model.aadhaarNo} , 
                    new  SqlParameter{ParameterName="@patientName", Value =model.patientName} , 
                    //new  SqlParameter{ParameterName="@relativeName", Value =model.relativeName} ,
                    new  SqlParameter{ParameterName="@age", Value =model.age ??(object)DBNull.Value},
                    new  SqlParameter{ParameterName="@patientGender", Value =model.patientGender} ,
                    new  SqlParameter{ParameterName="@occupation", Value =model.occupation??string.Empty} ,
                    new  SqlParameter{ParameterName="@districtId", Value =model.districtId} ,
                    new  SqlParameter{ParameterName="@address", Value =model.address} ,
                    new  SqlParameter{ParameterName="@tehsilId", Value =model.tehsilId} ,
                    new  SqlParameter{ParameterName="@areaRoadName", Value =model.areaRoadName} ,
                    new  SqlParameter{ParameterName="@policeStation", Value =model.policeStation} ,
                    new  SqlParameter{ParameterName="@regBytransIp", Value =model.regBytransIp} ,
                    new  SqlParameter{ParameterName="@transIp", Value =model.transIp} ,
                    new SqlParameter { ParameterName = "@requestKey", Value =model.requestKey??string.Empty},

                    new  SqlParameter{ParameterName="@forwardtypeId", Value =model.forwardtypeId} ,
                    new  SqlParameter{ParameterName="@forwardtoId", Value =model.forwardtoId} ,
                    new SqlParameter { ParameterName = "@healthUnitDistrictId", Value =model.healthUnitDistrictId},
                    new  SqlParameter{ParameterName="@idTypeId", Value =model.idTypeId} ,
                    new SqlParameter { ParameterName = "@idNumber", Value =model.idNumber??string.Empty},
                    new  SqlParameter{ParameterName="@details", Value =model.details},
                    new  SqlParameter{ParameterName="@doctorName", Value =model.doctorName},
                    new  SqlParameter{ParameterName="@designation", Value =model.Designation},
                      new  SqlParameter{ParameterName="@xmldata", Value =xmlData}
            };

            var _proc = @"proc_MLC_InsertUpdate @procId, @regByUser,@patientBroughtBy,@broughtByPersonrelation,@fullName,@mobileNo,@emailId,@broughtByDesignation,@idNo,@broughtByaddress,@broughtBystateId,@broughtBydistrictId,@pinCode,@patientName,@age,@patientGender,@occupation,@districtId,@address,@tehsilId,@areaRoadName,@policeStation,@regBytransIp,@transIp,@requestKey,@forwardtypeId,@forwardtoId,@healthUnitDistrictId,@idTypeId,@idNumber,@details,@doctorName,@designation,@xmldata";

            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        //public ResultSet MLCInsertUpdate(MLCModel model)
        //{
        //    var sqlparams = new SqlParameter[] { 
        //            new  SqlParameter{ParameterName="@procId",Value=1},
        //            new  SqlParameter{ParameterName="@regByUser", Value =model.regByUser} ,
        //            new  SqlParameter{ParameterName="@patientBroughtBy", Value =model.patientBroughtBy} ,
        //            new  SqlParameter{ParameterName="@broughtByPersonrelation", Value =(model.broughtByPersonrelation==null)?"":model.broughtByPersonrelation} ,
        //            new  SqlParameter{ParameterName="@fullName", Value =(model.fullName==null)?"":model.fullName} ,
        //            new  SqlParameter{ParameterName="@mobileNo", Value =(model.mobileNo==null)?"":model.mobileNo} ,
        //            new  SqlParameter{ParameterName="@emailId", Value =(model.emailId==null)?"":model.emailId} ,
        //            new  SqlParameter{ParameterName="@idNo", Value =model.idNo??string.Empty} ,
        //            new  SqlParameter{ParameterName="@broughtByaddress", Value =model.broughtByaddress},
        //            new  SqlParameter{ParameterName="@broughtBystateId", Value =model.broughtBystateId},
        //            new  SqlParameter{ParameterName="@broughtBydistrictId", Value =model.broughtBydistrictId},
        //            new  SqlParameter{ParameterName="@pinCode", Value =model.pinCode},
        //            //new  SqlParameter{ParameterName="@aadhaarNo", Value =model.aadhaarNo} , 
        //            new  SqlParameter{ParameterName="@patientName", Value =model.patientName} , 
        //            //new  SqlParameter{ParameterName="@relativeName", Value =model.relativeName} ,
        //            new  SqlParameter{ParameterName="@age", Value =model.age??(object)DBNull.Value} ,
        //            new  SqlParameter{ParameterName="@patientGender", Value =model.patientGender} ,
        //            new  SqlParameter{ParameterName="@occupation", Value =model.occupation??string.Empty} ,
        //            new  SqlParameter{ParameterName="@districtId", Value =model.districtId} ,
        //            new  SqlParameter{ParameterName="@address", Value =model.address} ,
        //            new  SqlParameter{ParameterName="@tehsilId", Value =model.tehsilId} ,
        //            new  SqlParameter{ParameterName="@areaRoadName", Value =model.areaRoadName} ,
        //            new  SqlParameter{ParameterName="@policeStation", Value =model.policeStation} ,
        //            new  SqlParameter{ParameterName="@regBytransIp", Value =model.regBytransIp} ,
        //            new  SqlParameter{ParameterName="@transIp", Value =model.transIp} ,
        //            new SqlParameter { ParameterName = "@requestKey", Value =model.requestKey??string.Empty},
        //            new  SqlParameter{ParameterName="@forwardtypeId", Value =model.forwardtypeId} ,
        //            new  SqlParameter{ParameterName="@forwardtoId", Value =model.forwardtoId} ,
        //            new  SqlParameter{ParameterName="@healthUnitDistrictId", Value =model.healthUnitDistrictId} ,
        //            new  SqlParameter{ParameterName="@idTypeId", Value =model.idTypeId} ,
        //            new SqlParameter { ParameterName = "@idNumber", Value =model.idNumber??string.Empty},
        //            new  SqlParameter{ParameterName="@details", Value =model.details}
        //    };

        //    var _proc = @"proc_MLC_InsertUpdate @procId, @regByUser,@patientBroughtBy,@broughtByPersonrelation,@fullName,@mobileNo,@emailId,@idNo,@broughtByaddress,@broughtBystateId,@broughtBydistrictId,@pinCode,@patientName,@age,@patientGender,@occupation,@districtId,@address,@tehsilId,@areaRoadName,@policeStation,@regBytransIp,@transIp,@requestKey,@forwardtypeId,@forwardtoId,@healthUnitDistrictId,@idTypeId,@idNumber,@details";

        //    var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
        //    return slist;
        //}

        #region Method Get Id Type
        public List<DropDownList> GetIdType()
        {
            var _proc = @"proc_GetIdType_MLC";
            var slist = this.Database.SqlQuery<DropDownList>(_proc).ToList();
            return slist;
        }
        #endregion

        public List<MLCModel> GetMLCList(long userID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=1}  ,
              new SqlParameter{ParameterName="@userId",Value=userID} 
            };
            var _proc = @"proc_getMLCList @procId,@userId";
            var slist = this.Database.SqlQuery<MLCModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public MLCModel GetMLCListBYRegistrationNo(long regisID, string registration, long regisIdMLC = 0)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=2}  ,
             new SqlParameter{ParameterName="@userId",Value=regisID} ,
              new SqlParameter{ParameterName="@registration",Value=registration} ,
              new SqlParameter{ParameterName="@reqdate",Value=(object)DBNull.Value} ,
              new SqlParameter{ParameterName="@regisIdMLC",Value=regisIdMLC} 
            };
            var _proc = @"proc_getMLCList @procId,@userId ,@registration,@reqdate,@regisIdMLC";
            var slist = this.Database.SqlQuery<MLCModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public List<MLCModel> getMLCChild(string registrationNo)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=registrationNo}  
            };
            var _proc = @"proc_getMLCChild @regisId";
            var slist = this.Database.SqlQuery<MLCModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public List<MLCAppProcessModel> GetMLCDetails(long regisIdMLC)
        {
            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisIdMLC",Value=regisIdMLC}  
            };
            var _proc = @"GetMLCdetailForCertificateRpt @regisIdMLC";
            var slist = this.Database.SqlQuery<MLCAppProcessModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public List<rptMLCChild> getMLCChilds(long regisIdMLC)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisIdMLC",Value=regisIdMLC}  
            };
            var _proc = @"GetMLCCHILDdetailForCertificateRpt @regisIdMLC";
            var slist = this.Database.SqlQuery<rptMLCChild>(_proc, sqlParam).ToList();
            return slist;
        }

        public int InsertUnSignedCertiPath_MLC(long regisId, string certificateFilePath)
        {
            var sqlParam = new SqlParameter[] { 
                 new SqlParameter{ParameterName="@regisIdMLC",Value=regisId},  
                 new SqlParameter{ParameterName="@certificateFilePath",Value=certificateFilePath}  
            };
            var _proc = @"proc_InsertUnSignedCertiPath_MLC @regisIdMLC,@certificateFilePath";
            var result = this.Database.ExecuteSqlCommand(_proc, sqlParam);
            return result;
        }

        #endregion

        public RegisterDetailsModel GetRegisterDetails()
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser}
            };
            var _proc = @"proc_RegisterDetailsMLC @regisId";
            var slist = this.Database.SqlQuery<RegisterDetailsModel>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        #region Delete Registration MLC
        public int DeleteRegistrationMLC(long regisIdMLC)
        {
            var sqlParams = new SqlParameter[] {    
                 new SqlParameter { ParameterName = "@regisIdMLC", Value = regisIdMLC} 
            };
            var query = "proc_DeleteRegistration_MLC @regisIdMLC";
            var result = this.Database.ExecuteSqlCommand(query, sqlParams);
            return result;
        }
        #endregion

        public List<MLCModel> bindDropdownlist(long rollId, int opdDistrictId)
        {
            var sqlparam = new SqlParameter[] {
            new SqlParameter{ParameterName="@procId",Value=1},
            new SqlParameter{ParameterName="@Id1",Value=rollId},
            new SqlParameter{ParameterName="@Id2",Value=opdDistrictId}
            };
            var _proc = @"proc_GetDataForDropDownListForwardTo @procId,@Id1,@Id2";
            var slist = this.Database.SqlQuery<MLCModel>(_proc, sqlparam).ToList();
            return slist;
        }

        #region Get Certificate MLC
        public List<MLCDetailsModel> GetCertificateMLC(string OPDNumber, string patientName, string patientGender, string MLCDate, int districtId, long healthUnitTypeId, long healthUnitId)
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@OPDNumber",Value=OPDNumber},
             new SqlParameter{ParameterName="@patientName",Value=patientName},
             new SqlParameter{ParameterName="@patientGender",Value=patientGender},
             new SqlParameter{ParameterName="@MLCDate",Value=string.IsNullOrEmpty(MLCDate)?(object)DBNull.Value:Convert.ToDateTime(MLCDate)},
             new SqlParameter{ParameterName="@districtId",Value=districtId},
             new SqlParameter{ParameterName="@healthUnitTypeId",Value=healthUnitTypeId},
             new SqlParameter{ParameterName="@healthUnitId",Value=healthUnitId}
             
            };
            var _proc = @"proc_GetDetails_MLC @OPDNumber,@patientName,@patientGender,@MLCDate,@districtId,@healthUnitTypeId,@healthUnitId";
            var slist = this.Database.SqlQuery<MLCDetailsModel>(_proc, sqlParam).ToList();
            return slist;
        }
        #endregion

        #region Get Nominee MLC
        public MLCNomineeModel GetNomineeMLC(long userId)
        {
            var sqlParam = new SqlParameter[] { 

             new SqlParameter{ParameterName="@userId",Value=userId}
             
            };
            var _proc = @"Proc_GetNominee_MLC @userId";
            var result = this.Database.SqlQuery<MLCNomineeModel>(_proc, sqlParam).FirstOrDefault();
            return result;
        }
        #endregion

        #region Insert Nominee Details MLC
        public ResultSet InsertNomineeDetailsMLC(MLCNomineeModel model)
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@nomineeId",Value=model.nomineeId},
             new SqlParameter{ParameterName="@regisIdMLC",Value=model.regisIdMLC},
             new SqlParameter{ParameterName="@nomineeName",Value=model.nomineeName??string.Empty},
             new SqlParameter{ParameterName="@mobileNumber",Value=model.mobileNumber??string.Empty},
             new SqlParameter{ParameterName="@idProof",Value=model.idProof},
             new SqlParameter{ParameterName="@idProofFilePath",Value=model.idProofFilePath??string.Empty},
             new SqlParameter{ParameterName="@downloadedBy",Value=model.downloadedBy},
             new SqlParameter{ParameterName="@transIp",Value=model.transIp}
             
            };
            var _proc = @"proc_InsertNomineeDetails_MLC @nomineeId,@regisIdMLC,@nomineeName,@mobileNumber,@idProof,@idProofFilePath,@downloadedBy,@transIp";
            var result = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return result;
        }
        #endregion

        #region Get Detail By NomneeId MLC
        public MLCNomineeModel GetDetailByNomneeIdMLC(long nomneeId)
        {
            var sqlParam = new SqlParameter[] { 

             new SqlParameter{ParameterName="@nomneeId",Value=nomneeId}
             
            };
            var _proc = @"Proc_GetDetailByNomneeId_MLC @nomneeId";
            var result = this.Database.SqlQuery<MLCNomineeModel>(_proc, sqlParam).FirstOrDefault();
            return result;
        }
        #endregion

        #region Get Nominee Log
        public List<MLCNomineeModel> GetNomineeLog(long userId)
        {
            var sqlParam = new SqlParameter[] { 

             new SqlParameter{ParameterName="@userId",Value=userId}
             
            };
            var _proc = @"Proc_GetNomineeLog_MLC @userId";
            var result = this.Database.SqlQuery<MLCNomineeModel>(_proc, sqlParam).ToList();
            return result;
        }
        #endregion
    }
}