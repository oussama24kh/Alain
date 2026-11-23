using MahApps.Metro.IconPacks;
using project_03.DTO;
using project_03.Services;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace project_03.User
{
    /// <summary>
    /// Interaction logic for AddEditUserWindow.xaml
    /// </summary>
    public partial class AddEditUserWindow : Window
    {
        public EventHandler? onRefresh;
        public User_DTO ResultUser { get; private set; }
        private bool _isEditMode;
        private readonly UserApiService _UserApiService;
        public AddEditUserWindow(UserApiService userApiService)
        {
            InitializeComponent();
            _isEditMode = false;
            ResultUser = new User_DTO();
            _UserApiService = userApiService;
        }
        public AddEditUserWindow(UserApiService userApiService, User_DTO existingUser) : this(userApiService)
        {
            if (existingUser == null) return;

            _isEditMode = true;
            ResultUser = existingUser;

            TxtWindowTitle.Text = "Modifier l'utilisateur";
            TxtWindowSubtitle.Text = $"Mise à jour du compte de {existingUser.FullName}";
            HeaderIcon.Kind = PackIconMaterialKind.AccountEdit;
            BtnSave.Content = "Mettre à jour";

            TxtPasswordLabel.Text = "NOUVEAU MOT DE PASSE (OPTIONNEL)";
            TxtPasswordHint.Visibility = Visibility.Visible;

            txtBx_FirstName.Text = existingUser.FirstName;
            txtBx_LastName.Text = existingUser.LastName;
            txtBx_Username.Text = existingUser.UserName;
            txtBx_Email.Text = existingUser.Email;

            CmbRole.SelectedIndex = (existingUser.Role == "Administrateur") ? 0 : 1;
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Visibility = Visibility.Collapsed;

            string firstName = txtBx_FirstName.Text.Trim();
            string lastName = txtBx_LastName.Text.Trim();
            string username = txtBx_Username.Text.Trim();
            string email = txtBx_Email.Text.Trim();
            string password = pwdBx_Password.Password;
            string role = CmbRole.SelectedValue.ToString()!;
            role = role.Split(":")[1].Trim();
            // Validation
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
            {
                ShowError("Veuillez remplir tous les champs obligatoires (*).");
                return;
            }
            if (await _CheckIfUsernameExist(username))
            {
                ShowError("Cet identifiant est déjà utilisé par un autre utilisateur.");
                txtBx_Username.Focus();
                return;
            }            

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ShowError("Veuillez saisir une adresse e-mail valide.");
                return;
            }

            if (!_isEditMode && password.Length < 6 )
            {
                ShowError("Le mot de passe doit contenir au moins 6 caractères.");
                return;
            }

            if (_isEditMode && !string.IsNullOrEmpty(password) && password.Length < 6)
            {
                ShowError("Le nouveau mot de passe doit contenir au moins 6 caractères.");
                return;
            }
            if (_isEditMode && !string.IsNullOrEmpty(pwdBx_Password.Password))
                ResultUser.Password = password;

            // Map values back to DTO

            ResultUser.FirstName = firstName;
            ResultUser.LastName = lastName;
            ResultUser.UserName = username;
            ResultUser.Email = email;
            ResultUser.Role = role;
            

            string confirmMessage = _isEditMode
                ? $"Voulez-vous vraiment enregistrer les modifications pour {firstName} {lastName} ?"
                : $"Voulez-vous vraiment créer le compte de {firstName} {lastName} ?";

            if (CustomMessageBox.Show(confirmMessage, "Confirmation", MessageBoxButton.YesNo,
                CustomMessageBoxImage.Question, owner: this) == MessageBoxResult.OK)
            {
                await ErrorHandler.ExecuteAsync(async () =>
                {
                    if(await _SaveUser())
                    {
                        onRefresh?.Invoke(this, EventArgs.Empty);
                        TxtWindowTitle.Text = "Modifier l'utilisateur";
                        TxtWindowSubtitle.Text = $"Mise à jour du compte de {ResultUser.FullName}";
                        HeaderIcon.Kind = PackIconMaterialKind.AccountEdit;
                        BtnSave.Content = "Mettre à jour";

                        TxtPasswordLabel.Text = "NOUVEAU MOT DE PASSE (OPTIONNEL)";
                        TxtPasswordHint.Visibility = Visibility.Visible;

                        CustomMessageBox.Show("L'utilisateur a été enregistré avec succès.",
                        "Succès", MessageBoxButton.OK, CustomMessageBoxImage.Information, owner: this);
                    }
                });
            }
        }
        private async Task<bool> _SaveUser()
        {
            if (_isEditMode)
            {
                await _UserApiService.EditUser(ResultUser);
                return true;
            }
            else
            {
                ResultUser.Password = pwdBx_Password.Password;
                int? id = await _UserApiService.AddNewUser(ResultUser);
                if (id == null || id.Value < 1)
                    return false;

                ResultUser.Id = id.Value;
                _isEditMode = true;
                return true;
            }
        }
        private Task<bool> _CheckIfUsernameExist(string Username)
        {
            if (_isEditMode)
            {
                if (Username.Trim().ToUpper() != ResultUser.UserName.Trim().ToUpper())
                    return _UserApiService.CheckIfUsernameExist(Username);

                return Task.FromResult(false);
            }
            return _UserApiService.CheckIfUsernameExist(Username);
        }
        private void ShowError(string message)
        {
            TxtError.Text = message;
            TxtError.Visibility = Visibility.Visible;
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}