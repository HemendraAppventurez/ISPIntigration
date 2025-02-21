using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCSHealthFamilyWelfareDept.Darpan2
{
    public class DAL_Client
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        public DAL_Client()
        {

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["CMSModule"].ConnectionString;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
        }

        public DataTable Dal_GetDataForPorting(int instance_Code, int project_Code, int clientGroupId, string client_DataDate, string Proc_name)
        {
            DataTable dtData = new DataTable();
            cmd.CommandText = Proc_name;
            //cmd.Parameters.AddWithValue("@instance_Code", instance_Code);
            //cmd.Parameters.AddWithValue("@project_Code", project_Code);
            //cmd.Parameters.AddWithValue("@clientGroupId", clientGroupId);
            //cmd.Parameters.AddWithValue("@client_DataDate", client_DataDate);
            cmd.Parameters.AddWithValue("@fromDate", client_DataDate);
            cmd.Parameters.AddWithValue("@toDate", client_DataDate);
            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                sda.Fill(dtData);
            return dtData;
        }


        internal int Dal_MsgLog(int instance_Code, int project_Code, string Proc_name, DateTime start, DateTime end, int status, string msg, int group_id = 0, string Datadate = "", int error_code = 0)
        {
            cmd.CommandText = Proc_name;
            cmd.Parameters.AddWithValue("@instance_Code", instance_Code);
            cmd.Parameters.AddWithValue("@project_Code", project_Code);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@group_id", group_id);
            cmd.Parameters.AddWithValue("@Req_Start_dt", start);
            cmd.Parameters.AddWithValue("@Req_End_dt", end);
            cmd.Parameters.AddWithValue("@msg", msg);
            cmd.Parameters.AddWithValue("@Datadate", Datadate);
            cmd.Parameters.AddWithValue("@error_code", error_code);
            conn.Open();
            int rowsAffected = cmd.ExecuteNonQuery();
            conn.Close();
            return rowsAffected;
        }
    }
}