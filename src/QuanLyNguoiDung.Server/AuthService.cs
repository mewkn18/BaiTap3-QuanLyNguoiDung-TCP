using QuanLyNguoiDung.Shared.Messages;
using QuanLyNguoiDung.Shared.Protocol; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuanLyNguoiDung.Server
{
    public class AuthService
    {
        private readonly NguoiDungRepository _repo;

        public AuthService(NguoiDungRepository repo)
        {
            _repo = repo;
        }

        // 1. Xử lý ĐĂNG KÝ (Lệnh DANGKY)
        public Response DangKy(DangKyData data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.Username) || string.IsNullOrWhiteSpace(data.Password))
            {
                return new Response
                {
                    MaLoi = ErrorCodes.DuLieuKhongHopLe,
                    ThongBao = "Dữ liệu đăng ký không hợp lệ."
                };
            }

            // Kiểm tra trùng tên đăng nhập
            if (_repo.GetByUsername(data.Username) != null)
            {
                return new Response
                {
                    MaLoi = ErrorCodes.TenDangNhapDaTonTai, 
                    ThongBao = "Tên đăng nhập đã tồn tại trong hệ thống."
                };
            }

            // Sinh Salt và băm mật khẩu bằng PBKDF2 tại Server
            string salt = PasswordHasher.GenerateSalt();
            string hash = PasswordHasher.HashPassword(data.Password, salt);

            // Ghi vào CSDL
            bool isSuccess = _repo.AddUser(data.Username, hash, salt, data.Email, data.HoTen, data.NgaySinh);
            if (isSuccess)
            {
                return new Response
                {
                    MaLoi = ErrorCodes.Ok,
                    ThongBao = "Đăng ký thành công."
                };
            }

            return new Response
            {
                MaLoi = ErrorCodes.LoiMayChu,
                ThongBao = "Lỗi hệ thống khi lưu cơ sở dữ liệu."
            };
        }

        // 2. Xử lý ĐĂNG NHẬP (Lệnh DANGNHAP)
        public Response DangNhap(DangNhapData data)
        {
            // Trả về cùng một thông báo lỗi cho cả 2 trường hợp sai Username và sai Password để đảm bảo bảo mật
            string invalidMsg = "Tên đăng nhập hoặc mật khẩu không chính xác.";

            if (data == null || string.IsNullOrWhiteSpace(data.Username) || string.IsNullOrWhiteSpace(data.Password))
            {
                return new Response
                {
                    MaLoi = ErrorCodes.SaiThongTinDangNhap,
                    ThongBao = invalidMsg
                };
            }

            var user = _repo.GetByUsername(data.Username);

            // Không tìm thấy user trong DB
            if (user == null)
            {
                return new Response
                {
                    MaLoi = ErrorCodes.SaiThongTinDangNhap,
                    ThongBao = invalidMsg
                };
            }

            // Kiểm tra mật khẩu
            bool isPasswordValid = PasswordHasher.VerifyPassword(data.Password, user.Value.Hash, user.Value.Salt);
            if (!isPasswordValid)
            {
                return new Response
                {
                    MaLoi = ErrorCodes.SaiThongTinDangNhap,
                    ThongBao = invalidMsg
                };
            }

            // Đăng nhập thành công -> Trả về NguoiDungInfo (TUYỆT ĐỐI không kèm PasswordHash / Salt)
            var userInfo = new NguoiDungInfo
            {
                UserId = user.Value.UserId,
                Username = user.Value.Username,
                Email = user.Value.Email,
                HoTen = user.Value.FullName,
                NgaySinh = user.Value.Birthday ?? default
            };

            return new Response
            {
                MaLoi = ErrorCodes.Ok,
                ThongBao = "Đăng nhập thành công.",
                DuLieu = JsonSerializer.SerializeToElement(userInfo, ProtocolJson.Options)
            };
        }

        // 3. Xử lý LẤY THÔNG TIN (Lệnh LAYTHONGTIN)
        public Response LayThongTin(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return new Response
                {
                    MaLoi = ErrorCodes.DuLieuKhongHopLe,
                    ThongBao = "Tên đăng nhập không được để trống."
                };
            }

            var user = _repo.GetByUsername(username);
            if (user == null)
            {
                return new Response
                {
                    MaLoi = ErrorCodes.SaiThongTinDangNhap,
                    ThongBao = "Không tìm thấy người dùng."
                };
            }

            var userInfo = new NguoiDungInfo
            {
                UserId = user.Value.UserId,
                Username = user.Value.Username,
                Email = user.Value.Email,
                HoTen = user.Value.FullName,
                NgaySinh = user.Value.Birthday ?? default
            };

            return new Response
            {
                MaLoi = ErrorCodes.Ok,
                ThongBao = "Lấy thông tin thành công.",
                DuLieu = JsonSerializer.SerializeToElement(userInfo, ProtocolJson.Options)
            };
        }
    }
}

