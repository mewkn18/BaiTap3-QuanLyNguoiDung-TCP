namespace QuanLyNguoiDung.Shared.Protocol;

/// <summary>
/// Mã lỗi của giao thức (bảng mã lỗi, mục 5 của tài liệu giao thức).
/// Chỉ dùng các hằng số này, không gõ tay số mã lỗi trong code.
/// </summary>
public static class ErrorCodes
{
    /// <summary>Thành công.</summary>
    public const int Ok = 0;

    /// <summary>Đăng ký thất bại vì tên đăng nhập đã có người dùng.</summary>
    public const int TenDangNhapDaTonTai = 1;

    /// <summary>
    /// Đăng nhập thất bại. Dùng chung cho cả sai tên đăng nhập và sai mật khẩu
    /// để người ngoài không dò được tên nào tồn tại.
    /// </summary>
    public const int SaiThongTinDangNhap = 2;

    /// <summary>Thiếu trường, sai định dạng hoặc JSON không đọc được.</summary>
    public const int DuLieuKhongHopLe = 3;

    /// <summary>Gọi lệnh cần đăng nhập mà yêu cầu không có token.</summary>
    public const int ChuaDangNhap = 4;

    /// <summary>Mã lệnh không nằm trong bảng mã lệnh.</summary>
    public const int LenhKhongHopLe = 5;

    /// <summary>Server gặp lỗi nội bộ; chi tiết chỉ ghi vào log của server.</summary>
    public const int LoiMayChu = 6;

    /// <summary>Token sai hoặc đã hết hạn; client quay về form Đăng nhập.</summary>
    public const int TokenHetHan = 7;

    /// <summary>Tên mã lỗi như trong tài liệu giao thức, dùng khi ghi log.</summary>
    public static string GetName(int maLoi) => maLoi switch
    {
        Ok => "OK",
        TenDangNhapDaTonTai => "TEN_DANG_NHAP_DA_TON_TAI",
        SaiThongTinDangNhap => "SAI_THONG_TIN_DANG_NHAP",
        DuLieuKhongHopLe => "DU_LIEU_KHONG_HOP_LE",
        ChuaDangNhap => "CHUA_DANG_NHAP",
        LenhKhongHopLe => "LENH_KHONG_HOP_LE",
        LoiMayChu => "LOI_MAY_CHU",
        TokenHetHan => "TOKEN_HET_HAN",
        _ => $"MA_LOI_{maLoi}",
    };

    /// <summary>
    /// Thông báo tiếng Việt mặc định cho một mã lỗi. Không chứa tên ngoại lệ.
    /// Server dùng chung để cùng một mã luôn cho cùng một thông báo.
    /// </summary>
    public static string GetDefaultMessage(int maLoi) => maLoi switch
    {
        Ok => "Thành công.",
        TenDangNhapDaTonTai => "Tên đăng nhập đã tồn tại.",
        SaiThongTinDangNhap => "Tên đăng nhập hoặc mật khẩu không đúng.",
        DuLieuKhongHopLe => "Dữ liệu không hợp lệ.",
        ChuaDangNhap => "Bạn chưa đăng nhập.",
        LenhKhongHopLe => "Lệnh không hợp lệ.",
        LoiMayChu => "Máy chủ gặp lỗi, vui lòng thử lại sau.",
        TokenHetHan => "Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại.",
        _ => "Đã xảy ra lỗi.",
    };
}
