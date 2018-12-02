using BookStoreLib;
using System.Windows;

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for PurchaseHistoryWindow.xaml
    /// </summary>
    public partial class PurchaseHistoryDialog : Window
    {
        public PurchaseHistoryDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO: Fetch data and load it into the data grid
            //Get order/purchase history 
            if (Account.IsLoggedIn)
            {
                PurchaseHistory history = new PurchaseHistory();
                history.GetPurchaseInfo(Account.currentUser.Username);
                this.listPurchaseHistory.ItemsSource = history.PurchaseHistoryList;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
