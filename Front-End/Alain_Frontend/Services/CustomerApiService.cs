using project_03.DTO;

namespace project_03.Services
{
    public class CustomerApiService
    {
        private readonly ApiClient _Client;
        public CustomerApiService(ApiClient client)
        {
            this._Client = client;
        }
        public Task<List<Model.Customer>?> GetAllCustomers()
        {
            return _Client.GetAsync<List<Model.Customer>>("api/Customer/All");
        }
        public Task<List<Model.Customer>?> GetCustomersByNationality(string Nationality)
        {
            return _Client.GetAsync<List<Model.Customer>?>($"api/Customer/Nationality/{Nationality}");
        }
        public Task<List<Model.Customer>?> FilterCustomersByCustomerName(string CustomerName)
        {
            return _Client.GetAsync<List<Model.Customer>?>($"api/Customer/Name/{CustomerName}");
        }
        public Task<List<Model.Customer>?> FilterCustomersByNationalNumber(string NationalNumber)
        {
            return _Client.GetAsync<List<Model.Customer>?>($"api/Customer/NationalNumber/{NationalNumber}");
        }
        public Task<List<Model.Customer>?> FilterCustomersByPhoneNumber(string PhoneNumber)
        {
            return _Client.GetAsync<List<Model.Customer>?>($"api/Customer/Phone/{PhoneNumber}");
        }
        public Task<int> CountCustomers(int Year,int Month)
        {
            return _Client.GetAsync<int>($"api/Customer/Count/Year/{Year}/Month/{Month}");
        }
        public Task<int> CountCustomers()
        {
            return _Client.GetAsync<int>($"api/Customer/Count");
        }
        public Task<Model.Customer?> GetCustomerInfoByNationalNumber(string NationalNumber)
        {
            return _Client.GetAsync<Model.Customer>($"api/Customer/Find/NationalNumber/{NationalNumber}");
        }
        public Task<Model.Customer?> GetCustomerInfoByCustomerID(int CustomerID)
        {
            return _Client.GetAsync<Model.Customer>($"api/Customer/Find/CustomerID/{CustomerID}");
        }
        public Task<int> AddNewCustomer(Model.Customer customer)
        {
            return _Client.PostAsync<int>("api/Customer", customer);
        }
        public Task EditCustomer(Model.Customer customer)
        {
            return _Client.PutAsync<Model.Customer>($"api/Customer/{customer.Id}", customer);
        }
        public Task<int> DeleteCustomer(string NationalNumber)
        {
            return _Client.DeleteAsync<int>($"api/Customer/{NationalNumber}");
        }
        public Task<bool> CheckIfNationalNumberAlreadyExist(string NationalNumber)
        {
            return _Client.GetAsync<bool>($"api/Customer/Check/Exist/NationalNumber/{NationalNumber}");
        }
    }
}