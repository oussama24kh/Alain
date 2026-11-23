using ALAIN_DataAccess;
using ALAIN_DataAccess.DTO;

namespace ALAIN_Business
{
    public class Payment
    {
        public static ALAIN_DataAccess.Model.Payment? GetPaymentInfoByRentalID(int RentalID)
        {
            return PaymentData.GetPaymentInfoByRentalID(RentalID);
        }
        public static int? AddNewPayment(ALAIN_DataAccess.Model.Payment payment)
        {
            return PaymentData.AddNewPayment(payment);
        }
        public static bool EditPayment(ALAIN_DataAccess.Model.Payment payment)
        {
            return PaymentData.EditPayment(payment);
        }
        public static decimal? GetTotalPrice(int RentalID)
        {
            return PaymentData.GetTotalPrice(RentalID);
        }
        public static decimal CountRevenue(int Year,int Month)
        {
            return PaymentData.CountRevenue(Year, Month);
        }
        public static decimal CountRevenue(int Year, int Month,int Day)
        {
            return PaymentData.CountRevenue(Year, Month, Day);
        }
        public static decimal CountRevenue()
        {
            return PaymentData.CountRevenue();
        }
        public static decimal CountRevenueCurrentWeek()
        {
            return PaymentData.CountRevenueCurrentWeek();
        }
        public static int CountPayment(int Year, int Month)
        {
            return PaymentData.CountPayment(Year, Month);
        }
        public static List<Payment_DTO> GetAllPayments()
        {
            return PaymentData.GetAllPayments();
        }
        public static List<Payment_DTO> FilterPaymentByRentalCode(string RentalCode)
        {
            return PaymentData.FilterPaymentByRentalCode(RentalCode);
        }
        public static decimal GetTotalRevenueByRentalCode(string RentalCode)
        {
            return PaymentData.GetTotalRevenueByRentalCode(RentalCode);
        }
        public static List<Payment_DTO> FilterPaymentByPlateNumber(string PlateNumber)
        {
            return PaymentData.FilterPaymentByPlateNumber(PlateNumber);
        }
        public static decimal GetTotalRevenueByPlateNumber(string PlateNumber)
        {
            return PaymentData.GetTotalRevenueByPlateNumber(PlateNumber);
        }
        public static List<Payment_DTO> FilterPaymentByNationalNumber(string NationalNumber)
        {
            return PaymentData.FilterPaymentByNationalNumber(NationalNumber);
        }
        public static decimal GetTotalRevenueByNationalNumber(string NationalNumber)
        {
            return PaymentData.GetTotalRevenueByNationalNumber(NationalNumber);
        }
        public static List<Payment_DTO> FilterPaymentByMonth(int Month)
        {
            return PaymentData.FilterPaymentByMonth(Month);
        }
        public static decimal GetTotalRevenueByMonth(int Month)
        {
            return PaymentData.GetTotalRevenueByMonth(Month);
        }
        public static List<Payment_DTO> FilterPaymentsByPeriod(int Year, int Month)
        {
            return PaymentData.FilterPaymentsByPeriod(Year, Month);
        }
        public static List<int> GetPaymentYears()
        {
            return PaymentData.GetPaymentYears();
        }
    }
}