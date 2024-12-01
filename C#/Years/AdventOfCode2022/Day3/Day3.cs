using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day3
    {
        public static void Solve(int part)
        {
            if (part == 1)
            Console.WriteLine(
                File.ReadAllLines(@"Day3\input.txt")
                .Select(sack => sack.Take(sack.Length/2).Intersect(sack.Skip(sack.Length/2)).First())
                .Sum(item => ItemValue(item))
            );
            
            else
            Console.WriteLine(
                File.ReadAllLines(@"Day3\input.txt")
                .Chunk(3)
                .Select(group => group.First().FirstOrDefault(item => group.All(sack => sack.Contains(item))))
                .Sum(item => ItemValue(item))
            );
        }

        private static int ItemValue(char item) => char.IsLower(item) ? item - 'a' + 1 : item - 'A' + 27;
    }
}