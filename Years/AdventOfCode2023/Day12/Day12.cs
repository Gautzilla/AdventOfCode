using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public static class Day12
    {
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day12\input.txt");

            int result = input.Sum(line => NbMatchingSpringSequences(line.GetSpringSequence(), line.GetRequiredConsecutiveLitSprings()));

            Console.WriteLine(result);
        }

        private static int[] GetRequiredConsecutiveLitSprings (this string line) => line
            .Split(' ')
            .Last()
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        private static string GetSpringSequence (this string line) => line
            .Split(' ')
            .First();
        
        private static int NbMatchingSpringSequences (string springs, int[] litLengths)
        {

            int litLengthIndex = 0;
            int nbConsecutiveLit = 0;

            foreach (var spring in springs)
            {
                if (spring == '?') return NbMatchingSpringSequences(springs.ReplaceUnknown(true), litLengths) + NbMatchingSpringSequences(springs.ReplaceUnknown(false), litLengths);

                if (spring == '.')
                {
                    if (nbConsecutiveLit == 0) continue;
                    if (litLengthIndex < litLengths.Length && litLengths[litLengthIndex] != nbConsecutiveLit) return 0;
                    litLengthIndex++;
                    nbConsecutiveLit = 0;
                }

                if (spring == '#')
                {
                    if (litLengthIndex < litLengths.Length && ++nbConsecutiveLit > litLengths[litLengthIndex]) return 0;
                }
            }

            return springs.Split('.', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Length).SequenceEqual(litLengths) ? 1 : 0;
        }

        private static string ReplaceUnknown (this string springs, bool isNextSpringLit)
        {
            int index = springs.IndexOf('?');
            var springsCharArray = springs.ToCharArray();
            springsCharArray[index] = isNextSpringLit ? '#' : '.';

            return new string(springsCharArray);
        }
    }
}