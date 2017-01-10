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
    public class ImportKSBoxType
    {
        string conString = DB_Base.DB_STR;
        string path = System.AppDomain.CurrentDomain.BaseDirectory;
        public void ImportBoxType(DC_BQ objBQ)
        {
            string sLogFormat = "";
            sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
            CreateErrorLog("CustomLogs/ErrorLog", sLogFormat + " : Products");
            try
            {
                DataSet ds = null;
                DeleteTempData(objBQ);
                ds = getBoxTypeList(objBQ);
                if (ds.Tables[0].Rows[0][0].ToString() != "Boxtype not found.")
                {
                    if (ds.Tables.Contains("boxtypes"))
                    {
                        SaveBoxTypeInTempTable(ds, objBQ);
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

        private DataSet getBoxTypeList(DC_BQ objBQ)
        {
            DataSet ds = null;
            //RFI - Domestic: nprijva0sksc2iugl9f5mm4722
            string KSToken = objBQ.KSToken;
            Uri uri = new Uri("https://api.kometsales.com/api/boxtype.list?authenticationToken=" + KSToken + "");
            string data = "";
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                    request.Method = WebRequestMethods.Http.Get;
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

        private void SaveBoxTypeInTempTable(DataSet _ds, DC_BQ objBQ)
        {
            SqlConnection connection = new SqlConnection(conString);
            if (_ds.Tables[1].Rows.Count > 0)
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    connection.Open();
                    bulkCopy.BatchSize = 500;
                    bulkCopy.DestinationTableName = "dbo." + objBQ.DataFrom + @"_BoxType";
                    foreach (DataColumn column in _ds.Tables["boxtypes"].Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    bulkCopy.WriteToServer(_ds.Tables[1]);
                    connection.Close();
                }
            }
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
        
        private void DeleteTempData(DC_BQ objBQ)
        {
            string strSQL = @"
                delete from dbo." + objBQ.DataFrom + @"_BoxType;
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
