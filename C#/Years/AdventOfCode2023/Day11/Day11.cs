namespace AdventOfCode2023
{
    public static class Day11
    {
        private static List<int> _expandedRowIndexes = [];
        private static List<int> _expandedColumnIndexes = [];
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day11\input.txt");

            FindExpandedLines(input);

            List<(int x, int y)> galaxies = ParseGalaxies(input);

            long totalDistance = ComputeDistance(galaxies, part);

            Console.WriteLine(totalDistance);
        }

        private static void FindExpandedLines(string[] input)
        {
            _expandedRowIndexes = Enumerable.Range(0, input.Length)
                .Where(y => input[y].All(c => c != '#'))
                .ToList();
            
            _expandedColumnIndexes = Enumerable.Range(0, input.First().Length)
                .Where(x => input.All(i => i[x] != '#'))
                .ToList();
        }

        private static List<(int x, int y)> ParseGalaxies(string[] input) => input
            .Select((content, y) => (content, y))
            .SelectMany(line => line.content
                .Select((spotValue, x) => (spotValue, x , line.y)))
            .Where(point => point.spotValue == '#')
            .Select(point => (point.x, point.y))
            .ToList();

        private static long ComputeDistance(List<(int x, int y)> galaxies, int part)
        {
            long output = 0;
            for (int galaxy = 0; galaxy < galaxies.Count; galaxy++)
            {
                for (int galaxy2 = galaxy+1; galaxy2 < galaxies.Count; galaxy2++)
                {
                    output += ManhattanDistance(galaxies[galaxy], galaxies[galaxy2], part == 1 ? 1 : 1000000 - 1);
                }
            }
            return output;
        }

        private static int ManhattanDistance ((int x, int y) c1, (int x, int y) c2, int expansionRate) => 
            Math.Abs(c2.x - c1.x) 
            + _expandedColumnIndexes.Count(i => IsWithinColumns(i, c1, c2)) * expansionRate 
            + Math.Abs(c2.y - c1.y) 
            + _expandedRowIndexes.Count(i => IsWithinRows(i, c1, c2)) * expansionRate;

        private static bool IsWithinColumns (int columnIndex, (int x, int y) c1, (int x, int y) c2) => 
            columnIndex > Math.Min(c1.x, c2.x) && columnIndex < Math.Max(c1.x, c2.x);

        private static bool IsWithinRows (int rowIndex, (int x, int y) c1, (int x, int y) c2) => 
            rowIndex > Math.Min(c1.y, c2.y) && rowIndex < Math.Max(c1.y, c2.y);
    }
}