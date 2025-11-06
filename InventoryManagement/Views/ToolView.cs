using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityMakerspace.Views
{
    public static class ToolView
    {
        public static void ShowTools(IEnumerable<Tool> tools)
        {
            Console.Clear();
            Console.WriteLine("=== Tools List (grouped by category) ===\n");
            var byCat = tools.GroupBy(t => t.Category);
            foreach (var grp in byCat)
            {
                Console.WriteLine($"Category: {grp.Key}");
                foreach (var t in grp)
                {
                    var avail = t.IsBorrowed ? "[BORROWED]" : "[AVAILABLE]";
                    Console.WriteLine($"  ID:{t.ToolId} | Condition: {t.Condition} {avail}");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        public static Tool PromptCreate()
        {
            Console.Write("Category: ");
            var cat = Console.ReadLine() ?? "";
            Console.Write("Condition (New/Good/Fair/Damaged): ");
            var cond = Console.ReadLine() ?? "";
            return new Tool { Category = cat, Condition = cond, IsBorrowed = false };
        }

        public static Tool PromptEdit(Tool existing)
        {
            Console.WriteLine($"Editing Tool ID {existing.ToolId}");
            Console.Write($"Category ({existing.Category}): ");
            var cat = Console.ReadLine(); if (string.IsNullOrWhiteSpace(cat)) cat = existing.Category;
            Console.Write($"Condition ({existing.Condition}): ");
            var cond = Console.ReadLine(); if (string.IsNullOrWhiteSpace(cond)) cond = existing.Condition;
            existing.Category = cat;
            existing.Condition = cond;
            return existing;
        }
    }
}

