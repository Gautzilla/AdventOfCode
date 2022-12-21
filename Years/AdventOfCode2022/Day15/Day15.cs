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

            if (part == 1) // Dirty brute force
            {
                int minX = _sensors.Min(s => s.Coord.x - s.HalfWidth);
                int maxX = _sensors.Max(s => s.Coord.x + s.HalfWidth);
                int y = 2000000;
                int emptySpots = 0;
                for (int x = minX; x < maxX; x++)
                {
                    if (_sensors.Any(s => s.Beacon == (x,y))) continue;
                    if (_sensors.Any(s => s.IsWithinRhombus((x,y)))) emptySpots++;
                }
            Console.WriteLine(emptySpots);
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

    }
}