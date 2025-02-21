using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{

    public class ILCModel
    {
        public long regisIdILC { get; set; }
        public long regByUser { get; set; }
        public string registrationNo { get; set; }
        //[Required(ErrorMessage = "Enter Hospital Name")]
        public string opdName { get; set; }
        [Required(ErrorMessage = "Enter OPD Receipt Number")]
        public string opdReceiptno { get; set; }
        [Required(ErrorMessage = "Enter Date of Check-up")]
        public string opdDate { get; set; }

        [Required(ErrorMessage = "Select District")]
        public int opdDistrictId { get; set; }
        public string DistrictName { get; set; }

        [Required(ErrorMessage = "Enter Address")]
        public string opdAddress { get; set; }

        //[Required(ErrorMessage = "PinCode is Required")]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Invalid PinCode")]
        public string opdPincode { get; set; }

        public int opdStateId { get; set; }
        public string StateName { get; set; }

        [Required(ErrorMessage = "Upload OPD Reciept")]
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
        [Required(ErrorMessage="Gender is Required")]
        public string gender { get; set; }
        public string rbtnGender { get; set; }
        [Required(ErrorMessage = "Select Category")]
        public int categoryId { get; set; }
        public string categoryName { get; set; }

        [System.Web.Mvc.Remote("CheckMobileExistence", "ILC", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Enter a valid 10 digit Mobile Number")]
        [Required(ErrorMessage = "Enter Mobile Number")]
        public string mobileNo { get; set; }

        [System.Web.Mvc.Remote("CheckMobileExistenceILC", "appRegistration", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Enter a valid 10 digit Mobile Number")]
        [Required(ErrorMessage = "Enter Mobile Number")]
        public string appmobileNo { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email")]
        [DataType(DataType.EmailAddress)]
       // [Required(ErrorMessage = "Enter Email Address")]
        public string emailId { get; set; }
       

        [Required(ErrorMessage = "Select Institution Type")]
        public int institutionTypeId { get; set; }
        public string institutionTypeName { get; set; }

        //[Required(ErrorMessage = "Enter Doctor Name")]
        public string doctorName { get; set; }

        [Required(ErrorMessage = "Enter Treatment From")]
        public string treatmentFrom { get; set; }

        [Required(ErrorMessage = "Enter Treament To")]
        public string treatmentto { get; set; }
        [Required(ErrorMessage = "Enter Number Of Days For Rest")]
        public int NoOfDays { get; set; }
        [Required(ErrorMessage = "Enter Reason")]
        public string reason { get; set; }
        // public int institutionTypeId { get; set; }
        public string remarks { get; set; }
        public string appliedStatus { get; set; }
        public string requestDate { get; set; }
        public string appoinInstName { get; set; }

        [Required(ErrorMessage = "Select District")]
        public int healthUnitDistrictId { get; set; }
        public string healthUnitDistrictName { get; set; }

        [Required(ErrorMessage = "Enter Illness Detail")]
        public string illnessDetail { get; set; }

        public string transIp { get; set; }
        public int stepValue { get; set; }
        public int step { get; set; }
        public string requestKey { get; set; }
        public int Extstep { get; set; }

        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }

        public List<ILCModel> ILCModelList { get; set; }



        public string certGenerateByDoc { get; set; }
        public string certGenerateByDesig { get; set; }
        public string inspecttionCompeletionDate { get; set; }
        public string diseaseName { get; set; }
        public string identificationMark { get; set; }
        public string Area { get; set; }

        public string certificateNo { get; set; }
        public string appliedDate { get; set; }
        public int bedRest { get; set; }

        //[Required(ErrorMessage = "Upload Photo")]
        //public string extPhoto { get; set; }
        //public string extPhotoPath { get; set; }
        //[Required(ErrorMessage = "Upload Thumb Impression or Signature")]
        //public string thumbSign { get; set; }
        //public string thumbSignPath { get; set; }
        [Required(ErrorMessage = "Upload Id File")]
        public string idFile { get; set; }
        public string idFilePath { get; set; }
        [Required(ErrorMessage = "Select Id Type")]
        public int idTypeId { get; set; }
        public string idTypeName { get; set; }
        [Required(ErrorMessage = "Id Number Required!")]
        public string idNumber { get; set; }


        [Required(ErrorMessage = "OPD Receipt Number Required!")]
        public string extOpdReceiptno { get; set; }
        [Required(ErrorMessage = "Inspected Date Required!")]
        public string extInspectedDate { get; set; }
        [Required(ErrorMessage = "Opd File Required!")]
        public string extOpdFile { get; set; }
        public string extOpdFilePath { get; set; }
       // [Required(ErrorMessage = "Doctor Name Required!")]
        public string extDoctorName { get; set; }
        [Required(ErrorMessage = "Treatment From Date Required!")]
        public string extTreatmentFrom { get; set; }
        [Required(ErrorMessage = "Treatment To Date Required!")]
        public string extTreatmentto { get; set; }
        [Required(ErrorMessage = "No Of Days Required!")]
        public int extNoOfDays { get; set; }
        [Required(ErrorMessage = "Old Certificate Number Required!")]
        public string oldCertificateNumber { get; set; }

        [Required(ErrorMessage = "Mark Of Identification Required!")]
        public string markOfIdentification { get; set; }

        public string Msg { get; set; }

        public string XmlCheckList { get; set; }
        //[Required(ErrorMessage = "OPD File Required!")]
        public bool isNewOPDFile { get; set; }
        //[Required(ErrorMessage = "Photo Required!")]
        public bool extPhotoId { get; set; }
        public string appType { get; set; }
        public string extented { get; set; }
        public string officer { get; set; }
        public string HUAuthorisedPerson { get; set; }
        public string RejectRemark { get; set; }
        public int districtId { get; set; }
    }
    #region Riya
    public class ILCDetailsModel
    {
        public long regisIdILC { get; set; }
        public int TotalCount { get; set; }
        public int InProcessCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }

        public int institutionTypeId { get; set; }
        public string fullName { get; set; }
        public string requestDate { get; set; }
        public string fatherName { get; set; }
        public string dob { get; set; }
        public string appliedStatus { get; set; }

        public int regByUser { get; set; }
        public string transIp { get; set; }

        public string registrationNo { get; set; }
        [Required(ErrorMessage = "Remark Required!")]
        public string rejectedRemarks { get; set; }
        [Required(ErrorMessage = "Date Required!")]
        public string inspectionDate { get; set; }
        [Required(ErrorMessage = "Inspection Document Required!")]
        public string inspectionRpt { get; set; }
        public string inspectionRptPath { get; set; }
        [Required(ErrorMessage = "Feedback Required!")]
        public bool inspectionRptStatus { get; set; }
        [Required(ErrorMessage = "Remark Required!")]
        public string inspectionRejectedRemark { get; set; }
        public int appStatus { get; set; }
        List<ILCDetailsModel> lstILCDetails { get; set; }

        [Required(ErrorMessage = "Enter Doctor Name")]
        public string certGenerateByDoc { get; set; }
        [Required(ErrorMessage = "Enter Designation")]
        public string certGenerateByDesig { get; set; }
        [Required(ErrorMessage = "Enter Date")]
        public string inspecttionCompeletionDate { get; set; }
        [Required(ErrorMessage = "Enter Illness Detail")]
        public string illnessDetail { get; set; }
        [Required(ErrorMessage = "Enter Disease Name")]
        public string diseaseName { get; set; }
        [Required(ErrorMessage = "Enter Identification Mark")]
        public string identificationMark { get; set; }
        [Required(ErrorMessage = "Enter Days Of Bed Rest")]
        public int bedRest { get; set; }
        public string Area { get; set; }

        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }
        public string certificateFilePath { get; set; }
        public string appType { get; set; }
        public string opdFilePath { get; set; }

       // public string inspReportFilePath { get; set; }
        public string certificateGeneratedDate { get; set; }
        public string RejectDate { get; set; }
        public string RejectRemark { get; set; }

        public long forwardtoId { get; set; }
    }
    #endregion
}