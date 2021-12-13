using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2015
{
    class Day2
    {
        static public void Solve(int part)
        {
            string filePath = @"..\..\..\Inputs\day2.txt";

            List<string> input = File.ReadAllLines(filePath).ToList();

            int paperSurface = 0;
            int ribbonLength = 0;

            foreach (string line in input)
            {
                int[] dimensions = line.Split('x').Select(a => int.Parse(a)).ToArray();
                paperSurface += 2 * (dimensions[0] * dimensions[1] + dimensions[0] * dimensions[2] + dimensions[1] * dimensions[2]) + dimensions.OrderBy(a => a).Take(2).Aggregate((x, y) => x * y);
                ribbonLength += dimensions.OrderBy(a => a).Take(2).Aggregate((x, y) => 2 * x + 2 * y) + dimensions.Aggregate((a, b) => a * b);
            }

            if (part == 1) Console.WriteLine($"Elves need {paperSurface} square meters of paper."); 
            else Console.WriteLine($"Elves need {ribbonLength} feet of ribbon.");
        }
    }
}
