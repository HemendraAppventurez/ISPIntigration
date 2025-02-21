using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using CCSHealthFamilyWelfareDept.DAL;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using CCSHealthFamilyWelfareDept.Models;

namespace CCSHealthFamilyWelfareDept.WebServices
{
    /// <summary>
    /// Summary description for FeatchNUHDataForSigning
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class FeatchNUHDataForSigning : System.Web.Services.WebService
    {
        NUH_DB objNUHDB = new NUH_DB();
        Service_DB db = new Service_DB();
        CMO_DB objCMODB = new CMO_DB();
        CHC_DB objCHCDB = new CHC_DB();
        Common_DB objComnDB = new Common_DB();

        public UserCredentials consumer;

        private DataTable checkConsumer()
        {
            DataTable dt = new DataTable();

            if (consumer != null)
            {
                dt = db.WebServiceAuthentication(consumer.userName, consumer.password, consumer.appVersion); 
            }
            else
            {
                dt.Clear();
                dt.Columns.Add("isValid");
                dt.Columns.Add("msg");
                DataRow _ravi = dt.NewRow();
                _ravi["isValid"] = "0";
                _ravi["msg"] = "Sorry, You are not Authorise for this.";
                dt.Rows.Add(_ravi);
            }

            dt.TableName = "InvalidAuth";
            return dt;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapHeader("consumer", Required = true)]
        public DataTable GetLoginDetails(string userName, string Password)
        {
            DataTable dt = new DataTable();
            int isValid = 0;
            dt = checkConsumer();

            if (dt != null && dt.Rows.Count > 0)
            {
                isValid = Convert.ToInt32(dt.Rows[0]["isValid"]);
            }

            if (isValid == 1)
            {
                dt = db.GetLoginDetails(userName, Password);
                dt.TableName = "LoginDetails";
            }

            return dt;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapHeader("consumer", Required = true)]
        public DataTable SaveSerialNo(string serialNo, long profileId)
        {
            DataTable dt = new DataTable();
            int isValid = 0;
            dt = checkConsumer();

            if (dt != null && dt.Rows.Count > 0)
            {
                isValid = Convert.ToInt32(dt.Rows[0]["isValid"]);
            }

            if (isValid == 1)
            {
                dt = db.RegisterCertificateSerialNo(profileId, serialNo);
                dt.TableName = "CertificateDetails";
            }

            return dt;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapHeader("consumer", Required = true)]
        public DataTable GetSignatureDetails(string serviceCode)
        {
            DataTable dt = new DataTable();
            int isValid = 0;
            dt = checkConsumer();

            if (dt != null && dt.Rows.Count > 0)
            {
                isValid = Convert.ToInt32(dt.Rows[0]["isValid"]);
            }

            if (isValid == 1)
            {
                dt = db.GetSignatureDetails(serviceCode);
                dt.TableName = "SignatureDetails";
            }

            return dt;
        } 

        [WebMethod]
        [System.Web.Services.Protocols.SoapHeader("consumer", Required = true)]
        public DataTable GetUnsignCertificateData(string userId, string serviceCode)
        { 
            DataTable dt = new DataTable();
            int isValid = 0;
            dt = checkConsumer();

            if (dt != null && dt.Rows.Count > 0)
            {
                isValid = Convert.ToInt32(dt.Rows[0]["isValid"]);
            }

            if (isValid == 1)
            { 
                long profileId = Convert.ToInt64(userId);

                if (serviceCode == "NUH")
                {
                    dt = GenerateUnsignedCerti_NUH(profileId);
                }
                else if (serviceCode == "DIC")
                {
                    dt = GenerateUnsignedCerti_DIC(profileId);
                }
                else if (serviceCode == "AGC")
                {
                    dt = GenerateUnsignedCerti_AGC(profileId);
                }
                else if (serviceCode == "MER")
                {
                    dt = GenerateUnsignedCerti_MER(profileId);
                }
                else if (serviceCode == "FIC")
                {
                    dt = GenerateUnsignedCerti_FIC(profileId);
                }
                else if (serviceCode == "ICC")
                {
                    dt = GenerateUnsignedCerti_ICC(profileId);
                }
                else if (serviceCode == "ILC")
                {
                    dt = GenerateUnsignedCerti_ILC(profileId);
                }
                else if (serviceCode == "IMC")
                {
                    dt = GenerateUnsignedCerti_IMC(profileId);
                }
                else if (serviceCode == "MLC")
                {
                    dt = GenerateUnsignedCerti_MLC(profileId);
                }
                else if (serviceCode == "DEC")
                {
                    dt = GenerateUnsignedCerti_DEC(profileId);
                }
            }

            return dt;
        }

        private DataTable GenerateUnsignedCerti_NUH(long userId)
        {
            string webUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');

            DataTable dt = db.GetUnsignedCertificateDetails(userId, webUrl, "proc_GetUnsignedCerti_NUH");

            dt.TableName = "CertificateList";
            return dt;
        }

        private DataTable GenerateUnsignedCerti_DIC(long userId)
        {
            string webUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');

            DataTable dt = db.GetUnsignedCertificateDetails(userId, webUrl, "proc_GetUnsignedCerti_DIC");

            dt.TableName = "CertificateList";
            return dt;
        }

        private DataTable GenerateUnsignedCerti_AGC(long userId)
        {
            string webUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');

            DataTable dt = db.GetUnsignedCertificateDetails(userId, webUrl, "proc_GetUnsignedCerti_AGC");

            dt.TableName = "CertificateList";
            return dt;
        }

        private DataTable GenerateUnsignedCerti_MER(long userId)
        {
            string webUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');

            DataTable dt = db.GetUnsignedCertificateDetails(userId, webUrl, "proc_GetUnsignedCerti_MER");

            dt.TableName = "CertificateList";
            return dt;
        }

        private DataTable GenerateUnsignedCerti_FIC(long userId)
        {
            string webUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');

            DataTable dt = db.GetUnsignedCertificateDetails(userId, webUrl, "proc_GetUnsignedCerti_FIC");

            dt.TableName = "CertificateList";
            return dt;
        }

        private DataTable GenerateUnsignedCerti_ICC(long userId)
        {
            string webUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');

            DataTable dt = db.GetUnsignedCertificateDetails(userId, webUrl, "proc_GetUnsignedCerti_ICC");

            dt.TableName = "CertificateList";
            return dt;
        }

        private DataTable GenerateUnsignedCerti_ILC(long userId)
        {
            string webUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');

            DataTable dt = db.GetUnsignedCertificateDetails(userId, webUrl, "proc_GetUnsignedCerti_ILC");

            dt.TableName = "CertificateList";
            return dt;
        }

        private DataTable GenerateUnsignedCerti_IMC(long userId)
        {
            string webUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');

            DataTable dt = db.GetUnsignedCertificateDetails(userId, webUrl, "proc_GetUnsignedCerti_IMC");

            dt.TableName = "CertificateList";
            return dt;
        }

        private DataTable GenerateUnsignedCerti_MLC(long userId)
        {
            string webUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');

            DataTable dt = db.GetUnsignedCertificateDetails(userId, webUrl, "proc_GetUnsignedCerti_MLC");

            dt.TableName = "CertificateList";
            return dt;
        }

        private DataTable GenerateUnsignedCerti_DEC(long userId)
        {
            string webUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');

            DataTable dt = db.GetUnsignedCertificateDetails(userId, webUrl, "proc_GetUnsignedCerti_DEC");

            dt.TableName = "CertificateList";
            return dt;
        }

        DataTable DefineTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("applicationId", typeof(Int64));
            dt.Columns.Add("CerFilePath", typeof(string));
            dt.Columns.Add("OrgCerFilePath", typeof(string));
            dt.Columns.Add("certificateNo", typeof(string));
            return dt;
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapHeader("consumer", Required = true)]
        public bool UpdateSignedCertificate(long RegId, byte[] fileData, string unsignFileUrl, long userId, string DistName, string serviceCode)
        {
            DataTable dt = new DataTable();
            int isValid = 0;
            dt = checkConsumer();

            if (dt != null && dt.Rows.Count > 0)
            {
                isValid = Convert.ToInt32(dt.Rows[0]["isValid"]);
            }

            if (isValid == 1)
            {
                string folderpath = "~/Content/writereaddata/SignedCertificate/" + serviceCode + "/" + DistName + "/";

                if (!System.IO.Directory.Exists(Server.MapPath(folderpath)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(folderpath));
                }

                string fileName = "Signed_" + Path.GetFileName(unsignFileUrl).Split('_').Last();

                string SignFileUrl = folderpath + fileName;

                bool res = false;

                try
                {
                    //    if (System.IO.File.Exists(Server.MapPath(unsignFileUrl)))
                    //    {
                    //        System.IO.File.Delete(Server.MapPath(unsignFileUrl));
                    //    }
                    File.WriteAllBytes(Server.MapPath(SignFileUrl), fileData);
                }
                catch { }
                string IPAddress = Common.GetIPAddress();

                ResultSet resultData = new ResultSet();

                if (serviceCode == "NUH")
                {
                    resultData = objCMODB.UpdateNUHCertificate(RegId, SignFileUrl, userId, IPAddress);
                    if (resultData.Flag == 1)
                    {
                        objCMODB.SendStausToNiveshwithBinaryFormat(RegId, resultData.RegisterByuserID, SignFileUrl);
                    }
                }
                else if (serviceCode == "DIC")
                {
                    resultData = objCMODB.UpdateDICCertificate(RegId, SignFileUrl, userId, IPAddress);
                }
                else if (serviceCode == "AGC")
                {
                    resultData = objCMODB.UpdateAGCCertificate(RegId, SignFileUrl, userId, IPAddress);
                }
                else if (serviceCode == "MER")
                {
                    resultData = objCMODB.UpdateMERCertificate(RegId, SignFileUrl, userId, IPAddress);
                }
                else if (serviceCode == "FIC")
                {
                    resultData = objCHCDB.UpdateFICCertificate(RegId, SignFileUrl, userId, IPAddress);
                }
                else if (serviceCode == "ICC")
                {
                    resultData = objCHCDB.UpdateICCCertificate(RegId, SignFileUrl, userId, IPAddress);
                }
                else if (serviceCode == "ILC")
                {
                    resultData = objCHCDB.UpdateILCCertificate(RegId, SignFileUrl, userId, IPAddress);
                }
                else if (serviceCode == "IMC")
                {
                    resultData = objCHCDB.UpdateIMCCertificate(RegId, SignFileUrl, userId, IPAddress);
                }
                else if (serviceCode == "MLC")
                {
                    resultData = objCHCDB.UpdateMLCCertificate(RegId, SignFileUrl, userId, IPAddress);
                }
                else if (serviceCode == "DEC")
                {
                    resultData = objCHCDB.UpdateDECCertificate(RegId, SignFileUrl, userId, IPAddress);
                }

                return res = resultData.Flag == 0 ? false : true;
            }
            else
            {
                return false;
            }
        }
    }
}
