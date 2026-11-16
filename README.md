# Bài tập 3 – Ứng dụng quản lý người dùng Client–Server (TCP)

Học phần Lập trình mạng căn bản. Client WinForms và Server TCP trao đổi với nhau theo giao thức nhóm tự định nghĩa; chỉ Server được truy cập cơ sở dữ liệu.

## Cấu trúc

| Thư mục | Vai trò |
| --- | --- |
| `src/QuanLyNguoiDung.Shared` | Thư viện dùng chung: lớp thông điệp, mã lệnh, mã lỗi, hàm đóng khung. |
| `src/QuanLyNguoiDung.Server` | Server TCP (console): nhận nhiều client, truy vấn CSDL, băm mật khẩu. |
| `src/QuanLyNguoiDung.Client` | Client WinForms: form Đăng nhập, Đăng ký, form chính. |
| `docs/` | Tài liệu giao thức (luật chung của cả nhóm). |

## Yêu cầu

- .NET 8 SDK. Client cần Windows và Visual Studio 2022 (workload .NET desktop).
- Mở `QuanLyNguoiDung.sln`, đặt Server làm project khởi động trước, chạy Server rồi mới chạy Client.

## Quy tắc làm việc

- Code đúng từng chữ theo tài liệu giao thức trong `docs/`. Cần đổi thì báo cả nhóm để sửa tài liệu trước.
- Mã lệnh, mã lỗi và lớp thông điệp chỉ lấy từ project Shared.
- Mỗi người một nhánh riêng, không push thẳng vào `main`; làm xong tạo pull request.
- Đặt tên biến, tên file có nghĩa (`txtUsername`, `btnDangNhap`), không để `textBox1`, `button1`.
