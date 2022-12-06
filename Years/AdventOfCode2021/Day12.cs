using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day12
    {
        private class Cave
        {
            public string name { get; }
            public bool isLarge { get; }
            public List<string> connectedCaves { get; }

            public Cave(string name)
            {
                this.name = name;
                isLarge = Char.IsUpper(name[0]);
                connectedCaves = new List<string>();
            }

            public void AddConnectedCave(string caveName)
            {
                if (!connectedCaves.Contains(caveName)) connectedCaves.Add(caveName);
            }
        }

        private static List<Cave> caves;

        public static void Solve(int part)
        {
            //string filePath = @"..\..\Inputs\day12Example.txt";
            string filePath = @"..\..\Inputs\day12.txt";

            List<string> input = File.ReadAllLines(filePath).ToArray().ToList();

            CreateCaves(input);

            bool isPart2 = part == 2;

            int paths = ComputePaths(caves.First(c => c.name == "start"), new List<string>() { "start" }, isPart2, false);

            Console.WriteLine($"There are {paths} paths.");
        }

        private static void CreateCaves(List<string> input)
        {
            caves = new List<Cave>();

            foreach (string line in input)
            {
                string[] connection = line.Split('-');
                for (int i = 0; i < 2; i++)
                {
                    if (!caves.Any(c => c.name == connection[i])) caves.Add(new Cave(connection[i]));

                    foreach (Cave cave in caves)
                    {
                        if (cave.name == connection[i])
                        {
                            cave.AddConnectedCave(connection[i == 0 ? 1 : 0]);
                        }
                    }
                }
            }
        }

        private static int ComputePaths(Cave currentCave, List<string> visitedCaves, bool isPart2, bool smallCaveVisitedTwice)
        {
            int pathCount = 0;

            foreach (string connectedCave in currentCave.connectedCaves)
            {
                if (connectedCave == "end")
                {
                    pathCount++;
                }
                else
                {
                    if (!visitedCaves.Contains(connectedCave) || Char.IsUpper(connectedCave[0]))
                    {
                        pathCount += ComputePaths(caves.First(c => c.name == connectedCave), visitedCaves.Append(connectedCave).ToList(), isPart2, smallCaveVisitedTwice);
                    }
                    else if (isPart2 && connectedCave != "start" && !smallCaveVisitedTwice && visitedCaves.Contains(connectedCave) && Char.IsLower(connectedCave[0]))
                    {
                        pathCount += ComputePaths(caves.First(c => c.name == connectedCave), visitedCaves.Append(connectedCave).ToList(), isPart2, true);
                    }
                }
            }
            return pathCount;
        }
    }
}
