using System.Windows;

namespace project_03.Customer
{
    /// <summary>
    /// Interaction logic for CustomerDetailsWindow.xaml
    /// </summary>
    public partial class CustomerDetailsWindow : Window
    {
        public CustomerDetailsWindow(Model.Customer customer)
        {
            InitializeComponent();
            CustomerDetailsControl.Load_Customer(customer);
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}