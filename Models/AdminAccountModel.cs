using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    #region Class for AdminLoginModel
    public class AdminLoginModel
    {
        [Required(ErrorMessage = "Enter User ID !")]
        [Display(Name = "User ID")]
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

    public class DigitalSignatureModel
    {
        public long profileId { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Creator { get; set; }
        public string Producer { get; set; }
        public string Keywords { get; set; }
        public string Signaturepath { get; set; }
        public string signpwd { get; set; }
        public string SigReason { get; set; }
        public string SigContact { get; set; }
        public string SigLocation { get; set; }
    }

    public class RollPermissionModel
    {
        public int serviceId { get; set; }
        public long rollId { get; set; }

        public static List<RollPermissionModel> FillPermission()
        {
            DAL.Account_DB db=new DAL.Account_DB();
            return db.GetPermissionDetails();
        }
    }

}