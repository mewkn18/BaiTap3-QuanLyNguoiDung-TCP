using System.Text;
using System.Text.Json;
using QuanLyNguoiDung.Server;
using QuanLyNguoiDung.Shared.Protocol;

Console.OutputEncoding = Encoding.UTF8;

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

// Chuỗi kết nối đọc từ appsettings.json (nằm cạnh file .exe), không gõ cứng trong code
string chuoiKetNoi;
try
{
    string duongDan = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
    using var doc = JsonDocument.Parse(File.ReadAllText(duongDan));
    chuoiKetNoi = doc.RootElement.GetProperty("ConnectionStrings").GetProperty("QuanLyNguoiDung").GetString()
                  ?? throw new InvalidDataException();
}
catch (Exception ex) when (ex is IOException or JsonException or KeyNotFoundException or InvalidDataException)
{
    Logger.Error("Không đọc được chuỗi kết nối trong appsettings.json (ConnectionStrings:QuanLyNguoiDung).");
    return;
}

var repo = new NguoiDungRepository(chuoiKetNoi);
var dispatcher = new CommandDispatcher(new AuthService(repo));
var server = new TcpServer(ProtocolDefaults.DefaultPort, dispatcher);
await server.RunAsync(cts.Token);
