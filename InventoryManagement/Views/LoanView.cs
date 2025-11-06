
using InventoryManagement.Models;
using System;
using System.Collections.Generic;

namespace CityMakerspace.Views
{
    public static class LoanView
    {
        public static void ShowLoans(IEnumerable<Loan> loans)
        {
            Console.Clear();
            Console.WriteLine("=== Loans ===\n");
            foreach (var l in loans)
            {
                var ret = l.ReturnDate.HasValue ? l.ReturnDate.Value.ToString("yyyy-MM-dd") : "Not returned";
                Console.WriteLine($"LoanId:{l.LoanId} MemberId:{l.MemberId} ToolId:{l.ToolId} Borrowed:{l.BorrowDate:yyyy-MM-dd} Return:{ret}");
            }
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        public static (int toolId, string condition) PromptBorrow()
        {
            Console.Write("Enter ToolId to borrow: ");
            var tid = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Record condition at checkout: ");
            var cond = Console.ReadLine() ?? "";
            return (tid, cond);
        }

        public static int PromptReturn()
        {
            Console.Write("Enter LoanId to return: ");
            return int.Parse(Console.ReadLine() ?? "0");
        }
    }
}
