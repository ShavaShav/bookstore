using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookStoreLib;

namespace BookStoreGUI
{
     /// <summary>
     /// Interaction logic for MainWindow.xaml
     /// </summary>
     public partial class LoginDialog : Window
     {
          public User User { get; private set; }

          public LoginDialog()
          {
               InitializeComponent();
          }

          private void OnOKButtonClicked(object sender, RoutedEventArgs e)
          {
               string username = this.usernameTextBox.Text;
               string password = this.passwordTextBox.Password;

               // Attempt login
               User = new User();
               if (User.Login(username, password))
               {
                    // Logged in succesfully, close.
                    this.DialogResult = true;
                    this.Close();
               }
               else
               {
                    // Login failed. Don't close yet, show errors and let user correct
                    MessageBox.Show("Please correct the following errors and try again: \n\n" 
                         + String.Join("\n", User.ErrorMessages));
               }
          }

          private void OnCancelButtonClicked(object sender, RoutedEventArgs e)
          {
               // Login aborted, close.
               this.DialogResult = false;
               this.Close();
          }
     }
}
