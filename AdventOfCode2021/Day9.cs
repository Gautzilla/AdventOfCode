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
            // string path = @"..\..\Inputs\day9Example.txt";
            string path = @"..\..\Inputs\day9.txt";

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

                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                if (Math.Abs(i) + Math.Abs(j) == 1)
                                {
                                    if (( (x+i) >= 0) && ((x+i) < map.GetLength(0)) && ((y+j) >= 0) && ((y+j) < map.GetLength(1)) && risky)
                                    {
                                        risky = map[x + i, y + j] > point;
                                    }
                                }
                            }
                        }

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

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (Math.Abs(i) + Math.Abs(j) == 1)
                        {
                            if ( ((x + i) >= 0) && ((x + i) < map.GetLength(0)) && ((y + j) >= 0) && ((y + j) < map.GetLength(1)) && !markedMap[x+i, y+j] && (map[x+i, y+j] != 9))
                            {
                                pointsToEvaluate.Add(new int[] { x + i, y + j });
                                markedMap[x + i, y + j] = true;
                            }
                        }
                    }
                }

                pointsToEvaluate.RemoveAt(0);
            }

            return basinSize;
        }

    }
}
