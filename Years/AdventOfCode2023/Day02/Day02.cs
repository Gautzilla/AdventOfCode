using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections.Immutable;

namespace AdventOfCode2023
{
    class Game
    {
        public int ID { get; }
        public List<(int red, int green, int blue)>  Sets { get; } = new();
        public (int red, int green, int blue) Stock { get; } = (12, 13, 14);
        public bool IsPossible { get; set; } = true;

        public Game (string line)
        {
            ID = int.Parse(line.Split(':').First().Split(' ').Last());
            var sets = line.Split(':').Last().Split(';');

            foreach (string set in sets)
            {
                var cubes = set
                    .Split(',')
                    .Select(c => c.Trim())
                    .Select(c => c.Split(' '))
                    .Select(c => (int.Parse(c.First()), c.Last()));

                (int red, int green, int blue) count = (0,0,0);

                if (cubes.Any(c => c.Item2 == "red")) count.red = cubes.First(c => c.Item2 == "red").Item1;
                if (cubes.Any(c => c.Item2 == "blue")) count.blue = cubes.First(c => c.Item2 == "blue").Item1;
                if (cubes.Any(c => c.Item2 == "green")) count.green = cubes.First(c => c.Item2 == "green").Item1;

                Sets.Add(count);

            }

            IsPossible = Sets.All(s => s.red <= Stock.red && s.blue <= Stock.blue && s.green <= Stock.green);
        }

    }
    public static class Day02
    {
        private static List<Game> games = new();
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day02\input.txt");

            games = input.Select(s => new Game(s)).ToList();

            if (part == 1) Console.WriteLine(games.Where(g => g.IsPossible).Sum(g => g.ID));
        }
    }
}