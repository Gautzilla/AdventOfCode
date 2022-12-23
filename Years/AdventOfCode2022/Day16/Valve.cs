using System;
using System.Linq;
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

        public int BestMove(HashSet<Valve> openedValves, int releasedPresure, int flowRate, int remainingTime)
        {
            //OPEN (not for AA if its flowRate == 0)
            if (this.FlowRate > 0)
            {
                openedValves = openedValves.Append(this).ToHashSet();
                releasedPresure += flowRate;
                flowRate += this.flowRate;
                remainingTime--;
            }

            //WAIT (no remaining valve to open at reach)
            if (openedValves.Contains(this) && this.ValveDistance.Where(v => v.Key.FlowRate > 0).Where(v => v.Value < remainingTime).All(v => openedValves.Contains(v.Key))) return releasedPresure + remainingTime * flowRate;

            //MOVE
            int maxPresure = releasedPresure;
            foreach (var tunnel in this.valveDistance.Where(t => t.Value < remainingTime).Where(t => t.Key.FlowRate > 0).Where(t => !openedValves.Contains(t.Key)))
            {              
                int presure = releasedPresure + flowRate * tunnel.Value;
                maxPresure = Math.Max(maxPresure, tunnel.Key.BestMove(new(openedValves), presure, flowRate, remainingTime - tunnel.Value));
            }

            //WAIT
            //releasedPresure = Math.Max(releasedPresure, this.BestMove(openedValves, releasedPresure + flowRate, flowRate, remainingTime - 1));


            return maxPresure;
        }
    }
}