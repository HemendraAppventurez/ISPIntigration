using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class MERModel
    {
        public long regisByuser { get; set; }
        public long regisIdMER { get; set; }
        public string registrationNo { get; set; }
        [Required(ErrorMessage = "Treatment is Required!")]
        public int treatmentId { get; set; }
        public string treatmentName { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string empfullName { get; set; }
        [Required(ErrorMessage = "Designation is Required!")]
        public string designation { get; set; }
        [Required(ErrorMessage = "Aadhaar No. is Required!")]
        [RegularExpression(@"^([0-9]{12})$", ErrorMessage = "Invalid Aadhar Number")]
        public string manavSampda_AadharNo { get; set; }
        [Required(ErrorMessage = "PPO Case is Required")]
        public string ppotreament { get; set; }
        [Required(ErrorMessage = "Father/Husband Name is Required!")]
        public string father_HusbandName { get; set; }
        [Required(ErrorMessage = "Office Name is Required!")]
        public string officeName { get; set; }
        [Required(ErrorMessage = "Office Incharge is Required!")]
        public string officeInchargeName { get; set; }
        [Required(ErrorMessage = "Department Name is Required!")]
        public string deptName { get; set; }
        [Required(ErrorMessage = "Basic Salary/Wages/Grade Scale is Required!")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Enter valid Basic Salary!")]
        public decimal basicSalary { get; set; }
        [Required(ErrorMessage = "Posting Address is Required!")]
        public string postingAddress { get; set; }
        [Required(ErrorMessage = "District is Required!")]
        public int postingDistrictId { get; set; }
        [Required(ErrorMessage = "Posting PinCode is Required!")]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Invalid PinCode Number")]
        public string postingPinCode { get; set; }
        public int postingStateId { get; set; }
        [Required(ErrorMessage = "Address is Required!")]
        public string permAddress { get; set; }
        [Required(ErrorMessage = "District is Required!")]
        public int permDistrictId { get; set; }
        public string DistrictName { get; set; }
        [Required(ErrorMessage = "PinCode is Required!")]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Invalid PinCode Number")]
        public string permPinCode { get; set; }
        [Required(ErrorMessage = "State is Required!")]
        public int permStateId { get; set; }
        public string StateName { get; set; }
        [Required(ErrorMessage = "Requesting Medical Reimbursement for is Required!")]
        public string patientType { get; set; }
        [Required(ErrorMessage = "Hospital Type is Required!")]
        public string hospitalType { get; set; }
        [Required(ErrorMessage = "Patient Name is Required!")]
        public string patientName { get; set; }
        [Required(ErrorMessage = "Age is Required")]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Age must be Number")]
        [Range(1, 100, ErrorMessage = "Age between 1 to 100")]
        public int patientage { get; set; }
        [Required(ErrorMessage = "Gender is Required")]
        public string patientgender { get; set; }
        [Required(ErrorMessage = "Relation with Employee is Required!")]
        public string patientrelationsWithEmployee { get; set; }
        //[Required(ErrorMessage = "Aadhar No is Required")]
        [RegularExpression(@"^([0-9]{12})$", ErrorMessage = "Invalid Aadhaar Number")]
        public string patientAadhaarNo { get; set; }
        [Required(ErrorMessage = "Disease Name is Required!")]
        public string patientdiseaseName { get; set; }
        [Required(ErrorMessage = "Treatment Period is Required!")]
        public string patienttreatmentFromDate { get; set; }
        [Required(ErrorMessage = "Treatment Period is Required!")]
        public string patienttreatmentToDate { get; set; }
        [Required(ErrorMessage = "Place where Disease Identified is Required!")]
        public string patientplaceOfDisease { get; set; }
        [Required(ErrorMessage = "Hospital Name is Required!")]
        public string patienthospitalName { get; set; }
        [Required(ErrorMessage = "Doctor Name is Required!")]
        public string patientdoctorName { get; set; }
        [Required(ErrorMessage = "Bank Name is Required!")]
        public string bankName { get; set; }
        [Required(ErrorMessage = "Branch Name is Required!")]
        public string branchName { get; set; }
        [Required(ErrorMessage = "Account No is Required!")]
        public string accountNo { get; set; }
        [Required(ErrorMessage = "IFSC Code is Required!")]
        public string ifscCode { get; set; }

        public string permanentDistrictName { get; set; }
        public string postingDistrictName { get; set; }
        public int? billId { get; set; }
        public string BillName { get; set; }
        public string billNo { get; set; }
        public string billdate { get; set; }
        public decimal? billAmount { get; set; }

        public string billFilePath { get; set; }
        public string xmlData { get; set; }
        public string appliedStatus { get; set; }
        public long certGenrBy { get; set; }
        public string requestDate { get; set; }

        [Required(ErrorMessage = "Date Of Birth is Required!")]
        public string dob { get; set; }
        [Required(ErrorMessage = "Gender is Required!")]
        public string gender { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistence", "MER", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Mobile Number Required!")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string mobileNo { get; set; }

        [System.Web.Mvc.Remote("CheckMobileExistenceMER", "appRegistration", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Mobile Number Required!")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string appMobile { get; set; }
        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }

        public int step { get; set; }
        public int stepValue { get; set; } 
        [Required(ErrorMessage = "Select Yes If Advance Already Taken!")]
        public string isAdvanceTaken { get; set; }

        [Required(ErrorMessage = "Amount is Required!")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Enter valid Amount!")]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Enter valid Amount!")]
        public decimal? advanceAmount { get; set; }
        [Required(ErrorMessage = "Letter No is Required!")]
        public string advanceLetterNo { get; set; }
        [Required(ErrorMessage = "Date is Required!")]
        public string advanceDate { get; set; }
        public string transIP { get; set; }
        [Required(ErrorMessage = "Retirement Field is Required ")]
        public bool isRetirement { get; set; }
        public decimal totalCalAmt{get;set;}
        public List<MERModel> MERModelList { get; set; }
        public string ForwardedToId { get; set; }
        public string ForwardedToIdDH { get; set; }
        public string ROLL { get; set; }
        public Int64 billidentity { get; set; }
        public string RejectRemark { get; set; }
        public string toOffice { get; set; }
        public long regByUser { get; set; }
        public decimal TotalBillAmount { get; set; }
    } 

    #region Riya
    public class MERstatusUpdationModel
    {
        //vk
        [Required(ErrorMessage = "Please Upload File")]
        public HttpPostedFileBase inspReportFilePhoto { get; set; }
        public string[] inspReportFilePhotoPath { get; set; }
        public string XmlDataPhoto { get; set; }


        public long regisIdMER { get; set; }
        public string registrationNo { get; set; }
        public string patientName { get; set; }
        public string patientrelationsWithEmployee { get; set; }
        public string patientdiseaseName { get; set; }
        public string appliedDate { get; set; }
        public string appliedStatus { get; set; }
        public string requestDate { get; set; }
        public int appStatus { get; set; }

        public int treatmentId { get; set; }
        public int isCertificateA { get; set; }
        

        [Required(ErrorMessage = "Remark Required!")]
        public string rejectedRemarks { get; set; }
        [Required(ErrorMessage = "Date Required!")]
        public string inspectionDate { get; set; }

       [Required(ErrorMessage = "Officer Name Required!")]
        public string officerName { get; set; }



        [Required(ErrorMessage = "Inspection report is required!")]
        public string inspectionRpt { get; set; }
        public string inspectionRptPath { get; set; }
        [Required(ErrorMessage = "Feedback Required!")]
        public bool inspectionRptStatus { get; set; }
        [Required(ErrorMessage = "Remark Required!")]
        public string inspectionRejectedRemark { get; set; }
        [Required(ErrorMessage = "Sanction Amount Required!")]
        //[Range(1, int.MaxValue, ErrorMessage = "Amount can not be 0")]
        //[RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})$", ErrorMessage = "Invalid price")]
        public decimal? sancationAmount { get; set; }
        [Required(ErrorMessage = "Sanction Date is Required!")]
        public string sanctionDate { get; set; }
        public long regByUser { get; set; }
        public string transIp { get; set; }
        public string mobileNo { get; set; }
        [Required(ErrorMessage = "Department Name is Required!")]
        public string departmentName { get; set; }
        [Required(ErrorMessage = "Officer Name Required!")]
        public string officersName { get; set; }
        [Required(ErrorMessage = "Letter Date Required!")]
        public string dateOfLetter { get; set; }
        [Required(ErrorMessage = "Letter No Required!")]
        public string letterNo { get; set; }
        public string CertificateNo { get; set; }
       
        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }
        public string certificateFilePath { get; set; }

        public string inspReportFilePath { get; set; }
        public string certificateGeneratedDate { get; set; }
        public string RejectDate { get; set; }
        public string RejectRemark { get; set; } 
        
        public string xmlBillSanction { get; set; }

        public int postingDistrictId { get; set; }
        public string ForwardedToId { get; set; }

        public List<MERstatusUpdationModel> lstMERDetails { get; set; }
    }
   
    public class MERrptModel
    {
        public long regisIdMER { get; set; }
        public long regByUser { get; set; }
        public string registrationNo { get; set; }
        public string deptName { get; set; }
        public string designation { get; set; }
        public int treatmentId { get; set; }
        public string patientName { get; set; }
        public string patientType { get; set; }
        public string AppPatientname { get; set; }
        public string patientrelationsWithEmployee { get; set; }
        public string father_HusbandName { get; set; }
        public string patienthospitalName { get; set; }
        public string patientdoctorName { get; set; }
        public decimal totalAmt { get; set; }
        public string patienttreatmentFromDate { get; set; }
        public string patienttreatmentToDate { get; set; }
        public string patientdiseaseName { get; set; }
        public string transDate { get; set; }
        public string officeName { get; set; }


        public string empfullName { get; set; }
        public string OfficerName { get; set; }
        public string OfficerDesignation { get; set; }


        public string cmoName { get; set; }
        [Required(ErrorMessage = "Department Name Required!")]
        public string departmentName { get; set; }
        [Required(ErrorMessage = "Officer Name Required!")]
        public string officersName { get; set; }
        [Required(ErrorMessage = "Date Of Letter Required!")]
        public string dateOfLetter { get; set; }
        [Required(ErrorMessage = "Letter No Required!")]
        public string letterNo { get; set; }
        public string certificateGeneratedDate { get; set; }
        
        public decimal sancationAmount{get;set;}
        public string AutoReferenceNo { get; set; }
        public string certificateNo { get; set; }


        public string gender { get; set; }
        public int empAge { get; set; }
        public decimal xRayAmt { get; set; }
        public decimal xRayAmount { get; set; }
        public string postingAddress { get; set; }
        public string patientgender { get; set; }
        public int patientage { get; set; }
        public string officeInchargeName { get; set; }
        public string hospitalType { get; set; }
        public string isAdvanceTaken { get; set; }
        public decimal advanceAmount { get; set; }
        public string advanceLetterNo { get; set; }
        public string advanceDate { get; set; }
        public string postingDistrictName { get; set; }
        public string postingPinCode { get; set; }

        public int postingDistrictId { get; set; }
        public string ForwardedToId { get; set; }
    }
    public class MERChildRptModel
    {
        public long Id { get; set; }
        public long regisByuser { get; set; }
        public long regisMERId { get; set; }
        public int billId { get; set; }
        public string billName { get; set; }
        public string billNo { get; set; }
        public string billdate { get; set; }
        public decimal billAmount { get; set; }
        public string billFilePath { get; set; }
    }
     #endregion

}