using MahApps.Metro.IconPacks;
using project_03.Model;
using project_03.Services;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace project_03.Car
{
    /// <summary>
    /// Interaction logic for AddEditCarWindow.xaml
    /// </summary>
    public partial class AddEditCarWindow : Window
    {
        public EventHandler? onRefresh;
        private readonly CarApiService _carService;
        private readonly BrandApiService _brandService;
        private Model.Car? _Car;
        private string? _PlateNumber;
        private enum enMode : byte { enAddNew,enUpdate }
        private enMode _Mode;
        public AddEditCarWindow(CarApiService carService, BrandApiService brandService)
        {
            InitializeComponent();
            _Car = null;
            _carService = carService;
            _brandService = brandService;
            _Mode = enMode.enAddNew;
        }
        public AddEditCarWindow(CarApiService carService, BrandApiService brandService, Model.Car car)
        {
            InitializeComponent();
            
            _Car = car;
            _PlateNumber = _Car.Plate;
            _carService = carService;
            _brandService = brandService;
            _Mode = enMode.enUpdate;
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_Car != null)
            {
                WindowTitle.Text = "Modifier la voiture";
                this.Title = "Modifier la voiture";

                txtBx_Plate.Text = _Car.Plate;
                txtBx_Price.Text = _Car.DailyRentPrice.ToString("0.##");
                Cb_Brand.SelectedValue = _Car.BrandName;
                txtBx_Seats.Text = _Car.NumberOfSeats.ToString();
                txtBx_Model.Text = _Car.Model;
                txtBx_Doors.Text = _Car.NumberOfDoors.ToString();
                txtBx_Mileage.Text = _Car.Mileage.ToString();
                Cbx_Status.SelectedIndex = (byte)_Car.CarStatus - 1;
                Cbx_BodyType.SelectedIndex = (byte)_Car.BodyType - 1;
                txtBx_Year.Text = _Car.ManufactureYear.ToString();
                Cb_FuelType.SelectedIndex = (byte)_Car.FuelType - 1;
                Cb_Transmission.SelectedIndex = (byte)_Car.Transmission - 1;
            }
            else
            {
                _Car = new Model.Car();
            }
            await ErrorHandler.ExecuteAsync(async () =>
            {
                Cb_Brand.ItemsSource = await _brandService.GetAllBrands();
            });
        }
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private async void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (CustomMessageBox.Show(
                "Souhaitez-vous enregistrer les informations de ce véhicule ?",
                "Confirmer l'ajout",
                MessageBoxButton.YesNo,
                CustomMessageBoxImage.Question,
                "Enregistrer",
                "Annuler",
                primaryIcon: PackIconMaterialKind.ContentSave,
                secondaryIcon: PackIconMaterialKind.Cancel,
                owner: this) != MessageBoxResult.OK)
            {
                return;
            }

            if (!ValidateCarForm())
                return;

            await ErrorHandler.ExecuteAsync(async () =>
            {
                if (_Mode == enMode.enAddNew || _PlateNumber != null)
                {
                    if (await _carService.CheckIfPlateAlreadyExist(txtBx_Plate.Text))
                    {
                        CustomMessageBox.Show("Ce numéro de plaque d'immatriculation est déjà enregistré dans le système.",
                            "Plaque d'immatriculation déjà existante",
                            MessageBoxButton.OK,
                            CustomMessageBoxImage.Warning,
                            owner: this);

                        txtBx_Plate.Focus();
                        return;
                    }
                }

                if (await _SaveCar())
                {
                    onRefresh?.Invoke(this, new EventArgs());

                    CustomMessageBox.Show(
                        $"Le véhicule {_Car!.BrandName} {_Car.Model} [{_Car.Plate}] a été enregistré avec succès.",
                        "Véhicule enregistré",
                        MessageBoxButton.OK,
                        CustomMessageBoxImage.Information,
                        "Ok");

                    WindowTitle.Text = "Modifier la voiture";
                    this.Title = "Modifier la voiture";
                }
                else
                {
                    CustomMessageBox.Show(
                        "Impossible d'enregistrer le véhicule. Veuillez vérifier les informations saisies et réessayer.",
                        "Échec de l'enregistrement",
                        MessageBoxButton.OK,
                        CustomMessageBoxImage.Error,
                        "Ok");
                }
            });
        }
        private bool ValidateCarForm()
        {
            if(_Car == null)
                _Car = new Model.Car();

            if (string.IsNullOrWhiteSpace(txtBx_Plate.Text))
            {
                CustomMessageBox.Show("Veuillez saisir un numéro de plaque d'immatriculation valide.", "Erreur de validation", 
                    MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                txtBx_Plate.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(Cb_Brand.Text))
            {
                CustomMessageBox.Show("Veuillez saisir la marque du véhicule.", "Erreur de validation", 
                    MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                Cb_Brand.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtBx_Model.Text))
            {
                CustomMessageBox.Show("Veuillez saisir le modèle du véhicule.", "Erreur de validation",
                    MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                txtBx_Model.Focus();
                return false;
            }

            if (!int.TryParse(txtBx_Year.Text.Trim(), out int year) || year < 1900)
            {
                CustomMessageBox.Show($"Veuillez saisir une année de fabrication valide (entre 1900 et {DateTime.Now.Year + 1}).", "Erreur de validation",
                    MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                txtBx_Year.Focus();
                return false;
            }

            // 3. ComboBox Selections
            if (Cb_FuelType.SelectedItem == null)
            {
                CustomMessageBox.Show("Veuillez sélectionner un type de carburant.", "Erreur de validation",
                    MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                Cb_FuelType.Focus();
                return false;
            }

            if (Cb_Transmission.SelectedItem == null)
            {
                CustomMessageBox.Show("Veuillez sélectionner un type de transmission.", "Erreur de validation", 
                    MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                Cb_Transmission.Focus();
                return false;
            }

            if (Cbx_BodyType.SelectedItem == null)
            {
                CustomMessageBox.Show("Veuillez sélectionner un type de carrosserie.", "Erreur de validation",
                    MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                Cbx_BodyType.Focus();
                return false;
            }

            // 4. Seats Validation
            if (!int.TryParse(txtBx_Seats.Text.Trim(), out int seats) || seats <= 0 || seats > 15)
            {
                CustomMessageBox.Show("Veuillez saisir un nombre de places valide (entre 1 et 15).", 
                    "Erreur de validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                txtBx_Seats.Focus();
                return false;
            }

            // 5. Doors Validation
            if (!int.TryParse(txtBx_Doors.Text.Trim(), out int doors) || doors <= 0 || doors > 6)
            {
                CustomMessageBox.Show("Veuillez saisir un nombre de portes valide (entre 1 et 6).", 
                    "Erreur de validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                txtBx_Doors.Focus();
                return false;
            }

            // 6. Mileage Validation
            if (!decimal.TryParse(txtBx_Mileage.Text.Trim(), out decimal mileage) || mileage < 0)
            {
                CustomMessageBox.Show("Veuillez saisir un kilométrage valide (ne peut pas être négatif).", 
                    "Erreur de validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                txtBx_Mileage.Focus();
                return false;
            }

            // 7. Status Selection
            if (Cbx_Status.SelectedItem == null)
            {
                CustomMessageBox.Show("Veuillez sélectionner le statut du véhicule.", "Erreur de validation",
                    MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                Cbx_Status.Focus();
                return false;
            }

            // 8. Daily Rent Price Validation
            if (!decimal.TryParse(txtBx_Price.Text.Trim(), out decimal price) || price <= 0)
            {
                CustomMessageBox.Show("Veuillez saisir un prix de location journalier valide supérieur à zéro.", 
                    "Erreur de validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                txtBx_Price.Focus();
                return false;
            }
            _Car.Plate = txtBx_Plate.Text.ToUpper().Trim();
            _Car.DailyRentPrice = Convert.ToDecimal(txtBx_Price.Text);
            _Car.BrandName = Cb_Brand.SelectedValue.ToString()!;
            _Car.NumberOfDoors = Convert.ToByte(txtBx_Doors.Text);
            _Car.NumberOfSeats = Convert.ToByte(txtBx_Seats.Text);
            _Car.Model = txtBx_Model.Text;
            _Car.ManufactureYear = Convert.ToInt16(txtBx_Year.Text);
            _Car.FuelType = (Model.enFuelType)(Cb_FuelType.SelectedIndex + 1);
            _Car.Transmission = (Model.enTransmission)(Cb_Transmission.SelectedIndex + 1);
            _Car.Mileage = Convert.ToInt32(txtBx_Mileage.Text);
            _Car.CarStatus = (Model.enCarStatus)(Cbx_Status.SelectedIndex + 1);
            _Car.BodyType = (Model.enBodyType)(Cbx_BodyType.SelectedIndex + 1);
            _Car.BrandName = Cb_Brand.SelectedValue.ToString()!;
            _Car.DailyRentPrice = Convert.ToDecimal(txtBx_Price.Text);
            _Car.BrandName = Cb_Brand.SelectedValue.ToString()!;
            _Car.NumberOfDoors = Convert.ToByte(txtBx_Doors.Text);
            _Car.NumberOfSeats = Convert.ToByte(txtBx_Seats.Text);
            _Car.Model = txtBx_Model.Text;
            _Car.ManufactureYear = Convert.ToInt16(txtBx_Year.Text);
            _Car.FuelType = (Model.enFuelType)(Cb_FuelType.SelectedIndex + 1);
            _Car.Transmission = (Model.enTransmission)(Cb_Transmission.SelectedIndex + 1);
            _Car.Mileage = Convert.ToInt32(txtBx_Mileage.Text);
            _Car.CarStatus = (Model.enCarStatus)(Cbx_Status.SelectedIndex + 1);
            _Car.BodyType = (Model.enBodyType)(Cbx_BodyType.SelectedIndex + 1);

            return true;
        }
        private async Task<bool> _SaveCar()
        {
            if (_Mode == enMode.enAddNew)
            {
                int? Id = await _carService.AddNewCar(_Car!);

                if (Id != null)
                    _Car!.Id = Id.Value;

                _Mode = enMode.enUpdate;
                return true;
            }
            else
            {
                await _carService.EditCar(_Car!);
                return true;
            }
        }
        private void NumberTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }
        private void NumberTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string? text = e.DataObject.GetData(DataFormats.Text) as string;

                if (string.IsNullOrEmpty(text) || !text.All(char.IsDigit))
                    e.CancelCommand();
            }
            else
                e.CancelCommand();
        }
        private void NumberTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = (e.Key == Key.Space);
        }
    }
}