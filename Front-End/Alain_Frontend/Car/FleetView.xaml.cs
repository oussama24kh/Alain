using MahApps.Metro.IconPacks;
using project_03.Car;
using project_03.DTO;
using project_03.Model;
using project_03.Services;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace project_03.Views
{
    /// <summary>
    /// Interaction logic for FleetView.xaml
    /// </summary>
    public partial class FleetView : UserControl
    {
        private readonly ApiClient _Client;
        private readonly CarApiService _carService;
        private readonly BodyTypeService _bodyTypeService;
        public FleetView()
        {
            InitializeComponent();
            _Client = new ApiClient();
            _carService = new CarApiService(_Client);
            _bodyTypeService = new BodyTypeService(_Client);
        }
        private async Task _LoadWindow()
        {
            UnsbscribeToEvents();

            try
            {
                await LoadCars();

                if (FleetGrid.Items.Count == 0)
                {
                    FilterGird.Visibility = Visibility.Collapsed;
                    FilterStatusStack.Visibility = Visibility.Collapsed;
                }
                else
                {
                    CBx_FilterBy.SelectedIndex = 0;
                    FilterStatusStack.Visibility = Visibility.Visible;
                }
            }
            finally
            {
                SubscribeToEvents();
            }
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await _LoadWindow();
        }
        private void SubscribeToEvents()
        {
            rdbtn_Tous.Checked += rdbtn_Tous_Checked;
            CBx_FilterBy.SelectionChanged += CBx_FilterBy_SelectionChanged;
        }
        private void UnsbscribeToEvents()
        {
            rdbtn_Tous.Checked -= rdbtn_Tous_Checked;
            CBx_FilterBy.SelectionChanged -= CBx_FilterBy_SelectionChanged;
        }
        private void LoadDataToForm(List<Car_DTO>? cars, Dictionary<string, int>? Statistics)
        {
            if (cars == null || cars.Count == 0)
            {
                FleetGrid.Visibility = Visibility.Collapsed;
                FilterStatusStack.Visibility = Visibility.Collapsed;
                EmptyStatePanel.Visibility = Visibility.Visible;
                txtblk_TotalCars.Text = "0 véhicules";
                return;
            }
            else
            {
                EmptyStatePanel.Visibility = Visibility.Collapsed;
                FleetGrid.Visibility = Visibility.Visible;
                FleetGrid.ItemsSource = cars;
                txtblk_TotalCars.Text = cars.Count.ToString() + " véhicules";
            }

            if (CBx_FilterBy.SelectedIndex <= 0 && Statistics != null)
            {
                FilterStatusStack.Visibility = Visibility.Visible;
                if (!Statistics.ContainsKey("DISPONIBLE") || Statistics["DISPONIBLE"] == 0)
                    rdbtn_Disponibles.Visibility = Visibility.Collapsed;
                if (!Statistics.ContainsKey("LOUE") || Statistics["LOUE"] == 0)
                    rdbtn_EnLocation.Visibility = Visibility.Collapsed;
                if (!Statistics.ContainsKey("EN MAINTENANCE") || Statistics["EN MAINTENANCE"] == 0)
                    rdbtn_EnMaintenance.Visibility = Visibility.Collapsed;
                if (!Statistics.ContainsKey("RETIRE") || Statistics["RETIRE"] == 0)
                    rdbtn_Retires.Visibility = Visibility.Collapsed;
            }
            else
                FilterStatusStack.Visibility = Visibility.Collapsed;
        }
        private void LoadDataToForm(List<Car_DTO>? cars)
        {
            if (cars == null)
                return;
            txtblk_TotalCars.Text = cars.Count.ToString() + " véhicules";
            if (cars.Count == 0)
            {
                FleetGrid.Visibility = Visibility.Collapsed;
                //FilterStatusStack.Visibility = Visibility.Collapsed;
                EmptyStatePanel.Visibility = Visibility.Visible;
            }
            else
            {
                EmptyStatePanel.Visibility = Visibility.Collapsed;
                FleetGrid.Visibility = Visibility.Visible;
                FleetGrid.ItemsSource = cars;
            }
        }
        private async Task LoadCars()
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                var CarListTask = _carService.GetAllCars();
                var CarStatusCountsTask = _carService.GetCarStatusCounts();

                await Task.WhenAll(CarListTask, CarStatusCountsTask);

                LoadDataToForm(await CarListTask, await CarStatusCountsTask);
            });
        }
        private async void CBx_FilterBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_Clear.Visibility = Visibility.Hidden;

            if (CBx_FilterBy.SelectedIndex == 0)
            {
                rdbtn_Tous.Checked -= rdbtn_Tous_Checked;
                rdbtn_Tous.IsChecked = true;
                rdbtn_Tous.Checked += Rdbtn_Tous_Checked;

                FilterStatusStack.Visibility = Visibility.Visible;
                txtbx_Filter.Visibility = Visibility.Collapsed;
                CBx_BodyTypes.Visibility = Visibility.Collapsed;

                await LoadCars();
            }
            else if (CBx_FilterBy.SelectedIndex == 4)
            {
                CBx_BodyTypes.Items.Clear();

                FilterStatusStack.Visibility = Visibility.Collapsed;
                txtbx_Filter.Visibility = Visibility.Collapsed;
                CBx_BodyTypes.Visibility = Visibility.Visible;

                CBx_BodyTypes.Items.Add("Tous");
                CBx_BodyTypes.SelectedIndex = 0;

                await ErrorHandler.ExecuteAsync(async () =>
                {
                    List<string>? BodyTypes =
                        await _bodyTypeService.GetAllBodyTypes();

                    if (BodyTypes != null)
                    {
                        foreach (var item in BodyTypes)
                            CBx_BodyTypes.Items.Add(item);
                    }
                });
            }
            else
            {
                FilterStatusStack.Visibility = Visibility.Collapsed;
                CBx_BodyTypes.Visibility = Visibility.Collapsed;
                txtbx_Filter.Visibility = Visibility.Visible;

                txtbx_Filter.TextChanged -= txtbx_Filter_TextChanged;
                txtbx_Filter.Text = "";

                await LoadCars();

                txtbx_Filter.TextChanged += txtbx_Filter_TextChanged;
            }
        }
        private async void CBx_BodyTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBx_FilterBy.SelectedIndex == 4)
            {
                if (CBx_BodyTypes.SelectedIndex == 0 ||
                    CBx_BodyTypes.SelectedIndex == -1)
                {
                    await LoadCars();
                }
                else
                {
                    string BodyType = CBx_BodyTypes.SelectedItem.ToString()!;

                    await ErrorHandler.ExecuteAsync(async () =>
                    {
                        var CarListTask =
                            _carService.FilterCarsByBodyType(BodyType);

                        var CarStatusCountsTask =
                            _carService.GetCarStatusCounts(BodyType);

                        await Task.WhenAll(CarListTask, CarStatusCountsTask);

                        LoadDataToForm(
                            await CarListTask,
                            await CarStatusCountsTask);
                    });
                }
            }
        }
        private async void Rdbtn_Tous_Checked(object sender, RoutedEventArgs e)
        {
            await LoadCars();
        }
        private async void txtbx_Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                Task<List<Car_DTO>?> CarListTask;
                Task<Dictionary<string, int>?> CarStatusCountsTask;

                if (string.IsNullOrWhiteSpace(txtbx_Filter.Text))
                {
                    btn_Clear.Visibility = Visibility.Hidden;

                    CarListTask = _carService.GetAllCars();
                    CarStatusCountsTask = _carService.GetCarStatusCounts();
                }
                else
                {
                    btn_Clear.Visibility = Visibility.Visible;

                    switch (CBx_FilterBy.SelectedIndex)
                    {
                        case 1:
                            CarListTask =
                                _carService.FilterCarsByPlateNumber(txtbx_Filter.Text);
                            break;

                        case 2:
                            CarListTask =
                                _carService.FilterCarsByBrand(txtbx_Filter.Text);
                            break;

                        case 3:
                            CarListTask =
                                _carService.FilterCarsByModel(txtbx_Filter.Text);
                            break;

                        default:
                            return;
                    }

                    CarStatusCountsTask = _carService.GetCarStatusCounts();
                }

                await Task.WhenAll(CarListTask, CarStatusCountsTask);

                LoadDataToForm(
                    await CarListTask,
                    await CarStatusCountsTask);
            });
        }
        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            txtbx_Filter.Text = "";
            btn_Clear.Visibility = Visibility.Hidden;
        }
        private void btn_AddNewCar_Click(object sender, RoutedEventArgs e)
        {
            AddEditCarWindow window = new AddEditCarWindow(
                new CarApiService(_Client),
                new BrandApiService(_Client));

            window.onRefresh += (sender, e) => _RefreshPage();

            window.ShowDialog();
        }
        private async void btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (FleetGrid.SelectedItem == null)
                return;

            var row = (DataGridRow)FleetGrid.ItemContainerGenerator
                .ContainerFromItem(FleetGrid.SelectedItem);

            if (row == null)
                return;

            var content = FleetGrid.Columns[0].GetCellContent(row);

            if (content is ContentPresenter presenter)
            {
                presenter.ApplyTemplate();

                var stackPanel =
                    VisualTreeHelper.GetChild(presenter, 0) as StackPanel;

                if (stackPanel == null)
                    return;

                string plate = ((TextBlock)stackPanel.Children[1]).Text;

                await ErrorHandler.ExecuteAsync(async () =>
                {
                    Model.Car? car =
                        await _carService.GetCarInfoByPlateNumber(plate);

                    if (car == null)
                    {
                        CustomMessageBox.Show(
                            "Le véhicule sélectionné n'existe pas ou n'a pas pu être trouvé dans la base de données.",
                            "Véhicule introuvable",
                            MessageBoxButton.OK,
                            CustomMessageBoxImage.Error,
                            "Ok");

                        return;
                    }

                    AddEditCarWindow window = new AddEditCarWindow(
                        _carService,
                        new BrandApiService(_Client),
                        car);

                    window.onRefresh = (sender, e) => _RefreshPage();

                    window.ShowDialog();
                });
            }
        }
        private async void btn_Details_Click(object sender, RoutedEventArgs e)
        {
            if (FleetGrid.SelectedItem == null)
                return;

            var row = (DataGridRow)FleetGrid.ItemContainerGenerator
                .ContainerFromItem(FleetGrid.SelectedItem);

            if (row == null)
                return;

            var content = FleetGrid.Columns[0].GetCellContent(row);

            if (content is ContentPresenter presenter)
            {
                presenter.ApplyTemplate();

                var stackPanel =
                    VisualTreeHelper.GetChild(presenter, 0) as StackPanel;

                if (stackPanel == null)
                    return;

                string plate = ((TextBlock)stackPanel.Children[1]).Text;

                await ErrorHandler.ExecuteAsync(async () =>
                {
                    Model.Car? car =
                        await _carService.GetCarInfoByPlateNumber(plate);

                    if (car == null)
                    {
                        CustomMessageBox.Show(
                            "Le véhicule sélectionné n'existe pas ou n'a pas pu être trouvé dans la base de données.",
                            "Véhicule introuvable",
                            MessageBoxButton.OK,
                            CustomMessageBoxImage.Error,
                            "Ok");

                        return;
                    }

                    Window window = new CarDetailsWindow(car);
                    window.ShowDialog();
                });
            }
        }
        private async void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (FleetGrid.SelectedItem == null)
                return;

            var row = (DataGridRow)FleetGrid.ItemContainerGenerator
                .ContainerFromItem(FleetGrid.SelectedItem);

            if (row == null)
                return;

            var content = FleetGrid.Columns[0].GetCellContent(row);

            if (content is ContentPresenter presenter)
            {
                presenter.ApplyTemplate();

                var stackPanel =
                    VisualTreeHelper.GetChild(presenter, 0) as StackPanel;

                if (stackPanel == null)
                    return;

                string fullName = ((TextBlock)stackPanel.Children[0]).Text;
                string plate = ((TextBlock)stackPanel.Children[1]).Text;

                if (CustomMessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer {fullName} [{plate}]",
                    "Confirmer la suppression",
                    MessageBoxButton.YesNo,
                    CustomMessageBoxImage.Question,
                    "Supprimer",
                    "Annuler",
                    primaryIcon: PackIconMaterialKind.Delete,
                    secondaryIcon: PackIconMaterialKind.Cancel) != MessageBoxResult.OK)
                {
                    return;
                }

                await ErrorHandler.ExecuteAsync(async () =>
                {
                    if (await _carService.DeleteCar(plate) == 0)
                    {
                        FilterGird.Visibility = Visibility.Collapsed;
                        FilterStatusStack.Visibility = Visibility.Collapsed;
                        FleetGrid.Visibility = Visibility.Collapsed;
                        EmptyStatePanel.Visibility = Visibility.Visible;
                    }

                    if (FleetGrid.ItemsSource is IList items)
                    {
                        items.Remove(row.Item);
                        FleetGrid.Items.Refresh();
                    }

                    if (FleetGrid.Items.Count == 0)
                    {
                        FleetGrid.Visibility = Visibility.Collapsed;
                        EmptyStatePanel.Visibility = Visibility.Visible;
                    }

                    int CountCars = int.Parse(txtblk_TotalCars.Text.Split(' ')[0]);

                    CountCars--;

                    txtblk_TotalCars.Text = CountCars + " véhicules";

                    CustomMessageBox.Show(
                        $"La voiture {fullName}: [{plate}] a été supprimée de la base de données.",
                        "Suppression réussie",
                        MessageBoxButton.OK,
                        CustomMessageBoxImage.Information);
                });
            }
        }
        private async void rdbtn_Tous_Checked(object sender, RoutedEventArgs e)
        {
            await LoadCars();
        }
        private async void rdbtn_Disponibles_Checked(object sender, RoutedEventArgs e)
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                LoadDataToForm(await _carService.FilterCarsByStatus("DISPONIBLE"));
            });
        }
        private async void rdbtn_EnLocation_Checked(object sender, RoutedEventArgs e)
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                LoadDataToForm(await _carService.FilterCarsByStatus("LOUE"));
            });
        }
        private async void rdbtn_EnMaintenance_Checked(object sender, RoutedEventArgs e)
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                LoadDataToForm( await _carService.FilterCarsByStatus("EN MAINTENANCE"));
            });
        }
        private async void rdbtn_Retires_Checked(object sender, RoutedEventArgs e)
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                LoadDataToForm(await _carService.FilterCarsByStatus("RETIRE"));
            });
        }
        private async void _RefreshPage()
        {
            if (CBx_FilterBy.SelectedIndex == 0)
            {
                if (rdbtn_Tous.IsChecked == true)
                {
                    await _LoadWindow();
                }
                else
                {
                    if (rdbtn_Disponibles.IsChecked == true)
                        await ErrorHandler.ExecuteAsync(async () =>
                            LoadDataToForm(await _carService.FilterCarsByStatus("DISPONIBLE")));

                    else if (rdbtn_EnLocation.IsChecked == true)
                        await ErrorHandler.ExecuteAsync(async () =>
                            LoadDataToForm(await _carService.FilterCarsByStatus("LOUE")));

                    else if (rdbtn_EnMaintenance.IsChecked == true)
                        await ErrorHandler.ExecuteAsync(async () =>
                            LoadDataToForm(await _carService.FilterCarsByStatus("EN MAINTENANCE")));

                    else
                        await ErrorHandler.ExecuteAsync(async () =>
                            LoadDataToForm(await _carService.FilterCarsByStatus("RETIRE")));
                }
            }
            else if (CBx_FilterBy.SelectedIndex == 4)
            {
                await LoadCarsBySelectedBodyType();
            }
            else
            {
                await LoadCarsByCurrentFilter();
            }

            FilterGird.Visibility = Visibility.Visible;
        }
        private async Task LoadCarsBySelectedBodyType()
        {
            if (CBx_BodyTypes.SelectedIndex <= 0)
            {
                await LoadCars();
                return;
            }

            string bodyType = CBx_BodyTypes.SelectedItem.ToString()!;

            await ErrorHandler.ExecuteAsync(async () =>
            {
                var carTask = _carService.FilterCarsByBodyType(bodyType);
                var countsTask = _carService.GetCarStatusCounts(bodyType);

                await Task.WhenAll(carTask, countsTask);

                LoadDataToForm(await carTask, await countsTask);
            });
        }
        private async Task LoadCarsByCurrentFilter()
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                string filter = txtbx_Filter.Text;

                if (string.IsNullOrWhiteSpace(filter))
                {
                    var carTask = _carService.GetAllCars();
                    var countsTask = _carService.GetCarStatusCounts();

                    await Task.WhenAll(carTask, countsTask);

                    LoadDataToForm(await carTask, await countsTask);
                    return;
                }

                Task<List<Car_DTO>?> carListTask;

                switch (CBx_FilterBy.SelectedIndex)
                {
                    case 1:
                        carListTask = _carService.FilterCarsByPlateNumber(filter);
                        break;

                    case 2:
                        carListTask = _carService.FilterCarsByBrand(filter);
                        break;

                    case 3:
                        carListTask = _carService.FilterCarsByModel(filter);
                        break;

                    default:
                        return;
                }

                var countsTask2 = _carService.GetCarStatusCounts();

                await Task.WhenAll(carListTask, countsTask2);

                LoadDataToForm(await carListTask, await countsTask2);
            });
        }
    }
}