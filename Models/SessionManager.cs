using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class SessionManager
    {
        public void RemoveSession()
        {
            try
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.RemoveAll();
            }
            catch
            { }
        }


       // Created By vk


        public string ReportName
        {
            get
            {
                if (HttpContext.Current.Session["ReportName"] != null)
                {
                    return HttpContext.Current.Session["ReportName"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["ReportName"] = value;
            }
        }
        public string ForwardedTypeName
        {
            get
            {
                if (HttpContext.Current.Session["ForwardedTypeName"] != null)
                {
                    return HttpContext.Current.Session["ForwardedTypeName"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["ForwardedTypeName"] = value;
            }
        }
        public int AppTypeID
        {
            get
            {
                if (HttpContext.Current.Session["AppTypeID"] != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["AppTypeID"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session["AppTypeID"] = value;
            }
        }

        public string FromDate
        {
            get
            {
                if (HttpContext.Current.Session["FromDate"] != null)
                {
                    return HttpContext.Current.Session["FromDate"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["FromDate"] = value;
            }
        }

        public string ToDate
        {
            get
            {
                if (HttpContext.Current.Session["ToDate"] != null)
                {
                    return HttpContext.Current.Session["ToDate"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["ToDate"] = value;
            }
        }
        public string RDistrictID
        {
            get
            {
                if (HttpContext.Current.Session["RDistrictID"] != null)
                {
                    return HttpContext.Current.Session["RDistrictID"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["RDistrictID"] = value;
            }
        }


        public string Username
        {
            get
            {
                if (HttpContext.Current.Session["username"] != null)
                {
                    return HttpContext.Current.Session["username"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["username"] = value;
            }
        }

        //Changes
        public string StructuralLayOut
        {
            get
            {
                if (HttpContext.Current.Session["StructuralLayOut"] != null)
                {
                    return HttpContext.Current.Session["StructuralLayOut"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["StructuralLayOut"] = value;
            }
        }
        public string ElectricityBills
        {
            get
            {
                if (HttpContext.Current.Session["ElectricityBills"] != null)
                {
                    return HttpContext.Current.Session["ElectricityBills"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["ElectricityBills"] = value;
            }
        }

        public string Registries  //shashi
        {
            get
            {
                if (HttpContext.Current.Session["Registries"] != null)
                {
                    return HttpContext.Current.Session["Registries"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["Registries"] = value;
            }
        }
        public string RentalAgreements
        {
            get
            {
                if (HttpContext.Current.Session["RentalAgreements"] != null)
                {
                    return HttpContext.Current.Session["RentalAgreements"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["RentalAgreements"] = value;
            }
        }

        public string AddressProofFile
        {
            get
            {
                if (HttpContext.Current.Session["AddressProofFile"] != null)
                {
                    return HttpContext.Current.Session["AddressProofFile"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["AddressProofFile"] = value;
            }
        }

        public string PicPhotoGraph
        {
            get
            {
                if (HttpContext.Current.Session["PicPhotoGraph"] != null)
                {
                    return HttpContext.Current.Session["PicPhotoGraph"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["PicPhotoGraph"] = value;
            }
        }

        public string UpMCiCerficate
        {
            get
            {
                if (HttpContext.Current.Session["UpMCiCerficate"] != null)
                {
                    return HttpContext.Current.Session["UpMCiCerficate"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["UpMCiCerficate"] = value;
            }
        }

        public string OwnerFPhotoGrapf
        {
            get
            {
                if (HttpContext.Current.Session["OwnerFPhotoGrapf"] != null)
                {
                    return HttpContext.Current.Session["OwnerFPhotoGrapf"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["OwnerFPhotoGrapf"] = value;
            }
        }

        public string OwnerFSignature
        {
            get
            {
                if (HttpContext.Current.Session["OwnerFSignature"] != null)
                {
                    return HttpContext.Current.Session["OwnerFSignature"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["OwnerFSignature"] = value;
            }
        }

       

        public Int64 UserID
        {
            get
            {
                if (HttpContext.Current.Session["userID"] != null)
                {
                    return Convert.ToInt64(HttpContext.Current.Session["userID"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session["userID"] = value;
            }
        }

        public Int64 RollID
        {
            get
            {
                if (HttpContext.Current.Session["rollID"] != null)
                {
                    return Convert.ToInt64(HttpContext.Current.Session["rollID"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session["rollID"] = value;
            }
        }

        public string DisplayName
        {
            get
            {
                if (HttpContext.Current.Session["displayName"] != null)
                {
                    return HttpContext.Current.Session["displayName"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["displayName"] = value;
            }
        }

        public string Transdate
        {
            get
            {
                if (HttpContext.Current.Session["TransDate"] != null)
                {
                    return HttpContext.Current.Session["TransDate"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["TransDate"] = value;
            }
        }

        public bool IsSessionActive(string key)
        {
            bool flag = false;
            try
            {
                if (HttpContext.Current.Session[key] != null && HttpContext.Current.Session[key].ToString() != "")
                {
                    flag = true;
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public string SeedValue
        {
            get
            {
                if (HttpContext.Current.Session["seedValue"] != null)
                {
                    return (HttpContext.Current.Session["seedValue"]).ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["seedValue"] = value;
            }
        }

        public string OTP
        {
            get
            {
                if (HttpContext.Current.Session["OTP"] != null)
                {
                    return HttpContext.Current.Session["OTP"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["OTP"] = value;
            }
        }

        public string EmailId
        {
            get
            {
                if (HttpContext.Current.Session["EmailId"] != null)
                {
                    return HttpContext.Current.Session["EmailId"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["EmailId"] = value;
            }
        }



        #region Nivesh User Details

        public string ControlID
        {
            get
            {
                if (HttpContext.Current.Session["ControlID"] != null)
                {
                    return HttpContext.Current.Session["ControlID"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["ControlID"] = value;
            }
        }

        public string UnitID
        {
            get
            {
                if (HttpContext.Current.Session["UnitID"] != null)
                {
                    return HttpContext.Current.Session["UnitID"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["UnitID"] = value;
            }
        }

        public string ServiceID
        {
            get
            {
                if (HttpContext.Current.Session["ServiceID"] != null)
                {
                    return HttpContext.Current.Session["ServiceID"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["ServiceID"] = value;
            }
        }

        public string RequestID
        {
            get
            {
                if (HttpContext.Current.Session["RequestID"] != null)
                {
                    return HttpContext.Current.Session["RequestID"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["RequestID"] = value;
            }
        }
        #endregion

        public string MobileNumber
        {
            get
            {
                if (HttpContext.Current.Session["MobileNumber"] != null)
                {
                    return HttpContext.Current.Session["MobileNumber"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["MobileNumber"] = value;
            }
        }

        public bool IsEmailVerified
        {
            get
            {
                if (HttpContext.Current.Session["isEmailverified"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["isEmailverified"]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                HttpContext.Current.Session["isEmailverified"] = value;
            }
        }

        public bool isMobileVerified
        {
            get
            {
                if (HttpContext.Current.Session["IsMobile"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["IsMobile"]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                HttpContext.Current.Session["IsMobile"] = value;
            }
        }

        public string Msg
        {
            get
            {
                if (HttpContext.Current.Session["Msg"] != null)
                {
                    return HttpContext.Current.Session["Msg"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["Msg"] = value;
            }
        }

        public bool isMSGSend
        {
            get
            {
                if (HttpContext.Current.Session["isMSGSend"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["isMSGSend"]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                HttpContext.Current.Session["isMSGSend"] = value;
            }
        }

        public bool IsEmailSend
        {
            get
            {
                if (HttpContext.Current.Session["IsEmailSend"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["IsEmailSend"]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                HttpContext.Current.Session["IsEmailSend"] = value;
            }
        }

        public string ProfilePicPath
        {
            get
            {
                if (HttpContext.Current.Session["ProfilePicPath"] != null)
                {
                    return HttpContext.Current.Session["ProfilePicPath"].ToString();
                }
                else
                {
                    return "~/images/avatar-1.jpg";
                }
            }
            set
            {
                HttpContext.Current.Session["ProfilePicPath"] = value;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                if (HttpContext.Current.Session["IsAuthenticated"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["IsAuthenticated"]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                HttpContext.Current.Session["IsAuthenticated"] = value;
            }
        }

        public Int64 UserIDUserPolcy
        {
            get
            {
                if (HttpContext.Current.Session["UserIDUserPolcy"] != null)
                {
                    return Convert.ToInt64(HttpContext.Current.Session["UserIDUserPolcy"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session["UserIDUserPolcy"] = value;
            }
        }

        public bool IsMaxLimit
        {
            get
            {
                if (HttpContext.Current.Session["IsMaxLimit"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["IsMaxLimit"]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                HttpContext.Current.Session["IsMaxLimit"] = value;
            }
        }

        public bool IsLoginUser
        {
            get
            {
                if (HttpContext.Current.Session["IsLoginUser"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["IsLoginUser"]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                HttpContext.Current.Session["IsLoginUser"] = value;
            }
        }

        public string AppRequestKey
        {
            get
            {
                if (HttpContext.Current.Session["AppRequestKey"] != null)
                {
                    return HttpContext.Current.Session["AppRequestKey"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["AppRequestKey"] = value;
            }
        }

        public string DistrictName
        {
            get
            {
                if (HttpContext.Current.Session["districtName"] != null)
                {
                    return HttpContext.Current.Session["districtName"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["districtName"] = value;
            }
        }



        public string DisignationName
        {
            get
            {
                if (HttpContext.Current.Session["designation"] != null)
                {
                    return HttpContext.Current.Session["designation"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["designation"] = value;
            }
        }

        public string DistrictNameHi
        {
            get
            {
                if (HttpContext.Current.Session["districtNameHi"] != null)
                {
                    return HttpContext.Current.Session["districtNameHi"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["districtNameHi"] = value;
            }
        }

        public string RollName
        {
            get
            {
                if (HttpContext.Current.Session["rollName"] != null)
                {
                    return HttpContext.Current.Session["rollName"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["rollName"] = value;
            }
        }

        public string RollAbbrName
        {
            get
            {
                if (HttpContext.Current.Session["rollAbbrName"] != null)
                {
                    return HttpContext.Current.Session["rollAbbrName"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["rollAbbrName"] = value;
            }
        }

        public string RollAbbrNameHi
        {
            get
            {
                if (HttpContext.Current.Session["rollAbbrNameHi"] != null)
                {
                    return HttpContext.Current.Session["rollAbbrNameHi"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["rollAbbrNameHi"] = value;
            }
        }

        public string AppSlug
        {
            get
            {
                if (HttpContext.Current.Session["appSlug"] != null)
                {
                    return HttpContext.Current.Session["appSlug"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["appSlug"] = value;
            }
        }

        public string MeeRegisNo
        {
            get
            {
                if (HttpContext.Current.Session["meeRegisNo"] != null)
                {
                    return HttpContext.Current.Session["meeRegisNo"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["meeRegisNo"] = value;
            }
        }
         
        public Int32 districtId
        {
            get
            {
                if (HttpContext.Current.Session["districtId"] != null)
                {
                    return Convert.ToInt32( HttpContext.Current.Session["districtId"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session["districtId"] = value;
            }
        }

        public Int32 ZoneId
        {
            get
            {
                if (HttpContext.Current.Session["zoneId"] != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["zoneId"].ToString());
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session["zoneId"] = value;
            }
        }

        public int ModuleId
        {
            get
            {
                if (HttpContext.Current.Session["ToDate"] != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session["ModuleId"]);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                HttpContext.Current.Session["ModuleId"] = value;
            }
        }

        public bool NiveshMitraFlag
        {
            get
            {
                if (HttpContext.Current.Session["NiveshMitraFlag"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["NiveshMitraFlag"]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                HttpContext.Current.Session["NiveshMitraFlag"] = value;
            }
        }

        public bool IspFlag
        {
            get
            {
                if (HttpContext.Current.Session["IspFlag"] != null)
                {
                    return Convert.ToBoolean(HttpContext.Current.Session["IspFlag"]);
                }
                else
                {
                    return false;
                }
            }
            set
            {
                HttpContext.Current.Session["IspFlag"] = value;
            }
        }

        public string ServiceCode
        {
            get
            {
                if (HttpContext.Current.Session["ServiceCode"] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session["ServiceCode"]);
                }
                else
                {
                    return "";
                }
            }
            set
            {
                HttpContext.Current.Session["ServiceCode"] = value;
            }
        }
 
         
    }
}