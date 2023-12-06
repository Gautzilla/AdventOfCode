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
            (long time, long distance) = (long.Parse(string.Join("",input.First().Where(char.IsDigit))), long.Parse(string.Join("",input.Last().Where(char.IsDigit))));
            Console.WriteLine(NbWinPossibilities(time, distance));        
        }

        private static IEnumerable<int> ParseInts (this string s) => s.Split(' ',StringSplitOptions.RemoveEmptyEntries)
                .Where(s => int.TryParse(s, out int temp))
                .Select(int.Parse);
        private static long Distance(long pressTime, long totalTime) => (totalTime - pressTime) * pressTime;
        private static int NbWinPossibilities(long time, long distance) => Enumerable.Range(1, (int)time).Where(t => Distance(t, time) > distance).Count();
    }
}