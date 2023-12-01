using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace AdventOfCode2023
{
    public static class Day01
    {
        private static readonly string[] _letterDigits = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};

        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day01\input.txt");
            
            Console.WriteLine(input
                .Select(line => CalibrationValue(line, part))
                .Sum());
        }

        private static int CalibrationValue(string line, int part)
        {
            if (part == 2)
            {
                line = ReplaceLetterDigits(line, true);
                line = ReplaceLetterDigits(line, false);
            }

            var filtered = string.Join("", line
                .Where(char.IsDigit));

            var combined = $"{filtered.First()}{filtered.Last()}";            

            return int.Parse(combined);
        }

        private static string ReplaceLetterDigits(string line, bool leftToRight)
        {
            int index = leftToRight ? 0 : (line.Length-_letterDigits.Min(l => l.Length - 1));
            int runningLength = 1;
            string running = "";
            bool digitFound = false;

            while (index + runningLength <= line.Length && index >= 0)
            {
                running = line.Substring(index, runningLength);

                string? letterDigit = _letterDigits.FirstOrDefault(d => d == running);
                if (!string.IsNullOrEmpty(letterDigit))
                {
                    digitFound = true;
                    break;
                } 

                if (_letterDigits.Any(d => d.StartsWith(running)))
                {
                    runningLength++;
                    continue;
                }

                runningLength = 1;
                index += leftToRight ? 1 : -1;
            }

            if (digitFound)
            {
                string value = Array.IndexOf(_letterDigits, running).ToString();
                line = line.Insert(index, value);
            }

            return line;
        }
    }
}