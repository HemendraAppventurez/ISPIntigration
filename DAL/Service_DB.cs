using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.DAL
{
    public class Service_DB
    {
        public DataTable GetLoginDetails(string UserName, string Password)
        {
            DataTable dt = new DataTable();
            Database dbobj = DatabaseFactory.CreateDatabase();
            DbCommand dbcmd = dbobj.GetStoredProcCommand("WINAPP_GetLoginDetails");
            dbobj.AddInParameter(dbcmd, "@userName", DbType.String, UserName);
            dbobj.AddInParameter(dbcmd, "@password", DbType.String, Password);
            try
            {
                IDataReader dr = dbobj.ExecuteReader(dbcmd);
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }

        public DataTable RegisterCertificateSerialNo(long profileId, string cerSerialNo)
        {
            DataTable dt = new DataTable();
            Database dbobj = DatabaseFactory.CreateDatabase();
            DbCommand dbcmd = dbobj.GetStoredProcCommand("WINAPP_RegisterCertificateSerialNo");
            dbobj.AddInParameter(dbcmd, "@profileid", DbType.Int64, profileId);
            dbobj.AddInParameter(dbcmd, "@certificateSerialNo", DbType.String, cerSerialNo);
            try
            {
                IDataReader dr = dbobj.ExecuteReader(dbcmd);
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }

        public DataTable GetSignatureDetails(string serviceCode)
        {
            DataTable dt = new DataTable();
            Database dbobj = DatabaseFactory.CreateDatabase();
            DbCommand dbcmd = dbobj.GetStoredProcCommand("proc_GetSignatureDetailsByServiceCode");
            dbobj.AddInParameter(dbcmd, "@serviceCode", DbType.String, serviceCode);
            try
            {
                IDataReader dr = dbobj.ExecuteReader(dbcmd);
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }

        public DataTable GetESDetailsByCMOId(long profileId, string WebdirUrl)
        {
            DataTable dt = new DataTable();
            Database dbobj = DatabaseFactory.CreateDatabase();
            DbCommand dbcmd = dbobj.GetStoredProcCommand("Proc_WSGetCompleteESDetailsByCMOId");
            dbobj.AddInParameter(dbcmd, "@cmoId ", DbType.Int64, profileId);
            dbobj.AddInParameter(dbcmd, "@DirUrl", DbType.String, WebdirUrl);
            try
            {
                IDataReader dr = dbobj.ExecuteReader(dbcmd);
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }

        public DataTable GetUnsignedCertificateDetails(long cmoProfileId, string webDirUrl, string procName)
        {
            DataTable dt = new DataTable();
            Database dbobj = DatabaseFactory.CreateDatabase();
            DbCommand dbcmd = dbobj.GetStoredProcCommand(procName);
            dbobj.AddInParameter(dbcmd, "@cmoProfileId", DbType.Int64, cmoProfileId);
            dbobj.AddInParameter(dbcmd, "@webDirUrl", DbType.String, webDirUrl);
            try
            {
                IDataReader dr = dbobj.ExecuteReader(dbcmd);
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }

        public DataTable WebServiceAuthentication(string userName, string password, string appVersion)
        {
            DataTable dt = new DataTable();
            Database dbobj = DatabaseFactory.CreateDatabase();
            DbCommand dbcmd = dbobj.GetStoredProcCommand("proc_WebServiceAuthentication");
            dbobj.AddInParameter(dbcmd, "@userName", DbType.String, userName);
            dbobj.AddInParameter(dbcmd, "@password", DbType.String, password);
            dbobj.AddInParameter(dbcmd, "@srvVersion", DbType.String, appVersion);
            try
            {
                IDataReader dr = dbobj.ExecuteReader(dbcmd);
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            return dt;
        }
    }
}