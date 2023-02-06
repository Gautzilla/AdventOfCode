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
            Console.WriteLine(GroveCoordinates());
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

                if (newPosition > oldPosition)
                {
                    _numbers = _numbers.Select(n => n.position > oldPosition && n.position <= newPosition ? (n.value, n.position -1) : n).ToList();
                }

                if (newPosition < oldPosition)
                {
                    _numbers = _numbers.Select(n => n.position < oldPosition && n.position >= newPosition ? (n.value, n.position +1) : n).ToList();
                }

                _numbers[n] = (_numbers[n].value, newPosition);
            }
        }

        private static int ComputeNewPosition((int value, int position) number)
        {
            int newPos = number.position + number.value;
            
            // We work on a _numbers.Count - 1 ring since the current number moves (its space is not left empty)

            // Wraps back to positive index
            if (newPos < 0) newPos += ((int)(Math.Abs(newPos)/(_numbers.Count - 1))+1)*(_numbers.Count-1);
            
            // Stays within the list range
            newPos %= (_numbers.Count-1);
            
            return newPos;
        }

        private static int GroveCoordinates()
        {
            int indexOfZero = _numbers.Single(n => n.value == 0).position;

            return new int[] {1000, 2000, 3000}
            .Select(n => (n+indexOfZero)%_numbers.Count)
            .Select(i => _numbers.Single(n => n.position == i).value)
            .Sum();
        }
    }
}