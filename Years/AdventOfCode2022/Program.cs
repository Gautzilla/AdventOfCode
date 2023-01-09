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
            Console.WriteLine("Part:");
            if (int.TryParse(Console.ReadLine(), out int part) && part == 1 || part == 2) 
            {
                stopWatch.Start();
                Day17.Solve(part);
                stopWatch.Stop();
            }
            else
            {
                Console.WriteLine("Part should be 1 or 2.");
                return;
            }

            Console.WriteLine($"\nSolved in {stopWatch.ElapsedMilliseconds} ms");
        }
    }
}
