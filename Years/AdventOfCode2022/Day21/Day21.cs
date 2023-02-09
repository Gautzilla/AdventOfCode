using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day21
    {
        class Monkey
        {
            public (Monkey first, Monkey second)? Operands { get; set; }
            public Func<long, long, long>? Operation { get; set; }
            public string Name { get; set; }
            public long? Value { get; set; }

            public Monkey(string name)
            {
                Name = name;
                Value = null;
            }

            public void SetMathOperation (char operationSymbol, Monkey firstMonkey, Monkey secondMonkey)
            {
                Operation = operationSymbol switch
                {
                    '+' => ((long a, long b) => a+b),
                    '-' => ((long a, long b) => a-b),
                    '*' => ((long a, long b) => a*b),
                    '/' => ((long a, long b) => a/b), // MAYBE SOME FLOAT ERROR HERE
                    _ => throw new NotImplementedException()
                };
                Operands = (firstMonkey, secondMonkey);
            }

            public void ComputeValue()
            {
                if (!Operands.HasValue || Operation == null) return;                
                if (Operands.Value.first.Value == null || Operands.Value.second.Value == null) return;

                Value = Operation(Operands.Value.first.Value.Value, Operands.Value.second.Value.Value);

                if (Name == "root") Console.WriteLine(Value);

                foreach (var monkey in _monkeys.Where(m => m.Value == null && m.Operands != null && (m.Operands.Value.first == this || m.Operands.Value.second == this)))
                monkey.ComputeValue();
            }

            public void Yell()
            {
                if (Value == null) return;

                foreach (var monkey in _monkeys.Where(m => m.Value == null && m.Operands != null && (m.Operands.Value.first == this || m.Operands.Value.second == this)))
                monkey.ComputeValue();
            }
        }

        private static HashSet<Monkey> _monkeys = new();

        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day21\input.txt");
            
            foreach (var line in input) ParseMonkey(line);
            
            Yell();
        }

        private static void ParseMonkey(string input)
        {
            var inputs = input.Split(' ').Select(part => part.Trim(':')).ToArray();

            string monkeyName = inputs.First();

            Monkey monkey = FindMonkey(monkeyName);
            
            if (inputs.Count() == 2)
            {
                monkey.Value = int.Parse(inputs.Last());
                return;
            }

            Monkey firstMonkey = FindMonkey(inputs[1]);
            Monkey secondMonkey = FindMonkey(inputs[3]);
            char operationSymbol = inputs[2].First();

            monkey.SetMathOperation(operationSymbol, firstMonkey, secondMonkey);
        }

        private static Monkey FindMonkey (string name)
        {
            if (_monkeys.Any(m => m.Name == name)) return _monkeys.Single(m => m.Name == name);

            Monkey monkey = new Monkey(name);
            _monkeys.Add(monkey);
            return monkey;
        }

        private static void Yell()
        {
            foreach (var monkey in _monkeys.Where(m => m.Value != null))
            {
                monkey.Yell();
            }
        }
    }
}