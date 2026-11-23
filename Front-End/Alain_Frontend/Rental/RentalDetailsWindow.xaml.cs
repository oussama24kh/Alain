using System.Windows;

namespace project_03.Rental
{
    /// <summary>
    /// Interaction logic for RentalDetailsWindow.xaml
    /// </summary>
    public partial class RentalDetailsWindow : Window
    {
        public RentalDetailsWindow(DTO.RentalDetails_DTO contract)
        {
            InitializeComponent();

            detailsViewControl.LoadContractData(contract);
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
