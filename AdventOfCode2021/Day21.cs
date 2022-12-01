using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day21 // Solution inspired by https://www.reddit.com/r/adventofcode/comments/rl6p8y/comment/hpkxh2c/?utm_source=reddit&utm_medium=web2x&context=3
    {
        static public void Solve(int part)
        {
            //string filePath = @"..\..\Inputs\day21Example.txt";
            string filePath = @"..\..\Inputs\day21.txt";

            List<string> input = File.ReadAllLines(filePath).ToList();

            int[] positions = { int.Parse(input[0].Last().ToString()), int.Parse(input[1].Last().ToString()) };
            positions[0]--; // Offset to account for the position 10 on the board (and the %10 in position computing)
            positions[1]--;

            int[] scores = { 0, 0 };

            if (part == 1)
            {
                var finalScore = PlayPracticeGame(positions, scores);
                Console.WriteLine($"Loser has a score of {finalScore.loserScore} after {finalScore.diceRolls} rolls: {finalScore.loserScore * finalScore.diceRolls}");
            }
            else
            {
                var totalWonGames = PlayDiracGame(positions[0], scores[0], positions[1], scores[1]);
                Console.WriteLine($"{totalWonGames.Player1Wins} ; {totalWonGames.Player2Wins} : Best player wins in {Math.Max(totalWonGames.Player1Wins, totalWonGames.Player2Wins)} universes.");
            }
        }


        private static (int diceRolls, int loserScore) PlayPracticeGame(int[] positions, int[] scores)
        {
            int player = 1; // Player 1 (players[0]) goes first
            int dice = 1;

            while (scores[player] < 1000)
            {
                player = 1 - player;
                positions[player] = (positions[player] + dice * 3 + 3) % 10; // extra 10 if dice rolled > 10 : matches with one loop in the board.
                scores[player] += positions[player] + 1; 
                dice += 3;
            }

            dice--; // Initialization at 1

            return (dice, scores[1-player]);
        }

        static private (int Score, int Frequency)[] diracDiceRolls = {(3, 1), (4,3), (5,6), (6,7), (7,6), (8,3), (9,1)};

        private static (long Player1Wins, long Player2Wins) PlayDiracGame(int player1Position, int player1Score, int player2Position, int player2Score)
        {
            if (player2Score > 20) return (0, 1);

            var wins = (Player1Wins: (long)0, Player2Wins: (long)0);

            foreach (var diceRoll in diracDiceRolls)
            {
                var nextTurnWins = PlayDiracGame(player2Position, player2Score, (player1Position + diceRoll.Score) % 10, player1Score + (player1Position + diceRoll.Score) % 10 + 1);
                wins = (wins.Player1Wins + nextTurnWins.Player2Wins * diceRoll.Frequency, wins.Player2Wins + nextTurnWins.Player1Wins * diceRoll.Frequency);
            }

            return wins;
        }

    }
}
