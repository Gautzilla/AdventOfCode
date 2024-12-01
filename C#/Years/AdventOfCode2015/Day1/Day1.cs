using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2015
{
    public static class Day1
    {
        public static void Solve(int part)
        { 
            string input = File.ReadAllText(@"Day1\input.txt");

            int floor = 0;
            for (int c = 0; c < input.Length; c++)
            {
                floor += input[c] == '(' ? 1 : -1;

                if (part == 2 && floor == -1)
                {
                    Console.WriteLine(c+1);
                    return;
                }
            }
            if (part == 1) Console.WriteLine(floor);
        }
    }
}