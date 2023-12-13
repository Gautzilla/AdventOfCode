namespace AdventOfCode2023
{
    public static class Day12
    {
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day12\input.txt");    

            input = input.Select(line => line.RepeatInput(part == 1 ? 1 : 5)).ToArray();

            long result = input.Sum(line => NbMatchingSpringSequences(line.GetSpringSequence(), line.GetRequiredConsecutiveLitSprings()));

            Console.WriteLine(result);
        }

        private static string RepeatInput (this string line, int numberOfTimes)
        {
            string springs = line.Split(' ').First(), requiredConsecutiveSprings = line.Split(' ').Last();
            return string.Join("?", Enumerable.Range(0,numberOfTimes).Select(i => springs)) + " " + string.Join(",", Enumerable.Range(0,numberOfTimes).Select(i => requiredConsecutiveSprings));
        }

        private static List<int> GetRequiredConsecutiveLitSprings (this string line) => line
            .Split(' ')
            .Last()
            .Split(',')
            .Select(int.Parse)
            .ToList();

        private static string GetSpringSequence (this string line) => line
            .Split(' ')
            .First();
            

        private static Dictionary<string, long> _mem = [];
        
        private static long NbMatchingSpringSequences (string springs, List<int> litLengths)
        {
            long result = 0;

            if (springs == "") return litLengths.Count == 0 ? 1 : 0;

            if (litLengths.Count == 0) return springs.Any(c => c == '#') ? 0 : 1;

            string key = springs + "_" + string.Join(",", litLengths);

            if (_mem.TryGetValue(key, out long res)) return res;

            if (".?".Contains(springs.First())) result += NbMatchingSpringSequences(springs[1..], new(litLengths));

            if ("#?".Contains(springs.First()))
            {
                if (springs.Length < litLengths.First()) return result;
                if (springs[..litLengths.First()].Contains('.')) return result;

                if (springs.Length == litLengths.First() || springs[litLengths.First()] != '#') result += NbMatchingSpringSequences(springs[Math.Min(springs.Length, litLengths.First()+1)..], new(litLengths[1..]));
            }

            _mem.Add(key, result);

            return result;
        }
    }
}