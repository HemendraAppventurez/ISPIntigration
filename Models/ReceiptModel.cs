using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class ReceiptModel
    {
        public long regisIdNUH { get; set; }
        public Int64 regisID { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public string StateName { get; set; }
        public string ApplicationNumber { get; set; }
        public string RegistrationCirtficatNo { get; set; }
        public string ApplicantsName { get; set; }
        public string IssuanceDate { get; set; }
        public string ApplicationDate { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentAddress { get; set; }
        public string prevailingRegistrationNo { get; set; }
        public string ownerName { get; set; }
        public string OwnerFatherName { get; set; }
        public string OwnerMobileNo { get; set; }
        public string OwnerAge { get; set; }
        public string OwnerAddress { get; set; }
        public string pincode { get; set; }
        public List<ReceiptModel> ReceiptList { get; set; }
    }

    public class PairamedicalModel
    {
        public long regisIdNUH { get; set; }
      
        public long NuhId { get; set; }
        public Int64 regisID { get; set; }
        public bool isDeclared { get; set; }
        public long doctorId { get; set; }
        public string doctorName { get; set; }
        public string doctorFathersName { get; set; }
        public string doctorQualification { get; set; }
        public string NameofFoundation { get; set; }
        public string doctorregistrationNo { get; set; }
        public string doctorPart_FullTime { get; set; }

        public string doctorAge { get; set; }
        public string doctoraddress { get; set; }
        public string dyear { get; set; }
        public string dsignature { get; set; }

        public string transDate { get; set; }
        public string GetDate { get; set; }
        //public string ownerMobileNo { get; set;}
        public string establishmentArea { get; set; }
        public string establishmentPlace { get; set; }
        public string registrationNo { get; set; }
        public string staffpincode { get; set; }
        public string ownerMobileNo { get; set; }
        public string ownerDistrict { get; set; }
        public string ownerstate { get; set; }
        public string owneraddress { get; set; }
        public bool isRenewal { get; set; }
        public List<PairamedicalModel> List { get; set; }
        public List<PairamedicalModel> Paramedicallist { get; set; }
        //public List<PairamedicalModel> doclist { get; set; }
        public bool isParamedicaDeclared { get; set; }
        public DateTime ParamedicalDeclarationDate { get; set; }
        public string ParamedicalDeclarationIp { get; set; }
        public long ParamedicalDeclarationUserId { get; set; }
        public int isSuccess { get; set; }
        public string doctorlist { get; set; }
        //public string doclist { get; set; }
        public DateTime isDeclaredDate { get; set; }
        public long DeclarationUserId { get; set; }
        public string DeclarationIp { get; set; }
        public string message { get; set; }
    }
}