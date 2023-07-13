using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace AdventOfCode2015
{
    public static class Day8
    {
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day8\input.txt");

            int totalCharNumber = input.Select(line => line.Length).Sum();
            int memoryCharNumber = input.Select(line => line.MemoryCount()).Sum();
            int encodedStringNumber = input.Select(line => line.EncodedCharCount()).Sum();

            if (part == 1) Console.WriteLine($"{totalCharNumber} - {memoryCharNumber} = {totalCharNumber-memoryCharNumber}");
            else Console.WriteLine($"{encodedStringNumber} - {totalCharNumber} = {encodedStringNumber-totalCharNumber}");
        }

        private static int MemoryCount(this string line)
        {
            line = line.Trim('"');
            line = line.Replace("\\\\", "A");
            line = line.Replace("\\\"", "A");
            line = Regex.Replace(line, @"\\x[0-9a-f]{2}", "A");

            return line.Length;
        }

        private static int EncodedCharCount(this string line)
        {
            line = line.Replace("\"", "AA");
            line = line.Replace("\\", "AA");

            return line.Length + 2; // Opening and closing "
        }
    }
}