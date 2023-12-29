using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2023
{
    public static class Day21
    {
        private static bool[][] _rocks = [];
        private static readonly (int x, int y)[] _directions = [(-1,0),(0,-1),(1,0),(0,1)];
        private static HashSet<(int x, int y)> _reachableCoordinates = [];
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day21\input.txt");
            int nbSteps = 64;

            _rocks = ParseMap(input, nbSteps);

            for (int step = 0; step < nbSteps; step++)
            {
                HashSet<(int x, int y)> newReachable = [.._reachableCoordinates.SelectMany(Neighbours)];
                _reachableCoordinates = newReachable;
            }

            Console.WriteLine(_reachableCoordinates.Count);
            //Console.WriteLine(string.Join("\r\n", _rocks.Select(row => string.Join("", row.Select(p => p ? '#' : '.')))));
            
        }

        private static bool[][] ParseMap(string[] input, int nbSteps)
        {
            bool[][] output = new bool[input.Length][];

            for (int y = 0; y < input.Length; y++)
            {
                output[y] = input[y]
                    .Select(c => c == '#')
                    .ToArray();
                    
                if (input[y].Contains('S')) _reachableCoordinates.Add((input[y].IndexOf('S'), y));
            }

            return output;
        }

        private static List<(int x, int y)> Neighbours (this (int x, int y) coord) => _directions
            .Select(dir => (coord.x + dir.x, coord.y + dir.y))
            .Where(c => c.IsValid() && !c.IsRock())
            .ToList();

        private static bool IsValid(this (int x, int y) coord) =>
            coord.x >= 0 
            && coord.y >= 0
            && coord.y < _rocks.Length            
            && coord.x < _rocks[coord.y].Length;

        private static bool IsRock(this (int x, int y) coord) => _rocks[coord.y][coord.x];
    }
}