using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public class Monkey
    {
        private int lcm;
        public int LCM
        {
            get { return lcm; }
            set { lcm = value; }
        }
        
        private int iD;
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
        
        private Queue<long> items = new();
        public Queue<long> Items
        {
            get { return items; }
            set { items = value; }
        }        
        
        private Func<long, long> operation = (a => 0);
        public Func<long, long> Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        private int testValue = 1;
        public int TestValue
        {
            get { return testValue; }
            set { testValue = value; }
        }

        private (int,int) throwsTo = (0,0);
        public (int ifTrue, int ifFalse) ThrowsTo
        {
            get { return throwsTo; }
            set { throwsTo = value; }
        }        

        private long inspectedItems;
        public long InspectedItems
        {
            get { return inspectedItems; }
            private set { inspectedItems = value; }
        }

        private int _worryLevelDivider;
        
        

        public Monkey(string[] input, int lcm, int part)
        {
            iD = int.Parse(input[0].Split(" ").Last().TrimEnd(':'));
            items = ParseItems(input[1]);            
            operation = ParseOperation(input[2]);
            testValue = ParseTestValue(input[3]);
            throwsTo = ParseThrow(input.Skip(4).Where(s => s != string.Empty).ToArray());
            inspectedItems = 0;
            this.lcm = lcm;
            _worryLevelDivider = part == 1 ? 3 : 1;
        }     

        private Queue<long> ParseItems(string input)
        {
            Queue<long> queue = new();
            foreach (long item in input.Split(":").Last().Split(",").Select(s => long.Parse(s))) queue.Enqueue(item);
            return queue;
        }   

        private Func<long, long> ParseOperation(string input)
        {
            if (input.Split(" ").Last() == "old")
            {
                return input.Split(" ")[6] switch
            {
                "*" => (a => a * a),
                "+" => (a => a + a),
                "-" => (a => a - a),
                _ => (a => a / a)
            };
            }

            int number = int.Parse(input.Split(" ").Last());
            return input.Split(" ")[6] switch
            {
                "*" => (a => a * number),
                "+" => (a => a + number),
                "-" => (a => a - number),
                _ => (a => a / number)
            };
        }

        private int ParseTestValue(string input) => int.Parse(input.Split(" ").Last());
    
        private (int, int) ParseThrow(string[] inputs) => (int.Parse(inputs.First().Split(" ").Last()), int.Parse(inputs.Last().Split(" ").Last()));
    
        public (long item, int monkey) InspectItem()
        {
            inspectedItems++;

            long newItemValue = (Operation(Items.Dequeue()) / _worryLevelDivider ) % lcm;
            int newMonkey = newItemValue % testValue == 0 ? ThrowsTo.ifTrue : ThrowsTo.ifFalse;
            return (newItemValue, newMonkey);
        }
    }
}