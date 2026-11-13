using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuanLyNguoiDung.Shared.Messages;

/// <summary>
/// Yêu cầu client gửi lên server (mục 3 của tài liệu giao thức).
/// Ví dụ: <c>{"lenh":"DANGNHAP","token":null,"duLieu":{"username":"a","password":"b"}}</c>
/// </summary>
public sealed class Request
{
    /// <summary>Mã lệnh, lấy từ <see cref="Protocol.Commands"/>.</summary>
    [JsonPropertyName("lenh")]
    public string Lenh { get; set; } = "";

    /// <summary>Token đăng nhập; <c>null</c> với DANGKY và DANGNHAP.</summary>
    [JsonPropertyName("token")]
    public string? Token { get; set; }

    /// <summary>Tham số của lệnh; <c>null</c> khi lệnh không có tham số.</summary>
    [JsonPropertyName("duLieu")]
    public JsonElement? DuLieu { get; set; }

    /// <summary>
    /// Tạo yêu cầu. <paramref name="duLieu"/> là một lớp dữ liệu như
    /// <see cref="DangNhapData"/>, hoặc <c>null</c> nếu lệnh không có tham số.
    /// </summary>
    public static Request Create(string lenh, object? duLieu = null, string? token = null) =>
        new()
        {
            Lenh = lenh,
            Token = token,
            DuLieu = MessageData.ToElement(duLieu),
        };

    /// <summary>
    /// Đọc <c>duLieu</c> thành kiểu <typeparamref name="T"/>; <c>null</c> nếu không có dữ liệu.
    /// Ném <see cref="JsonException"/> nếu dữ liệu sai cấu trúc.
    /// </summary>
    public T? GetData<T>() => MessageData.Read<T>(DuLieu);

    /// <summary>
    /// Như <see cref="GetData{T}"/> nhưng không ném ngoại lệ: trả <c>false</c> khi thiếu dữ liệu
    /// hoặc sai cấu trúc. Server dùng để trả mã <see cref="Protocol.ErrorCodes.DuLieuKhongHopLe"/>.
    /// Hàm này chỉ kiểm tra cấu trúc; nội dung (email, độ dài...) server vẫn phải tự kiểm tra.
    /// </summary>
    public bool TryGetData<T>([NotNullWhen(true)] out T? data) => MessageData.TryRead(DuLieu, out data);

    /// <summary>Chỉ in mã lệnh, vì <c>duLieu</c> có thể chứa mật khẩu. An toàn khi ghi log.</summary>
    public override string ToString() => $"Request(Lenh={Lenh}, CoToken={Token is not null})";
}
