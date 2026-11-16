using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNguoiDung.Shared.Messages;
using QuanLyNguoiDung.Shared.Protocol;

namespace QuanLyNguoiDung.Client
{
    public partial class MainForm : Form
    {
        private readonly ServerConnection _serverConnection;
        private readonly Form _dangNhapForm;

        private bool _daXuLyDongKetNoi;
        private bool _dangChuyenVeDangNhap;

        public MainForm(
            ServerConnection serverConnection,
            Form dangNhapForm,
            NguoiDungInfo thongTinBanDau,
            string hetHanToken = null)
        {
            InitializeComponent();

            _serverConnection = serverConnection;
            _dangNhapForm = dangNhapForm;

            HienThiThongTinNguoiDung(thongTinBanDau, hetHanToken);
        }

        private void HienThiThongTinNguoiDung(NguoiDungInfo info, string hetHanToken = null)
        {
            if (info == null)
            {
                return;
            }

            lblUserId.Text = info.UserId.ToString();
            lblUsername.Text = info.Username;
            lblHoTen.Text = info.HoTen;
            lblEmail.Text = info.Email;
            lblNgaySinh.Text = info.NgaySinh.ToString("dd/MM/yyyy");

            if (!string.IsNullOrWhiteSpace(hetHanToken))
            {
                lblHetHanToken.Text = hetHanToken;
            }
        }

        private void KhoaGiaoDienKhiCho(bool dangCho, string trangThai)
        {
            btnLamMoi.Enabled = !dangCho;
            btnDangXuat.Enabled = !dangCho;
            lblTrangThai.Text = $"Trạng thái: {trangThai}";
        }

        private async void btnLamMoi_Click(object sender, EventArgs e)
        {
            await TaiLaiThongTinAsync();
        }

        private async Task TaiLaiThongTinAsync()
        {
            KhoaGiaoDienKhiCho(true, "Đang lấy thông tin từ máy chủ...");

            try
            {
                var request = new Request
                {
                    Lenh = Commands.LayThongTin,
                    Token = _serverConnection.Token,
                    DuLieu = null
                };

                Response response = await _serverConnection.SendAsync(request);

                if (response.MaLoi == ErrorCodes.Ok)
                {
                    var info = response.GetData<NguoiDungInfo>();
                    if (info != null)
                    {
                        HienThiThongTinNguoiDung(info);
                    }

                    KhoaGiaoDienKhiCho(false, "Đã cập nhật thông tin mới nhất.");
                    return;
                }

                if (response.MaLoi == ErrorCodes.TokenHetHan || response.MaLoi == ErrorCodes.ChuaDangNhap)
                {
                    MessageBox.Show(
                        ErrorMessages.TuMa(response.MaLoi),
                        "Phiên đăng nhập hết hạn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    await QuayVeDangNhapAsync(guiLenhDangXuat: false);
                    return;
                }

                MessageBox.Show(
                    ErrorMessages.TuMa(response.MaLoi),
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                KhoaGiaoDienKhiCho(false, "Đã kết nối");
            }
            catch (SocketException)
            {
                MessageBox.Show(
                    "Mất kết nối tới máy chủ. Vui lòng đăng nhập lại.",
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                await QuayVeDangNhapAsync(guiLenhDangXuat: false);
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "Kết nối mạng bị gián đoạn. Vui lòng đăng nhập lại.",
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                await QuayVeDangNhapAsync(guiLenhDangXuat: false);
            }
        }

        private async void btnDangXuat_Click(object sender, EventArgs e)
        {
            await QuayVeDangNhapAsync(guiLenhDangXuat: true);
        }

        private async Task QuayVeDangNhapAsync(bool guiLenhDangXuat)
        {
            if (_dangChuyenVeDangNhap)
            {
                return;
            }

            _dangChuyenVeDangNhap = true;
            KhoaGiaoDienKhiCho(true, "Đang đăng xuất...");

            await DongKetNoiAnToanAsync(guiLenhDangXuat);

            _daXuLyDongKetNoi = true;
            _dangNhapForm.Show();
            Close();
        }

        private async Task DongKetNoiAnToanAsync(bool guiLenhDangXuat)
        {
            if (_daXuLyDongKetNoi)
            {
                return;
            }

            _daXuLyDongKetNoi = true;

            try
            {
                if (guiLenhDangXuat && _serverConnection.IsConnected)
                {
                    var request = new Request
                    {
                        Lenh = Commands.DangXuat,
                        Token = null,
                        DuLieu = null
                    };

                    await _serverConnection.SendAsync(request);
                }
            }
            catch (SocketException)
            {
            }
            catch (IOException)
            {
            }
            finally
            {
                _serverConnection.Token = null;
                await _serverConnection.CloseAsync();
            }
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_daXuLyDongKetNoi && _serverConnection.IsConnected)
            {
                e.Cancel = true;
                KhoaGiaoDienKhiCho(true, "Đang đóng kết nối...");
                await DongKetNoiAnToanAsync(guiLenhDangXuat: true);

                if (!_dangChuyenVeDangNhap && !_dangNhapForm.IsDisposed)
                {
                    _dangNhapForm.Close();
                }

                Close();
            }
            else if (!_dangChuyenVeDangNhap && e.CloseReason == CloseReason.UserClosing && !_dangNhapForm.IsDisposed)
            {
                _dangNhapForm.Close();
            }
        }
    }
}