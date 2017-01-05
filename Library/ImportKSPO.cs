using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace BQ
{
    public class ImportKSPO
    {
        string conString = DB_Base.DB_STR;
        string path = System.AppDomain.CurrentDomain.BaseDirectory;
        public DataSet ImportKSInvoideDataForAzure(DC_BQ objBQ)
        {
            string sLogFormat = "";
            DataSet ds = null;
            try
            {
                string FromDate = objBQ.FromDate;
                string ToDate = objBQ.ToDate;
                string strSQL = "";
                strSQL = @"
        	    select replace(convert(varchar(10),date,111),'/','-') truckdate,replace(convert(varchar(10),date," + DB_Base.BQDataRegion + @"),'/','-') 
                truckdate2 from [dbo].[DateBackbone](convert(datetime,'" + FromDate + @"'," + DB_Base.BQDataRegion + @"), convert(datetime,'" + ToDate + @"'," + DB_Base.BQDataRegion + @"));";

                SqlConnection con = new SqlConnection(conString);
                con.Open();
                SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
                DataTable dtdate = new DataTable();
                adpt.Fill(dtdate);
                con.Close();
                con.Dispose();
                string TruckDate = "", truckdate2 = "";
                if (dtdate.Rows.Count > 0)
                {
                    for (int i = 0; i < dtdate.Rows.Count; i++)
                    {
                        TruckDate = dtdate.Rows[i]["truckdate"].ToString();
                        truckdate2 = dtdate.Rows[i]["truckdate2"].ToString();
                        DeleteTempData();
                        
                        ds = getPOList(TruckDate, objBQ);

                        if (ds.Tables[0].Rows[0][0].ToString() != "Orders not found.OK")
                        {
                            if (ds.Tables.Contains("PurchaseOrders"))
                            {
                                SaveInvoiceInTempTable(ds, truckdate2, objBQ);
                                SendmailIfDuplicate();
                            }
                        }
                    }

                }
                //return ds;
            }
            catch (Exception exp)
            {
                sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
                CreateErrorLog("CustomLogs/ErrorLogPO", sLogFormat + " : " + objBQ.FromDate + " : " + objBQ.DataFrom + " : " + objBQ.CallFrom);
                CreateErrorLog("CustomLogs/ErrorLogPO", exp.ToString());

                string strExceptionInEmail = sLogFormat + " : " + objBQ.FromDate + " : " + objBQ.DataFrom + " : " + objBQ.CallFrom + "<br><br>";
                strExceptionInEmail += "Exception Details: <br>";
                strExceptionInEmail += exp.ToString();
                SendmailIfException(strExceptionInEmail);
            }
            finally
            {
                //ds.Dispose();
            }
            return ds;
        }
        public void SendmailIfException(string _message)
        {
            System.Net.Mail.MailMessage Mail = new System.Net.Mail.MailMessage("blumensoft@royalcorp.net", "faruka@aphix.ca");
            Mail.Sender = new System.Net.Mail.MailAddress("blumensoft@royalcorp.net");
            Mail.From = new System.Net.Mail.MailAddress("blumensoft@royalcorp.net");
            Mail.To.Add("efrenc@aphix.ca");
            Mail.Bcc.Add("faaruk.ahmed@gmail.com");

            Mail.Subject = "Exception in Data warehouse-PO";
            Mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com");
            client.Credentials = new System.Net.NetworkCredential("blumensoft@royalcorp.net", "Password!8");
            client.EnableSsl = true;
            client.Port = 587;

            Mail.Body = "<html><body><br>" + "<center><b> </b></center> <br> " + _message + "</body></html>";
            client.Send(Mail);
            Mail.Dispose();
        }
        public void ImportKSInvoideDataForAzureForToday(DC_BQ objBQ)
        {
            string sLogFormat = "";
            DataSet ds = null;
            try
            {
                string FromDate = objBQ.FromDate;
                string ToDate = objBQ.ToDate;
                string strSQL = "";
                strSQL = @"
        	    select replace(convert(varchar(10),date,111),'/','-') truckdate,replace(convert(varchar(10),date," + DB_Base.BQDataRegion + @"),'/','-') 
                truckdate2 from [dbo].[DateBackbone](convert(datetime,'" + FromDate + @"'," + DB_Base.BQDataRegion + @"), convert(datetime,'" + ToDate + @"'," + DB_Base.BQDataRegion + @"));";

                SqlConnection con = new SqlConnection(conString);
                con.Open();
                SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
                DataTable dtdate = new DataTable();
                adpt.Fill(dtdate);
                con.Close();
                con.Dispose();
                string TruckDate = "", truckdate2 = "";
                if (dtdate.Rows.Count > 0)
                {
                    for (int i = 0; i < dtdate.Rows.Count; i++)
                    {
                        TruckDate = dtdate.Rows[i]["truckdate"].ToString();
                        truckdate2 = dtdate.Rows[i]["truckdate2"].ToString();
                        DeleteTempData();
                        DeleteDataByDate(truckdate2, objBQ);
                        ds = getPOListAllStatus (TruckDate, objBQ);
                        if (ds.Tables[0].Rows[0][0].ToString() != "Orders not found.OK")
                        {
                            if (ds.Tables.Contains("PurchaseOrders"))
                            {
                                SaveInvoiceInTempTable(ds, truckdate2, objBQ);
                                SendmailIfDuplicate();
                            }
                        }
                    }

                }
                //return ds;
            }
            catch (Exception exp)
            {
                sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
                CreateErrorLog("CustomLogs/ErrorLogPO", sLogFormat + " : " + objBQ.FromDate + " : " + objBQ.DataFrom + " : " + objBQ.CallFrom);
                CreateErrorLog("CustomLogs/ErrorLogPO", exp.ToString());
                string strExceptionInEmail = sLogFormat + " : " + objBQ.FromDate + " : " + objBQ.DataFrom + " : " + objBQ.CallFrom + "<br><br>";
                strExceptionInEmail += "Exception Details: <br>";
                strExceptionInEmail += exp.ToString();
                SendmailIfException(strExceptionInEmail);
            }
            finally
            {
                ds.Dispose();
            }
        }
        private DataSet getPOList(string Truckdate, DC_BQ objBQ)
        {
            DataSet ds = null;
            //RFI - Domestic: nprijva0sksc2iugl9f5mm4722
            string KSToken = objBQ.KSToken;
            Uri uri = new Uri("https://api.kometsales.com/api/purchase.order.list?authenticationToken=" + KSToken + "&date=" + Truckdate + "");
            string data = "";
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                try
                {
                    string result = "";

                    // Create the web request  
                    //HttpWebRequest request = WebRequest.Create("https://api.kometsales.com/api/purchase.order.list?authenticationToken=" + KSToken + "&date=" + Truckdate + "&status=Approved") as HttpWebRequest;
                    HttpWebRequest request = WebRequest.Create("https://api.kometsales.com/api/purchase.order.list?authenticationToken=" + KSToken + "&date=" + Truckdate + "") as HttpWebRequest;
                    // Get response  
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        // Get the response stream  
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        // Read the whole contents and return as a string  
                        result = reader.ReadToEnd();
                        ds = DerializeDataTable(result);
                    }
                }
                catch (Exception exp)
                { }
                finally
                { }

            }
            return ds;
        }
        private DataSet getPOListAllStatus(string Truckdate, DC_BQ objBQ)
        {
            DataSet ds = null;
            //RFI - Domestic: nprijva0sksc2iugl9f5mm4722
            string KSToken = objBQ.KSToken;
            Uri uri = new Uri("https://api.kometsales.com/api/purchase.order.list?authenticationToken=" + KSToken + "&date=" + Truckdate + "");
            string data = "";
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                try
                {
                    string result = "";

                    // Create the web request  
                    HttpWebRequest request = WebRequest.Create("https://api.kometsales.com/api/purchase.order.list?authenticationToken=" + KSToken + "&date=" + Truckdate + "") as HttpWebRequest;

                    // Get response  
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        // Get the response stream  
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        // Read the whole contents and return as a string  
                        result = reader.ReadToEnd();
                        ds = DerializeDataTable(result);
                    }
                }
                catch (Exception exp)
                { }
                finally
                { }

            }
            return ds;
        }
        protected void CreateLogTime(string _localLogPath, string _message)
        {
            CreateLogFiles Err = new CreateLogFiles();
            Err.ErrorLog((path + _localLogPath), _message);
        }
        private void SaveInvoiceInTempTable(DataSet _ds, string Truckdate, DC_BQ objBQ)
        {
            ReadKSData objRD = new ReadKSData();
            DataSet dsColName = objRD.GetPOColumnName();

            DataTable dataTable;
            string strSQL = "";
            SqlConnection connection = new SqlConnection(conString);
            if (_ds.Tables["PurchaseOrders"].Rows.Count > 0)
            {
                dataTable = dsColName.Tables[0];
                string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                                     .Select(x => x.ColumnName)
                                     .ToArray();
                //var listOfStrings = new List<string> { "Cars", "Trucks", "Boats" };
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    connection.Open();
                    bulkCopy.BatchSize = 500;
                    bulkCopy.BulkCopyTimeout = 0;
                    bulkCopy.DestinationTableName = "dbo.PB_PO_PurchaseOrders";
                    foreach (DataColumn column in _ds.Tables["PurchaseOrders"].Columns)
                    {
                        if (columnNames.Contains(column.ColumnName))
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                    }
                    bulkCopy.WriteToServer(_ds.Tables["PurchaseOrders"]);
                    connection.Close();
                }
            }
            if (_ds.Tables.Contains("details"))
            {
                DataTable dt = _ds.Tables["details"];
                if (dt.Rows.Count > 0)
                {
                    dataTable = dsColName.Tables[1];
                    string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                                         .Select(x => x.ColumnName)
                                         .ToArray();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        bulkCopy.BatchSize = 500;
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = "dbo.PB_PO_Details";
                        foreach (DataColumn column in _ds.Tables["details"].Columns)
                        {
                            if (columnNames.Contains(column.ColumnName))
                            {
                                bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                            }
                        }
                        bulkCopy.WriteToServer(_ds.Tables["details"]);
                        connection.Close();
                    }
                }
            }
            if (_ds.Tables.Contains("breakdowns"))
            {
                dataTable = dsColName.Tables[3];
                string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                                     .Select(x => x.ColumnName)
                                     .ToArray();
                if (_ds.Tables["breakdowns"].Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        bulkCopy.BatchSize = 500;
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = "dbo.PB_PO_BreakDowns";
                        foreach (DataColumn column in _ds.Tables["breakdowns"].Columns)
                        {
                            if (columnNames.Contains(column.ColumnName))
                            {
                                bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                            }
                        }
                        bulkCopy.WriteToServer(_ds.Tables["breakdowns"]);
                        connection.Close();
                    }
                }
            }
            if (_ds.Tables.Contains("boxes"))
            {
                dataTable = dsColName.Tables[2];
                string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                                     .Select(x => x.ColumnName)
                                     .ToArray();
                if (_ds.Tables["boxes"].Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        bulkCopy.BatchSize = 500;
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = "dbo.PB_PO_Boxes";
                        foreach (DataColumn column in _ds.Tables["boxes"].Columns)
                        {
                            if (columnNames.Contains(column.ColumnName))
                            {
                                bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                            }
                        }
                        bulkCopy.WriteToServer(_ds.Tables["boxes"]);
                        connection.Close();
                    }
                }
            }
            if (_ds.Tables.Contains("CustomFields"))
            {
                dataTable = dsColName.Tables[4];
                string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                                     .Select(x => x.ColumnName)
                                     .ToArray();

                if (_ds.Tables["CustomFields"].Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        bulkCopy.BatchSize = 500;
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = "dbo.PB_PO_CustomFields";
                        foreach (DataColumn column in _ds.Tables["CustomFields"].Columns)
                        {
                            if (columnNames.Contains(column.ColumnName))
                            {
                                bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                            }
                        }
                        bulkCopy.WriteToServer(_ds.Tables["CustomFields"]);
                        connection.Close();
                    }
                }
            }

            strSQL = @"
        insert into dbo.PB_PO_duplicate_invoices (PO_number, source, status, old_shipdate, new_shipdate) 
        select N.number, '" + objBQ.DataFrom + @"', 1, O.shipdate, N.shipdate 
        from dbo." + objBQ.DataFrom + @"_PB_PO_PurchaseOrders O inner join PB_PO_PurchaseOrders N on N.number=O.number
        where O.number in (
	        select distinct number from PB_PO_PurchaseOrders 
        );

        delete from dbo." + objBQ.DataFrom + @"_PB_PO_Breakdowns where PO_number in (
	        select distinct number from PB_PO_PurchaseOrders 
        );
        delete from dbo." + objBQ.DataFrom + @"_PB_PO_Boxes where PO_number in (
	        select distinct number from PB_PO_PurchaseOrders 
        );
        delete from dbo." + objBQ.DataFrom + @"_PB_PO_CustomFields where PO_number in (
	        select distinct number from PB_PO_PurchaseOrders 
        );
        delete from dbo." + objBQ.DataFrom + @"_PB_PO_Details where PO_number in (
	        select distinct number from PB_PO_PurchaseOrders 
        );
        delete from dbo." + objBQ.DataFrom + @"_PB_PO_PurchaseOrders where number in (
	        select distinct number from PB_PO_PurchaseOrders 
        );
        update PB_PO_Details set details_Id=0 where details_Id is null;
        update PB_PO_Boxes set details_Id=0 where details_Id is null;
        update PB_PO_BreakDowns set details_Id=0 where details_Id is null;
        update PB_PO_CustomFields set details_Id=0 where details_Id is null;


        update PB_PO_Details set 
	        id_PO = (select top 1 PB_PO_PurchaseOrders.id from PB_PO_PurchaseOrders 
	        where PB_PO_PurchaseOrders.purchaseOrders_Id=PB_PO_Details.purchaseOrders_Id 
	        and PB_PO_Details.id_PO is null 
	        and convert(datetime,shipDate," + DB_Base.BQDataRegion + @")= convert(datetime,'" + Truckdate + @"'," + DB_Base.BQDataRegion + @") 
	        )	where id_PO is null;	

        update PB_PO_Details set 
	        PO_number = (select top 1 PB_PO_PurchaseOrders.number from PB_PO_PurchaseOrders 
	        where PB_PO_PurchaseOrders.purchaseOrders_Id=PB_PO_Details.purchaseOrders_Id 
	        and PB_PO_Details.PO_number is null 
	        and convert(datetime,shipDate," + DB_Base.BQDataRegion + @")= convert(datetime,'" + Truckdate + @"'," + DB_Base.BQDataRegion + @") 
	        ) where PO_number is null;

       update PB_PO_Boxes set 
	        PO_number = (select top 1 PB_PO_Details.PO_number from PB_PO_Details 
	        inner join PB_PO_PurchaseOrders on PB_PO_PurchaseOrders.purchaseOrders_Id=PB_PO_Details.purchaseOrders_Id
	        and PB_PO_PurchaseOrders.number=PB_PO_Details.PO_number
            and PB_PO_PurchaseOrders.id=PB_PO_Details.id_PO
	        where PB_PO_Boxes.PO_number is null 
            and convert(datetime,shipDate," + DB_Base.BQDataRegion + @")= convert(datetime,'" + Truckdate + @"'," + DB_Base.BQDataRegion + @") 
	        and PB_PO_Details.details_Id=PB_PO_Boxes.details_Id 
	        ) where PO_number is null;	
    
        update PB_PO_BreakDowns set 
	        PO_number = (select top 1 PB_PO_Details.PO_number from PB_PO_Details 
	        inner join PB_PO_PurchaseOrders on PB_PO_PurchaseOrders.purchaseOrders_Id=PB_PO_Details.purchaseOrders_Id
	        and PB_PO_PurchaseOrders.number=PB_PO_Details.PO_number
            and PB_PO_PurchaseOrders.id=PB_PO_Details.id_PO
	        where PB_PO_BreakDowns.PO_number is null 
            and convert(datetime,shipDate," + DB_Base.BQDataRegion + @")= convert(datetime,'" + Truckdate + @"'," + DB_Base.BQDataRegion + @") 
	        and PB_PO_Details.details_Id=PB_PO_BreakDowns.details_Id 
	        ) where PO_number is null;	

        update PB_PO_CustomFields set 
	        PO_number = (select top 1 PB_PO_Details.PO_number from PB_PO_Details 
	        inner join PB_PO_PurchaseOrders on PB_PO_PurchaseOrders.purchaseOrders_Id=PB_PO_Details.purchaseOrders_Id
	        and PB_PO_PurchaseOrders.number=PB_PO_Details.PO_number
            and PB_PO_PurchaseOrders.id=PB_PO_Details.id_PO
	        where PB_PO_CustomFields.PO_number is null 
            and convert(datetime,shipDate," + DB_Base.BQDataRegion + @")= convert(datetime,'" + Truckdate + @"'," + DB_Base.BQDataRegion + @") 
	        and PB_PO_Details.details_Id=PB_PO_CustomFields.details_Id 
	        ) where PO_number is null;	
            
        insert into dbo." + objBQ.DataFrom + @"_PB_PO_PurchaseOrders select * from dbo.PB_PO_PurchaseOrders;           
        insert into dbo." + objBQ.DataFrom + @"_PB_PO_Details select * from dbo.PB_PO_Details;           
        insert into dbo." + objBQ.DataFrom + @"_PB_PO_CustomFields select * from dbo.PB_PO_CustomFields;           
        insert into dbo." + objBQ.DataFrom + @"_PB_PO_Boxes select * from dbo.PB_PO_Boxes;           
        insert into dbo." + objBQ.DataFrom + @"_PB_PO_Breakdowns select * from dbo.PB_PO_Breakdowns;  
            ";

            connection.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, connection);
            object objResult = null;

            SqlCommand dbCommand = adpt.SelectCommand;
            dbCommand.CommandText = strSQL;
            dbCommand.CommandTimeout = 0;
            dbCommand.CommandType = CommandType.Text;

            objResult = dbCommand.ExecuteScalar();
            connection.Close();

            string sLogFormat = "";
            sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
            CreateLogTime("CustomLogs/ExportLogPO", sLogFormat + " PO Ship Date: " + Truckdate + " : " + objBQ.DataFrom + " : " + objBQ.CallFrom);
        }
        private DataSet DerializeDataTable(string str)
        {
            XmlDocument xd = new XmlDocument();
            str = "{ \"rootNode\": {" + str.Trim().TrimStart('{').TrimEnd('}') + "} }";
            xd = (XmlDocument)JsonConvert.DeserializeXmlNode(str);
            DataSet ds = new DataSet();
            ds.ReadXml(new XmlNodeReader(xd));
            return ds;

        }
        private void DeleteDataByDate(string Truckdate, DC_BQ objBQ)
        {
            string strSQL = @"
                DELETE B FROM dbo." + objBQ.DataFrom + @"_PO_Breakdowns B
                INNER JOIN dbo." + objBQ.DataFrom + @"_PO_PurchaseOrders I ON B.PO_number=I.number
                and convert(datetime,shipDate," + DB_Base.BQDataRegion + @")= convert(datetime,'" + Truckdate + @"'," + DB_Base.BQDataRegion + @");
    
                DELETE B FROM dbo." + objBQ.DataFrom + @"_PO_Boxes B
                INNER JOIN dbo." + objBQ.DataFrom + @"_PO_PurchaseOrders I ON B.PO_number=I.number
                and convert(datetime,shipDate," + DB_Base.BQDataRegion + @")= convert(datetime,'" + Truckdate + @"'," + DB_Base.BQDataRegion + @");
                
                DELETE B FROM dbo." + objBQ.DataFrom + @"_PO_Details B
                INNER JOIN dbo." + objBQ.DataFrom + @"_PO_PurchaseOrders I ON B.PO_number=I.number
                and convert(datetime,shipDate," + DB_Base.BQDataRegion + @")= convert(datetime,'" + Truckdate + @"'," + DB_Base.BQDataRegion + @");

                DELETE B FROM dbo." + objBQ.DataFrom + @"_PO_CustomFields B
                INNER JOIN dbo." + objBQ.DataFrom + @"_PO_PurchaseOrders I ON B.PO_number=I.number
                and convert(datetime,shipDate," + DB_Base.BQDataRegion + @")= convert(datetime,'" + Truckdate + @"'," + DB_Base.BQDataRegion + @");

                DELETE FROM dbo." + objBQ.DataFrom + @"_PO_PurchaseOrders
                WHERE convert(datetime,shipDate," + DB_Base.BQDataRegion + @")= convert(datetime,'" + Truckdate + @"'," + DB_Base.BQDataRegion + @");
            ";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            object objResult = null;

            SqlCommand dbCommand = adpt.SelectCommand;
            dbCommand.CommandTimeout = 0;
            dbCommand.CommandText = strSQL;
            dbCommand.CommandType = CommandType.Text;

            objResult = dbCommand.ExecuteScalar();


        }
        public void DeleteAll(DC_BQ objBQ)
        {
            string strSQL = @"
                DELETE FROM dbo." + objBQ.DataFrom + @"_PB_PO_Breakdowns;
                DELETE FROM dbo." + objBQ.DataFrom + @"_PB_PO_Boxes;
                DELETE FROM dbo." + objBQ.DataFrom + @"_PB_PO_Details;
                DELETE FROM dbo." + objBQ.DataFrom + @"_PB_PO_CustomFields;
                DELETE FROM dbo." + objBQ.DataFrom + @"_PB_PO_PurchaseOrders;
            ";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            object objResult = null;

            SqlCommand dbCommand = adpt.SelectCommand;
            dbCommand.CommandTimeout = 0;
            dbCommand.CommandText = strSQL;
            dbCommand.CommandType = CommandType.Text;
            objResult = dbCommand.ExecuteScalar();
        }
        private void DeleteTempData()
        {
            string strSQL = @"
                delete from PB_PO_breakdowns;
                delete from [PB_PO_boxes];
                delete from [PB_PO_details];
                delete from PB_PO_CustomFields;
                delete from [PB_PO_PurchaseOrders];
                update PB_PO_duplicate_invoices set status=0 where status=1;
            ";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            object objResult = null;

            SqlCommand dbCommand = adpt.SelectCommand;
            dbCommand.CommandText = strSQL;
            dbCommand.CommandTimeout = 0;
            dbCommand.CommandType = CommandType.Text;

            objResult = dbCommand.ExecuteScalar();


        }
        protected void CreateErrorLog(string _localLogPath, string _message)
        {
            CreateLogFiles Err = new CreateLogFiles();
            Err.ErrorLog((path + _localLogPath), _message);
        }
        public void SendmailIfDuplicate()
        {

            ReadKSData objRD = new ReadKSData();
            DataTable dtDuplicates = objRD.GetNewDuplicates();

            if (dtDuplicates.Rows.Count == 0)
            {
                return;
            }
            try
            {
                String sBody;
                System.Net.Mail.MailMessage Mail = new System.Net.Mail.MailMessage("blumensoft@royalcorp.net", "faruka@aphix.ca");
                Mail.Sender = new System.Net.Mail.MailAddress("blumensoft@royalcorp.net");
                Mail.From = new System.Net.Mail.MailAddress("blumensoft@royalcorp.net");
                Mail.To.Add("efrenc@aphix.ca");
                Mail.Bcc.Add("faaruk.ahmed@gmail.com");

                Mail.Subject = "Duplicates PO orders";
                Mail.IsBodyHtml = true;
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                client.Credentials = new System.Net.NetworkCredential("blumensoft@royalcorp.net", "Password!8");
                client.EnableSsl = true;
                client.Port = 587;


                sBody = "PDF";

                string backcolor = "808080";

                string ItemData = "";
                if (dtDuplicates.Rows.Count > 0)
                {
                    ItemData = "<br><br>" + ExportTohtmlDetails(dtDuplicates, backcolor);
                }


                string message = "";
                message = "<html><body><br>" + "<center><b> </b></center> <br> ";
                Mail.Body = message + ItemData + "</body></html>";
                client.Send(Mail);
                Mail.Dispose();

            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
        private string ExportTohtmlDetails(DataTable dt, string backcolor)
        {
            string tab = "\t";

            StringBuilder sb = new StringBuilder();


            sb.AppendLine(tab + tab + "<table style=\"border-collapse:collapse; text-align:center;\">");

            // headers.
            sb.Append(tab + tab + tab + "<tr style =\"background-color:#" + backcolor + "; color:#ffffff;\">");

            foreach (DataColumn dc in dt.Columns)
            {
                sb.AppendFormat("<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">{0}</td>", dc.ColumnName);
            }

            sb.AppendLine("</tr>");

            // data rows
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(tab + tab + tab + "<tr>");

                foreach (DataColumn dc in dt.Columns)
                {
                    string cellValue = dr[dc] != null ? dr[dc].ToString() : "";
                    sb.AppendFormat("<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">{0}</td>", cellValue.Replace("/", "-"));
                }

                sb.AppendLine("</tr>");
            }

            sb.AppendLine(tab + tab + "</table>");
            return sb.ToString();
        }
    }
}
