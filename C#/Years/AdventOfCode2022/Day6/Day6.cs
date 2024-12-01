using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day6
    {
        public static void Solve(int part)
        { 
            string input = File.ReadAllText(@"Day6\input.txt");
            int bufferSize = part == 1 ? 4 : 14;

            for (int i = 0; i < input.Length-bufferSize; i++)
            {
                if (input.Skip(i).Take(bufferSize).Distinct().Count() == bufferSize)
                {
                    Console.WriteLine(i+bufferSize);
                    return;
                }
            }
        }
    }
}