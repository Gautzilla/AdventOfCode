using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2023
{
    public static class Day06
    {
        private static (int time, int distance)[] _races = [];
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day06\input.txt");

            _races = input
                .First()
                .ParseInts()
                .Zip(input
                    .Last()
                    .ParseInts())
                .ToArray();

            Console.WriteLine(_races
                .Aggregate(1, (a,b) => a * Enumerable.Range(1, b.time).Where(t => (b.time - t) * t > b.distance).Count()));
        }

        private static IEnumerable<int> ParseInts (this string s) => s.Split(' ',StringSplitOptions.RemoveEmptyEntries)
                .Where(s => int.TryParse(s, out int temp))
                .Select(int.Parse);
    }
}