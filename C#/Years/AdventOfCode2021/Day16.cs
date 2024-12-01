using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    class Day16
    {
        private static int puzzlePart;
        static public void Solve(int part)
        {
            // string filePath = @"..\..\Inputs\day16Example.txt";
            string filePath = @"..\..\Inputs\day16.txt";

            string input = File.ReadAllText(filePath);

            puzzlePart = part;
            Console.WriteLine(AnalyzePacket(HexToBinary(input))[1]); // Analyzis output : [0] packet length [1] packet output value
        }

        static private string HexToBinary(string hexString)
        {
            string binaryString = "";
            foreach (char c in hexString)
            {
                binaryString += (Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'));
            }
            return binaryString;
        }

        static private long[] AnalyzePacket(string packet)
        {
            int version = Convert.ToInt32(new string(packet.Take(3).ToArray()), 2);
            int iD = Convert.ToInt32(new string(packet.Skip(3).Take(3).ToArray()), 2);

            packet = new string(packet.Skip(6).ToArray());

            if (iD == 4)
            {
                return AnalyzeLiteralValuePacket(packet, version);
            }
            else
            {
                return AnalyzeOperatorPacket(packet, version, iD);
            }
        }

        static private long[] AnalyzeLiteralValuePacket (string packet, int version)
        {
            string value = "";
            bool lastGroupFound = false;
            int i = 0;

            long[] output = new long[2];

            while (!lastGroupFound)
            {
                string group = new string (packet.Skip(5*i).Take(5).ToArray());
                lastGroupFound = group[0] == '0';
                value += new string(group.Skip(1).ToArray());
                i++;
            }

            output[0] = 6 + 5*i; // Packet length : 6 header bits + value bits

            if (puzzlePart == 1) output[1] = version;
            else output[1] = Convert.ToInt64(value, 2);

            return (output);
        }

        static private long[] AnalyzeOperatorPacket (string packet, int version, int iD)
        {
            int subPacketsCountLength = packet[0] == '0' ? 15 : 11;

            long[] output = new long[2];

            List<long[]> outputs = new List<long[]>();

            int subPacketsLength = Convert.ToInt32(new string(packet.Skip(1).Take(subPacketsCountLength).ToArray()), 2);
            int subPacketsCount = 0;
            
            int packetLength = subPacketsCountLength + 1; // Before adding each subPacket's length

            packet = new string (packet.Skip(packetLength).ToArray());

            int i = 0;
            while (subPacketsCount < subPacketsLength)
            {
                outputs.Add(AnalyzePacket(packet));
                packet = new string(packet.Skip((int)outputs[i][0]).ToArray());
                packetLength += (int)outputs[i][0];

                subPacketsCount += subPacketsCountLength == 15 ? (int)outputs[i][0] : 1; // SubPackets count method changes: whether total bits number or total subpackets number
                i++;
            }

            output[0] = packetLength + 6; // PacketLength + 6 header bits removed in previous Analysis method

            if (puzzlePart == 1) output[1] = outputs.Sum(o => o[1]) + version;
            else
            {
                switch (iD)
                {
                    case 0:
                        output[1] = outputs.Sum(o => o[1]);
                        break;
                    case 1:
                        output[1] = outputs.Aggregate((long)1, (a,o) => a*o[1]);
                        break;
                    case 2:
                        output[1] = outputs.Min(o => o[1]);
                        break;
                    case 3:
                        output[1] = outputs.Max(o => o[1]);
                        break;
                    case 5:
                        output[1] = outputs[0][1] > outputs[1][1] ? 1 : 0;
                        break;
                    case 6:
                        output[1] = outputs[0][1] < outputs[1][1] ? 1 : 0;
                        break;
                    case 7:
                        output[1] = outputs[0][1] == outputs[1][1] ? 1 : 0;
                        break;
                }
            }

            return output;
        }
    }
}
