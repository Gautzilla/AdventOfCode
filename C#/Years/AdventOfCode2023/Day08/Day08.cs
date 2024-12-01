using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public static class Day08
    {
        private record Node
        {
            public string Name { get; }
            public Node? LeftNode { get; set; }
            public Node? RightNode { get; set; }

            public Node (string name)
            {
                Name = name;
            }
        }
        private static string _directions = string.Empty;
        private static HashSet<Node> _nodes = new();
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day08\input.txt");

            _directions = input.First();

            foreach (var s in input.Skip(2))
            {
                Match m = Regex.Match(s, @"(?<current>[\w\d]{3}) = \((?<left>[\w\d]{3}), (?<right>[\w\d]{3})\)");

                string nodeName = m.Groups["current"].Value;
                string leftName = m.Groups["left"].Value;
                string rightName = m.Groups["right"].Value;

                AddNode(nodeName);
                AddNode(leftName);
                AddNode(rightName);
                
                Node current = GetNode(nodeName);                
                current.LeftNode = GetNode(leftName);
                current.RightNode = GetNode(rightName);
            }

            
            if (part == 1) Console.WriteLine(Part1());
            else Console.WriteLine(LCM(_nodes.Where(n => n.Name.EndsWith('A')).Select(node => Part2(node))));
        }

        private static int Part1()
        {
            int step = 0;
            Node? currentNode = _nodes.First(n => n.Name == "AAA");
            while (currentNode != null && currentNode.Name != "ZZZ")
            {
                currentNode = _directions[step%_directions.Length] == 'L' ? currentNode.LeftNode : currentNode.RightNode;
                step++;
            }
            return step;
        }

        private static long Part2(Node node)
        {
            int step = 0;
            while (!node.Name.EndsWith('Z'))
            {
                node = (_directions[step%_directions.Length] == 'L' ? node.LeftNode : node.RightNode)!;
                step++;
            }
            return step;
        }

        private static void AddNode (string name)
        {
            if (_nodes.Any(n => n.Name == name)) return;
            _nodes.Add(new Node(name));
        }

        private static Node GetNode (string name) => _nodes.FirstOrDefault(n => n.Name == name) ?? new Node(name);
    
        static long GCD(long n1, long n2)
        {
            if (n2 == 0) return n1;
            return GCD(n2, n1 % n2);            
        }

        public static long LCM(IEnumerable<long> numbers) => numbers.Aggregate((S, val) => S * val / GCD(S, val));
    
    }
}