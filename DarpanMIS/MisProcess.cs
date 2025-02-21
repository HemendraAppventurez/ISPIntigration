using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for MisProcess
/// </summary>
public class MisProcess
{

    public MisProcess()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private string TokenValidationURL = "https://up.cmdashboard.nic.in/smis/AuthClientPage.asmx/doAuth?";  
    private string clientMISKey = ""; /// Get from database
                                                                            
    private string clientMISKey2380 = "JMeB6i9tu4UOREMPy7nOO24RqTlIXwXWi+lt9ffK7nc="; /// Age Certificate(2380) Get from database
    private string clientMISKey2375 = "EgG9/h2btQB0e7bodNOiwGgxCRvfBCDknbkcwFxMYoY=";//"b5jEMEtk6DKXYGTjnONxF3OYAMtGDxkb+X9VCUsc3OI="; /// Death Certificate(2375) Get from database
    private string clientMISKey2373 = "PjqYwtoYRLmyzT+B1cl1ymNN8J5V/F2024I+ZeFoazc=";//"/yd98sT0ksTRy0Yc1CfXlGId2dVSJWUH1zD9TNliroQ="; /// Disability Certificate(2373) Get from database
    private string clientMISKey2372 = "j3yEm4AaR3M8Qn7VxXRZtyaBMT96wxujWUFgpYYhtE4="; /// Fitness Certificates(2372) Get from database  
    private string clientMISKey2371 = "/7wSZNld733wJY9YB4NTt0N4XHjYfXSrKgaPfRZxiqA=";//"bwasIZSQozm4K9v6pwOeGxhBVbJZOnwXjtJi1WLH99k="; /// Illness Certificate(2371) Get from database                             
    private string clientMISKey2382 = "ah3nfjhIUbs52mDwCI/J7pNvGC9xw4Pk+eqRWX+diIA=";//"w112xkN9JxI4XlAQxKcp9dLkkxAb5sC3b77tb4GFdeo="; /// Immunization Certificate for(2382) Get from database
    private string clientMISKey2374 = "1IoLSWuvzY8j/1ErV07hjtdxaAZJnwjoiB/qWOQEocU=";//"1kap/xQhIWjcacJ3pc9ebojgQY9bhksFbz8OzQIB+K4="; /// Immunization Certificate(2374) Get from database
    private string clientMISKey2378 = "bzMfP3zVoOKgEp5ldiSKd4aHBdYIj4FPXdrnDFMdJQk=";//"5QiG/Q3MNndSKt+EhofH6x6q8+YrhIiVF9GuQyNYosw="; /// Medico- Legal Certificate(2378) Get from database
    private string clientMISKey2376 = "f8heaZzJzcsgJdExQ2ZxJDs9dEqgmUf64VlAsXsuSls="; /// Family Planning(2376) Get from database  
    private string clientMISKey2377 = "f5fukBaYAB3vnrVJTXPthT9Ox8OmcszAZ41d0j3u9II=";//"j4ARmFn1RvQxIwClCB93Zz0FOrbaoTnOZomYk9oRPeg="; /// Medical Reimbursement(2377) Get from database                     
    private string clientMISKey2159 = "UGdllkwWIPd877QOp/+AsKzdiPnPB0RBnKMS2somvxg=";//"tcXddrSXWSh2bf8T/jy35ku7ij1wsO0kgrmxb+muN8g="; ///  Medical Establishments(2159) Get from database   


    private string Project_Code = "";// Project Code as per the project definition.;
    private string Instance_code = "2"; // Instance Code as per the project definition.

