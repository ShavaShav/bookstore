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
            BookCatalog bookCat = new BookCatalog();
            DsBookCat = bookCat.GetBookInfo();
            this.DataContext = DsBookCat.Tables["Category"];
            bookOrder = new BookOrder();
            this.listViewOrders.ItemsSource = bookOrder.OrderItemList;
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
                this.textBlockStatus.Text = "You are logged in as " + User.FullName + ".";
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            OrderItemDialog orderItemDialog = new OrderItemDialog();
            DataRowView selectedRow;
            selectedRow = (DataRowView)this.dataGridBooks.SelectedItems[0];
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
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.listViewOrders.SelectedItem != null)
            {
                var selectedOrderItem = this.listViewOrders.SelectedItem as OrderItem;
                bookOrder.RemoveItem(selectedOrderItem.BookID);
            }
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            int orderId;
            orderId = bookOrder.PlaceOrder(User.Id);
            MessageBox.Show("Your order has been placed. Your order id is " +
            orderId.ToString());
        }
    }
}
