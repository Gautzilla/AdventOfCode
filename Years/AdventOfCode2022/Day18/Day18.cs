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
        private static List<(int x, int y, int z)> cubesOfAir = new();
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day18\input.txt");

            (int x, int y, int z)[] cubes = input.Select(c => (int.Parse(c.Split(',')[0]), int.Parse(c.Split(',')[1]), int.Parse(c.Split(',')[2]))).ToArray();
            int faces = 0;
            foreach (var cube in cubes)
            {
                faces += 6 - ConnectedCubes(cube, cubes);
            }
            Console.WriteLine(faces);
        }

        private static (int x, int y, int z)[] _directions = 
        {
            (-1, 0, 0),
            (1, 0, 0),
            (0, -1, 0),
            (0, 1, 0),
            (0, 0, -1),
            (0, 0, 1)
        };

        private static int ConnectedCubes((int x, int y, int z) cube, (int x, int y, int z)[] cubes)
        {
            return (_directions.Where(d => cubes.Contains((cube.x + d.x, cube.y + d.y, cube.z + d.z))).Count());
        }
    }
}