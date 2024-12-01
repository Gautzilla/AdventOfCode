using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace AdventOfCode2015
{
    public static class Day6
    {
        private static bool[][] _lights = new bool[1000][];
        private static int[][] _dimmableLights = new int[1000][];
        public static void Solve(int part)
        { 
            for (int row = 0; row < _lights.Length; row++)
            {
                _lights[row] = new bool[1000];
                _dimmableLights[row] = new int[1000];
            }


            string[] input = File.ReadAllLines(@"Day6\input.txt");
            
            foreach (string task in input) 
            {
                ProcessInstruction(task, part);
            }

            Console.WriteLine(part == 1 ? CountLitLights() : CountDimmableLightsLuminosity());
        }

        private static void ProcessInstruction(string task, int part)
        {
            string regex = @"(?<instruction>\D+)(?<upperLeftCorner>\d+,\d+) through (?<lowerRightCorner>\d+,\d+)";

            Match match = Regex.Match(task, regex);

            (int x, int y) upperLeftCorner = GetCoordinates(match.Groups["upperLeftCorner"].Value);
            (int x, int y) lowerRightCorner = GetCoordinates(match.Groups["lowerRightCorner"].Value);
            string instruction = match.Groups["instruction"].Value.Trim();

            for (int y = upperLeftCorner.y; y <= lowerRightCorner.y; y++)
            {
                for (int x = upperLeftCorner.x; x <= lowerRightCorner.x; x++)
                {
                    if (part == 1) SwitchLight(x, y ,instruction);
                    else DimLight(x, y, instruction);
                }
            }
        }

        private static (int x, int y) GetCoordinates(string corner)
        {
            var coordinates = corner.Split(',').Select(s => int.Parse(s));
            return (coordinates.First(), coordinates.Last());
        }

        private static void SwitchLight(int x, int y, string instruction) => _lights[y][x] = instruction switch
        {
            "turn on" => true,
            "turn off" => false,
            "toggle" => !_lights[y][x],
            _ => throw new NotImplementedException()
        };

        private static void DimLight(int x, int y, string instruction) => _dimmableLights[y][x] += instruction switch
        {
            "turn on" => 1,
            "turn off" => _dimmableLights[y][x] > 0 ? -1 : 0,
            "toggle" => 2,
            _ => throw new NotImplementedException()
        };

        private static int CountLitLights() => _lights.Select(row => row.Count(light => light)).Sum();
        private static int CountDimmableLightsLuminosity() => _dimmableLights.Select(row => row.Sum()).Sum();
    }
}