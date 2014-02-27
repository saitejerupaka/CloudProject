using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using ProjectWeb.DataService;


namespace ProjectWeb.Web
{
    public partial class FilesDisplay : System.Web.UI.Page
    {
        private User CurrentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentUser = GetUser();
            if (CurrentUser == null)
            {
                Response.Redirect("Login.aspx");
            }
            lblUserName.Text = "Hello! " + CurrentUser.FirstName + "," + CurrentUser.LastName +". Below are your list of files backed up. ";
            gvFiles.DataSource = DataServices.GetUploadFileList(CurrentUser.UserEmail);
            gvFiles.DataBind();
        }

        private User GetUser()
        {
            return (User)Session["User"];
        }

        private void ReadFile(string bucketName, string fileName)
        {
            string dest = Path.Combine(HttpRuntime.CodegenDir, fileName);
            IAmazonS3 client;
            using (client = Amazon.AWSClientFactory.CreateAmazonS3Client())
            {
                try
                {
                    GetObjectRequest request = new GetObjectRequest()
                    {
                        BucketName = bucketName,
                        Key = fileName
                    };
                    using (GetObjectResponse response = client.GetObject(request))
                    {
                        if (!File.Exists(dest))
                        {
                            response.WriteResponseStreamToFile(dest);
                        }
                    }
                }

                catch (AmazonS3Exception amazonS3Exception)
                {
                    if (amazonS3Exception.ErrorCode != null &&
                        (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                        amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        Console.WriteLine("Please check the provided AWS Credentials.");
                        Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                    }
                    else
                    {
                        Console.WriteLine("An error occurred with the message '{0}' when reading an object", amazonS3Exception.Message);
                    }
                }
            }
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.TransmitFile(dest);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();

            // Clean up temporary file.
            System.IO.File.Delete(dest);
        }

        protected void gvFiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":
                    int index = Convert.ToInt32(e.CommandArgument);
                    string filename = gvFiles.Rows[index].Cells[0].Text;
                    ReadFile(CurrentUser.BucketName, filename);
                    break;
                default:
                    break;
            }
        }

        protected void lbtnLogOut_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Session.Abandon();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Redirect("Login.aspx");
        }

    }
}