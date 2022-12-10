using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day9
    {
        static (int x, int y) tail = (0,0);
        static (int x, int y) head = (0,0);
        static readonly Dictionary<string, (int x, int y)> _directions = new Dictionary<string, (int x, int y)>()
        {
            {"R", (1,0)},
            {"L", (-1,0)},
            {"U", (0,1)},
            {"D", (0,-1)},
        };
        static HashSet<(int x, int y)> visitedSquares = new();

        public static void Solve(int part)
        { 
            (string direction, int number)[] instructions = File.ReadAllLines(@"Day9\input.txt").Select(line => (line.Split(" ").First(), int.Parse(line.Split(" ").Last()))).ToArray();
            visitedSquares.Add((0,0));
            
            foreach (var instruction in instructions) ManageInstruction(instruction);
            
            Console.WriteLine(visitedSquares.Count());
        }

        private static void ManageInstruction((string direction, int number) instruction)
        {
            for (int inst = 0; inst < instruction.number; inst++)
            {
                MoveHead(instruction.direction);
                MoveTail();
                visitedSquares.Add(tail);
            }
        }

        private static void MoveHead(string direction)
        {
                head.x += _directions[direction].x;
                head.y += _directions[direction].y;
        }

        private static void MoveTail()
        {
            if (Math.Abs(head.x - tail.x) > 1)
            {
                tail.x += head.x > tail.x ? 1 : -1;
                tail.y += head.y != tail.y ? (head.y > tail.y ? 1 : -1) : 0;
            }
            if (Math.Abs(head.y - tail.y) > 1)
            {
                tail.y += head.y > tail.y ? 1 : -1;
                tail.x += head.x != tail.x ? (head.x > tail.x ? 1 : -1) : 0;
            }
        }


    }
}