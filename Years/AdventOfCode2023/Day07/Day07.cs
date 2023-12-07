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
        private static readonly char[] _cardValues = ['2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'];

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
            public int Bid { get;}
            public HandType Type { get;}

            public Hand(string line)
            {
                Cards = line.Split(' ').First().ToCharArray();
                Bid = int.Parse(line.Split(' ').Last());

                Type = Cards.Distinct().Count() switch
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
        }

        private static List<Hand> _hands = new();
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day07\input.txt");

            _hands.AddRange(input.Select(i => new Hand(i)));

            ArrangeHands();

            int score = _hands
                .Select((hand, rank) => (hand, rank))
                .Aggregate(0, (a,b) => a + b.hand.Bid * (b.rank+1));

            Console.WriteLine(score);
        }
        
        private static void ArrangeHands() => _hands = _hands
                .OrderBy(h => h.Type)
                .ThenBy(h => Array.IndexOf(_cardValues, h.Cards[0]))
                .ThenBy(h => Array.IndexOf(_cardValues, h.Cards[1]))
                .ThenBy(h => Array.IndexOf(_cardValues, h.Cards[2]))
                .ThenBy(h => Array.IndexOf(_cardValues, h.Cards[3]))
                .ThenBy(h => Array.IndexOf(_cardValues, h.Cards[4]))
                .ToList();
    }
}