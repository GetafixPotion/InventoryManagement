
using CityMakerspace.Views;
using InventoryManagement.Controllers;
using InventoryManagement.Models;
using InventoryManagement.Views;
using System;
using System.Linq;

class Program
{
    static AuthController auth = new AuthController();
    static ToolController toolC = new ToolController();
    static LoanController loanC = new LoanController();

    static void Main()
    {
        while (true)
        {
            AuthView.ShowWelcome();
            var (username, password) = AuthView.ShowLoginPrompt();
            var user = auth.Login(username, password);
            if (user == null)
            {
                Console.WriteLine("Invalid credentials. Try again. Press Enter.");
                Console.ReadLine();
                continue;
            }

            if (user.Role == "Admin") AdminLoop(user);
            else MemberLoop(user);
        }
    }

    static void MemberLoop(Member user)
    {
        while (true)
        {
            MainView.ShowMemberMenu(user);
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ToolView.ShowTools(toolC.GetAll());
                    break;
                case "2":
                    LoanView.ShowLoans(loanC.GetByMember(user.MemberId));
                    break;
                case "3":
                    var (toolId, cond) = LoanView.PromptBorrow();
                    var success = loanC.Borrow(user.MemberId, toolId, DateTime.Now, cond);
                    Console.WriteLine(success ? "Borrowed successfully." : "Failed to borrow (maybe already borrowed).");
                    Console.WriteLine("Press Enter...");
                    Console.ReadLine();
                    break;
                case "4":
                    var loanId = LoanView.PromptReturn();
                    var ok = loanC.Return(loanId, DateTime.Now);
                    Console.WriteLine(ok ? "Return recorded." : "Return failed (check loan id or date).");
                    Console.WriteLine("Press Enter...");
                    Console.ReadLine();
                    break;
                case "5": return;
                default:
                    Console.WriteLine("Invalid option. Press Enter...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void AdminLoop(Member user)
    {
        while (true)
        {
            MainView.ShowAdminMenu(user);
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ToolView.ShowTools(toolC.GetAll());
                    break;
                case "2":
                    ManageToolsMenu();
                    break;
                case "3":
                    LoanView.ShowLoans(loanC.GetAll());
                    break;
                case "4": return;
                default:
                    Console.WriteLine("Invalid option. Press Enter...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void ManageToolsMenu()
    {
        Console.Clear();
        Console.WriteLine("Tool Management");
        Console.WriteLine("1. Create");
        Console.WriteLine("2. Update");
        Console.WriteLine("3. Delete");
        Console.WriteLine("Any other key to return");
        var c = Console.ReadLine();
        if (c == "1")
        {
            var t = ToolView.PromptCreate();
            toolC.Create(t);
            Console.WriteLine("Created. Press Enter...");
            Console.ReadLine();
        }
        else if (c == "2")
        {
            Console.Write("Enter ToolId to edit: ");
            if (!int.TryParse(Console.ReadLine(), out var id)) return;
            var existing = toolC.GetById(id);
            if (existing == null) { Console.WriteLine("Not found. Enter..."); Console.ReadLine(); return; }
            var updated = ToolView.PromptEdit(existing);
            toolC.Update(updated);
            Console.WriteLine("Updated. Press Enter..."); Console.ReadLine();
        }
        else if (c == "3")
        {
            Console.Write("Enter ToolId to delete: ");
            if (!int.TryParse(Console.ReadLine(), out var id)) return;
            var ok = toolC.Delete(id);
            Console.WriteLine(ok ? "Deleted." : "Cannot delete (maybe borrowed).");
            Console.WriteLine("Press Enter..."); Console.ReadLine();
        }
    }
}