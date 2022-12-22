using System;
using System.Collections.Generic;

namespace AdventOfCode2022
{   
    class Valve
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        
        private int flowRate;
        public int FlowRate
        {
            get { return flowRate; }
            set { flowRate = value; }
        }
        
        private HashSet<Valve> tunnels;
        public HashSet<Valve> Tunnels
        {
            get { return tunnels; }
            set { tunnels = value; }
        }

        private Dictionary<Valve, int> valveDistance;
        public Dictionary<Valve, int> ValveDistance
        {
            get { return valveDistance; }
            set { valveDistance = value; }
        }
        
        
        public Valve(string name, int presure)
        {
            this.name = name;
            this.flowRate = presure;
            tunnels = new();
            valveDistance = new();
        }

        public void AddTunnel(Valve valve) => tunnels.Add(valve);

        public void ComputeDistances(HashSet<Valve> valves)
        {
            Queue<(Valve valve, int distance)> paths = new();
            foreach (Valve valve in tunnels) paths.Enqueue((valve, 1));

            while (paths.Count > 0)
            {
                var current = paths.Dequeue();

                if (valveDistance.ContainsKey(current.valve) || current.valve == this) continue;
                
                valveDistance.Add(current.valve, current.distance);
                foreach (Valve tunnel in current.valve.tunnels) paths.Enqueue((tunnel, current.distance + 1));
            }
        }
    }
}