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
                Boolean expectedLoggedIn = true;
                int expectedUserId = 0;

                // Create a fresh user, execute login method
                User testUser = User.Login(username, password);

                // Assert on results
                Assert.AreEqual(testUser.IsLoggedIn, expectedLoggedIn);
                Assert.AreEqual(expectedUserId, testUser.Id); // id
               Assert.AreEqual(username, testUser.Username); // username
               Assert.AreEqual(password, testUser.Password); // password
          }

          // Test Case #5
          [TestMethod]
          public void LoginWrongPassword()
          {
               // Inputs
               string username = "shaverz",
                    password = "rs1234";

               // Expected outputs
               Boolean expectedLoggedIn = false;
               int expectedUserId = -1;

               // Create a fresh user, execute login method
               User testUser = User.Login(username, password);

               // Assert on results
               Assert.AreEqual(testUser.IsLoggedIn, expectedLoggedIn);
               Assert.AreEqual(expectedUserId, testUser.Id); // id
               Assert.IsTrue(testUser.ErrorMessages.Contains("Incorrect username or password."));
          }

          // Test Case #6
          [TestMethod]
          public void LoginShortPassword()
          {
               // Inputs
               string username = "shaverz",
                    password = "zs12";

            // Expected outputs
            Boolean expectedLoggedIn = false;
            int expectedUserId = -1;

            // Create a fresh user, execute login method
            User testUser = User.Login(username, password);

            // Assert on results
            Assert.AreEqual(testUser.IsLoggedIn, expectedLoggedIn);
            Assert.AreEqual(expectedUserId, testUser.Id); // id
               Assert.IsTrue(testUser.ErrorMessages.Contains("Password must be at least 6 characters."));
          }

          // Test Case #7
          [TestMethod]
          public void LoginPasswordStartWithLetter()
          {
               // Inputs
               string username = "shaverz",
                    password = "1234rs";

            // Expected outputs
            Boolean expectedLoggedIn = false;
            int expectedUserId = -1;

            // Create a fresh user, execute login method
            User testUser = User.Login(username, password);

            // Assert on results
            Assert.AreEqual(testUser.IsLoggedIn, expectedLoggedIn);
            Assert.AreEqual(expectedUserId, testUser.Id); // id
               Assert.IsTrue(testUser.ErrorMessages.Contains("Password must start with an alphabetical character."));
          }

          // Test Case #8
          [TestMethod]
          public void LoginPasswordContainsOnlyAlphaNumberic()
          {
               // Inputs
               string username = "shaverz",
                    password = "zs1234@";

            // Expected outputs
            Boolean expectedLoggedIn = false;
            int expectedUserId = -1;

            // Create a fresh user, execute login method
            User testUser = User.Login(username, password);

            // Assert on results
            Assert.AreEqual(testUser.IsLoggedIn, expectedLoggedIn);
            Assert.AreEqual(expectedUserId, testUser.Id); // id
               Assert.IsTrue(testUser.ErrorMessages.Contains("Password must only contain alpha numeric characters."));
          }
     }
}
