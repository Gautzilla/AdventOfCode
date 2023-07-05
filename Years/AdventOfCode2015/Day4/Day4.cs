using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2015
{
    public static class Day4
    {
        static System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        public static void Solve(int part)
        { 
            string input = File.ReadAllText(@"Day4\input.txt");
            int tail = 0;

            while (true)
            {
                string stringToConvert = input + tail;

                Byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(stringToConvert));
                if (part == 1 && hash[0] == 0 && hash[1] == 0 && hash[2] < 0x10)
                {
                    Console.WriteLine(tail);
                    return;
                } else if (hash[0] == 0 && hash[1] == 0 && hash[2] == 0)
                {
                    Console.WriteLine(tail);
                    return;
                }

                tail++;
            }
        }
    }
}