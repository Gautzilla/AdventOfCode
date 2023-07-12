using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2015
{
    public static class Day7
    {
        private static Dictionary<string, long> _wires = new();
        private static Queue<string> _waitingInstructions = new();
        private static List<string> input = new();
        public static void Solve(int part)
        { 
            input = File.ReadAllLines(@"Day7\input.txt").ToList();

            string part1 = ProcessInstructions();

            Console.WriteLine($"Part 1: {ProcessInstructions()}");
        }

        private static string ProcessInstructions()
        {
            foreach(string line in input) _waitingInstructions.Enqueue(line);

            while (!_wires.ContainsKey("a") && _waitingInstructions.Count > 0)
            {
                string instruction = _waitingInstructions.Dequeue();
                (string wire, long value)? lineResult = ProcessLine(instruction);

                if (lineResult.HasValue)
                {
                    _wires.Add(lineResult.Value.wire, lineResult.Value.value);
                    continue;
                }
                _waitingInstructions.Enqueue(instruction);
            }

            return _wires.ContainsKey("a") ? _wires["a"].ToString() : "There is no wire named a.";
        }

        private static (string wire, long value)? ProcessLine(string line)
        {
            string[] operation = line.Split(" -> ").First().Split(" ");
            string destinationWire = line.Split(" -> ").Last().Trim();

            if (operation.Length == 1)
            {
                // DIRECT VALUE ASSIGNMENT
                if (long.TryParse(operation[0], out long value)) return (destinationWire,value);

                // WIRE VALUE ASSIGNMENT
                if (_wires.ContainsKey(operation[0])) return (destinationWire,_wires[operation[0]]);

                return null;
            }

            // NOT
            if (operation[0] == "NOT")
            {
                string inputValue = operation[1];
                return(_wires.ContainsKey(inputValue) ? (destinationWire,~_wires[inputValue]) : null);
            }         

            if (! (long.TryParse(operation[0], out long value1) || _wires.ContainsKey(operation[0]))) return null;
            if (! (long.TryParse(operation[2], out long value2) || _wires.ContainsKey(operation[2]))) return null;

            long inputValue1 = _wires.ContainsKey(operation[0]) ? _wires[operation[0]] : value1;
            long inputValue2 = _wires.ContainsKey(operation[2]) ? _wires[operation[2]] : value2;

            return operation[1] switch
            {
                "LSHIFT" => (destinationWire,(inputValue1 << (int)inputValue2)),
                "RSHIFT" => (destinationWire,(inputValue1 >> (int)inputValue2)),
                "AND" => (destinationWire,inputValue1 & inputValue2),
                "OR" => (destinationWire,inputValue1 | inputValue2),
                _ => throw new NotImplementedException()
            };          
        }
    }
}