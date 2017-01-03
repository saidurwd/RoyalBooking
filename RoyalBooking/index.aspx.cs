using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RoyalBooking
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!String.IsNullOrEmpty(Request.QueryString["cid"]))
            {
                // param is not set
                string companyID = Request.QueryString["cid"];
                Session["CompanyID"] = Request.QueryString["cid"];

                if (companyID == "1")
                {
                    Session["CompanyName"] = "Royal Flowers Inc. Domestic";
                }
                else if (companyID == "2")
                {
                    Session["CompanyName"] = "Royal Flowers Inc. International";
                }
                else
                {
                    Session["CompanyName"] = "Royal Flowers GmbH";
                }                
            }

            Label1.Text = Session["CompanyName"].ToString();
        }
    }
}