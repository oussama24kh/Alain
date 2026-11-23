using project_03.Services;
using System.Windows;
using System.Windows.Controls;

namespace project_03.Customer.Controls
{
    /// <summary>
    /// Interaction logic for CustomerSearchControl.xaml
    /// </summary>
    public partial class CustomerSearchControl : UserControl
    {
        public Model.Customer? Customer { get; private set; }
        public CustomerApiService? CustomerApiService { private get; set; }
        public CustomerSearchControl()
        {
            InitializeComponent();
            Customer = null;
            Btn_Search.IsEnabled = false;
            Btn_Search.Opacity = 0.5;
            txtSearchCin.Text = "";
        }
        private async void Btn_Search_Click(object sender, RoutedEventArgs e)
        {
            string cin = txtSearchCin.Text.Trim();
            if (string.IsNullOrEmpty(cin))
            {
                CustomMessageBox.Show("Veuillez saisir un numéro CIN valide.", "Attention",
                    MessageBoxButton.OK, CustomMessageBoxImage.Warning);
                return;
            }
            await LoadCustomerByCin(cin);
        }
        public async Task LoadCustomerByCin(string cin)
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                if (CustomerApiService == null)
                    throw new Exception();

                Model.Customer? customer =
                    await CustomerApiService.GetCustomerInfoByNationalNumber(cin);

                if (customer != null)
                    LoadCustomer(customer);
                else
                {
                    Customer = null;
                    CustomerDetailsControl.ResetForm();

                    CustomMessageBox.Show(
                        $"Aucun client trouvé avec le CIN : {cin}",
                        "Information",
                        MessageBoxButton.OK);
                }
            });
        }
        public async Task LoadCustomerByCustomerID(int CustomerID)
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                if (CustomerApiService == null)
                    throw new Exception();

                Model.Customer? customer =
                    await CustomerApiService.GetCustomerInfoByCustomerID(CustomerID);

                if (customer != null)
                    LoadCustomer(customer);
                else
                {
                    Customer = null;
                    CustomerDetailsControl.ResetForm();

                    CustomMessageBox.Show(
                        "Aucun client trouvé.",
                        "Information",
                        MessageBoxButton.OK);
                }
            });
        }
        public void LoadCustomer(Model.Customer customer)
        {
            Customer = customer;
            txtSearchCin.Text = customer.NationalNo;
            CustomerDetailsControl.Load_Customer(customer);
        }
        private void txtSearchCin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchCin.Text))
            {
                Btn_Search.IsEnabled = false;
                Btn_Search.Opacity = 0.5;
            }
            else
            {
                Btn_Search.IsEnabled = true;
                Btn_Search.Opacity = 1;
            }
        }
    }
}