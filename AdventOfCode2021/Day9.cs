using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day9
    {
        static bool[,] markedMap;
        static int[,] map;

        public static void Solve(int part)
        {
            //string path = @"D:\Documents Gauthier\Programmation\AdventOfCode2021\Day9\exampleInput.txt";
            string path = @"D:\Documents Gauthier\Programmation\AdventOfCode2021\Day9\input.txt";

            List<string> input = File.ReadAllLines(path).ToArray().ToList();

            map = new int[input[0].Length,input.Count()];

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    map[x, y] = int.Parse(input[y][x].ToString());
                }
            }

            if (part == 1) // PART 1 : Risk
            {
                int risk = 0;

                for (int y = 0; y < map.GetLength(1); y++)
                {
                    for (int x = 0; x < map.GetLength(0); x++)
                    {
                        bool risky = true;
                        int point = map[x, y];

                        if ((x > 0) && risky) risky = map[x - 1, y] > point;
                        if ((y > 0) && risky) risky = map[x, y - 1] > point;
                        if ((x < map.GetLength(0) - 1) && risky) risky = map[x + 1, y] > point;
                        if ((y < map.GetLength(1) - 1) && risky) risky = map[x, y + 1] > point;

                        if (risky) risk += point + 1;
                    }
                }

                Console.WriteLine(risk);
            } else // PART 2 : Basins
            {
                markedMap = new bool[map.GetLength(0), map.GetLength(1)];

                List<int> basins = new List<int>();

                for (int y = 0; y < map.GetLength(1); y++)
                {
                    for (int x = 0; x < map.GetLength(0); x++)
                    {
                        if (!markedMap[x,y])
                        {
                            markedMap[x, y] = true;
                            if (map[x,y] != 9)
                            {
                                basins.Add(Basin(x,y)); // New basin detected
                            }
                        }
                    }
                }

                Console.WriteLine(basins.OrderByDescending(a => a).Take(3).Aggregate((a,b) => a*b));
            }            
        }

        private static int Basin (int initX, int initY) // Compute size of the Basin using a search algorithm
        {
            int basinSize = 0;
            markedMap[initX, initY] = true;

            List<int[]> pointsToEvaluate = new List<int[]>();

            pointsToEvaluate.Add(new int[] { initX, initY });

            while(pointsToEvaluate.Count() > 0)
            {
                int x = pointsToEvaluate[0][0];
                int y = pointsToEvaluate[0][1];

                basinSize++;

                if ((x > 0) && !markedMap[x - 1, y] && (map[x - 1, y] != 9))
                {
                    pointsToEvaluate.Add(new int[] { x - 1, y });
                    markedMap[x - 1, y] = true;

                }
                if ((y > 0) && !markedMap[x, y - 1] && (map[x, y - 1] != 9))
                {
                    pointsToEvaluate.Add(new int[] { x, y - 1 });
                    markedMap[x, y - 1] = true;
                }
                if ((x < map.GetLength(0) - 1) && !markedMap[x + 1, y] && (map[x + 1, y] != 9))
                {
                    pointsToEvaluate.Add(new int[] { x + 1, y });
                    markedMap[x + 1, y] = true;
                }
                if ((y < map.GetLength(1) - 1) && !markedMap[x, y + 1] && (map[x, y + 1] != 9))
                {
                    pointsToEvaluate.Add(new int[] { x, y + 1 });
                    markedMap[x, y + 1] = true;
                }

                pointsToEvaluate.RemoveAt(0);
            }

            return basinSize;
        }

    }
}
