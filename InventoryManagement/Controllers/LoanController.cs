using InventoryManagement.Data;
using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Controllers
{
    public class LoanController
    {
        public IEnumerable<Loan> GetAll()
        {
            var dt = Db.ExecuteQuery("SELECT LoanId, MemberId, ToolId, BorrowDate, ReturnDate FROM Loans ORDER BY BorrowDate DESC");
            foreach (DataRow r in dt.Rows)
            {
                yield return rowToLoan(r);
            }
        }

        public IEnumerable<Loan> GetByMember(int memberId)
        {
            var dt = Db.ExecuteQuery("SELECT LoanId, MemberId, ToolId, BorrowDate, ReturnDate FROM Loans WHERE MemberId=@m ORDER BY BorrowDate DESC",
                new SqlParameter("@m", memberId));
            foreach (DataRow r in dt.Rows) yield return rowToLoan(r);
        }

        private Loan rowToLoan(DataRow r) => new Loan
        {
            LoanId = (int)r["LoanId"],
            MemberId = (int)r["MemberId"],
            ToolId = (int)r["ToolId"],
            BorrowDate = (DateTime)r["BorrowDate"],
            ReturnDate = r["ReturnDate"] == DBNull.Value ? (DateTime?)null : (DateTime)r["ReturnDate"]
        };

        public bool Borrow(int memberId, int toolId, DateTime borrowDate, string conditionAtCheckout)
        {
            // validate not already borrowed
            var isBorrowed = Db.ExecuteScalar("SELECT IsBorrowed FROM Tools WHERE ToolId=@id", new SqlParameter("@id", toolId));
            if (isBorrowed == null) return false;
            if (Convert.ToBoolean(isBorrowed)) return false;

            // insert loan
            Db.ExecuteNonQuery("INSERT INTO Loans (MemberId, ToolId, BorrowDate, ReturnDate) VALUES (@m,@t,@b,NULL)",
                new SqlParameter("@m", memberId),
                new SqlParameter("@t", toolId),
                new SqlParameter("@b", borrowDate));

            // update tool state and optionally record condition in Tools table -> update Condition
            Db.ExecuteNonQuery("UPDATE Tools SET IsBorrowed=1, Condition=@cond WHERE ToolId=@id",
                new SqlParameter("@cond", conditionAtCheckout),
                new SqlParameter("@id", toolId));
            return true;
        }

        public bool Return(int loanId, DateTime returnDate)
        {
            // get loan
            var dt = Db.ExecuteQuery("SELECT LoanId, ToolId, BorrowDate, ReturnDate FROM Loans WHERE LoanId=@id",
                new SqlParameter("@id", loanId));
            if (dt.Rows.Count == 0) return false;
            var r = dt.Rows[0];
            var borrowDate = (DateTime)r["BorrowDate"];
            if (returnDate < borrowDate) return false; // validation

            var toolId = (int)r["ToolId"];
            // set return date
            Db.ExecuteNonQuery("UPDATE Loans SET ReturnDate=@ret WHERE LoanId=@id",
                new SqlParameter("@ret", returnDate),
                new SqlParameter("@id", loanId));

            // mark tool as available
            Db.ExecuteNonQuery("UPDATE Tools SET IsBorrowed=0 WHERE ToolId=@tid", new SqlParameter("@tid", toolId));
            return true;
        }
    }
}
