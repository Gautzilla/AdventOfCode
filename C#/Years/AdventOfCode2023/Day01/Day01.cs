using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;

namespace AdventOfCode2023
{
    public static class Day01
    {
        private const string _regexPart1 = @"\d";
        private const string _regexPart2 = @"\d|one|two|three|four|five|six|seven|eight|nine";
        private static readonly string[] _digitStrings = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};

        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day01\input.txt");
            
            Console.WriteLine(input
                .Select(line => CalibrationValue(line, part))
                .Sum());
        }

        private static int CalibrationValue(string line, int part)
        {
            string regex = part == 1 ? _regexPart1 : _regexPart2;

            string[] digits = [Regex.Match(line, regex).Value, Regex.Match(line, regex, RegexOptions.RightToLeft).Value];

            digits = digits
                .Select(d => _digitStrings.Contains(d) ? Array.IndexOf(_digitStrings, d).ToString() : d)
                .ToArray();

            return int.Parse($"{digits.First()}{digits.Last()}");
        }
    }
}