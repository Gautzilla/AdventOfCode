using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day18
    {
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day18\input.txt");

            int[][] cubes = input.Select(c =>c.Split(',').Select(i => int.Parse(i)).ToArray()).ToArray();
            int faces = 0;
            foreach (var cube in cubes)
            {
                faces += 6 - cubes.Count(c => CubesAreConnected(cube, c));
            }
            Console.WriteLine(faces);
        }

        private static bool CubesAreConnected(int[] cubeA, int[] cubeB)
        {
            if (cubeA.Where((val, coord) => cubeB[coord] == val).Count() != 2) return false;
            return (cubeA.Select((val, coord) => Math.Abs(cubeB[coord] - val)).Sum() == 1);
        }
    }
}