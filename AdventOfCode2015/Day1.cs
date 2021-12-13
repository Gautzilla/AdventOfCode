using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2015
{
    class Day1
    {
        static public void Solve(int part)
        {
            // string filePath = @"..\..\Inputs\day1Example.txt";
            string filePath = @"..\..\..\Inputs\day1.txt";

            string input = File.ReadAllText(filePath);

            int floor = input.Aggregate(0, (a, b) => b == '(' ? ++a : --a);

            if (part == 1) Console.WriteLine($"Santa is led to the floor {floor}.");

            else
            {
                int pos = 0;
                int charPos = 0;

                foreach (char c in input)
                {
                    charPos++;
                    pos += c == '(' ? 1 : -1;
                    if (pos == -1) break;
                }
                Console.WriteLine($"Santa reaches basement with instruction {charPos}.");
            }    
        }
    }
}
