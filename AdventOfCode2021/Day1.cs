using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day1
    {
        static public void Solve(int part)
        {
            // string path = @"..\..\Inputs\day4Example.txt";
            string path = @"..\..\Inputs\day1.txt";

            List<int> input = File.ReadAllLines(path).Select(a => int.Parse(a)).ToList();

            int increases = 0;

            if (part == 1)
            {
                for (int i = 1; i < input.Count(); i++)
                {
                    if (input[i] > input[i - 1]) increases++;
                }
            } else
            { 
                for (int i = 3; i < input.Count(); i++)
                {
                    if (input[i] + input[i-1] + input[i-2] > input[i - 1] + input[i - 2] + input[i - 3]) increases++;
                }
            }
            Console.WriteLine(increases);
        }
    }
}
