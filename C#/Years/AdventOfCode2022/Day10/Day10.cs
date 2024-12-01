using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day10
    {
        static private int _register = 1;
        static private int _cycle = 1;
        static private int _signalStrength = 0;
        static private readonly int[] _recordedCycles = {20, 60, 100, 140, 180, 220};
        static private List<string> _crt = new();
        static private int _part = 1;
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day10\input.txt");
            _part = part;

            foreach (var instruction in input)
            {
                ManageCycle();
                string[] parts = instruction.Split(" ").ToArray();
                
                if (parts[0] == "addx") 
                {
                    _cycle++;
                    ManageCycle();
                    _register += int.Parse(parts[1]);
                }

                _cycle++;
            }

            if (part == 1) Console.WriteLine(_signalStrength);
            else  Console.WriteLine(String.Join("\n", _crt));
        }

        private static void ManageCycle()
        {
            if (_part == 1)
            {
                if (_recordedCycles.Contains(_cycle)) _signalStrength += _cycle * _register;
                return;
            }
            int position = (_cycle-1) % 40;
            if (position == 0) _crt.Add(string.Empty);
            _crt[_crt.Count - 1] += (position >= _register-1 && position <= _register + 1) ? "#" : ".";
        }
    }
}