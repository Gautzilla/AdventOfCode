using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2023
{
    public static class Day10
    {   
        private static readonly Dictionary<char, bool[]> _connections = new () {
        {'|', [true, false, true, false]},        
        {'-', [false, true, false, true]},
        {'L', [true, true, false, false]},
        {'J', [true, false, false, true]},
        {'7', [false, false, true, true]},
        {'F', [false, true, true, false]},
        {'.', [false, false, false, false]},
        {'S', [true, true, true, true]}
        };

        private static readonly (int x, int y)[] _directions = [(0,-1),(1,0),(0,1),(-1,0)];
        
        private record Pipe
        {         
            public (int x, int y) Coordinates { get; set; }
            public char Char { get; set; }
            public bool[] Connections { get; set; }
            public bool IsStartingPosition { get; set; }

            public Pipe((int x, int y) coordinates, char c)
            {
                Coordinates = coordinates;
                Char = c;
                Connections = _connections[c];
                IsStartingPosition = c == 'S';
            }
        }

        private static Pipe[][] _pipes = [[]];

        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day10\input.txt");

            _pipes = input.Select((line, y) => line.Select((c, x) => new Pipe((x,y), c)).ToArray()).ToArray();

            Pipe startingPipe = _pipes.Single(l => l.Any(p => p.IsStartingPosition)).Single(p => p.IsStartingPosition);
            Console.WriteLine(LoopLength(startingPipe)/2);
        }

        private static int LoopLength (Pipe current)
        {
            bool hasLooped = false;
            bool lastPipeIsStartingPipe = false;
            HashSet<Pipe> visited = [];

            while (!hasLooped)
            {
                for (int d = 0; d < _directions.Length; d++)
                {
                    if(!current.Connections[d]) continue;

                    var direction = _directions[d];
                    
                    (int x, int y) nextCoords = (current.Coordinates.x + direction.x, current.Coordinates.y + direction.y);
                    
                    if (!IsInsideBounds(nextCoords.x, nextCoords.y)) continue;

                    Pipe next = _pipes[nextCoords.y][nextCoords.x];

                    if (!next.Connections[(d + 2)%_directions.Length]) continue; // next pipe isn't connected to current pipe
                    
                    if (visited.Contains(next)) continue;

                    if (next.IsStartingPosition && lastPipeIsStartingPipe) continue; // We don't want to go back after the first step

                    if (!current.IsStartingPosition) visited.Add(current);
                    lastPipeIsStartingPipe = current.IsStartingPosition;
                    current = next;

                    if (current.IsStartingPosition) hasLooped = true;
                }
            }

            return visited.Count + 1;
        }

        private static bool IsInsideBounds (int x, int y) => x >= 0 && y >= 0 && x < _pipes.First().Length && y < _pipes.Length;
    }
}