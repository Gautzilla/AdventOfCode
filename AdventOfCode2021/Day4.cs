using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day4
    {
        static public void Solve()
        {
            string filePath = @"D:\Documents Gauthier\Programmation\AdventOfCode2021\Day4\input.txt";
            List<string> input = File.ReadAllLines(filePath).ToList();
            List<string> chosenNumbers = input[0].Split(',').ToList();

            List<string[,]> grids = CreateGrids(input);

            bool bingo = false;
            bool noUncompleteGrid = false;
            List<int> uncompleteGrids = Enumerable.Range(0, grids.Count() - 1).ToList();

            for (int i = 5; i < chosenNumbers.Count(); i++)
            {
                for (int j = 0; j < grids.Count(); j++)
                {
                    bingo = Bingo(grids[j], chosenNumbers.Take(i).ToList());

                    if (bingo)
                    {
                        uncompleteGrids.RemoveAll(a => a == j);
                        if (uncompleteGrids.Count() == 0)
                        {
                            Console.WriteLine(GridScore(grids[j], chosenNumbers.Take(i).ToList()));
                            noUncompleteGrid = true;
                        }
                        if (noUncompleteGrid) break;
                    }
                }
                if (noUncompleteGrid) break;
            }
        }

        static List<string[,]> CreateGrids(List<string> input)
        {
            int activeGrid = -1;
            int gridLine = 0;
            List<string[,]> grids = new List<string[,]>();

            foreach (string s in input.Skip(1))
            {
                if (s == "")
                {
                    grids.Add(new string[5, 5]);
                    activeGrid++;
                    gridLine = 0;
                }
                else
                {
                    string[] values = s.Split(' ').Where(a => a != "").ToArray();

                    for (int i = 0; i < values.Length; i++)
                    {
                        grids[activeGrid][gridLine, i] = values[i];
                    }
                    gridLine++;
                }
            }

            return grids;
        }

        static bool Bingo(string[,] grid, List<string> numbers)
        {
            for (int x = 0; x < 5; x++)
            {
                bool falseNumber = false;
                for (int y = 0; y < 5; y++)
                {
                    if (!numbers.Contains(grid[x, y])) falseNumber = true;
                }
                if (!falseNumber) return true;
            }

            for (int y = 0; y < 5; y++)
            {
                bool falseNumber = false;
                for (int x = 0; x < 5; x++)
                {
                    if (!numbers.Contains(grid[x, y])) falseNumber = true;
                }
                if (!falseNumber) return true;
            }

            return false;
        }

        static int GridScore(string[,] grid, List<string> numbers)
        {
            int unmarkedNumbers = 0;


            foreach (string s in grid)
            {
                if (!numbers.Contains(s))
                {
                    unmarkedNumbers += int.Parse(s);
                }
            }

            return (unmarkedNumbers * int.Parse(numbers.Last()));
        }
    }
}
