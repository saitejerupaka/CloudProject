using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace ProjectDataService
{
    public class DataService
    {
        string connString = ConfigurationManager.ConnectionStrings["GridProject"].ConnectionString;
        SqlCommand cmd;
        SqlConnection con;
        public int RegisterUser(string email, string fname, string lname, string pwd, string bucketName)
        {
            int added = 0;
            using (con = new SqlConnection(@connString))
            {
                con.Open();
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_RegisterUser";
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@fname", fname);
                    cmd.Parameters.AddWithValue("@lname", lname);
                    cmd.Parameters.AddWithValue("@pwd", pwd);
                    cmd.Parameters.AddWithValue("@bucketname", bucketName);
                    added = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return added;
        }

        public User LoginUser(string useremail, string pwd)
        {
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

        public int AddFilePath(string FilePath, string Email, string FileName)
        {
            int added = 0;
            using (con = new SqlConnection(@connString))
            {
                con.Open();
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_AddFilePath";
                    cmd.Parameters.AddWithValue("@email", Email);
                    cmd.Parameters.AddWithValue("@filepath", FilePath);
                    cmd.Parameters.AddWithValue("@filename", FileName);
                    added = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return added;
        }

        public List<FileMetaData> GetUploadFileList(string email)
        {
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
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
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

        public int UpdateFilePath(int FileID, string FilePath, string FileName)
        {
            int updated = 0;
            using (con = new SqlConnection(@connString))
            {
                con.Open();
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_UpdateFilePath";
                    cmd.Parameters.AddWithValue("@FileID", FileID);
                    cmd.Parameters.AddWithValue("@FilePath", FilePath);
                    cmd.Parameters.AddWithValue("@FileName", FileName);
                    updated = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return updated;

        }

        public int UpdateLastModified(int FileID)
        {
            int updated = 0;
            using (con = new SqlConnection(@connString))
            {
                con.Open();
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_UpdateLastModified";
                    cmd.Parameters.AddWithValue("@file_ID", FileID);
                    updated = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return updated;
        }

        public int DeleteFile(int FileID)
        {
            int deleted = 0;
            using (con = new SqlConnection(@connString))
            {
                con.Open();
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_DeleteFile";
                    cmd.Parameters.AddWithValue("@file_ID", FileID);
                    deleted = cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return deleted;
        }


    }
}
