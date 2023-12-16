using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2023
{
    public static class Day16
    {
        enum Direction
        {
            North,
            East,
            South,
            West
        }

        private static readonly (int x, int y)[] _directions = [(0,-1), (1,0), (0,1), (-1,0)];

        private static readonly Dictionary<char, Direction[][]> _encounterDirection = new()
        {
           {'\\', [[Direction.West], [Direction.South], [Direction.East], [Direction.North]]},           
           {'/', [[Direction.East], [Direction.North], [Direction.West], [Direction.South]]},           
           {'-', [[Direction.East , Direction.West], [Direction.East], [Direction.East , Direction.West], [Direction.West]]},           
           {'|', [[Direction.North], [Direction.North, Direction.South], [Direction.South], [Direction.North, Direction.South]]},           
           {'.', [[Direction.North], [Direction.East], [Direction.South], [Direction.West]]}, 
        };

        private static char[][] _encounters = [];
        private static (int x, int y) _layoutSize;
        private static HashSet<(int x, int y, int dir)> _visitedTiles = [];
        private static HashSet<(int x, int y)> _energizedTiles = [];
        private static Queue<((int x, int y) coordinates, Direction direction)> _beams = [];

        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day16\input.txt");

            _layoutSize = (input.First().Length, input.Length);

            _encounters = input
                .Select(line => line.ToArray())
                .ToArray();

            _beams.Enqueue(((0,0), Direction.East));

            //Console.WriteLine(String.Join("\r\n",Enumerable.Range(0,_layoutSize.y).Select(y => string.Join("", Enumerable.Range(0, _layoutSize.x).Select(x => _encounters[y][x])))));

            while (_beams.Count > 0) MoveBeam();

            Console.WriteLine(_energizedTiles.Count);

            //Console.WriteLine(String.Join("\r\n",Enumerable.Range(0,_layoutSize.y).Select(y => string.Join("", Enumerable.Range(0, _layoutSize.x).Select(x => _encounters[y][x])))));
            //Console.WriteLine(String.Join("\r\n",Enumerable.Range(0,_layoutSize.y).Select(y => string.Join("", Enumerable.Range(0, _layoutSize.x).Select(x => _energizedTiles.Contains((x,y)) ? '#' : '.')))));
        }

        private static void MoveBeam()
        {
            var (coordinates, direction) = _beams.Dequeue();

            _visitedTiles.Add((coordinates.x, coordinates.y, (int)direction));

            //Console.WriteLine($"({coordinates.x},{coordinates.y}), moving {direction})");
           // Console.ReadKey();

            if (!coordinates.IsInsideLayout()) return;

            _energizedTiles.Add(coordinates);

          //  Console.WriteLine(string.Join(" ", _energizedTiles.Select(coordinates => $"({coordinates.x},{coordinates.y})")));

            var encounter = _encounters[coordinates.y][coordinates.x];
            var nextDirections = _encounterDirection[encounter][(int)direction]; // Accounts for beam splitting
            
            foreach (var nextDirection in nextDirections)
            {
                (int x, int y) nextCoordinates = (coordinates.x + _directions[(int)nextDirection].x, coordinates.y + _directions[(int)nextDirection].y);
                if (!_visitedTiles.Contains((nextCoordinates.x, nextCoordinates.y, (int)nextDirection))) _beams.Enqueue((nextCoordinates, nextDirection));
            }
        }

        private static bool IsInsideLayout (this (int x, int y) coordinates) => coordinates.x >= 0 && coordinates.x < _layoutSize.x && coordinates.y >= 0 && coordinates.y < _layoutSize.y ;


    }
}