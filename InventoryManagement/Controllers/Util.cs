using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace InventoryManagement.Controllers
{
    public static class Util
    {
        public static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            var sb = new StringBuilder();
            foreach(var b in hash) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}
