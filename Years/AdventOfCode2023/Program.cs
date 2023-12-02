using System.Diagnostics;
using AdventOfCode2023;

Stopwatch stopWatch = new Stopwatch();
            Console.WriteLine("Part:");
            if (int.TryParse(Console.ReadLine(), out int part) && part == 1 || part == 2) 
            {
                stopWatch.Start();
                Day02.Solve(part);
                stopWatch.Stop();
            }
            else
            {
                Console.WriteLine("Part should be 1 or 2.");
                return;
            }

            Console.WriteLine($"\nSolved in {stopWatch.ElapsedMilliseconds} ms");