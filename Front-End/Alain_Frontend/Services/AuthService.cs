using project_03.DTO.Auth;
using System.Net.Http;

namespace project_03.Services
{
    public class AuthService
    {
        private readonly ApiClient _httpClient;
        public AuthService(ApiClient httpClient)
        {
            _httpClient = httpClient;
        }
        public Task<TokenResponse?> LoginAsync(LoginRequest request)
        {
            return _httpClient.PostAsync<TokenResponse>("api/auth/login", request);
        }
        public Task<TokenResponse?> RefreshTokenAsync(RefreshRequest request)
        {
            return _httpClient.PostAsync<TokenResponse>("api/auth/refresh", request);
        }
        public Task? LogoutAsync(LogoutRequest request)
        {
            return _httpClient.PostAsync<Task>("api/auth/logout", request);
        }
    }
}