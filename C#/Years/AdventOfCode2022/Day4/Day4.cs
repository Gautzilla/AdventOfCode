using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day4
    {
        public static void Solve(int part)
        { 
            Console.WriteLine(
                File.ReadAllLines(@"Day4\input.txt")
                .Select(pair => pair.Split(",").Select(section => ParseSection(section)).ToArray())
                .Where(section => part == 1 ? FullyOverlaps(section) : OverlapsAtAll(section))
                .Count()
            );
        }

        private static bool FullyOverlaps((int l, int h)[] sections) => sections.Any(s => s.l == sections.Min(s => s.l) && s.h == sections.Max(s => s.h));
        private static bool OverlapsAtAll((int l, int h)[] sections) => sections
        .SelectMany(section => Enumerable.Range(section.l, section.h-section.l+1))
        .GroupBy(iD => iD)
        .Any(iD => iD.Count() > 1);
        private static (int l, int h) ParseSection(string section) => (int.Parse(section.Split("-").First()), int.Parse(section.Split("-").Last()));
    }
}