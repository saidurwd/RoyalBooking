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
    public class ImportKSPrebooks
    {
        string conString = DB_Base.DB_STR;
        string path = System.AppDomain.CurrentDomain.BaseDirectory;
        public DataSet ImportKSPrebooksDataForAzure(DC_BQ objBQ)
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
                        //DeleteDataByDate(truckdate2, objBQ);
                        ds = getPrebooksList(TruckDate, objBQ);
                        if (ds.Tables[0].Rows[0][0].ToString() != "Orders not found.OK")
                        {
                            if (ds.Tables.Contains("prebooks"))
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
                CreateErrorLog("CustomLogs/ErrorLogPrebookImport", sLogFormat + " : " + objBQ.FromDate + " : " + objBQ.DataFrom + " : " + objBQ.CallFrom);
                CreateErrorLog("CustomLogs/ErrorLogPrebookImport", exp.ToString());

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

            Mail.Subject = "Exception in Data warehouse-Prebooks";
            Mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com");
            client.Credentials = new System.Net.NetworkCredential("blumensoft@royalcorp.net", "Password!8");
            client.EnableSsl = true;
            client.Port = 587;

            Mail.Body = "<html><body><br>" + "<center><b> </b></center> <br> " + _message + "</body></html>";
            client.Send(Mail);
            Mail.Dispose();
        }
        
        private DataSet getPrebooksList(string Truckdate, DC_BQ objBQ)
        {
            DataSet ds = null;
            //RFI - Domestic: nprijva0sksc2iugl9f5mm4722
            string KSToken = objBQ.KSToken;
            Uri uri = new Uri("https://api.kometsales.com/api/prebook.details.list?authenticationToken=" + KSToken + "&date=" + Truckdate + "");
            string data = "";
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                try
                {
                    string result = "";
                    // Create the web request  
                    HttpWebRequest request = WebRequest.Create("https://api.kometsales.com/api/prebook.details.list?authenticationToken=" + KSToken + "&date=" + Truckdate + "&status=Approved") as HttpWebRequest;
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

        public DataSet getPrebookSummary(string Truckdate, DC_BQ objBQ)
        {
            DataSet ds = null;
            //RFI - Domestic: nprijva0sksc2iugl9f5mm4722
            string KSToken = objBQ.KSToken;
            
            Uri uri = new Uri("https://api.kometsales.com/api/prebook.details.list?authenticationToken=" + KSToken + "&date=" + Truckdate + "&prebook="+objBQ.PrebooksId+"");
            string data = "";
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                try
                {
                    string result = "";
                    // Create the web request  
                    HttpWebRequest request = WebRequest.Create("https://api.kometsales.com/api/prebook.details.list?authenticationToken=" + KSToken + "&date=" + Truckdate + "&prebook=" + objBQ.PrebooksId + "") as HttpWebRequest;
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

        public DataSet getCustomerShipTo(DC_BQ objBQ, Prebooks objPB)
        {
            DataSet ds = null;
            string KSToken = objBQ.KSToken;
            Uri uri = new Uri("https://api.kometsales.com/api/customer.shipto.list?authenticationToken=" + KSToken + "&customerId=" + objPB.customerId + "");
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                try
                {
                    string result = "";
                    // Create the web request  
                    HttpWebRequest request = WebRequest.Create("https://api.kometsales.com/api/customer.shipto.list?authenticationToken=" + KSToken + "&customerId=" + objPB.customerId + "") as HttpWebRequest;
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
            DataSet dsColName = objRD.GetPrebooksColumnName();

            DataTable dataTable;
            string strSQL = "";
            SqlConnection connection = new SqlConnection(conString);
            if (_ds.Tables["prebooks"].Rows.Count > 0)
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
                    bulkCopy.DestinationTableName = "dbo.KS_Prebooks";
                    foreach (DataColumn column in _ds.Tables["prebooks"].Columns)
                    {
                        if (columnNames.Contains(column.ColumnName))
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                    }
                    bulkCopy.WriteToServer(_ds.Tables["prebooks"]);
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
                        bulkCopy.DestinationTableName = "dbo.KS_PrebooksDetails";
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
                dataTable = dsColName.Tables[2];
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
                        bulkCopy.DestinationTableName = "dbo.KS_PrebooksBreakDowns";
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
           
            

            strSQL = @"
        insert into dbo.Prebooks_duplicates (number, source, status, old_truckDate, new_truckDate) 
        select N.number, '" + objBQ.DataFrom + @"', 1, O.truckDate, N.truckDate 
        from dbo." + objBQ.DataFrom + @"_KS_Prebooks O inner join KS_Prebooks N on N.number=O.number
        where O.number in (
	        select distinct number from KS_Prebooks 
        );

        delete from dbo." + objBQ.DataFrom + @"_KS_PrebooksBreakDowns where number in (
	        select distinct number from KS_Prebooks 
        );
        delete from dbo." + objBQ.DataFrom + @"_KS_PrebooksDetails where number in (
	        select distinct number from KS_Prebooks 
        );
        delete from dbo." + objBQ.DataFrom + @"_KS_Prebooks where number in (
	        select distinct number from KS_Prebooks 
        );
        
        --update KS_PrebooksBreakDowns set prebooks_Id=0 where prebooks_Id is null;
                
        update KS_PrebooksDetails set 
                    number = (select top 1 KS_Prebooks.number from KS_Prebooks 
                        where KS_Prebooks.prebooks_Id=KS_PrebooksDetails.prebooks_Id 
                        and KS_PrebooksDetails.number is null 
                        and convert(datetime,Truckdate,101)= convert(datetime,'" + Truckdate + @"',101) 
                    ) 
        where number is null;
                
        update KS_PrebooksBreakDowns set 
            number = (select top 1 KS_Prebooks.number from KS_PrebooksDetails
                inner join  KS_Prebooks on 
                KS_Prebooks.prebooks_Id=KS_PrebooksDetails.prebooks_Id
                and KS_Prebooks.number=KS_PrebooksDetails.number
                where KS_PrebooksBreakDowns.details_Id=KS_PrebooksDetails.details_Id 
                and KS_PrebooksBreakDowns.number is null
                and convert(datetime,Truckdate,101)= convert(datetime,'" + Truckdate + @"',101)),
            prebooks_Id = (select top 1 KS_PrebooksDetails.prebooks_Id from KS_PrebooksDetails
                inner join  KS_Prebooks on 
                KS_Prebooks.number=KS_PrebooksDetails.number
                where KS_PrebooksBreakDowns.details_Id=KS_PrebooksDetails.details_Id
                and KS_PrebooksBreakDowns.prebooks_Id is null
                and convert(datetime,Truckdate,101)= convert(datetime,'" + Truckdate + @"',101) 
            )
        where number is null and prebooks_Id is null;

        insert into dbo." + objBQ.DataFrom + @"_KS_Prebooks select * from  dbo.KS_Prebooks;           
        insert into dbo." + objBQ.DataFrom + @"_KS_PrebooksDetails select * from  dbo.KS_PrebooksDetails;           
        insert into dbo." + objBQ.DataFrom + @"_KS_PrebooksBreakDowns select * from  dbo.KS_PrebooksBreakDowns;           
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
            CreateLogTime("CustomLogs/ErrorLogPrebookImport", sLogFormat + " PO Ship Date: " + Truckdate + " : " + objBQ.DataFrom + " : " + objBQ.CallFrom);
        }

        public void SaveBreakDown(DataTable _dt, DC_BQ objBQ)
        {
            ReadKSData objRD = new ReadKSData();
            DataSet dsColName = objRD.GetPrebooksColumnName();

            DataTable dataTable;
            string strSQL = "";
            SqlConnection connection = new SqlConnection(conString);

            strSQL = @"
                DELETE FROM dbo.KS_PrebooksBreakDowns;
            ";

            connection.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, connection);
            object objResult = null;

            SqlCommand dbCommand = adpt.SelectCommand;
            dbCommand.CommandText = strSQL;
            dbCommand.CommandTimeout = 0;
            dbCommand.CommandType = CommandType.Text;
            objResult = dbCommand.ExecuteScalar();

            if (_dt.Rows.Count>0)
            {
                dataTable = dsColName.Tables[2];
                string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                                     .Select(x => x.ColumnName)
                                     .ToArray();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        bulkCopy.BatchSize = 500;
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = "dbo.KS_PrebooksBreakDowns";
                        foreach (DataColumn column in _dt.Columns)
                        {
                            if (columnNames.Contains(column.ColumnName))
                            {
                                bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                            }
                        }
                        bulkCopy.WriteToServer(_dt);
                        connection.Close();
                    }
                
            }
            
            connection.Close();

            string sLogFormat = "";
            sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
            CreateLogTime("CustomLogs/PrebookBreakDownSave", sLogFormat + " PO Ship Date: " + objBQ.ProcessDay + " : " + objBQ.DataFrom + " : " + objBQ.CallFrom);
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
        public void DeleteDataAll(DC_BQ objBQ)
        {
            string strSQL = @"
                DELETE FROM dbo." + objBQ.DataFrom + @"_KS_PrebooksBreakDowns;
                DELETE FROM dbo." + objBQ.DataFrom + @"_KS_PrebooksDetails;
                DELETE FROM dbo." + objBQ.DataFrom + @"_KS_Prebooks;
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
                delete from KS_PrebooksBreakDowns;
                delete from [KS_PrebooksDetails];
                delete from [KS_Prebooks];
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

                Mail.Subject = "Duplicates Prebooks";
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
