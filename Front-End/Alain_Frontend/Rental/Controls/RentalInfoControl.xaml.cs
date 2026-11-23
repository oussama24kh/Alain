using project_03.Model;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace project_03.Rental.Controls
{
    /// <summary>
    /// Interaction logic for RentalInfoControl.xaml
    /// </summary>
    public partial class RentalInfoControl : UserControl
    {
        public RentalInfoControl()
        {
            InitializeComponent();
        }
        private void NumberTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }
        private void NumberTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string? text = e.DataObject.GetData(DataFormats.Text) as string;

                if (string.IsNullOrEmpty(text) || !text.All(char.IsDigit))
                    e.CancelCommand();
            }
            else
                e.CancelCommand();
        }
        private void NumberTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = (e.Key == Key.Space);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dpStartDate.SelectedDate = DateTime.Now;
        }
        public void SetAddNew()
        {
            dpStartDate.DisplayDateStart = DateTime.Now;
            dpStartDate.DisplayDateEnd = DateTime.Now.AddDays(4);
            dpStartDate.SelectedDate = DateTime.Today;

            dpEndDate.DisplayDateStart = DateTime.Now.AddDays(1);
            dpEndDate.DisplayDateEnd = DateTime.Now.AddDays(45);
        }
        public void LoadDataToForm(Model.Rental rental)
        {
            if (string.IsNullOrWhiteSpace(rental.RentalCode))
                txtContractRef.Text = rental.RentalCode;

            cbStatus.IsEnabled = true;
            dpStartDate.SelectedDate = rental.StartDate;
            dpEndDate.SelectedDate = rental.ExpectedEndDate;            
        }
        public Model.Rental? ValidateAndGetRentalInfo()
        {
            try
            {
                // 1. Validate Dates
                if (this.dpStartDate.SelectedDate == null || this.dpEndDate.SelectedDate == null)
                {
                    CustomMessageBox.Show("Veuillez sélectionner les dates de début et de fin de location.",
                                    "Validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning);
                    return null;
                }

                DateTime startDate = this.dpStartDate.SelectedDate.Value;
                DateTime endDate = this.dpEndDate.SelectedDate.Value;

                if (endDate < startDate)
                {
                    CustomMessageBox.Show("La date de fin ne peut pas être antérieure à la date de début.",
                                    "Validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning);
                    return null;
                }

                // 2. Validate Total Price
                if (!decimal.TryParse(this.txtTotalPrice.Text, out decimal totalPrice) || totalPrice <= 0)
                {
                    CustomMessageBox.Show("Le prix total calculé est invalide.",
                                    "Validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning);
                    return null;
                }

                // 3. Validate Payment Status & Conditional Advance Amount
                string? paymentStatus = this.cbPaymentStatus.SelectedValue as string;
                decimal advanceAmount = 0;

                if (paymentStatus == "Avance")
                {
                    if (!decimal.TryParse(this.txtAdvanceAmount.Text, out advanceAmount) || advanceAmount <= 0)
                    {
                        CustomMessageBox.Show("Veuillez saisir un montant d'avance valide.",
                                        "Validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning);
                        return null;
                    }

                    if (advanceAmount >= totalPrice)
                    {
                        CustomMessageBox.Show("Le montant de l'avance ne peut pas être supérieur ou égal au prix total. Veuillez sélectionner 'Payé' si le montant est intégral.",
                                        "Validation", MessageBoxButton.OK, CustomMessageBoxImage.Warning);
                        return null;
                    }
                }
                enRentalStatus status = (cbStatus.SelectedIndex == 0) ? enRentalStatus.EN_LOCATION : enRentalStatus.ANNULEE;

                return new Model.Rental(startDate, endDate, status);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue lors de la validation des données du contrat : {ex.Message}",
                                "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        public Model.Payment? ValidateAndGetPayment(int rentalId,int PaymentId)
        {
            // 1. Validate Total Price
            if (!decimal.TryParse(txtTotalPrice.Text.Trim(), out decimal totalAmount) || totalAmount <= 0)
            {
                MessageBox.Show(
                    "Veuillez saisir un prix total valide supérieur à 0.",
                    "Erreur de Validation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                txtTotalPrice.Focus();
                return null;
            }

            // 2. Validate Payment Status Selection
            if (cbPaymentStatus.SelectedValue == null)
            {
                CustomMessageBox.Show(
                    "Veuillez sélectionner un statut de paiement.",
                    "Erreur de Validation",
                    MessageBoxButton.OK,
                    CustomMessageBoxImage.Warning
                );
                cbPaymentStatus.Focus();
                return null;
            }

            string paymentStatus = "";
            decimal amountPaid = 0;

            // 3. Conditional Validation for 'AVANCE' (Advance Payment)
            if (cbPaymentStatus.SelectedIndex == 1)
            {
                paymentStatus = "Avance";
                if (!decimal.TryParse(txtAdvanceAmount.Text.Trim(), out amountPaid) || amountPaid <= 0)
                {
                    CustomMessageBox.Show(
                        "Le statut 'Avance' nécessite de saisir un montant d'avance valide supérieur à 0.",
                        "Montant d'Avance Requis",
                        MessageBoxButton.OK,
                    CustomMessageBoxImage.Warning
                    );
                    txtAdvanceAmount.Focus();
                    txtAdvanceAmount.SelectAll();
                    return null;
                }

                // Validate that advance does not exceed the total price
                if (amountPaid >= totalAmount)
                {
                    CustomMessageBox.Show(
                        "Le montant de l'avance ne peut pas être supérieur ou égal au prix total. Veuillez choisir le statut 'Payé' le cas échéant.",
                        "Montant Invalide",
                        MessageBoxButton.OK,
                        CustomMessageBoxImage.Warning
                    );
                    txtAdvanceAmount.Focus();
                    return null;
                }
            }
            else
            {
                // For 'EN_ATTENTE', 'ANNULE', or 'REMBOURSE', amount paid initially is 0 (or adjust based on your business logic)
                paymentStatus = "En attente";
                amountPaid = 0;
            }

            // 4. Return the Populated Payment Object
            return new Model.Payment
            {
                PaymentID = PaymentId,
                RentalID = rentalId,
                TotalAmount = totalAmount,
                AmountPaid = amountPaid,
                PaymentStatus = paymentStatus
            };
        }
        public void LoadPaymentInfo(Model.Payment payment)
        {
            txtTotalPrice.Text = payment.TotalAmount.ToString("0.##");
            if(payment.AmountPaid > 0)
            {
                cbPaymentStatus.SelectedIndex = 1;
                pnlAdvanceAmount.Visibility = Visibility.Visible;
                txtAdvanceAmount.Text = payment.AmountPaid.ToString("0.##");
            }
            else
                pnlAdvanceAmount.Visibility= Visibility.Collapsed;
            cbPaymentStatus.SelectedItem = payment.PaymentStatus;
        }
        public void LoadRentalInfo(Model.Rental rental)
        {
            txtContractRef.Text = rental.RentalCode;
            cbStatus.SelectedIndex = (rental.Status == Model.enRentalStatus.EN_LOCATION) ? 0 : 1;
            dpStartDate.SelectedDate = rental.StartDate;
            dpEndDate.SelectedDate = rental.ExpectedEndDate;
            txtNotes.Text = rental.Notes;
        }
        private void cbPaymentStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPaymentStatus.SelectedIndex == 0 || cbPaymentStatus.SelectedIndex == -1)
                pnlAdvanceAmount.Visibility = Visibility.Collapsed;
            else
                pnlAdvanceAmount.Visibility = Visibility.Visible;
        }
    }
}