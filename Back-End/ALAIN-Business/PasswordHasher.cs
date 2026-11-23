using System.Security.Cryptography;

namespace ALAIN_Business
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 600_000;
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize
            );

            byte[] result = new byte[SaltSize + HashSize];

            Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, result, SaltSize, HashSize);

            return Convert.ToBase64String(result);
        }
        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) ||
                string.IsNullOrWhiteSpace(storedHash))
                return false;

            try
            {
                byte[] decoded = Convert.FromBase64String(storedHash);

                if (decoded.Length != SaltSize + HashSize)
                    return false;

                byte[] salt = decoded[..SaltSize];
                byte[] originalHash = decoded[SaltSize..];

                byte[] newHash = Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    Iterations,
                    HashAlgorithmName.SHA256,
                    HashSize
                );

                return CryptographicOperations.FixedTimeEquals(
                    originalHash,
                    newHash
                );
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}