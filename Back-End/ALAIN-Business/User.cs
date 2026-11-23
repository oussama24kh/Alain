using ALAIN_Business.Auth;
using ALAIN_DataAccess;
using ALAIN_DataAccess.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ALAIN_Business
{
    public class User
    {
        public static bool ChangeUserStatus(string UserName, bool Status)
        {
            return Status ? BanUser(UserName) : UnbanUser(UserName);
        }
        public static Dictionary<string,int>? GetUserStatistics()
        {
            return UserData.GetUserStatistics();
        }
        private static bool BanUser(string UserName)
        {
            return UserData.BanUser(UserName);
        }
        private static bool UnbanUser(string UserName)
        {
            return UserData.UnbanUser(UserName);
        }
        public static List<User_DTO> GetAllUsers()
        {
            return UserData.GetAllUsers();
        }
        public static int? AddNewUser(User_DTO user)
        {
            user.Password = PasswordHasher.HashPassword(user.Password);

            return UserData.AddNewUser(user);
        }
        public static bool EditUser(User_DTO user)
        {
            user.Password = PasswordHasher.HashPassword(user.Password);

            return UserData.EditUser(user);
        }
        public static bool CheckIfUsernameExist(string Username)
        {
            return UserData.CheckIfUsernameExist(Username);
        }
        public static User_DTO? GetUserInfoByUsername(string Username)
        {
            return UserData.GetUserInfoByUsername(Username);
        }
        public static User_DTO? GetUserInfoByUserID(int UserID)
        {
            return UserData.GetUserInfoByUserID(UserID);
        }
        private static string GenerateAccessToken(User_DTO user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("THIS_IS_A_VERY_SECRET_KEY_123456"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "Alain_Api",
                audience: "Alain_Api_Users",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private static string HashRefreshToken(string refreshToken)
        {
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));

            return Convert.ToHexString(hash);
        }
        public static TokenResponse? Login(LoginRequest request)
        {
            User_DTO? login = UserData.GetUserInfoByUsername(request.Username);
            if (login == null)
                return null;

            if(!PasswordHasher.VerifyPassword(request.Password, login.Password))
                return null;

            if (!login.IsActive)
                return null;

            string accessToken = GenerateAccessToken(login);
            var refreshToken = GenerateRefreshToken();

            login.RefreshTokenHash = HashRefreshToken(refreshToken);
            login.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
            login.RefreshTokenRevokedAt = null;

            UserData.UpdateRefreshToken(
                login.Id,
                login.RefreshTokenHash,
                login.RefreshTokenExpiresAt
            );

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
        public static TokenResponse? Refresh(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return null;

            string hash = HashRefreshToken(refreshToken);

            // Retrieve the user using the refresh token hash.
            User_DTO? user = UserData.GetUserByRefreshTokenHash(hash);

            if (user == null)
                return null;

            if (!user.IsActive)
                return null;

            if (user.RefreshTokenRevokedAt != null)
                return null;

            if (user.RefreshTokenExpiresAt <= DateTime.UtcNow)
                return null;

            // Generate a new refresh token.
            string newRefreshToken = GenerateRefreshToken();

            string newHash = HashRefreshToken(newRefreshToken);

            DateTime expiration = DateTime.UtcNow.AddDays(7);

            // Atomically replace the old refresh token.
            bool updated = UserData.RotateRefreshToken(
                user.Id,
                hash,
                newHash,
                expiration
            );

            if (!updated)
                return null;

            return new TokenResponse
            {
                AccessToken = GenerateAccessToken(user),
                RefreshToken = newRefreshToken
            };
        }
        
        public static bool Logout(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return false;

            string hash = HashRefreshToken(refreshToken);

            User_DTO? user = UserData.GetUserByRefreshTokenHash(hash);

            if (user == null)
                return false;

            if (user.RefreshTokenRevokedAt != null)
                return false;

            return UserData.RevokeRefreshToken(
                user.Id,
                hash,
                DateTime.UtcNow
            );
        }
        private static string GenerateRefreshToken()
        {
            byte[] randomBytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(randomBytes);
        }
    }
}