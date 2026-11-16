using QuanLyNguoiDung.Shared.Protocol;

namespace QuanLyNguoiDung.Client
{
    public static class ErrorMessages
    {
        public static string TuMa(int maLoi, string? thongBaoServer = null) => maLoi switch
        {
            ErrorCodes.Ok => "Thành công.",
            ErrorCodes.TenDangNhapDaTonTai => "Tên đăng nhập đã có người dùng.",
            ErrorCodes.SaiThongTinDangNhap => "Tên đăng nhập hoặc mật khẩu không đúng.",
            ErrorCodes.DuLieuKhongHopLe => "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.",
            ErrorCodes.ChuaDangNhap => "Bạn chưa đăng nhập.",
            ErrorCodes.LenhKhongHopLe => "Lệnh không hợp lệ.",
            ErrorCodes.LoiMayChu => "Máy chủ gặp sự cố. Vui lòng thử lại sau.",
            ErrorCodes.TokenHetHan => "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.",
            _ => thongBaoServer ?? "Đã xảy ra lỗi."
        };
    }
}