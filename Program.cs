using ChipSecuritySystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipSecuritySystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Color startColor = GetColor(args, "-startColor", "Blue");
            Color endColor = GetColor(args, "-endColor", "Green");

            bool smallestList = args.Contains("-shortest");

            ChipService s = new ChipService();
            List<ColorChip> list = s.GetTestRandomChips(10);
            Console.WriteLine("This is the list of bi-color chips:");

            foreach(var item in list)
            {
               Console.WriteLine(item.ToString());
            }
            Console.WriteLine("");
            List<ColorChip> validCombinations = s.ValidCombinations(list, startColor, endColor, smallestList);
            if (validCombinations == null)
                Console.WriteLine(Constants.ErrorMessage);
            else
            {
                Console.WriteLine($"Valid combination found from {startColor} to {endColor}.  The {(smallestList ? "shortest" : "longest")} combination is:");
                foreach (var item in validCombinations)
                    Console.WriteLine(item.ToString());
            }
            Console.ReadLine();
        }

        private static Color GetColor(string[] args, string name, string defaultValue)
        {
            int index = Array.IndexOf(args, name);
            string argValue = defaultValue;
            if (index != -1 && index < args.Length - 1)
            {
                argValue = args[index + 1];
            }
            switch (argValue)
            {
                case "Red":
                    return Color.Red;
                case "Green":
                    return Color.Green;
                case "Blue":
                    return Color.Blue;
                case "Yellow":
                    return Color.Yellow;
                case "Orange":
                    return Color.Orange;
                case "Purple":
                    return Color.Purple;
                default:
                    throw new Exception("Unknown color: " + argValue);
            }
        }
    }
}
