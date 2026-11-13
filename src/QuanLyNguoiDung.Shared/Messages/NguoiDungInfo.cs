using System.Text.Json.Serialization;

namespace QuanLyNguoiDung.Shared.Messages;

/// <summary>
/// Thông tin người dùng server trả về cho LAYTHONGTIN.
/// Không bao giờ có mật khẩu, chuỗi băm hay salt: lớp này không có chỗ để chứa chúng.
/// </summary>
public class NguoiDungInfo
{
    [JsonPropertyName("userId")]
    [JsonPropertyOrder(1)]
    public int UserId { get; set; }

    [JsonPropertyName("username")]
    [JsonPropertyOrder(2)]
    public string Username { get; set; } = "";

    [JsonPropertyName("email")]
    [JsonPropertyOrder(3)]
    public string Email { get; set; } = "";

    [JsonPropertyName("hoTen")]
    [JsonPropertyOrder(4)]
    public string HoTen { get; set; } = "";

    /// <summary>Ngày sinh, trong JSON là chuỗi <c>yyyy-MM-dd</c>.</summary>
    [JsonPropertyName("ngaySinh")]
    [JsonPropertyOrder(5)]
    public DateOnly NgaySinh { get; set; }
}
