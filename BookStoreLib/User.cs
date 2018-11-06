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

        public string Email { get; set; }
        public List<string> ErrorMessages { set; get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }

        public User(string username, string firstname, string lastname, string email, string phone)
        {
            this.Username = username;
            this.FirstName = firstname;
            this.LastName = lastname;
            this.Email = email;
            this.Phone = phone;
        }

        public bool logout()
        {
            if (IsLoggedIn)
            {
                // Nullify user
                this.Id = -1;
                this.Username = "";
                this.Password = "";
                this.FullName = "";
                this.Type = "";
                this.IsManager = false;
                this.IsLoggedIn = false;
            }

            return !this.IsLoggedIn; // true if logged out
        }

        public void setUserId(int id)
        {
            this.Id = id;
        }

        public new string ToString()
        {
            String returnString =
                "Username: " + Username + "\n" +
                "First Name: " + FirstName + "\n" +
                "Last Name: " + LastName + "\n" +
                "Email: " + Email + "\n" +
                "Phone: " + Phone + "\n";

            return returnString;
        }
    } //end User Body
}//end namespace
