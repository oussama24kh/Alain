namespace project_03.Services
{
    public class PaymentApiService
    {
        private readonly ApiClient _Client;
        public PaymentApiService(ApiClient client)
        {
            this._Client = client;
        }
        public Task<List<int>?> GetPaymentYears()
        {
            return _Client.GetAsync<List<int>>("api/Payment/Years");
        }
        public Task<List<DTO.Payment_DTO>?> GetAllPayments()
        {
            return _Client.GetAsync<List<DTO.Payment_DTO>>("api/Payment/All");
        }
        public Task<int> CountPayment(int Year, int Month)
        {
            return _Client.GetAsync<int>($"api/Payment/Count/Year/{Year}/Month/{Month}");
        }
        public Task<decimal> CountCurrentWeekRevenue()
        {
            return _Client.GetAsync<decimal>("api/Payment/Revenue/CurrentWeek");
        }
        public Task<decimal> GetRevenue()
        {
            return _Client.GetAsync<decimal>("api/Payment/Revenue/Total");
        }
        public Task<decimal> GetRevenue(int Year, int Month)
        {
            return _Client.GetAsync<decimal>($"api/Payment/Revenue/Year/{Year}/Month/{Month}");
        }
        public Task<decimal> GetRevenue(int Year, int Month, int Day)
        {
            return _Client.GetAsync<decimal>($"api/Payment/Revenue/Year/{Year}/Month/{Month}/Day/{Day}");
        }
        public Task<decimal> GetTotalAmountByRentalID(int rentalID)
        {
            return _Client.GetAsync<decimal>($"api/Payment/TotalAmount/{rentalID}");
        }
        public Task<Model.Payment?> GetPaymentInfoByRentalID(int RentalID)
        {
            return _Client.GetAsync<Model.Payment>($"api/Payment/Find/RentalID/{RentalID}");
        }
        public Task<int?> AddNewPayment(Model.Payment payment)
        {
            return _Client.PostAsync<int?>("api/Payment", payment);
        }
        public Task EditPayment(Model.Payment payment)
        {
            return _Client.PutAsync<Model.Payment>($"api/Payment/{payment.PaymentID}", payment);
        }
        public Task<List<DTO.Payment_DTO>?> FilterPaymentsByRentalCode(string RentalCode)
        {
            return _Client.GetAsync<List<DTO.Payment_DTO>>($"api/Payment/RentalCode/{RentalCode}");
        }
        public Task<decimal> GetTotalRevenueByRentalCode(string RentalCode)
        {
            return _Client.GetAsync<decimal>($"api/Payment/Revenue/RentalCode/{RentalCode}");
        }
        public Task<List<DTO.Payment_DTO>?> FilterPaymentsByPlateNumber(string PlateNumber)
        {
            return _Client.GetAsync<List<DTO.Payment_DTO>>($"api/Payment/PlateNumber/{PlateNumber}");
        }
        public Task<decimal> GetTotalRevenueByPlateNumber(string PlateNumber)
        {
            return _Client.GetAsync<decimal>($"api/Payment/Revenue/PlateNumber/{PlateNumber}");
        }
        public Task<List<DTO.Payment_DTO>?> FilterPaymentsByNationalNumber(string NationalNumber)
        {
            return _Client.GetAsync<List<DTO.Payment_DTO>>($"api/Payment/NationalNumber/{NationalNumber}");
        }
        public Task<decimal> GetTotalRevenueByNationalNumber(string NationalNumber)
        {
            return _Client.GetAsync<decimal>($"api/Payment/Revenue/NationalNumber/{NationalNumber}");
        }
        public Task<List<DTO.Payment_DTO>?> FilterPaymentsByMonth(int Month)
        {
            return _Client.GetAsync<List<DTO.Payment_DTO>>($"api/Payment/Month/{Month}");
        }
        public Task<decimal> GetTotalRevenueByMonth(int Month)
        {
            return _Client.GetAsync<decimal>($"api/Payment/Revenue/Month/{Month}");
        }
        public Task<List<DTO.Payment_DTO>?> FilterPaymentsByPeriod(int Year, int Month)
        {
            return _Client.GetAsync<List<DTO.Payment_DTO>>($"api/Payment/Year/{Year}/Month/{Month}");
        }
    }
}