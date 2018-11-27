using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookStoreLib
{

    [TestClass]
    public class TestLogin
    {

        [TestInitialize]
        public void SetUp()
        {
            // Set up logic
            // TODO: Delete test database and run the sql setup script here,
            // so that we have a fresh instance of the database for each test.
            // Create the test user that we will assert our login methods on.
        }

        [TestCleanup]
        public void TearDown()
        {
            // Tear down logic
        }

        // Test Case #1
        [TestMethod]
        public void LoginSuccess()
        {
            // Inputs
            string username = "shaverz",
                 password = "zs1234";

            // Expected outputs
            Boolean expectedReturn = true;
            int expectedUserId = 1;

            // Create a fresh user, execute login method
            Account.Login(username, password);

            // Assert on results
            Assert.AreEqual(expectedReturn, Account.IsLoggedIn);
            Assert.AreEqual(expectedUserId, Account.currentUser.Id); // id
            Assert.AreEqual(username, Account.currentUser.Username); // username
        }

        // Test Case #5
        [TestMethod]
        public void LoginWrongPassword()
        {
            // Inputs
            string username = "shaverz",
                 password = "rs1234";

            // Expected outputs
            Boolean expectedReturn = false;

            // Create a fresh user, execute login method
            Account.Login(username, password);

            // Assert on results
            Assert.AreEqual(expectedReturn, Account.IsLoggedIn);
            Assert.IsTrue(Account.ErrorMessages.Contains("Invalid username/password"));
        }

        // Test Case #6
        [TestMethod]
        public void LoginShortPassword()
        {
            // Inputs
            string username = "shaverz",
                 password = "zs12";

            // Expected outputs
            Boolean expectedReturn = false;

            // Create a fresh user, execute login method
            Account.Login(username, password);

            // Assert on results
            Assert.AreEqual(expectedReturn, Account.IsLoggedIn);
            Assert.IsTrue(Account.ErrorMessages.Contains("Password must be at least 6 characters."));
        }

        // Test Case #7
        [TestMethod]
        public void LoginPasswordStartWithLetter()
        {
            // Inputs
            string username = "shaverz",
                 password = "1234rs";

            // Expected outputs
            Boolean expectedReturn = false;

            // Create a fresh user, execute login method
            Account.Login(username, password);

            // Assert on results
            Assert.AreEqual(expectedReturn, Account.IsLoggedIn);
            Assert.IsTrue(Account.ErrorMessages.Contains("Password must start with an alphabetical character."));
        }

        // Test Case #8
        [TestMethod]
        public void LoginPasswordContainsOnlyAlphaNumberic()
        {
            // Inputs
            string username = "shaverz",
                 password = "zs1234@";

            // Expected outputs
            Boolean expectedReturn = false;

            // Create a fresh user, execute login method
            Account.Login(username, password);

            // Assert on results
            Assert.AreEqual(expectedReturn, Account.IsLoggedIn);
            Assert.IsTrue(Account.ErrorMessages.Contains("Password must only contain alpha numeric characters."));
        }
    }
}
