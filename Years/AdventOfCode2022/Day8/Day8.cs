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

        static Day8() => _map = File.ReadAllLines(@"Day8\input.txt").Select(line => line.Select(c => (int)char.GetNumericValue(c)).ToArray()).ToArray();
        public static void Solve(int part)
        { 
            int maxScenicScore = int.MinValue;

            for (int y = 0; y < _map.Length; y++)
            {
                for (int x = 0; x < _map[0].Length; x++)
                {
                    if (part == 1 && IsVisible(x,y)) _visibleTrees.Add((x,y));
                    else maxScenicScore = Math.Max(maxScenicScore, ScenicScore(x,y));
                }
            }

            Console.WriteLine(part == 1 ? _visibleTrees.Count : maxScenicScore);
        }

        private static bool IsVisible(int x, int y)
        {
            if (IsEdgeOfMap(x,y)) return true;

            var lines = GetLines(x ,y);
            int height = _map[y][x];

            if (IsVisibleInLine(lines.row, x, height)) return true;
            if (IsVisibleInLine(lines.column, y, height)) return true;
            
            return false;
        }

        private static bool IsVisibleInLine(List<int> line, int pos, int height)
        {
            if (line.Take(pos).All(tree => tree < height)) return true;
            if (line.Skip(pos+1).All(tree => tree < height)) return true;
            return false;
        }

        private static int ScenicScore(int x, int y)
        {
            if (IsEdgeOfMap(x,y)) return 0;
            
            var lines = GetLines(x, y);
            int height = _map[y][x];

            int scenicScore = ScenicScoreInLine(lines.row, x, height);
            scenicScore *= ScenicScoreInLine(lines.column, y, height);

            return scenicScore;
        }

        private static int ScenicScoreInLine(List<int> line, int pos, int height)
        {
            List<int> alignedTreesBefore = line.Take(pos).Reverse().ToList();
            List<int> alignedTreesAfter = line.Skip(pos+1).ToList();

            return ScenicScoreInDirection(alignedTreesBefore, height) * ScenicScoreInDirection(alignedTreesAfter, height);
        }

        private static int ScenicScoreInDirection(List<int> alignedTrees, int height)
        {
            List<int> visibleTrees = alignedTrees.TakeWhile(tree => tree < height).ToList();
            if (visibleTrees.Count < alignedTrees.Count()) visibleTrees.Add(alignedTrees.First(tree => tree >= height)); // First tree that blocks line of sight is visible
        
            return visibleTrees.Count();
        }

        private static (List<int> row, List<int> column) GetLines(int x, int y)
        {
            List<int> row = _map[y].ToList();
            List<int> column = _map.SelectMany(rows => rows.Where((v, xTree) => xTree == x)).ToList();

            return (row, column);
        }

        private static bool IsEdgeOfMap(int x, int y) => x == 0 || x == _map[0].Length || y == 0 || y == _map.Length;
    }
}