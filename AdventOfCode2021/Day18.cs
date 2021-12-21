using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day18
    {
        private class SnailfishNumber
        {
            public SnailfishNumber leftNum { get; set; }
            public SnailfishNumber rightNum { get; set; }
            public bool isPrimary { get; set; }
            public int value { get; set; }

            public SnailfishNumber AddSnailfishNumber (SnailfishNumber numToAdd)
            {
                return new SnailfishNumber { isPrimary = false, leftNum = this, rightNum = numToAdd };
            }

            public string DisplaySnailfishNumber()
            {
                string leftNumberDisplay = "";
                string rightNumberDisplay = "";

                leftNumberDisplay = leftNum.isPrimary ? (leftNum.value).ToString() : leftNum.DisplaySnailfishNumber();
                rightNumberDisplay = rightNum.isPrimary ? (rightNum.value).ToString() : rightNum.DisplaySnailfishNumber();

                string number = $"[{leftNumberDisplay},{rightNumberDisplay}]";
                return number;
            }

            public static explicit operator SnailfishNumber(int i)
            {
                return new SnailfishNumber() { isPrimary = true, value = i };
            }

            public string ReduceSnailfishNumber()
            {
                string numberToReduce = DisplaySnailfishNumber();

                bool explodes = true;
                bool splits = true;

                while (explodes || splits)
                {
                    if (Explode(numberToReduce).Item1)
                    {
                        numberToReduce = Explode(numberToReduce).Item2;
                        explodes = true;
                        continue;
                    } else explodes = false;
                    if (Split(numberToReduce).Item1)
                    {
                        numberToReduce = Split(numberToReduce).Item2;
                        splits = true;
                        continue;
                    } else splits = false;
                }

                return numberToReduce;
            }

            private (bool, string) Explode(string stringNumber)
            {
                bool primaryNumber = true;
                bool firstNumber = true;

                bool explodeNumber = false;

                int i = 0;
                int level = 0;

                foreach (char c in stringNumber)
                {
                    switch (c)
                        {
                        case '[':
                            level++;
                            primaryNumber = false;
                            firstNumber = true;
                            break;
                        case ']':
                            level--;
                            primaryNumber = false;
                            break;
                        case ',':
                            firstNumber = false;
                            break;
                        default :
                            if (primaryNumber && !firstNumber && level > 4)
                            {
                                explodeNumber = true;
                            }
                            primaryNumber = true;
                            break;
                        }
                    if (explodeNumber) break;
                    i++;
                }

                if (explodeNumber)
                {
                    string leftNumber;
                    string rightNumber;

                    int leftNumberSize;
                    int rightNumberSize;


                    if (int.TryParse(new string(stringNumber.Skip(i-3).Take(2).ToArray()), out int leftNumberInt))
                    {
                        leftNumberSize = 2;
                        leftNumber = leftNumberInt.ToString();
                    } else
                    {
                        leftNumberSize = 1;
                        leftNumber = stringNumber[i-2].ToString();
                    }

                    if (int.TryParse(new string(stringNumber.Skip(i).Take(2).ToArray()), out int rightNumberInt))
                    {
                        rightNumberSize = 2;
                        rightNumber = rightNumberInt.ToString();
                    } else
                    {
                        rightNumberSize = 1;
                        rightNumber = stringNumber[i].ToString();
                    }

                    for (int j = i+rightNumberSize; j < stringNumber.Length; j++) if(int.TryParse(stringNumber[j].ToString(), out int numValue))
                     {
                        int numberToReplaceSize;
                        int sum;

                        if (int.TryParse(new string (stringNumber.Skip(j).Take(2).ToArray()), out int numToReplace))
                        {
                            numberToReplaceSize = 2;
                            sum = int.Parse(rightNumber) + numToReplace;
                        } else
                        {
                            numberToReplaceSize = 1;
                            sum = int.Parse(rightNumber) + numValue;
                        }
                        stringNumber = stringNumber.Remove(j,numberToReplaceSize).Insert(j, sum.ToString());
                        break;
                    }

                    stringNumber = stringNumber.Remove(i-(2+leftNumberSize), 3+rightNumberSize+leftNumberSize).Insert(i-(2+leftNumberSize), "0");

                    for (int j = i-(3+leftNumberSize); j >=0; j--) if(int.TryParse(stringNumber[j].ToString(), out int numValue))
                     {
                        int numberToReplaceSize;
                        int sum;

                        if (int.TryParse(new string (stringNumber.Skip(j-1).Take(2).ToArray()), out int numToReplace))
                        {
                            numberToReplaceSize = 2;
                            sum = int.Parse(leftNumber) + numToReplace;
                        } else
                        {
                            numberToReplaceSize = 1;
                            sum = int.Parse(leftNumber) + numValue;
                        }
                        stringNumber = stringNumber.Remove(j-numberToReplaceSize+1,numberToReplaceSize).Insert(j-numberToReplaceSize+1, sum.ToString());
                        break;
                    }

                }
                return (explodeNumber, stringNumber);
            }

            private (bool, string) Split(string stringNumber)
            {
                int i = 0;
                bool largeNumber = false;

                bool splitNumber = false;

                foreach (char c in stringNumber)
                {
                    switch (c)
                        {
                        case '[':
                        case ']':
                        case ',':
                            largeNumber = false;
                            break;
                        default :
                            if (largeNumber) splitNumber = true;
                            largeNumber = true;
                            break;
                        }
                    if (splitNumber) break;
                    i++;
                }

                if (splitNumber)
                {
                    int numberToSplit = int.Parse(new string (stringNumber.Skip(i-1).Take(2).ToArray()));
                    int leftSplitNumber = numberToSplit / 2;
                    int rightSplitNumber = numberToSplit % 2 == 0 ? numberToSplit / 2 : numberToSplit / 2 + 1;

                    string newSplitNumber = $"[{leftSplitNumber.ToString()},{rightSplitNumber.ToString()}]";

                    stringNumber = stringNumber.Remove(i-1, 2).Insert(i-1, newSplitNumber);
                }
                return (splitNumber, stringNumber);
            }

            public int GetMagnitude()
            {
                int magnitude = 0;

                if (leftNum.isPrimary) magnitude+=3*leftNum.value;
                else magnitude += 3*(leftNum.GetMagnitude());

                if (rightNum.isPrimary) magnitude+=2*rightNum.value;
                else magnitude += 2*(rightNum.GetMagnitude());

                return magnitude;
            }
        }

        static public void Solve(int part)
        {
            //string filePath = @"..\..\Inputs\day18Example.txt";
            string filePath = @"..\..\Inputs\day18.txt";

            string[] input = File.ReadAllLines(filePath);

            List<SnailfishNumber> numbers = new List<SnailfishNumber>();

            foreach (string line in input)
            {
                numbers.Add(ParseSnailfishNumber(line));
            }

            if (part == 1)
            {
                SnailfishNumber sum = numbers.Aggregate((a,b) => ParseSnailfishNumber(a.AddSnailfishNumber(b).ReduceSnailfishNumber()));
                Console.WriteLine(sum.GetMagnitude());
            } else
            {
                Console.WriteLine(MaxMagnitude(numbers));
            }

            
         }

        static private SnailfishNumber ParseSnailfishNumber(string input)
        {
            Stack<SnailfishNumber> snailfishStack = new Stack<SnailfishNumber>();

            SnailfishNumber leftNum = new SnailfishNumber();
            SnailfishNumber rightNum = new SnailfishNumber();

            SnailfishNumber snailFish = new SnailfishNumber();

            foreach (char c in input)
            {
                switch (c)
                {
                    case ']':
                        rightNum = snailfishStack.Pop();
                        leftNum = snailfishStack.Pop();
                        snailfishStack.Push( new SnailfishNumber() { leftNum = leftNum, rightNum = rightNum });
                        break;
                    case ',':
                    case '[':
                        break;
                    default:
                        snailfishStack.Push((SnailfishNumber)int.Parse(c.ToString()));
                        break;
                }
            }

            return snailfishStack.Pop();
        }

        private static int MaxMagnitude (List<SnailfishNumber> numbers)
        {
            int maxMagnitude = int.MinValue;
            for (int i = 0; i < numbers.Count(); i++)
            {
                for (int j = 0; j < numbers.Count(); j++)
                {
                    maxMagnitude = Math.Max(maxMagnitude , ParseSnailfishNumber(numbers[i].AddSnailfishNumber(numbers[j]).ReduceSnailfishNumber()).GetMagnitude());
                }
            }
            return maxMagnitude;
        }
    }
}
