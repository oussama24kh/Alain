using project_03.DTO;
using project_03.Model;

namespace project_03.Services
{
    public class UserApiService
    {
        private readonly ApiClient _Client;
        public UserApiService(ApiClient client)
        {
            _Client = client;
        }
        public Task<List<User_DTO>?> GetAllUsers()
        {
            return _Client.GetAsync<List<User_DTO>>("api/User/All");
        }
        public Task<Company?> GetCompanyInfo()
        {
            return _Client.GetAsync<Company>("api/Company");
        }
        public Task<bool> SaveCompanySettings(int CompanyID, Company companySettings)
        {
            return _Client.PutAsync<bool>($"api/Company/CompanyID/{CompanyID}", companySettings);
        }
        public Task<int> AddNewUser(User_DTO user)
        {
            return _Client.PostAsync<int>("api/User", user);
        }
        public Task EditUser(User_DTO user)
        {
            return _Client.PutAsync<User_DTO>($"api/User/{user.Id}", user);
        }
        public Task<bool> CheckIfUsernameExist(string Username)
        {
            return _Client.GetAsync<bool>($"api/User/Check/Exist/Username/{Username}");
        }
        public Task<User_DTO?> GetUserInfoByUsername(string Username)
        {
            return _Client.GetAsync<User_DTO>($"api/User/Find/Username/{Username}");
        }
        public Task<User_DTO?> GetUserInfoByUserID(int UserID)
        {
            return _Client.GetAsync<User_DTO>($"api/User/Find/UserID/{UserID}");
        }
        public Task<bool> ChangeUserStatus(string UserName, bool Status)
        {
            return _Client.PutAsync<bool>($"api/User/UserName/{UserName}/Status/{Status}", UserName);
        }
        public Task<Dictionary<string,int>?> GetUsersStatistics()
        {
            return _Client.GetAsync<Dictionary<string, int>>($"api/User/Statistics");
        }
    }
}