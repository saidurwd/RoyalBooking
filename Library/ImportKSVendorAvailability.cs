using System;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using System.Net;
using System.Web.Script.Serialization;

namespace BQ
{
    public class ImportKSVendorAvailability
    {
        string conString = DB_Base.DB_STR;
        string path = System.AppDomain.CurrentDomain.BaseDirectory;

        public void ImportVendorAvailability(DC_BQ objBQ)
        {

            DataSet ds = null;
            try
            {
                //DeleteTempData(objBQ);
                ds = getDataList(objBQ);
                if (ds.Tables[0].Rows[0][0].ToString() != "credits not found.")
                {
                    if (ds.Tables.Contains("inventoryItems"))
                    {
                        SaveDataInTempTable(ds, objBQ);
                    }
                }
            }
            catch (Exception exp)
            {
                string sLogFormat = "";
                sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
                CreateErrorLog("CustomLogs/ErrorLogVA", sLogFormat + " : VA - " + objBQ.ProcessDay + " - " + objBQ.CallFrom + " - " + objBQ.CreditStatus);
                CreateErrorLog("CustomLogs/ErrorLogVA", exp.ToString());
                string strExceptionInEmail = sLogFormat + " : VA - " + objBQ.ProcessDay + " - " + objBQ.CallFrom + " - " + objBQ.CreditStatus + "<br><br>";
                strExceptionInEmail += "Exception Details: <br>";
                strExceptionInEmail += exp.ToString();
                SendmailIfException(strExceptionInEmail);
            }
            finally
            {

            }

        }

        public DataSet ImportVendorAvailabilityReturn(DC_BQ objBQ)
        {

            DataSet ds = null;
            try
            {
                //DeleteTempData(objBQ);
                ds = getDataList(objBQ);
                //if (ds.Tables[0].Rows[0][0].ToString() != "credits not found.")
                //{
                //    if (ds.Tables.Contains("inventoryItems"))
                //    {
                //        SaveDataInTempTable(ds, objBQ);
                //    }
                //}
            }
            catch (Exception exp)
            {
                string sLogFormat = "";
                sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
                CreateErrorLog("CustomLogs/ErrorLogVA", sLogFormat + " : VA - " + objBQ.ProcessDay + " - " + objBQ.CallFrom + " - " + objBQ.CreditStatus);
                CreateErrorLog("CustomLogs/ErrorLogVA", exp.ToString());
                string strExceptionInEmail = sLogFormat + " : VA - " + objBQ.ProcessDay + " - " + objBQ.CallFrom + " - " + objBQ.CreditStatus + "<br><br>";
                strExceptionInEmail += "Exception Details: <br>";
                strExceptionInEmail += exp.ToString();
                SendmailIfException(strExceptionInEmail);
            }
            finally
            {

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

            Mail.Subject = "Exception in Data warehouse";
            Mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com");
            client.Credentials = new System.Net.NetworkCredential("blumensoft@royalcorp.net", "Password!8");
            client.EnableSsl = true;
            client.Port = 587;

            Mail.Body = "<html><body><br>" + "<center><b> </b></center> <br> " + _message + "</body></html>";
            client.Send(Mail);
            Mail.Dispose();
        }
        private DataSet getDataList(DC_BQ objBQ)
        {
            DataSet ds = null;
            //RFI - Domestic: nprijva0sksc2iugl9f5mm4722
            string KSToken = objBQ.KSToken;
            string orderDate = objBQ.ProcessDay;


            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.kometsales.com/api/vendor.availability.items.list");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    authenticationToken = "" + KSToken + "",
                    dateFrom = "" + objBQ.FromDate + "",
                    dateTo = "" + objBQ.ToDate + ""
                });

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                ds = DerializeDataTable(result);
            }

            return ds;
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
        public void DeleteTempData(DC_BQ objBQ)
        {
            string strSQL = @"
                DELETE FROM  dbo.[VA_inventoryItems];
                DELETE FROM dbo." + objBQ.DataFrom + @"_VA_inventoryItems
            ";
            SqlConnection con = new SqlConnection(conString);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            object objResult = null;

            SqlCommand dbCommand = adpt.SelectCommand;
            dbCommand.CommandText = strSQL;
            dbCommand.CommandType = CommandType.Text;

            objResult = dbCommand.ExecuteScalar();
            adpt.Dispose();
            dbCommand.Dispose();
            con.Dispose();
        }
        private void SaveDataInTempTable(DataSet _ds, DC_BQ objBQ)
        {
            ReadKSData objRD = new ReadKSData();
            DataSet dsColName = objRD.GetColumnNameVendorAvailability();

            DataTable dataTable;
            SqlConnection connection = new SqlConnection(conString);
            if (_ds.Tables["inventoryItems"].Rows.Count > 0)
            {
                dataTable = dsColName.Tables[0];
                string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                                     .Select(x => x.ColumnName)
                                     .ToArray();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    connection.Open();
                    bulkCopy.BatchSize = 500;
                    bulkCopy.BulkCopyTimeout = 0;
                    bulkCopy.DestinationTableName = "dbo.VA_inventoryItems";
                    foreach (DataColumn column in _ds.Tables["inventoryItems"].Columns)
                    {
                        if (columnNames.Contains(column.ColumnName))
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                    }
                    bulkCopy.WriteToServer(_ds.Tables["inventoryItems"]);
                    connection.Close();
                }
                dataTable.Dispose();
            }
            string strSQL = "";
            strSQL = @"        
            insert into dbo." + objBQ.DataFrom + @"_VA_inventoryItems select * from  dbo.VA_inventoryItems;           
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

            dsColName.Dispose();
            _ds.Dispose();
            connection.Dispose();

            string sLogFormat = "";
            sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
            CreateLogTime("CustomLogs/ExportLogVA", sLogFormat + " Date: " + objBQ.ProcessDay + " : " + objBQ.DataFrom + " : " + objBQ.CallFrom);
        }
        protected void CreateLogTime(string _localLogPath, string _message)
        {
            CreateLogFiles Err = new CreateLogFiles();
            Err.ErrorLog((path + _localLogPath), _message);
        }
        protected void CreateErrorLog(string _localLogPath, string _message)
        {
            CreateLogFiles Err = new CreateLogFiles();
            Err.ErrorLog((path + _localLogPath), _message);
        }
    }
}
