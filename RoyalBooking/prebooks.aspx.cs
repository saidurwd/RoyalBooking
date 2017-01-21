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
            if (Session["CompanyID"] == null)
            {
                Response.Redirect("login.aspx");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Prebooks - Date Duration
            //ImportPrebooks(BQ.DB_Base.KSRFIDomesticToken, BQ.DB_Base.KSRFIDomesticDataFrom);
            //ImportPrebooks(BQ.DB_Base.KSRFIInternationalToken, BQ.DB_Base.KSRFIInternationalDataFrom);

            //ImportPrebooks(BQ.DB_Base.KSDemoToken, BQ.DB_Base.KSDemoDataFrom);

            if (Session["CompanyID"] == null)
            {
                return;
            }
            string companyID = Session["CompanyID"].ToString();
            if (companyID == "1")
            {
                ImportLastDaysPOData(BQ.DB_Base.KSRFIDomesticToken, BQ.DB_Base.KSRFIDomesticDataFrom);
            }
            else if (companyID == "2")
            {
                ImportLastDaysPOData(BQ.DB_Base.KSRFIInternationalToken, BQ.DB_Base.KSRFIInternationalDataFrom);
            }
            else if (companyID == "3")
            {
                ImportLastDaysPOData(BQ.DB_Base.KSGmbHToken, BQ.DB_Base.KSGmbHDataFrom);
            }
            else
            {
                ImportLastDaysPOData(BQ.DB_Base.KSDemoToken, BQ.DB_Base.KSDemoDataFrom);
            }

        }
        protected void ImportLastDaysPOData(string _KSToken, string _DataFrom)
        {


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
                    objBQ.OrderStatus = "Confirmed by Farm";
                    DataSet ds = objK.ImportKSInvoideDataForAzure(objBQ);

                    objK = new BQ.ImportKSPO();
                    objBQ.OrderStatus = "Approved";
                    ds = objK.ImportKSInvoideDataForAzure(objBQ);

                    objK = new BQ.ImportKSPO();
                    objBQ.OrderStatus = "Pending Approval";
                    ds = objK.ImportKSInvoideDataForAzure(objBQ);


                    //objK.ImportKSInvoideDataForAzure(objBQ);

                    //string validationfile = "";
                    //validationfile = ExportToExcel(ds.Tables[1], "purchaseOrders");
                    //validationfile = ExportToExcel(ds.Tables[2], "details");

                    //if (ds.Tables.Contains("breakdowns"))
                    //{
                    //    validationfile = ExportToExcel(ds.Tables["breakdowns"], "breakdowns");
                    //}
                    //if (ds.Tables.Contains("boxes"))
                    //{
                    //    validationfile = ExportToExcel(ds.Tables["boxes"], "boxes");
                    //}
                    //if (ds.Tables.Contains("customFields"))
                    //{
                    //    validationfile = ExportToExcel(ds.Tables["customFields"], "customFields");
                    //}
                    //if (ds.Tables.Contains("vendorAvailabilityDetails"))
                    //{
                    //    validationfile = ExportToExcel(ds.Tables["vendorAvailabilityDetails"], "vendorAvailabilityDetails");
                    //}
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
            //ReadKSPrebooks(BQ.DB_Base.KSDemoToken, BQ.DB_Base.KSDemoDataFrom);
            if (Session["CompanyID"] == null)
            {
                return;
            }
            string companyID = Session["CompanyID"].ToString();
            if (companyID == "1")
            {
                ReadKSPrebooks(BQ.DB_Base.KSRFIDomesticToken, BQ.DB_Base.KSRFIDomesticDataFrom);
            }
            else if (companyID == "2")
            {
                ReadKSPrebooks(BQ.DB_Base.KSRFIInternationalToken, BQ.DB_Base.KSRFIInternationalDataFrom);
            }
            else if (companyID == "3")
            {
                ReadKSPrebooks(BQ.DB_Base.KSGmbHToken, BQ.DB_Base.KSGmbHDataFrom);
            }
            else
            {
                ReadKSPrebooks(BQ.DB_Base.KSDemoToken, BQ.DB_Base.KSDemoDataFrom);
            }

        }
        protected void btnImportVA_Click(object sender, EventArgs e)
        {

            //Read Prebooks
            //ReadKSPrebooks(BQ.DB_Base.KSRFIDomesticToken, BQ.DB_Base.KSRFIDomesticDataFrom);
            //ReadKSPrebooks(BQ.DB_Base.KSDemoToken, BQ.DB_Base.KSDemoDataFrom);
            if (Session["CompanyID"] == null)
            {
                return;
            }
            string companyID = Session["CompanyID"].ToString();
            if (companyID == "1")
            {
                ProcessVendorAvailabilityData(BQ.DB_Base.KSRFIDomesticToken, BQ.DB_Base.KSRFIDomesticDataFrom);
            }
            else if (companyID == "2")
            {
                ProcessVendorAvailabilityData(BQ.DB_Base.KSRFIInternationalToken, BQ.DB_Base.KSRFIInternationalDataFrom);
            }
            else if (companyID == "3")
            {
                ProcessVendorAvailabilityData(BQ.DB_Base.KSGmbHToken, BQ.DB_Base.KSGmbHDataFrom);
            }
            else
            {
                ProcessVendorAvailabilityData(BQ.DB_Base.KSDemoToken, BQ.DB_Base.KSDemoDataFrom);
            }

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
                objBQ.vendorName = txtvendor.Value;
                objBQ.OrderStatus = ddOrderType.SelectedItem.Value.ToString();

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
            string strFilePath = "";
            if (dt.Rows.Count > 0)
            {
                System.Web.UI.WebControls.DataGrid grid =
                            new System.Web.UI.WebControls.DataGrid();
                grid.HeaderStyle.Font.Bold = true;
                grid.DataSource = dt;


                grid.DataBind();
                strFilePath = path + "logs/" + filename + ".xls";

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
            try
            {
                DeletePOPB();
                Button2_Click(null, null);
                try
                {
                    ReadKSData objLog = new BQ.ReadKSData();
                    DataTable dt = objLog.ReadDeleteMoveLog();
                    GenerateExcelAndDownload(dt);
                }
                catch (Exception expInner)
                {

                }
            }
            catch (Exception exp)
            {
                CreateErrorLog("CustomLogs/LogPrebookDeleteError", exp.ToString());
            }
            //string _KSToken = "";
            //string _KSDataFrom = "";
            //if (Session["CompanyID"] == null)
            //{
            //    return;
            //}
            //string companyID = Session["CompanyID"].ToString();
            //if (companyID == "1")
            //{
            //    _KSToken = BQ.DB_Base.KSRFIDomesticToken; _KSDataFrom = BQ.DB_Base.KSRFIDomesticDataFrom;
            //}
            //else if (companyID == "2")
            //{
            //    _KSToken = BQ.DB_Base.KSRFIInternationalToken; _KSDataFrom = BQ.DB_Base.KSRFIInternationalDataFrom;
            //}
            //else if (companyID == "3")
            //{
            //    _KSToken = BQ.DB_Base.KSGmbHToken; _KSDataFrom = BQ.DB_Base.KSGmbHDataFrom;

            //}
            //else
            //{
            //    _KSToken = BQ.DB_Base.KSDemoToken; _KSDataFrom = BQ.DB_Base.KSDemoDataFrom;
            //}
            ////_KSToken = BQ.DB_Base.KSDemoToken; _KSDataFrom = BQ.DB_Base.KSDemoDataFrom;
            //string sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
            //UpdateKSPrebook objUpdate = new BQ.UpdateKSPrebook();
            //objUpdate.UpdatePrebookDeleteStatusAll();
            //try
            //{
            //    foreach (GridViewRow gr in GridView1.Rows)
            //    {
            //        CheckBox _chkRow = gr.FindControl("chkRow") as CheckBox;
            //        if (_chkRow.Checked == true)
            //        {
            //            TextBox _txtRefNo = gr.FindControl("txtKeyField") as TextBox; //PO ID
            //            TextBox _txtprebook = gr.FindControl("txtprebook") as TextBox; //Prebook ID
            //            TextBox _txtprebookItemId = gr.FindControl("txtprebookItemId") as TextBox; //Prebook Item Id
            //            TextBox _txtpoItemId = gr.FindControl("txtpoItemId") as TextBox; //PO Item Id
            //            TextBox _txtNumber = gr.FindControl("txtNumber") as TextBox; //PO Number
            //            TextBox _txtProductDescription = gr.FindControl("txtProductDescription") as TextBox;
            //            TextBox _txtTruckDate = gr.FindControl("txtTruckDate") as TextBox;

            //            BQ.DC_BQ objBQ = new BQ.DC_BQ();
            //            objBQ.KSToken = _KSToken;
            //            objBQ.DataFrom = _KSDataFrom;
            //            objBQ.PrebooksId = Int32.Parse(_txtprebook.Text);
            //            objBQ.ProductId = Int32.Parse(_txtprebookItemId.Text); //Prebook Item Id
            //            objBQ.poItemId = Int32.Parse(_txtpoItemId.Text); //PO Item Id

            //            objBQ.SearchSrting = _txtProductDescription.Text; //As Product Description
            //            objBQ.ProcessDay = _txtTruckDate.Text;

            //            //ImportKSPrebooks objIP = new ImportKSPrebooks();
            //            //DataSet dsPrebook = objIP.getPrebookSummary(_txtTruckDate.Text, objBQ);
            //            //objBQ.InvoiceNumber = dsPrebook.Tables["prebooks"].Rows[0]["id"].ToString();

            //            DeleteKSPrebooks objK = new BQ.DeleteKSPrebooks();
            //            DataSet dsPO = objK.DeleteKSPOById(objBQ);
            //            DataSet ds = objK.DeleteKSPrebooksById(objBQ);

            //            if (ds.Tables[0].Rows[0][1].ToString() == "1")
            //            {
            //                string DeleteOrMove = "Delete: Delete Success";
            //                string IsSuccess = "1";
            //                string NewPrebookId = "";
            //                string NewTruckDate = "";
            //                //Update Database
            //                objUpdate = new BQ.UpdateKSPrebook();
            //                objUpdate.UpdatePrebookDeleteStatus(objBQ, DeleteOrMove, IsSuccess, NewPrebookId, NewTruckDate);
            //            }
            //            else {
            //                string DeleteOrMove = "Delete: fail";
            //                string IsSuccess = "0";
            //                string NewPrebookId = "";
            //                string NewTruckDate = "";
            //                //Update Database
            //                objUpdate = new BQ.UpdateKSPrebook();
            //                objUpdate.UpdatePrebookDeleteStatus(objBQ, DeleteOrMove, IsSuccess, NewPrebookId, NewTruckDate);
            //            }

            //            CreateErrorLog("CustomLogs/LogPrebookDeleteItems", sLogFormat + " Number: " + _txtNumber.Text + " - Product: " + _txtProductDescription.Text + "  - Data From: " + objBQ.DataFrom + "  - API Message: " + ds.Tables[0].Rows[0][0].ToString());
            //        }
            //    }
            //}
            //catch (Exception exp)
            //{
            //    CreateErrorLog("CustomLogs/ErrorLogPrebookDelete", exp.ToString());
            //}
            //finally
            //{
            //    ReadKSData objK = new BQ.ReadKSData();
            //    DataTable dt = objK.ReadDeleteMoveLog();
            //    GenerateExcelAndDownload(dt);
            //}
        }
        protected void btnMove_Click(object sender, EventArgs e)
        {
            string _KSToken = "";
            string _KSDataFrom = "";
            if (Session["CompanyID"] == null)
            {
                return;
            }
            string companyID = Session["CompanyID"].ToString();
            if (companyID == "1")
            {
                _KSToken = BQ.DB_Base.KSRFIDomesticToken; _KSDataFrom = BQ.DB_Base.KSRFIDomesticDataFrom;
            }
            else if (companyID == "2")
            {
                _KSToken = BQ.DB_Base.KSRFIInternationalToken; _KSDataFrom = BQ.DB_Base.KSRFIInternationalDataFrom;
            }
            else if (companyID == "3")
            {
                _KSToken = BQ.DB_Base.KSGmbHToken; _KSDataFrom = BQ.DB_Base.KSGmbHDataFrom;

            }
            else
            {
                _KSToken = BQ.DB_Base.KSDemoToken; _KSDataFrom = BQ.DB_Base.KSDemoDataFrom;
            }
            //_KSToken = BQ.DB_Base.KSDemoToken; _KSDataFrom = BQ.DB_Base.KSDemoDataFrom;
            string sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
            UpdateKSPrebook objUpdate = new BQ.UpdateKSPrebook();
            objUpdate.UpdatePrebookDeleteStatusAll();
            try
            {
                foreach (GridViewRow gr in GridView1.Rows)
                {
                    try
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
                            objBQ.KSToken = _KSToken;
                            objBQ.DataFrom = _KSDataFrom;
                            objBQ.PrebooksId = Int32.Parse(_txtprebook.Text);
                            objBQ.ProductId = Int32.Parse(_txtprebookItemId.Text); //Prebook Item Id
                            objBQ.poItemId = Int32.Parse(_txtpoItemId.Text); //PO Item Id

                            BQ.Prebooks objPB = new BQ.Prebooks();

                            objBQ.SearchSrting = _txtProductDescription.Text; //As Product Description

                            DateTime TruckDate = GetCorrespondingWeekDay(Convert.ToDateTime(txtDateMoveStart.Text), Convert.ToDateTime(txtDateMoveEnd.Text), Convert.ToDateTime(_txtTruckDate.Text));
                            objBQ.ProcessDay = TruckDate.ToString("yyyy-MM-dd");

                            //return;

                            ImportKSPrebooks objIP = new ImportKSPrebooks();
                            DataSet dsPrebook = objIP.getPrebookSummary(_txtTruckDate.Text, objBQ);
                            if (dsPrebook.Tables.Contains("prebooks"))
                            {
                                objBQ.InvoiceNumber = dsPrebook.Tables["prebooks"].Rows[0]["id"].ToString();
                                objPB.customerId = dsPrebook.Tables["prebooks"].Rows[0]["customerId"].ToString();

                                if (dsPrebook.Tables["prebooks"].Columns.Contains("customerPoNumber"))
                                {
                                    objPB.customerPoNumber = dsPrebook.Tables["prebooks"].Rows[0]["customerPoNumber"].ToString();
                                }
                                if (dsPrebook.Tables["prebooks"].Columns.Contains("comments"))
                                {
                                    objPB.comments = dsPrebook.Tables["prebooks"].Rows[0]["comments"].ToString();
                                }
                                if (dsPrebook.Tables["prebooks"].Columns.Contains("shipName"))
                                {
                                    string _shipName = dsPrebook.Tables["prebooks"].Rows[0]["shipName"].ToString();
                                    if (_shipName != "")
                                    {
                                        DataSet dsShipTo = objIP.getCustomerShipTo(objBQ, objPB);
                                        string _shipToId = dsShipTo.Tables[1].Select("name='" + _shipName + "'").CopyToDataTable().Rows[0]["id"].ToString(); ;
                                        objPB.shipToId = _shipToId;
                                    }
                                }
                                if (dsPrebook.Tables.Contains("details"))
                                {
                                    try
                                    {
                                        DataTable dtPBD = dsPrebook.Tables["details"].Select("prebookItemId=" + _txtprebookItemId.Text + "").CopyToDataTable();
                                        string _unitPrice = dtPBD.Rows[0]["unitPrice"].ToString(); ;
                                        objPB.unitPrice = _unitPrice;
                                        objPB.pbQty = 0;
                                        if (dsPrebook.Tables.Contains("breakdowns"))
                                        {
                                            try
                                            {
                                                DataTable dtPBBreakDown = dsPrebook.Tables["breakdowns"].Select("details_Id=" + dtPBD.Rows[0]["details_Id"].ToString() + "").CopyToDataTable();
                                                objIP.SaveBreakDown(dtPBBreakDown, objBQ);
                                            }
                                            catch
                                            {

                                            }
                                        }
                                        if (dsPrebook.Tables["details"].Columns.Contains("markCode"))
                                        {
                                            objPB.markCode = dsPrebook.Tables["details"].Rows[0]["markCode"].ToString();
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }


                                DeleteKSPrebooks objK = new BQ.DeleteKSPrebooks();
                                DataSet dsPO = objK.DeleteKSPOById(objBQ);
                                DataSet ds = objK.DeleteKSPrebooksById(objBQ);

                                if (ds.Tables[0].Rows[0][1].ToString() == "1")
                                {
                                    //Create Prebook
                                    CreateKSPrebooks objCreate = new BQ.CreateKSPrebooks();
                                    //DataSet dsCreate = objCreate.CreateKSPrebooksById(objBQ, objPB);
                                    DataSet dsCreate = objCreate.CreateKSPrebooksByIdVA(objBQ, objPB);
                                    
                                    if (dsCreate.Tables[0].Rows[0]["status"].ToString() == "1")
                                    {
                                        string NewPrebookId = dsCreate.Tables[0].Rows[0]["prebookId"].ToString();
                                        DataSet dsCreatePBAddProd = objCreate.CreatePrebooksAddProductVA(objBQ, objPB, NewPrebookId);
                                        if (dsCreatePBAddProd != null)
                                        {
                                            if (dsCreatePBAddProd.Tables[0].Rows[0]["status"].ToString() == "1")
                                            {
                                                string DeleteOrMove = "Delete and Move: Success";
                                                string IsSuccess = "1";
                                                string NewPrebookNo = dsCreate.Tables[0].Rows[0]["prebookNumber"].ToString();
                                                string NewTruckDate = objBQ.ProcessDay;
                                                //Update Database
                                                objUpdate = new BQ.UpdateKSPrebook();
                                                objUpdate.UpdatePrebookDeleteStatus(objBQ, DeleteOrMove, IsSuccess, NewPrebookNo, NewTruckDate);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string DeleteOrMove = "Delete and Move: Delete Success, Move fail";
                                        string IsSuccess = "0";
                                        string NewPrebookId = "";
                                        string NewTruckDate = "";
                                        //Update Database
                                        objUpdate = new BQ.UpdateKSPrebook();
                                        objUpdate.UpdatePrebookDeleteStatus(objBQ, DeleteOrMove, IsSuccess, NewPrebookId, NewTruckDate);
                                    }
                                }
                                else
                                {
                                    string DeleteOrMove = "Delete and Move: All fail";
                                    string IsSuccess = "0";
                                    string NewPrebookId = "";
                                    string NewTruckDate = "";
                                    //Update Database
                                    objUpdate = new BQ.UpdateKSPrebook();
                                    objUpdate.UpdatePrebookDeleteStatus(objBQ, DeleteOrMove, IsSuccess, NewPrebookId, NewTruckDate);
                                }
                                CreateErrorLog("CustomLogs/LogPrebookDeleteItems", sLogFormat + " Number: " + _txtNumber.Text + " - Product: " + _txtProductDescription.Text + "  - Data From: " + objBQ.DataFrom + "  - API Message: " + ds.Tables[0].Rows[0][0].ToString());
                            }
                            else
                            {
                                string DeleteOrMove = "Delete and Move: Getting old Prebook data fail";
                                string IsSuccess = "0";
                                string NewPrebookId = "";
                                string NewTruckDate = "";
                                //Update Database
                                objUpdate = new BQ.UpdateKSPrebook();
                                objUpdate.UpdatePrebookDeleteStatus(objBQ, DeleteOrMove, IsSuccess, NewPrebookId, NewTruckDate);
                            }

                        }
                    }
                    catch (Exception exp)
                    {
                        CreateErrorLog("CustomLogs/LogPrebookMoveError", sLogFormat + exp.ToString());
                    }
                }
                ReadKSData objLog = new BQ.ReadKSData();
                DataTable dt = objLog.ReadDeleteMoveLog();
                GenerateExcelAndDownload(dt);
            }
            catch (Exception exp)
            {
                CreateErrorLog("CustomLogs/ErrorLogPrebookDelete", exp.ToString());
            }
            finally
            {

            }
        }
        private void DeletePOPB()
        {
            string _KSToken = "";
            string _KSDataFrom = "";
            string pbQty = "0";
            string Percent = txtPercent.Value;
            if (Percent == "") { Percent = "0"; }
            if (Session["CompanyID"] == null)
            {
                return;
            }
            string companyID = Session["CompanyID"].ToString();
            if (companyID == "1")
            {
                _KSToken = BQ.DB_Base.KSRFIDomesticToken; _KSDataFrom = BQ.DB_Base.KSRFIDomesticDataFrom;
            }
            else if (companyID == "2")
            {
                _KSToken = BQ.DB_Base.KSRFIInternationalToken; _KSDataFrom = BQ.DB_Base.KSRFIInternationalDataFrom;
            }
            else if (companyID == "3")
            {
                _KSToken = BQ.DB_Base.KSGmbHToken; _KSDataFrom = BQ.DB_Base.KSGmbHDataFrom;

            }
            else
            {
                _KSToken = BQ.DB_Base.KSDemoToken; _KSDataFrom = BQ.DB_Base.KSDemoDataFrom;
            }
            //_KSToken = BQ.DB_Base.KSDemoToken; _KSDataFrom = BQ.DB_Base.KSDemoDataFrom;
            string sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
            UpdateKSPrebook objUpdate = new BQ.UpdateKSPrebook();
            objUpdate.UpdatePrebookDeleteStatusAll();
            try
            {
                foreach (GridViewRow gr in GridView1.Rows)
                {
                    try
                    {
                        CheckBox _chkRow = gr.FindControl("chkRow") as CheckBox;
                        if (_chkRow.Checked == true)
                        {
                            pbQty = "0";
                            TextBox _txtRefNo = gr.FindControl("txtKeyField") as TextBox; //PO ID
                            TextBox _txtprebook = gr.FindControl("txtprebook") as TextBox; //Prebook ID
                            TextBox _txtprebookItemId = gr.FindControl("txtprebookItemId") as TextBox; //Prebook Item Id
                            TextBox _txtpoItemId = gr.FindControl("txtpoItemId") as TextBox; //PO Item Id
                            TextBox _txtNumber = gr.FindControl("txtNumber") as TextBox; //PO Number
                            TextBox _txtProductDescription = gr.FindControl("txtProductDescription") as TextBox;
                            TextBox _txtTruckDate = gr.FindControl("txtTruckDate") as TextBox;

                            BQ.DC_BQ objBQ = new BQ.DC_BQ();
                            objBQ.KSToken = _KSToken;
                            objBQ.DataFrom = _KSDataFrom;
                            objBQ.PrebooksId = Int32.Parse(_txtprebook.Text);
                            objBQ.ProductId = Int32.Parse(_txtprebookItemId.Text); //Prebook Item Id
                            objBQ.poItemId = Int32.Parse(_txtpoItemId.Text); //PO Item Id

                            BQ.Prebooks objPB = new BQ.Prebooks();

                            objBQ.SearchSrting = _txtProductDescription.Text; //As Product Description
                            
                            // get date from previous prebook date
                            objBQ.ProcessDay = _txtTruckDate.Text;

                            // To get date from a given date.
                            //DateTime TruckDate = GetCorrespondingWeekDay(Convert.ToDateTime(txtDateMoveStart.Text), Convert.ToDateTime(txtDateMoveEnd.Text), Convert.ToDateTime(_txtTruckDate.Text));
                            //objBQ.ProcessDay = TruckDate.ToString("yyyy-MM-dd");

                            //return;

                            ImportKSPrebooks objIP = new ImportKSPrebooks();
                            DataSet dsPrebook = objIP.getPrebookSummary(_txtTruckDate.Text, objBQ);
                            if (dsPrebook.Tables.Contains("prebooks"))
                            {
                                objBQ.InvoiceNumber = dsPrebook.Tables["prebooks"].Rows[0]["id"].ToString();
                                objPB.customerId = dsPrebook.Tables["prebooks"].Rows[0]["customerId"].ToString();

                                if (dsPrebook.Tables["prebooks"].Columns.Contains("customerPoNumber"))
                                {
                                    objPB.customerPoNumber = dsPrebook.Tables["prebooks"].Rows[0]["customerPoNumber"].ToString();
                                }
                                if (dsPrebook.Tables["prebooks"].Columns.Contains("comments"))
                                {
                                    objPB.comments = dsPrebook.Tables["prebooks"].Rows[0]["comments"].ToString();
                                }
                                if (dsPrebook.Tables["prebooks"].Columns.Contains("shipName"))
                                {
                                    string _shipName = dsPrebook.Tables["prebooks"].Rows[0]["shipName"].ToString();
                                    if (_shipName != "")
                                    {
                                        DataSet dsShipTo = objIP.getCustomerShipTo(objBQ, objPB);
                                        string _shipToId = dsShipTo.Tables[1].Select("name='" + _shipName + "'").CopyToDataTable().Rows[0]["id"].ToString(); ;
                                        objPB.shipToId = _shipToId;
                                    }
                                }
                                if (dsPrebook.Tables.Contains("details"))
                                {
                                    try
                                    {
                                        DataTable dtPBD = dsPrebook.Tables["details"].Select("prebookItemId=" + _txtprebookItemId.Text + "").CopyToDataTable();
                                        string _unitPrice = dtPBD.Rows[0]["unitPrice"].ToString();

                                        objPB.unitPrice = _unitPrice;
                                        pbQty = dtPBD.Rows[0]["totalBoxes"].ToString();
                                        pbQty =(Int32.Parse(pbQty) * Int32.Parse(Percent) / 100).ToString();
                                        objPB.pbQty = Int32.Parse(pbQty);
                                        
                                        if (dsPrebook.Tables.Contains("breakdowns"))
                                        {
                                            try
                                            {
                                                DataTable dtPBBreakDown = dsPrebook.Tables["breakdowns"].Select("details_Id=" + dtPBD.Rows[0]["details_Id"].ToString() + "").CopyToDataTable();
                                                objIP.SaveBreakDown(dtPBBreakDown, objBQ);
                                            }
                                            catch
                                            {

                                            }
                                        }
                                        if (dsPrebook.Tables["details"].Columns.Contains("markCode"))
                                        {
                                            objPB.markCode = dsPrebook.Tables["details"].Rows[0]["markCode"].ToString();
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }


                                DeleteKSPrebooks objK = new BQ.DeleteKSPrebooks();
                                DataSet dsPO = objK.DeleteKSPOById(objBQ);
                                DataSet ds = objK.DeleteKSPrebooksById(objBQ);

                                if (ds.Tables[0].Rows[0][1].ToString() == "1")
                                {
                                    //Create Prebook
                                    CreateKSPrebooks objCreate = new BQ.CreateKSPrebooks();
                                    DataSet dsCreate = objCreate.CreateKSPrebooksById(objBQ, objPB);
                                    if (dsCreate.Tables[0].Rows[0]["status"].ToString() == "1")
                                    {
                                        string DeleteOrMove = "Delete and Create: Success";
                                        string IsSuccess = "1";
                                        string NewPrebookId = dsCreate.Tables[0].Rows[0]["prebookNumber"].ToString();
                                        string NewTruckDate = objBQ.ProcessDay;
                                        //Update Database
                                        objUpdate = new BQ.UpdateKSPrebook();
                                        objUpdate.UpdatePrebookDeleteStatus(objBQ, DeleteOrMove, IsSuccess, NewPrebookId, NewTruckDate);
                                    }
                                    else
                                    {
                                        string DeleteOrMove = "Delete and Create: Delete Success, Create Failed";
                                        string IsSuccess = "0";
                                        string NewPrebookId = "";
                                        string NewTruckDate = "";
                                        //Update Database
                                        objUpdate = new BQ.UpdateKSPrebook();
                                        objUpdate.UpdatePrebookDeleteStatus(objBQ, DeleteOrMove, IsSuccess, NewPrebookId, NewTruckDate);
                                    }
                                }
                                else
                                {
                                    string DeleteOrMove = "Delete and Create: All fail";
                                    string IsSuccess = "0";
                                    string NewPrebookId = "";
                                    string NewTruckDate = "";
                                    //Update Database
                                    objUpdate = new BQ.UpdateKSPrebook();
                                    objUpdate.UpdatePrebookDeleteStatus(objBQ, DeleteOrMove, IsSuccess, NewPrebookId, NewTruckDate);
                                }
                                CreateErrorLog("CustomLogs/LogPrebookDeleteItems", sLogFormat + " Number: " + _txtNumber.Text + " - Product: " + _txtProductDescription.Text + "  - Data From: " + objBQ.DataFrom + "  - API Message: " + ds.Tables[0].Rows[0][0].ToString());
                            }
                            else
                            {
                                string DeleteOrMove = "Delete and Create: Getting old Prebook data fail";
                                string IsSuccess = "0";
                                string NewPrebookId = "";
                                string NewTruckDate = "";
                                //Update Database
                                objUpdate = new BQ.UpdateKSPrebook();
                                objUpdate.UpdatePrebookDeleteStatus(objBQ, DeleteOrMove, IsSuccess, NewPrebookId, NewTruckDate);
                            }

                        }
                    }
                    catch (Exception exp)
                    {
                        CreateErrorLog("CustomLogs/LogPrebookDeleteError", sLogFormat + exp.ToString());
                    }
                }
                
            }
            catch (Exception exp)
            {
                CreateErrorLog("CustomLogs/LogPrebookDeleteError", exp.ToString());
            }
            finally
            {

            }
        }
        private void GenerateExcelAndDownload(DataTable dt)
        {
            string attachment = "attachment; filename=DeleteMoveLog.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
            Response.Flush();
        }
        protected void CreateErrorLog(string _localLogPath, string _message)
        {
            BQ.CreateLogFiles Err = new BQ.CreateLogFiles();
            Err.ErrorLog((path + _localLogPath), _message);
        }

        private DateTime GetCorrespondingWeekDay(DateTime startDate, DateTime endDate, DateTime DateToCompare)
        {
            TimeSpan diff = endDate - startDate;
            int days = diff.Days;
            DateTime CorrespondingWeekDay = startDate;
            for (var i = 0; i <= days; i++)
            {
                var testDate = startDate.AddDays(i);
                var WeekDay = DateToCompare.DayOfWeek;
                if (testDate.DayOfWeek == DateToCompare.DayOfWeek)
                {
                    CorrespondingWeekDay = testDate;
                    break;
                }
                //switch (testDate.DayOfWeek)
                //{
                //    //case DateToCompare.DayOfWeek:
                //    //case DayOfWeek.Saturday:
                //    //case DayOfWeek.Sunday:
                //    //    Console.WriteLine(testDate.ToShortDateString());
                //    //    break;
                //}
            }
            return CorrespondingWeekDay;
        }

        protected void ProcessVendorAvailabilityData(string _KSToken, string _DataFrom)
        {
            BQ.DC_BQ objBQ = new BQ.DC_BQ();
            objBQ.KSToken = _KSToken; // "nprijva0sksc2iugl9f5mm4722"; //RFI-Domestic
            objBQ.DataFrom = _DataFrom;
            objBQ.CallFrom = BQ.DB_Base.Call_From_Credit_60_1;
            string conString = BQ.DB_Base.DB_STR;
            string FromDate = DateTime.Now.AddDays(-7).ToShortDateString().ToString();
            string ToDate = DateTime.Now.AddDays(52).ToShortDateString().ToString();
            string strSQL = "";
            strSQL = @"
            select replace(convert(varchar(10),min(date1),111),'/','-') truckdate2, 
            replace(convert(varchar(10),max(date1),111),'/','-') truckdate3 from (
        	select [date] date1, replace(convert(varchar(10),date,111),'/','-') truckdate, replace(convert(varchar(10),date," + BQ.DB_Base.BQDataRegion + @"),'/','-') truckdate2 
            from [dbo].[DateBackbone](convert(datetime,'" + FromDate + @"'," + BQ.DB_Base.BQDataRegion + @"), 
                convert(datetime,'" + ToDate + @"'," + BQ.DB_Base.BQDataRegion + "))) A;";

            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            DataTable dtdate = new DataTable();
            adpt.Fill(dtdate);
            con.Close();
            con.Dispose();
            string fromDate = "";
            string toDate = "";
            if (dtdate.Rows.Count > 0)
            {
                for (int i = 0; i < dtdate.Rows.Count; i++)
                {
                    fromDate = dtdate.Rows[i]["truckdate2"].ToString();
                    toDate = dtdate.Rows[i]["truckdate3"].ToString();
                    objBQ.FromDate = fromDate;
                    objBQ.ToDate = toDate;
                    objBQ.ProcessDay = fromDate;
                    objBQ.InvoiceNumber = "";

                    BQ.ImportKSVendorAvailability objK = new BQ.ImportKSVendorAvailability();
                    objK.DeleteTempData(objBQ);
                    DataSet ds = objK.ImportVendorAvailabilityReturn(objBQ);
                    //string validationfile = ExportToExcel(ds.Tables[1], "inventoryItems");

                    //fromDate = DateTime.Parse(toDate).AddDays(1).ToShortDateString().ToString();
                    //fromDate = DateTime.Parse(fromDate).ToString("yyyy-MM-dd");
                    //toDate = DateTime.Parse(toDate).AddDays(10).ToShortDateString().ToString();
                    //toDate = DateTime.Parse(toDate).ToString("yyyy-MM-dd");

                    //objBQ.FromDate = fromDate;
                    //objBQ.ToDate = toDate;
                    //objBQ.ProcessDay = fromDate;
                    //objBQ.InvoiceNumber = "";

                    //objK.ImportVendorAvailability(objBQ);




                }
            }

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

    static void Main(string[] args)
       {
             DateTime startDate=new DateTime(2011,3,1);
             DateTime endDate = DateTime.Now;
 
             TimeSpan diff = endDate - startDate;
             int days = diff.Days;
             for (var i = 0; i <= days; i++)
             {
                 var testDate = startDate.AddDays(i);
                 switch (testDate.DayOfWeek)
                 {
                     case DayOfWeek.Saturday:
                     case DayOfWeek.Sunday:
                         Console.WriteLine(testDate.ToShortDateString());
                         break;
                 }
             }
           Console.ReadLine();
       }

    
                            

                            DateTime[] dates = { new DateTime(2008, 10, 6), new DateTime(2008, 10, 7) }; //etc....
                            var mondays = dates.Where(d => d.DayOfWeek == DayOfWeek.Monday); // = {10/6/2008}
    */
