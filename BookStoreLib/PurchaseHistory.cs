using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLib
{
    //shows the order/purchase history for the logged in user
    public class PurchaseHistory
    {
        //SqlConnection conn; 
        public PurchaseHistory()
        {
            //sql database connection object
            //conn = new SqlConnection(Properties.Settings.Default.DatabaseConnectionString);
        }
        public void GetPurchaseInfo(string Username)
        {
            /*try
            {
                //Queries order/purchase history for the logged in user
                //get order ids for logged in user
                string orderIDs = "SELECT OrderID FROM[dbo].[Orders]"+
                                  "JOIN[dbo].[User] ON Orders.UserID = [User].ID" +
                                  "WHERE Username = 'shaverz'";
                //loop through order ids to get order items associated with the logged in user
                string orderItems = "SELECT OrderItem.OrderID, OrderDate, [Status], ISBN, Quantity" +
                                    "FROM [dbo].[Orders] JOIN [dbo].[OrderItem] ON Orders.OrderID = OrderItem.OrderID"+
                                    "WHERE OrderItem.OrderID = 1;";
                //bind to the UI
                //SqlCommand cmdPurchaseHis = new SqlCommand(SqlQueryPurchaseHistory, conn);
                //SqlDataAdapter daPurchaseHis = new SqlDataAdapter(cmdPurchaseHis);
                //DataTable dt = new DataTable("Orders");
                //daPurchaseHis.Fill(dt);
                //return dt; return eventually DataTable object

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e); 
                /*return null;
            }*/
        }

    }
}
