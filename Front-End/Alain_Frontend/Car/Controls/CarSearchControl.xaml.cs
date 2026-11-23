using project_03.Model;
using project_03.Services;
using System.Windows;
using System.Windows.Controls;

namespace project_03.Car.Controls
{
    public partial class CarSearchControl : UserControl
    {
        public CarApiService? CarApiService { private get; set; }
        public Model.Car? Car { get; private set; }

        public CarSearchControl()
        {
            InitializeComponent();

            Car = null;
            Btn_Search.IsEnabled = false;
            Btn_Search.Opacity = 0.5;
            txtSearchPlate.Text = "";
        }

        private async void Btn_Search_Click(object sender, RoutedEventArgs e)
        {
            string plate = txtSearchPlate.Text.Trim();

            if (string.IsNullOrWhiteSpace(plate))
            {
                CustomMessageBox.Show(
                    "Veuillez saisir une matricule valide.",
                    "Attention",
                    MessageBoxButton.OK,
                    CustomMessageBoxImage.Warning);

                return;
            }

            await LoadCarByPlate(plate);
        }

        private async Task LoadCarByPlate(string plate)
        {
            await LoadCarAsync(
                () => CarApiService!.GetCarInfoByPlateNumber(plate),
                $"Aucun véhicule trouvé avec la matricule : {plate}");
        }

        public async Task LoadCarByCarID(int CarID)
        {
            await LoadCarAsync(
                () => CarApiService!.GetCarInfoByCarID(CarID),
                "Aucun véhicule trouvé");
        }

        private async Task LoadCarAsync(
            Func<Task<Model.Car?>> getCar,
            string notFoundMessage)
        {
            if (CarApiService == null)
            {
                CustomMessageBox.Show(
                    "Le service de gestion des véhicules n'est pas disponible.",
                    "Erreur",
                    MessageBoxButton.OK,
                    CustomMessageBoxImage.Error);

                return;
            }

            await ErrorHandler.ExecuteAsync(async () =>
            {
                Model.Car? car = await getCar();

                if (car == null)
                {
                    Car = null;
                    carDetailsControl.ResetForm();

                    CustomMessageBox.Show(
                        notFoundMessage,
                        "Information",
                        MessageBoxButton.OK,
                        CustomMessageBoxImage.Information);

                    return;
                }

                if (car.CarStatus == enCarStatus.DISPONIBLE)
                {
                    LoadCar(car);
                }
                else
                {
                    CustomMessageBox.Show(
                        $"Le véhicule sélectionné ({car.Plate}) est actuellement INDISPONIBLE et ne peut pas être loué.",
                        "Véhicule Indisponible",
                        MessageBoxButton.OK,
                        CustomMessageBoxImage.Error);

                    Car = null;
                    carDetailsControl.ResetForm();
                }
            });
        }

        public void LoadCar(Model.Car car)
        {
            Car = car;
            carDetailsControl.LoadCarData(car);
            txtSearchPlate.Text = car.Plate;
        }

        private void txtSearchPlate_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchPlate.Text))
            {
                Btn_Search.IsEnabled = false;
                Btn_Search.Opacity = 0.5;
            }
            else
            {
                Btn_Search.IsEnabled = true;
                Btn_Search.Opacity = 1;
            }
        }
    }
}