using System.Text;

namespace QuanLyNguoiDung.Shared.Protocol;

/// <summary>
/// Các hằng số chung của giao thức (mục 1 và mục 2 của tài liệu giao thức).
/// </summary>
public static class ProtocolDefaults
{
    /// <summary>Cổng TCP mặc định của server (nằm trong dải 1024–49151).</summary>
    public const int DefaultPort = 9000;

    /// <summary>
    /// Độ dài tối đa của một thông điệp, tính theo số ký tự của dòng JSON.
    /// Dòng dài hơn bị coi là vi phạm giao thức và kết nối sẽ bị đóng.
    /// </summary>
    public const int MaxMessageLength = 65536;

    /// <summary>Ký tự kết thúc một thông điệp.</summary>
    public const char FrameTerminator = '\n';

    /// <summary>Bảng mã dùng ở cả hai phía: UTF-8, không có BOM.</summary>
    public static readonly Encoding Utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
}
