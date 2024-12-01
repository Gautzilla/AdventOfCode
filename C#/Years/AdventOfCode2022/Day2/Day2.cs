using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day2
    {
        private static readonly int[] _rules = {2, 0, 1}; // index of the shape that each index beats (RPS)

        public static void Solve(int part)
        {
            int result = 0;
            foreach (string line in File.ReadAllLines(@"Day2\input.txt"))
            {
                if (part == 1) result += line.First() - 'X' + 1 + Match (line.First(), line.Last());
                else result += SelfShape(line.First(), line.Last()) + 1 + ( line.Last() - 'X' ) * 3;
            }

            Console.WriteLine(result);
        }

        private static int Match(char opp, char self) 
        {
            int oppVal = opp - 'A';
            int selfVal = self - 'X';

            if (_rules[oppVal] == selfVal) return 0;
            if (oppVal == selfVal) return 3;
            return 6;
        }

        private static int SelfShape(char opp, char result)
        {
            int oppVal = opp - 'A';
            if (result == 'X') return _rules[oppVal];
            if (result == 'Y') return oppVal;
            return Array.IndexOf(_rules, oppVal);
        }
    }
}