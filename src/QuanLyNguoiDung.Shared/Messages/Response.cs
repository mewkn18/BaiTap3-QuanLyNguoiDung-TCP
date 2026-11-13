using System.Text.Json;
using System.Text.Json.Serialization;
using QuanLyNguoiDung.Shared.Protocol;

namespace QuanLyNguoiDung.Shared.Messages;

/// <summary>
/// Phản hồi server trả về (mục 3 của tài liệu giao thức).
/// Ví dụ: <c>{"maLoi":2,"thongBao":"Tên đăng nhập hoặc mật khẩu không đúng.","duLieu":null}</c>
/// </summary>
public sealed class Response
{
    /// <summary>0 là thành công, khác 0 là lỗi; lấy từ <see cref="ErrorCodes"/>.</summary>
    [JsonPropertyName("maLoi")]
    public int MaLoi { get; set; }

    /// <summary>Câu tiếng Việt thân thiện, không chứa tên ngoại lệ.</summary>
    [JsonPropertyName("thongBao")]
    public string ThongBao { get; set; } = "";

    /// <summary>Dữ liệu trả về; <c>null</c> khi không có.</summary>
    [JsonPropertyName("duLieu")]
    public JsonElement? DuLieu { get; set; }

    /// <summary><c>true</c> khi <see cref="MaLoi"/> là <see cref="ErrorCodes.Ok"/>.</summary>
    [JsonIgnore]
    public bool IsSuccess => MaLoi == ErrorCodes.Ok;

    /// <summary>
    /// Tạo phản hồi thành công. <paramref name="duLieu"/> là một lớp dữ liệu như
    /// <see cref="NguoiDungInfo"/>, hoặc <c>null</c>.
    /// </summary>
    public static Response Success(object? duLieu = null, string? thongBao = null) =>
        new()
        {
            MaLoi = ErrorCodes.Ok,
            ThongBao = thongBao ?? ErrorCodes.GetDefaultMessage(ErrorCodes.Ok),
            DuLieu = MessageData.ToElement(duLieu),
        };

    /// <summary>
    /// Tạo phản hồi lỗi. Bỏ trống <paramref name="thongBao"/> để dùng câu mặc định của mã lỗi,
    /// nhờ đó cùng một mã luôn cho cùng một thông báo.
    /// </summary>
    public static Response Error(int maLoi, string? thongBao = null) =>
        new()
        {
            MaLoi = maLoi,
            ThongBao = thongBao ?? ErrorCodes.GetDefaultMessage(maLoi),
        };

    /// <summary>
    /// Đọc <c>duLieu</c> thành kiểu <typeparamref name="T"/>; <c>null</c> nếu không có dữ liệu.
    /// Ném <see cref="JsonException"/> nếu dữ liệu sai cấu trúc.
    /// </summary>
    public T? GetData<T>() => MessageData.Read<T>(DuLieu);

    /// <summary>Chỉ in mã lỗi, không in dữ liệu. An toàn khi ghi log.</summary>
    public override string ToString() => $"Response(MaLoi={MaLoi} {ErrorCodes.GetName(MaLoi)})";
}
