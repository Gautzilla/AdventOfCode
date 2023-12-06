using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2023
{
    public static class Day05
    {
        private static List<(long start, long stop, bool hasEvolved)> _seeds = new();
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day05\input.txt");

            if (part == 1) ParseSeedsP1(input.First());   
            else ParseSeedsP2(input.First());

            foreach (var line in input.Skip(1))
            {
                if (line == string.Empty) continue;

                if (line.Contains("map"))
                {
                    _seeds = _seeds.Select(s => (s.start, s.stop, false)).ToList();
                    continue;
                }

                var mapItem = ParseMapItem(line);
                List<(long start, long stop, bool hasEvolved)> newSeeds = new(_seeds.Where(s => s.hasEvolved));

                foreach (var seed in _seeds.Where(s => !s.hasEvolved))
                {
                    if (!seed.Intercepts((mapItem.start, mapItem.stop)))
                    {
                         newSeeds.Add(seed);
                         continue;
                    }
                    newSeeds.AddRange(Intersection(seed, (mapItem.start, mapItem.stop), mapItem.offset));
                }

                _seeds = new(newSeeds);
            }

            Console.WriteLine(_seeds.Min(s => s.start));
        }

        private static void ParseSeedsP1 (string inputLine) => _seeds = inputLine
                .Split(' ')
                .Where(s => long.TryParse(s, out long i))
                .Select(long.Parse)
                .Select(i => (i,i, false))
                .ToList();
        
        private static void ParseSeedsP2 (string inputLine) => _seeds = inputLine
                .Split(' ')
                .Where(s => long.TryParse(s, out long temp))
                .Chunk(2)
                .Select(c => (long.Parse(c.First()), long.Parse(c.First()) + long.Parse(c.Last()) - 1, false))
                .ToList();

        private static (long start, long stop, long offset) ParseMapItem(string line)
        {
            var ints = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
            return (ints[1], ints[1] + ints[2], ints[0] - ints[1]);
        }

        private static bool Intercepts(this (long start, long stop, bool hasEvolved) source, (long start, long stop) mapItem) => !(source.stop < mapItem.start || source.start > mapItem.stop);

        private static List<(long start, long stop, bool hasEvolved)> Intersection((long start, long stop, bool hasEvolved) source, (long start, long stop) mapItem, long mapItemOffset)
        {
            List<(long start, long stop, bool hasEvolved)> output = new();

            if (source.start < mapItem.start) output.Add((source.start, mapItem.start-1, false));

            output.Add((Math.Max(source.start, mapItem.start) + mapItemOffset, Math.Min(source.stop, mapItem.stop) + mapItemOffset, true));

            if (source.stop > mapItem.stop) output.Add((mapItem.stop + 1, source.stop, false));

            return output;
        }
    }
}