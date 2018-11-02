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
        private static string connString = Properties.Settings.Default.DatabaseConnectionString;
        

        // Store user data redudantly in this layer (talked with Prof Yuan about this)
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string FullName { get; private set; }
        public string Type { get; private set; }
        public bool IsManager { get; private set; }

        public bool Login(string username, string password)
        {

            var conn = new SqlConnection(connString);
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
                    Id = (int)reader["Id"];
                    Username = (string)reader["Username"];
                    Password = (string)reader["Password"];
                    FullName = (string)reader["FullName"];
                    Type = (string)reader["Type"];
                    IsManager = (bool)reader["Manager"];
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
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