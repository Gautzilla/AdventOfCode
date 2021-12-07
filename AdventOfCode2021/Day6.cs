using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day6
    {
        static public void Solve()
        {
            string filePath = @"D:\Documents Gauthier\Programmation\AdventOfCode2021\Day6\input.txt";

            List<int> input = File.ReadAllLines(filePath)[0].Split(',').Select(a => int.Parse(a)).ToList();
            List<long> lanternfishes = Enumerable.Repeat<long>(0, 9).ToList();

            foreach (int fish in input) lanternfishes[fish]++;

            int dayCount = 256;

            for (int i = 0; i < dayCount; i++)
            {
                List<long> newDay = Enumerable.Repeat<long>(0, 9).ToList();

                for (int j = 0; j < lanternfishes.Count(); j++)
                {
                    if (j == 0)
                    {
                        newDay[6] += lanternfishes[j];
                        newDay[8] += lanternfishes[j];
                    } else
                    {
                        newDay[j - 1] += lanternfishes[j];
                    }
                }

                lanternfishes = newDay;
            }

            Console.WriteLine($"There are {lanternfishes.Aggregate((a, b) => a + b)} fishes on day {dayCount}");
        }
    }
}
