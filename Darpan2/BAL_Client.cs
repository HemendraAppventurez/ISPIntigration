using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Darpan2
{
    public class BAL_Client
    {
        public BAL_Client()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DataTable Bal_GetDataForPorting(int instance_Code, int project_Code, int clientGroupId, string client_DataDate, string Proc_name)
        {
            DAL_Client obj = new DAL_Client();
            return obj.Dal_GetDataForPorting(instance_Code, project_Code, clientGroupId, client_DataDate, Proc_name);
        }

        public void Bal_MsgLog(int instance_Code, int project_Code, string Proc_name, DateTime start, DateTime end, int status, string msg, int group_id = 0, string Datadate = "", int error_code = 0)
        {
            DAL_Client obj = new DAL_Client();
            obj.Dal_MsgLog(instance_Code, project_Code, Proc_name, start, end, status, msg, group_id, Datadate);
        }
    }
}