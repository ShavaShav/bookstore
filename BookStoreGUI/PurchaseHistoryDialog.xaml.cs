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
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
