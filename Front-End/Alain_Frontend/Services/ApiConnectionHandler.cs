using System.Windows;

namespace project_03.Services
{
    public static class ApiConnectionHandler
    {
        private static readonly object _lock = new object();
        private static DateTime _lastNotification = DateTime.MinValue;
        private static readonly TimeSpan NotificationInterval = TimeSpan.FromSeconds(30);
        public static void ConnectionLost()
        {
            lock (_lock)
            {
                if (DateTime.UtcNow - _lastNotification < NotificationInterval)
                    return;

                _lastNotification = DateTime.UtcNow;
            }

            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(
                    "Impossible de se connecter à l'API.",
                    "Erreur de connexion",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            });
        }
    }
}