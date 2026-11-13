using System.Text.Json.Serialization;

namespace QuanLyNguoiDung.Shared.Messages;

/// <summary>
/// Tham số của lệnh DANGNHAP. Mật khẩu là chuỗi người dùng vừa nhập, client không tự băm.
/// </summary>
public sealed class DangNhapData
{
    [JsonPropertyName("username")]
    public required string Username { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }

    /// <summary>Không in mật khẩu. An toàn khi ghi log.</summary>
    public override string ToString() => $"DangNhapData(Username={Username})";
}
