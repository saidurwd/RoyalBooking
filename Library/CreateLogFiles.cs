﻿using System;
using System.IO;

namespace BQ
{
    public class CreateLogFiles
    {
        //private string sLogFormat;
        private string sErrorTime;

        public CreateLogFiles()
        {
            //sLogFormat used to create log files format :
            // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
            //sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            //this variable used to create log filename format "
            //for example filename : ErrorLogYYYY.MM.DD
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            sErrorTime = sYear +"." + sMonth + "."+ sDay;
        }
        public void ErrorLog(string sPathName, string sErrMsg)
        {
            StreamWriter sw = new StreamWriter(sPathName + sErrorTime + ".log", true);
            //sw.WriteLine(sLogFormat + sErrMsg);
            sw.WriteLine(sErrMsg);
            sw.Flush();
            sw.Close();
        }

        
    }
}
