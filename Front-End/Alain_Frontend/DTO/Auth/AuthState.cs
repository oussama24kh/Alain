using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace project_03.DTO.Auth
{
    public class AuthState
    {
        public bool IsAuthenticated { get; private set; }
        public string AccessToken { get; private set; } = "";
        public string RefreshToken { get; private set; } = "";
        public string? UserID { get; private set; }
        public string? Role { get; private set; }
        public void SetTokens(TokenResponse response)
        {
            AccessToken = response.AccessToken;
            RefreshToken = response.RefreshToken;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(response.AccessToken);

            UserID = jwt.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier ||
                c.Type == "sub")?.Value;

            Role = jwt.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Role ||
                c.Type == "role" ||
                c.Type == "roles")?.Value;

            IsAuthenticated = true;
        }
        public void Clear()
        {
            AccessToken = "";
            RefreshToken = "";
            UserID = null;
            Role = null;

            IsAuthenticated = false;
        }
    }
}