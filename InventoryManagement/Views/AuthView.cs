using System;

namespace InventoryManagement.Views
{
    public static class AuthView
    {
        public static void ShowWelcome()
        {
            Console.Clear();
            Console.WriteLine("=== City Makerspace Lending System ===\n");
        }

        public static (string username, string password) ShowLoginPrompt()
        {
            Console.Write("Username: ");
            var u = Console.ReadLine()?.Trim();
            Console.Write("Password: ");
            var p = ReadPassword();
            return (u ?? "", p ?? "");
        }

        private static string ReadPassword()
        {
            var pwd = "";
            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && pwd.Length > 0)
                {
                    pwd = pwd[..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    pwd += key.KeyChar;
                    Console.Write("*");
                }
            }
            Console.WriteLine();
            return pwd;
        }
    }
}