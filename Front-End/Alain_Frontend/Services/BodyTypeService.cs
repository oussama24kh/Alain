using System.Net.Http;
using System.Net.Http.Json;

namespace project_03.Services
{
    public class BodyTypeService
    {
        private readonly ApiClient _Client;
        public BodyTypeService(ApiClient client)
        {
            this._Client = client;
        }
        public Task<Dictionary<string, int>?> GetCarStatusCounts()
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
        public Task<List<string>?> GetAllBodyTypes()
        {
            return _Client.GetAsync<List<string>>("api/BodyType/All");
            /*try
            {
                HttpResponseMessage response = await _Client.GetAsync("api/BodyType/All");

                response.EnsureSuccessStatusCode();

                List<string>? BodyTypes = await response.Content.ReadFromJsonAsync<List<string>>();

                return BodyTypes ?? new List<string>();
            }
            catch { throw; }*/
        }
    }
}
