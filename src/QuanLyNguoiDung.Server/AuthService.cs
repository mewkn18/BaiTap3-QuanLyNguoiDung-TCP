using System.Net.Mail;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using QuanLyNguoiDung.Shared.Messages;
using QuanLyNguoiDung.Shared.Protocol;

namespace QuanLyNguoiDung.Server
{
    /// <summary>
    /// Xử lý DANGKY, DANGNHAP, LAYTHONGTIN trên CSDL thật.
    /// Mật khẩu băm bằng PasswordHasher (PBKDF2-SHA256, 600.000 vòng, salt 16 byte).
    /// </summary>
    public class AuthService : IAuthService
    {
        private const string ThongBaoSaiDangNhap = "Tên đăng nhập hoặc mật khẩu không chính xác.";

        private readonly NguoiDungRepository _repo;

        public AuthService(NguoiDungRepository repo)
        {
            _repo = repo;
        }

        // Repository dùng ADO.NET đồng bộ, nên chạy trên thread pool để không chặn task phục vụ client.
        public Task<Response> DangKyAsync(JsonElement? duLieu) => Task.Run(() => DangKy(duLieu));
        public Task<Response> DangNhapAsync(JsonElement? duLieu) => Task.Run(() => DangNhap(duLieu));
        public Task<Response> LayThongTinAsync(int userId) => Task.Run(() => LayThongTin(userId));

        // 1. ĐĂNG KÝ
        private Response DangKy(JsonElement? duLieu)
        {
            if (!TryGetData(duLieu, out DangKyData? data))
                return Loi(ErrorCodes.DuLieuKhongHopLe, "Dữ liệu đăng ký không hợp lệ.");

            // Server tự kiểm tra lại, không tin dữ liệu từ client
            if (string.IsNullOrWhiteSpace(data.Username) || string.IsNullOrWhiteSpace(data.Password) ||
                string.IsNullOrWhiteSpace(data.HoTen) || !EmailHopLe(data.Email))
                return Loi(ErrorCodes.DuLieuKhongHopLe, "Dữ liệu đăng ký không hợp lệ.");

            string username = data.Username.Trim();
            if (_repo.GetByUsername(username) != null)
                return Loi(ErrorCodes.TenDangNhapDaTonTai, "Tên đăng nhập đã tồn tại trong hệ thống.");

            // Sinh salt và băm mật khẩu tại server
            string salt = PasswordHasher.GenerateSalt();
            string hash = PasswordHasher.HashPassword(data.Password, salt);

            try
            {
                if (_repo.AddUser(username, hash, salt, data.Email.Trim(), data.HoTen.Trim(), data.NgaySinh))
                    return new Response { MaLoi = ErrorCodes.Ok, ThongBao = "Đăng ký thành công." };
            }
            catch (SqlException ex) when (ex.Number is 2627 or 2601)
            {
                // Hai client đăng ký cùng tên cùng lúc: ràng buộc UNIQUE chặn lại
                return Loi(ErrorCodes.TenDangNhapDaTonTai, "Tên đăng nhập đã tồn tại trong hệ thống.");
            }

            return Loi(ErrorCodes.LoiMayChu, "Lỗi hệ thống khi lưu cơ sở dữ liệu.");
        }

        // 2. ĐĂNG NHẬP: sai tên hay sai mật khẩu đều trả cùng mã 2 và cùng thông báo
        private Response DangNhap(JsonElement? duLieu)
        {
            if (!TryGetData(duLieu, out DangNhapData? data) ||
                string.IsNullOrWhiteSpace(data.Username) || string.IsNullOrWhiteSpace(data.Password))
                return Loi(ErrorCodes.SaiThongTinDangNhap, ThongBaoSaiDangNhap);

            var user = _repo.GetByUsername(data.Username.Trim());
            if (user == null || !PasswordHasher.VerifyPassword(data.Password, user.Value.Hash, user.Value.Salt))
                return Loi(ErrorCodes.SaiThongTinDangNhap, ThongBaoSaiDangNhap);

            // Không bao giờ trả PasswordHash / Salt
            var info = new DangNhapResult
            {
                UserId = user.Value.UserId,
                Username = user.Value.Username,
                Email = user.Value.Email,
                HoTen = user.Value.FullName,
                NgaySinh = user.Value.Birthday ?? default
            };
            return Response.Success(info, "Đăng nhập thành công.");
        }

        // 3. LẤY THÔNG TIN theo UserId của phiên đã đăng nhập
        private Response LayThongTin(int userId)
        {
            var user = _repo.GetById(userId);
            if (user == null)
                return Loi(ErrorCodes.ChuaDangNhap, "Không tìm thấy người dùng, vui lòng đăng nhập lại.");

            var info = new NguoiDungInfo
            {
                UserId = user.Value.UserId,
                Username = user.Value.Username,
                Email = user.Value.Email,
                HoTen = user.Value.FullName,
                NgaySinh = user.Value.Birthday ?? default
            };
            return Response.Success(info, "Lấy thông tin thành công.");
        }

        // Đọc dữ liệu lệnh; JSON thiếu trường bắt buộc hoặc sai kiểu thì trả false
        private static bool TryGetData<T>(JsonElement? duLieu, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out T? data)
            where T : class
        {
            data = null;
            if (duLieu is not { ValueKind: JsonValueKind.Object } el) return false;
            try
            {
                data = el.Deserialize<T>(ProtocolJson.Options);
                return data != null;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        private static bool EmailHopLe(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                var addr = new MailAddress(email.Trim());
                return addr.Address == email.Trim() && addr.Host.Contains('.');
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static Response Loi(int maLoi, string thongBao)
            => new Response { MaLoi = maLoi, ThongBao = thongBao, DuLieu = null };
    }
}
