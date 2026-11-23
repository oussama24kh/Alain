using System.Net.Http;

namespace project_03.Services
{
    public class BrandApiService
    {
        private readonly ApiClient _Client;
        public BrandApiService(ApiClient client)
        {
            this._Client = client;
        }
        public Task<List<string>?> GetAllBrands()
        {
            return _Client.GetAsync<List<string>?>("api/Brand/All");
        }
    }
}