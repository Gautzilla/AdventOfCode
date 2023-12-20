using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Data;

namespace AdventOfCode2023
{ 
    public static class Day20
    {     
        abstract class Module
        {
            public string Name { get; set; } = "";
            public List<Module> ConnectedModules { get; private set; } = [];
            public abstract void ReceivePulse(bool pulse, Module from);

            public void SendPulse(bool pulse)
            {
                foreach (var destination in ConnectedModules) 
                {
                    //Console.WriteLine($"{Name} -{(pulse ? "high" : "low")}-> {destination.Name}");

                    _nbPulseSent[pulse ? 1 : 0]++;

                    _pulses.Enqueue((destination, this, pulse));
                }
            }

            public Module (string name)
            {
                Name = name;
            }

            public void AddConnection (Module module)
            {
                if (module.GetType() == typeof(Conjuction))
                {
                    (module as Conjuction)!.Memory.Add(this,false);
                }

                ConnectedModules.Add(module);
            }
        } 

        class FlipFlop : Module
        {        
            public bool State { get; set; } = false;
            public override void ReceivePulse(bool pulse, Module from)
            {
                //Console.WriteLine($"{Name} receives {(pulse ? "high" : "low")} pulse.");
                if (pulse) return;

                State = !State;
                SendPulse(State);
            }

            public FlipFlop(string name) : base (name)
            {
                
            }
        }

        class Conjuction : Module
        {
            public Dictionary<Module, bool> Memory { get; set; } = [];

            public override void ReceivePulse(bool pulse, Module from)
            {
                //Console.WriteLine($"{Name} receives {(pulse ? "high" : "low")} pulse.");
                Memory[from] = pulse;
                SendPulse(Memory.Any(pulse => !pulse.Value));
            }

            public Conjuction(string name) : base (name)
            {

            }
        }

        class Broadcast : Module
        {
            public override void ReceivePulse(bool pulse, Module from)
            {
                //Console.WriteLine($"{Name} receives {(pulse ? "high" : "low")} pulse.");
                SendPulse(pulse);
            }

            public Broadcast (string name) : base (name)
            {
                
            }
        }

        class Dump : Module
        {
            public Dump(string name) : base(name)
            {
            }

            public override void ReceivePulse(bool pulse, Module from)
            {
                
            }
        }

        private static List<Module> _modules = [];

        private static Queue<(Module destination, Module from, bool pulse)> _pulses = [];

        private static int[] _nbPulseSent = [0,0];

        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"C:\Users\Gauthier\source\repos\AdventOfCode\Years\AdventOfCode2023\Day20\input.txt");

            foreach (var line in input) CreateModule(line);
            foreach (var line in input) CreateConnections(line);
            
            Broadcast broadcaster = (_modules.Single(m => m.Name == "broadcaster") as Broadcast)!;
            
            for (int i = 0; i < 1000; i ++)
            {
                _pulses.Enqueue((broadcaster, broadcaster, false));
                _nbPulseSent[0]++;

                while (_pulses.Count != 0)
                {             
                    var send = _pulses.Dequeue();
                    //Console.WriteLine($"{(send.pulse ? "high" : "low")} pulse sent to {send.destination.Name}");
                    send.destination.ReceivePulse(send.pulse, send.from);
                }
                //Console.ReadKey();
            }

            Console.WriteLine($" {_nbPulseSent[0]} * {_nbPulseSent[1]} = {_nbPulseSent[0] * _nbPulseSent[1]}");
            
        }

        private static void CreateModule (string line)
        {
            string name = line.Split(' ').First();

            if (name.First() == '%')
            {
                _modules.Add(new FlipFlop(name[1..]));
                return;
            }
            if (name.First() == '&')
            {
                _modules.Add(new Conjuction(name[1..]));
                return;
            }
            if (name == "broadcaster")
            {
                _modules.Add(new Broadcast(name));
                return;
            }
            _modules.Add(new Dump(name));
        }

        private static void CreateConnections (string line) // broadcaster -> a, b, c
        {
            string name = line.Split(' ').First();
            name = (name == "broadcaster") ? name : name[1..];
            
            Module module = _modules.Single(m => m.Name == name);

            List<Module> connected = line
                .Split("->")
                .Last()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)  
                .Select(s => s.Trim(','))
                .Select(name => _modules.SingleOrDefault(m => m.Name == name) ?? new Dump(name))
                .ToList();

            foreach (var connection in connected) module.AddConnection(connection); 
        }
    }
}