using QuanLyNguoiDung.Shared.Messages;
using QuanLyNguoiDung.Shared.Protocol;
using System.Data.Common;

namespace QuanLyNguoiDung.Server;

public class CommandDispatcher
{
    private readonly IAuthService _auth;
    public CommandDispatcher(IAuthService auth) { _auth = auth; }

    public async Task<Response> DispatchAsync(Request yeuCau, ClientSession phien)
    {
        try
        {
            switch (yeuCau.Lenh)
            {
                case Commands.DangKy:
                    return await _auth.DangKyAsync(yeuCau.DuLieu);

                case Commands.DangNhap:
                    Response kq = await _auth.DangNhapAsync(yeuCau.DuLieu);
                    if (kq.MaLoi == ErrorCodes.Ok)
                        phien.UserId = kq.GetData<DangNhapResult>()?.UserId;
                    return kq;

                case Commands.LayThongTin:
                    // (Cường sẽ gắn kiểm tra token ở đây)
                    if (phien.UserId == null)
                        return Loi(ErrorCodes.ChuaDangNhap, "Bạn chưa đăng nhập.");
                    return await _auth.LayThongTinAsync(phien.UserId.Value);

                case Commands.DangXuat:
                    phien.UserId = null;
                    return new Response { MaLoi = ErrorCodes.Ok, ThongBao = "Đăng xuất thành công.", DuLieu = null };

                default:
                    return Loi(ErrorCodes.LenhKhongHopLe, "Lệnh không hợp lệ.");
            }
        }
        catch (DbException ex)
        {
            Logger.Error($"Lỗi cơ sở dữ liệu khi xử lý {yeuCau.Lenh}: {ex.Message}");
            return Loi(ErrorCodes.LoiMayChu, "Máy chủ đang gặp sự cố, vui lòng thử lại sau.");
        }
        catch (Exception ex)
        {
            Logger.Error($"Lỗi không lường trước khi xử lý {yeuCau.Lenh}: {ex.Message}");
            return Loi(ErrorCodes.LoiMayChu, "Máy chủ đang gặp sự cố, vui lòng thử lại sau.");
        }
    }

    private static Response Loi(int ma, string thongBao)
        => new Response { MaLoi = ma, ThongBao = thongBao, DuLieu = null };
}