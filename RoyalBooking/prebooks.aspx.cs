using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
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
            //ImportPrebooks(BQ.DB_Base.KSRFIDomesticToken, BQ.DB_Base.KSRFIDomesticDataFrom);
            //ImportPrebooks(BQ.DB_Base.KSRFIInternationalToken, BQ.DB_Base.KSRFIInternationalDataFrom);

            ImportPrebooks(BQ.DB_Base.KSDemoToken, BQ.DB_Base.KSDemoDataFrom);
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

                    string validationfile = "";
                    validationfile = ExportToExcel(ds.Tables[1], "prebooks");
                    validationfile = ExportToExcel(ds.Tables[2], "details");

                    if (ds.Tables.Contains("breakdowns"))
                    {
                        validationfile = ExportToExcel(ds.Tables["breakdowns"], "breakdowns");
                    }

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
            //ReadKSPrebooks(BQ.DB_Base.KSRFIDomesticToken, BQ.DB_Base.KSRFIDomesticDataFrom);
            ReadKSPrebooks(BQ.DB_Base.KSDemoToken, BQ.DB_Base.KSDemoDataFrom);
        }

        protected void ReadKSPrebooks(string _KSToken, string _DataFrom)
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
        private string ExportToExcel(DataTable dt, string filename)
        {
            DateTime dte = DateTime.Now;
            int Year = dte.Year;
            int Month = dte.Month;
            int Date = dte.Day;
            int Hour = dte.Hour;
            int Minute = dte.Minute;
            int Second = dte.Second;
            string FileDate = Year.ToString() + "-" + Month.ToString() + "-" + Date.ToString() + " " + Hour.ToString() + " " + Minute.ToString() + " " + Second.ToString();
            filename = filename + "-" + FileDate;
            filename = filename.Replace(" ", "_");

            if (dt.Rows.Count > 0)
            {
                System.Web.UI.WebControls.DataGrid grid =
                            new System.Web.UI.WebControls.DataGrid();
                grid.HeaderStyle.Font.Bold = true;
                grid.DataSource = dt;


                grid.DataBind();
                string strFilePath = path + "logs/" + filename + ".xls";

                using (StreamWriter sw = new StreamWriter(strFilePath))
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        grid.RenderControl(hw);
                    }
                }

            }
            return filename + ".xls";
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
            string sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
            try
            {
                foreach (GridViewRow gr in GridView1.Rows)
                {
                    CheckBox _chkRow = gr.FindControl("chkRow") as CheckBox;
                    if (_chkRow.Checked == true)
                    {
                        TextBox _txtRefNo = gr.FindControl("txtKeyField") as TextBox;
                        TextBox _txtproductId = gr.FindControl("txtproductId") as TextBox;
                        
                        TextBox _txtNumber = gr.FindControl("txtNumber") as TextBox;
                        TextBox _txtProductDescription = gr.FindControl("txtProductDescription") as TextBox;
                        TextBox _txtTruckDate = gr.FindControl("txtTruckDate") as TextBox;

                        BQ.DC_BQ objBQ = new BQ.DC_BQ();
                        objBQ.KSToken = BQ.DB_Base.KSDemoToken;
                        objBQ.DataFrom = BQ.DB_Base.KSDemoDataFrom;
                        objBQ.PrebooksId = Int32.Parse(_txtRefNo.Text);
                        objBQ.ProductId = Int32.Parse(_txtproductId.Text);

                        objBQ.InvoiceNumber = _txtNumber.Text;
                        objBQ.SearchSrting = _txtProductDescription.Text; //As Product Description
                        objBQ.ProcessDay = _txtTruckDate.Text;

                        DeleteKSPrebooks objK = new BQ.DeleteKSPrebooks();
                        DataSet ds = objK.DeleteKSPrebooksById(objBQ);

                        if (ds.Tables[0].Rows[0][1].ToString() == "1")
                        {
                            UpdateKSPrebook objUpdate = new BQ.UpdateKSPrebook();
                            objUpdate.UpdatePrebookDeleteStatus(objBQ);
                        }

                        CreateErrorLog("CustomLogs/ErrorLogPrebookDeleteItems", sLogFormat+ " Number: " + _txtNumber.Text + " - Product: " + _txtProductDescription.Text + "  - Data From: " + objBQ.DataFrom + "  - API Message: " + ds.Tables[0].Rows[0][0].ToString());
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