using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day2
    {
        static public void Solve(int part)
        {
            // string path = @"..\..\Inputs\day2Example.txt";
            string path = @"..\..\Inputs\day2.txt";

            List<string> input = File.ReadAllLines(path).ToList();

            List<string[]> instructions = new List<string[]>();
            foreach (string line in input) instructions.Add(line.Split(' '));

            int horizontal = 0;
            int depth = 0;

            if (part == 1)
            {
                foreach (string[] instruction in instructions)
                {
                    switch (instruction[0])
                    {
                        case ("forward"):
                            horizontal += int.Parse(instruction[1]);
                            break;
                        case ("down"):
                            depth += int.Parse(instruction[1]);
                            break;
                        case ("up"):
                            depth -= int.Parse(instruction[1]);
                            break;
                    }
                }                
            } else
            {
                int aim = 0;

                foreach (string[] instruction in instructions)
                {
                    switch (instruction[0])
                    {
                        case ("forward"):
                            horizontal += int.Parse(instruction[1]);
                            depth += aim * int.Parse(instruction[1]);
                            break;
                        case ("down"):
                            aim += int.Parse(instruction[1]);
                            break;
                        case ("up"):
                            aim -= int.Parse(instruction[1]);
                            break;
                    }
                }
            }

            Console.WriteLine($"Horizontal : {horizontal} ; Depth : {depth} ; Product : {depth * horizontal}");
        }
    }
}
