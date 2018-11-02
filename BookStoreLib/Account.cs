using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLib
{
    class Account
    {



        // Returns true if successful login
        public static bool Login(string username, string password)
        {
            // Attempt login
            DALUser dbUser = new DALUser();
            this.IsLoggedIn = dbUser.Login(username, password);

            if (this.IsLoggedIn)
            {
                // Successful login, set user
                this.Username = dbUser.Username;
                this.Password = dbUser.Password;
                this.FullName = dbUser.FullName;
                this.Type = dbUser.Type;
                this.IsManager = dbUser.IsManager;
            }
            else
            {
                // Wrong username/password
                this.Id = -1;
                ErrorMessages.Add("Incorrect username or password.");
            }

            return this.IsLoggedIn;
        }

        private bool isValidPassword(string password)
        {
            ErrorMessages.Clear();

            // If password is smaller than 6
            if (password.Length < 6)
                ErrorMessages.Add("Password is smaller than 6 characters");

            // If first letter is not a letter
            if (!char.IsLetter(password[0]))
                ErrorMessages.Add("Password has to start with a letter");

            // If string contains characters that are not letters or digits
            if (!password.All(Char.IsLetterOrDigit))
                ErrorMessages.Add("Password cannot contain non-alphanumeric characters");

            // If password does not contain both letters and numbers
            var containsDigits = false;
            var containsLetters = false;

            for (var i = 0; i < password.Length; i++)
            {
                if (char.IsDigit(password[i])) containsDigits = true;
                if (char.IsLetter(password[i])) containsLetters = true;
            }

            if (!containsLetters || !containsDigits)
                ErrorMessages.Add("Password should contain both letters and digits");

            // Else, return true
            return ErrorMessages.Count == 0;
        }

        public static bool Register(
            string username,
            string password,
            string firstName,
            string lastName,
            string email,
            string phone,
            string addressLine1,
            string addressLine2,
            string city,
            string province,
            string postalCode)
        {
            throw new NotImplementedException();
        }
    }













}
}
