using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BQ
{
    public class ReadKSData
    {
        public DataSet ReadInvoiceData()
        {
            DataSet ds = GetInvoiceQueryResult();
            return ds;
        }
        public DataSet ReadMissingInvoiceData()
        {
            DataSet ds = GetMissingInvoiceQueryResult();
            return ds;
        }
        public DataSet ReadVendorData()
        {
            DataSet ds = GetVendorQueryResult();
            return ds;
        }
        public DataSet ReadCreditData()
        {
            DataSet ds = GetCreditQueryResult();
            return ds;
        }
        public DataSet ReadKSPrebooks(DC_BQ objBQ)
        {
            DataSet ds = GetPrebooksQueryResult(objBQ);
            return ds;
        }
        public DataSet GetInvoiceQueryResult()
        {
            string strSQL = @"SELECT     
                                    total, shipCity, isnull(countConfirmed,0) countConfirmed, billZipCode, shipZipCode, locationId, shipName, 
                                    CONVERT(VARCHAR(30), shipDate, 121) shipDate, locationName, billAddress, id, billCountry, customerId, 
                                    customerPONumber, isnull(invoices_Id,0) invoices_Id, isEcommerce, salesPersonName, carrierId, customerName, additionalCharges, status, shipState, isnull(salesPersonId,0) salesPersonId, shipCountry, 
                                    number invoice_number, subtotal, billCity, createdOn, carrierName, totalBoxes, shipAddress, taxes, billState, orderRef, rootNode_Id, system_name
                             FROM         invoices;
                             SELECT 
                                    * 
                             FROM additionalChargesDetails;
                             SELECT  
                                    awb, details_Id, boxType, ISNULL(stemsBunch, 0) AS stemsBunch, unitType, productDescription, unitPrice, units, productId, boxTypeId, REPLACE(isnull(amount,0), ',', '')  amount, totalUnits, 
                                    growerId, salesType, ref, bunchesBox, totalBoxes, growerName, notes, markCode, detailId, invoices_Id, invoice_number, ID_INVOICE
                                    ,isnull(brandId,0) brandId, isnull(brandName,'') brandName, system_name
                             FROM details;
                             SELECT 
                                    * 
                             FROM boxes;
                             SELECT 
                                    * 
                             FROM breakdowns;";
            string constr = DB_Base.DB_STR;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            adpt.Dispose();
            con.Close();
            con.Dispose();
            return ds;

        }
        public DataSet GetMissingInvoiceQueryResult()
        {
            string strSQL = @"SELECT     
                                total, shipCity, isnull(countConfirmed,0) countConfirmed, billZipCode, shipZipCode, locationId, shipName, 
                                CONVERT(VARCHAR(30), shipDate, 121) shipDate, locationName, billAddress, id, billCountry, customerId, 
                                customerPONumber, isnull(invoices_Id,0) invoices_Id, isEcommerce, salesPersonName, carrierId, customerName, additionalCharges, status, shipState, isnull(salesPersonId,0) salesPersonId, shipCountry, 
                                number invoice_number, subtotal, billCity, createdOn, carrierName, totalBoxes, shipAddress, taxes, billState, orderRef, rootNode_Id, system_name
                                FROM         invoices
                                where isnull(isItNew,0)=1;
                             SELECT 
                                A.* FROM additionalChargesDetails A inner join invoices I on I.number = A.invoice_number where isnull(I.isItNew,0)=1;;
                             SELECT  
                                awb, details_Id, boxType, ISNULL(stemsBunch, 0) AS stemsBunch, unitType, productDescription, unitPrice, units, productId, boxTypeId, REPLACE(isnull(amount,0), ',', '')  amount, totalUnits, 
                                growerId, salesType, ref, bunchesBox, D.totalBoxes, growerName, notes, markCode, detailId, D.invoices_Id, invoice_number, ID_INVOICE
                                ,isnull(brandId,0) brandId, isnull(brandName,'') brandName, system_name
                                FROM details D inner join invoices I on I.number = D.invoice_number where isnull(I.isItNew,0)=1;;
                             SELECT 
                                B.* FROM boxes B inner join invoices I on I.number = B.invoice_number where isnull(I.isItNew,0)=1;;
                             SELECT 
                                B.* FROM breakdowns B inner join invoices I on I.number = B.invoice_number where isnull(I.isItNew,0)=1;
                            SELECT 
	                            I.number invoice_number, I.customerName, I.totalBoxes, B.totalBoxes,
	                            I.subtotal, B.subtotal,
	                            I.additionalCharges, B.additionalCharges,
	                            I.total, B.total,
	                            case 
		                            when isChangesInOrder=1 then
	                                'Change in Total Boxes - Old value: ' + convert(varchar(20),B.totalBoxes) + '  New Value: ' + convert(varchar(20),I.totalBoxes)
	                                when isChangesInOrder=2 then
	                                'Change in Sub Total - Old value: ' + convert(varchar(20),B.subtotal) + '  New Value: ' + convert(varchar(20),I.subtotal)
	                                when isChangesInOrder=3 then
	                                'Change in Additional Charges - Old value: ' + convert(varchar(20),B.additionalCharges) + '  New Value: ' + convert(varchar(20),I.additionalCharges)
	                                when isChangesInOrder=4 then
	                                'Change in Total - Old value: ' + convert(varchar(20),B.total) + '  New Value: ' + convert(varchar(20),I.total)
	                                else
			                            ''
	                            end Reason
                                FROM dbo.BQInvoice B, invoices I
                                where I.number=B.invoice_number and isnull(isChangesInOrder,0)<>0	 ;


                ";
            string constr = DB_Base.DB_STR;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            adpt.Dispose();
            con.Close();
            con.Dispose();
            return ds;

        }

        public DataSet GetVendorQueryResult()
        {

            string strSQL = @"
                             SELECT * FROM vendors;
                                ";
            string constr = DB_Base.DB_STR;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            adpt.Dispose();
            con.Close();
            con.Dispose();
            return ds;

        }

        public DataSet GetCreditQueryResult()
        {

            string strSQL = @"
                             SELECT     
                                id, number as invoice_number, isnull(customerId,0) customerId, customerName, 
                                CONVERT(VARCHAR(30), shipDate, 121) shipDate, 
                                carrierId, carrierName, isnull(salesPersonId,0) salesPersonId, 
                                salesPersonName, locationName, customerPONumber, 
                                isnull(totalBoxesOrder,0) totalBoxesOrder, isnull(totalOrder,0) totalOrder, 
                                isnull(totalCredit,0) totalCredit, isnull(returnedTax,0) returnedTax, credits_Id, 
                                rootNode_Id, isnull(locationId,0) locationId, status, system_name
                            FROM         
                                creditHeader;
                            SELECT    
                                detailId, awb, productId, productDescription, growerId, growerName, boxTypeId, boxType, 
                                isnull(quantityBoxes,0) quantityBoxes, isnull(flowerSales,0) flowerSales, 
                                isnull(creditUnits,0) creditUnits, isnull(creditAmount,0) creditAmount, 
                                isnull(creditFreight,0) creditFreight, creditReasons, credits_Id, invoice_number, status, system_name
                            FROM 
                                creditDetails;
                            SELECT    
                                comment, createdBy, CONVERT(VARCHAR(30), createdOn, 121) createdOn, credits_Id, invoice_number, status, system_name
                            FROM         
                                creditComments;
                                ";
            string constr = DB_Base.DB_STR;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            adpt.Dispose();
            con.Close();
            con.Dispose();
            return ds;

        }
        public DataTable GetCreditQueryInvoiceList()
        {

            string strSQL = @"
                             SELECT     
                                distinct number as invoice_number
                            FROM         
                                creditHeader;
                                ";
            string constr = DB_Base.DB_STR;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            adpt.Dispose();
            con.Close();
            con.Dispose();
            return dt;

        }
        private DataSet GetPrebooksQueryResult(DC_BQ objBQ)
        {
            string strSQL = @"
                            SELECT P.number, P.truckDate, P.customerName
                            FROM dbo." + objBQ.DataFrom + @"_KS_Prebooks P
                            INNER JOIN dbo." + objBQ.DataFrom + @"_KS_PrebooksDetails D on P.prebooks_Id=D.prebooks_Id
                            where D.productDescription like '%" + objBQ.SearchSrting.Replace("'", "'") +
                            "%' AND truckDate >= convert(datetime,'" + objBQ.FromDate + @"', " + BQ.DB_Base.BQDataRegion +
                            @") 
                                and truckDate <= convert(datetime,'" + objBQ.ToDate + @"', " + BQ.DB_Base.BQDataRegion +
                            @") ";

            string constr = DB_Base.DB_STR;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            adpt.Dispose();
            con.Close();
            con.Dispose();
            return ds;

        }
        public DataSet GetColumnName()
        {

            string strSQL = @"
            select top 1 * from invoices;
            select top 1 * from details;
            select top 1 * from boxes;
            select top 1 * from breakdowns;
            select top 1 * from additionalChargesDetails;
                                ";
            string constr = DB_Base.DB_STR;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            adpt.Dispose();
            con.Close();
            con.Dispose();
            return ds;

        }
        public DataSet GetPOColumnName()
        {

            string strSQL = @"
            select top 1 * from PO_PurchaseOrders;
            select top 1 * from PO_details;
            select top 1 * from PO_boxes;
            select top 1 * from PO_breakdowns;
            select top 1 * from PO_CustomFields;
                                ";
            string constr = DB_Base.DB_STR;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            adpt.Dispose();
            con.Close();
            con.Dispose();
            return ds;

        }
        public DataSet GetPrebooksColumnName()
        {
            string strSQL = @"
            select top 1 * from KS_Prebooks;
            select top 1 * from KS_PrebooksDetails;
            select top 1 * from KS_PrebooksBreakDowns;";
            string constr = DB_Base.DB_STR;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            adpt.Dispose();
            con.Close();
            con.Dispose();
            return ds;

        }
        public DataSet GetColumnNameCredit()
        {

            string strSQL = @"
            select top 1 * from creditHeader;
            select top 1 * from creditDetails;
            select top 1 * from creditComments;";
            string constr = DB_Base.DB_STR;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            adpt.Dispose();
            con.Close();
            con.Dispose();
            return ds;

        }
        public DataSet GetColumnNameVendorAvailability()
        {

            string strSQL = @"
            select top 1 * from VA_inventoryItems;";
            string constr = DB_Base.DB_STR;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            adpt.Dispose();
            con.Close();
            con.Dispose();
            return ds;
        }
        public DataTable GetNewDuplicates()
        {
            string strSQL = @"
                             SELECT number 'Order#', [source] 'Company', old_truckDate 'Old Ship Date', new_truckDate 'New Ship Date', insert_date 'Process On' from Prebooks_duplicates where status=1;
                                ";
            string constr = DB_Base.DB_STR;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(strSQL, con);
            DataSet ds = new DataSet();
            adpt.Fill(ds);
            adpt.Dispose();
            con.Close();
            con.Dispose();
            return ds.Tables[0];

        }
    }
}

