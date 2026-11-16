using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyNguoiDung.Shared.Messages;

namespace QuanLyNguoiDung.Server
{
    public class NguoiDungRepository
    {
        private readonly string _connectionString;

        public NguoiDungRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Tìm người dùng theo Username (Lấy thông tin + Hash + Salt để kiểm tra đăng nhập)
        public (int UserId, string Username, string Hash, string Salt, string Email, string FullName, DateOnly? Birthday)? GetByUsername(string username)
        {
            string sql = "SELECT UserId, Username, PasswordHash, PasswordSalt, Email, FullName, Birthday FROM NguoiDung WHERE Username = @Username";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return (
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.IsDBNull(6) ? null : DateOnly.FromDateTime(reader.GetDateTime(6))
                );
            }
            return null;
        }

        // Tìm người dùng theo UserId (dùng cho LAYTHONGTIN, phiên chỉ lưu UserId)
        public (int UserId, string Username, string Hash, string Salt, string Email, string FullName, DateOnly? Birthday)? GetById(int userId)
        {
            string sql = "SELECT UserId, Username, PasswordHash, PasswordSalt, Email, FullName, Birthday FROM NguoiDung WHERE UserId = @UserId";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return (
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.IsDBNull(6) ? null : DateOnly.FromDateTime(reader.GetDateTime(6))
                );
            }
            return null;
        }

        // Thêm người dùng mới
        public bool AddUser(string username, string hash, string salt, string email, string fullName, DateOnly? birthday)
        {
            string sql = @"INSERT INTO NguoiDung (Username, PasswordHash, PasswordSalt, Email, FullName, Birthday) 
                        VALUES (@Username, @PasswordHash, @PasswordSalt, @Email, @FullName, @Birthday)";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 256).Value = hash;
            cmd.Parameters.Add("@PasswordSalt", SqlDbType.NVarChar, 256).Value = salt;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email;
            cmd.Parameters.Add("@FullName", SqlDbType.NVarChar, 100).Value = fullName;
            cmd.Parameters.Add("@Birthday", SqlDbType.Date).Value = (object?)birthday?.ToDateTime(TimeOnly.MinValue) ?? DBNull.Value;

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
