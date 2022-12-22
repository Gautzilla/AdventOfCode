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
            
            Console.WriteLine(String.Join("\n", _valves.Single(v => v.Name == "AA").ValveDistance.Select(vd => $"{vd.Key.Name} : {vd.Value}")));
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
    }
}