using QuanLyNguoiDung.Shared.Messages;
using QuanLyNguoiDung.Shared.Protocol;
using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNguoiDung.Client;

// Form tạm để khung chạy được. Khánh Duy thay bằng form Đăng nhập thật.
public class DangNhapForm : Form
{
    private readonly ServerConnection _conn = new();

    private TextBox txtIp;
    private TextBox txtCong;
    private TextBox txtUsername;
    private TextBox txtPassword;
    private Button btnDangNhap;
    private Button btnMoDangKy;
    private Label lblTrangThai;


    public DangNhapForm()
    {
        KhoiTaoGiaoDien();
        _conn.MatKetNoi += (_, msg) => CapNhatTrangThai("Mất kết nối: " + msg, Color.Red);
        VisibleChanged += (_, _) =>
        {
            if (Visible && !_conn.IsConnected) CapNhatTrangThai("Chưa kết nối", Color.Gray);
        };
        CapNhatTrangThai("Chưa kết nối", Color.Gray);
    }

    private void KhoiTaoGiaoDien()
    {
        this.Text = "Đăng Nhập Hệ Thống";
        this.ClientSize = new Size(400, 340);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        int labelX = 40, boxX = 140, width = 200, startY = 30, step = 40;

        this.Controls.Add(new Label { Text = "IP:", Location = new Point(labelX, startY + 3), AutoSize = true });
        txtIp = new TextBox { Text = "127.0.0.1", Location = new Point(boxX, startY), Width = width };
        this.Controls.Add(txtIp);

        this.Controls.Add(new Label { Text = "Cổng:", Location = new Point(labelX, startY + step + 3), AutoSize = true });
        txtCong = new TextBox { Text = "9000", Location = new Point(boxX, startY + step), Width = width };
        this.Controls.Add(txtCong);

        this.Controls.Add(new Label { Text = "Tài khoản:", Location = new Point(labelX, startY + step * 2 + 3), AutoSize = true });
        txtUsername = new TextBox { Location = new Point(boxX, startY + step * 2), Width = width };
        this.Controls.Add(txtUsername);

        this.Controls.Add(new Label { Text = "Mật khẩu:", Location = new Point(labelX, startY + step * 3 + 3), AutoSize = true });
        txtPassword = new TextBox { Location = new Point(boxX, startY + step * 3), Width = width, UseSystemPasswordChar = true };
        this.Controls.Add(txtPassword);

        this.Controls.Add(new Label { Text = "Trạng thái:", Location = new Point(labelX, startY + step * 4 + 3), AutoSize = true });
        lblTrangThai = new Label { Text = "Chưa kết nối", Location = new Point(boxX, startY + step * 4 + 3), AutoSize = true, ForeColor = Color.Gray };
        this.Controls.Add(lblTrangThai);

        btnDangNhap = new Button { Text = "Đăng nhập", Location = new Point(boxX, startY + step * 5), Width = 95 };
        btnDangNhap.Click += btnDangNhap_Click;
        this.Controls.Add(btnDangNhap);

        btnMoDangKy = new Button { Text = "Đăng ký", Location = new Point(boxX + 105, startY + step * 5), Width = 95 };
        btnMoDangKy.Click += btnMoDangKy_Click;
        this.Controls.Add(btnMoDangKy);
    }

    private void CapNhatTrangThai(string text, Color color)
    {
        if (InvokeRequired) { BeginInvoke(() => CapNhatTrangThai(text, color)); return; }
        lblTrangThai.Text = text;
        lblTrangThai.ForeColor = color;
    }

    private void KhoaNut(bool khoa)
    {
        btnDangNhap.Enabled = !khoa;
        btnMoDangKy.Enabled = !khoa;
        txtIp.Enabled = txtCong.Enabled = !khoa;
    }

    private async void btnDangNhap_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu.", "Thông báo");
            return;
        }
        if (!int.TryParse(txtCong.Text, out int cong) || cong < 1 || cong > 65535)
        {
            MessageBox.Show("Cổng không hợp lệ.", "Thông báo");
            return;
        }

        KhoaNut(true);
        try
        {
            if (!_conn.IsConnected)
            {
                CapNhatTrangThai("Đang kết nối...", Color.Orange);
                await _conn.ConnectAsync(txtIp.Text.Trim(), cong);
                CapNhatTrangThai("Đã kết nối", Color.Green);
            }

            var req = new Request
            {
                Lenh = Commands.DangNhap,
                Token = null,
                DuLieu = JsonSerializer.SerializeToElement(new DangNhapData
                {
                    Username = txtUsername.Text.Trim(),
                    Password = txtPassword.Text
                })
            };

            Response resp = await _conn.SendAsync(req);

            if (resp.MaLoi == ErrorCodes.Ok)
            {
                var result = resp.GetData<DangNhapResult>();
                if (result == null)
                {
                    MessageBox.Show("Dữ liệu từ máy chủ không hợp lệ.", "Lỗi");
                    return;
                }

                _conn.Token = result.Token; // Lưu token vào lớp mạng
                txtPassword.Clear();

                // Mở form chính của Cường, truyền kết nối và thông tin người dùng
                var mainForm = new MainForm(_conn, this, result,
                    result.HetHan?.ToLocalTime().ToString("HH:mm dd/MM/yyyy"));
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show(ErrorMessages.TuMa(resp.MaLoi, resp.ThongBao), "Đăng nhập thất bại");
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
        catch (Exception ex) when (ex is SocketException or IOException or InvalidOperationException)
        {
            CapNhatTrangThai("Lỗi kết nối", Color.Red);
            await _conn.CloseAsync(); // LỖI 5: ĐÓNG KẾT NỐI KHI CÓ LỖI
            MessageBox.Show("Không kết nối được tới server. Kiểm tra IP/cổng.", "Lỗi");
        }
        finally
        {
            KhoaNut(false);
        }
    }

    private void btnMoDangKy_Click(object sender, EventArgs e)
    {
        // Lấy IP/Cổng hiện tại
        if (!int.TryParse(txtCong.Text, out int cong)) cong = 9000;

        // Mở form Đăng Ký và truyền IP/Cổng sang
        var f = new DangKyForm(txtIp.Text.Trim(), cong);
        f.ShowDialog(this);
    }
    

    private void DangNhapForm_Load(object sender, EventArgs e)
    {

    }
}
