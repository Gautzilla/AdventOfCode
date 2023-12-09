namespace AdventOfCode2023
{
    public static class Day09
    {
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day09\input.txt");

            List<List<int>> inputs = ParseInput(input);

            if (part == 1) Console.WriteLine(Part1(inputs));
            else Console.WriteLine(Part2(inputs));
        }

        private static List<List<int>> ParseInput(string[] input) => input
            .Select(s => s
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList())
            .ToList();

        private static int Part1 (List<List<int>> inputs) => inputs
            .Select(GetEndNumbers)
            .Select(ends => ends.Select(e => e.last).Sum())
            .Sum();

        private static int Part2 (List<List<int>> inputs) => inputs
            .Select(GetEndNumbers)
            .Select(ends => ends.Select(e => e.first).Reverse())
            .Select(firstNumbers => firstNumbers.Aggregate((a,b) => b-a))
            .Sum();

        private static List<(int first, int last)> GetEndNumbers(List<int> input)
        {
            List<(int, int)> endNumbers = [(input.First(),input.Last())];

            while (input.Any(i => i != 0))
            {                
                List<int> copy = new(input);
                input = copy.Skip(1).Select((val, index) => copy[index+1] - copy[index]).ToList();
                endNumbers.Add((input.First(),input.Last()));
            }
            return endNumbers;
        }
    }
}