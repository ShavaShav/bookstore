using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BookStoreLib
{
    //shows the order/purchase history for the logged in user
    public class PurchaseHistory
    {
        ObservableCollection<PurchaseObj> purchaseHistoryList;
        public PurchaseHistory()
        {
            purchaseHistoryList = new ObservableCollection<PurchaseObj>();
        }
        public ObservableCollection<PurchaseObj> PurchaseHistoryList
        {
            get { return purchaseHistoryList; }
        }
        public void GetPurchaseInfo(string Username)
        {
            List<int> orderIDs = new List<int>();
            //Queries order/purchase history for the logged in user
            //get order ids for logged in user
            string orderIDsQuery = "SELECT OrderID FROM [Orders]" +
                              "INNER JOIN [User] ON [Orders].UserID = [User].ID WHERE " +
                              "Username = @Username";
            //loop through order ids to get order items associated with the logged in user
            string orderItemsQuery = "SELECT OrderItem.OrderID, OrderDate, [Status], ISBN, Quantity " +
                                "FROM [Orders] INNER JOIN [OrderItem] ON Orders.OrderID = OrderItem.OrderID "+
                                "WHERE OrderItem.OrderID = @orderID;";
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.DatabaseConnectionString))
            {
                SqlCommand command = new SqlCommand(orderIDsQuery, conn);
                command.Parameters.AddWithValue("@Username",Username);
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        orderIDs.Add((int)(reader["OrderID"]));     
                    }
                    conn.Close();
                    using (SqlConnection conn2 = new SqlConnection(Properties.Settings.Default.DatabaseConnectionString))
                    {
                        SqlCommand command2 = new SqlCommand(orderItemsQuery, conn2);
                        conn2.Open();
                        SqlDataReader reader2;
                        try
                        {
                            foreach (int orderID in orderIDs)
                            {
                                command2.Parameters.AddWithValue("@orderID", orderID);
                                using (reader2 = command2.ExecuteReader())
                                {
                                    while (reader2.Read())
                                    {
                                        PurchaseObj purchaseObj = new PurchaseObj(reader2["OrderID"].ToString(),
                                                                                  reader2["OrderDate"].ToString(),
                                                                                  reader2["ISBN"].ToString(),
                                                                                  reader2["Quantity"].ToString(),
                                                                                  reader2["Status"].ToString());
                                        purchaseHistoryList.Add(purchaseObj);
                                    }
                                    command2.Parameters.Clear();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e);
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
        }

    }
}