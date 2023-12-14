using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2023
{
    public static class Day14
    {
        enum Direction
        {
            North,
            West,
            South,
            East
        }
        class Rock
        {
            public (int x, int y) Coordinates { get; set; }
            public bool CanMove { get; set; }

            public Rock(int x, int y, char c)
            {
                Coordinates = (x,y);
                CanMove = c == 'O';
            }

            public void Move(IEnumerable<Rock> otherRocks, Direction direction)
            {
                if (!CanMove) return;

                if (direction == Direction.North)
                {
                    var rocksNorth = otherRocks
                    .Where(r => r.Coordinates.x == Coordinates.x)
                    .Where(r => r.Coordinates.y < Coordinates.y);

                    if (!rocksNorth.Any()) Coordinates = (Coordinates.x, 0);
                    else 
                    {
                        int newY = rocksNorth.Max(r => r.Coordinates.y) + 1;
                        Coordinates = (Coordinates.x, newY);
                    }
                }

                if (direction == Direction.East)
                {
                    var rocksEast = otherRocks
                    .Where(r => r.Coordinates.y == Coordinates.y)
                    .Where(r => r.Coordinates.x > Coordinates.x);

                    if (!rocksEast.Any()) Coordinates = (_size.x - 1, Coordinates.y);
                    else 
                    {
                        int newX = rocksEast.Min(r => r.Coordinates.x) - 1;
                        Coordinates = (newX, Coordinates.y);
                    }
                }

                if (direction == Direction.South)
                {
                    var rocksSouth = otherRocks
                    .Where(r => r.Coordinates.x == Coordinates.x)
                    .Where(r => r.Coordinates.y > Coordinates.y);

                    if (!rocksSouth.Any()) Coordinates = (Coordinates.x, _size.y-1);
                    else 
                    {
                        int newY = rocksSouth.Min(r => r.Coordinates.y) - 1;
                        Coordinates = (Coordinates.x, newY);
                    }
                }

                if (direction == Direction.West)
                {
                    var rocksWest = otherRocks
                    .Where(r => r.Coordinates.y == Coordinates.y)
                    .Where(r => r.Coordinates.x < Coordinates.x);

                    if (!rocksWest.Any()) Coordinates = (0, Coordinates.y);
                    else 
                    {
                        int newX = rocksWest.Max(r => r.Coordinates.x) + 1;
                        Coordinates = (newX, Coordinates.y);
                    }
                }             
            }
        }

        static List<Rock> _rocks = [];
        static (int x, int y) _size = (0,0);

        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day14\input.txt");

            _size = (input.First().Length, input.Length);

            _rocks = ParseRocks(input);

            Console.WriteLine(part == 1 ? SolvePart1() : SolvePart2());
        }

        private static List<Rock> ParseRocks (string[] input) => input
                .SelectMany((row, y) => row
                    .Select((val, x) => (val,x,y)))
                    .Where(point => point.val != '.')
                    .Select(rock => new Rock(rock.x, rock.y, rock.val))
                .ToList();

        private static int SolvePart1()
        {
            _rocks = OrderRocks(Direction.North);
                
            foreach (var rock in _rocks)
            {
                rock.Move(_rocks, Direction.North);
            }

            return _rocks.Where(r => r.CanMove).Sum(r => _size.y - r.Coordinates.y);
        }

        private static List<string> _memory = [];

        private static int SolvePart2()
        {
            int indexLoopStart = -1;

            while (true)   
            {
                string state = ExportRocks(_rocks) ?? "";

                indexLoopStart = _memory.IndexOf(state);

                if (indexLoopStart != -1) break;
                
                Cycle();

                _memory.Add(state);
            }             

            int loopLength = _memory.Count - indexLoopStart;
            string targetState = _memory[(1000000000 - 1 - indexLoopStart)%loopLength + indexLoopStart];

            List<Rock> targetRocks = ParseRocks(targetState.Split("\r\n").ToArray());

            return targetRocks.Where(r => r.CanMove).Sum(r => _size.y - r.Coordinates.y);
        }

        private static void Cycle ()
        {
            for (int dirIndex = 0; dirIndex < 4; dirIndex++)
                {
                    Direction direction = (Direction)dirIndex;

                    _rocks = OrderRocks(direction);
                    
                    foreach (var rock in _rocks)
                    {
                        rock.Move(_rocks, direction);
                    }
                }
        }

        private static List<Rock> OrderRocks (Direction direction) => direction switch
        {
            Direction.North => [.. _rocks.OrderBy(r => r.Coordinates.y).ThenBy(r => r.Coordinates.x)],
            Direction.West => [.. _rocks.OrderBy(r => r.Coordinates.x).ThenBy(r => r.Coordinates.y)],
            Direction.South => [.. _rocks.OrderByDescending(r => r.Coordinates.y).ThenBy(r => r.Coordinates.x)],
            Direction.East => [.. _rocks.OrderByDescending(r => r.Coordinates.x).ThenBy(r => r.Coordinates.y)],
            _ => throw new ArgumentException("Unknown direction")
        };

        private static string ExportRocks (List<Rock> rocks) => string.Join("\r\n", 
            Enumerable.Range(0, rocks.Max(r => r.Coordinates.y)+1)
                .Select(y => String.Join("",
                    Enumerable.Range(0, rocks.Max(r => r.Coordinates.x)+1)
                        .Select(x => rocks.Any(r => r.Coordinates == (x,y)) ? (rocks.Single(rock => rock.Coordinates == (x,y)).CanMove ? 'O' : '#') : '.'))));
    }
}