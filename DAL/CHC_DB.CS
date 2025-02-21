using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class CHC_DB : DbContext
    {
        
        public CHC_DB()
            : base("CMSModule")
        {
        }
        #region FIC Riya
        public FICModel getApplicationCountFIC(int institutionTypeId)//get count of Application FIC
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@institutionTypeId",Value=institutionTypeId}  
            };
            var _proc = @"getApplicationCountFIC @institutionTypeId";
            var slist = this.Database.SqlQuery<FICModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public List<FICDetailsModel> GetAllFICList(int procId, string registrationNo, int appType, string requestDate, int Appstatus, int institutionTypeId)//int institutionTypeId)//Get FIC Detail
        {

            var sqlParam = new SqlParameter[] { 
                new SqlParameter{ParameterName="@procId",Value=procId},  
                new SqlParameter{ParameterName="@registrationNo",Value=registrationNo}, 
                new SqlParameter{ParameterName="@appType",Value=appType},
                new SqlParameter{ParameterName="@requestDate",Value=requestDate},
                new SqlParameter{ParameterName="@status",Value=Appstatus},
            new SqlParameter{ParameterName="@institutionTypeId",Value=institutionTypeId}  
            };
            var _proc = @"proc_getAllFIC_Details @procId,@registrationNo,@appType,@requestDate,@status,@institutionTypeId";
            var slist = this.Database.SqlQuery<FICDetailsModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public List<FICDetailsModel> GetAllFICListchc(int institutionTypeId)//Get FIC Detail
        {

            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=1},
            new SqlParameter{ParameterName="@institutionTypeId",Value=institutionTypeId}
            };
            var _proc = @"proc_getAllFIC_Detailschc @procId,@institutionTypeId";
            var slist = this.Database.SqlQuery<FICDetailsModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public FICModel GetFICList(string regisId, int status)
        {

            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@registrationNo",Value=regisId},
            new SqlParameter{ParameterName="@appliedStatus",Value=status}
            };
            var _proc = @"getFICforUpDateApplication @registrationNo,@appliedStatus";
            var slist = this.Database.SqlQuery<FICModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public ResultSet FICAppStatusInsertUpdate(long regisIdFIC, int appStatus, string rejectedRemarks, string inspectionRptPath,
            string inspectionRejectedRemark, string certGenerateByDoc, string certGenerateByDesig, string inspecttionCompeletionDate, string identificationMark, long regByUser, string transIp)
        {
            var sqlparams = new SqlParameter[] { 
                    new  SqlParameter{ParameterName="@regisIdFIC",Value=regisIdFIC},
                    new  SqlParameter{ParameterName="@appStatus",Value=appStatus},
                    new  SqlParameter{ParameterName="@appRejectedRemark",Value=(rejectedRemarks==null)?"":rejectedRemarks},
                    //new  SqlParameter{ParameterName="@inspectionDate",Value=(inspectionDate==null)?"":inspectionDate},
                    new  SqlParameter{ParameterName="@inspectionRptPath",Value=(inspectionRptPath==null)?"":inspectionRptPath},
                    new  SqlParameter{ParameterName="@inspectionRejectedRemark",Value=(inspectionRejectedRemark==null)?"":inspectionRejectedRemark},
                    new  SqlParameter{ParameterName="@certGenerateByDoc",Value=(certGenerateByDoc==null)?"":certGenerateByDoc},
                    new  SqlParameter{ParameterName="@certGenerateByDesig",Value=(certGenerateByDesig==null)?"":certGenerateByDesig},
                    new  SqlParameter{ParameterName="@inspecttionCompeletionDate",Value=(inspecttionCompeletionDate==null)?"":inspecttionCompeletionDate},
                    new  SqlParameter{ParameterName="@identificationMark",Value=(identificationMark==null)?"":identificationMark},
                    new  SqlParameter{ParameterName="@appRejectedAcceptedBy",Value=regByUser},
                    new  SqlParameter{ParameterName="@transIp",Value=transIp},
                   
         
            };

            var _proc = @"AppStatusUpdateFIC @regisIdFIC,@appStatus,@appRejectedRemark,@inspectionRptPath,@inspectionRejectedRemark,@certGenerateByDoc, @certGenerateByDesig, @inspecttionCompeletionDate, @identificationMark,@appRejectedAcceptedBy,@transIp";

            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
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

        public int InsertUnSignedCertiPath_FIC(long regisId, string certificateFilePath)
        {
            var sqlParam = new SqlParameter[] { 
                 new SqlParameter{ParameterName="@regisIdFIC",Value=regisId},  
                 new SqlParameter{ParameterName="@certificateFilePath",Value=certificateFilePath}  
            };
            var _proc = @"proc_InsertUnSignedCertiPath_FIC @regisIdFIC,@certificateFilePath";
            var result = this.Database.ExecuteSqlCommand(_proc, sqlParam);
            return result;
        }

        #endregion
        #region ILC Riya
        public ILCDetailsModel getApplicationCountILC(int institutionTypeId)//get count of Application ILC
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@institutionTypeId",Value=institutionTypeId}  
            };
            var _proc = @"getApplicationCountILC @institutionTypeId";
            var slist = this.Database.SqlQuery<ILCDetailsModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        //public List<ILCDetailsModel> GetAllLICList(int institutionTypeId)//Get FIC Detail
        //{

        //    var sqlParam = new SqlParameter[] { 
        //    new SqlParameter{ParameterName="@institutionTypeId",Value=institutionTypeId}  
        //    };
        //    var _proc = @"proc_getAllILC_Details @institutionTypeId";
        //    var slist = this.Database.SqlQuery<ILCDetailsModel>(_proc, sqlParam).ToList();
        //    return slist;
        //}
        public List<ILCDetailsModel> GetAllLICList(int procId, string registrationNo, int appType, string requestDate, int Appstatus, int institutionTypeId)//int institutionTypeId)//Get FIC Detail
        {

            var sqlParam = new SqlParameter[] { 
                new SqlParameter{ParameterName="@procId",Value=procId},  
                new SqlParameter{ParameterName="@registrationNo",Value=registrationNo}, 
                new SqlParameter{ParameterName="@appType",Value=appType},
                new SqlParameter{ParameterName="@requestDate",Value=requestDate},
                new SqlParameter{ParameterName="@status",Value=Appstatus},
            new SqlParameter{ParameterName="@institutionTypeId",Value=institutionTypeId}  
            };
            var _proc = @"proc_getAllILC_Details @procId,@registrationNo,@appType,@requestDate,@status,@institutionTypeId";
            var slist = this.Database.SqlQuery<ILCDetailsModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public ILCDetailsModel GetILCList(string regisId, int status)
        {

            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@registrationNo",Value=regisId},
            new SqlParameter{ParameterName="@appliedStatus",Value=status}
            };
            var _proc = @"getILCforUpDateApplication @registrationNo,@appliedStatus";
            var slist = this.Database.SqlQuery<ILCDetailsModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public ResultSet ILCAppStatusInsertUpdate(long regisIdILC, int appStatus, string rejectedRemarks, string inspectionDate, string inspectionRptPath, string inspectionRejectedRemark,
            string certGenerateByDoc, string certGenerateByDesig, string inspecttionCompeletionDate, string diseaseName, string identificationMark, int bedRest, long regByUser, string transIp)
        {
            var sqlparams = new SqlParameter[] { 
                    new  SqlParameter{ParameterName="@regisIdILC",Value=regisIdILC},
                    new  SqlParameter{ParameterName="@appStatus",Value=appStatus},
                    new  SqlParameter{ParameterName="@appRejectedRemark",Value=(rejectedRemarks==null)?"":rejectedRemarks},
                    new  SqlParameter{ParameterName="@inspectionDate",Value=(inspectionDate==null)?"":inspectionDate},
                    new  SqlParameter{ParameterName="@inspectionRptPath",Value=(inspectionRptPath==null)?"":inspectionRptPath},
                    new  SqlParameter{ParameterName="@inspectionRejectedRemark",Value=(inspectionRejectedRemark==null)?"":inspectionRejectedRemark},
                    new  SqlParameter{ParameterName="@certGenerateByDoc",Value=(certGenerateByDoc==null)?"":certGenerateByDoc},
                    new  SqlParameter{ParameterName="@certGenerateByDesig",Value=(certGenerateByDesig==null)?"":certGenerateByDesig},
                    new  SqlParameter{ParameterName="@inspecttionCompeletionDate",Value=(inspecttionCompeletionDate==null)?"":inspecttionCompeletionDate},
                    new  SqlParameter{ParameterName="@diseaseName",Value=(diseaseName==null)?"":diseaseName},
                    new  SqlParameter{ParameterName="@identificationMark",Value=(identificationMark==null)?"":identificationMark},
                    
                    new  SqlParameter{ParameterName="@bedRest",Value=(bedRest==null)?0:bedRest},
                    new  SqlParameter{ParameterName="@appRejectedAcceptedBy",Value=regByUser},
                    new  SqlParameter{ParameterName="@transIp",Value=transIp},
                   
         
            };

            var _proc = @"AppStatusUpdateILC @regisIdILC,@appStatus,@appRejectedRemark,@inspectionDate,@inspectionRptPath,@inspectionRejectedRemark,@appRejectedAcceptedBy,@certGenerateByDoc,@certGenerateByDesig,@inspecttionCompeletionDate,@diseaseName,@identificationMark,@bedRest,@transIp";

            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
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

        public int InsertUnSignedCertiPath_ILC(long regisId, string certificateFilePath)
        {
            var sqlParam = new SqlParameter[] { 
                 new SqlParameter{ParameterName="@regisIdILC",Value=regisId},  
                 new SqlParameter{ParameterName="@certificateFilePath",Value=certificateFilePath}  
            };
            var _proc = @"proc_InsertUnSignedCertiPath_ILC @regisIdILC,@certificateFilePath";
            var result = this.Database.ExecuteSqlCommand(_proc, sqlParam);
            return result;
        }

        #endregion
        #region DEC Riya
        public List<DECModel> GetAllRegistration_DEC(long userId,string registrationNo, string appliedDate)
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@userId",Value=userId}  ,
             //new SqlParameter{ParameterName="@Ids",Value=Ids,TypeName="[dbo].[Ids]"}  ,
             new SqlParameter{ParameterName="@registrationNo",Value=registrationNo}  ,
             new SqlParameter{ParameterName="@appliedDate",Value=appliedDate}
             
            };
            var _proc = @"proc_GetAllRegistration_DEC @userId,@registrationNo,@appliedDate";
            var slist = this.Database.SqlQuery<DECModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public ResultSet GenrateCertificateDEC(DECModel model)
        {
            var sqlParam = new SqlParameter[] { 
               new SqlParameter("@regisIdDEC", model.regisIdDEC),
               new SqlParameter("@appStatus", model.appStatus),
               new SqlParameter("@userId", model.regByuser),
               new SqlParameter("@transIp", model.transIp)

            };
            var _proc = @"proc_GenrateCertificateDEC @regisIdDEC,@appStatus,@userId,@transIp";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }
        public DECModel getApplicationCountDEC(int institutionTypeId)//get count of Application FIC
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@institutionTypeId",Value=institutionTypeId}  
            };
            var _proc = @"getApplicationCountDEC @institutionTypeId";
            var slist = this.Database.SqlQuery<DECModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public DECModel GetDECList(long regisIdDEC)
        {

            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisIdDEC",Value=regisIdDEC}
            };
            var _proc = @"Proc_getDECForCHC @regisIdDEC";
            var slist = this.Database.SqlQuery<DECModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public List<DECModel> GetAllDECList(long regisIdDEC)
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@regisIdDEC",Value=regisIdDEC}
             
            };
            var _proc = @"proc_GetAllCertificateGenratedList_DEC @regisIdDEC";
            var slist = this.Database.SqlQuery<DECModel>(_proc, sqlParam).ToList();
            return slist;
        }
        #endregion
       

        #region IMC abhijeet
        #region Riya
        public ProcessType getIMCprocessCount(int institutionTypeId)
        {
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@institutionTypeId",Value=institutionTypeId}
             };
            var _proc = @"proc_IMC_countProcess @institutionTypeId";
            var slist = this.Database.SqlQuery<ProcessType>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        #endregion
        public List<IMCModel> GetAllIMCList(int procId, long regisIdIMC, string registration, string mobile, string requestdDate, int status, int institutionTypeId)
        {
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=procId},
            new SqlParameter{ParameterName="@regisIdIMC",Value=regisIdIMC},
            new SqlParameter{ParameterName="@registration",Value=registration},
            new SqlParameter{ParameterName="@mobile",Value=mobile},
            new SqlParameter{ParameterName="@requestDate",Value=requestdDate},
             new SqlParameter{ParameterName="@status",Value=status},
             new SqlParameter{ParameterName="@institutionTypeId",Value=institutionTypeId}
            };
            var _proc = @"proc_getAllIMC @procId,@regisIdIMC,@registration,@mobile,@requestDate,@status,@institutionTypeId";
            var slist = this.Database.SqlQuery<IMCModel>(_proc, sqlparams).ToList();
            return slist;

        }



        public List<IMCModel> GetAllInprocessIMCList(int procId, long regisIdIMC, string registration, string mobile, string requestdDate, int status, int forwardType)
        {
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=procId},
            new SqlParameter{ParameterName="@regisIdIMC",Value=regisIdIMC},
            new SqlParameter{ParameterName="@registration",Value=registration},
            new SqlParameter{ParameterName="@mobile",Value=mobile},
            new SqlParameter{ParameterName="@requestDate",Value=requestdDate},

             new SqlParameter{ParameterName="@status",Value=status},
              new SqlParameter{ParameterName="@forwardType",Value=forwardType}

            };
            var _proc = @"proc_getAllInprocessIMC @procId,@regisIdIMC,@registration,@mobile,@requestDate,@status,@forwardType";
            var slist = this.Database.SqlQuery<IMCModel>(_proc, sqlparams).ToList();
            return slist;

        }
        public List<IMCModel> GetAllApprovedIMCList(int procId, string registration, string mobile, string requestdDate, int forwardType)
        {
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=procId},
            new SqlParameter{ParameterName="@registration",Value=registration},
            new SqlParameter{ParameterName="@mobile",Value=mobile},
            new SqlParameter{ParameterName="@requestDate",Value=requestdDate},
            new SqlParameter{ParameterName="@forwardType",Value=forwardType}
            
            };
            var _proc = @"proc_getAllApprovedIMC @procId,@registration,@mobile,@requestDate,@forwardType";
            var slist = this.Database.SqlQuery<IMCModel>(_proc, sqlparams).ToList();
            return slist;

        }
        public List<IMCModel> GetAllRejectedIMCList(int procId, string registration, string mobile, string requestdDate, int forwardType)
        {
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=procId},
            new SqlParameter{ParameterName="@registration",Value=registration},
            new SqlParameter{ParameterName="@mobile",Value=mobile},
            new SqlParameter{ParameterName="@requestDate",Value=requestdDate},
            new SqlParameter{ParameterName="@forwardType",Value=forwardType}
            };
            var _proc = @"proc_getAllRejectedIMC @procId,@registration,@mobile,@requestDate,@forwardType";
            var slist = this.Database.SqlQuery<IMCModel>(_proc, sqlparams).ToList();
            return slist;

        }
        public IMCAppProcessModel getIMCStatus(long regisFAPId)
        {
            var sqlparams = new SqlParameter[] { 
           
            new SqlParameter{ParameterName="@regisIdIMC",Value=regisFAPId}
            };
            var _proc = @"proc_getIMCStatus @regisIdIMC";
            var slist = this.Database.SqlQuery<IMCAppProcessModel>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public ResultSet UpdateIMCProcess(long regisIMCId, int appStatus, bool status, string inspectionDate, string hospital_establishment, string inspectionReportFilePath,
            string rejectedRemarks, long userId, string transIp,string ImmunXML, string xmldata)
        {
            var sqlparams = new SqlParameter[] { 
           
            new SqlParameter{ParameterName="@regisIMCId",Value=regisIMCId},
            new SqlParameter{ParameterName="@appStatus",Value=appStatus} ,
            new SqlParameter{ParameterName="@status",Value=status},
            new SqlParameter{ParameterName="@inspectionDate",Value=inspectionDate??string.Empty},
            new SqlParameter{ParameterName="@hospital_establishment",Value=hospital_establishment??string.Empty},
            new SqlParameter{ParameterName="@inspectionReportFilePath",Value=inspectionReportFilePath??string.Empty},
            new SqlParameter{ParameterName="@rejectedRemarks",Value=rejectedRemarks??string.Empty},
            new SqlParameter{ParameterName="@userId",Value=userId},
            new SqlParameter{ParameterName="@transIp",Value=transIp},
             new SqlParameter{ParameterName="@ImmunXML",Value=ImmunXML??string.Empty},
             new SqlParameter{ParameterName="@xmldata",Value=xmldata}
            };
            var _proc = @"proc_UpdateAppProcessIMC @regisIMCId ,@appStatus ,@status, @inspectionDate, @hospital_establishment, @inspectionReportFilePath,@rejectedRemarks,@userId ,@transIp,@ImmunXML,@xmldata";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        #endregion

        #region Muheeb
        public ResultSet CHCInsertUpdate(CHCModel model)
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
                    new  SqlParameter{ParameterName="@relationWithDeathPerson",Value=model.relationWithDeathPerson},
                    new  SqlParameter{ParameterName="@forwardtypeId",Value=model.forwardtypeId},
                    new  SqlParameter{ParameterName="@healthUnitDistrictId",Value=model.healthUnitDistrictId},
                    new  SqlParameter{ParameterName="@forwardtoId",Value=model.forwardtoId},
                    new  SqlParameter{ParameterName="@DeathPersonName",Value=model.deathPersonName},
                    new  SqlParameter{ParameterName="@DeathPersonAadhaarNo",Value=model.aadhaarNo??String.Empty},
                    new  SqlParameter{ParameterName="@DeathPersonGender",Value=model.DeathPersonGender},
                    new  SqlParameter{ParameterName="@maritalStatusId",Value=model.maritalStatusId},
                    new  SqlParameter{ParameterName="@religionId",Value=model.religionId},
                    new  SqlParameter{ParameterName="@spouseName",Value=model.spouseName},
                    new  SqlParameter{ParameterName="@fatherName",Value=model.fathersName},
                    new  SqlParameter{ParameterName="@motherName",Value=model.motherName},
                    new  SqlParameter{ParameterName="@dod",Value=model.dod},
                    new  SqlParameter{ParameterName="@isCauseCertified",Value=model.isCauseCertified},
	                new  SqlParameter{ParameterName="@diseaseNameOrCause",Value=model.diseaseNameOrCause??String.Empty},
	                new  SqlParameter{ParameterName="@isInfoCorrect",Value=model.isInfoCorrect},
	                new  SqlParameter{ParameterName="@regBytransIp",Value=model.regBytransIp},
	                new  SqlParameter{ParameterName="@transIp",Value=model.transIp},
                    new SqlParameter { ParameterName = "@requestKey", Value =model.requestKey??string.Empty}                 
         
            };

            var _proc = @"dbo.proc_DEC_InsertUpdate  @ProcId , @regByuser , @fullName, @mobileNo, @emailId , @address , @stateId ,@districtId , @Pincode , @relationWithDeathPerson ,@forwardtypeId,@healthUnitDistrictId,@forwardtoId, @DeathPersonName , @DeathPersonAadhaarNo , @DeathPersonGender , @maritalStatusId ,@religionId,@spouseName , @fatherName ,@motherName , @dod , @isCauseCertified , @diseaseNameOrCause , @isInfoCorrect , @regBytransIp ,@transIp,@requestKey";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }

        public int DeleteRegistrationCHC(long regisIdCHC)
        {
            var sqlParams = new SqlParameter[] {    
                 new SqlParameter { ParameterName = "@regisIdFAP", Value = regisIdCHC} 
            };
            var query = "proc_DeleteRegistration_DEC @regisIdDEC";
            var result = this.Database.ExecuteSqlCommand(query, sqlParams);
            return result;
        }
        public List<CHCModel> rblforwardType()
        {
            var _proc = @"proc_IMC_forwardType";
            var slist = this.Database.SqlQuery<CHCModel>(_proc).ToList();
            return slist;
        }


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
                    new  SqlParameter{ParameterName="@idNo", Value =model.idNo??string.Empty} ,
                    new  SqlParameter{ParameterName="@broughtByaddress", Value =model.broughtByaddress},
                    new  SqlParameter{ParameterName="@broughtBystateId", Value =model.broughtBystateId},
                    new  SqlParameter{ParameterName="@broughtBydistrictId", Value =model.broughtBydistrictId},
                    new  SqlParameter{ParameterName="@pinCode", Value =model.pinCode},
                    //new  SqlParameter{ParameterName="@aadhaarNo", Value =model.aadhaarNo} , 
                    new  SqlParameter{ParameterName="@patientName", Value =model.patientName} , 
                    //new  SqlParameter{ParameterName="@relativeName", Value =model.relativeName} ,
                    new  SqlParameter{ParameterName="@age", Value =model.age} ,
                    new  SqlParameter{ParameterName="@patientGender", Value =model.patientGender} ,
                    new  SqlParameter{ParameterName="@occupation", Value =model.occupation} ,
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

            var _proc = @"proc_MLC_InsertUpdate @procId, @regByUser,@patientBroughtBy,@broughtByPersonrelation,@fullName,@mobileNo,@emailId,@idNo,@broughtByaddress,@broughtBystateId,@broughtBydistrictId,@pinCode,@patientName,@age,@patientGender,@occupation,@districtId,@address,@tehsilId,@areaRoadName,@policeStation,@regBytransIp,@transIp,@requestKey,@forwardtypeId,@forwardtoId,@healthUnitDistrictId,@idTypeId,@idNumber,@details,@doctorName,@designation,@xmldata";

            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public int DeleteRegistrationMLC(long regisIdMLC)
        {
            var sqlParams = new SqlParameter[] {    
                 new SqlParameter { ParameterName = "@regisIdMLC", Value = regisIdMLC} 
            };
            var query = "proc_DeleteRegistration_MLC @regisIdMLC";
            var result = this.Database.ExecuteSqlCommand(query, sqlParams);
            return result;
        }


        //public IEnumerable<T> ViewDeathCertificate<T>(int userid, int ProcId,string ReqDate,string applNo)
        //{
        //    var sqlParam = new SqlParameter[] { 
        //          new SqlParameter { ParameterName = "@userId", Value =userid}, 
        //          new SqlParameter { ParameterName = "@reqdate", Value =ReqDate??string.Empty}, 
        //          new SqlParameter { ParameterName = "@registration", Value =applNo},
        //           new SqlParameter { ParameterName = "@ProcID", Value =ProcId}, 
        //    };
        //    var sqlQuery = @"proc_getDECList @ProcID, @userid,@registration, @reqdate";
        //    var sList = this.Database.SqlQuery<T>(sqlQuery, sqlParam);
        //    return sList;

        //}

        public List<CHCModel> ViewDeathCertificate(long userid, int ProcId, string ReqDate, string applNo)
        {
            var sqlParam = new SqlParameter[] { 
                  new SqlParameter { ParameterName = "@userId", Value =userid}, 
                  new SqlParameter { ParameterName = "@reqdate", Value =ReqDate??string.Empty}, 
                  new SqlParameter { ParameterName = "@registration", Value =applNo},
                   new SqlParameter { ParameterName = "@ProcID", Value =ProcId}, 
            };
            var _proc = @"proc_getDECList @ProcID, @userid,@registration, @reqdate";
            var slist = this.Database.SqlQuery<CHCModel>(_proc, sqlParam).ToList();
            return slist;
        }


        public List<MLCModel> ViewMLCList(long userid, int ProcId, string ReqDate, string applNo)
        {
            var sqlParam = new SqlParameter[] { 
                  new SqlParameter { ParameterName = "@userId", Value =userid}, 
                  new SqlParameter { ParameterName = "@reqdate", Value =ReqDate??string.Empty}, 
                  new SqlParameter { ParameterName = "@registration", Value =applNo},
                   new SqlParameter { ParameterName = "@ProcID", Value =ProcId}, 
            };
            var _proc = @"proc_getMLCList @ProcID, @userid,@registration, @reqdate";
            var slist = this.Database.SqlQuery<MLCModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public CHCModel GetCHCListBYRegistrationNo(long regisID, string registration)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=2}  ,
             new SqlParameter{ParameterName="@userId",Value=regisID} ,
              new SqlParameter{ParameterName="@registration",Value=registration} 
            };
            var _proc = @"proc_getDECList @procId,@userId ,@registration";
            var slist = this.Database.SqlQuery<CHCModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        #endregion
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
       

        #region Medico Legal Azeez
        public ProcessType RegistrationCount_MLC(long userId)
        {
            var sqlParam = new SqlParameter[] { 
                new SqlParameter{ParameterName="@userId",Value=userId} 
            };
            var _proc = @"proc_RegistrationCount_MLC @userId";
            var slist = this.Database.SqlQuery<ProcessType>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }

        public List<MLCDetailsModel> GetAllRegistration_MLC(long userId, DataTable Ids, string registrationNo, string appliedDate)
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@userId",Value=userId}  ,
             new SqlParameter{ParameterName="@Ids",Value=Ids,TypeName="[dbo].[Ids]"}  ,
             new SqlParameter{ParameterName="@registrationNo",Value=registrationNo}  ,
             new SqlParameter{ParameterName="@appliedDate",Value=string.IsNullOrEmpty(appliedDate)?(object)DBNull.Value:Convert.ToDateTime(appliedDate)}
             
            };
            var _proc = @"proc_GetAllRegistration_MLC @userId,@Ids,@registrationNo,@appliedDate";
            var slist = this.Database.SqlQuery<MLCDetailsModel>(_proc, sqlParam).ToList();
            return slist;
        }

        #region Method Get Committee Details For DLL for MLC
        public List<SelectListItem> proc_GetCommitteeDetailsForDLL_MLC()
        {
            var _proc = @"proc_GetCommitteeDetailsForDLL_MLC";
            var slist = this.Database.SqlQuery<SelectListItem>(_proc).ToList();
            return slist;
        }
        #endregion

        #region Method Get Committee Member MLC
        public List<DropDownList> GetCommitteeMember_MLC(long regisIdMLC)
        {
            var sqlparam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@regisIdMLC",Value=regisIdMLC}
            };
            var _proc = @"proc_GetCommitteeMember_MLC @regisIdMLC";
            var slist = this.Database.SqlQuery<DropDownList>(_proc, sqlparam).ToList();
            return slist;
        }
        #endregion

        public ResultSet UpdateAppProcessMLC(long regisIdMLC, int appStatus, long committeeId, string inspectionDate, string inspReportFilePath, string certificateFilePath, string rejectedRemarks, long userId, string transIp, string XmlData, string treatmentFrom, string treatmentto, int restFor)//NUHAppProcessModel model
        {
            var sqlParam = new SqlParameter[] { 
               new SqlParameter("@regisIdMLC", regisIdMLC),
               new SqlParameter("@appStatus", appStatus),
               new SqlParameter("@committeeId", committeeId),
               new SqlParameter("@inspectionDate", string.IsNullOrEmpty(inspectionDate) ? (object)DBNull.Value : Convert.ToDateTime(inspectionDate)),
               new SqlParameter("@inspReportFilePath", inspReportFilePath ?? string.Empty),
               new SqlParameter("@certificateFilePath", certificateFilePath ?? string.Empty),
               new SqlParameter("@rejectedRemarks", rejectedRemarks ?? string.Empty),
               new SqlParameter("@userId", userId),
               new SqlParameter("@transIp", transIp),
               new SqlParameter("@XmlData", XmlData ?? string.Empty),
               new SqlParameter("@treatmentFrom", treatmentFrom ?? string.Empty),
               new SqlParameter("@treatmentto", treatmentto ?? string.Empty),
               new SqlParameter("@restFor", restFor)

            };
            var _proc = @"proc_UpdateAppProcessMLC @regisIdMLC,@appStatus,@committeeId,@inspectionDate,@inspReportFilePath,@certificateFilePath,@rejectedRemarks,@userId,@transIp,@XmlData,@treatmentFrom,@treatmentto,@restFor";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }

        public List<MLCDetailsModel> GetScheduleOfCommittee_MLC(long committeeId, string inspectiondate)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@committeeId",Value=committeeId}  ,
             new SqlParameter{ParameterName="@inspectiondate",Value=inspectiondate}
             
            };
            var _proc = @"proc_GetScheduleOfCommittee_MLC @committeeId,@inspectiondate";
            var slist = this.Database.SqlQuery<MLCDetailsModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public ResultSet GenrateCertificateMLC(MLCAppProcessModel model)
        {
            var sqlParam = new SqlParameter[] { 
               new SqlParameter("@regisIdMLC", model.regisIdMLC),
               new SqlParameter("@appStatus", model.appStatus),
               new SqlParameter("@userId", model.userId),
               new SqlParameter("@transIp", model.transIp)

            };
            var _proc = @"proc_GenrateCertificateMLC @regisIdMLC,@appStatus,@userId,@transIp";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }

        #region Get Nominee Details
        public List<MLCNomineeModel> GetNomineeDetails(long regisId)
        {
            var sqlParam = new SqlParameter[] { 

             new SqlParameter{ParameterName="@regisId",Value=regisId}
             
            };
            var _proc = @"Proc_GetNomineeDetails_MLC @regisId";
            var result = this.Database.SqlQuery<MLCNomineeModel>(_proc, sqlParam).ToList();
            return result;
        }
        #endregion

        #endregion

        #region Immunization Certificate for Children Azeez
        public ProcessType RegistrationCount_ICC(long userId)
        {
            var sqlParam = new SqlParameter[] { 
                new SqlParameter{ParameterName="@userId",Value=userId} 
            };
            var _proc = @"proc_RegistrationCount_ICC @userId";
            var slist = this.Database.SqlQuery<ProcessType>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }

        public List<ICCDetailsModel> GetAllRegistration_ICC(long userId, DataTable Ids, string registrationNo, string appliedDate)
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@userId",Value=userId}  ,
             new SqlParameter{ParameterName="@Ids",Value=Ids,TypeName="[dbo].[Ids]"}  ,
             new SqlParameter{ParameterName="@registrationNo",Value=registrationNo}  ,
             new SqlParameter{ParameterName="@appliedDate",Value=string.IsNullOrEmpty(appliedDate)?(object)DBNull.Value:Convert.ToDateTime(appliedDate)}
             
            };
            var _proc = @"proc_GetAllRegistration_ICC @userId,@Ids,@registrationNo,@appliedDate";
            var slist = this.Database.SqlQuery<ICCDetailsModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public ResultSet UpdateAppProcessICC(long regisIdICC, int appStatus, string certificateFilePath, string rejectedRemarks, long userId, string transIp)
        {
            var sqlParam = new SqlParameter[] { 
               new SqlParameter("@regisIdICC", regisIdICC),
               new SqlParameter("@appStatus", appStatus),
               new SqlParameter("@certificateFilePath", certificateFilePath ?? string.Empty),
               new SqlParameter("@rejectedRemarks", rejectedRemarks ?? string.Empty),
               new SqlParameter("@userId", userId),
               new SqlParameter("@transIp", transIp)
            };
            var _proc = @"proc_UpdateAppProcessICC @regisIdICC,@appStatus,@certificateFilePath,@rejectedRemarks,@userId,@transIp";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }
         
        public ICCAppProcessModel GetICCList(long regisId, int appStatus)
        { 
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisId},
            new SqlParameter{ParameterName="@appliedStatus",Value=appStatus}
            };
            var _proc = @"getICCDetail @regisId,@appliedStatus";
            var slist = this.Database.SqlQuery<ICCAppProcessModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }

        #endregion
         
        public ResultSet UpdateFICCertificate(long regisId, string certificatePath, long regisBy, string Ipaddress)
        {
            var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@regisId",Value=regisId}  ,
              new SqlParameter{ParameterName="@certificatePath",Value=certificatePath} ,
              new SqlParameter{ParameterName="@regisBy",Value=regisBy}  ,
              new SqlParameter{ParameterName="@Ipaddress",Value=Ipaddress} 
             
            };
            var _proc = @"proc_FICUpdate @regisId,@certificatePath,@regisBy ,@Ipaddress";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }

        public ResultSet UpdateICCCertificate(long regisId, string certificatePath, long regisBy, string Ipaddress)
        {
            var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@regisId",Value=regisId}  ,
              new SqlParameter{ParameterName="@certificatePath",Value=certificatePath} ,
              new SqlParameter{ParameterName="@regisBy",Value=regisBy}  ,
              new SqlParameter{ParameterName="@Ipaddress",Value=Ipaddress} 
             
            };
            var _proc = @"proc_ICCUpdate @regisId,@certificatePath,@regisBy ,@Ipaddress";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
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
            var _proc = @"proc_ILCUpdate @regisId,@certificatePath,@regisBy ,@Ipaddress";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }

        public ResultSet UpdateIMCCertificate(long regisId, string certificatePath, long regisBy, string Ipaddress)
        {
            var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@regisId",Value=regisId}  ,
              new SqlParameter{ParameterName="@certificatePath",Value=certificatePath} ,
              new SqlParameter{ParameterName="@regisBy",Value=regisBy}  ,
              new SqlParameter{ParameterName="@Ipaddress",Value=Ipaddress} 
             
            };
            var _proc = @"proc_IMCUpdate @regisId,@certificatePath,@regisBy ,@Ipaddress";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }

        public ResultSet UpdateMLCCertificate(long regisId, string certificatePath, long regisBy, string Ipaddress)
        {
            var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@regisId",Value=regisId}  ,
              new SqlParameter{ParameterName="@certificatePath",Value=certificatePath} ,
              new SqlParameter{ParameterName="@regisBy",Value=regisBy}  ,
              new SqlParameter{ParameterName="@Ipaddress",Value=Ipaddress} 
             
            };
            var _proc = @"proc_MLCUpdate @regisId,@certificatePath,@regisBy ,@Ipaddress";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }

        public ResultSet UpdateDECCertificate(long regisId, string certificatePath, long regisBy, string Ipaddress)
        {
            var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@regisId",Value=regisId}  ,
              new SqlParameter{ParameterName="@certificatePath",Value=certificatePath} ,
              new SqlParameter{ParameterName="@regisBy",Value=regisBy}  ,
              new SqlParameter{ParameterName="@Ipaddress",Value=Ipaddress} 
             
            };
            var _proc = @"proc_DECUpdate @regisId,@certificatePath,@regisBy ,@Ipaddress";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }
        //------------------------------------------------------------------
        #region AmitendraSingh
        public NUHDetailsModel GetCertificate(string ServiceType, long regisId = 0)
        {
            var sqlParam = new SqlParameter[] { 
           new SqlParameter{ParameterName="@ServiceType",Value=ServiceType} ,
             new SqlParameter{ParameterName="@regisId",Value=regisId} 
             
            };
            var _proc = @"proc_getCertificateNo @ServiceType,@regisId";
            var slist = this.Database.SqlQuery<NUHDetailsModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        #endregion 
    
        #region ISP

        public List<AcknowledgementReq> GetUserDetailsForStatusPush(string applicationid)
        {
            var sqlparams = new SqlParameter[] {


            new SqlParameter{ParameterName="@application_id",Value=applicationid}
             };
            var _proc = @"GetISPUseDetailsForStatusUpdate @application_id";
            var slist = this.Database.SqlQuery<AcknowledgementReq>(_proc, sqlparams).ToList();
            return slist;
        }

        #endregion
    }
}