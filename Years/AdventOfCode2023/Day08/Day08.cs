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
                Match m = Regex.Match(s, @"(?<current>\w{3}) = \((?<left>\w{3}), (?<right>\w{3})\)");

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

            int step = 0;
            Node? currentNode = _nodes.First(n => n.Name == "AAA");
            while (currentNode != null && currentNode.Name != "ZZZ")
            {
                currentNode = _directions[step%_directions.Length] == 'L' ? currentNode.LeftNode : currentNode.RightNode;
                step++;
            }
            Console.WriteLine(step);
        }

        private static void AddNode (string name)
        {
            if (!_nodes.Any(n => n.Name == name)) _nodes.Add(new Node(name));
        }

        private static Node GetNode (string name) => _nodes.FirstOrDefault(n => n.Name == name) ?? new Node(name);
    }
}