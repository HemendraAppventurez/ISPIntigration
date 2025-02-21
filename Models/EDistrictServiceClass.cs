using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using CCSHealthFamilyWelfareDept.DAL;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class EDistrictServiceClass
    {
        string Application_Number = "", Service_Code = "", returnres = "", username = "";
        public static string DepartmentRegistrationID = "0A8047AC665C4511426FB153AD264A50";//"2A1EA89EFE380DC9B051BHGTFDB2A9C9";

        public bool sendServiceRequest(string RequestKey)
        {
            Common_DB comndb = new Common_DB();
            ServiceModel sm = new ServiceModel();
            sm.requestKey = RequestKey;
            sm.deptRegistrationId = EDistrictServiceClass.DepartmentRegistrationID;
            sm.transIp = Common.GetIPAddress();


            try
            {
                EDistrictSVC.Service objSvc = new EDistrictSVC.Service();

                //RequestKey = Convert.ToString(this.HttpContext.Request["RequestKey"]);
                //RequestKey = Convert.ToString(HttpContext.Request["RequestKey"]);
                string resXML = objSvc.SendRequest(RequestKey, EDistrictServiceClass.DepartmentRegistrationID);

                //System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~\\Content\\ReadWriteData\\ErrorLog\\brdcrmsvSVC" + DateTime.Now.Ticks.ToString() + ".txt"), resXML);

                sm.serviceResponse = resXML;

                XmlDocument myXML = new XmlDocument();
                myXML.LoadXml(resXML);
                returnres = myXML.GetElementsByTagName("ReturnType")[0].InnerText;

                if (returnres == "Success")
                {
                    username = myXML.GetElementsByTagName("UserName")[0].InnerText;

                    comndb.InsertSendRequest(sm);

                    return true;
                    //ModelState.AddModelError("", "E-Puja - " + username);
                }
                else
                {
                    comndb.InsertSendRequest(sm);

                    return false;
                    //ModelState.AddModelError("", "E-Puja - Error");
                }
            }
            catch (Exception ex)
            {
                //System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~\\Content\\ReadWriteData\\ErrorLog\\brdcrmsvSVC" + DateTime.Now.Ticks.ToString() + ".txt"), ex.Message);

                sm.serviceResponse = sm.serviceResponse + "_" + ex.Message;

                comndb.InsertSendRequest(sm);

                return false;
            }
        }


        public bool postServiceResponse(string RequestKey, string ApplicationNo, string ServiceCode, string ApplicationType)
        {
            Common_DB comndb = new Common_DB();
            ServiceModel sm = new ServiceModel();
            sm.requestKey = RequestKey;
            sm.deptRegistrationId = EDistrictServiceClass.DepartmentRegistrationID;
            sm.transIp = Common.GetIPAddress();
            sm.applicationNumber = ApplicationNo;
            sm.serviceCode = ServiceCode;
            sm.applicationType = ApplicationType;

            try
            {
                EDistrictSVC.Service objSvc = new EDistrictSVC.Service();

                //RequestKey = Convert.ToString(this.HttpContext.Request["RequestKey"]);
                //RequestKey = Convert.ToString(HttpContext.Request["RequestKey"]);
                string resXML = objSvc.SendResponse(RequestKey, EDistrictServiceClass.DepartmentRegistrationID, ApplicationNo, ServiceCode);

                sm.serviceResponse = resXML;

                XmlDocument myXML = new XmlDocument();
                myXML.LoadXml(resXML);
                returnres = myXML.GetElementsByTagName("ReturnType")[0].InnerText;

                if (returnres == "1")
                {
                    //username = myXML.GetElementsByTagName("UserName")[0].InnerText;
                    comndb.InsertSendResponse(sm);
                    return true;
                    //ModelState.AddModelError("", "E-Puja - " + username);
                }
                else
                {
                    comndb.InsertSendResponse(sm);
                    return false;
                    //ModelState.AddModelError("", "E-Puja - Error");
                }
            }
            catch (Exception ex)
            {
                sm.serviceResponse = sm.serviceResponse + "_" + ex.Message;

                comndb.InsertSendResponse(sm);
                return false;
            }
        }
    }

    public enum EDistrict_ServiceCode
    {
        MEE = 11501,
        ILC = 11506,
        FIC = 11506,
        DIC = 11502,
        IMC = 11508,
        DEC = 11509,
        FAP = 11504,
        MER = 11505,
        MLC = 11507,
        AGC = 11503,
        ICC = 11508
    }    
}