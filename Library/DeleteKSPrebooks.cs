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
    public class DeleteKSPrebooks
    {
        string conString = DB_Base.DB_STR;
        string path = System.AppDomain.CurrentDomain.BaseDirectory;
        public DataSet DeleteKSPrebooksById(DC_BQ objBQ)
        {
            string sLogFormat = "";
            DataSet ds = null;
            try
            {
                ds = DeletePrebooks(objBQ);
            }
            catch (Exception exp)
            {
                sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
                CreateErrorLog("CustomLogs/ErrorLogPrebookDelete", sLogFormat + " : " + objBQ.PrebooksId + " : " + objBQ.ProductId + " : " + objBQ.CallFrom);
                CreateErrorLog("CustomLogs/ErrorLogPrebookDelete", exp.ToString());

                string strExceptionInEmail = sLogFormat + " : " + objBQ.PrebooksId + " : " + objBQ.ProductId + " : " + objBQ.CallFrom + "<br><br>";
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
        public DataSet DeleteKSPOById(DC_BQ objBQ)
        {
            string sLogFormat = "";
            DataSet ds = null;
            try
            {
                ds = DeletePO(objBQ);
            }
            catch (Exception exp)
            {
                sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
                CreateErrorLog("CustomLogs/ErrorLogPrebookDelete", sLogFormat + " : " + objBQ.PrebooksId + " : " + objBQ.ProductId + " : " + objBQ.CallFrom);
                CreateErrorLog("CustomLogs/ErrorLogPrebookDelete", exp.ToString());

                string strExceptionInEmail = sLogFormat + " : " + objBQ.PrebooksId + " : " + objBQ.ProductId + " : " + objBQ.CallFrom + "<br><br>";
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

        private DataSet DeletePrebooks(DC_BQ objBQ)
        {
            DataSet ds = null;
            string KSToken = objBQ.KSToken;
            
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.kometsales.com/api/prebook.item.delete");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    authenticationToken = "" + KSToken + "",
                    prebookId = "" + objBQ.PrebooksId + "",
                    prebookItemId = "" + objBQ.ProductId + ""
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
        private DataSet DeletePO(DC_BQ objBQ)
        {
            //DataSet ds = null;
            //string KSToken = objBQ.KSToken;

            //var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.kometsales.com/api/purchase.order.item.delete");
            //httpWebRequest.ContentType = "application/json";
            //httpWebRequest.Method = "POST";

            //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            //{
            //    string json = new JavaScriptSerializer().Serialize(new
            //    {
            //        authenticationToken = "" + KSToken + "",
            //        poItemId = "" + objBQ.poItemId + ""
            //    });

            //    streamWriter.Write(json);
            //}

            //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    var result = streamReader.ReadToEnd();
            //    ds = DerializeDataTable(result);
            //}

            //return ds;

            DataSet ds = null;
            string KSToken = objBQ.KSToken;
            Uri uri = new Uri("https://api.kometsales.com/api/purchase.order.item.delete");
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                try
                {
                    string result = "";
                    // Create the web request  
                    HttpWebRequest request = WebRequest.Create("https://api.kometsales.com/api/purchase.order.item.delete?authenticationToken=" + KSToken + "&poItemId=" + objBQ.poItemId + "") as HttpWebRequest;
                    request.Method = "POST";
                    request.ContentType = "application/json";
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
