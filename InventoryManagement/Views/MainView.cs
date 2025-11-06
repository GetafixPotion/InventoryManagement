
using InventoryManagement.Models;
using System;

namespace CityMakerspace.Views
{
    public static class MainView
    {
        public static void ShowMemberMenu(Member m)
        {
            Console.Clear();
            Console.WriteLine($"Welcome {m.Username} ({m.Role})\n");
            Console.WriteLine("1. View Tools");
            Console.WriteLine("2. My Loans");
            Console.WriteLine("3. Borrow Tool");
            Console.WriteLine("4. Return Tool");
            Console.WriteLine("5. Logout");
            Console.Write("Choose: ");
        }

        public static void ShowAdminMenu(Member m)
        {
            Console.Clear();
            Console.WriteLine($"Welcome {m.Username} ({m.Role})\n");
            Console.WriteLine("1. View Tools");
            Console.WriteLine("2. Manage Tools (Create/Update/Delete)");
            Console.WriteLine("3. View All Loans");
            Console.WriteLine("4. Logout");
            Console.Write("Choose: ");
        }
    }
}

