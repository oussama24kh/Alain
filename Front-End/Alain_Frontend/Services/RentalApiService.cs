using project_03.DTO;

namespace project_03.Services
{
    public class RentalApiService
    {
        private readonly ApiClient _Client;
        public RentalApiService(ApiClient client)
        {
            this._Client = client;
        }
        public Task<List<Rental_DTO>?> GetAllRentals()
        {
            return _Client.GetAsync<List<Rental_DTO>>("api/Rental/All");
        }
        public Task<List<Rental_DTO>?> FilterRentalsByRentalStatus(string Status)
        {
            return _Client.GetAsync<List<Rental_DTO>>($"api/Rental/Status/{Status}");
        }
        public Task<List<Rental_DTO>?> FilterRentalsByRentalCode(string RentalCode)
        {
            return _Client.GetAsync<List<Rental_DTO>>($"api/Rental/Code/{RentalCode}");
        }
        public Task<List<Rental_DTO>?> FilterRentalsByPlateNumber(string PlateNumber)
        {
            return _Client.GetAsync<List<Rental_DTO>>($"api/Rental/PlateNumber/{PlateNumber}");
        }
        public Task<List<Rental_DTO>?> FilterRentalsByNationalNumber(string NationalNumber)
        {
            return _Client.GetAsync<List<Rental_DTO>>($"api/Rental/NationalNumber/{NationalNumber}");
        }
        public Task<List<Rental_DTO>?> GetLatestRentals(int NumberOfRentals)
        {
            return _Client.GetAsync<List<Rental_DTO>>($"api/Rental/Latest/{NumberOfRentals}");
        }
        public Task<int> CountRentalsByStatus(string Status)
        {
            return _Client.GetAsync<int>($"api/Rental/Count/Status/{Status}");
        }
        public Task<int> CountActiveRentals()
        {
            return _Client.GetAsync<int>("api/Rental/Count/Active");
        }
        public Task<int> CountRentalsByStatus(string Status, int Year, int Month)
        {
            return _Client.GetAsync<int>($"api/Rental/Count/Status/{Status}/Year/{Year}/Month/{Month}");
        }
        public Task<int> CountOverdueRentals()
        {
            return _Client.GetAsync<int>($"api/Rental/Count/Overdue");
        }
        public Task<int> CountCompletedRentals(int Year,int Month)
        {
            return _Client.GetAsync<int>($"api/Rental/Count/Completed/Year/{Year}/Month/{Month}");
        }
        public Task<Model.Rental?> AddNewRental(Model.Rental rental)
        {
            return _Client.PostAsync<Model.Rental>("api/Rental", rental);
        }
        public Task<Model.Rental?> GetRentalInfoByRentalID(int RentalID)
        {
            return _Client.GetAsync<Model.Rental>($"api/Rental/Find/RentalID/{RentalID}");
        }
        public Task<Model.Rental?> GetRentalInfoByRentalCode(string RentalCode)
        {
            return _Client.GetAsync<Model.Rental?>($"api/Rental/Find/RentalCode/{RentalCode}");
        }
        public Task<DTO.RentalDetails_DTO?> GetRentalDetailsByRentalCode(string RentalCode)
        {
            return _Client.GetAsync<DTO.RentalDetails_DTO?>($"api/Rental/RentalDetails/Find/RentalCode/{RentalCode}");
        }
        public Task EditRental(Model.Rental rental)
        {
            return _Client.PutAsync<Model.Rental>($"api/Rental/{rental.RentalID}", rental);
        }
        public Task<List<string>?> GetActiveStatus()
        {
            return _Client.GetAsync<List<string>>("api/RentalStatus/Active");
        }
        public Task<int> GetRentalDuration(string RentalCode)
        {
            return _Client.GetAsync<int>($"api/Rental/Duration/RentalCode/{RentalCode}");
        }
    }
}