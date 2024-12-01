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

            bool isHidden = false;

            //LEFT
            for (int i = x-1; i >= 0; i--) if (_map[y][i] >= _map[y][x]) {isHidden = true; break;}
            if (!isHidden) return true;
            isHidden = false;

            //RIGHT
            for (int i = x+1; i < _map[0].Length; i++) if (_map[y][i] >= _map[y][x]) {isHidden = true; break;}
            if (!isHidden) return true;
            isHidden = false;

            //UP
            for (int j = y-1; j >= 0; j--) if (_map[j][x] >= _map[y][x]) {isHidden = true; break;}
            if (!isHidden) return true;
            isHidden = false;

            //DOWN
            for (int j = y+1; j < _map.Length; j++) if (_map[j][x] >= _map[y][x]) {isHidden = true; break;}
            if (!isHidden) return true;
            
            return false;
        }

        private static int ScenicScore(int x, int y)
        {
            if (IsEdgeOfMap(x,y)) return 0;

            int[] scores = new int[4];
            
            //LEFT
            for (int i = x-1; i >= 0; i--) 
            {
                scores[0]++;
                if(_map[y][i] >= _map[y][x]) break;
            }

            if(scores[0] == 0) return 0;

            //RIGHT
            for (int i = x+1; i < _map[0].Length; i++) 
            {
                scores[1]++;
                if (_map[y][i] >= _map[y][x]) break;
            }

            if(scores[1] == 0) return 0;

            //UP
            for (int j = y-1; j >= 0; j--) 
            {
                scores[2]++;
                if (_map[j][x] >= _map[y][x]) break;
            }

            if(scores[2] == 0) return 0;

            //DOWN
            for (int j = y+1; j < _map.Length; j++) 
            {
                scores[3]++;
                if (_map[j][x] >= _map[y][x]) break;
            }

            return scores.Aggregate((a,b) => a*b);
        }

        private static bool IsEdgeOfMap(int x, int y) => x == 0 || x == _map[0].Length-1 || y == 0 || y == _map.Length-1;
    }
}