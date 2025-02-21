using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Enter Name")]
        public string fullName { get; set; }

        [Required(ErrorMessage = "Enter Designation")]
        public string designation { get; set; }

        [Required(ErrorMessage = "Select Id Proof")]
        public int idProofId { get; set; }

        [Required(ErrorMessage = "Enter Id Proof No.")]
        public string idProofNo { get; set; }

        [System.Web.Mvc.Remote("CheckMobileExistence", "Account", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Enter a valid 10 digit Mobile Number")]
        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Enter Mobile Number")]
        public string mobileNo { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email")]
        [Display(Name = "Email ID")]
        [DataType(DataType.EmailAddress)]
        public string emailId { get; set; }

        [Required(ErrorMessage = "Select District")]
        public int districtId { get; set; }

        [Required(ErrorMessage = "Enter User Id")]
        [RegularExpression(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$&*]).{8,12})", ErrorMessage = "Enter atleast 1 lower case letter, 1 upper case letter, 1 digit and 1 special character and must not be less than 8 characters and more than 12 characters")]
        [System.Web.Mvc.Remote("CheckUserIdExistence", "UserManagement", HttpMethod = "POST", ErrorMessage = "User Id already exists")]
        public string userId { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$&*]).{8,16})", ErrorMessage = "Enter atleast 1 lower case letter, 1 upper case letter, 1 digit and 1 special character and must not be less than 8 characters and more than 16 characters")]
        public string password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Enter Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match")]
        public string confirmPassword { get; set; }

        public string transIp { get; set; }

        public long rollId { get; set; }
        public long insertBy { get; set; }

        public string msg { get; set; }
        public int queryExeFlag { get; set; }
        public string userName { get; set; }
    }

    public class PermissionModel
    {
        public int serviceId { get; set; }
        public string serviceName { get; set; }
        public string abbrServiceName { get; set; }
        public bool IsSrvProcessPer { get; set; }
        public bool IsSrvApplyPer { get; set; }
        public bool IsSrvReportPer { get; set; }
    }

    #region Class for UserProfileModel
    public class UserProfileModel
    {
        public Int64 profileId { get; set; }
        public Int64 rollId { get; set; }
        public string DistrictName { get; set; }
        public string fullName { get; set; }
        public string userName { get; set; }
        public string lastLoginDate { get; set; }
        public string changPasswordDate { get; set; }
    }
    #endregion

    #region Class for PasswordChangeModel
    public class PasswordModel
    {
        public string profileId { get; set; }
        public string UserName { get; set; }
        public Int64 UserId { get; set; }
        public Int64 RequestId { get; set; } 

        [Required(ErrorMessage = "Enter Password!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$&*]).{8,16})", ErrorMessage = "Enter atleast 1 lower case letter, 1 upper case letter, 1 digit and 1 special character and must not be less than 8 characters and more than 16 characters!")]
        public string newPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password!")]
        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage = "The password and confirmation password do not match!")]
        public string confirmPassword { get; set; }
        public string seed { get; set; }
         
        public string CancelUrl { get; set; }

        public string validationToken { get; set; }

        public string transIp { get; set; }

        public string ActionTo { get; set; }
    }
    #endregion

    public class CommitteeModel
    {
        public Int64 commMemId { get; set; }
        [Required(ErrorMessage = "Member Name Required!")]
        public string commMemName { get; set; }
        [Required(ErrorMessage = "Member Desigantion Required!")]
        public string commMemDesig { get; set; }
        [Required(ErrorMessage = "Member Department Required!")]
        public string commMemDept { get; set; }
        public bool isActive { get; set; }
        public string transIp { get; set; }
        public Int64 userId { get; set; }
        public List<CommitteeModel> AllCommitteeDetails { get; set; }

    }

    public class ManageAccountModel
    {
        public long userId { get; set; }
        public string userName { get; set; }
        public string DistrictName { get; set; }
        public string rollName { get; set; }
        public int failLoginCount { get; set; }
    }
}