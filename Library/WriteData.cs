using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Threading;
using Google.Apis.Bigquery.v2;
using Google.Apis.Bigquery.v2.Data;
using System.Data;
using Google.Apis.Services;
using Newtonsoft.Json;

using Google.Apis.Util;
using System.Diagnostics;
using System.Xml;
using System.Net;

namespace BQ
{
    public class WriteData
    {
        string path = System.AppDomain.CurrentDomain.BaseDirectory;
        public bool InsertTodaysData(DC_BQ objBQ)
        {
            bool done = false;
            try
            {
                string sLogFormat = "";
                sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
                //CreateLogTime("CustomLogs/ExportLogToday", sLogFormat);
                CreateLogTime("CustomLogs/ExportLogToday", sLogFormat + " Invoice Date: " + objBQ.ProcessDay + " : " + objBQ.DataFrom + " : " + objBQ.CallFrom);

                BQService objBS = new BQService();
                BigqueryService Service = objBS.GetServices();

                BQ.ReadKSData objRead = new BQ.ReadKSData();
                DataSet ds = objRead.ReadInvoiceData();
                if (ds.Tables[0].Rows.Count != 0)
                {
                    DoStreaming("CustomLogs/ExportLogToday", Service, objBQ, ds.Tables[0], DB_Base.Invoice_Header_Today);
                    if (ds.Tables[1].Rows.Count != 0)
                    {
                        DoStreaming("CustomLogs/ExportLogToday", Service, objBQ, ds.Tables[1], DB_Base.Invoice_AdditionalChargesDetails_Today);
                    }
                    if (ds.Tables[2].Rows.Count != 0)
                    {
                        DoStreaming("CustomLogs/ExportLogToday", Service, objBQ, ds.Tables[2], DB_Base.Invoice_Details_Today);
                    }
                    if (ds.Tables[3].Rows.Count != 0)
                    {
                        DoStreaming("CustomLogs/ExportLogToday", Service, objBQ, ds.Tables[3], DB_Base.Invoice_Boxes_Today);
                    }
                    if (ds.Tables[4].Rows.Count != 0)
                    {
                        DoStreaming("CustomLogs/ExportLogToday", Service, objBQ, ds.Tables[4], DB_Base.Invoice_Breakdowns_Today);
                    }
                }
                //CreateLogTime("CustomLogs/ExportLogToday", sLogFormat);
                done = true;
                //return done;
            }
            catch (Exception exp)
            {
                CreateErrorLog("CustomLogs/ErrorLog", objBQ.DatasetId + " - " + objBQ.CallFrom + " - " + exp.ToString());
            }
            finally
            {

            }
            return done;
        }
        public bool InsertWeeklyInvoiceData(DC_BQ objBQ)
        {
            bool done = false;
            try
            {
                string sLogFormat = "";
                sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
                //CreateLogTime("CustomLogs/ExportLogWeekly", sLogFormat);
                CreateLogTime("CustomLogs/ExportLogWeekly", sLogFormat + " Invoice Date: " + objBQ.ProcessDay + " : " + objBQ.DataFrom + " : " + objBQ.CallFrom);
                BQService objBS = new BQService();
                BigqueryService Service = objBS.GetServices();

                BQ.ReadKSData objRead = new BQ.ReadKSData();
                DataSet ds = objRead.ReadInvoiceData();
                if (ds.Tables[0].Rows.Count != 0)
                {
                    DoStreaming("CustomLogs/ExportLogWeekly", Service, objBQ, ds.Tables[0], DB_Base.Invoice_Header_Weekly);
                    if (ds.Tables[1].Rows.Count != 0)
                    {
                        DoStreaming("CustomLogs/ExportLogWeekly", Service, objBQ, ds.Tables[1], DB_Base.Invoice_AdditionalChargesDetails_Weekly);
                    }
                    if (ds.Tables[2].Rows.Count != 0)
                    {
                        DoStreaming("CustomLogs/ExportLogWeekly", Service, objBQ, ds.Tables[2], DB_Base.Invoice_Details_Weekly);
                    }
                    if (ds.Tables[3].Rows.Count != 0)
                    {
                        DoStreaming("CustomLogs/ExportLogWeekly", Service, objBQ, ds.Tables[3], DB_Base.Invoice_Boxes_Weekly);
                    }
                    if (ds.Tables[4].Rows.Count != 0)
                    {
                        DoStreaming("CustomLogs/ExportLogWeekly", Service, objBQ, ds.Tables[4], DB_Base.Invoice_Breakdowns_Weekly);
                    }
                }
                //CreateLogTime("CustomLogs/ExportLogWeekly", sLogFormat);
                done = true;
                //return done;
            }
            catch (Exception exp)
            {
                CreateErrorLog("CustomLogs/ErrorLog", objBQ.DatasetId + " - " + objBQ.CallFrom + " - " + exp.ToString());
            }
            finally
            {

            }
            return done;
        }
        public bool InsertPreviousData(DC_BQ objBQ)
        {
            bool done = false;
            try
            {
                string sLogFormat = "";
                sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
                //CreateLogTime("CustomLogs/ExportLogBeforeWeek", sLogFormat);
                CreateLogTime("CustomLogs/ExportLogBeforeWeek", sLogFormat + " Invoice Date: " + objBQ.ProcessDay + " : " + objBQ.DataFrom + " : " + objBQ.CallFrom);
                BQService objBS = new BQService();
                BigqueryService Service = objBS.GetServices();

                BQ.ReadKSData objRead = new BQ.ReadKSData();
                DataSet ds = objRead.ReadInvoiceData();
                if (ds.Tables[0].Rows.Count != 0)
                {
                    DoStreaming("CustomLogs/ExportLogBeforeWeek", Service, objBQ, ds.Tables[0], DB_Base.Invoice_Header);
                    if (ds.Tables[1].Rows.Count != 0)
                    {
                        DoStreaming("CustomLogs/ExportLogBeforeWeek", Service, objBQ, ds.Tables[1], DB_Base.Invoice_AdditionalChargesDetails);
                    }
                    if (ds.Tables[2].Rows.Count != 0)
                    {
                        DoStreaming("CustomLogs/ExportLogBeforeWeek", Service, objBQ, ds.Tables[2], DB_Base.Invoice_Details);
                    }
                    if (ds.Tables[3].Rows.Count != 0)
                    {
                        DoStreaming("CustomLogs/ExportLogBeforeWeek", Service, objBQ, ds.Tables[3], DB_Base.Invoice_Boxes);
                    }
                    if (ds.Tables[4].Rows.Count != 0)
                    {
                        DoStreaming("CustomLogs/ExportLogBeforeWeek", Service, objBQ, ds.Tables[4], DB_Base.Invoice_Breakdowns);
                    }
                }
                //CreateLogTime("CustomLogs/ExportLogBeforeWeek", sLogFormat);
                done = true;
                //return done;
            }
            catch (Exception exp)
            {
                CreateErrorLog("CustomLogs/ErrorLog", objBQ.DatasetId + " - " + objBQ.CallFrom + " - " + exp.ToString());
            }
            finally
            {

            }
            return done;
        }
        public bool InsertMissingInvoice(DC_BQ objBQ)
        {
            bool done = false;
            try
            {
                string sLogFormat = "";
                sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
                //CreateLogTime("CustomLogs/ExportLogMissing", sLogFormat);

                BQService objBS = new BQService();
                BigqueryService Service = objBS.GetServices();

                BQ.ReadKSData objRead = new BQ.ReadKSData();
                DataSet ds = objRead.ReadMissingInvoiceData();
                CreateLogTime("CustomLogs/ExportLogMissing", sLogFormat + " Invoice Date: " + objBQ.ProcessDay + " : " + objBQ.DataFrom + " Order Count: " + ds.Tables[0].Rows.Count);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    string InvoiceList = "";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        InvoiceList += ds.Tables[0].Rows[i]["invoice_number"].ToString() + " - ";
                        InvoiceList += ds.Tables[0].Rows[i]["customerName"].ToString() + Environment.NewLine;
                        //InvoiceList = InvoiceList.Substring(0, InvoiceList.Length - 2);
                    }
                    CreateLogTime("CustomLogs/ExportLogMissing", InvoiceList);
                    if (ds.Tables[5].Rows.Count != 0)
                    {
                        CreateLogTime("CustomLogs/ExportLogChangesInInvoiceAfterConfirming", sLogFormat + " Invoice Date: " + objBQ.ProcessDay + " : " + objBQ.DataFrom + " Order Count: " + ds.Tables[5].Rows.Count);
                        InvoiceList = "";
                        for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                        {

                            InvoiceList += ds.Tables[5].Rows[i]["invoice_number"].ToString() + " - ";
                            InvoiceList += ds.Tables[5].Rows[i]["customerName"].ToString() + " - ";
                            InvoiceList += ds.Tables[5].Rows[i]["Reason"].ToString() + Environment.NewLine;
                            //InvoiceList = InvoiceList.Substring(0, InvoiceList.Length - 2);
                        }
                        CreateLogTime("CustomLogs/ExportLogChangesInInvoiceAfterConfirming", InvoiceList);
                    }
                    //DoStreaming("CustomLogs/ExportLogMissing", Service, objBQ, ds.Tables[0], DB_Base.Invoice_Header);
                    //if (ds.Tables[1].Rows.Count != 0)
                    //{
                    //    DoStreaming("CustomLogs/ExportLogMissing", Service, objBQ, ds.Tables[1], DB_Base.Invoice_AdditionalChargesDetails);
                    //}
                    //if (ds.Tables[2].Rows.Count != 0)
                    //{
                    //    DoStreaming("CustomLogs/ExportLogMissing", Service, objBQ, ds.Tables[2], DB_Base.Invoice_Details);
                    //}
                    //if (ds.Tables[3].Rows.Count != 0)
                    //{
                    //    DoStreaming("CustomLogs/ExportLogMissing", Service, objBQ, ds.Tables[3], DB_Base.Invoice_Boxes);
                    //}
                    //if (ds.Tables[4].Rows.Count != 0)
                    //{
                    //    DoStreaming("CustomLogs/ExportLogMissing", Service, objBQ, ds.Tables[4], DB_Base.Invoice_Breakdowns);
                    //}
                }
                //CreateLogTime("CustomLogs/ExportLogMissing", sLogFormat);
                done = true;
                //return done;
            }
            catch (Exception exp)
            {
                CreateErrorLog("CustomLogs/ErrorLog", exp.ToString());
            }
            finally
            {

            }
            return done;
        }

        public bool InsertCreditDataRegular(DC_BQ objBQ)
        {
            bool done = false;
            try
            {
                string sLogFormat = "";
                sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
                CreateLogTime("CustomLogs/ExportLogCredit", sLogFormat + " - Credit Date: " + objBQ.ProcessDay + " : " + objBQ.DataFrom + " : " + objBQ.CallFrom);
                BQService objBS = new BQService();
                BigqueryService Service = objBS.GetServices();
                BQ.ReadKSData objRead = new BQ.ReadKSData();

                DataTable dtInvoiceList = objRead.GetCreditQueryInvoiceList();
                if (dtInvoiceList.Rows.Count > 0)
                {
                    for (int i = 0; i < dtInvoiceList.Rows.Count; i++)
                    {
                        objBQ.InvoiceNumber = dtInvoiceList.Rows[i]["invoice_number"].ToString();

                        BQ.BQReadCreditData objY = new BQ.BQReadCreditData();
                        DataTable dtBQCredit = objY.ReadCredit(objBQ);
                        if (dtBQCredit.Rows.Count < 1)
                        {
                            CreateLogTime("CustomLogs/CreditStatus", " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== " + " - Not found: " + objBQ.InvoiceNumber);
                            DataSet ds = objRead.ReadCreditData(objBQ.InvoiceNumber);
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                DoStreaming("CustomLogs/ExportLogCredit", Service, objBQ, ds.Tables[0], DB_Base.Credit_Header);
                                if (ds.Tables[1].Rows.Count != 0)
                                {
                                    DoStreaming("CustomLogs/ExportLogCredit", Service, objBQ, ds.Tables[1], DB_Base.Credit_Details);
                                }
                                if (ds.Tables[2].Rows.Count != 0)
                                {
                                    DoStreaming("CustomLogs/ExportLogCredit", Service, objBQ, ds.Tables[2], DB_Base.Credit_Comments);
                                }
                            }
                        }
                        else
                        {
                            CreateLogTime("CustomLogs/CreditStatus", " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== " + " - Found: " + objBQ.InvoiceNumber + " - " + objBQ.DatasetId);
                        }
                    }
                }
                //CreateLogTime("CustomLogs/ExportLogCredit", sLogFormat);
                done = true;
                //return done;
            }
            catch (Exception exp)
            {
                CreateErrorLog("CustomLogs/ErrorLog", objBQ.DatasetId + " - " + objBQ.CallFrom + " - " + exp.ToString());
            }
            finally
            {

            }
            return done;
        }
        public bool InsertCreditDataArchive(DC_BQ objBQ)
        {
            bool done = false;
            try
            {
                string sLogFormat = "";
                sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
                CreateLogTime("CustomLogs/ExportLogCredit", sLogFormat + " - Credit Date: " + objBQ.ProcessDay + " : " + objBQ.DataFrom);
                BQService objBS = new BQService();
                BigqueryService Service = objBS.GetServices();
                BQ.ReadKSData objRead = new BQ.ReadKSData();
                DataSet ds = objRead.ReadCreditData();
                if (ds.Tables[0].Rows.Count != 0)
                {
                    DoStreaming("CustomLogs/ExportLogCredit", Service, objBQ, ds.Tables[0], DB_Base.Credit_Header);
                    if (ds.Tables[1].Rows.Count != 0)
                    {
                        DoStreaming("CustomLogs/ExportLogCredit", Service, objBQ, ds.Tables[1], DB_Base.Credit_Details);
                    }
                    if (ds.Tables[2].Rows.Count != 0)
                    {
                        DoStreaming("CustomLogs/ExportLogCredit", Service, objBQ, ds.Tables[2], DB_Base.Credit_Comments);
                    }
                }
                done = true;
            }
            catch (Exception exp)
            {
                CreateErrorLog("CustomLogs/ErrorLog", exp.ToString());
            }
            finally
            {

            }
            return done;
        }

        public bool InsertVendorData(DC_BQ objBQ)
        {
            bool done = false;
            try
            {
                string sLogFormat = "";
                sLogFormat = " ======== " + DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ======== ";
                CreateLogTime("CustomLogs/ExportLogMonthly", sLogFormat);
                CreateLogTime("CustomLogs/ExportLogMonthly", "Vendor Data");

                BQService objBS = new BQService();
                BigqueryService Service = objBS.GetServices();

                BQ.ReadKSData objRead = new BQ.ReadKSData();
                DataSet ds = objRead.ReadVendorData();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    DoStreaming("CustomLogs/ExportLogMonthly", Service, objBQ, ds.Tables[0], DB_Base.Vendors);
                }
                CreateLogTime("CustomLogs/ExportLogMonthly", sLogFormat);
                done = true;
            }
            catch (Exception exp)
            {
                CreateErrorLog("CustomLogs/ErrorLog", exp.ToString());
            }
            finally
            {

            }
            return done;
        }
        protected void DoStreaming(string _localLogPath, BigqueryService Service, DC_BQ objBQ, DataTable dt, string _TableName)
        {
            List<TableDataInsertAllRequest.RowsData> rowList = GetRowsList(dt);
            TableDataInsertAllRequest content = new TableDataInsertAllRequest()
            {
                Kind = "bigquery#tableDataInsertAllRequest",
                Rows = rowList,
                IgnoreUnknownValues = true,
                SkipInvalidRows = true
            };

            var insertTask = Service.Tabledata.InsertAll(content,
                    objBQ.ProjectId, objBQ.DatasetId, _TableName);
            TableDataInsertAllResponse response = insertTask.Execute();
            CreateLog(_localLogPath, response, _TableName, objBQ.ProcessDay);
        }
        protected List<TableDataInsertAllRequest.RowsData> GetRowsList(DataTable dt)
        {
            var rowList = new List<TableDataInsertAllRequest.RowsData>();
            if (dt.Rows.Count > 0)
            {
                for (int index = 0; index < dt.Rows.Count; index++)
                {
                    var row = new TableDataInsertAllRequest.RowsData();

                    row.Json = new Dictionary<string, object>();
                    foreach (DataColumn column in dt.Columns)
                    {
                        string ColumnName = column.ColumnName;
                        string ColumnData = dt.Rows[index][ColumnName].ToString(); //dtrow[column].ToString();
                        row.Json.Add(ColumnName, ColumnData);
                    }
                    rowList.Add(row);
                }
            }
            return rowList;
        }
        protected void CreateLog(string _localLogPath, TableDataInsertAllResponse _response, string _tableName, string _processDay)
        {
            string message = "", noOfErr = "0";
            string Location = "";
            string Message = "";
            string Reason = "";
            int i = 1;
            if (_response.InsertErrors != null)
            {
                CreateLogFiles Err = new CreateLogFiles();
                noOfErr = _response.InsertErrors.Count.ToString();
                IEnumerable<TableDataInsertAllResponse.InsertErrorsData> insertErrors = _response.InsertErrors;
                foreach (var insertError in insertErrors)
                {
                    IEnumerable<ErrorProto> lst = insertError.Errors;
                    foreach (var ls in lst)
                    {
                        Location = ls.Location;
                        Message = ls.Message;
                        Reason = ls.Reason;

                        message = i.ToString() + " of " + noOfErr + " - " + _tableName + " - " + _processDay + " - " + Location + " - " + Message + " - " + Reason;
                        Err.ErrorLog((path + _localLogPath), message);
                        i = i + 1;
                    }
                }
            }
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

