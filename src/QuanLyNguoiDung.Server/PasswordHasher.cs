using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace QuanLyNguoiDung.Server
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16; // 16 byte salt
        private const int KeySize = 32;  // 256 bit hash
        private const int Iterations = 600000; // Đúng 600.000 vòng theo phân công của nhóm

        // Sinh Salt 16 byte ngẫu nhiên
        public static string GenerateSalt()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            return Convert.ToBase64String(salt);
        }

        // Băm mật khẩu dùng PBKDF2 với SHA-256
        public static string HashPassword(string password, string saltBase64)
        {
            byte[] salt = Convert.FromBase64String(saltBase64);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                KeySize
            );
            return Convert.ToBase64String(hash);
        }

        // So sánh chống Timing Attack
        public static bool VerifyPassword(string inputPassword, string storedHash, string storedSalt)
        {
            string inputHash = HashPassword(inputPassword, storedSalt);
            byte[] inputBytes = Convert.FromBase64String(inputHash);
            byte[] storedBytes = Convert.FromBase64String(storedHash);

            return CryptographicOperations.FixedTimeEquals(inputBytes, storedBytes);
        }
    }
}
