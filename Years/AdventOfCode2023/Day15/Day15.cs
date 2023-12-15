using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;

namespace AdventOfCode2023
{
    public static class Day15
    {
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllText(@"Day15\input.txt").Split(',');

            Console.WriteLine(input.Sum(HASH));
        }

        /*
            Determine the ASCII code for the current character of the string.
            Increase the current value by the ASCII code you just determined.
            Set the current value to itself multiplied by 17.
            Set the current value to the remainder of dividing itself by 256.
        */

        private static int HASH (this string s) => s.Aggregate(0, (a, b) => ((a + b) * 17) % 256);
    }
}