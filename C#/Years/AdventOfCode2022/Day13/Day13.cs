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
            string input = File.ReadAllText(@"Day13\input.txt");

            if (part == 1)
            {
                Console.WriteLine(input
                .Split("\r\n\r\n")
                .Select(s => (s.Split("\r\n").First(), s.Split("\r\n").Last()))
                .Select(t => (new Packet(t.Item1), new Packet(t.Item2)))
                .Select((packets, idx) => packets.Item2.IsGreaterThan(packets.Item1) ? idx + 1 : 0)
                .Sum());
            }

            if (part == 2)
            {
                Packet divider1 = new Packet("[[2]]");
                Packet divider2 = new Packet("[[6]]");

                List<Packet> sortedPackets = new() {divider1, divider2};
                
                foreach (Packet packet in input.Split("\r\n").Where(s => s!=string.Empty).Select(s => new Packet(s.Trim('\n'))))
                {
                    int idx = 0;
                    foreach (Packet packetInList in sortedPackets)
                    {
                        if (packetInList.IsGreaterThan(packet)) break;
                        idx++;
                    }
                    sortedPackets.Insert(idx, packet);
                }
                
                Console.WriteLine((sortedPackets.IndexOf(divider1)+1) * (sortedPackets.IndexOf(divider2)+1)); 
            }
        }
    }
}