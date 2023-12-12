using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2023
{
    public static class Day12
    {
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day12\input.txt");

            int result = SolvePart1(input);

            Console.WriteLine(result);
        }

        private static int SolvePart1 (string[] input)
        {
            int output = 0;

            foreach (var line in input)
            {
                int[] seriesLengths = line
                    .Split(' ')
                    .Last()
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray();
                
                string springs = line
                    .Split(' ')
                    .First();

                for (int litSprings = 0; litSprings < Math.Pow(2,springs.Count(c => c == '?')); litSprings++)
                {
                    var unknowns = springs
                        .Select((c, index) => (c, index))
                        .Where(spring => spring.c == '?')
                        .ToArray();
                        
                    var temp = springs.ToCharArray();

                    for (int springToLit = 0; springToLit < unknowns.Length; springToLit++)
                    {
                        char springTemp = ((int)Math.Pow(2,springToLit) | litSprings) == litSprings ? '#' : '.';
                        temp[unknowns[springToLit].index] = springTemp;                        
                    }

                    if (FitLengths(new string(temp), seriesLengths)) output++;
                }
            }

            return output;
        }

        private static bool FitLengths (string springs, int[] seriesLengths) => seriesLengths
            .SequenceEqual(springs
                .Split('.', StringSplitOptions.RemoveEmptyEntries)
                .Select(groups => groups.Length)
                .ToArray()
            );
    }
}