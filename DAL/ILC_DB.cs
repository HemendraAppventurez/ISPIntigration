using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class ILC_DB : DbContext
    {
        SessionManager objSM = new SessionManager();
        #region Default Constructor

        public ILC_DB()
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
            var _proc = @"proc_checkuserILC @regisId";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }

        public ILCModel GetILCById(long Id)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@Id",Value=@Id}
            };
            var _proc = @"proc_getuserCommondetails @Id";
            var slist = this.Database.SqlQuery<ILCModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }

        #region check email existence
        public ResultSet CheckEmailMobileExistence(string checkValue, string Type,long regisId)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@RegisId", Value = regisId }
                };

            var sqlQuery = @"proc_checkEmailMobileExistenceILC @checkValue,@Type, @RegisId";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
       
        #endregion

        public ResultSet ILCInsertUpdate(ILCModel model)
        {
            var sqlparams = new SqlParameter[] { 
                  new  SqlParameter{ParameterName="@procId",Value=1},
                  new  SqlParameter{ParameterName="@regisId",Value=model.regByUser},
                  new  SqlParameter{ParameterName="@regisIdILC",Value=model.regisIdILC},
                  new  SqlParameter{ParameterName="@opdReceiptno",Value=model.opdReceiptno},
                  new  SqlParameter{ParameterName="@opdDate",Value=model.opdDate},
                  new  SqlParameter{ParameterName="@opdDistrictId",Value=model.opdDistrictId},
                  new  SqlParameter{ParameterName="@opdAddress",Value=model.opdAddress},
                  new  SqlParameter{ParameterName="@opdPincode",Value=model.opdPincode??string.Empty},
                  new  SqlParameter{ParameterName="@opdStateId",Value=model.opdStateId},
                  new  SqlParameter{ParameterName="@opdFilePath",Value=model.opdFilePath},
                  new  SqlParameter{ParameterName="@fullName",Value=model.fullName},
                  new  SqlParameter{ParameterName="@fatherName",Value=model.fatherName},
                  new  SqlParameter{ParameterName="@dob",Value=model.dob},
                  new  SqlParameter{ParameterName="@gender",Value=(model.gender==null)?"":model.gender },
                  new  SqlParameter{ParameterName="@categoryId",Value=model.categoryId},
                  new  SqlParameter{ParameterName="@mobileNo",Value=model.mobileNo},
                  new  SqlParameter{ParameterName="@emailId",Value=model.emailId??string.Empty},
                  
                  new  SqlParameter{ParameterName="@forwardType",Value=model.forwardtypeId},
                  new  SqlParameter{ParameterName="@forwardtoId",Value=model.forwardtoId},
                  new  SqlParameter{ParameterName="@doctorName",Value=(model.doctorName==null)?"":model.doctorName },
                  new  SqlParameter{ParameterName="@reason",Value=model.reason},
                  new  SqlParameter{ParameterName="@treatmentFrom",Value=model.treatmentFrom},
                  new  SqlParameter{ParameterName="@treatmentto",Value=model.treatmentto},
                  new  SqlParameter{ParameterName="@NoOfDays",Value=model.NoOfDays},
                  new  SqlParameter{ParameterName="@remarks",Value=model.remarks??string.Empty},
                  new  SqlParameter{ParameterName="@transIP",Value=model.transIp},
                  new  SqlParameter{ParameterName="@illnessDetail",Value=model.illnessDetail},
                  new  SqlParameter{ParameterName="@healthUnitDistrictId",Value=model.healthUnitDistrictId},
                  new SqlParameter { ParameterName = "@requestKey", Value =model.requestKey??string.Empty},

                  //new SqlParameter { ParameterName = "@extPhoto", Value =model.extPhotoPath},
                  //new SqlParameter { ParameterName = "@thumbSign", Value =model.thumbSignPath},
                  new SqlParameter { ParameterName = "@idTypeId", Value =model.idTypeId},
                  new SqlParameter { ParameterName = "@idNumber", Value =(model.idNumber==null)?"":model.idNumber},
                  new SqlParameter { ParameterName = "@idFile", Value =model.idFilePath},

                  new SqlParameter { ParameterName = "@markOfIdentification", Value= (model.markOfIdentification==null)?"":model.markOfIdentification},
                  

                  new SqlParameter { ParameterName = "@oldCertificateNumber", Value =(model.oldCertificateNumber==null)?"":model.oldCertificateNumber },
                  new SqlParameter { ParameterName = "@extOpdReceiptno", Value =(model.extOpdReceiptno==null)?"":model.extOpdReceiptno },
                  new SqlParameter { ParameterName = "@extInspectedDate", Value =(model.extInspectedDate==null)?"":model.extInspectedDate },
                  new SqlParameter { ParameterName = "@extOpdFile", Value =(model.extOpdFilePath==null)?"":model.extOpdFilePath },
                  new SqlParameter { ParameterName = "@extDoctorName", Value =(model.extDoctorName==null)?"":model.extDoctorName },
                  new SqlParameter { ParameterName = "@extTreatmentFrom", Value =(model.extTreatmentFrom==null)?"":model.extTreatmentFrom },
                  new SqlParameter { ParameterName = "@extTreatmentto", Value =(model.extTreatmentto==null)?"":model.extTreatmentto },
                  new SqlParameter { ParameterName = "@extNoOfDays", Value =model.extNoOfDays}


            };

            var _proc = @"proc_ILC_InsertUpdate @procId, @regisId,@regisIdILC,@opdReceiptno,@opdDate,@opdDistrictId,@opdAddress,@opdPincode,@opdStateId,@opdFilePath,@fullName, @fatherName, 
@dob,  @gender, @categoryId, @mobileNo, @emailId,@forwardType,@forwardtoId,@doctorName,  @reason,@treatmentFrom,@treatmentto,@NoOfDays, @remarks ,
@transIP,@illnessDetail,@healthUnitDistrictId,@requestKey,@idTypeId,@idNumber,@idFile,
@oldCertificateNumber ,@extOpdReceiptno ,@extInspectedDate,@extOpdFile , @extDoctorName ,@extTreatmentFrom ,@extTreatmentto ,@extNoOfDays,@markOfIdentification ";

            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public ResultSet ILCInsertUpdateExtendedDays(ILCModel model)
        {
            var sqlparams = new SqlParameter[] { 
                 
                  
                  new  SqlParameter{ParameterName="@regisIdILC",Value=model.regisIdILC},
                  new SqlParameter { ParameterName = "@oldCertificateNumber", Value =(model.oldCertificateNumber==null)?"":model.oldCertificateNumber },
                   new  SqlParameter{ParameterName="@reason",Value=model.reason},
                   new SqlParameter { ParameterName = "@idNumber", Value =(model.idNumber==null)?"":model.idNumber},   
                  new SqlParameter { ParameterName = "@extOpdReceiptno", Value =(model.extOpdReceiptno==null)?"":model.extOpdReceiptno },
                  new SqlParameter { ParameterName = "@extInspectedDate", Value =(model.extInspectedDate==null)?"":model.extInspectedDate },
                  new SqlParameter { ParameterName = "@extOpdFile", Value =(model.extOpdFilePath==null)?"":model.extOpdFilePath },
                  new SqlParameter { ParameterName = "@extDoctorName", Value =(model.extDoctorName==null)?"":model.extDoctorName },
                  new SqlParameter { ParameterName = "@extTreatmentFrom", Value =(model.extTreatmentFrom==null)?"":model.extTreatmentFrom },
                  new SqlParameter { ParameterName = "@extTreatmentto", Value =(model.extTreatmentto==null)?"":model.extTreatmentto },
                  new SqlParameter { ParameterName = "@extNoOfDays", Value =model.extNoOfDays},


                  new  SqlParameter{ParameterName="@transIP",Value=model.transIp}
                 
               


            };

            var _proc = @"proc_ILC_InsertUpdateExtended @regisIdILC ,@oldCertificateNumber,@reason,@idNumber ,
@extOpdReceiptno ,@extInspectedDate,@extOpdFile,@extDoctorName,@extTreatmentFrom,@extTreatmentto,@extNoOfDays,@transIp ";

            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }

        public List<ILCModel> GetILCList(long userId)
        {

            var sqlParam = new SqlParameter[] { 
                 new SqlParameter{ParameterName="@procId",Value=1},
            new SqlParameter{ParameterName="@userId",Value=userId}
            };
            var _proc = @"proc_getILC_Details @procId,@userId";
            var slist = this.Database.SqlQuery<ILCModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public ILCModel GetILCListBYRegistrationNo(long userId)
        { 
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=2},
            new SqlParameter{ParameterName="@userId",Value=userId} 
            };
            var _proc = @"proc_getILC_Details @procId,@userId";
            var slist = this.Database.SqlQuery<ILCModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }

        public RegisterDetailsModel GetRegisterDetails()
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser}
            };
            var _proc = @"proc_RegisterDetailsILC @regisId";
            var slist = this.Database.SqlQuery<RegisterDetailsModel>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        #endregion

        #region Riya
        public List<ILCModel> rblforwardType()
        {
            var _proc = @"proc_IMC_forwardType";
            var slist = this.Database.SqlQuery<ILCModel>(_proc).ToList();
            return slist;
        }
        public List<ILCModel> bindDropdownlist(long rollId, int opdDistrictId)
        {
            var sqlparam = new SqlParameter[] {
            new SqlParameter{ParameterName="@procId",Value=1},
            new SqlParameter{ParameterName="@Id1",Value=rollId},
            new SqlParameter{ParameterName="@Id2",Value=opdDistrictId}
            };
            var _proc = @"proc_GetDataForDropDownListForwardTo @procId,@Id1,@Id2";
            var slist = this.Database.SqlQuery<ILCModel>(_proc, sqlparam).ToList();
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
        #endregion

        #region Delete Registration ILC
        public int DeleteRegistrationILC(long regisIdILC)
        {
            var sqlParams = new SqlParameter[] {    
                 new SqlParameter { ParameterName = "@regisIdILC", Value = regisIdILC} 
            };
            var query = "proc_DeleteRegistration_ILC @regisIdILC";
            var result = this.Database.ExecuteSqlCommand(query, sqlParams);
            return result;
        }
        #endregion

        public ILCModel GetILCdetailByCertNo(string oldCertificateNumber, long userId)
        {
            var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@certificateNo",Value=oldCertificateNumber},
              new SqlParameter{ParameterName="@regByUser",Value=userId}  
            };
            var _proc = @"GetILCDetailByCrtificateNo @certificateNo,@regByUser";
            var slist = this.Database.SqlQuery<ILCModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }

        public ResultSet UpdateILCCertificate(long regisId, string certificatePath, long regisBy, string Ipaddress)
        {
            var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@regisId",Value=regisId}  ,
              new SqlParameter{ParameterName="@certificatePath",Value=certificatePath} ,
              new SqlParameter{ParameterName="@regisBy",Value=regisBy}  ,
              new SqlParameter{ParameterName="@Ipaddress",Value=Ipaddress} 
             
            };
            var _proc = @"proc_NUHUpdate @regisId,@certificatePath,@regisBy ,@Ipaddress";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }
    }
}
