using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNguoiDung.Shared.Messages;
using QuanLyNguoiDung.Shared.Protocol;

namespace QuanLyNguoiDung.Client
{
    public class DangKyForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtNhapLaiMatKhau;
        private TextBox txtEmail;
        private TextBox txtHoTen;
        private DateTimePicker dtpNgaySinh;
        private Button btnDangKy;
        private Label lblTrangThai;

        public DangKyForm()
        {
            KhoiTaoGiaoDien();
        }

        private void KhoiTaoGiaoDien()
        {
            this.Text = "Đăng Ký Tài Khoản";
            this.ClientSize = new Size(400, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            int labelX = 30, boxX = 140, width = 200, startY = 20, step = 40;

            this.Controls.Add(new Label { Text = "Tài khoản:", Location = new Point(labelX, startY + 3), AutoSize = true });
            txtUsername = new TextBox { Location = new Point(boxX, startY), Width = width };
            this.Controls.Add(txtUsername);

            this.Controls.Add(new Label { Text = "Mật khẩu:", Location = new Point(labelX, startY + step + 3), AutoSize = true });
            txtPassword = new TextBox { Location = new Point(boxX, startY + step), Width = width, UseSystemPasswordChar = true };
            this.Controls.Add(txtPassword);

            this.Controls.Add(new Label { Text = "Nhập lại MK:", Location = new Point(labelX, startY + step * 2 + 3), AutoSize = true });
            txtNhapLaiMatKhau = new TextBox { Location = new Point(boxX, startY + step * 2), Width = width, UseSystemPasswordChar = true };
            this.Controls.Add(txtNhapLaiMatKhau);

            this.Controls.Add(new Label { Text = "Email:", Location = new Point(labelX, startY + step * 3 + 3), AutoSize = true });
            txtEmail = new TextBox { Location = new Point(boxX, startY + step * 3), Width = width };
            this.Controls.Add(txtEmail);

            this.Controls.Add(new Label { Text = "Họ tên:", Location = new Point(labelX, startY + step * 4 + 3), AutoSize = true });
            txtHoTen = new TextBox { Location = new Point(boxX, startY + step * 4), Width = width };
            this.Controls.Add(txtHoTen);

            this.Controls.Add(new Label { Text = "Ngày sinh:", Location = new Point(labelX, startY + step * 5 + 3), AutoSize = true });
            dtpNgaySinh = new DateTimePicker { Location = new Point(boxX, startY + step * 5), Width = width, Format = DateTimePickerFormat.Short };
            this.Controls.Add(dtpNgaySinh);

            btnDangKy = new Button { Text = "Đăng ký", Location = new Point(boxX, startY + step * 6), Width = 100 };
            btnDangKy.Click += btnDangKy_Click;
            this.Controls.Add(btnDangKy);

            lblTrangThai = new Label { Text = "", Location = new Point(labelX, startY + step * 7), AutoSize = true, ForeColor = Color.Red };
            this.Controls.Add(lblTrangThai);
        }

        private void KhoaNut(bool khoa)
        {
            btnDangKy.Enabled = !khoa;
            txtUsername.Enabled = txtPassword.Enabled = txtNhapLaiMatKhau.Enabled = !khoa;
            txtEmail.Enabled = txtHoTen.Enabled = dtpNgaySinh.Enabled = !khoa;
        }

        private bool KiemTraHopLe()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtNhapLaiMatKhau.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các ô bắt buộc.", "Thiếu thông tin");
                return false;
            }

            if (txtPassword.Text != txtNhapLaiMatKhau.Text)
            {
                MessageBox.Show("Hai mật khẩu không trùng nhau.", "Lỗi");
                txtNhapLaiMatKhau.Clear();
                txtNhapLaiMatKhau.Focus();
                return false;
            }

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(txtEmail.Text.Trim(), emailPattern))
            {
                MessageBox.Show("Email không đúng định dạng.", "Lỗi");
                txtEmail.Focus();
                return false;
            }

            return true;
        }

        private async void btnDangKy_Click(object sender, EventArgs e)
        {
            if (!KiemTraHopLe()) return;

            KhoaNut(true);
            lblTrangThai.Text = "Đang xử lý...";

            try
            {
                
                string ip = "127.0.0.1";
                int cong = 9000;

                var conn = new ServerConnection();
                await conn.ConnectAsync(ip, cong);

                var req = new Request
                {
                    Lenh = Commands.DangKy, 
                    Token = null,
                    DuLieu = JsonSerializer.SerializeToElement(new DangKyData
                    {
                        Username = txtUsername.Text.Trim(),
                        Password = txtPassword.Text,
                        Email = txtEmail.Text.Trim(),
                        HoTen = txtHoTen.Text.Trim(),
                        NgaySinh = DateOnly.FromDateTime(dtpNgaySinh.Value)
                    })
                };

                Response resp = await conn.SendAsync(req);
                await conn.CloseAsync();

                if (resp.MaLoi == ErrorCodes.Ok) 
                {
                    MessageBox.Show("Đăng ký thành công! Bạn có thể đăng nhập.", "Thành công");
                    this.Close();
                }
                else
                {
                    MessageBox.Show(ErrorMessages.TuMa(resp.MaLoi, resp.ThongBao), "Đăng ký thất bại");
                }
            }
            catch (Exception ex) when (ex is SocketException or IOException or InvalidOperationException)
            {
                MessageBox.Show("Không kết nối được tới server.", "Lỗi kết nối");
            }
            finally
            {
                KhoaNut(false);
                lblTrangThai.Text = "";
            }
        }
    }
}