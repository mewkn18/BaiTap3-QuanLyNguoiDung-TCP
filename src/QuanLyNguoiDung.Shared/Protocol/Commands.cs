namespace QuanLyNguoiDung.Shared.Protocol;

/// <summary>
/// Mã lệnh của giao thức (bảng mã lệnh, mục 4 của tài liệu giao thức).
/// Chỉ dùng các hằng số này, không gõ tay chuỗi mã lệnh trong code.
/// </summary>
public static class Commands
{
    /// <summary>Đăng ký tài khoản. Không cần token.</summary>
    public const string DangKy = "DANGKY";

    /// <summary>Đăng nhập. Không cần token.</summary>
    public const string DangNhap = "DANGNHAP";

    /// <summary>Lấy thông tin người dùng đang đăng nhập. Cần token.</summary>
    public const string LayThongTin = "LAYTHONGTIN";

    /// <summary>Đăng xuất. Không cần token; sau phản hồi hai bên đóng kết nối.</summary>
    public const string DangXuat = "DANGXUAT";

    /// <summary>Mã lệnh có nằm trong bảng mã lệnh hay không. Phân biệt chữ hoa, chữ thường.</summary>
    public static bool IsKnown(string? lenh) =>
        lenh is DangKy or DangNhap or LayThongTin or DangXuat;
}
