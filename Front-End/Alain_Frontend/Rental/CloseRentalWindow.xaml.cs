using project_03.Model;
using project_03.Services;
using System.Windows;

namespace project_03.Rental
{
    /// <summary>
    /// Interaction logic for CloseRentalWindow.xaml
    /// </summary>
    public partial class CloseRentalWindow : Window
    {
        private PaymentApiService _PaymentApiService;
        private RentalApiService _RentalApiService;
        private Model.Rental _Rental;
        private Model.Payment? _Payment;
        public CloseRentalWindow(Model.Rental rental)
        {
            InitializeComponent();
            
            _Rental = rental;

            dpActualEndDate.DisplayDateStart = DateTime.Now.AddDays(-7);
            dpActualEndDate.DisplayDateEnd = DateTime.Now.AddDays(3);
            dpActualEndDate.SelectedDate = DateTime.Today;

            ApiClient apiClient = new ApiClient();
            _PaymentApiService = new PaymentApiService(apiClient);
            _RentalApiService = new RentalApiService(apiClient);
        }
        private async void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validate Return Date
            if (!dpActualEndDate.SelectedDate.HasValue)
            {
                CustomMessageBox.Show("Veuillez sélectionner la date de retour effective.", "Date Requise",
                    MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                dpActualEndDate.Focus();
                return;
            }

            // 2. Validate End Mileage
            if (!int.TryParse(txtEndMileage.Text.Trim(), out int endMileage) || endMileage < 0)
            {
                CustomMessageBox.Show("Veuillez saisir un kilométrage de retour valide.", "Kilométrage Invalide",
                    MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                txtEndMileage.Focus();
                return;
            }

            // 3. Ensure end mileage is not less than start mileage
            if (endMileage < _Rental.StartMileage)
            {
                CustomMessageBox.Show($"Le kilométrage de retour ({endMileage} km) ne peut pas être inférieur au kilométrage de départ ({_Rental.StartMileage} km).",
                    "Erreur de Kilométrage", MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                txtEndMileage.Focus();
                txtEndMileage.SelectAll();
                return;
            }

            _Rental.EndMileage = endMileage;
            _Rental.EndDate = dpActualEndDate.SelectedDate.Value;
            _Rental.Status = enRentalStatus.TERMINEE;

            if (CustomMessageBox.Show("Êtes-vous sûr de vouloir enregistrer ce contrat de location ?",
                "Confirmation d'enregistrement", MessageBoxButton.YesNo, CustomMessageBoxImage.Question,
                owner: this) != MessageBoxResult.OK)
                return;

            bool RequestSucceeded = false;

            await ErrorHandler.ExecuteAsync(async () =>
            {
                await _RentalApiService.EditRental(_Rental);
                RequestSucceeded = true;
            });

            if (!RequestSucceeded)
                return;

            CustomMessageBox.Show("Le contrat a été enregistré avec succès.", "Succès",
                MessageBoxButton.OK, CustomMessageBoxImage.Information);

            Close();
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                _Payment = await _PaymentApiService.GetPaymentInfoByRentalID(_Rental.RentalID);                    
            });

            if (_Payment != null)
            {
                txtTotal.Text = $"{_Payment.TotalAmount.ToString("F2")} MAD";
                if (_Payment.AmountPaid > 0)
                {
                    AvancementPanel.Visibility = Visibility.Visible;
                    txtAvancment.Text = $"{_Payment.AmountPaid.ToString("F2")} MAD";
                }
            }
        }
    }
}