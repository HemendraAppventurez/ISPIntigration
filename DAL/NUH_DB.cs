using CCSHealthFamilyWelfareDept.Models;
using CCSHealthFamilyWelfareDept.ReportModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class NUH_DB : DbContext
    {
        SessionManager objSM = new SessionManager();

        #region Default Constructor
        public NUH_DB()
            : base("CMSModule")
        { }
        #endregion

        #region NUH new

        #region Abhijeet Code

        public ResultSet IsRegister()
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser}
            };
            var _proc = @"proc_checkuserNUH @regisId";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }

        #region check email existence
        //public ResultSet CheckEmailMobileExistence(string checkValue, string Type)
        //{
        //    var sqlParams = new SqlParameter[] { 
        //            new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
        //            new SqlParameter { ParameterName = "@Type", Value = Type }

        //        };

        //    var sqlQuery = @"proc_checkEmailMobleExistenceNUH @checkValue,@Type";
        //    var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
        //    return sDetails;
        //}
        public ResultSet CheckEmailMobileExistence(string checkValue, string Type, long regisId = 0, string meeRegisNo = "", long regByUser = 0)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@RegisId", Value = regisId },
                    new SqlParameter { ParameterName = "@meeRegisNo", Value = meeRegisNo },
                    new SqlParameter { ParameterName = "@regByUser", Value = regByUser }
                };

            var sqlQuery = @"proc_checkEmailMobleExistenceNUH @checkValue,@Type, @RegisId,@meeRegisNo,@regByUser";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion

        public ResultSet InsertUpdateNursingHome(NUHmodel model)
        {
            var sqlParams = new SqlParameter[] {
                        new SqlParameter { ParameterName = "@regisIdNUH ", Value =model.regisIdNUH} ,
                        new SqlParameter { ParameterName = "@regByUser ", Value =model.regByUser} ,
                        new SqlParameter { ParameterName = "@medicalEstablishmentId ", Value =model.medicalEstablishmentId} ,
                         new SqlParameter { ParameterName = "@medicalEstablishmentOther ", Value =model.medicalEstablishmentOther??string.Empty} ,
                        new SqlParameter { ParameterName = "@establishmentCategoriesId ", Value =model.establishmentCategoriesId} ,
                        new SqlParameter { ParameterName = "@establishmentSubCategoriesId", Value =model.establishmentSubCategoriesId} ,
                        new SqlParameter { ParameterName = "@establishmentName ", Value =model.establishmentName??string.Empty} ,
                        new SqlParameter { ParameterName = "@stateId ", Value =model.stateId} ,
                        new SqlParameter { ParameterName = "@districtid", Value =model.districtid} ,
                        new SqlParameter { ParameterName = "@address ", Value =model.address} ,
                        new SqlParameter { ParameterName = "@pinCode ", Value =model.pinCode} ,
                        new SqlParameter { ParameterName = "@addressproofFilePath ", Value =model.addressproofFilePath} ,
                        new SqlParameter { ParameterName = "@telephoneNo", Value =model.telephoneNo??string.Empty} ,
                        new SqlParameter { ParameterName = "@website ", Value =model.website??string.Empty} ,
                        new SqlParameter { ParameterName = "@medicalFacilities ", Value =model.medicalFacilities} ,
                        new SqlParameter { ParameterName = "@isBelongToMedical ", Value =model.isBelongToMedical} ,
                        new SqlParameter { ParameterName = "@applicantName", Value =model.applicantName} ,
                         new SqlParameter { ParameterName = "@applicantAddress", Value =model.applicantAddress} ,
                          new SqlParameter { ParameterName = "@applicantStateId", Value =model.applicantStateId} ,
                           new SqlParameter { ParameterName = "@applicantDistrictId", Value =model.applicantDistrictId} ,
                            new SqlParameter { ParameterName = "@applicantPincode", Value =model.applicantPincode} ,
                        new SqlParameter { ParameterName = "@qualification ", Value =model.qualification} ,
                        new SqlParameter { ParameterName = "@institution ", Value =model.institution} ,
                        new SqlParameter { ParameterName = "@registrationNumber ", Value =model.registrationNumber??(object)DBNull.Value} ,
                        new SqlParameter { ParameterName = "@Central_StateCouncilName ", Value =model.Central_StateCouncilName??(object)DBNull.Value} ,
                        new SqlParameter { ParameterName = "@applicantMobileNo ", Value =model.applicantMobileNo} ,
                        new SqlParameter { ParameterName = "@applicantEmailId ", Value =model.applicantEmailId} ,
                        new SqlParameter { ParameterName = "@upmci_smfCertificateFilepath ", Value =model.upmci_smfCertificateFilePath??(object)DBNull.Value} ,
                        new SqlParameter { ParameterName = "@medicinSystemId ", Value =model.medicinSystemId} ,
                         new SqlParameter { ParameterName = "@numberofBed", Value =model.numberofBed} ,
                        new SqlParameter { ParameterName = "@clinicalServicesId ", Value =model.clinicalServicesId} ,
                        new SqlParameter { ParameterName = "@clinicalEstablishmentTypeId ", Value =model.clinicalEstablishmentTypeId} ,
                        new SqlParameter { ParameterName = "@clinicalEstablishmentSubTypeId", Value =model.clinicalEstablishmentSubTypeId} ,
                        new SqlParameter { ParameterName = "@clinicalEstablishmentTypeOther ", Value =model.clinicalEstablishmentTypeOther??string.Empty} ,
                        new SqlParameter { ParameterName = "@isNOC ", Value =model.isNOC} ,
                         new SqlParameter { ParameterName = "@nocCertificationNo ", Value =model.nocCertificationNo??string.Empty} ,
                        new SqlParameter { ParameterName = "@nOCFilePath ", Value =model.nOCFilePath??string.Empty} ,
                        new SqlParameter { ParameterName = "@isDispose ", Value =model.isDispose} ,
                        new SqlParameter { ParameterName = "@disposedNo ", Value =model.disposedNo??string.Empty} ,
                        new SqlParameter { ParameterName = "@disposedFile ", Value =model.disposedFilePath??string.Empty} ,
                        new SqlParameter { ParameterName = "@isFirefightingSystem ", Value =model.isFirefightingSystem} ,
                        new SqlParameter { ParameterName = "@firefightingSystemFilePath", Value =model.firefightingSystemFilePath??string.Empty} ,
                        new SqlParameter { ParameterName = "@notarizedAffidavitFilePath ", Value =model.notarizedAffidavitFilePath??string.Empty} ,
                        new SqlParameter { ParameterName = "@regBytransIp", Value =model.regBytransIp} ,
                        new SqlParameter { ParameterName = "@transIP ", Value =model.transIP} ,
                        new SqlParameter { ParameterName = "@xmldata ", Value =model.xml??(object)DBNull.Value},
                        new SqlParameter { ParameterName = "@otherEstablishmentCategory ", Value =model.otherEstablishmentCategory??string.Empty} ,
                        new SqlParameter { ParameterName = "@otherServiceType ", Value =model.otherServiceType??string.Empty},
                        new SqlParameter { ParameterName = "@requestKey", Value =model.requestKey??string.Empty},
                        new SqlParameter { ParameterName = "@operateId", Value =model.operatedId},
                          new SqlParameter { ParameterName = "@HfrNo ", Value =model.HfrNo??string.Empty},
                        new SqlParameter { ParameterName = "@operatedName", Value =model.operatedName},


                        new SqlParameter { ParameterName = "@establishmentArea ", Value =model.establishmentArea} ,
                        new SqlParameter { ParameterName = "@establishmentPlace ", Value =model.establishmentPlace} ,
                        new SqlParameter { ParameterName = "@landType ", Value =model.landType} ,
                        new SqlParameter { ParameterName = "@structuralLyoutFile ", Value =model.structuralLyoutFilePath??(object)DBNull.Value} ,
                      
                        //new SqlParameter { ParameterName = "@piAge", Value =model.piAge} ,
                        //new SqlParameter { ParameterName = "@piFatherName ", Value =model.piFatherName} ,
                      
                        new SqlParameter { ParameterName = "@piPhotograph ", Value =model.piPhotographPath},
                        //new SqlParameter { ParameterName = "@piSignature ", Value =model.piSignaturePath} ,

                        new SqlParameter { ParameterName = "@XmlDataOwner ", Value =model.XmlDataOwner},
                        new SqlParameter { ParameterName = "@XmlDataDoc ", Value =model.XmlDataDoc},
                        new SqlParameter { ParameterName = "@isInPatient ", Value =model.isInPatient},
                        new SqlParameter { ParameterName = "@isOutPatient ", Value =model.isOutPatient},
                        new SqlParameter { ParameterName = "@isLaboratory ", Value =model.isLaboratory},
                        new SqlParameter { ParameterName = "@isImaging ", Value =model.isImaging},
                        new SqlParameter { ParameterName = "@otherOutPatient ", Value =model.otherOutPatient??string.Empty},
                        new SqlParameter { ParameterName = "@otherLaboratory ", Value =model.otherLaboratory??string.Empty},
                        new SqlParameter { ParameterName = "@otherImaging ", Value =model.otherImaging??string.Empty},
                        new SqlParameter { ParameterName = "@otherFacility ", Value =model.otherFacility??string.Empty},
                        new SqlParameter { ParameterName = "@xmldataOutPatient ", Value =model.xmldataOutPatient??string.Empty},
                        new SqlParameter { ParameterName = "@xmldataLaboratory ", Value =model.xmldataLaboratory??string.Empty},
                        new SqlParameter { ParameterName = "@xmldataImaging ", Value =model.xmldataImaging??string.Empty},
                        new SqlParameter { ParameterName = "@isRenewal ", Value =model.isRenewal},
                        new SqlParameter { ParameterName = "@isCertificateFromPortal ", Value =model.isCertificateFromPortal},
                        new SqlParameter { ParameterName = "@outerRegistrationNo ", Value =model.outerRegistrationNo??(object)DBNull.Value},
                        new SqlParameter { ParameterName = "@outerCertificateNo ", Value =model.outerCertificateNo??(object)DBNull.Value},
                        new SqlParameter { ParameterName = "@outerCertificateFile ", Value =model.outerCertificateFilePath??(object)DBNull.Value},
                        new SqlParameter { ParameterName = "@establishmentPlaceOther", Value =model.establishmentPlaceOther??(object)DBNull.Value} ,
                           new SqlParameter { ParameterName = "@ElectrycityBill", Value =model.ElectrycityBillPath??string.Empty},
                              new SqlParameter { ParameterName = "@Registry ", Value =model.RegistryPath??string.Empty},
                                 new SqlParameter { ParameterName = "@RentalAgreement ", Value =model.RentalAgreementPath??string.Empty}
        };

            var sqlProc = @"Sp_InsertUpdateNUH @regisIdNUH ,
      @regByUser  ,
      @medicalEstablishmentId  ,
      @medicalEstablishmentOther  ,
      @establishmentCategoriesId  ,
      @establishmentSubCategoriesId  ,
      @establishmentName  ,
      @stateId  ,
      @districtid  ,
      @address ,
      @pinCode ,
      @addressproofFilePath ,
      @telephoneNo ,
      @website ,
      @medicalFacilities,
      @isBelongToMedical,
      @applicantName ,
      @applicantAddress ,
      @applicantStateId ,
      @applicantDistrictId ,
      @applicantPincode,
      @qualification,
      @institution,
      @registrationNumber ,
      @Central_StateCouncilName ,
      @applicantMobileNo,
      @applicantEmailId ,
      @upmci_smfCertificateFilepath ,
      @medicinSystemId ,
      @clinicalServicesId ,
      @numberofBed ,
      @clinicalEstablishmentTypeId ,
      @clinicalEstablishmentSubTypeId ,
      @clinicalEstablishmentTypeOther ,
      @isNOC ,
      @nocCertificationNo ,
      @nOCFilePath ,
      @isDispose  ,
      @disposedNo ,
      @disposedFile,
      @isFirefightingSystem ,
      @firefightingSystemFilePath ,
      @notarizedAffidavitFilePath ,
      @regBytransIp ,
      @transIP,
      @xmldata,
      @otherEstablishmentCategory ,
      @otherServiceType ,
      @requestKey,
      @operateId ,
      @HfrNo,
      @operatedName ,
      @establishmentArea ,
      @establishmentPlace ,
      @landType,
     @structuralLyoutFile,
      @piPhotograph ,
    
      @XmlDataOwner  ,
      @XmlDataDoc  ,
      @isInPatient  ,
      @isOutPatient  ,
      @isLaboratory  ,
      @isImaging  ,
      @otherOutPatient,
      @otherLaboratory ,
      @otherImaging,
      @otherFacility ,
      @xmldataOutPatient  ,
      @xmldataLaboratory  ,
      @xmldataImaging,
@isRenewal,
@isCertificateFromPortal,
@outerRegistrationNo,
@outerCertificateNo,
@outerCertificateFile,
@establishmentPlaceOther,
@ElectrycityBill,
@Registry,
@RentalAgreement
";
            var sList = this.Database.SqlQuery<ResultSet>(sqlProc, sqlParams).ToList().FirstOrDefault();
            return sList;
        }

        public List<NUHmodel> GetNUHList(long userID)//getuserwiselist
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=1}  ,
              new SqlParameter{ParameterName="@userId",Value=userID} 
            };
            var _proc = @"proc_getNuHList @procId,@userId";
            var slist = this.Database.SqlQuery<NUHmodel>(_proc, sqlParam).ToList();
            return slist;
        }


        //Get Nivesh user Details
        //public NiveshMitraSendStatusModel GetNiveshUserDetailsToSendMedicalRegisStatus(long registerByUserID)
        //{
        //    var sqlParam = new SqlParameter[] { 
        //   // new SqlParameter{ParameterName="@procId",Value=2}  ,
        //     new SqlParameter{ParameterName="@registerByUserID",Value=registerByUserID} 
        //    };
        //    var _proc = @"Proc_GetNiveshUserDetailsToSendMedicalRegisStatus @registerByUserID";
        //    var slist = this.Database.SqlQuery<NiveshMitraSendStatusModel>(_proc, sqlParam).SingleOrDefault();
        //    return slist;
        //}

        public NiveshMitraSendStatusModel GetNiveshUserDetailsToSendMedicalRegisStatus(long regisIdNUH)
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@regisIdNUH",Value=regisIdNUH} 
            };
            var _proc = @"Proc_GetNiveshUserDetailsToSendMedicalRegisStatus @regisIdNUH";
            var slist = this.Database.SqlQuery<NiveshMitraSendStatusModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }


        #region Vinod Get QueryDetails

        public QueryModel GetQueryDetailsByCMO(long regisID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@MedicalEstID",Value=regisID}
              
            };
            var _proc = @"Pro_GetQueryDetails @MedicalEstID";
            var slist = this.Database.SqlQuery<QueryModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        #endregion

        public NUHmodel GetNUHListBYRegistrationNo(long regisID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=2}  ,
             new SqlParameter{ParameterName="@userId",Value=regisID} 
            };
            var _proc = @"proc_getNuHList @procId,@userId";
            var slist = this.Database.SqlQuery<NUHmodel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public NUHmodel GetNUHListForQueryReply(long regisID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=3}  ,
             new SqlParameter{ParameterName="@userId",Value=regisID} 
            };
            var _proc = @"proc_getNuHList @procId,@userId";
            var slist = this.Database.SqlQuery<NUHmodel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public List<NUHmodel> getNUHChild(long regisFAP)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisFAP}  
            };
            var _proc = @"proc_getNUHChild @regisId";
            var slist = this.Database.SqlQuery<NUHmodel>(_proc, sqlParam).ToList();
            return slist;
        }
        public List<NUHdoctorModel> getNUHdoc(long regisNUHId)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisNUHId}  
            };
            var _proc = @"proc_getNUHdoc @regisId";
            var slist = this.Database.SqlQuery<NUHdoctorModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public List<NUHPartnerModel> getNUHPartner(long regisNUHId)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisNUHId}  
            };
            var _proc = @"proc_getNUHPartnerRenewal @regisId";
            var slist = this.Database.SqlQuery<NUHPartnerModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public List<rptNUHModel> GetDetail(long regisId)
        {
            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisId}  
            };
            var _proc = @"proc_rptNUH @regisId";
            var slist = this.Database.SqlQuery<rptNUHModel>(_proc, sqlParam).ToList();
            return slist;
        }
        #endregion



        public int UploadAffidavitNUH(long regisIdNUH, string notarizedAffidavitFilePath)
        {
            int res = 0;
            res = this.Database.ExecuteSqlCommand("proc_UploadAffidavitNUH @regisIdNUH,@notarizedAffidavitFilePath",
               new SqlParameter("@regisIdNUH", regisIdNUH),
               new SqlParameter("@notarizedAffidavitFilePath", notarizedAffidavitFilePath)
               );
            return res;
        }

        public List<rptCertificateNUHModel> GetDetails(long regisId)
        {
            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisId}  
            };
            var _proc = @"GetNUHrptDetail @regisId";
            var slist = this.Database.SqlQuery<rptCertificateNUHModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public List<rptNHUChild> getNUHChilds(long regisNUH)
        {
            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisNUH}  
            };
            var _proc = @"proc_getNUHChild @regisId";
            var slist = this.Database.SqlQuery<rptNHUChild>(_proc, sqlParam).ToList();
            return slist;
        }

        public RegisterDetailsModel GetRegisterDetails(bool isRenewal = false)
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisByuser},
            new SqlParameter{ParameterName="@isRenewal",Value=isRenewal}
         
            };
            var _proc = @"proc_RegisterDetailsNUH @regisId,@isRenewal";
            var slist = this.Database.SqlQuery<RegisterDetailsModel>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }

        #region Riya
        public List<NUHmodel> GetAllNUHList()
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=1}  
             
            };
            var _proc = @"proc_getAllNuHList @procId";
            var slist = this.Database.SqlQuery<NUHmodel>(_proc, sqlParam).ToList();
            return slist;
        }
        public List<NUHmodel> GetoutPatient(long regisId)
        {
            var sqlparams = new SqlParameter[] {            
             new SqlParameter{ParameterName="@regisId",Value=regisId}
              
            };
            var _proc = @"proc_GetoutPatient @regisId";
            var slist = this.Database.SqlQuery<NUHmodel>(_proc, sqlparams).ToList();
            return slist;
        }
        public List<NUHmodel> GetNUHlaboratory(long regisId)
        {
            var sqlparams = new SqlParameter[] {            
             new SqlParameter{ParameterName="@regisId",Value=regisId}
              
            };
            var _proc = @"proc_GetNUHlaboratory @regisId";
            var slist = this.Database.SqlQuery<NUHmodel>(_proc, sqlparams).ToList();
            return slist;
        }
        public List<NUHmodel> GetNUHimaging(long regisId)
        {
            var sqlparams = new SqlParameter[] {            
             new SqlParameter{ParameterName="@regisId",Value=regisId}
              
            };
            var _proc = @"proc_GetNUHimaging @regisId";
            var slist = this.Database.SqlQuery<NUHmodel>(_proc, sqlparams).ToList();
            return slist;
        }
        #endregion

        #region Ankita
        public List<AGCModel> GetAllAGCList()
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=1}  
             
            };
            var _proc = @"proc_getAllAGCList @procId";
            var slist = this.Database.SqlQuery<AGCModel>(_proc, sqlParam).ToList();
            return slist;
        }
        #endregion

        #region Delete Registration NUH
        public int DeleteRegistrationNUH(long regisIdNUH)
        {
            var sqlParams = new SqlParameter[] {    
                 new SqlParameter { ParameterName = "@regisIdNUH", Value = regisIdNUH} 
            };
            var query = "proc_DeleteRegistration_NUH @regisIdNUH";
            var result = this.Database.ExecuteSqlCommand(query, sqlParams);
            return result;
        }
        #endregion

        public List<rptCertificateNUHModel> GetESDetailsByCMOId(long regisId)
        {
            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@cmoId",Value=regisId}  
            };
            var _proc = @"GetCompleteESDetailsByCMOId @cmoId";
            var slist = this.Database.SqlQuery<rptCertificateNUHModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public ResultSet SearchDetails(SearchDetailModel model, long userId)
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@userId",Value=userId},
            new SqlParameter{ParameterName="@meeRegisNo",Value=model.meeRegisNo},
            new SqlParameter{ParameterName="@certificateNo",Value=model.certificateNo}
            };
            var _proc = @"proc_SearchDetailsForRenewal @userId,@meeRegisNo,@certificateNo";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }

        public NUHmodel GetDetailsNUHByRegisId(long regisID)
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@regisId",Value=regisID} 
            };
            var _proc = @"proc_GetDetailsNUHByRegisId @regisId";
            var slist = this.Database.SqlQuery<NUHmodel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        #endregion

        #region NUH Renew
        public List<NUHmodel> getNUHdoctorList(long regisIdNUH)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdNUH",Value=regisIdNUH}
              };
            var _proc = @"proc_getNUHDoctor  @regisIdNUH";
            var res = this.Database.SqlQuery<NUHmodel>(_proc, sqlparams).ToList();
            return res;
        }

        public List<NUHmodel> getNUHParamedicalList(long regisIdNUH)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdNUH",Value=regisIdNUH}
              };
            var _proc = @"proc_getNUH_ParamedicalStaff  @regisIdNUH";
            var res = this.Database.SqlQuery<NUHmodel>(_proc, sqlparams).ToList();
            return res;
        }

        public List<NUH_FacilityOffered> NUH_FacilityOfferedForRenewal(int procId, long Id)
        {
            var sqlparam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@procId",Value=procId},
              new SqlParameter{ParameterName="@reregisIdNUH",Value=Id}
            };
            var _proc = @"NUH_FacilityOfferedForRenewal @procId,@reregisIdNUH";
            var slist = this.Database.SqlQuery<NUH_FacilityOffered>(_proc, sqlparam).ToList();
            return slist;
        }

        public ResultSet NUHInsertUpdateRenewal(NUHmodel model)
        {
            var sqlParams = new SqlParameter[] {
                        new SqlParameter { ParameterName = "@regisIdNUH", Value =model.regisIdNUH} ,
                        new SqlParameter { ParameterName = "@regByUser", Value =model.regByUser} ,
                        new SqlParameter { ParameterName = "@isMEEaddressChange", Value =model.isMEEaddressChange} ,
                        new SqlParameter { ParameterName = "@telephoneNo", Value =model.telephoneNo??string.Empty} ,
                        new SqlParameter { ParameterName = "@website", Value =model.website??string.Empty} ,
                        new SqlParameter { ParameterName = "@address", Value =model.address} ,
                        new SqlParameter { ParameterName = "@pinCode", Value =model.pinCode} ,
                        new SqlParameter { ParameterName = "@addressproofFilePath", Value =model.addressproofFilePath} ,
                        new SqlParameter { ParameterName = "@medicalFacilities", Value =model.medicalFacilities} ,
                        new SqlParameter { ParameterName = "@isInPatient", Value =model.isInPatient},
                        new SqlParameter { ParameterName = "@numberofBed", Value =model.numberofBed},
                        new SqlParameter { ParameterName = "@isOutPatient", Value =model.isOutPatient},
                        new SqlParameter { ParameterName = "@otherOutPatient", Value =model.otherOutPatient??string.Empty},
                        new SqlParameter { ParameterName = "@isLaboratory", Value =model.isLaboratory},
                        new SqlParameter { ParameterName = "@otherLaboratory", Value =model.otherLaboratory??string.Empty},
                        new SqlParameter { ParameterName = "@isImaging", Value =model.isImaging},                       
                        new SqlParameter { ParameterName = "@otherImaging", Value =model.otherImaging??string.Empty},
                        new SqlParameter { ParameterName = "@clinicalEstablishmentTypeOther", Value =model.clinicalEstablishmentTypeOther??string.Empty},
                        new SqlParameter { ParameterName = "@xmldataOutPatient", Value =model.xmldataOutPatient??string.Empty},
                        new SqlParameter { ParameterName = "@xmldataLaboratory", Value =model.xmldataLaboratory??string.Empty},
                        new SqlParameter { ParameterName = "@xmldataImaging", Value =model.xmldataImaging??string.Empty},
                        new SqlParameter { ParameterName = "@xmldata", Value =model.xml??(object)DBNull.Value},//paramedical staff
                        new SqlParameter { ParameterName = "@XmlDataDoc", Value =model.XmlDataDoc},
                        new SqlParameter { ParameterName = "@transIP", Value =model.transIP} ,
                        new SqlParameter { ParameterName = "@requestKey", Value =model.requestKey??string.Empty},
                        new SqlParameter { ParameterName = "@meeRegisNo", Value =model.meeRegisNo},
                        new SqlParameter { ParameterName = "@applicantMobileNo", Value =model.applicantMobileNo},
                        new SqlParameter { ParameterName = "@isCertificateFromPortal", Value =model.isCertificateFromPortal},
                        new SqlParameter { ParameterName = "@nOCFilePath ", Value =model.nOCFilePath??string.Empty} ,
                        new SqlParameter { ParameterName = "@disposedFile ", Value =model.disposedFilePath??string.Empty} ,
                        new SqlParameter { ParameterName = "@firefightingSystemFilePath", Value =model.firefightingSystemFilePath??string.Empty},                        
                        new SqlParameter { ParameterName = "@disposedNo ", Value =model.disposedNo??string.Empty} ,
                        new SqlParameter { ParameterName = "@nocCertificationNo ", Value =model.nocCertificationNo??string.Empty},

                        //Vinod
                        new SqlParameter { ParameterName = "@structuralLyoutFile", Value =model.structuralLyoutFilePath??string.Empty} ,
                        new SqlParameter { ParameterName = "@ownerFPhotograph", Value =model.ownerFPhotographPath??string.Empty} ,
                        new SqlParameter { ParameterName = "@ownerFSignature", Value =model.ownerFSignaturePath??string.Empty} ,
                        new SqlParameter { ParameterName = "@piPhotograph", Value =model.piPhotographPath??string.Empty},
                        new SqlParameter { ParameterName = "@upmci_smfCertificateFile", Value =model.upmci_smfCertificateFilePath??string.Empty} ,

                        //Abhishek
                         new SqlParameter { ParameterName = "@establishmentPlace", Value =model.establishmentPlace} ,
                        new SqlParameter { ParameterName = "@landType", Value =model.landType} ,
                            new SqlParameter { ParameterName = "@isNOC", Value =model.isNOC} ,
                        new SqlParameter { ParameterName = "@isFirefightingSystem", Value =model.isFirefightingSystem} ,
                          new SqlParameter { ParameterName = "@isDispose", Value =model.isDispose} ,

                          new SqlParameter { ParameterName = "@applicantEmailId", Value =model.applicantEmailId??(object)DBNull.Value} ,
                           new SqlParameter { ParameterName = "@qualification", Value =model.qualification} ,
                           new SqlParameter { ParameterName = "@institution", Value =model.institution} ,
                            new SqlParameter { ParameterName = "@Central_StateCouncilName", Value =model.Central_StateCouncilName??(object)DBNull.Value} ,
                             new SqlParameter { ParameterName = "@registrationNumber", Value =model.registrationNumber??(object)DBNull.Value} ,
                              new SqlParameter { ParameterName = "@applicantAddress", Value =model.applicantAddress} ,
                               new SqlParameter { ParameterName = "@applicantStateId", Value =model.applicantStateId} ,
                                 new SqlParameter { ParameterName = "@applicantDistrictId", Value =model.applicantDistrictId} ,
                                 new SqlParameter { ParameterName = "@applicantPincode", Value =model.applicantPincode} ,
                                  new SqlParameter { ParameterName = "@applicantName", Value =model.applicantName} ,
                                    new SqlParameter { ParameterName = "@ElectrycityBill", Value =model.ElectrycityBillPath??string.Empty},
                                      new SqlParameter { ParameterName = "@Registry", Value =model.RegistryPath??string.Empty},
                                          new SqlParameter { ParameterName = "@RentalAgreement", Value =model.RentalAgreementPath??string.Empty},
                                          new SqlParameter { ParameterName = "@HfrNo", Value =model.HfrNo??string.Empty}
        };

            var sqlProc = @"Sp_NUHInsertUpdateRenewal @regisIdNUH ,
                                                      @regByUser ,
                                                      @isMEEaddressChange ,
                                                      @telephoneNo ,
                                                      @website ,
                                                      @address  ,
                                                      @pinCode ,
                                                      @addressproofFilePath ,
                                                      @medicalFacilities,
                                                      @isInPatient ,
                                                      @numberofBed,
                                                      @isOutPatient ,
                                                      @otherOutPatient,
                                                      @isLaboratory ,
                                                      @otherLaboratory ,
                                                      @isImaging,
                                                      @otherImaging ,
                                                      @clinicalEstablishmentTypeOther,
                                                      @xmldataOutPatient,
                                                      @xmldataLaboratory ,
                                                      @xmldataImaging ,
                                                      @xmldata ,
                                                      @XmlDataDoc,
                                                      @transIP ,
                                                      @requestKey,
                                                      @meeRegisNo,
                                                      @applicantMobileNo,@isCertificateFromPortal,@nOCFilePath,@disposedFile,
@firefightingSystemFilePath,@disposedNo,@nocCertificationNo,@structuralLyoutFile,@ownerFPhotograph ,@ownerFSignature ,@piPhotograph,@upmci_smfCertificateFile,@establishmentPlace,@landType,@isNOC,@isFirefightingSystem,@isDispose,@applicantEmailId,@qualification,@institution,@Central_StateCouncilName,@registrationNumber,@applicantAddress,@applicantStateId,@applicantDistrictId,@applicantPincode,@applicantName,@ElectrycityBill,@Registry,@RentalAgreement,@HfrNo";
            var sList = this.Database.SqlQuery<ResultSet>(sqlProc, sqlParams).ToList().FirstOrDefault();
            return sList;
        }

        #endregion

        #region Alakshendra


        public Declaration ShowDeclarationdata(long regisID, int ProcId)
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@userId",Value=regisID} ,
                 new SqlParameter{ParameterName="@ProcId",Value=ProcId}
            };
            var _proc = @" SP_ShowDeclarationdata @userId,@ProcId";
            var slist = this.Database.SqlQuery<Declaration>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }



        public NUHmodel GetNUHListBYRegistrationNo_MEE(long regisID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=2}  ,
             new SqlParameter{ParameterName="@userId",Value=regisID} 
            };
            var _proc = @"proc_getNuHList_MEE @procId,@userId";
            var slist = this.Database.SqlQuery<NUHmodel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }

        #endregion


        #region Anku


        public Declaration ShowAffidavitData(long regisIdNUH)
        {
            var sqlParam = new SqlParameter[]{
           new SqlParameter{ParameterName="@regisIdNUH",Value=regisIdNUH}
           };
            var _proc = @"proc_ShowAffidavitData @regisIdNUH";
            var slist = this.Database.SqlQuery<Declaration>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }



        public Declaration UpdateAffidavitNUH(Declaration model)
        {
            var sqlParam = new SqlParameter[] {
           
             new SqlParameter{ParameterName="@regisIdNUH",Value=model.NuhId},
             new SqlParameter{ParameterName="@isDeclared",Value=model.isDeclared},
             new SqlParameter{ParameterName="@DeclarationIp",Value=model.DeclarationIp},
             new SqlParameter{ParameterName="@DeclarationUserId",Value=model.DeclarationUserId}
         };
            var _proc = @"Proc_UpdateAffidavit @regisIdNUH,@isDeclared,@DeclarationIp,@DeclarationUserId";
            var slist = this.Database.SqlQuery<Declaration>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }


        public ReceiptModel GetNUHReceipt(long regisIdNUH)
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@regisIdNUH",Value=regisIdNUH} 
            };
            var _proc = @"proc_GetNUHReceipt @regisIdNUH";
            var slist = this.Database.SqlQuery<ReceiptModel>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }

        public List<ReceiptModel> ReceiptList(long regisIdNUH)
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@regisId",Value=regisIdNUH} 
            };
            var _proc = @"proc_GetNUHReceipt @regisId";
            var slist = this.Database.SqlQuery<ReceiptModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public List<PairamedicalModel> GetParamedicalDetails(long regisIdNUH)
        {
            var sqlParam = new SqlParameter[]{
            new SqlParameter{ParameterName="@regisIdNUH",Value=regisIdNUH} 
         };
            var _proc = @"proc_GetParamedicalDetails @regisIdNUH";
            var slist = this.Database.SqlQuery<PairamedicalModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public List<Declaration> GetOwnerList(long regisIdNUH)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisIdNUH",Value=regisIdNUH}
            };
            var _proc = @"getownerList @regisIdNUH";
            var slist = this.Database.SqlQuery<Declaration>(_proc, sqlParam).ToList();
            return slist;
        }

        public PairamedicalModel UpdateParamedical(PairamedicalModel model)
        {
            var sqlParam = new SqlParameter[]{
            new SqlParameter{ParameterName="@regisIdNUH",Value=model.NuhId},
            new SqlParameter{ParameterName="@isParamedical",Value=model.isParamedicaDeclared},
            new SqlParameter{ParameterName="@ParamedicalDeclarationIp",Value=model.ParamedicalDeclarationIp},
            new SqlParameter{ParameterName="@ParamedicalDeclarationUserId",Value=model.ParamedicalDeclarationUserId},
            new SqlParameter{ParameterName="@doctor",Value=model.doctorlist},
            new SqlParameter{ParameterName="@DeclarationUserId",Value=model.DeclarationUserId},
            new SqlParameter{ParameterName="@DeclarationIp",Value=model.DeclarationIp}
            };
            var _proc = @"UpdateParamedical @regisIdNUH,@isParamedical,@ParamedicalDeclarationIp,@ParamedicalDeclarationUserId,@doctor,@DeclarationUserId,@DeclarationIp";

            var slist = this.Database.SqlQuery<PairamedicalModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }



        #endregion

        #region Get application list behalf RequestID

        public List<NUHmodel> GetApplicationByRequestId(int procId, string ControlID, string UnitID, string ServiceID, string RequestID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=procId}  ,
              new SqlParameter{ParameterName="@ControlID",Value=ControlID},
              new SqlParameter{ParameterName="@UnitID",Value=UnitID},
              new SqlParameter{ParameterName="@ServiceID",Value=ServiceID},
              new SqlParameter{ParameterName="@RequestID",Value=RequestID} 
            };
            var _proc = @"proc_GetApplicationByRequestId @procId,@ControlID,@UnitID,@ServiceID,@RequestID";
            var slist = this.Database.SqlQuery<NUHmodel>(_proc, sqlParam).ToList();
            return slist;
        }

        public NUHmodel GetDistrictByRequestId(int procId, string ControlID, string UnitID, string ServiceID, string RequestID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=procId}  ,
              new SqlParameter{ParameterName="@ControlID",Value=ControlID},
              new SqlParameter{ParameterName="@UnitID",Value=UnitID},
              new SqlParameter{ParameterName="@ServiceID",Value=ServiceID},
              new SqlParameter{ParameterName="@RequestID",Value=RequestID} 
            };
            var _proc = @"proc_GetApplicationByRequestId @procId,@ControlID,@UnitID,@ServiceID,@RequestID";
            var slist = this.Database.SqlQuery<NUHmodel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }

        #endregion

        #region
        public List<NiveshMitraRegistrationModel> UpdateOuterpostRequestData(string ControlID, string UnitID, string ServiceID, string RequestID, Int64 regisIdNUH)
        {
            var sqlParams = new SqlParameter[] {                 
                new SqlParameter { ParameterName = "@ControlID", Value =ControlID  },
                 new SqlParameter { ParameterName = "@UnitID", Value =UnitID  },
                 new SqlParameter { ParameterName = "@ServiceID", Value =ServiceID  },
                 new SqlParameter { ParameterName = "@RequestID", Value =RequestID  },
                 new SqlParameter { ParameterName = "@regisIdNUH", Value =regisIdNUH }
            };

            var sqlProc = "Proc_UpdateOuterpostRequestData @ControlID,@UnitID,@ServiceID,@RequestID,@regisIdNUH";
            var sList = this.Database.SqlQuery<NiveshMitraRegistrationModel>(sqlProc, sqlParams).ToList();
            return sList;
        }
        #endregion

    }
}