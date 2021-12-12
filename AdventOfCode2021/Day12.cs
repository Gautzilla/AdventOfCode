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
            caves = new List<Cave>();

            List<List<string>> paths = new List<List<string>>();
            List<List<string>> completePaths = new List<List<string>>();

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

            foreach (string startingCave in caves.First(c => c.name == "start").connectedCaves)
            {
                paths.Add(new List<string>() { "start", startingCave });
            }


            while (paths.Count() > 0)
            {
                List<List<string>> nextPaths = new List<List<string>>();

                foreach (List<string> path in paths)
                {
                    if (path.Last() == "end") completePaths.Add(new List<string>(path));
                    else if (path.Last() != "start") {
                        foreach (string connectedCave in caves.First(c => c.name == path.Last()).connectedCaves)
                        {
                        if (caves.First(c => c.name == connectedCave).isLarge || ( !path.Contains(connectedCave) || part == 2 && CanVisitSmallCaveAgain(path)) )
                            {
                                nextPaths.Add(new List<string>(path));
                                nextPaths.Last().Add(connectedCave);
                            }
                        }
                    }
                }
                paths = nextPaths;
            }

            Console.WriteLine($"There are {completePaths.Count()} paths.");
        }

        private static bool CanVisitSmallCaveAgain (List<string> path)
        {
            return (!path.Where(c => !caves.First(d => d.name == c).isLarge).GroupBy(c => c).Any(g => g.Count() > 1));
        }
    }
}
