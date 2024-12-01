using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day5
    {
        static public void Solve(int part)
        {
            // string path = @"..\..\Inputs\day5Example.txt";
            string path = @"..\..\Inputs\day5.txt";

            List<string> input = File.ReadAllLines(path).ToList();

            List<int[]> vents = input.Select(a => a.Split(new char[] { ',' , '-', '>', ' ' }, StringSplitOptions.RemoveEmptyEntries)).Select(b => b.Select(c => int.Parse(c)).ToArray()).ToList();

            if (part == 1) vents.RemoveAll(a => ((a[0] != a[2]) && (a[1] != a[3])));

            int maxValue = vents.Select(a => a.Max()).Max()+1;
            int[,] grid = new int[maxValue, maxValue];

            foreach (int[] i in vents)
            {
                int x1 = i[0];
                int x2 = i[2];
                int y1 = i[1];
                int y2 = i[3];

                if (x1 == x2)
                {
                    if (y1 < y2)
                    {
                        for (int j = y1; j <= y2; j++)
                        {
                            grid[x1, j]++;
                        }
                    } else
                    {
                        for (int j = y1; j >= y2; j--)
                        {
                            grid[x1, j]++;
                        }
                    }
                } else if (y1 == y2)
                {
                    if (x1 < x2)
                    {
                        for (int j = x1; j <= x2; j++)
                        {
                            grid[j, y1]++;
                        }
                    }
                    else
                    {
                        for (int j = x1; j >= x2; j--)
                        {
                            grid[j, y1]++;
                        }
                    }
                } else if (x1 < x2)
                    {
                        if (y1 < y2)
                        {
                            for(int j = 0; j <= x2 - x1; j++)
                            {
                                grid[x1 + j, y1 + j]++;
                            }
                        } else
                        {
                            for (int j = 0; j <= x2 - x1; j++)
                            {
                                grid[x1 + j, y1 - j]++;
                            }
                        }
                    } else
                {
                    if (y1 < y2)
                    {
                        for (int j = 0; j <= x1 - x2; j++)
                        {
                            grid[x1 - j, y1 + j]++;
                        }
                    }
                    else
                    {
                        for (int j = 0; j <= x1 - x2; j++)
                        {
                            grid[x1 - j, y1 - j]++;
                        }
                    }
                }
            }

            int overlaps = 0;

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(0); j++)
                {
                    if (grid[j, i] > 1) overlaps++;
                }
            }

            Console.WriteLine($"\n {overlaps} overlaps");
        }
    }
}
