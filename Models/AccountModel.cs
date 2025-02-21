using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    #region Class for LoginModel
    public class LoginModel
    {
        [Required(ErrorMessage = "Enter Mobile Number !")]
        [Display(Name = "Mobile Number")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter Password!")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter Captcha!")]
        [Display(Name = "Captcha")]
        public string Captcha { get; set; }

        public string seed { get; set; }
    }
    #endregion



    public class MedicalRegistrationDetailsModel
    {

        public long regisIdNUH { get; set; }
        public long regisId { get; set; }
        public string registrationNo { get; set; }
        public long regByUser { get; set; }
        public string notarizedAffidavitFilePath { get; set; }
        public string establishmentName { get; set; }
        public int medicalEstablishmentId { get; set; }
        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }
        public string certificateFilePath { get; set; }
        public string certificateGeneratedDate { get; set; }
        public string inspReportFilePath { get; set; }
        public string RejectDate { get; set; }
        public string RejectRemark { get; set; }
        public int QueryFlag { get; set; }
        //public List<MedicalRegistrationDetailsModel> NUHDetailsList { get; set; }
    }

    public class NiveshMitraSendStatusModel
    {
        public string Control_ID { get; set; }
        public string Unit_Id { get; set; }
        public string ApplicationID { get; set; }
        public string Company_Name { get; set; }
        public string Industry_District { get; set; }
        public string Industry_District_Id { get; set; }
        public string ServiceID { get; set; }
        public string ProcessIndustryID { get; set; }
        public string StatusCode { get; set; }

        public int StepId { get; set; }

        public string ServiceStatus { get; set; }
        public string Remarks { get; set; }

        public string FeeAmount { get; set; }
        public string FeeStatus { get; set; }

        public string TransectionID { get; set; }
        public string TranSactionDate { get; set; }

        public string TransectionDateAndTime { get; set; }
        public string NocCertificateNumber { get; set; }

        public string NocUrl { get; set; }
        public string IsNocUrlActiveYesNo { get; set; }
        public string Passalt { get; set; }
        public DateTime SendDate { get; set; }
        public string ResStatus { get; set; }
        public string RequestId { get; set; }
        public string PendencyLevel { get; set; }
        public string ObjectRejectionCode { get; set; }

        public string IsCertificateValidLifeTime { get; set; }
        public string CertificateExpireDateDDMMYYYY { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public string D3 { get; set; }
        public string D4 { get; set; }
        public string D5 { get; set; }
        public string D6 { get; set; }
        public string D7 { get; set; }

        public string QueryExFlag { get; set; }

        public Int64 UserID { get; set; }
        public string UserName { get; set; }

        public string CMODistrictName { get; set; }
        public string ReasonName { get; set; }

        public string DesignationName { get; set; }

    }

    public class NiveshMitraUserDetailsModel
    {
        public Int64 UserID { get; set; }
        public Int64 profileId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string password { get; set; }
        public string RecFlag { get; set; }
        public Int64 insertBy { get; set; }
        public Int64 modifyBy { get; set; }
        public DateTime insertDate { get; set; }
        public DateTime? modifyDate { get; set; }
        public string emailId { get; set; }
        public string mobileNo { get; set; }

        public long RollID { get; set; }
        public DateTime LastLoginDate { get; set; }

        public string ControlID { get; set; }
        public string UnitId { get; set; }
        public string ServiceID { get; set; }
        public string RequestID { get; set; }
    }

    #region Class for NiveshMitra Registration Model

    public class NiveshMitraRegistrationModel
    {

        public string Control_ID { get; set; }
        public string Unit_Id { get; set; }
        public string Company_Name { get; set; }
        public string Industry_District { get; set; }
        public string Industry_District_Id { get; set; }
        public string Industry_Address { get; set; }
        public string Pin_Code { get; set; }
        public string Occupier_Name { get; set; }
        public string Occupier_Father_Mother_Name { get; set; }
        public string Occupier_Email_ID { get; set; }
        public string Occupier_Mobile_No { get; set; }
        public string Occupier_DOB { get; set; }
        public string Occupier_PAN { get; set; }
        public string Occupier_Gender { get; set; }
        public string Occupier_Address { get; set; }
        public string Occupier_Country_Id { get; set; }
        public string Occupier_State_ID { get; set; }
        public string Occupier_District_ID { get; set; }
        public string Occupier_District_Name { get; set; }
        public string Occupier_Pin_Code { get; set; }
        public string Nature_of_Activity { get; set; }
        public string Installed_Capacity { get; set; }
        public string Employees { get; set; }

        public string Nature_of_Operation { get; set; }
        public string Project_Cost { get; set; }
        public string Organization_Type_ID { get; set; }
        public string Organization_Type { get; set; }
        public string Industry_Type_ID { get; set; }
        public string Industry_Type_Name { get; set; }
        public string Expected_date_construction { get; set; }
        public string Project_Status { get; set; }
        public string Industry_Color { get; set; }
        public string Expected_date_production { get; set; }
        public string Unit_Category { get; set; }
        public string Items_Manufactured { get; set; }

        public string insertDate { get; set; }
        public string transIp { get; set; }
        public string isUpdated { get; set; }
        public string lastUpdatedDate { get; set; }
        public string sourceofRegistration { get; set; }
        public string ServiceID { get; set; }

        public string ProcessIndustryID { get; set; }
        public string passsalt { get; set; }

        public string RequestID { get; set; }
        public string ApplicationID { get; set; }



        public string Annual_Turnover { get; set; }
        public string Occupier_First_Name { get; set; }
        public string Occupier_Middle_Name { get; set; }
        public string Occupier_Last_Name { get; set; }


        public long profileId { get; set; }

        public string fullName { get; set; }
        public string fatherName { get; set; }
        public string Password { get; set; }
        public string confirmPassword { get; set; }

        public string NtransIp { get; set; }

        public string msg { get; set; }

        public Int32 queryExeFlag { get; set; }

        public string userName { get; set; }

        public string seed { get; set; }

        public DateTime DTDob { get; set; }
        public int categoryId { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }

        public string requestKey { get; set; }

        public long regisIdNUH { get; set; }
    }
    #endregion

    #region Class for RegistrationModel
    public class RegistrationModel
    {
        public long profileId { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Enter Full Name !")]
        public string fullName { get; set; }

        [Display(Name = "Father's Name")]
        [Required(ErrorMessage = "Enter Father's Name !")]
        public string fatherName { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Enter Date of Birth !")]
        public string dob { get; set; }
        public DateTime DTDob { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Select Category !")]
        public string categoryId { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Select Gender !")]
        public string gender { get; set; }

        [System.Web.Mvc.Remote("CheckMobileExistence", "Account", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists !")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Enter a valid 10 digit Mobile Number!")]
        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Enter Mobile Number !")]
        public string mobileNo { get; set; }

        //[System.Web.Mvc.Remote("CheckEmailExistence", "Account", HttpMethod = "POST", ErrorMessage = "Email Address already exists !")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email !")]
        [Display(Name = "Email ID")]
        [DataType(DataType.EmailAddress)]
        //[Required(ErrorMessage = "Enter Email Address!")]
        public string emailId { get; set; }

        [Required(ErrorMessage = "Enter Password !")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$&*]).{8,16})", ErrorMessage = "Enter atleast 1 lower case letter, 1 upper case letter, 1 digit and 1 special character and must not be less than 8 characters and more than 16 characters!")]
        public string password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Enter Confirm Password !")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match!")]
        public string confirmPassword { get; set; }

        public string transIp { get; set; }

        public string msg { get; set; }
        public int queryExeFlag { get; set; }
        public string userName { get; set; }

        public string seed { get; set; }

        public string requestKey { get; set; }
    }
    #endregion

    #region Class for UserDetailsModel
    public class UserDetailsModel
    {
        public Int64 UserId { get; set; }
        public Int64 profileId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string password { get; set; }
        public string RecFlag { get; set; }
        public Int64 insertBy { get; set; }
        public Int64 modifyBy { get; set; }
        public DateTime insertDate { get; set; }
        public DateTime? modifyDate { get; set; }
        public string emailId { get; set; }
        public string mobileNo { get; set; }
        public bool? IsReset { get; set; }
        public string ImageUrl { get; set; }
        public Int32 IsMobileVarified { get; set; }
        public Int32 IsEmailVarified { get; set; }
        public Int32 IsResetByAdmin { get; set; }
        public Int32 isFirstLogin { get; set; }
        public Int32 isForceChange { get; set; }
        public string Status { get; set; }
        public int FailLoginCount { get; set; }
        public Int32 Flag { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string Msg { get; set; }
        public DateTime? ChangePasswordDate { get; set; }
        public long rollId { get; set; }

        public bool isLock { get; set; }
        public string UserStatus { get; set; }
        public string profilePicPath { get; set; }

        public string fatherName { get; set; }
        public string designation { get; set; }

        public string DistrictName { get; set; }
        public string DistrictNameHi { get; set; }
        public string rollName { get; set; }
        public string rollAbbrName { get; set; }
        public string rollAbbrNameHi { get; set; }
        public int districtId { get; set; }
        public string categoryName { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string idProofName { get; set; }
        public string idProofNo { get; set; }
        public int zoneId { get; set; }
        public bool NiveshMitraFlag { get; set; }

        public List<UserDetailsModel> AllUserDetails { get; set; }
        public List<PermissionModel> PermissionList { get; set; }
    }
    #endregion

    #region Class for userPolicyModel
    public class userPolicyModel
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        public Int64 UserId { get; set; }
        [Required(ErrorMessage = "Enter OTP send on your Registered Mobile No.!")]
        public string Opt { get; set; }

        public bool IsMobileVarified { get; set; }
        public bool IsEmailVarified { get; set; }
        public string Displayname { get; set; }

        public bool IsEmailSend { get; set; }
        public bool IsMsgSend { get; set; }
        public bool IsMaxLimit { get; set; }
    }
    #endregion

    #region Class for ForgotPasswordModel
    public class ForgotPasswordModel
    {
        public int flag { get; set; }
        public Int64 UserId { get; set; }

        [Required(ErrorMessage = "Enter User Name !")]
        public string UserName { get; set; }

        public string ResetBy { get; set; }

        [Display(Name = "New password")]
        [Required(ErrorMessage = "Enter New password !")]
        [RegularExpression(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$&*]).{8,16})", ErrorMessage = "Enter atleast 1 lower case letter, 1 upper case letter, 1 digit and 1 special character and must not be less than 8 characters and more than 16 characters!")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm New Password!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The new password and confirmation password did not match!")]
        public string cPassword { get; set; }

        public DateTime ChangePasswordDate { get; set; }

        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Enter Mobile Number !")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Enter a valid 10 digit Mobile No.!")]
        public string MobileNo { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Enter Email Address !")]
        [EmailAddress(ErrorMessage = "Enter a valid Email Address !")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Captcha Required !")]
        [Display(Name = "Captcha")]
        public string Captcha { get; set; }

        public Int64 RandonNo { get; set; }
        public bool IsMobileVarified { get; set; }
        public bool IsEmailVarified { get; set; }
        public bool IsReset { get; set; }
        public bool IsResetByAdmin { get; set; }
        public IEnumerable<ForgotPasswordModel> UserInfoDetail { get; set; }
        public int status { get; set; }

        [Display(Name = "OTP (One Time password)")]
        [Required(ErrorMessage = "Enter OTP  !")]
        public Int64? otp { get; set; }
        public int otpCount { get; set; }
        public string seed { get; set; }
        public DateTime? OTPSendDate { get; set; }
        public string isResend { get; set; }
    }
    #endregion

    #region Class for PasswordChangeModel
    public class PasswordChangeModel
    {

        public string UserName { get; set; }
        public Int64 UserId { get; set; }
        public Int64 RequestId { get; set; }
        [Required(ErrorMessage = "Enter Old Password!")]
        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        public string oldPassword { get; set; }

        [Required(ErrorMessage = "Enter New Password!")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$&*]).{8,16})", ErrorMessage = "Enter atleast 1 lower case letter, 1 upper case letter, 1 digit and 1 special character and must not be less than 8 characters and more than 16 characters!")]
        public string newPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm New Password!")]
        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage = "The new password and confirmation password do not match!")]
        public string confirmPassword { get; set; }
        public string seed { get; set; }

        [Required(ErrorMessage = "Captch Required !")]
        [Display(Name = "Captcha")]
        public string Captcha { get; set; }

        public string CancelUrl { get; set; }

        public string validationToken { get; set; }

        public string transIp { get; set; }
    }
    #endregion

    #region Class for MyAccountModel
    public class MyAccountModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Enter Name !")]
        public string userName { get; set; }

        //[System.Web.Mvc.Remote("CheckMobileExistence", "Account", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists !")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Enter a valid 10 digit Mobile Number!")]
        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Enter Mobile Number !")]
        public string mobileNo { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email !")]
        [Display(Name = "Email ID")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Enter Email Id!")]
        public string emailId { get; set; }

        [Display(Name = "Father's / Husband's Name")]
        [Required(ErrorMessage = "Enter Father's / Husband's Name !")]
        public string fatherName { get; set; }

        [Display(Name = "Designation")]
        [Required(ErrorMessage = "Enter Designation !")]
        public string designation { get; set; }

        public string seed { get; set; }

        public string transIp { get; set; }
        public string profilePicName { get; set; }
        public string profilePicPath { get; set; }

        public long userId { get; set; }
        public long profileId { get; set; }
        public string validationToken { get; set; }
    }
    #endregion

    #region Query Execute Class
    public class QueryExecuteModel
    {
        public string queryExe { get; set; } // I,AE,E
        public string msgText { get; set; }//msg
        public string msgStatus { get; set; } // 
        public string returnValue { get; set; }
    }
    #endregion

    #region Class For ServiceModel
    public class ServiceModel
    {
        public Int64 sendResponseId { get; set; }
        public string requestKey { get; set; }
        public string deptRegistrationId { get; set; }
        public string serviceResponse { get; set; }
        public string applicationNumber { get; set; }
        public string serviceCode { get; set; }
        public string applicationType { get; set; }
        public string transIp { get; set; }
    }
    #endregion
}