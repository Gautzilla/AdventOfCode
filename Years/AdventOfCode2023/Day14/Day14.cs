namespace AdventOfCode2023
{
    public static class Day14
    {
        public static void Solve(int part)
        { 
            List<string> input = [.. File.ReadAllLines(@"Day14\input.txt")];

            int score = 0;

            if (part == 1) score = input
                .Rotate(false)
                .Roll(true)
                .Rotate(true)
                .Score();

            if (part == 2)
            {
                List<string> memory = [];
                List<int> scores = [];

                int loopStartIndex = -1;

                while (loopStartIndex == -1)
                {
                    memory.Add(input.Store());
                    scores.Add(input.Score());                    

                    input = input
                        .Rotate(false)
                        .Roll(true)
                        .Rotate(true)
                        .Roll(true)
                        .Rotate(true)
                        .Roll(true)
                        .Rotate(false)
                        .Roll(false);

                    loopStartIndex = memory.IndexOf(input.Store());                                      
                }

                score = scores[ResultIndex(1000000000, memory.Count, loopStartIndex)];                
            }
            
            Console.WriteLine(score);
        }
        
        private static int Score (this List<string> rocks) => rocks.Select((row, y) => row.Count(c => c == 'O') * (rocks.Count - y)).Sum();

        private static string Store (this List<string> rocks) => string.Join("\r\n", rocks);

        private static List<string> Rotate (this List<string> rocks, bool isClockwise)
        {
            List<string> result = [];

            for (int y = 0; y < rocks.First().Length; y++)
            {
                string newLine = "";
                for (int x = 0; x < rocks.Count; x++)
                {
                    if (!isClockwise) newLine += rocks[x][rocks.First().Length - y - 1];
                    else newLine += rocks[rocks.Count - x -1][y];
                }
                result.Add(newLine);
            }

            return result;
        }

        private static List<string> Roll (this List<string> rocks, bool isRollLeft) => rocks
            .Select(line => string.Join("#", 
                line.Split('#')
                    .Select(roundedRocks => string.Join("", roundedRocks
                            .OrderBy(c => (isRollLeft ? "O." : ".O").IndexOf(c))))))
            .ToList();

        private static int ResultIndex(int nbTotalCycles, int memoryCount, int loopStartIndex) => 
            (nbTotalCycles - loopStartIndex )%(memoryCount - loopStartIndex ) + loopStartIndex;
    }
}