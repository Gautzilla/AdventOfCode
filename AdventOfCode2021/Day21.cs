using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day21
    {
        static public void Solve(int part)
        {
            string filePath = @"..\..\Inputs\day21Example.txt";
            //string filePath = @"..\..\Inputs\day21.txt";

            List<string> input = File.ReadAllLines(filePath).ToList();

            Tuple<int, int> player1 = new Tuple<int, int>(int.Parse(input[0].Last().ToString()), 0);
            Tuple<int, int> player2 = new Tuple<int, int>(int.Parse(input[1].Last().ToString()), 0);

            if (part == 1) PlayPracticeGame(player1, player2);
            else PlayDiracGame(player1, player2);
        }

        static private void PlayPracticeGame (Tuple<int, int> player1, Tuple<int, int> player2)
        {
            int dice = 0;
            int diceRolled = 0;

            bool player1ToPlay = true;

            while (player1.Item2 < 1000 && player2.Item2 < 1000)
            {
                Tuple<int, int> rollDice = RollPracticeDice(dice);
                int score = rollDice.Item2;
                dice = rollDice.Item1;

                if (player1ToPlay) player1 = PlayTurn(player1, score);
                else player2 = PlayTurn(player2, score);

                diceRolled += 3;

                player1ToPlay = !player1ToPlay;
            }

            int finalScore = diceRolled * (player1.Item2 >= 1000 ? player2.Item2 : player1.Item2);
            Console.WriteLine($"Final score : {finalScore}");
        }

        static private void PlayDiracGame( Tuple<int, int> player1, Tuple <int, int> player2)
        {

        }

        private static Tuple<int, int> RollPracticeDice (int dice)
        {
            int score = 0;
            for (int i = 0; i < 3; i++)
            {
                if (++dice > 100) dice = 1;
                score += dice;
            }
            return new Tuple<int,int>(dice, score);
        }

        private static Tuple<int, int> PlayTurn (Tuple<int, int> player, int score)
        {
            int newPosition = (player.Item1 + score) % 10 == 0 ? 10 : (player.Item1 + score) % 10;
            int newScore = player.Item2 + newPosition;

            return new Tuple<int, int>(newPosition, newScore);
        }
    }
}
