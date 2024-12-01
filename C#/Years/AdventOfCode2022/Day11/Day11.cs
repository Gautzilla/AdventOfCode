using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day11
    {
        private static List<Monkey> _monkeys = new();
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day11\input.txt");

            int lcm = LCM(input.Where(i => i.Contains("Test:")).Select(i => int.Parse(i.Split(" ").Last())).ToList());

            foreach (var monkey in input.Chunk(7)) _monkeys.Add(new Monkey(monkey, lcm, part));
            
            for (int round = 1; round <= (part == 1 ? 20 : 10000); round++)
            {
                foreach (Monkey monkey in _monkeys)
                {
                    while (monkey.Items.Count > 0)
                    {
                        var throwedItem = monkey.InspectItem();
                        _monkeys.First(m => m.ID == throwedItem.monkey).Items.Enqueue(throwedItem.item);
                    }
                }
            }
            Console.WriteLine(_monkeys.Select(m => m.InspectedItems).OrderByDescending(iI => iI).Take(2).Aggregate((a,b) => a*b));
        }

        private static int LCM (List<int> numbers) // Really not optimized!!
        {
            int output = numbers.Min();
            while (true)
            {
                if (numbers.All(n => output%n == 0)) break;
                output++;
            }
            return output;
        }
    }
}