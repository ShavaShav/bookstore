using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLib
{
     /**
     * User model 
     */
     public class User
     {
          public int Id { set; get; }
          public string Username { set; get; }
          public string Password { set; get; }
          public string FullName { set; get; }
          public string Type { set; get; }
          public bool IsManager { set; get; }
          public bool IsLoggedIn { set; get; }
          public List<string> ErrorMessages { set; get; }

          public User()
          {
            // Null user
            this.Id = -1;
            this.Username = "";
            this.Password = "";
            this.FullName = "";
            this.Type = "";
            this.IsManager = false;
            this.IsLoggedIn = false;
            this.ErrorMessages = new List<string>();
        }

        // Returns true if successful login
        public static User Login(string username, string password)
          {
               var ErrorMessages = new List<string>();

               // Validate username
               if (username == "")
               {
                    ErrorMessages.Add("Username cannot be blank.");
               }

               // Validate password
               if (password.Length <= 0)
               {
                    ErrorMessages.Add("Password cannot be blank.");
               }
               else if (password.Length < 6)
               {
                    ErrorMessages.Add("Password must be at least 6 characters.");
               }
               else
               {
                    if (!char.IsLetter(password[0]))
                    {
                         ErrorMessages.Add("Password must start with an alphabetical character.");
                    }

                    if (!password.All(Char.IsLetterOrDigit))
                    {
                         ErrorMessages.Add("Password must only contain alpha numeric characters.");
                    }
               }

               if (ErrorMessages.Count() > 0)
               {
                    // Validation error (see tests in LoginUnitTest.cs)
                    User invalidUser = new User();
                    invalidUser.ErrorMessages = ErrorMessages;
                    return invalidUser;
               }

               // Attempt login
               DALUser dbUser = new DALUser();
               return dbUser.Login(username, password);
          }

     }
}