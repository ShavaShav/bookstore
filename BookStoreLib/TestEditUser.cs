using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;

namespace BookStoreLib
{
    [TestClass]
    public class TestEditUser
    {
        // Test user expectations for each test
        const int originalId = 2;
        const string originalUsername = "edit_user";
        const string originalPassword = "testpass1234";
        const string originalEmail = "edit@user.com";
        const string originalFirstName = "John";
        const string originalLastName = "Smith";
        const string originalPhone = "123 123 1234";

        // User object to be logged in on each test
        User testUser;

        [TestInitialize]
        public void SetUp()
        {
            // Reset the test user's db fields and the current user for each test
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
                    "WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", originalId);
                cmd.Parameters.AddWithValue("@username", originalUsername);
                cmd.Parameters.AddWithValue("@password", originalPassword);
                cmd.Parameters.AddWithValue("@email", originalEmail);
                cmd.Parameters.AddWithValue("@firstName", originalFirstName);
                cmd.Parameters.AddWithValue("@lastName", originalLastName);
                cmd.Parameters.AddWithValue("@phone", originalPhone);

                try
                {
                    cmd.ExecuteNonQuery();

                    Account.Login(originalUsername, originalPassword);
                    testUser = Account.currentUser;
                }
                catch (Exception e)
                {
                    Assert.Fail(e.Message);
                }
            }

        }

        private void AssertUserNotUpdated()
        {
            // Assert user fields are the same as original database entries
            Assert.AreEqual(testUser.Id, originalId);
            Assert.AreEqual(testUser.Username, originalUsername);
            Assert.AreEqual(testUser.Email, originalEmail);
            Assert.AreEqual(testUser.FirstName, originalFirstName);
            Assert.AreEqual(testUser.LastName, originalLastName);
            Assert.AreEqual(testUser.Phone, originalPhone);
        }

        [TestMethod]
        public void EditUserSuccess()
        {
            string newUsername = "modified_user";
            string newPassword = "newpass99";
            string newEmail = "new@test.ca";
            string newFirstName = "Jane";
            string newLastName = "Doe";
            string newPhone = "226 222 3333";

            bool output = Account.Edit(
                newUsername, newPassword, newEmail, newFirstName, newLastName, newPhone
            );

            // Should return TRUE
            Assert.IsTrue(output);

            // Unaltered fields should stay the same
            Assert.AreEqual(testUser.Id, originalId);

            // Current user should be updated
            Assert.AreEqual(testUser.Username, newUsername);
            Assert.AreEqual(testUser.Email, newEmail);
            Assert.AreEqual(testUser.FirstName, newFirstName);
            Assert.AreEqual(testUser.LastName, newLastName);
            Assert.AreEqual(testUser.Phone, newPhone);

            // Logout, try to log back in with new user
            Account.Logout();
            Assert.AreNotEqual(testUser.Username, newUsername); // quick sanity test

            Account.Login(newUsername, newPassword); // this also tests changed pass for free
            // since now held and modified staticly in Account class, we need to re-get the updated instance.
            // Note: Static instance makes state hard to reason about for this very reason.
            User refreshedUser = Account.currentUser; 

            // Logged in user chould have the updated details
            Assert.AreEqual(refreshedUser.Username, newUsername);
            Assert.AreEqual(refreshedUser.Email, newEmail);
            Assert.AreEqual(refreshedUser.FirstName, newFirstName);
            Assert.AreEqual(refreshedUser.LastName, newLastName);
            Assert.AreEqual(refreshedUser.Phone, newPhone);
        }

        [TestMethod]
        public void EditUserLoggedOut()
        {
            Account.Logout();
            Assert.IsFalse(testUser.IsLoggedIn); // TODO: Remove this layering..
            Assert.IsFalse(Account.IsLoggedIn);
            Assert.AreNotEqual(testUser.Id, originalId);

            string newUsername = "modified_user";
            string newPassword = "newpass99";
            string newEmail = "new@test.ca";
            string newFirstName = "Jane";
            string newLastName = "Doe";
            string newPhone = "226 222 3333";
            string errorMessage = "Current user is not valid.";

            // Attempt to modify the logged out user
            bool output = Account.Edit(
                newUsername, newPassword, newEmail, newFirstName, newLastName, newPhone
            );

            // Should return FALSE and an error message
            Assert.IsFalse(output);
            Assert.IsTrue(Account.ErrorMessages.Contains(errorMessage));

            // Current user should not have the updated fields
            Assert.AreNotEqual(testUser.Username, newUsername);
            Assert.AreNotEqual(testUser.Email, newEmail);
            Assert.AreNotEqual(testUser.FirstName, newFirstName);
            Assert.AreNotEqual(testUser.LastName, newLastName);
            Assert.AreNotEqual(testUser.Phone, newPhone);
        }

        [TestMethod]
        public void EditUsernameNonUnique()
        {
            string newUsername = "shaverz";
            string errorMessage = "Username or email already exists.";

            bool output = Account.Edit(
                newUsername, originalPassword, originalEmail, originalFirstName, originalLastName, originalPhone
            );

            // Should return FALSE and an error message
            Assert.IsFalse(output);
            Assert.IsTrue(Account.ErrorMessages.Contains(errorMessage));

            // User should not be updated
            AssertUserNotUpdated();
        }

        [TestMethod]
        public void EditUsernameBlank()
        {
            string newUsername = "";
            string errorMessage = "Username cannot be blank.";

            bool output = Account.Edit(
                newUsername, originalPassword, originalEmail, originalFirstName, originalLastName, originalPhone
            );

            // Should return FALSE and an error message
            Assert.IsFalse(output);
            Assert.IsTrue(Account.ErrorMessages.Contains(errorMessage));

            // User should not be updated
            AssertUserNotUpdated();
        }

        [TestMethod]
        public void EditPasswordBadStart()
        {
            string newPassword = "1234ts";
            string errorMessage = "Password must start with an alphabetical character.";

            bool output = Account.Edit(
                originalUsername, newPassword, originalEmail, originalFirstName, originalLastName, originalPhone
            );

            // Should return FALSE and an error message
            Assert.IsFalse(output);
            Assert.IsTrue(Account.ErrorMessages.Contains(errorMessage));

            // User should not be updated
            AssertUserNotUpdated();
        }

        [TestMethod]
        public void EditPasswordNonAlphaNum()
        {
            string newPassword = "ts1234@";
            string errorMessage = "Password must only contain alpha numeric characters.";

            bool output = Account.Edit(
                originalUsername, newPassword, originalEmail, originalFirstName, originalLastName, originalPhone
            );

            // Should return FALSE and an error message
            Assert.IsFalse(output);
            Assert.IsTrue(Account.ErrorMessages.Contains(errorMessage));

            // User should not be updated
            AssertUserNotUpdated();
        }

        [TestMethod]
        public void EditPasswordShort()
        {
            string newPassword = "ts12";
            string errorMessage = "Password must be at least 6 characters.";

            bool output = Account.Edit(
                originalUsername, newPassword, originalEmail, originalFirstName, originalLastName, originalPhone
            );

            // Should return FALSE and an error message
            Assert.IsFalse(output);
            Assert.IsTrue(Account.ErrorMessages.Contains(errorMessage));

            // User should not be updated
            AssertUserNotUpdated();
        }


        [TestMethod]
        public void EditPhoneInvalid()
        {
            string newPhone = "22-99(2)";
            string errorMessage = "Phone number is invalid.";

            bool output = Account.Edit(
                originalUsername, originalPassword, originalEmail, originalFirstName, originalLastName, newPhone
            );

            // Should return FALSE and an error message
            Assert.IsFalse(output);
            Assert.IsTrue(Account.ErrorMessages.Contains(errorMessage));

            // User should not be updated
            AssertUserNotUpdated();
        }

        [TestMethod]
        public void EditPhoneBlank()
        {
            string newPhone = "";
            string errorMessage = "Phone number is invalid.";

            bool output = Account.Edit(
                originalUsername, originalPassword, originalEmail, originalFirstName, originalLastName, newPhone
            );

            // Should return FALSE and an error message
            Assert.IsFalse(output);
            Assert.IsTrue(Account.ErrorMessages.Contains(errorMessage));

            // User should not be updated
            AssertUserNotUpdated();
        }

        [TestMethod]
        public void EditEmailNonUnique()
        {
            string newEmail = "shaverz@uwindsor.ca";
            string errorMessage = "Username or email already exists.";

            bool output = Account.Edit(
                originalUsername, originalPassword, newEmail, originalFirstName, originalLastName, originalPhone
            );

            // Should return FALSE and an error message
            Assert.IsFalse(output);
            Assert.AreEqual(Account.ErrorMessages.Count, 1);
            Assert.AreEqual(Account.ErrorMessages[0], errorMessage);

            // User should not be updated
            AssertUserNotUpdated();
        }

        [TestMethod]
        public void EditEmailInvalid()
        {
            string newEmail = "bademail";
            string errorMessage = "Email address is invalid.";

            bool output = Account.Edit(
                originalUsername, originalPassword, newEmail, originalFirstName, originalLastName, originalPhone
            );

            // Should return FALSE and an error message
            Assert.IsFalse(output);
            Assert.IsTrue(Account.ErrorMessages.Contains(errorMessage));

            // User should not be updated
            AssertUserNotUpdated();
        }

        [TestMethod]
        public void EditEmailBlank()
        {
            string newEmail = "";
            string errorMessage = "Email address is invalid.";

            bool output = Account.Edit(
                originalUsername, originalPassword, newEmail, originalFirstName, originalLastName, originalPhone
            );

            // Should return FALSE and an error message
            Assert.IsFalse(output);
            Assert.IsTrue(Account.ErrorMessages.Contains(errorMessage));

            // User should not be updated
            AssertUserNotUpdated();
        }

        [TestMethod]
        public void EditFirstNameBlank()
        {
            string newFirstName = "";
            string errorMessage = "First name cannot be blank.";

            bool output = Account.Edit(
                originalUsername, originalPassword, originalEmail, newFirstName, originalLastName, originalPhone
            );

            // Should return FALSE and an error message
            Assert.IsFalse(output);
            Assert.IsTrue(Account.ErrorMessages.Contains(errorMessage));

            // User should not be updated
            AssertUserNotUpdated();
        }

        [TestMethod]
        public void EditLastNameBlank()
        {
            string newLastName = "";
            string errorMessage = "Last name cannot be blank.";

            bool output = Account.Edit(
                originalUsername, originalPassword, originalEmail, originalFirstName, newLastName, originalPhone
            );

            // Should return FALSE and an error message
            Assert.IsFalse(output);
            Assert.IsTrue(Account.ErrorMessages.Contains(errorMessage));

            // User should not be updated
            AssertUserNotUpdated();
        }
    }
}
