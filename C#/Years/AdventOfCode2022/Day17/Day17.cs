using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace AdventOfCode2022
{
    public static class Day17
    {
        class State
        {
            public int Jet { get; set; }
            public int RockShape { get; set; }
            public string Rocks { get; set; }
            public int HeightGain { get; set; }

            public State(int jet, int rockShape, HashSet<(int x, int y)> rocks)
            {
                Jet = jet;
                RockShape = rockShape;
                if (rocks.Count == 0) {Rocks = ""; return;}
                int stateHeight = rocks.Max(r => r.y) - rocks.Min(r => r.y) +1;
                Rocks = String.Join("\n", Enumerable.Range(rocks.Min(r => r.y), stateHeight).Select(y => String.Join("",Enumerable.Range(1, 7).Select(x => rocks.Contains((x,y)) ? '#' : '.'))));
            }

            public bool IsSameState(State other)
            {
                return Jet == other.Jet && RockShape == other.RockShape && Rocks == other.Rocks;
            }
        }

        private static readonly (int x, int y)[][] _rocks = { // rocks origin is at the bottom left corner
            new (int, int)[] {(0,0), (1,0), (2,0), (3,0)}, // _
            new (int, int)[] {(1,0), (0,-1), (1,-1), (2,-1), (1,-2)}, // +
            new (int, int)[] {(0,0), (1,0), (2,0), (2,-1), (2,-2)}, // J
            new (int, int)[] {(0,0), (0,-1), (0,-2), (0,-3)}, // |
            new (int, int)[] {(0,0), (0,-1), (1,0), (1,-1)}, // #
            };

        private static HashSet<(int x, int y)> _occupied = new();
        private static readonly string _jetPattern = File.ReadAllText(@"Day17\input.txt");
        private static long _height = 0;
        private static int _jetState = 0; 
        private static int _fallingRockIndex;
        private static List<State> _states = new(); // Checks for repetition
        private static List<int> pattern = new();
        public static void Solve(int part)
        { 
            (int x, int y) startingPos = (3, -4);

            long numberOfRocks = part == 1 ? 2022 : 1000000000000;
            long indexOfPattern = 0;

            int nLines = 100; //Number of lines in buffer
            
            for (long rock = 0; rock < numberOfRocks; rock++)
            {
                if (_occupied.Count > 0) startingPos.y = _occupied.Min(o => o.y) - 4;

                (int x, int y)[] fallingRock = _rocks[rock%_rocks.Length].Select(p => (p.x + startingPos.x, p.y + startingPos.y)).ToArray();
               
                _fallingRockIndex = (int)(rock % _rocks.Length);

                // Check for pattern
                State currentState = new (_jetState, _fallingRockIndex, _occupied);
                if (pattern.Count == 0 && _states.Any(s => s.IsSameState(currentState)))
                {
                    State firstOfPattern = _states.Single(s => s.IsSameState(currentState));
                    pattern = _states.Skip(_states.IndexOf(firstOfPattern)).Select(s => s.HeightGain).ToList();
                    indexOfPattern = rock;
                    _height = Math.Abs(_height - nLines); // Stores the height of the top rock line
                }

                if (pattern.Count == 0) 
                {
                    _states.Add(currentState);
                    int prevHeight = _occupied.Count > 0 ? _occupied.Min(o => o.y) : 0;
                    Fall(fallingRock);
                    currentState.HeightGain = prevHeight - _occupied.Min(o => o.y);
                    RemoveBottom(nLines);
                }
                else 
                {
                    // Shortcuts the rest of the rocks on the basis of the pattern
                    long completePatterns = (numberOfRocks - rock)/pattern.Count()*pattern.Sum();
                    long remainingRocks = pattern.Take((int)((numberOfRocks - rock)%pattern.Count())).Sum();
                    _height += completePatterns + remainingRocks;
                    break;
                }                
            }
            
            if (pattern.Count == 0) Console.WriteLine(Math.Abs(_height + _occupied.Min(o => o.y)));
            else Console.WriteLine(_height);
        }

        private static void Fall((int x, int y)[] rock)
        {
            // jet pushes the rock
            int jet = _jetPattern[_jetState] == '>' ? 1 : -1;
            _jetState = ++_jetState % _jetPattern.Length;
            
            if (rock.Min(p => p.x) + jet >= 1 && rock.Max(p => p.x) + jet <= 7 && !rock.Any(p => _occupied.Contains((p.x + jet, p.y)))) rock = rock.Select(p => (p.x + jet, p.y)).ToArray();

            // rock falls down
            if (rock.Any(p => p.y == -1) || rock.Any(p => _occupied.Contains((p.x, p.y+1))))
            {
                _occupied = _occupied.Concat(rock).ToHashSet();
                return;
            }

            rock = rock.Select(p => (p.x, p.y + 1)).ToArray();
            Fall(rock);
        }

        private static void RemoveBottom(int nLines)
        {
            if (_occupied.Min(o => o.y) >= -nLines) return;
            
            int delta = _occupied.Min(o => o.y) + nLines;
            _occupied.RemoveWhere(o => o.y > delta);
            _occupied = _occupied.Select(o => (o.x, o.y -= delta)).ToHashSet();
            _height += delta;            
        }
    }
}