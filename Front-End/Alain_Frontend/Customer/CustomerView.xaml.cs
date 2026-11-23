using MahApps.Metro.IconPacks;
using project_03.Services;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace project_03.Customer
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
        private ApiClient _Client;
        private CustomerApiService _CustomerApiService;
        private RentalApiService _RentalApiService;
        private CountryApiService _CountryApiService;
        public CustomerView()
        {
            InitializeComponent();
            _Client = new ApiClient();
            _CustomerApiService = new CustomerApiService(_Client);
            _RentalApiService = new RentalApiService(_Client);
            _CountryApiService = new CountryApiService(_Client);
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await _LoadCustomers();
            UnsbscribeToEvents();
            CBx_FilterBy.SelectedIndex = 0;
            SbscribeToEvents();
        }
        private void UnsbscribeToEvents()
        {
            CBx_FilterBy.SelectionChanged -= CBx_Filter_SelectionChanged;
        }
        private void SbscribeToEvents()
        {
            CBx_FilterBy.SelectionChanged += CBx_Filter_SelectionChanged;
        }
        private async void CBx_Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_Clear.Visibility = Visibility.Hidden;

            if (CBx_FilterBy.SelectedIndex == 0)
            {
                txtbx_Filter.Visibility = Visibility.Collapsed;
                CBx_Nationality.Visibility = Visibility.Collapsed;
                await _LoadDefaultDataToForm();
            }
            else if (CBx_FilterBy.SelectedIndex == 4)
            {
                CBx_Nationality.Items.Clear();
                txtbx_Filter.Visibility = Visibility.Collapsed;
                CBx_Nationality.Visibility = Visibility.Visible;

                CBx_Nationality.SelectionChanged -= CBx_Nationality_SelectionChanged;

                await _LoadDefaultDataToForm();

                await ErrorHandler.ExecuteAsync(async () =>
                {
                    List<string>? Countries =
                        await _CountryApiService.GetActiveCountries();

                    if (Countries != null)
                    {
                        if (Countries.Count > 1)
                            CBx_Nationality.Items.Add("Tous");

                        foreach (var item in Countries)
                            CBx_Nationality.Items.Add(item);
                    }
                });

                CBx_Nationality.SelectedIndex = 0;
                CBx_Nationality.SelectionChanged += CBx_Nationality_SelectionChanged;
            }
            else
            {
                CBx_Nationality.Visibility = Visibility.Collapsed;
                txtbx_Filter.Visibility = Visibility.Visible;

                txtbx_Filter.TextChanged -= txtbx_Filter_TextChanged;
                txtbx_Filter.Text = "";

                await _LoadDefaultDataToForm();

                txtbx_Filter.TextChanged += txtbx_Filter_TextChanged;
            }
        }
        private async Task _LoadCustomers()
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                var CustomerListTask = _CustomerApiService.GetAllCustomers();
                var CountNewCustomers = _CustomerApiService.CountCustomers(DateTime.Now.Year, DateTime.Now.Month);
                var CountActiveRentals = _RentalApiService.CountActiveRentals();

                await Task.WhenAll(CustomerListTask, CountNewCustomers, CountActiveRentals);

                List<Model.Customer>? customers = await CustomerListTask;

                if (customers == null || customers.Count == 0)
                    FilterGird.Visibility = Visibility.Collapsed;

                _LoadDataToForm(customers, await CountNewCustomers, await CountActiveRentals);
                txtTotalCustomers.Text = txtNewCustomerCount.Text;
            });
        }
        private async Task _LoadDefaultDataToForm()
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                var CustomerListTask = _CustomerApiService.GetAllCustomers();
                var CountNewCustomers = _CustomerApiService.CountCustomers(DateTime.Now.Year, DateTime.Now.Month);
                var CountActiveRentals = _RentalApiService.CountActiveRentals();

                await Task.WhenAll(CustomerListTask, CountNewCustomers, CountActiveRentals);

                _LoadDataToForm(
                    await CustomerListTask,
                    await CountNewCustomers,
                    await CountActiveRentals);
            });
        }

        private async Task _LoadDefaultDataToForm(Task<List<string>> ActiveCountriesTask)
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                var CustomerListTask = _CustomerApiService.GetAllCustomers();
                var CountNewCustomers = _CustomerApiService.CountCustomers(DateTime.Now.Year, DateTime.Now.Month);
                var CountActiveRentals = _RentalApiService.CountActiveRentals();

                await Task.WhenAll(CustomerListTask, CountNewCustomers, CountActiveRentals, ActiveCountriesTask);

                List<string> Countries = await ActiveCountriesTask;

                if (Countries.Count == 1)
                    CBx_Nationality.Items.Add(Countries[0]);
                else
                {
                    CBx_Nationality.Items.Add("Tous");

                    foreach (var item in Countries)
                        CBx_Nationality.Items.Add(item);
                }

                _LoadDataToForm(
                    await CustomerListTask,
                    await CountNewCustomers,
                    await CountActiveRentals);
            });
        }
        private void _LoadDataToForm(List<Model.Customer>? customers, int CountCustomers, int CountActiveRentals)
        {
            if (customers == null || customers.Count == 0)
            {
                CustomersGrid.Visibility = Visibility.Collapsed;
                EmptyStatePanel.Visibility = Visibility.Visible;
            }
            else
            {
                EmptyStatePanel.Visibility = Visibility.Collapsed;
                CustomersGrid.Visibility = Visibility.Visible;
 
                CustomersGrid.ItemsSource = customers;
            }
            txtTotalCustomers.Text = CustomersGrid.Items.Count.ToString();
            txtNewCustomerCount.Text = CountCustomers.ToString();
            txtActiveRentals.Text = CountActiveRentals.ToString();
        }
        private void _LoadDataToForm(List<Model.Customer>? customers)
        {
            if(customers == null || customers.Count == 0)
            {
                CustomersGrid.Visibility = Visibility.Collapsed;
                EmptyStatePanel.Visibility = Visibility.Visible;
            }
            else
            {
                EmptyStatePanel.Visibility = Visibility.Collapsed;
                CustomersGrid.Visibility = Visibility.Visible;
                CustomersGrid.ItemsSource = customers;
            }
        }
        private async void txtbx_Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbx_Filter.Text))
            {
                btn_Clear.Visibility = Visibility.Hidden;
                await _LoadDefaultDataToForm();
            }
            else
            {
                btn_Clear.Visibility = Visibility.Visible;

                await ErrorHandler.ExecuteAsync(async () =>
                {
                    switch (CBx_FilterBy.SelectedIndex)
                    {
                        case 1:
                            _LoadDataToForm(await _CustomerApiService.FilterCustomersByCustomerName(txtbx_Filter.Text));
                            break;

                        case 2:
                            _LoadDataToForm(await _CustomerApiService.FilterCustomersByNationalNumber(txtbx_Filter.Text));
                            break;

                        case 3:
                            _LoadDataToForm(await _CustomerApiService.FilterCustomersByPhoneNumber(txtbx_Filter.Text));
                            break;
                    }
                });
            }
        }
        private async void CBx_Nationality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBx_FilterBy.SelectedIndex == 4)
            {
                if (CBx_Nationality.Items.Count < 2 ||
                    CBx_Nationality.SelectedIndex == 0 ||
                    CBx_Nationality.SelectedIndex == -1)
                {
                    await _LoadDefaultDataToForm();
                }
                else
                {
                    await ErrorHandler.ExecuteAsync(async () =>
                    {
                        _LoadDataToForm(
                            await _CustomerApiService.GetCustomersByNationality(
                                CBx_Nationality.SelectedItem.ToString()!));
                    });
                }
            }
        }
        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            txtbx_Filter.Text = "";
            btn_Clear.Visibility = Visibility.Hidden;
        }
        private async void btn_Details_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersGrid.SelectedItem != null)
            {
                var row = (DataGridRow)CustomersGrid.ItemContainerGenerator
                    .ContainerFromItem(CustomersGrid.SelectedItem);

                if (row != null)
                {
                    var content = CustomersGrid.Columns[0].GetCellContent(row);

                    if (content is ContentPresenter presenter)
                    {
                        presenter.ApplyTemplate();

                        var stackPanel = VisualTreeHelper.GetChild(presenter, 0) as StackPanel;

                        if (stackPanel != null)
                        {
                            string NationalNumber = ((TextBlock)stackPanel.Children[1]).Text;

                            Model.Customer? customer = null;
                            bool RequestSucceeded = false;

                            await ErrorHandler.ExecuteAsync(async () =>
                            {
                                customer = await _CustomerApiService.GetCustomerInfoByNationalNumber(NationalNumber);
                                RequestSucceeded = true;
                            });

                            if (!RequestSucceeded)
                                return;

                            if (customer == null)
                            {
                                CustomMessageBox.Show("Le client demandé est introuvable.", "client introuvable",
                                    MessageBoxButton.OK, CustomMessageBoxImage.Error, "Ok");
                                return;
                            }

                            Window window = new CustomerDetailsWindow(customer);
                            window.ShowDialog();
                        }
                    }
                }
            }
        }
        private void BtnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddEditCustomerWindow window = new AddEditCustomerWindow(_CustomerApiService,_CountryApiService);
            window.onRefresh += (sender, e) => _RefreshPage();
            window.ShowDialog();
        }
        private async void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersGrid.SelectedItem != null)
            {
                var row = (DataGridRow)CustomersGrid.ItemContainerGenerator.ContainerFromItem(CustomersGrid.SelectedItem);

                if (row != null)
                {
                    var content = CustomersGrid.Columns[0].GetCellContent(row);

                    if (content is ContentPresenter presenter)
                    {
                        presenter.ApplyTemplate();

                        var stackPanel = VisualTreeHelper.GetChild(presenter, 0) as StackPanel;

                        if (stackPanel != null)
                        {
                            string NationalNumber = ((TextBlock)stackPanel.Children[1]).Text;

                            Model.Customer? customer = null;
                            bool RequestSucceeded = false;

                            await ErrorHandler.ExecuteAsync(async () =>
                            {
                                customer = await _CustomerApiService.GetCustomerInfoByNationalNumber(NationalNumber);
                                RequestSucceeded = true;
                            });

                            if (!RequestSucceeded)
                                return;

                            if (customer == null)
                            {
                                CustomMessageBox.Show("Le client demandé est introuvable.", "client introuvable",
                                    MessageBoxButton.OK, CustomMessageBoxImage.Error, "Ok");
                                return;
                            }

                            AddEditCustomerWindow window = new AddEditCustomerWindow(customer, _CustomerApiService, _CountryApiService);
                            window.onRefresh += (sender, e) => _RefreshPage();
                            window.ShowDialog();
                        }
                    }
                }
            }
        }
        private async void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersGrid.SelectedItem != null)
            {
                var row = (DataGridRow)CustomersGrid.ItemContainerGenerator.ContainerFromItem(CustomersGrid.SelectedItem);

                if (row != null)
                {
                    var content = CustomersGrid.Columns[0].GetCellContent(row);

                    if (content is ContentPresenter presenter)
                    {
                        presenter.ApplyTemplate();

                        var stackPanel = VisualTreeHelper.GetChild(presenter, 0) as StackPanel;

                        if (stackPanel != null)
                        {
                            string NationalNumber = ((TextBlock)stackPanel.Children[1]).Text;

                            if (CustomMessageBox.Show($"Êtes-vous sûr de vouloir supprimer ce client ? Cette action est irréversible.",
                                "Confirmer la suppression", MessageBoxButton.YesNo, CustomMessageBoxImage.Question,
                                "Supprimer", "Annuler", primaryIcon: PackIconMaterialKind.Delete,
                                secondaryIcon: PackIconMaterialKind.Cancel) == MessageBoxResult.OK)
                            {
                                int RemainingCustomers = -1;
                                bool DeleteSucceeded = false;

                                await ErrorHandler.ExecuteAsync(async () =>
                                {
                                    RemainingCustomers = await _CustomerApiService.DeleteCustomer(NationalNumber);
                                    DeleteSucceeded = true;
                                });

                                if (!DeleteSucceeded)
                                    return;

                                if (RemainingCustomers == 0)
                                {
                                    FilterGird.Visibility = Visibility.Collapsed;
                                    EmptyStatePanel.Visibility = Visibility.Visible;
                                }

                                if (CustomersGrid.ItemsSource is IList items)
                                {
                                    items.Remove(row.Item);
                                    CustomersGrid.Items.Refresh();
                                }

                                if (CustomersGrid.Items.Count == 0)
                                {
                                    CustomersGrid.Visibility = Visibility.Collapsed;
                                    EmptyStatePanel.Visibility = Visibility.Visible;
                                }

                                int TotalCustomers = Convert.ToInt32(txtTotalCustomers.Text);
                                TotalCustomers--;
                                txtTotalCustomers.Text = TotalCustomers.ToString();

                                await ErrorHandler.ExecuteAsync(async () =>
                                {
                                    txtNewCustomerCount.Text = (await _CustomerApiService.CountCustomers(
                                        DateTime.Now.Year, DateTime.Now.Month)).ToString();
                                });

                                CustomMessageBox.Show("Le client a été supprimé avec succès.", "Suppression réussie",
                                    MessageBoxButton.OK, CustomMessageBoxImage.Information);
                            }
                        }
                    }
                }
            }
        }
        private async void _RefreshPage()
        {
            if (CBx_FilterBy.SelectedIndex == 4)
                CBx_Nationality_SelectionChanged(this, null!);
            else
                CBx_Filter_SelectionChanged(this, null!);

            FilterGird.Visibility = Visibility.Visible;
        }
    }
}