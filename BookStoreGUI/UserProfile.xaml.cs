using BookStoreLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class UserProfile : Window
    {
        public User User { get; private set; }
        private bool IsEditing;

        public UserProfile(User user)
        {
            InitializeComponent();

            this.User = user; // set instance for window

            // initialize
            ResetFields();
            MakeEditable(false);
        }

        private void ResetFields()
        {
            this.boxUsername.Text = User.Username;
            this.boxPassword.Password = User.Password;
            this.boxEmail.Text = User.Email;
            this.boxFirstName.Text = User.FirstName;
            this.boxLastName.Text = User.LastName;
            this.boxPhone.Text = User.Phone;
        }

        private void MakeEditable(bool editable)
        {
            this.boxUsername.IsEnabled = editable;
            this.boxPassword.IsEnabled = editable;
            this.boxEmail.IsEnabled = editable;
            this.boxFirstName.IsEnabled = editable;
            this.boxLastName.IsEnabled = editable;
            this.boxPhone.IsEnabled = editable;

            this.buttonUpdate.Content = editable ? "Save" : "Edit";
            IsEditing = editable;
        }

        private void OnUpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!IsEditing)
            {
                // Allow user to update fields
                MakeEditable(true);
            }
            else
            {
                // Save updated fields
                MakeEditable(false);

                // Could get the current user from Account here, but there is no gaurantee it's the same
                // user that opened the window. This is a problem encapsulating current user in the Account class

                // Check that current user in Account class is same as our User. If not close, as stale profile
                if (!Account.IsLoggedIn || Account.currentUser.Id != User.Id)
                {
                    MessageBox.Show("User doesn't match, I call hacks!");
                    this.Close();
                    this.Owner.Focus();
                    return;
                }

                // Account does validation and populates errors
                bool updated = Account.Edit(
                    this.boxUsername.Text,
                    this.boxPassword.Password,
                    this.boxEmail.Text,
                    this.boxFirstName.Text,
                    this.boxLastName.Text,
                    this.boxPhone.Text
                );

                if (updated)
                {
                    MessageBox.Show("Successfully updated your profile");
                }
                else
                {
                    MessageBox.Show("Please correct the following problems and try again: \n\n"
                        + String.Join("\n- ", Account.ErrorMessages));
                    MakeEditable(true);
                }
            }
        }

        private void OnCloseButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Close(); // dont save details, just close
            this.Owner.Focus();
        }
    }
}
