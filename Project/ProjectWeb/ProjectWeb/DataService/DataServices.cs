using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProjectWeb.DataService
{
    public static class DataServices
    {

        public static User LoginUser(string useremail, string pwd)
        {
            string connString = ConfigurationManager.ConnectionStrings["GridProject"].ConnectionString;
            SqlCommand cmd;
            SqlConnection con;
            SqlDataAdapter sda;
            DataSet ds;
            User user = null;
            using (con = new SqlConnection(connString))
            {
                con.Open();
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_LoginUser";
                    cmd.Parameters.AddWithValue("@email", useremail);
                    cmd.Parameters.AddWithValue("@pwd", pwd);
                    sda = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    sda.Fill(ds);
                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        user = new User();
                        user.UserEmail = ds.Tables[0].Rows[0]["UserEmail"].ToString();
                        user.BucketName = ds.Tables[0].Rows[0]["BucketName"].ToString();
                        user.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                        user.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                    }
                }
                con.Close();
            }
            return user;
        }

        public static List<FileMetaData> GetUploadFileList(string email)
        {
            string connString = ConfigurationManager.ConnectionStrings["GridProject"].ConnectionString;
            SqlCommand cmd;
            SqlConnection con;
            SqlDataAdapter sda;
            DataSet ds;
            List<FileMetaData> filelist = null;
            FileMetaData file = null;
            using (con = new SqlConnection(@connString))
            {
                con.Open();
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_GetFileList";
                    cmd.Parameters.AddWithValue("@email", email);
                    sda = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    sda.Fill(ds);
                    filelist = new List<FileMetaData>();
                    if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            file = new FileMetaData();
                            file.FileID = Convert.ToInt16(row["FileID"]);
                            file.FileName = row["FileName"] as string;
                            file.FilePath = row["FilePath"] as string;
                            file.Email_User = row["Email_User"] as string;
                            file.LastModified = Convert.ToDateTime(row["LastModified"]);
                            filelist.Add(file);
                        }
                    }
                }
                con.Close();
            }
            return filelist;
        }
    }
}