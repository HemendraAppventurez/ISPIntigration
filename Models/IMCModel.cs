using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class IMCModel : IMCimmunizationModel
    {
        [Required(ErrorMessage = "Required!")]
        public bool isVaccined { get; set; }
        [Required(ErrorMessage = "OPD Reciept No Required!")]
        public string opdReciept { get; set; }
        //[Required(ErrorMessage = "Vaccine Date Required!")]
        //public string vaccineDate { get; set; }
        //[Required(ErrorMessage = "Vaccine Name Required!")]
        //public string vaccineOldName { get; set; }
        [Required(ErrorMessage = " OPD File Required!")]
        public string opdFile { get; set; }
        public string opdFilePath { get; set; }
        [Required(ErrorMessage = "Mark Of Identification is Required!")]
        public string markOfIdentification { get; set; }


        public string ImmunizationDate { get; set; }
        public long regisIdIMC { get; set; }
        public long regByUser { get; set; }
        public string registrationNo { get; set; }
        [Required(ErrorMessage = "Enter Name")]
        public string fullName { get; set; }
        [Required(ErrorMessage = "Enter Father Name")]
        public string fatherName { get; set; }
        [Required(ErrorMessage = "Enter Date of Birth")]
        public string dob { get; set; }
        [Required(ErrorMessage = "Enter Age")]
        public int age { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistence", "IMC", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Enter Mobile Number")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string mobileNo { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistenceIMC", "appRegistration", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Enter a valid 10 digit Mobile Number")]
        [Required(ErrorMessage = "Enter Mobile Number")]
        public string appmobileNo { get; set; }
        //[Required(ErrorMessage = "Enter Email Id")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter valid Email !")]
        public string emailId { get; set; }
        //[Required(ErrorMessage = "Enter Passport No.")]
        public string passportNo { get; set; }
        [Required(ErrorMessage = "Select State")]
        public int stateId { get; set; }
        public string StateName { get; set; }
        [Required(ErrorMessage = "Select District")]
        public int districtId { get; set; }
        public string DistrictName { get; set; }
        [Required(ErrorMessage = "Enter Address")]
        public string address { get; set; }
       // [Required(ErrorMessage = "Enter Pincode")]
        [RegularExpression("[0-9]{6}", ErrorMessage = "Invalid PinCode")]
        public string pinCode { get; set; }
        [Required(ErrorMessage = "Select Type of Immunization Certificate")]
        public int immuCertiTypeId { get; set; }
        public string immuCertiTypeName { get; set; }
        [Required(ErrorMessage = "Enter Reason For Application")]
        public string reason { get; set; }
        [Required(ErrorMessage = "Choose Yes or No")]
        public bool isAlreadyTaken { get; set; }

        [Required(ErrorMessage = "Select District")]
        public long forwardDistrictId { get; set; }
        public string forwardDistrictName { get; set; }
        [Required(ErrorMessage = "Select Area")]
        public long forwardtoId { get; set; }
        public string forwardtoName { get; set; }
        [Required(ErrorMessage = "Upload Relevant Proof")]
        public string DH_PHC_CHCProofFile { get; set; }
        public string DH_PHC_CHCProofFilePath { get; set; }
        [Required(ErrorMessage = "Enter Date Of Immunization")]
        public string DH_PHC_CHCDate { get; set; }
        [Required(ErrorMessage = "Enter Name Of Health Unit")]
        public string hospitalEstablishment { get; set; }
        //[Required(ErrorMessage = "Appoinment is Required")]
        //public string appointmentDate { get; set; }

        public int appStatus { get; set; }
        [Required(ErrorMessage = "Enter Health Unit")]
        public long forwardtypeId { get; set; }
        public string forwardtypeName { get; set; }

        public bool isCertify { get; set; }
        public string appliedStatus { get; set; }
        public string requestDate { get; set; }

        public bool status { get; set; }
        public string requestKey { get; set; }
        public int vaccineId { get; set; }
        public string vaccineName { get; set; }
        public int step { get; set; }
        public int stepValue { get; set; }
        public string isGENERATED { get; set; }

        public string certificateNo { get; set; }
        public string cerificateGeneratedate { get; set; }

        public long regisByuser { get; set; }
        public string transIp { get; set; }
        public string xmldata { get; set; }
        public string XmlDataChecklist { get; set; }
        public bool isUpload { get; set; }
        public string uploadCertificatePath { get; set; }
        public string certificateFilePath { get; set; }
        public string inspectionDate { get; set; }

        public string HUName { get; set; }
        public string HUDistrict { get; set; }
        public string HUAuthorisedPerson { get; set; }
         
        public string immunizationDoctor { get; set; }
        public string immunizationProofFile { get; set; }
        public string immunizationProofFilePath { get; set; }
        public int UpdateStep { get; set; }
        public string officer { get; set; }
         
        public string RejectDate { get; set; }
        public string RejectRemark { get; set; }

        public long forwardType { get; set; }

        public List<IMCModel> IMCModelList { get; set; }
    }
    #region Riya
    public class IMCimmunizationModel
    {
        //public long vaccineId { get; set; }
        //public long immuId { get; set; }
        public string vaccineName { get; set; }
        public string immunizationProof { get; set; }
        public string ImmunizationDate { get; set; }
        public string doctorName { get; set; }
        public string immunizationRemark { get; set; }
        public List<IMCModel> appImmunList { get; set; }
        public List<IMCimmunizationModel> appImmunizationList { get; set; }
        //public List<IMCAppProcessModel> appImmunizationtblList { get; set; }
        //public string isGENERATED { get; set; }
        //public string certificateNo { get; set; }
        //public string certificateGeneratedDate { get; set; }
        ////Added by Muheeb 21/07/2018

    }
    public class IMCimmunizationtblModel
    {
        public long vaccineId { get; set; }
     
        public string vaccineName { get; set; }
        public string immunizationProof { get; set; }
        public string ImmunizationDate { get; set; }
        public string doctorName { get; set; }
        public string immunizationRemark { get; set; }
        public List<IMCAppProcessModel> appImmunListtbl { get; set; }
       
       

    }
    #endregion
    public class IMCAppProcessModel : IMCimmunizationtblModel
    {
        public long regisIdIMC { get; set; }
        public bool isAlreadyTaken { get; set; }
        [Required(ErrorMessage = "Hospital/Establishmen is Required")]
        public string hospitalEstablishment { get; set; }
        [Required(ErrorMessage = "Please Upload file")]
        public string DH_PHC_CHCProofFile { get; set; }
        public string DH_PHC_CHCProofFilePath { get; set; }
        [Required(ErrorMessage = "Remark is Required")]
        public string rejectedRemarks { get; set; }
        [Required(ErrorMessage = "DH/PH/CHC date is Required")]
        public string DH_PHC_CHCDate { get; set; }
        public bool status { get; set; }
        public int appStatus { get; set; }
        public string appliedStatus { get; set; }
        public string ImmunXML { get; set; }

        public long forwardType { get; set; }

        public List<IMCAppProcessModel> updateIMCList { get; set; }

    }
}