namespace AdventOfCode2023
{
    public static class Day13
    {
        class Pattern
        {
            public List<(int x, int y)> Rocks { get; set; }
            public (int x, int y) Size { get; set; }
            public int NbRowsBeforeLine { get; set; } = 0;
            public int NbColumnBeforeLine { get; set; } = 0;
            public int SummarizedNote { get; set; }

            public Pattern(IEnumerable<string> input)
            {
                Rocks = input
                    .SelectMany((row, y) => row
                        .Select((val, x) => (val, x, y)))
                        .Where(point => point.val == '#')
                        .Select(point => (point.x, point.y))
                    .ToList();

                Size = (Rocks.Max(rock => rock.x), Rocks.Max(rock => rock.y));

                NbColumnBeforeLine = FindLoop(true);
                NbRowsBeforeLine = FindLoop(false);

                SummarizedNote = NbColumnBeforeLine + 100 * NbRowsBeforeLine;
            }

            private int FindLoop(bool isLoopVertical)
            {
                int size = isLoopVertical ? Size.x : Size.y;

                for (int index = 1; index <= size; index++)
                {
                    if (IsLoop(index, isLoopVertical)) return index;
                }

                return 0;
            }

            private bool IsLoop (int index, bool isLoopVertical)
            {
                int size = isLoopVertical ? Math.Min(index, Size.x - index + 1) : Math.Min(index, Size.y - index + 1);
                
                List<(int x, int y)> RocksA = isLoopVertical ? Rocks.Where(r => r.x >= index - size && r.x < index).ToList() : Rocks.Where(r => r.y >= index - size && r.y < index).ToList();
                List<(int x, int y)> RocksB = isLoopVertical ? Rocks.Where(r => r.x >= index && r.x < index + size).ToList() : Rocks.Where(r => r.y >= index && r.y < index + size).ToList();

                if (_part == 1 && RocksA.Count != RocksB.Count) return false;

                if (_part == 2)
                { 
                    if (Math.Abs(RocksA.Count - RocksB.Count) != 1) return false;

                    var RocksATemp = RocksA.Count < RocksB.Count ? RocksA : RocksB;
                    var RocksBTemp = RocksA.Count < RocksB.Count ? RocksB : RocksA;

                    RocksA = RocksATemp;
                    RocksB = RocksBTemp;
                }

                foreach (var rockA in RocksA)
                {
                    var sameLineRocks = isLoopVertical ? RocksB.Where(r => r.y == rockA.y) : RocksB.Where(r => r.x == rockA.x);

                    if (isLoopVertical && !sameLineRocks.Any(rockB => rockB.x - index == (index - rockA.x - 1)))  return false;
                    if (!isLoopVertical && !sameLineRocks.Any(rockB => rockB.y - index == (index - rockA.y - 1))) return false;                    
                }
                
                return true;
            }

            public void Print() => Console.WriteLine(string.Join("\r\n",
                Enumerable.Range(0, Size.y+1)
                    .Select(y => string.Join("", Enumerable.Range(0, Size.x+1)
                        .Select(x => Rocks.Contains((x,y)) ? '#' : '.')))));
        }

        private static List<Pattern> _patterns = [];
        private static int _part = 1;
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day13\input.txt");
            
            _part = part;

            ParsePatterns(input);           

            Console.WriteLine(_patterns.Sum(p => p.SummarizedNote));
        }

        private static void ParsePatterns (string[] input)
        {
            List<string> rows = [];
            foreach (var line in input)
            {
                if (line == string.Empty && rows.Count > 0)
                {
                    _patterns.Add(new Pattern(rows));
                    rows = [];
                    continue;
                }
                rows.Add(line);
            }
            if (rows.Count > 0) _patterns.Add(new Pattern(rows));
        }
    }
}