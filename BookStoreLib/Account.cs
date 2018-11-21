using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookStoreLib
{
    public static class Account
    {
        public static bool IsLoggedIn;
        public static List<string> ErrorMessages = new List<string>();
        public static User currentUser = null;

        private const string PHONE_REGEX = @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$";

        // Returns true if successful login
        public static bool Login(string username, string password)
        {
            ErrorMessages.Clear();

            if (!isValidPassword(password)) return false;

            // Attempt login
            DALUser dbUser = new DALUser();
            currentUser = dbUser.Login(username, password);

            // dbUser's login function sets currentUser to a user object if login was successful.
            IsLoggedIn = (currentUser != null);
            return IsLoggedIn;
        }

        public static bool Logout()
        {
            try
            {
                // Nullify user (MainWindow probably still holds instance)
                currentUser.Id = -1;
                currentUser.Username = "";
                currentUser.Password = "";
                currentUser.FullName = "";
                currentUser.Type = "";
                currentUser.IsManager = false;
                currentUser.IsLoggedIn = false;

                // Dereference our instance
                currentUser = null;
                IsLoggedIn = false;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }

            return IsLoggedIn;
        }

        /* Function Register:
         * 
         * Inputs: 
         *  - A User object containing the user to be registered
         *  - A string containing the password
         *  
         * Outputs:
         *  - Returns a user object if registration is successful, null if registration fails
         * 
         */
        public static User Register(User user, string password)
        {
            ErrorMessages.Clear();

            // Create a DALUser object
            DALUser dbUser = new DALUser();
            // Check if password is valid
            if (!isValidPassword(password)) return null;

            // Check if username or email exist in DB
            if (dbUser.UsernameOrEmailExistsInDb(user)) return null;

            // Attempt registration, return the user object if successful
            return dbUser.Register(user, password);
        }

        // Edits all of current users fields
        public static bool Edit(string username, string password, string email, string firstName, string lastName, string phone)
        {
            // Validate fields
            if (!IsLoggedIn || currentUser.Id < 0)
            {
                ErrorMessages.Add("Current user is not valid.");
                return false; // dont bother validating rest
            }

            if (username.Equals(""))
                ErrorMessages.Add("Username cannot be blank.");

            isValidPassword(password); // Function populates error messages

            if (email.Equals("") || !isValidEmail(email))
                ErrorMessages.Add("Email address is invalid.");

            if (firstName.Equals(""))
                ErrorMessages.Add("First name cannot be blank.");

            if (lastName.Equals(""))
                ErrorMessages.Add("Last name cannot be blank.");

            var validPhone = Regex.Match(phone, PHONE_REGEX, RegexOptions.IgnoreCase);
            if (phone.Equals("") || !validPhone.Success)
                ErrorMessages.Add("Phone number is invalid.");

            if (ErrorMessages.Count > 0)
                return false; // didnt pass validation

            // Attempt edit, returns true if successful
            DALUser dbUser = new DALUser();
            bool updated = dbUser.Edit(currentUser.Id, username, password, email, firstName, lastName, phone);
            
            if (updated)
            {
                // details are persisted, can now set the current user instance's variables
                currentUser.Username = username;
                currentUser.Email = email;
                currentUser.FirstName = firstName;
                currentUser.LastName = lastName;
                currentUser.Phone = phone;
            }

            return updated;
        }

        private static bool isValidPassword(string password)
        {
            // If password is smaller than 6
            if (password.Length < 6)
                ErrorMessages.Add("Password must be at least 6 characters.");

            // If first letter is not a letter
            if (!char.IsLetter(password[0]))
                ErrorMessages.Add("Password must start with an alphabetical character.");

            // If string contains characters that are not letters or digits
            if (!password.All(Char.IsLetterOrDigit))
                ErrorMessages.Add("Password must only contain alpha numeric characters.");

            // If password does not contain both letters and numbers
            var containsDigits = false;
            var containsLetters = false;

            for (var i = 0; i < password.Length; i++)
            {
                if (char.IsDigit(password[i])) containsDigits = true;
                if (char.IsLetter(password[i])) containsLetters = true;
            }

            if (!containsLetters || !containsDigits)
                ErrorMessages.Add("Password should contain both letters and numbers");

            // Else, return true
            return ErrorMessages.Count == 0;
        }

        private static bool isValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}

