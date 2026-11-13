using System.Text.Encodings.Web;
using System.Text.Json;

namespace QuanLyNguoiDung.Shared.Protocol;

/// <summary>
/// Cấu hình JSON dùng chung cho mọi thông điệp.
/// Tên trường JSON đã được khai bằng <c>[JsonPropertyName]</c> trên từng lớp,
/// nên đúng tên trường theo tài liệu giao thức dù dùng cấu hình nào.
/// </summary>
public static class ProtocolJson
{
    /// <summary>
    /// Ghi một dòng, không thụt lề. Ký tự xuống dòng trong nội dung luôn được JSON
    /// viết thành hai ký tự <c>\n</c> nên một thông điệp không bao giờ chứa xuống dòng thật.
    /// Tiếng Việt được giữ nguyên, không đổi thành <c>\uXXXX</c>.
    /// </summary>
    public static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = false,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    /// <summary>Đổi đối tượng thành một dòng JSON.</summary>
    public static string Serialize<T>(T value) => JsonSerializer.Serialize(value, Options);

    /// <summary>Đọc một dòng JSON. Ném <see cref="JsonException"/> nếu JSON sai.</summary>
    public static T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, Options);
}
