using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using System.Data;
using System.Collections;

using System.Web;
using System.Configuration;
using System.Data.SqlTypes;
using System.Collections.ObjectModel;
using System.IO;

using System.Xml;
using Newtonsoft.Json;
using System.Net;


namespace BQ
{
    public class ImportKSData
    {
        string conString = DB_Base.DB_STR;
        string path = System.AppDomain.CurrentDomain.BaseDirectory;
        public void ImportInvoice(DC_BQ objBQ)
        {
            string sLogFormat = "";
            sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
            CreateErrorLog("CustomLogs/ErrorLog", sLogFormat + " : " + objBQ.FromDate + " : " + objBQ.DataFrom);
            try
            {
                DataSet ds = null;

                DeleteTempData();
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
                string TruckDate = "", truckdate2 = "";
                if (dtdate.Rows.Count > 0)
                {
                    for (int i = 0; i < dtdate.Rows.Count; i++)
                    {
                        TruckDate = dtdate.Rows[i]["truckdate"].ToString();
                        truckdate2 = dtdate.Rows[i]["truckdate2"].ToString();
                        ds = getInvList(TruckDate, objBQ);
                        if (ds.Tables[0].Rows[0][0].ToString() != "Orders not found.")
                        {
                            if (ds.Tables.Contains("invoices"))
                            {
                                SaveInTempTable(ds, truckdate2, objBQ);
                            }
                        }
                    }

                }
            }
            catch (Exception exp)
            {
                CreateErrorLog("CustomLogs/ErrorLog", exp.ToString());
            }
            finally
            {

            }
        }
        public void ImportInvoiceAllStatus(DC_BQ objBQ)
        {
            string sLogFormat = "";
            sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
            CreateErrorLog("CustomLogs/ErrorLog", sLogFormat + " : " + objBQ.FromDate + " : " + objBQ.DataFrom);
            try
            {
                DataSet ds = null;

                DeleteTempData();
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
                string TruckDate = "", truckdate2 = "";
                if (dtdate.Rows.Count > 0)
                {
                    for (int i = 0; i < dtdate.Rows.Count; i++)
                    {
                        TruckDate = dtdate.Rows[i]["truckdate"].ToString();
                        truckdate2 = dtdate.Rows[i]["truckdate2"].ToString();
                        ds = getInvListAllStatus(TruckDate, objBQ);
                        if (ds.Tables[0].Rows[0][0].ToString() != "Orders not found.")
                        {
                            if (ds.Tables.Contains("invoices"))
                            {
                                SaveInTempTable(ds, truckdate2, objBQ);
                            }
                        }
                    }

                }
            }
            catch (Exception exp)
            {
                CreateErrorLog("CustomLogs/ErrorLog", exp.ToString());
            }
            finally
            {

            }
        }
        public void ImportInvoiceByNumber(DC_BQ objBQ)
        {
            string sLogFormat = "";
            sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
            CreateErrorLog("CustomLogs/ErrorLog", sLogFormat + " : " + objBQ.InvoiceNumber + " : " + objBQ.DataFrom);
            try
            {
                DataSet ds = null;

                DeleteTempData();
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
                string TruckDate = "", truckdate2 = "";
                if (dtdate.Rows.Count > 0)
                {
                    for (int i = 0; i < dtdate.Rows.Count; i++)
                    {
                        TruckDate = dtdate.Rows[i]["truckdate"].ToString();
                        truckdate2 = dtdate.Rows[i]["truckdate2"].ToString();
                        ds = getInvListbyNumber(TruckDate, objBQ);
                        if (ds.Tables[0].Rows[0][0].ToString() != "Orders not found.")
                        {
                            if (ds.Tables.Contains("invoices"))
                            {
                                SaveInTempTablebyNumber(ds, truckdate2, objBQ.InvoiceNumber, objBQ);
                            }
                        }
                    }

                }
            }
            catch (Exception exp)
            {
                CreateErrorLog("CustomLogs/ErrorLog", exp.ToString());
            }
            finally
            {

            }
        }
        private DataSet getInvList(string Truckdate, DC_BQ objBQ)
        {
            DataSet ds = null;
            //RFI - Domestic: nprijva0sksc2iugl9f5mm4722
            string KSToken = objBQ.KSToken;
            Uri uri = new Uri("https://api.kometsales.com/api/invoice.details.list?authenticationToken=" + KSToken + "&orderDate=" + Truckdate + "&status=1");
            string data = "";
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                    request.Method = WebRequestMethods.Http.Post;
                    request.ContentLength = data.Length;
                    request.ContentType = "application/x-www-form-urlencoded";
                    StreamWriter writer = new StreamWriter(request.GetRequestStream());
                    writer.Write(data);
                    writer.Close();
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string strKsResponse = reader.ReadToEnd();
                    response.Close();
                    ds = DerializeDataTable(strKsResponse);
                }
                catch (Exception exp)
                { }
                finally
                { }

            }
            return ds;
        }
        private DataSet getInvListbyNumber(string Truckdate, DC_BQ objBQ)
        {
            DataSet ds = null;
            //RFI - Domestic: nprijva0sksc2iugl9f5mm4722
            string KSToken = objBQ.KSToken;
            Uri uri = new Uri("https://api.kometsales.com/api/invoice.details.list?authenticationToken=" + KSToken + "&orderDate=" + Truckdate + "&status=1&order=" + objBQ.InvoiceNumber + "");
            string data = "";
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                    request.Method = WebRequestMethods.Http.Post;
                    request.ContentLength = data.Length;
                    request.ContentType = "application/x-www-form-urlencoded";
                    StreamWriter writer = new StreamWriter(request.GetRequestStream());
                    writer.Write(data);
                    writer.Close();
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string strKsResponse = reader.ReadToEnd();
                    response.Close();
                    ds = DerializeDataTable(strKsResponse);
                }
                catch (Exception exp)
                { }
                finally
                { }

            }
            return ds;
        }

        private DataSet getInvListAllStatus(string Truckdate, DC_BQ objBQ)
        {
            DataSet ds = null;
            //RFI - Domestic: nprijva0sksc2iugl9f5mm4722
            string KSToken = objBQ.KSToken;
            Uri uri = new Uri("https://api.kometsales.com/api/invoice.details.list?authenticationToken=" + KSToken + "&orderDate=" + Truckdate + "");
            string data = "";
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                    request.Method = WebRequestMethods.Http.Post;
                    request.ContentLength = data.Length;
                    request.ContentType = "application/x-www-form-urlencoded";
                    StreamWriter writer = new StreamWriter(request.GetRequestStream());
                    writer.Write(data);
                    writer.Close();
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string strKsResponse = reader.ReadToEnd();
                    response.Close();
                    ds = DerializeDataTable(strKsResponse);
                }
                catch (Exception exp)
                { }
                finally
                { }

            }
            return ds;
        }

        private void SaveInTempTable(DataSet _ds, string Truckdate, DC_BQ objBQ)
        {
            string strSQL = "";
            SqlConnection connection = new SqlConnection(conString);
            if (_ds.Tables[1].Rows.Count > 0)
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    connection.Open();
                    bulkCopy.BatchSize = 500;
                    bulkCopy.DestinationTableName = "dbo.invoices";
                    foreach (DataColumn column in _ds.Tables["invoices"].Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    bulkCopy.WriteToServer(_ds.Tables[1]);
                    connection.Close();
                }
            }
            if (_ds.Tables.Contains("details"))
            {
                DataTable dt = _ds.Tables[2];
                if (dt.Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        bulkCopy.BatchSize = 500;
                        bulkCopy.DestinationTableName = "dbo.details";
                        foreach (DataColumn column in _ds.Tables["details"].Columns)
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                        bulkCopy.WriteToServer(dt);
                        connection.Close();
                    }
                }
            }
            if (_ds.Tables.Contains("breakdowns"))
            {
                if (_ds.Tables["breakdowns"].Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        bulkCopy.BatchSize = 500;
                        bulkCopy.DestinationTableName = "dbo.breakdowns";
                        foreach (DataColumn column in _ds.Tables["breakdowns"].Columns)
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                        bulkCopy.WriteToServer(_ds.Tables["breakdowns"]);
                        connection.Close();
                    }
                }
            }
            if (_ds.Tables.Contains("boxes"))
            {
                if (_ds.Tables["boxes"].Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        bulkCopy.BatchSize = 500;
                        bulkCopy.DestinationTableName = "dbo.boxes";
                        foreach (DataColumn column in _ds.Tables["boxes"].Columns)
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                        bulkCopy.WriteToServer(_ds.Tables["boxes"]);
                        connection.Close();
                    }
                }
            }
            if (_ds.Tables.Contains("additionalChargesDetails"))
            {
                if (_ds.Tables["additionalChargesDetails"].Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        bulkCopy.BatchSize = 500;
                        bulkCopy.DestinationTableName = "dbo.additionalChargesDetails";
                        foreach (DataColumn column in _ds.Tables["additionalChargesDetails"].Columns)
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                        bulkCopy.WriteToServer(_ds.Tables["additionalChargesDetails"]);
                        connection.Close();
                    }
                }
            }

            strSQL = @"
        update invoices set system_name='" + objBQ.DataFrom + @"';
        
        update details set 
	        ID_INVOICE = (select top 1 invoices.id from invoices 
	        where invoices.invoices_Id=details.invoices_Id 
	        and details.ID_INVOICE is null 
	        and convert(datetime,shipDate," + DB_Base.BQDataRegion + @")= convert(datetime,'" + Truckdate + @"'," + DB_Base.BQDataRegion + @") 
	        )	where ID_INVOICE is null;	

        update details set 
	        invoice_number = (select top 1 invoices.number from invoices 
	        where invoices.invoices_Id=details.invoices_Id 
	        and details.invoice_number is null 
	        and convert(datetime,shipDate," + DB_Base.BQDataRegion + @")= convert(datetime,'" + Truckdate + @"'," + DB_Base.BQDataRegion + @") 
	        ) where invoice_number is null;

        update additionalChargesDetails set 
	        invoice_number = (select top 1 invoices.number from invoices 
	        where invoices.invoices_Id=additionalChargesDetails.invoices_Id 
	        and additionalChargesDetails.invoice_number is null 
	        and convert(datetime,shipDate," + DB_Base.BQDataRegion + @")= convert(datetime,'" + Truckdate + @"'," + DB_Base.BQDataRegion + @") 
	        )	where invoice_number is null;		        
		
         update boxes set 
	            invoice_number = (select top 1 details.invoice_number from details 
	            inner join  invoices on invoices.invoices_Id=details.invoices_Id
	            and invoices.number=details.invoice_number
	            and invoices.id=details.ID_INVOICE
	            where boxes.invoice_number is null 
                and convert(datetime,shipDate," + DB_Base.BQDataRegion + @")= convert(datetime,'" + Truckdate + @"'," + DB_Base.BQDataRegion + @") 
	            and details.detailId=boxes.detailId 
	            )	where invoice_number is null;		
	        
        update breakdowns set 
            invoice_number = (select top 1 details.invoice_number from details 
            inner join  invoices on invoices.invoices_Id=details.invoices_Id
            and invoices.number=details.invoice_number
            and invoices.id=details.ID_INVOICE
            where breakdowns.invoice_number is null 
            and convert(datetime,shipDate," + DB_Base.BQDataRegion + @")= convert(datetime,'" + Truckdate + @"'," + DB_Base.BQDataRegion + @") 
            and details.detailId=breakdowns.detailId 
            )	where invoice_number is null;	        

            ";

            connection.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, connection);
            object objResult = null;

            SqlCommand dbCommand = adpt.SelectCommand;
            dbCommand.CommandText = strSQL;
            dbCommand.CommandType = CommandType.Text;

            objResult = dbCommand.ExecuteScalar();
            connection.Close();
        }
        private void SaveInTempTablebyNumber(DataSet _ds, string Truckdate, string _invoice, DC_BQ objBQ)
        {
            string strSQL = "";
            SqlConnection connection = new SqlConnection(conString);
            if (_ds.Tables[1].Rows.Count > 0)
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    connection.Open();
                    bulkCopy.BatchSize = 500;
                    bulkCopy.DestinationTableName = "dbo.invoices";
                    foreach (DataColumn column in _ds.Tables["invoices"].Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    bulkCopy.WriteToServer(_ds.Tables[1]);
                    connection.Close();
                }
            }
            if (_ds.Tables.Contains("details"))
            {
                DataTable dt = _ds.Tables[2];
                if (dt.Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        bulkCopy.BatchSize = 500;
                        bulkCopy.DestinationTableName = "dbo.details";
                        foreach (DataColumn column in _ds.Tables["details"].Columns)
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                        bulkCopy.WriteToServer(dt);
                        connection.Close();
                    }
                }
            }
            if (_ds.Tables.Contains("breakdowns"))
            {
                if (_ds.Tables["breakdowns"].Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        bulkCopy.BatchSize = 500;
                        bulkCopy.DestinationTableName = "dbo.breakdowns";
                        foreach (DataColumn column in _ds.Tables["breakdowns"].Columns)
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                        bulkCopy.WriteToServer(_ds.Tables["breakdowns"]);
                        connection.Close();
                    }
                }
            }
            if (_ds.Tables.Contains("boxes"))
            {
                if (_ds.Tables["boxes"].Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        bulkCopy.BatchSize = 500;
                        bulkCopy.DestinationTableName = "dbo.boxes";
                        foreach (DataColumn column in _ds.Tables["boxes"].Columns)
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                        bulkCopy.WriteToServer(_ds.Tables["boxes"]);
                        connection.Close();
                    }
                }
            }
            if (_ds.Tables.Contains("additionalChargesDetails"))
            {
                if (_ds.Tables["additionalChargesDetails"].Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        bulkCopy.BatchSize = 500;
                        bulkCopy.DestinationTableName = "dbo.additionalChargesDetails";
                        foreach (DataColumn column in _ds.Tables["additionalChargesDetails"].Columns)
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                        bulkCopy.WriteToServer(_ds.Tables["additionalChargesDetails"]);
                        connection.Close();
                    }
                }
            }


            strSQL = @"
            update invoices set system_name='" + objBQ.DataFrom + @"';
        
            update details set 
	            ID_INVOICE = (select top 1 invoices.id from invoices 
	            where invoices.invoices_Id=details.invoices_Id 
	            and details.ID_INVOICE is null 
	            )	where ID_INVOICE is null;	

            update details set invoice_number = " + _invoice + @";
            update additionalChargesDetails set invoice_number = " + _invoice + @";
            update boxes set invoice_number = " + _invoice + @";
            update breakdowns set invoice_number = " + _invoice + @";
            ";

            connection.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, connection);
            object objResult = null;

            SqlCommand dbCommand = adpt.SelectCommand;
            dbCommand.CommandText = strSQL;
            dbCommand.CommandType = CommandType.Text;

            objResult = dbCommand.ExecuteScalar();
            connection.Close();
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
        private void DeleteTempData()
        {
            string strSQL = @"
                delete from breakdowns;
                delete from [boxes];
                delete from [details];
                delete from additionalChargesDetails;
                delete from [invoices];
            ";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            object objResult = null;

            SqlCommand dbCommand = adpt.SelectCommand;
            dbCommand.CommandText = strSQL;
            dbCommand.CommandType = CommandType.Text;

            objResult = dbCommand.ExecuteScalar();


        }

        protected void CreateErrorLog(string _localLogPath, string _message)
        {
            CreateLogFiles Err = new CreateLogFiles();
            Err.ErrorLog((path + _localLogPath), _message);
        }
    }
}
