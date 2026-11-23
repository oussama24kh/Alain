using MahApps.Metro.IconPacks;
using project_03.Services;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace project_03.Customer
{
    /// <summary>
    /// Interaction logic for AddEditCustomerWindow.xaml
    /// </summary>
    public partial class AddEditCustomerWindow : Window
    {
        public EventHandler? onRefresh;
        public Model.Customer? _Customer { get; private set; }
        private bool _isEditMode { get; set; }
        private readonly CountryApiService _CountryApiService;
        private readonly CustomerApiService _CustomerApiService;
        public AddEditCustomerWindow(CustomerApiService customerApiService,CountryApiService countryApiService)
        {
            InitializeComponent();
            _CountryApiService = countryApiService;

            _isEditMode = false;
            WindowTitleText.Text = "Ajouter un Nouveau Client";
            TxtSaveButton.Text = "Ajouter Client";
            IconSave.Kind = PackIconMaterialKind.AccountPlus;
            CmbNationality.SelectedIndex = 0;
            _Customer = null;
            _CustomerApiService = customerApiService;
        }

        public AddEditCustomerWindow(Model.Customer existingCustomer, CustomerApiService customerApiService,
            CountryApiService countryApiService) : this(customerApiService, countryApiService)
        {
            if (existingCustomer != null)
            {
                _isEditMode = true;
                WindowTitleText.Text = "Modifier le Profil Client";
                TxtSaveButton.Text = "Enregistrer";
                IconSave.Kind = PackIconMaterialKind.ContentSave;

                _Customer = existingCustomer;
                PopulateFormFields();
            }
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                CmbNationality.ItemsSource = await _CountryApiService.GetAllCountries();
                CmbNationality.SelectedValue = "Maroc";
            });

            DpDateOfBirth.DisplayDateStart = DateTime.Now.AddYears(-100);
            DpDateOfBirth.DisplayDateEnd = DateTime.Now.AddYears(-18);
        }
        private void PopulateFormFields()
        {
            TxtNationalNumber.Text = _Customer!.NationalNo;
            TxtFirstName.Text = _Customer.FirstName;
            TxtLastName.Text = _Customer.LastName;
            TxtPhone.Text = _Customer.Phone;
            TxtEmail.Text = _Customer.Email;
            TxtAddress.Text = _Customer.Address;

            SelectComboBoxItem(CmbNationality, _Customer.Nationality);

            if (_Customer.DateOfBirth != default)
            {
                DpDateOfBirth.SelectedDate = _Customer.DateOfBirth;
            }
        }
        private bool ValidateCustomerForm(out Model.Customer validatedCustomer)
        {
            validatedCustomer = new Model.Customer();

            // 1. National Number (CIN) Validation
            if (string.IsNullOrWhiteSpace(TxtNationalNumber.Text))
            {
                CustomMessageBox.Show("Veuillez saisir le numéro national (CIN).", "Erreur de validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                TxtNationalNumber.Focus();
                return false;
            }

            // 2. First Name Validation
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text))
            {
                CustomMessageBox.Show("Veuillez saisir le prénom du client.", "Erreur de validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                TxtFirstName.Focus();
                return false;
            }

            // 3. Last Name Validation
            if (string.IsNullOrWhiteSpace(TxtLastName.Text))
            {
                CustomMessageBox.Show("Veuillez saisir le nom du client.", "Erreur de validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                TxtLastName.Focus();
                return false;
            }

            // 4. Phone Validation
            if (string.IsNullOrWhiteSpace(TxtPhone.Text))
            {
                CustomMessageBox.Show("Veuillez saisir le numéro de téléphone.", "Erreur de validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                TxtPhone.Focus();
                return false;
            }

            // 5. Email Validation
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!string.IsNullOrWhiteSpace(TxtEmail.Text) && !Regex.IsMatch(TxtEmail.Text.Trim(), emailPattern))
            {
                CustomMessageBox.Show("Veuillez saisir une adresse email valide.", "Erreur de validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                TxtEmail.Focus();
                return false;
            }

            // 6. Date of Birth Validation
            if (!DpDateOfBirth.SelectedDate.HasValue)
            {
                CustomMessageBox.Show("Veuillez sélectionner la date de naissance.", "Erreur de validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                DpDateOfBirth.Focus();
                return false;
            }

            // 7. Nationality ComboBox Validation
            if (CmbNationality.SelectedItem == null)
            {
                CustomMessageBox.Show("Veuillez sélectionner une nationalité.", "Erreur de validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning, owner: this);
                CmbNationality.Focus();
                return false;
            }

            validatedCustomer.NationalNo = TxtNationalNumber.Text.Trim().ToUpper();
            validatedCustomer.FirstName = TxtFirstName.Text.Trim();
            validatedCustomer.LastName = TxtLastName.Text.Trim();
            validatedCustomer.Phone = TxtPhone.Text.Trim();
            validatedCustomer.Email = TxtEmail.Text.Trim();
            validatedCustomer.DateOfBirth = DpDateOfBirth.SelectedDate.Value;
            validatedCustomer.Nationality = CmbNationality.SelectedValue.ToString()!;
            validatedCustomer.Address = TxtAddress.Text.Trim();
            validatedCustomer.TotalRentals = _isEditMode && _Customer != null ? _Customer.TotalRentals : 0;

            return true;
        }
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateCustomerForm(out Model.Customer validCustomer))
                return;

            bool NationalNumberExists = false;

            await ErrorHandler.ExecuteAsync(async () =>
            {
                NationalNumberExists = await _CheckIfNationalNumberAlreadyExist(TxtNationalNumber.Text);
            });

            if (NationalNumberExists)
            {
                CustomMessageBox.Show("Ce numéro de carte nationale est déjà enregistré dans le système.",
                    "Numéro de carte nationale déjà existant", MessageBoxButton.OK,
                    CustomMessageBoxImage.Warning, owner: this);
                return;
            }

            if (_Customer == null)
            {
                _Customer = validCustomer;
            }
            else
            {
                _Customer.NationalNo = validCustomer.NationalNo;
                _Customer.FirstName = validCustomer.FirstName;
                _Customer.LastName = validCustomer.LastName;
                _Customer.Phone = validCustomer.Phone;
                _Customer.Email = validCustomer.Email;
                _Customer.DateOfBirth = validCustomer.DateOfBirth;
                _Customer.Nationality = validCustomer.Nationality;
                _Customer.Address = validCustomer.Address;
            }

            if (CustomMessageBox.Show("Voulez-vous vraiment enregistrer les modifications apportées à ce client ?",
                "Confirmation d'enregistrement", MessageBoxButton.YesNo, CustomMessageBoxImage.Question,
                "Enregistrer", "Annuler", primaryIcon: PackIconMaterialKind.ContentSave,
                secondaryIcon: PackIconMaterialKind.Cancel) == MessageBoxResult.OK)
            {
                bool Saved = false;

                await ErrorHandler.ExecuteAsync(async () =>
                {
                    Saved = await _SaveCustomer();
                });

                if (Saved)
                {
                    WindowTitleText.Text = "Modifier le Profil Client";
                    TxtSaveButton.Text = "Enregistrer";
                    IconSave.Kind = PackIconMaterialKind.ContentSave;

                    CustomMessageBox.Show("Le client a été enregistré avec succès.",
                        "Enregistrement réussi", MessageBoxButton.OK,
                        CustomMessageBoxImage.Information, "Ok");
                }

                _isEditMode = true;
            }
        }
        private Task<bool> _CheckIfNationalNumberAlreadyExist(string NationalNumber)
        {
            if (_isEditMode)
            {
                if (_Customer!.NationalNo.ToUpper() == NationalNumber.ToUpper())
                    return Task.FromResult(false);
            }
            return _CustomerApiService.CheckIfNationalNumberAlreadyExist(NationalNumber);
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void SelectComboBoxItem(ComboBox comboBox, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Content?.ToString() == value)
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }
        private async Task<bool> _SaveCustomer()
        {
            if (_isEditMode)
            {
                await _CustomerApiService.EditCustomer(_Customer!);
                onRefresh?.Invoke(this, EventArgs.Empty);
                return true;
            }
            else
            {
                int? Id = await _CustomerApiService.AddNewCustomer(_Customer!);

                if (Id != null && Id > 0)
                    _Customer!.Id = Id.Value;

                _isEditMode = true;
                onRefresh?.Invoke(this, EventArgs.Empty);
                return true;
            }
        }
    }
}