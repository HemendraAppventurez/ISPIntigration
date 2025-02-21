using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    #region RIYA
    public class ReportsModel
    {
        //[Required(ErrorMessage="Select Application Type")]
        public int appTypeId { get; set; }
        [Required(ErrorMessage = "Select From Date")]
        public string fromDate { get; set; }
        [Required(ErrorMessage = "Select TO Date")]
        public string toDate { get; set; }

        public int id { get; set; }
        public string DistrictName { get; set; }

        public int DistrictId { get; set; }
        [Required(ErrorMessage = "Select District")]
        public string[] DistrictIds { get; set; }
        public int citizen { get; set; }
        public int cmo { get; set; }
        public int total { get; set; }
        public int Pending { get; set; }//Scruitny

        public int TotalPending { get; set; }
        public int ApplicationRejected { get; set; }
        public int Application_Accepted { get; set; }
        public int Inspection_Scheduled { get; set; }
        public int Inspection_Rpt_uploaded { get; set; }
        public int Inspection_Rpt_Acc { get; set; }
        public int Certificate_Generated { get; set; }
        public int InspectionRpt_Rejected { get; set; }

        public int Application_Forwarded { get; set; }
        public int District_Committee_Report_Uploaded { get; set; }
        public int State_Committee_Report_Uploaded { get; set; }
        public int Sanction_Apporved { get; set; }

        public int Forwarded_to_Confirm_Disabilty { get; set; }

        public string Module { get; set; }
        public string ModuleCode { get; set; }
        public long RollID { get; set; }

        //Service Module
        public Int32 ModuleId { get; set; }

        //Application List Vinod
        public long RegistrationId { get; set; }
        public string establishmentName { get; set; }
        public string medicalEstablishment { get; set; }
        public string appliedDate { get; set; }
        public string registrationNo { get; set; }
        public string UPMCI_SMF_Number { get; set; }
        //  public string addressproofFilePath { get; set; }
        public string notarizedAffidavitFilePath { get; set; }
        public string appliedStatus { get; set; }
        public int appStatus { get; set; }

        public string ApplicationNumber { get; set; }
        public string SelectedDistrictID { get; set; }
        /// <summary>
        ///CM office report
        /// </summary>
        /// 

        public int AppFromPortal { get; set; }
        public int AppNotFromPortal { get; set; }
        public int Nistarit { get; set; }
        public int Lambit { get; set; }
        public decimal Percentage { get; set; }
        [Required(ErrorMessage = "Select Health Unit")]
        public int forwardtypeId { get; set; }
        public string forwardtypeName { get; set; }

        /// <summary>
        /// CHC
        /// </summary>
        /// 
        //public int pending { get; set; }
        public int InProcess { get; set; }

        public int RptType { get; set; }

        

        // public List<ReportsModel> ReportModelList { get; set; }
    }

    #endregion

    public class CountReportModel
    {
        public int zoneId { get; set; }
        public string zoneName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int serviceId { get; set; }
        public string serviceName { get; set; }
        public int totalReceived { get; set; }
        public int approved { get; set; }
        public int rejected { get; set; }
        public int pendingInTimeLimit { get; set; }
        public int pendingOverTimeLimit { get; set; }
        public long rollId { get; set; }
        public long profileId { get; set; }
        public string fullName { get; set; }
        public string CMServiceName { get; set; }

        public string fromDate { get; set; }
        public string toDate { get; set; }

        public int totalcount { get; set; }
        public string opdAdd { get; set; }
        public string HospitalName { get; set; }
        public int appCurrStatus { get; set; }
    }

    public class ApplicationDetailsModel
    {
        public string applicationStatus { get; set; }
        public long regisId { get; set; }
        public int serviceId { get; set; }
        public string serviceName { get; set; }
        public string zoneName { get; set; }
        public string DistrictName { get; set; }
        public int appCurrStatus { get; set; }
        public int timeLimitDays { get; set; }
        public int receivedDays { get; set; }
        public string registrationNo { get; set; }
        public string applicationDate { get; set; }
        public string applicantName { get; set; }
        public string AadharNoMks { get; set; }
        public string MobileNo { get; set; }
        public string MaskapplicantMobileNo { get; set; }
        public string HospitalName { get; set; }
        public string opdAdd { get; set; }

        public string fromDate { get; set; }
        public string toDate { get; set; }

        private string _applicantMobileNo;
        public string applicantMobileNo
        {
            get { return _applicantMobileNo; }
            set
            {
                _applicantMobileNo = value;
                if (!string.IsNullOrEmpty(_applicantMobileNo) && _applicantMobileNo.Length >= 10)
                {
                    var firstDigits = _applicantMobileNo.Substring(0, 6);
                    var lastDigits = _applicantMobileNo.Substring(_applicantMobileNo.Length - 4, 4);
                    var requiredMask = new String('X', _applicantMobileNo.Length - lastDigits.Length);
                    var maskedString = string.Concat(requiredMask, lastDigits);
                    MaskapplicantMobileNo = Regex.Replace(maskedString, ".{4}", "$0 ");
                }
                else
                {
                    MaskapplicantMobileNo = "";
                }
            }
        }


        public string MaskAadharNo { get; set; }
        private string _AadharNo;
        public string AadharNo
        {
            get { return _AadharNo; }
            set
            {
                // Set B to some new value
                _AadharNo = value;

                // Assign C
                //MaskStuAadharNo = string.Format("B has been set to {0}", value);

                if (!string.IsNullOrEmpty(_AadharNo) && _AadharNo.Length >= 12)
                {
                    var firstDigits = _AadharNo.Substring(0, 8);
                    var lastDigits = _AadharNo.Substring(_AadharNo.Length - 4, 4);

                    var requiredMask = new String('X', _AadharNo.Length - lastDigits.Length);

                    var maskedString = string.Concat(requiredMask, lastDigits);
                    MaskAadharNo = Regex.Replace(maskedString, ".{4}", "$0 ");
                }
                else
                {
                    MaskAadharNo = "";
                }
            }
        }


        public string approvedDate { get; set; }
        public string rejectedDate { get; set; }
        public string rejectedRemark { get; set; }
        public string appliedStatus { get; set; }
        public List<ApplicationDetailsModel> appDetailList { get; set; }
    }

    public class ApplicationStatusReportDetailsModel
    {
        //-----------
        public string medicalEstablishment { get; set; }
        public string appliedDate { get; set; }
        public string registrationNo { get; set; }
        public string UPMCI_SMF_Number { get; set; }
        public string addressproofFilePath { get; set; }
        public string appliedStatus { get; set; }
        //---------

        public int appStatus { get; set; }
        public int appTypeId { get; set; }
        public long RegistrationId { get; set; }
        public long regByUser { get; set; }
        // [Required(ErrorMessage = "Please Choose Medical Establishment")]
        public int medicalEstablishmentId { get; set; }
        public string medicalEstablishmentName { get; set; }
        //   [Required(ErrorMessage = "Medical Establishment Other is Required")]
        public string medicalEstablishmentOther { get; set; }
        //  [Required(ErrorMessage = "Please Choose Type of Establishment")]
        public int establishmentCategoriesId { get; set; }
        public string EstablishmentCategoriesName { get; set; }
        //   [Required(ErrorMessage = "Select Establishment Category")]
        public int establishmentSubCategoriesId { get; set; }
        public string clinicalEstablishmentSubTypeName { get; set; }
        //  [Required(ErrorMessage = " Establishment Name is Required")]
        public string establishmentName { get; set; }

        //  [Required(ErrorMessage = "Please Choose State")]
        public int stateId { get; set; }
        public string StateName { get; set; }
        // [Required(ErrorMessage = "Please Choose District")]
        //   public int districtid { get; set; }

        public int DistrictId { get; set; }

        public string DistrictName { get; set; }
        public string ownernDistrictName { get; set; }
        public string ownernStateName { get; set; }
        // [Required(ErrorMessage = "Address is Required")]
        public string address { get; set; }

        // [Required(ErrorMessage = "Pin Code Is Required")]
        //  [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
        public string pinCode { get; set; }

        //  [Required(ErrorMessage = "Please Attach Address Proof")]
        public string addressproofFile { get; set; }
        //   public string addressproofFilePath { get; set; }

        //[Required(ErrorMessage = "Please Attach Building Structural Layout")]
        public string structuralLyoutFile { get; set; }
        public string structuralLyoutFilePath { get; set; }

        //  [RegularExpression("[0-9]{10,15}", ErrorMessage = "Invalid number")]
        public string telephoneNo { get; set; }

        public string website { get; set; }

        //  [Required(ErrorMessage = "Medical Services Are Required")]
        public string medicalFacilities { get; set; }

        //[Required(ErrorMessage = "Name is Required")]
        public string ownerName { get; set; }

        //[Required(ErrorMessage = "Address Required")]
        public string ownerAddress { get; set; }

        //[Required(ErrorMessage = "Please Choose State")]
        public int ownerStateId { get; set; }

        //[Required(ErrorMessage = "Please Choose District")]
        public int ownerDistrictId { get; set; }

        //[Required(ErrorMessage = "Pin Code is Required")]
        //[RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
        public string ownerPincode { get; set; }


        //[Required(ErrorMessage = "Mobile No is Required")]
        //[RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string ownerMobileNo { get; set; }

        //[Required(ErrorMessage = "Email Id is Required")]
        //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email !")]
        public string ownerEmailId { get; set; }

        public bool isBelongToMedical { get; set; }
        public bool isMEEaddressChange { get; set; }
        // [Required(ErrorMessage = "Name is Required")]
        public string applicantName { get; set; }
        //  [Required(ErrorMessage = "Address is Required")]
        public string applicantAddress { get; set; }
        //  [Display(Name = "State")]
        public int applicantStateId { get; set; }
        public string applicantStateName { get; set; }
        //   [Required(ErrorMessage = "Please Choose District")]
        public int applicantDistrictId { get; set; }
        public string applicantDistrictName { get; set; }

        //     [Required(ErrorMessage = "Pin Code Is Required")]
        //   [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
        public string applicantPincode { get; set; }
        [Required(ErrorMessage = "Qualification is Required")]
        public string qualification { get; set; }
        [Required(ErrorMessage = "Institution is Required")]
        public string institution { get; set; }
        //[Required(ErrorMessage = "Registration No is Required")]
        public string registrationNumber { get; set; }
        //[Required(ErrorMessage = "Central State/Council Name is Required")]
        public string Central_StateCouncilName { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistence", "NUH", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Mobile No is Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string applicantMobileNo { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistence", "AppRegistration", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Mobile No is Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string appMobileNo { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email !")]
        public string applicantEmailId { get; set; }

        //[Required(ErrorMessage = "Please Attach Certificate")]
        public string upmci_smfCertificateFile { get; set; }
        public string upmci_smfCertificateFilePath { get; set; }
        //[Required(ErrorMessage = "Please Choose System Of Medicine")]
        public int medicinSystemId { get; set; }
        public string medicinSystemName { get; set; }
        //[Required(ErrorMessage = "Please Choose Type of Clinical Service")]
        public int clinicalServicesId { get; set; }
        public string clinicalServicesName { get; set; }
        //[Required(ErrorMessage = "Please Select Type of Clinical Establishment")]
        public int clinicalEstablishmentTypeId { get; set; }
        public string clinicalEstablishmentTypeName { get; set; }
        [Required(ErrorMessage = "Please Select Establishment Category")]
        public int clinicalEstablishmentSubTypeId { get; set; }
        //[Required(ErrorMessage = "Other is Required")]
        public string clinicalEstablishmentTypeOther { get; set; }
        [Required(ErrorMessage = "Number of Beds Required")]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Only numeric value allow")]
        public int numberofBed { get; set; }
        [Required(ErrorMessage = "Choose Yes If You Have NOC")]
        public bool isNOC { get; set; }
        [Required(ErrorMessage = "Choose Yes If You Have Disposed Certificate")]
        public bool isDispose { get; set; }
        [Required(ErrorMessage = "Please Attach NOC")]
        public string nOCFile { get; set; }
        [Required(ErrorMessage = "Please Attach Disposed Cerificate")]
        public string disposedFile { get; set; }
        [Required(ErrorMessage = "Certification No. is Required")]
        public string nocCertificationNo { get; set; }
        [Required(ErrorMessage = "Certification No. is Required")]
        public string disposedNo { get; set; }
        public string nOCFilePath { get; set; }
        public string disposedFilePath { get; set; }
        [Required(ErrorMessage = "Choose Yes Only If You Have Installed Fire Fighter System")]
        public bool isFirefightingSystem { get; set; }
        [Required(ErrorMessage = "Please Attach File")]
        public string firefightingSystemFile { get; set; }
        public string firefightingSystemFilePath { get; set; }
        //[Required(ErrorMessage = "Please Attach Affidavit")]
        public string notarizedAffidavitFile { get; set; }
        public string notarizedAffidavitFilePath { get; set; }
        public string regBytransIp { get; set; }
        public DateTime regBytransdate { get; set; }
        public string transIP { get; set; }
        public DateTime transDate { get; set; }

        [Required(ErrorMessage = "Other Establishment Category is Required")]
        public string otherEstablishmentCategory { get; set; }

        //[Required(ErrorMessage = "Other Type of Sevice is Required")]
        public string otherServiceType { get; set; }

        public string requestDate { get; set; }
        //public int nursingDocumentId { get; set; }
        public string EstablishmentSubCategoriesName { get; set; }
        public string staffName { get; set; }
        public string staffqualification { get; set; }
        public string staffinstitution { get; set; }
        public string staffRegistrationType { get; set; }
        public string staffRegistrationNo { get; set; }
        public string staffNameOfMCI_SMF { get; set; }
        public string filePath { get; set; }
        public string docFilePath { get; set; }
        public string xml { get; set; }
        public string XmlDataOwner { get; set; }
        public string XmlDataDoc { get; set; }
        public long certGenrBy { get; set; }
        public string requestKey { get; set; }
        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }
        public int step { get; set; }
        public int stepValue { get; set; }
        public string operatedBy { get; set; }
        [Required(ErrorMessage = "Operated Name is Required")]
        public string operatedName { get; set; }
        [Required(ErrorMessage = "Select Operate Type")]
        public int operatedId { get; set; }
        public string stafffatherName { get; set; }
        public string part_fullTime { get; set; }
        public long doctorId { get; set; }
        public string doctorName { get; set; }
        public string doctorFathersName { get; set; }
        public string doctorQualification { get; set; }
        public string NameofFoundation { get; set; }
        public string doctorregistrationNo { get; set; }
        public string doctorPart_FullTime { get; set; }
        public string xmldataParmedical { get; set; }
        public string xmldatadoctor { get; set; }
        public string xmldatacheckList { get; set; }

        [Required(ErrorMessage = "Establishment Area Required")]
        public string establishmentArea { get; set; }
        [Required(ErrorMessage = "Establishment Type Required")]
        public string establishmentPlace { get; set; }
        [Required(ErrorMessage = "Land Type Required")]
        public string landType { get; set; }
        //[Required(ErrorMessage = "Land Type Required")]
        [Display(Name = "Age")]
        public int ownerAge { get; set; }
        public string ownerFatherName { get; set; }
        // [Required(ErrorMessage = "Age Required")]
        //public int piAge { get; set; }
        // [Required(ErrorMessage = "Father Name Required")]
        //public string piFatherName { get; set; }
        public string ownerPhotograph { get; set; }
        public string ownerPhotographPath { get; set; }
        public string ownerSignature { get; set; }
        public string ownerSignaturePath { get; set; }
        [Required(ErrorMessage = "Upload Photo")]
        public string piPhotograph { get; set; }
        public string piPhotographPath { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string ownerNameF { get; set; }
        [Required(ErrorMessage = "Age is Required")]
        public int ownerAgeF { get; set; }
        [Required(ErrorMessage = "Father Name is Required")]
        public string ownerFatherNameF { get; set; }
        [Required(ErrorMessage = "Mobile No is Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string ownerMobileNoF { get; set; }
        [Required(ErrorMessage = "Email Id is Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email !")]
        public string ownerEmailIdF { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        public string ownerAddressF { get; set; }
        [Required(ErrorMessage = "State is Required")]
        public string ownerStateIdF { get; set; }
        [Required(ErrorMessage = "District is Required")]
        public string ownerDistrictIdF { get; set; }
        [Required(ErrorMessage = "Pin Code is Required")]
        [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
        public string ownerPincodeF { get; set; }
        [Required(ErrorMessage = "Photo is Required")]
        public string ownerFPhotograph { get; set; }
        public string ownerFPhotographPath { get; set; }
        [Required(ErrorMessage = "Signature is Required")]
        public string ownerFSignature { get; set; }
        public string ownerFSignaturePath { get; set; }


        public string doctorregistrationType { get; set; }


        [Required(ErrorMessage = "Choose Yes If You Have InPatient")]
        public bool isInPatient { get; set; }
        [Required(ErrorMessage = "Choose Yes If You Have OutPatient")]
        public bool isOutPatient { get; set; }
        public string otherOutPatient { get; set; }
        [Required(ErrorMessage = "Choose Yes If You Have Laboratory")]
        public bool isLaboratory { get; set; }
        public string otherLaboratory { get; set; }
        [Required(ErrorMessage = "Choose Yes If You Have Imaging")]
        public bool isImaging { get; set; }
        public string otherImaging { get; set; }
        public string otherFacility { get; set; }

        public int outPatientId { get; set; }
        public string outPatientName { get; set; }
        public int laboratoryid { get; set; }
        public string laboratoryName { get; set; }
        public int imagingId { get; set; }
        public string imagingName { get; set; }

        public string xmldataOutPatient { get; set; }
        public string xmldataLaboratory { get; set; }
        public string xmldataImaging { get; set; }

        public string meeRegisNo { get; set; }
        public string appType { get; set; }
        public bool isRenewal { get; set; }

        public bool isCertificateFromPortal { get; set; }
        [Required(ErrorMessage = "Old Registration No is Required")]
        public string outerRegistrationNo { get; set; }
        [Required(ErrorMessage = "Old Certificate No is Required")]
        public string outerCertificateNo { get; set; }
        [Required(ErrorMessage = "Old Certificate is Required")]
        public string outerCertificateFile { get; set; }
        public string outerCertificateFilePath { get; set; }


        [Required(ErrorMessage = "Query Raised is Required")]
        public string QueryRaisedByCMO { get; set; }
        public string ReplyQueryByApplicant { get; set; }

        public string QMessage { get; set; }

        public int ReasonID { get; set; }
        public string QueryStatus { get; set; }

        //public List<ApplicationStatusReportDetailsModel> NUHModelList { get; set; }
        // public List<NUHPartnerModel> NUHPartnerList { get; set; }
        //  public List<NUHdoctorModel> NUHDOCList { get; set; }
        [Required(ErrorMessage = "Please upload file")]
        public string queryFile { get; set; }
        public string queryFilePath { get; set; }
        public string QueryRaisedDate { get; set; }
        public string QueryReplyDate { get; set; }

        [Required(ErrorMessage = "Place Of Establishment Other is Required")]
        public string establishmentPlaceOther { get; set; }

        //Added MER MODEL
        public string patientName { get; set; }
        public string patientrelationsWithEmployee { get; set; }
        public string patientdiseaseName { get; set; }

        //DIC MODEL
        public string fullName { get; set; }
        public string mobileNo { get; set; }
        public string disabilityType { get; set; }
        public string inspectionDate { get; set; }
        public string inspReportFilePath { get; set; }
        public string certificateGeneratedDate { get; set; }
        public string RejectDate { get; set; }
        public string RejectRemark { get; set; }


        public string claimantMobileNo { get; set; }
        public decimal? claimAmount { get; set; }
        public string claimantName { get; set; }
        public string dateofDeath { get; set; }
        public string affidavitfilePath { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string emailId { get; set; }

        public List<ApplicationStatusReportDetailsModel> ReportModelList { get; set; }
        public string buttonSearchValue { get; set; }
        public int hdnappCS { get; set; }


    }


    public class ApplicationWorkFlowStepStatusModel
    {

        public string[] inspReportFilePhotoPath { get; set; }

        public string[] inspReportFilePhotoPathCometti { get; set; }

        public string CMOName { get; set; }
        public string establishmentName { get; set; }
        public string CategoryName { get; set; }
        public string appliedDate { get; set; }
        public string registrationNo { get; set; }

        public string treatmentName { get; set; }
        public string UPMCI_SMF_Number { get; set; }

        public int AppStatus { get; set; }
        public string ApplicationType { get; set; }


        public string AppSubmittedDate { get; set; }
        public string AppAcceptedDate { get; set; }
        public string AppAcceptedBy { get; set; }
        public string AppRejectedDate { get; set; }
        public string AppRejectBy { get; set; }

        public string InspectionSheduleDate { get; set; }
        public string InspectionSheduleBy { get; set; }

        public string InspReportUploadedDate { get; set; }
        public string InspectionRejectedAcceptedDate { get; set; }
        public string InspectionReportRejectedBy { get; set; }

        public string applicantSubTypeName { get; set; }

        public string SanctionApproveDate { get; set; }
        public string SanctionApproveBy { get; set; }

        public string CertificateGeneratedDate { get; set; }
        public string CertificateGeneratedBy { get; set; }

        public string InspectionDate { get; set; }
        public string InspReportFilePath { get; set; }

        public string appliedStatus { get; set; }
        public string appRejectedRemark { get; set; }

        public string InspectionRejectRemark { get; set; }

        public string CertificateNumber { get; set; }

        public string InspectionReportAcceptBy { get; set; }

        public string DistrictName { get; set; }

        public string AppSubmittedBy { get; set; }

        public string EmailId { get; set; }

        //FAP Model
        public string claimantName { get; set; }
        public string claimantMobileNo { get; set; }
        public decimal claimAmount { get; set; }
        public string affidavit { get; set; }
        public string forwardTo { get; set; }
        public string ForwardType { get; set; }
        public string IsVerify { get; set; }
        public string isApprove { get; set; }
        public string healthUnitName { get; set; }

        public string VerifyDate { get; set; }

        public string VerifyByUser { get; set; }
        public string forwardByuser { get; set; }
        public string forwardDate { get; set; }
        public string DistrictComettiReportUploadedDate { get; set; }
        public string StateComettiReportUploadedDate { get; set; }

        //AGC Model

        public string xRayPlateNo { get; set; }
        public string xRayDate { get; set; }
        public string dentalPlateNo { get; set; }
        public string Age { get; set; }

        public string districtReportFilePath { get; set; }
        public string stateReportFilePath { get; set; }

        public string stateReportUploadedBy { get; set; }
        public string districtReportUploadedBy { get; set; }

        public string InspectionStatus { get; set; }

        public string SancationDate { get; set; }
        public string sanctiontransDate { get; set; }
        public decimal SancationAmount { get; set; }
        public string SanctionBy { get; set; }
        //Query Model
        public string QueryRaised { get; set; }
        public string QueryReply { get; set; }
        public string QueryRaisedDate { get; set; }
        public string ReplyReleventFilePath { get; set; }
        public string QueryReplyDate { get; set; }
        public string QueryRaisedBy { get; set; }
        public string QueryReplyBy { get; set; }
        //Inspection Report Upload Model
        public string ComettiMemberName { get; set; }
        public string certificateFilePath { get; set; }
        public string pendingAt { get; set; }
        public string isCertificateGenerated { get; set; }
        public string certificateNo { get; set; }

        //DIC Model
        public string patientName { get; set; }
        public string patientrelationsWithEmployee { get; set; }
        public string patientdiseaseName { get; set; }
        public string isInspReportAccepted { get; set; }


        public string fullName { get; set; }
        public string mobileNo { get; set; }
        public string isLessFourtyPer { get; set; }
        public string disabilityPer { get; set; }
        public string nextInspectionDate { get; set; }
        public string TestTypeId { get; set; }
        public string disabilityType { get; set; }

        public string markOfIdentification { get; set; }
        public string conditionId { get; set; }
        public string reassId { get; set; }
        public string reassPeriod { get; set; }
        public string reassPeriodType { get; set; }

        //MER Model

        public string departmentName { get; set; }
        public string officerName { get; set; }
        public string dateOfLetter { get; set; }
        public string letterNo { get; set; }

        public string ApplyingFor { get; set; }
        public string isCertFromPortal { get; set; }
        public string oldCertificateNumber { get; set; }
        public string releventProof { get; set; }

        public int step { get; set; }

        public string fatherName { get; set; }
        public string dob { get; set; }
        public string appCancelDate { get; set; }
        public string appCancelBy { get; set; }
        public string appCancelFile { get; set; }
        public string AppCancleRemark { get; set; }
      //  public List<ApplicationWorkFlowStepStatusModel> AppStatusList { get; set; }
    }

    public class cmoOfficeRpt
    {
        //-----------
        public string medicalEstablishment { get; set; }
        public string appliedDate { get; set; }
        public string registrationNo { get; set; }
        public string UPMCI_SMF_Number { get; set; }
        public string addressproofFilePath { get; set; }
        public string appliedStatus { get; set; }
        //---------

        public int appStatus { get; set; }
        public int appTypeId { get; set; }
        public long RegistrationId { get; set; }
        public long regByUser { get; set; }
        // [Required(ErrorMessage = "Please Choose Medical Establishment")]
        public int medicalEstablishmentId { get; set; }
        public string medicalEstablishmentName { get; set; }
        //   [Required(ErrorMessage = "Medical Establishment Other is Required")]
        public string medicalEstablishmentOther { get; set; }
        //  [Required(ErrorMessage = "Please Choose Type of Establishment")]
        public int establishmentCategoriesId { get; set; }
        public string EstablishmentCategoriesName { get; set; }
        //   [Required(ErrorMessage = "Select Establishment Category")]
        public int establishmentSubCategoriesId { get; set; }
        public string clinicalEstablishmentSubTypeName { get; set; }
        //  [Required(ErrorMessage = " Establishment Name is Required")]
        public string establishmentName { get; set; }

        //  [Required(ErrorMessage = "Please Choose State")]
        public int stateId { get; set; }
        public string StateName { get; set; }
        // [Required(ErrorMessage = "Please Choose District")]
        //   public int districtid { get; set; }

        public int DistrictId { get; set; }

        public string DistrictName { get; set; }
        public string ownernDistrictName { get; set; }
        public string ownernStateName { get; set; }
        // [Required(ErrorMessage = "Address is Required")]
        public string address { get; set; }

        // [Required(ErrorMessage = "Pin Code Is Required")]
        //  [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
        public string pinCode { get; set; }

        //  [Required(ErrorMessage = "Please Attach Address Proof")]
        public string addressproofFile { get; set; }
        //   public string addressproofFilePath { get; set; }

        //[Required(ErrorMessage = "Please Attach Building Structural Layout")]
        public string structuralLyoutFile { get; set; }
        public string structuralLyoutFilePath { get; set; }

        //  [RegularExpression("[0-9]{10,15}", ErrorMessage = "Invalid number")]
        public string telephoneNo { get; set; }

        public string website { get; set; }

        //  [Required(ErrorMessage = "Medical Services Are Required")]
        public string medicalFacilities { get; set; }

        //[Required(ErrorMessage = "Name is Required")]
        public string ownerName { get; set; }

        //[Required(ErrorMessage = "Address Required")]
        public string ownerAddress { get; set; }

        //[Required(ErrorMessage = "Please Choose State")]
        public int ownerStateId { get; set; }

        //[Required(ErrorMessage = "Please Choose District")]
        public int ownerDistrictId { get; set; }

        //[Required(ErrorMessage = "Pin Code is Required")]
        //[RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
        public string ownerPincode { get; set; }


        //[Required(ErrorMessage = "Mobile No is Required")]
        //[RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string ownerMobileNo { get; set; }

        //[Required(ErrorMessage = "Email Id is Required")]
        //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email !")]
        public string ownerEmailId { get; set; }

        public bool isBelongToMedical { get; set; }
        public bool isMEEaddressChange { get; set; }
        // [Required(ErrorMessage = "Name is Required")]
        public string applicantName { get; set; }
        //  [Required(ErrorMessage = "Address is Required")]
        public string applicantAddress { get; set; }
        //  [Display(Name = "State")]
        public int applicantStateId { get; set; }
        public string applicantStateName { get; set; }
        //   [Required(ErrorMessage = "Please Choose District")]
        public int applicantDistrictId { get; set; }
        public string applicantDistrictName { get; set; }

        //     [Required(ErrorMessage = "Pin Code Is Required")]
        //   [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
        public string applicantPincode { get; set; }
        [Required(ErrorMessage = "Qualification is Required")]
        public string qualification { get; set; }
        [Required(ErrorMessage = "Institution is Required")]
        public string institution { get; set; }
        //[Required(ErrorMessage = "Registration No is Required")]
        public string registrationNumber { get; set; }
        //[Required(ErrorMessage = "Central State/Council Name is Required")]
        public string Central_StateCouncilName { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistence", "NUH", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Mobile No is Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string applicantMobileNo { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistence", "AppRegistration", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Mobile No is Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string appMobileNo { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email !")]
        public string applicantEmailId { get; set; }

        //[Required(ErrorMessage = "Please Attach Certificate")]
        public string upmci_smfCertificateFile { get; set; }
        public string upmci_smfCertificateFilePath { get; set; }
        //[Required(ErrorMessage = "Please Choose System Of Medicine")]
        public int medicinSystemId { get; set; }
        public string medicinSystemName { get; set; }
        //[Required(ErrorMessage = "Please Choose Type of Clinical Service")]
        public int clinicalServicesId { get; set; }
        public string clinicalServicesName { get; set; }
        //[Required(ErrorMessage = "Please Select Type of Clinical Establishment")]
        public int clinicalEstablishmentTypeId { get; set; }
        public string clinicalEstablishmentTypeName { get; set; }
        [Required(ErrorMessage = "Please Select Establishment Category")]
        public int clinicalEstablishmentSubTypeId { get; set; }
        //[Required(ErrorMessage = "Other is Required")]
        public string clinicalEstablishmentTypeOther { get; set; }
        [Required(ErrorMessage = "Number of Beds Required")]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Only numeric value allow")]
        public int numberofBed { get; set; }
        [Required(ErrorMessage = "Choose Yes If You Have NOC")]
        public bool isNOC { get; set; }
        [Required(ErrorMessage = "Choose Yes If You Have Disposed Certificate")]
        public bool isDispose { get; set; }
        [Required(ErrorMessage = "Please Attach NOC")]
        public string nOCFile { get; set; }
        [Required(ErrorMessage = "Please Attach Disposed Cerificate")]
        public string disposedFile { get; set; }
        [Required(ErrorMessage = "Certification No. is Required")]
        public string nocCertificationNo { get; set; }
        [Required(ErrorMessage = "Certification No. is Required")]
        public string disposedNo { get; set; }
        public string nOCFilePath { get; set; }
        public string disposedFilePath { get; set; }
        [Required(ErrorMessage = "Choose Yes Only If You Have Installed Fire Fighter System")]
        public bool isFirefightingSystem { get; set; }
        [Required(ErrorMessage = "Please Attach File")]
        public string firefightingSystemFile { get; set; }
        public string firefightingSystemFilePath { get; set; }
        //[Required(ErrorMessage = "Please Attach Affidavit")]
        public string notarizedAffidavitFile { get; set; }
        public string notarizedAffidavitFilePath { get; set; }
        public string regBytransIp { get; set; }
        public DateTime regBytransdate { get; set; }
        public string transIP { get; set; }
        public DateTime transDate { get; set; }

        [Required(ErrorMessage = "Other Establishment Category is Required")]
        public string otherEstablishmentCategory { get; set; }

        //[Required(ErrorMessage = "Other Type of Sevice is Required")]
        public string otherServiceType { get; set; }

        public string requestDate { get; set; }
        //public int nursingDocumentId { get; set; }
        public string EstablishmentSubCategoriesName { get; set; }
        public string staffName { get; set; }
        public string staffqualification { get; set; }
        public string staffinstitution { get; set; }
        public string staffRegistrationType { get; set; }
        public string staffRegistrationNo { get; set; }
        public string staffNameOfMCI_SMF { get; set; }
        public string filePath { get; set; }
        public string docFilePath { get; set; }
        public string xml { get; set; }
        public string XmlDataOwner { get; set; }
        public string XmlDataDoc { get; set; }
        public long certGenrBy { get; set; }
        public string requestKey { get; set; }
        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }
        public int step { get; set; }
        public int stepValue { get; set; }
        public string operatedBy { get; set; }
        [Required(ErrorMessage = "Operated Name is Required")]
        public string operatedName { get; set; }
        [Required(ErrorMessage = "Select Operate Type")]
        public int operatedId { get; set; }
        public string stafffatherName { get; set; }
        public string part_fullTime { get; set; }
        public long doctorId { get; set; }
        public string doctorName { get; set; }
        public string doctorFathersName { get; set; }
        public string doctorQualification { get; set; }
        public string NameofFoundation { get; set; }
        public string doctorregistrationNo { get; set; }
        public string doctorPart_FullTime { get; set; }
        public string xmldataParmedical { get; set; }
        public string xmldatadoctor { get; set; }
        public string xmldatacheckList { get; set; }

        [Required(ErrorMessage = "Establishment Area Required")]
        public string establishmentArea { get; set; }
        [Required(ErrorMessage = "Establishment Type Required")]
        public string establishmentPlace { get; set; }
        [Required(ErrorMessage = "Land Type Required")]
        public string landType { get; set; }
        //[Required(ErrorMessage = "Land Type Required")]
        [Display(Name = "Age")]
        public int ownerAge { get; set; }
        public string ownerFatherName { get; set; }
        // [Required(ErrorMessage = "Age Required")]
        //public int piAge { get; set; }
        // [Required(ErrorMessage = "Father Name Required")]
        //public string piFatherName { get; set; }
        public string ownerPhotograph { get; set; }
        public string ownerPhotographPath { get; set; }
        public string ownerSignature { get; set; }
        public string ownerSignaturePath { get; set; }
        [Required(ErrorMessage = "Upload Photo")]
        public string piPhotograph { get; set; }
        public string piPhotographPath { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string ownerNameF { get; set; }
        [Required(ErrorMessage = "Age is Required")]
        public int ownerAgeF { get; set; }
        [Required(ErrorMessage = "Father Name is Required")]
        public string ownerFatherNameF { get; set; }
        [Required(ErrorMessage = "Mobile No is Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string ownerMobileNoF { get; set; }
        [Required(ErrorMessage = "Email Id is Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email !")]
        public string ownerEmailIdF { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        public string ownerAddressF { get; set; }
        [Required(ErrorMessage = "State is Required")]
        public string ownerStateIdF { get; set; }
        [Required(ErrorMessage = "District is Required")]
        public string ownerDistrictIdF { get; set; }
        [Required(ErrorMessage = "Pin Code is Required")]
        [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
        public string ownerPincodeF { get; set; }
        [Required(ErrorMessage = "Photo is Required")]
        public string ownerFPhotograph { get; set; }
        public string ownerFPhotographPath { get; set; }
        [Required(ErrorMessage = "Signature is Required")]
        public string ownerFSignature { get; set; }
        public string ownerFSignaturePath { get; set; }


        public string doctorregistrationType { get; set; }


        [Required(ErrorMessage = "Choose Yes If You Have InPatient")]
        public bool isInPatient { get; set; }
        [Required(ErrorMessage = "Choose Yes If You Have OutPatient")]
        public bool isOutPatient { get; set; }
        public string otherOutPatient { get; set; }
        [Required(ErrorMessage = "Choose Yes If You Have Laboratory")]
        public bool isLaboratory { get; set; }
        public string otherLaboratory { get; set; }
        [Required(ErrorMessage = "Choose Yes If You Have Imaging")]
        public bool isImaging { get; set; }
        public string otherImaging { get; set; }
        public string otherFacility { get; set; }

        public int outPatientId { get; set; }
        public string outPatientName { get; set; }
        public int laboratoryid { get; set; }
        public string laboratoryName { get; set; }
        public int imagingId { get; set; }
        public string imagingName { get; set; }

        public string xmldataOutPatient { get; set; }
        public string xmldataLaboratory { get; set; }
        public string xmldataImaging { get; set; }

        public string meeRegisNo { get; set; }
        public string appType { get; set; }
        public bool isRenewal { get; set; }

        public bool isCertificateFromPortal { get; set; }
        [Required(ErrorMessage = "Old Registration No is Required")]
        public string outerRegistrationNo { get; set; }
        [Required(ErrorMessage = "Old Certificate No is Required")]
        public string outerCertificateNo { get; set; }
        [Required(ErrorMessage = "Old Certificate is Required")]
        public string outerCertificateFile { get; set; }
        public string outerCertificateFilePath { get; set; }


        [Required(ErrorMessage = "Query Raised is Required")]
        public string QueryRaisedByCMO { get; set; }
        public string ReplyQueryByApplicant { get; set; }

        public string QMessage { get; set; }

        public int ReasonID { get; set; }
        public string QueryStatus { get; set; }

        //public List<ApplicationStatusReportDetailsModel> NUHModelList { get; set; }
        // public List<NUHPartnerModel> NUHPartnerList { get; set; }
        //  public List<NUHdoctorModel> NUHDOCList { get; set; }
        [Required(ErrorMessage = "Please upload file")]
        public string queryFile { get; set; }
        public string queryFilePath { get; set; }
        public string QueryRaisedDate { get; set; }
        public string QueryReplyDate { get; set; }

        [Required(ErrorMessage = "Place Of Establishment Other is Required")]
        public string establishmentPlaceOther { get; set; }

        //Added MER MODEL
        public string patientName { get; set; }
        public string patientrelationsWithEmployee { get; set; }
        public string patientdiseaseName { get; set; }

        //DIC MODEL
        public string fullName { get; set; }
        public string mobileNo { get; set; }
        public string disabilityType { get; set; }
        public string inspectionDate { get; set; }
        public string inspReportFilePath { get; set; }
        public string certificateGeneratedDate { get; set; }
        public string RejectDate { get; set; }
        public string RejectRemark { get; set; }


        public string claimantMobileNo { get; set; }
        public string claimantName { get; set; }
        public string dateofDeath { get; set; }
        public string affidavitfilePath { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string emailId { get; set; }

        public List<ApplicationStatusReportDetailsModel> ReportModelList { get; set; }
        public string buttonSearchValue { get; set; }



    }

}