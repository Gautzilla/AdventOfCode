using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day15
    {
        private static HashSet<Sensor> _sensors = new();
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day15\input.txt");

            foreach (var line in input) ParseSensor(line);

            if (part == 1) 
            {
                int yLine = 2000000;

                HashSet<HashSet<int>> intersections = _sensors.Select(s => s.IntersectionsWithLine(yLine)).Where(i => i.Count > 0).ToHashSet();
                var spotsWithoutBeacons = ConcatLines(intersections);
                Console.WriteLine(spotsWithoutBeacons.Sum(spot => spot.x2 - spot.x1 + 1) - _sensors.Select(s => s.Beacon).Distinct().Count(b => b.y == yLine));
            } else // Must look for crossings between sides of all rhombuses or something
            {
                
            }
        }

        private static void ParseSensor(string line)
        {
            Match m = Regex.Match(line, @"Sensor at x=(?<xS>-?\d+), y=(?<yS>-?\d+): closest beacon is at x=(?<xB>-?\d+), y=(?<yB>-?\d+)");
            
            int xS = int.Parse(m.Groups["xS"].Value);
            int yS = int.Parse(m.Groups["yS"].Value);
            int xB = int.Parse(m.Groups["xB"].Value);
            int yB = int.Parse(m.Groups["yB"].Value);

            _sensors.Add(new Sensor((xS, yS), (xB, yB)));
        }

        private static HashSet<(int x1, int x2)> ConcatLines(HashSet<HashSet<int>> lines)
        {
            HashSet<(int x1, int x2)> output = new();
            var orderedLines = lines.OrderBy(l => l.Min(c => c)).ToList();

            int x1 = orderedLines.First().Min();
            int x2 = orderedLines.First().Max();

            for (int i = 0; i < lines.Count; i++)
            {
                if (orderedLines[i].Min() > x2)
                {
                    output.Add((x1, x2));
                    x1 = orderedLines[i].Min();
                    x2 = orderedLines[i].Max();
                }

                x1 = Math.Min(x1,orderedLines[i].Min());
                x2 = Math.Max(x2,orderedLines[i].Max());
            }
            output.Add((x1,x2));
            return output;
        }

    }
}