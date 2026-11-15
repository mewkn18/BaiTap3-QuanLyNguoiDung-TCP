using QuanLyNguoiDung.Shared.Messages;
using QuanLyNguoiDung.Shared.Protocol;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace QuanLyNguoiDung.Server;

public class ClientSession
{
    private readonly TcpClient _client;
    private readonly CommandDispatcher _dispatcher;
    private readonly ConcurrentDictionary<Guid, ClientSession> _danhSach;

    public Guid Id { get; } = Guid.NewGuid();
    public string DiaChi { get; }
    public int? UserId { get; set; }

    public ClientSession(TcpClient client, CommandDispatcher dispatcher,
                         ConcurrentDictionary<Guid, ClientSession> danhSach)
    {
        _client = client;
        _dispatcher = dispatcher;
        _danhSach = danhSach;
        DiaChi = client.Client.RemoteEndPoint?.ToString() ?? "không rõ";
        BatKeepAlive(client);
    }

    public async Task RunAsync(CancellationToken token)
    {
        _danhSach[Id] = this;
        Logger.Info($"Client kết nối: {DiaChi} (đang có {_danhSach.Count} client)");
        try
        {
            using (_client)
            using (token.Register(() => _client.Close()))
            {
                var stream = _client.GetStream();
                var utf8 = new UTF8Encoding(false);
                using var reader = new StreamReader(stream, utf8);
                using var writer = new StreamWriter(stream, utf8) { AutoFlush = true };

                while (!token.IsCancellationRequested)
                {
                    string? dong = await reader.ReadLineAsync();
                    if (dong == null)
                    {
                        Logger.Info($"Client {DiaChi} đã đóng kết nối");
                        break;
                    }

                    Response phanHoi;
                    Request? yeuCau = null;
                    try
                    {
                        yeuCau = JsonSerializer.Deserialize<Request>(dong);
                    }
                    catch (JsonException) { }

                    if (yeuCau == null)
                    {
                        phanHoi = new Response
                        {
                            MaLoi = ErrorCodes.DuLieuKhongHopLe,
                            ThongBao = "Dữ liệu gửi lên không hợp lệ."
                        };
                    }
                    else
                    {
                        Logger.Info($"{DiaChi} gửi lệnh {yeuCau.Lenh}");
                        phanHoi = await _dispatcher.DispatchAsync(yeuCau, this);
                    }

                    await writer.WriteLineAsync(JsonSerializer.Serialize(phanHoi));

                    if (yeuCau?.Lenh == Commands.DangXuat)
                    {
                        Logger.Info($"{DiaChi} đăng xuất");
                        break;
                    }
                }
            }
        }
        catch (OperationCanceledException) { }
        catch (ObjectDisposedException) { }
        catch (IOException ex) { Logger.Warn($"{DiaChi} mất kết nối: {ex.Message}"); }
        catch (SocketException ex) { Logger.Warn($"{DiaChi} lỗi socket: {ex.SocketErrorCode}"); }
        catch (Exception ex) { Logger.Error($"{DiaChi} lỗi không lường trước: {ex}"); }
        finally
        {
            UserId = null;
            _danhSach.TryRemove(Id, out _);
            Logger.Info($"Đã gỡ phiên {DiaChi} (còn {_danhSach.Count} client)");
        }
    }

    private static void BatKeepAlive(TcpClient client)
    {
        try
        {
            var s = client.Client;
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            s.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, 30);
            s.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, 5);
            s.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, 3);
        }
        catch (SocketException ex)
        {
            Logger.Warn($"Không bật được keep-alive: {ex.SocketErrorCode}");
        }
    }
}