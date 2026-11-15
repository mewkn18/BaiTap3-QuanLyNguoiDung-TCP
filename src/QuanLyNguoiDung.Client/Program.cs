using System;
using System.Windows.Forms;

namespace QuanLyNguoiDung.Client
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            // Chạy form Đăng nhập đầu tiên
            Application.Run(new DangNhapForm());
        }
    }
} 