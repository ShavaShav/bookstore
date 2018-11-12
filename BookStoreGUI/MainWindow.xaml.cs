using System;
using System.Collections.Generic;
using System.Data;
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
//Main GUI file for prefaceBookStore
namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User User { get; set; } // instance of current user
        private DataSet DsBookCat { get; set; }
        private BookOrder bookOrder { get; set; }


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            User = Account.currentUser;
            bookOrder = new BookOrder(); // start new order

            InitializeUI();
        }

        // Initialize state of all UI componets
        public void InitializeUI()
        {
            // Unset category
            categoriesComboBox.SelectedItem = -1;

            // Intialize book table
            BookCatalog bookCat = new BookCatalog();
            DsBookCat = bookCat.GetBookInfo();
            Console.WriteLine(DsBookCat.ToString());

            this.DataContext = DsBookCat.Tables["Category"];

            // Intialize book order context (resets order table)
            bookOrder = new BookOrder();
            this.listViewOrders.ItemsSource = bookOrder.OrderItemList;

            // Intialize status bar message
            this.textBlockStatus.Text = "Please login before proceeding to checkout.";
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
                this.textBlockStatus.Text = "You are logged in as " + User.FirstName + " " + User.LastName + ".";
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            //to clear out the whole Main Window fields such as categories, books, and orders.
            if (Account.IsLoggedIn && Account.Logout())
            {
                InitializeUI(); // reset the UI
                bookOrder = new BookOrder(); // start a new order (user will need to log back in to complete)
            }
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if row selected
            if (this.dataGridBooks.SelectedItems.Count < 1)
            {
                MessageBox.Show("You must select a book before adding.");
                return;
            }

            // An empty row that is selected will return something other than a DataRowView (why god why?)
            if (!(this.dataGridBooks.SelectedItems[0] is System.Data.DataRowView))
            {
                MessageBox.Show("Row is empty. Must select a valid book..");
                return;
            }

            // Selected row is valid.
            DataRowView selectedRow = (DataRowView)this.dataGridBooks.SelectedItems[0];

            // Check if selected row has empty fields
            for (int i = 0; i < selectedRow.Row.ItemArray.Count(); i++)
            {
                if (selectedRow.Row.IsNull(i))
                {
                    MessageBox.Show("Cannot add a book with empty fields.");
                    return;
                }
            }

            // Create book order from the chosen book and user input
            OrderItemDialog orderItemDialog = new OrderItemDialog();
            orderItemDialog.isbnTextBox.Text = selectedRow.Row.ItemArray[0].ToString();
            orderItemDialog.titleTextBox.Text = selectedRow.Row.ItemArray[2].ToString();
            orderItemDialog.priceTextBox.Text = selectedRow.Row.ItemArray[4].ToString();
            orderItemDialog.Owner = this;
            orderItemDialog.ShowDialog();
            if (orderItemDialog.DialogResult == true)
            {
                string isbn = orderItemDialog.isbnTextBox.Text;
                string title = orderItemDialog.titleTextBox.Text;
                double unitPrice = double.Parse(orderItemDialog.priceTextBox.Text);
                int quantity = int.Parse(orderItemDialog.quantityTextBox.Text);
                bookOrder.AddItem(new OrderItem(isbn, title, unitPrice, quantity));
                updateTotal();
            }
        }

        private void RemoveBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.listViewOrders.SelectedItem != null)
            {
                var selectedOrderItem = this.listViewOrders.SelectedItem as OrderItem;
                bookOrder.RemoveItem(selectedOrderItem.BookID);

                updateTotal();
            } else
            {
                MessageBox.Show("Please select a book to remove from orders.");
            }
        }

        //updates the running total price
        private void updateTotal()
        {
            // TODO: set total field with bookOrgder.getTotal()
            double total = 0.0;
            total = bookOrder.GetOrderTotal();
            totalPrice.Text = total.ToString();
            return;
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Account.IsLoggedIn)
            {
                MessageBox.Show("You must be logged in to place an order.");
                return;
            }

            if (this.listViewOrders.Items.Count < 1)
            {
                MessageBox.Show("Order list is empty. Please add a book before checking out.");
                return;
            }

            int orderId;
            orderId = bookOrder.PlaceOrder(User.Id);
            MessageBox.Show("Your order has been placed. Your order id is " + orderId.ToString());

            // Start a new order
            bookOrder = new BookOrder();
        }
    }
}
