using project_03.Model;
using System.Windows.Controls;

namespace project_03.Car.Controls
{
    /// <summary>
    /// Interaction logic for CarDetailsControl.xaml
    /// </summary>
    public partial class CarDetailsControl : UserControl
    {
        public CarDetailsControl()
        {
            InitializeComponent();
        }
        public void LoadCarData(Model.Car car)
        {
            if (car == null) return;

            TxtBrandModel.Text = $"{car.BrandName} {car.Model}";
            TxtPlate.Text = car.Plate;
            TxtYear.Text = car.ManufactureYear.ToString();
            TxtFuelType.Text = car.FuelType.ToString();
            TxtTransmission.Text = car.Transmission.ToString();
            TxtBodyType.Text = car.BodyType.ToString().Replace("_"," ");
            TxtSeatsDoors.Text = $"{car.NumberOfSeats} places / {car.NumberOfDoors} portes";
            TxtMileage.Text = $"{car.Mileage:N0} km";
            TxtStatus.Text = car.CarStatus.ToString();
            TxtPrice.Text = $"{car.DailyRentPrice:0.00} MAD / jour";
        }
        public void ResetForm()
        {
            string s = "N/A";

            TxtBrandModel.Text = s;
            TxtPlate.Text = s;
            TxtYear.Text = s;
            TxtFuelType.Text = s;
            TxtTransmission.Text = s;
            TxtBodyType.Text = s;
            TxtSeatsDoors.Text = s;
            TxtMileage.Text = s;
            TxtStatus.Text = s;
            TxtPrice.Text = s;
        }
    }
}