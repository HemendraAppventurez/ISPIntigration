using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.ReportModel
{
    public class rptNUHModel
    {
        public long regisIdNUH { get; set; }
        public string registrationNo { get; set; }
        public string address { get; set; }
        public string medicinSystemName { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }
        public string pinCode { get; set; }
        public string medicalFacilities { get; set; }
        public string applicantName { get; set; }
        public string qualification { get; set; }
        public string institution { get; set; }
        public string registrationNumber { get; set; }
        public string Central_StateCouncilName { get; set; }
        public string applicantMobileNo { get; set; }
        public string applicantEmailId { get; set; }
        public string medicalEstablishmentName { get; set; }
        public string EstablishmentCategoriesName { get; set; }
        public string fullName { get; set; }
        public string fatherName { get; set; }
        public int Age { get; set; }
    }
    public class rptCertificateNUHModel
    {
        public long regisIdNUH { get; set; }
        public string registrationNo { get; set; }
        public string Date { get; set; }
        public string ToDate { get; set; }
        public string EstablishmentName { get; set; }
        public string EstablishmentType { get; set; }
        public string ADDRESS { get; set; }
        public string operatedBy { get; set; }
        public string services { get; set; }
        public string Pname { get; set; }
        public string personInChrageAddress { get; set; }
        public string qualification { get; set; }
        public string institution { get; set; }
        public string registrationNumber { get; set; }
        public string Central_StateCouncilName { get; set; }
        public string certificateNo { get; set; }

        public string certificateFilePath { get; set; }

        public string StateName { get; set; }
        public string DistrictName { get; set; }
        public string pinCode { get; set; }
        public string operatedByName { get; set; }
        public string meeRegisNo {get;set;}
        public string certiGendBy { get; set; }
        public string certificateGeneratedDate { get; set; }
        public string Title { get; set; }
        public string applicantMobileNo { get; set; }
        public int districtid { get; set; }
        public string InPatientOutPatient { get; set; }
    }

    public class rptNHUChild
    {
        //public long regisIdNUH { get; set; }
        //public string staffName { get; set; }
        //public string staffqualification { get; set; }
        public string stafffatherName { get; set; }
        public string staffRegistrationType { get; set; }
        public string part_fullTime { get; set; }
        public long nursingDocumentId { get; set; }
        public long regisIdNUH { get; set; }
        public string staffName { get; set; }
        public string staffqualification { get; set; }
        public string staffinstitution { get; set; }
        public string staffRegistrationNo { get; set; }
        public string staffNameOfMCI_SMF { get; set; }
        public string filePath { get; set; }
    }

    public class rptNHUdocChild
    {
       public string doctorName { get; set; }
       public string doctorFathersName { get; set; }
       public string doctorQualification { get; set; }
       public string NameofFoundation { get; set; }
       public string doctorregistrationType { get; set; }
       public string doctorregistrationNo { get; set; }
       public string doctorPart_FullTime { get; set; }


       
    }
    public class rptNHUOwnerChild
    {
          public string ownerName { get; set; }
          public int ownerAge { get; set; }
          public string ownerFatherName { get; set; }
          public string ownerMobileNo { get; set; }
          public string ownerEmailId { get; set; }
          public string ownerAddress { get; set; }
          public int ownerStateId { get; set; }
          public string ownerStateName { get; set; }
          public int ownerDistrictId { get; set; }
          public string ownerDistrictName { get; set; }
          public string ownerPincode  { get; set; }



    }
}