using System;
using System.Data.SqlClient;

namespace BookStoreLib
{
    /**
    * Data access object for User table 
    */
    public class DALUser
    {
        private SqlCommand RegisterSQL = new SqlCommand
        {
            CommandText = "INSERT INTO [User] VALUES ("
                     + "@Username, @Password, @FirstName, @LastName, @Email, @Phone, @AddressLine1, @AddressLine2, @City, @Province, @PostalCode "
        };

        private SqlCommand loginSQL = new SqlCommand
        {
            CommandText = "Select * from [User] where "
                     + "Username = @Username and Password = @Password "
        };

        private static string connString = Properties.Settings.Default.DatabaseConnectionString;

        // Store user data redudantly in this layer (talked with Prof Yuan about this)

        public User Login(string username, string password)
        {

            var conn = new SqlConnection(connString);
            try
            {
                loginSQL.Connection = conn;
                loginSQL.Parameters.AddWithValue("@Username", username);
                loginSQL.Parameters.AddWithValue("@Password", password);
                conn.Open();
                var reader = loginSQL.ExecuteReader();

                if (reader.Read())
                {
                    User user = new User(
                        (string)reader["Username"],
                        (string)reader["FirstName"],
                        (string)reader["LastName"],
                        (string)reader["Email"],
                        (string)reader["Phone"],
                        (string)reader["AddressLine1"],
                        (string)reader["AddressLine2"],
                        (string)reader["City"],
                        (string)reader["Province"],
                        (string)reader["PostalCode"]);
                    user.setUserId((int)reader["Id"]);
                    return user;
                }
                else
                {
                    Account.ErrorMessages.Clear();
                    Account.ErrorMessages.Add("Invalid username/password");
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public bool Register(User user, string password)
        {
            var conn = new SqlConnection(connString);
            try
            {
                RegisterSQL.Connection = conn;
                RegisterSQL.Parameters.AddWithValue("@Username", user.Username);
                RegisterSQL.Parameters.AddWithValue("@Password", password);
                RegisterSQL.Parameters.AddWithValue("@FirstName", user.FirstName);
                RegisterSQL.Parameters.AddWithValue("@LastName", user.LastName);
                RegisterSQL.Parameters.AddWithValue("@Email", user.Email);
                RegisterSQL.Parameters.AddWithValue("@Phone", user.Phone);
                RegisterSQL.Parameters.AddWithValue("@AddressLine1", user.AddressLine1);
                RegisterSQL.Parameters.AddWithValue("@AddressLine2", user.AddressLine2);
                RegisterSQL.Parameters.AddWithValue("@City", user.City);
                RegisterSQL.Parameters.AddWithValue("@Province", user.Province);
                RegisterSQL.Parameters.AddWithValue("@PostalCode", user.PostalCode);

                conn.Open();
                var writer = RegisterSQL.ExecuteNonQuery();
                if (writer.Equals(1))
                    return true;
                else return false;
            }
            catch (Exception e)
            {
                Account.ErrorMessages.Add("Account creation failed due to database error");
                Console.Error.WriteLine(e);
                return false;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
    }
}