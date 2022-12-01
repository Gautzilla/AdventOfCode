using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day1
    {
        public static void Solve(int part)
        {
            List<int> calories = File.ReadAllText(@"Day1\day1Input1.txt")
            .Split("\r\n\r\n") // Splits elves
            .Select(elf => elf.Split("\r\n").Sum(cal => int.Parse(cal))) // Sums each elf's cals
            .OrderByDescending(totCal => totCal)
            .ToList();
            
            Console.WriteLine(part == 1 ? calories.First() : calories.Take(3).Sum());
        }
    }
}