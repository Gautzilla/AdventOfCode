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

            List<int> row = _map[y].ToList();
            List<int> column = _map.SelectMany(rows => rows.Where((v, xTree) => xTree == x)).ToList();
            int height = _map[y][x];

            if (IsVisibleInRow(row, x, height)) return true;
            if (IsVisibleInRow(column, y, height)) return true;
            
            return false;
        }

        private static bool IsVisibleInRow(List<int> row, int pos, int height)
        {
            if (row.Take(pos).All(tree => tree < height)) return true;
            if (row.Skip(pos+1).All(tree => tree < height)) return true;
            return false;
        }
    }
}