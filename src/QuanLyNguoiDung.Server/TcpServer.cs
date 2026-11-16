using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace QuanLyNguoiDung.Server;

public class TcpServer
{
    private readonly int _cong;
    private readonly CommandDispatcher _dispatcher;
    private readonly ConcurrentDictionary<Guid, ClientSession> _danhSach = new();

    public TcpServer(int cong, CommandDispatcher dispatcher)
    {
        _cong = cong;
        _dispatcher = dispatcher;
    }

    public async Task RunAsync(CancellationToken token)
    {
        var listener = new TcpListener(IPAddress.Any, _cong);
        try
        {
            listener.Start();
            Logger.Info($"Server đang lắng nghe ở cổng {_cong}");

            while (!token.IsCancellationRequested)
            {
                TcpClient client = await listener.AcceptTcpClientAsync(token);
                var phien = new ClientSession(client, _dispatcher, _danhSach);
                _ = phien.RunAsync(token);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            listener.Stop();
            Logger.Info("Server đã dừng.");
        }
    }
}