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
        enum Category
        {
            Seed,
            Soil,
            Fertilizer,
            Water,
            Light,
            Temperature,
            Humidity,
            Location
        }

        private record MapItem
        {
            public Category SourceCategory { get; set; }
            public (long start, long stop) SourceRange { get; set; }
            public long Offset { get; set; }

            public MapItem(Category category, string mapLine)
            {
                SourceCategory = category;
                long[] values = mapLine
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse)
                    .ToArray();

                SourceRange = (values[1], values[1] + values[2] - 1);
                Offset = values[0] - values[1];
            }
        }
        private static List<(Category category, long value)> _seeds = new();
        private static HashSet<MapItem> _map = new();
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day05\input.txt");

            ParseSeeds(input.First());
            ParseMaps(input.Skip(3));   

            if (part == 1) Console.WriteLine(_seeds.Select(s => PlantSeed([], s).seed.value).Min()); 

        }

        private static void ParseSeeds (string inputLine) => _seeds = inputLine
                .Split(' ')
                .Where(s => long.TryParse(s, out long i))
                .Select(s => (Category.Seed,long.Parse(s)))
                .ToList();

        private static void ParseMaps(IEnumerable<string> input)
        {
            Category mapCategory = Category.Seed;
            foreach (var line in input)
            {
                if (line.Contains("map")) 
                {
                    mapCategory++;
                    continue;
                }
                if (line != string.Empty) _map.Add(new MapItem(mapCategory, line));
            }
        }

        private static (List<(Category category, long value)> history,(Category category, long value) seed) PlantSeed(List<(Category category, long value)> history, (Category category, long value) seed)
        {
            history.Add(seed);

            if (seed.category == Category.Location) return (history, seed);

            var map = _map
                .Where(m => m.SourceCategory == seed.category)
                .FirstOrDefault(m => m.SourceRange.start <= seed.value && m.SourceRange.stop >= seed.value);

            seed.category++;

            if(map != null) seed.value += map.Offset;
            
            return PlantSeed(history,(seed.category, seed.value));
        }
    }
}