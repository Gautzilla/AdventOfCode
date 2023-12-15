using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public static class Day15
    {

        private const int _nbBoxes = 256;
        private static List<(string label, int focalLength)>[] _boxes = [];
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllText(@"C:\Users\Gauthier\source\repos\AdventOfCode\Years\AdventOfCode2023\Day15\input.txt").Split(',');

            if (part == 1) Console.WriteLine(input.Sum(HASH));
            else
            {
                _boxes = Enumerable.Range(0, _nbBoxes).Select(i => new List<(string label, int focalLength)>()).ToArray();
                foreach (var instruction in input) ProcessInstruction(instruction);
                Console.WriteLine(_boxes.HASHMAP());
            }
        }
        
        private static int HASH (this string s) => s.Aggregate(0, (a, b) => ((a + b) * 17) % 256);

        private static void ProcessInstruction(string instruction)
        {
            string regex = @"(?<label>\w+)(?<operation>[=-])(?<focalLength>\d+)?";
            Match match = Regex.Match(instruction, regex);

            string label = match.Groups["label"].Value;            
            int boxIndex = label.HASH();
            string operation = match.Groups["operation"].Value;

            if (operation == "=")
            {
                int focalLength = int.Parse(match.Groups["focalLength"].Value);
                AddLense(boxIndex, label, focalLength);
            } else
            {
                RemoveLense(boxIndex, label);
            }
        }

        private static void RemoveLense (int boxIndex, string label)
        {
            _boxes[boxIndex] = _boxes[boxIndex].Where(lense => lense.label != label).ToList();
        }

        private static void AddLense (int boxIndex, string label, int focalLength)
        {
            var box = _boxes[boxIndex];
            
            if (!box.Any(lense => lense.label == label))
            {
                box.Add((label, focalLength));
                return;
            }
            
            int indexLenseToReplace = box.IndexOf(box.First(lense => lense.label == label));
            box[indexLenseToReplace] = (label, focalLength);
        }

        private static int HASHMAP (this List<(string label, int focalLength)>[] boxes) => boxes
            .Select((content, boxIndex) => (boxIndex + 1) * content
                .Select((lense, slotINdex) => (slotINdex + 1) * lense.focalLength)
                .Sum())
            .Sum();
    }
}