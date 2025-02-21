using CCSHealthFamilyWelfareDept.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CCSHealthFamilyWelfareDept.Controllers
{
    public class ResourceController : ApiController
    {
        [HttpPost]
        //[Route("api/file/upload/{id}")]
        public HttpResponseMessage UploadFiles(int id)
        {
            FileData plist = new FileData();

            string path = "", error = "";
            string filename = "";

            if (HttpContext.Current.Request.Files.Count == 1)
            {
                if (id == 10)
                {
                    string Dirpath = "~/Content/writereaddata/AGC/";

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(Dirpath)))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(Dirpath));
                    }

                    string ext = Path.GetExtension(HttpContext.Current.Request.Files[0].FileName);
                    long size = HttpContext.Current.Request.Files[0].ContentLength;

                    if ((ext.ToLower() == ".jpg" || ext.ToLower() == ".pdf") && size > 0)
                    {
                        filename = Path.GetFileNameWithoutExtension(HttpContext.Current.Request.Files[0].FileName) + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;
                        string completepath = Path.Combine(HttpContext.Current.Server.MapPath(Dirpath), filename);
                        if (System.IO.File.Exists(completepath))
                        {
                            System.IO.File.Delete(completepath);
                        }

                        if (size > 2097152)
                        {
                            error = "warning_Maximum 2MB file size are allowed";
                        }
                        else
                        {
                            HttpContext.Current.Request.Files[0].SaveAs(completepath);
                            path = Dirpath + filename;

                            plist.name = HttpContext.Current.Request.Files[0].FileName;
                            plist.path = path;
                            plist.fullpath = System.Configuration.ConfigurationManager.AppSettings["rootUrl"] + path.Substring(1);
                        }
                    }
                    else
                    {
                        error = "warning_Invalid File Format only pdf and jpg files are allow!";
                    }
                }
                else
                {
                    error = "invalid input!";
                }
            }
            else
            {
                error = "select only one file!";
            }

            plist.error = error;

            return Request.CreateResponse(HttpStatusCode.OK, plist);
        }
    }
}