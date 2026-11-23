using project_03.Services;
using System.Windows.Controls;
using System.Windows;
using project_03.DTO;

namespace project_03.Payment
{
    /// <summary>
    /// Interaction logic for PaymentView.xaml
    /// </summary>
    public partial class PaymentView : UserControl
    {
        private PaymentApiService _PaymentApiService;
        public PaymentView()
        {
            InitializeComponent();

            _PaymentApiService = new PaymentApiService(new ApiClient());
            CmbFilterField.SelectionChanged += CmbFilterField_SelectionChanged;
            TxtFilterValue.TextChanged += TxtFilterValue_TextChanged;
            CmbFilterMonth.SelectionChanged += CmbFilterMonth_SelectionChanged;
            CmbFilterYear.SelectionChanged += CmbFilterMonth_SelectionChanged;
        }
        private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await _LoadPayments();
        }
        private async Task _LoadPayments()
        {
            await ErrorHandler.ExecuteAsync(async () =>
            {
                var PaymentDataTask = _PaymentApiService.GetAllPayments();
                var MonthRevenueTask = _PaymentApiService.GetRevenue(DateTime.Now.Year, DateTime.Now.Month);
                var WeekRevenueTask = _PaymentApiService.CountCurrentWeekRevenue();
                var DayRevenueTask = _PaymentApiService.GetRevenue(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var CountCompletedPaymentTask = _PaymentApiService.CountPayment(DateTime.Now.Year, DateTime.Now.Month);
                var CountTotalRevenue = _PaymentApiService.GetRevenue();

                await Task.WhenAll(PaymentDataTask, DayRevenueTask, MonthRevenueTask, CountCompletedPaymentTask, WeekRevenueTask);

                _LoadDataToForm(await PaymentDataTask, await MonthRevenueTask, await WeekRevenueTask,
                    await DayRevenueTask, await CountCompletedPaymentTask, await CountTotalRevenue);
            });
        }
        private void _LoadDataToForm(List<DTO.Payment_DTO>? payments, decimal RevenueMonth, decimal RevenueWeek,
            decimal RevenueDay, int CompletedPayment, decimal TotalRevenue)
        {
            if (payments == null || payments.Count == 0)
            {
                FilterGrid.Visibility = Visibility.Collapsed;
                PaymentsGrid.Visibility = Visibility.Collapsed;
                PaymentEmptyStatePanel.Visibility = Visibility.Visible;
            }
            else
            {
                PaymentEmptyStatePanel.Visibility = Visibility.Collapsed;
                FilterGrid.Visibility = Visibility.Visible;
                PaymentsGrid.Visibility = Visibility.Visible;
                RevenueTotal_Stack.Visibility = Visibility.Visible;

                PaymentsGrid.ItemsSource = payments;
            }
            TxtMonthRevenue.Text = $"{RevenueMonth.ToString("N0")} MAD";
            TxtCurrentWeekRevenue.Text = $"{RevenueWeek.ToString("N0")} MAD";
            TxtCollectedToday.Text = $"{RevenueDay.ToString("N0")} MAD";
            TxtTotalPaidCount.Text = CompletedPayment.ToString();
            txtblk_TotalRevenue.Text = $"{TotalRevenue.ToString("N2")} MAD";
        }
        private void _LoadDataToForm(List<DTO.Payment_DTO>? payments, decimal? TotalRevenue)
        {
            if (payments == null || payments.Count == 0)
            {
                RevenueTotal_Stack.Visibility = Visibility.Hidden;
                PaymentsGrid.Visibility = Visibility.Collapsed;
                PaymentEmptyStatePanel.Visibility = Visibility.Visible;
            }
            else
            {
                PaymentEmptyStatePanel.Visibility = Visibility.Collapsed;
                PaymentsGrid.Visibility = Visibility.Visible;
                RevenueTotal_Stack.Visibility = Visibility.Visible;

                txtblk_TotalRevenue.Text = $"{TotalRevenue.ToString()} MAD";
                PaymentsGrid.ItemsSource = payments;
            }
        }
        private async void CmbFilterField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbFilterField.SelectedIndex <= 0)
            {
                Period_StackPanel.Visibility = Visibility.Collapsed;
                TxtFilterValue.Visibility = Visibility.Collapsed;
                await _LoadPayments();
            }
            else if (CmbFilterField.SelectedIndex == 4)
            {
                TxtFilterValue.Visibility = Visibility.Collapsed;
                Period_StackPanel.Visibility = Visibility.Visible;

                List<int>? years = await _PaymentApiService.GetPaymentYears();
                if (years != null && years.Count > 0)
                {
                    if (years.Count > 1)
                        CmbFilterYear.Items.Add("Toutes les années");

                    foreach(var year in years)
                        CmbFilterYear.Items.Add(year);

                    CmbFilterYear.SelectedIndex = 0;
                }
                CmbFilterMonth.SelectedIndex = 0;
            }
            else
            {
                TxtFilterValue.Text = "";

                Period_StackPanel.Visibility = Visibility.Collapsed;
                TxtFilterValue.Visibility = Visibility.Visible;
            }
        }
        private async void TxtFilterValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtFilterValue.Text))
            {
                await _LoadPayments();
            }
            else
            {
                bool RequestSucceeded = false;

                Task<decimal>? TotalRevenueTask = null;
                Task<List<Payment_DTO>?>? paymentsTask = null;

                await ErrorHandler.ExecuteAsync(async () =>
                {
                    switch(CmbFilterField.SelectedIndex)
                    {
                        case 1:
                            paymentsTask = _PaymentApiService.FilterPaymentsByRentalCode(TxtFilterValue.Text);
                            TotalRevenueTask = _PaymentApiService.GetTotalRevenueByRentalCode(TxtFilterValue.Text);

                            RequestSucceeded = true;
                            break;

                        case 2:
                            paymentsTask = _PaymentApiService.FilterPaymentsByPlateNumber(TxtFilterValue.Text);
                            TotalRevenueTask = _PaymentApiService.GetTotalRevenueByPlateNumber(TxtFilterValue.Text);

                            RequestSucceeded = true;
                            break;

                        case 3:
                            paymentsTask = _PaymentApiService.FilterPaymentsByNationalNumber(TxtFilterValue.Text);
                            TotalRevenueTask = _PaymentApiService.GetTotalRevenueByNationalNumber(TxtFilterValue.Text);

                            RequestSucceeded = true;
                            break;
                        default:
                            return;
                    }
                });
                await Task.WhenAll(new Task?[] { paymentsTask, TotalRevenueTask }.OfType<Task>());

                if (RequestSucceeded)
                    _LoadDataToForm(await paymentsTask!, await TotalRevenueTask!);
            }
        }
        private async void CmbFilterMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Task<decimal>? TotalRevenueTask = null;
            Task<List<Payment_DTO>?>? paymentsTask = null;
            if (CmbFilterField.SelectedIndex == 4)
            {
                if (CmbFilterYear.SelectedIndex <= 0)
                {
                    if (CmbFilterMonth.SelectedIndex <= 0)
                    {
                        await _LoadPayments();
                        return;
                    }
                    int Month = CmbFilterMonth.SelectedIndex;
                    paymentsTask = _PaymentApiService.FilterPaymentsByMonth(Month);
                    TotalRevenueTask = _PaymentApiService.GetTotalRevenueByMonth(Month);
                }
                else
                {
                    int Year = Convert.ToInt32(CmbFilterYear.SelectedValue);
                    int Month = CmbFilterMonth.SelectedIndex;
                    paymentsTask = _PaymentApiService.FilterPaymentsByPeriod(Year, Month);
                    TotalRevenueTask = _PaymentApiService.GetRevenue(Year, Month);
                }
            }
            await Task.WhenAll(new Task?[] { paymentsTask, TotalRevenueTask }.OfType<Task>());    
            _LoadDataToForm(await paymentsTask!, await TotalRevenueTask!);
        }
        private async void BtnReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Payment_DTO selectedPayment)
            {
                int Duration = 0;
                await ErrorHandler.ExecuteAsync(async () =>
                {
                    RentalApiService rentalApiService = new RentalApiService(new ApiClient());
                    Duration = await rentalApiService.GetRentalDuration(selectedPayment.RentalCode);
                });

                var receiptWindow = new ReceiptWindow(selectedPayment, Duration)
                {
                    Owner = Window.GetWindow(this)
                };

                receiptWindow.ShowDialog();
            }
        }
    }
}