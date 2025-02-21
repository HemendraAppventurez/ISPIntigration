using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class AGCModel
    {
        public long regisIdAGC { get; set; }
        public long regByUser { get; set; }
        public string registrationNo { get; set; }
        [Required(ErrorMessage = "Select Application Type")]
        public int applicantTypeId { get; set; }
        public string applicantTypeName { get; set; }
        [Required(ErrorMessage = "Select a Value")]
        public int applicantSubTypeId { get; set; }
        public string applicantSubTypeName { get; set; }
        [Required(ErrorMessage = "Enter Other")]
        public string applicantSubTypeOther { get; set; }
        [Required(ErrorMessage = "Order Detail Required")]
        public string orderDetail { get; set; }
        [Required(ErrorMessage = "Order Number Required")]
        public string orderNo { get; set; }
        [Required(ErrorMessage = "Order Date Required")]
        public string orderDate { get; set; }
        [Required(ErrorMessage = "Order File Required")]
        public string orderFile { get; set; }

        public string orderFilePath { get; set; }
        [Required(ErrorMessage = "Name Required")]
        public string fullName { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistence", "AGC", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Mobile Number Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string mobileNo { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistenceAGC", "AppRegistration", AdditionalFields = "idNumber", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Mobile No is Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string applicantMobileNo { get; set; }
        //[Required(ErrorMessage = "Email Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email !")]
        public string emailId { get; set; }
        [Required(ErrorMessage = "Select Gender")]
        public string gender { get; set; }
        [Required(ErrorMessage = "Address Required")]
        public string address { get; set; }
        [Required(ErrorMessage = "Select State")]
        public int stateId { get; set; }
        public string StateName { get; set; }
        [Required(ErrorMessage = "Select District")]
        public int districtId { get; set; }
        public string DistrictName { get; set; }
        [Required(ErrorMessage = "Pin Code Required")]
        [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
        public string pinCode { get; set; }
        [Required(ErrorMessage = "Select Id Type")]
        public int idTypeId { get; set; }
        public string idTypeName { get; set; }
        [Required(ErrorMessage = "Id Number Required")]
        public string idNumber { get; set; }
        [Required(ErrorMessage = "Document Required")]
        public string documentFile { get; set; }
        public string documentFilePath { get; set; }
        [Required(ErrorMessage = "Name Required")]
        public string susName { get; set; }
        [Required(ErrorMessage = "Father Name Required")]
        public string susFatherName { get; set; }
        [Required(ErrorMessage = "Mother Name Required")]
        public string susMotherName { get; set; }
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Age must be Number")]
        [Range(1, 100, ErrorMessage = "Age between 1 to 100")]        
        public int? susFatherAge { get; set; }
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Age must be Number")]
        [Range(1, 100, ErrorMessage = "Age between 1 to 100")]
        public int? susMotherAge { get; set; }
        //[Required(ErrorMessage = "Mobile No Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string susMobileNo { get; set; }
        //[Required(ErrorMessage = "Email Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email !")]
        public string susEmail { get; set; }
        [Required(ErrorMessage = "Address Required")]
        public string susaddress { get; set; }
        [Required(ErrorMessage = "Select State")]
        public int susstateId { get; set; }
        public string susStateName { get; set; }
        [Required(ErrorMessage = "Select District")]
        public int susdistrictId { get; set; }
        public string susDistrictName { get; set; }
        [Required(ErrorMessage = "Pin Code Required")]
        [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
        public string suspinCode { get; set; }
        [Required(ErrorMessage = "Mark Of Identification Required")]
        public string markOfIdentification { get; set; }
        [Required(ErrorMessage = "Photograph Required")]
        public string susphotoFile { get; set; }
        public string susphotoFilePath { get; set; }
        [Required(ErrorMessage = "Thumb Impression Required")]
        public string susThumbFile { get; set; }
        public string susThumbFilePath { get; set; }
        [Required(ErrorMessage = "Appointment Date Required")]
        public string appointmentDate { get; set; }
        public string regByDate { get; set; }
        public string regByIp { get; set; }
        public string transDate { get; set; }
        public string transIp { get; set; }
        public string appliedStatus { get; set; }
        public int appStatus { get; set; }
        public string appliedDate { get; set; }
        
        [Required(ErrorMessage = "Enter Remark")]
        public string rejectedRemarks { get; set; }
        [Required(ErrorMessage = "Select Committee")]
        public long committeeId { get; set; }
       
        [Required(ErrorMessage = "Select Inspection Date")]
        public string inspectionDate { get; set; }
        [Required(ErrorMessage = "Please Upload File")]
        public string inspectionRpt { get; set; }
        public string inspectionRptPath { get; set; }
        [Required(ErrorMessage = "Remark Required!")]
        public string inspectionRejectedRemark { get; set; }
        public decimal sancationAmount { get; set; }
        [Required(ErrorMessage = "Feedback Required!")]
        public bool inspectionRptStatus { get; set; }
        public string applicationDate { get; set; }
        public bool status { get; set; }
        public int Age { get; set; }
        public long certGenrBy { get; set; }
        public string requestKey { get; set; }
        public string certificateNo { get; set; }
        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }

        public string certificateFilePath { get; set; } 

        public string xmldatacheckList { get; set; }
        public int step { get; set; }
        public int stepValue { get; set; }
        public int UpdateStep { get; set; }

        public string xRayPlateNo { get; set; }
        public string dentalPlateNo { get; set; }

        public byte[] Photo { get; set; }
        public byte[] Thumb { get; set; }
        public long inprocessAGCId { get; set; }

        public string xRayDate { get; set; }
        public string certiGendBy { get; set; }
        public List<AGCModel> AGCModelList { get; set; }


        public string inspReportFilePath { get; set; }
        public string certificateGeneratedDate { get; set; }
        public string RejectDate { get; set; }
        public string RejectRemark { get; set; }
    }
    public class committeModel
{
    public string commMemName { get; set; }
    public string commMemDesig { get; set; }
    public string commMemDept { get; set; }
}
    public class AGCcommitteeInfo
    {
        public long regisIdAGC { get; set; }
        public string registrationNo { get; set; }
        public string fullName { get; set; }
        public string mobileNo { get; set; }
        public string emailId { get; set; }
        public int committeeId { get; set; }
        public string appliedStatus { get; set; }
        public int appStatus { get; set; }
        public string inspectionDate { get; set; }
    }
    public class AGCAppProcessModel
    {

        [Required(ErrorMessage = "Please Upload File")]
        public HttpPostedFileBase inspReportFilePhoto { get; set; }
        public string[] inspReportFilePhotoPath { get; set; }
        public string XmlDataPhoto { get; set; }


        public string fullName { get; set; }
        public string mobileNo { get; set; }
        public long regisIdAGC { get; set; }
        public string appliedStatus { get; set; }
        public int appStatus { get; set; }
        [Required(ErrorMessage = "Remark is Required")]
        public string rejectedRemarks { get; set; }
        [Required(ErrorMessage = "Select Committee")]
        public long committeeId { get; set; }
        [Required(ErrorMessage = "Age is Required")]
        public string Age { get; set; }
        [Required(ErrorMessage = "Select Inspection Date")]
        public string inspectionDate { get; set; }
        [Required(ErrorMessage = "Please Upload File")]
        public string inspectionRpt { get; set; }
        public string inspectionRptPath { get; set; }
       
        public string applicationDate { get; set; }
        public bool status { get; set; }
        public long userId { get; set; }
        public int forwardedType { get; set; }
        [Required(ErrorMessage = "Select Forwarded To")]
        public long forwardedTo { get; set; }
        public string registrationNo { get; set; }
        public string transIp { get; set; }
        public int rblDisbilityPer { get; set; }

        [Required(ErrorMessage = "x-Ray Plate No Required!")]
        public string xRayPlateNo { get; set; }
        [Required(ErrorMessage = "x-Ray Plate No Required!")]
        public string xRayDate { get; set; }
        [Required(ErrorMessage = "Dental Plate No Required!")]
        public string dentalPlateNo { get; set; }
        [Required(ErrorMessage = "Mark Of Identification Required!")]
        public string markOfIdentification { get; set; }

        public int susdistrictId { get; set; }
    }

    public class AGCReportModel
    {
        public long regisIdAGC { get; set; }
        public string registrationNo { get; set; }
        public string certiGendBy { get; set; }
        public long inprocessAGCId { get; set; }
        public string susName { get; set; }
        public string susFatherName { get; set; }
        public string appliedDate { get; set; }
        public string age { get; set; }
        public string DistrictName { get; set; }
        public int appStatus { get; set; }
        public string certificateNo { get; set; }
        public string susaddress { get; set; }
        public string usstateName { get; set; }
        public string susdistrictName { get; set; }
        public string suspinCode { get; set; }
        public string orderDetail { get; set; }
        public string orderNo { get; set; }
        public string xRayPlateNo { get; set; }
        public string dentalPlateNo { get; set; }
        public string markOfIdentification { get; set; }
        public string susphotoFile { get; set; }
        public string susThumbFile { get; set; }
        public string RayDate { get; set; }
        public string orderDate { get; set; }
        public string applicantSubTypeName { get; set; }
        public int susdistrictId { get; set; }
        public byte[] Photo { get; set; }
        public byte[] Thumb { get; set; }
        public string xRayDate { get; set; }
        public List<AGCReportModel> AGCReportModelList { get; set; }
    }
}