using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLib
{
     /**
     * Data access object for User table 
     */
     public class DALUser
     {
          public int Login(string username, string password)
          {
               var conn = new SqlConnection(Properties.Settings.Default.ZSDatabaseConnectionString);
               try
               {
                    SqlCommand loginSQL = new SqlCommand
                    {
                         Connection = conn,
                         CommandText = "Select Id from [User] where "
                         + "Username = @Username and Password = @Password "
                    };
                    loginSQL.Parameters.AddWithValue("@Username", username);
                    loginSQL.Parameters.AddWithValue("@Password", password);
                    conn.Open();
                    int id = (int)loginSQL.ExecuteScalar();
                    return id < 0 ? -1 : id;
               }
               catch (Exception e)
               {
                    Console.WriteLine(e.ToString());
                    return -1;
               }
               finally
               {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                         conn.Close();
                    }
               }
          }
     }
}