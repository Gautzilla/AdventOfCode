using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2015
{
    public static class Day3
    {
        private static ((int x, int y) santa, (int x, int y) roboSanta) positions = ((0,0),(0,0));
        private static HashSet<(int, int)> _visitedHouses = new() {positions.santa};
        private static Dictionary<char, (int x, int y)> _directions = new Dictionary<char, (int x, int y)>()
        {
            {'^', (0, 1)},
            {'>', (1, 0)},
            {'v', (0, -1)},
            {'<', (-1, 0)},
        };
        public static void Solve(int part)
        { 
            string input = File.ReadAllText(@"Day3\input.txt");

            for (int c = 0; c < input.Length; c++)
                {
                    char direction = input[c];

                    bool moveRoboSanta = part == 2 && c % 2 == 1;

                    MoveSanta(direction, moveRoboSanta);
                    _visitedHouses.Add(moveRoboSanta ? positions.roboSanta : positions.santa);
                    
                }
                Console.WriteLine(_visitedHouses.Count);   
        }

        private static void MoveSanta(char direction, bool moveRoboSanta)
        {
            if (moveRoboSanta)
            {
                positions.roboSanta.x += _directions[direction].x;
                positions.roboSanta.y += _directions[direction].y;
            }
            else
            {
                positions.santa.x += _directions[direction].x;
                positions.santa.y += _directions[direction].y;
            }
            
        }
    }
}