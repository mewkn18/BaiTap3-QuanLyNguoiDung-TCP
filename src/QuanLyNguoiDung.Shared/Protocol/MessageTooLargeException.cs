namespace QuanLyNguoiDung.Shared.Protocol;

/// <summary>
/// Bên kia gửi một thông điệp dài hơn <see cref="ProtocolDefaults.MaxMessageLength"/>.
/// Kế thừa <see cref="IOException"/> nên khối <c>catch (IOException)</c> sẵn có
/// sẽ xử lý như một kết nối hỏng: ghi log rồi đóng kết nối.
/// </summary>
public sealed class MessageTooLargeException : IOException
{
    public MessageTooLargeException()
        : base($"Thông điệp dài quá {ProtocolDefaults.MaxMessageLength} ký tự.")
    {
    }
}
