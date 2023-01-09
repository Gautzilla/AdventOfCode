using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day17
    {
        private static readonly (int x, int y)[][] _rocks = { // rocks origin is at the bottom left corner
            new (int, int)[] {(0,0), (1,0), (2,0), (3,0)}, // _
            new (int, int)[] {(1,0), (0,-1), (1,-1), (2,-1), (1,-2)}, // +
            new (int, int)[] {(0,0), (1,0), (2,0), (2,-1), (2,-2)}, // J
            new (int, int)[] {(0,0), (0,-1), (0,-2), (0,-3)}, // |
            new (int, int)[] {(0,0), (0,-1), (1,0), (1,-1)}, // #
            };

        private static HashSet<(int x, int y)> _occupied = new();
        private static readonly string _jetPattern = File.ReadAllText(@"Day17\input.txt");
        private static int _jetState = 0; 
        public static void Solve(int part)
        { 
            (int x, int y) startingPos = (3, -4);
            
            for (long rock = 0; rock < 2022; rock++)
            {
                if (_occupied.Count > 0) startingPos.y = _occupied.Min(o => o.y) - 4;

                (int x, int y)[] fallingRock = _rocks[rock%_rocks.Length].Select(p => (p.x + startingPos.x, p.y + startingPos.y)).ToArray();

                Fall(fallingRock);
            }

            Console.WriteLine(Math.Abs(_occupied.Min(o => o.y)));
        }

        private static void Fall((int x, int y)[] rock)
        {
            // jet pushes the rock
            int jet = _jetPattern[_jetState] == '>' ? 1 : -1;
            _jetState = ++_jetState % _jetPattern.Length;
            
            if (rock.Min(p => p.x) + jet >= 1 && rock.Max(p => p.x) + jet <= 7 && !rock.Any(p => _occupied.Contains((p.x + jet, p.y)))) rock = rock.Select(p => (p.x + jet, p.y)).ToArray();

            // rock falls down
            if (rock.Any(p => p.y == -1) || rock.Any(p => _occupied.Contains((p.x, p.y+1))))
            {
                _occupied = _occupied.Concat(rock).ToHashSet();
                if(_jetState == 0) Console.WriteLine(_occupied.Min(p => p.y));
                //Display(rock);
                return;
            }

            rock = rock.Select(p => (p.x, p.y + 1)).ToArray();

            Fall(rock);
        }

        private static void Display((int x, int y)[] falling)
        {
            Console.WriteLine();
            for (int y = falling.Min(o => o.y); y <= 0; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (falling.Contains((x,y)))
                    {
                        Console.Write('@');
                        continue;
                    }

                    if (y == 0) Console.Write(x == 0 || x == 8 ? '+' : '_');
                    else if (x == 0 || x == 8) Console.Write('|');
                    else 
                    {
                        if (_occupied.Count == 0 || !_occupied.Contains((x,y))) Console.Write('.');
                        else Console.Write('#');
                    }
                }
                Console.WriteLine();
            }            
            Console.ReadKey();
        }
    }
}