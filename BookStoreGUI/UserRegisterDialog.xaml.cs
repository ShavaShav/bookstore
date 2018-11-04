using BookStoreLib;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for UserRegisterDialog.xaml
    /// </summary>
    public partial class UserRegisterDialog : Window
    {
        private const String emailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

        private const String phonePattern = @"^\(?([0-9]{3})\)?[-.● ]?([0-9]{3})[-.●]?([0-9]{4})$";

        private const String postalCodePattern = @"[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ] ?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]";

        private const String numberFilter = @"[^\d]";

        public UserRegisterDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if any of the required fields are empty
            if(
                UsernameBox.Text.Trim().Equals("") ||
                PasswordBox.Password.Trim().Equals("") ||
                FirstNameBox.Text.Trim().Equals("") || 
                LastNameBox.Text.Trim().Equals("") ||
                EmailBox.Text.Trim().Equals("") ||
                PhoneBox.Text.Trim().Equals("") ||
                AddressLine1Box.Text.Trim().Equals("") ||
                CityBox.Text.Trim().Equals("") ||
                ProvinceComboBox.SelectedIndex == -1 ||
                PostalCodeBox.Text.Trim().Equals("") 
                )
            {
                MessageBox.Show("Please make sure to all required information has been entered.", "Incomplete Form");
                return;
            }

            // TODO: Check if username exists in database. Display a messagebox in that case.
            // TODO: Check if email exists in database. Display a messagebox in that case

            // Check if password matches confirm password box
            if(PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("The passwords do not match. Please check the passwords you have entered", "Password Mismatch");
                return;
            }

            // Check if a valid email is entered
            if (!Regex.IsMatch(EmailBox.Text, emailPattern))
            {
                MessageBox.Show("Invalid email address entered. Please check the email entered.", "Invalid Email");
                return;
            }

            // Check if a valid phone number is entered
            if (!Regex.IsMatch(PhoneBox.Text, phonePattern))
            {
                MessageBox.Show("Invalid phone number entered. Please check the number entered.", "Invalid Phone number");
                return;
            }

            // Check if a valid postal code is entered
            if(!Regex.IsMatch(PostalCodeBox.Text.Trim().ToUpper(), postalCodePattern))
            {
                MessageBox.Show(
                    "Invalid postal code entered. Please check the postal code you have entered.\n\n" +
                    "- Postal codes are in the format of A1B 2C3 with the gap being optional.\n\n" +
                    "- Postal codes can't contain the letters D, F, I, O, Q, or U, and cannot start with W or Z", "Invalid Postal Code");
                return;
            }

            // If no error is detected, prepare the fields
            var username = UsernameBox.Text.Trim();
            var firstName = FirstNameBox.Text.Trim();
            firstName = firstName.First().ToString().ToUpper() + firstName.Substring(1);
            var lastName = LastNameBox.Text.Trim();
            lastName = lastName.First().ToString().ToUpper() + lastName.Substring(1);
            var email = EmailBox.Text.Trim().ToLower();
            var phone = Regex.Replace(PhoneBox.Text.Trim(), numberFilter, "");
            var addressLine1 = AddressLine1Box.Text.Trim();
            var addressLine2 = AddressLine2Box.Text.Trim();
            var city = CityBox.Text.Trim();
            city = city.First().ToString().ToUpper() + city.Substring(1);
            var province = ProvinceComboBox.Text;
            var postalCode = PostalCodeBox.Text.Trim().ToUpper();
            postalCode.Replace(" ", "");

            // Create the User object to pass to the register function
            User user = new User(
                username, firstName, lastName, email, phone, addressLine1, 
                addressLine2, city, province, postalCode);

            // DEBUG: Show the user's info before starting registration process
            MessageBox.Show(user.ToString(), "User Info from User.cs");

            if(Account.Register(user, PasswordBox.Password) == null)
            {
                String errorString = "Login failed due to the following errors:\n\n";
                Account.ErrorMessages.ForEach(error => {
                    errorString += "● " + error +".\n";
                });
                MessageBox.Show(errorString, "Login Failed");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
