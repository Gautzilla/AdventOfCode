using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day8
    {
        public static void Solve(int part)
        {
            // string path = @"..\..\Inputs\day8Example.txt";
            string path = @"..\..\Inputs\day8.txt";

            List<string[]> input = File.ReadAllLines(path).Select(a => a.Split('|')).ToArray().ToList();

            int count = 0;

            if (part == 1)
            {
                foreach (string[] line in input)
                {
                    count += line[1].Split(' ').Where(a => (a.Length == 2 || a.Length == 3 || a.Length == 4 || a.Length == 7)).Count();
                }

                Console.WriteLine($"Il y a {count} fois les valeurs 1, 4, 7 ou 8.");
            } else
            {
                foreach (string[] line in input)
                {
                    List<string> keys = line[0].Split(' ').ToList();

                    Dictionary<int, string> dict = new Dictionary<int, string>();

                    dict.Add(1, keys.Where(a => a.Length == 2).First());
                    dict.Add(4, keys.Where(a => a.Length == 4).First());
                    dict.Add(7, keys.Where(a => a.Length == 3).First());
                    dict.Add(8, keys.Where(a => a.Length == 7).First());
                    dict.Add(3, keys.Where(a => a.Length == 5).FirstOrDefault(a => a.ToCharArray().Intersect(dict[1].ToCharArray()).Count() == 2)); // 3 : 5 characters and contains every character from 1
                    dict.Add(9, keys.Where(a => a.Length == 6).FirstOrDefault(a => a.ToCharArray().Intersect(dict[3].ToCharArray()).Count() == 5)); // 9 : 6 characters and contains every character from 3
                    dict.Add(0, keys.Where(a => a.Length == 6).FirstOrDefault(a => (a.ToCharArray().Intersect(dict[1].ToCharArray()).Count() == 2) && a != dict[9])); // 0 : 6 characters and contains every character from 1, isn't 9
                    dict.Add(6, keys.Where(a => a.Length == 6).FirstOrDefault(a => a != dict[0] && a != dict[9])); // 6 : 6 characters, isn't 0 nor 9
                    dict.Add(5, keys.Where(a => a.Length == 5).FirstOrDefault(a => a != dict[3] && (a.ToCharArray().Intersect(dict[9].ToCharArray()).Count() == 5))); // 5 : 5 characters all contained in 9, isn't 3
                    dict.Add(2, keys.Where(a => a.Length == 5).FirstOrDefault(a => a != dict[3] && a != dict[5])); // Last one

                    List<string> values = line[1].Split(' ').ToList();

                    string outputValue = "";

                    foreach (string value in values)
                    {
                        outputValue += dict.FirstOrDefault(a => (a.Value.Length == value.Length) && (a.Value.ToCharArray().Intersect(value.ToCharArray()).Count() == value.Length)).Key.ToString(); // Appends key char if value is same length and contains same characters
                    }

                    count += int.Parse(outputValue);
                }
                Console.WriteLine($"La somme des valeurs vaut {count}");
            }
        }
    }
}
