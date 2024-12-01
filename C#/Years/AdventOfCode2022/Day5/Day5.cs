using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day5
    {
        private static List<Stack<string>> _stacks = new();
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day5\input.txt");

            InitializeStacks(input);
            PopulateStacks(input.TakeWhile(line => line.Contains('[')).ToArray());
            MoveCrates(input.SkipWhile(line => !line.Contains("move")).ToArray(), part);
        
            foreach (var stack in _stacks) if (stack.Count > 0) Console.Write(stack.Pop());
        }

        private static void InitializeStacks(string[] input)
        {
            int numberOfStacks = int.Parse(input.First(line => !line.Contains('[')).Split(" ").Where(num => num.Length > 0).Last());
            for (int i = 0; i < numberOfStacks; i++) _stacks.Add(new Stack<string>());
        }

        private static void PopulateStacks(string[] input)
        {
            string[] parsedStacks = new string[_stacks.Count];

            foreach (string line in input)
            {
                char[][] stacks = line.Chunk(4).ToArray();

                for (int stack = 0; stack < stacks.Length; stack++)
                {
                    if (stacks[stack].Any(char.IsLetter))
                    {
                        parsedStacks[stack] += stacks[stack].Single(char.IsLetter).ToString(); // Parsing is FIFO
                    }
                }
            }

            parsedStacks = parsedStacks.Select(stack => string.Join("", stack.Reverse())).ToArray(); // Stacks are LIFO

            for (int stack = 0; stack < parsedStacks.Length; stack++)
            {
                for (int crate = 0; crate < parsedStacks[stack].Length; crate ++)
                {
                    _stacks[stack].Push(parsedStacks[stack][crate].ToString()); 
                }
            }
        }

        private static void MoveCrates(string[] instructions, int part)
        {
            foreach (string instruction in instructions)
            {
                Match instructionInfos = Regex.Match(instruction, @"move (?<amount>\d+) from (?<fromStack>\d+) to (?<toStack>\d+)");
                int amount = int.Parse(instructionInfos.Groups["amount"].Value);
                int fromStack = int.Parse(instructionInfos.Groups["fromStack"].Value) - 1;
                int toStack = int.Parse(instructionInfos.Groups["toStack"].Value) - 1;

                if (part == 1)
                {
                    for (int i = 0; i < amount; i++)
                    {
                        if (_stacks[fromStack].TryPop(out string? crate)) _stacks[toStack].Push(crate);
                    }
                } else
                {
                    Stack<string> movedCrates = new();
                    for (int i = 0; i < amount; i++)
                    {
                        if (_stacks[fromStack].TryPop(out string? crate)) movedCrates.Push(crate);
                    }
                    for (int i = 0; i < amount; i++)
                    {
                        if (movedCrates.TryPop(out string? crate)) _stacks[toStack].Push(crate);
                    }
                }
            }
        }
    }
}