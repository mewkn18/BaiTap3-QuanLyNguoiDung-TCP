using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNguoiDung.Server
{
    public static class Logger
    {
        private static readonly object _lock = new();

        public static void Info(string noiDung) => Ghi("INFO", noiDung);
        public static void Warn(string noiDung) => Ghi("WARN", noiDung);
        public static void Error(string noiDung) => Ghi("ERROR", noiDung);

        private static void Ghi(string mucDo, string noiDung)
        {
            string dong = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{mucDo}] {noiDung}";
            lock (_lock)
            {
                Console.WriteLine(dong);
            }
        }
    }
}
