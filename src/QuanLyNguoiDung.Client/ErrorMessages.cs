using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNguoiDung.Client
{
    internal class ErrorMessages
    {
        public static string TuMa(int maLoi, string? thongBaoServer = null) => maLoi switch
        {
            0 => "Thành công.", // ErrorCodes.OK
            1 => "Tên đăng nhập đã có người dùng.",
            2 => "Tên đăng nhập hoặc mật khẩu không đúng.",
            3 => "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.",
            4 => "Bạn chưa đăng nhập.",
            5 => "Lệnh không hợp lệ.",
            6 => "Máy chủ gặp sự cố. Vui lòng thử lại sau.",
            7 => "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.",
            _ => thongBaoServer ?? "Đã xảy ra lỗi."
        };
    }
}
