using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace AdventOfCode2022
{
    public static class Day13
    {
        public static void Solve(int part)
        { 
            string[] input = File.ReadAllLines(@"Day13\input.txt");

            foreach (string inp in input.Where(i => i!=string.Empty))
            {
                Console.WriteLine(inp + " :");
                Packet packet = new Packet(inp, null);
                Console.WriteLine(packet.Content.Count);
            }
        }
    }
}