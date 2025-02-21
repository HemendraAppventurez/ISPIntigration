using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CCSHealthFamilyWelfareDept.Models;
using CCSHealthFamilyWelfareDept.ReportModel;
using System.Configuration;
using System.Security.Policy;
using System.Net;
using System.IO;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class CMO_DB : DbContext
    {
        #region Default Constructor
        public CMO_DB()
            : base("CMSModule")
        { }
        #endregion
        #region NUH
        #region
        public ProcessType getMethodApplicationCountNUH(long userId)
        {
            var sqlParam = new SqlParameter[] { 
                new SqlParameter{ParameterName="@userId",Value=userId}
            };
            var _proc = @"proc_NUH_countProcess @userId";
            var slist = this.Database.SqlQuery<ProcessType>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }
        public List<ProcessType> GetDistrictList(int procId, int Id)
        {
            var sqlparam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@procId",Value=procId},
              new SqlParameter{ParameterName="@Id",Value=Id}
            };
            var _proc = @"proc_GetDataForDropDownList @procId,@Id";
            var slist = this.Database.SqlQuery<ProcessType>(_proc, sqlparam).ToList();
            return slist;
        }
        public List<NUHDetailsModel> GetAllNUHListForCMO(long regisId = 0, string fromdate = "", string todate = "", int district = 0)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=1}  ,
             new SqlParameter{ParameterName="@regisId",Value=regisId},
              new SqlParameter {ParameterName="@fromdate",Value=fromdate},
                new SqlParameter {ParameterName="@todate",Value=todate},
                new SqlParameter {ParameterName="@district",Value=district}
             
            };
            var _proc = @"proc_getAllNuHListForCMO @procId ,@regisId,@fromdate,@todate,@district";
            var slist = this.Database.SqlQuery<NUHDetailsModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public CancleNUHregistration CancleNUHApplicationForCMO(CancleNUHregistration model)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisIdNUH",Value=Convert.ToInt64(model.hdnNUHId)}  ,
             new SqlParameter{ParameterName="@AppCancleRemark",Value=model.Resion},
              new SqlParameter {ParameterName="@AppCancleFilePath",Value=model.FilePath??string.Empty},
                new SqlParameter {ParameterName="@AppCancelBy",Value=model.UserID},
                new SqlParameter {ParameterName="@AppCancleByIp",Value=model.Ip}
            };
            var _proc = @"Proc_CancleNUHApp @regisIdNUH,@AppCancleRemark,@AppCancleFilePath,@AppCancelBy,@AppCancleByIp";
            var slist = this.Database.SqlQuery<CancleNUHregistration>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }

        #endregion
        #region Riya
        public ProcessType getApplicationCountNUH(long userId, string fromdate = "", string todate = "", int district = 0)
        {
            var sqlParam = new SqlParameter[] { 
                new SqlParameter{ParameterName="@userId",Value=userId},
                new SqlParameter {ParameterName="@fromdate",Value=fromdate},
                new SqlParameter {ParameterName="@todate",Value=todate},
                new SqlParameter {ParameterName="@district",Value=district}
            };
            var _proc = @"proc_NUH_countProcess @userId,@fromdate,@todate,@district";
            var slist = this.Database.SqlQuery<ProcessType>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }
        #endregion
        #region Azeez
        public List<NUHDetailsModel> GetAllNUHList(long regisId = 0, string fromdate = "", string todate = "", int district = 0)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=1}  ,
             new SqlParameter{ParameterName="@regisId",Value=regisId},
              new SqlParameter {ParameterName="@fromdate",Value=fromdate},
                new SqlParameter {ParameterName="@todate",Value=todate},
                new SqlParameter {ParameterName="@district",Value=district}
             
            };
            var _proc = @"proc_getAllNuHList @procId ,@regisId,@fromdate,@todate,@district";
            var slist = this.Database.SqlQuery<NUHDetailsModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public NUHDetailsModel GetNUHCertificate(long regisId = 0)
        {
            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisId} 
             
            };
            var _proc = @"proc_getNUHCertificateNo @regisId";
            var slist = this.Database.SqlQuery<NUHDetailsModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }


        public NUHmodel InsertUpdateQueryRaisedDetail(string QueryRaised, string file, string RaiseByID, string RaiseByIP, long RegisNuHID, string Action)
        {
            var sqlParam = new SqlParameter[] { 
                 new SqlParameter{ParameterName="@QueryRaised",Value=QueryRaised} ,
              new SqlParameter{ParameterName="@RaisedById",Value=RaiseByID},
              new SqlParameter{ParameterName="@RaisedByIp",Value=RaiseByIP},
              new SqlParameter{ParameterName="@regisIdNUH",Value=RegisNuHID},
              new SqlParameter{ParameterName="@QueryReply",Value=""},
              new SqlParameter{ParameterName="@ReplyReleventFilePath",Value=file ?? string.Empty},
              new SqlParameter{ParameterName="@ReplyById",Value=""},
              new SqlParameter{ParameterName="@ReplyByIp",Value=""}, 
              new SqlParameter{ParameterName="@Action",Value=Action}
             
             
            };
            var _proc = @"ProcUpdateQueryRaisedByCMO @regisIdNUH, @QueryRaised,@RaisedById,@QueryReply,@ReplyReleventFilePath,@RaisedByIp, @ReplyById, @ReplyByIp,@Action";
            var slist = this.Database.SqlQuery<NUHmodel>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }


        public string SendStausToNiveshwithBinaryFormat(Int64 regisIdNUH, Int64 RegisByUserId, string SignFilePathUrl)
        {
            CMO_DB objCMODB = new CMO_DB();
            Common objCom = new Common();
            string returnMsg = "fail";
            bool isContinue = false;
            var result = objCMODB.GetNiveshUserDetailByID(RegisByUserId);
            if (result != null)
            {
                UPHEALTHNIC.upswp_niveshmitraservices ObjSendAppSubmitStatus = new UPHEALTHNIC.upswp_niveshmitraservices();
                //NiveshMitraSendStatusModel objStatusModel = objCMODB.GetNiveshMitraUserDetailsByID(result.RegisterByuserID).FirstOrDefault();
                NiveshMitraSendStatusModel objStatusModel = objCMODB.GetNiveshMitraUserDetailsByID(regisIdNUH).FirstOrDefault();
                if (objStatusModel != null)
                {

                    string StatusResult = string.Empty;
                    string StatusResultBinaryFormat = string.Empty;
                    string UrlFile = ConfigurationManager.AppSettings["DownloadShinedCertificateUrl"].ToString() + SignFilePathUrl.Substring(2, SignFilePathUrl.Length - 2);
                    //string UrlFile = SignFilePathUrl;

                    objStatusModel.ProcessIndustryID = objStatusModel.UserName;
                    objStatusModel.ApplicationID = regisIdNUH.ToString(); //objStatusModel.UserID.ToString();

                    objStatusModel.StatusCode = "15";
                    objStatusModel.Remarks = "Certificate Generated";
                    objStatusModel.PendencyLevel = "Entrepreneur level pendency";

                    objStatusModel.FeeAmount = "";
                    objStatusModel.FeeStatus = "";
                    objStatusModel.TransectionID = "";
                    objStatusModel.TranSactionDate = "";
                    objStatusModel.TransectionDateAndTime = "";
                    //   objStatusModel.NocCertificateNumber = "";
                    objStatusModel.NocUrl = UrlFile;
                    objStatusModel.IsNocUrlActiveYesNo = "Yes";
                    objStatusModel.Passalt = ConfigurationManager.AppSettings["PassKey"].ToString();
                    objStatusModel.ObjectRejectionCode = "";
                    objStatusModel.IsCertificateValidLifeTime = "No";
                    // objStatusModel.CertificateExpireDateDDMMYYYY = objStatusModel.CertificateExpireDateDDMMYYYY;
                    objStatusModel.D1 = "";
                    objStatusModel.D2 = "";
                    objStatusModel.D3 = "";
                    objStatusModel.D4 = "";
                    objStatusModel.D5 = "";
                    objStatusModel.D6 = "";
                    objStatusModel.D7 = "";


                    try
                    {
                        StatusResult = ObjSendAppSubmitStatus.WReturn_CUSID_STATUS(objStatusModel.Control_ID, objStatusModel.Unit_Id, objStatusModel.ServiceID, objStatusModel.ProcessIndustryID, objStatusModel.ApplicationID, objStatusModel.StatusCode,
                                           objStatusModel.Remarks, objStatusModel.PendencyLevel, objStatusModel.FeeAmount, objStatusModel.FeeStatus, objStatusModel.TransectionID, objStatusModel.TranSactionDate, objStatusModel.TransectionDateAndTime, objStatusModel.NocCertificateNumber, objStatusModel.NocUrl, objStatusModel.IsNocUrlActiveYesNo, objStatusModel.Passalt, objStatusModel.RequestId, objStatusModel.ObjectRejectionCode
                                            , objStatusModel.IsCertificateValidLifeTime, objStatusModel.CertificateExpireDateDDMMYYYY, objStatusModel.D1, objStatusModel.D2, objStatusModel.D3, objStatusModel.D4, objStatusModel.D5, objStatusModel.D6, objStatusModel.D7);

                        if (StatusResult.ToUpper() == "SUCCESS" || StatusResult.Contains("NM0011"))
                        {
                            objStatusModel.SendDate = System.DateTime.Now;
                            objStatusModel.ResStatus = "";
                            objStatusModel.ServiceStatus = StatusResult;
                            objStatusModel.StepId = 10;

                            string Attachment = SignFilePathUrl;
                            if (!string.IsNullOrEmpty(Attachment))
                            {
                                string base64String = "";
                                // var url = Microsoft.SqlServer.Server.MapPath(Convert.ToString(Attachment));
                                var url = HttpContext.Current.Server.MapPath(Attachment);
                                using (WebClient wc = new WebClient())
                                {
                                    var name = System.IO.Path.GetFileName(url);
                                    var bytes = wc.DownloadData(url);
                                    base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                                }

                                String ext = Path.GetExtension(Attachment);

                                StatusResultBinaryFormat = ObjSendAppSubmitStatus.WReturn_CUSID_Entrepreneur_NOC_IN_BINARYFORMAT(objStatusModel.Control_ID, objStatusModel.Unit_Id, objStatusModel.ServiceID,
                                    objStatusModel.ProcessIndustryID, objStatusModel.NocCertificateNumber, base64String, objCom.GetMimeType(ext), objStatusModel.Passalt, objStatusModel.RequestId);

                            }

                            returnMsg = "sucess";
                        }
                        else
                        { 
                            objStatusModel.SendDate = System.DateTime.Now;
                            objStatusModel.ResStatus = "";
                            objStatusModel.ServiceStatus = StatusResult;
                            objStatusModel.StepId = 10; 
                        }
                    }
                    catch (Exception ex)
                    {
                        objStatusModel.SendDate = System.DateTime.Now;
                        objStatusModel.ResStatus = ex.Message.ToString();
                        objStatusModel.ServiceStatus = "EXCEPTION";
                        objStatusModel.StepId = 10;
                    }

                    try
                    { 
                        objStatusModel = objCMODB.SaveCmoActionAndNiveshStatus(objStatusModel).FirstOrDefault(); 
                    }
                    catch (Exception ex)
                    {

                    } 
                }  
            }
            else
            {
                returnMsg = "sucess";
            }

            return returnMsg;
        }


        //Method for Save nivesh status

        public List<NiveshMitraSendStatusModel> SaveCmoActionAndNiveshStatus(NiveshMitraSendStatusModel model)
        {

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

        public ResultSet GetNiveshUserDetailByID(long regisByUserID)
        {

            var sqlParams = new SqlParameter[] {                 
                new SqlParameter { ParameterName = "@regisByUserID", Value =regisByUserID  },
                 
            };

            var sqlProc = "Proc_GetNiveshDetailsByID @regisByUserID";
            var sList = this.Database.SqlQuery<ResultSet>(sqlProc, sqlParams).FirstOrDefault();
            return sList;
        }


        //GetNiveshMitraUser Details
        //public List<NiveshMitraSendStatusModel> GetNiveshMitraUserDetailsByID(long regisByUserID)
        //{

        //    var sqlParams = new SqlParameter[] {                 
        //        new SqlParameter { ParameterName = "@regisByUserID", Value =regisByUserID  },
                 
        //    };

        //    var sqlProc = "Proc_GetNiveshMitrsUserDetailsByID @regisByUserID";
        //    var sList = this.Database.SqlQuery<NiveshMitraSendStatusModel>(sqlProc, sqlParams).ToList();
        //    return sList;
        //}
        public List<NiveshMitraSendStatusModel> GetNiveshMitraUserDetailsByID(long regisIdNUH)
        {

            var sqlParams = new SqlParameter[] {                 
                new SqlParameter { ParameterName = "@regisIdNUH", Value =regisIdNUH  },
                 
            };

            var sqlProc = "Proc_GetNiveshMitrsUserDetailsByID @regisIdNUH";
            var sList = this.Database.SqlQuery<NiveshMitraSendStatusModel>(sqlProc, sqlParams).ToList();
            return sList;
        }


        public List<NUHDetailsModel> GetScheduleOfCommittee(string inspectiondate, long UserID)
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@UserID",Value=UserID}  ,
             new SqlParameter{ParameterName="@inspectiondate",Value=inspectiondate}
             
            };
            var _proc = @"proc_GetScheduleOfCommittee @UserID,@inspectiondate";
            var slist = this.Database.SqlQuery<NUHDetailsModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public ResultSet UpdateAppProcessNUH(long regisIdNUH, int appStatus, long committeeId, string inspectionDate, string inspReportFilePath, string certificateFilePath, string rejectedRemarks, int ReasonID, long userId, string transIp, string XmlData, string XmlDataPhoto)//NUHAppProcessModel model
        {
            var sqlParam = new SqlParameter[] { 
               new SqlParameter("@regisIdNUH", regisIdNUH),
               new SqlParameter("@appStatus", appStatus),
               new SqlParameter("@committeeId", committeeId),
               new SqlParameter("@inspectionDate", string.IsNullOrEmpty(inspectionDate) ? (object)DBNull.Value : Convert.ToDateTime(inspectionDate)),
               new SqlParameter("@inspReportFilePath", inspReportFilePath ?? string.Empty),
               new SqlParameter("@certificateFilePath", certificateFilePath ?? string.Empty),
               new SqlParameter("@rejectedRemarks", rejectedRemarks ?? string.Empty),
               new SqlParameter("@ReasonID", ReasonID),
               new SqlParameter("@userId", userId),
               new SqlParameter("@transIp", transIp),
               new SqlParameter("@XmlData", XmlData ?? string.Empty),
               new SqlParameter("@XmlDataPhoto", XmlDataPhoto ?? string.Empty),
               
            };
            var _proc = @"proc_UpdateAppProcessNUH @regisIdNUH,@appStatus,@committeeId,@inspectionDate,@inspReportFilePath,@certificateFilePath,@rejectedRemarks,@ReasonID,@userId,@transIp,@XmlData,@XmlDataPhoto";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
            //int res = 0;
            //res = this.Database.ExecuteSqlCommand("proc_UpdateAppProcessNUH @regisIdNUH,@appStatus,@committeeId,@inspectionDate,@inspReportFilePath,@certificateFilePath,@rejectedRemarks,@userId,@transIp",
            //   new SqlParameter("@regisIdNUH", model.regisIdNUH),
            //   new SqlParameter("@appStatus", model.appStatus),
            //   new SqlParameter("@committeeId", model.committeeId),
            //   new SqlParameter("@inspectionDate", string.IsNullOrEmpty(model.inspectionDate) ? (object)DBNull.Value : Convert.ToDateTime(model.inspectionDate)),
            //   new SqlParameter("@inspReportFilePath", model.inspReportFilePath ?? string.Empty),
            //   new SqlParameter("@certificateFilePath", model.certificateFilePath ?? string.Empty),
            //   new SqlParameter("@rejectedRemarks", model.rejectedRemarks ?? string.Empty),
            //   new SqlParameter("@userId", model.userId),
            //   new SqlParameter("@transIp", model.transIp)
            //   );
            //return res;
        }


        public NUHAppProcessModel GetNUHList(long regisId, int status)
        {

            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisid",Value=regisId},
            new SqlParameter{ParameterName="@appliedStatus",Value=status}
            };
            var _proc = @"getNUHforUpDateApplication @regisid,@appliedStatus";
            var slist = this.Database.SqlQuery<NUHAppProcessModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }

        public List<rptCertificateNUHModel> GetDetail(long regisId)
        {
            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisId}  
            };
            var _proc = @"GetNUHrptDetail @regisId";
            var slist = this.Database.SqlQuery<rptCertificateNUHModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public int InsertUnSignedCertiPath_NUH(long regisId, string certificateFilePath)
        {
            var sqlParam = new SqlParameter[] { 
                 new SqlParameter{ParameterName="@regisIdNUH",Value=regisId},  
                 new SqlParameter{ParameterName="@certificateFilePath",Value=certificateFilePath}  
            };
            var _proc = @"proc_InsertUnSignedCertiPath_NUH @regisIdNUH,@certificateFilePath";
            var result = this.Database.ExecuteSqlCommand(_proc, sqlParam);
            return result;
        }

        public List<rptNHUChild> getNUHChild(long regisNUH)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisNUH}  
            };
            var _proc = @"proc_getNUHChild @regisId";
            var slist = this.Database.SqlQuery<rptNHUChild>(_proc, sqlParam).ToList();
            return slist;
        }
        public List<rptNHUChild> getNUHChildRpt(long regisNUH)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisNUH}  
            };
            var _proc = @"proc_getNUHChildRpt @regisId";
            var slist = this.Database.SqlQuery<rptNHUChild>(_proc, sqlParam).ToList();
            return slist;
        }
        public List<rptNHUdocChild> getNUHChildDOCRpt(long regisNUH)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisNUH}  
            };
            var _proc = @"proc_getNUHChildDOCRpt @regisId";
            var slist = this.Database.SqlQuery<rptNHUdocChild>(_proc, sqlParam).ToList();
            return slist;
        }
        public List<rptNHUOwnerChild> getNUHChildOwnerRpt(long regisNUH)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisNUH}  
            };
            var _proc = @"proc_getNUHChildOwnerRpt @regisId";
            var slist = this.Database.SqlQuery<rptNHUOwnerChild>(_proc, sqlParam).ToList();
            return slist;
        }
        #endregion
        public ResultSet UpdateNUHCertificate(long regisId, string certificatePath, long regisBy, string Ipaddress)
        {
            var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@regisId",Value=regisId}  ,
              new SqlParameter{ParameterName="@certificatePath",Value=certificatePath} ,
              new SqlParameter{ParameterName="@regisBy",Value=regisBy}  ,
              new SqlParameter{ParameterName="@Ipaddress",Value=Ipaddress} 
             
            };
            var _proc = @"proc_NUHUpdate @regisId,@certificatePath,@regisBy ,@Ipaddress";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }

        #endregion
        #region abhijeet
        #region Family Planing
        public ProcessType getFAPprocessCount(int ProcID, long userId)
        {
            var sqlparams = new SqlParameter[] { 
                 new SqlParameter{ParameterName="@procId",Value=ProcID},
            new SqlParameter{ParameterName="@userId",Value=userId}
             };
            var _proc = @"proc_FAP_countProcess @procId,@userId";
            var slist = this.Database.SqlQuery<ProcessType>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }

        public List<FAPModel> GetAllRegistrationFAP()
        {
            var _proc = @"proc_GetAllRegistrationFAP";
            var slist = this.Database.SqlQuery<FAPModel>(_proc).ToList();
            return slist;
        }

        public ProcessType getApplicationCountFAP(long userId)
        {
            var sqlParam = new SqlParameter[] { 
                new SqlParameter{ParameterName="@userId",Value=userId} 
            };
            var _proc = @"proc_GetCountVerifiedStatus_FAP @userId";
            var slist = this.Database.SqlQuery<ProcessType>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }

        public List<FAPModel> GetForwordedAppByUserId(long userId, string registrationNo, string mobile, string isVerify, string appliedDate)
        {
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@userId",Value=userId},
            new SqlParameter{ParameterName="@registrationNo",Value=registrationNo},
            new SqlParameter{ParameterName="@mobile",Value=mobile},
            new SqlParameter{ParameterName="@isVerify",Value=string.IsNullOrEmpty(isVerify)?(object)DBNull.Value:Convert.ToBoolean(Convert.ToInt32(isVerify))},
            new SqlParameter{ParameterName="@appliedDate",Value=string.IsNullOrEmpty(appliedDate)?(object)DBNull.Value:Convert.ToDateTime(appliedDate)}
            };
            var _proc = @"proc_GetForwordedAppByUserId_FAP @userId,@registrationNo,@mobile,@isVerify,@appliedDate";
            var slist = this.Database.SqlQuery<FAPModel>(_proc, sqlparams).ToList();
            return slist;
        }

        public List<FAPModel> GetAllFAPList(int procId, long regisFAPId, string registration, string mobile, string requestdDate, int status)
        {
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=procId},
            new SqlParameter{ParameterName="@regisFAPId",Value=regisFAPId},
            new SqlParameter{ParameterName="@registration",Value=registration},
            new SqlParameter{ParameterName="@mobile",Value=mobile},
            new SqlParameter{ParameterName="@requestDate",Value=requestdDate},
            new SqlParameter{ParameterName="@status",Value=status}
            };
            var _proc = @"proc_getAllFAP @procId,@regisFAPId,@registration,@mobile,@requestDate,@status";
            var slist = this.Database.SqlQuery<FAPModel>(_proc, sqlparams).ToList();
            return slist;
        }

        public List<FAPModel> getFAPChild(long regisFAP)
        {

            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisFAP}  
            };
            var _proc = @"proc_getFAPChild @regisId";
            var slist = this.Database.SqlQuery<FAPModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public List<FAPModel> GetAllFAPInProcessList(int procId, long regisFAPId, string registration, string mobile, string requestdDate, int status)
        {
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=procId},
            new SqlParameter{ParameterName="@regisFAPId",Value=regisFAPId},
            new SqlParameter{ParameterName="@registration",Value=registration},
            new SqlParameter{ParameterName="@mobile",Value=mobile},
            new SqlParameter{ParameterName="@requestDate",Value=requestdDate},
             new SqlParameter{ParameterName="@status",Value=status}
            };
            var _proc = @"proc_getAllFAPInProcess @procId,@regisFAPId,@registration,@mobile,@requestDate,@status";
            var slist = this.Database.SqlQuery<FAPModel>(_proc, sqlparams).ToList();
            return slist;

        }
        public List<FAPModel> GetAllFAPApprovedList(int procId, string registration, string mobile, string requestdDate)
        {
            var sqlparams = new SqlParameter[] { 
             new SqlParameter{ParameterName="@procId",Value=procId},
            new SqlParameter{ParameterName="@registration",Value=registration},
            new SqlParameter{ParameterName="@mobile",Value=mobile},
            new SqlParameter{ParameterName="@requestDate",Value=requestdDate}
            };
            var _proc = @"proc_getAllFAPApproved @procId,@registration,@mobile,@requestDate";
            var slist = this.Database.SqlQuery<FAPModel>(_proc, sqlparams).ToList();
            return slist;

        }
        public List<FAPModel> GetAllFAPRejectedList(int procId, string registration, string mobile, string requestdDate)
        {
            var sqlparams = new SqlParameter[] { 
             new SqlParameter{ParameterName="@procId",Value=procId},
            new SqlParameter{ParameterName="@registration",Value=registration},
            new SqlParameter{ParameterName="@mobile",Value=mobile},
            new SqlParameter{ParameterName="@requestDate",Value=requestdDate}
            };
            var _proc = @"proc_getAllFAPRejected @procId,@registration,@mobile,@requestDate";
            var slist = this.Database.SqlQuery<FAPModel>(_proc, sqlparams).ToList();
            return slist;

        }
        public List<FAPModel> GetFAPScheduledCommitteeList(string inspectionDate, long UserID)
        {
            var sqlparams = new SqlParameter[] { 
                new SqlParameter{ParameterName="@UserID",Value=UserID},
            new SqlParameter{ParameterName="@inspectionDate",Value=inspectionDate}
            };
            var _proc = @"proc_FAPschedulelList @UserID,@inspectionDate";
            var slist = this.Database.SqlQuery<FAPModel>(_proc, sqlparams).ToList();
            return slist;

        }
        public FAPAppProcessModel getFAPStatus(long regisFAPId)
        {
            var sqlparams = new SqlParameter[] { 
           
            new SqlParameter{ParameterName="@regisFAPId",Value=regisFAPId}
            };
            var _proc = @"proc_getFAPStatus @regisFAPId";
            var slist = this.Database.SqlQuery<FAPAppProcessModel>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public ResultSet updateFAPProcess(FAPAppProcessModel model, string xmldata)
        {
            var sqlparams = new SqlParameter[] { 
           
            new SqlParameter{ParameterName="@regisIdFAP",Value=model.regisIdFAP},
            new SqlParameter{ParameterName="@appStatus",Value=model.appStatus},
            new SqlParameter{ParameterName="@status",Value=model.status},
            new SqlParameter{ParameterName="@isverify",Value=model.isverify},
            new SqlParameter{ParameterName="@forwardTo",Value=model.forwardtoId},
            new SqlParameter{ParameterName="@committeeId",Value=model.committeeId},
            new SqlParameter{ParameterName="@inspectionDate",Value=model.inspectionDate??(object)DBNull.Value},
            new SqlParameter{ParameterName="@districtReportFilePath",Value=model.districtReportFilePath??String.Empty},
            new SqlParameter{ParameterName="@stateReportFilePath",Value=model.stateReportFilePath??String.Empty},
            new SqlParameter{ParameterName="@sanctionAmount",Value=model.sanctionAmount},
            new SqlParameter{ParameterName="@sanctionDate",Value=model.sancationDate??(object)DBNull.Value},
            new SqlParameter{ParameterName="@rejectedRemarks",Value=model.rejectedRemarks??String.Empty},
            new SqlParameter{ParameterName="@userId",Value=model.userId},
            new SqlParameter{ParameterName="@transIp",Value=model.transIp},
            new SqlParameter{ParameterName="@xmldata",Value=xmldata??(object)DBNull.Value},
            new SqlParameter{ParameterName="@xmldataPhoto",Value=model.XmlDataPhoto??(object)DBNull.Value},
            };
            var _proc = @"proc_UpdateAppProcessFAP  @regisIdFAP,@appStatus, @status, @isverify, @forwardTo, @committeeId, @inspectionDate, @districtReportFilePath, @stateReportFilePath ,@sanctionAmount, @sanctionDate,@rejectedRemarks, @userId,@transIp, @xmldata,@xmldataPhoto";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }

        public List<FAPAppProcessModel> rblforwardType()
        {
            var _proc = @"proc_FAP_forwardType";
            var slist = this.Database.SqlQuery<FAPAppProcessModel>(_proc).ToList();
            return slist;
        }
        public List<FAPAppProcessModel> bindDropdownlist(long rollId)
        {
            var sqlparam = new SqlParameter[] {
            new SqlParameter{ParameterName="@procId",Value=32},
            new SqlParameter{ParameterName="@Id",Value=rollId}
            };
            var _proc = @"proc_GetDataForDropDownList @procId,@Id";
            var slist = this.Database.SqlQuery<FAPAppProcessModel>(_proc, sqlparam).ToList();
            return slist;
        }
        public List<DropDownList> binddesignation()
        {
            var sqlparam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=31}
            };
            var _proc = @"proc_GetDataForDropDownList @procId";
            var slist = this.Database.SqlQuery<DropDownList>(_proc, sqlparam).ToList();
            return slist;
        }
        #endregion

        #region DIC
        public ProcessType getDICprocessCount(long userId)
        {
            var sqlParam = new SqlParameter[] { 
                new SqlParameter{ParameterName="@userId",Value=userId} 
            };
            var _proc = @"proc_DIC_countProcess @userId";
            var slist = this.Database.SqlQuery<ProcessType>(_proc, sqlParam).FirstOrDefault();
            return slist;

        }
        public DICModel GetDICCertificate(long regisId = 0)
        {
            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisId} 
             
            };
            var _proc = @"proc_getDICCertificateNo @regisId";
            var slist = this.Database.SqlQuery<DICModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public List<DICModel> GetAllRegistrationDIC(long regisId = 0)
        {
            var sqlParam = new SqlParameter[] { 
                new SqlParameter{ParameterName="@regisId",Value=regisId} 
            };
            var _proc = @"proc_GetAllRegistrationDIC @regisId";
            var slist = this.Database.SqlQuery<DICModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public List<DICModel> GetAllDICList(int procId, long regisDICId, string registration, string mobile, string requestdDate, int status)
        {
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=procId},
            new SqlParameter{ParameterName="@regisDICId",Value=@regisDICId},
            new SqlParameter{ParameterName="@registration",Value=registration},
            new SqlParameter{ParameterName="@mobile",Value=mobile},
            new SqlParameter{ParameterName="@requestDate",Value=requestdDate},
             new SqlParameter{ParameterName="@status",Value=status}
            };
            var _proc = @"proc_getAllDIC @procId,@regisDICId,@registration,@mobile,@requestDate,@status";
            var slist = this.Database.SqlQuery<DICModel>(_proc, sqlparams).ToList();
            return slist;

        }
        public List<DICModel> GetAllDICInProcessList(int procId, long regisDICId, string registration, string mobile, string requestdDate, int status)
        {
            var sqlparams = new SqlParameter[] { 
            new SqlParameter{ParameterName="@procId",Value=procId},
            new SqlParameter{ParameterName="@regisDICId",Value=regisDICId},



            new SqlParameter{ParameterName="@registration",Value=registration},
            new SqlParameter{ParameterName="@mobile",Value=mobile},
            new SqlParameter{ParameterName="@requestDate",Value=requestdDate},
             new SqlParameter{ParameterName="@status",Value=status}
            };
            var _proc = @"proc_getAllDICInProcess @procId,@regisDICId,@registration,@mobile,@requestDate,@status";
            var slist = this.Database.SqlQuery<DICModel>(_proc, sqlparams).ToList();
            return slist;

        }
        public List<DICModel> GetAllDICApprovedList(int procId, string registration, string mobile, string requestdDate)
        {
            var sqlparams = new SqlParameter[] { 
             new SqlParameter{ParameterName="@procId",Value=procId},
            new SqlParameter{ParameterName="@registration",Value=registration},
            new SqlParameter{ParameterName="@mobile",Value=mobile},
            new SqlParameter{ParameterName="@requestDate",Value=requestdDate}
            };
            var _proc = @"proc_getAllDICApproved @procId,@registration,@mobile,@requestDate";
            var slist = this.Database.SqlQuery<DICModel>(_proc, sqlparams).ToList();
            return slist;

        }
        public List<DICModel> GetAllDICRejectedList(int procId, string registration, string mobile, string requestdDate)
        {
            var sqlparams = new SqlParameter[] { 
             new SqlParameter{ParameterName="@procId",Value=procId},
            new SqlParameter{ParameterName="@registration",Value=registration},
            new SqlParameter{ParameterName="@mobile",Value=mobile},
            new SqlParameter{ParameterName="@requestDate",Value=requestdDate}
            };
            var _proc = @"proc_getAllDICRejected @procId,@registration,@mobile,@requestDate";
            var slist = this.Database.SqlQuery<DICModel>(_proc, sqlparams).ToList();
            return slist;

        }

        public List<DICModel> GetScheduleOfCommitteeDIC(string inspectionDate, long userId)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@userId",Value=userId}  ,
             new SqlParameter{ParameterName="@inspectionDate",Value=inspectionDate}  
            };
            var _proc = @"proc_GetScheduleOfCommitteeDIC @userId,@inspectionDate";
            var slist = this.Database.SqlQuery<DICModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public ResultSet UpdateAppProcessDIC(DICAppProcessModel model)
        {
            var sqlParam = new SqlParameter[] { 
               new SqlParameter("@regisIdDIC", model.regisIdDIC),
               new SqlParameter("@appStatus", model.appStatus),
               //new SqlParameter("@committeeId", model.committeeId),
               new SqlParameter("@inspectionDate", string.IsNullOrEmpty(model.inspectionDate) ? (object)DBNull.Value : Convert.ToDateTime(model.inspectionDate)),
               new SqlParameter("@inspReportFilePath", model.inspReportFilePath ?? string.Empty),
               new SqlParameter("@certificateFilePath", model.certificateFilePath ?? string.Empty),
               new SqlParameter("@rejectedRemarks", model.rejectedRemarks ?? string.Empty),
               new SqlParameter("@forwardedType", model.forwardedType),
               new SqlParameter("@forwardedTo", model.forwardedTo),
               new SqlParameter("@testTypeId", model.testTypeId),
               new SqlParameter("@isLessFourtyPer", model.isLessFourtyPer),
               new SqlParameter("@xmlCommiMembers", model.xmlCommiMembers??string.Empty),
               new SqlParameter("@userId", model.userId),
               new SqlParameter("@transIp", model.transIp),
               new SqlParameter("@disabilityPer", model.disabilityPer),
               new SqlParameter("@markOfIdentification", model.markOfIdentification??string.Empty),
               new SqlParameter("@conditionId", model.conditionId),
               new SqlParameter("@reassId", model.reassId),
               new SqlParameter("@reassPeriod", model.reassPeriod),
               new SqlParameter("@reassPeriodType", model.reassPeriodType??string.Empty),
               new SqlParameter("@xmlPhotoData", model.XmlDataPhoto??string.Empty),
               
            };
            var _proc = @"proc_UpdateAppProcessDIC @regisIdDIC,@appStatus,@inspectionDate,@inspReportFilePath,@certificateFilePath,@rejectedRemarks,@forwardedType,@forwardedTo,@testTypeId,@isLessFourtyPer,@xmlCommiMembers,@userId,@transIp,@disabilityPer,@markOfIdentification,@conditionId,@reassId,@reassPeriod,@reassPeriodType,@xmlPhotoData";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }

        public List<DICCertificateDetailModel> GetCertificateDetialDIC(long regisIdDIC)
        {
            var sqlParam = new SqlParameter[] { 
               new SqlParameter("@regisIdDIC", regisIdDIC)
            };
            var _proc = @"proc_GetCertificateDetialDIC @regisIdDIC";
            var slist = this.Database.SqlQuery<DICCertificateDetailModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public int InsertUnSignedCertiPath_DIC(long regisId, string certificateFilePath)
        {
            var sqlParam = new SqlParameter[] { 
                 new SqlParameter{ParameterName="@regisIdDIC",Value=regisId},  
                 new SqlParameter{ParameterName="@certificateFilePath",Value=certificateFilePath}  
            };
            var _proc = @"proc_InsertUnSignedCertiPath_DIC @regisIdDIC,@certificateFilePath";
            var result = this.Database.ExecuteSqlCommand(_proc, sqlParam);
            return result;
        }

        public ResultSet UpdateDICCertificate(long regisId, string certificatePath, long regisBy, string Ipaddress)
        {
            var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@regisId",Value=regisId}  ,
              new SqlParameter{ParameterName="@certificatePath",Value=certificatePath} ,
              new SqlParameter{ParameterName="@regisBy",Value=regisBy}  ,
              new SqlParameter{ParameterName="@Ipaddress",Value=Ipaddress} 
             
            };
            var _proc = @"proc_DICUpdate @regisId,@certificatePath,@regisBy ,@Ipaddress";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }
        public List<committeModelDIC> GetCommitteeMemberDIC(long inprocessDICId)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@inprocessDICId",Value=inprocessDICId} 
             };
            var _proc = @"Proc_GetCommitteeMemberDIC @inprocessDICId";
            var slist = this.Database.SqlQuery<committeModelDIC>(_proc, sqlParam).ToList();
            return slist;
        }
        #endregion
        #endregion


        #region Riya
        #region AGC
        public ProcessType getApplicationCountAGC(long userId)
        {
            var sqlParam = new SqlParameter[] { 
                new SqlParameter{ParameterName="@userId",Value=userId} 
            };
            var _proc = @"proc_AGC_countProcess @userId";
            var slist = this.Database.SqlQuery<ProcessType>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }

        public List<AGCModel> GetAllAGCList(long regisId = 0)
        {
            var sqlParam = new SqlParameter[] { 
                new SqlParameter{ParameterName="@regisId",Value=regisId} 
            };
            var _proc = @"proc_getAllAGC_Details @regisId";
            var slist = this.Database.SqlQuery<AGCModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public AGCAppProcessModel GetAGCList(string regisId, int status)
        {

            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@registrationNo",Value=regisId},
            new SqlParameter{ParameterName="@appliedStatus",Value=status}
            };
            var _proc = @"getAGCforUpDateApplication @registrationNo,@appliedStatus";
            var slist = this.Database.SqlQuery<AGCAppProcessModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public ResultSet AGCAppStatusInsertUpdate(long regisIdAGC, int appStatus, bool status, string rejectedRemarks, string xRayPlateNo, string xRayDate, string dentalPlateNo, string markOfIdentification, string age, string inspectionDate, string inspectionRptPath, long regByUser, string transIp, int forwardtype, long forwardto, string XmlData, string XmlDataPhoto)
        {
            var sqlparams = new SqlParameter[] { 
                    new  SqlParameter{ParameterName="@regisIdAGC",Value=regisIdAGC},
                    new  SqlParameter{ParameterName="@appStatus",Value=appStatus},
                    new  SqlParameter{ParameterName="@status",Value=status},
                    new  SqlParameter{ParameterName="@rejectedRemark",Value=rejectedRemarks??string.Empty},
                    //new  SqlParameter{ParameterName="@committeeId",Value=committeeId},
                    new  SqlParameter{ParameterName="@xRayPlateNo",Value=xRayPlateNo??string.Empty},
                    new  SqlParameter{ParameterName="@xRayDate",Value=xRayDate??string.Empty},
                    new  SqlParameter{ParameterName="@dentalPlateNo",Value=dentalPlateNo??string.Empty},
                    new  SqlParameter{ParameterName="@markOfIdentification",Value=markOfIdentification??string.Empty},
                    new  SqlParameter{ParameterName="@age",Value=age??string.Empty},
                    new  SqlParameter{ParameterName="@inspectionDate",Value=inspectionDate??string.Empty},
                    new  SqlParameter{ParameterName="@inspectionRptPath",Value=inspectionRptPath??string.Empty},                    
                    new  SqlParameter{ParameterName="@userId",Value=regByUser},
                    new  SqlParameter{ParameterName="@transIp",Value=transIp},
                    new  SqlParameter{ParameterName="@forwardTypeId",Value=forwardtype},
                    new  SqlParameter{ParameterName="@forwardto",Value=forwardto},
                    new  SqlParameter{ParameterName="@XmlData",Value=XmlData??string.Empty},

                    new  SqlParameter{ParameterName="@xmldataPhoto",Value=XmlDataPhoto??string.Empty}
         
            };

            var _proc = @"AppStatusUpdateAGC @regisIdAGC,@appStatus, @status, @rejectedRemark,@xRayPlateNo,@xRayDate,@dentalPlateNo,@markOfIdentification, @age, @inspectionDate, @inspectionRptPath,@userId, @transIp, @forwardTypeId,@forwardto, @xmldata,@xmldataPhoto";

            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }

        public AGCModel GetAGCListBYRegistrationNo(string registrationNo)
        {

            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@registrationNo",Value=registrationNo}
            };
            var _proc = @"GetViewOfAGC @registrationNo";
            var slist = this.Database.SqlQuery<AGCModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public List<AGCcommitteeInfo> GetScheduleOfCommitteeAGC(string inspectionDate, long UserID)
        {
            var sqlParam = new SqlParameter[] { 
             new SqlParameter{ParameterName="@UserID",Value=UserID}  ,
             new SqlParameter{ParameterName="@inspectionDate",Value=inspectionDate} 
             
            };
            var _proc = @"proc_GetScheduleOfCommitteeAGC @UserID,@inspectionDate";
            var slist = this.Database.SqlQuery<AGCcommitteeInfo>(_proc, sqlParam).ToList();
            return slist;
        }
        public List<AGCReportModel> GetCertificateDetialAGC(long regisAGCId)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@regisId",Value=regisAGCId} 
             };
            var _proc = @"proc_rptAGC_Certificate @regisId";
            var slist = this.Database.SqlQuery<AGCReportModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public int InsertUnSignedCertiPath_AGC(long regisId, string certificateFilePath)
        {
            var sqlParam = new SqlParameter[] { 
                 new SqlParameter{ParameterName="@regisIdAGC",Value=regisId},  
                 new SqlParameter{ParameterName="@certificateFilePath",Value=certificateFilePath}  
            };
            var _proc = @"proc_InsertUnSignedCertiPath_AGC @regisIdAGC,@certificateFilePath";
            var result = this.Database.ExecuteSqlCommand(_proc, sqlParam);
            return result;
        }
        public List<committeModel> GetCommitteeMemberAGC(long inprocessAGCId)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@inprocessAGCId",Value=inprocessAGCId} 
             };
            var _proc = @"Proc_GetCommitteeMemberAGC @inprocessAGCId";
            var slist = this.Database.SqlQuery<committeModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public AGCModel GetAGCCertificate(long regisId = 0)
        {
            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisId} 
             
            };
            var _proc = @"proc_getAGCCertificateNo @regisId";
            var slist = this.Database.SqlQuery<AGCModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public ResultSet UpdateAGCCertificate(long regisId, string certificatePath, long regisBy, string Ipaddress)
        {
            var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@regisId",Value=regisId}  ,
              new SqlParameter{ParameterName="@certificatePath",Value=certificatePath} ,
              new SqlParameter{ParameterName="@regisBy",Value=regisBy}  ,
              new SqlParameter{ParameterName="@Ipaddress",Value=Ipaddress} 
             
            };
            var _proc = @"proc_AGCUpdate @regisId,@certificatePath,@regisBy ,@Ipaddress";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }
        #endregion
        #region MER
        public ProcessType getApplicationCountMER(long userId, string ROLL)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@userId",Value=userId} ,
             new SqlParameter{ParameterName="@ROLL",Value=ROLL} 
            };

            var _proc = @"proc_MER_countProcess @userId,@ROLL";
            var slist = this.Database.SqlQuery<ProcessType>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }

        public List<MERstatusUpdationModel> GetAllMERList(long userId, string ROLL, long regisId = 0)
        {
            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@userId",Value=userId} ,
            new SqlParameter{ParameterName="@ROLL",Value=ROLL},
            new SqlParameter{ParameterName="@regisId",Value=regisId}
            };

            var _proc = @"proc_getAllMER_Details @userId,@ROLL,@regisId";
            var slist = this.Database.SqlQuery<MERstatusUpdationModel>(_proc, sqlParam).ToList();
            return slist;
        }

        public MERstatusUpdationModel GetMERList(string regisId, int status)
        {

            var sqlParam = new SqlParameter[] { 
            new SqlParameter{ParameterName="@registrationNo",Value=regisId},
            new SqlParameter{ParameterName="@appliedStatus",Value=status}
            };
            var _proc = @"getMERforUpDateApplication @registrationNo,@appliedStatus";
            var slist = this.Database.SqlQuery<MERstatusUpdationModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public List<MERstatusUpdationModel> GetScheduleOfinspectonMER(string inspectiondate)
        {
            var sqlParam = new SqlParameter[] {
             new SqlParameter{ParameterName="@inspectiondate",Value=inspectiondate}
             
            };
            var _proc = @"proc_GetScheduleOfCommitteeMER @inspectiondate";
            var slist = this.Database.SqlQuery<MERstatusUpdationModel>(_proc, sqlParam).ToList();
            return slist;
        }
        public ResultSet MERAppStatusInsertUpdate(long regisIdMER, int appStatus, string rejectedRemarks, string inspectionDate, string departmentName, string officerName, string dateOfLetter, string letterNo, string inspectionRptPath, string inspectionRejectedRemark, decimal? sancationAmount, string sancationDate, long regByUser, string transIp, string xml, string xmlDataPhoto)
        {
            var sqlparams = new SqlParameter[] { 
                    new  SqlParameter{ParameterName="@regisIdMER",Value=regisIdMER},
                    new  SqlParameter{ParameterName="@appStatus",Value=appStatus},
                    new  SqlParameter{ParameterName="@appRejectedRemark",Value=(rejectedRemarks==null)?"":rejectedRemarks},
                    new  SqlParameter{ParameterName="@inspectionDate",Value=(inspectionDate==null)?"":inspectionDate},

                    new  SqlParameter{ParameterName="@departmentName",Value=(departmentName==null)?"":departmentName},
                    new  SqlParameter{ParameterName="@officerName",Value=(officerName==null)?"":officerName},
                    new  SqlParameter{ParameterName="@dateOfLetter",Value=(dateOfLetter==null)?"":dateOfLetter},
                    new  SqlParameter{ParameterName="@letterNo",Value=(letterNo==null)?"":letterNo},

                    new  SqlParameter{ParameterName="@inspectionRptPath",Value=(inspectionRptPath==null)?"":inspectionRptPath},
                    new  SqlParameter{ParameterName="@inspectionRejectedRemark",Value=(inspectionRejectedRemark==null)?"":inspectionRejectedRemark},
                    new  SqlParameter{ParameterName="@sancationAmount",Value=(sancationAmount==null)?0:sancationAmount},
                    new  SqlParameter{ParameterName="@sancationDate",Value=(sancationDate==null)?"":sancationDate},
                    new  SqlParameter{ParameterName="@appRejectedAcceptedBy",Value=regByUser},
                    new  SqlParameter{ParameterName="@transIp",Value=transIp},
                   new  SqlParameter{ParameterName="@xml",Value=xml},

                   new  SqlParameter{ParameterName="@xmldataPhoto",Value=xmlDataPhoto},

                   
         
            };

            var _proc = @"AppStatusUpdateMER @regisIdMER,@appStatus,@appRejectedRemark,@inspectionDate,@departmentName,@officerName,@dateOfLetter,@letterNo,@inspectionRptPath,@inspectionRejectedRemark,@sancationAmount,@sancationDate,@appRejectedAcceptedBy,@transIp,@xml,@xmldataPhoto";

            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlparams).FirstOrDefault();
            return slist;
        }
        public List<MERrptModel> GetMERdetailCertificateRpt(long userId)
        {
            var sqlparams = new SqlParameter[] { 
            
           
            new SqlParameter{ParameterName="@regisIdMER",Value=userId}
             };
            var _proc = @"GetMERdetailForCertificateRpt  @regisIdMER";
            var slist = this.Database.SqlQuery<MERrptModel>(_proc, sqlparams).ToList();
            return slist;
        }

        public int InsertUnSignedCertiPath_MER(long regisId, string certificateFilePath)
        {
            var sqlParam = new SqlParameter[] { 
                 new SqlParameter{ParameterName="@regisIdMER",Value=regisId},  
                 new SqlParameter{ParameterName="@certificateFilePath",Value=certificateFilePath}  
            };
            var _proc = @"proc_InsertUnSignedCertiPath_MER @regisIdMER,@certificateFilePath";
            var result = this.Database.ExecuteSqlCommand(_proc, sqlParam);
            return result;
        }

        public MERModel GetMERCertificate(long regisId = 0)
        {
            var sqlParam = new SqlParameter[] { 
           
             new SqlParameter{ParameterName="@regisId",Value=regisId} 
             
            };
            var _proc = @"proc_getMERCertificateNo @regisId";
            var slist = this.Database.SqlQuery<MERModel>(_proc, sqlParam).SingleOrDefault();
            return slist;
        }
        public ResultSet UpdateMERCertificate(long regisId, string certificatePath, long regisBy, string Ipaddress)
        {
            var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@regisId",Value=regisId}  ,
              new SqlParameter{ParameterName="@certificatePath",Value=certificatePath} ,
              new SqlParameter{ParameterName="@regisBy",Value=regisBy}  ,
              new SqlParameter{ParameterName="@Ipaddress",Value=Ipaddress} 
             
            };
            var _proc = @"proc_MERUpdate @regisId,@certificatePath,@regisBy ,@Ipaddress";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }
        #endregion
        #endregion
        //PARAM
        public List<MERModel> getMERChild(long regisId)
        {
            var sqlparams = new SqlParameter[] { 
            
            new SqlParameter{ParameterName="@regisId",Value=regisId}
           
             };
            var _proc = @"proc_getMERChild @regisId";
            var slist = this.Database.SqlQuery<MERModel>(_proc, sqlparams).ToList();
            return slist;
        }

        public List<FAPAppProcessModel> bindDropdownlistForAuditFilters(long rollId)
        {
            var sqlparam = new SqlParameter[] {
            new SqlParameter{ParameterName="@procId",Value=64},
            new SqlParameter{ParameterName="@Id",Value=rollId}
            };
            var _proc = @"proc_GetDataForDropDownList @procId,@Id";
            var slist = this.Database.SqlQuery<FAPAppProcessModel>(_proc, sqlparam).ToList();
            return slist;
        }

        #region Revert Certificate Generation NUH
        public ResultSet RevertCertificateGeneration(long regisId, string Ipaddress, long regisBy)
        {
            var sqlParam = new SqlParameter[] { 
              new SqlParameter{ParameterName="@regisIdNUH",Value=regisId}    ,
              new SqlParameter{ParameterName="@Ipaddress",Value=Ipaddress} , 
              new SqlParameter{ParameterName="@regisBy",Value=regisBy} 
            };
            var _proc = @"proc_RevertCertificateGeneration_NUH @regisIdNUH,@Ipaddress,@regisBy";
            var slist = this.Database.SqlQuery<ResultSet>(_proc, sqlParam).FirstOrDefault();
            return slist;
        }
        #endregion

        #region Get All Cert Validity By MeeRegisNo.
        public List<NUHDetailsModel> GetAllCertValidityByMeeRegisNo(string meeRegisNo)
        {
            var sqlParam = new SqlParameter[] {  
             new SqlParameter{ParameterName="@meeRegisNo",Value=meeRegisNo}  
            };
            var _proc = @"proc_GetAllCertValidityByMeeRegisNo_NUH @meeRegisNo";
            var slist = this.Database.SqlQuery<NUHDetailsModel>(_proc, sqlParam).ToList();
            return slist;
        }
        #endregion

     
    }
}