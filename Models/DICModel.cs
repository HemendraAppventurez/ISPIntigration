using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    #region riya
    public class DICModel
    {
        public long regisIdDIC { get; set; }
        public long regByUser { get; set; }
        public string registrationNo { get; set; }

        [Required(ErrorMessage = "Select Applied Type")]
        public string ApplyingFor { get; set; }
        [Required(ErrorMessage = "Revised Certificate Proof Required")]
        public string releventProof { get; set; }
        [Required(ErrorMessage = "Full Name Required")]
        public string fullName { get; set; }
        [Required(ErrorMessage = "Father's name Required")]
        public string fatherName { get; set; }
        [Required(ErrorMessage = "Date of Birth Required")]
        public string dob { get; set; }
        [Required(ErrorMessage = "Age Required")]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Age must be Number")]
        [Range(1, 100, ErrorMessage = "Age between 1 to 100")]
        public int? age{get;set;}
        [Required(ErrorMessage = "Please Choose Gender")]
        public string gender { get; set; }
        [Required(ErrorMessage = "Please Select Category")]
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistence", "DIC", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Mobile Number Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string mobileNo { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistenceDIC", "appRegistration", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Mobile Number Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string appmobileNo { get; set; }
        //[Required(ErrorMessage = "Email Id Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email !")]
        public string emailId { get; set; }
        //[Required(ErrorMessage = "Adhar Number Required")]
        public string adharNumber { get; set; }
        //[Required(ErrorMessage = "Please Choose Medical Establishment")]
        public int stateId { get; set; }
        public string StateName { get; set; }
        [Required(ErrorMessage = "Please Select District")]
        public int districtId { get; set; }
        public string DistrictName { get; set; }
        [Required(ErrorMessage = "Address Required")]
        public string address { get; set; }
        [Required(ErrorMessage = "Pin Code Required")]
        [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid PinCode")]
        public string pinCode { get; set; }
        [Required(ErrorMessage = "Specify Disability Type")]
        public string disabilityType { get; set; }
        [Required(ErrorMessage = "Choose Disability Type")]
        public int disabilityTypeId { get; set; }
        public string disabilityTypeName { get; set; }
        [Required(ErrorMessage = "Disability Details Required")]
        public string disabilityDetail { get; set; }
        //[Required(ErrorMessage = "Please Upload Image")]
        public string photoPath { get; set; }
        [Required(ErrorMessage = "Please Upload Passport Size Photo")]
        public string passportsizephoto { get; set; }
        [Required(ErrorMessage = "Please Upload Id Proof")]
        public string idProofPath { get; set; }
        [Required(ErrorMessage = "Please Upload Required Document")]
        public string documentPath { get; set; }
         [Required(ErrorMessage = "Please Upload Thumb Impression")]
        public string thumbImpPath { get; set; }
        [Required(ErrorMessage = "Appear Date Required")]
        public string appearDate { get; set; }
        //[Required(ErrorMessage = "Please Choose Medical Establishment")]
        public bool isNotAppeared { get; set; }
        public string regByTransIp { get; set; }
        public string transIp { get; set; }

        [Required(ErrorMessage = "Please Choose Id Proof Type")]
        public int idProofId { get; set; }
        [Required(ErrorMessage = "Photo Id Number Required")]
        public string photoIdNo { get; set; }
        public string idProofName { get; set; }
        [Required(ErrorMessage = "Please Choose Address Proof Type")]
        public int addressProofId { get; set; }
        public string addressProofName { get; set; }

        public string releventProofpath { get; set; }
        public string photoPathpath { get; set; }
        public string passportsizephotopath { get; set; }
        public string idProofPathpath { get; set; }
        public string documentPathpath { get; set; }
        public string thumbImpPathpath { get; set; }
        public string requestDate { get; set; }
        public string appliedStatus { get; set; } 
        public int appStatus { get; set; }

        public string inspectionDate { get; set; }
        public string nextInspectionDate { get; set; }
        public int appFor { get; set; }
        public int Verify { get; set; }
        public long certGenrBy { get; set; }
        public string requestKey { get; set; }
        public string nocCertificationNo { get; set; }
        public string CertificateNo { get; set; }
        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }
        public List<DICModel> DICModelList { get; set; }

        [Required(ErrorMessage = "Select Yes/No")]
        public string isCertFromPortal { get; set; }
        //[Required(ErrorMessage = "Date of Birth Required")]
        public string oldCertificateNumber { get; set; }
        public string certificateFilePath { get; set; }

        public string xmldatacheckList { get; set; }
        public int step { get; set; }
        public int stepValue { get; set; }
        public int UpdateStep { get; set; }
        public string appType { get; set; }
        public string msg { get; set; }


      
        public string inspReportFilePath { get; set; }
        public string certificateGeneratedDate { get; set; }
        public string RejectDate { get; set; }
        public string RejectRemark { get; set; }

    }
    public class committeModelDIC
    {
        public string commMemName { get; set; }
        public string commMemDesig { get; set; }
        public string commMemDept { get; set; }
    }
    public class DICAppProcessModel
    {

        [Required(ErrorMessage = "Please Upload File")]
        public HttpPostedFileBase inspReportFilePhoto { get; set; }
        public string[] inspReportFilePhotoPath { get; set; }
        public string XmlDataPhoto { get; set; }



        public long regisIdDIC { get; set; }
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
        [Required(ErrorMessage = "Enter Remark")]
        public string rejectedRemarks { get; set; }

        public int forwardedType { get; set; }
        [Required(ErrorMessage = "Select Forwarded To")]
        public long forwardedTo { get; set; }
       
        public int testTypeId { get; set; }
        public bool isLessFourtyPer { get; set; }
        public string xmlCommiMembers { get; set; }
        public long userId { get; set; }
        public string transIp { get; set; }

        [Range(40, 100, ErrorMessage = "Percentage of Disability Between 40 to 100")]
        [RegularExpression("^(100)|([0-9]{1,2}){0,1}(\\.[0-9]{1,2}){0,1}$", ErrorMessage = "Enter Valid Percentage of Disability")]
        [Required(ErrorMessage = "Enter Percentage of Disability")]
        public decimal disabilityPer { get; set; }

        [Required(ErrorMessage = "Enter Mark Of Identification")]
        public string markOfIdentification { get; set; }
        [Required(ErrorMessage = "Condition Of Disability Required!")]
        public int conditionId { get; set; }
        public string conditionName { get; set; }
        [Required(ErrorMessage = "Select Re-assessment")]
        public int reassId { get; set; }
        [Required(ErrorMessage = "Re-assessment Period Required!")]
        public int reassPeriod { get; set; }
        public string reassName { get; set; }
        [Required(ErrorMessage = "Re-assessment Period Type Required!")]
        public string reassPeriodType { get; set; }
    }

    public class DICCertificateDetailModel
    {
        public long regisIdDIC { get; set; }
        public string registrationNo { get; set; }
        public string fullName { get; set; }
        public string fatherName { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string disabilityType { get; set; }
        public string disabilityDetail { get; set; }
        public string certificateNo { get; set; }
        public string certiGenDate { get; set; }
        public int monthYear { get; set; }
        public string disabilityPer { get; set; }
        public string markOfIdentification { get; set; }
        public long inprocessDICId { get; set; }
        public string passportsizephoto { get; set; }
        public byte[] Photo { get; set; }
        public int committeeId  {get;set;}
        public string conditionName { get; set; }
        public string reassName { get; set; }
        public string photoIdNo { get; set; }
        public string certiGendBy { get; set; }
        public int reassPeriod { get; set; }
        public string reassPeriodType { get; set; }
        public string idProofName { get; set; }
        public int districtId { get; set; }
    }

    public class DisabilityType
    {
       // public long vaccineIdICC { get; set; }
        public long disabilityTypeId { get; set; }
        public string disabilityTypeName { get; set; }
        public int isExsistDis { get; set; }
        public string DisabilityDescription { get; set; }
       // public string iscertified { get; set; }
        public List<ICCModel> appDisabilityType { get; set; }
    }
     
    #endregion
}