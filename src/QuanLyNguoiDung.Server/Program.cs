using System.Text;
using QuanLyNguoiDung.Server;

Console.OutputEncoding = Encoding.UTF8;

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

var dispatcher = new CommandDispatcher(new FakeAuthService());
var server = new TcpServer(9000, dispatcher);
await server.RunAsync(cts.Token);