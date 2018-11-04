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
        public static void Login(string username, string password)
        {
            if (!isValidPassword(password)) return;
            // Attempt login
            DALUser dbUser = new DALUser();
            currentUser = dbUser.Login(username, password);

            // dbUser's login function sets currentUser to a user object if login was successful.
            IsLoggedIn = (currentUser == null) ? false : true;
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
            // Check if password is valid
            if (!isValidPassword(password)) return null;

            // Create a DALUser object and call the Register() function
            // If registration is successful, return the user object
            DALUser dbUser = new DALUser();
            if (dbUser.Register(user, password))
                return user;

            else return null;
        }

        private static bool isValidPassword(string password)
        {
            ErrorMessages.Clear();

            // If password is smaller than 6
            if (password.Length < 6)
                ErrorMessages.Add("Password is smaller than 6 characters");

            // If first letter is not a letter
            if (!char.IsLetter(password[0]))
                ErrorMessages.Add("Password does not start with a letter");

            // If string contains characters that are not letters or digits
            if (!password.All(Char.IsLetterOrDigit))
                ErrorMessages.Add("Password contains non-alphanumeric characters");

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

