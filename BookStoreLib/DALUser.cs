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
          //private static string connString = Properties.Settings.Default.ZSDatabaseConnectionString;
          private static string connString = Properties.Settings.Default.XYDatabaseConnectionString; // Use new DB provided for assignment 3

          public User Login(string username, string password)
          {

            var conn = new SqlConnection(connString);
            var user = new User();
            try
            {
                SqlCommand loginSQL = new SqlCommand
                {
                    Connection = conn,
                    CommandText = "Select * from [User] where "
                     + "Username = @Username and Password = @Password "
                };
                loginSQL.Parameters.AddWithValue("@Username", username);
                loginSQL.Parameters.AddWithValue("@Password", password);
                conn.Open();
                var reader = loginSQL.ExecuteReader();

                if (reader.Read())
                {
                    // Success, return the user model
                    user.Id = (int)reader["Id"];
                    user.Username = (string)reader["Username"];
                    user.Password = (string)reader["Password"];
                    user.FullName = (string)reader["FullName"];
                    user.Type = (string)reader["Type"];
                    user.IsManager = (bool)reader["Manager"];
                    user.IsLoggedIn = true;
                }
                else
                {
                    user.ErrorMessages.Add("Incorrect username or password.");
                }

            }
            catch (Exception e)
            {
                user.ErrorMessages.Add(e.ToString());
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return user;
        }
    }
}