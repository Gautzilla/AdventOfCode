using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Day16.Solve(2);

            stopWatch.Stop();
            Console.WriteLine($"\nSolved in {stopWatch.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }
    }
}
