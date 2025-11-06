using System.Data;
using System.Data.SqlClient;
using InventoryManagement.Data;
using InventoryManagement.Models;
using System.Collections.Generic;

namespace InventoryManagement.Controllers
{
    public class ToolController
    {
        public IEnumerable<Tool> GetAll()
        {
            var dt = Db.ExecuteQuery("SELECT ToolId, Category, Condition, IsBorrowed FROM Tools ORDER BY Category, ToolId");
            foreach (DataRow r in dt.Rows)
            {
                yield return new Tool
                {
                    ToolId = (int)r["ToolId"],
                    Category = (string)r["Category"],
                    Condition = (string)r["Condition"],
                    IsBorrowed = (bool)r["IsBorrowed"]
                };
            }
        }

        public int Create(Tool t)
        {
            return Db.ExecuteNonQuery("INSERT INTO Tools (Category, Condition, IsBorrowed) VALUES (@c,@cond,@b)",
                new SqlParameter("@c", t.Category),
                new SqlParameter("@cond", t.Condition),
                new SqlParameter("@b", t.IsBorrowed));
        }

        public bool Update(Tool t)
        {
            return Db.ExecuteNonQuery("UPDATE Tools SET Category=@c, Condition=@cond WHERE ToolId=@id",
                new SqlParameter("@c", t.Category),
                new SqlParameter("@cond", t.Condition),
                new SqlParameter("@id", t.ToolId)) > 0;
        }

        public bool Delete(int toolId)
        {
            // Only delete if not borrowed
            var isBorrowed = Db.ExecuteScalar("SELECT IsBorrowed FROM Tools WHERE ToolId=@id", new SqlParameter("@id", toolId));
            if (isBorrowed == null) return false;
            if (Convert.ToBoolean(isBorrowed)) return false;
            return Db.ExecuteNonQuery("DELETE FROM Tools WHERE ToolId=@id", new SqlParameter("@id", toolId)) > 0;
        }

        public Tool GetById(int id)
        {
            var dt = Db.ExecuteQuery("SELECT ToolId, Category, Condition, IsBorrowed FROM Tools WHERE ToolId=@id",
                new SqlParameter("@id", id));
            if (dt.Rows.Count == 0) return null;
            var r = dt.Rows[0];
            return new Tool
            {
                ToolId = (int)r["ToolId"],
                Category = (string)r["Category"],
                Condition = (string)r["Condition"],
                IsBorrowed = (bool)r["IsBorrowed"]
            };
        }

        public bool SetBorrowed(int toolId, bool borrowed)
        {
            return Db.ExecuteNonQuery("UPDATE Tools SET IsBorrowed=@b WHERE ToolId=@id",
                new SqlParameter("@b", borrowed),
                new SqlParameter("@id", toolId)) > 0;
        }
    }
}
