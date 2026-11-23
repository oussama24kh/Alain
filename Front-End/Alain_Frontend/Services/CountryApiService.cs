namespace project_03.Services
{
    public class CountryApiService
    {
        private readonly ApiClient _Client;
        public CountryApiService(ApiClient client)
        {
            this._Client = client;
        }
        public Task<List<string>?> GetAllCountries()
        {
            return _Client.GetAsync<List<string>>("api/Country/All");
        }
        public Task<List<string>?> GetActiveCountries()
        {
            return _Client.GetAsync<List<string>>("api/Country/Active");
        }
    }
}