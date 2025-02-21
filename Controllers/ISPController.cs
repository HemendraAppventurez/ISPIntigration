using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCSHealthFamilyWelfareDept.Models;
using CCSHealthFamilyWelfareDept.DAL;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.IO;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    public class ISPController : Controller
    {
        //
        // GET: /ISP/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        #region ISP
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
                    result = token;
                }
            }
            catch (Exception ex)
            {
                return "";
            }


            return result;
        }

        public static string encryptString(string plainText, string pass)
        {
            byte[] salt = new byte[] {
            0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30 ,
            0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31};
            byte[] encrypted;
            // byte[] key = new Rfc2898DeriveBytes(pass, salt, 1000, HashAlgorithmName.SHA512).GetBytes(32);
            pass = "Gl7H01dpIRJ1YPZhmNQJ03nUu/NKxkjIFtP6XHDomJY=";
            byte[] Key = Convert.FromBase64String(pass);
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 };
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {

                    swEncrypt.Write(plainText);
                    swEncrypt.Flush();
                    csEncrypt.FlushFinalBlock();
                    encrypted = msEncrypt.ToArray();
                    Console.WriteLine(encrypted);
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        #endregion
    }
}
