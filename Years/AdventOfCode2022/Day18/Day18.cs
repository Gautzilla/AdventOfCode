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
        private static (int x, int y, int z)[] _lavaCubes = new(int x, int y, int z)[0];
        private static (int x, int y, int z) _gridSize;
        private static HashSet<(int x, int y, int z)> _cubesToVisit = new();
        private static Stack<(int x, int y, int z)> _cubeStack = new();
        private static int output = 0;
        private static (int x, int y, int z)[] _directions = 
        {
            (-1, 0, 0),
            (1, 0, 0),
            (0, -1, 0),
            (0, 1, 0),
            (0, 0, -1),
            (0, 0, 1)
        };
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day18\input.txt");

            _lavaCubes = input.Select(c => (int.Parse(c.Split(',')[0]), int.Parse(c.Split(',')[1]), int.Parse(c.Split(',')[2]))).ToArray();
            
            _gridSize = (_lavaCubes.Max(cube => cube.x) + 1, _lavaCubes.Max(cube => cube.y) + 1, _lavaCubes.Max(cube => cube.z) + 1);

            for (int x = -1; x <= _gridSize.x; x++)
            {
                for (int y = -1; y <= _gridSize.y; y++)
                {
                    for (int z = -1; z <= _gridSize.z; z++)
                    {
                        if (!_lavaCubes.Contains((x,y,z))) _cubesToVisit.Add((x,y,z));
                    }
                }
            }            

                        
            while (_cubeStack.Count > 0 || _cubesToVisit.Count > 0)
            {
                // Stop if all remaining cubes are trapped air cubes
                if (part == 2 && _cubeStack.Count == 0 && !_cubesToVisit.Any(c => IsOnBorder(c))) break;

                // Add remaining unexplored cubes to the stack
                if (_cubeStack.Count == 0)
                {
                    var nextCube = part == 1 ? _cubesToVisit.First() : _cubesToVisit.First(c => IsOnBorder(c));
                    _cubeStack.Push(nextCube);
                    _cubesToVisit.Remove(nextCube);
                }

                BFS();
            }

            Console.WriteLine(output);
        }

        private static void BFS()
        {
            var air = _cubeStack.Pop();

            foreach (var dir in _directions)
            {
                var neighbour = (air.x + dir.x, air.y + dir.y, air.z + dir.z);

                if (_lavaCubes.Contains(neighbour)) output++;

                else if (_cubesToVisit.Contains(neighbour))
                {
                    _cubeStack.Push(neighbour);
                    _cubesToVisit.RemoveWhere(c => c == neighbour);
                }
            }
        }

        private static bool IsOnBorder((int x, int y, int z) cube) => cube.x == 0 || cube.x == _gridSize.x - 1 || cube.y == 0 || cube.y == _gridSize.y - 1 || cube.z == 0 || cube.z == _gridSize.z - 1;
    }
}