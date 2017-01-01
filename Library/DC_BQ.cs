using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQ
{
    public class DB_Base
    {
        public static string KSRFIDomesticToken = "nprijva0sksc2iugl9f5mm4722";
        public static string KSRFIInternationalToken = "s8jm0u6ds3jrg85e5p77okl11n";
        public static string KSRFGToken = "ofleftucl52u19r2c0pqff0jkr";
        public static string KSJOYToken = "7oa8s7ljmj2jgtgqios2a4d9vv";

        public static string KSRFIDomesticDataFrom = "Domestic";
        public static string KSRFIInternationalDataFrom = "International";
        public static string KSRFGDataFrom = "RFG";
        public static string KSJOYDataFrom = "Joy";

        //public const string DB_STR = @"Data Source=.\BLUMENSOFT;Initial Catalog=BQ;Persist Security Info=True;User ID=sa;Password=sql@2012; Min Pool Size=5;Max Pool Size=200;Connect Timeout=0;MultipleActiveResultSets=True;";
        public const string DB_STR = @"Data Source=.\SQL2014;Initial Catalog=BQAZURE;Persist Security Info=True;User ID=sa;Password=sql2014; Min Pool Size=5;Max Pool Size=200;Connect Timeout=0;MultipleActiveResultSets=True;";
        //public const string DB_STR = @"Data Source=DESKTOP-AF7JH3R\MSSQLSERVER2014;Initial Catalog=BQAZURE;Persist Security Info=True;User ID=sa;Password=SQL2014$$; Min Pool Size=5;Max Pool Size=200;Connect Timeout=0;MultipleActiveResultSets=True;";
        //public const string DB_STR = @"Data Source=royal-dw01.database.windows.net;Initial Catalog=DataWarehouse;Persist Security Info=True;User ID=royaladmin@royal-dw01;Password=zxcasdQWE!@#; Min Pool Size=5;Max Pool Size=200;Connect Timeout=0;MultipleActiveResultSets=True;";
        public static string AppName = "RFISales";
        public static string BQDataRegion = "101";
        
        //RFI_Domestic
        //RFI_International
        //RFG
        public static string BQProjectID = "rfi-sales-data";
        public static string BQDataSetForRFIDomestic = "RFI_Domestic";
        public static string BQDataSetForRFIInternational = "RFI_International";
        public static string BQDataSetForRFG = "RFG";
        public static string BQDataSetForAllCompany = "CommonDataSet";

        public static string BQDataSetForArciveRFIDomestic = "RFI_Domestic_Archive";
        public static string BQDataSetForArciveRFIInternational = "RFI_International_Archive";
        
        public static string BQTableOperation = "bigquery#table";
        public static string BQInsertDataOperation = "bigquery#tableDataInsertAllRequest";

        //Invoice Tables - Today
        public static string Invoice_Header_Today = "Invoice_Header_Today";
        public static string Invoice_AdditionalChargesDetails_Today = "Invoice_AdditionalChargesDetails_Today";
        public static string Invoice_Details_Today = "Invoice_Details_Today";
        public static string Invoice_Boxes_Today = "Invoice_Boxes_Today";
        public static string Invoice_Breakdowns_Today = "Invoice_Breakdowns_Today";

        //Invoice Tables - Weekly
        public static string Invoice_Header_Weekly = "Invoice_Header_Weekly";
        public static string Invoice_AdditionalChargesDetails_Weekly = "Invoice_AdditionalChargesDetails_Weekly";
        public static string Invoice_Details_Weekly = "Invoice_Details_Weekly";
        public static string Invoice_Boxes_Weekly = "Invoice_Boxes_Weekly";
        public static string Invoice_Breakdowns_Weekly = "Invoice_Breakdowns_Weekly";

        //Invoice Tables - Archive
        public static string Invoice_Header = "Invoice_Header";
        public static string Invoice_AdditionalChargesDetails = "Invoice_AdditionalChargesDetails";
        public static string Invoice_Details = "Invoice_Details";
        public static string Invoice_Boxes = "Invoice_Boxes";
        public static string Invoice_Breakdowns = "Invoice_Breakdowns";

        //Vendor
        public static string Vendors = "Vendors";

        //Credit Tables
        public static string Credit_Header = "Credit_Header";
        public static string Credit_Details = "Credit_Details";
        public static string Credit_Comments = "Credit_Comments";

        //Call From
        public static string Call_From_Today_Sales = "Today Sales";
        public static string Call_From_Weekly_Sales = "Weekly Sales";
        public static string Call_From_Day_8th_Sales = "Day_8th Sales";
        public static string Call_From_Monthly_Sales = "Monthly Sales";
        public static string Call_From_Import_Prebooks_Date_Duration = "Import Prebook Date Duration";
        public static string Call_From_Credit_60_1 = "Credit 60-1";

        public static string Call_From_Today_Sales_CommonDataSet = "Today Sales for Common Dataset";
        public static string Call_From_Weekly_Sales_CommonDataSet = "Weekly Sales for Common Dataset";
        public static string Call_From_Day_8th_Sales_CommonDataSet = "Day_8th Sales for Common Dataset";
        public static string Call_From_Credit_60_1_CommonDataSet = "Credit 60-1 for Common Dataset";
        
    }
    
    public class DC_BQ
    {

        private string p_ProcessDay = System.DateTime.Today.ToShortDateString();
        public string ProcessDay
        {
            get
            {
                return p_ProcessDay;
            }
            set
            {
                p_ProcessDay = value;
            }
        }

        private string p_FromDate = System.DateTime.Today.ToShortDateString();
        public string FromDate
        {
            get
            {
                return p_FromDate;
            }
            set
            {
                p_FromDate = value;
            }
        }
        private string p_ToDate = System.DateTime.Today.ToShortDateString();
        public string ToDate
        {
            get
            {
                return p_ToDate;
            }
            set
            {
                p_ToDate = value;
            }
        }
        private string p_ProjectId = String.Empty;
        public string ProjectId
        {
            get
            {
                return p_ProjectId;
            }
            set
            {
                p_ProjectId = value;
            }
        }
        private string p_DatasetId = String.Empty;
        public string DatasetId
        {
            get
            {
                return p_DatasetId;
            }
            set
            {
                p_DatasetId = value;
            }
        }

        private string p_KSToken = String.Empty;
        public string KSToken
        {
            get
            {
                return p_KSToken;
            }
            set
            {
                p_KSToken = value;
            }
        }

        private string p_InvoiceNumber = String.Empty;
        public string InvoiceNumber
        {
            get
            {
                return p_InvoiceNumber;
            }
            set
            {
                p_InvoiceNumber = value;
            }
        }

        private string p_DataFrom = String.Empty;
        public string DataFrom
        {
            get
            {
                return p_DataFrom;
            }
            set
            {
                p_DataFrom = value;
            }
        }
        
        private int p_CreditStatus = -1;
        public int CreditStatus
        {
            get
            {
                return p_CreditStatus;
            }
            set
            {
                p_CreditStatus = value;
            }
        }

        private string p_BQDate = System.DateTime.Today.AddDays(-1).ToShortDateString();
        public string BQDate
        {
            get
            {
                return p_BQDate;
            }
            set
            {
                p_BQDate = value;
            }
        }

        private string p_CallFrom = String.Empty;
        public string CallFrom
        {
            get
            {
                return p_CallFrom;
            }
            set
            {
                p_CallFrom = value;
            }
        }

        private string p_SearchSrting = String.Empty;
        public string SearchSrting
        {
            get
            {
                return p_SearchSrting;
            }
            set
            {
                p_SearchSrting = value;
            }
        }

        private int p_productId = -1;
        public int ProductId
        {
            get
            {
                return p_productId;
            }
            set
            {
                p_productId = value;
            }
        }

        private int p_prebooks_Id = -1;
        public int PrebooksId
        {
            get
            {
                return p_prebooks_Id;
            }
            set
            {
                p_prebooks_Id = value;
            }
        }
    }
}
