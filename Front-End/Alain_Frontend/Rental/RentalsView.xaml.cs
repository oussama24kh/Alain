using project_03.DTO;
using project_03.Services;
using System.Windows.Controls;
using System.Windows;

namespace project_03.Rental
{
    /// <summary>
    /// Interaction logic for RentalsView.xaml
    /// </summary>
    public partial class RentalsView : UserControl
    {
        private readonly ApiClient _Client;
        private readonly RentalApiService _RentalApiService;
        public RentalsView()
        {
            InitializeComponent();
            _Client = new ApiClient();
            _RentalApiService = new RentalApiService(_Client);
            CBx_FilterBy.SelectedIndex = 0;
            CBx_FilterBy.SelectionChanged += CBx_FilterBy_SelectionChanged;
            CBx_Status.SelectionChanged += CBx_Status_SelectionChanged;
        }
        private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await _LoadRentals();

            if (RentalsDataGrid.Items.Count == 0)
            {
                FilterGird.Visibility = Visibility.Collapsed;
            }
        }
        private async Task _LoadRentals()
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                var CountActiveRentals = _RentalApiService.CountActiveRentals();
                var CountOverdueRentals = _RentalApiService.CountRentalsByStatus("EN RETARD");
                var CompletedInMonth = _RentalApiService.CountCompletedRentals(DateTime.Now.Year, DateTime.Now.Month);
                var GetAllRentals = _RentalApiService.GetAllRentals();

                await Task.WhenAll(CountActiveRentals, CountOverdueRentals, CompletedInMonth, GetAllRentals);