    private BoMis ObjMisData = new BoMis();
    public BoMis ValidateMIS_Request(string strdata)
    {
        // ' Check token is not empty and NULL
        if (string.IsNullOrEmpty(strdata) | strdata == "")
        {
            return null;
        }
        BoMispayload Objpayload = new BoMispayload();
        Objpayload = JsonConvert.DeserializeObject<BoMispayload>(Base64Decode(strdata));
        string dicriptstring = Objpayload.data;
        Project_Code = Objpayload.projcode;  // receivend dynamic project code from query string of cpat
        Instance_code = Objpayload.instcode;  // receivend dynamic project code from query string of cpat

        if (Project_Code == "2380")
        {
            clientMISKey = clientMISKey2380;
           
        }
        else if (Project_Code == "2375")
        {
            clientMISKey = clientMISKey2375;
           
        }
        else if (Project_Code == "2373")
        {
            clientMISKey = clientMISKey2373;
           
        }
        else if (Project_Code == "2372")
        {
            clientMISKey = clientMISKey2372;
           
        }
        else if (Project_Code == "2371")
        {
            clientMISKey = clientMISKey2371;
            
        }
        else if (Project_Code == "2382")
        {
            clientMISKey = clientMISKey2382;
           
        }
        else if (Project_Code == "2374")
        {
            clientMISKey = clientMISKey2374;
            
        }
        else if (Project_Code == "2378")
        {
            clientMISKey = clientMISKey2378;
         
        }
        else if (Project_Code == "2376")
        {
            clientMISKey = clientMISKey2376;
           
        }
        else if (Project_Code == "2377")
        {
            clientMISKey = clientMISKey2377;
           
        }
        else if (Project_Code == "2159")
        {
            clientMISKey = clientMISKey2159;
            
        }
        else 
        {
            clientMISKey = "";
        }

        //RanSchedule("dicriptstring:" + dicriptstring);
        //'Compute payload hash
        string payloadHash = ComputeSha512Hash(Objpayload.data + clientMISKey);
        // 'Compare Output hash with Computed hash 
        if ((Objpayload.hash == payloadHash))
        {
            BoMis ObjMis = new BoMis();
            //Get IV From payload and store in session
            HttpContext.Current.Session["MIS_ClientIV"] = Objpayload.iv;
            // 'Dectypt the payload 
            string DectyptString = DecryptData(Objpayload.data, clientMISKey);
            ObjMis = JsonConvert.DeserializeObject<BoMis>(DectyptString);
            //RanSchedule("Data string MIS process");
            // ' Validate Token Request
            if (TokenValidation(ObjMis.token, ObjMis.nonce, ObjMis.timestamp) != "failure")
            {
                ObjMisData = ObjMis;
                //RanSchedule("Success Token validate");
                return ObjMisData;
            }
            else
                return null;
        }
        else
            return null;
    }


    public static void RanSchedule(string cpatStr)
    {

        string path = @"C:\up-health.in\online\DarpanMIS\log\misrequestlog.text";


        using (StreamWriter writer = new StreamWriter(path, true))
        {
            writer.WriteLine("\n\n-------Response Started at {" + DateTime.Now.ToString() + "}-------");

            writer.WriteLine(cpatStr);
            writer.WriteLine("\n\n-------Response Ended at {" + DateTime.Now.ToString() + "}-------");
        }
    }
    //void writelog(string msg)
    //{
    //    using (StreamWriter _testData = new StreamWriter(HttpContext.Current.Server.MapPath("~/data.txt"), true))
    //    {
    //        _testData.WriteLine(msg); // Write the file.
    //    }
    //}
    public string TokenValidation(string Token, string nonce, string timestamp)
    {
        // *********Client can validate and the nonce and timestemp  
        BoMispayload Objpayload_token = new BoMispayload();
        Objpayload_token.data = EncryptData(Token + "#" + Project_Code + "#" + nonce + "#" + timestamp, clientMISKey); //Token+ProjectCode+nonce+timestamp        
        Objpayload_token.hash = ComputeSha512Hash(Objpayload_token.data + clientMISKey);
        Objpayload_token.projcode = Project_Code;
        Objpayload_token.instcode = Instance_code;
        Objpayload_token.iv = HttpContext.Current.Session["MIS_ClientIV"].ToString(); // IV

        // 'Prepare token valiation URL      
        string handshakingUrl = TokenValidationURL + "localTokenId=" + Base64Encode(JsonConvert.SerializeObject(Objpayload_token));
        //Work on - 12052022
        System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072;
        HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create(handshakingUrl);
        HttpWebResponse httpres = (HttpWebResponse)httpreq.GetResponse();
        StreamReader sr = new StreamReader(httpres.GetResponseStream());
        // 'Get Reponse
        var responseString = sr.ReadToEnd();
        // 'Check Invalid Request
        if ((responseString == "Invalid Request"))
        {
           RanSchedule("Invalid Request MIS process");
            // writelog("Invalid Request");
            HttpContext.Current.Session["Clienttoken"] = "";
            return "failure";
        }
        BoMispayload Objpayload = new BoMispayload();
        Objpayload = JsonConvert.DeserializeObject<BoMispayload>(Base64Decode(responseString));
        // 'Compute payload hash
        string payloadHash = ComputeSha512Hash(Objpayload.data + clientMISKey);
        // 'Compare Output hash with  payload hash 
        if ((Objpayload.hash == payloadHash))
        {
    
            // 'Dectypt the payload with client MIS key
            HttpContext.Current.Session["MIS_ClientIV"] = Objpayload.iv;

            string DectyptString = DecryptData(Objpayload.data, clientMISKey);
            string[] strArr;
            strArr = DectyptString.Split('#');
            if ((strArr[0] == "failure"))
            {
                RanSchedule("failure");
                HttpContext.Current.Session["Clienttoken"] = "";
                return strArr[0];
            }
            else
            {
               
                // ' Get New Token on each successful validation 
                HttpContext.Current.Session["Clienttoken"] = strArr[1];
                HttpContext.Current.Session["Clientnonce"] = strArr[2];
                HttpContext.Current.Session["Clienttimestamp"] = strArr[3];
                RanSchedule("Successfull _ inside token validation");

                return strArr[0];

            }
        }
        else
            RanSchedule("failure not match");
            return "failure";
    }

