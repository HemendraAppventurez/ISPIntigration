using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

using Amazon.SimpleNotificationService;
using Amazon.Runtime;
using Amazon.SimpleNotificationService.Model;
using System.Threading.Tasks;
using System.Configuration;

namespace CCSHealthFamilyWelfareDept.Models
{

    /// <summary>
    /// Code file contains Code for Email send
    /// </summary>
    public class MailMsg
    {
        public static string ServerName
        {
            get
            {
                String _ServerName = System.Configuration.ConfigurationManager.ConnectionStrings["ServerName"].ConnectionString;
                if (String.IsNullOrEmpty(_ServerName))
                    return "Medical Health and Family Welfare Department";

                else
                    return _ServerName;
            }
        }

        public static string FromMail
        {
            get
            {
                String _FromMail = System.Configuration.ConfigurationManager.ConnectionStrings["FromMail"].ConnectionString;
                if (String.IsNullOrEmpty(_FromMail))
                    return "do_not_reply@otpl.co.in";

                else
                    return _FromMail;
            }
        }

        public static string SendEmail(string mailTo, string mailCC, string mailSub, string mailMsg, string mailBCC)
        {
            MailerClass objmail = new MailerClass();

            string[] BccMaill = new string[1];
            string[] mailtol = new string[1];
            string[] ccMaill = new string[1];
            mailtol[0] = mailTo;
            ccMaill[0] = mailCC;
            BccMaill[0] = mailBCC;
            string FMail = "\"" + MailMsg.ServerName + "\"" + MailMsg.FromMail;
            string result = objmail.sendMail(mailtol, BccMaill, ccMaill, FMail, mailMsg, mailSub);
            return result;
        }

        public static string SendEmail(string mailTo, string mailCC, string mailSub, string mailMsg, string Attachment, string mailBCC)
        {
            MailerClass objmail = new MailerClass();

            string[] BccMaill = new string[1];
            string[] mailtol = new string[1];
            string[] ccMaill = new string[1];
            mailtol[0] = mailTo;
            ccMaill[0] = mailCC;
            BccMaill[0] = mailBCC;
            string FMail = "\"" + MailMsg.ServerName + "\"" + MailMsg.FromMail;
            string result = objmail.sendMail(mailtol, BccMaill, ccMaill, FMail, mailMsg, mailSub, Attachment);
            return result;
        }
    }
    /// <summary>
    /// Code file contains Code for SMS Send
    /// </summary>
    public class SMS
    {
        //public static String SMSSend(String Message, String Recipient, string templateID = "")  Commented on 06072023 for forgot otp
        //{
        //    //string ipAddres = Common.getIPAddress();
        //    try
        //    {
        //        WebClient Client = new WebClient();
        //        Recipient = Recipient.Trim();
        //        if (Recipient.Substring(Recipient.Length - 1, 1) == ",")
        //            Recipient = Recipient.Substring(0, Recipient.Length - 1);

               
        //        string baseurl = "";

