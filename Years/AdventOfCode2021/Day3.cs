using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day3
    {
        static public void Solve(int part)
        {
            //string path = @"..\..\Inputs\day3Example.txt";
            string path = @"..\..\Inputs\day3.txt";

            List<string> input = File.ReadAllLines(path).ToList();

            string gammaRate = "";
            string epsilonRate = "";

            if (part == 1)
            {
                for (int i = 0; i < input[0].Length; i++)
                {
                    gammaRate += input.Count(a => a[i] == '1') > (input.Count() / 2) ? '1' : '0';
                    epsilonRate += gammaRate[i] == '1' ? '0' : '1';
                }

                Console.WriteLine($"Gamma Rate : {Convert.ToInt32(gammaRate, 2)} \nEpsilon Rate : {Convert.ToInt32(epsilonRate, 2)} \nPower Consumption : {Convert.ToInt32(gammaRate, 2) * Convert.ToInt32(epsilonRate, 2)}");
            }

            if (part == 2)
            {
                List<string> oxygenGeneratorRating = new List<string>(input);
                List<string> co2ScrubberRating = new List<string>(input);

                int i = 0;

                while (oxygenGeneratorRating.Count() > 1)
                {
                    oxygenGeneratorRating = oxygenGeneratorRating.Where(a => (oxygenGeneratorRating.Count(b => b[i] == a[i]) > oxygenGeneratorRating.Count(b => b[i] != a[i])) || (oxygenGeneratorRating.Count(b => b[i] == a[i]) == oxygenGeneratorRating.Count(b => b[i] != a[i])) && (a[i] == '1')).ToList();
                    i++;
                }

                i = 0;

                while (co2ScrubberRating.Count() > 1)
                {
                    co2ScrubberRating = co2ScrubberRating.Where(a => (co2ScrubberRating.Count(b => b[i] == a[i]) < co2ScrubberRating.Count(b => b[i] != a[i])) || (co2ScrubberRating.Count(b => b[i] == a[i]) == co2ScrubberRating.Count(b => b[i] != a[i])) && (a[i] == '0')).ToList();
                    i++;
                }

                Console.WriteLine($"Oxygen Generator Rating : {Convert.ToInt32(oxygenGeneratorRating.First(), 2)} \nCO2 Scrubber Rating : {Convert.ToInt32(co2ScrubberRating.First(), 2)} \nLife Support Rating : {Convert.ToInt32(oxygenGeneratorRating.First(), 2) * Convert.ToInt32(co2ScrubberRating.First(), 2)}");
            }
            
        }
    }
}
