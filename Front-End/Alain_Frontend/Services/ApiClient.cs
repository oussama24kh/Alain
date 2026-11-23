using project_03.DTO.Auth;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Windows;
using System.Windows.Xps;

namespace project_03.Services
{
    public class ApiClient
    {
        private readonly SemaphoreSlim _refreshLock = new(1, 1);
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public HttpClient Client { get; }
        public ApiClient()
        {
            Client = new HttpClient { BaseAddress = new Uri("https://localhost:7103") };
        }
        public Task<T?> GetAsync<T>(string url)
        {
            return SendAsync<T>(
                new HttpRequestMessage(HttpMethod.Get, url),
                notFoundReturnsDefault: true);
        }
        public Task<T?> PostAsync<T>(string url, object data)
        {
            return SendAsync<T>(
                new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = JsonContent.Create(data)
                });
        }
        public Task<T?> PutAsync<T>(string url, object data)
        {
            return SendAsync<T>(
                new HttpRequestMessage(HttpMethod.Put, url)
                {
                    Content = JsonContent.Create(data)
                });
        }
        public Task<T?> DeleteAsync<T>(string url)
        {
            return SendAsync<T>(new HttpRequestMessage(HttpMethod.Delete, url));
        }
        private async Task<T?> SendAsync<T>(HttpRequestMessage request, bool notFoundReturnsDefault = false)
        {
            try
            {
                // Clone the request before sending it.
                // HttpRequestMessage cannot be sent twice.
                using (request)
                using (var retryRequest = await CloneRequestAsync(request))
                {
                    // Attach current access token
                    SetAuthorizationHeader(request);

                    var response = await Client.SendAsync(request);

                    if (response.StatusCode == HttpStatusCode.Unauthorized && App.AuthState.IsAuthenticated)
                    {
                        response.Dispose();

                        try
                        {
                            await RefreshAccessTokenAsync();
                        }
                        catch (UnauthorizedAccessException)
                        {
                            App.RedirectToLogin();
                            throw;
                        }

                        SetAuthorizationHeader(retryRequest);

                        response = await Client.SendAsync(retryRequest);
                    }

                    using (response)
                    {
                        if (notFoundReturnsDefault && response.StatusCode == HttpStatusCode.NotFound)
                        {
                            return default;
                        }

                        string content = await response.Content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new ApiException(
                                response.StatusCode,
                                content);
                        }

                        if (string.IsNullOrWhiteSpace(content))
                            return default;

                        return JsonSerializer.Deserialize<T>(content, JsonOptions);
                    }
                }
            }
            catch (HttpRequestException)
            {
                throw new ApiConnectionException(
                    "Impossible de se connecter à l'API.");
            }
            catch (TaskCanceledException)
            {
                throw new TimeoutException(
                    "Le délai d'attente de la requête a expiré.");
            }
        }
        private void SetAuthorizationHeader(HttpRequestMessage request)
        {
            request.Headers.Authorization = null;

            if (App.AuthState.IsAuthenticated)
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", App.AuthState.AccessToken);
            }
        }
        private async Task RefreshAccessTokenAsync()
        {
            await _refreshLock.WaitAsync();

            try
            {
                var currentToken = App.AuthState.AccessToken;

                if (string.IsNullOrWhiteSpace(App.AuthState.RefreshToken))
                {
                    App.AuthState.Clear();

                    throw new UnauthorizedAccessException(
                        "Your session has expired. Please log in again.");
                }

                var request = new RefreshRequest
                {
                    RefreshToken = App.AuthState.RefreshToken
                };

                using var response = await Client.PostAsJsonAsync(
                    "api/auth/refresh",
                    request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    App.AuthState.Clear();

                    throw new UnauthorizedAccessException(
                        "Your session has expired. Please log in again.");
                }

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<TokenResponse>(JsonOptions);

                if (result == null ||
                    string.IsNullOrWhiteSpace(result.AccessToken) ||
                    string.IsNullOrWhiteSpace(result.RefreshToken))
                {
                    App.AuthState.Clear();

                    throw new UnauthorizedAccessException(
                        "Invalid refresh token response.");
                }

                // Save both new tokens
                App.AuthState.SetTokens(result);
            }
            finally
            {
                _refreshLock.Release();
            }
        }
        private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri);

            foreach (var header in request.Headers)
            {
                if (header.Key.Equals(
                    "Authorization",
                    StringComparison.OrdinalIgnoreCase))
                    continue;

                clone.Headers.TryAddWithoutValidation(
                    header.Key,
                    header.Value);
            }

            if (request.Content != null)
            {
                var content = await request.Content.ReadAsByteArrayAsync();

                clone.Content = new ByteArrayContent(content);

                foreach (var header in request.Content.Headers)
                {
                    clone.Content.Headers.TryAddWithoutValidation(
                        header.Key,
                        header.Value);
                }
            }
            return clone;
        }
    }
}