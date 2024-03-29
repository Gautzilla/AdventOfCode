using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day16
    {
        private static HashSet<Valve> _valves = new();
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day16\input.txt");

            CreateValves(input);
            foreach (Valve valve in _valves) valve.ComputeDistances(_valves);
            
            if (part == 1) Console.WriteLine(BestMove(_valves.Single(v => v.Name == "AA"), new(), 0, 0, 30, true));        
            else
            {
                Console.WriteLine(BestMove(_valves.Single(v => v.Name == "AA"), new(), 0, 0, 26, false));        
            }
        }

        private static void CreateValves(string[] input)
        {
            Dictionary<string, string> tunnelsDic = new();

            foreach (var line in input)
            {
                Match m = Regex.Match(line, @"Valve (?<valve>[A-Z]+) has flow rate=(?<flowRate>\d+); tunnels? leads? to valves? (?<tunnels>.+)");


                string valveName = m.Groups["valve"].Value;                
                int flowRate = int.Parse(m.Groups["flowRate"].Value);

                _valves.Add(new Valve(valveName, flowRate));
                tunnelsDic.Add(m.Groups["valve"].Value, m.Groups["tunnels"].Value);
            }

            foreach (var kvp in tunnelsDic)
            {
                Valve valve = _valves.Single(v => v.Name == kvp.Key);
                Valve[] tunnels = kvp.Value.Split(", ").Select(s => _valves.Single(v => v.Name == s)).ToArray();
                foreach (Valve tunnel in tunnels) valve.AddTunnel(tunnel);
            }
        }

        private static int BestMove(Valve current, HashSet<Valve> openedValves, int releasedPresure, int flowRate, int remainingTime, bool isLastRun)
        {
            //OPEN (not for AA if its flowRate == 0)
            if (current.FlowRate > 0)
            {
                openedValves = openedValves.Append(current).ToHashSet();
                releasedPresure += flowRate;
                flowRate += current.FlowRate;
                remainingTime--;

                // WAIT IF NO MORE REACHABLE VALVE
                if (current.ValveDistance.Where(v => v.Value < remainingTime).All(v => openedValves.Contains(v.Key))) 
                {
                    return releasedPresure + remainingTime * flowRate + (isLastRun ? 0 : BestMove(_valves.Single(v => v.Name == "AA"), new(openedValves), 0, 0, 26, true));
                }
            }

            //MOVE
            int maxPresure = releasedPresure;
            foreach (var tunnel in current.ValveDistance.Where(t => t.Value < remainingTime).Where(t => !openedValves.Contains(t.Key)))
            {              
                int presure = releasedPresure + flowRate * tunnel.Value;
                maxPresure = Math.Max(maxPresure, BestMove(tunnel.Key, new(openedValves), presure, flowRate, remainingTime - tunnel.Value, isLastRun));
            }

            // With elephant: it might be advantageous to stay at current position and wait for the elephant to do stuff
            if (!isLastRun) maxPresure = Math.Max(maxPresure, releasedPresure + remainingTime * flowRate + BestMove(_valves.Single(v => v.Name == "AA"), new(openedValves), 0, 0, 26, true));
            
            return maxPresure;
        }
    }
}