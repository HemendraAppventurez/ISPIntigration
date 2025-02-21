using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCSHealthFamilyWelfareDept.Models;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class AppRegistration_DB : DbContext
    {
        #region Default Constructor
        public AppRegistration_DB()
            : base("CMSModule")
        { }
        #endregion
        SessionManager objSM = new SessionManager();
        #region NUH
        #region check email existence
        public ResultSet CheckEmailMobileExistence(string checkValue, string Type, long regisId = 0)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@regisId", Value = regisId }

                };

            var sqlQuery = @"proc_checkEmailMobleExistenceNUH_CMO @checkValue,@Type,@regisId";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion
        public NUHmodel getNUHStep(long regisId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisNUHId",Value=regisId}
              };
            var _proc = @"proc_getNUH_Step  @regisNUHId";
            var res = this.Database.SqlQuery<NUHmodel>(_proc, sqlparams).FirstOrDefault();
            return res;
        }
        public Declaration ShowAffidavitData(long regisIdNUH)
        {
            var sqlParam = new SqlParameter[]{
           new SqlParameter{ParameterName="@regisIdNUH",Value=regisIdNUH}
           };
            var _proc = @"proc_ShowAffidavitData @regisIdNUH";
            var slist = this.Database.SqlQuery<Declaration>(_proc, sqlParam).FirstOrDefault();
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
        public List<PairamedicalModel> GetParamedicalDetails(long regisIdNUH)
        {
            var sqlParam = new SqlParameter[]{
            new SqlParameter{ParameterName="@regisIdNUH",Value=regisIdNUH} 
         };
            var _proc = @"proc_GetParamedicalDetails @regisIdNUH";
            var slist = this.Database.SqlQuery<PairamedicalModel>(_proc, sqlParam).ToList();
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
        public ResultSet InsertUpdateNUH(NUHmodel model)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdNUH",Value=model.regisIdNUH},
                          new SqlParameter {ParameterName="@regByUser",Value= model.regByUser},
                          new SqlParameter {ParameterName="@applicantName",Value=model.applicantName??string.Empty },
                          new SqlParameter {ParameterName="@applicantAddress",Value=model.applicantAddress ??string.Empty},
                          new SqlParameter {ParameterName="@applicantStateId",Value=model.applicantStateId },
                          new SqlParameter {ParameterName="@applicantDistrictId",Value= model.applicantDistrictId},
                          new SqlParameter {ParameterName="@applicantPincode",Value= model.applicantPincode??string.Empty},
                          new SqlParameter {ParameterName="@qualification",Value=model.qualification ??string.Empty},
                          new SqlParameter {ParameterName="@applicantMobileNo",Value=model.applicantMobileNo??string.Empty },
                          new SqlParameter {ParameterName="@applicantEmailId",Value=model.applicantEmailId ??string.Empty},
                          new SqlParameter {ParameterName="@registrationNumber",Value=model.registrationNumber??(object)DBNull.Value },
                          new SqlParameter {ParameterName="@regBytransIp",Value=model.regBytransIp },
                          new SqlParameter {ParameterName="@establishmentName",Value=model.establishmentName??string.Empty },
                          new SqlParameter {ParameterName="@medicalEstablishmentId",Value=model.medicalEstablishmentId },
                          new SqlParameter {ParameterName="@medicalEstablishmentOther",Value=model.medicalEstablishmentOther??string.Empty },
                           new SqlParameter {ParameterName="@medicalFacilities",Value=model.medicalFacilities??string.Empty },
                          new SqlParameter {ParameterName="@operatedId",Value=model.operatedId },
                          new SqlParameter {ParameterName="@operatedName",Value=model.operatedName ??string.Empty},
                          new SqlParameter {ParameterName="@stateId",Value=model.stateId },
                          new SqlParameter {ParameterName="@districtid",Value=model.districtid },
                          new SqlParameter {ParameterName="@pinCode",Value= model.pinCode??string.Empty},
                          new SqlParameter {ParameterName="@isFirefightingSystem",Value=model.isFirefightingSystem },
                          new SqlParameter {ParameterName="@isNOC",Value=model.isNOC },
                          new SqlParameter {ParameterName="@step",Value=model.step },
                          new SqlParameter {ParameterName="@xmldataParmedical",Value=model.xmldataParmedical??(object)DBNull.Value },
                          new SqlParameter {ParameterName="@xmldatadoctor",Value=model.xmldatadoctor??string.Empty },
                          new SqlParameter {ParameterName="@xmldatacheckList",Value=model.xmldatacheckList??string.Empty },
                          new SqlParameter {ParameterName="@stepValue",Value=model.stepValue},
                          new SqlParameter {ParameterName="@institution",Value=model.institution??string.Empty},
                          new SqlParameter {ParameterName="@Central_StateCouncilName",Value=model.Central_StateCouncilName??(object)DBNull.Value},

                          new SqlParameter {ParameterName="@establishmentArea",Value=model.establishmentArea??string.Empty},
                          new SqlParameter {ParameterName="@establishmentPlace",Value=model.establishmentPlace??string.Empty},
                          new SqlParameter {ParameterName="@landType",Value=model.landType??string.Empty},
                          new SqlParameter {ParameterName="@telephoneNo",Value=model.telephoneNo??string.Empty},
                          new SqlParameter {ParameterName="@website",Value=model.website??string.Empty},
                          new SqlParameter {ParameterName="@address",Value=model.address??string.Empty},
                          new SqlParameter {ParameterName="@nocCertificationNo",Value=model.nocCertificationNo??string.Empty},
                          new SqlParameter {ParameterName="@isDispose",Value=model.isDispose},
                          new SqlParameter {ParameterName="@disposedNo",Value=model.disposedNo??string.Empty},
                          new SqlParameter {ParameterName="@XmlDataOwner",Value=model.XmlDataOwner??string.Empty},
                          //new SqlParameter {ParameterName="@medicalFacilities",Value=model.medicalFacilities}
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

                         new SqlParameter { ParameterName = "@numberofBed ", Value =model.numberofBed},
                         new SqlParameter { ParameterName = "@clinicalEstablishmentTypeOther ", Value =model.clinicalEstablishmentTypeOther??string.Empty},
                        new SqlParameter { ParameterName = "@isRenewal ", Value =model.isRenewal},
                        new SqlParameter { ParameterName = "@isCertificateFromPortal ", Value =model.isCertificateFromPortal},
                        new SqlParameter { ParameterName = "@outerRegistrationNo ", Value =model.outerRegistrationNo??(object)DBNull.Value},
                        new SqlParameter { ParameterName = "@outerCertificateNo ", Value =model.outerCertificateNo??(object)DBNull.Value},
                        new SqlParameter { ParameterName = "@establishmentPlaceOther", Value =model.establishmentPlaceOther??(object)DBNull.Value},
                         new SqlParameter { ParameterName = "@ElectrycityBill", Value =model.ElectrycityBillPath??string.Empty},
                              new SqlParameter { ParameterName = "@Registry ", Value =model.RegistryPath??string.Empty},
                                new SqlParameter { ParameterName = "@RentalAgreement ", Value =model.RentalAgreementPath??string.Empty}



                        
                };
            var _proc = @"proc_NUH_Registration_CMO  @regisIdNUH, @regByUser, @applicantName , @applicantAddress , @applicantStateId, @applicantDistrictId,  @applicantPincode, 
    @qualification , @applicantMobileNo ,@applicantEmailId , @registrationNumber, @regBytransIp , @establishmentName , @medicalEstablishmentId ,@medicalEstablishmentOther,@medicalFacilities,@operatedId , @operatedName ,
@stateId,@districtid,@pinCode,
    @isFirefightingSystem ,@isNOC , @step , @xmldataParmedical, @xmldatadoctor, @xmldatacheckList,@stepValue,@institution,@Central_StateCouncilName,
@establishmentArea,@establishmentPlace,@landType,@telephoneNo,@website,@address,@nocCertificationNo,@isDispose,@disposedNo,@XmlDataOwner,
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
      @xmldataImaging,@numberofBed,@clinicalEstablishmentTypeOther,
      @isRenewal,@isCertificateFromPortal,@outerRegistrationNo,@outerCertificateNo,@establishmentPlaceOther,@ElectrycityBill,@Registry,@RentalAgreement";


            var res = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return res;
        }



        //      @institution NVARCHAR(50),
        //@Central_StateCouncilName NVARCHAR(50)

        public List<NUHmodel> getNUHCompleteRegistration(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_getNUHComplete  @userId";
            var res = this.Database.SqlQuery<NUHmodel>(_proc, sqlparams).ToList();
            return res;
        }


        public List<NUHmodel> getNUHInCompleteRegistration(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_getNUH_InComplete  @userId";
            var res = this.Database.SqlQuery<NUHmodel>(_proc, sqlparams).ToList();
            return res;
        }
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
        //param
        public List<NUHPartnerModel> getNUHOwnerList(string regisIdNUH)
        {
            var sqlparams = new SqlParameter[]{
             new SqlParameter {ParameterName="@regisIdNUH",Value=regisIdNUH}
              };
            var _proc = @"proc_getOWNERLIST  @regisIdNUH";
            var res = this.Database.SqlQuery<NUHPartnerModel>(_proc, sqlparams).ToList();
            return res;
        }

        #endregion
        #region FAP
        public ResultSet CheckEmailMobileExistenceFAP(string checkValue, string Type, long regisId = 0)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@regisId", Value = regisId }

                };

            var sqlQuery = @"proc_checkEmailMobleExistenceFAP_CMO @checkValue,@Type,@regisId";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        public FAPModel getFAPStep(long regisId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdFAP",Value=regisId}
              };
            var _proc = @"Get_FAP_CMO  @regisIdFAP";
            var res = this.Database.SqlQuery<FAPModel>(_proc, sqlparams).FirstOrDefault();
            return res;
        }
        public ResultSet FAPInsertUpdate(long regisIdFAP, long regisByuser, string sterilizedName, int? sterilizedAge, string fatherName, string spouseName, int? spouseAge,
            string claimantMobileNo, string sterilizedGender, string sterilizedAddress, int stateId, int sterlizedDistrictId, string heathUnitName, string heathUnitAddress, string doctorName,
            string admittedDate, string operationDate, int operationTypeId, string dateofReleased, int healthunitDistrictId, string IpAddress, string XmlData, string AppRequestKey, int step,
            string complicationsDetails, string informationDate, string confirmationDate, string sevakendraName, string sevadoctorName, int compensationCategoryId,
            string dateofDeath, decimal? claimAmount, string claimantName, int? claimantAge, string claimantAddress, string claimantAadhaarNo, int relationId,
            string accountHolderName, string bankName, string branchName, string accountNo, string ifscCode, string xmldatacheckList, int UpdateStep)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdFAP",Value=regisIdFAP},  
                          new SqlParameter {ParameterName="@regisByuser",Value=regisByuser},  
                          new SqlParameter {ParameterName="@sterilizedName",Value=(sterilizedName==null)?"":sterilizedName},  
                          new SqlParameter {ParameterName="@sterilizedAge",Value=(sterilizedAge==null)?0:sterilizedAge},  
                          new SqlParameter {ParameterName="@fatherName",Value=(fatherName==null)?"":fatherName},  
                          new SqlParameter {ParameterName="@spouseName",Value=(spouseName==null)?"":spouseName},  
                          new SqlParameter {ParameterName="@spouseAge",Value=(spouseAge==null)?0:spouseAge}, 
                          new SqlParameter {ParameterName="@claimantMobileNo",Value=(claimantMobileNo==null)?"":claimantMobileNo},  
                          new SqlParameter {ParameterName="@sterilizedGender",Value=(sterilizedGender==null)?"":sterilizedGender},  
                          new SqlParameter {ParameterName="@sterilizedAddress",Value=(sterilizedAddress==null)?"":sterilizedAddress},  
                          new SqlParameter {ParameterName="@stateId",Value=(stateId==null)?0:stateId},
                          new SqlParameter {ParameterName="@sterlizedDistrictId",Value=(sterlizedDistrictId==null)?0:sterlizedDistrictId}, 
                          new SqlParameter {ParameterName="@heathUnitName",Value=(heathUnitName==null)?"":heathUnitName}, 
                          new SqlParameter {ParameterName="@heathUnitAddress",Value=(heathUnitAddress==null)?"":heathUnitAddress}, 
                          new SqlParameter {ParameterName="@doctorName",Value=(doctorName==null)?"":doctorName}, 
                          new SqlParameter {ParameterName="@admittedDate",Value=admittedDate??(object)DBNull.Value}, 
                          new SqlParameter {ParameterName="@operationDate",Value=operationDate??(object)DBNull.Value}, 
                          new SqlParameter {ParameterName="@operationTypeId",Value=(operationTypeId==null)?0:operationTypeId}, 
                          new SqlParameter {ParameterName="@dateofReleased",Value=dateofReleased??(object)DBNull.Value},
                          new SqlParameter {ParameterName="@healthunitDistrictId",Value=(healthunitDistrictId==null)?0:healthunitDistrictId}, 
                          new SqlParameter {ParameterName="@transIp",Value=IpAddress},
                          new SqlParameter {ParameterName="@xmlData",Value=(XmlData==null)?"":XmlData}, 
                         // new SqlParameter {ParameterName="@AppRequestKey",Value=AppRequestKey},
                          new SqlParameter {ParameterName="@step",Value=step},


                          new SqlParameter {ParameterName="@complicationsDetails",Value=(complicationsDetails==null)?"":complicationsDetails},  
                          new SqlParameter {ParameterName="@informationDate",Value=informationDate??(object)DBNull.Value}, 
                          new SqlParameter {ParameterName="@confirmationDate",Value=confirmationDate??(object)DBNull.Value}, 
                          new SqlParameter {ParameterName="@sevakendraName",Value=(sevakendraName==null)?"":sevakendraName},  
                          new SqlParameter {ParameterName="@sevadoctorName",Value=(sevadoctorName==null)?"":sevadoctorName},  
                          new SqlParameter {ParameterName="@compensationCategoryId",Value=(compensationCategoryId==null)?0:compensationCategoryId},  
                          new SqlParameter {ParameterName="@dateofDeath",Value=dateofDeath??(object)DBNull.Value},
                          new SqlParameter {ParameterName="@claimAmount",Value=claimAmount??(object)DBNull.Value},
                          new SqlParameter {ParameterName="@claimantName",Value=(claimantName==null)?"":claimantName},  
                          new SqlParameter {ParameterName="@claimantAge",Value=(claimantAge==null)?0:claimantAge},  
                          new SqlParameter {ParameterName="@claimantAddress",Value=(claimantAddress==null)?"":claimantAddress},  
                          new SqlParameter {ParameterName="@claimantAadhaarNo",Value=(claimantAadhaarNo==null)?"":claimantAadhaarNo},
                          new SqlParameter {ParameterName="@relationId",Value=(relationId==null)?0:relationId},  
                          new SqlParameter {ParameterName="@accountHolderName",Value=(accountHolderName==null)?"":accountHolderName},  
                          new SqlParameter {ParameterName="@bankName",Value=(bankName==null)?"":bankName},  
                          new SqlParameter {ParameterName="@branchName",Value=(branchName==null)?"":branchName},  
                          new SqlParameter {ParameterName="@accountNo",Value=(accountNo==null)?"":accountNo},  
                          new SqlParameter {ParameterName="@ifscCode",Value=(ifscCode==null)?"":ifscCode},

                          new SqlParameter{ParameterName="@xmldatacheckList",Value=(xmldatacheckList==null)?"":xmldatacheckList},
                          
                          new SqlParameter{ParameterName="@UpdateStep",Value=UpdateStep}

                };
            var _proc = @"proc_FAP_Registration_CMO @regisIdFAP,@regisByuser,@sterilizedName,@sterilizedAge ,@fatherName  ,@spouseName ,@spouseAge  ,
