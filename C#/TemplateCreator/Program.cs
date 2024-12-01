using System;
using System.Text.RegularExpressions;
using System.IO;

namespace TemplateCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Year:");
            string year = Console.ReadLine();
            Console.WriteLine("Day:");
            string day = Console.ReadLine();

            string dayTemplate = File.ReadAllText(@"Template.cs");
            dayTemplate = Regex.Replace(dayTemplate, "AdventOfCode", $"AdventOfCode{year}");
            dayTemplate = Regex.Replace(dayTemplate, "Day", $"Day{day}");

            string directoryPath = @$"..\\Years\\AdventOfCode{year}\\Day{day}";
            Directory.CreateDirectory(directoryPath);
            File.WriteAllText(directoryPath + @$"\\Day{day}.cs", dayTemplate);
            File.Create(directoryPath + @"\\input.txt");
        }
    }
}
