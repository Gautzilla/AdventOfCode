using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Numerics;

namespace AdventOfCode2023
{
    public static class Day06
    {
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day06\input.txt");

            if (part == 1) SolvePart1(input);
            else SolvePart2(input);
        }

        private static void SolvePart1(string[] input)
        {
            (int time, int distance)[] races = input
                .First()
                .ParseInts()
                .Zip(input
                    .Last()
                    .ParseInts())
                .ToArray();

            Console.WriteLine(races
                .Aggregate(1, (a,b) => a * NbWinPossibilities(b.time, b.distance)));
        }

        private static void SolvePart2(string[] input)
        {
            (long time, long distance) = (input.First().ParseSplitLong(), input.Last().ParseSplitLong());
            Console.WriteLine(NbWinPossibilities(time, distance));        
        }

        private static IEnumerable<int> ParseInts (this string s) => s.Split(' ',StringSplitOptions.RemoveEmptyEntries)
                .Where(s => int.TryParse(s, out int temp))
                .Select(int.Parse);
        private static long ParseSplitLong(this string s) => long.Parse(string.Join("",s.Where(char.IsDigit)));
        private static int NbWinPossibilities(long time, long distance)
        {
            double delta = time * time - 4 * distance;
            var r1 = Math.Floor((time - Math.Sqrt(delta))/2) + 1; // Floor + 1 instead of Ceiling because we want (time - x) * x > d and not >= d. 
            var r2 = Math.Ceiling((time + Math.Sqrt(delta))/2) - 1; // This way the integer roots are not included.

            return (int)(r2-r1+1);
        }
    }
}