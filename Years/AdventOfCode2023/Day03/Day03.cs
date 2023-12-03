namespace AdventOfCode2023
{
    public static class Day03
    {
        private static char[][] _schematic = [];
        private static readonly (int x, int y)[] _directions = {(-1,0), (-1, -1), (0, -1), (1,-1), (1,0), (1,1), (0,1), (-1,1)};
        private static HashSet<Number> _numbers = new();

        private record Number
        {
            public int Value { get; }
            public (int x, int y) Coordinates { get; }
            public bool IsPart { get; } = false;

            public Number (int value, (int x, int y) coordinates)
            {
                Value = value;
                Coordinates = coordinates;

                for (int x = Coordinates.x; x < Coordinates.x + value.ToString().Length; x++)
                {
                    foreach (var dir in _directions)
                    {
                        if (!IsValid(x + dir.x, Coordinates.y + dir.y)) continue;

                        char charToCheck = _schematic[Coordinates.y + dir.y][x + dir.x];

                        if(charToCheck == '.' || char.IsDigit(charToCheck)) continue;
                        
                        IsPart = true;
                        return;
                    }
                }
            }
        }

        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day03\input.txt");

            _schematic = ParseSchematic(input);
            ParseNumbers(input);

            if (part == 1) Console.WriteLine(_numbers.Where(n => n.IsPart).Sum(n => n.Value));
        }

        private static char[][] ParseSchematic(string[] input) => input.Select(s => s.ToCharArray()).ToArray();

        private static void ParseNumbers(string[] input)
        {
            string num = string.Empty;

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    if (char.IsDigit(input[y][x])) 
                    {
                        num += input[y][x];
                        if (x != input[y].Length - 1) continue;
                    }

                    if (num == string.Empty) continue;

                    _numbers.Add(new Number(int.Parse(num), (x - num.Length, y)));
                    num = string.Empty;
                }  
            }
        }

        private static bool IsValid(int x, int y) => x >= 0 && y >= 0 && x < _schematic.First().Length && y < _schematic.Length;
    }
}