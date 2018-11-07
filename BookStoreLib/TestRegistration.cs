using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;

namespace BookStoreLib
{
    [TestClass]
    public class TestRegistration
    {
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
            string 
                username = "newUser",
                password = "nu1234";

            User sampleUser = new User("newUser", "John", "Doe", "newUser@gmail.com", "2656546565");

            // Expected outputs for register()
            Boolean expectedRegisterReturn = true;
            // Can't assert on ID unless we have a clean DB

            // Create a fresh user, execute register method
            User newUser = null;
            bool actualRegisterReturn;
            try
            {
                newUser = Account.Register(sampleUser, password);
                actualRegisterReturn = (newUser == null) ? false : true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                actualRegisterReturn = false;
            }

            // Assert on results: if actualRegisterReturn is true, it means that Registration was successful
            Assert.AreEqual(expectedRegisterReturn, actualRegisterReturn);

            // Expected outputs for subsequent login()
            Boolean expectedLoginReturn = true;
            int expectedUserId = newUser.Id;

            // Create a fresh user, execute register method
            Account.Login(username, password);

            // Assert login kept the same user.
            Assert.AreEqual(expectedLoginReturn, Account.IsLoggedIn);
            Assert.AreEqual(expectedUserId, Account.currentUser.Id);

            // Clean up registered user, or test will fail next time.
            // TODO: We should reset database on each test run, see TestInitialize
            DeleteUser(newUser.Id);
        }

        [TestMethod]
        public void RegisterUniqueEmail()
        {
            // Inputs
            string password = "ns1234";

            User sampleUser = new User("newUser", "John", "Doe", "shaverz@uwindsor.ca", "2656546565");

            // Expected outputs
            User expectedReturn = null;

            // Create a fresh user, execute register method
            User testUser = Account.Register(sampleUser, password);

            // Assert on results
            Assert.AreEqual(expectedReturn, testUser);
            Assert.IsTrue(Account.ErrorMessages.Contains("Username or email already exists in database"));
        }

        [TestMethod]
        public void RegisterUniqueUsername()
        {
            // Inputs
            string password = "ns1234";

            User sampleUser = new User("shaverz", "John", "Doe", "newUser@gmail.com", "2656546565");

            // Expected outputs
            User expectedReturn = null;

            // Create a fresh user, execute register method
            User testUser = Account.Register(sampleUser, password);

            // Assert on results
            Assert.AreEqual(expectedReturn, testUser);
            Assert.IsTrue(Account.ErrorMessages.Contains("Username or email already exists in database"));
        }

        [TestMethod]
        public void RegisterShortPassword()
        {
            // Inputs
            string password = "ns12";

            User sampleUser = new User("newUser", "John", "Doe", "newUser@gmail.com", "2656546565");

            // Expected outputs
            User expectedReturn = null;

            // Create a fresh user, execute login method
            User testUser = Account.Register(sampleUser, password);

            // Assert on results
            Assert.AreEqual(expectedReturn, testUser);
            Assert.IsTrue(Account.ErrorMessages.Contains("Password must be at least 6 characters."));
        }

        [TestMethod]
        public void RegisterPasswordStartWithLetter()
        {
            // Inputs
            string password = "1234rs";

            User sampleUser = new User("newUser", "John", "Doe", "newUser@gmail.com", "2656546565");

            // Expected outputs
            User expectedReturn = null;

            // Create a fresh user, execute login method
            User testUser = Account.Register(sampleUser, password);

            // Assert on results
            Assert.AreEqual(expectedReturn, testUser);
            Assert.IsTrue(Account.ErrorMessages.Contains("Password must start with an alphabetical character."));
        }

        [TestMethod]
        public void RegisterPasswordContainsOnlyAlphaNumberic()
        {
            // Inputs
            string password = "ns1234@";

            User sampleUser = new User("newUser", "John", "Doe", "newUser@gmail.com", "2656546565");

            // Expected outputs
            User expectedReturn = null;

            // Create a fresh user, execute register method
            User testUser = Account.Register(sampleUser, password);

            // Assert on results
            Assert.AreEqual(expectedReturn, testUser);
            Assert.IsTrue(Account.ErrorMessages.Contains("Password must only contain alpha numeric characters."));
        }
    }
}
