using BookStoreLib;
using System;
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
        public UserRegisterDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if any of the required fields are empty
            if(
                UsernameBox.Text.Equals("") ||
                FirstNameBox.Text.Equals("") || 
                LastNameBox.Text.Equals("") ||
                EmailBox.Text.Equals("") ||
                PhoneBox.Text.Equals("") ||
                AddressLine1Box.Text.Equals("") ||
                CityBox.Text.Equals("") ||
                ProvinceComboBox.SelectedIndex == -1 ||
                PostalCodeBox.Text.Equals("") 
                )
            {
                MessageBox.Show("Please make sure to all required information has been entered.", "Incomplete Form");
                return;
            }

            // TODO: Check if username exists in database. Display a messagebox in that case.
            // TODO: Check if email exists in database. Display a messagebox in that case.

            // Check if a valid email is entered
            if (!IsValidEmail(EmailBox.Text))
            {
                MessageBox.Show("Invalid email address entered. Please check the email entered.", "Invalid Email");
                return;
            }

            // Check if a valid phone number is entered
            if (!IsValidPhone(PhoneBox.Text))
            {
                MessageBox.Show("Invalid phone number entered. Please check the number entered.", "Invalid Phone number");
                return;
            }

            // Check if a valid postal code is entered
            if(!IsValidPostalCode(PostalCodeBox.Text))
            {
                MessageBox.Show(
                    "Invalid postal code entered. Please check the postal code you have entered.\n\n" +
                    "- Postal codes are in the format of A1B 2C3 with the gap being optional.\n\n" +
                    "- Postal codes can't contain the letters D, F, I, O, Q, or U, and cannot start with W or Z", "Invalid Postal Code");
                return;
            }

            // If no error is detected, create the User object to pass to the register function
            User user = new User(
                UsernameBox.Text, FirstNameBox.Text, LastNameBox.Text, 
                EmailBox.Text, PhoneBox.Text, AddressLine1Box.Text, 
                AddressLine2Box.Text, CityBox.Text, 
                ProvinceComboBox.SelectedValue.ToString(), PostalCodeBox.Text);

            Account.Register(user, PasswordBox.Password);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool IsValidEmail(string emailaddress)
        {
            String theEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

            if (Regex.IsMatch(emailaddress, theEmailPattern))
                return true;
            return false;
        }

        private bool IsValidPhone(string phonenumber)
        {
            String phonePattern = @"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$";

            if (Regex.IsMatch(phonenumber, phonePattern))
                return true;
            return false;
        }

        private bool IsValidPostalCode(string postalcode)
        {
            String postalCodePattern = @"[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ] ?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]";

            if (Regex.IsMatch(postalcode, postalCodePattern))
                return true;
            return false;
        }
    }
}
