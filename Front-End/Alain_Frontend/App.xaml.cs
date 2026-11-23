using project_03.DTO.Auth;
using project_03.Login;
using System.Configuration;
using System.Data;
using System.Windows;

namespace project_03
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static int _isRedirecting;

        public static void RedirectToLogin()
        {
            if (Interlocked.Exchange(ref _isRedirecting, 1) == 1)
                return;

            try
            {
                Current.Dispatcher.Invoke(() =>
                {
                    AuthState.Clear();

                    // Already on the login screen
                    if (Current.MainWindow is LoginWindow)
                        return;

                    var oldWindow = Current.MainWindow;

                    var loginWindow = new LoginWindow();

                    // Set the new main window BEFORE closing the old one
                    Current.MainWindow = loginWindow;

                    loginWindow.Show();

                    oldWindow?.Close();
                });
            }
            finally
            {
                Interlocked.Exchange(ref _isRedirecting, 0);
            }
        }
        public static AuthState AuthState { get; } = new AuthState();
    }
}