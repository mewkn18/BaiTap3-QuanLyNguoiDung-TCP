using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNguoiDung.Client
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblTieuDe;
        private Label lblTrangThai;
        private GroupBox grpThongTin;

        private Label lblUserIdTieuDe;
        private Label lblUserId;
        private Label lblUsernameTieuDe;
        private Label lblUsername;
        private Label lblHoTenTieuDe;
        private Label lblHoTen;
        private Label lblEmailTieuDe;
        private Label lblEmail;
        private Label lblNgaySinhTieuDe;
        private Label lblNgaySinh;
        private Label lblHetHanTokenTieuDe;
        private Label lblHetHanToken;

        private Button btnLamMoi;
        private Button btnDangXuat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblTieuDe = new Label();
            lblTrangThai = new Label();
            grpThongTin = new GroupBox();
            lblUserIdTieuDe = new Label();
            lblUserId = new Label();
            lblUsernameTieuDe = new Label();
            lblUsername = new Label();
            lblHoTenTieuDe = new Label();
            lblHoTen = new Label();
            lblEmailTieuDe = new Label();
            lblEmail = new Label();
            lblNgaySinhTieuDe = new Label();
            lblNgaySinh = new Label();
            lblHetHanTokenTieuDe = new Label();
            lblHetHanToken = new Label();
            btnLamMoi = new Button();
            btnDangXuat = new Button();
            pnlHeader.SuspendLayout();
            grpThongTin.SuspendLayout();
            SuspendLayout();

            // pnlHeader
            pnlHeader.BackColor = Color.FromArgb(30, 64, 175);
            pnlHeader.Controls.Add(lblTrangThai);
            pnlHeader.Controls.Add(lblTieuDe);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(484, 68);
            pnlHeader.TabIndex = 0;

            // lblTieuDe
            lblTieuDe.AutoSize = true;
            lblTieuDe.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.White;
            lblTieuDe.Location = new Point(20, 12);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(218, 25);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "THÔNG TIN TÀI KHOẢN";

            // lblTrangThai
            lblTrangThai.AutoSize = true;
            lblTrangThai.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblTrangThai.ForeColor = Color.FromArgb(187, 247, 208);
            lblTrangThai.Location = new Point(22, 41);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(123, 15);
            lblTrangThai.TabIndex = 1;
            lblTrangThai.Text = "Trạng thái: Đã kết nối";

            // grpThongTin
            grpThongTin.Controls.Add(lblHetHanToken);
            grpThongTin.Controls.Add(lblHetHanTokenTieuDe);
            grpThongTin.Controls.Add(lblNgaySinh);
            grpThongTin.Controls.Add(lblNgaySinhTieuDe);
            grpThongTin.Controls.Add(lblEmail);
            grpThongTin.Controls.Add(lblEmailTieuDe);
            grpThongTin.Controls.Add(lblHoTen);
            grpThongTin.Controls.Add(lblHoTenTieuDe);
            grpThongTin.Controls.Add(lblUsername);
            grpThongTin.Controls.Add(lblUsernameTieuDe);
            grpThongTin.Controls.Add(lblUserId);
            grpThongTin.Controls.Add(lblUserIdTieuDe);
            grpThongTin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpThongTin.Location = new Point(22, 84);
            grpThongTin.Name = "grpThongTin";
            grpThongTin.Size = new Size(440, 240);
            grpThongTin.TabIndex = 1;
            grpThongTin.TabStop = false;
            grpThongTin.Text = "Hồ sơ người dùng";

            // lblUserIdTieuDe
            lblUserIdTieuDe.AutoSize = true;
            lblUserIdTieuDe.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblUserIdTieuDe.ForeColor = Color.FromArgb(75, 85, 99);
            lblUserIdTieuDe.Location = new Point(18, 34);
            lblUserIdTieuDe.Name = "lblUserIdTieuDe";
            lblUserIdTieuDe.Size = new Size(102, 17);
            lblUserIdTieuDe.TabIndex = 0;
            lblUserIdTieuDe.Text = "Mã người dùng:";

            // lblUserId
            lblUserId.AutoSize = true;
            lblUserId.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblUserId.ForeColor = Color.FromArgb(17, 24, 39);
            lblUserId.Location = new Point(165, 34);
            lblUserId.Name = "lblUserId";
            lblUserId.Size = new Size(27, 17);
            lblUserId.TabIndex = 1;
            lblUserId.Text = "---";

            // lblUsernameTieuDe
            lblUsernameTieuDe.AutoSize = true;
            lblUsernameTieuDe.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblUsernameTieuDe.ForeColor = Color.FromArgb(75, 85, 99);
            lblUsernameTieuDe.Location = new Point(18, 68);
            lblUsernameTieuDe.Name = "lblUsernameTieuDe";
            lblUsernameTieuDe.Size = new Size(99, 17);
            lblUsernameTieuDe.TabIndex = 2;
            lblUsernameTieuDe.Text = "Tên đăng nhập:";

            // lblUsername
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblUsername.ForeColor = Color.FromArgb(17, 24, 39);
            lblUsername.Location = new Point(165, 68);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(27, 17);
            lblUsername.TabIndex = 3;
            lblUsername.Text = "---";

            // lblHoTenTieuDe
            lblHoTenTieuDe.AutoSize = true;
            lblHoTenTieuDe.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblHoTenTieuDe.ForeColor = Color.FromArgb(75, 85, 99);
            lblHoTenTieuDe.Location = new Point(18, 102);
            lblHoTenTieuDe.Name = "lblHoTenTieuDe";
            lblHoTenTieuDe.Size = new Size(67, 17);
            lblHoTenTieuDe.TabIndex = 4;
            lblHoTenTieuDe.Text = "Họ và tên:";

            // lblHoTen
            lblHoTen.AutoSize = true;
            lblHoTen.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblHoTen.ForeColor = Color.FromArgb(17, 24, 39);
            lblHoTen.Location = new Point(165, 102);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Size = new Size(27, 17);
            lblHoTen.TabIndex = 5;
            lblHoTen.Text = "---";

            // lblEmailTieuDe
            lblEmailTieuDe.AutoSize = true;
            lblEmailTieuDe.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblEmailTieuDe.ForeColor = Color.FromArgb(75, 85, 99);
            lblEmailTieuDe.Location = new Point(18, 136);
            lblEmailTieuDe.Name = "lblEmailTieuDe";
            lblEmailTieuDe.Size = new Size(42, 17);
            lblEmailTieuDe.TabIndex = 6;
            lblEmailTieuDe.Text = "Email:";

            // lblEmail
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(17, 24, 39);
            lblEmail.Location = new Point(165, 136);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(27, 17);
            lblEmail.TabIndex = 7;
            lblEmail.Text = "---";

            // lblNgaySinhTieuDe
            lblNgaySinhTieuDe.AutoSize = true;
            lblNgaySinhTieuDe.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblNgaySinhTieuDe.ForeColor = Color.FromArgb(75, 85, 99);
            lblNgaySinhTieuDe.Location = new Point(18, 170);
            lblNgaySinhTieuDe.Name = "lblNgaySinhTieuDe";
            lblNgaySinhTieuDe.Size = new Size(69, 17);
            lblNgaySinhTieuDe.TabIndex = 8;
            lblNgaySinhTieuDe.Text = "Ngày sinh:";

            // lblNgaySinh
            lblNgaySinh.AutoSize = true;
            lblNgaySinh.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblNgaySinh.ForeColor = Color.FromArgb(17, 24, 39);
            lblNgaySinh.Location = new Point(165, 170);
            lblNgaySinh.Name = "lblNgaySinh";
            lblNgaySinh.Size = new Size(27, 17);
            lblNgaySinh.TabIndex = 9;
            lblNgaySinh.Text = "---";

            // lblHetHanTokenTieuDe
            lblHetHanTokenTieuDe.AutoSize = true;
            lblHetHanTokenTieuDe.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblHetHanTokenTieuDe.ForeColor = Color.FromArgb(75, 85, 99);
            lblHetHanTokenTieuDe.Location = new Point(18, 204);
            lblHetHanTokenTieuDe.Name = "lblHetHanTokenTieuDe";
            lblHetHanTokenTieuDe.Size = new Size(94, 17);
            lblHetHanTokenTieuDe.TabIndex = 10;
            lblHetHanTokenTieuDe.Text = "Hết hạn token:";

            // lblHetHanToken
            lblHetHanToken.AutoSize = true;
            lblHetHanToken.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblHetHanToken.ForeColor = Color.FromArgb(17, 24, 39);
            lblHetHanToken.Location = new Point(165, 204);
            lblHetHanToken.Name = "lblHetHanToken";
            lblHetHanToken.Size = new Size(27, 17);
            lblHetHanToken.TabIndex = 11;
            lblHetHanToken.Text = "---";

            // btnLamMoi
            btnLamMoi.BackColor = Color.FromArgb(37, 99, 235);
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnLamMoi.ForeColor = Color.White;
            btnLamMoi.Location = new Point(22, 340);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(210, 38);
            btnLamMoi.TabIndex = 2;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += btnLamMoi_Click;

            // btnDangXuat
            btnDangXuat.BackColor = Color.FromArgb(220, 38, 38);
            btnDangXuat.FlatAppearance.BorderSize = 0;
            btnDangXuat.FlatStyle = FlatStyle.Flat;
            btnDangXuat.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnDangXuat.ForeColor = Color.White;
            btnDangXuat.Location = new Point(252, 340);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(210, 38);
            btnDangXuat.TabIndex = 3;
            btnDangXuat.Text = "Đăng xuất";
            btnDangXuat.UseVisualStyleBackColor = false;
            btnDangXuat.Click += btnDangXuat_Click;

            // MainForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(484, 398);
            Controls.Add(btnDangXuat);
            Controls.Add(btnLamMoi);
            Controls.Add(grpThongTin);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản Lý Người Dùng - Màn Hình Chính";
            FormClosing += MainForm_FormClosing;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            grpThongTin.ResumeLayout(false);
            grpThongTin.PerformLayout();
            ResumeLayout(false);
        }
    }
}