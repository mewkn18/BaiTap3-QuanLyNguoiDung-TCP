using QuanLyNguoiDung.Shared.Messages;
using System.Text.Json;

namespace QuanLyNguoiDung.Server;

public interface IAuthService
{
    Task<Response> DangKyAsync(JsonElement? duLieu);
    Task<Response> DangNhapAsync(JsonElement? duLieu);
    Task<Response> LayThongTinAsync(int userId);
}