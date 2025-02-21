using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Models
{
    public class MailerClass
    {
        CCSHealthFamilyWelfareDept.DAL.Common_DB objComnDb = new DAL.Common_DB();
        public string sendMail(string[] mailTo, string[] BCCMailID1, string[] mailCC, string mailFrom, string mailMsg, string mailSub)
        {
            MailMessage mlMsg = new MailMessage();
            SmtpClient smpt = new SmtpClient(ConfigurationManager.ConnectionStrings["MailServer"].ToString(), 25);

            if (ConfigurationManager.ConnectionStrings["MailUserID"].ToString() != "")
            {
                smpt.Credentials = new System.Net.NetworkCredential(ConfigurationManager.ConnectionStrings["MailUserID"].ToString(), ConfigurationManager.ConnectionStrings["MailUserPWD"].ToString());
            }


            foreach (string mt in mailTo)
            {
                if (mt != null)
                {
                    if (mt.Trim() != "")
                    {
                        mlMsg.To.Add(mt);
                    }
                }
            }

            if (BCCMailID1 != null)
            {
                foreach (var bcc in BCCMailID1)
                {
                    if (bcc != null)
                    {
                        if (bcc.Trim() != "")
                        {
                            mlMsg.Bcc.Add(bcc);
                        }
                    }
                }
            }

            if (mailCC != null)
            {
                foreach (var cc in mailCC)
                {
                    if (cc != null)
                    {
                        if (cc.Trim() != "")
                        {
                            mlMsg.CC.Add(cc);
                        }
                    }
                }
            }

            try
            {
                string[] cp = ConfigurationManager.ConnectionStrings["copyMail"].ConnectionString.Split(';');

                if (cp != null)
                {
                    foreach (var bcc in cp)
                    {
                        if (bcc != null)
                        {
                            if (bcc.Trim() != "")
                            {
                                mlMsg.Bcc.Add(bcc);
                            }
                        }
                    }
                }
            }
            catch (Exception t)
            {

            }


            mlMsg.From = new MailAddress(mailFrom);

            mlMsg.Body = mailMsg;
            mlMsg.Subject = mailSub;
            //mlMsg.BodyEncoding = System.Text.Encoding.Unicode;
            mlMsg.IsBodyHtml = true;

            try
            {
                smpt.Send(mlMsg);
                objComnDb.EmailLog(mailTo[0], mailSub, mailMsg, true, "Success");
                return "Success";
            }
            catch (Exception ex)
            {
                objComnDb.EmailLog(mailTo[0], mailSub, mailMsg, false, ex.Message);
                return ex.Message;
            }
        }

        public string sendMail(string[] mailTo, string[] BCCMailID1, string[] mailCC, string mailFrom, string mailMsg, string mailSub, string Attachment)
        {
            MailMessage mlMsg = new MailMessage();
            SmtpClient smpt = new SmtpClient(ConfigurationManager.ConnectionStrings["MailServer"].ToString(), 25);

            if (ConfigurationManager.ConnectionStrings["MailUserID"].ToString() != "")
            {
                smpt.Credentials = new System.Net.NetworkCredential(ConfigurationManager.ConnectionStrings["MailUserID"].ToString(), ConfigurationManager.ConnectionStrings["MailUserPWD"].ToString());
            }


            foreach (string mt in mailTo)
            {
                if (mt != null)
                {
                    if (mt.Trim() != "")
                    {
                        mlMsg.To.Add(mt);
                    }
                }
            }

            if (BCCMailID1 != null)
            {
                foreach (var bcc in BCCMailID1)
                {
                    if (bcc != null)
                    {
                        if (bcc.Trim() != "")
                        {
                            mlMsg.Bcc.Add(bcc);
                        }
                    }
                }
            }

            if (mailCC != null)
            {
                foreach (var cc in mailCC)
                {
                    if (cc != null)
                    {
                        if (cc.Trim() != "")
                        {
                            mlMsg.CC.Add(cc);
                        }
                    }
                }
            }

            try
            {
                string[] cp = ConfigurationManager.ConnectionStrings["copyMail"].ConnectionString.Split(';');

                if (cp != null)
                {
                    foreach (var bcc in cp)
                    {
                        if (bcc != null)
                        {
                            if (bcc.Trim() != "")
                            {
                                mlMsg.Bcc.Add(bcc);
                            }
                        }
                    }
                }
            }
            catch (Exception t)
            {

            }


            mlMsg.From = new MailAddress(mailFrom);
            if (!string.IsNullOrEmpty(Attachment))
            {
                string Path = HttpContext.Current.Server.MapPath(Attachment);
                mlMsg.Attachments.Add(new Attachment((Path)));
            }
            mlMsg.Body = mailMsg;
            mlMsg.Subject = mailSub;
            //mlMsg.BodyEncoding = System.Text.Encoding.Unicode;
            mlMsg.IsBodyHtml = true;

            try
            {
                smpt.Send(mlMsg);
                objComnDb.EmailLog(mailTo[0], mailSub, mailMsg, true, "Success");
                return "Success";
            }
            catch (Exception ex)
            {
                objComnDb.EmailLog(mailTo[0], mailSub, mailMsg, false, ex.Message);
                return ex.Message;
            }
        }
    }
}