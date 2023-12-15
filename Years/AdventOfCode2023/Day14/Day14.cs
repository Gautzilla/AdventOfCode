namespace AdventOfCode2023
{
    public static class Day14
    {
        public static void Solve(int part)
        { 
            List<string> input = [.. File.ReadAllLines(@"Day14\input.txt")];

            input = input.Rotate(true)
                .Select(RollLeft).ToList()
                .Rotate(false);
            
            Console.WriteLine(Score(input));
        }
        
        private static int Score (List<string> input) => input.Select((row, y) => row.Count(c => c == 'O') * (input.Count - y)).Sum();

        private static List<string> Rotate (this List<string> input, bool isClockwise)
        {
            List<string> result = [];

            for (int y = 0; y < input.First().Length; y++)
            {
                string newLine = "";
                for (int x = 0; x < input.Count; x++)
                {
                    if (isClockwise) newLine += input[x][input.First().Length - y - 1];
                    else newLine += input[input.Count - x -1][y];
                }
                result.Add(newLine);
            }

            return result;
        }

        private static string RollLeft (string line) => string.Join("#", 
            line.Split('#')
                .Select(roundedRocks => string.Join("", roundedRocks
                        .OrderBy(c => "O.".IndexOf(c)))));
    }
}