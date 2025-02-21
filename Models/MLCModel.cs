using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class MLCModel
    {
        public long regisIdMLC { get; set; }
        public long regByUser { get; set; }
        public string registrationNo { get; set; }
        [Required(ErrorMessage = "Select Patient Brought By")]
        public string patientBroughtBy { get; set; }
        [Required(ErrorMessage = "Select Brought By Person Relation")]
        public string broughtByPersonrelation { get; set; }
        [Required(ErrorMessage = "Requester Name Required")]
        public string fullName { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistence", "MLC", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Requester Mobile Number Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string mobileNo { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistenceMLC", "appRegistration", AdditionalFields = "idNo", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Requester Mobile Number Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string appmobileNo { get; set; }
        //[Required(ErrorMessage = "Requester Email Id Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email !")]
        public string emailId { get; set; }
        [Required(ErrorMessage = "Id Number Required")]
        public string idNo { get; set; }
        [Required(ErrorMessage = "FIR Number Required")]
        public string FIRno { get; set; }
        
        [Required(ErrorMessage = "Enter Address")]
        public string broughtByaddress { get; set; }
        //    [Required(ErrorMessage = "Adhar Number Required")]
        //    [RegularExpression(@"^([0-9]{12})$", ErrorMessage = "Invalid Aadhar Number")]
        //public string aadhaarNo {get; set;}
        [Required(ErrorMessage = "Patient Name Required")]
        public string patientName { get; set; }
        [Required(ErrorMessage = "Patient Father Name Required")]
        public string patientFatherName { get; set; }
        [Required(ErrorMessage = "Select Relation")]
        public string relation { get; set; }
        //[Required(ErrorMessage = "Relative Name Required")]
        //public string relativeName { get; set; }
        [Required(ErrorMessage = "Age Required")]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Age must be Number")]
        [Range(1, 100, ErrorMessage = "Age between 1 to 100")]
        public int? age { get; set; }
        [Required(ErrorMessage = "Select Gender")]
        public string patientGender { get; set; }
        //[Required(ErrorMessage = "Occupation Required")]
        public string occupation { get; set; }
        [Required(ErrorMessage = "Select State")]
        public int stateId { get; set; }
        public string stateName { get; set; }
        [Required(ErrorMessage = "Select District")]
        public int districtId { get; set; }
        public string DistrictName { get; set; }
        [Required(ErrorMessage = "Address Required")]
        public string address { get; set; }
        [Required(ErrorMessage = "Select State")]
        public int broughtBystateId { get; set; }
        public string broughtBystateName { get; set; }
        [Required(ErrorMessage = "Select District")]
        public int broughtBydistrictId { get; set; }
        public string broughtBydistrictName { get; set; }
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Invalid Pin Code")]
        //[Required(ErrorMessage = "Enter Pin Code")]
        public string pinCode { get; set; }
        //[Required(ErrorMessage = "Select Tehsil")]
        public int tehsilId { get; set; }
        public string tehsilName { get; set; }
        [Required(ErrorMessage = "Road Name Required")]
        public string areaRoadName { get; set; }
        [Required(ErrorMessage = "Police Station Required")]
        public string policeStation { get; set; }
        public string regBytransIp { get; set; }
        public string transIp { get; set; }
        public string appliedStatus { get; set; }
        [Required(ErrorMessage = "Doctor Name Required")]
        public string doctorName { get; set; }
        [Required(ErrorMessage = "Designation Required")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "Seniority No Required")]
        public string seniorityNo { get; set; }
        
        public int? restDays { get; set; }
        
        public string enquiryDetails { get; set; }

        public string requestKey { get; set; }

        //added by Muheeb
        public string reqdate { get; set; }
        public long forwardtypeId { get; set; }
        public string forwardtypeName { get; set; }

        [Required(ErrorMessage = "Select Area")]
        public long forwardtoId { get; set; }
        public string forwardtoName { get; set; }

        [Required(ErrorMessage = "Select District")]
        public int healthUnitDistrictId { get; set; }
        public string healthUnitDistrictName { get; set; }
        //added by Muheeb

        [Required(ErrorMessage = "Id Type Required")]
        public int idTypeId { get; set; }
        public string idTypeName { get; set; }
        [Required(ErrorMessage = "Id Number Required")]
        public string idNumber { get; set; }
        [Required(ErrorMessage = "Details Required")]
        public string details { get; set; }

        public string appliedDate { get; set; }

        public string broughtByDesignation { get; set; }

        public int step { get; set; }
        public int stepValue { get; set; }
        public string xmldata { get; set; }

        [Required(ErrorMessage = "Select Medico-Legal Type")]
        public string medicoLegalType { get; set; }
        [Required(ErrorMessage = "OPD Number Required")]
        public string OPDNumber { get; set; }
        [Required(ErrorMessage = "Medico-Legal Date Required")]
        public string MLCDate { get; set; }
        [Required(ErrorMessage = "Medico-Legal Time Required")]
        public string MLCtime { get; set; }

        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }
        [Required(ErrorMessage = "Mark Of Identification Required")]
        public string markOfIdentification { get; set; }
        public string officer { get; set; }
        public List<MLCModel> MLCModelList { get; set; }
    }

    public class MLCDetailsModel
    {
        public long regisIdMLC { get; set; }
        public long regByUser { get; set; }
        public string registrationNo { get; set; }
        public string appliedDate { get; set; }
        public string patientBroughtBy { get; set; }
        public string fullName { get; set; }
        public string mobileNo { get; set; }
        public string emailId { get; set; }
        public string idNo { get; set; }
        public string aadhaarNo { get; set; } 
        public string broughtByaddress { get; set; }
        public string broughtBystateName { get; set; }
        public string broughtBydistrictName { get; set; }
        public string pinCode { get; set; }
        public string patientName { get; set; }
        public int? age { get; set; }
        public string patientGender { get; set; }
        public string occupation { get; set; } 
        public string DistrictName { get; set; }
        public string address { get; set; }
        public string tehsilName { get; set; }
        public string areaRoadName { get; set; }
        public string policeStation { get; set; }
        public string idNumber { get; set; }
        public string idTypeName { get; set; }
        public string details { get; set; }
        public string forwardtypeName { get; set; }
        public string forwardtoName { get; set; }
        public string healthUnitDistrictName { get; set; } 
        public string appliedStatus { get; set; }
        public int appStatus { get; set; }
        public bool appStatus1 { get; set; }
        public string inspectionDate { get; set; }

        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }
        public string certificateFilePath { get; set; }

        public string MLCDate { get; set; }

        public int downloadedCertiCount { get; set; }
        public string officer { get; set; }
        public string certificateGeneratedDate { get; set; }
        public List<MLCDetailsModel> MLCDetailsList { get; set; }
    }

    public class MLCAppProcessModel
    {
        public long regisIdMLC { get; set; }
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
        public long userId { get; set; }
        public string transIp { get; set; }
        public string XmlData { get; set; }
        [Required(ErrorMessage = "Treatment From Date Required!")]
        public string treatmentFrom { get; set; }
        [Required(ErrorMessage = "Treatment To Date Required!")]
        public string treatmentto { get; set; }
        [Required(ErrorMessage = "Days For Rest Required!")]
        public int restdays { get; set; }
        public int restFor { get; set; }


        public string registrationNo  { get; set; }
        public string patientName  { get; set; }
        public string patientFatherName { get; set; }
        public string patientBroughtBy { get; set; }
        public string dtl { get; set; }
        public int age  { get; set; }
        public string patientGender { get; set; }
        public string fullName  { get; set; }
        public string broughtByDesignation  { get; set; }
        public string certificateNo { get; set; }
        public string certificateGeneratedDate { get; set; }
        public string doctorName { get; set; }
        public string seniorityNo { get; set; }
        public string MLCDate { get; set; }
        public string MLCtime { get; set; }
        public string OPDNumber { get; set; }
        public string forwardtypeName { get; set; }

        public string HUName { get; set; }
        public string HUDistrict { get; set; }
        public string HUAuthorisedPerson { get; set; }
        public string markOfIdentification { get; set; }

        public long forwardtoId { get; set; }
    }

    public class rptMLCChild
    {
        public long EnquiryID  { get; set; }
        public long regisId  { get; set; }
        public long regisByuser  { get; set; }
        public string EnquiryDetails { get; set; }
    }

    public class MLCNomineeModel
    {
        public long regisIdMLC { get; set; }
        public long nomineeId { get; set; }
        public string nomineeName { get; set; }
        public string mobileNumber { get; set; }
        public int idProof { get; set; }
        [Required(ErrorMessage = "Upload Id File")]
        public string idProofFile { get; set; }
        public string idProofFilePath { get; set; }
        public string transIp { get; set; }

        public string idProofName { get; set; }
        public long downloadedBy { get; set; }
        public int downloadStep { get; set; }
        [Required(ErrorMessage = "Enter OTP send on your Registered Mobile No.!")]
        public string OTP { get; set; }
        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }
        public string downloadType { get; set; }
        public string downloadedDate { get; set; }
        public string registrationNo { get; set; }

        public List<MLCNomineeModel> MLCNomineeList { get; set; }
    }
}