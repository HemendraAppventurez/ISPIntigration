using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace CCSHealthFamilyWelfareDept.Models
{

    #region abhijeet code

    public class FAPModel
    {
        public long regisIdFAP { get; set; }
        public long regisByuser { get; set; }
        public string registrationNo { get; set; }
        [Required(ErrorMessage = "Compensation Category is Required")]
        public int compensationCategoryId { get; set; }
        public string compensationCategoryName { get; set; }
        [Required(ErrorMessage = "Released date is Required")]
        public string dateofReleased { get; set; }
        [Required(ErrorMessage = "Death date is Required")]
        public string dateofDeath { get; set; }
        [Required(ErrorMessage = "Admitted date is Required")]
        public string admittedDate { get; set; }
        [Required(ErrorMessage = "Complications Details are Required")]
        public string complicationsDetails { get; set; }
        [Required(ErrorMessage = "Name of Health Unit is Required")]
        public string heathUnitName { get; set; }
        [Required(ErrorMessage = "Address  is Required")]
        public string heathUnitAddress { get; set; }
        
        public int stateId { get; set; }
        public string stateName { get; set; }
        [Required(ErrorMessage = "District is Required")]
        public int healthunitDistrictId { get; set; }
        public string healthunitDistrictName { get; set; }
        [Required(ErrorMessage = "Docter Name  is Required")]
        public string doctorName { get; set; }
        [Required(ErrorMessage = "Claimant's Name  is Required")]
        public string claimantName { get; set; }

        public string claimantDob { get; set; }
        [Required(ErrorMessage = "Claimant's Age is Required")]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Age must be Number")]
        [Range(18, 100, ErrorMessage = "Age between 1 to 100")]
        public int claimantAge { get; set; }
        [Required(ErrorMessage = "Relation with Sterilized Person is Required")]
        public int relationId { get; set; }
        public string relationName { get; set; }
        [Required(ErrorMessage = "Claimant's Address is Required")]
        public string claimantAddress { get; set; }
        [Required(ErrorMessage = "Claimant's Aadhaar is Required")]
        public string claimantAadhaarNo { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistence", "FAP", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Claimant's Mobile no. is Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string claimantMobileNo { get; set; }
        [System.Web.Mvc.Remote("CheckMobileExistenceFAP", "AppRegistration", HttpMethod = "POST", ErrorMessage = "Mobile Number already exists")]
        [Required(ErrorMessage = "Claimant's Mobile no. is Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        public string strMobileNo { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string sterilizedName { get; set; }

        public string sterilizedDob { get; set; }
        [Required(ErrorMessage = "Age is Required")]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Age must be Number")]
        [Range(1, 100, ErrorMessage = "Age between 1 to 100")]
        public int? sterilizedAge { get; set; }
        [Required(ErrorMessage = "Father's Name is Required")]
        public string fatherName { get; set; }
        [Required(ErrorMessage = "Spouse Name is Required")]
        public string spouseName { get; set; }

        [Required(ErrorMessage = "Spouse's Age is Required")]
        [RegularExpression("[0-9]{1,}", ErrorMessage = "Age must be Number")]
        [Range(1, 100, ErrorMessage = "Age between 1 to 100")]
        public int? spouseAge { get; set; }
        [Required(ErrorMessage = "Gender is Required")]
        public string sterilizedGender { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        public string sterilizedAddress { get; set; }

        [Required(ErrorMessage = "District  is Required")]
        public int sterlizedDistrictId { get; set; }
        public string sterlizedDistrictName { get; set; }
        
        public int employementId { get; set; }
        public string serviceType { get; set; }
        public string emplomentName { get; set; }
        [Required(ErrorMessage = "Operation Date is Required")]
        public string operationDate { get; set; }
        [Required(ErrorMessage = "Type Of Surgery is Required")]
        public int operationTypeId { get; set; }

        public string operationTypeName { get; set; }
        [Required(ErrorMessage = "Other Process is Required")]
        public string operationOtherprocess { get; set; }

        public bool isConfirm { get; set; }
        //[Required(ErrorMessage = "Claim Amount is Required")]

        //[RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Amount must be Number")]
        public decimal? claimAmount { get; set; }
        [Required(ErrorMessage = "Issue Reporting Date is Required")]
        public string informationDate { get; set; }
        [Required(ErrorMessage = "Name of Health Unit is Required")]
        public string sevakendraName { get; set; }
        [Required(ErrorMessage = "Confirmation Date of Failure is Required")]
        public string confirmationDate { get; set; }
        [Required(ErrorMessage = "Doctor's Name is Required")]
        public string sevadoctorName { get; set; }
        #region Riya
        [Required(ErrorMessage = "Bank Name is Required")]
        public string bankName { get; set; }
        [Required(ErrorMessage = "Account Holder Name is Required")]
        public string accountHolderName { get; set; }
        [Required(ErrorMessage = "Branch Name is Required")]
        public string branchName { get; set; }
        [Required(ErrorMessage = "Account No is Required")]
        public string accountNo { get; set; }
        [Required(ErrorMessage = "IFSC Code is Required")]
        public string ifscCode { get; set; }
        #endregion

        public string opdDistricthospitalfilePath { get; set; }
        public string usgfilePath { get; set; }
        public string uptfilePath { get; set; }
        public string antenatalcardfilePath { get; set; }
        public string affidavitfilePath { get; set; }
        public string sterilizationcertificatefilePath { get; set; }

        [Required(ErrorMessage = "Please Upload document")]
        public string opdDistricthospitalfile { get; set; }
        [Required(ErrorMessage = "Please Upload document")]
        public string usgfile { get; set; }
        [Required(ErrorMessage = "Please Upload document")]
        public string uptfile { get; set; }
        [Required(ErrorMessage = "Please Upload document")]
        public string antenatalcardfile { get; set; }
        //[Required(ErrorMessage = "Please Upload document")]
        public string affidavitfile { get; set; }
        [Required(ErrorMessage = "Please Upload document")]
        public string sterilizationcertificatefile { get; set; }

        //[Required(ErrorMessage = "Please Upload document")]
        public string deathCertificateFile { get; set; }
        public string deathCertificateFilePath { get; set; }

        [Required(ErrorMessage = "Please Upload document")]
        public string dischargeSummaryFile { get; set; }
        public string dischargeSummaryFilePath { get; set; }

        public string childName { get; set; }
        public int? childAge { get; set; }
        public string gender { get; set; }
        public string maritalStatus { get; set; }
        public string regBytransdate { get; set; }

        public bool isCertify { get; set; }
        public string appliedStatus { get; set; }
        public int appStatus { get; set; }
        public string requestDate { get; set; }
        public long forwardTo { get; set; }
        public string requestKey { get; set; }
        public List<FAPModel> FAPList { get; set; }
        public string xmldatacheckList { get; set; }
        public int step { get; set; }
        public int stepValue { get; set; }
        public int UpdateStep { get; set; }
        public string sanctiontransDate { get; set; }
        public string districtReportFilePath { get; set; }
        public string stateReportFilePath { get; set; }
        public string RejectDate { get; set; }
        public string RejectRemark { get; set; }



      
    }
    public class FAPAppProcessModel
    {
        //Vinod
        [Required(ErrorMessage = "Please Upload File")]
        public HttpPostedFileBase inspReportFilePhoto { get; set; }
        public string[] inspReportFilePhotoPath { get; set; }
        public string XmlDataPhoto { get; set; }


        public string registrationNo { get; set; }
        public string claimantMobileNo { get; set; }
        public long regisIdFAP { get; set; }
        public int appStatus { get; set; }
        public bool status { get; set; }
        [Required(ErrorMessage = "Select Committee")]
        public int committeeId { get; set; }

        public long forwardtypeId { get; set; }
        public string forwardtypeName { get; set; }
        [Required(ErrorMessage = "Please Select Forward to")]
        public long forwardtoId { get; set; }
        public string forwardtoName { get; set; }
        [Required(ErrorMessage = "Select Inspection Date")]
        public string inspectionDate { get; set; }
        [Required(ErrorMessage = "Please Upload File")]
        public string districtReportFile { get; set; }
        public string districtReportFilePath { get; set; }
        [Required(ErrorMessage = "Please Upload File")]
        public string stateReportFile { get; set; }
        public string stateReportFilePath { get; set; }
        [Required(ErrorMessage = "Remark is Required")]
        public string rejectedRemarks { get; set; }
        [Required(ErrorMessage = "Sanction Amount is Required")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Amount must be Number")]
        public decimal sanctionAmount { get; set; }
        [Required(ErrorMessage = "Sanction Date is Required")]
        public string sancationDate { get; set; }
        public long userId { get; set; }
        public string transIp { get; set; }

        public bool isverify { get; set; }

        public int districtId { get; set; } 
    }

    public class CompancationModel
    {
        public Int32 compensationCategoryId { get; set; }
        public String compensationCategoryName { get; set; }
        public Boolean isRequiredData { get; set; }
       
        public Decimal clameAmount { get; set; }
        public Decimal clameAmount2 { get; set; }
        public Int32 dayLimitForClameAmt { get; set; }
    }
    #endregion
}