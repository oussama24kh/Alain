using project_03.DTO;
using project_03.Model;
using project_03.Rental;
using project_03.Services;
using System.Windows;
using System.Windows.Controls;

namespace project_03.User
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private PaymentApiService _PaymentApiService;
        private CarApiService _CarApiService;
        private UserApiService _UserApiService;
        public SettingsView()
        {
            InitializeComponent();

            ApiClient client = new ApiClient();
            _PaymentApiService = new PaymentApiService(client);
            _CarApiService = new CarApiService(client);
            _UserApiService = new UserApiService(client);
        }
        private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await _LoadData();
        }
        private async Task _LoadData()
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                Task<decimal> MonthRevenueTask = _PaymentApiService.GetRevenue(DateTime.Now.Year, DateTime.Now.Month);
                Task<int> CountAvailableCarTask = _CarApiService.CountCarsByStatus("DISPONIBLE");
                Task<int> CountRentedCarTask = _CarApiService.CountCarsByStatus("LOUE");
                Task<int> CountAllCarsTask = _CarApiService.CountAllCars();
                Task<Company?> GetCompanyInfoTask = _UserApiService.GetCompanyInfo();
                Task<Dictionary<string, int>?> statisticsTask = _UserApiService.GetUsersStatistics();
                Task<List<User_DTO>?> GetUsersListTask = _UserApiService.GetAllUsers();

                await Task.WhenAll(MonthRevenueTask, CountRentedCarTask, CountAvailableCarTask,
                    CountAllCarsTask, GetCompanyInfoTask, GetUsersListTask, statisticsTask);

                Dictionary<string, int>? dic = await statisticsTask;

                UsersGrid.ItemsSource = await GetUsersListTask;
                await _LoadCompanyInfo(await GetCompanyInfoTask);
                txtMonthRevenue.Text = $"{await MonthRevenueTask} MAD";
                txtRentedVehicles.Text = (await CountRentedCarTask).ToString();
                txtAvailableVehicles.Text = $"{await CountAvailableCarTask} / {await CountAllCarsTask}";
                if (dic != null)
                    txtStatistics.Text = $"{(dic["ActiveUsers"] + dic["InactiveUsers"])} membres de l'équipe · {dic["ActiveUsers"]} actifs, {dic["InactiveUsers"]} inactif";
            });
        }
        private async Task _LoadCompanyInfo(Company? company)
        {
            if (company == null)
                return;

            txtName.Text = company.Name;
            txtEmail.Text = company.Email;
            txtPhone.Text = company.Phone;
            txtAddress.Text = company.Address;
        }
        private async void btn_AddNewUser_Click(object sender, RoutedEventArgs e)
        {
            AddEditUserWindow window = new AddEditUserWindow(_UserApiService);
            window.onRefresh += async (sender, e) => await _LoadData();
            window.ShowDialog();
        }
        private async void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is DTO.User_DTO user)
            {
                User_DTO? usr = await _UserApiService.GetUserInfoByUsername(user.UserName);
                if(usr == null)
                {
                    CustomMessageBox.Show("L'utilisateur sélectionné est introuvable.",
                       "Utilisateur introuvable", MessageBoxButton.OK, CustomMessageBoxImage.Warning);
                    return;
                }
                AddEditUserWindow window = new AddEditUserWindow(_UserApiService, usr);
                if (Convert.ToInt32(App.AuthState.UserID) == user.Id)
                    window.CmbRole.IsEnabled = false;
                window.onRefresh = async (sender, e) => await _LoadData();
                window.ShowDialog();
            }
        }
        private async void BtnToggleBanUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.DataContext is not User_DTO selectedUser)
                return;

            if (selectedUser.Id == Convert.ToInt32(App.AuthState.UserID))
            {
                CustomMessageBox.Show(
                    "Vous ne pouvez pas bannir votre propre compte.",
                    "Action impossible",
                    MessageBoxButton.OK,
                    CustomMessageBoxImage.Warning
                );
                return;
            }

            bool isCurrentlyActive = selectedUser.IsActive;

            string confirmMessage = isCurrentlyActive
                ? $"Voulez-vous vraiment bannir l'utilisateur {selectedUser.FullName} ({selectedUser.UserName}) ?\nIl ne pourra plus se connecter à l'application."
                : $"Voulez-vous vraiment débannir l'utilisateur {selectedUser.FullName} ({selectedUser.UserName}) ?\nIl pourra à nouveau se connecter à l'application.";

            string confirmTitle = isCurrentlyActive
                ? "Confirmation du bannissement"
                : "Confirmation du débannissement";

            MessageBoxResult confirmResult = CustomMessageBox.Show(
                confirmMessage,
                confirmTitle,
                MessageBoxButton.YesNo,
                CustomMessageBoxImage.Warning
            );

            if (confirmResult != MessageBoxResult.OK)
                return;

            bool updated = await _UserApiService.ChangeUserStatus(selectedUser.UserName, isCurrentlyActive);

            if (!updated)
            {
                CustomMessageBox.Show(
                    "L'utilisateur sélectionné est introuvable ou a peut-être été supprimé.",
                    "Utilisateur introuvable",
                    MessageBoxButton.OK,
                    CustomMessageBoxImage.Warning
                );
                await _LoadData();
                return;
            }

            await _LoadData();

            string successMessage = isCurrentlyActive
                ? $"L'utilisateur {selectedUser.FullName} a été banni avec succès."
                : $"L'utilisateur {selectedUser.FullName} a été débanni avec succès.";

            string successTitle = isCurrentlyActive
                ? "Utilisateur banni"
                : "Utilisateur débanni";

            CustomMessageBox.Show(
                successMessage,
                successTitle,
                MessageBoxButton.OK,
                CustomMessageBoxImage.Information
            );
        }
        private async void btn_change_Password(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                User_DTO? user = await _UserApiService.GetUserInfoByUserID(Convert.ToInt32(App.AuthState.UserID));
                if (user == null)
                {
                    CustomMessageBox.Show("L'utilisateur sélectionné est introuvable.",
                       "Utilisateur introuvable", MessageBoxButton.OK, CustomMessageBoxImage.Warning);
                    return;
                }
                AddEditUserWindow window = new AddEditUserWindow(_UserApiService, user);
                window.CmbRole.IsEnabled = false;
                window.onRefresh = async (sender, e) => await _LoadData();
                window.ShowDialog();
            }
        }
        private async void Btn_EditCompanyInfo_Click(object sender, RoutedEventArgs e)
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                Company? company = await _UserApiService.GetCompanyInfo();
                if (company == null)
                {
                    CustomMessageBox.Show(
                        "Les informations de l'entreprise sont introuvables dans la base de données.",
                        "Entreprise introuvable",
                        MessageBoxButton.OK,
                        CustomMessageBoxImage.Warning
                    );
                    return;
                }
                EditCompanyWindow window = new EditCompanyWindow(_UserApiService, company);
                window.onRefresh += async (obj, e) => await _LoadCompanyInfo(e);
                window.ShowDialog();
            });
        }
    }
}