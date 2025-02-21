using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using Newtonsoft.Json;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class Common
    {
        #region Get Ip Address
        public static String GetIPAddress()
        {
            String ip = "";
            try
            {
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    string trueIP = ipRange[le];
                }
                else
                {
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
            }
            catch
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return ip;
        }
        #endregion

        #region Convert String Array to DataTable
        public DataTable strArrayToDataTable(string[] arr)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            foreach (var item in arr)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = item;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        #endregion

        #region Generate OTP

        public string generateno()
        {
            string characters = string.Empty;
            string numbers = "1234567890";
            characters += numbers;

            string otp = string.Empty;
            string otpUser = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }

            return otp;
        }

        #endregion


        public string GetMimeType(string extension)
        {
            if (extension == null)
                throw new ArgumentNullException("extension");

            if (extension.StartsWith("."))
            {
                extension = extension.Substring(1);
            }
            switch (extension.ToLower())
            {
                case "jpeg": return "image/jpeg";
                case "jpg": return "image/jpeg";
                case "png": return "image/png";
                case "pdf": return "application/pdf";
                default: return "application/octet-stream";
            }
        }

        #region Check Date Format
        public bool IsValidDateFormat(string date = "")
        {
            DateTime dt;
            string[] formats = { "dd/MM/yyyy" };
            if (!DateTime.TryParseExact(date, formats,
                            System.Globalization.CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out dt))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Validate Image Ext With Size
        public string ValidateImageExtWithSize(HttpPostedFileBase image, int ImageSize)
        {
            string massage;
            String fn = Path.GetFileNameWithoutExtension(image.FileName);
            String ext = Path.GetExtension(image.FileName);
            char[] SpecialChars = "!@#$%^&*()+=~`\\|/?><,\"".ToCharArray();
            int indexOf = fn.IndexOfAny(SpecialChars);
            String fileName = fn;
            int count = fileName.Split('.').Length - 1;
            if (count > 1)
            {
                massage = "Double extension not allowed in file name";

            }
            else
            {
                if (indexOf != -1)
                {
                    massage = "Special character not allowed in file name";

                }
                else
                {
                    string mimetype = image.ContentType;
                    if ((ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg") && (mimetype == "image/jpeg" || mimetype == "image/jpg"))
                    {
                        fn = "";

                        string fFullName = image.FileName;
                        int len = fFullName.Length;
                        string ext1 = Path.GetExtension(fFullName);
                        string str = fFullName.Substring(fFullName.LastIndexOf("\\") + 1);
                        len = str.Length;
                        string fileN = str.Substring(0, len - ext1.Length);

                        Regex FilenameRegex = null;
                        FilenameRegex = new Regex("(.*?)\\.(jpeg|jpg|JPEG|JPG)$", RegexOptions.IgnoreCase);
                        int index = fileN.IndexOf(".");

                        if (!FilenameRegex.IsMatch(fFullName) || index != -1)
                        {
                            massage = "Please upload .jpg or .jpeg file only";

                        }
                        else
                        {
                            string Photoname = Path.GetFileNameWithoutExtension(image.FileName);
                            string fileSize = image.ContentLength.ToString();
                            String ImageFileNameMBA = image.FileName;

                            Byte[] stu_imageMBA = new Byte[image.ContentLength];

                            Stream fs = image.InputStream;
                            fs.Read(stu_imageMBA, 0, Convert.ToInt32(fileSize));

                            fs.Seek(0, SeekOrigin.Begin);
                            StreamReader sr = new StreamReader(fs, true);

                            string firstLine = sr.ReadLine().ToString();

                            //string firstLine = "JFIF";

                            if ((firstLine.IndexOf("JFIF") > -1) || (firstLine.IndexOf("Exif") > -1))
                            {

                                if (image.ContentLength <= 1024 * ImageSize)
                                {
                                    massage = "Valid";

                                }

                                else
                                {
                                    massage = "File size can not exceed " + ImageSize + " KB ";
                                }
                            }
                            else
                            {
                                massage = "Please upload .jpg or .jpeg file only";
                            }
                        }
                    }
                    else
                    {
                        massage = "Please upload .jpg or .jpeg file only";
                    }
                }
            }
            return massage;
        }
        #endregion

        public string ValidateImageExtWithSizeForDocuments(HttpPostedFileBase Uploadedfile)
        {
            string massage;
            String fn = Path.GetFileNameWithoutExtension(Uploadedfile.FileName);
            String ext = Path.GetExtension(Uploadedfile.FileName);
            char[] SpecialChars = "!@#$%^&*()+=~`\\|/?><,\"".ToCharArray();
            int indexOf = fn.IndexOfAny(SpecialChars);
            String fileName = fn;
            int count = fileName.Split('.').Length - 1;
            if (count > 1)
            {
                massage = "Double extension not allowed in File name";

            }
            else
            {
                if (indexOf != -1)
                {
                    massage = "Special character not allowed in File name";

                }
                else
                {
                    string mimetype = Uploadedfile.ContentType;
                    if ((ext == ".jpg" || ext == ".jpeg") && (mimetype == "image/jpeg" || mimetype == "image/jpg"))
                    {
                        fn = "";

                        string fFullName = Uploadedfile.FileName;
                        int len = fFullName.Length;
                        string ext1 = Path.GetExtension(fFullName);
                        string str = fFullName.Substring(fFullName.LastIndexOf("\\") + 1);
                        len = str.Length;
                        string fileN = str.Substring(0, len - ext1.Length);

                        Regex FilenameRegex = null;
                        FilenameRegex = new Regex("(.*?)\\.(jpeg|jpg|JPEG|JPG)$", RegexOptions.IgnoreCase);
                        int index = fileN.IndexOf(".");

                        if (!FilenameRegex.IsMatch(fFullName) || index != -1)
                        {
                            massage = "Please upload .jpg, .jpeg or .pdf File only";

                        }
                        else
                        {
                            string Photoname = Path.GetFileNameWithoutExtension(Uploadedfile.FileName);
                            string fileSize = Uploadedfile.ContentLength.ToString();
                            String ImageFileNameMBA = Uploadedfile.FileName;

                            Byte[] stu_imageMBA = new Byte[Uploadedfile.ContentLength];

                            Stream fs = Uploadedfile.InputStream;
                            fs.Read(stu_imageMBA, 0, Convert.ToInt32(fileSize));

                            fs.Seek(0, SeekOrigin.Begin);
                            StreamReader sr = new StreamReader(fs, true);

                            string firstLine = sr.ReadLine().ToString();

                            //string firstLine = "JFIF";

                            if ((firstLine.IndexOf("JFIF") > -1) || (firstLine.IndexOf("Exif") > -1))
                            {


                                massage = "Valid";


                            }
                            else
                            {
                                massage = "Please upload .jpg, .jpeg or .pdf File only";
                            }
                        }
                    }
                    else if (ext == ".pdf" && mimetype == "application/pdf")
                    {
                        fn = "";

                        string fFullName = Uploadedfile.FileName;
                        int len = fFullName.Length;
                        string ext1 = Path.GetExtension(fFullName);
                        string str = fFullName.Substring(fFullName.LastIndexOf("\\") + 1);
                        len = str.Length;
                        string fileN = str.Substring(0, len - ext1.Length);

                        Regex FilenameRegex = null;
                        //FilenameRegex = new Regex("(.*?)\\.(jpeg|jpg|JPEG|JPG)$", RegexOptions.IgnoreCase);
                        FilenameRegex = new Regex("(.*?)\\.(pdf|PDF)$", RegexOptions.IgnoreCase);
                        int index = fileN.IndexOf(".");

                        if (!FilenameRegex.IsMatch(fFullName) || index != -1)
                        {
                            massage = "Please upload .jpg, .jpeg or .pdf File only";

                        }
                        else
                        {
                            string Photoname = Path.GetFileNameWithoutExtension(Uploadedfile.FileName);
                            string fileSize = Uploadedfile.ContentLength.ToString();
                            String ImageFileNameMBA = Uploadedfile.FileName;

                            Byte[] stu_imageMBA = new Byte[Uploadedfile.ContentLength];

                            Stream fs = Uploadedfile.InputStream;
                            fs.Read(stu_imageMBA, 0, Convert.ToInt32(fileSize));

                            fs.Seek(0, SeekOrigin.Begin);
                            StreamReader sr = new StreamReader(fs, true);

                            string firstLine = sr.ReadLine().ToString();

                            //string firstLine = "JFIF";

                            if ((firstLine.IndexOf("%PDF") > -1))
                            {
                                massage = "Valid";

                            }
                            else
                            {
                                massage = "Please upload .jpg, .jpeg or .pdf File only";
                            }
                        }
                    }
                    else
                    {
                        massage = "Please upload .jpg, .jpeg or .pdf File only";
                    }
                }
            }
            return massage;
        }


        #region Convert Image To Base64 String
        public string ConvertImageToBase64String(string imagePath)
        {
            string base64String = null;
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
        #endregion

        #region Convert JSON To XML Method
        public XmlDocument ConvertJSONToXML(string JsonValue, string rootNode, string node)
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml = JsonConvert.DeserializeXmlNode("{\"" + node + "\":" + JsonValue + "}", rootNode);
            }
            catch (Exception ex)
            {
                throw;
            }
            return xml;
        }
        #endregion

        #region Generate new Password
        public string CreateNewPassword_LowerCase()
        {

            Random random = new Random();

            int Size = 3;
            string input = "abcdefghijklmnopqrstuvwxyz";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < Size; i++)
            {
                ch = input[random.Next(0, input.Length)];
                builder.Append(ch);
            }
            return builder.ToString();

        }

        public string CreateNewPassword_UperCase()
        {

            Random random = new Random();

            int Size = 3;
            string input = "QWERTYUIOPLKJHGFDSAZXCVBNM";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < Size; i++)
            {
                ch = input[random.Next(0, input.Length)];
                builder.Append(ch);
            }
            return builder.ToString();

        }

        public string CreateNewPassword_Numeric()
        {

            Random random = new Random();

            int Size = 2;
            string input = "1234567890";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < Size; i++)
            {
                ch = input[random.Next(0, input.Length)];
                builder.Append(ch);
            }
            return builder.ToString();

        }

        public string CreateNewPassword_Special()
        {

            Random random = new Random();

            int Size = 2;
            string input = "@#$*";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < Size; i++)
            {
                ch = input[random.Next(0, input.Length)];
                builder.Append(ch);
            }
            return builder.ToString();

        }

        public string Ramdomselection(string newpasswd)
        {
            char[] charArray = newpasswd.ToCharArray();

            char[] shuffledArray = new char[charArray.Length];
            int rndNo;

            Random rnd = new Random();
            for (int i = charArray.Length; i >= 1; i--)
            {
                rndNo = rnd.Next(1, i + 1) - 1;
                shuffledArray[i - 1] = charArray[rndNo];
                charArray[rndNo] = charArray[i - 1];
            }
            string str = new string(shuffledArray);
            //string str = string(shuffledArray);

            return str;
        }

        public string GetPlainPassword()
        {
            string gennewPassword = CreateNewPassword_LowerCase() + CreateNewPassword_UperCase() + CreateNewPassword_Numeric() + CreateNewPassword_Special();
            return Ramdomselection(gennewPassword);
        }
        #endregion

        public bool GetClameAmountFAP(int CompancationCatId, string dateOfDeath, string DateOfOpration, out decimal amount)
        {
            CCSHealthFamilyWelfareDept.DAL.FAP_DB objFAPDB = new CCSHealthFamilyWelfareDept.DAL.FAP_DB();
            bool isSucc = false;
            amount = 0;
            CompancationModel data = objFAPDB.GetCompensationAmont(CompancationCatId);
            if (data != null)
            {
                if (data.isRequiredData)
                {
                    try
                    {
                        DateTime dateOfDeathDate = DateTime.ParseExact(dateOfDeath, "dd/MM/yyyy", null);
                        DateTime DateOfOprationDate = DateTime.ParseExact(DateOfOpration, "dd/MM/yyyy", null);
                        isSucc = true;
                        if (DateOfOprationDate.AddDays(data.dayLimitForClameAmt) >= DateOfOprationDate)
                        {
                            amount = data.clameAmount;
                        }
                        else
                        {
                            amount = data.clameAmount2;
                        }
                    }
                    catch
                    {
                        isSucc = false;
                    }
                }
                else
                {
                    isSucc = true;
                    amount = data.clameAmount;
                }
            }
            return isSucc;
        }
    }



    #region abhijeet code
    public class DropDownList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isRequiredData { get; set; }
        public string chkType { get; set; }
        public int chk { get; set; }
    }
    public class ResultSet
    {
       
        public string MobileNo { get; set; }
        public int appStatus { get; set; }
        public long RegisIdNUH { get; set; }
        public long RegisIdFIC { get; set; }
        public long RegisIdDIC { get; set; }
        public long RegisIdDEC { get; set; }
        public long RegisId { get; set; }
        public string RegistrationNo { get; set; }
        public string inspectionDate { get; set; }
        public int Flag { get; set; }
        public string Msg { get; set; }
        public int isExists { get; set; }
        public string userName { get; set; }
        public bool isRenewal { get; set; }
        public int operatedId { get; set; }
        //change
        public string ReplyQuery { get; set; }
        public string Query { get; set; }
        public long RegisterByuserID { get; set; }
        public string NocUrl { get; set; }
        
    }
    public class ProcessType
    {
        public int District { get; set; }
        public string Year { get; set; }
        public string toDate { get; set; }
        public string fromDate { get; set; }
        public long totalAppReceived { get; set; }
        public long totalAppInProcess { get; set; }
        public long totalAppApproved { get; set; }
        public long totalAppRejected { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProcessType> DistrictList { get; set; }
    }
    #endregion

    public class CMODistrictModel
    {
        public int districtId { get; set; }
    }

    public enum AbbrServiceName
    {
        MEE = 1, //Registration of Medical Establishment
        ILC = 2, //Issuance of Illness Certificate 
        FIC = 3, //Issuance of Fitness Certificates
        DIC = 4, //Issuance of Disability Certificate
        IMC = 5, //Issuance of Immunization Certificate
        DEC = 6, //Issuance of Death Certificate
        FAP = 7, //Payment for Unsuccessful Family Planning
        MER = 8, //Payment of Medical Reimbursement
        MLC = 9, //Issuance of Medico- Legal Certificate
        AGC = 10, //Issuance of Age Certificate
        ICC = 11 //Issuance of Immunization Certificate for Children
    }

    public static class PermissionLinks
    {
        public static PermissionModel LinkPermission(List<PermissionModel> lstPermi, int enumsValue)
        {
            PermissionModel objPermi = new PermissionModel();
            objPermi = lstPermi.Where(m => m.serviceId == enumsValue).FirstOrDefault();
            return objPermi;
        }
    }
}