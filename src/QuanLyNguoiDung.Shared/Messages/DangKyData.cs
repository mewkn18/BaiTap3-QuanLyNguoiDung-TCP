using System.Text.Json.Serialization;

namespace QuanLyNguoiDung.Shared.Messages;

/// <summary>
/// Tham số của lệnh DANGKY. Mật khẩu là chuỗi người dùng vừa nhập, client không tự băm.
/// Thiếu trường nào thì đọc dữ liệu sẽ lỗi; nội dung (email, độ dài...) server vẫn phải tự kiểm tra.
/// </summary>
public sealed class DangKyData
{
    [JsonPropertyName("username")]
    public required string Username { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("hoTen")]
    public required string HoTen { get; set; }

    /// <summary>Ngày sinh, trong JSON là chuỗi <c>yyyy-MM-dd</c>.</summary>
    [JsonPropertyName("ngaySinh")]
    public required DateOnly NgaySinh { get; set; }

    /// <summary>Không in mật khẩu. An toàn khi ghi log.</summary>
    public override string ToString() => $"DangKyData(Username={Username})";
}
