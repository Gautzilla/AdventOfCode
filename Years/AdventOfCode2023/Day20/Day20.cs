using System.Numerics;

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
                    Conjuction conj = (module as Conjuction)!;
                    conj.Memory.Add(this,false);
                    conj.LoopLength.Add(this, null);
                }

                ConnectedModules.Add(module);
            }
        } 

        class FlipFlop : Module
        {        
            public bool State { get; set; } = false;
            public override void ReceivePulse(bool pulse, Module from)
            {
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
            public Dictionary<Module, long?> LoopLength { get; set; } = [];

            public override void ReceivePulse(bool pulse, Module from)
            {
                Memory[from] = pulse;

                if (pulse && Name == "bn" && !LoopLength[from].HasValue)
                {
                    LoopLength[from] = _nbButtonPressed;
                    if (LoopLength.All(l => l.Value.HasValue)) _machineIsUp = true;
                }

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
        private static bool _machineIsUp = false;
        private static int _nbButtonPressed = 0;

        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"C:\Users\Gauthier\source\repos\AdventOfCode\Years\AdventOfCode2023\Day20\input.txt");

            foreach (var line in input) CreateModule(line);
            foreach (var line in input) CreateConnections(line);
            
            Broadcast broadcaster = (_modules.Single(m => m.Name == "broadcaster") as Broadcast)!;
            
            _nbButtonPressed = 0;

            while (true)
            {
                _nbButtonPressed++;

                _pulses.Enqueue((broadcaster, broadcaster, false));
                _nbPulseSent[0]++;

                while (_pulses.Count != 0)
                {             
                    var send = _pulses.Dequeue();
                    send.destination.ReceivePulse(send.pulse, send.from);
                }
                //Console.ReadKey();

                
                if(part == 1 && _nbButtonPressed == 1000) break;
                if (part == 2 && _machineIsUp) break;
            }

            long result = _nbPulseSent[0] * _nbPulseSent[1];

            if (part == 2) result = LCM(_modules.OfType<Conjuction>().Single(m => m.Name == "bn").LoopLength.Select(l => l.Value).OfType<long>());
            
            Console.WriteLine(result);
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

        private static long GCD(long n1, long n2)
        {
            if (n2 == 0) return n1;
            return GCD(n2, n1 % n2);            
        }

        private static long LCM(IEnumerable<long> numbers) => numbers.Aggregate((S, val) => S * val / GCD(S, val));
    }
}