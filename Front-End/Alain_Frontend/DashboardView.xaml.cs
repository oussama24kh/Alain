using project_03.Car;
using project_03.Customer;
using project_03.Global_Classes;
using project_03.Rental;
using project_03.Services;
using System.Windows;
using System.Windows.Controls;

namespace project_03
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        public EventHandler? onViewAllRentals;
        private RentalApiService _RentalApiService;
        private CarApiService _CarApiService;
        private PaymentApiService _PaymentApiService;
        private ApiClient _Client;
        public DashboardView()
        {
            InitializeComponent();

            onViewAllRentals = null;
            _Client = new ApiClient();
            _RentalApiService = new RentalApiService(_Client);
            _CarApiService = new CarApiService(_Client);
            _PaymentApiService = new PaymentApiService(_Client);
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtCurrentDate.Text = Format.DateToLong(DateTime.Now);

            await _LoadData();
        }
        private async Task _LoadData()
        {
            try
            {
                var CountActiveRentalTask = _RentalApiService.CountActiveRentals();
                var CountActiveCarsTask = _CarApiService.CountCarsByStatus("DISPONIBLE");
                var CountAllCarsTask = _CarApiService.CountAllCars();
                var CountRevenueTask = _PaymentApiService.GetRevenue(DateTime.Now.Year, DateTime.Now.Month);
                var CountLateRentalsTask = _RentalApiService.CountRentalsByStatus("EN RETARD");
                var GetLatestRentalsTask = _RentalApiService.GetLatestRentals(5);
                var CountOverdueRentalsTask = _RentalApiService.CountOverdueRentals();

                await Task.WhenAll(CountActiveRentalTask, CountActiveCarsTask, CountAllCarsTask,
                    CountRevenueTask, CountLateRentalsTask, GetLatestRentalsTask, CountOverdueRentalsTask);

                List<DTO.Rental_DTO>? rentals = await GetLatestRentalsTask;
                if (rentals == null || rentals.Count == 0)
                {
                    RentalData.Visibility = Visibility.Collapsed;
                    DashboardEmptyStatePanel.Visibility = Visibility.Visible;
                }
                else
                {
                    DashboardEmptyStatePanel.Visibility = Visibility.Collapsed;
                    RentalData.Visibility = Visibility.Visible;

                    RentalsDataGrid.ItemsSource = rentals;
                }

                txtActiveRentalsCount.Text = (await CountActiveRentalTask).ToString();
                txtAvailableCarsCount.Text = (await CountActiveCarsTask).ToString();
                txtAvailableCarsPercentage.Text = $"Sur {(await CountAllCarsTask).ToString()} véhicules au total";
                txtMonthlyRevenue.Text = $"{(await CountRevenueTask).ToString()} MAD";
                txtOverdueRentalsCount.Text = (await CountLateRentalsTask).ToString();
                txtOverdueRentalsCount.Text = (await CountOverdueRentalsTask).ToString();
            }
            catch { }
        }
        private void BtnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            Window window = new AddEditCustomerWindow(new CustomerApiService(_Client),new CountryApiService(_Client));
            window.ShowDialog();
        }
        private void BtnAddCar_Click(object sender, RoutedEventArgs e)
        {
            Window window = new AddEditCarWindow(_CarApiService, new BrandApiService(_Client));
            window.ShowDialog();
        }
        private async void BtnNewRental_Click(object sender, RoutedEventArgs e)
        {
            Window window = new AddEditRentalWindow();
            window.ShowDialog();
            await _LoadData();
        }
        private void BtnViewAllRentals_Click(object sender, RoutedEventArgs e)
        {
            onViewAllRentals?.Invoke(this, e);
        }
        private async void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            string RentalCode = "";
            if (RentalsDataGrid.SelectedItem is DTO.Rental_DTO selectedRental)
                RentalCode = selectedRental.RentalCode;
            DTO.RentalDetails_DTO? rental = await _RentalApiService.GetRentalDetailsByRentalCode(RentalCode);
            if (rental == null)
            {
                MessageBox.Show($"Aucun contrat n'a été trouvé avec la référence : '{RentalCode}'.",
                            "Contrat Introuvable", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Window window = new RentalDetailsWindow(rental);
            window.ShowDialog();
        }
        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is DTO.Rental_DTO clickedContract)
            {
                if (clickedContract.Status == "TERMINEE" || clickedContract.Status == "ANNULEE")
                {
                    CustomMessageBox.Show("Ce contrat est verrouillé.", "Action interdite", MessageBoxButton.OK,
                        CustomMessageBoxImage.Warning);
                    return;
                }
                string RentalCode = "";
                if (RentalsDataGrid.SelectedItem is DTO.Rental_DTO selectedRental)
                    RentalCode = selectedRental.RentalCode;

                Model.Rental? rental = await _RentalApiService.GetRentalInfoByRentalCode(RentalCode);
                if (rental == null)
                {
                    MessageBox.Show($"Aucun contrat n'a été trouvé avec la référence : '{RentalCode}'.",
                                "Contrat Introuvable", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                Window window = new AddEditRentalWindow(rental);
                window.ShowDialog();
                await _LoadData();
            }
        }
        private async void btnCloturer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is DTO.Rental_DTO clickedContract)
            {
                if (clickedContract.Status == "TERMINEE" || clickedContract.Status == "ANNULEE")
                {
                    CustomMessageBox.Show("Ce contrat est verrouillé.", "Action interdite", MessageBoxButton.OK,
                        CustomMessageBoxImage.Warning);
                    return;
                }
                string RentalCode = "";
                if (RentalsDataGrid.SelectedItem is DTO.Rental_DTO selectedRental)
                    RentalCode = selectedRental.RentalCode;

                Model.Rental? rental = await _RentalApiService.GetRentalInfoByRentalCode(RentalCode);
                if (rental == null)
                {
                    MessageBox.Show($"Aucun contrat n'a été trouvé avec la référence : '{RentalCode}'.",
                                "Contrat Introuvable", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                Window window = new CloseRentalWindow(rental);
                window.ShowDialog();
                await _LoadData();
            }
        }
    }
}