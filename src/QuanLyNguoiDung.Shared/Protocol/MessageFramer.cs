using System.Text;
using System.Text.Json;

namespace QuanLyNguoiDung.Shared.Protocol;

/// <summary>
/// Đóng khung thông điệp (mục 2 của tài liệu giao thức): mỗi thông điệp là một chuỗi JSON
/// nằm trên đúng một dòng, kết thúc bằng ký tự <c>\n</c>.
/// <para>
/// TCP chỉ chuyển một dòng byte liên tục, không có ranh giới thông điệp: hai thông điệp gửi sát
/// nhau có thể đến dính liền trong một lần đọc, và một thông điệp dài có thể bị cắt thành nhiều
/// lần đọc. Dấu <c>\n</c> cho bên nhận biết chỗ kết thúc.
/// </para>
/// <para>
/// Lớp này có hai tầng. Tầng khung (<see cref="WriteFrameAsync"/>, <see cref="ReadFrameAsync"/>)
/// chỉ lo tách thông điệp. Tầng JSON (<see cref="WriteAsync{T}"/>, <see cref="ReadAsync{T}"/>)
/// đổi qua lại giữa đối tượng và một khung.
/// </para>
/// <para>
/// Không gọi <c>WriteAsync</c> đồng thời từ hai tác vụ trên cùng một writer.
/// </para>
/// </summary>
public static class MessageFramer
{
    /// <summary>
    /// Tạo reader dùng UTF-8. Dùng hàm này ở cả client và server để hai phía cùng bảng mã.
    /// Đóng reader không đóng stream bên dưới.
    /// </summary>
    public static StreamReader CreateReader(Stream stream) =>
        new(stream, ProtocolDefaults.Utf8, detectEncodingFromByteOrderMarks: false,
            bufferSize: 4096, leaveOpen: true);

    /// <summary>
    /// Tạo writer dùng UTF-8 (không BOM), tự đẩy bộ đệm sau mỗi lần ghi.
    /// Đóng writer không đóng stream bên dưới.
    /// </summary>
    public static StreamWriter CreateWriter(Stream stream) =>
        new(stream, ProtocolDefaults.Utf8, bufferSize: 4096, leaveOpen: true)
        {
            AutoFlush = true,
            NewLine = "\n",
        };

    // ---- Tầng khung --------------------------------------------------------------------

    /// <summary>
    /// Ghi một khung: nội dung rồi ký tự <c>\n</c>, và đẩy bộ đệm đi ngay.
    /// </summary>
    /// <exception cref="ArgumentException">Nội dung chứa ký tự xuống dòng hoặc dài quá giới hạn.</exception>
    public static async Task WriteFrameAsync(
        TextWriter writer, string content, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(content);

        if (content.AsSpan().IndexOfAny('\n', '\r') >= 0)
        {
            throw new ArgumentException("Nội dung một khung không được chứa ký tự xuống dòng.", nameof(content));
        }

        if (content.Length > ProtocolDefaults.MaxMessageLength)
        {
            throw new ArgumentException(
                $"Thông điệp dài quá {ProtocolDefaults.MaxMessageLength} ký tự.", nameof(content));
        }

        // Ghép chung với ký tự kết thúc để đi ra trong một lần ghi.
        await writer.WriteAsync((content + ProtocolDefaults.FrameTerminator).AsMemory(), cancellationToken)
            .ConfigureAwait(false);
        await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Đọc một khung (một dòng). Trả về <c>null</c> khi bên kia đã đóng kết nối đàng hoàng.
    /// </summary>
    /// <exception cref="MessageTooLargeException">Dòng dài hơn <see cref="ProtocolDefaults.MaxMessageLength"/>.</exception>
    /// <exception cref="IOException">Kết nối đóng khi đang nhận dở một thông điệp.</exception>
    public static async Task<string?> ReadFrameAsync(
        TextReader reader, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(reader);

        // Đọc từng ký tự để chặn dòng quá dài ngay khi đang đọc, thay vì đọc hết vào bộ nhớ rồi mới kiểm tra.
        // StreamReader có bộ đệm bên trong nên đọc từng ký tự không gọi xuống mạng cho mỗi ký tự.
        var sb = new StringBuilder();
        var one = new char[1];

        while (true)
        {
            int read = await reader.ReadAsync(one.AsMemory(0, 1), cancellationToken).ConfigureAwait(false);

            if (read == 0)
            {
                if (sb.Length == 0)
                {
                    return null;
                }

                throw new IOException("Kết nối bị đóng khi đang nhận dở một thông điệp.");
            }

            char c = one[0];
            if (c == ProtocolDefaults.FrameTerminator)
            {
                break;
            }

            if (sb.Length >= ProtocolDefaults.MaxMessageLength)
            {
                throw new MessageTooLargeException();
            }

            sb.Append(c);
        }

        // Chấp nhận cả \r\n phòng khi bên kia ghi bằng WriteLine trên Windows.
        if (sb.Length > 0 && sb[^1] == '\r')
        {
            sb.Length--;
        }

        return sb.ToString();
    }

    // ---- Tầng JSON ---------------------------------------------------------------------

    /// <summary>Đổi đối tượng thành JSON một dòng rồi ghi thành một khung.</summary>
    public static Task WriteAsync<T>(TextWriter writer, T message, CancellationToken cancellationToken = default) =>
        WriteFrameAsync(writer, ProtocolJson.Serialize(message), cancellationToken);

    /// <summary>
    /// Đọc một khung rồi đổi thành đối tượng. Dòng trống bị bỏ qua.
    /// Trả về <c>null</c> khi bên kia đã đóng kết nối đàng hoàng.
    /// </summary>
    /// <exception cref="JsonException">
    /// Khung không phải JSON hợp lệ. Server nên trả mã
    /// <see cref="ErrorCodes.DuLieuKhongHopLe"/> rồi đọc tiếp, không cần đóng kết nối.
    /// </exception>
    /// <exception cref="MessageTooLargeException">Thông điệp quá dài.</exception>
    /// <exception cref="IOException">Kết nối đóng khi đang nhận dở một thông điệp.</exception>
    public static async Task<T?> ReadAsync<T>(TextReader reader, CancellationToken cancellationToken = default)
        where T : class
    {
        while (true)
        {
            string? frame = await ReadFrameAsync(reader, cancellationToken).ConfigureAwait(false);

            if (frame is null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(frame))
            {
                continue;
            }

            // JSON "null" cũng không hợp lệ: null được dành để báo kết nối đã đóng.
            return ProtocolJson.Deserialize<T>(frame)
                ?? throw new JsonException("Thông điệp rỗng.");
        }
    }
}
