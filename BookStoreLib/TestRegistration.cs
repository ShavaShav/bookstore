using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;

namespace BookStoreLib
{
    [TestClass]
    public class TestRegistration
    {
        // Sample inputs for Register() function, since it takes a lot of parameters
        string sampleUsername = "jdoe";
        string samplePassword = "pwd12345";
        string sampleFirstName = "John";
        string sampleLastName = "Doe";
        string sampleEmail = "jdoe@example.com";
        string samplePhone = "2656546565";
        string sampleAddressLine1 = "123 Address Street";
        string sampleAddressLine2 = null;
        string sampleCity = "Windsor";
        string sampleProvince = "ON";
        string samplePostalCode = "A1B2C3";

        [TestInitialize]
        public void SetUp()
        {
            // Set up logic
            // TODO: Delete test database and run the sql setup script here,
            // so that we have a fresh instance of the database for each test.
            // For now, deleting user after asserting created, so db is brought
            // back to initial state on completion.
        }

        public void DeleteUser(int userId)
        {
            try
            {
                using (var sc = new SqlConnection(Properties.Settings.Default.DatabaseConnectionString))
                using (var cmd = sc.CreateCommand())
                {
                    sc.Open();
                    cmd.CommandText = "DELETE FROM [User] WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void RegisterSuccess()
        {
            // Inputs
            string username = "newUser",
                 email = "newUser@gmail.com",
                 password = "nu1234";

            // Expected outputs for register()
            Boolean expectedRegisterReturn = true;
            // Can't assert on ID unless we have a clean DB

            // Create a fresh user, execute register method
            Account newUser = new Account();
            Boolean actualRegisterReturn = newUser.Register(
                username, password, sampleFirstName, sampleLastName, email,
                samplePhone, sampleAddressLine1, sampleAddressLine2, sampleCity,
                sampleProvince, samplePostalCode);

            // Assert on results
            Assert.AreEqual(expectedRegisterReturn, actualRegisterReturn);
            Assert.AreEqual(username, newUser.Username); // username
            Assert.AreEqual(email, newUser.Email);       // email
            Assert.AreEqual(password, newUser.Password); // password
            // TODO: Add other fields

            // Expected outputs for subsequent login()
            Boolean expectedLoginReturn = true;
            int expectedUserId = newUser.Id;

            // Create a fresh user, execute register method
            Account loginUser = new Account();
            Boolean loginReturn = loginUser.Login(username, password);

            // Assert login kept the same user.
            Assert.AreEqual(expectedLoginReturn, loginReturn);
            Assert.AreEqual(expectedUserId, loginUser.Id);

            // Clean up registered user, or test will fail next time.
            // TODO: We should reset database on each test run, see TestInitialize
            DeleteUser(newUser.Id);
        }

        [TestMethod]
        public void RegisterUniqueEmail()
        {
            // Inputs
            string username = "newUser",
                 email = "shaverz@uwindsor.ca", // Assumes shaverz@uwindsor.ca in db
                 password = "ns1234";

            // Expected outputs
            Boolean expectedReturn = false;
            int expectedUserId = -1;

            // Create a fresh user, execute register method
            Account testUser = new Account();
            Boolean actualReturn = testUser.Register(
                username, password, sampleFirstName, sampleLastName, email,
                samplePhone, sampleAddressLine1, sampleAddressLine2, sampleCity,
                sampleProvince, samplePostalCode);

            // Assert on results
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, testUser.Id);
            Assert.IsTrue(testUser.ErrorMessages.Contains("Password must only contain alpha numeric characters."));
        }

        [TestMethod]
        public void RegisterUniqueUsername()
        {
            // Inputs
            string username = "shaverz",  // Assumes shaverz in db
                 email = "newUser@gmail.com",
                 password = "ns1234";

            // Expected outputs
            Boolean expectedReturn = false;
            int expectedUserId = -1;

            // Create a fresh user, execute register method
            Account testUser = new Account();
            Boolean actualReturn = testUser.Register(
                username, password, sampleFirstName, sampleLastName, email,
                samplePhone, sampleAddressLine1, sampleAddressLine2, sampleCity,
                sampleProvince, samplePostalCode);

            // Assert on results
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, testUser.Id);
            Assert.IsTrue(testUser.ErrorMessages.Contains("Password must only contain alpha numeric characters."));
        }

        [TestMethod]
        public void RegisterShortPassword()
        {
            // Inputs
            string username = "newUser",
                 email = "newUser@gmail.com",
                 password = "ns12";

            // Expected outputs
            Boolean expectedReturn = false;
            int expectedUserId = -1;

            // Create a fresh user, execute login method
            Account testUser = new Account();
            Boolean actualReturn = testUser.Register(
                username, password, sampleFirstName, sampleLastName, email,
                samplePhone, sampleAddressLine1, sampleAddressLine2, sampleCity,
                sampleProvince, samplePostalCode);

            // Assert on results
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, testUser.Id);
            Assert.IsTrue(testUser.ErrorMessages.Contains("Password must be at least 6 characters."));
        }

        [TestMethod]
        public void RegisterPasswordStartWithLetter()
        {
            // Inputs
            string username = "newUser",
                 email = "newUser@gmail.com",
                 password = "1234rs";

            // Expected outputs
            Boolean expectedReturn = false;
            int expectedUserId = -1;

            // Create a fresh user, execute login method
            Account testUser = new Account();
            Boolean actualReturn = testUser.Register(
                username, password, sampleFirstName, sampleLastName, email,
                samplePhone, sampleAddressLine1, sampleAddressLine2, sampleCity,
                sampleProvince, samplePostalCode);

            // Assert on results
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, testUser.Id);
            Assert.IsTrue(testUser.ErrorMessages.Contains("Password must start with an alphabetical character."));
        }

        [TestMethod]
        public void RegisterPasswordContainsOnlyAlphaNumberic()
        {
            // Inputs
            string username = "newUser",
                 email = "newUser@gmail.com",
                 password = "ns1234@";

            // Expected outputs
            Boolean expectedReturn = false;
            int expectedUserId = -1;

            // Create a fresh user, execute register method
            Account testUser = new Account();
            Boolean actualReturn = testUser.Register(
                username, password, sampleFirstName, sampleLastName, email,
                samplePhone, sampleAddressLine1, sampleAddressLine2, sampleCity,
                sampleProvince, samplePostalCode);

            // Assert on results
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.AreEqual(expectedUserId, testUser.Id);
            Assert.IsTrue(testUser.ErrorMessages.Contains("Password must only contain alpha numeric characters."));
        }
    }
}
