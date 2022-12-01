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
            int max = 0;

            if (part == 1) max = String.Join("-", File.ReadAllLines("Day1\\day1Input1.txt"))
            .Split("--") // Split elves
            .Select(elve => elve.Split("-")) // Split foods
            .Select(cals => cals.Sum(cal => int.Parse(cal)))
            .Max();

            else max = String.Join("-", File.ReadAllLines("Day1\\day1Input1.txt"))
            .Split("--") // Split elves
            .Select(elve => elve.Split("-")) // Split foods
            .Select(cals => cals.Sum(cal => int.Parse(cal)))
            .OrderByDescending(totCal => totCal)
            .Take(3)
            .Sum();

            Console.WriteLine(max);
        }
    }
}