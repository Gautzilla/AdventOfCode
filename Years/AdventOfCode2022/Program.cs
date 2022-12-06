using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter the part of the problem to solve.");
                return;
            }

            if (int.TryParse(args.First(), out int part) && part == 1 || part == 2) Day5.Solve(part);
            else
            {
                Console.WriteLine("Part should be 1 or 2.");
                return;
            }

            stopWatch.Stop();
            Console.WriteLine($"\nSolved in {stopWatch.ElapsedMilliseconds} ms");
        }
    }
}
