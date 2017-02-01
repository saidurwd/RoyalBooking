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
        public void UpdatePrebookDeleteStatus(DC_BQ objBQ, Prebooks objPB, string DeleteOrMove, string IsSuccess, string NewPrebookId, string NewTruckDate, string _BatchNo)
        {
            UpdatePrebookDeleteStatusById(objBQ, objPB, DeleteOrMove, IsSuccess, NewPrebookId, NewTruckDate, _BatchNo);
        }
        
        private void UpdatePrebookDeleteStatusById(DC_BQ objBQ, Prebooks objPB, string DeleteOrMove, string IsSuccess, string NewPrebookId, string NewTruckDate, string _BatchNo)
        {
            string strSQL = @"
                UPDATE " + objBQ.DataFrom + "_PB_PO_Details SET DeleteStatus=" + IsSuccess + " WHERE prebookItemId=" + objBQ.ProductId + @";
                INSERT INTO dbo.PB_PO_Details_Delete_Move_log(orderType, stemsBunch, notes, customField1, standingOrder, details_Id, bunches, units, totalBoxes, unitType, carrierName, referenceNumber, poItemId, customerId, lineItemStatus, productDescription, 
                         productId, boxType, customerName, totalUnits, markCode, prebook, unitCost, prebookTruckDate, quantityConfirmed, 
                        carrierId, totalCost, purchaseOrders_Id, prebookItemId, PO_number, id_PO, DeleteStatus, [source], truckDate, 
                        DeleteOrMove, IsSuccess, NewPrebookId, NewTruckDate, IsNew, BatchNo, InvoiceNumber, customerIdPB, customerPoNumber, comments, shipToId, unitPrice, markCodePB, pbQty)
                        SELECT orderType, stemsBunch, notes, customField1, standingOrder, details_Id, bunches, units, totalBoxes, unitType, carrierName, referenceNumber, poItemId, customerId, lineItemStatus, productDescription, 
                         productId, boxType, customerName, totalUnits, markCode, prebook, unitCost, prebookTruckDate, quantityConfirmed, 
                        carrierId, totalCost, purchaseOrders_Id, prebookItemId, PO_number, id_PO, " + IsSuccess + @", '" + objBQ.DataFrom + "',convert(datetime,'" + objBQ.ProcessDay + @"'),
                        '" + DeleteOrMove + "', " + IsSuccess + ", '" + NewPrebookId + "', '" + NewTruckDate + @"', 1, '"+ _BatchNo + @"',
                        '" + objBQ.InvoiceNumber + "','" + objPB.customerId + "','" + objPB.customerPoNumber + "','" + objPB.comments + @"',
                        '" + objPB.shipToId + "','" + objPB.unitPrice + "','" + objPB.markCode + "', " + objPB.pbQty + @"
                        FROM " + objBQ.DataFrom + "_PB_PO_Details WHERE prebookItemId=" + objBQ.ProductId + @"";
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
        public void UpdatePrebookDeleteStatusAll()
        {
            UpdatePrebookDeleteStatus();
        }
        private void UpdatePrebookDeleteStatus()
        {
            string strSQL = @"UPDATE PB_PO_Details_Delete_Move_log SET IsNew=0;";
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