    private string Base64Encode(string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }
    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
    public static string GenerateIV()
    {
        AesCryptoServiceProvider aesEncryption = new AesCryptoServiceProvider();
        aesEncryption.BlockSize = 128;
        aesEncryption.Mode = CipherMode.CBC;
        aesEncryption.Padding = PaddingMode.PKCS7;  
        string ivStr = Convert.ToBase64String(aesEncryption.IV);
        HttpContext.Current.Session["MIS_ClientIV"] = ivStr;
        return ivStr;
    }
    private static string EncryptData(string iPlainStr, string iCompleteEncodedKey)
    {
        GenerateIV();
        AesCryptoServiceProvider aesEncryption = new AesCryptoServiceProvider();
        aesEncryption.KeySize = 256;
        aesEncryption.BlockSize = 128;
        aesEncryption.Mode = CipherMode.CBC;
        aesEncryption.Padding = PaddingMode.PKCS7;
        aesEncryption.IV = Convert.FromBase64String(HttpContext.Current.Session["MIS_ClientIV"].ToString());
        aesEncryption.Key = Convert.FromBase64String(iCompleteEncodedKey);
        //aesEncryption.Key = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(iCompleteEncodedKey)).Split(',')[1])
        byte[] plainText = ASCIIEncoding.UTF8.GetBytes(iPlainStr);
        ICryptoTransform crypto = aesEncryption.CreateEncryptor();
        byte[] cipherText = crypto.TransformFinalBlock(plainText, 0, plainText.Length);
        return Convert.ToBase64String(cipherText);
    }
    private static string DecryptData(string iEncryptedText, string iCompleteEncodedKey)
    {
        AesCryptoServiceProvider aesEncryption = new AesCryptoServiceProvider();
        aesEncryption.KeySize = 256;
        aesEncryption.BlockSize = 128;
        aesEncryption.Mode = CipherMode.CBC;
        aesEncryption.Padding = PaddingMode.PKCS7;
        aesEncryption.IV = Convert.FromBase64String(HttpContext.Current.Session["MIS_ClientIV"].ToString());
        aesEncryption.Key = Convert.FromBase64String(iCompleteEncodedKey);
        ICryptoTransform decrypto = aesEncryption.CreateDecryptor();
        byte[] encryptedBytes = Convert.FromBase64CharArray(iEncryptedText.ToCharArray(), 0, iEncryptedText.Length);
        return ASCIIEncoding.UTF8.GetString(decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
    }

    private static string ComputeSha512Hash(string rawData)
    {
        using (SHA512 sha512Hash = SHA512.Create())
        {
            byte[] bytes = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i <= bytes.Length - 1; i++)
                builder.Append(bytes[i].ToString("x2"));

            return builder.ToString();
        }
    }


}