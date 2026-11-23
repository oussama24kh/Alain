using Microsoft.VisualBasic;
using project_03.DTO;
using System.Net.Http;
using System.Net.Http.Json;

namespace project_03.Services
{
    public class CarApiService
    {
        private readonly ApiClient _Client;
        public CarApiService(ApiClient client)
        {
            this._Client = client;
        }
        public Task<List<Car_DTO>?> GetAllCars()
        {
            return _Client.GetAsync<List<Car_DTO>>("api/Car/All");
        }
        public Task<List<Car_DTO>?> FilterCarsByBodyType(string BodyType)
        {
            return _Client.GetAsync<List<Car_DTO>>($"api/Car/BodyType/{BodyType}");
            /*try
            {
                //HttpResponseMessage response = await _Client.GetAsync($"api/Car/BodyType/{BodyType}");
                //response.EnsureSuccessStatusCode();

                List<Car_DTO>? cars = await _Client.GetFromJsonAsync<List<Car_DTO>>($"api/Car/BodyType/{BodyType}");
                return cars ?? new List<Car_DTO>();
            }
            catch { return new List<Car_DTO>(); }*/
        }
        public Task<List<Car_DTO>?> FilterCarsByPlateNumber(string PlateNumber)
        {
            return _Client.GetAsync<List<Car_DTO>>($"api/Car/PlateNumber/{PlateNumber}");
            /*try
            {
                List<Car_DTO>? cars = await _Client.GetFromJsonAsync<List<Car_DTO>>($"api/Car/PlateNumber/{PlateNumber}");
                return cars ?? new List<Car_DTO>();
            }
            catch { return new List<Car_DTO>(); }*/
        }
        public Task<List<Car_DTO>?> FilterCarsByBrand(string Brand)
        {
            return _Client.GetAsync<List<Car_DTO>>($"api/Car/Brand/{Brand}");
            /*try
            {
                List<Car_DTO>? cars = await _Client.GetFromJsonAsync<List<Car_DTO>>($"api/Car/Brand/{Brand}");
                return cars ?? new List<Car_DTO>();
            }
            catch { return new List<Car_DTO>(); }*/
        }
        public Task<List<Car_DTO>?> FilterCarsByModel(string Model)
        {
            return _Client.GetAsync<List<Car_DTO>?>($"api/Car/Model/{Model}");
            /*try
            {
                List<Car_DTO>? cars = await _Client.GetFromJsonAsync<List<Car_DTO>>($"api/Car/Model/{Model}");
                return cars ?? new List<Car_DTO>();
            }
            catch { return new List<Car_DTO>(); }*/
        }
        public Task<List<Car_DTO>?> FilterCarsByStatus(string Status)
        {
            return _Client.GetAsync<List<Car_DTO>?>($"api/Car/Status/{Status}");
        }
        public Task<int> CountCarsByStatus(string Status)
        {
            return _Client.GetAsync<int>($"api/Car/Count/Status/{Status}");
        }
        public Task<int> CountAllCars()
        {
            return _Client.GetAsync<int>("api/Car/Count/All");
        }
        public Task<Dictionary<string,int>?> GetCarStatusCounts()
        {
            return _Client.GetAsync<Dictionary<string, int>>("api/Car/Statistics/All");
            /*try
            {
                HttpResponseMessage response = await _Client.GetAsync("api/Car/Statistics/All");

                response.EnsureSuccessStatusCode();

                Dictionary<string, int>? carStatusCounts = await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();

                return carStatusCounts ?? new Dictionary<string, int>();
            }
            catch { throw; }*/
        }
        public Task<Dictionary<string, int>?> GetCarStatusCounts(string BodyType)
        {
            return _Client.GetAsync<Dictionary<string, int>>($"api/Car/Statistics/BodyType/{BodyType}");
        }
        public Task<Model.Car?> GetCarInfoByPlateNumber(string PlateNumber)
        {
            return _Client.GetAsync<Model.Car>($"api/Car/Find/PlateNumber/{PlateNumber}");
        }
        public Task<Model.Car?> GetCarInfoByCarID(int CarID)
        {
            return _Client.GetAsync<Model.Car>($"api/Car/Find/CarID/{CarID}");
        }
        public Task<int> DeleteCar(string PlateNumber)
        {
            return _Client.DeleteAsync<int>($"api/Car/{PlateNumber}");
        }
        public Task<int> AddNewCar(Model.Car car)
        {
            return _Client.PostAsync<int>("api/Car", car);
        }
        public Task EditCar(Model.Car car)
        {
            return _Client.PutAsync<Model.Car>($"api/Car/{car.Id}",car);
        }
        public Task<bool> CheckIfPlateAlreadyExist(string PlateNumber)
        {
            return _Client.GetAsync<bool>($"api/Car/Check/Exist/PlateNumber/{PlateNumber}");
        }
    }
}