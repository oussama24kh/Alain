using System.Windows.Controls;
using System.Windows.Media;

namespace project_03.Rental.Controls
{
    /// <summary>
    /// Interaction logic for RentalDetailsControl.xaml
    /// </summary>
    public partial class RentalDetailsControl : UserControl
    {
        public RentalDetailsControl()
        {
            InitializeComponent();
        }
        public void LoadContractData(DTO.RentalDetails_DTO contract)
        {
            if (contract == null) return;

            txtContractReference.Text = contract.RentalCode;
            txtStatusBadge.Text = contract.Status.ToString().Replace("_"," ");

            txtCustomerName.Text = $"Client: {contract.CustomerFullName}";
            txtCustomerPhone.Text = $"Téléphone: {contract.CustomerPhone}";
            txtCustomerNationalId.Text = $"N° National: {contract.CustomerNationalId}";

            // Car Info
            txtCarModel.Text = $"Véhicule: {contract.CarBrand} {contract.CarModel}";
            txtCarPlate.Text = $"Immatriculation: {contract.CarLicensePlate}";

            // Mileage Info
            txtStartMileage.Text = $"{contract.StartMileage:N0} km";

            if (contract.EndMileage.HasValue && contract.EndMileage > 0)
            {
                txtEndMileage.Text = $"{contract.EndMileage.Value:N0} km";

                // Calculate distance traveled
                decimal distance = contract.EndMileage.Value - contract.StartMileage;
                txtTotalDistance.Text = $"{distance:N0} km";
            }
            else
            {
                txtEndMileage.Text = "Non restitué";
                txtTotalDistance.Text = "En cours";
            }

            // Dates Planning
            txtStartDate.Text = $"Début: {contract.StartDate:dd/MM/yyyy}";
            txtExpectedEndDate.Text = $"Fin Prévue: {contract.ExpectedEndDate:dd/MM/yyyy}";

            if (contract.ActualEndDate.HasValue)
            {
                txtActualEndDate.Text = $"Fin Effective: {contract.ActualEndDate.Value:dd/MM/yyyy}";
                //txtActualEndDate.Foreground = (Brush)FindResource("TextPrimaryBrush");
            }
            else
            {
                txtActualEndDate.Text = "Fin Effective: -";
                //txtActualEndDate.Foreground = (Brush)FindResource("TextSecondaryBrush");
            }

            // Financials
            txtTotalAmount.Text = $"{contract.TotalAmount:N2} MAD";
        }
    }
}
