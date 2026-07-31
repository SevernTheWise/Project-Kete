using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kete.Services.Models
{
    public class EmergencyItem
    {
        public string Name;
        public string Category;
        public double RecommendedQtyPerPerson;
        public string Unit;
        public double OwnedQuantity;
        public bool IsNonExpiring;
        public string ExpiryDate;
        public string Notes = "";

        public static string[] Categories =
        {
        "Water",
        "Food",
        "First Aid",
        "Light & Power",
        "Communication",
        "Warmth & Clothing",
        "Sanitation",
        "Documents & Cash",
        "Tools",
        "Pet Supplies"
    };

        public void itemConstructor()
        {
            string temp;
            Console.WriteLine("Enter the item name:\n");
            Name = Console.ReadLine() ?? "";
            while (Name == "")
            {
                Console.WriteLine("Enter the item name:\n");
                Name = Console.ReadLine() ?? "";
            }
            Console.Clear();
            Console.WriteLine("Catagories:\n");
            for (int i = 0; i < Categories.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Categories[i]}");
                Thread.Sleep(100);
            }
            Console.Write("Choose a category (1-10): ");

            int choice = int.Parse(Console.ReadLine() ?? "");
            while (choice >= 11 || choice <= 0)
            {
                Console.Write("Choose a category (1-10): ");
                choice = int.Parse(Console.ReadLine() ?? "");
            }
            Category = Categories[choice - 1];

        }
    }
}
