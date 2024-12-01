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
        private static List<(long value, int position)> _numbers = new();
        private static readonly int _decryptionKey = 811589153;
        public static void Solve(int part)
        { 
            ParseInput();
            if (part == 2) _numbers = _numbers.Select(n => (n.value * _decryptionKey, n.position)).ToList();
            
            int numberOfMixes = part == 1 ? 1 : 10;
            for (int mix = 0; mix < numberOfMixes; mix++) MoveNumbers();

            Console.WriteLine(GroveCoordinates());
        }

        private static void ParseInput()
        {
            string[] input = File.ReadAllLines(@"Day20\input.txt");
            _numbers = input.Select((val,pos) => (long.Parse(val), pos)).ToList();
        }

        private static void MoveNumbers()
        {
            for (int n = 0; n < _numbers.Count; n++)
            {
                int oldPosition = _numbers[n].position;
                int newPosition = ComputeNewPosition(_numbers[n]);

                _numbers = _numbers
                .Select(n => IsInsidePositions(n.position, oldPosition, newPosition) ? (n.value, n.position + (newPosition < oldPosition ? 1 : -1)) : n)
                .ToList();

                _numbers[n] = (_numbers[n].value, newPosition);
            }
        }

        private static int ComputeNewPosition((long value, int position) number)
        {
            long newPos = number.position + number.value;
            
            // We work on a _numbers.Count - 1 ring since the current number moves (its space is not left empty)

            // Wraps back to positive index
            if (newPos < 0) newPos += ((long)(Math.Abs(newPos)/(_numbers.Count - 1))+1)*(_numbers.Count-1);
            
            // Stays within the list range
            newPos %= (_numbers.Count-1);
            
            return (int)newPos;
        }

        private static long GroveCoordinates()
        {
            int indexOfZero = _numbers.Single(n => n.value == 0).position;

            return new int[] {1000, 2000, 3000}
            .Select(n => (n+indexOfZero)%_numbers.Count)
            .Select(i => _numbers.Single(n => n.position == i).value)
            .Sum();
        }

        private static bool IsInsidePositions(int position, int oldPosition, int newPosition) => position >= Math.Min(newPosition, oldPosition) && position <= Math.Max(newPosition, oldPosition);
    }
}