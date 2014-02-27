using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;
using System.IO;

using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using Amazon.S3;
using Amazon.S3.Model;
using ProjectWeb.DataService;

namespace ProjectWeb
{
    public partial class Login : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            lblFeedBack.Visible = false;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = tbUName.Text;
            string password = tbPwd.Text;
            User user = DataServices.LoginUser(email, password);
            if (user != null)
            {
               Session["User"] = user;
               Response.Redirect("FilesDisplay.aspx");
            }
            else
            {
                lblFeedBack.Visible = true;
                lblFeedBack.Text = "Sorry! Please enter existing emailid/password.";
            }
        }
    }
}