using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{

    public class NUHmodel
    {

       
        public long regisIdNUH { get; set; }
        public long regByUser { get; set; }
        public string registrationNo { get; set; }
        [Required(ErrorMessage = "Please Choose Medical Establishment")]
        public int medicalEstablishmentId { get; set; }
        public string medicalEstablishmentName { get; set; }
        [Required(ErrorMessage = "Medical Establishment Other is Required")]
        public string medicalEstablishmentOther { get; set; }
        [Required(ErrorMessage = "Please Choose Type of Establishment")]
        public int establishmentCategoriesId { get; set; }
        public string EstablishmentCategoriesName { get; set; }
        [Required(ErrorMessage = "Select Establishment Category")]
        public int establishmentSubCategoriesId { get; set; }
        public string clinicalEstablishmentSubTypeName { get; set; }
        [Required(ErrorMessage = " Establishment Name is Required")]
        public string establishmentName { get; set; }
        public string HfrNo { get; set; }
        [Required(ErrorMessage = "Please Choose State")]
        public int stateId { get; set; }
        public string StateName { get; set; }
        [Required(ErrorMessage = "Please Choose District")]
        public int districtid { get; set; }
        public string DistrictName { get; set; }
        public string ownernDistrictName { get; set; }
        public string ownernStateName { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        public string address { get; set; }

        [Required(ErrorMessage = "Pin Code Is Required")]
        [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
        public string pinCode { get; set; }

        [Required(ErrorMessage = "Please Attach Address Proof")]
        public string addressproofFile { get; set; }
        public string addressproofFilePath { get; set; }

        //[Required(ErrorMessage = "Please Attach Building Structural Layout")]
        public string structuralLyoutFile { get; set; }
        public string structuralLyoutFilePath { get; set; }

        [RegularExpression("[0-9]{10,15}", ErrorMessage = "Invalid number")]
        public string telephoneNo { get; set; }

        public string website { get; set; }

        [Required(ErrorMessage = "Medical Services Are Required")]
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
        [Required(ErrorMessage = "Name is Required")]
        public string applicantName { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        public string applicantAddress { get; set; }
        [Display(Name="State")]
        public int applicantStateId { get; set; }
        public string applicantStateName { get; set; }
        [Required(ErrorMessage = "Please Choose District")]
        public int applicantDistrictId { get; set; }
        public string applicantDistrictName { get; set; }

        [Required(ErrorMessage = "Pin Code Is Required")]
        [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
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
        public string appliedStatus { get; set; }
        //public int nursingDocumentId { get; set; }
        public string EstablishmentSubCategoriesName { get; set; }
        public string staffName { get; set; }
        public string staffqualification { get; set; }
        public string staffinstitution { get; set; }
        public string staffRegistrationType { get; set; }
        public string staffRegistrationNo { get; set; }
        public string staffNameOfMCI_SMF { get; set; }
        public string staffHprNo { get; set; }
        public string staffAge { get; set; }
        public string staffaddress { get; set; }
        public string syear { get; set; }
        public string ssign { get; set; }

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
        public string doctorHprNo { get; set; }
        public string doctorAge { get; set; }
        public string doctoraddress { get; set; }
        public string dyear { get; set; }
        public string dsignature { get; set; }

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
        [Display(Name="Age")]
        public int ownerAge { get; set; }
        public string ownerFatherName { get; set; }
        // [Required(ErrorMessage = "Age Required")]
        //public int piAge { get; set; }
        // [Required(ErrorMessage = "Father Name Required")]
        //public string piFatherName { get; set; }
        public string ownerPhotograph { get; set; }
        public string ownerPhotographPath { get; set; }
        //[Required(ErrorMessage = "Signature is Required")]
        public string ownerSignature { get; set; }
        public string ownerSignaturePath { get; set; }
        [Required(ErrorMessage = "Upload Photo")]
        public string piPhotograph { get; set; }
        public string piPhotographPath { get; set; }
        // [Required(ErrorMessage = "Upload Signature")]
        //public string piSignature { get; set; }
        //public string piSignaturePath { get; set; }

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

        public bool isDeclared { get; set; }
        public DateTime DeclarationDate { get; set; }
        public string DeclarationIp { get; set; }
        public long DeclarationUserId { get; set; }

        //public string doctorName { get; set; }
        //public string doctorFathersName { get; set; }
        //public string doctorQualification { get; set; }
        //public string NameofFoundation { get; set; }
        public string doctorregistrationType { get; set; }
        //public string doctorregistrationNo { get; set; }
        //public string doctorPart_FullTime { get; set; }

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
        public string RejectRemark { get; set; }

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

        public List<NUHmodel> NUHModelList { get; set; }
        public List<NUHPartnerModel> NUHPartnerList { get; set; }
        public List<NUHdoctorModel> NUHDOCList { get; set; }
        [Required(ErrorMessage = "Please upload file")]
        public string queryFile { get; set; }
        public string queryFilePath { get; set; }
        public string QueryRaisedDate { get; set; }
        public string QueryReplyDate { get; set; }

        [Required(ErrorMessage = "Place Of Establishment Other is Required")]
        public string establishmentPlaceOther { get; set; }

        public string AppCancleFilePath { get; set; }

        [Required(ErrorMessage = " Electricity Bill is Required")] //Shashi
        public string ElectrycityBill { get; set; }
        
        public string ElectrycityBillPath { get; set; }

        [Required(ErrorMessage = "Registry is Required")]  //Shashi
        public string Registry { get; set; }
        public string RegistryPath { get; set; }

        [Required(ErrorMessage = " RentalAgreement is Required")]  //Shashi
        public string RentalAgreement { get; set; }
        public string RentalAgreementPath { get; set; }


        
    }


    public class QueryModel
    {

        public string QueryDetails { get; set; }
    }

    public class RegisterDetailsModel
    {
        public long regisIdNUH { get; set; }
        public long regisId { get; set; }
        public string registrationNo { get; set; }
        public long regByUser { get; set; }
        public string notarizedAffidavitFilePath { get; set; }
        public bool isRenewal { get; set; }
        public int operatedId { get; set; }
        public long doctorId { get; set; }
        public string DeclarationDate { get; set; }
        public string ParamedicalDeclarationDate { get; set; }
      
    }

    public class NUHDetailsModel
    {
        public long regisIdNUH { get; set; }
        public string establishmentName { get; set; }
        public string EstablishmentType { get; set; }
        public string medicalEstablishment { get; set; }
       public string appliedDate { get; set; }
        public string registrationNo { get; set; }
        public string ownerName { get; set; }
        public string ownerMobileNo { get; set; }
        public string ownerEmailId { get; set; }
        public string ownerAddress { get; set; }
        public string UPMCI_SMF_Number { get; set; }
        public string addressproofFilePath { get; set; }
        public string notarizedAffidavitFilePath { get; set; }
        public string appliedStatus { get; set; }
        public int appStatus { get; set; }
        public string inspectionDate { get; set; }
        public int medicalEstablishmentId { get; set; }
        public int districtid { get; set; }
        public string CertificateNo { get; set; }
        public long regByUser { get; set; }
        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }
        public string certificateFilePath { get; set; }
        public string certificateGeneratedDate { get; set; }
        public string inspReportFilePath { get; set; }
        public string RejectDate { get; set; }
        public string RejectRemark { get; set; }
        public DateTime transDate { get; set; }
        public Int64 SrNo { get; set; }
        public List<NUHDetailsModel> NUHDetailsList { get; set; }


        [Required(ErrorMessage = "Query Raised is Required")]
        public string QueryRaisedByCMO { get; set; }
        public string ReplyQueryByApplicant { get; set; }

        public string QueryStatus { get; set; }

        public string QMessage { get; set; }
        public int DaysOfPendency { get; set; }
        public string StatusLimit { get; set; }

        public string meeRegisNo { get; set; }
        public string meeRegisNoGenDate { get; set; }
        public string address { get; set; }
        public string DistrictName { get; set; }
        public string StateName { get; set; }
        public string pinCode { get; set; }
        public int NewRenewYear { get; set; }
        public string IssuedOn { get; set; }
        public string ValidTill { get; set; }
        public bool isInPatient { get; set; }
        public int numberofBed { get; set; }
        public string certiGendBy { get; set; }
        public string districtGendBy { get; set; }
    }

    public class NUHAppProcessModel
    {
        public long regisIdNUH { get; set; }
        public int appStatus { get; set; }
        [Required(ErrorMessage = "Select Committee")]
        public long committeeId { get; set; }
        [Required(ErrorMessage = "Select Inspection Date")]
        public string inspectionDate { get; set; }
        [Required(ErrorMessage = "Please Upload File")]
        public string inspReportFile { get; set; }
        public string inspReportFilePath { get; set; }
        public string certificateFile { get; set; }
        public string certificateFilePath { get; set; }

        [Required(ErrorMessage = "Please Upload File")]
        public HttpPostedFileBase inspReportFilePhoto { get; set; }
        public string [] inspReportFilePhotoPath { get; set; }
        [Required(ErrorMessage = "Enter Remark")]
        public string rejectedRemarks { get; set; }
        public long userId { get; set; }
        public string transIp { get; set; }
        public string XmlData { get; set; }
        public string XmlDataPhoto { get; set; }
        public int districtid { get; set; }


        [Required(ErrorMessage = "Query Raised is Required")]
        public string QueryRaisedByCMO { get; set; }
        public string ReplyQueryByApplicant { get; set; }
        [Required(ErrorMessage = "Select Reason")]
        public int ReasonID { get; set; }
    }

    public class NUHgenerateCertificateRpt
    {
        public long regisIdNUH { get; set; }
        public int appStatus { get; set; }
    }

    public class NUHdoctorModel//to show in grid
    {
        public long doctorId { get; set; }
        public string doctorName { get; set; }
        public string doctorFathersName { get; set; }
        public string doctorQualification { get; set; }
        public string NameofFoundation { get; set; }
        public string doctorregistrationType { get; set; }
        public string doctorregistrationNo { get; set; }
        public string doctorHprNo { get; set; }
        public string doctorPart_FullTime { get; set; }
        public string docFilePath { get; set; }
        public string doctorAge { get; set; }
        public string doctoraddress { get; set; }
        public string dyear { get; set; }
        public string dsignature { get; set; }
        public List<NUHdoctorModel> NUHDOCList { get; set; }
    }

    public class NUHPartnerModel//to show in grid
    {
        public string ownerName { get; set; }
        public int ownerAge { get; set; }
        public string ownerFatherName { get; set; }
        public string ownerMobileNo { get; set; }
        public string ownerEmailId { get; set; }
        public string ownerAddress { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }
        public string ownerPincode { get; set; }
        public string ownerPhotograph { get; set; }
        public string ownerSignature { get; set; }

        public List<NUHPartnerModel> NUHPartnerList { get; set; }
    }

    public class SearchDetailModel
    {
        [Required(ErrorMessage = "Enter Establishment Registration Number")]
        public string meeRegisNo { get; set; }
        [Required(ErrorMessage = "Enter Certificate Number")]
        public string certificateNo { get; set; }
        [Required(ErrorMessage = "Choose the Required Field")]
        public string isCertFrmPortal { get; set; }
    }

    public class NUH_FacilityOffered
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int chk { get; set; }
    }

}