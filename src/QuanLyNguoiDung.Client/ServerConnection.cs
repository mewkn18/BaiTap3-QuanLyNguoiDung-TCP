using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using QuanLyNguoiDung.Shared.Messages;
using QuanLyNguoiDung.Shared.Protocol;

namespace QuanLyNguoiDung.Client
{
    public class ServerConnection : IAsyncDisposable
    {
        private TcpClient? _client;
        private StreamReader? _reader;
        private StreamWriter? _writer;
        private readonly SemaphoreSlim _sendLock = new(1, 1);

        public bool IsConnected => _client?.Connected == true;

        // Lưu token sau khi đăng nhập thành công
        public string? Token { get; set; }

        // Sự kiện báo mất kết nối
        public event EventHandler<string>? MatKetNoi;

        public async Task ConnectAsync(string ip, int port, CancellationToken ct = default)
        {
            if (IsConnected) return;

            _client = new TcpClient();
            try
            {
                await _client.ConnectAsync(ip, port, ct);
            }
            catch (SocketException ex)
            {
                _client.Dispose();
                _client = null;
                throw new InvalidOperationException("Không kết nối được tới server.", ex);
            }

            var stream = _client.GetStream();
            _reader = MessageFramer.CreateReader(stream);
            _writer = MessageFramer.CreateWriter(stream);
        }

        public async Task<Response> SendAsync(Request request, CancellationToken ct = default)
        {
            if (!IsConnected || _writer == null || _reader == null)
                throw new InvalidOperationException("Chưa kết nối tới server.");

            // Tự động gắn Token nếu có (dùng chữ hoa Token)
            if (request.Token == null && Token != null)
                request.Token = Token;

            await _sendLock.WaitAsync(ct);
            try
            {
                // Sử dụng hàm WriteAsync của Quang
                await MessageFramer.WriteAsync(_writer, request, ct);

                // Sử dụng hàm ReadAsync của Quang
                var response = await MessageFramer.ReadAsync<Response>(_reader, ct);

                if (response == null)
                    throw new IOException("Server đã đóng kết nối.");

                // Xử lý mã lỗi 7 (dùng chữ hoa MaLoi và TokenHetHan)
                if (response.MaLoi == ErrorCodes.TokenHetHan)
                {
                    Token = null;
                }

                return response;
            }
            catch (IOException)
            {
                MatKetNoi?.Invoke(this, "Mất kết nối tới server.");
                throw;
            }
            catch (SocketException)
            {
                MatKetNoi?.Invoke(this, "Mất kết nối tới server.");
                throw;
            }
            finally
            {
                _sendLock.Release();
            }
        }

        public async Task CloseAsync()
        {
            try
            {
                if (_writer != null) await _writer.FlushAsync();
            }
            catch { }

            _writer?.Dispose();
            _reader?.Dispose();
            _client?.Close();
            _client = null;
            Token = null;
        }

        public async ValueTask DisposeAsync()
        {
            await CloseAsync();
        }
    }
}