//using project_03.DTO;
//using project_03.DTO.Auth;
//using project_03.Global_Classes;
using project_03.DTO.Auth;
using project_03.Services;
using System.Windows;
using System.Windows.Input;

namespace project_03.Login
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public static AuthService AuthService { get; private set; } = null!;
        public LoginWindow()
        {
            InitializeComponent();
            AuthService = new AuthService(new ApiClient());
            Loaded += LoginWindow_Loaded;
            BtnLogin.IsEnabled = true;
        }
        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            /*if (Properties.Settings.Default.RememberMe &&
                !string.IsNullOrWhiteSpace(Properties.Settings.Default.SavedUsername))
            {
                txtBx_Username.Text = Properties.Settings.Default.SavedUsername;
                chk_RememberMe.IsChecked = true;
                pwdBx_Password.Focus();
            }
            else
            {
                txtBx_Username.Focus();
            }*/
        }
        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnLogin_Click(sender, e);
            }
        }
        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            BtnLogin.IsEnabled = false;
            ErrorBanner.Visibility = Visibility.Collapsed;

            string username = txtBx_Username.Text.Trim();
            string password = pwdBx_Password.Password;

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Veuillez saisir votre identifiant.");
                txtBx_Username.Focus();
                BtnLogin.IsEnabled = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Veuillez saisir votre mot de passe.");
                pwdBx_Password.Focus();
                BtnLogin.IsEnabled = true;
                return;
            }
            BtnLogin.IsEnabled = false;

            try
            {
                var request = new LoginRequest
                {
                    Username = username,
                    Password = password
                };

                TokenResponse? result = await AuthService.LoginAsync(request);
                if (result == null || string.IsNullOrWhiteSpace(result.AccessToken))
                {
                    throw new Exception("Identifiant ou mot de passe incorrect.");
                }
                App.AuthState.SetTokens(result);
                MainWindow main = new MainWindow(this);
                Application.Current.MainWindow = main;

                main.Show();

                Hide();
            }
            catch
            {
                ShowError("Identifiant ou mot de passe incorrect.");
            }
            finally
            {
                BtnLogin.IsEnabled = true;
            }
        }
        private void ShowError(string message)
        {
            TxtError.Text = message;
            ErrorBanner.Visibility = Visibility.Visible;
        }
    }
}
