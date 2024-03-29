using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public static class Day19
    {
        class RatingRange
        {
            public int[] Ranges { get; set; } = [..Enumerable.Range(0,8).Select(i => i%2 == 0 ? 0 : 4001)]; // Exclusive xMin, xMax, mMin, mMax, aMin, aMax, sMin, sMax
            public bool IsAccepted { get; set; }
            public long Size => new int[] { 1, 3, 5, 7 }.Select(i => Ranges[i] - Ranges[i - 1] - 1).Aggregate((long)1, (a, b) => a * b); // - 1 because both borders are exclusive

            public RatingRange()
            {
                IsAccepted = false;
            }

            public RatingRange(RatingRange original)
            {
                Ranges = [..original.Ranges];
                IsAccepted = false;
            }

            public bool ContainsRating(Rating rating)
            {
                if (rating.X <= Ranges[0]) return false;
                if (rating.X >= Ranges[1]) return false;
                if (rating.M <= Ranges[2]) return false;
                if (rating.M >= Ranges[3]) return false;
                if (rating.A <= Ranges[4]) return false;
                if (rating.A >= Ranges[5]) return false;
                if (rating.S <= Ranges[6]) return false;
                if (rating.S >= Ranges[7]) return false;
                return true;
            }
        }

        class Workflow
        {
            public List<string> Instructions { get; set; }
            public string Name { get; set; }            

            public Workflow(string input) // px{a<2006:qkq,m>2090:A,rfg}
            {
                Name = input.Split('{').First();
                Instructions = [..input.Split('{').Last().TrimEnd('}').Split(',')];
            }

            public void ProcessInstructions (RatingRange ratingRange)
            {
                foreach (var instruction in Instructions)
                {
                    string destination = instruction.Split(':').Last();

                    if (!instruction.Contains(':')) // Directly accepted or rejected
                    {
                        SendInstruction(ratingRange, destination);
                        continue;
                    }
                    
                    string operation = instruction.Split(':').First();
                    int indexBase = "xmas".IndexOf(operation.First()) * 2;
                    int indexModifier = "><".IndexOf(operation[1]); // Index where to store the value (ex: x<n : store n as in "x in ]1, n[")

                    int value = int.Parse(operation[2..]);

                    RatingRange subRatingRange = new(ratingRange);

                    subRatingRange.Ranges[indexBase + indexModifier] = value;
                    SendInstruction(subRatingRange, destination);
                    
                    ratingRange.Ranges[indexBase + (indexModifier == 1 ? 0 : 1)] = value + (indexModifier == 0 ? 1 : -1); // Removes the subRatingRange from the current ratingRange
                }
            }

            private void SendInstruction (RatingRange ratingRange, string destination)
            {
                if (destination == "A")
                {
                    ratingRange.IsAccepted = true;
                    _ratingRanges.Add(ratingRange);
                    return;
                }
                if (destination == "R") return;
                
                Workflow nextWorkflow = _workflows.Single(w => w.Name == destination);
                nextWorkflow.ProcessInstructions(ratingRange);
            }
        }

        class Rating
        {
            public int X { get; set; }
            public int M { get; set; }
            public int A { get; set; }
            public int S { get; set; }
            public bool IsAccepted { get; set; }

            public Rating(string input) // {x=787,m=2655,a=1222,s=2876}
            {
                Match match = Regex.Match(input, @"{x=(?<x>\d+),m=(?<m>\d+),a=(?<a>\d+),s=(?<s>\d+)}");
                X = int.Parse(match.Groups["x"].Value);
                M = int.Parse(match.Groups["m"].Value);
                A = int.Parse(match.Groups["a"].Value);
                S = int.Parse(match.Groups["s"].Value);

                IsAccepted = _ratingRanges.Where(r => r.IsAccepted).Any(r => r.ContainsRating(this));
            }
        }

        private static HashSet<RatingRange> _ratingRanges = [];
        private static List<Workflow> _workflows = [];
        private static List<Rating> _ratings = [];

        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day19\input.txt");

            ParseWorkflows(input);
            ParseRatingRanges();            
            ParseRatings(input[(_workflows.Count+1)..]);
            
            if (part == 1) Console.WriteLine(_ratings.Where(r => r.IsAccepted).Select(r => r.X + r.M + r.A + r.S).Sum());
            else Console.WriteLine(_ratingRanges.Where(r => r.IsAccepted).Sum(range => range.Size));
        }

        private static void ParseWorkflows(string[] input)
        {
            foreach (string line in input)
            {
                if (line == string.Empty) break;
                _workflows.Add(new Workflow(line));
            }
        }

        private static void ParseRatingRanges() => _workflows.Single(w => w.Name == "in").ProcessInstructions(new()); // Initializes the rating ranges by passing the whole possible range to the first workflow

        private static void ParseRatings(string[] input) => _ratings
            .AddRange(input
                .Select(line => new Rating(line)));
    }
}