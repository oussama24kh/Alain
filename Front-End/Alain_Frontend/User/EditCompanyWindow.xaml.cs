using project_03.DTO;
using project_03.Model;
using project_03.Services;
using System.Text.RegularExpressions;
using System.Windows;

namespace project_03.User
{
    /// <summary>
    /// Interaction logic for EditCompanyWindow.xaml
    /// </summary>
    public partial class EditCompanyWindow : Window
    {
        public EventHandler<Company>? onRefresh;
        public Company ResultCompany { get; private set; }
        private readonly UserApiService _userApiService;
        public EditCompanyWindow(UserApiService userApiService, Company? currentCompany = null)
        {
            InitializeComponent();

            _userApiService = userApiService;
            ResultCompany = currentCompany != null
                ? new Company
                {
                    Id = 1,
                    Name = currentCompany.Name,
                    Email = currentCompany.Email,
                    Phone = currentCompany.Phone,
                    Address = currentCompany.Address,
                    City = currentCompany.City
                }
                : new Company();

            txtBx_Name.Text = ResultCompany.Name;
            txtBx_Email.Text = ResultCompany.Email;
            txtBx_Phone.Text = ResultCompany.Phone;
            txtBx_Address.Text = ResultCompany.Address;
            txtBx_City.Text = ResultCompany.City;

            Loaded += (_, _) => txtBx_Name.Focus();
        }
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            ErrorBanner.Visibility = Visibility.Collapsed;

            string name = txtBx_Name.Text.Trim();
            string email = txtBx_Email.Text.Trim();
            string phone = txtBx_Phone.Text.Trim();
            string address = txtBx_Address.Text.Trim();
            string city = txtBx_City.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(address) ||
                string.IsNullOrWhiteSpace(city))
            {
                ShowError("Veuillez remplir tous les champs obligatoires (*).");
                return;
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ShowError("Veuillez saisir une adresse e-mail valide.");
                txtBx_Email.Focus();
                return;
            }

            MessageBoxResult confirm = CustomMessageBox.Show(
                $"Voulez-vous vraiment enregistrer les informations de l'entreprise « {name} » ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                CustomMessageBoxImage.Question
            );

            if (confirm != MessageBoxResult.OK)
                return;

            ResultCompany.Name = name;
            ResultCompany.Email = email;
            ResultCompany.Phone = phone;
            ResultCompany.Address = address;
            ResultCompany.City = city;

            await ErrorHandler.ExecuteAsync(async () =>
            {
                if (await _userApiService.SaveCompanySettings(1, ResultCompany))
                {
                    onRefresh?.Invoke(this, ResultCompany);
                    CustomMessageBox.Show(
                        "Les informations de l'entreprise ont été enregistrées avec succès.",
                        "Enregistrement réussi",
                        MessageBoxButton.OK,
                        CustomMessageBoxImage.Information, owner: this
                    );
                }
                else
                {
                    CustomMessageBox.Show(
                        "Impossible d'enregistrer les informations de l'entreprise. Veuillez réessayer.",
                        "Échec de l'enregistrement",
                        MessageBoxButton.OK,
                        CustomMessageBoxImage.Error, owner: this
                    );
                }
            });
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ShowError(string message)
        {
            TxtError.Text = message;
            ErrorBanner.Visibility = Visibility.Visible;
        }
    }
}
