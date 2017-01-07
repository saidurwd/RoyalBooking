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
            //ImportLastDaysPOData(BQ.DB_Base.KSDemoToken, BQ.DB_Base.KSDemoDataFrom);
        }
        protected void ImportLastDaysPOData(string _KSToken, string _DataFrom)
        {

            //RFG - 2014 - 07 - 26 to 2015 - 06 - 10
            //Int - 2015-05-07 to 2016-03-09
            //Domestic - 2013-09-15 to 2016-03-09
            //Joy - 2014-07-01 to 2016-03-09
            //string FromDate = "12/08/2016";
            //string ToDate = "12/15/2016";
            string FromDate = txtDateFrom.Value;// "12/25/2016";
            string ToDate = txtDateTo.Value; //"12/30/2016";
            //try
            //{
            BQ.DC_BQ objBQ = new BQ.DC_BQ();
            objBQ.KSToken = _KSToken;
            objBQ.DataFrom = _DataFrom;
            objBQ.CallFrom = BQ.DB_Base.Call_From_Monthly_Sales;

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
            
            BQ.ImportKSPO objK = new BQ.ImportKSPO();
            objK.DeleteAll(objBQ);
            if (dtdate.Rows.Count > 0)
            {
                for (int i = 0; i < dtdate.Rows.Count; i++)
                {
                    truckdate2 = dtdate.Rows[i]["truckdate2"].ToString();
                    objBQ.FromDate = truckdate2;
                    objBQ.ToDate = truckdate2;
                    objBQ.ProcessDay = truckdate2;

                    objK = new BQ.ImportKSPO();
                    DataSet ds = objK.ImportKSInvoideDataForAzure(objBQ);
                    //objK.ImportKSInvoideDataForAzure(objBQ);

                    string validationfile = "";
                    validationfile = ExportToExcel(ds.Tables[1], "purchaseOrders");
                    validationfile = ExportToExcel(ds.Tables[2], "details");

                    if (ds.Tables.Contains("breakdowns"))
                    {
                        validationfile = ExportToExcel(ds.Tables["breakdowns"], "breakdowns");
                    }
                    if (ds.Tables.Contains("boxes"))
                    {
                        validationfile = ExportToExcel(ds.Tables["boxes"], "boxes");
                    }
                    if (ds.Tables.Contains("customFields"))
                    {
                        validationfile = ExportToExcel(ds.Tables["customFields"], "customFields");
                    }
                    if (ds.Tables.Contains("vendorAvailabilityDetails"))
                    {
                        validationfile = ExportToExcel(ds.Tables["vendorAvailabilityDetails"], "vendorAvailabilityDetails");
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
                        TextBox _txtRefNo = gr.FindControl("txtKeyField") as TextBox; //PO ID
                        TextBox _txtprebook = gr.FindControl("txtprebook") as TextBox; //Prebook ID
                        TextBox _txtprebookItemId = gr.FindControl("txtprebookItemId") as TextBox; //Prebook Item Id
                        TextBox _txtpoItemId = gr.FindControl("txtpoItemId") as TextBox; //PO Item Id
                        TextBox _txtNumber = gr.FindControl("txtNumber") as TextBox; //PO Number
                        TextBox _txtProductDescription = gr.FindControl("txtProductDescription") as TextBox;
                        TextBox _txtTruckDate = gr.FindControl("txtTruckDate") as TextBox;

                        BQ.DC_BQ objBQ = new BQ.DC_BQ();
                        objBQ.KSToken = BQ.DB_Base.KSDemoToken;
                        objBQ.DataFrom = BQ.DB_Base.KSDemoDataFrom;
                        objBQ.PrebooksId = Int32.Parse(_txtprebook.Text);
                        objBQ.ProductId = Int32.Parse(_txtprebookItemId.Text); //Prebook Item Id
                        objBQ.poItemId = Int32.Parse(_txtpoItemId.Text); //PO Item Id
                        
                        
                        objBQ.SearchSrting = _txtProductDescription.Text; //As Product Description
                        objBQ.ProcessDay = _txtTruckDate.Text;

                        
                        ImportKSPrebooks objIP = new ImportKSPrebooks();
                        DataSet dsPrebook = objIP.getPrebookSummary(_txtTruckDate.Text, objBQ);
                        objBQ.InvoiceNumber = dsPrebook.Tables["prebooks"].Rows[0]["id"].ToString();

                        DeleteKSPrebooks objK = new BQ.DeleteKSPrebooks();
                        DataSet dsPO = objK.DeleteKSPOById(objBQ);
                        DataSet ds = objK.DeleteKSPrebooksById(objBQ);





                        if (ds.Tables[0].Rows[0][1].ToString() == "1")
                        {
                            UpdateKSPrebook objUpdate = new BQ.UpdateKSPrebook();
                            objUpdate.UpdatePrebookDeleteStatus(objBQ);
                        }

                        CreateErrorLog("CustomLogs/LogPrebookDeleteItems", sLogFormat + " Number: " + _txtNumber.Text + " - Product: " + _txtProductDescription.Text + "  - Data From: " + objBQ.DataFrom + "  - API Message: " + ds.Tables[0].Rows[0][0].ToString());
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

/*
its called: purchase.order.item.delete
and this is the main information:Parámetros de entrada:
authenticationToken (required)(string:50): Komet Sales security token.
poItemId (required)(integer:20): Komet Sales internal ID of the PO item that you want to delete. You can obtain this value from the purchase.order.list API method.
Parámetros de salida:
status (integer:1): transaction status. 1 for success or 0 for failure.
message (string:500): description of the status of the transaction.

    */
