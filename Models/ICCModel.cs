using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class ICCModel : ApplicantImmunization
    {
        public long regisIdICC { get; set; }
        public long regisByuser { get; set; }
        public string registrationNo { get; set; }
        public string transDate { get; set; }

        [Required(ErrorMessage = "Enter father Name")]
        public string fatherName { get; set; }
        [Required(ErrorMessage = "Enter mother Name")]
        public string motherName { get; set; }
        [Required(ErrorMessage = "Select Relation")]
        public string relation { get; set; }
       // [Required(ErrorMessage = "Enter Child Name")]
        public string fullName { get; set; }
        //[Required(ErrorMessage = "Enter Email Id")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email !")]
        public string emailId { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistence", "ICC", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Enter Mobile Number")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string mobileNo { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistenceICC", "AppRegistration", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Mobile No is Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string applicantMobileNo { get; set; }
        [Required(ErrorMessage = "Enter Date Of Birth")]
        public string dob { get; set; }
        [Required(ErrorMessage = "Enter Address")]
        public string address { get; set; }
        [Required(ErrorMessage = "Select State")]
        public int stateId { get; set; }
        public string StateName { get; set; }
        [Required(ErrorMessage = "Select District")]
        public int districtId { get; set; }
        public string DistrictName { get; set; }
        //[Required(ErrorMessage = "Enter Pin Code")]
        [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid PinCode")]
        public string pinCode { get; set; }
        public string regBytransIp { get; set; }
        public string transIP { get; set; }

        [Required(ErrorMessage = "Please Confirm Your Information")]
        public string isConfirm { get; set; }
        public string appliedStatus { get; set; }
        public string ApplicantImmunization { get; set; }
        [Required(ErrorMessage = "Please Upload Required Document")]
        public string immunizationBook { get; set; }
        public string immunizationBookpath { get; set; }
        [Required(ErrorMessage = "Please Upload Required Document")]
        public string immunizationBackSideBook { get; set; }
        public string immunizationBackSideBookpath { get; set; }
        public string XmlData { get; set; }
        public string XmlDataChecklist { get; set; }

        //public long vaccineIdICC { get; set; }
        //public long immuId { get; set; }
        //public string immuName { get; set; }
        //public int isExsistImmuName { get; set; }
        //public string dateOfImmunization { get; set; }
        //public string iscertified { get; set; }
        //public List<ICCModel> appImmunList { get; set; }

        public long forwardtypeId { get; set; }
        public string forwardtypeName { get; set; }

        [Required(ErrorMessage = "Select Area")]
        public long forwardtoId { get; set; }
        public string forwardtoName { get; set; }

        [Required(ErrorMessage = "Select District")]
        public int healthUnitDistrictId { get; set; }
        public string healthUnitDistrictName { get; set; }

        public string requestKey { get; set; }
        public string appliedDate { get; set; } 

        public int stepValue { get; set; }
        public int step { get; set; }
        public int UpdateStep { get; set; }

        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }


        public string hospitalName { get; set; }
        public string HUDistrict { get; set; }
        public string HUAuthorisedPerson { get; set; }
        public string officer { get; set; }
        public string RejectRemark { get; set; }
        public List<ICCModel> ICCModelList { get; set; }
    }
    #region Riya
    public class ApplicantImmunization
    {
        public long vaccineIdICC { get; set; }
        public long immuId { get; set; }
        public string immuName { get; set; }
        public int isExsistImmuName { get; set; }
        public string dateOfImmunization { get; set; }
        public string iscertified { get; set; }
        public List<ICCModel> appImmunList { get; set; }
        //Added by Muheeb 21/07/2018
        public string isGENERATED { get; set; }
        public string certificateNo { get; set; }
        public string certificateGeneratedDate { get; set; }
        //Added by Muheeb 21/07/2018

    }
    #endregion

    public class ICCDetailsModel
    {
        public long regisIdICC { get; set; }
        public long regByUser { get; set; }
        public string registrationNo { get; set; }
        public string appliedDate { get; set; } 
        public string fatherName { get; set; }
        public string relation { get; set; }
        public string fullName { get; set; }
        public string emailId { get; set; }
        public string mobileNo { get; set; }
        public string dob { get; set; }
        public string address { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }
        public string pinCode { get; set; }
        public string forwardtypeName { get; set; }
        public string forwardtoName { get; set; }
        public string healthUnitDistrictName { get; set; }
        public string appliedStatus { get; set; }
        public int appStatus { get; set; }
        public bool appStatus1 { get; set; }
        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }
        public string certificateFilePath { get; set; }

        public string certificateGeneratedDate { get; set; }
        public string RejectDate { get; set; }
        public string RejectRemark { get; set; } 

        public List<ICCDetailsModel> MLCDetailsList { get; set; }
    }

    public class ICCAppProcessModel
    {
        public long regisIdICC { get; set; }
        public int appStatus { get; set; }
        public string certificateFile { get; set; }
        public string certificateFilePath { get; set; }
        [Required(ErrorMessage = "Enter Remark")]
        public string rejectedRemarks { get; set; }
        public long userId { get; set; }
        public string transIp { get; set; }
        public string immunizationBook { get; set; }
        public string immunizationBackSideBook { get; set; }
        public long forwardtoId { get; set; }
    }
}