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
        class Rock
        {
            public (int x, int y) Coordinates { get; set; }
            public bool CanMove { get; set; }

            public Rock(int x, int y, char c)
            {
                Coordinates = (x,y);
                CanMove = c == 'O';
            }

            public void Move(IEnumerable<Rock> otherRocks)
            {
                if (!CanMove) return;

                var rocksAbove = otherRocks
                    .Where(r => r.Coordinates.x == Coordinates.x)
                    .Where(r => r.Coordinates.y < Coordinates.y);

                if (!rocksAbove.Any()) Coordinates = (Coordinates.x, 0);
                else 
                {
                    int newY = rocksAbove.Max(r => r.Coordinates.y) + 1;
                    Coordinates = (Coordinates.x, newY);
                }
            }
        }

        static List<Rock> _rocks = [];

        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day14\input.txt");

            ParseRocks(input);

            //DisplayRocks();

            foreach (var rock in _rocks.OrderBy(r => r.Coordinates.y).ThenBy(r => r.Coordinates.x))
            {
                rock.Move(_rocks);
            }

            //DisplayRocks();

            Console.WriteLine(_rocks.Where(r => r.CanMove).Sum(r => input.Length - r.Coordinates.y));
        }

        private static void ParseRocks (string[] input)
        {
            _rocks = input
                .SelectMany((row, y) => row
                    .Select((val, x) => (val,x,y)))
                    .Where(point => point.val != '.')
                    .Select(rock => new Rock(rock.x, rock.y, rock.val))
                .ToList();
        }

        private static void DisplayRocks()
        {
            Console.WriteLine(string.Join("\r\n", Enumerable.Range(0, _rocks.Max(r => r.Coordinates.y)+1)
                .Select(y => String.Join("",Enumerable.Range(0, _rocks.Max(r => r.Coordinates.x)+1)
                    .Select(x => _rocks.Any(r => r.Coordinates == (x,y)) ? (_rocks.Single(rock => rock.Coordinates == (x,y)).CanMove ? 'O' : '#') : '.')))));
            Console.WriteLine("\r\n\r\n");
        }
    }
}