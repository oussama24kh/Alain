using ALAIN_DataAccess;

namespace ALAIN_Business
{
    public class Customer
    {
        public static List<ALAIN_DataAccess.Model.Customer> GetAllCutomers()
        {
            return CustomerData.GetAllCutomers();
        }
        public static List<ALAIN_DataAccess.Model.Customer> FilterCustomersByNationality(string Nationality)
        {
            return CustomerData.FilterCustomersByNationality(Nationality);
        }
        public static List<ALAIN_DataAccess.Model.Customer> FilterCustomersByCustomerName(string Name)
        {
            return CustomerData.FilterCustomersByCustomerName(Name);
        }
        public static List<ALAIN_DataAccess.Model.Customer> FilterCustomersByNationalNumber(string NationalNumber)
        {
            return CustomerData.FilterCustomersByNationalNumber(NationalNumber);
        }
        public static List<ALAIN_DataAccess.Model.Customer> FilterCustomersByPhoneNumber(string Phone)
        {
            return CustomerData.FilterCustomersByPhoneNumber(Phone);
        }
        public static ALAIN_DataAccess.Model.Customer? GetCustomerInfoByNationalNumber(string NationalNumber)
        {
            return CustomerData.GetCustomerInfoByNationalNumber(NationalNumber);
        }
        public static ALAIN_DataAccess.Model.Customer? GetCustomerInfoByCustomerID(int CustomerID)
        {
            return CustomerData.GetCustomerInfoByCustomerID(CustomerID);
        }
        public static bool CheckIfNationalNumberAlreadyExist(string NationalNo)
        {
            return CustomerData.CheckIfNationalNumberAlreadyExist(NationalNo);
        }
        public static int CountCustomerNew(int Year,int Month)
        {
            return CustomerData.CountCustomerNew(Year, Month);
        }
        public static int CountTotalCustomers()
        {
            return CustomerData.CountTotalCustomers();
        }
        public static int? AddNewCustomer(ALAIN_DataAccess.Model.Customer customer)
        {
            return CustomerData.AddNewCustomer(customer);
        }
        public static bool EditCustomer(ALAIN_DataAccess.Model.Customer customer)
        {
            return CustomerData.EditCustomer(customer);
        }
        public static int DeleteCustomer(string customer)
        {
            return CustomerData.DeleteCustomer(customer);
        }
    }
}