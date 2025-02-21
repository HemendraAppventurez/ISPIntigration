using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class Declaration
    {
        public long regisIdNUH { get; set; }
        public long NuhId { get; set; }
        public string ownerName { get; set; }
        public int ownerAge { get; set; }
        public string ownerFatherName { get; set; }
        public string ownerPincode { get; set; }
        public string ownerMobileNo { get; set; }
        public int operatedId { get; set; }
        public bool isBelongToMedical { get; set; }
        public string ownerAddress { get; set; }
        public string ownerSignature { get; set; }
        public string ownerDistrict { get; set; }

        public string ClinicPinCode { get; set; }
        public string establishmentArea { get; set; }
        public string ClinicDistrict { get; set; }
        public string ClinicAddress { get; set; } 
        public string transDate { get; set; }
        public string GetDate { get; set; }
        public Int64 regisID { get; set; }
        public List<Declaration> DeclarationList { get; set; }
        public List<Declaration> OwnerList { get; set; }
        public string establishmentPlace { get; set; }
        public string registrationNumber { get; set; }
        public string Central_StateCouncilName { get; set; }
        public string InchargeDiscrict { get; set; }
        public long doctorId { get; set; }
        public bool isDeclared { get; set; }
        public DateTime DeclarationDate { get; set; }
        public string DeclarationIp { get; set; }
        public long DeclarationUserId { get; set; }
        public int isSuccess { get; set; }
        public bool isRenewal { get; set; }
        public string landType { get; set; }
        public string establishmentName { get; set; }
        public string establishmentPlaceUnit { get; set; }

   }
}