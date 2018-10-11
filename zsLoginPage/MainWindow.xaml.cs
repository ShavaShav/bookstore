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

using BookStoreLib;

namespace BookStoreGUI
{
     /// <summary>
     /// Interaction logic for MainWindow.xaml
     /// </summary>
     public partial class MainWindow : Window
     {
          private User User { get; set; } // instance of current user

          public MainWindow()
          {
               InitializeComponent();
          }

          private void listViewOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
          {

          }

          private void LoginButton_Click(object sender, RoutedEventArgs e)
          {
               var loginDialog = new LoginDialog();
               loginDialog.Owner = this;
               loginDialog.ShowDialog();

               if (loginDialog.DialogResult == true)
               {
                    // Login successful
                    User = loginDialog.User;
                    this.textBlockStatus.Text = "You are logged in as " + User.Username + ".";
               }
          }

          private void ExitButton_Click(object sender, RoutedEventArgs e)
          {
               this.Close();
          }

          private void AddBookButton_Click(object sender, RoutedEventArgs e)
          {

          }

          private void CheckoutButton_Click(object sender, RoutedEventArgs e)
          {

          }
     }
}