@claimantMobileNo ,@sterilizedGender ,@sterilizedAddress,@stateId,@sterlizedDistrictId  ,@heathUnitName  ,@heathUnitAddress  ,@doctorName  ,
@admittedDate  ,@operationDate  ,@operationTypeId ,@dateofReleased ,@healthunitDistrictId,@transIp, @xmlData ,@step,
@complicationsDetails,@informationDate,@confirmationDate,@sevakendraName,@sevadoctorName ,@compensationCategoryId ,
@dateofDeath ,@claimAmount,@claimantName ,@claimantAge  ,@claimantAddress ,@claimantAadhaarNo,@relationId  ,
@accountHolderName,@bankName,@branchName,@accountNo ,@ifscCode,@xmldatacheckList,@UpdateStep";

            var res = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return res;
        }

        public List<FAPModel> getFAPCompleteRegistration(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_getFAPComplete  @userId";
            var res = this.Database.SqlQuery<FAPModel>(_proc, sqlparams).ToList();
            return res;
        }

        public List<FAPModel> getFAPCompleteRegistrationbyAppNo(long userId, string ApplicationNo)
        {
            var sqlparams = new SqlParameter[]{
                          new SqlParameter {ParameterName="@userId",Value=userId},
                           new SqlParameter {ParameterName="@applicationNo",Value=ApplicationNo}
              };
            var _proc = @"proc_getFAPCompleteByApplicationNo  @userId,@applicationNo";
            var res = this.Database.SqlQuery<FAPModel>(_proc, sqlparams).ToList();
            return res;
        }

        public List<FAPModel> getFAPInCompleteRegistration(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_getFAP_InComplete  @userId";
            var res = this.Database.SqlQuery<FAPModel>(_proc, sqlparams).ToList();
            return res;
        }
        public List<FAPModel> getFAPdoctorList(long regisId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisId",Value=regisId}
              };
            var _proc = @"proc_getFAPChild @regisId";
            var res = this.Database.SqlQuery<FAPModel>(_proc, sqlparams).ToList();
            return res;
        }

        #endregion

        #region AGC
        #region check email existence
        public ResultSet CheckEmailMobileExistenceAGC(string checkValue, string Type, long regisId = 0, string idNumber = "")
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@regisId", Value = regisId },
                    new SqlParameter { ParameterName = "@idNumber", Value = idNumber } 
                };

            var sqlQuery = @"proc_checkEmailMobleExistenceAGC_CMO @checkValue,@Type,@regisId,@idNumber";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion
        public AGCModel getAGCStep(long regisId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdAGC",Value=regisId}
              };
            var _proc = @"Get_AGC_CMO  @regisIdAGC";
            var res = this.Database.SqlQuery<AGCModel>(_proc, sqlparams).FirstOrDefault();
            return res;
        }

        public ResultSet AGCInsertUpdate(long regisIdAGC, long regisByuser, int applicantTypeId, int applicantSubTypeId, string applicantSubTypeOther, string orderDetail, string orderNo, string orderDate,
                    string fullName, string gender, string idNumber, int idTypeId, string mobileNo, string emailId, string address, int stateId, int districtId, string pinCode,
                    string susName, string susFatherName, string susMotherName, int? susFatherAge, int? susMotherAge, string susMobileNo, string susEmail, string appointmentDate,
                    string susaddress, int susstateId, int susdistrictId, string suspinCode, string markOfIdentification, string IpAddress, int step, int UpdateStep, string xmldatacheckList)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdAGC",Value=regisIdAGC},  
                          new SqlParameter {ParameterName="@regByUser",Value=regisByuser},  
                          new SqlParameter {ParameterName="@applicantTypeId",Value=(applicantTypeId==null)?0:applicantTypeId},  
                          new SqlParameter {ParameterName="@applicantSubTypeId",Value=(applicantSubTypeId==null)?0:applicantSubTypeId},  
                          new SqlParameter {ParameterName="@applicantSubTypeOther",Value=(applicantSubTypeOther==null)?"":applicantSubTypeOther},  
                          new SqlParameter {ParameterName="@orderDetail",Value=(orderDetail==null)?"":orderDetail},  
                          new SqlParameter {ParameterName="@orderNo",Value=(orderNo==null)?"":orderNo},  
                          new SqlParameter {ParameterName="@orderDate",Value=orderDate??(object)DBNull.Value},  
                          //new SqlParameter {ParameterName="@orderFilePath",Value=(orderFilePath==null)?"":orderFilePath},  
                          new SqlParameter {ParameterName="@fullName",Value=(fullName==null)?"":fullName},  
                          new SqlParameter {ParameterName="@gender",Value=(gender==null)?"":gender},
                          new SqlParameter {ParameterName="@idNumber",Value=(idNumber==null)?"":idNumber}, 
                          new SqlParameter {ParameterName="@idTypeId",Value=(idTypeId==null)?0:idTypeId}, 
                          new SqlParameter {ParameterName="@mobileNo",Value=(mobileNo==null)?"":mobileNo}, 
                          new SqlParameter {ParameterName="@emailId",Value=(emailId==null)?"":emailId}, 
                          new SqlParameter {ParameterName="@address",Value=(address==null)?"":address}, 
                          new SqlParameter {ParameterName="@stateId",Value=(stateId==null)?0:stateId}, 
                          new SqlParameter {ParameterName="@districtId",Value=(districtId==null)?0:districtId}, 
                          new SqlParameter {ParameterName="@pinCode",Value=(pinCode==null)?"":pinCode}, 
                          //new SqlParameter {ParameterName="@documentFilePath",Value=(documentFilePath==null)?"":documentFilePath}, 
                          new SqlParameter {ParameterName="@susName",Value=(susName==null)?"":susName},  
                          new SqlParameter {ParameterName="@susFatherName",Value=(susFatherName==null)?"":susFatherName},  
                          new SqlParameter {ParameterName="@susMotherName",Value=(susMotherName==null)?"":susMotherName},  
                          new SqlParameter {ParameterName="@susFatherAge",Value=susFatherAge??(object)DBNull.Value},
                          new SqlParameter {ParameterName="@susMotherAge",Value=susMotherAge??(object)DBNull.Value},
                          new SqlParameter {ParameterName="@susMobileNo",Value=(susMobileNo==null)?"":susMobileNo},  
                          new SqlParameter {ParameterName="@susEmail",Value=(susEmail==null)?"":susEmail},  
                          new SqlParameter {ParameterName="@appointmentDate",Value=appointmentDate??(object)DBNull.Value},  
                          new SqlParameter {ParameterName="@susaddress",Value=(susaddress==null)?"":susaddress},  
                          new SqlParameter {ParameterName="@susstateId",Value=(susstateId==null)?0:susstateId},  
                          new SqlParameter {ParameterName="@susdistrictId",Value=(susdistrictId==null)?0:susdistrictId},  
                          new SqlParameter {ParameterName="@suspinCode",Value=(suspinCode==null)?"":suspinCode},
                          new SqlParameter {ParameterName="@markOfIdentification",Value=(markOfIdentification==null)?"":markOfIdentification},

                          new SqlParameter {ParameterName="@step",Value=step},
                          new SqlParameter {ParameterName="@UpdateStep",Value=UpdateStep},
                           new SqlParameter {ParameterName="@IpAddress",Value=IpAddress},
                           new SqlParameter {ParameterName="@xmldatacheckList",Value=(xmldatacheckList==null)?"":xmldatacheckList}, 
                          
                        //  new SqlParameter{ParameterName="@UpdateStep",Value=UpdateStep}

                };
            var _proc = @"proc_AGC_Registration_CMO @regisIdAGC,@regByUser, @applicantTypeId, @applicantSubTypeId, @applicantSubTypeOther,@orderDetail,@orderNo, @orderDate, 
                    @fullName, @gender, @idNumber, @idTypeId, @mobileNo, @emailId, @address, @stateId, @districtId, @pinCode,
                    @susName, @susFatherName, @susMotherName,@susFatherAge, @susMotherAge, @susMobileNo, @susEmail, @appointmentDate,
                    @susaddress,@susstateId,@susdistrictId,@suspinCode,@markOfIdentification, @IpAddress ,@step,@UpdateStep,@xmldatacheckList";

            var res = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return res;
        }
        public List<AGCModel> getAGCCompleteRegistration(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_getAGCComplete  @userId";
            var res = this.Database.SqlQuery<AGCModel>(_proc, sqlparams).ToList();
            return res;
        }
        public List<AGCModel> getAGCInCompleteRegistration(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_getAGC_InComplete  @userId";
            var res = this.Database.SqlQuery<AGCModel>(_proc, sqlparams).ToList();
            return res;
        }
        #endregion

        #region DIC
        #region check email existence
        public ResultSet CheckEmailMobileExistenceDIC(string checkValue, string Type, long regisId)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@regisId", Value = regisId }

                };

            var sqlQuery = @"proc_checkEmailMobleExistenceDIC @checkValue,@Type,@regisId";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion
        public DICModel getDICStep(long regisId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdDIC",Value=regisId}
              };
            var _proc = @"Get_DIC_CMO  @regisIdDIC";
            var res = this.Database.SqlQuery<DICModel>(_proc, sqlparams).FirstOrDefault();
            return res;
        }

        public ResultSet DICInsertUpdate(DICModel model)
        {
            var sqlparams = new SqlParameter[]{

                    new  SqlParameter{ParameterName="@regisIdDIC",Value=model.regisIdDIC},
                    new  SqlParameter{ParameterName="@regByUser",Value=model.regByUser},
                    new  SqlParameter{ParameterName="@ApplyingFor", Value =model.ApplyingFor??string.Empty} ,
                    new  SqlParameter{ParameterName="@isCertFromPortal", Value=model.isCertFromPortal??string.Empty},//
	               // new  SqlParameter{ParameterName="@releventProof", Value =model.releventProofpath??string.Empty} ,
                    new  SqlParameter{ParameterName="@oldCertificateNumber", Value=model.oldCertificateNumber??string.Empty},//
	                new  SqlParameter{ParameterName="@fullName", Value =model.fullName??string.Empty} ,
	                new  SqlParameter{ParameterName="@fatherName", Value =model.fatherName??string.Empty} ,
	                new  SqlParameter{ParameterName="@dob", Value =model.dob??string.Empty} ,
                    new  SqlParameter{ParameterName="@age",Value=model.age??(object)DBNull.Value},//
	                new  SqlParameter{ParameterName="@gender", Value =model.gender??string.Empty} ,
	                new  SqlParameter{ParameterName="@categoryId", Value =model.categoryId} ,
	                new  SqlParameter{ParameterName="@mobileNo", Value =model.mobileNo??string.Empty} ,
	                new  SqlParameter{ParameterName="@emailId", Value =model.emailId??string.Empty} ,
                    new  SqlParameter{ParameterName="@adharNumber", Value =model.adharNumber??string.Empty},
	                new  SqlParameter{ParameterName="@stateId", Value =model.stateId} ,
	                new  SqlParameter{ParameterName="@districtId", Value =model.districtId} ,
	                new  SqlParameter{ParameterName="@address ", Value =model.address??string.Empty} ,
	                new  SqlParameter{ParameterName="@pinCode ", Value =model.pinCode??string.Empty} ,
                    new  SqlParameter{ParameterName="@disabilityTypeId ", Value =model.disabilityTypeId},
	                new  SqlParameter{ParameterName="@disabilityType ", Value =model.disabilityType??string.Empty} ,
	                new  SqlParameter{ParameterName="@disabilityDetail ", Value =model.disabilityDetail??string.Empty} ,
	                //new  SqlParameter{ParameterName="@photoPath ", Value =model.photoPathpath??string.Empty} ,
                    //new  SqlParameter{ParameterName="@passportsizephoto", Value =model.passportsizephotopath??string.Empty} ,
                    new  SqlParameter{ParameterName="@photoIdNo ", Value =model.photoIdNo??string.Empty},
                    new  SqlParameter{ParameterName="@idProofId", Value =model.idProofId},
	                //new  SqlParameter{ParameterName="@idProofPath ", Value =model.idProofPathpath??string.Empty} ,
                    new  SqlParameter{ParameterName="@addressProofId", Value =model.addressProofId},
	                //new  SqlParameter{ParameterName="@documentPath", Value =model.documentPath??string.Empty} ,
                    //new  SqlParameter{ParameterName="@thumbImpPath", Value =model.thumbImpPathpath??string.Empty} ,
                    new SqlParameter{ParameterName="@xmldatacheckList", Value=model.xmldatacheckList??string.Empty},
	                new  SqlParameter{ParameterName="@transIp", Value =model.transIp},
                    new  SqlParameter{ParameterName="@step", Value =model.step},
                    new  SqlParameter{ParameterName="@stepValue", Value=model.stepValue}

                };
            var _proc = @"proc_DIC_Registration_CMO @regisIdDIC,@regByUser,@ApplyingFor,@isCertFromPortal,@oldCertificateNumber,
@fullName,@fatherName,@dob,@age,@gender,@categoryId,@mobileNo,@emailId,@adharNumber,@stateId,@districtId,@address ,@pinCode ,@disabilityTypeId,
@disabilityType ,@disabilityDetail ,@photoIdNo,@idProofId,@addressProofId,@xmldatacheckList,@transIp,@step,@stepValue";

            var res = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return res;
        }

        public ResultSet DICInsertUpdateRenewal(DICModel model)
        {
            var sqlparams = new SqlParameter[]{

                    new  SqlParameter{ParameterName="@regisIdDIC",Value=model.regisIdDIC},
                    new  SqlParameter{ParameterName="@regByUser",Value=model.regByUser},
                    new  SqlParameter{ParameterName="@ApplyingFor", Value =model.ApplyingFor??string.Empty} ,
                    new  SqlParameter{ParameterName="@isCertFromPortal", Value=model.isCertFromPortal??string.Empty},
                    new  SqlParameter{ParameterName="@oldCertificateNumber", Value=model.oldCertificateNumber??string.Empty},
                    //new  SqlParameter{ParameterName="@fullName", Value =model.fullName??string.Empty} ,
                    //new  SqlParameter{ParameterName="@fatherName", Value =model.fatherName??string.Empty} ,
                    //new  SqlParameter{ParameterName="@dob", Value =model.dob??string.Empty} ,
                   new  SqlParameter{ParameterName="@age",Value=model.age??(object)DBNull.Value},
                    //new  SqlParameter{ParameterName="@gender", Value =model.gender??string.Empty} ,
                    //new  SqlParameter{ParameterName="@categoryId", Value =model.categoryId} ,
                    new  SqlParameter{ParameterName="@mobileNo", Value =model.mobileNo??string.Empty} ,
	                new  SqlParameter{ParameterName="@emailId", Value =model.emailId??string.Empty} ,
                    //new  SqlParameter{ParameterName="@adharNumber", Value =model.adharNumber??string.Empty},
	                new  SqlParameter{ParameterName="@stateId", Value =model.stateId} ,
	                new  SqlParameter{ParameterName="@districtId", Value =model.districtId} ,
	                new  SqlParameter{ParameterName="@address ", Value =model.address??string.Empty} ,
	                new  SqlParameter{ParameterName="@pinCode ", Value =model.pinCode??string.Empty} ,
                    new  SqlParameter{ParameterName="@disabilityTypeId ", Value =model.disabilityTypeId},
	                new  SqlParameter{ParameterName="@disabilityType ", Value =model.disabilityType??string.Empty} ,
	                new  SqlParameter{ParameterName="@disabilityDetail ", Value =model.disabilityDetail??string.Empty} ,
                    new  SqlParameter{ParameterName="@photoIdNo ", Value =model.photoIdNo??string.Empty},
                    new  SqlParameter{ParameterName="@idProofId", Value =model.idProofId},
                    new  SqlParameter{ParameterName="@addressProofId", Value =model.addressProofId},
                    new  SqlParameter{ParameterName="@xmldatacheckList", Value=model.xmldatacheckList??string.Empty},
	                new  SqlParameter{ParameterName="@transIp", Value =model.transIp},
                    new  SqlParameter{ParameterName="@step", Value =model.step},
                    new  SqlParameter{ParameterName="@stepValue", Value=model.stepValue}

                };
            var _proc = @"proc_DIC_RegistrationRenewal_CMO @regisIdDIC,@regByUser,@ApplyingFor,@isCertFromPortal,@oldCertificateNumber,@age,@mobileNo,
@emailId,@stateId,@districtId,@address ,@pinCode ,@disabilityTypeId,
@disabilityType ,@disabilityDetail ,@photoIdNo,@idProofId,@addressProofId,@xmldatacheckList,@transIp,@step,@stepValue";

            var res = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return res;
        }

        public List<DICModel> getDICCompleteRegistration(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_getDICComplete  @userId";
            var res = this.Database.SqlQuery<DICModel>(_proc, sqlparams).ToList();
            return res;
        }
        public List<DICModel> getDICInCompleteRegistration(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_getDIC_InComplete  @userId";
            var res = this.Database.SqlQuery<DICModel>(_proc, sqlparams).ToList();
            return res;
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
        #region MER
        #region check email existence
        public ResultSet CheckEmailMobileExistenceMER(string checkValue, string Type, long regisId = 0)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@regisId", Value = regisId }

                };

            var sqlQuery = @"proc_checkEmailMobleExistenceMER_CMO @checkValue,@Type,@regisId";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion
        public MERModel getMERStep(long regisId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdMER",Value=regisId}
              };
            var _proc = @"proc_getMER_Step  @regisIdMER";
            var res = this.Database.SqlQuery<MERModel>(_proc, sqlparams).FirstOrDefault();
            return res;
        }

        public ResultSet Insert_UpdateMER(MERModel model, string ROLL)
        {

            var sqlparams = new SqlParameter[] {
            new SqlParameter{ParameterName="@regisIdMER",Value=model.regisIdMER},
            new SqlParameter{ParameterName ="@step",Value=model.step}, 
            new SqlParameter{ParameterName ="@stepValue",Value=model.stepValue}, 
            new SqlParameter{ParameterName="@regisByuser",Value=model.regisByuser},
            new SqlParameter{ParameterName="@treatmentId",Value=model.treatmentId},
            new SqlParameter{ParameterName="@empfullName",Value=model.empfullName??string.Empty},
            new SqlParameter{ParameterName="@designation",Value=model.designation??string.Empty},
            new SqlParameter{ParameterName="@manavSampda_AadharNo",Value=model.manavSampda_AadharNo??string.Empty},
            new SqlParameter{ParameterName="@isRetirement",Value=model.isRetirement},
            new SqlParameter{ParameterName="@ppotreament",Value=model.ppotreament??string.Empty},
            new SqlParameter{ParameterName="@father_HusbandName",Value=model.father_HusbandName??string.Empty},
            new SqlParameter{ParameterName="@officeName",Value=model.officeName??(object)DBNull.Value},
            new SqlParameter{ParameterName="@deptName",Value=model.deptName??string.Empty},
            new SqlParameter{ParameterName="@basicSalary",Value=model.basicSalary},
            new SqlParameter{ParameterName="@dob",Value=model.dob??string.Empty},
            new SqlParameter{ParameterName="@gender",Value=model.gender??string.Empty},
            new SqlParameter{ParameterName="@mobileNo",Value=model.mobileNo??string.Empty},
            new SqlParameter{ParameterName="@postingAddress",Value=model.postingAddress??string.Empty},
            new SqlParameter{ParameterName="@postingDistrictId",Value=model.postingDistrictId},
            new SqlParameter{ParameterName="@postingPinCode",Value=model.postingPinCode??string.Empty},
            new SqlParameter{ParameterName="@postingStateId",Value=model.postingStateId},
            new SqlParameter{ParameterName="@permAddress",Value=model.permAddress??string.Empty},
            new SqlParameter{ParameterName="@permDistrictId",Value=model.permDistrictId},
            new SqlParameter{ParameterName="@permPinCode",Value=model.permPinCode??string.Empty},
            new SqlParameter{ParameterName="@permStateId",Value=model.permStateId},
            new SqlParameter{ParameterName="@patientType",Value=model.patientType??string.Empty},
            new SqlParameter{ParameterName="@patientName",Value=model.patientName??string.Empty},
            new SqlParameter{ParameterName="@patientage",Value=model.patientage},
            new SqlParameter{ParameterName="@patientgender",Value=model.patientgender??string.Empty},
            new SqlParameter{ParameterName="@patientrelationsWithEmployee",Value=model.patientrelationsWithEmployee??string.Empty},
            new SqlParameter{ParameterName="@patientAadhaarNo",Value=model.patientAadhaarNo??(object)DBNull.Value},
            new SqlParameter{ParameterName="@patientdiseaseName",Value=model.patientdiseaseName??string.Empty},
            new SqlParameter{ParameterName="@patienttreatmentFromDate",Value=model.patienttreatmentFromDate??string.Empty},
            new SqlParameter{ParameterName="@patienttreatmentToDate",Value=model.patienttreatmentToDate??string.Empty},
            new SqlParameter{ParameterName="@patientplaceOfDisease",Value=model.patientplaceOfDisease??string.Empty},
            new SqlParameter{ParameterName="@patienthospitalName",Value=model.patienthospitalName??string.Empty},
            new SqlParameter{ParameterName="@patientdoctorName",Value=model.patientdoctorName??string.Empty},
            new SqlParameter{ParameterName="@bankName",Value=model.bankName??string.Empty},
            new SqlParameter{ParameterName="@branchName",Value=model.branchName??string.Empty},
            new SqlParameter{ParameterName="@accountNo",Value=model.accountNo??string.Empty},
            new SqlParameter{ParameterName="@ifscCode",Value=model.ifscCode??string.Empty},
            new SqlParameter{ParameterName="@xmldata",Value=model.xmlData??string.Empty},           
            new SqlParameter{ParameterName="@transIp",Value=model.transIP??string.Empty},
            new SqlParameter { ParameterName = "@officeInchargeName", Value =model.officeInchargeName??(object)DBNull.Value},
            new SqlParameter { ParameterName = "@hospitalType", Value =model.hospitalType??string.Empty},           
            new SqlParameter { ParameterName = "@isAdvanceTaken", Value =model.isAdvanceTaken??string.Empty},           
            new SqlParameter { ParameterName = "@advanceLetterNo", Value =model.advanceLetterNo??string.Empty},
            new SqlParameter { ParameterName = "@advanceDate", Value =model.advanceDate??string.Empty},
             new SqlParameter { ParameterName = "@advanceAmount", Value =model.advanceAmount??(object)DBNull.Value},
              new SqlParameter { ParameterName = "@ROLL", Value =ROLL}
            };

            var _proc = @"proc_MER_Registration_CMO  @regisIdMER, @step,@stepValue, @regisByuser , @treatmentId , @empfullName, @designation, @manavSampda_AadharNo,@isRetirement, @ppotreament, @father_HusbandName, @officeName, @deptName,@basicSalary ,@dob , @gender, @mobileNo, @postingAddress , @postingDistrictId , @postingPinCode , @postingStateId ,@permAddress , @permDistrictId, @permPinCode , @permStateId, @patientType , @patientName, @patientage , @patientgender, @patientrelationsWithEmployee , @patientAadhaarNo, @patientdiseaseName , @patienttreatmentFromDate, @patienttreatmentToDate , @patientplaceOfDisease , @patienthospitalName , @patientdoctorName, @bankName, @branchName, @accountNo, @ifscCode,@xmldata,@transIp, @officeInchargeName ,
    @hospitalType, @isAdvanceTaken , @advanceLetterNo, @advanceDate, @advanceAmount,@ROLL";
            var res = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return res;
        }

        public List<MERModel> getMERChild(long regisId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisId",Value=regisId}
              };
            var _proc = @"proc_getMERChild_CMO  @regisId";
            var res = this.Database.SqlQuery<MERModel>(_proc, sqlparams).ToList();
            return res;
        }


        public List<MERModel> getMERCompleteRegistration(long userId, string ROLL)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId},
                           new SqlParameter {ParameterName="@ROLL",Value=ROLL}
              };
            var _proc = @"proc_getMERComplete  @userId,@ROLL";
            var res = this.Database.SqlQuery<MERModel>(_proc, sqlparams).ToList();
            return res;
        }
        public List<MERModel> getMERInCompleteRegistration(long userId, string ROLL)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId},
                          new SqlParameter {ParameterName="@ROLL",Value=ROLL}
              };
            var _proc = @"proc_getMER_InComplete  @userId,@ROLL";
            var res = this.Database.SqlQuery<MERModel>(_proc, sqlparams).ToList();
            return res;
        }
        #endregion
        #region ILC
        #region check email existence
        public ResultSet CheckEmailMobileExistenceILC(string checkValue, string Type, long regisId)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@RegisId", Value = regisId }

                };

            var sqlQuery = @"proc_checkEmailMobleExistenceILC @checkValue,@Type,@RegisId";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion

        public ILCModel getILCStep(long regisId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdILC",Value=regisId}
              };
            var _proc = @"proc_getILC_Step  @regisIdILC";
            var res = this.Database.SqlQuery<ILCModel>(_proc, sqlparams).FirstOrDefault();
            return res;
        }
        public ILCModel getILCForExtendDateCHC(long regisId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdILC",Value=regisId}
              };
            var _proc = @"proc_getILCForExtendDateCHC  @regisIdILC";
            var res = this.Database.SqlQuery<ILCModel>(_proc, sqlparams).FirstOrDefault();
            return res;
        }
        public List<ILCModel> rblforwardType()
        {
            var _proc = @"proc_IMC_forwardType";
            var slist = this.Database.SqlQuery<ILCModel>(_proc).ToList();
            return slist;
        }


        public ResultSet Insert_UpdateILC(ILCModel model)
        {
            var sqlparams = new SqlParameter[] {
            new SqlParameter{ParameterName="@step",Value=model.step},
            new SqlParameter{ParameterName="@stepValue",Value=model.stepValue},
            new SqlParameter{ParameterName="@regisId",Value=model.regisIdILC},
            new SqlParameter{ParameterName="@opdName",Value=model.opdName??string.Empty},
            new SqlParameter{ParameterName="@opdReceiptno",Value=model.opdReceiptno??string.Empty},
            new SqlParameter{ParameterName="@opdDate",Value=model.opdDate??string.Empty},
            new SqlParameter{ParameterName="@opdDistrictId",Value=model.opdDistrictId},
            new SqlParameter{ParameterName="@opdAddress",Value=model.opdAddress??string.Empty},
            new SqlParameter{ParameterName="@opdPincode",Value=model.opdPincode??string.Empty},
            new SqlParameter{ParameterName="@opdStateId",Value=model.opdStateId},
            new SqlParameter{ParameterName="@opdFilePath",Value=model.opdFilePath??string.Empty},
            new SqlParameter{ParameterName="@fullName",Value=model.fullName??string.Empty},
            new SqlParameter{ParameterName="@fatherName",Value=model.fatherName??string.Empty},
            new SqlParameter{ParameterName="@dob",Value=model.dob??string.Empty},
            new SqlParameter{ParameterName="@gender",Value=model.gender??string.Empty},
            new SqlParameter{ParameterName="@categoryId",Value=model.categoryId},
            new SqlParameter{ParameterName="@mobileNo",Value=model.appmobileNo??string.Empty},
            new SqlParameter{ParameterName="@emailId",Value=model.emailId??string.Empty},
            new SqlParameter{ParameterName="@forwardType",Value=model.forwardtypeId},
            new SqlParameter{ParameterName="@forwardtoId",Value=model.forwardtoId},
            new SqlParameter{ParameterName="@doctorName",Value=model.doctorName??string.Empty},
            new SqlParameter{ParameterName="@reason",Value=model.reason??string.Empty},           
            new SqlParameter{ParameterName="@treatmentFrom",Value=model.treatmentFrom??string.Empty},
            new SqlParameter{ParameterName="@treatmentto",Value=model.treatmentto??string.Empty},
            new SqlParameter{ParameterName="@NoOfDays",Value=model.NoOfDays},
            new SqlParameter{ParameterName="@remarks",Value=model.remarks??string.Empty},
            new SqlParameter{ParameterName="@transIP",Value=model.transIp??string.Empty},
            new SqlParameter{ParameterName="@illnessDetail",Value=model.illnessDetail??string.Empty},
            new SqlParameter{ParameterName="@healthUnitDistrictId",Value=model.healthUnitDistrictId},
            new SqlParameter{ParameterName="@regbyuser",Value=model.regByUser},
            new SqlParameter{ParameterName="@XmlCheckList",Value=model.XmlCheckList??string.Empty},
            new SqlParameter{ParameterName="@markOfIdentification",Value=model.markOfIdentification??string.Empty},
            new SqlParameter{ParameterName="@idTypeId",Value=model.idTypeId},
            new SqlParameter{ParameterName="@idNumber",Value=model.idNumber??string.Empty},

            
            };
            var _proc = @"proc_ILC_Registration_CHC @step,@stepValue, @regisId , @opdName , @opdReceiptno , @opdDate , @opdDistrictId, @opdAddress, @opdPincode ,@opdStateId  ,
    @opdFilePath , @fullName ,@fatherName, @dob, @gender,@categoryId ,@mobileNo , @emailId, @forwardType, @forwardtoId , @doctorName, @reason, @treatmentFrom , @treatmentto ,@NoOfDays,
    @remarks, @transIP, @illnessDetail, @healthUnitDistrictId,@regbyuser,@XmlCheckList,@markOfIdentification,@idTypeId,@idNumber";
            var res = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return res;
        }

        public ResultSet Insert_UpdateExtDateILC(long regisIdILC, long regByUser, long forwardType, long forwardtoId, int healthUnitDistrictId,
string reason, string idNumber, string extOpdReceiptno, string extInspectedDate, string extOpdFile, string extDoctorName, string extTreatmentFrom, string extTreatmentto, int extNoOfDays,
string transIp, int Extstep, string mobileNo)
        {
            var sqlparams = new SqlParameter[] {
            
            new SqlParameter{ParameterName="@regisIdILC",Value=regisIdILC},
            new SqlParameter{ParameterName="@regByUser",Value=regByUser},
            new SqlParameter{ParameterName="@forwardtype",Value=forwardType},
            new SqlParameter{ParameterName="@forwardtoId",Value=forwardtoId},
            
            new SqlParameter{ParameterName="@healthUnitDistrictId",Value=healthUnitDistrictId},
           // new SqlParameter{ParameterName="@idNumber",Value=idNumber??string.Empty},
           new SqlParameter{ParameterName="@reason",Value=reason??string.Empty},
            new SqlParameter{ParameterName="@extOpdReceiptno",Value=extOpdReceiptno??string.Empty},
            new SqlParameter{ParameterName="@extInspectedDate",Value=extInspectedDate??string.Empty},
            new SqlParameter{ParameterName="@extOpdFile",Value=extOpdFile??string.Empty},
            new SqlParameter{ParameterName="@extDoctorName",Value=extDoctorName??string.Empty},
            new SqlParameter{ParameterName="@extTreatmentFrom",Value=extTreatmentFrom??string.Empty},
            new SqlParameter{ParameterName="@extTreatmentto",Value=extTreatmentto??string.Empty},
            new SqlParameter{ParameterName="@extNoOfDays",Value=extNoOfDays},
            new SqlParameter{ParameterName="@transIp",Value=transIp??string.Empty},
            new SqlParameter{ParameterName="@Extstep",Value=Extstep},
            new SqlParameter{ParameterName="@mobileNo",Value=mobileNo} 
            
            };
            var _proc = @"InsertUpdateILCextenDayCHC @regisIdILC,@regByUser ,@forwardType  ,@forwardtoId    ,@healthUnitDistrictId  ,@reason,
@extOpdReceiptno ,@extInspectedDate,@extOpdFile,@extDoctorName,@extTreatmentFrom,@extTreatmentto,@extNoOfDays,@transIp,@Extstep,@mobileNo ";
            var res = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return res;
        }
        public List<ILCModel> GetComplete_ILC(long userId, string certificateno)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId},
                          new SqlParameter {ParameterName="@certificateno",Value=certificateno}
              };
            var _proc = @"proc_GetComplete_ILC  @userId,@certificateno";
            var res = this.Database.SqlQuery<ILCModel>(_proc, sqlparams).ToList();
            return res;
        }
        public List<ILCModel> GetInComplete_ILC(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_GetInComplete_ILC  @userId";
            var res = this.Database.SqlQuery<ILCModel>(_proc, sqlparams).ToList();
            return res;
        }

        #region Method Get Check List ILC
        public List<DropDownList> GetCheckList_ILC()
        {
            var _proc = @"proc_GetCheckList_ILC";
            var slist = this.Database.SqlQuery<DropDownList>(_proc).ToList();
            return slist;
        }
        #endregion

        #endregion

        #region FIC
        #region check email existence
        public ResultSet CheckEmailMobileExistenceFIC(string checkValue, string Type, long regisId)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@RegisId", Value = regisId } 
                };

            var sqlQuery = @"proc_checkEmailMobleExistenceFIC @checkValue,@Type,@RegisId";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion

        public FICModel GetRegistration_FIC(long regisId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdFIC",Value=regisId}
              };
            var _proc = @"proc_GetRegistration_FIC  @regisIdFIC";
            var res = this.Database.SqlQuery<FICModel>(_proc, sqlparams).FirstOrDefault();
            return res;
        }

        public ResultSet Insert_UpdateFIC(FICModel model)
        {
            var sqlparams = new SqlParameter[] {
                    new  SqlParameter{ParameterName="@step",Value=model.step},
                    new  SqlParameter{ParameterName="@stepvalue",Value=model.stepValue},
                    new  SqlParameter{ParameterName="@regByUser",Value=model.regByUser},
                    new  SqlParameter{ParameterName="@regisIdFIC",Value=model.regisIdFIC},  
                    new  SqlParameter{ParameterName="@applicationReason",Value=model.applicationReason??string.Empty},
                    new  SqlParameter{ParameterName="@fullName",Value=model.fullName??string.Empty},
                    new  SqlParameter{ParameterName="@fatherName",Value=model.fatherName??string.Empty},
                    new  SqlParameter{ParameterName="@dob",Value=model.dob??string.Empty},
                    new  SqlParameter{ParameterName="@gender",Value=model.gender??string.Empty},
                    new  SqlParameter{ParameterName="@categoryId",Value=model.categoryId},
                    new  SqlParameter{ParameterName="@mobileNo",Value=model.appmobileNo??string.Empty},
                    new  SqlParameter{ParameterName="@emailId",Value=model.emailId??string.Empty},
                     new  SqlParameter{ParameterName="@opdAddress",Value=model.opdAddress??string.Empty},
                     new  SqlParameter{ParameterName="@opdStateId",Value=model.opdStateId},
                    new  SqlParameter{ParameterName="@opdDistrictId",Value=model.opdDistrictId},
                    new  SqlParameter{ParameterName="@opdPinCode",Value=model.opdPinCode??string.Empty},
                    new  SqlParameter { ParameterName = "@markOfIdentification", Value =model.markOfIdentification??string.Empty},


                    //new  SqlParameter{ParameterName="@opdName",Value=model.opdName??string.Empty},
                    new  SqlParameter{ParameterName="@opdRecNo",Value=model.opdRecNo??string.Empty},
                    new  SqlParameter{ParameterName="@opdDate",Value=model.opdDate??string.Empty},
                   
                    new  SqlParameter{ParameterName="@forwardType",Value=model.forwardtypeId},
                    new  SqlParameter{ParameterName="@forwardtoId",Value=model.forwardtoId},
                    new  SqlParameter{ParameterName="@healthUnitDistrictId",Value=model.healthUnitDistrictId},
                    new  SqlParameter{ParameterName="@fitnessAdvicedBy",Value=model.fitnessAdvicedBy??string.Empty},
                    new  SqlParameter{ParameterName="@treatmentFrom",Value=model.treatmentFrom??string.Empty},
                    new  SqlParameter{ParameterName="@treatmentto",Value=model.treatmentto??string.Empty},
                  
                    
                    new  SqlParameter{ParameterName="@transIp",Value=model.transIp},
                    new  SqlParameter{ParameterName="@isMedicalHistory",Value=model.isMedicalHistory??string.Empty},
                    new  SqlParameter{ParameterName="@XmlCheckList",Value=model.XmlCheckList??string.Empty},
                    new  SqlParameter{ParameterName="@detailsMedicalHistory",Value=model.detailsMedicalHistory??string.Empty},
                    new SqlParameter { ParameterName = "@idTypeId", Value =model.idTypeId},
                    new SqlParameter { ParameterName = "@idNumber", Value =(model.idNumber==null)?"":model.idNumber},
            };
            var _proc = @"proc_FIC_Registration_CHC @step,
       @stepvalue , @regByuser, @regisIdFIC, @applicationReason, @fullName , @fatherName , @dob , @gender , @categoryId , @mobileNo ,@emailId ,@opdRecNo , @opdDate , @opdAddress, @opdPinCode , @opdStateId, @opdDistrictId , @forwardType , @forwardtoId ,
    @healthUnitDistrictId, @fitnessAdvicedBy, @treatmentFrom , @treatmentto ,  @transIp, @isMedicalHistory , @XmlCheckList,@markOfIdentification,@detailsMedicalHistory,@idTypeId,@idNumber";
            var res = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return res;
        }
        public ResultSet Insert_UpdateFICextended(FICModel model)
        {
            var sqlparams = new SqlParameter[] {
                    new  SqlParameter{ParameterName="@regisIdFIC",Value=model.regisIdFIC},
                    new  SqlParameter { ParameterName ="@ILCcertificateNo", Value =model.certificateNo!=null ?model.certificateNo:(object)DBNull.Value},
                    new  SqlParameter{ParameterName="@regByUser",Value=model.regByUser},
                    new  SqlParameter{ParameterName="@applicationReason",Value=model.applicationReason??string.Empty},
                    new  SqlParameter{ParameterName="@opdRecNo",Value=model.opdRecNo??string.Empty},
                   // new  SqlParameter{ParameterName="@opdFilePath",Value=model.opdFilePath??string.Empty},
                    new  SqlParameter{ParameterName="@fitnessAdvicedBy",Value=model.fitnessAdvicedBy??string.Empty},
                    new  SqlParameter{ParameterName="@opdDate",Value=model.opdDate??string.Empty},
                    new  SqlParameter{ParameterName="@mobileNo",Value=model.mobileNo??string.Empty},
                    new  SqlParameter{ParameterName="@emailId",Value=model.emailId??string.Empty},
                    new  SqlParameter{ParameterName="@transIp",Value=model.transIp},
                    new  SqlParameter{ParameterName="@step",Value=model.step},
                     new  SqlParameter{ParameterName="@stepValue",Value=model.stepValue},
                     new  SqlParameter{ParameterName="@XmlCheckList",Value=model.XmlCheckList??string.Empty},
                    
            };
            var _proc = @"proc_ILCtoFIC_Registration_CHC  @regisIdFIC,@ILCcertificateNo,@regByUser,@applicationReason,@opdRecNo,
@fitnessAdvicedBy,@opdDate,@mobileNo,@emailId ,@transIp,@step,@stepValue,@XmlCheckList";
            var res = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return res;
        }
        public List<FICModel> GetComplete_FIC(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_GetComplete_FIC  @userId";
            var res = this.Database.SqlQuery<FICModel>(_proc, sqlparams).ToList();
            return res;
        }
        public List<FICModel> GetInComplete_FIC(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_GetInComplete_FIC  @userId";
            var res = this.Database.SqlQuery<FICModel>(_proc, sqlparams).ToList();
            return res;
        }




        #endregion

        #region  IMC
        #region check email existence
        public ResultSet CheckEmailMobileExistenceIMC(string checkValue, string Type, long regisId)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@RegisId", Value = regisId } 
                };

            var sqlQuery = @"proc_checkEmailMobleExistenceIMC @checkValue,@Type,@RegisId";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion

        public ResultSet Insert_UpdateIMC(IMCModel model)
        {

            var sqlparams = new SqlParameter[] {
            

                new SqlParameter{ParameterName="@regisIdIMC",Value=model.regisIdIMC},
                new SqlParameter{ParameterName="@regisByuser",Value=model.regisByuser},
                 new SqlParameter{ParameterName="@isVaccined",Value=true},
                  new SqlParameter{ParameterName="@reason",Value=model.reason??string.Empty},
                   new SqlParameter { ParameterName = "@opdReciept",Value=model.opdReciept??string.Empty},                    
                new SqlParameter{ParameterName="@fullName",Value=model.fullName??string.Empty},
                new SqlParameter{ParameterName="@fatherName",Value=model.fatherName??string.Empty},
                new SqlParameter{ParameterName="@dob",Value=model.dob??string.Empty},
                new SqlParameter{ParameterName="@age",Value=model.age},
                new SqlParameter{ParameterName="@mobileNo",Value=model.mobileNo??string.Empty},
                new SqlParameter{ParameterName="@emailId",Value=model.emailId??string.Empty},
                new SqlParameter{ParameterName="@passportNo",Value=model.passportNo??string.Empty},
                new SqlParameter{ParameterName="@address",Value=model.address??string.Empty},
                new SqlParameter{ParameterName="@stateId",Value=model.stateId},
                new SqlParameter{ParameterName="@districtId",Value=model.districtId},
                new SqlParameter{ParameterName="@pinCode",Value=model.pinCode??string.Empty},
                new SqlParameter { ParameterName = "@markOfIdentification",Value=model.markOfIdentification??string.Empty},
                new SqlParameter{ParameterName="@forwardtoId",Value=model.forwardtoId},
                new SqlParameter{ParameterName="@forwardtypeId",Value=model.forwardtypeId},
                new SqlParameter{ParameterName="@forwardDistrictId",Value=model.forwardDistrictId},

                new SqlParameter{ParameterName="@step",Value=model.step},
                new SqlParameter{ParameterName="@UpdateStep",Value=model.UpdateStep},
                 new SqlParameter{ParameterName="@transIp",Value=model.transIp??string.Empty},
                new SqlParameter{ParameterName="@xmldata",Value=model.xmldata??string.Empty},
                 new SqlParameter{ParameterName="@XmlDataChecklist", Value=model.XmlDataChecklist??string.Empty},
              new SqlParameter{ParameterName="@stepvalue",Value=model.stepValue}
            //new SqlParameter { ParameterName = "@vaccineDate",Value=model.vaccineDate??string.Empty},
            //new SqlParameter { ParameterName = "@vaccineOldName",Value=model.vaccineOldName??string.Empty},
           // new SqlParameter { ParameterName = "@opdFile",Value=model.opdFilePath??string.Empty},
          
              
            
            };
            var _proc = @"proc_IMC_Registration_CHC  @regisIdIMC ,@regisByuser ,@isVaccined ,@reason ,@opdReciept ,@fullName ,@fatherName ,@dob ,@age ,@mobileNo ,@emailId ,
@passportNo ,@address ,@stateId ,@districtId ,@pinCode,@markOfIdentification,@forwardtoId,@forwardtypeId,@forwardDistrictId ,@step,@UpdateStep ,@transIp ,@xmldata,@XmlDataChecklist,@stepvalue ";
            var res = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return res;
        }
        public IMCModel GetRegistration_IMC(long regisId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdFIC",Value=regisId}
              };
            var _proc = @"proc_GetRegistration_IMC  @regisIdFIC";
            var res = this.Database.SqlQuery<IMCModel>(_proc, sqlparams).FirstOrDefault();
            return res;
        }
        public List<IMCModel> GetComplete_IMC(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_GetComplete_IMC  @userId";
            var res = this.Database.SqlQuery<IMCModel>(_proc, sqlparams).ToList();
            return res;
        }
        public List<IMCModel> GetInComplete_IMC(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_GetInComplete_IMC  @userId";
            var res = this.Database.SqlQuery<IMCModel>(_proc, sqlparams).ToList();
            return res;
        }
        public List<IMCModel> getIMCImmuCHC(long regisIdIMC)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdIMC",Value=regisIdIMC}
              };
            var _proc = @"proc_getIMCImmuCHC  @regisIdIMC";
            var res = this.Database.SqlQuery<IMCModel>(_proc, sqlparams).ToList();
            return res;
        }
        //public List<IMCModel> BindImmunizationDetailsIMC()
        //{

        //    var sqlProc = @"Proc_GetImmunizationTypeIMC";
        //    var list = this.Database.SqlQuery<IMCModel>(sqlProc).ToList();
        //    return list;

        //}
        #endregion
        #region MLC
        #region check email existence
        public ResultSet CheckEmailMobileExistenceMLC(string checkValue, string Type, long regisId, string idNo)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@RegisId", Value = regisId },
                    new SqlParameter { ParameterName = "@idNo", Value = idNo }  
                };

            var sqlQuery = @"proc_checkEmailMobleExistenceMLC @checkValue,@Type,@RegisId,@idNo";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion
        public ResultSet Insert_UpdateMLC(MLCModel model)
        {
            var sqlParams = new SqlParameter[] { 
            
                new SqlParameter{ParameterName="@step",Value=model.step},
                new SqlParameter{ParameterName="@stepValue",Value=model.stepValue},
                new SqlParameter{ParameterName="@regisIdMLC",Value=model.regisIdMLC},
                new SqlParameter{ParameterName="@regByUser",Value=model.regByUser},
                new SqlParameter{ParameterName="@patientBroughtBy",Value=model.patientBroughtBy??string.Empty},
                new SqlParameter{ParameterName="@broughtByPersonrelation",Value=model.broughtByPersonrelation??string.Empty},
                new SqlParameter{ParameterName="@fullName",Value=model.fullName??string.Empty},
                new SqlParameter{ParameterName="@mobileNo",Value=model.mobileNo??string.Empty},
                new SqlParameter{ParameterName="@emailId",Value=model.emailId??string.Empty},
                new SqlParameter{ParameterName="@broughtByDesignation",Value=model.broughtByDesignation??string.Empty},
                new SqlParameter{ParameterName="@idNo",Value=model.idNo??string.Empty},
                new SqlParameter{ParameterName="@FIRno",Value=model.FIRno??string.Empty},
                new SqlParameter{ParameterName="@broughtByaddress",Value=model.broughtByaddress??string.Empty},
                new SqlParameter{ParameterName="@broughtBystateId",Value=model.broughtBystateId},
                new SqlParameter{ParameterName="@broughtBydistrictId",Value=model.broughtBydistrictId},
                new SqlParameter{ParameterName="@pinCode",Value=model.pinCode??string.Empty},
                new SqlParameter{ParameterName="@patientName",Value=model.patientName??string.Empty},
                new SqlParameter{ParameterName="@patientFatherName",Value=model.patientFatherName??string.Empty},
                new SqlParameter{ParameterName="@age",Value=model.age ??(object)DBNull.Value},
                new SqlParameter{ParameterName="@patientGender",Value=model.patientGender??string.Empty},
                new SqlParameter{ParameterName="@occupation",Value=model.occupation??string.Empty},
                new SqlParameter{ParameterName="@stateId",Value=model.stateId},
                new SqlParameter{ParameterName="@districtId",Value=model.districtId},
                new SqlParameter{ParameterName="@address",Value=model.address??string.Empty},
                new SqlParameter{ParameterName="@tehsilName",Value=model.tehsilName??string.Empty},
                new SqlParameter{ParameterName="@areaRoadName",Value=model.areaRoadName??string.Empty},
                new SqlParameter{ParameterName="@policeStation",Value=model.policeStation??string.Empty},
                new SqlParameter{ParameterName="@transIp",Value=model.transIp??string.Empty},
                new SqlParameter{ParameterName="@forwardtypeId",Value=model.forwardtypeId},
                new SqlParameter{ParameterName="@forwardtoId",Value=model.forwardtoId},
                new SqlParameter{ParameterName="@healthUnitDistrictId",Value=model.healthUnitDistrictId},
                new SqlParameter{ParameterName="@idTypeId",Value=model.idTypeId},
                new SqlParameter{ParameterName="@idNumber",Value=model.idNumber??string.Empty},
                new SqlParameter{ParameterName="@details",Value=model.details??string.Empty},
                new SqlParameter{ParameterName="@doctorName",Value=model.doctorName??string.Empty},
                new SqlParameter{ParameterName="@designation",Value=model.Designation??string.Empty},
                new SqlParameter{ParameterName="@seniorityNo",Value=model.seniorityNo??string.Empty},
                new SqlParameter{ParameterName="@restDays",Value=model.restDays ??(object)DBNull.Value},
                new SqlParameter{ParameterName="@xmldata",Value=model.xmldata??string.Empty},            
                new SqlParameter{ParameterName="@medicoLegalType",Value=model.medicoLegalType??string.Empty},            
                new SqlParameter{ParameterName="@OPDNumber",Value=model.OPDNumber??string.Empty},
                new SqlParameter{ParameterName="@MLCDate",Value=!string.IsNullOrEmpty(model.MLCDate)?Convert.ToDateTime(model.MLCDate): (object)DBNull.Value},
                new SqlParameter{ParameterName="@MLCtime",Value=model.MLCtime??string.Empty},
                new SqlParameter{ParameterName="@markOfIdentification",Value=model.markOfIdentification??string.Empty}
            };
            var _proc = @"proc_MLC_Registration_CHC @step,
    @stepValue,
    @regisIdMLC,
    @regByUser,
    @patientBroughtBy,
    @broughtByPersonrelation,
    @fullName,
    @mobileNo,
    @emailId,
    @broughtByDesignation,
    @idNo,
    @FIRno,
    @broughtByaddress,
    @broughtBystateId,
    @broughtBydistrictId,
    @pinCode,
    @patientName,
@patientFatherName,
    @age,
    @patientGender,
    @occupation,
@stateId,
    @districtId,
    @address,
    @tehsilName,
    @areaRoadName,
    @policeStation,
    @transIp,
    @forwardtypeId,
    @forwardtoId,
    @healthUnitDistrictId,
    @idTypeId,
    @idNumber,
    @details,
    @doctorName,
    @designation,
    @seniorityNo,
    @restDays,
    @xmldata,
    @medicoLegalType,
    @OPDNumber,
    @MLCDate,
    @MLCtime,
    @markOfIdentification";
            var res = this.Database.SqlQuery<ResultSet>(_proc, sqlParams).FirstOrDefault();
            return res;
        }
        public MLCModel getMLCStep(long regisId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdMLC",Value=regisId}
              };
            var _proc = @"proc_getMLC_Step  @regisIdMLC";
            var res = this.Database.SqlQuery<MLCModel>(_proc, sqlparams).FirstOrDefault();
            return res;
        }

        public List<rptMLCChild> getMLCEnquiry(long regisIdMLC)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdMLC",Value=regisIdMLC}
              };
            var _proc = @"proc_getMLC_Enquiry  @regisIdMLC";
            var res = this.Database.SqlQuery<rptMLCChild>(_proc, sqlparams).ToList();
            return res;
        }
        public List<DropDownList> GetIdType()
        {
            var _proc = @"proc_GetIdType_MLC";
            var slist = this.Database.SqlQuery<DropDownList>(_proc).ToList();
            return slist;
        }
        public List<MLCModel> GetComplete_MLC(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_GetComplete_MLC  @userId";
            var res = this.Database.SqlQuery<MLCModel>(_proc, sqlparams).ToList();
            return res;
        }
        public List<MLCModel> GetInComplete_MLC(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_GetInComplete_MLC  @userId";
            var res = this.Database.SqlQuery<MLCModel>(_proc, sqlparams).ToList();
            return res;
        }
        #endregion
        #region ICC riya
        #region check email existence
        public ResultSet CheckEmailMobileExistenceICC(string checkValue, string Type, long regisId = 0)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@regisId", Value = regisId }

                };

            var sqlQuery = @"proc_checkEmailMobleExistenceICC_CMO @checkValue,@Type,@regisId";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion
        public ICCModel getICCStep(long regisId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdICC",Value=regisId}
              };
            var _proc = @"Get_ICC_CMO  @regisIdICC";
            var res = this.Database.SqlQuery<ICCModel>(_proc, sqlparams).FirstOrDefault();
            return res;
        }
        public ResultSet ICCInsertUpdate(ICCModel model)
        {
            var sqlparams = new SqlParameter[]{

                        
       new SqlParameter {ParameterName="@regisIdICC",Value=model.regisIdICC},  
       new SqlParameter {ParameterName="@regisByuser",Value=model.regisByuser}, 
       new SqlParameter {ParameterName="@fatherName",Value=model.fatherName??string.Empty},  
       new SqlParameter {ParameterName="@motherName",Value=model.motherName??string.Empty},  
       new SqlParameter {ParameterName="@fullName",Value=model.fullName??string.Empty},  
       new SqlParameter {ParameterName="@emailId",Value=model.emailId??string.Empty},  
       new SqlParameter {ParameterName="@mobileNo",Value=model.mobileNo??string.Empty},  
       new SqlParameter {ParameterName="@dob",Value=model.dob??string.Empty},  
       new SqlParameter {ParameterName="@stateId",Value=model.stateId},
       new SqlParameter {ParameterName="@districtId",Value=model.districtId},  
       new SqlParameter {ParameterName="@address",Value=model.address??string.Empty},  
       new SqlParameter {ParameterName="@pinCode",Value=model.pinCode??string.Empty},  
       //new SqlParameter {ParameterName="@immunizationBookpath",Value=model.immunizationBookpath??string.Empty},  
       new SqlParameter {ParameterName="@transIP",Value=model.transIP??string.Empty}, 
       new SqlParameter {ParameterName="@forwardtypeId",Value=model.forwardtypeId},  
       new SqlParameter {ParameterName="@forwardtoId",Value=model.forwardtoId},  
       new SqlParameter {ParameterName="@healthUnitDistrictId",Value=model.healthUnitDistrictId},  
       new SqlParameter {ParameterName="@step",Value=model.step},  
       new SqlParameter {ParameterName="@UpdateStep",Value=model.UpdateStep},
       new SqlParameter {ParameterName="@ImmunizationDetails",Value=model.XmlData??string.Empty},
       new SqlParameter {ParameterName="@XmlDataChecklist",Value=model.XmlDataChecklist??string.Empty},
                };
            var _proc = @"proc_ICC_Registration_CMO @regisIdICC,@regisByuser,
@fatherName ,@motherName ,@fullName ,@emailId ,@mobileNo ,@dob ,@stateId ,@districtId ,@address,@pinCode ,
@transIP ,@forwardtypeId ,@forwardtoId ,@healthUnitDistrictId ,@step,@UpdateStep,@ImmunizationDetails,@XmlDataChecklist ";

            var res = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return res;
        }
        public List<ICCModel> BindImmunizationDetails()
        {

            var sqlProc = @"Proc_GetImmunizationType";
            var list = this.Database.SqlQuery<ICCModel>(sqlProc).ToList();
            return list;

        }
        public List<FAPAppProcessModel> rblforwardTypeICC()
        {
            var _proc = @"proc_FAP_forwardType";
            var slist = this.Database.SqlQuery<FAPAppProcessModel>(_proc).ToList();
            return slist;
        }
        public List<ICCModel> getICCCompleteRegistration(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_getICCComplete  @userId";
            var res = this.Database.SqlQuery<ICCModel>(_proc, sqlparams).ToList();
            return res;
        }
        public List<ICCModel> getICCInCompleteRegistration(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_GetInComplete_ICC  @userId";
            var res = this.Database.SqlQuery<ICCModel>(_proc, sqlparams).ToList();
            return res;
        }
        public List<ICCModel> getICCImmuCHC(long regisIdICC)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdICC",Value=regisIdICC}
              };
            var _proc = @"proc_getICCImmuCHC  @regisIdICC";
            var res = this.Database.SqlQuery<ICCModel>(_proc, sqlparams).ToList();
            return res;
        }
        #endregion
        #region DEC riya
        public ResultSet CheckEmailMobileExistenceDEC(string checkValue, string Type, long regisId)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type },
                    new SqlParameter { ParameterName = "@RegisId", Value = regisId } 
                };

            var sqlQuery = @"proc_checkEmailMobleExistenceDEC @checkValue,@Type,@RegisId";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        public DECModel getDECStep(long regisId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@regisIdDEC",Value=regisId}
              };
            var _proc = @"proc_getDEC_Step  @regisIdDEC";
            var res = this.Database.SqlQuery<DECModel>(_proc, sqlparams).FirstOrDefault();
            return res;
        }
        public ResultSet Insert_UpdateDEC(DECModel model)
        {
            var sqlparams = new SqlParameter[] {
            new SqlParameter{ParameterName="@step",Value=model.step},
            new SqlParameter{ParameterName="@stepvalue",Value=model.stepValue},
            new SqlParameter{ParameterName="@regByuser",Value=model.regByusers},
            new SqlParameter{ParameterName="@regisIdDEC",Value=model.regisIdDEC},
            new SqlParameter{ParameterName="@fullName",Value=model.fullName??string.Empty},
            new SqlParameter{ParameterName="@mobileNo",Value=model.mobileNo??string.Empty},
            new SqlParameter{ParameterName="@emailId",Value=model.emailId??string.Empty},
            new SqlParameter{ParameterName="@address",Value=model.address??string.Empty},
            new SqlParameter{ParameterName="@stateId",Value=model.stateId},
            new SqlParameter{ParameterName="@districtid",Value=model.districtid},
            new SqlParameter{ParameterName="@pinCode",Value=model.pinCode??string.Empty},
            new SqlParameter{ParameterName="@relationId",Value=model.relationId},
            new SqlParameter{ParameterName="@transIp",Value=model.transIp},
            new SqlParameter{ParameterName="@forwardtypeId",Value=model.forwardtypeId},
            new SqlParameter{ParameterName="@healthUnitDistrictId",Value=model.healthUnitDistrictId},
            new SqlParameter{ParameterName="@forwardtoId",Value=model.forwardtoId},     
            new SqlParameter{ParameterName="@DeathPersonName",Value=model.deathPersonName??string.Empty},
            new SqlParameter{ParameterName="@dod",Value=model.dod??string.Empty},
            new SqlParameter{ParameterName="@DeathPersonGender",Value=model.DeathPersonGender??string.Empty},
            new SqlParameter{ParameterName="@fatherName",Value=model.fathersName??string.Empty},
            new SqlParameter{ParameterName="@motherName",Value=model.motherName??string.Empty},
            new SqlParameter{ParameterName="@religionId",Value=model.religionId},
            new SqlParameter{ParameterName="@DeathPersonAadhaarNo",Value=model.aadhaarNo??string.Empty},
            new SqlParameter{ParameterName="@maritalStatusId",Value=model.maritalStatusId},
            new SqlParameter{ParameterName="@addressType",Value=model.addressType??string.Empty},
            new SqlParameter{ParameterName="@deathPersonAddress",Value=model.deathPersonAddress??string.Empty},
            new SqlParameter{ParameterName="@deathPersonStateId",Value=model.deathPersonStateId},
            new SqlParameter{ParameterName="@deathPersonDistrictId",Value=model.deathPersonDistrictId},
            new SqlParameter{ParameterName="@deathPersonPinCode",Value=model.deathPersonPinCode??string.Empty},
         
         

            
            };
            var _proc = @"proc_DEC_Registration_CHC @step ,@stepvalue ,@regByuser ,@regisIdDEC,@fullName,@mobileNo,@emailId,@address,@stateId,@districtId,
@Pincode,@relationId,@forwardtypeId,@healthUnitDistrictId ,@forwardtoId ,@DeathPersonName,@dod,@DeathPersonGender ,@fatherName,@motherName,
@religionId  ,@DeathPersonAadhaarNo,@maritalStatusId ,@addressType,@deathPersonAddress,@deathPersonStateId,@deathPersonDistrictId,@deathPersonPinCode,@transIp";
            var res = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return res;
        }
        public List<DECModel> GetComplete_DEC(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_GetComplete_DEC  @userId";
            var res = this.Database.SqlQuery<DECModel>(_proc, sqlparams).ToList();
            return res;
        }
        public List<DECModel> GetInComplete_DEC(long userId)
        {
            var sqlparams = new SqlParameter[]{

                          new SqlParameter {ParameterName="@userId",Value=userId}
              };
            var _proc = @"proc_GetInComplete_DEC  @userId";
            var res = this.Database.SqlQuery<DECModel>(_proc, sqlparams).ToList();
            return res;
        }
        #endregion

        #region NUH Renewal
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
        public List<NUHPartnerModel> getNUHPartner(long regisNUHId)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisNUHId}  
            };
            var _proc = @"proc_getNUHPartnerRenewal @regisId";
            var slist = this.Database.SqlQuery<NUHPartnerModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public ResultSet SearchDetails(SearchDetailModel model, long districtId)
        {
            long regisByuser = objSM.UserID;
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@districtId",Value=districtId},
            new SqlParameter{ParameterName="@meeRegisNo",Value=model.meeRegisNo},
            new SqlParameter{ParameterName="@certificateNo",Value=model.certificateNo}
            };
            var _proc = @"proc_SearchDetailsForRenewalCMO @districtId,@meeRegisNo,@certificateNo";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public NUHmodel GetDetailsNUHByRegisId(long regisID)
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@regisId",Value=regisID} 
            };
            var _proc = @"proc_GetDetailsNUHByRegisIdCMO @regisId";
            var slist = this.Database.SqlQuery<NUHmodel>(_proc, sqlParam).SingleOrDefault();
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
                        //new SqlParameter { ParameterName = "@addressproofFilePath", Value =model.addressproofFilePath} ,
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
                        new SqlParameter { ParameterName = "@xmldatacheckList", Value =model.xmldatacheckList},
                        new SqlParameter { ParameterName = "@transIP", Value =model.transIP} ,
                        new SqlParameter { ParameterName = "@requestKey", Value =model.requestKey??string.Empty},
                        new SqlParameter { ParameterName = "@meeRegisNo", Value =model.meeRegisNo},
                        new SqlParameter { ParameterName = "@applicantMobileNo", Value =model.appMobileNo},
                        new SqlParameter { ParameterName = "@isCertificateFromPortal", Value =model.isCertificateFromPortal},

                        //Abhishek
                        new SqlParameter { ParameterName = "@structuralLyoutFile", Value =model.structuralLyoutFilePath??string.Empty} ,
                        new SqlParameter { ParameterName = "@piPhotograph", Value =model.piPhotographPath??string.Empty},
                        new SqlParameter { ParameterName = "@upmci_smfCertificateFile", Value =model.upmci_smfCertificateFilePath??string.Empty} ,
                        new SqlParameter { ParameterName = "@establishmentPlace", Value =model.establishmentPlace} ,
                        new SqlParameter { ParameterName = "@landType", Value =model.landType} ,
                        new SqlParameter { ParameterName = "@isNOC", Value =model.isNOC} ,
                        new SqlParameter { ParameterName = "@isFirefightingSystem", Value =model.isFirefightingSystem} ,
                        new SqlParameter { ParameterName = "@isDispose", Value =model.isDispose} ,
                        new SqlParameter { ParameterName = "@applicantEmailId", Value =model.applicantEmailId} ,
                        new SqlParameter { ParameterName = "@qualification", Value =model.qualification} ,
                        new SqlParameter { ParameterName = "@institution", Value =model.institution} ,
                        new SqlParameter { ParameterName = "@Central_StateCouncilName", Value =model.Central_StateCouncilName??(object)DBNull.Value} ,
                        new SqlParameter { ParameterName = "@registrationNumber", Value =model.registrationNumber??(object)DBNull.Value} ,
                        new SqlParameter { ParameterName = "@applicantAddress", Value =model.applicantAddress} ,
                        new SqlParameter { ParameterName = "@applicantStateId", Value =model.applicantStateId} ,
                        new SqlParameter { ParameterName = "@applicantDistrictId", Value =model.applicantDistrictId} ,
                        new SqlParameter { ParameterName = "@applicantPincode", Value =model.applicantPincode} ,
                        new SqlParameter { ParameterName = "@applicantName", Value =model.applicantName} ,
                        new SqlParameter { ParameterName = "@nocCertificationNo", Value =model.nocCertificationNo??string.Empty},
                        new SqlParameter { ParameterName = "@nOCFilePath", Value =model.nOCFilePath??string.Empty} ,
                        new SqlParameter { ParameterName = "@disposedFile", Value =model.disposedFilePath??string.Empty} ,
                        new SqlParameter { ParameterName = "@firefightingSystemFilePath", Value =model.firefightingSystemFilePath??string.Empty},                        
                        new SqlParameter { ParameterName = "@disposedNo", Value =model.disposedNo??string.Empty} ,
                        new SqlParameter { ParameterName = "@ElectrycityBill", Value =model.ElectrycityBillPath??string.Empty},
                        new SqlParameter { ParameterName = "@Registry ", Value =model.RegistryPath??string.Empty},
                        new SqlParameter { ParameterName = "@RentalAgreement ", Value =model.RentalAgreementPath??string.Empty},
                        new SqlParameter { ParameterName = "@ownerSignature ", Value =model.ownerSignature??string.Empty}
        };

            var sqlProc = @"Sp_NUHInsertUpdateRenewalCMO @regisIdNUH ,
                                                      @regByUser ,
                                                      @isMEEaddressChange ,
                                                      @telephoneNo ,
                                                      @website ,
                                                      @address  ,
                                                      @pinCode ,
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
                                                      @xmldatacheckList,
                                                      @transIP ,
                                                      @requestKey,
                                                      @meeRegisNo,
                                                      @applicantMobileNo,

@isCertificateFromPortal, @structuralLyoutFile, @piPhotograph, @upmci_smfCertificateFile, @establishmentPlace, @landType, @isNOC, @isFirefightingSystem, @isDispose, @applicantEmailId, @qualification, @institution, @Central_StateCouncilName, @registrationNumber, @applicantAddress, @applicantStateId, @applicantDistrictId, @applicantPincode, @applicantName, @nocCertificationNo, @nOCFilePath, @disposedFile, @firefightingSystemFilePath, @disposedNo ,@ElectrycityBill,@Registry,@RentalAgreement,@ownerSignature";
            var sList = this.Database.SqlQuery<ResultSet>(sqlProc, sqlParams).ToList().FirstOrDefault();
            return sList;
        }
        #endregion
    }



}