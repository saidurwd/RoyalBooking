using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BQ;


namespace RoyalBooking
{
    public partial class prebooks : System.Web.UI.Page
    {
        String path = HttpContext.Current.Request.PhysicalApplicationPath;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Prebooks - Date Duration
            ImportPrebooks(BQ.DB_Base.KSRFIDomesticToken, BQ.DB_Base.KSRFIDomesticDataFrom);
            //ImportPrebooks(BQ.DB_Base.KSRFIInternationalToken, BQ.DB_Base.KSRFIInternationalDataFrom);
        }
        protected void ImportPrebooks(string _KSToken, string _DataFrom)
        {

            string FromDate = txtDateFrom.Value;// "12/25/2016";
            string ToDate = txtDateTo.Value; //"12/30/2016";
            //try
            //{
            BQ.DC_BQ objBQ = new BQ.DC_BQ();
            objBQ.KSToken = _KSToken;
            objBQ.DataFrom = _DataFrom;
            objBQ.CallFrom = BQ.DB_Base.Call_From_Import_Prebooks_Date_Duration;

            string conString = BQ.DB_Base.DB_STR;
            string strSQL = "";
            strSQL = @"
        	select replace(convert(varchar(10),date," + BQ.DB_Base.BQDataRegion + @"),'/','-') truckdate2 
            from [dbo].[DateBackbone](convert(datetime,'" + FromDate + @"'," + BQ.DB_Base.BQDataRegion + @"), convert(datetime,'" + ToDate + @"'," + BQ.DB_Base.BQDataRegion + @"));";

            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            DataTable dtdate = new DataTable();

            adpt.Fill(dtdate);
            con.Close();
            string truckdate2 = "";
            BQ.ImportKSPrebooks objK = new BQ.ImportKSPrebooks();
            objK.DeleteDataAll(objBQ);

            if (dtdate.Rows.Count > 0)
            {
                for (int i = 0; i < dtdate.Rows.Count; i++)
                {
                    truckdate2 = dtdate.Rows[i]["truckdate2"].ToString();
                    objBQ.FromDate = truckdate2;
                    objBQ.ToDate = truckdate2;
                    objBQ.ProcessDay = truckdate2;

                    objK = new BQ.ImportKSPrebooks();
                    DataSet ds = objK.ImportKSPrebooksDataForAzure(objBQ);

                }
            }
            //}
            //catch (Exception exp)
            //{
            //    sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
            //    CreateErrorLog("CustomLogs/ErrorLog", sLogFormat + " : " + FromDate+ " : " + ToDate + " : " + _DataFrom);
            //    CreateErrorLog("CustomLogs/ErrorLog", exp.ToString());
            //}
            //finally
            //{

            //}
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            //Read Prebooks
            ReradKSPrebooks(BQ.DB_Base.KSRFIDomesticToken, BQ.DB_Base.KSRFIDomesticDataFrom);
        }

        protected void ReradKSPrebooks(string _KSToken, string _DataFrom)
        {
            try
            {
                BQ.DC_BQ objBQ = new BQ.DC_BQ();
                objBQ.KSToken = _KSToken;
                objBQ.SearchSrting = txtdescription.Value;
                objBQ.FromDate = txtDateFrom.Value;
                objBQ.ToDate = txtDateTo.Value;
                objBQ.DataFrom = _DataFrom;

                ReadKSData objK = new BQ.ReadKSData();
                DataSet ds = objK.ReadKSPrebooks(objBQ);
                GridView1.DataSource = ds.Tables[0];
                GridView1.DataBind();
            }
            catch (Exception exp)
            {
                CreateErrorLog("CustomLogs/ErrorLog", exp.ToString());
            }
            finally
            {

            }
        }
        private string CreateListOfItem2()
        {
            StringBuilder strServiceList = new StringBuilder();
            foreach (GridViewRow gr in GridView1.Rows)
            {

                CheckBox _chkRow = gr.FindControl("chkRow") as CheckBox;
                if (_chkRow.Checked == true)
                {
                    TextBox _txtRefNo = gr.FindControl("txtKeyField") as TextBox;
                    strServiceList.Append("" + _txtRefNo.Text + "");
                    strServiceList.Append(",");
                }
            }
            string strText = strServiceList.ToString();
            if (strText.Length > 0)
            {
                strText = strText.Substring(0, strText.Length - 1);
            }
            return strText;

        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow gr in GridView1.Rows)
                {
                    CheckBox _chkRow = gr.FindControl("chkRow") as CheckBox;
                    if (_chkRow.Checked == true)
                    {
                        TextBox _txtRefNo = gr.FindControl("txtKeyField") as TextBox;
                        TextBox _txtproductId = gr.FindControl("txtproductId") as TextBox;
                        
                        BQ.DC_BQ objBQ = new BQ.DC_BQ();
                        objBQ.KSToken = BQ.DB_Base.KSRFIDomesticToken;
                        objBQ.DataFrom = BQ.DB_Base.KSRFIDomesticDataFrom;
                        objBQ.PrebooksId =Int32.Parse(_txtRefNo.Text);
                        objBQ.ProductId = Int32.Parse(_txtproductId.Text);

                        //DeleteKSPrebooks objK = new BQ.DeleteKSPrebooks();
                        //DataSet ds = objK.DeleteKSPrebooksById(objBQ);

                        UpdateKSPrebook objUpdate = new BQ.UpdateKSPrebook();
                        objUpdate.UpdatePrebookDeleteStatus(objBQ);

                        CreateErrorLog("CustomLogs/ErrorLogPrebookDeleteItems", "Number: " + _txtRefNo.Text + " Product Id:" + _txtproductId.Text + " Data From: "  + objBQ.DataFrom);
                    }
                }
            }
            catch (Exception exp)
            {
                CreateErrorLog("CustomLogs/ErrorLogPrebookDelete", exp.ToString());
            }
            finally
            {

            }
        }
        protected void CreateErrorLog(string _localLogPath, string _message)
        {
            BQ.CreateLogFiles Err = new BQ.CreateLogFiles();
            Err.ErrorLog((path + _localLogPath), _message);
        }
    }
}