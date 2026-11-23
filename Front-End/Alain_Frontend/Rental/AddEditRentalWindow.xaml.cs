using project_03.Services;
using System.Windows;
using System.Windows.Input;

namespace project_03.Rental
{
    /// <summary>
    /// Interaction logic for AddEditRentalWindow.xaml
    /// </summary>
    public partial class AddEditRentalWindow : Window
    {
        private bool _isEditMode;
        private Model.Rental? _Rental;
        private Model.Payment? _Payment;
        private readonly RentalApiService _RentalApiService;
        private readonly PaymentApiService _PaymentApiService;
        private readonly CarApiService _CarApiService;
        private readonly CustomerApiService _CustomerApiService;
        public EventHandler? onRefresh;
        private int _RentalId;
        private int _PaymentId;
        public AddEditRentalWindow()
        {
            InitializeComponent();
            _isEditMode = false;
            txtTitle.Text = "Nouveau Contrat de Location";

            _Rental = null;
            _Payment = null;

            ApiClient client = new ApiClient();
            _CarApiService = new CarApiService(client);
            _CustomerApiService = new CustomerApiService(client);
            _RentalApiService = new RentalApiService(client);
            _PaymentApiService = new PaymentApiService(client);

            carSearchControl.CarApiService = _CarApiService;
            customerSearchControl.CustomerApiService = _CustomerApiService;
            
            _RentalId = 0;

            RentalInfoControl.SetAddNew();
        }
        public AddEditRentalWindow(Model.Rental existingRental)
        {
            InitializeComponent();
            _Rental = existingRental;
            _isEditMode = true;
            txtTitle.Text = "Modifier le Contrat de Location";
            
            _RentalId = existingRental.RentalID;

            ApiClient client = new ApiClient();
            _RentalApiService = new RentalApiService(client);
            _PaymentApiService = new PaymentApiService(client);
            _CustomerApiService = new CustomerApiService(client);
            _CarApiService = new CarApiService(client);

            customerSearchControl.CustomerApiService = _CustomerApiService;

            RentalInfoControl.LoadDataToForm(existingRental);
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isEditMode && _Rental != null)
            {
                Model.Car? car = null;
                Model.Customer? customer = null;
                bool RequestSucceeded = false;

                await ErrorHandler.ExecuteAsync(async () =>
                {
                    var FindCarTask = _CarApiService.GetCarInfoByCarID(_Rental.CarID);
                    var FindCustomerTask = _CustomerApiService.GetCustomerInfoByCustomerID(_Rental.CustomerID);
                    var FindPaymentTask = _PaymentApiService.GetPaymentInfoByRentalID(_Rental.RentalID);

                    await Task.WhenAll(FindCarTask, FindCustomerTask, FindPaymentTask);

                    car = await FindCarTask;
                    customer = await FindCustomerTask;
                    _Payment = await FindPaymentTask;

                    RequestSucceeded = true;
                });

                if (!RequestSucceeded)
                    return;

                if (car == null)
                {
                    CustomMessageBox.Show("Aucun véhicule n'a été trouvé.", "Véhicule Introuvable",
                                    MessageBoxButton.OK, CustomMessageBoxImage.Warning);
                    Close();
                    return;
                }

                if (customer == null)
                {
                    MessageBox.Show("Aucun client n'a été trouvé avec l'identifiant ou la référence spécifiée.",
                                "Client Introuvable", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Close();
                    return;
                }

                carSearchControl.LoadCar(car);
                customerSearchControl.LoadCustomer(customer);

                if (_Payment != null)
                {
                    RentalInfoControl.LoadRentalInfo(_Rental);
                    RentalInfoControl.LoadPaymentInfo(_Payment);
                    _PaymentId = _Payment.PaymentID;
                }
            }
        }
        private void Toolbar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _Rental = _LoadCustomerAndCarInfoToRentalObject(_RentalId);
            try
            {
                if (_Rental != null)
                {
                    _Payment = RentalInfoControl.ValidateAndGetPayment(_Rental.RentalID, _PaymentId);

                    if (_Payment != null)
                    {
                        bool SaveSucceeded = false;

                        await ErrorHandler.ExecuteAsync(async () =>
                        {
                            if (_isEditMode)
                            {
                                await _RentalApiService.EditRental(_Rental);
                                await _PaymentApiService.EditPayment(_Payment);
                            }
                            else
                            {
                                _Rental = await _RentalApiService.AddNewRental(_Rental);

                                if (_Rental == null)
                                    throw new Exception();

                                RentalInfoControl.cbStatus.IsEnabled = true;

                                _Payment.RentalID = _Rental.RentalID;

                                int? PaymentID = await _PaymentApiService.AddNewPayment(_Payment);

                                if (PaymentID != null && PaymentID >= 0)
                                    _Payment.PaymentID = PaymentID.Value;

                                RentalInfoControl.txtContractRef.Text = _Rental.RentalCode;
                                RentalInfoControl.cbStatus.IsEnabled = true;
                                txtTitle.Text = "Modifier le Contrat de Location";
                            }
                            SaveSucceeded = true;
                        });
                        if (!SaveSucceeded)
                            return;

                        string paymentDetailsMsg = "";

                        if (RentalInfoControl.cbPaymentStatus.SelectedIndex == 1 &&
                            RentalInfoControl.cbStatus.SelectedIndex == 1)
                        {
                            paymentDetailsMsg = $"• Statut de paiement : Avance\n• Montant de l'avance : {RentalInfoControl.txtAdvanceAmount.Text:F2} DH / {RentalInfoControl.txtTotalPrice.Text:F2} DH";
                        }

                        _RentalId = _Rental.RentalID;
                        _isEditMode = true;

                        CustomMessageBox.Show($"Le contrat de location a été enregistré avec succès !\n{paymentDetailsMsg}",
                                              "Succès", MessageBoxButton.OK, CustomMessageBoxImage.Information, owner: this);

                        onRefresh?.Invoke(this, EventArgs.Empty);
                    }
                }
                else
                    tabControl.SelectedIndex = 2;
            }
            catch
            {
                CustomMessageBox.Show(
                                "Erreur lors de l'enregistrement dans la base de données", "Erreur Système",
                                MessageBoxButton.OK, CustomMessageBoxImage.Error);
            }
        }
        private Model.Rental? _LoadCustomerAndCarInfoToRentalObject()
        {
            return _LoadCustomerAndCarInfoToRentalObject(0);
        }
        private Model.Rental? _LoadCustomerAndCarInfoToRentalObject(int RentalID)
        {
            Model.Car? car = carSearchControl.Car;
            if (car == null)
            {
                CustomMessageBox.Show("Veuillez rechercher et sélectionner un véhicule valide.",
                        "Véhicule Manquant", MessageBoxButton.OK, CustomMessageBoxImage.Warning);
                tabControl.SelectedIndex = 0;
                return null;
            }
            Model.Customer? customer = customerSearchControl.Customer;
            if (customer == null)
            {
                CustomMessageBox.Show("Veuillez rechercher et sélectionner un client valide.",
                    "Client Manquant", MessageBoxButton.OK, CustomMessageBoxImage.Warning);
                tabControl.SelectedIndex = 1;
                return null;
            }
            _Rental = RentalInfoControl.ValidateAndGetRentalInfo();
            if (_Rental == null)
                return null;

            _Rental.RentalID = RentalID;
            _Rental.CarID = car.Id;
            _Rental.StartMileage = car.Mileage;
            _Rental.CustomerID = customer.Id;
            _Rental.DailyPrice = car.DailyRentPrice;

            string actionText = _isEditMode ? "modifier ce contrat" : "enregistrer ce nouveau contrat";
            MessageBoxResult result = CustomMessageBox.Show(
                $"Êtes-vous sûr de vouloir {actionText} pour le client {customer.FirstName + " " + customer.LastName} (Véhicule : {car.Plate}) ?",
                "Confirmation de l'enregistrement", MessageBoxButton.YesNo, CustomMessageBoxImage.Question);

            return (result == MessageBoxResult.OK)? _Rental : null;
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}