using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class AuditMethods
    {

        public bool IsValidLink(string url, string documentName, out string errorMsg)
        {
            string statusCode = string.Empty;
            string isLinkAllowed = string.Empty;

            IsLinkAllowed(url, out isLinkAllowed);
            if (isLinkAllowed == "valid")
            {
                //getRequestStatus(url, out statusCode);
                statusCode = "valid";
                if (!string.IsNullOrEmpty(statusCode) && statusCode.ToLower() == "valid")
                {
                    errorMsg = "";
                    return true;
                }
                else
                {
                    errorMsg = documentName + " is Invalid File.";
                    return false;
                }
            }
            else
            {
                errorMsg = documentName + " is " + isLinkAllowed;
                return false;
            }

        }



        private string IsLinkAllowed(string url, out string message)
        {
            try
            {
                var ext = url.Split('.').LastOrDefault();
                if (!string.IsNullOrEmpty(ext) && (ext.ToLower().Equals("pdf") || ext.ToLower().Equals("jpg") || ext.ToLower().Equals("jpeg")))
                {
                    message = "valid";
                }
                else
                {
                    message = "Invalid File.";
                }

                return message;
            }
            catch
            {
                message = "Invalid File.";
                return message;
            }
        }


        private HttpStatusCode getRequestStatus(string url, out string stsdesc)
        {
            try
            {
                if (url.StartsWith("~/") == true)
                {
                    url = url.Substring(1, url.Length - 1);
                }
               
                url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + url;



                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                // Sends the HttpWebRequest and waits for a response.
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();



                stsdesc = myHttpWebResponse.StatusDescription;

                // Releases the resources of the response.
                myHttpWebResponse.Close();

                if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    stsdesc = "valid";
                }

                return myHttpWebResponse.StatusCode;
            }
            catch (Exception ex)
            {
                stsdesc = ex.Message;

                return HttpStatusCode.NotFound;
            }
        }

        public string RemoveSpecialCharactors(string inputString)
        {
            return Regex.Replace(inputString, @"[^a-zA-Z\s]+", "");
        }

        public string FilterForAlphabetNumaric(string inputString)
        {
            return Regex.Replace(inputString, @"[^0-9a-zA-Z\s-]+", "");
        }
    }
}