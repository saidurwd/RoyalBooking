using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security;

namespace RoyalBooking
{
    public partial class login1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            if (System.Web.Security.FormsAuthentication.Authenticate(txtUserName.Text, txtPassword.Text))
            {
                System.Web.Security.FormsAuthentication.RedirectFromLoginPage(txtUserName.Text, false);
            }
            else
            {
                lblErrorMessage.Text = "Invalid username and/or password!";
            }
        }
    }
}