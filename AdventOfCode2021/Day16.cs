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
        static public void Solve(int part)
        {
            // string path = @"..\..\Inputs\day16Example.txt";
            string path = @"..\..\Inputs\day16.txt";

            Console.WriteLine(AnalyzePacket(HexToBinary("EE00D40C823060"))[1]);

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

        static private int[] AnalyzeLiteralValuePacket (string packet, int version, int iD)
        {
            string value = "";
            bool lastGroupFound = false;
            int i = 0;

            int[] output = new int[2];

            while (!lastGroupFound)
            {
                string group = new string (packet.Skip(5*i).Take(5).ToArray());
                lastGroupFound = group[0] == '0';
                value += new string(group.Skip(1).ToArray());
                i++;
            }

            output[0] = 6 + 5*i; // Packet length
            output[1] = Convert.ToInt32(value, 2);

            return (output);
        }

        static private int[] AnalyzeOperatorPacket (string packet, int version, int iD)
        {
            int subPacketsCountLength = packet[0] == '0' ? 15 : 11;

            int[] output = new int[2];

            List<int[]> outputs = new List<int[]>();

            int subPacketsCount = Convert.ToInt32(new string(packet.Skip(1).Take(subPacketsCountLength).ToArray()), 2);

            
            int packetLength = subPacketsCountLength + 1;

            packet = new string (packet.Skip(packetLength).ToArray());

            for (int i = 0; i < subPacketsCount; i ++)
            {
                outputs.Add(AnalyzePacket(packet));
                packet = new string (packet.Skip(outputs[i][1]).ToArray());
                packetLength += outputs[i][0];
            }

            output[0] = outputs.Sum(o => o[0]);
            output[1] = packetLength;

            return output;
        }

        static private int[] AnalyzePacket (string packet)
        {
            int version = Convert.ToInt32( new string(packet.Take(3).ToArray()), 2);
            int iD = Convert.ToInt32( new string(packet.Skip(3).Take(3).ToArray()), 2);

            packet = new string(packet.Skip(6).ToArray());

            int[] output = new int[2];

            if (iD == 4)
            {
                output = AnalyzeLiteralValuePacket(packet, version, iD);
            } else
            {
                output = AnalyzeOperatorPacket(packet, version, iD);
            }

            return output;
        }
    }
}
