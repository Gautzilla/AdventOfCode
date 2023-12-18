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
            North = 1,
            East = 2,
            South = 4,
            West = 8,
            Vertical = North | South,
            Horizontal = East | West
        }

        private record Block
        {           
            public (int x, int y) Coordinates { get; set; }

            public int HeatLoss { get; set; }

            public Dictionary<Direction, int> MinHeatLost { get; set; }

            public Block((int x, int y) coordinates, int heatLoss)
            {
                Coordinates = coordinates;
                HeatLoss = heatLoss;
                MinHeatLost = new(){
                    {Direction.North, int.MaxValue},
                    {Direction.East, int.MaxValue},
                    {Direction.South, int.MaxValue},
                    {Direction.West, int.MaxValue}
                };
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

            FindMinHeatLossPath(part);

            Console.WriteLine(_map[_map.GetLength(0)-1, _map.GetLength(1)-1].MinHeatLost.Values.Min());
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

        private static void FindMinHeatLossPath(int part)
        {
            _queue.Enqueue((_map[0,0], 0, Direction.Vertical));
            _queue.Enqueue((_map[0,0], 0, Direction.Horizontal));

            (int nbMinStepsForward, int nbMaxStepsForward) = part == 1 ? (1,3) : (4,10);

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

                    if (block.MinHeatLost[direction.Key] <= heatLoss) continue;

                    block.MinHeatLost[direction.Key] = heatLoss;        

                    for (int nbStepsForward = 1; nbStepsForward <= nbMaxStepsForward; nbStepsForward++)
                    {
                        (int x, int y) nextCoordinates = (block.Coordinates.x + direction.Value.x * nbStepsForward, block.Coordinates.y + direction.Value.y * nbStepsForward);
                        if (!nextCoordinates.IsInsideMap()) continue;

                        heatLoss += _map[nextCoordinates.x,nextCoordinates.y].HeatLoss;

                        if (nbStepsForward >= nbMinStepsForward) _queue.Enqueue((_map[nextCoordinates.x, nextCoordinates.y], heatLoss, ~directions));
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