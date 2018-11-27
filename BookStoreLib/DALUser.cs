using System;
using System.Collections.Generic;
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
            CommandText = "INSERT INTO [User] OUTPUT Inserted.Id VALUES ("
                     + "@Username, @Password, @FirstName, @LastName, @Email, @Phone )"
        };

        private SqlCommand loginSQL = new SqlCommand
        {
            CommandText = "Select * from [User] where "
                     + "Username = @Username and Password = @Password "
        };

        private SqlCommand uniquenessCheckSQL = new SqlCommand
        {
            CommandText = "Select * from [User] where "
                     + "Username = @Username or Email = @Email "
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
                        (string)reader["Phone"]);
                    user.Id = (int)reader["Id"];
                    user.Password = (string)reader["Password"];
                    return user;
                }
                else
                {
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

        // TODO, remove this function as we now have a UNIQUE constraint
        internal bool UsernameOrEmailExistsInDb(User user)
        {
            var conn = new SqlConnection(connString);
            try
            {
                uniquenessCheckSQL.Connection = conn;
                uniquenessCheckSQL.Parameters.AddWithValue("@Username", user.Username);
                uniquenessCheckSQL.Parameters.AddWithValue("@Email", user.Email);

                conn.Open();
                var reader = uniquenessCheckSQL.ExecuteReader();
                if (reader.HasRows)
                {
                    Account.ErrorMessages.Add("Username or email already exists in database");
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
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

        public User Register(User user, string password)
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

                conn.Open();
                user.Id = (int)RegisterSQL.ExecuteScalar();

                return user.Id > 0 ? user : null; 
                // caller will hold a modified version of user after this call. dangerous
            }
            catch (Exception e)
            {
                Account.ErrorMessages.Add("Account creation failed due to database error");
                Console.Error.WriteLine(e);
                return null;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        public bool Edit(int userId, string username, string password, string email, string firstName, string lastName, string phone)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                string queryString = "UPDATE [User] SET " +
                    "Username = @username, " +
                    "Password = @password, " +
                    "Email = @email, " +
                    "FirstName = @firstName, " +
                    "LastName = @lastName, " +
                    "Phone = @phone " +
                    "WHERE id = @id";

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@id", userId);
                command.Parameters.AddWithValue("@username", username.Trim());
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@email", email.Trim());
                command.Parameters.AddWithValue("@firstName", firstName.Trim());
                command.Parameters.AddWithValue("@lastName", lastName.Trim());
                command.Parameters.AddWithValue("@phone", phone.Trim());
                connection.Open();

                try
                {
                    var rowsUpdated = command.ExecuteNonQuery();
                    return rowsUpdated == 1;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    Account.ErrorMessages.Add("Username or email already exists.");
                    return false; // constraint failed
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}