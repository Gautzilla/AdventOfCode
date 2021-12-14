using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day14
    {
        public static void Solve(int part)
        {
            //string filePath = @"..\..\Inputs\day14Example.txt";
            string filePath = @"..\..\Inputs\day14.txt";

            List<string> input = File.ReadAllLines(filePath).ToArray().ToList();

            Dictionary<string, long> pairsCount = CreateCountDictionary(input.Skip(2).ToList(), 0);
            Dictionary<string, long> singlesCount = CreateCountDictionary(input.Skip(2).ToList(), 1);

            Dictionary<string, string[]> pairsInsertions = CreatePairsInsertionsRules(input.Skip(2).ToList()); // Insertion rules: C inserted in NN creates NC and CN pairs
            Dictionary<string, string> singlesInsertions = CreateSinglesInsertionsRules(input.Skip(2).ToList()); // Insertion rules: C inserted in NN adds character C to the total

            int stepCount = part == 1 ? 10 : 40;

            for (int c = 0; c < input[0].Length; c++) // pairs and singles count initial population
            {
                singlesCount[input[0][c].ToString()]++;
                if (c < input[0].Length-1) pairsCount[new string(input[0].Skip(c).Take(2).ToArray())]++;
            }

            for (int step = 0; step < stepCount; step++)
            {
                Dictionary<string, long> newPairsCount = new Dictionary<string, long>(pairsCount);
                newPairsCount.ToList().ForEach(a => newPairsCount[a.Key] = 0);

                pairsCount.ToList().ForEach(a => {
                    singlesCount[singlesInsertions[a.Key]] += a.Value;
                    for (int i = 0; i <= 1; i++) newPairsCount[pairsInsertions[a.Key][i]] += a.Value;
                    });

                pairsCount = newPairsCount;
            }

            Console.WriteLine(singlesCount.ToList().OrderBy(a => a.Value).Last().Value - singlesCount.ToList().OrderBy(a => a.Value).First().Value);
        }

        private static Dictionary<string, long> CreateCountDictionary (List<string> input, int singlesOrPairs) // 1 for singles, 0 for pairs
        {
            Dictionary<string, long> pairsCount = new Dictionary<string, long>();

            foreach (string line in input) pairsCount[line.Split(' ').Where(s => s!="->").ToArray()[singlesOrPairs]] = 0;

            return pairsCount;
        }

        private static Dictionary<string, string[]> CreatePairsInsertionsRules(List<string> input)
        {
            Dictionary<string, string[]> pairsInsertions = new Dictionary<string, string[]>();

            foreach (string line in input)
            {
                string originalPair = line.Split(' ').Where(s => s != "->").ToArray()[0];
                string newChar = line.Split(' ').Where(s => s != "->").ToArray()[1];

                string[] newPairs = new string[] { originalPair[0] + newChar , newChar + originalPair[1]};

                pairsInsertions[originalPair] = newPairs;
            }
            return pairsInsertions;
        }

        private static Dictionary<string, string> CreateSinglesInsertionsRules(List<string> input)
        {
            Dictionary<string, string> singlesInsertions = new Dictionary<string, string>();

            foreach (string line in input) singlesInsertions[line.Split(' ').Where(s => s != "->").ToArray()[0]] = line.Split(' ').Where(s => s != "->").ToArray()[1];

            return singlesInsertions;
        }
    }
}
