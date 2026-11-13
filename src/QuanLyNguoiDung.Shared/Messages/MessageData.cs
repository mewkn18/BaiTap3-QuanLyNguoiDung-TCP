using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using QuanLyNguoiDung.Shared.Protocol;

namespace QuanLyNguoiDung.Shared.Messages;

/// <summary>Đổi qua lại giữa đối tượng và trường <c>duLieu</c> của thông điệp.</summary>
internal static class MessageData
{
    public static JsonElement? ToElement(object? data) =>
        data is null
            ? null
            : JsonSerializer.SerializeToElement(data, data.GetType(), ProtocolJson.Options);

    public static T? Read<T>(JsonElement? element) =>
        element is { ValueKind: not JsonValueKind.Null and not JsonValueKind.Undefined } e
            ? e.Deserialize<T>(ProtocolJson.Options)
            : default;

    public static bool TryRead<T>(JsonElement? element, [NotNullWhen(true)] out T? data)
    {
        try
        {
            data = Read<T>(element);
        }
        catch (JsonException)
        {
            data = default;
            return false;
        }

        return data is not null;
    }
}
