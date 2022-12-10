using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace AdventOfCode2022
{
    public static class Day9
    {
        static private (int x, int y)[] _knots = new(int x, int y)[0];
        static private readonly Dictionary<string, (int x, int y)> _directions = new Dictionary<string, (int x, int y)>()
        {
            {"R", (1,0)},
            {"L", (-1,0)},
            {"U", (0,-1)},
            {"D", (0,1)},
        };
        static private HashSet<(int x, int y)> _visitedSquares = new();

        public static void Solve(int part)
        { 
            (string direction, int number)[] instructions = File.ReadAllLines(@"Day9\input.txt").Select(line => (line.Split(" ").First(), int.Parse(line.Split(" ").Last()))).ToArray();
            
            _knots = part == 1 ? new (int,int)[2] : new (int,int)[10];
            
            _visitedSquares.Add((0,0));
            
            foreach (var instruction in instructions) ManageInstruction(instruction);
            
            Console.WriteLine(_visitedSquares.Count());
        }

        private static void ManageInstruction((string direction, int number) instruction)
        {
            for (int inst = 0; inst < instruction.number; inst++)
            {
                _knots[0] = _knots[0].MoveHead(instruction.direction);
                
                for (int knot = 1; knot < _knots.Length; knot++ ) _knots[knot] = _knots[knot].Follow(_knots[knot-1]);
                
                _visitedSquares.Add(_knots.Last());
            }
        }

        private static (int x, int y) MoveHead(this (int x, int y) head, string direction) => (head.x + _directions[direction].x, head.y + _directions[direction].y);

        private static (int x, int y) Follow(this (int x, int y) knot, (int x, int y) previousKnot)
        {
            int x = knot.x;
            int y = knot.y;

            if (Math.Abs(previousKnot.x - knot.x) > 1)
            {
                x += previousKnot.x > knot.x ? 1 : -1;
                y += previousKnot.y != knot.y ? (previousKnot.y > knot.y ? 1 : -1) : 0;
                return (x,y);
            }
            if (Math.Abs(previousKnot.y - knot.y) > 1)
            {
                y += previousKnot.y > knot.y ? 1 : -1;
                x += previousKnot.x != knot.x ? (previousKnot.x > knot.x ? 1 : -1) : 0;
                return (x,y);
            }

            return (x,y);
        }
    }
}