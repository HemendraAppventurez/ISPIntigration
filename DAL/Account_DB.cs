using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCSHealthFamilyWelfareDept.Models;
using System.Data;
using System.Reflection;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class Account_DB : DbContext
    {
        #region Default Constructor
        public Account_DB()
            : base("CMSModule")
        { }
        #endregion


        #region Method Nivesh Mitra User registration

        public List<NiveshMitraRegistrationModel> NiveshMitraRegistration(NiveshMitraRegistrationModel model)
        {
            var sqlParams = new SqlParameter[] { 
                
                new SqlParameter { ParameterName = "@ControlId", Value =model.Control_ID },
                new SqlParameter { ParameterName = "@UnitId", Value =model.Unit_Id },
                new SqlParameter { ParameterName = "@CompanyName", Value =model.Company_Name },
                new SqlParameter { ParameterName = "@IndustryDistrict", Value =model.Industry_District },
                new SqlParameter { ParameterName = "@IndustryDistrictID", Value =model.Industry_District_Id }, 
                new SqlParameter { ParameterName = "@Industry_Address", Value =model.Industry_Address }, 
                 new SqlParameter { ParameterName = "@Pin_Code", Value =model.Pin_Code },
                new SqlParameter { ParameterName = "@Occupier_Name", Value =model.Occupier_Name },
                new SqlParameter { ParameterName = "@Occupier_Father_Mother_Name", Value =model.Occupier_Father_Mother_Name??string.Empty },
                new SqlParameter { ParameterName = "@Occupier_Email_ID", Value =model.Occupier_Email_ID },
                new SqlParameter { ParameterName = "@Occupier_Mobile_No", Value =model.@Occupier_Mobile_No },
                new SqlParameter { ParameterName = "@Occupier_PAN", Value =model.Occupier_PAN },
                new SqlParameter { ParameterName = "@Occupier_Address", Value =model.Occupier_Address },


                new SqlParameter { ParameterName = "@Occupier_Country_Id", Value =model.Occupier_Country_Id },
                new SqlParameter { ParameterName = "@Occupier_State_ID", Value =model.Occupier_State_ID },
                new SqlParameter { ParameterName = "@Occupier_District_ID", Value =model.Occupier_District_ID },
                new SqlParameter { ParameterName = "@Occupier_District_Name", Value =model.Occupier_District_Name },
                new SqlParameter { ParameterName = "@Occupier_Pin_Code", Value =model.Occupier_Pin_Code },
                new SqlParameter { ParameterName = "@Nature_of_Activity", Value =model.Nature_of_Activity },
                new SqlParameter { ParameterName = "@Installed_Capacity", Value =model.Installed_Capacity??string.Empty },
                new SqlParameter { ParameterName = "@Employees", Value =model.Employees },
                new SqlParameter { ParameterName = "@Nature_of_Operation", Value =model.Nature_of_Operation },
                new SqlParameter { ParameterName = "@Project_Cost", Value =model.Project_Cost },


                new SqlParameter { ParameterName = "@Organization_Type_ID", Value =model.Organization_Type_ID },
                new SqlParameter { ParameterName = "@Organization_Type", Value =model.Organization_Type },
                new SqlParameter { ParameterName = "@Industry_Type_ID", Value =model.Industry_Type_ID },
                new SqlParameter { ParameterName = "@Industry_Type_Name", Value =model.Industry_Type_Name },
                new SqlParameter { ParameterName = "@Expected_date_construction", Value =model.Expected_date_construction },
                new SqlParameter { ParameterName = "@Project_Status", Value =model.Project_Status }, 
                new SqlParameter { ParameterName = "@Industry_Color", Value =model.Industry_Color??string.Empty },
                new SqlParameter { ParameterName = "@Expected_date_production", Value =model.Expected_date_production },
                new SqlParameter { ParameterName = "@Unit_Category", Value =model.Unit_Category },
                new SqlParameter { ParameterName = "@Items_Manufactured", Value =model.Items_Manufactured },

                new SqlParameter { ParameterName = "@isUpdated", Value =model.isUpdated??string.Empty},
                new SqlParameter { ParameterName = "@lastUpdatedDate", Value =model.lastUpdatedDate??string.Empty },
                new SqlParameter { ParameterName = "@sourceofRegistration", Value =model.sourceofRegistration },
                new SqlParameter { ParameterName = "@ServiceID", Value =model.ServiceID },
                new SqlParameter { ParameterName = "@ProcessIndustryID", Value =model.ProcessIndustryID??string.Empty },
                new SqlParameter { ParameterName = "@passsalt", Value =model.passsalt },
                new SqlParameter { ParameterName = "@RequestID", Value =model.RequestID},
                new SqlParameter { ParameterName = "@ApplicationID", Value =model.ApplicationID },
                
                 
               
                new SqlParameter { ParameterName = "@Annual_Turnover", Value =model.Annual_Turnover },
                new SqlParameter { ParameterName = "@insertDate", Value =model.insertDate??string.Empty },
                new SqlParameter { ParameterName = "@Occupier_First_Name", Value =model.Occupier_First_Name },
                new SqlParameter { ParameterName = "@Occupier_Middle_Name", Value =model.Occupier_Middle_Name },
                new SqlParameter { ParameterName = "@Occupier_Last_Name", Value =model.Occupier_Last_Name },
              
                new SqlParameter { ParameterName = "@fullName", Value =model.fullName },
                new SqlParameter { ParameterName = "@fatherName", Value =model.fatherName },
                new SqlParameter { ParameterName = "@dob", Value =model.DTDob },
                new SqlParameter { ParameterName = "@categoryId", Value =model.categoryId },
                new SqlParameter { ParameterName = "@gender", Value =model.Occupier_Gender },
                new SqlParameter { ParameterName = "@mobileNo", Value =model.Occupier_Mobile_No },
                new SqlParameter { ParameterName = "@emailId", Value =model.Occupier_Email_ID },
                new SqlParameter { ParameterName = "@password", Value =model.Password },
                new SqlParameter { ParameterName = "@transIp", Value =model.NtransIp },
                new SqlParameter { ParameterName = "@requestKey", Value =model.requestKey }
            };

            var sqlProc = "Proc_NiveshMitraRegistration @ControlId ,@UnitId ,@CompanyName ,@IndustryDistrict ,@IndustryDistrictID ,@Industry_Address, @Pin_Code ,@Occupier_Name ,@Occupier_Father_Mother_Name ,@Occupier_Email_ID ,@Occupier_Mobile_No, @Occupier_PAN,@Occupier_Address ,@Occupier_Country_Id , @Occupier_State_ID ,@Occupier_District_ID ,@Occupier_District_Name ,@Occupier_Pin_Code ,@Nature_of_Activity ,@Installed_Capacity ,@Employees ,@Nature_of_Operation,@Organization_Type_ID,@Organization_Type,";
            sqlProc += " @Industry_Type_ID,@Industry_Type_Name ,@Expected_date_construction ,@Project_Status ,@Project_Cost ,@insertDate,@isUpdated,@lastUpdatedDate,@sourceofRegistration ,@ServiceID,";
            sqlProc += " @ProcessIndustryID ,@passsalt ,@RequestID ,@ApplicationID , @Industry_Color,@Expected_date_production, @Unit_Category,@Items_Manufactured ,@Annual_Turnover , @Occupier_First_Name,";
            sqlProc += "  @Occupier_Middle_Name ,@Occupier_Last_Name  ,@fullName , @fatherName ,@dob  ,@categoryId  , @gender  , @mobileNo  , @emailId, @password, @transIp , @requestKey ";

            var sList = this.Database.SqlQuery<NiveshMitraRegistrationModel>(sqlProc, sqlParams).ToList();
            return sList;
        }
        #endregion



        public List<NiveshMitraSendStatusModel> SendApplicationSubmittedStatus(NiveshMitraSendStatusModel model)
        {
            DataTable dt = new DataTable();
            var sqlParams = new SqlParameter[] {                 
                new SqlParameter { ParameterName = "@ControlId", Value =model.Control_ID  },
                new SqlParameter { ParameterName = "@UnitId", Value =model.Unit_Id },
                new SqlParameter { ParameterName = "@ServiceId", Value =model.ServiceID },
                new SqlParameter { ParameterName = "@ProcessIndustryID", Value =model.ProcessIndustryID }, 
                new SqlParameter { ParameterName = "@ApplicationId", Value =model.ApplicationID }, 
                new SqlParameter { ParameterName = "@StatusCode", Value =model.StatusCode },
                new SqlParameter { ParameterName = "@Remarks", Value =model.Remarks },
                new SqlParameter { ParameterName = "@FeeAmount", Value =model.FeeAmount },
                new SqlParameter { ParameterName = "@FeeStatus", Value =model.FeeStatus },
                new SqlParameter { ParameterName = "@TransectionID", Value =model.TransectionID },
                new SqlParameter { ParameterName = "@TranSactionDate", Value =model.TranSactionDate },
                new SqlParameter { ParameterName = "@TransectionDateAndTime", Value =model.TransectionDateAndTime },
                new SqlParameter { ParameterName = "@NocCertificateNumber", Value =model.NocCertificateNumber },
                new SqlParameter { ParameterName = "@NocUrl", Value =model.NocUrl },
                new SqlParameter { ParameterName = "@IsNocUrlActiveYesNo", Value =model.IsNocUrlActiveYesNo },
                new SqlParameter { ParameterName = "@Passalt", Value =model.Passalt },
              
                new SqlParameter { ParameterName = "@RequestId", Value =model.RequestId },
                new SqlParameter { ParameterName = "@PendencyLevel", Value =model.PendencyLevel },
                new SqlParameter { ParameterName = "@ObjectRejectionCode", Value =model.ObjectRejectionCode },
                new SqlParameter { ParameterName = "@IsCertificateValidLifeTime", Value =model.IsCertificateValidLifeTime },
                new SqlParameter { ParameterName = "@CertificateExpireDateDDMMYYYY", Value =model.CertificateExpireDateDDMMYYYY },
                new SqlParameter { ParameterName = "@D1", Value =model.D1 },
                new SqlParameter { ParameterName = "@D2", Value =model.D2 },
                new SqlParameter { ParameterName = "@D3", Value =model.D3 },
                new SqlParameter { ParameterName = "@D4", Value =model.D4 },
                new SqlParameter { ParameterName = "@D5", Value =model.D5 },
                new SqlParameter { ParameterName = "@D6", Value =model.D6 },
                new SqlParameter { ParameterName = "@D7", Value =model.D7 },

                new SqlParameter { ParameterName = "@SendDate", Value =model.SendDate },
                new SqlParameter { ParameterName = "@ResStatus", Value =model.ResStatus },
                new SqlParameter { ParameterName = "@StepId", Value =model.StepId },
                new SqlParameter { ParameterName = "@ServiceStatus", Value =model.ServiceStatus??string.Empty }
                
            };

            var sqlProc = "Proc_SendNiveshApplicationSubmittedStatus @ControlId ,@UnitId ,@ServiceId ,@ProcessIndustryID ,@ApplicationId ,@StatusCode ,@Remarks ,@FeeAmount ,";
            sqlProc += "@FeeStatus,@TransectionID,@TranSactionDate ,@TransectionDateAndTime ,@NocCertificateNumber ,@NocUrl,@IsNocUrlActiveYesNo ,@Passalt ,@RequestId ,@PendencyLevel,@ObjectRejectionCode ,";
            sqlProc += "@IsCertificateValidLifeTime ,@CertificateExpireDateDDMMYYYY ,@D1 ,@D2 ,@D3 ,@D4 ,@D5 ,@D6 ,@D7 ,@SendDate ,@ResStatus ,@ServiceStatus ,@StepId ";

            var sList = this.Database.SqlQuery<NiveshMitraSendStatusModel>(sqlProc, sqlParams).ToList();
            return sList;


        }

        #region Vinod
        public List<NiveshMitraSendStatusModel> SendRegistrationStatus(NiveshMitraSendStatusModel model)
        {
            DataTable dt = new DataTable();
            var sqlParams = new SqlParameter[] {                 
                new SqlParameter { ParameterName = "@ControlId", Value =model.Control_ID  },
                new SqlParameter { ParameterName = "@UnitId", Value =model.Unit_Id },
                new SqlParameter { ParameterName = "@ServiceId", Value =model.ServiceID },
                new SqlParameter { ParameterName = "@ProcessIndustryID", Value =model.ProcessIndustryID }, 
                new SqlParameter { ParameterName = "@ApplicationId", Value =model.ApplicationID }, 
                new SqlParameter { ParameterName = "@StatusCode", Value =model.StatusCode },
                new SqlParameter { ParameterName = "@Remarks", Value =model.Remarks },
                new SqlParameter { ParameterName = "@FeeAmount", Value =model.FeeAmount },
                new SqlParameter { ParameterName = "@FeeStatus", Value =model.FeeStatus },
                new SqlParameter { ParameterName = "@TransectionID", Value =model.TransectionID },
                new SqlParameter { ParameterName = "@TranSactionDate", Value =model.TranSactionDate },
                new SqlParameter { ParameterName = "@TransectionDateAndTime", Value =model.TransectionDateAndTime },
                new SqlParameter { ParameterName = "@NocCertificateNumber", Value =model.NocCertificateNumber },
                new SqlParameter { ParameterName = "@NocUrl", Value =model.NocUrl },
                new SqlParameter { ParameterName = "@IsNocUrlActiveYesNo", Value =model.IsNocUrlActiveYesNo },
                new SqlParameter { ParameterName = "@Passalt", Value =model.Passalt },
              
                new SqlParameter { ParameterName = "@RequestId", Value =model.RequestId },
                new SqlParameter { ParameterName = "@PendencyLevel", Value =model.PendencyLevel },
                new SqlParameter { ParameterName = "@ObjectRejectionCode", Value =model.ObjectRejectionCode },
                new SqlParameter { ParameterName = "@IsCertificateValidLifeTime", Value =model.IsCertificateValidLifeTime },
                new SqlParameter { ParameterName = "@CertificateExpireDateDDMMYYYY", Value =model.CertificateExpireDateDDMMYYYY },
                new SqlParameter { ParameterName = "@D1", Value =model.D1 },
                new SqlParameter { ParameterName = "@D2", Value =model.D2 },
                new SqlParameter { ParameterName = "@D3", Value =model.D3 },
                new SqlParameter { ParameterName = "@D4", Value =model.D4 },
                new SqlParameter { ParameterName = "@D5", Value =model.D5 },
                new SqlParameter { ParameterName = "@D6", Value =model.D6 },
                new SqlParameter { ParameterName = "@D7", Value =model.D7 },

                new SqlParameter { ParameterName = "@SendDate", Value =model.SendDate },
                new SqlParameter { ParameterName = "@ResStatus", Value =model.ResStatus },
                new SqlParameter { ParameterName = "@StepId", Value =model.StepId },
                new SqlParameter { ParameterName = "@ServiceStatus", Value =model.ServiceStatus??string.Empty }
                
            };

            var sqlProc = "Proc_SendNiveshRegistrationStatus @ControlId ,@UnitId ,@ServiceId ,@ProcessIndustryID ,@ApplicationId ,@StatusCode ,@Remarks ,@FeeAmount ,";
            sqlProc += "@FeeStatus,@TransectionID,@TranSactionDate ,@TransectionDateAndTime ,@NocCertificateNumber ,@NocUrl,@IsNocUrlActiveYesNo ,@Passalt ,@RequestId ,@PendencyLevel,@ObjectRejectionCode ,";
            sqlProc += "@IsCertificateValidLifeTime ,@CertificateExpireDateDDMMYYYY ,@D1 ,@D2 ,@D3 ,@D4 ,@D5 ,@D6 ,@D7 ,@SendDate ,@ResStatus ,@ServiceStatus ,@StepId ";

            var sList = this.Database.SqlQuery<NiveshMitraSendStatusModel>(sqlProc, sqlParams).ToList();
            return sList;


        }
        #endregion

        //public List<MedicalRegistrationDetailsModel>GetExistingMedicalEstabRegistration(Int64 RegisterUserID)
        //{

        //    var sqlParams = new SqlParameter[] {                 
        //        new SqlParameter { ParameterName = "@RegisterUserID", Value =RegisterUserID  }
                
        //    };

        //    var sqlProc = "Proc_GetNiveshMedicalRegistrationDetail @RegisterUserID";
        //    var sList = this.Database.SqlQuery<MedicalRegistrationDetailsModel>(sqlProc, sqlParams).ToList();
        //    return sList;
        //}
        public List<MedicalRegistrationDetailsModel> GetExistingMedicalEstabRegistration(Int64 regisIdNUH)
        {

            var sqlParams = new SqlParameter[] {                 
                new SqlParameter { ParameterName = "@regisIdNUH", Value =regisIdNUH  }
                
            };

            var sqlProc = "Proc_GetNiveshMedicalRegistrationDetail @regisIdNUH";
            var sList = this.Database.SqlQuery<MedicalRegistrationDetailsModel>(sqlProc, sqlParams).ToList();
            return sList;
        }

        public List<NiveshMitraUserDetailsModel> GetNiveshMitraUserDetails(string ControlID, string UnitID, string ServiceID, string RequestID, string UserName)
        {

            var sqlParams = new SqlParameter[] {                 
                new SqlParameter { ParameterName = "@ControlID", Value =ControlID  },
                 new SqlParameter { ParameterName = "@UnitID", Value =UnitID  },
                 new SqlParameter { ParameterName = "@ServiceID", Value =ServiceID  },
                 new SqlParameter { ParameterName = "@RequestID", Value =RequestID  },
                 new SqlParameter { ParameterName = "@UserName", Value =UserName },
                 new SqlParameter { ParameterName = "@rollId", Value ="" }
            };

            var sqlProc = "Proc_GetNiveshMitrsUserLoginDetails @ControlID,@UnitID,@ServiceID,@RequestID,@UserName,@rollId";
            var sList = this.Database.SqlQuery<NiveshMitraUserDetailsModel>(sqlProc, sqlParams).ToList();
            return sList;
        }


        #region Method User Registration
        public List<RegistrationModel> Registration(RegistrationModel model)
        {
            var sqlParams = new SqlParameter[] {                 
                new SqlParameter { ParameterName = "@fullName", Value =model.fullName },
                new SqlParameter { ParameterName = "@fatherName", Value =model.fatherName },
                new SqlParameter { ParameterName = "@dob", Value =model.DTDob },
                new SqlParameter { ParameterName = "@categoryId", Value =model.categoryId },
                new SqlParameter { ParameterName = "@gender", Value =model.gender },
                new SqlParameter { ParameterName = "@mobileNo", Value =model.mobileNo },
                new SqlParameter { ParameterName = "@emailId", Value =model.emailId??string.Empty },
                new SqlParameter { ParameterName = "@password", Value =model.password },
                new SqlParameter { ParameterName = "@transIp", Value =model.transIp },
                new SqlParameter { ParameterName = "@requestKey", Value =model.requestKey }
            };

            var sqlProc = "proc_Registration @fullName,@fatherName,@dob,@categoryId,@gender,@mobileNo,@emailId,@password,@transIp,@requestKey";
            var sList = this.Database.SqlQuery<RegistrationModel>(sqlProc, sqlParams).ToList();
            return sList;
        }
        #endregion

        #region check email existence
        public ResultSet CheckEmailMobileExistence(string checkValue, string Type)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue },
                    new SqlParameter { ParameterName = "@Type", Value = Type }

                };

            var sqlQuery = @"proc_checkEmailMobleExistence @checkValue,@Type";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion

        public List<UserDetailsModel> GetUserDetails(string UserName, Int64 rollId)
        {
            var sqlParams = new SqlParameter[] {                 
                new SqlParameter { ParameterName = "@userName", Value =UserName  },
                 new SqlParameter { ParameterName = "@rollId", Value =rollId  }
            };

            var sqlProc = "Proc_GetUserLoginDetails @userName,@rollId";
            var sList = this.Database.SqlQuery<UserDetailsModel>(sqlProc, sqlParams).ToList();
            return sList;
        }

        #region Update Mobile Verification Status
        public ResultSet UpdateMobileverify(Int64 UserId, String verifyid, String IpAddress)
        {
            try
            {
                var count = this.Database.SqlQuery<ResultSet>("Proc_UpdateMobileverify @userId, @verifyid, @IpAddress",
                    new SqlParameter("userId", UserId),
                    new SqlParameter("verifyid", verifyid),
                    new SqlParameter("IpAddress", IpAddress)).ToList().FirstOrDefault();
                return count;
            }
            catch
            { return null; }

        }
        #endregion

        #region OTP varification and count
        public IEnumerable<ForgotPasswordModel> OTPVarification(ForgotPasswordModel model)
        {
            var sqlparam = new SqlParameter[]
            { 
             
              new SqlParameter{ParameterName="@flag",Value= model.flag },
              new SqlParameter{ParameterName="@UserId",Value=model.UserId  },              
              new SqlParameter{ParameterName="@MobileNo",Value=model.MobileNo  }               
            
            };

            var sqlQuery = @"Proc_UpdateAndVerifyOtpCount @flag,@UserId,@MobileNo";

            var IList = this.Database.SqlQuery<ForgotPasswordModel>(sqlQuery, sqlparam);

            return IList;


        }
        #endregion

        #region Create SMS Log
        public int SMSLog(String Message, String MobileNo, String SMSStatus, String userId)
        {
            try
            {
                int count = this.Database.ExecuteSqlCommand("Proc_SMSLog @MobileNo, @SMSText, @SMSStatus,@UserId",
                    new SqlParameter("MobileNo", MobileNo),
                    new SqlParameter("SMSText", Message),
                    new SqlParameter("SMSStatus", SMSStatus),
                    new SqlParameter("UserId", userId)
                    );
                return count;
            }
            catch
            { return 0; }

        }
        #endregion

        public UserDetailsModel ManageloginAndGetStatus(Int64 UserId, string Status, int AllowedWrongAttampt)
        {
            var sqlParams = new SqlParameter[] {                 
                new SqlParameter { ParameterName = "@userid", Value =UserId },
                new SqlParameter { ParameterName = "@PasswordFlag", Value =Status },
                 new SqlParameter { ParameterName = "@AllowedWrongAttampt", Value =AllowedWrongAttampt }
            };

            var sqlProc = "proc_ValidateUserPolicy_UserLogin @userid,@PasswordFlag,@AllowedWrongAttampt";
            var sList = this.Database.SqlQuery<UserDetailsModel>(sqlProc, sqlParams).ToList().FirstOrDefault();
            return sList;
        }

        #region Forget password
        public ForgotPasswordModel GetUserDetailByEmailIdOrMobile(int flag, string mobileno, string emailid, string UserName)
        {
            var sqlparam = new SqlParameter[]
            {  
              new SqlParameter{ParameterName="@flag",Value= flag }, 
              new SqlParameter{ParameterName="@MobileNo",Value=mobileno==null?"":mobileno  },
              new SqlParameter{ParameterName="@EmailId",Value=emailid==null?"":emailid  }, 
              new SqlParameter{ParameterName="@userName",Value=UserName  } 
            };

            var sqlQuery = @"proc_GetUserDetailsByEmailAndMobileNo @flag,@MobileNo,@EmailId,@userName";
            var IList = this.Database.SqlQuery<ForgotPasswordModel>(sqlQuery, sqlparam).ToList().FirstOrDefault();
            return IList;
        }
        #endregion

        public int ChangePassword(PasswordChangeModel model)
        {
            int res = 0;
            res = this.Database.ExecuteSqlCommand("proc_UpdateUserPassword @userId,@password,@transIp",
               new SqlParameter("@userId", model.UserId),
               new SqlParameter("@password", model.newPassword),
               new SqlParameter("@transIp", model.transIp)
               );
            return res;
        }
        #region ISP
        public int AddISPLoginDetails(string sessionKey, string encryptdata, string token, string applicant_id, string request_id, string ispUserDetails, string service_code)
        {
            int res = 0;
            res = this.Database.ExecuteSqlCommand("AddISPLoginDetails @decriptedsessionKey, @incriptedsessionKey, @ispToken, @applicant_id, @request_id,@request_validated,@ispUserDetails, @service_codeISP",
               new SqlParameter("@decriptedsessionKey", sessionKey),
               new SqlParameter("@incriptedsessionKey", encryptdata),
               new SqlParameter("@ispToken", token),
               new SqlParameter("@applicant_id", applicant_id),
               new SqlParameter("@request_id", request_id),
               new SqlParameter("@request_validated", 'Y'),
               new SqlParameter("@ispUserDetails", ispUserDetails),
               new SqlParameter("@service_codeISP", service_code)
               );
            return res;
        }

        public int UpdateISPDetails(string application_id, string service_code, string request_id)
        {

            int res = 0;
            try
            {
                res = this.Database.ExecuteSqlCommand("ISPUpdateUserApplicationDetails  @application_id,@service_codeISP,@request_id",

                   new SqlParameter("@application_id", application_id),
                   new SqlParameter("@service_codeISP", service_code),
                   new SqlParameter("@request_id", request_id)


                   );
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public int ISPErrorLog(string MethordName, string ErrorMessage, string RegistrationNo, string ActionLevel, string ActionBy)
        {

            int res = 0;
            try
            {
                res = this.Database.ExecuteSqlCommand("AddISPErrorLog @MethordName,@ErrorMessage,@RegistrationNo,@ActionLevel,@ActionBy",

                   new SqlParameter("@MethordName", MethordName),
                   new SqlParameter("@ErrorMessage", ErrorMessage),
                     new SqlParameter("@RegistrationNo", RegistrationNo),
                   new SqlParameter("@ActionLevel", ActionLevel),
                   new SqlParameter("@ActionBy", ActionBy)


                   );
            }
            catch (Exception ex)
            {

            }
            return res;
        }
        #endregion 

        public UserDetailsModel GetUserById(long? userid)
        {
            SqlParameter param = new SqlParameter("@UserId", userid ?? (object)DBNull.Value);
            var sqlProc = "Execute proc_GetUserByUserId @UserId";
            var sList = this.Database.SqlQuery<UserDetailsModel>(sqlProc, param).FirstOrDefault();
            return sList;
        }

        #region Update User Profile Method
        public List<QueryExecuteModel> UpdateUserProfile(MyAccountModel model)
        {
            var sqlParams = new SqlParameter[] {
                 new SqlParameter { ParameterName = "@profileId", Value =model.profileId}, 
                 new SqlParameter { ParameterName = "@profileName", Value =model.userName},
                 new SqlParameter { ParameterName = "@fatherName", Value = model.fatherName},    
                 new SqlParameter { ParameterName = "@designation", Value = model.designation??string.Empty},   
                 new SqlParameter { ParameterName = "@emailId", Value = model.emailId??string.Empty},   
                 new SqlParameter { ParameterName = "@transIp", Value = model.transIp},   
                 new SqlParameter { ParameterName = "@profileImagePath", Value = model.profilePicPath??string.Empty}   
            };
            var query = "proc_UpdateUserProfile @profileId,@profileName,@fatherName,@designation,@emailId,@transIp,@profileImagePath";
            var sList = this.Database.SqlQuery<QueryExecuteModel>(query, sqlParams).ToList();
            return sList;
        }
        #endregion

        public List<UserDetailsModel> GetAdminDetails(string UserName)
        {

            var sqlParams = new SqlParameter[] {                 
                new SqlParameter { ParameterName = "@userName", Value =UserName  } 
            };

            var sqlProc = "proc_GetLoginDetailsForAdmin @userName";
            var sList = this.Database.SqlQuery<UserDetailsModel>(sqlProc, sqlParams).ToList();
            return sList;
        }

        public List<RollPermissionModel> GetPermissionDetails()
        {
            var sqlProc = "proc_GetRollWiseAllowedServices";
            var sList = this.Database.SqlQuery<RollPermissionModel>(sqlProc).ToList();
            return sList;
        }

        #region Method for Create User Password
        public List<PasswordChangeModel> GetUsersForCreatePassword(long rollId, long userId)
        {
            var sqlParams = new SqlParameter[] {                 
                new SqlParameter { ParameterName = "@rollId", Value =rollId  }, 
                new SqlParameter { ParameterName = "@userId", Value =userId  } 
            };

            var sqlProc = "proc_GetUsersForCreatePassword @rollId,@userId";
            var sList = this.Database.SqlQuery<PasswordChangeModel>(sqlProc, sqlParams).ToList();
            return sList;
        }

        public int CreatePasswordByAdminFirstTime(PasswordChangeModel model)
        {
            int res = 0;
            res = this.Database.ExecuteSqlCommand("proc_CreateUserPasswordbyAdmin @userId,@Newpassword,@transIp",
               new SqlParameter("@userId", model.UserId),
               new SqlParameter("@Newpassword", model.newPassword),
               new SqlParameter("@transIp", model.transIp)
               );
            return res;
        }
        #endregion



        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        #region

        public NUHmodel GetPendingApplicationByRequestId(int procId, string ControlID, string UnitID, string ServiceID, string RequestID)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=procId}  ,
              new SqlParameter{ParameterName="@ControlID",Value=ControlID},
              new SqlParameter{ParameterName="@UnitID",Value=UnitID},
              new SqlParameter{ParameterName="@ServiceID",Value=ServiceID},
              new SqlParameter{ParameterName="@RequestID",Value=RequestID} 
            };
            var _proc = @"proc_GetPendingApplicationByRequestId @procId,@ControlID,@UnitID,@ServiceID,@RequestID";
            var slist = this.Database.SqlQuery<NUHmodel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }

        public List<NiveshMitraUserDetailsModel> GetUserDetailsByUserId(Int64 regByUser)
        {

            var sqlParams = new SqlParameter[] {                 
                new SqlParameter { ParameterName = "@regByUser", Value =regByUser  }
            };

            var sqlProc = "Proc_GetUserDetailsByUserId @regByUser";
            var sList = this.Database.SqlQuery<NiveshMitraUserDetailsModel>(sqlProc, sqlParams).ToList();
            return sList;
        }

        #endregion
    }
}