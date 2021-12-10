using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day10
    {
        
        public static void Solve(int part)
        {
            //string path = @"D:\Documents Gauthier\Programmation\AdventOfCode2021\Day9\exampleInput.txt";
            //string path = @"D:\Documents Gauthier\Programmation\AdventOfCode2021\Day9\input.txt";
            string path = @"C:\Users\User\Desktop\exampleInput.txt";

            List<string> input = File.ReadAllLines(path).ToArray().ToList();

            char[] openSymbolsRef = { '(', '[', '{', '<' };
            char[] closeSymbolsRef = { ')', ']', '}', '>' };

            List<string> corruptedLines = new List<string>();

            long score = 0;

            foreach (string line in input)
            {
                List<char> openSymbols = new List<char>();

                char corrupted = ' ';

                foreach (char c in line)
                {
                    if (openSymbolsRef.Contains(c)) openSymbols.Add(c);
                    else if (GetIndex(c, closeSymbolsRef) == GetIndex(openSymbols.Last(), openSymbolsRef)) openSymbols.RemoveAt(openSymbols.Count() - 1);
                    else corrupted = c;

                    if (corrupted != ' ')
                    {
                        if (part == 1)
                        {
                            long[] scores = {3,57,1197,25137};
                            score += scores[GetIndex(c, closeSymbolsRef)];
                            break;
                        } else
                        {
                            corruptedLines.Add(line);
                        }
                    }
                }
            }

            if (part == 2)
            {
                List<string> uncompleteLines = input.Where(a => !corruptedLines.Contains(a)).ToList();
                List<long> scores = new List<long>();

                foreach (string line in uncompleteLines)
                {
                    List<char> openSymbols = new List<char>();

                    foreach (char c in line)
                    {
                        if (openSymbolsRef.Contains(c)) openSymbols.Add(c);
                        else if (GetIndex(c, closeSymbolsRef) == GetIndex(openSymbols.Last(), openSymbolsRef)) openSymbols.RemoveAt(openSymbols.Count() - 1);
                    }

                    List<char> missingCloseSymbols = openSymbols.Select(a => closeSymbolsRef[GetIndex(a, openSymbolsRef)]).Reverse().ToList();

                    scores.Add(missingCloseSymbols.Aggregate((long)0, (a, b) => (a * 5) + (GetIndex(b, closeSymbolsRef) + 1)));
                }

                score = scores.OrderBy(a => a).Skip(scores.Count() / 2).First();
            }

                Console.Write(score);
        }

        private static int GetIndex(char c, char[] charArray)
        {
            int index = 0;
            foreach (char ch in charArray)
            {
                if (ch == c) return index;
                index++;
            }
            return 0; // shouldn't happen
        }
    }
}
