using System.Windows;

namespace project_03.Services
{
    public static class ErrorHandler
    {
        public static async Task ExecuteAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (ApiException)
            {
                ApiConnectionHandler.ConnectionLost();
            }
            catch (ApiConnectionException)
            {
                ApiConnectionHandler.ConnectionLost();
            }
            catch (TimeoutException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Délai d'attente dépassé",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            catch
            {
                MessageBox.Show(
                    "Une erreur inattendue s'est produite.",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}