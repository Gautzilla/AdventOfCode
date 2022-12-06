using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2015
{
    class Day3
    {
        static public void Solve(int part)
        {
            string filePath = @"..\..\..\Inputs\day3.txt";

            string input = File.ReadAllText(filePath);

            Console.WriteLine($"{CountHouses(input, part == 2)} houses got at least one present!");
        }

        static private int CountHouses(string input, bool robotSanta)
        {
            List<int[]> houses = new List<int[]>();

            int[] santaCoordinates = new int[] { 0, 0 };
            int[] robotSantaCoordinates = new int[] { 0, 0 };
            int[] coordinates = new int[] { 0, 0 };

            houses.Add(coordinates.ToArray());

            int turn = 0;

            foreach (char c in input)
            {
                if (robotSanta)
                {
                    if (turn++ % 2 != 0) coordinates = santaCoordinates;
                    else coordinates = robotSantaCoordinates;
                }

                switch (c)
                {
                    case '>':
                        coordinates[0]++;
                        break;
                    case '<':
                        coordinates[0]--;
                        break;
                    case '^':
                        coordinates[1]++;
                        break;
                    case 'v':
                        coordinates[1]--;
                        break;
                }
                if (!houses.Any(c => c[0] == coordinates[0] && c[1] == coordinates[1])) houses.Add(coordinates.ToArray());
            }
            return (houses.Count());
        }
    }
}
