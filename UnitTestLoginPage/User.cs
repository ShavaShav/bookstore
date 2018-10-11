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
          public Boolean IsLoggedIn { set; get; }
          public List<string> ErrorMessages { set; get; }

          // Returns true if successful login
          public Boolean Login(string username, string password)
          {
               ErrorMessages = new List<string>();

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
                    this.Id = -1;
                    return false;
               }

               // Attempt login
               DALUser dbUser = new DALUser();
               this.Id = dbUser.Login(username, password);
               
               if (this.Id >= 0)
               {
                    // Successful login, set user
                    this.Username = username;
                    this.Password = password;
                    return true;
               }
               else
               {
                    // Wrong username/password
                    this.Id = -1;
                    ErrorMessages.Add("Incorrect username or password.");
                    return false;
               }
          }
     }
}