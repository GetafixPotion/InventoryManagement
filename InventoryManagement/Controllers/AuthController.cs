using System;
using System.Data;
using InventoryManagement.Models;
using InventoryManagement.Data;
using System.Data.SqlClient;



namespace InventoryManagement.Controllers
{
    public class AuthController
    {
        public Member Login(string username, string password) 
        {
            var hash = Util.HashPassword(password);
            var sql = "SELECT MemberId, Username, PasswordHash, Role FROM Members WHERE Username = @u AND PasswordHash = @h";
            var dt = Db.ExecuteQuery(sql,
                new SqlParameter("@u", username),
                new SqlParameter("@h", hash));

            if (dt.Rows.Count == 0) return null;
            var r = dt.Rows[0];
            return new Member
            {
                MemberId = (int)r["MemberId"],
                Username = (string)r["Username"],
                PasswordHash = (string)r["PasswordHash"],
                Role = (string)r["Role"]
            };

        
        }
        public bool CreateUser(string username, string password, string role = "Member")
        {
            var exists = Db.ExecuteScalar("SELECT COUNT(1) FROM Members WHERE Username=@u", new SqlParameter("@u", username));
            if (Convert.ToInt32(exists) > 0) return false;
            var hash = Util.HashPassword(password);
            Db.ExecuteNonQuery("INSERT INTO Members (Username, Password, Role) VALUES (@u,@h,@r)",
                new SqlParameter("@u", username),
                new SqlParameter("@h", hash),
                new SqlParameter("@r", role));
            return true;
        }
    }
}
