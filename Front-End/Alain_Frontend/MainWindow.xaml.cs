using project_03.Login;
using project_03.Services;
using project_03.Views;
using System.Windows;

namespace project_03
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApiClient _Client;
        private readonly LoginWindow _LoginWindow;
        public MainWindow(LoginWindow window)
        {
            InitializeComponent();

            _LoginWindow = window;
            if (App.AuthState.Role != "Administrateur")
            {
                rdbtn_Payments.Visibility = Visibility.Collapsed;
                rdbtn_Setting.Visibility = Visibility.Collapsed;
            }

            _Client = new ApiClient();
            rdbtn_DashBoard.Checked -= rdbtn_DashBoard_Checked;
            rdbtn_DashBoard.IsChecked = true;
            rdbtn_DashBoard.Checked += rdbtn_DashBoard_Checked;
        }
        private void rdbtn_DashBoard_Checked(object sender, RoutedEventArgs e)
        {
            if (MainContent.Content.GetType() != typeof(DashboardView))
            {
                DashboardView view = new DashboardView();
                view.onViewAllRentals += (sender, e) => rdbtn_Rentals.IsChecked = true;
                MainContent.Content = view;
            }
        }
        private void rdbtn_Fleet_Checked(object sender, RoutedEventArgs e)
        {
            if (MainContent.Content.GetType() != typeof(FleetView))
                MainContent.Content = new FleetView();
        }
        private void rdbtn_Clients_Checked(object sender, RoutedEventArgs e)
        {
            if (MainContent.Content.GetType() != typeof(Customer.CustomerView))
                MainContent.Content = new Customer.CustomerView();
        }
        private void rdbtn_Rentals_Checked(object sender, RoutedEventArgs e)
        {
            if (MainContent.Content.GetType() != typeof(Rental.RentalsView))
                MainContent.Content = new Rental.RentalsView();
        }
        private void rdbtn_Payments_Checked(object sender, RoutedEventArgs e)
        {
            if (MainContent.Content.GetType() != typeof(Payment.PaymentView))
                MainContent.Content = new Payment.PaymentView();
        }
        private void rdbtn_Setting_Checked(object sender, RoutedEventArgs e)
        {
            if (MainContent.Content.GetType() != typeof(User.SettingsView))
                MainContent.Content = new User.SettingsView();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DashboardView view = new DashboardView();
            view.onViewAllRentals += (sender, e) => rdbtn_Rentals.IsChecked = true;
            MainContent.Content = view;
        }
        private void btn_Logout_Click(object sender, RoutedEventArgs e)
        {
            App.AuthState.Clear();
            Application.Current.MainWindow = _LoginWindow;

            _LoginWindow.Show();
            _LoginWindow.txtBx_Username.Text = "";
            _LoginWindow.txtBx_Username.Focus();
            _LoginWindow.pwdBx_Password.Password = "";
            _LoginWindow.ErrorBanner.Visibility = Visibility.Collapsed;
            

            this.Closed -= Window_Closed;
            Close();
        }
        private void Window_Closed(object? sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}