using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLib
{
    public static class Account
    {
        public static bool IsLoggedIn;
        public static List<string> ErrorMessages = new List<string>();
        public static User currentUser = null;

        // Returns true if successful login
        public static bool Login(string username, string password)
        {
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
            // Create a DALUser object
            DALUser dbUser = new DALUser();
            // Check if password is valid
            if (!isValidPassword(password)) return null;

            // Check if username or email exist in DB
            if (dbUser.UsernameOrEmailExistsInDb(user)) return null;

            // Attempt registration, return the user object if successful
            if (dbUser.Register(user, password))
                return user;

            else return null;
        }

        private static bool isValidPassword(string password)
        {
            ErrorMessages.Clear();

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
    }
}

