using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2023
{
    public static class Day09
    {
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day09\input.txt");

            List<List<int>> inputs = ParseInput(input);

            if (part == 1) Console.WriteLine(Part1(inputs));
        }

        private static List<List<int>> ParseInput(string[] input) => input
            .Select(s => s
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList())
            .ToList();

        private static int Part1 (List<List<int>> inputs) => inputs
            .Select(GetLastNumbers)
            .Select(lastNumbers => lastNumbers.Aggregate((a,b) => a+b))
            .Sum();

        private static List<int> GetLastNumbers(List<int> input)
        {

            List<int> lastNumbers = [input.Last()];

            while (input.Any(i => i != 0))
            {                
                List<int> copy = new(input);
                input = copy.Skip(1).Select((val, index) => copy[index+1] - copy[index]).ToList();
                lastNumbers.Add(input.Last());
            }

            return lastNumbers;
        }
    }
}