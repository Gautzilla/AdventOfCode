namespace AdventOfCode2023
{
    public static class Day04
    {
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day04\input.txt");
            
            if (part == 1) 
            {
                Console.WriteLine(input
                    .Select(line => line.NumberOfWinningNumbers())
                    .Select(nbOfWinningNumbers => nbOfWinningNumbers == 0 ? 0 : Math.Pow(2, nbOfWinningNumbers - 1))
                    .Sum());
            }
        }

        private static int NumberOfWinningNumbers (this string card) => card
                    .Split(':').Last()
                    .Split('|')
                    .Select(serie => serie.Trim().Split(' ').Distinct().Where(s => int.TryParse(s, out int o)))
                    .Aggregate((a,b) => a.Intersect(b))
                    .Count();
    }
}