                _LoadDataToForm(await GetAllRentals, await CountActiveRentals, await CountOverdueRentals, await CompletedInMonth);
            });
        }
        private void _LoadDataToForm(List<Rental_DTO>? rentals, int ActiveRentals, int OverdueRentals, int CompletedRentals)
        {
            if (rentals == null || rentals.Count == 0)
            {
                RentalsDataGrid.Visibility = Visibility.Collapsed;
                EmptyStatePanel.Visibility = Visibility.Visible;
            }
            else
            {
                EmptyStatePanel.Visibility = Visibility.Collapsed;
                RentalsDataGrid.Visibility = Visibility.Visible;

                RentalsDataGrid.ItemsSource = rentals;          
            }
            txtActiveRentals.Text = ActiveRentals.ToString();
            txtOverdueRentals.Text = OverdueRentals.ToString();
            txtCompletedThisMonth.Text = CompletedRentals.ToString();
        }
        private void _LoadDataToForm(List<Rental_DTO>? rentals)
        {
            if (rentals == null || rentals.Count == 0)
            {
                RentalsDataGrid.Visibility = Visibility.Collapsed;
                EmptyStatePanel.Visibility = Visibility.Visible;
            }
            else
            {
                EmptyStatePanel.Visibility = Visibility.Collapsed;
                RentalsDataGrid.Visibility = Visibility.Visible;

                RentalsDataGrid.ItemsSource = rentals;
            }
        }
        private void BtnAddRental_Click(object sender, RoutedEventArgs e)
        {
            AddEditRentalWindow window = new AddEditRentalWindow();
            window.onRefresh += (sender, e) => _RefreshPage();
            window.ShowDialog();
            
        }
        private async void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            string RentalCode = "";
            if (RentalsDataGrid.SelectedItem is DTO.Rental_DTO selectedRental)
                RentalCode = selectedRental.RentalCode;

            DTO.RentalDetails_DTO? rental = null;
            bool RequestSucceeded = false;

            await ErrorHandler.ExecuteAsync(async () =>
            {
                rental = await _RentalApiService.GetRentalDetailsByRentalCode(RentalCode);
                RequestSucceeded = true;
            });

            if (!RequestSucceeded)
                return;

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

                string RentalCode = clickedContract.RentalCode;

                Model.Rental? rental = null;
                bool RequestSucceeded = false;

                await ErrorHandler.ExecuteAsync(async () =>
                {
                    rental = await _RentalApiService.GetRentalInfoByRentalCode(RentalCode);
                    RequestSucceeded = true;
                });

                if (!RequestSucceeded)
                    return;

                if (rental == null)
                {
                    MessageBox.Show($"Aucun contrat n'a été trouvé avec la référence : '{RentalCode}'.",
                                "Contrat Introuvable", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                AddEditRentalWindow window = new AddEditRentalWindow(rental);
                window.onRefresh += (sender, e) => _RefreshPage();
                window.ShowDialog();
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

                string RentalCode = clickedContract.RentalCode;

                Model.Rental? rental = null;
                bool RequestSucceeded = false;

                await ErrorHandler.ExecuteAsync(async () =>
                {
                    rental = await _RentalApiService.GetRentalInfoByRentalCode(RentalCode);
                    RequestSucceeded = true;
                });

                if (!RequestSucceeded)
                    return;

                if (rental == null)
                {
                    CustomMessageBox.Show($"Aucun contrat n'a été trouvé avec la référence : '{RentalCode}'.",
                                "Contrat Introuvable", MessageBoxButton.OK, CustomMessageBoxImage.Warning);
                    return;
                }

                Window window = new CloseRentalWindow(rental);
                window.ShowDialog();
                _RefreshPage();
            }
        }
        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            txtbx_Filter.Text = "";
            btn_Clear.Visibility = Visibility.Hidden;
        }
        private async void CBx_FilterBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_Clear.Visibility = Visibility.Hidden;

            if (CBx_FilterBy.SelectedIndex == 0 || CBx_FilterBy.SelectedIndex == -1)
            {
                txtbx_Filter.Visibility = Visibility.Collapsed;
                CBx_Status.Visibility = Visibility.Collapsed;

                await _LoadRentals();
            }
            else if (CBx_FilterBy.SelectedIndex == 4)
            {
                CBx_Status.Items.Clear();
                txtbx_Filter.Visibility = Visibility.Collapsed;
                CBx_Status.Visibility = Visibility.Visible;

                List<string>? RentalStatus = null;
                bool RequestSucceeded = false;

                await ErrorHandler.ExecuteAsync(async () =>
                {
                    RentalStatus = await _RentalApiService.GetActiveStatus();
                    RequestSucceeded = true;
                });

                if (!RequestSucceeded)
                    return;

                if (RentalStatus != null)
                {
                    if (RentalStatus.Count > 1)
                        CBx_Status.Items.Add("Tous");

                    foreach (var item in RentalStatus)
                        CBx_Status.Items.Add(item);
                }

                CBx_Status.SelectedIndex = 0;
            }
            else
            {
                txtbx_Filter.Text = "";

                CBx_Status.Visibility = Visibility.Collapsed;
                txtbx_Filter.Visibility = Visibility.Visible;
            }
        }
        private async void CBx_Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBx_FilterBy.SelectedIndex == 4)
            {
                if (CBx_Status.SelectedIndex == 0 || CBx_Status.SelectedIndex == -1)
                    await _LoadRentals();
                else
                {
                    List<DTO.Rental_DTO>? rentals = null;
                    bool RequestSucceeded = false;

                    await ErrorHandler.ExecuteAsync(async () =>
                    {
                        rentals = await _RentalApiService.FilterRentalsByRentalStatus(CBx_Status.SelectedItem.ToString()!);
                        RequestSucceeded = true;
                    });

                    if (RequestSucceeded)
                        _LoadDataToForm(rentals);
                }
            }
        }
        private async void txtbx_Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbx_Filter.Text))
            {
                await _LoadRentals();
            }
            else
            {
                btn_Clear.Visibility = Visibility.Visible;

                List<DTO.Rental_DTO>? rentals = null;
                bool RequestSucceeded = false;

                await ErrorHandler.ExecuteAsync(async () =>
                {
                    switch (CBx_FilterBy.SelectedIndex)
                    {
                        case 1:
                            rentals = await _RentalApiService.FilterRentalsByRentalCode(txtbx_Filter.Text);
                            break;
                        case 2:
                            rentals = await _RentalApiService.FilterRentalsByPlateNumber(txtbx_Filter.Text);
                            break;
                        case 3:
                            rentals = await _RentalApiService.FilterRentalsByNationalNumber(txtbx_Filter.Text);
                            break;
                        default:
                            return;
                    }

                    RequestSucceeded = true;
                });

                if (RequestSucceeded)
                    _LoadDataToForm(rentals);
            }
        }
        private async void _RefreshPage()
        {
            if (CBx_FilterBy.SelectedIndex == 0)
                await _LoadRentals();
            else if (CBx_FilterBy.SelectedIndex == 4)
                CBx_Status_SelectionChanged(this, null!);
            else
                CBx_FilterBy_SelectionChanged(this, null!);

            FilterGird.Visibility = Visibility.Visible;
        }
    }
}