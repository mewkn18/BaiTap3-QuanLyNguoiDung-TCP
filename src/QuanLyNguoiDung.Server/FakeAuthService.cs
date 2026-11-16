using QuanLyNguoiDung.Shared.Messages;
using QuanLyNguoiDung.Shared.Protocol;
using System.Text.Json;

namespace QuanLyNguoiDung.Server;

// TẠM THỜI: chỉ để chạy thử, xoá khi thay AuthService của Đức
public class FakeAuthService : IAuthService
{
    public Task<Response> DangKyAsync(JsonElement? duLieu) => Task.FromResult(Ok("Đăng ký thành công (giả)."));
    public Task<Response> DangNhapAsync(JsonElement? duLieu) => Task.FromResult(Ok("Đăng nhập thành công (giả)."));
    public Task<Response> LayThongTinAsync(int userId) => Task.FromResult(Ok("Lấy thông tin thành công (giả)."));

    private static Response Ok(string thongBao)
        => new Response { MaLoi = ErrorCodes.Ok, ThongBao = thongBao, DuLieu = null };
}