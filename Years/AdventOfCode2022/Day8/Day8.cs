using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day8
    {
        private static int[][] _map;
        private static HashSet<(int x, int y)> _visibleTrees = new();
        public static void Solve(int part)
        { 
            _map = File.ReadAllLines(@"Day8\input.txt").Select(line => line.Select(c => (int)char.GetNumericValue(c)).ToArray()).ToArray();

            for (int y = 0; y < _map.Length; y++)
            {
                for (int x = 0; x < _map[0].Length; x++)
                {
                    if (IsVisible(x,y)) _visibleTrees.Add((x,y));
                }
            }

            for (int y = 0; y < _map.Length; y++)
            {
                for (int x = 0; x < _map[0].Length; x++)
                {
                    Console.ForegroundColor = _visibleTrees.Contains((x,y)) ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.Write(_map[y][x].ToString());
                }
                Console.WriteLine();
            }
            Console.ResetColor();
            Console.WriteLine(_visibleTrees.Count);
        }

        private static bool IsVisible(int x, int y)
        {
            if (x == 0 || x == _map[0].Length || y == 0 || y == _map.Length) return true;

            if (_map[y].Take(x).All(tree => tree < _map[y][x])) return true;
            if (_map[y].Skip(x+1).All(tree => tree < _map[y][x])) return true;

            if (_map.SelectMany(rows => rows.Where((v, xTree) => xTree == x)).Take(y).All(tree => tree < _map[y][x])) return true;
            if (_map.SelectMany(rows => rows.Where((v, xTree) => xTree == x)).Skip(y+1).All(tree => tree < _map[y][x])) return true;

            return false;
        }
    }
}