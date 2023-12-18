using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Data;

namespace AdventOfCode2023
{
    public static class Day17
    {
        [Flags]
        enum Direction
        {
            North,
            East,
            South,
            West
        }

        private record Block
        {           
            public (int x, int y) Coordinates { get; set; }

            public int HeatLoss { get; set; }

            public int[] MinHeatLost { get; set; }

            public Block((int x, int y) coordinates, int heatLoss)
            {
                Coordinates = coordinates;
                HeatLoss = heatLoss;
                MinHeatLost = Enumerable.Repeat(int.MinValue, 4).ToArray();
            }
        }

        private static readonly Dictionary<Direction, (int x, int y)> _directions = new()
        {
            {Direction.North, (0, -1)},
            {Direction.East, (1, 0)},
            {Direction.South, (0, 1)},
            {Direction.West, (-1, 0)},
        };

        private static Block[,] _map = new Block[0,0];
        private static Queue<(Block block, int totalHeatLoss, Direction direction)> _queue = [];
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day17\input.txt");

            ParseMap(input);

            FindMinHeatLossPath();

            Console.WriteLine(_map[_map.GetLength(0)-1, _map.GetLength(1)-1].MinHeatLost.Min());
        }

        private static void ParseMap(string[] input)
        {
            _map = new Block[input.First().Length, input.Length];

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    _map[x,y] = new Block((x,y), (int)char.GetNumericValue(input[y][x]));
                }
            }
        }

        private static void FindMinHeatLossPath()
        {
            _queue.Enqueue((_map[0,0], 0, Direction.East | Direction.South));

            while (_queue.Count != 0)
            {
                var step = _queue.Dequeue();

                Block block = step.block;
                Direction directions = step.direction;

                if (!block.IsInsideMap()) continue;
                
                foreach (var direction in _directions)
                {
                    if (!directions.HasFlag(direction.Key)) continue;

                    int heatLoss = step.totalHeatLoss;                    

                    if (block.MinHeatLost[(int)direction.Key] <= heatLoss) continue;

                    block.MinHeatLost[(int)direction.Key] = heatLoss;        

                    for (int stepForward = 1; stepForward <= 3; stepForward++)
                    {
                        (int x, int y) nextCoordinates = (block.Coordinates.x + direction.Value.x * stepForward, block.Coordinates.y + direction.Value.y * stepForward);
                        if (!nextCoordinates.IsInsideMap()) continue;

                        heatLoss += _map[nextCoordinates.x,nextCoordinates.y].HeatLoss;

                        _queue.Enqueue((_map[nextCoordinates.x, nextCoordinates.y], heatLoss, ~directions));
                    }                    
                }
            }
        }

        private static bool IsInsideMap (this Block path) => path.Coordinates.IsInsideMap();

        private static bool IsInsideMap (this (int x, int y) coordinates) => 
        coordinates.x >= 0
        && coordinates.x < _map.GetLength(0) 
        && coordinates.y >= 0 
        && coordinates.y < _map.GetLength(1);
    }
}