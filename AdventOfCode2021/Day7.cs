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
        public static void SolvePart1()
        {
            //string path = @"C:\Users\User\Desktop\day7.txt";
            string path = @"C:\Users\User\Desktop\day7Example.txt";

            string input = File.ReadAllLines(path)[0];

            List<int> submarines = input.Split(',').Select(a => int.Parse(a)).OrderBy(a => a).ToList();

            int median = submarines[submarines.Count() / 2]; // Optimality property of the median

            int fuel = 0;

            foreach (int submarine in submarines)
            {
                Console.WriteLine($"Le sous-marin en position {submarine} va consommer {Math.Abs(submarine - median)} fuel");
                fuel += Math.Abs(submarine - median);
            }

            Console.WriteLine($"On consomme {fuel} fuel pour aller en position {median}");
        }

        public static void SolvePart2()
        {
            string path = @"C:\Users\User\Desktop\day7.txt";
            //string path = @"C:\Users\User\Desktop\day7Example.txt";

            string input = File.ReadAllLines(path)[0];

            List<int> submarines = input.Split(',').Select(a => int.Parse(a)).OrderBy(a => a).ToList();

            int meanLow = submarines.Sum() / submarines.Count(); // We check the 2 closest values from the float mean
            int meanHigh = meanLow + 1;

            int fuelLow = 0;
            int fuelHigh = 0;

            foreach (int submarine in submarines)
            {
                fuelLow += Enumerable.Range(0, Math.Abs(meanLow-submarine)+1).Sum();
                fuelHigh += Enumerable.Range(0, Math.Abs(meanHigh-submarine)+1).Sum();
            }

            int fuel = Math.Min(fuelLow, fuelHigh);
            int mean = fuelLow < fuelHigh ? meanLow : meanHigh;

            Console.WriteLine($"On consomme {fuel} fuel pour aller en position {mean}");
        }
    }
}
