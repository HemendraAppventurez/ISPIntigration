using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class CHCModel
    {
        
            public long regisIdDEC { get; set; }
            public string registrationNo { get; set; }
            public long regByuser { get; set; }
            public long regByusers { get; set; }
            [Required(ErrorMessage = "Enter Name")]
            public string fullName { get; set; }
            [System.Web.Mvc.Remote("CheckMobileExistence", "DEC", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
            [Required(ErrorMessage = "Enter Mobile Number")]
            [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
            public string mobileNo { get; set; }
            [Required(ErrorMessage = "Enter Email")]
            [EmailAddress(ErrorMessage = "Enter valid Email !")]
            public string emailId { get; set; }

            [Required(ErrorMessage = "Enter Address")]
            public string address { get; set; }
            public int stateId { get; set; }
            public string StateName { get; set; }
            //[Required(ErrorMessage = "Select District")]
            public int districtid { get; set; }
            //public string DistrictName { get; set; }
            [Required(ErrorMessage = "Enter Pin Code")]
            [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid Pincode")]
            public string pinCode { get; set; }
            [Required(ErrorMessage = "Enter Relation")]
            public string relationWithDeathPerson { get; set; }
           // [Required(ErrorMessage = "Enter Health Unit")]
            public long forwardtypeId { get; set; }
           // public string forwardtypeName { get; set; }
            [Required(ErrorMessage = "select District")]
            public int healthUnitDistrictId { get; set; }
            public string healthUnitDistrictName { get; set; }
            //[Required(ErrorMessage = "Enter Area")]
            public long forwardtoId { get; set; }
          //  public string forwardtoName { get; set; }
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

            public bool isCauseCertified { get; set; }
            [Required(ErrorMessage = "Enter Cause")]
            public string diseaseNameOrCause { get; set; }
            [Required(ErrorMessage = "Please Confirm Information Provided By You")]
            public bool isInfoCorrect { get; set; }
            public string regBytransIp { get; set; }
            public string regBytransdate { get; set; }
            public string transDate { get; set; }
            public string transIp { get; set; }
            public string requestKey { get; set; }
            public string appliedStatus { get; set; }
            public string reqdate { get; set; }
            public List<CHCModel> DECModelList { get; set; }
       
    }

}