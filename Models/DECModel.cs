using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class DECModel
    {
        public long regisIdDEC { get; set; }
        public string registrationNo { get; set; }
        public long regByuser { get; set; }
        public long regByusers { get; set; }
        [Required(ErrorMessage = "Enter Name")]
        public string fullName { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistenceDEC", "appRegistration", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Enter Mobile Number")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string mobileNo { get; set; }
        //[Required(ErrorMessage = "Enter Email")]
        [EmailAddress(ErrorMessage = "Enter valid Email !")]
        public string emailId { get; set; }

        [Required(ErrorMessage = "Enter Address")]
        public string address { get; set; }
        public int stateId { get; set; }
        public string StateName { get; set; }
        [Required(ErrorMessage = "Select District")]
        public int districtid { get; set; }
        public string DistrictName { get; set; }
        //[Required(ErrorMessage = "Enter Pin Code")]
        [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
        public string pinCode { get; set; }
        [Required(ErrorMessage = "Select Relation")]
        public int relationId { get; set; }
        //public string relationName { get; set; }
        //[Required(ErrorMessage = "Enter Relation")]
        //public string relationWithDeathPerson { get; set; }
        [Required(ErrorMessage = "Enter Health Unit")]
        public long forwardtypeId { get; set; }
        public string forwardtypeName { get; set; }
        [Required(ErrorMessage = "select District")]
        public int healthUnitDistrictId { get; set; }
        public string healthUnitDistrictName { get; set; }
        [Required(ErrorMessage = "Enter Area")]
        public long forwardtoId { get; set; }
        public string forwardtoName { get; set; }
        [Required(ErrorMessage = "Enter Name")]
        public string deathPersonName { get; set; }
        //[RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
        [RegularExpression("[0-9]{12}", ErrorMessage = "Invalid aadhar number")]
        public string aadhaarNo { get; set; }
        [Required(ErrorMessage = "Select Gender")]
        public string DeathPersonGender { get; set; }
        [Required(ErrorMessage = "Death Person's Marital Status")]
        public int maritalStatusId { get; set; }
        public string maritalStatusName { get; set; }
        [Required(ErrorMessage = "Select Religion")]
        public int religionId { get; set; }
        public string religionName { get; set; }
        [Required(ErrorMessage = "Enter Spouse Name")]
        public string spouseName { get; set; }
        [Required(ErrorMessage = "Enter Father Name")]
        public string fathersName { get; set; }
        [Required(ErrorMessage = "Enter Mother Name")]
        public string motherName { get; set; }
        [Required(ErrorMessage = "Enter Date of Death")]
        public string dod { get; set; }
        [Required(ErrorMessage = "Address Type Required")]
        public string addressType { get; set; }
        
        //public bool isCauseCertified { get; set; }
        //[Required(ErrorMessage = "Enter Cause")]
       // public string diseaseNameOrCause { get; set; }
        [Required(ErrorMessage = "Please Confirm Information Provided By You")]
        public bool isInfoCorrect { get; set; }
        public string regBytransIp { get; set; }
        public string regBytransdate { get; set; }
        public string transDate { get; set; }
        public string transIp { get; set; }
        public string requestKey { get; set; }
        public string appliedStatus { get; set; }


        public int TotalCount { get; set; }
        public int InProcessCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public int institutionTypeId { get; set; }
        public int stepValue { get; set; }
        public int step { get; set; }
        public int appStatus { get; set; }

        public string certificateNo { get; set; }
        public string certificateGeneratedDate { get; set; }
        public string certificateFilePath { get; set; }

        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }


        public int bloodRelationId { get; set; }
        public string bloodRelationName { get; set; }
        [Required(ErrorMessage = "Enter Name")]
        public string nomineeName { get; set; }
        [Required(ErrorMessage = "Enter Mobile Number")]
        public string NomineeMobileNo { get; set; }
        public int idProofId { get; set; }
        public string idProofFilePath { get; set; }
        [Required(ErrorMessage = "Upload Id Proof File")]
        public string idProofFile { get; set; }


        public string downloadType { get; set; }
        public Int64 regisIdNomineeDEC { get; set; }
        public int downloadCount { get; set; }
        public string idProofName { get; set; }
        public string relationName { get; set; }

        public string HUName { get; set; }
        public string HUDistrict { get; set; }
        public string HUAuthorisedPerson { get; set; }

        [Required(ErrorMessage = "Deceased Address Requied")]
        public string deathPersonAddress { get; set; }
        [Required(ErrorMessage = "Select Deceased State")]
        public int deathPersonStateId { get; set; }
        public string deathPersonStateName { get; set; }
        [Required(ErrorMessage = "Select Deceased District")]
        public int deathPersonDistrictId { get; set; }
        public string deathPersonDistrictName { get; set; }
        [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
        public string deathPersonPinCode { get; set; }
        public string officer { get; set; }
        public List<DECModel> DECModelList { get; set; }
    }

    public class DECotpModel
    {

        public Int64 regisIdNomineeDEC { get; set; }
        public Int64 UserId { get; set; }
        public string userName { get; set; }
        public string DisplayName { get; set; }
        public string password { get; set; }
        public long insertBy { get; set; }
        public string mobileNo { get; set; }


        public string Email { get; set; }
        public string Mobile { get; set; }
       
        [Required(ErrorMessage = "Enter OTP send on your Registered Mobile No.!")]
        public string Opt { get; set; }

        public bool IsMobileVarified { get; set; }
        public bool IsEmailVarified { get; set; }
       

        public bool IsEmailSend { get; set; }
        public bool IsMsgSend { get; set; }
        public bool IsMaxLimit { get; set; }
    }
}