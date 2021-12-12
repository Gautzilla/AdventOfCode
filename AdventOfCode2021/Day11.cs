using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day11
    {

        static int[,] map;
        static bool[,] flashMap;
        static int flashes;

        public static void Solve(int part)
        {
            //string path = @"..\..\Inputs\day11Example.txt";
            string path = @"..\..\Inputs\day11.txt";

            List<string> input = File.ReadAllLines(path).ToArray().ToList();

            map = new int[input[0].Length, input.Count()];
            flashMap = new bool[input[0].Length, input.Count()];
            flashes = 0;

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    map[x, y] = int.Parse(input[y][x].ToString());
                }
            }

            int stepCount = 100;
            int step = 0;

            while (true)
            {
                step++;
                flashMap = new bool[input[0].Length, input.Count()];

                for (int y = 0; y < map.GetLength(1); y++)
                {
                    for (int x = 0; x < map.GetLength(0); x++)
                    {
                        map[x, y]++;
                    }
                }

                for (int y = 0; y < map.GetLength(1); y++)
                {
                    for (int x = 0; x < map.GetLength(0); x++)
                    {
                        if ( (map[x, y] > 9) && !flashMap[x,y]) Flash(x, y);
                    }
                }

                VisualizeMap(step);

                if (part == 1 && step == stepCount) break;

                if (part == 2)
                {
                    bool everybodyFlashes = true;

                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        for (int x = 0; x < map.GetLength(0); x++)
                        {
                            if (map[x, y] != 0) everybodyFlashes = false;
                        }
                    }
                    if (everybodyFlashes) break;
                }
            }

            Console.WriteLine($"{flashes} flashes. Everybody flashes on step {step}");

        }

        static void Flash (int x, int y)
        {
            flashMap[x, y] = true;
            map[x, y] = 0;
            flashes++;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ( (x+i >= 0) && (x+i < map.GetLength(0)) && (y + j >= 0) && (y + j < map.GetLength(1)) && !flashMap[x+i,y+j] && (Math.Abs(i) + Math.Abs(j) != 0))
                    {
                        if (++map[x + i, y + j] > 9) Flash(x + i, y + j);
                    }
                }
            }
        }

        static void VisualizeMap(int step)
        {
            System.Threading.Thread.Sleep(40);
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"STEP {step + 1} :\n");

            ConsoleColor[] colors = { ConsoleColor.Red, ConsoleColor.DarkGray, ConsoleColor.Gray, ConsoleColor.White, ConsoleColor.DarkCyan, ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Magenta };

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Console.ForegroundColor = colors[map[x, y]];
                    Console.Write(map[x, y]);
                }
                Console.Write("\n");
            }
        }
    }
}
