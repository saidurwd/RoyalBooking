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
    public class CreateKSPrebooks
    {
        string conString = DB_Base.DB_STR;
        string path = System.AppDomain.CurrentDomain.BaseDirectory;
        public DataSet CreateKSPrebooksById(DC_BQ objBQ, Prebooks objPB)
        {
            string sLogFormat = "";
            DataSet ds = null;
            try
            {
                ds = CreatePrebooks(objBQ, objPB);
            }
            catch (Exception exp)
            {
                sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
                CreateErrorLog("CustomLogs/ErrorLogPrebookDelete", sLogFormat + " : " + objBQ.InvoiceNumber + " : " + objBQ.ProductId + " : " + objBQ.CallFrom);
                CreateErrorLog("CustomLogs/ErrorLogPrebookDelete", exp.ToString());

                string strExceptionInEmail = sLogFormat + " : " + objBQ.InvoiceNumber + " : " + objBQ.ProductId + " : " + objBQ.CallFrom + "<br><br>";
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

        private DataSet CreatePrebooks(DC_BQ objBQ, Prebooks objPB)
        {
            DataSet ds = null;
            string KSToken = objBQ.KSToken;
            
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.kometsales.com/api/prebook.create");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            ReadKSData objK = new BQ.ReadKSData();
            DataSet dsJson = objK.ReadKSPOJSON(objBQ, objPB);
            string json = dsJson.Tables[0].Rows[0][0].ToString();
            json = json.Remove(json.Length - 2);
            json = json.Substring(2, json.Length - 2);
            string jsonDetails = dsJson.Tables[1].Rows[0][0].ToString();
            jsonDetails = jsonDetails.Remove(jsonDetails.Length - 1);
            jsonDetails = jsonDetails.Substring(1, jsonDetails.Length - 1);
            json += ", \"prebookItems\":[" + jsonDetails+"]}";
            json = "{" + json;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
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
        
        protected void CreateLogTime(string _localLogPath, string _message)
        {
            CreateLogFiles Err = new CreateLogFiles();
            Err.ErrorLog((path + _localLogPath), _message);
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
        protected void CreateErrorLog(string _localLogPath, string _message)
        {
            CreateLogFiles Err = new CreateLogFiles();
            Err.ErrorLog((path + _localLogPath), _message);
        }

    }
}
