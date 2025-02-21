using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    #region Riya
    public class FICModel
    { 
        public long regisIdFIC { get; set; }
        public long regByUser { get; set; }
        public string registrationNo { get; set; }
        public string userName { get; set; }

        //[Required(ErrorMessage = "Enter Hospital Name")]
        public string opdName { get; set; }
        [Required(ErrorMessage = "Enter OPD Receipt Number")]
        public string opdRecNo { get; set; }
        [Required(ErrorMessage = "Enter Date of Check-up")]
        public string opdDate { get; set; }
        [Required(ErrorMessage = "Enter Address")]
        public string opdAddress { get; set; }
        //[Required(ErrorMessage = "Enter Pin code")]
        [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid PinCode")]
        public string opdPinCode { get; set; }
        [Required(ErrorMessage = "Select State")]
        public int opdStateId { get; set; }
        public string StateName { get; set; }
        [Required(ErrorMessage = "Select District")]
        public int opdDistrictId { get; set; }
        [Required(ErrorMessage = "Upload Document")]
        public string opdFile { get; set; }
        public string opdFilePath { get; set; }
        [Required(ErrorMessage = "Select Area")]
        public long forwardtoId { get; set; }
        public string forwardtoName { get; set; }
        public long forwardtypeId { get; set; }
        public string forwardtypeName { get; set; }
        [Required(ErrorMessage = "Enter Full Name")]
        public string fullName { get; set; }
        [Required(ErrorMessage = "Enter Father's Name")]
        public string fatherName { get; set; }
        [Required(ErrorMessage = "Enter Date of Birth")]
        public string dob { get; set; }
        public string gender { get; set; }
        public string rbtnGender { get; set; }
        [Required(ErrorMessage = "Select Category")]
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistence", "FIC", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Enter a valid 10 digit Mobile Number")]
        [Required(ErrorMessage = "Enter Mobile Number")]
        public string mobileNo { get; set; }
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email")]
        [DataType(DataType.EmailAddress)]
        //[Required(ErrorMessage = "Enter Email Address")]
        public string emailId { get; set; }

        [System.Web.Mvc.Remote("CheckMobileExistenceFIC", "appRegistration", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Enter a valid 10 digit Mobile Number")]
        [Required(ErrorMessage = "Enter Mobile Number")]
        public string appmobileNo { get; set; }

        [Required(ErrorMessage = "Select Institution Type")]
        public int institutionTypeId { get; set; }
        public string institutionTypeName { get; set; }

        //[Required(ErrorMessage = "Enter Doctor Name")]
        public string fitnessAdvicedBy { get; set; }
        //[Required(ErrorMessage = "Enter Treatment From")]
        public string treatmentFrom { get; set; }
        //[Required(ErrorMessage = "Enter Treatment To")]
        public string treatmentto { get; set; }
        [Required(ErrorMessage = "Enter Reason")]
        public string applicationReason { get; set; }
        [Required(ErrorMessage = "Enter Detail of Illness")]
        public string illnessDetail { get; set; }
        [Required(ErrorMessage = "Select District")]
        public int districtId { get; set; }
        public string DistrictName { get; set; }
        [Required(ErrorMessage = "Select Institution For Appointment")]
        public int appoinInstId { get; set; }
        public string appoinInstName { get; set; }
        public string transIp { get; set; }
        public int appStatus { get; set; }
        public string remarks { get; set; }
        [Required(ErrorMessage = "Remark Required!")]
        public string rejectedRemarks { get; set; }
        public string appliedStatus { get; set; }

        public int TotalCount { get; set; }
        public int InProcessCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }

        public string applicationDate { get; set; }
        //[Required(ErrorMessage = "Date Required!")]
        //public string inspectionDate { get; set; }
        [Required(ErrorMessage = "Inspection Document Required!")]
        public string inspectionRpt { get; set; }
        public string inspectionRptPath { get; set; }
        [Required(ErrorMessage = "Remark Required!")]
        public string inspectionRejectedRemark { get; set; }
        [Required(ErrorMessage = "Feedback Required!")]
        public bool inspectionRptStatus { get; set; }
        [Required(ErrorMessage = "Select Yes or No")]
        public string isMedicalHistory { get; set; }

        [Required(ErrorMessage = "Select District")]
        public int healthUnitDistrictId { get; set; }
        public string healthUnitDistrictName { get; set; }
        public string hdnMeHistory { get; set; }

        public string requestKey { get; set; }
        public string appliedDate { get; set; }

        [Required(ErrorMessage = "Enter Certificate Number")]
        public string certificateNo { get; set; }

        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }

        public List<FICModel> FICModelList { get; set; }

        [Required(ErrorMessage = "Enter Doctor Name")]
        public string certGenerateByDoc { get; set; }
        [Required(ErrorMessage = "Enter Designation")]
        public string certGenerateByDesig { get; set; }
        [Required(ErrorMessage = "Enter Date")]
        public string inspecttionCompeletionDate { get; set; }
        [Required(ErrorMessage = "Enter Identification Mark")]
        public string identificationMark { get; set; }
        public string Area { get; set; }

        [Required(ErrorMessage = "Enter Details of Medical History")]
        public string detailsMedicalHistory { get; set; }

        [Required(ErrorMessage = "Upload Id File")]
        public string idFile { get; set; }
        public string idFilePath { get; set; }
        [Required(ErrorMessage = "Select Id Type")]
        public int idTypeId { get; set; }
        [Required(ErrorMessage = "Id Number Required!")]
        public string idNumber { get; set; }
        [Required(ErrorMessage = "Enter Mark Of Identification")]
        public string markOfIdentification { get; set; }
       // [Required(ErrorMessage = "No Of Days Required!")]
        public int NoOfDays { get; set; }
        public int regisIdILC { get; set; }
        public string AppType { get; set; }
        public string idTypeName { get; set; }

        public int stepValue { get; set; }
        public int step { get; set; }
        public string XmlCheckList { get; set; }
        public int Msg { get; set; }
        public string ILCcertificateNo { get; set; }
        public string RegType { get; set; }

        public string HUDistrict { get; set; }
        public string HUAuthorisedPerson { get; set; }
        public string officer { get; set; }
        public string RejectRemark { get; set; }
    }
    #region Riya
    public class FICDetailsModel
    {
        public long regisIdFIC { get; set; }
        public string registrationNo { get; set; }
        public string fullName { get; set; }
        public string fatherName { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string categoryName { get; set; }
        public string mobileNo { get; set; }
        public string appliedStatus { get; set; }
        public int appStatus { get; set; }
        public string applicationDate { get; set; }
        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }
        public string certificateFilePath { get; set; }
        public string RegType { get; set; }

        public string inspReportFilePath { get; set; }
        public string certificateGeneratedDate { get; set; }
        public string RejectDate { get; set; }
        public string RejectRemark { get; set; }

        public List<FICDetailsModel> FICDetailsList { get; set; }
    }
    #endregion
    #endregion
}