        //        if (templateID.Trim() != "")
        //        {
        //            baseurl = "Your SMS API Details";
        //            Stream data = Client.OpenRead(baseurl);
        //            StreamReader reader = new StreamReader(data);
        //            string s = reader.ReadToEnd();
        //            data.Close();
        //            reader.Close();
        //            return "success";
        //        }
        //        else
        //        {
        //            return "missing_temp_id";

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        public static String SMSSend(String Message, String Recipient, string templateID = "")
        {
            //string ipAddres = Common.getIPAddress();
            try
            {
                WebClient Client = new WebClient();
                Recipient = Recipient.Trim();
                if (Recipient.Substring(Recipient.Length - 1, 1) == ",")
                    Recipient = Recipient.Substring(0, Recipient.Length - 1);
              //10/08/2023  SendMessageMWNew(Recipient, Message);
                //SendMessageMW(Recipient, Message);
                //SendMessageMW();
                // string baseurl = "";

                //if (templateID.Trim() != "")
                //{
                //    baseurl = "Your SMS API Details";
                //    Stream data = Client.OpenRead(baseurl);
                //    StreamReader reader = new StreamReader(data);
                //    string s = reader.ReadToEnd();
                //    data.Close();
                //    reader.Close();
                return "success";
                //}
                //else
                //{
               // return "missing_temp_id";

                //}


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public static String SMSSendNew(String Message, String Recipient, string templateID = "")
        {
            //string ipAddres = Common.getIPAddress();
            try
            {
                WebClient Client = new WebClient();
                Recipient = Recipient.Trim();
                if (Recipient.Substring(Recipient.Length - 1, 1) == ",")
                    Recipient = Recipient.Substring(0, Recipient.Length - 1);
                //10/08/2023   SendMessageMWNew(Recipient, Message);
               // SendMessageMW(Recipient, Message);
                //SendMessageMW();
                // string baseurl = "";

                //if (templateID.Trim() != "")
                //{
                //    baseurl = "Your SMS API Details";
                //    Stream data = Client.OpenRead(baseurl);
                //    StreamReader reader = new StreamReader(data);
                //    string s = reader.ReadToEnd();
                //    data.Close();
                //    reader.Close();
                   return "success";
                //}
                //else
                //{
                   return "missing_temp_id";

                //}


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        



        public static String SMSSendOTP(String Message, String Recipient, string templateID = "")
        {
            //string ipAddres = Common.getIPAddress();
            try
            {
                WebClient Client = new WebClient();
                Recipient = Recipient.Trim();
                if (Recipient.Substring(Recipient.Length - 1, 1) == ",")
                    Recipient = Recipient.Substring(0, Recipient.Length - 1);
                SendMessageMWNew(Recipient, Message);
                //SendMessageMW(Recipient, Message);
                //SendMessageMW();
                // string baseurl = "";

                //if (templateID.Trim() != "")
                //{
                //    baseurl = "Your SMS API Details";
                //    Stream data = Client.OpenRead(baseurl);
                //    StreamReader reader = new StreamReader(data);
                //    string s = reader.ReadToEnd();
                //    data.Close();
                //    reader.Close();
                return "success";
                //}
                //else
                //{
                // return "missing_temp_id";

                //}


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public static String SMSSendNewOTP(String Message, String Recipient, string templateID = "")
        {
            //string ipAddres = Common.getIPAddress();
            try
            {
                WebClient Client = new WebClient();
                Recipient = Recipient.Trim();
                if (Recipient.Substring(Recipient.Length - 1, 1) == ",")
                    Recipient = Recipient.Substring(0, Recipient.Length - 1);
                SendMessageMWNew(Recipient, Message);
                // SendMessageMW(Recipient, Message);
                //SendMessageMW();
                // string baseurl = "";

                //if (templateID.Trim() != "")
                //{
                //    baseurl = "Your SMS API Details";
                //    Stream data = Client.OpenRead(baseurl);
                //    StreamReader reader = new StreamReader(data);
                //    string s = reader.ReadToEnd();
                //    data.Close();
                //    reader.Close();
                return "success";
                //}
                //else
                //{
                return "missing_temp_id";

                //}


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }






        public static string SendMessageMWNew(String MobileNo, String message)
        {
            try
            {
                string User = ConfigurationManager.AppSettings["User"].ToString();
                string AuthKey = ConfigurationManager.AppSettings["AuthKey"].ToString();
                WebClient Client = new WebClient();
                MobileNo = MobileNo.Trim();
                if (MobileNo.Substring(MobileNo.Length - 1, 1) == ",")
                    MobileNo = MobileNo.Substring(0, MobileNo.Length - 1);
                string baseurl = "https://amazesms.in/api/pushsms?user=" + User + "&authkey=" + AuthKey + "&sender=DGMHUP&mobile=" + MobileNo + "&text=" + message + "";
                Stream data = Client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
                return s;


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }



   
    public class TestSMS
    {
        public static String SMSSend(String Message, String Recipient)
        {
            //string ipAddres = Common.getIPAddress();
            try
            {
                //WebClient Client = new WebClient();
                //Recipient = Recipient.Trim();
                //if (Recipient.Substring(Recipient.Length - 1, 1) == ",")
                //    Recipient = Recipient.Substring(0, Recipient.Length - 1);

                ////string baseurl = "";

                
                //Stream data = Client.OpenRead(baseurl);
                //StreamReader reader = new StreamReader(data);
                //string s = reader.ReadToEnd();
                //data.Close();
                //reader.Close();
                //return s;

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}