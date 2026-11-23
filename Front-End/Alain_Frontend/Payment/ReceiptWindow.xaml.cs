using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace project_03.Payment
{
    /// <summary>
    /// Interaction logic for ReceiptWindow.xaml
    /// </summary>
    public partial class ReceiptWindow : Window
    {
        private readonly DTO.Payment_DTO _payment;

        public ReceiptWindow(DTO.Payment_DTO payment, int Duration)
        {
            InitializeComponent();
            _payment = payment;
            PopulateReceiptData(Duration);
        }

        private void PopulateReceiptData(int Duration)
        {
            if (_payment == null) return;

            TxtInvoiceNumber.Text = _payment.RentalCode;
            TxtPaymentDate.Text = _payment.PaymentDate.ToString("dd/MM/yyyy HH:mm");
            TxtCustomerName.Text = _payment.CustomerName;
            TxtVehicleInfo.Text = $"{_payment.CarFullName} ({_payment.CarPlate})";
            TxtRentalDays.Text = $"Durée : {Duration} Jours";
            TxtAmount.Text = $"{_payment.Amount:N2} MAD";
        }
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}