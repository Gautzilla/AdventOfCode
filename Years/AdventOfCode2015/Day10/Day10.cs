using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace AdventOfCode2015
{
    public static class Day10
    {
        private static int _repetitionsPart1 = 40;
        private static int _repetitionsPart2 = 50;
        public static void Solve(int part)
        { 
            string input = File.ReadAllText(@"Day10\input.txt");

            int repetitions = part == 1 ? _repetitionsPart1 : _repetitionsPart2;
            
            for (int i = 0; i < repetitions; i++) input = LookAndSay(input);

            Console.WriteLine(input.Length);
        }

        private static string LookAndSay(string input) => String.Join("", Regex.Matches(input, @"(.)\1*").Select(match => $"{match.Value.Length}{match.Value[0]}"));
    }
}