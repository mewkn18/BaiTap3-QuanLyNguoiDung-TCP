using System.Text.Json.Serialization;

namespace QuanLyNguoiDung.Shared.Messages;

/// <summary>
/// Dữ liệu trong phản hồi DANGNHAP thành công: thông tin người dùng cộng thêm token.
/// Client có thể đọc bằng <c>GetData&lt;NguoiDungInfo&gt;()</c> nếu không cần token.
/// </summary>
public sealed class DangNhapResult : NguoiDungInfo
{
    /// <summary>Token đăng nhập; <c>null</c> nếu nhóm không làm phần token.</summary>
    [JsonPropertyName("token")]
    [JsonPropertyOrder(6)]
    public string? Token { get; set; }

    /// <summary>Thời điểm token hết hạn, giờ UTC (<c>DateTime.UtcNow</c>), ghi theo ISO 8601.</summary>
    [JsonPropertyName("hetHan")]
    [JsonPropertyOrder(7)]
    public DateTime? HetHan { get; set; }
}
