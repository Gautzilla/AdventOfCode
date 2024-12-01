using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day14
    {
        private static HashSet<(int x, int y)> _tiles = new();
        private static int _bottom;
        private static int _sandParticles = 0;
        private static (int x, int y) _firstTile;
        private static Stack<(int x, int y)> _previousTiles = new();
        private static Queue<(int x, int y)> _lookup = new();
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day14\input.txt");
            
            foreach (string line in input) ParseRocks(line);
            _bottom = _tiles.Max(t => t.y) + 2;

            _firstTile = (500, 0);
            _lookup.Enqueue(_firstTile);

            if (part == 1) Flow(_firstTile);
            else while (_lookup.Count > 0) BFS();
            
            Console.WriteLine(_sandParticles);
        }

        private static void ParseRocks(string path)
        {
            (int x, int y)[] coords = path
            .Split(" -> ")
            .Select(c => (int.Parse(c.Split(',').First()),int.Parse(c.Split(',').Last())))
            .ToArray();
            
            for (int node = 1; node < coords.Length; node++)
            {
                var node1 = coords[node];
                var node2 = coords[node - 1];

                if (node1.x != node2.x)
                {
                    int x1 = Math.Min(node1.x, node2.x);
                    int x2 = Math.Max(node1.x, node2.x);
                    for (int x = x1; x <= x2; x++) _tiles.Add((x, node1.y));
                }

                if (node1.y != node2.y)
                {
                    int y1 = Math.Min(node1.y, node2.y);
                    int y2 = Math.Max(node1.y, node2.y);
                    for (int y = y1; y <= y2; y++) _tiles.Add((node1.x, y));
                }
            }
        }

        private static readonly (int x, int y)[] _directions = { (0, 1), (-1, 1), (1, 1)};

        private static void Flow((int x, int y) coordinates)
        {
            if (!_previousTiles.Any(c => c == coordinates)) _previousTiles.Push(coordinates);

            if (!_tiles.Any(t => t.x == coordinates.x && t.y > coordinates.y)) return;
            if (_tiles.Contains(coordinates)) return;
            

            foreach (var d in _directions)
            {
                if (coordinates.y + d.y == _bottom || _tiles.Any(t => t == (coordinates.x + d.x, coordinates.y + d.y)))
                {
                    continue;
                }

                Flow((coordinates.x + d.x, coordinates.y + d.y));
                return;
            }           

            _tiles.Add(coordinates);

            _previousTiles.Pop();

            _sandParticles++;

            Flow(_previousTiles.Peek());
        }

        private static void BFS()
        {
            if (_lookup.Count == 0) return;
            
            var current = _lookup.Dequeue();
            _tiles.Add(current);
            _sandParticles++;

            foreach (var d in _directions)
            {
                (int x, int y) neighbour = (current.x + d.x, current.y + d.y);
                if (_tiles.Contains(neighbour) || _lookup.Contains(neighbour) || neighbour.y >= _bottom) continue;

                _lookup.Enqueue(neighbour);
            }
        }
    }
}