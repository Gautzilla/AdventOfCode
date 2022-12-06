using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day7
    {
        public static void Solve(int part)
        {
            // string path = @"..\..\Inputs\day7Example.txt";
            string path = @"..\..\Inputs\day7.txt";

            string input = File.ReadAllLines(path)[0];

            List<int> submarines = input.Split(',').Select(a => int.Parse(a)).OrderBy(a => a).ToList();

            if (part == 1)
            {
                int median = submarines[submarines.Count() / 2]; // Optimality property of the median

                int fuel = 0;

                foreach (int submarine in submarines)
                {
                    fuel += Math.Abs(submarine - median);
                }

                Console.WriteLine($"On consomme {fuel} fuel pour aller en position {median}");
            } else
            {
                int meanLow = submarines.Sum() / submarines.Count(); // We check the 2 closest values from the float mean
                int meanHigh = meanLow + 1;

                int fuelLow = 0;
                int fuelHigh = 0;

                foreach (int submarine in submarines)
                {
                    fuelLow += Enumerable.Range(0, Math.Abs(meanLow - submarine) + 1).Sum();
                    fuelHigh += Enumerable.Range(0, Math.Abs(meanHigh - submarine) + 1).Sum();
                }

                int fuel = Math.Min(fuelLow, fuelHigh);
                int mean = fuelLow < fuelHigh ? meanLow : meanHigh;

                Console.WriteLine($"On consomme {fuel} fuel pour aller en position {mean}");
            }            
        }
    }
}
