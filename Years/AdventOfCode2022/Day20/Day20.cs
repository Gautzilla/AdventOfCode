using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day20
    {
        private static List<(int value, int position)> _numbers = new();
        public static void Solve(int part)
        { 
            ParseInput();
            MoveNumbers();
            Console.WriteLine(String.Join("\n", _numbers.Select(n => $"{n.position} : {n.value}")));
        }

        private static void ParseInput()
        {
            string[] input = File.ReadAllLines(@"Day20\input.txt");
            _numbers = input.Select((val,pos) => (int.Parse(val), pos)).ToList();
        }

        private static void MoveNumbers()
        {
            for (int n = 0; n < _numbers.Count; n++)
            {
                int oldPosition = _numbers[n].position;
                int newPosition = ComputeNewPosition(_numbers[n]);

                Console.WriteLine($"{_numbers[n].value} : {oldPosition} => {newPosition}");

                if (newPosition > _numbers[n].position)
                {
                    _numbers = _numbers.Select(n => n.position > oldPosition && n.position <= newPosition ? (n.value, n.position -1) : n).ToList();
                }

                if (newPosition < _numbers[n].position)
                {
                    _numbers = _numbers.Select(n => n.position < oldPosition && n.position >= newPosition ? (n.value, n.position +1) : n).ToList();
                }

                _numbers[n] = (_numbers[n].value, _numbers[n].position);
            }
        }

        private static int ComputeNewPosition((int value, int position) number)
        {
            int newPos = number.position + number.value;

            // List is circular: moving last number 1 step to the right goes to index 1 (not 0)
            newPos += newPos/_numbers.Count;

            // Wraps back to positive index
            if (newPos < 0) newPos -= (int)Math.Ceiling((double)newPos/_numbers.Count);

            // Stays within the list range
            newPos %= _numbers.Count;

            return newPos;
        }
    }
}