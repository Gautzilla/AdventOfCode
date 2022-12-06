using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day13
    {

        static bool[,] page;

        public static void Solve(int part)
        {
            //string path = @"..\..\Inputs\day13Example.txt";
            string path = @"..\..\Inputs\day13.txt";

            List<string> input = File.ReadAllLines(path).ToArray().ToList();

            CreatePage(input);

            List<string[]> instructions = input.SkipWhile(i => i != "").Skip(1).Select(i => i.Split(' ').Last().Split('=').ToArray()).ToList();

            int folds = part == 1 ? 1 : instructions.Count();

            for (int f = 0; f < folds; f++) Fold(instructions[f]);

            VisualizePage();

            Console.WriteLine($"{CountDots()} dots.");

            

        }

        private static void CreatePage(List<string> input)
        {
            List<int[]> dots = input.TakeWhile(i => i != "").Select(a => a.Split(',').Select(s => int.Parse(s)).ToArray()).ToList();

            int maxValueX = dots.Select(d => d[0]).Max() + 1;
            int maxValueY = dots.Select(d => d[1]).Max() + 1;

            page = new bool[maxValueX, maxValueY];

            foreach (int[] line in dots) page[line[0], line[1]] = true;
        }

        private static void Fold(string[] instruction)
        {
            int lineToFold = int.Parse(instruction[1]);
            int foldedPageXLength = instruction[0] == "x" ? lineToFold : page.GetLength(0);
            int foldedPageYLength = instruction[0] == "y" ? lineToFold : page.GetLength(1);

            bool[,] foldedPage = new bool[foldedPageXLength,foldedPageYLength];

            for (int y = 0; y < page.GetLength(1); y++)
            {
                for (int x = 0; x < page.GetLength(0); x++)
                {
                    if ((x < foldedPageXLength) && (y < foldedPageYLength)) foldedPage[x, y] = page[x, y];
                    else if (page[x,y])
                    {
                        switch (instruction[0])
                        {
                            case "x":
                                foldedPage[2 * lineToFold - x, y] = true;
                                break;
                            case "y":
                                foldedPage[x, 2 * lineToFold - y] = true;
                                break;
                        }
                    }
                }
            }

            page = foldedPage;
        }

        private static int CountDots()
        {
            int dots = 0;

            for (int y = 0; y < page.GetLength(1); y++)
            {
                for (int x = 0; x < page.GetLength(0); x++) dots += page[x,y]? 1 : 0;
            }

            return dots;
        }

        private static void VisualizePage()
        {
            for (int y = 0; y < page.GetLength(1); y++)
            {
                for (int x = 0; x < page.GetLength(0); x++) Console.Write(page[x, y] ? "#" : " ");

                Console.Write("\n");
            }
        }
    }
}
