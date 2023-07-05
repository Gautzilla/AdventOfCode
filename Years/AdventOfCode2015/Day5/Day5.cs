using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2015
{
    public static class Day5
    {
        private static string _vowels = "aeiou";
        private static string[] _forbiddenStrings = {"ab", "cd", "pq", "xy"};
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day5\input.txt");
            
            Console.WriteLine(input.Count(line => line.IsNice(part)));
        }

        private static bool IsNice(this string input, int part) 
        {
            if (part == 1) return input.ContainsThreeVowels() && input.ContainsLetterTwiceInARow() && !input.ContainsForbiddenSubstring();
            return true;
        }

        private static bool ContainsThreeVowels(this string input) => input.Where(c => _vowels.Contains(c)).Count() >= 3;

        private static bool ContainsLetterTwiceInARow(this string input)
        {
            for (int i = 1; i < input.Length; i++) if(input[i] == input[i-1]) return true;
            return false;
        }

        private static bool ContainsForbiddenSubstring(this string input)
        {
            return _forbiddenStrings.Any(substring => input.IndexOf(substring) != -1);
        }
    }
}