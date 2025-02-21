using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;
//using System.Net.Mime.MediaTypeNames;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class Services
    {
        public static string PostValidateISPLogin(object obj, string url)
        {
            ISPLoginResponse model = new ISPLoginResponse();
            string result = string.Empty;
            try
            {

                using (var clients=new HttpClient())
                {
                    HttpContent hc = new StringContent(JsonConvert.SerializeObject(obj));
                    hc.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    var response = clients.PostAsync(url, hc).Result;
                    if(response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;

                    }

                }
            }
            catch(Exception ex)
            {
                Account_DB obj1 = new Account_DB();
                obj1.ISPErrorLog("PostValidateISPLogin", ex.InnerException.InnerException.Message,"", "User Action", "01");
                
                return "";
            }


            return result;
        }


       

        public static string HitAPIWithToken( string json,string url, string token)
        {
            var result = "";
            try
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + token);
                httpWebRequest.Method = "POST";
                using (var streamWriter = new

                StreamWriter(httpWebRequest.GetRequestStream()))
                {


                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                Account_DB obj1 = new Account_DB();
                obj1.ISPErrorLog("HitAPIWithToken", ex.Message, "", "User Action", "01");
                result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            return result;
        }

        public static string HitAPIWithTokennew(string json, string url, string token)
        {
            var result = "";
            try
            {

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + token);
                httpWebRequest.Method = "POST";
                var finaljson = "{\"dept_id\":\"" + "6240616" + "\",\"e_data\":\"" + json + "\"}";
                using (var streamWriter = new

                StreamWriter(httpWebRequest.GetRequestStream()))
                {


                    //streamWriter.Write(json);
                    streamWriter.Write(finaljson);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                Account_DB obj = new Account_DB();
                obj.ISPErrorLog("HitAPIWithTokennew", ex.Message,"", "User Action", "01");
                result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
               
                obj.ISPErrorLog("HitAPIWithTokennew", result, "", "User Action", "01");


            }
            return result;
        }
        public static string PostValidateISP(string url)
        {
            ISPLoginResponse model = new ISPLoginResponse();
            string result = string.Empty;
            try
            {

                using (var clients = new HttpClient())
                {
                    //HttpContent hc = new StringContent(JsonConvert.SerializeObject(obj));
                    //hc.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var content = new StringContent(string.Empty, null);
                    var response = clients.PostAsync(url,content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;

                    }

                }
            }
            catch (Exception ex)
            {
                return "";
            }


            return result;
        }


        public static string GetISPToken()
        {
            ISPLoginRequest req = new ISPLoginRequest();
            string token = "";
            string result = string.Empty;
            try
            {

                req.username = "6240616";
                req.password = "SWwXW7trJaMigcPWSIdH";
                string dsata = Services.PostValidateISPLogin(req, "http://164.100.181.91/ispws/authenticate");

                var Response = JsonConvert.DeserializeObject<ISPLoginResponse>(dsata);
                if (Response.statusMessage == "SUCCESSFUL")
                {

                    token = Response.token;
                }
            }
            catch (Exception ex)
            {
                return "";
            }


            return result;
        }
    }


    public class ISPLoginResponse
    {
        public string error { get; set; }
        public string statusMessage { get; set; }
        public string token { get; set; }
        public DateTime timestamp { get; set; }
    }

    public class ValidateResponse
    {
        public string error { get; set; }
        public string statusMessage { get; set; }
        public string data { get; set; }
        public string timestamp { get; set; }

    }
    public class DecryptResponse
    {
        public string applicant_id { get; set; }
        public string service_code { get; set; }
        public string request_id { get; set; }
        public string request_validated { get; set; }
    }
    public class ApplicantCommanDetails
    {
        public string applicant_id { get; set; }
        public string gender { get; set; }
        public string father_or_husband_or_guardian { get; set; }
        public DateTime dob { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public int occupation { get; set; }
        public int income { get; set; }
        public string family_id { get; set; }
        public string member_id { get; set; }
        public string aadhar_reference_no { get; set; }
        public string service_code { get; set; }
        public string d1 { get; set; }
        public string d2 { get; set; }
        public string d3 { get; set; }
        public string d4 { get; set; }
        public string d5 { get; set; }
        public string d6 { get; set; }
        public string d7 { get; set; }
        public string d8 { get; set; }
        public string d9 { get; set; }
        public string d10 { get; set; }
        public string d11 { get; set; }
        public string d12 { get; set; }
        public string d13 { get; set; }
        public string d14 { get; set; }
        public string d15 { get; set; }
        public string d16 { get; set; }
        public string d17 { get; set; }
        public string d18 { get; set; }
        public string d19 { get; set; }
        public string d20 { get; set; }
        public string first_name_eng { get; set; }
        public string middle_name_eng { get; set; }
        public string last_name_eng { get; set; }
        public string first_name_hindi { get; set; }
        public string middle_name_hindi { get; set; }
        public string last_name_hindi { get; set; }
        public string father_or_husband_or_guardian_name_eng { get; set; }
        public string father_or_husband_or_guardian_name_hindi { get; set; }
        public string mother_name_eng { get; set; }
        public string mother_name_hindi { get; set; }
        public int category { get; set; }
        public string pan_no { get; set; }
        public int marital_status { get; set; }
        public string residential_house_no { get; set; }
        public string residential_mohalla { get; set; }
        public string residential_pin { get; set; }
        public int residential_area_type { get; set; }
        public int residential_state { get; set; }
        public int residential_district { get; set; }
        public int residential_tehsil { get; set; }
        public int residential_police_station { get; set; }
        public int residential_vikaskhand { get; set; }
        public int residential_grampanchayat { get; set; }
        public int residential_rajasvagram { get; set; }
        public string residential_nagarnigam { get; set; }
        public string residential_nagarpalika { get; set; }
        public string residential_nagarpanchayat { get; set; }
        public string residential_ward { get; set; }
        public string permanent_house_no { get; set; }
        public string permanent_mohalla { get; set; }
        public string permanent_pin { get; set; }
        public int permanent_area_type { get; set; }
        public int permanent_state { get; set; }
        public int permanent_district { get; set; }
        public int permanent_tehsil { get; set; }
        public int permanent_police_station { get; set; }
        public int permanent_vikaskhand { get; set; }
        public int permanent_grampanchayat { get; set; }
        public int permanent_rajasvagram { get; set; }
        public string permanent_nagarnigam { get; set; }
        public string permanent_nagarpalika { get; set; }
        public string permanent_nagarpanchayat { get; set; }
        public string permanent_ward { get; set; }
        public string service_name_eng { get; set; }
        public string service_name_hindi { get; set; }

    }
    public class AcknowledgementReq
    {
        public string request_id { get; set; }
        public string applicant_id { get; set; }
        public string service_code { get; set; }
        public string application_id { get; set; }
        public string application_url { get; set; }
        public string status_code { get; set; }
        public string remarks { get; set; }
        public string pendency_level { get; set; }
        public string pending_with_officer { get; set; }
        public string action_taken_time { get; set; }
        public string selected_district_for_processing_application{ get; set; }
        public string designated_code { get; set; }
        public string designated_location_code { get; set; }
        public string designated_target_date { get; set; }
        public string first_appellate_code { get; set; }
        public string first_appellate_location_code { get; set; }
        public string first_appellate_days { get; set; }
        public string second_appellate_code { get; set; }
        public string second_appellate_location_code  { get; set; }
        public string second_appellate_days  { get; set; }
        public string certificate_url { get; set; }
        public string certificate_no { get; set; }
        public string dbt_amount { get; set; }
        public string certificate_expiry_date { get; set; }
        public string d1 { get; set; }
        public string d10 { get; set; }
        public string d11 { get; set; }
        public string d12 { get; set; }
        public string d13 { get; set; }
        public string d14 { get; set; }
        public string d15 { get; set; }
        public string d16 { get; set; }
        public string d17 { get; set; }
        public string d18 { get; set; }
        public string d19 { get; set; }
        public string d20 { get; set; }
    }
    #region ISP
    public class ISPLoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class ISPValidateRequest
    {
        public string sessionkey { get; set; }
        
    }
    #endregion 
}