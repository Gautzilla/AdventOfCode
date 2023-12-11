using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2023
{
    public static class Day11
    {
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day11\input.txt");

            input = input.Expand();

            List<(int x, int y)> galaxies = input
                .Select((content, y) => (content, y))
                .SelectMany(line => line.content
                    .Select((spotValue, x) => (spotValue, x , line.y)))
                .Where(point => point.spotValue == '#')
                .Select(point => (point.x, point.y))
                .ToList();

            int totalDistance = 0;
            for (int galaxy = 0; galaxy < galaxies.Count; galaxy++)
            {
                for (int galaxy2 = galaxy+1; galaxy2 < galaxies.Count; galaxy2++)
                {
                    totalDistance += ManhattanDistance(galaxies[galaxy], galaxies[galaxy2]);
                }
            }

            Console.WriteLine(totalDistance);
        }

        private static string[] Expand (this string[] input)
        {
            var expandedRowIndexes = Enumerable.Range(0, input.Length).Where(y => input[y].All(c => c!='#'));
            var expandedColumnIndexes = Enumerable.Range(0, input.First().Length).Where(x => input.All(i => i[x] != '#'));

            List<string> temp = [];
            for (int y = 0; y < input.First().Length; y++)
            {
                string inputLine = input[y];
                string expandedLine = "";
                for (int x = 0; x < inputLine.Length; x++)
                {
                    expandedLine += inputLine[x];
                    if (expandedColumnIndexes.Contains(x)) expandedLine += inputLine[x];
                }
                temp.Add(expandedLine);
                if (expandedRowIndexes.Contains(y)) temp.Add(expandedLine);
            }

            return [.. temp];
        }

        private static int ManhattanDistance ((int x, int y) c1, (int x, int y) c2) => Math.Abs(c2.x - c1.x) + Math.Abs(c2.y - c1.y);
    }
}