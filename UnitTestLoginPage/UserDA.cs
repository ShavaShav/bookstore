using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestLoginPage
{
     public class UserDA
     {
          public int Login(string username, string password)
          {
               var conn = new SqlConnection(Properties.Settings.Default.ZSDatabaseConnectionString);
               try
               {
                    SqlCommand loginSQL = new SqlCommand();
                    loginSQL.Connection = conn;
                    loginSQL.CommandText = "Select Id from [User] where "
                         + "Username = @Username and Password = @Password ";
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