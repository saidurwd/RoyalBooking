using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BQ
{
    public class UpdateKSPrebook
    {
        public void UpdatePrebookDeleteStatus(DC_BQ objBQ)
        {
            UpdatePrebookDeleteStatusById(objBQ);
        }
        
        private void UpdatePrebookDeleteStatusById(DC_BQ objBQ)
        {
            string strSQL = @"UPDATE " + objBQ.DataFrom + "_KS_PrebooksDetails SET DeleteStatus=1 WHERE prebookItemId=" + objBQ.ProductId+ @"
                INSERT INTO dbo.Prebook_Delete_Log(PrebookId,PrebookItemId,number,productDescription, [source], truckDate) VALUES(
                "+ objBQ.PrebooksId + ", "+ objBQ.ProductId + ", '"+ objBQ.InvoiceNumber + "', '"+ objBQ.SearchSrting + "','"+ objBQ.DataFrom + "',convert(datetime,'" + objBQ.ProcessDay + @"', " + BQ.DB_Base.BQDataRegion + @")
            )
            ";

            string constr = DB_Base.DB_STR;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            object objResult = null;

            SqlCommand dbCommand = adpt.SelectCommand;
            dbCommand.CommandTimeout = 0;
            dbCommand.CommandText = strSQL;
            dbCommand.CommandType = CommandType.Text;

            objResult = dbCommand.ExecuteScalar();


        }
    }
}

