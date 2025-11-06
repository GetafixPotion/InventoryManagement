using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace InventoryManagement.Data
{
    public static class Db
    {
        public static string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=InventoryManagement;Integrated Security=True;";
        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }
        public static object ExecuteScalar(string sql, params SqlParameter[] parms) { 
        
        using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            if (parms != null) cmd.Parameters.AddRange(parms);
            return cmd.ExecuteScalar();
        
        }
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parms)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            if (parms != null) cmd.Parameters.AddRange(parms);
            return cmd.ExecuteNonQuery();
        }

        public static DataTable ExecuteQuery(string sql, params SqlParameter[] parms)
        {
            using var conn = GetConnection();
            using var da = new SqlDataAdapter(sql, conn);
            if (parms != null)
            {
                foreach (var p in parms) da.SelectCommand.Parameters.Add(p);
            }
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}
