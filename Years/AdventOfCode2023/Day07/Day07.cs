using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2023
{
    public static class Day07
    {
        private static readonly char[] _cardValuesPart1 = ['2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'];
        private static readonly char[] _cardValuesPart2 = ['J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A'];

        enum HandType
        {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind
        }

        private class Hand
        {         
            public char[] Cards { get; private set;}
            public char[] CardsValues { get; private set;}
            public int Bid { get;}
            public HandType Type { get; }

            public Hand(string line, int part)
            {
                Cards = line.Split(' ').First().ToCharArray();
                CardsValues = line.Split(' ').First().ToCharArray(); // Jokers are not replaced for tie breaking

                Bid = int.Parse(line.Split(' ').Last());

                if (part == 2) ReplaceJokers();

                Type = DetermineType();                
            }

            private void ReplaceJokers()
            {
                if (Cards.All(c => c=='J')) return;

                Cards = Cards
                    .Select(c => c == 'J' ? Cards
                        .Where(c => c!= 'J')
                        .GroupBy(c => c)
                        .OrderByDescending(g => g.Count())
                        .First()
                        .First()
                    : c)
                    .ToArray();
            }

            private HandType DetermineType() => Cards.Distinct().Count() switch
                {
                    1 => HandType.FiveOfAKind,
                    2 when Cards.GroupBy(c => c).OrderBy(g => g.Count()).Last().Count() == 4 => HandType.FourOfAKind,
                    2 => HandType.FullHouse,
                    3 when Cards.GroupBy(c => c).OrderBy(g => g.Count()).Last().Count() == 3 => HandType.ThreeOfAKind,
                    3 => HandType.TwoPair,
                    4 => HandType.OnePair,
                    _ => HandType.HighCard
                };
        }

        private static List<Hand> _hands = new();
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day07\input.txt");

            _hands.AddRange(input.Select(i => new Hand(i, part)));

            SortHands(part == 1 ? _cardValuesPart1 : _cardValuesPart2);

            int score = _hands
                .Select((hand, rank) => (hand, rank))
                .Aggregate(0, (a,b) => a + b.hand.Bid * (b.rank+1));

            /* HANDS SORTING VISUALIZATION
            Console.WriteLine(String.Join("\r\n", _hands.Select(h => $"{string.Join("", h.CardsValues)} - {h.Type} - {h.Bid}")));
            */

            Console.WriteLine(score);
        }
        
        private static void SortHands(char[] cardValues) => _hands = [.._hands
                .OrderBy(h => h.Type)
                .ThenBy(h => Array.IndexOf(cardValues, h.CardsValues[0]))
                .ThenBy(h => Array.IndexOf(cardValues, h.CardsValues[1]))
                .ThenBy(h => Array.IndexOf(cardValues, h.CardsValues[2]))
                .ThenBy(h => Array.IndexOf(cardValues, h.CardsValues[3]))
                .ThenBy(h => Array.IndexOf(cardValues, h.CardsValues[4]))];
    }
}