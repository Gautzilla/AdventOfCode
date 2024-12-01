namespace AdventOfCode2023
{
    public static class Day05
    {
        private record Seed
        {
            public long Start { get;}
            public long Stop { get;}
            public bool HasEvolved { get; set; }

            public Seed(long start, long stop, bool hasEvolved)
            {
                Start = start;
                Stop = stop;
                HasEvolved = hasEvolved;        
            }
        }

        private record MapItem
        {
            public long Start { get;}
            public long Stop { get;}
            public long Offset { get;}

            public MapItem(string line)
            {
                var ints = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

                Start = ints[1];
                Stop = ints[1] + ints[2];
                Offset = ints[0] - ints[1];
            }
        }
        private static List<Seed> _seeds = [];
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day05\input.txt");

            if (part == 1) ParseSeedsP1(input.First());   
            else ParseSeedsP2(input.First());

            foreach (var line in input.Skip(1)) ProcessLine(line);

            Console.WriteLine(_seeds.Min(s => s.Start));
        }

        private static void ParseSeedsP1 (string inputLine) => _seeds = inputLine
                .Split(' ')
                .Where(s => long.TryParse(s, out long i))
                .Select(long.Parse)
                .Select(l => new Seed(l, l, false))
                .ToList();
        
        private static void ParseSeedsP2 (string inputLine) => _seeds = inputLine
                .Split(' ')
                .Where(s => long.TryParse(s, out long temp))
                .Chunk(2)
                .Select(chunk => chunk.Select(long.Parse))
                .Select(chunk => new Seed(chunk.First(), chunk.First() + chunk.Last() - 1, false))
                .ToList();

        private static void ProcessLine(string line)
        {
            if (line == string.Empty) return;

            if (line.Contains("map"))
            {
                // New category, every seed is reinitialized
                foreach (var seed in _seeds) seed.HasEvolved = false;
                return;
            }

            MapItem mapItem = new(line);
            
            _seeds = EvolveSeeds(mapItem);
        }

        private static List<Seed> EvolveSeeds(MapItem mapItem)
        {
            List<Seed> newSeeds = new(_seeds.Where(s => s.HasEvolved));

            foreach (var seed in _seeds.Where(s => !s.HasEvolved))
            {
                if (!seed.Intercepts(mapItem))
                {
                        newSeeds.Add(seed);
                        continue;
                }
                newSeeds.AddRange(Intersection(seed, mapItem));
            }

            return newSeeds;
        }

        private static bool Intercepts(this Seed source, MapItem mapItem) => !(source.Stop < mapItem.Start || source.Start > mapItem.Stop);

        private static List<Seed> Intersection(Seed source, MapItem mapItem)
        {
            List<Seed> output = new();

            // Part that is not concerned by the map input (lower value than map input start)
            if (source.Start < mapItem.Start) output.Add(new Seed(source.Start, mapItem.Start-1, false));

            // Part that is concerned by the map input
            output.Add(new Seed(Math.Max(source.Start, mapItem.Start) + mapItem.Offset, Math.Min(source.Stop, mapItem.Stop) + mapItem.Offset, true));

            // Part that is not concerned by the map input (higher value than map input start)
            if (source.Stop > mapItem.Stop) output.Add(new Seed(mapItem.Stop + 1, source.Stop, false));

            return output;
        }
    }
}