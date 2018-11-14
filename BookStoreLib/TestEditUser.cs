using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;

namespace BookStoreLib
{
    [TestClass]
    public class TestEditUser
    {
        // Test user expectations for each test
        int testId = 2;
        string testUsername = "edit_user";
        string testPassword = "testpass1234";
        string testEmail = "edit@user.com";
        string testFirstName = "John";
        string testLastName = "Smith";
        string testPhone = "1231231234";

        [TestInitialize]
        public void SetUp()
        {
            // Reset the test user's fields for each test
            try
            {
                using (var sc = new SqlConnection(Properties.Settings.Default.DatabaseConnectionString))
                using (var cmd = sc.CreateCommand())
                {
                    sc.Open();
                    cmd.CommandText = "UPDATE [User] SET " + 
                        "Username = @username, " +
                        "Password = @password, " +
                        "Email = @email, " +
                        "FirstName = @firstName, " +
                        "LastName = @lastName, " +
                        "Phone = @phone " +
                        "WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", testId);
                    cmd.Parameters.AddWithValue("@username", testId);
                    cmd.Parameters.AddWithValue("@password", testId);
                    cmd.Parameters.AddWithValue("@email", testId);
                    cmd.Parameters.AddWithValue("@firstName", testId);
                    cmd.Parameters.AddWithValue("@lastName", testId);
                    cmd.Parameters.AddWithValue("@phone", testId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void EditUserSuccess()
        {
        }

        [TestMethod]
        public void EditUserLoggedOut()
        {
        }

        [TestMethod]
        public void EditUsernameNonUnique()
        {
        }

        [TestMethod]
        public void EditUsernameBlank()
        {
        }

        [TestMethod]
        public void EditPasswordBadStart()
        {
        }

        [TestMethod]
        public void EditPasswordNonAlphaNum()
        {
        }

        [TestMethod]
        public void EditPasswordShort()
        {
        }


        [TestMethod]
        public void EditPhoneInvalid()
        {
        }

        [TestMethod]
        public void EditPhoneBlank()
        {
        }

        [TestMethod]
        public void EditEmailNonUnique()
        {
        }

        [TestMethod]
        public void EditEmailInvalid()
        {
        }

        [TestMethod]
        public void EditEmailBlank()
        {
        }

        [TestMethod]
        public void EditFirstNameBlank()
        {
        }

        [TestMethod]
        public void EditLastNameBlank()
        {
        }
    }
}
