using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCSHealthFamilyWelfareDept.Models;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class UserManagement_DB : DbContext
    {
        #region Default Constructor
        public UserManagement_DB()
            : base("CMSModule")
        { }
        #endregion

        #region USER

        #region Method Create User
        public List<UserModel> CreateUser(UserModel model)
        {
            var sqlParams = new SqlParameter[] {                 
                new SqlParameter { ParameterName = "@fullName", Value =model.fullName },
                new SqlParameter { ParameterName = "@idProofId", Value =model.idProofId },
                new SqlParameter { ParameterName = "@idProofNo", Value =model.idProofNo}, 
                new SqlParameter { ParameterName = "@designation", Value =model.designation }, 
                new SqlParameter { ParameterName = "@mobileNo", Value =model.mobileNo },
                new SqlParameter { ParameterName = "@emailId", Value =model.emailId??string.Empty },  
                new SqlParameter { ParameterName = "@userId", Value =model.userId },  
                new SqlParameter { ParameterName = "@password", Value =model.password }, 
                new SqlParameter { ParameterName = "@districtId", Value =model.districtId },  
                new SqlParameter { ParameterName = "@rollId", Value =model.rollId },
                new SqlParameter { ParameterName = "@insertBy", Value =model.insertBy },
                new SqlParameter { ParameterName = "@transIp", Value =model.transIp }
            };

            var sqlProc = "proc_CreateUser @fullName,@idProofId,@idProofNo,@designation,@mobileNo,@emailId,@userId,@password,@districtId,@rollId,@insertBy,@transIp";
            var sList = this.Database.SqlQuery<UserModel>(sqlProc, sqlParams).ToList();
            return sList;
        }
        #endregion

        #region check user id existence
        public ResultSet CheckUserIdExistence(string checkValue)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@checkValue", Value = checkValue }
                };

            var sqlQuery = @"proc_checkUserIdExistence @checkValue";
            var sDetails = this.Database.SqlQuery<ResultSet>(sqlQuery, sqlParams).FirstOrDefault();
            return sDetails;
        }
        #endregion

        public List<UserDetailsModel> GetUsersList(long insertBy, string fullName, string mobileNo)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@insertBy", Value = insertBy },
                    new SqlParameter { ParameterName = "@fullName", Value = fullName??string.Empty },
                    new SqlParameter { ParameterName = "@mobileNo", Value = mobileNo??string.Empty }
                };

            var sqlQuery = @"proc_GetUsers @insertBy,@fullName,@mobileNo";
            var sList = this.Database.SqlQuery<UserDetailsModel>(sqlQuery, sqlParams).ToList();
            return sList;
        }

        public List<PermissionModel> GetServicePermission(long rollId, long userId)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@rollId", Value = rollId },
                    new SqlParameter { ParameterName = "@userId", Value = userId }
                };

            var sqlQuery = @"proc_GetServicePermission @rollId,@userId";
            var sList = this.Database.SqlQuery<PermissionModel>(sqlQuery, sqlParams).ToList();
            return sList;
        }

        public int InsertServicePermission(long userId, string transIp, string serviceXML)
        {
            var sqlParams = new SqlParameter[] {  
                    new SqlParameter { ParameterName = "@userId", Value = userId },
                    new SqlParameter { ParameterName = "@transIp", Value = transIp },
                    new SqlParameter { ParameterName = "@serviceXML", Value = serviceXML }
                };

            var sqlQuery = @"proc_InsertServicePermission @userId,@transIp,@serviceXML";
            var varResult = this.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
            return varResult;
        }

        public List<UserProfileModel> GetUserProfileAD(long userId, long rollId, int districtId, string fullName, string userName, long profileId = 0)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@userId", Value = userId },
                    new SqlParameter { ParameterName = "@rollId", Value = rollId },
                    new SqlParameter { ParameterName = "@districtId", Value = districtId },
                    new SqlParameter { ParameterName = "@fullName", Value = fullName??string.Empty },
                    new SqlParameter { ParameterName = "@userName", Value = userName??string.Empty },
                    new SqlParameter { ParameterName = "@profileId", Value = profileId }
                };

            var sqlQuery = @"proc_GetUserProfile_AD @userId,@rollId,@districtId,@fullName,@userName,@profileId";
            var sList = this.Database.SqlQuery<UserProfileModel>(sqlQuery, sqlParams).ToList();
            return sList;
        }

        public int ChangePassword(PasswordModel model)
        {
            int res = 0;
            res = this.Database.ExecuteSqlCommand("proc_UpdateUserPassword @userId,@password,@transIp",
               new SqlParameter("@userId", model.UserId),
               new SqlParameter("@password", model.newPassword),
               new SqlParameter("@transIp", model.transIp)
               );
            return res;
        }
        #endregion

        #region Committee

        public ResultSet InsertCommittee(CommitteeModel model)
        {
            var sqlParams = new SqlParameter[] {
                new SqlParameter { ParameterName = "@commMemId", Value =model.commMemId} ,
                 new SqlParameter { ParameterName = "@transIp ", Value =model.transIp} ,
                 new SqlParameter { ParameterName = "@createdBy ", Value =model.userId} ,
                 new SqlParameter { ParameterName = "@commMemName ", Value =model.commMemName} ,
                 new SqlParameter { ParameterName = "@commMemDesig ", Value =model.commMemDesig} ,
                 new SqlParameter { ParameterName = "@commMemDept ", Value =model.commMemDept} ,
                 
        };

            var sqlProc = @"Proc_InsertCMOCommittee @commMemId,@transIp,@createdBy,@commMemName,@commMemDesig,@commMemDept";
            var sList = this.Database.SqlQuery<ResultSet>(sqlProc, sqlParams).ToList().FirstOrDefault();
            return sList;
        }

        public List<CommitteeModel> GetCommitteeMemberList(long UserID,string Name,string Designation)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@UserID", Value = UserID },
                    new SqlParameter { ParameterName = "@Name", Value = Name },
                    new SqlParameter { ParameterName = "@designation", Value = Designation }
                };

            var sqlQuery = @"proc_GetCommitteeMemberList @UserID,@Name,@designation";
            var sList = this.Database.SqlQuery<CommitteeModel>(sqlQuery, sqlParams).ToList();
            return sList;
        }
        public CommitteeModel GetCommitteeMemberById(long commMemId)
        {
            var sqlParams = new SqlParameter[] {
                 new SqlParameter { ParameterName = "@commMemId ", Value =commMemId}                 
        };

            var sqlProc = @"proc_GetCommitteeMemberById @commMemId";
            var sList = this.Database.SqlQuery<CommitteeModel>(sqlProc, sqlParams).ToList().FirstOrDefault();
            return sList;
        }
        public ResultSet ActiveDeactiveMember(long commMemId, bool isActive)
        {
            var sqlParams = new SqlParameter[] {
                 new SqlParameter { ParameterName = "@commMemId ", Value =commMemId},
                 new SqlParameter { ParameterName = "@isActive ", Value =isActive}              
        };

            var sqlProc = @"proc_ActiveDeactiveMember @commMemId,@isActive";
            var sList = this.Database.SqlQuery<ResultSet>(sqlProc, sqlParams).ToList().FirstOrDefault();
            return sList;
        }
        #endregion

        public List<ManageAccountModel> GetAccountDetailsByUsername(string userName, long rollId, long userId)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@userName", Value = userName },
                    new SqlParameter { ParameterName = "@resetByRollId", Value = rollId },
                    new SqlParameter { ParameterName = "@resetByUserId", Value = userId }
                };

            var sqlQuery = @"proc_GetAccountDetailsByUsername @userName,@resetByRollId,@resetByUserId";
            var sList = this.Database.SqlQuery<ManageAccountModel>(sqlQuery, sqlParams).ToList();
            return sList;
        }

        public int ResetAccount(long userId, string resetType, string password, long resetBy, string transIp)
        {
            var sqlParams = new SqlParameter[] { 
                    new SqlParameter { ParameterName = "@userId", Value = userId },
                    new SqlParameter { ParameterName = "@resetType", Value = resetType },
                    new SqlParameter { ParameterName = "@password", Value = password },
                    new SqlParameter { ParameterName = "@resetBy", Value = resetBy },
                    new SqlParameter { ParameterName = "@transIp", Value = transIp }
                };

            var sqlQuery = @"proc_ResetAccount @userId,@resetType,@password,@resetBy,@transIp";
            var sList = this.Database.ExecuteSqlCommand(sqlQuery, sqlParams);
            return sList;
        }
    }
}