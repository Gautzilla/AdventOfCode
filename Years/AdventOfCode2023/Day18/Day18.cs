namespace AdventOfCode2023
{
    public static class Day18
    {
        private static Dictionary<string, (int x, int y)> _directions = new()
        {
            {"L", (-1,0)},
            {"R", (1,0)},
            {"U", (0,1)},
            {"D", (0,-1)},
        };

        private static List<(long x, long y)> _coordinates = [(0,0)];

        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day18\input.txt");

            foreach (var line in input) line.Process(part);
            
            Console.WriteLine(_coordinates.NbPoints());
        }

        private static void Process (this string instruction, int part)
        {
            string[] parts = [..instruction.Split(' ')];
            string direction;
            int distance;

            direction = parts.First();
            distance = int.Parse(parts[1]);            
            
            (long x, long y) newCoordinates = _coordinates.Last().Move(_directions[direction], distance);  

            _coordinates.Add(newCoordinates);
        }

        private static (long x, long y) Move(this (long x, long y) coordinates, (int x, int y) direction, int distance) => (coordinates.x + distance * direction.x, coordinates.y + distance * direction.y);
    
        private static long Area (this List<(long x, long y)> coordinates) => Math.Abs(coordinates
            .Append(coordinates.First())
            .Select((c,i) => (c,i))
            .Skip(1)
            .Select(_ => _.c.x * coordinates[_.i - 1].y - _.c.y * coordinates[_.i - 1].x)
            .Sum() / 2); // Shoelace theorem

        private static long NbOuterPoints (this List<(long x, long y)> coordinates) => coordinates
            .Append(coordinates.First())
            .Select((c,i) => (c,i))
            .Skip(1)
            .Select(_ => Math.Abs(_.c.x - coordinates[_.i - 1].x) + Math.Abs(_.c.y - coordinates[_.i - 1].y))
            .Sum();

        private static long NbPoints (this List<(long x, long y)> coordinates) => 
            coordinates.Area() + coordinates.NbOuterPoints() / 2 + 1; // Picks' theorem: A = i + b/2 - 1 => i+b = A + b/2 + 1
    }
}